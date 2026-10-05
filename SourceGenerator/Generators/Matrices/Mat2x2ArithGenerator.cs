using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the arithmetic members of the square matrix of 2 rows and 2 columns of every kind a number names:
/// <c>short</c>, <c>ushort</c>, <c>int</c>, <c>uint</c>, <c>long</c>, <c>ulong</c>, <c>half</c>, <c>float</c> and
/// <c>double</c>.
/// <para>The arithmetic of a shape of a square matrix is the one of that shape alone: the value of a shape
/// reaches no member of the arithmetic of another shape, so this generator shares no part of itself with the one
/// of another shape and it holds every part of the arithmetic of the shape it reaches.</para>
/// <para>The members of the shape are the rotation of an angle, the skew of two angles, the scale of a single
/// component, of two of them and of a column of the value, and the inverse and the determinant of the value. The
/// scale is a member of every kind and the rotation and the skew are the members of a floating point kind alone:
/// the members of the type of the value are the ones this generator reaches, and the inverse and the determinant
/// are the members of the class of the math members, which are written by hand.</para>
/// </summary>
[Generator]
public class Mat2x2ArithGenerator : IIncrementalGenerator
{
    /// <summary>The number of the rows and the number of the columns of the square matrix of this generator.</summary>
    private const int Size = 2;

