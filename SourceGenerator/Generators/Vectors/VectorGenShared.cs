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
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The name and the type arguments of every interface</returns>
    public static List<(string Name, List<string> Args)> ArithInterfaces(Typ typ, int size, bool storeVariant)
    {
        var args = new List<string> { VecName(typ, size, storeVariant), typ.compType };
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
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The name of every type parameter and the type argument of every one of them</returns>
    public static (List<string> Params, List<string> Args) SwizzleTypes(Typ typ, int size, bool set, bool storeVariant)
    {
        var parameters = new List<string> { set ? "TIn" : "TSelf" };
        var args = new List<string> { VecName(typ, size, storeVariant) };
        for (var d = 2; d <= 4; d++)
        {
            if (d == size) continue;
            if (set && d > size) continue;
            parameters.Add($"TVec{d}");
            // a combination of another size is a vector without the storage variant
            args.Add(VecName(typ, d, false));
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
    /// <param name="jsonHelpers">True when the file uses the types of <c>System.Text.Json</c></param>
    /// <param name="ns">The namespace of the file, the one of the vectors when it is null</param>
    public static void FileHeader(
        StringBuilder sb, bool simdHelpers, bool swizzleHelpers = false, bool jsonHelpers = false, string? ns = null)
    {
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using System.Runtime.InteropServices;");
        sb.AppendLine("using System.Runtime.Intrinsics;");
        // the json members of a vector need the serializer types, only the files that carry them import it
        if (jsonHelpers)
        {
            sb.AppendLine("using System.Text.Json;");
            sb.AppendLine("using System.Text.Json.Serialization;");
        }

        sb.AppendLine("using Coplt.Mathematics;");
        sb.AppendLine("using Coplt.Mathematics.Generics;");
        if (simdHelpers) sb.AppendLine("using Coplt.Mathematics.Simd;");
        if (swizzleHelpers) sb.AppendLine("using Coplt.Mathematics.Generics.Swizzle;");
        sb.AppendLine("using Coplt.Shader;");
        sb.AppendLine();
        sb.AppendLine($"namespace {ns ?? VectorGenerator.VecNamespace};");
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
    /// True when the vector has a storage variant. Only the 2 component vectors whose register is 64 bits wide
    /// and the 3 component ones have one: the first ones keep the exact bits of their value instead of a padded
    /// 128 bit register, the last ones keep their components in fields and have no register at all.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <returns>True when the storage variant exists</returns>
    public static bool HasStorageVariant(Typ typ, int size) =>
        typ.arith && typ.simd && (size == 3 || (size == 2 && 8 * typ.size * 2 == 64));

    /// <summary>
    /// Returns the name of a vector, the storage variant of a vector has the <c>s</c> suffix.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The name of the vector</returns>
    public static string VecName(Typ typ, int size, bool storeVariant) =>
        storeVariant ? $"{typ.name}{size}s" : $"{typ.name}{size}";

    /// <summary>
    /// Returns the bit size of the register that keeps the value of a vector, 0 when the vector has no register.
    /// A 3 or 4 component vector is padded to 4 lanes, the 2 component ones keep the exact width of their value
    /// beside the storage variant of a 4 byte component vector, whose value is widened to 128 bits because the
    /// 64 bit register of it is not accelerated on every platform.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The bit size of the register</returns>
    public static int Register(Typ typ, int size, bool storeVariant)
    {
        if (!typ.simd) return 0;
        if (size == 2)
        {
            var exact = 8 * typ.size * 2;
            if (storeVariant) return exact;
            return exact == 64 ? 128 : exact;
        }

        // the storage variant of a 3 component vector has no register, it keeps its components in fields
        if (size == 3 && storeVariant) return 0;
        return 8 * typ.size * 4;
    }

    /// <summary>
    /// True when the value of the vector is kept in a 64 bit register behind a raw <c>ulong</c> field instead of
    /// a <c>Vector64</c> field, the property of the vector reinterprets the bits of it.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>True when the value is kept in a raw ulong field</returns>
    public static bool Uses64(Typ typ, int size, bool storeVariant) => storeVariant && size == 2 && typ.simd;

    /// <summary>
    /// True when the vector is backed by a hardware accelerated register.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>True when the vector is backed by a register</returns>
    public static bool Simd(Typ typ, int size, bool storeVariant) => Register(typ, size, storeVariant) != 0;

    /// <summary>
    /// Returns the size of the value of a vector in bytes. The value of a vector that has no register is kept in
    /// fields, a 3 component one is padded to the width of a 4 component one.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The size of the value of the vector in bytes</returns>
    public static int ByteSize(Typ typ, int size, bool storeVariant) =>
        Simd(typ, size, storeVariant)
            ? Register(typ, size, storeVariant) / 8
            : typ.size * (size == 3 ? 4 : size);

    /// <summary>
    /// Returns the number of lanes of the register of a vector.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The number of lanes of the register</returns>
    public static int Lanes(Typ typ, int size, bool storeVariant) =>
        Register(typ, size, storeVariant) / (8 * typ.size);

    /// <summary>
    /// Returns the number of lanes of the register of a vector that are padding, 0 when the register is exactly
    /// as wide as the vector and the vector has no register at all.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The number of padding lanes</returns>
    public static int PadLanes(Typ typ, int size, bool storeVariant) => Lanes(typ, size, storeVariant) - size;

    /// <summary>
    /// Builds a result from a raw simd expression. The constructor masks the padding lanes of a register that is
    /// wider than the vector, an expression that keeps them at zero writes the field directly and skips the mask.
    /// </summary>
    /// <param name="simd">True when the vector is backed by a hardware accelerated simd type</param>
    /// <param name="pad">True when the register of the vector has padding lanes</param>
    /// <param name="expr">The raw simd expression of the result</param>
    /// <param name="masked">True when the expression can leave something else than zero in the padding lanes</param>
    /// <returns>The construction of the result</returns>
    public static string Vector(bool simd, bool pad, string expr, bool masked = false) =>
        simd && pad && masked ? $"new({expr})" : $"new() {{ vector = {expr} }}";

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
