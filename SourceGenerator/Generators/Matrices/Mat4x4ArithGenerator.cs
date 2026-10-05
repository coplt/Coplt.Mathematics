using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the arithmetic members of the square matrix of 4 rows and 4 columns of every kind the arithmetic
/// reaches, which are the ones of a floating point component: <c>half</c>, <c>float</c> and <c>double</c>.
/// <para>The arithmetic of a shape of a square matrix is the one of that shape alone: the value of a shape
/// reaches no member of the arithmetic of another shape, so this generator shares no part of itself with the one
/// of another shape and it holds every part of the arithmetic of the shape it reaches.</para>
/// <para>The members of the shape are the ones the template <c>mat.arith.4x4.tt</c> of the project the library is
/// migrated from holds: the matrix whose upper left is a rotation of 3 rows and 3 columns and whose fourth column is
/// a translation of 3 components, the scale of a single component, of three of them and of a value of 3 components,
/// the translation of a value of 3 components, the rotation of the space around the axis of a value of 3 components
/// by an angle, the rotation of the three Euler angles of a value of 3 components, the rotation of a single axis of
/// the space by an angle, the view of an eye position and a target, the projection of a field of view and of a view
/// volume, the transform of a translation, a rotation and a scale beside the one of a translation and a rotation, the
/// rotation and the transform of a vector in the class of the math members, and the inverse, the fast inverse and the
/// determinant of the value in it. So far this generator reaches the matrix of a rotation and a translation, the
/// scale, the translation and the rotation of the space, which are the members of a rotation of 3 rows and 3 columns
/// beside the ones of a value of 3 components; the members of the view of the space, of the projection of it, of the
/// transform of a value, of a vector in the class of the math members and of the inverse, the fast inverse and the
/// determinant of the value are to come.</para>
/// </summary>
[Generator]
public class Mat4x4ArithGenerator : IIncrementalGenerator
{
    /// <summary>The number of the rows and the number of the columns of the square matrix of this generator.</summary>
    private const int Size = 4;

    /// <summary>
    /// The number of the rows and the number of the columns of the rotation the value of the matrix of this shape
    /// holds, which is the value of the upper left of it: a rotation of the space of 3 rows and 3 columns and a
    /// translation of 3 components are the whole of the space of a matrix of 4 rows and 4 columns.
    /// </summary>
    private const int Rotation = 3;

    /// <summary>The doc of the members the arithmetic of the shape reaches.</summary>
    private const string Doc = """
                               The members the arithmetic of the shape reaches are the ones the template <c>mat.arith.4x4.tt</c> of the
                               project the library is migrated from holds: the constructors beside the ones of the value, which take the
                               rotation and the translation of a matrix of 3 rows and 3 columns and the rotation of a quaternion, the
                               rotations of an axis and an angle, of the six orders of the Euler angles and of the three axes, the views
                               of an eye position and a target, the projections of a field of view and of a view volume, the transform of
                               a translation, a rotation and a scale beside the one of a translation and a rotation, the scales, the
                               translation, the rotation and the transform of a vector in the class of the math members, and the inverse,
                               the fast inverse and the determinant of the value in it. So far this file reaches the matrix of a rotation
                               of 3 rows and 3 columns and a translation, the scale of a single component, of three of them and of a value
                               of 3 components, the translation of a value of 3 components and the rotation of the space around the axis
                               of a value of 3 components by an angle, of the three axes of it and of the three Euler angles of it in one
                               of the six orders of them, which are the members of a rotation of 3 rows and 3 columns beside the ones of
                               a value of 3 components. The members of the view of an eye position and a target, of the projections of a
                               field of view and of a view volume, of the transform of a translation, a rotation and a scale and of the
                               one of a translation and a rotation, of a vector in the class of the math members and of the inverse, the
                               fast inverse and the determinant of the value are to come.
                               """;

