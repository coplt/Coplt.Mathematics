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
/// scale, the translation, the rotation of the space, the views of it, the projections of it and the rotation and the
/// transform of a value of 3 components in the class of the math members; the members of the transform of a
/// translation, a rotation and a scale beside the one of a translation and a rotation are to come beside the type of a
/// quaternion, and the inverse, the fast inverse and the determinant of the value are written by hand.</para>
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

    /// <summary>
    /// The kinds the arithmetic of a square matrix reaches, which are every kind a number names: the matrix of a
    /// rotation of 3 rows and 3 columns and a translation, the scale of a single component, of three of them and of a
    /// value of 3 components and the translation of a value of 3 components are the members of every one of them, and
    /// the rotation of the space, the views of it, the projections of it and the rotation and the transform of a value
    /// of 3 components in the class of the math members are the members of a floating point kind alone, since the
    /// angle of a rotation, the value of a view and a projection and the value of a space of a matrix are the ones of
    /// a floating point component.
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
        // the doc of the type of the value is carried by another declaration of it, so this file reaches the members
        // of the arithmetic of the shape alone
        sb.AppendLine($"public partial struct {name}");
        sb.AppendLine("{");
        // the members of a rotation of 3 rows and 3 columns, of a translation of 3 components and of the scale are
        // the members of every kind a number names, so the file reaches them, and the members of the rotation of the
        // space, of the view of it and of the projection of it are the members of a floating point kind alone, which
        // read the members of a rotation of 3 rows and 3 columns of them where the rotation of the space is the whole
        // of the value
        sb.AppendLine("    // the members that take the rotation of a quaternion and the ones of the transform of a translation, a");
        sb.AppendLine("    // rotation and a scale beside the one of a translation and a rotation are to come, and the type of a");
        sb.AppendLine("    // quaternion is not written yet, so this file reaches the members of the value of the space, the ones of a");
        sb.AppendLine("    // rotation of it, the ones of the view and the projection of it and the ones of the rotation and the");
        sb.AppendLine("    // transform of a value of 3 components of it");
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
            sb.AppendLine();
            Views(sb, name, typ);
            sb.AppendLine();
            Projections(sb, name, typ);
        }

        sb.AppendLine("}");
        // the members that take the value of a matrix beside the one of a value of 3 components are the members of
        // the class of the math members, which the value of a floating point kind alone reaches, since the value of a
        // matrix of another kind is not the one of a rotation and a translation of the space
        if (typ.f)
        {
            sb.AppendLine();
            Transforms(sb, typ);
        }

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
    /// and 4 columns holds the x, y and z axes of the space beside the w axis of it, which a scale of the space
    /// leaves where it is, so the value of the w axis is the one of the kind.
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
                   every axis of the space is scaled on its own, and the w axis of it keeps the one of the kind</para>
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
                   Returns the matrix that scales the x axis of the space by <paramref name="x"/>, the y axis of
                   it by <paramref name="y"/> and the z axis of it by <paramref name="z"/>
                   <para>The matrix holds the three values on the diagonal of it and the zero of the kind of it everywhere
                   else, so every axis of the space is scaled on its own, and the w axis of it keeps the one of the
                   kind</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"x\">The value the x axis of the space is scaled by</param>");
        sb.AppendLine("    /// <param name=\"y\">The value the y axis of the space is scaled by</param>");
        sb.AppendLine("    /// <param name=\"z\">The value the z axis of the space is scaled by</param>");
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
                   Returns the matrix that scales the x axis of the space by the first component of
                   <paramref name="v"/>, the y axis of it by the second component of it and the z axis of it by
                   the third component of it
                   <para>The matrix holds the components of the value on the diagonal of it and the zero of the kind of it
                   everywhere else, so every axis of the space is scaled on its own, and the w axis of it keeps the one
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
    /// the rotation of the space turns the x, y and z axes of it and leaves the w axis where it is, so the value of
    /// the upper left of the matrix is a rotation of the shape of 3 rows and 3 columns and the last column of it is the
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
        sb.AppendLine("    // the rotation of the space turns the x, y and z axes of it and leaves the w axis where it is,");
        sb.AppendLine("    // so the value of the upper left of the matrix is the rotation of the shape of 3 rows and 3 columns");
        sb.AppendLine("    // of it and the last column of it is the zero of the kind beside the one of it, which is the value");
        sb.AppendLine("    // of the fourth coordinate of the space");
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
                   axis of the rotation is of the length one and not the zero of the kind, and the rotation turns the x, y
                   and z axes of the space and leaves the w axis where it is</para>
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
                   is looked along towards the origin, and the rotation turns the x, y and z axes of the space and leaves
                   the w axis where it is</para>
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
                   is looked along towards the origin, and the rotation turns the x, y and z axes of the space and leaves
                   the w axis where it is</para>
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
                        is looked along towards the origin, and the rotation turns the x, y and z axes of the space and leaves the
                        w axis where it is</para>
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
    /// Adds the members that reach the matrix of the view of the space to the square matrix <paramref name="name"/>:
    /// the view of the space is the matrix that reads a value of the space as the value of the eye of a view, which is
    /// turned towards the value the view looks along, so the value of the +z axis of the space of the view is the
    /// one that is looked along and the value of the fourth column of the matrix is the one of the eye of it.
    /// <para>The members of the value of an eye position and the one of a target it looks at read the members of the
    /// value of a direction between the two of them, so the arithmetic of the view of the space is held by the members
    /// of a direction alone. The view of the space is the one of the left of it, which keeps the value that is looked
    /// along on the +z axis of the space, or the one of the right of it, which holds the opposite of that value on the
    /// -z axis of it, and the member that names the rows of the matrix reaches the view of the value as the matrix
    /// whose rows are the axes of the view, which reads a value of the space as the value of the view without a
    /// transpose.</para>
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Views(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Rotation, false);
        var vec4 = VectorGenShared.VecName(typ, Size, false);
        var rotation = MatrixGenerator.Name(typ, Rotation, Rotation, false);
        var one = typ.one;
        // the doc of the value of an eye position and the one of a target it looks at
        const string Eye = """
                           <param name="eye">The value of the eye position of the view, which is a point of the space</param>
                           <param name="target">The value the view looks at, which is a point of the space</param>
                           <param name="up">The value that stays over the one that is looked along, which is of the length one</param>
                           """;
        // the doc of the value of a direction the view looks along
        const string Direction = """
                                 <param name="eye">The value of the eye position of the view, which is a point of the space</param>
                                 <param name="dir">The value the view looks along, which is of the length one</param>
                                 <param name="up">The value that stays over the one that is looked along, which is of the length one</param>
                                 """;
        // a blank line separates two members, and the first one of them follows the member of another group
        var first = true;

        void Separate()
        {
            if (!first) sb.AppendLine();
            first = false;
        }

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the left of it of the eye position <paramref name="eye"/>,
                    which looks along the value between the two points of it and <paramref name="target"/>
                    <para>Every value of the member is read where it is: the two points of the value of the eye position and the
                    one of the target have to be apart from each other, and the value that stays over the one that is looked
                    along has to be of the length one and not collinear with it. The value of the eye position is the one of the
                    fourth column of the matrix, which is the value the space is read from</para>
                    {Eye}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookAt({vec} eye, {vec} target, {vec} up) => LookTo(eye, target - eye, up);");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the right of it of the eye position <paramref name="eye"/>,
                    which looks along the value between the two points of it and <paramref name="target"/>
                    <para>Every value of the member is read where it is: the two points of the value of the eye position and the
                    one of the target have to be apart from each other, and the value that stays over the one that is looked
                    along has to be of the length one and not collinear with it. The view of the value is the one of the left of
                    it with the value that is looked along turned around, so it looks along the -z axis of the space of the view,
                    since the two of them look along the value with the axes of the space of the view apart</para>
                    {Eye}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookAt_RH({vec} eye, {vec} target, {vec} up) => LookTo_RH(eye, target - eye, up);");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the left of it whose rows are the axes of the view, of the eye
                    position <paramref name="eye"/>, which looks along the value between the two points of it and
                    <paramref name="target"/>
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose. Every value of the member is read where it is: the two points of the
                    value of the eye position and the one of the target have to be apart from each other, and the value that stays
                    over the one that is looked along has to be of the length one and not collinear with it</para>
                    {Eye}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookAt_Row({vec} eye, {vec} target, {vec} up) =>" +
                      " LookTo_Row(eye, target - eye, up);");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the right of it whose rows are the axes of the view, of the eye
                    position <paramref name="eye"/>, which looks along the value between the two points of it and
                    <paramref name="target"/>
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose, and the view of the value is the one of the left of it with the value
                    that is looked along turned around, so it looks along the -z axis of the space of the view. Every value of the
                    member is read where it is: the two points of the value of the eye position and the one of the target have to
                    be apart from each other, and the value that stays over
                    the one that is looked along has to be of the length one and not collinear with it</para>
                    {Eye}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookAt_RH_Row({vec} eye, {vec} target, {vec} up) =>" +
                      " LookTo_RH_Row(eye, target - eye, up);");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the left of it of the eye position <paramref name="eye"/>,
                    which looks along the value of <paramref name="dir"/>
                    <para>Every value of the member is read where it is: the value of the direction is the one the view looks
                    along, and the value that stays over the one that is looked along has to be of the length one and not
                    collinear with it. The value of the eye position is the one of the fourth column of the matrix, which is the
                    value the space is read from</para>
                    {Direction}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookTo({vec} eye, {vec} dir, {vec} up) => LookTo_RH(eye, -dir, up);");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the right of it of the eye position <paramref name="eye"/>,
                    which looks along the value of <paramref name="dir"/>
                    <para>Every value of the member is read where it is: the value of the direction is the one the view looks
                    along, and the value that stays over the one that is looked along has to be of the length one and not
                    collinear with it. The view of the value is the one of the left of it with the value that is looked along
                    turned around, so it looks along the -z axis of the space of the view</para>
                    {Direction}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookTo_RH({vec} eye, {vec} dir, {vec} up)");
        sb.AppendLine("    {");
        sb.AppendLine("        // the value that is looked along, the value of the x axis of the space of the view and the one of");
        sb.AppendLine("        // the y axis of it: the value of the x axis is at a right angle with the one that is looked along");
        sb.AppendLine("        // and the one that stays over it, and the value of the y axis is at a right angle with the two of");
        sb.AppendLine("        // them");
        sb.AppendLine("        var f = math.normalize(dir);");
        sb.AppendLine("        var s = math.normalize(math.cross(f, up));");
        sb.AppendLine("        var u = math.cross(s, f);");
        sb.AppendLine("        // the axes of the view are the columns of the matrix and the values of them are the rows of it, so the");
        sb.AppendLine("        // matrix of the rotation of the view is the transpose of the value that holds the axes of it");
        sb.AppendLine($"        var r = math.transpose(new {rotation}(s, u, -f));");
        sb.AppendLine("        return new(");
        sb.AppendLine("            r.c0.as4,");
        sb.AppendLine("            r.c1.as4,");
        sb.AppendLine("            r.c2.as4,");
        sb.AppendLine($"            new {vec4}(-math.dot(eye, s), -math.dot(eye, u), math.dot(eye, f), {one})");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the right of it whose rows are the axes of the view, of the eye
                    position <paramref name="eye"/>, which looks along the value of <paramref name="dir"/>
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose. Every value of the member is read where it is: the value of the
                    direction is the one the view looks along, and the value that stays over the one that is looked along has to
                    be of the length one and not collinear with it</para>
                    {Direction}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookTo_Row({vec} eye, {vec} dir, {vec} up) => LookTo_RH_Row(eye, -dir, up);");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the view of the space of the right of it whose rows are the axes of the view, of the eye
                    position <paramref name="eye"/>, which looks along the value of <paramref name="dir"/>
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose. Every value of the member is read where it is: the value of the
                    direction is the one the view looks along, and the value that stays over the one that is looked along has to
                    be of the length one and not collinear with it</para>
                    {Direction}
                    <returns>The matrix of the view</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookTo_RH_Row({vec} eye, {vec} dir, {vec} up)");
        sb.AppendLine("    {");
        sb.AppendLine("        // the value that is looked along, the value of the x axis of the space of the view and the one of");
        sb.AppendLine("        // the y axis of it, which the member reads on the rows of the matrix");
        sb.AppendLine("        var f = math.normalize(dir);");
        sb.AppendLine("        var s = math.normalize(math.cross(f, up));");
        sb.AppendLine("        var u = math.cross(s, f);");
        sb.AppendLine("        // the rows of the matrix are the axes of the space of the view, so the matrix reads a value of the");
        sb.AppendLine("        // space as the value of the view without a transpose");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(s, -math.dot(eye, s)),");
        sb.AppendLine($"            new {vec4}(u, -math.dot(eye, u)),");
        sb.AppendLine($"            new {vec4}(-f, math.dot(eye, f)),");
        sb.AppendLine($"            new {vec4}(default, default, default, {one})");
        sb.AppendLine("        );");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the members that reach the matrix of the projection of the space to the square matrix
    /// <paramref name="name"/>: the projection of the space reads the value of the volume of the view, which is the
    /// space between the two planes at the distance of the near value and the one of the far value of it from the eye
    /// of the view, as the value of the cube of the view, whose value on every axis of the space is between the
    /// negative one of the kind and the one of it and whose value on the ±z axis of it is the one of the kind at the
    /// near plane of the volume and the zero of it at the far plane of it, so the range of the ±z axis of the cube
    /// is reversed and the member of a projection reads the far plane of the volume as the zero of the kind. The
    /// reversed range holds the most of the precision of the kind beside the near plane of the volume, so the depth of
    /// the value of the ±z axis of the cube is cleared as the one of the kind and tested with the value that is
    /// greater than the other one.
    /// <para>The projection of the value of a field of view reads the angle of the x axis of the space of the view
    /// beside the aspect of it, and the projection of the value of a view volume reads the two planes of every axis of
    /// the space. The member that names the rows of the matrix reads the projection of the value as the matrix whose
    /// rows are the axes of it, which reads a value of the space as the value of the view without a transpose.</para>
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Projections(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec4 = VectorGenShared.VecName(typ, Size, false);
        var one = typ.one;
        var two = typ.two;
        var half = typ.half;
        // the doc of the range of the ±z axis of the cube of the view, which every member of a projection carries:
        // the range is the reversed one, which holds the most of the precision of the kind beside the near plane of the
        // volume, so the depth of a value of the ±z axis of the cube is cleared as the one of the kind and tested
        // with the value that is greater than the other one
        const string Depth = """
                             <remarks>
                             <para>The range of the ±z axis of the cube of the view is the <b>reversed</b> one, which is <b>[1, 0]</b>
                             beside the one [0, 1] of a space whose ±z axis is read the other way: the value of the near
                             plane of the volume is the one of the kind on that axis and the one of the far plane of it is the zero of
                             it.</para>
                             <para>The reversed range reads the value of the near plane of the volume as the one of the kind, which is the
                             value the kind of the component reads the most precisely, so the value of the ±z axis of the cube is
                             <b>more precise</b> than the one of a range that is not reversed. The depth of the value of the ±z axis of
                             the cube is <b>cleared as the one of the kind</b> and the test of it reads the <b>greater</b> of two values,
                             which is <c>Greater</c> or <c>GreaterEqual</c>.</para>
                             </remarks>
                             """;
        // a blank line separates two members, and the first one of them follows the member of another group
        var first = true;

        void Separate()
        {
            if (!first) sb.AppendLine();
            first = false;
        }

        // the range of the ±z axis of the cube of the view is the reversed one, which is [1, 0] beside the one
        // [0, 1] of a space whose ±z axis is read the other way: the value of the near plane of the
        // volume is the one of the kind on the ±z axis of the cube and the one of the far plane of it is the zero of
        // that axis, which holds the most of the precision of the kind beside the near plane of the volume, so the
        // depth of the value of the ±z axis of the cube is cleared as the one of the kind and tested with the value
        // that is greater than the other one
        sb.AppendLine("    // the range of the ±z axis of the cube of the view is the reversed one, which is [1, 0] beside the one");
        sb.AppendLine("    // [0, 1] of a space whose ±z axis is read the other way: the value of the near plane of");
        sb.AppendLine("    // the volume is the one of the kind on that axis and the one of the far plane of it is the zero of it, so");
        sb.AppendLine("    // the member of a projection of the volume of a single axis of the space reads the far plane of it as the");
        sb.AppendLine("    // zero of the kind. The reversed range holds the most of the precision of the kind beside the near plane of");
        sb.AppendLine("    // the volume, and the depth of a graphics api that reads the value of the cube is cleared as the one of");
        sb.AppendLine("    // the kind and tested with the greater of two values, which is Greater or GreaterEqual");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the left of it of the value of a view volume of
                    <paramref name="width"/> and <paramref name="height"/> centered on the +z axis of the space
                    <para>The volume of the value is the one between the two planes at the distance of <paramref name="near"/> and
                    the one of the value of <paramref name="far"/> from the eye of the view, which is the origin of the space, and
                    the value of the volume is read on the x axis of the space by the value of the width of it and on the
                    y axis of it by the one of the height of it</para>
                    <param name="width">The width of the view volume</param>
                    <param name="height">The height of the view volume</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Ortho({scalar} width, {scalar} height, {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var rcpdz = {one} / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}({two} / width, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, {two} / height, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, -rcpdz, default),");
        sb.AppendLine($"            new {vec4}(default, default, far * rcpdz, {one})");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the left of it of the value of a view volume whose x axis
                    of the space is between <paramref name="left"/> and <paramref name="right"/> and whose y axis of it
                    is between <paramref name="bottom"/> and <paramref name="top"/>
                    <para>The volume of the value is the one between the two planes at the distance of <paramref name="near"/> and
                    the one of the value of <paramref name="far"/> from the eye of the view, which is the origin of the space</para>
                    <param name="left">The value of the x axis of the space the view volume starts at</param>
                    <param name="right">The value of the x axis of the space the view volume ends at</param>
                    <param name="bottom">The value of the y axis of the space the view volume starts at</param>
                    <param name="top">The value of the y axis of the space the view volume ends at</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Ortho({scalar} left, {scalar} right, {scalar} bottom, {scalar} top," +
                      $" {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var rcp_width = {one} / (right - left);");
        sb.AppendLine($"        var rcp_height = {one} / (top - bottom);");
        sb.AppendLine($"        var rcpdz = {one} / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(rcp_width + rcp_width, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, rcp_height + rcp_height, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, -rcpdz, default),");
        sb.AppendLine($"            new {vec4}(-(left + right) * rcp_width, -(top + bottom) * rcp_height, far * rcpdz, {one})");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the right of it of the value of a view volume of
                    <paramref name="width"/> and <paramref name="height"/> centered on the -z axis of the space
                    <para>The projection of the value is the one of the left of it on the +z axis turned around, which is the -z
                    axis, and it reads the value of the volume between <paramref name="near"/> and <paramref name="far"/> in front of
                    the eye of the view as the one of the cube of the view</para>
                    <param name="width">The width of the view volume</param>
                    <param name="height">The height of the view volume</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Ortho_RH({scalar} width, {scalar} height, {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var rcpdz = {one} / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}({two} / width, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, {two} / height, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, rcpdz, default),");
        sb.AppendLine($"            new {vec4}(default, default, far * rcpdz, {one})");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the right of it of the value of a view volume whose
                    x axis of the space is between <paramref name="left"/> and <paramref name="right"/> and whose y
                    axis of it is between <paramref name="bottom"/> and <paramref name="top"/>
                    <para>The projection of the value is the one of the left of it on the +z axis turned around, which is the -z
                    axis, and it reads the value of the volume between <paramref name="near"/> and <paramref name="far"/> in front of
                    the eye of the view as the one of the cube of the view</para>
                    <param name="left">The value of the x axis of the space the view volume starts at</param>
                    <param name="right">The value of the x axis of the space the view volume ends at</param>
                    <param name="bottom">The value of the y axis of the space the view volume starts at</param>
                    <param name="top">The value of the y axis of the space the view volume ends at</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Ortho_RH({scalar} left, {scalar} right, {scalar} bottom, {scalar} top," +
                      $" {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var rcp_width = {one} / (right - left);");
        sb.AppendLine($"        var rcp_height = {one} / (top - bottom);");
        sb.AppendLine($"        var rcpdz = {one} / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(rcp_width + rcp_width, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, rcp_height + rcp_height, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, rcpdz, default),");
        sb.AppendLine($"            new {vec4}(-(left + right) * rcp_width, -(top + bottom) * rcp_height, far * rcpdz, {one})");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the left of it of the value of a field of view of
                    <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>
                    <para>The value of the field is the angle of the y axis of the space of the view and the aspect of the
                    view is the value of the x axis of it beside the one of the y axis of it. The value of the volume
                    between <paramref name="near"/> and <paramref name="far"/> in front of the eye of the view, which is the origin
                    of the space, is read as the one of the cube of the view, and the value of the fourth component of the
                    result of the member is the one of the +z axis of the space of the view</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov({scalar} vertical_fov, {scalar} aspect, {scalar} near," +
                      $" {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine($"        var r = far / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(w, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, h, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, {one} - r, {one}),");
        sb.AppendLine($"            new {vec4}(default, default, r * near, default)");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the right of it of the value of a field of view of
                    <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>
                    <para>The projection of the value is the one of the left of it on the +z axis turned around, which is the -z
                    axis, and it reads the value of the volume between <paramref name="near"/> and <paramref name="far"/> in front
                    of the eye of the view as the one of the cube of the view</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov_RH({scalar} vertical_fov, {scalar} aspect," +
                      $" {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine($"        var r = far / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(w, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, h, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, r - {one}, -{one}),");
        sb.AppendLine($"            new {vec4}(default, default, r * near, default)");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the left of it of the value of a field of view of
                    <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>, whose rows are the axes of the view
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose. The value of the volume between <paramref name="near"/> and
                    <paramref name="far"/> in front of the eye of the view is read as the one of the cube of the view, and the
                    value of the fourth component of the result of the member is the one of the +z axis of the space of the
                    view</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov_Row({scalar} vertical_fov, {scalar} aspect," +
                      $" {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine($"        var r = far / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            w,       default, default,           default,");
        sb.AppendLine($"            default, h,       default,           default,");
        sb.AppendLine($"            default, default, {one} - r,         {one},");
        sb.AppendLine($"            default, default, r * near,          default");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the right of it of the value of a field of view of
                    <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>, whose rows are the axes of the view
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose, and the projection of the value is the one of the left of it on the
                    +z axis turned around, which is the -z axis</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <param name="far">The distance to the far plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov_RH_Row({scalar} vertical_fov, {scalar} aspect," +
                      $" {scalar} near, {scalar} far)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine($"        var r = far / (far - near);");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            w,       default, default,   default,");
        sb.AppendLine($"            default, h,       default,   default,");
        sb.AppendLine($"            default, default, r - {one}, -{one},");
        sb.AppendLine($"            default, default, r * near,  default");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the left of it of the value of an infinite field of view
                    of <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>
                    <para>The value of the field is the angle of the y axis of the space of the view and the aspect of the
                    view is the value of the x axis of it beside the one of the y axis of it. The volume of the value is
                    the one in front of the eye of the view without a far plane, so the value of the +z axis of the space is
                    read where it is and the value of the fourth component of the result of the member is the one of the +z
                    axis of the space</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov({scalar} vertical_fov, {scalar} aspect, {scalar} near)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(w, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, h, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, default, {one}),");
        sb.AppendLine($"            new {vec4}(default, default, near, default)");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the right of it of the value of an infinite field of view
                    of <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>
                    <para>The projection of the value is the one of the left of it on the +z axis turned around, which is the -z
                    axis, whose volume is the one in front of the eye of the view without a far plane</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov_RH({scalar} vertical_fov, {scalar} aspect, {scalar} near)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            new {vec4}(w, default, default, default),");
        sb.AppendLine($"            new {vec4}(default, h, default, default),");
        sb.AppendLine($"            new {vec4}(default, default, default, -{one}),");
        sb.AppendLine($"            new {vec4}(default, default, near, default)");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the left of it of the value of an infinite field of view
                    of <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>, whose rows are the axes of the view
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose. The volume of the value is the one in front of the eye of the view
                    without a far plane, so the value of the +z axis of the space is read where it is</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov_Row({scalar} vertical_fov, {scalar} aspect, {scalar} near)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            w,       default, default, default,");
        sb.AppendLine($"            default, h,       default, default,");
        sb.AppendLine($"            default, default, default, {one},");
        sb.AppendLine($"            default, default, near,    default");
        sb.AppendLine("        );");
        sb.AppendLine("    }");

        Separate();
        AddDoc(sb, $"""
                    Returns the matrix of the projection of the space of the right of it of the value of an infinite field of view
                    of <paramref name="vertical_fov"/> and the aspect of <paramref name="aspect"/>, whose rows are the axes of the view
                    <para>The rows of the matrix are the axes of the space of the view, which reads a value of the space as the
                    value of the view without a transpose, and the projection of the value is the one of the left of it on the
                    +z axis turned around, which is the -z axis</para>
                    <param name="vertical_fov">The angle of the field of view of the y axis of the space, in radians</param>
                    <param name="aspect">The value of the x axis of the space of the view beside the one of the y axis of it</param>
                    <param name="near">The distance to the near plane of the view volume</param>
                    <returns>The matrix of the projection</returns>
                    """, "    ");
        AddDoc(sb, Depth, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} PerspectiveFov_RH_Row({scalar} vertical_fov, {scalar} aspect, {scalar} near)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var h = {one} / math.tan(vertical_fov * {half});");
        sb.AppendLine("        var w = h / aspect;");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            w,       default, default, default,");
        sb.AppendLine($"            default, h,       default, default,");
        sb.AppendLine($"            default, default, default, -{one},");
        sb.AppendLine($"            default, default, near,    default");
        sb.AppendLine("        );");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the members that reach the value of a rotation of the space and the one of a transform of it to the class
    /// of the math members of a floating point kind of this generator: the rotation of a value of 3 components by the
    /// value of a matrix is the sum of the three axes of the space of it scaled by the components of the value, which
    /// reads the fourth column of the matrix for no part, and the transform of it is the same sum beside the value of
    /// the origin of the space, which is the value of the fourth column of it. Every member is emitted into the two
    /// classes of the class of the math members, the one that names the value of the matrix and the one that reaches it
    /// as a member of the value.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Transforms(StringBuilder sb, Typ typ)
    {
        var name = MatrixGenerator.Name(typ, Size, Size, false);
        var vec = VectorGenShared.VecName(typ, Rotation, false);
        // a blank line separates two members, and the first one of them follows the declaration of the class of the
        // math members
        var first = true;

        void Separate()
        {
            if (!first) sb.AppendLine();
            first = false;
        }

        // emits the member of the class of the math members, which reaches the value of the matrix beside the one of
        // the value of 3 components
        void Member(string summary, string returns, string signature, string expression)
        {
            Separate();
            AddDoc(sb, $"""
                        {summary}
                        <param name="a">The value, a matrix of {Size} rows and {Size} columns</param>
                        <param name="b">The value of {Rotation} components the member reads</param>
                        <returns>{returns}</returns>
                        """, "    ");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {vec} {signature} =>");
            sb.AppendLine($"        {expression};");
        }

        sb.AppendLine("public static partial class math");
        sb.AppendLine("{");
        Member("""
               Returns the value of <paramref name="b"/> turned by the three axes of the space of <paramref name="a"/>
               <para>The value of the member is the sum of the three axes of the space of the matrix scaled by the components
               of the value of 3 components, so the value of the fourth column of the matrix is not part of it and the length
               of the value is the one it was handed where the three axes of the space are of the one of the kind</para>
               """, "The value turned by the three axes of the space of the matrix",
            $"rotate({name} a, {vec} b)",
            "math.fma(b.as4.xxxx, a.c0, math.fma(b.as4.yyyy, a.c1, b.as4.zzzz * a.c2)).xyz");
        Member("""
               Returns the value of <paramref name="b"/> read as the value of the space of <paramref name="a"/>, which is the
               value of the origin of the space of it beside the value that is turned by the three axes of it
               <para>The value of the member is the sum of the three axes of the space of the matrix scaled by the components
               of the value of 3 components beside the value of the fourth column of it, which is the value of the origin of
               the space the matrix reads</para>
               """, "The value of the space of the matrix",
            $"transform({name} a, {vec} b)",
            "math.fma(b.as4.xxxx, a.c0, math.fma(b.as4.yyyy, a.c1, math.fma(b.as4.zzzz, a.c2, a.c3))).xyz");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("public static partial class math_ex");
        sb.AppendLine("{");
        first = true;
        Separate();
        sb.AppendLine($"    /// <inheritdoc cref=\"math.rotate({name}, {vec})\"/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {vec} rotate(this {name} a, {vec} b) => math.rotate(a, b);");
        Separate();
        sb.AppendLine($"    /// <inheritdoc cref=\"math.transform({name}, {vec})\"/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {vec} transform(this {name} a, {vec} b) => math.transform(a, b);");
        sb.AppendLine("}");
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