    /// <summary>
    /// The kinds the arithmetic of a square matrix reaches, which are every kind a number names: the scale of a
    /// single component, of two of them and of a column of the value is a member of every one of them, and the
    /// rotation of an angle, the skew of two angles and the inverse and the determinant of the value are the members
    /// of a floating point kind alone.
    /// </summary>
    private static readonly Typ[] Kinds = Typ.Typs.Where(static typ => typ.arith).ToArray();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => AddSources(ctx));
    }

    /// <summary>
    /// Adds the file of the members of the arithmetic of the square matrix of 2 rows and 2 columns of every kind
    /// the arithmetic reaches, which holds the members of that shape and kind.
    /// </summary>
    /// <param name="ctx">The context of the generator</param>
    private static void AddSources(IncrementalGeneratorPostInitializationContext ctx)
    {
        foreach (var typ in Kinds)
        {
            var name = MatrixGenerator.Name(typ, Size, Size, false);
            ctx.AddSource(
                $"{VectorGenerator.VecNamespace}.square.{name}.g.cs",
                SourceText.From(Gen(name, typ), Encoding.UTF8));
        }
    }

    /// <summary>
    /// Generates the file of the members of the arithmetic of the square matrix <paramref name="name"/>, which is
    /// the type of the value of the matrix: the class of the math members reaches the members of a matrix as well,
    /// but it is the class of every shape and every kind, so the file of a shape and a kind reaches the type of the
    /// value of it and the members of the arithmetic that take the value of a single matrix.
    /// </summary>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    /// <returns>The file of the members</returns>
    private static string Gen(string name, Typ typ)
    {
        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, simdHelpers: false);
        sb.AppendLine();
        // the doc of the type of the value is carried by another declaration of it, so this file reaches the members
        // of the arithmetic of the shape alone
        sb.AppendLine($"public partial struct {name}");
        sb.AppendLine("{");
        // the rotation and the skew are the members of a floating point kind alone, the scale is the member of every
        // kind of a number, and a blank line separates the members of the type
        if (typ.f)
        {
            Rotate(sb, name, typ);
            sb.AppendLine();
        }

        Scale(sb, name, typ);
        if (typ.f)
        {
            sb.AppendLine();
            Skew(sb, name, typ);
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Adds the lines of <paramref name="doc"/> to the source, every one of them is a line of the doc of the member
    /// that follows them.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="doc">The doc of a member, which carries no comment of its own</param>
    /// <param name="indent">The indentation of the member the doc belongs to</param>
    private static void AddDoc(StringBuilder sb, string doc, string indent)
    {
        foreach (var line in doc.Replace("\r\n", "\n").Split('\n'))
            sb.AppendLine(line.Length == 0 ? $"{indent}///" : $"{indent}/// {line}");
    }

    /// <summary>
    /// Adds the member that rotates the plane by an angle to the square matrix <paramref name="name"/>, which is
    /// the matrix of the rotation of a value of 2 components around the origin.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Rotate(StringBuilder sb, string name, Typ typ)
    {
        // the angle of a rotation is a component of the kind of the matrix, so the rotation of a value is the one
        // of a floating point kind alone and the kind of the matrix decides the member
        var scalar = typ.compType;
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that rotates the plane around the origin by <paramref name=\"angle\"/>, which");
        sb.AppendLine("    /// is the angle of the rotation of the value of 2 components that the matrix is multiplied by, and the");
        sb.AppendLine("    /// angle of the rotation is in radians");
        sb.AppendLine("    /// <para>The rotation keeps the length of the value it is handed and the origin of the plane, and the");
        sb.AppendLine("    /// x axis of the plane reaches the y one where the angle of the rotation is a right angle, so a");
        sb.AppendLine("    /// value turns the way the plane turns where the angle of the rotation grows</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"angle\">The angle of the rotation, in radians</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Rotate({scalar} angle)");
        sb.AppendLine("    {");
        sb.AppendLine("        math.sincos(angle, out var s, out var c);");
        sb.AppendLine("        return new(c, -s, s, c);");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the members that scale the plane to the square matrix <paramref name="name"/>, which are the matrices
    /// whose diagonal holds the values the axes of the plane are scaled by.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Scale(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec = VectorGenShared.VecName(typ, Size, false);

        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that scales every axis of the plane by <paramref name=\"s\"/>");
        sb.AppendLine("    /// <para>The matrix holds the value on the diagonal of it and the zero of the kind of it everywhere");
        sb.AppendLine("    /// else, so every axis of the plane is scaled on its own</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"s\">The value every axis of the plane is scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({scalar} s) => new(");
        sb.AppendLine("        s,       default,");
        sb.AppendLine("        default, s");
        sb.AppendLine("    );");
        sb.AppendLine();

        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that scales the x axis of the plane by <paramref name=\"x\"/> and the y axis of");
        sb.AppendLine("    /// it by <paramref name=\"y\"/>");
        sb.AppendLine("    /// <para>The matrix holds the two values on the diagonal of it and the zero of the kind of it everywhere");
        sb.AppendLine("    /// else, so every axis of the plane is scaled on its own</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"x\">The value the x axis of the plane is scaled by</param>");
        sb.AppendLine("    /// <param name=\"y\">The value the y axis of the plane is scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({scalar} x, {scalar} y) => new(");
        sb.AppendLine("        x,       default,");
        sb.AppendLine("        default, y");
        sb.AppendLine("    );");
        sb.AppendLine();

        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that scales the x axis of the plane by the first component of");
        sb.AppendLine("    /// <paramref name=\"v\"/> and the y axis of it by the second component of it");
        sb.AppendLine("    /// <para>The matrix holds the components of the value on the diagonal of it and the zero of the kind of");
        sb.AppendLine("    /// it everywhere else, so every axis of the plane is scaled on its own</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"v\">The value whose components the axes of the plane are scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({vec} v) => new(");
        sb.AppendLine("        v.x,     default,");
        sb.AppendLine("        default, v.y");
        sb.AppendLine("    );");
    }

    /// <summary>
    /// Adds the member that skews the plane by two angles to the square matrix <paramref name="name"/>, which is
    /// the matrix whose diagonal is the one of the kind of the value and whose other components are the tangents of
    /// the two angles.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Skew(StringBuilder sb, string name, Typ typ)
    {
        // the tangent of an angle is a component of the kind of the matrix, so the skew of a value is the one of a
        // floating point kind alone and the kind of the matrix decides the member
        var scalar = typ.compType;
        var vec = VectorGenShared.VecName(typ, Size, false);
        var one = typ.one;
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that skews the plane by the angles <paramref name=\"ax\"/> and <paramref name=\"ay\"/>,");
        sb.AppendLine("    /// which is the matrix whose diagonal is the one of the kind of the value and whose other components");
        sb.AppendLine("    /// are the tangents of the two angles");
        sb.AppendLine("    /// <para>The product of the matrix with a value of 2 components is the value whose first component holds");
        sb.AppendLine("    /// the product of the tangent of <paramref name=\"ax\"/> with the second component of it beside its own,");
        sb.AppendLine("    /// and whose second component holds the product of the tangent of <paramref name=\"ay\"/> with the first");
        sb.AppendLine("    /// component of it beside its own, so every axis of the plane is skewed along the other one</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"ax\">The angle of the skew of the y axis of the plane along the x axis of it</param>");
        sb.AppendLine("    /// <param name=\"ay\">The angle of the skew of the x axis of the plane along the y axis of it</param>");
        sb.AppendLine("    /// <returns>The matrix of the skew</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Skew({scalar} ax, {scalar} ay) => Skew(new {vec}(ax, ay));");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that skews the plane by the angle of the first component of <paramref name=\"v\"/>");
        sb.AppendLine("    /// along the y axis of it and by the angle of the second component of it along the x axis of it");
        sb.AppendLine("    /// <para>The matrix holds the tangents of the two angles beside the one of the kind of the value on the");
        sb.AppendLine("    /// diagonal of it, so every axis of the plane is skewed along the other one</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"v\">The value whose components the angles of the skew are</param>");
        sb.AppendLine("    /// <returns>The matrix of the skew</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Skew({vec} v)");
        sb.AppendLine("    {");
        // the two angles reach the member of the vector in one call, so the form of a value of 2 components is the
        // one that computes the two tangents and the form of two components hands the two of them over to it
        sb.AppendLine("        var t = math.tan(v);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            {one}, t.x,");
        sb.AppendLine($"            t.y, {one}");
        sb.AppendLine("        );");
        sb.AppendLine("    }");
    }
}
