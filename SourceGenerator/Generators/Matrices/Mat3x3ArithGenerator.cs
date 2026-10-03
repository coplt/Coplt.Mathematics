using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the arithmetic members of the square matrix of 3 rows and 3 columns of every kind the arithmetic
/// reaches, which are the ones of a floating point component: <c>half</c>, <c>float</c> and <c>double</c>.
/// <para>The arithmetic of a shape of a square matrix is the one of that shape alone: the value of a shape
/// reaches no member of the arithmetic of another shape, so this generator shares no part of itself with the one
/// of another shape and it holds every part of the arithmetic of the shape it reaches.</para>
/// <para>The members of the shape are the ones the template <c>mat.arith.3x3.tt</c> of the project the library is
/// migrated from holds, and none of them is emitted yet: this generator reaches the host of them for every kind of
/// the shape, so the members of a kind are written into the file of that kind afterwards.</para>
/// </summary>
[Generator]
public class Mat3x3ArithGenerator : IIncrementalGenerator
{
    /// <summary>The number of the rows and the number of the columns of the square matrix of this generator.</summary>
    private const int Size = 3;

    /// <summary>The doc of the members the arithmetic of the shape reaches.</summary>
    private const string Doc = """
                               The members the arithmetic of the shape reaches are the ones the template <c>mat.arith.3x3.tt</c> of the
                               project the library is migrated from holds: the constructors beside the ones of the value, which take the
                               upper left of a matrix of 4 rows and 4 columns and the rotation of a quaternion, the rotations of an axis
                               and an angle, of the six orders of the Euler angles and of the three axes, the rotations a forward and an
                               up vector reach, the scales, the conversion of a matrix of 4 rows and 4 columns, and the inverse and the
                               determinant of the value in the class of the math members. None of them is emitted yet, so this file
                               reaches the type of the value of the matrix alone, which is the type the members of the value are added to
                               and the one the members of a single matrix take their value from when they are added to the class of the
                               math members.
                               """;

    /// <summary>
    /// The kinds the arithmetic of a square matrix reaches, which are the ones of a floating point component:
    /// <c>half</c>, <c>float</c> and <c>double</c>. A rotation, a scale, an inverse and a determinant are members
    /// of the arithmetic of a floating point kind alone.
    /// </summary>
    private static readonly Typ[] Kinds = Typ.Typs.Where(static typ => typ.f).ToArray();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => AddSources(ctx));
    }

    /// <summary>
    /// Adds the file of every kind the arithmetic reaches for the square matrix of 3 rows and 3 columns, which is
    /// the host of the members of the arithmetic of that shape and kind.
    /// </summary>
    /// <param name="ctx">The context of the generator</param>
    private static void AddSources(IncrementalGeneratorPostInitializationContext ctx)
    {
        foreach (var typ in Kinds)
        {
            var name = MatrixGenerator.Name(typ, Size, Size, false);
            ctx.AddSource(
                $"{VectorGenerator.VecNamespace}.matarith.{name}.g.cs",
                SourceText.From(Gen(name), Encoding.UTF8));
        }
    }

    /// <summary>
    /// Generates the file of the host of the members of the arithmetic of the square matrix <paramref name="name"/>,
    /// which is the type of the value of the matrix: the class of the math members reaches the members of a matrix
    /// as well, but it is the class of every shape and every kind, so the file of a shape and a kind reaches the
    /// type of the value of it and the doc of that type names the members that are to come.
    /// </summary>
    /// <param name="name">The name of the square matrix</param>
    /// <returns>The file of the host</returns>
    private static string Gen(string name)
    {
        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, simdHelpers: false);
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// The arithmetic members of the square matrix of {Size} rows and {Size} columns of <see cref=\"{name}\"/>");
        sb.AppendLine("/// <para>");
        foreach (var line in Doc.Replace("\r\n", "\n").Split('\n'))
            sb.AppendLine(line.Length == 0 ? "///" : $"/// {line}");
        sb.AppendLine("/// </para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public partial struct {name}");
        sb.AppendLine("{");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
