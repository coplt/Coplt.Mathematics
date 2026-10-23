using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the member that transposes every matrix of <see cref="Typ"/>: the transpose of a matrix is the matrix
/// whose rows are the columns of the value and the columns of it are the rows of the value, so the type of the
/// result of a transpose is the type of the value with the two counts of it swapped. The type of the result is a
/// part of the type of the value and the compiler cannot infer it from the value alone once the two counts of it
/// differ, so the member of every shape names the type of the result itself and a caller reaches the member of the
/// shape of the value.
/// <para>Every kind of a number and every shape of a matrix has the member of its own: the value of a shape that
/// keeps its columns in a register reaches the member of the simd library for that shape where the shape of it is
/// one the member of the library covers, and every other shape and every kind without a register is transposed by
/// the scalar path, which reads the component of a column of the value at the index of a column of the result and
/// builds that column out of the components it reads. The members are emitted into the two classes of the members
/// of the math class: the member of the math class names the type of the result where it is called and the member
/// of the value, which the class of the value carries, reads like a member of the value itself.</para>
/// </summary>
[Generator]
public class MatrixTransposeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (var typ in Typ.Typs)
            {
                for (var rows = 2; rows <= 4; rows++)
                {
                    for (var cols = 2; cols <= 4; cols++)
                    {
                        var name = MatrixGenerator.Name(typ, rows, cols, false);
                        ctx.AddSource(
                            $"{VectorGenerator.VecNamespace}.transpose.{name}.g.cs",
                            SourceText.From(Gen(typ, rows, cols), Encoding.UTF8));
                    }
                }
            }
        });
    }

    /// <summary>
    /// Generates the member that transposes the matrix of <paramref name="rows"/> rows and
    /// <paramref name="cols"/> columns of <paramref name="typ"/>.
    /// </summary>
    /// <param name="typ">The type of a component of the matrix</param>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <returns>The file of the member</returns>
    private static string Gen(Typ typ, int rows, int cols)
    {
        var type = MatrixGenerator.Name(typ, rows, cols, false);
        var resType = MatrixGenerator.Name(typ, cols, rows, false);
        // the register of a column of the value, which is the 128 bit one of it at the least
        var simd = VectorGenShared.Simd(typ, rows, false);
        var register = VectorGenShared.Register(typ, rows, false);
        // the member of the simd library of a shape and the number of the registers it takes, which is the count of
        // the columns of the value beside the padding lanes the member of a shape that is not square takes
        var helper = Helper(rows, cols);
        var operands = helper is null ? 0 : Operands(rows, cols);

        // true when the register of a column of the result is wider than that column, so the lanes of the register
        // of it that follow the components of the column are padding and the member of the simd library of the
        // shape keeps them at zero, which lets the column write the field of the vector directly instead of going
        // through the constructor that masks the padding lanes of it
        var pad = VectorGenShared.PadLanes(typ, cols, false) > 0;

        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, simdHelpers: true);
        sb.AppendLine();

        // the member of the math class, the type of the result is named where the member is called
        sb.AppendLine("public static partial class math");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>Returns the transpose of the value, which is the matrix whose rows are the columns of the value</summary>");
        sb.AppendLine($"    /// <param name=\"m\">The value, a matrix of {rows} rows and {cols} columns</param>");
        sb.AppendLine($"    /// <returns>The transpose of the value, which is a matrix of {cols} rows and {rows} columns</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {resType} transpose({type} m)");
        sb.AppendLine("    {");
        if (simd && helper is not null)
        {
            var args = Enumerable.Range(0, cols).Select(j => $"m.c{j}.vector").Concat(
                Enumerable.Range(cols, operands - cols).Select(_ => "default"));
            // the lanes of the register of a column of the result that follow the components of that column are
            // padding and the member of the simd library keeps them at zero, so the column writes the field of the
            // vector directly and skips the mask of the constructor of it
            var result = Enumerable.Range(0, rows).Select(i => VectorGenShared.Vector(simd, pad, $"r.c{i}"));
            sb.AppendLine($"        if (Vector{register}.IsHardwareAccelerated)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var r = simd_matrix.{helper}({string.Join(", ", args)});");
            sb.AppendLine($"            return new({string.Join(", ", result)});");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        // the scalar path: the column of the result at the index of a column of the value is the component of every
        // column of the value at the index of that column of the result
        sb.AppendLine("        return new(");
        for (var i = 0; i < rows; i++)
        {
            var comp = Typ.xyzw[i];
            var values = string.Join(", ", Enumerable.Range(0, cols).Select(j => $"m.c{j}.{comp}"));
            sb.AppendLine($"            new({values}){(i == rows - 1 ? "" : ",")}");
        }

        sb.AppendLine("        );");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();

        // the member of the value, it reads like the member of the value itself
        sb.AppendLine("public static partial class math_ex");
        sb.AppendLine("{");
        sb.AppendLine($"    /// <inheritdoc cref=\"math.transpose({type})\"/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {resType} transpose(this {type} m) => math.transpose(m);");
        sb.AppendLine("}");

        return sb.ToString();
    }

    /// <summary>
    /// Returns the member of the simd library that transposes the matrix of <paramref name="rows"/> rows and
    /// <paramref name="cols"/> columns, which is null for a matrix the library does not cover. Every shape of the
    /// range of the generator is one the library covers: the transpose of a matrix of 2 rows and 2 columns is the
    /// swap of the two components that are not on the diagonal of it and the member of the library reaches it out
    /// of the two registers of the value, which the scalar path reaches with four reads of a component.
    /// </summary>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <returns>The name of the member of the simd library or null</returns>
    private static string? Helper(int rows, int cols) => (rows, cols) switch
    {
        (2, 2) => "Transpose2x2",
        (3, 3) => "Transpose3x3",
        (4, 4) => "Transpose4x4",
        (2, 4) => "Transpose2x4To4x2",
        (4, 2) => "Transpose4x2To2x4",
        (3, 4) => "Transpose4x4",
        (4, 3) => "Transpose4x4",
        (2, 3) => "Transpose2x4To4x2",
        (3, 2) => "Transpose4x2To2x4",
        _ => null,
    };

    /// <summary>
    /// Returns the number of the registers the member of the simd library of a shape takes, which is the count of
    /// the columns of the value and the number of the registers the member of it of the shape of the result takes
    /// where the shape of the value is not the shape of the result.
    /// </summary>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <returns>The number of the registers</returns>
    private static int Operands(int rows, int cols) => (rows, cols) switch
    {
        (2, 2) => 2,
        (3, 3) => 3,
        (4, 4) => 4,
        (2, 4) => 4,
        (4, 2) => 2,
        (3, 4) => 4,
        (4, 3) => 4,
        (2, 3) => 4,
        (3, 2) => 2,
        _ => 0,
    };
}
