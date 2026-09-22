using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// A member that creates a vector out of another one. Every member that creates a vector from a part of a
    /// shorter one stands on a public constructor of its own, which a caller that does not need the interface
    /// behind the member can use as well, and the constructor forwards to the member. The members that insert a
    /// component into a pair of the components of a shorter vector keep their own body because the constructor of
    /// one of them would take the same arguments as the constructor of the merge of two pairs.
    /// </summary>
    /// <param name="Signature">The signature of the create member</param>
    /// <param name="Body">The body of a member that has no constructor and no accelerated form</param>
    /// <param name="Summary">The documentation of the constructor of the member, null when it has none</param>
    /// <param name="Ctor">The parameters of the constructor of the member, null when it has none</param>
    /// <param name="Forward">The body of the constructor of the member</param>
    /// <param name="Accel">The raw simd expression of the result of the accelerated form</param>
    /// <param name="Fallback">The component wise construction of the result</param>
    private sealed record CtorMember(
        string Signature,
        string? Body = null,
        string? Summary = null,
        string? Ctor = null,
        string? Forward = null,
        string? Accel = null,
        string? Fallback = null);

    /// <summary>
    /// Generates the members that create the vector described by <paramref name="typ"/> out of another one, they
    /// implement the <c>IVectorCtor</c> interfaces: a vector of 2 components is created from its two components,
    /// a longer one is created from its own components as well, a vector of 3 or 4 components is created by
    /// merging a pair of components of a shorter vector and a vector of 4 components is also created by merging
    /// a triple. The name of a member is the name of the components it takes from its arguments: the pair of
    /// <c>Create(xy, z)</c> holds the <c>x</c> and <c>y</c> components, and the pair of <c>InsertY(xz, y)</c>
    /// holds the <c>x</c> and <c>z</c> ones because the <c>y</c> one comes from the value behind it.
    /// <para>Every member has an accelerated form that fills the register of the vector with a simd shuffle and a
    /// component wise form that the vector without a register falls back to. The padding lane of the register of
    /// a 3 component vector holds zero in both of them, so a member writes the field of the vector directly
    /// instead of going through the constructor that masks it</para>
    /// <para>The members that create a vector from a part of a shorter one stand on a public constructor of the
    /// vector, so a caller that does not need the interface behind the member can create the vector with the
    /// constructor alone</para>
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenCtor(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var vecName = $"Vector{reg}";
        // the register of the half of the register of the vector, the halves of it are combined by a create
        var half = $"Vector{reg / 2}";
        // the digits of the index of a shuffle are cast the same way as the ones of the swizzle members
        var cast = typ.shuffleCast;
        // a shorter vector is always the regular one, a storage variant exists for the regular one only
        var type2 = VectorGenShared.VecName(typ, 2, false);
        var type3 = VectorGenShared.VecName(typ, 3, false);
        // the register of the 2 component vector: it is the half of the register of the vector itself when a
        // component is 8 bytes wide, and a wider register whose lower lanes are the half otherwise
        var reg2 = VectorGenShared.Register(typ, 2, false);

        // reads the half of the register of the vector out of a 2 component vector
        string HalfOf(string name) => reg2 == reg / 2 ? $"{name}.vector" : $"{name}.vector.GetLower()";

        // the half of the register that holds a pair of the components of the member
        string HalfOfValues(params string[] values) => $"{half}.Create({string.Join(", ", values)})";

        // the index of a shuffle, the cast of the first digit settles the width of every one of them
        string Index(params int[] values)
        {
            var index = new StringBuilder();
            for (var i = 0; i < values.Length; i++)
            {
                if (i != 0) index.Append(", ");
                index.Append(cast);
                index.Append(values[i]);
            }

            return $"{vecName}.Create({index})";
        }

        // the interfaces the members implement, they are declared by the part that holds the members instead of
        // the base part because the base part has no member of them
        var ifaces = size switch
        {
            2 => new List<string>
            {
                $"IVector2Ctor<{type}, {scalar}>",
            },
            3 => new List<string>
            {
                $"IVector3CtorFromVector2<{type}, {scalar}, {type2}>",
            },
            _ => new List<string>
            {
                $"IVector4CtorFromVector2<{type}, {scalar}, {type2}>",
                $"IVector4CtorFromVector3<{type}, {scalar}, {type3}>",
            },
        };

        // the members of the vector: the signature of every create member, the documentation of the constructor
        // that stands behind it, the parameters and the mark of that constructor, the raw simd expression of the
        // accelerated form and the component wise form that a vector without a register falls back to.
        // A member that builds the same value as another one forwards to it instead of repeating it
        var members = new List<CtorMember>();
        if (size == 2)
        {
            members.Add(new($"Create({scalar} x, {scalar} y)", Body: "new(x, y)"));
        }
        else if (size == 3)
        {
            // the components of the vector are the two of the pair followed by the value and by the padding
            // lane, which the half of the register that holds the value sets to zero
            members.Add(new($"Create({scalar} x, {scalar} y, {scalar} z)", Body: "new(x, y, z)"));
            members.Add(new($"Create(in {type2} xy, {scalar} z)", Forward: "this = Create(xy, z)",
                Summary: "Creates a vector from the pair of the <c>x</c> and <c>y</c> components and the <c>z</c> component",
                Ctor: $"in {type2} xy, {scalar} z", Fallback: "new(xy.x, xy.y, z)",
                Accel: $"{vecName}.Create({HalfOf("xy")}, {HalfOfValues("z", "default")})"));
            // the pair is read from the upper half of the register, the shuffle moves it to the components
            // behind the value
            members.Add(new($"Create({scalar} x, in {type2} yz)", Forward: "this = Create(x, yz)",
                Summary: "Creates a vector from the <c>x</c> component and the pair of the <c>y</c> and <c>z</c> components",
                Ctor: $"{scalar} x, in {type2} yz", Fallback: "new(x, yz.x, yz.y)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOfValues("x", "default")}, {HalfOf("yz")}), {Index(0, 2, 3, 1)})"));
            members.Add(new($"InsertY(in {type2} xz, {scalar} y)", Fallback: "new(xz.x, y, xz.y)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOf("xz")}, {HalfOfValues("y", "default")}), {Index(0, 2, 1, 3)})"));
        }
        else
        {
            members.Add(new($"Create({scalar} x, {scalar} y, {scalar} z, {scalar} w)", Body: "new(x, y, z, w)"));
            // the two pairs of the arguments are the two halves of the register
            members.Add(new($"Create(in {type2} xy, in {type2} zw)", Forward: "this = Create(xy, zw)",
                Summary: "Creates a vector from two pairs of components",
                Ctor: $"in {type2} xy, in {type2} zw", Fallback: "new(xy.x, xy.y, zw.x, zw.y)",
                Accel: $"{vecName}.Create({HalfOf("xy")}, {HalfOf("zw")})"));
            members.Add(new($"Create(in {type2} xy, {scalar} z, {scalar} w)", Forward: "this = Create(xy, z, w)",
                Summary: "Creates a vector from the pair of the <c>x</c> and <c>y</c> components and the two other components",
                Ctor: $"in {type2} xy, {scalar} z, {scalar} w", Fallback: "new(xy.x, xy.y, z, w)",
                Accel: $"{vecName}.Create({HalfOf("xy")}, {HalfOfValues("z", "w")})"));
            members.Add(new($"Create({scalar} x, {scalar} y, in {type2} zw)", Forward: "this = Create(x, y, zw)",
                Summary: "Creates a vector from the two first components and the pair of the <c>z</c> and <c>w</c> ones",
                Ctor: $"{scalar} x, {scalar} y, in {type2} zw", Fallback: "new(x, y, zw.x, zw.y)",
                Accel: $"{vecName}.Create({HalfOfValues("x", "y")}, {HalfOf("zw")})"));
            // the pair is read from the upper half of the register, the shuffle moves it to the components
            // between the two values
            members.Add(new($"Create({scalar} x, in {type2} yz, {scalar} w)", Forward: "this = Create(x, yz, w)",
                Summary: "Creates a vector from the <c>x</c> and <c>w</c> components and the pair of the <c>y</c> and <c>z</c> ones",
                Ctor: $"{scalar} x, in {type2} yz, {scalar} w", Fallback: "new(x, yz.x, yz.y, w)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOfValues("x", "w")}, {HalfOf("yz")}), {Index(0, 2, 3, 1)})"));
            // the members that insert a pair of components keep their own body: the constructor of the first two
            // of them would take the same arguments as the constructor of a merge of two pairs
            members.Add(new($"InsertYZ(in {type2} xw, in {type2} yz)", Fallback: "new(xw.x, yz.x, yz.y, xw.y)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOf("xw")}, {HalfOf("yz")}), {Index(0, 2, 3, 1)})"));
            members.Add(new($"InsertYZ(in {type2} xw, {scalar} y, {scalar} z)", Fallback: "new(xw.x, y, z, xw.y)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOf("xw")}, {HalfOfValues("y", "z")}), {Index(0, 2, 3, 1)})"));
            // the two members that insert the x and w components build what a create builds
            members.Add(new($"InsertXW(in {type2} yz, in {type2} xw)", Body: "InsertYZ(xw, yz)"));
            members.Add(new($"InsertXW(in {type2} yz, {scalar} x, {scalar} w)", Body: "Create(x, yz, w)"));
            members.Add(new($"InsertYW(in {type2} xz, in {type2} yw)", Fallback: "new(xz.x, yw.x, xz.y, yw.y)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOf("xz")}, {HalfOf("yw")}), {Index(0, 2, 1, 3)})"));
            members.Add(new($"InsertYW(in {type2} xz, {scalar} y, {scalar} w)", Fallback: "new(xz.x, y, xz.y, w)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOf("xz")}, {HalfOfValues("y", "w")}), {Index(0, 2, 1, 3)})"));
            members.Add(new($"InsertXZ(in {type2} yw, in {type2} xz)", Body: "InsertYW(xz, yw)"));
            members.Add(new($"InsertXZ(in {type2} yw, {scalar} x, {scalar} z)", Fallback: "new(x, yw.x, z, yw.y)",
                Accel: $"{vecName}.Shuffle({vecName}.Create({HalfOfValues("x", "z")}, {HalfOf("yw")}), {Index(0, 2, 1, 3)})"));
            // the register of a 3 component vector is as wide as the one of the vector itself, the component
            // is appended to it and the shuffle turns the vector around it
            members.Add(new($"Create(in {type3} xyz, {scalar} w)", Forward: "this = Create(xyz, w)",
                Summary: "Creates a vector from the triple of the <c>x</c>, <c>y</c> and <c>z</c> components and the <c>w</c> component",
                Ctor: $"in {type3} xyz, {scalar} w", Fallback: "new(xyz.x, xyz.y, xyz.z, w)",
                Accel: "xyz.vector.WithElement(3, w)"));
            members.Add(new($"Create({scalar} x, in {type3} yzw)", Forward: "this = Create(x, yzw)",
                Summary: "Creates a vector from the <c>x</c> component and the triple of the <c>y</c>, <c>z</c> and <c>w</c> components",
                Ctor: $"{scalar} x, in {type3} yzw", Fallback: "new(x, yzw.x, yzw.y, yzw.z)",
                Accel: $"{vecName}.Shuffle(yzw.vector.WithElement(3, x), {Index(3, 0, 1, 2)})"));
            members.Add(new($"InsertY(in {type3} xzw, {scalar} y)", Fallback: "new(xzw.x, y, xzw.y, xzw.z)",
                Accel: $"{vecName}.Shuffle(xzw.vector.WithElement(3, y), {Index(0, 3, 1, 2)})"));
            members.Add(new($"InsertZ(in {type3} xyw, {scalar} z)", Fallback: "new(xyw.x, xyw.y, z, xyw.z)",
                Accel: $"{vecName}.Shuffle(xyw.vector.WithElement(3, z), {Index(0, 1, 3, 2)})"));
        }

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        for (var i = 0; i < ifaces.Count; i++)
        {
            sb.AppendLine($"    {ifaces[i]}" + (i == ifaces.Count - 1 ? "" : ","));
        }

        sb.AppendLine("{");

        // the constructor of a member that creates a vector from a part of a shorter one forwards to the member,
        // the member below holds the construction of the vector
        sb.AppendLine();
        sb.AppendLine("    #region ctors");
        foreach (var member in members)
        {
            if (member.Ctor == null) continue;
            sb.AppendLine();
            sb.AppendLine($"    /// <summary>{member.Summary}</summary>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public {type}({member.Ctor}) => {member.Forward};");
        }

        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();
        sb.AppendLine("    #region create");

        // every member implements one of the members of the interfaces above and inherits its documentation, the
        // member that has no accelerated form is the body itself and the vector without a register falls back to
        // the component wise form of the member that has one
        foreach (var member in members)
        {
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            if (member.Accel == null || !simd)
            {
                sb.AppendLine($"    public static {type} {member.Signature} => {member.Body ?? member.Fallback};");
            }
            else
            {
                sb.AppendLine($"    public static {type} {member.Signature}");
                sb.AppendLine("    {");
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return new() {{ vector = {member.Accel} }};");
                sb.AppendLine($"        return {member.Fallback};");
                sb.AppendLine("    }");
            }
        }

        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