    /// <summary>
    /// The kinds the arithmetic of a square matrix reaches, which are every kind a number names: the matrix of a
    /// rotation of 3 rows and 3 columns and a translation, the scale of a single component, of three of them and of a
    /// value of 3 components and the translation of a value of 3 components are the members of every one of them, and
    /// a rotation of the space is the member of a floating point kind alone, since the angle of a rotation is the one
    /// of a floating point component.
    /// </summary>
    private static readonly Typ[] Kinds = Typ.Typs.Where(static typ => typ.arith).ToArray();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => AddSources(ctx));
    }

    /// <summary>
    /// Adds the file of the members of the arithmetic of the square matrix of 4 rows and 4 columns of every kind
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
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// The arithmetic members of the square matrix of {Size} rows and {Size} columns of <see cref=\"{name}\"/>");
        sb.AppendLine("/// <para>");
        AddDoc(sb, Doc, "");
        sb.AppendLine("/// </para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public partial struct {name}");
        sb.AppendLine("{");
        // the members of a rotation of 3 rows and 3 columns, of a translation of 3 components and of the scale are
        // the members of every kind a number names, so the file reaches them, and the members of the rotation of the
        // space are the members of a floating point kind alone, which read the members of a rotation of 3 rows and 3
        // columns of them
        sb.AppendLine("    // the members that take the rotation of a quaternion, the view of the space, the projection of it and");
        sb.AppendLine("    // the transform of a value are to come, and the type of a quaternion is not written yet, so this file");
        sb.AppendLine("    // reaches the members of the value of the space and the ones of a rotation of it alone");
        sb.AppendLine();
        From3x3(sb, name, typ);
        sb.AppendLine();
        Scales(sb, name, typ);
        sb.AppendLine();
        Translates(sb, name, typ);
        // a rotation of the space is the member of a floating point kind alone, since the angle of a rotation is the
        // one of a floating point component
        if (typ.f)
        {
            sb.AppendLine();
            Rotations(sb, name, typ);
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
    /// Adds the member that reaches the matrix of the rotation of a value of 3 components and of the translation of
    /// another one to the square matrix <paramref name="name"/>: the value of the upper left of the matrix is the
    /// rotation, the value of the fourth column of it is the translation and the last row of it is the zero of the
    /// kind beside the one of it, so a rotation of the space and a translation of it are the whole of the matrix the
    /// member reaches.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void From3x3(StringBuilder sb, string name, Typ typ)
    {
        var rotation = MatrixGenerator.Name(typ, Rotation, Rotation, false);
        var vec = VectorGenShared.VecName(typ, Rotation, false);
        var vec4 = VectorGenShared.VecName(typ, Size, false);
        var one = typ.one;
        AddDoc(sb, $"""
                    Returns the matrix of {Size} rows and {Size} columns whose upper left is the value of
                    <paramref name="rotation"/> and whose fourth column is the value of <paramref name="translation"/>
                    <para>The value of the upper left is the whole of the rotation, the value of the fourth column of the matrix
                    is the one the origin of the space is moved by and the last row of it is the zero of the kind beside the one
                    of it, which is the row the value of the fourth coordinate of the space is read from</para>
                    """, "    ");
        sb.AppendLine("    /// <param name=\"rotation\">The value of the upper left, a rotation of the space of 3 rows and 3 columns</param>");
        sb.AppendLine("    /// <param name=\"translation\">The value of the fourth column, which is the value of the origin of the space</param>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {name}({rotation} rotation, {vec} translation) => this = new(");
        sb.AppendLine("        rotation.c0.as4,");
        sb.AppendLine("        rotation.c1.as4,");
        sb.AppendLine("        rotation.c2.as4,");
        sb.AppendLine($"        new {vec4}(translation, {one})");
        sb.AppendLine("    );");
    }

    /// <summary>
    /// Adds the members that reach the matrix that scales the space to the square matrix <paramref name="name"/>,
    /// which are the matrices whose diagonal holds the values the axes of the space are scaled by: a matrix of 4 rows
    /// and 4 columns holds the three axes of the space beside the fourth one of them, which a scale of the space
    /// leaves where it is, so the value of the fourth one is the one of the kind.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Scales(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec = VectorGenShared.VecName(typ, Rotation, false);
        var vec4 = VectorGenShared.VecName(typ, Size, false);
        var one = typ.one;

        AddDoc(sb, """
                   Returns the matrix that scales every axis of the space by <paramref name="s"/>
                   <para>The matrix holds the value on the diagonal of it and the zero of the kind of it everywhere else, so
                   every axis of the space is scaled on its own, and the fourth axis of it keeps the one of the kind</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"s\">The value every axis of the space is scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({scalar} s) => new(");
        sb.AppendLine("        s,       default, default, default,");
        sb.AppendLine("        default, s,       default, default,");
        sb.AppendLine("        default, default, s,       default,");
        sb.AppendLine($"        default, default, default, {one}");
        sb.AppendLine("    );");
        sb.AppendLine();

        AddDoc(sb, """
                   Returns the matrix that scales the first axis of the space by <paramref name="x"/>, the second axis of
                   it by <paramref name="y"/> and the third axis of it by <paramref name="z"/>
                   <para>The matrix holds the three values on the diagonal of it and the zero of the kind of it everywhere
                   else, so every axis of the space is scaled on its own, and the fourth axis of it keeps the one of the
                   kind</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"x\">The value the first axis of the space is scaled by</param>");
        sb.AppendLine("    /// <param name=\"y\">The value the second axis of the space is scaled by</param>");
        sb.AppendLine("    /// <param name=\"z\">The value the third axis of the space is scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({scalar} x, {scalar} y, {scalar} z) => new(");
        sb.AppendLine("        x,       default, default, default,");
        sb.AppendLine("        default, y,       default, default,");
        sb.AppendLine("        default, default, z,       default,");
        sb.AppendLine($"        default, default, default, {one}");
        sb.AppendLine("    );");
        sb.AppendLine();

        AddDoc(sb, """
                   Returns the matrix that scales the first axis of the space by the first component of
                   <paramref name="v"/>, the second axis of it by the second component of it and the third axis of it by
                   the third component of it
                   <para>The matrix holds the components of the value on the diagonal of it and the zero of the kind of it
                   everywhere else, so every axis of the space is scaled on its own, and the fourth axis of it keeps the one
                   of the kind</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"v\">The value whose components the axes of the space are scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        if (VectorGenShared.Simd(typ, Rotation, false))
        {
            // the padding lane of the value of a vector that keeps it in a register is zero, so a column of the
            // matrix is the swizzle of the value that reads the component the column scales beside the padding lane,
            // and the value of the swizzle of 4 components is the column itself, which keeps the padding lane of it
            // at zero, so the column reaches it without a mask
            sb.AppendLine($"    public static {name} Scale({vec} v) => new(");
            sb.AppendLine("        v.as4.xwww,");
            sb.AppendLine("        v.as4.wyww,");
            sb.AppendLine("        v.as4.wwzw,");
            sb.AppendLine($"        new {vec4}(default, default, default, {one})");
            sb.AppendLine("    );");
        }
        else
        {
            sb.AppendLine($"    public static {name} Scale({vec} v) => new(");
            sb.AppendLine("        v.x,     default, default, default,");
            sb.AppendLine("        default, v.y,     default, default,");
            sb.AppendLine("        default, default, v.z,     default,");
            sb.AppendLine($"        default, default, default, {one}");
            sb.AppendLine("    );");
        }
    }

    /// <summary>
    /// Adds the member that reaches the matrix that translates the origin of the space to the square matrix
    /// <paramref name="name"/>: the matrix keeps every axis of the space where it is, so the diagonal of it is the one
    /// of the kind, and the value of the fourth column of it is the value the origin of the space is moved by, which
    /// the value of a transform of the space reads beside the three axes of it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Translates(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Rotation, false);
        var vec4 = VectorGenShared.VecName(typ, Size, false);
        var one = typ.one;
        AddDoc(sb, """
                   Returns the matrix that translates the origin of the space by <paramref name="translation"/>
                   <para>The matrix holds the one of the kind on the diagonal of it, so no axis of the space is scaled and no
                   axis of it is turned, and the value of the fourth column of it is the value of the origin of the space</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"translation\">The value the origin of the space is moved by</param>");
        sb.AppendLine("    /// <returns>The matrix of the translation</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Translate({vec} translation) => new(");
        sb.AppendLine($"        new {vec4}({one}, default, default, default),");
        sb.AppendLine($"        new {vec4}(default, {one}, default, default),");
        sb.AppendLine($"        new {vec4}(default, default, {one}, default),");
        sb.AppendLine($"        new {vec4}(translation, {one})");
        sb.AppendLine("    );");
    }

    /// <summary>
    /// The three rotations of a single axis, which are the axes of the space in the order of them: the axis of a
    /// rotation keeps the value it is handed, the axis that follows the one of the rotation reaches the one that
    /// follows the two of them where the angle of the rotation is a right angle.
    /// </summary>
    private static readonly (string Axis, string Next, string Next2)[] Axes =
    {
        ("x", "y", "z"),
        ("y", "z", "x"),
        ("z", "x", "y"),
    };

    /// <summary>
    /// The six orders of the three Euler angles, which are the six permutations of the three axes of the space.
    /// </summary>
    private static readonly string[] Orders = { "XYZ", "XZY", "YXZ", "YZX", "ZXY", "ZYX" };

    /// <summary>
    /// Adds the members that reach the matrix of a rotation of the space to the square matrix <paramref name="name"/>:
    /// the rotation of the space turns the three axes of it and leaves the fourth one where it is, so the value of the
    /// upper left of the matrix is a rotation of the shape of 3 rows and 3 columns and the last column of it is the
    /// zero of the kind beside the one of it. Every member reads the member of a rotation of the shape of 3 rows and 3
    /// columns whose angle is the one it is handed, so a rotation of the space holds nothing of the arithmetic of a
    /// rotation and the angle of the value is read once.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Rotations(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec = VectorGenShared.VecName(typ, Rotation, false);
        var rotation = MatrixGenerator.Name(typ, Rotation, Rotation, false);
        var vec4 = VectorGenShared.VecName(typ, Size, false);
        sb.AppendLine("    // the rotation of the space turns the three axes of it and leaves the fourth one where it is, so the");
        sb.AppendLine("    // value of the upper left of the matrix is the rotation of the shape of 3 rows and 3 columns of it and");
        sb.AppendLine("    // the last column of it is the zero of the kind beside the one of it, which is the value of the fourth");
        sb.AppendLine("    // coordinate of the space");
        var last = $"new {vec4}(default, default, default, {typ.one})";

        // the value of a member of this shape is the rotation of the shape of 3 rows and 3 columns of it beside the
        // last column: every column of the rotation is read as the value of 4 components of it, which is the column
        // itself with the zero of the kind in the fourth one
        void Body(string call)
        {
            sb.AppendLine("    {");
            sb.AppendLine($"        var r = {call};");
            sb.AppendLine($"        return new(r.c0.as4, r.c1.as4, r.c2.as4, {last});");
            sb.AppendLine("    }");
        }

        sb.AppendLine();
        AddDoc(sb, """
                   Returns the matrix that rotates the value around the axis of <paramref name="axis"/> by
                   <paramref name="angle"/>, which is the rotation of the value around the origin of the space
                   <para>Every angle of the rotation is in radians and the rotation of an angle is clockwise where the axis
                   of it is looked along towards the origin, so the length of the value is the one it was handed where the
                   axis of the rotation is of the length one and not the zero of the kind, and the rotation turns the three
                   axes of the space and leaves the fourth one where it is</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"axis\">The axis of the rotation, which is of the length one</param>");
        sb.AppendLine("    /// <param name=\"angle\">The angle of the rotation, in radians</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation around the axis</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} AxisAngle({vec} axis, {scalar} angle)");
        Body($"{rotation}.AxisAngle(axis, angle)");

        sb.AppendLine();
        AddDoc(sb, """
                   Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the one
                   of the z-x-y order
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                   is looked along towards the origin, and the rotation turns the three axes of the space and leaves the
                   fourth one where it is</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation in the z-x-y order</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Euler({vec} xyz) => EulerZXY(xyz);");

        sb.AppendLine();
        AddDoc(sb, """
                   Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/> in the order
                   <paramref name="order"/> names
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                   is looked along towards the origin, and the rotation turns the three axes of the space and leaves the
                   fourth one where it is</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
        sb.AppendLine("    /// <param name=\"order\">The order the three rotations of the angles are applied in</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation in the order of <paramref name=\"order\"/></returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Euler({vec} xyz, RotationOrder order) => order switch");
        sb.AppendLine("    {");
        foreach (var order in Orders)
            sb.AppendLine($"        RotationOrder.{order} => Euler{order}(xyz),");
        sb.AppendLine("        _ => EulerZXY(xyz),");
        sb.AppendLine("    };");

        foreach (var order in Orders)
        {
            var axes = order.ToLowerInvariant();
            sb.AppendLine();
            AddDoc(sb, $"""
                        Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the rotation
                        around the {axes[0]} axis by {Word(axes[0])} of them, then the rotation around the {axes[1]} axis by {Word(axes[1])} one
                        and finally the rotation around the {axes[2]} axis by {Word(axes[2])} one
                        <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                        is looked along towards the origin, and the rotation turns the three axes of the space and leaves the fourth
                        one where it is</para>
                        """, "    ");
            sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
            sb.AppendLine($"    /// <returns>The matrix of the rotation in the {Spelling(order)} order</returns>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {name} Euler{order}({vec} xyz)");
            Body($"{rotation}.Euler{order}(xyz)");
        }

        foreach (var (axis, next, next2) in Axes)
        {
            sb.AppendLine();
            AddDoc(sb, $"""
                        Returns the matrix that rotates the value around the {axis} axis by <paramref name="angle"/>, which is the
                        rotation of the value around the origin of the space
                        <para>Every angle of the rotation is in radians and the rotation of an angle is clockwise where the axis of
                        it is looked along towards the origin, so the {axis} axis keeps the value it is handed, the {next} axis
                        reaches the {next2} axis where the angle of the rotation is a right angle and the length of the value is the
                        one it was handed</para>
                        """, "    ");
            sb.AppendLine("    /// <param name=\"angle\">The angle of the rotation, in radians</param>");
            sb.AppendLine($"    /// <returns>The matrix of the rotation around the {axis} axis</returns>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {name} Rotate{axis.ToUpperInvariant()}({scalar} angle)");
            Body($"{rotation}.Rotate{axis.ToUpperInvariant()}(angle)");
        }
    }

    /// <summary>
    /// Returns the name of the position of an axis inside the value of the three Euler angles, which is the first
    /// position, the second one and the third one.
    /// </summary>
    /// <param name="axis">The axis</param>
    /// <returns>The name of the position of the axis</returns>
    private static string Word(char axis) => "xyz".IndexOf(axis) switch
    {
        0 => "the first",
        1 => "the second",
        _ => "the third",
    };

    /// <summary>
    /// Returns the name of an order of the angles with the axes of it separated by a hyphen.
    /// </summary>
    /// <param name="order">The name of the order</param>
    /// <returns>The name of the order with the axes of it spelled out</returns>
    private static string Spelling(string order) => string.Join("-", order.Select(char.ToLowerInvariant));
}
