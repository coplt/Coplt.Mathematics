using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the shuffle members of the vector described by <paramref name="typ"/>. A shuffle combines two
    /// vectors of the same type, the low half of its result is taken from the first one and the high half from
    /// the second one, and every combination of the components is a member of its own so the pattern of a
    /// shuffle is a constant of the member: an accelerated vector shuffles its two registers with a single
    /// instruction and a vector without a register reads the components the pattern names. The pair of sources
    /// has a member as well, its pattern is only known at run time and it dispatches it with the table of the
    /// register or with <c>shuffle_soft</c>. Every member is static and the sources are passed by readonly
    /// reference, they implement <c>IVectorShuffle</c> and they are emitted into their own file.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The shuffle members of the vector, null when the vector has none of them</returns>
    private static string? GenShuffle(Typ typ, int size, bool storeVariant)
    {
        // a shuffle combines two vectors of 4 components, the shorter vectors have no member of it
        if (size != 4) return null;
        // the storage variant of a vector only exists for the shorter ones, a vector of 4 components fills
        // its register completely
        if (storeVariant) return null;

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        var attr = "[MethodImpl(256)]";
        var comp = VectorGenShared.Components(size);
        var lh = "Shuffle42";
        // the two members whose pattern is not a part of their name dispatch it with a helper
        var soft = $"shuffle_soft.shuffle<{type}, {scalar}>(a, b, lh)";
        var table = simd ? "new(simd_shuffle.Shuffle(a.vector, b.vector, lh))" : soft;

        // every combination of the components is a member, the first two of the digits of its name are the
        // components of the low half of the result and the last two the ones of its high half
        var patterns = new List<(string Name, string Result)>();
        for (var i = 0; i < comp.Length; i++)
        {
            for (var j = 0; j < comp.Length; j++)
            {
                for (var k = 0; k < comp.Length; k++)
                {
                    for (var l = 0; l < comp.Length; l++)
                    {
                        patterns.Add((
                            $"{comp[i]}{comp[j]}_{comp[k]}{comp[l]}",
                            $"new(a.{comp[i]}, a.{comp[j]}, b.{comp[k]}, b.{comp[l]})"));
                    }
                }
            }
        }

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, simd);
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    IVectorShuffle<{type}>");
        sb.AppendLine("{");

        // the pattern of this member is a value, it dispatches it to the member of its pattern
        sb.AppendLine();
        // the member implements the interface, it inherits its documentation from it
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} shuffle(in {type} a, in {type} b, {lh} lh) => {table};");

        foreach (var (name, result) in patterns)
        {
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            // the table of the register shuffles the members of a pattern, a vector without a register reads
            // the components the pattern names
            sb.AppendLine($"    public static {type} shuffle_{name}(in {type} a, in {type} b) => " +
                          (simd ? $"new(simd_shuffle.Shuffle_{name}(a.vector, b.vector));" : $"{result};"));
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Generates the helper that shuffles a vector that has no register to shuffle. The pattern of the dynamic
    /// member of such a vector is only known at run time, so the helper cannot be compiled to a single
    /// instruction and it dispatches the pattern with a switch: it reads the components the pattern names and
    /// writes them into the result, which is the same code for every vector type because the components are
    /// reached through the indexer of <c>IVector</c>. The helper is emitted once for all of the vectors.
    /// </summary>
    /// <returns>The helper</returns>
    private static string GenShuffleSoft()
    {
        var comp = Typ.xyzw;
        var lh = "Shuffle42";

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// The shuffle of a vector that has no register to shuffle");
        sb.AppendLine("/// <para>A vector without a register keeps its components in fields, so a shuffle of it reads the");
        sb.AppendLine("/// components the pattern names and writes them into the result. The components are reached through");
        sb.AppendLine("/// the indexer of <see cref=\"IVector{TSelf,TScalar}\"/>, so the helper is the same for every vector");
        sb.AppendLine("/// type and it is not a part of the surface of the library</para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("internal static class shuffle_soft");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the shuffle of the pattern <paramref name=\"lh\"/>");
        sb.AppendLine("    /// <para>The pattern is a value instead of a part of the name of the member, so the switch of it is");
        sb.AppendLine("    /// the only way to reach every one of the 256 combinations</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <typeparam name=\"T\">The type of the vectors to shuffle</typeparam>");
        sb.AppendLine("    /// <typeparam name=\"S\">The type of a component of the vectors</typeparam>");
        sb.AppendLine("    /// <param name=\"a\">The vector the low half of the result takes its components from</param>");
        sb.AppendLine("    /// <param name=\"b\">The vector the high half of the result takes its components from</param>");
        sb.AppendLine("    /// <param name=\"lh\">The pattern of the shuffle, it is the name of the member that shuffles it</param>");
        sb.AppendLine("    /// <returns>The vector that the pattern <paramref name=\"lh\"/> names</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine("    public static T shuffle<T, S>(in T a, in T b, Shuffle42 lh)");
        sb.AppendLine("        where T : unmanaged, IVector<T, S>");
        sb.AppendLine("        where S : unmanaged");
        sb.AppendLine("    {");
        sb.AppendLine("        T r = default;");
        sb.AppendLine("        switch (lh)");
        sb.AppendLine("        {");
        for (var i = 0; i < comp.Length; i++)
        {
            for (var j = 0; j < comp.Length; j++)
            {
                for (var k = 0; k < comp.Length; k++)
                {
                    for (var l = 0; l < comp.Length; l++)
                    {
                        // the digits of the name of a case are the indices of the components of its result
                        sb.AppendLine($"            case {lh}.{comp[i]}{comp[j]}_{comp[k]}{comp[l]}:");
                        sb.AppendLine($"                r[0] = a[{i}];");
                        sb.AppendLine($"                r[1] = a[{j}];");
                        sb.AppendLine($"                r[2] = b[{k}];");
                        sb.AppendLine($"                r[3] = b[{l}];");
                        sb.AppendLine("                break;");
                    }
                }
            }
        }

        sb.AppendLine("            default:");
        sb.AppendLine("                throw new ArgumentOutOfRangeException(nameof(lh), lh, null);");
        sb.AppendLine("        }");
        sb.AppendLine("        return r;");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
