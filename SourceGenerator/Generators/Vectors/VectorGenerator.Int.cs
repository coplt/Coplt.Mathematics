using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the integer members of the vector described by <paramref name="typ"/>, they implement
    /// <c>IVectorInteger</c>: the check of a power of two and, for a vector that has no sign, the rounding up to
    /// the next power of two of every component. They are emitted into their own file, so they stay separate from
    /// the plain arithmetic.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenInt(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        // the bool vector that has the same number of components as the vector
        var boolType = $"b{typ.size * 8}v{size}";

        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        // the rounding up to the next power of two is only meaningful for a vector that has no sign
        sb.AppendLine(typ.sig
            ? $"    IVectorInteger<{type}, {boolType}>"
            : $"    IVectorUnsignedInteger<{type}, {boolType}>");
        sb.AppendLine("{");

        #region is_pow2

        sb.AppendLine();
        sb.AppendLine("    #region is_pow2");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        // a component is a power of two when the only bit that is set is cleared by the subtraction of one and
        // every other bit is kept, the zero leaves the all ones mask of the subtraction and a negative value has
        // the top bit set, so both of them are excluded by the comparison with the zero of the vector
        sb.AppendLine(typ.sig
            ? $"    public readonly {boolType} is_pow2() => ((this & (this - {type}.One)) == {type}.Zero) & (this > {type}.Zero);"
            : $"    public readonly {boolType} is_pow2() => ((this & (this - {type}.One)) == {type}.Zero) & (this != {type}.Zero);");
        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        if (!typ.sig)
        {
            #region up2pow2

            sb.AppendLine();
            sb.AppendLine("    #region up2pow2");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public readonly {type} up2pow2()");
            sb.AppendLine("    {");
            // the subtraction turns every bit below the highest set one into a one, so the addition of one
            // carries into the next power of two, a zero wraps around and stays zero
            sb.AppendLine($"        var a = this - {type}.One;");
            for (var shift = 1; shift <= 8; shift <<= 1) sb.AppendLine($"        a |= a >> {shift};");
            if (typ.size >= 4) sb.AppendLine("        a |= a >> 16;");
            if (typ.size >= 8) sb.AppendLine("        a |= a >> 32;");
            sb.AppendLine($"        return a + {type}.One;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    #endregion");

            #endregion
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
