using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
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
    /// a 3 component vector holds zero in both of them, so the accelerated form writes the field of the vector
    /// directly instead of going through the constructor that masks it</para>
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
        var pad = VectorGenShared.PadLanes(typ, size, storeVariant) > 0;
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

        // the construction of a result from a raw simd expression, see VectorGenShared.Vector
        string Result(string expr) => VectorGenShared.Vector(simd, pad, expr);

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

        // the members of the vector: the signature of every one of them, the raw simd expression of the result
        // of the accelerated form and the component wise form that a vector without a register falls back to.
        // A member that builds the same value as another one forwards to it instead of repeating it
        var members = new List<(string Signature, string? Accel, string Fallback)>();
        if (size == 2)
        {
            members.Add(($"Create({scalar} x, {scalar} y)", null, "new(x, y)"));
        }
        else if (size == 3)
        {
            // the components of the vector are the two of the pair followed by the value and by the padding
            // lane, which the half of the register that holds the value sets to zero
            members.Add(($"Create({scalar} x, {scalar} y, {scalar} z)", null, "new(x, y, z)"));
            members.Add(($"Create(in {type2} xy, {scalar} z)",
                $"{vecName}.Create({HalfOf("xy")}, {HalfOfValues("z", "default")})",
                "new(xy.x, xy.y, z)"));
            // the pair is read from the upper half of the register, the shuffle moves it to the components
            // behind the value
            members.Add(($"Create({scalar} x, in {type2} yz)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOfValues("x", "default")}, {HalfOf("yz")}), {Index(0, 2, 3, 1)})",
                "new(x, yz.x, yz.y)"));
            members.Add(($"InsertY(in {type2} xz, {scalar} y)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOf("xz")}, {HalfOfValues("y", "default")}), {Index(0, 2, 1, 3)})",
                "new(xz.x, y, xz.y)"));
        }
        else
        {
            members.Add(($"Create({scalar} x, {scalar} y, {scalar} z, {scalar} w)", null, "new(x, y, z, w)"));
            // the two pairs of the arguments are the two halves of the register
            members.Add(($"Create(in {type2} xy, in {type2} zw)",
                $"{vecName}.Create({HalfOf("xy")}, {HalfOf("zw")})",
                "new(xy.x, xy.y, zw.x, zw.y)"));
            members.Add(($"Create(in {type2} xy, {scalar} z, {scalar} w)",
                $"{vecName}.Create({HalfOf("xy")}, {HalfOfValues("z", "w")})",
                "new(xy.x, xy.y, z, w)"));
            members.Add(($"Create({scalar} x, {scalar} y, in {type2} zw)",
                $"{vecName}.Create({HalfOfValues("x", "y")}, {HalfOf("zw")})",
                "new(x, y, zw.x, zw.y)"));
            // the pair is read from the upper half of the register, the shuffle moves it to the components
            // between the two values
            members.Add(($"Create({scalar} x, in {type2} yz, {scalar} w)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOfValues("x", "w")}, {HalfOf("yz")}), {Index(0, 2, 3, 1)})",
                "new(x, yz.x, yz.y, w)"));
            members.Add(($"InsertYZ(in {type2} xw, in {type2} yz)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOf("xw")}, {HalfOf("yz")}), {Index(0, 2, 3, 1)})",
                "new(xw.x, yz.x, yz.y, xw.y)"));
            members.Add(($"InsertYZ(in {type2} xw, {scalar} y, {scalar} z)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOf("xw")}, {HalfOfValues("y", "z")}), {Index(0, 2, 3, 1)})",
                "new(xw.x, y, z, xw.y)"));
            // the two members that insert the x and w components build what a create builds
            members.Add(($"InsertXW(in {type2} yz, in {type2} xw)", null, "InsertYZ(xw, yz)"));
            members.Add(($"InsertXW(in {type2} yz, {scalar} x, {scalar} w)", null, "Create(x, yz, w)"));
            members.Add(($"InsertYW(in {type2} xz, in {type2} yw)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOf("xz")}, {HalfOf("yw")}), {Index(0, 2, 1, 3)})",
                "new(xz.x, yw.x, xz.y, yw.y)"));
            members.Add(($"InsertYW(in {type2} xz, {scalar} y, {scalar} w)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOf("xz")}, {HalfOfValues("y", "w")}), {Index(0, 2, 1, 3)})",
                "new(xz.x, y, xz.y, w)"));
            members.Add(($"InsertXZ(in {type2} yw, in {type2} xz)", null, "InsertYW(xz, yw)"));
            members.Add(($"InsertXZ(in {type2} yw, {scalar} x, {scalar} z)",
                $"{vecName}.Shuffle({vecName}.Create({HalfOfValues("x", "z")}, {HalfOf("yw")}), {Index(0, 2, 1, 3)})",
                "new(x, yw.x, z, yw.y)"));
            // the register of a 3 component vector is as wide as the one of the vector itself, the component
            // is appended to it and the shuffle turns the vector around it
            members.Add(($"Create(in {type3} xyz, {scalar} w)",
                $"xyz.vector.WithElement(3, w)",
                "new(xyz.x, xyz.y, xyz.z, w)"));
            members.Add(($"Create({scalar} x, in {type3} yzw)",
                $"{vecName}.Shuffle(yzw.vector.WithElement(3, x), {Index(3, 0, 1, 2)})",
                "new(x, yzw.x, yzw.y, yzw.z)"));
            members.Add(($"InsertY(in {type3} xzw, {scalar} y)",
                $"{vecName}.Shuffle(xzw.vector.WithElement(3, y), {Index(0, 3, 1, 2)})",
                "new(xzw.x, y, xzw.y, xzw.z)"));
            members.Add(($"InsertZ(in {type3} xyw, {scalar} z)",
                $"{vecName}.Shuffle(xyw.vector.WithElement(3, z), {Index(0, 1, 3, 2)})",
                "new(xyw.x, xyw.y, z, xyw.z)"));
        }

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        for (var i = 0; i < ifaces.Count; i++)
        {
            sb.AppendLine($"    {ifaces[i]}" + (i == ifaces.Count - 1 ? "" : ","));
        }

        sb.AppendLine("{");

        foreach (var (signature, accel, fallback) in members)
        {
            sb.AppendLine();
            // the member implements one of the members of the interfaces above and inherits its documentation
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            // a vector without a register has no accelerated form at all
            if (accel == null || !simd)
            {
                sb.AppendLine($"    public static {type} {signature} => {fallback};");
            }
            else
            {
                sb.AppendLine($"    public static {type} {signature}");
                sb.AppendLine("    {");
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {Result(accel)};");
                sb.AppendLine($"        return {fallback};");
                sb.AppendLine("    }");
            }
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
