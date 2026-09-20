using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// The emission helpers shared by the base part and the arithmetic part of <see cref="VectorGenerator"/>.
/// </summary>
internal static class VectorGenShared
{
    /// <summary>
    /// Returns the arithmetic interfaces the vector described by <paramref name="typ"/> implements: a signed
    /// vector has the signed arithmetic, the other ones the plain arithmetic, and a 3 component vector also has
    /// the cross product on top of it.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <returns>The name and the type arguments of every interface</returns>
    public static List<(string Name, List<string> Args)> ArithInterfaces(Typ typ, int size)
    {
        var args = new List<string> { $"{typ.name}{size}", typ.compType };
        var ifaces = new List<(string Name, List<string> Args)>();
        if (typ.sig) ifaces.Add(("ISignedVectorArithmetic", args));
        else if (size != 3) ifaces.Add(("IVectorArithmetic", args));
        if (size == 3) ifaces.Add(("IVector3Arithmetic", args));
        return ifaces;
    }

    /// <summary>
    /// Returns the names of the type parameters and the type arguments of the interface that collects the
    /// swizzle interfaces of a vector of <paramref name="size"/> components: the getters or the setters, the
    /// same sized type is the first one and a setter only exists for a combination that is not longer than the
    /// vector because its indices have to be distinct.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="set">True for the interface that collects the setters</param>
    /// <returns>The name of every type parameter and the type argument of every one of them</returns>
    public static (List<string> Params, List<string> Args) SwizzleTypes(Typ typ, int size, bool set)
    {
        var parameters = new List<string> { set ? "TIn" : "TSelf" };
        var args = new List<string> { $"{typ.name}{size}" };
        for (var d = 2; d <= 4; d++)
        {
            if (d == size) continue;
            if (set && d > size) continue;
            parameters.Add($"TVec{d}");
            args.Add($"{typ.name}{d}");
        }

        return (parameters, args);
    }

    /// <summary>
    /// Makes the reference to a generic interface. A reference cannot carry the type arguments of a constructed
    /// interface, so the reference names the interface with the type parameters it declares and its text shows
    /// the constructed interface.
    /// </summary>
    /// <param name="name">The name of the interface without its type arguments</param>
    /// <param name="parameters">The names of the type parameters the interface declares</param>
    /// <param name="args">The type arguments of the constructed interface</param>
    /// <returns>The reference</returns>
    public static string IfaceRef(string name, List<string> parameters, List<string> args)
    {
        // the arguments are shown in the text of the reference and every one of them is a reference of its own
        var text = new StringBuilder();
        for (var i = 0; i < args.Count; i++)
        {
            if (i != 0) text.Append(", ");
            text.Append($"<see cref=\"{args[i]}\"/>");
        }

        return $"<see cref=\"{name}{{{string.Join(",", parameters)}}}\">{name}&lt;{text}&gt;</see>";
    }

    /// <summary>
    /// The name of the field that keeps the value of a 64 bit vector. The <c>vector</c> property of the vector
    /// reinterprets it, so the value of the vector does not have to go through a 64 bit vector type.
    /// </summary>
    public const string Vector64Field = "_vector";

    /// <summary>
    /// The 128 bit value of a 64 bit vector, its field is widened by the utility of the vector type.
    /// </summary>
    /// <param name="self">The prefix of the vector, empty for the vector itself</param>
    /// <param name="scalar">The type of a component of the 128 bit register</param>
    /// <returns>The raw simd expression of the value</returns>
    public static string Load64(string self, string scalar) => $"{self}{Vector64Field}.Load64<{scalar}>()";

    /// <summary>
    /// Builds a 64 bit vector from a 128 bit expression, the lower 64 bits of it are the value of the vector.
    /// </summary>
    /// <param name="expr">The raw simd expression of the result</param>
    /// <returns>The construction of the result</returns>
    public static string From128(string expr) => $"new() {{ {Vector64Field} = ({expr}).AsUInt64()[0] }}";

    /// <summary>
    /// Emits the header every generated vector file starts with.
    /// </summary>
    /// <param name="sb">The builder of the file</param>
    /// <param name="simdHelpers">True when the file uses the helpers of <c>Coplt.Mathematics.Simd</c></param>
    /// <param name="swizzleHelpers">True when the file uses the swizzle interfaces</param>
    public static void FileHeader(StringBuilder sb, bool simdHelpers, bool swizzleHelpers = false)
    {
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using System.Runtime.InteropServices;");
        sb.AppendLine("using System.Runtime.Intrinsics;");
        sb.AppendLine("using Coplt.Mathematics;");
        sb.AppendLine("using Coplt.Mathematics.Generics;");
        if (simdHelpers) sb.AppendLine("using Coplt.Mathematics.Simd;");
        if (swizzleHelpers) sb.AppendLine("using Coplt.Mathematics.Generics.Swizzle;");
        sb.AppendLine("using Coplt.Shader;");
        sb.AppendLine();
        sb.AppendLine($"namespace {VectorGenerator.VecNamespace};");
    }

    /// <summary>
    /// Returns the names of the components of a vector of <paramref name="size"/> components.
    /// </summary>
    /// <param name="size">The number of components of the vector</param>
    /// <returns>The name of every component</returns>
    public static string[] Components(int size)
    {
        var comp = new string[size];
        for (var i = 0; i < size; i++) comp[i] = Typ.xyzw[i];
        return comp;
    }

    /// <summary>
    /// Builds a result from a raw simd expression. The constructor masks the padding lane of a 3 component
    /// vector, an expression that keeps that lane at zero writes the field directly and skips the mask.
    /// </summary>
    /// <param name="simd">True when the vector is backed by a hardware accelerated simd type</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="expr">The raw simd expression of the result</param>
    /// <param name="masked">True when the expression can leave something else than zero in the padding lane</param>
    /// <returns>The construction of the result</returns>
    public static string Vector(bool simd, int size, string expr, bool masked = false) =>
        simd && size == 3 && masked ? $"new({expr})" : $"new() {{ vector = {expr} }}";

    /// <summary>
    /// Joins the part of every component with <paramref name="sep"/>.
    /// </summary>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="get">The part of a single component</param>
    /// <param name="sep">The separator between two components</param>
    /// <returns>The joined parts</returns>
    public static string Join(int size, Func<int, string> get, string sep = ", ")
    {
        var b = new StringBuilder();
        for (var i = 0; i < size; i++)
        {
            if (i != 0) b.Append(sep);
            b.Append(get(i));
        }

        return b.ToString();
    }
}
