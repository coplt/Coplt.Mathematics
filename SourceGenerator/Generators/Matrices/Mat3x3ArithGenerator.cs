using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the arithmetic members of the square matrix of 3 rows and 3 columns of every kind a number names:
/// <c>short</c>, <c>ushort</c>, <c>int</c>, <c>uint</c>, <c>long</c>, <c>ulong</c>, <c>half</c>, <c>float</c> and
/// <c>double</c>.
/// <para>The arithmetic of a shape of a square matrix is the one of that shape alone: the value of a shape
/// reaches no member of the arithmetic of another shape, so this generator shares no part of itself with the one
/// of another shape and it holds every part of the arithmetic of the shape it reaches.</para>
/// <para>The members of the shape are the value of the upper left of a matrix of 4 rows and 4 columns of it, the
/// scale of a single component, of three of them and of a value of 3 components, the rotation of the space around
/// the axis of a value of 3 components by an angle, the rotation that a forward and an up value of 3 components
/// reach, the rotation of the three Euler angles of a value of 3 components and the rotation of a single axis of
/// the space by an angle. The value of the upper left and the scale are the members of every kind of a number and
/// the rotations are the members of a floating point kind alone: every one of them is the member of the type of the
/// value, so this file reaches them, and the inverse and the determinant of the value are the members of the class
/// of the math members, which are written by hand.</para>
/// </summary>
[Generator]
public class Mat3x3ArithGenerator : IIncrementalGenerator
{
    /// <summary>The number of the rows and the number of the columns of the square matrix of this generator.</summary>
    private const int Size = 3;

    /// <summary>
    /// The kinds the arithmetic of a square matrix reaches, which are every kind a number names: the value of the
    /// upper left of a matrix of 4 rows and 4 columns and the scale of a single component, of three of them and of a
    /// value of 3 components are the members of every one of them, and the rotations are the members of a floating
    /// point kind alone.
    /// </summary>
    private static readonly Typ[] Kinds = Typ.Typs.Where(static typ => typ.arith).ToArray();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => AddSources(ctx));
    }

    /// <summary>
    /// Adds the file of the members of the arithmetic of the square matrix of 3 rows and 3 columns of every kind
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
        // a matrix of 3 rows and 3 columns is the upper left of a matrix of 4 rows and 4 columns, so the value of
        // one of the two reaches the value of the other one, and a rotation of a value is also the one of the
        // quaternion that holds it, which the file of the quaternion reaches
        sb.AppendLine("    // a matrix of 3 rows and 3 columns is the upper left of a matrix of 4 rows and 4 columns, and the");
        sb.AppendLine("    // rotation of a value is also the one of the quaternion that holds it, which the ctor below reaches");
        sb.AppendLine();
        From4x4(sb, name, typ);
        sb.AppendLine();
        Scales(sb, name, typ);
        // the rotation of the space is the member of a floating point kind alone, since the angle of a rotation is
        // the one of a floating point component
        if (typ.f)
        {
            sb.AppendLine();
            Quaternion(sb, name, typ);
            sb.AppendLine();
            LookRotation(sb, name, typ);
            sb.AppendLine();
            LookRotationSafe(sb, name, typ);
            sb.AppendLine();
            AxisAngle(sb, name, typ);
            sb.AppendLine();
            EulerAngles(sb, name, typ);
            sb.AppendLine();
            EulerOrders(sb, name, typ);
            sb.AppendLine();
            AxisRotations(sb, name, typ);
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
    /// Adds the members that reach the matrix of the rotation of the three Euler angles to the square matrix
    /// <paramref name="name"/>, which are the member of the angles alone, which reaches the order of the z-x-y
    /// angles, and the member that names the order of the angles it reaches.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void EulerAngles(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Size, false);
        AddDoc(sb, """
                   Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the one
                   of the z-x-y order
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of
                   it is looked along towards the origin</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation in the z-x-y order</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Euler({vec} xyz) => EulerZXY(xyz);");
        sb.AppendLine();
        AddDoc(sb, """
                   Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/> in the order
                   <paramref name="order"/> names
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of
                   it is looked along towards the origin</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
        sb.AppendLine("    /// <param name=\"order\">The order the three rotations of the angles are applied in</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation in the order of <paramref name=\"order\"/></returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Euler({vec} xyz, RotationOrder order) => order switch");
        sb.AppendLine("    {");
        foreach (var order in Orders)
            sb.AppendLine($"        RotationOrder.{order.Name} => Euler{order.Name}(xyz),");
        // the order of the z-x-y angles is reached by the member of the angles alone, so the switch reaches it
        // beside the ones the members of the orders reach, and every other value of the order is the identity
        sb.AppendLine("        RotationOrder.ZXY => EulerZXY(xyz),");
        sb.AppendLine("        _ => Identity,");
        sb.AppendLine("    };");
    }

    /// <summary>
    /// The five orders of the three Euler angles whose matrix is reached by a member of its own, which are every
    /// order of the three axes beside the one of the z-x-y angles that the member of the angles alone reaches: the
    /// axes of an order are the axes of the three rotations in the order they are applied in, <c>Input</c> is the
    /// value of the angles the member of the z-x-y order is handed, which is negated where the order of the axes is
    /// an odd permutation of them, and every column of the result is a column of the matrix of the z-x-y order with
    /// the components of it named by the same permutation.
    /// </summary>
    private static readonly (string Name, string Axes, string Input, bool Negate, string C0, string C1, string C2)[]
        Orders =
        {
            ("XYZ", "xyz", "yzx", false, "2.zxy", "0.zxy", "1.zxy"),
            ("XZY", "xzy", "zyx", true, "2.zyx", "1.zyx", "0.zyx"),
            ("YXZ", "yxz", "xzy", true, "0.xzy", "2.xzy", "1.xzy"),
            ("YZX", "yzx", "zxy", false, "1.yzx", "2.yzx", "0.yzx"),
            ("ZYX", "zyx", "yxz", true, "1.yxz", "0.yxz", "2.yxz"),
        };

    /// <summary>
    /// Adds the members that reach the matrix of the rotation of the three Euler angles in one of the orders of
    /// them to the square matrix <paramref name="name"/>: the six orders are the six permutations of the three axes
    /// of the space, and the matrix of a permutation of the axes is the one of the order of the z-x-y angles with
    /// every axis named by the permutation of it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void EulerOrders(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Size, false);
        sb.AppendLine("    // the six orders of the angles are the six permutations of the three axes of the space, and the matrix of");
        sb.AppendLine("    // a permutation of the axes is the one of the order of the z-x-y angles with every axis named by the");
        sb.AppendLine("    // permutation of it: a member that follows reaches the member of the z-x-y order with the angles the");
        sb.AppendLine("    // permutation of the axes names and names the rows and the columns of the result with it again");
        foreach (var order in Orders)
        {
            var axes = order.Axes;
            sb.AppendLine();
            AddDoc(sb, $"""
                        Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the rotation
                        around the {axes[0]} axis by {Word(axes[0])} of them, then the rotation around the {axes[1]} axis by {Word(axes[1])} one
                        and finally the rotation around the {axes[2]} axis by {Word(axes[2])} one
                        <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                        is looked along towards the origin</para>
                        """, "    ");
            sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
            sb.AppendLine($"    /// <returns>The matrix of the rotation in the {Spelling(order.Name)} order</returns>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {name} Euler{order.Name}({vec} xyz)");
            sb.AppendLine("    {");
            sb.AppendLine($"        var m = EulerZXY({(order.Negate ? "-" : "")}xyz.{order.Input});");
            sb.AppendLine($"        return new(m.c{order.C0}, m.c{order.C1}, m.c{order.C2});");
            sb.AppendLine("    }");
        }

        sb.AppendLine();
        EulerZxy(sb, name, typ);
    }

    /// <summary>
    /// Adds the member that reaches the matrix of the rotation of the three Euler angles in the order of the z-x-y
    /// angles to the square matrix <paramref name="name"/>, which is the order that every other order of the angles
    /// reaches: a member of another order is the member of this one with the angles the permutation of the axes of
    /// it names and with the rows and the columns of the result named by the same permutation, so this member is
    /// the one that holds the computation of the matrix. The value of a kind that has a register holds the pack of
    /// the angles of it, which computes the matrix with one sine and one cosine of every axis and a few shuffles of
    /// them, and a value that has no register computes every component of it on its own, so the packed form reaches
    /// nothing for it and the member of the kind holds the plain formula of the matrix.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void EulerZxy(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Size, false);
        var one = typ.one;
        AddDoc(sb, """
                   Returns the matrix of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the rotation
                   around the z axis by the third of them, then the rotation around the x axis by the first one and finally the
                   rotation around the y axis by the second one
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                   is looked along towards the origin</para>
                   """, "    ");
        sb.AppendLine("    /// <param name=\"xyz\">The angles of the x axis, the y axis and the z axis, in radians</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation in the z-x-y order</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} EulerZXY({vec} xyz)");
        sb.AppendLine("    {");
        if (VectorGenShared.Simd(typ, Size, false))
        {
            sb.AppendLine("        var (s, c) = math.sincos(xyz);");
            sb.AppendLine();
            sb.AppendLine("        // the plain formula of the matrix of the order of the z-x-y angles, which the pack of the sine and");
            sb.AppendLine("        // the cosine of the three angles computes with fewer operations:");
            sb.AppendLine("        // return new(");
            sb.AppendLine("        //     c.y * c.z + s.x * s.y * s.z,    c.z * s.x * s.y - c.y * s.z,    c.x * s.y,");
            sb.AppendLine("        //     c.x * s.z,                      c.x * c.z,                      -s.x,");
            sb.AppendLine("        //     c.y * s.x * s.z - c.z * s.y,    c.y * c.z * s.x + s.y * s.z,    c.x * c.y");
            sb.AppendLine("        // );");
            sb.AppendLine();
            sb.AppendLine("        // the sine and the cosine of the three angles, the pair of the two first of them one after the other");
            sb.AppendLine("        var s_xy_c_xy = math.shuffle_xy_xy(s.as4, c.as4);");
            sb.AppendLine("        var s_zw_c_zw = math.shuffle_zw_zw(s.as4, c.as4);");
            sb.AppendLine();
            sb.AppendLine("        // (s.x * s.y, s.x * c.y, c.x * s.y, c.x * c.y)");
            sb.AppendLine("        var P = s_xy_c_xy.xxzz * s_xy_c_xy.ywyw;");
            sb.AppendLine();
            sb.AppendLine("        var z1 = s_zw_c_zw.xzxz; // (s.z, c.z, s.z, c.z)");
            sb.AppendLine("        var z2 = s_zw_c_zw.zxzx; // (c.z, s.z, c.z, s.z)");
            sb.AppendLine("        var Y = s_xy_c_xy.wwyy; // (c.y, c.y, s.y, s.y)");
            sb.AppendLine();
            sb.AppendLine("        // (c.y * c.z, -c.y * s.z, -s.y * c.z, s.y * s.z)");
            sb.AppendLine($"        var term2 = math.chg_sign(Y * z2, new({one}, -{one}, -{one}, {one}));");
            sb.AppendLine();
            sb.AppendLine("        var r4 = math.fma(P.xxyy, z1, term2);");
            sb.AppendLine("        var cx_z = s_xy_c_xy.z * z1;");
            sb.AppendLine();
            sb.AppendLine("        var c01_xy = math.shuffle_xy_xy(r4, cx_z);");
            sb.AppendLine("        var c0 = math.shuffle_xy_xy(c01_xy.xzxz, r4.zzzz).xyz;");
            sb.AppendLine("        var c1 = math.shuffle_xy_xy(c01_xy.ywyw, r4.wwww).xyz;");
            sb.AppendLine("        var c2 = math.shuffle_xy_xy(P.zwzw, -s.as4).xzy;");
            sb.AppendLine("        return new(c0, c1, c2);");
        }
        else
        {
            // a value that has no register computes every component of it on its own, so the member of the kind
            // holds the plain formula of the matrix of the angles
            sb.AppendLine("        var (s, c) = math.sincos(xyz);");
            sb.AppendLine();
            sb.AppendLine("        return new(");
            sb.AppendLine("            c.y * c.z + s.x * s.y * s.z,    c.z * s.x * s.y - c.y * s.z,    c.x * s.y,");
            sb.AppendLine("            c.x * s.z,                      c.x * c.z,                      -s.x,");
            sb.AppendLine("            c.y * s.x * s.z - c.z * s.y,    c.y * c.z * s.x + s.y * s.z,    c.x * c.y");
            sb.AppendLine("        );");
        }

        sb.AppendLine("    }");
    }

    /// <summary>
    /// The three rotations of a single axis, which are the axes of the space in the order of them: the axis of a
    /// rotation keeps the value it is handed, the axis that follows the one of the rotation reaches the one that
    /// follows the two of them where the angle of the rotation is a right angle, and the three columns of the matrix
    /// are three swizzles of the pair the sine and the cosine of the angle are packed into, which hold the components
    /// of the column and the zero of the kind beside them, so the view of 3 components of a column reaches it
    /// without a mask.
    /// </summary>
    private static readonly (string Axis, string Next, string Next2, string C0, string C1, string C2)[] Rotations =
    {
        ("x", "y", "z", "t.ywww.as3", "t.wxzw.as3", "u.wzxw.as3"),
        ("y", "z", "x", "u.xwzw.as3", "t.wyww.as3", "t.zwxw.as3"),
        ("z", "x", "y", "t.xzww.as3", "u.zxww.as3", "t.wwyw.as3"),
    };

    /// <summary>
    /// Adds the members that reach the matrix of the rotation of a single axis to the square matrix
    /// <paramref name="name"/>: the value of a kind that has a register holds the pack of the sine and the cosine of
    /// the angle, which is read beside the zero of the kind so that the second component of the sine of the value is
    /// the zero of the kind and the second component of the cosine of it is the one of it, which are the two
    /// constants the matrix of a rotation of an axis holds beside the sine and the cosine of the angle, and every
    /// column of the matrix is a swizzle of one of the two values that the view of 3 components of it reaches
    /// without a mask. A value that has no register computes every component of it on its own, so the member of the
    /// kind holds the plain formula of the matrix.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void AxisRotations(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var one = typ.one;
        var simd = VectorGenShared.Simd(typ, Size, false);
        var first = true;
        foreach (var (axis, next, next2, c0, c1, c2) in Rotations)
        {
            if (!first) sb.AppendLine();
            first = false;
            AddDoc(sb, $"""
                        Returns the matrix that rotates the value around the {axis} axis by <paramref name="angle"/>, which is
                        the rotation of the value around the origin of the space
                        <para>Every angle of the rotation is in radians and the rotation of an angle is clockwise where the axis of
                        it is looked along towards the origin, so the {axis} axis keeps the value it is handed, the {next} axis
                        reaches the {next2} axis where the angle of the rotation is a right angle and the length of the value is
                        the one it was handed</para>
                        """, "    ");
            sb.AppendLine("    /// <param name=\"angle\">The angle of the rotation, in radians</param>");
            sb.AppendLine($"    /// <returns>The matrix of the rotation around the {axis} axis</returns>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {name} Rotate{axis.ToUpperInvariant()}({scalar} angle)");
            sb.AppendLine("    {");
            if (simd)
            {
                sb.AppendLine("        // the angle is read beside the zero of the kind, so the sine of the value is the sine of the angle of");
                sb.AppendLine("        // the rotation beside the zero of the kind and the cosine of it is the cosine of the angle of it beside");
                sb.AppendLine("        // the one of it, which are the two constants the matrix of a rotation of an axis holds");
                sb.AppendLine($"        var (s, c) = math.sincos({vec4}.Scalar(angle));");
                sb.AppendLine("        // the pair of the cosine, the one, the sine and the zero of the kind, which the shuffle reads out of the");
                sb.AppendLine("        // two values, and the same pair with the sign of the sine of it flipped");
                sb.AppendLine("        var t = math.shuffle_xy_xy(c, s); // (c, 1, s, 0)");
                sb.AppendLine($"        var u = math.chg_sign(t, new({one}, {one}, -{one}, {one})); // (c, 1, -s, 0)");
                sb.AppendLine("        // a column of the matrix is a swizzle of one of the two pairs, which holds the components of the");
                sb.AppendLine("        // column and the zero of the kind in the fourth one, so the view of 3 components of it is the column");
                sb.AppendLine("        return new(");
                sb.AppendLine($"            {c0},");
                sb.AppendLine($"            {c1},");
                sb.AppendLine($"            {c2}");
                sb.AppendLine("        );");
            }
            else
            {
                // a value that has no register computes every component of it on its own, so the member of the kind
                // holds the plain formula of the matrix: the sine and the cosine of the angle are read in one call
                sb.AppendLine("        math.sincos(angle, out var s, out var c);");
                sb.AppendLine("        return new(");
                PlainRotation(sb, axis, one);
                sb.AppendLine("        );");
            }

            sb.AppendLine("    }");
        }
    }

    /// <summary>
    /// Adds the arguments of the plain matrix of a rotation around <paramref name="axis"/> to the source: the axis
    /// of the rotation keeps the value it is handed, the two other axes turn by the angle of the rotation, which
    /// holds the cosine of it on the diagonal of the matrix and the sine of it beside that one, where the axis that
    /// follows the one of the rotation holds the negative of the sine of it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="axis">The axis of the rotation</param>
    /// <param name="one">The literal of the one of the kind</param>
    private static void PlainRotation(StringBuilder sb, string axis, string one)
    {
        var index = "xyz".IndexOf(axis);
        var value = new string[3, 3];
        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 3; column++) value[row, column] = "default";
        }

        value[index, index] = one;
        value[(index + 1) % 3, (index + 1) % 3] = "c";
        value[(index + 1) % 3, (index + 2) % 3] = "-s";
        value[(index + 2) % 3, (index + 1) % 3] = "s";
        value[(index + 2) % 3, (index + 2) % 3] = "c";
        // the values of a column of the matrix are packed to the widest of them, so the rows read as a table
        var width = new int[3];
        for (var column = 0; column < 3; column++)
        {
            for (var row = 0; row < 3; row++) width[column] = Math.Max(width[column], value[row, column].Length);
        }

        for (var row = 0; row < 3; row++)
        {
            sb.AppendLine($"            {value[row, 0].PadRight(width[0])}, {value[row, 1].PadRight(width[1])}, " +
                          $"{value[row, 2]}{(row == 2 ? "" : ",")}");
        }
    }

    /// <summary>
    /// Adds the members that reach the value of the upper left of a matrix of 4 rows and 4 columns to the square
    /// matrix <paramref name="name"/>, which are the member the value of a matrix of 4 rows and 4 columns reaches
    /// and the member that hands a value of that shape over: a matrix of 3 rows and 3 columns is the value of the
    /// upper left of a matrix of 4 rows and 4 columns, so the value of the one of the two reaches the value of the
    /// other one, and the conversion of the value of a matrix of 4 rows and 4 columns is the one that the type of
    /// the value carries beside the member of the upper left of it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void From4x4(StringBuilder sb, string name, Typ typ)
    {
        var m4x4 = MatrixGenerator.Name(typ, 4, 4, false);
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the value of the upper left of the matrix of 4 rows and 4 columns of <paramref name=\"m4x4\"/>,");
        sb.AppendLine("    /// which is the value of the 3 rows and the 3 columns of it");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"m4x4\">The matrix of 4 rows and 4 columns the upper left of which is the value</param>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {name}({m4x4} m4x4)");
        sb.AppendLine("    {");
        sb.AppendLine("        c0 = m4x4.c0.xyz;");
        sb.AppendLine("        c1 = m4x4.c1.xyz;");
        sb.AppendLine("        c2 = m4x4.c2.xyz;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns the value of the upper left of the matrix of 4 rows and 4 columns of");
        sb.AppendLine("    /// <paramref name=\"m4x4\"/></summary>");
        sb.AppendLine("    /// <param name=\"m4x4\">The matrix of 4 rows and 4 columns the upper left of which is the value</param>");
        sb.AppendLine("    /// <returns>The value of the upper left of <paramref name=\"m4x4\"/></returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static explicit operator {name}({m4x4} m4x4) => new(m4x4);");
    }

    /// <summary>
    /// Adds the member that reaches the matrix of the rotation of a quaternion to the square matrix
    /// <paramref name="name"/>: the value of a quaternion is the rotation of the space of 3 rows and 3 columns of
    /// it, so the member reads the four components of the value of the quaternion where the value of it is the one
    /// of that rotation.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Quaternion(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Size, false);
        var quaternion = QuaternionGenerator.Name(typ);
        var one = typ.one;
        var negOne = $"-{one}";
        AddDoc(sb, """
                   Returns the matrix of the rotation of the value of <paramref name="q"/>
                   <para>The value of a quaternion is the rotation of the space of 3 rows and 3 columns of it, so the member
                   reads the four components of the value where the value of the quaternion is the one of that rotation</para>
                   <param name="q">The quaternion of the rotation</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {name}({quaternion} q)");
        sb.AppendLine("    {");
        sb.AppendLine("        var v = q.value;");
        sb.AppendLine("        var v2 = v + v;");
        sb.AppendLine("        // a component of the value is turned around where the component of the value of the sign is the");
        sb.AppendLine("        // negative one, which is the exclusive or of the sign of the component that the column reaches");
        sb.AppendLine($"        var npn = new {vec}({negOne}, default, {negOne});");
        sb.AppendLine($"        var nnp = new {vec}({negOne}, {negOne}, default);");
        sb.AppendLine($"        var pnn = new {vec}(default, {negOne}, {negOne});");
        sb.AppendLine($"        c0 = math.fms(v2.yyy, math.chg_sign(v.yxw, npn), v2.zzz * math.chg_sign(v.zwx, pnn)) +");
        sb.AppendLine($"            new {vec}({one}, default, default);");
        sb.AppendLine($"        c1 = math.fms(v2.zzz, math.chg_sign(v.wzy, nnp), v2.xxx * math.chg_sign(v.yxw, npn)) +");
        sb.AppendLine($"            new {vec}(default, {one}, default);");
        sb.AppendLine($"        c2 = math.fms(v2.xxx, math.chg_sign(v.zwx, pnn), v2.yyy * math.chg_sign(v.wzy, nnp)) +");
        sb.AppendLine($"            new {vec}(default, default, {one});");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the members that reach the matrix that scales the space to the square matrix <paramref name="name"/>,
    /// which are the matrices whose diagonal holds the values the axes of the space are scaled by.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void Scales(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec = VectorGenShared.VecName(typ, Size, false);

        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that scales every axis of the space by <paramref name=\"s\"/>");
        sb.AppendLine("    /// <para>The matrix holds the value on the diagonal of it and the zero of the kind of it everywhere");
        sb.AppendLine("    /// else, so every axis of the space is scaled on its own</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"s\">The value every axis of the space is scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({scalar} s) => new(");
        sb.AppendLine("        s,       default, default,");
        sb.AppendLine("        default, s,       default,");
        sb.AppendLine("        default, default, s");
        sb.AppendLine("    );");
        sb.AppendLine();

        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that scales the x axis of the space by <paramref name=\"x\"/>, the y axis of");
        sb.AppendLine("    /// it by <paramref name=\"y\"/> and the z axis of it by <paramref name=\"z\"/>");
        sb.AppendLine("    /// <para>The matrix holds the three values on the diagonal of it and the zero of the kind of it everywhere");
        sb.AppendLine("    /// else, so every axis of the space is scaled on its own</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"x\">The value the x axis of the space is scaled by</param>");
        sb.AppendLine("    /// <param name=\"y\">The value the y axis of the space is scaled by</param>");
        sb.AppendLine("    /// <param name=\"z\">The value the z axis of the space is scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Scale({scalar} x, {scalar} y, {scalar} z) => new(");
        sb.AppendLine("        x,       default, default,");
        sb.AppendLine("        default, y,       default,");
        sb.AppendLine("        default, default, z");
        sb.AppendLine("    );");
        sb.AppendLine();

        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that scales the x axis of the space by the first component of");
        sb.AppendLine("    /// <paramref name=\"v\"/>, the y axis of it by the second component of it and the z axis of it by");
        sb.AppendLine("    /// the third component of it");
        sb.AppendLine("    /// <para>The matrix holds the components of the value on the diagonal of it and the zero of the kind of it");
        sb.AppendLine("    /// everywhere else, so every axis of the space is scaled on its own</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"v\">The value whose components the axes of the space are scaled by</param>");
        sb.AppendLine("    /// <returns>The matrix of the scale</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        // the padding lane of the value of a vector that keeps it in a register is zero, so a column of the
        // matrix is the swizzle of the value that reads the component the column scales beside the padding lane:
        // the swizzle is of 4 components and the column of 3, and the padding lane of the swizzle stays zero,
        // so the column reads the swizzle as the value of 3 components without masking anything
        if (VectorGenShared.Simd(typ, Size, false))
        {
            sb.AppendLine($"    public static {name} Scale({vec} v) => new(");
            sb.AppendLine("        v.as4.xwww.as3,");
            sb.AppendLine("        v.as4.wyww.as3,");
            sb.AppendLine("        v.as4.wwzw.as3");
            sb.AppendLine("    );");
        }
        else
        {
            sb.AppendLine($"    public static {name} Scale({vec} v) => new(");
            sb.AppendLine("        v.x,     default, default,");
            sb.AppendLine("        default, v.y,     default,");
            sb.AppendLine("        default, default, v.z");
            sb.AppendLine("    );");
        }
    }

    /// <summary>
    /// Adds the member that reaches the matrix of the rotation of the space around the axis of a value by an angle
    /// to the square matrix <paramref name="name"/>, which is the product of the axis with the one of the kind beside
    /// the sine and the cosine of the angle: every column of the matrix reaches the component of the axis that the
    /// column names, which is the one the axis holds at the two other components of it with the sign of the sine of
    /// the angle beside it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void AxisAngle(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec = VectorGenShared.VecName(typ, Size, false);
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var one = typ.one;
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix that rotates the value around the axis of <paramref name=\"axis\"/> by");
        sb.AppendLine("    /// <paramref name=\"angle\"/>, which is the rotation of the value around the origin of the space");
        sb.AppendLine("    /// <para>Every angle of the rotation is in radians and the rotation of an angle is clockwise where the axis of");
        sb.AppendLine("    /// it is looked along towards the origin, so the length of the value is the one it was handed where the axis");
        sb.AppendLine("    /// of the rotation is of the length one and not the zero of the kind</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"axis\">The axis of the rotation, which is of the length one</param>");
        sb.AppendLine("    /// <param name=\"angle\">The angle of the rotation, in radians</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation around the axis</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} AxisAngle({vec} axis, {scalar} angle)");
        sb.AppendLine("    {");
        sb.AppendLine("        math.sincos(angle, out var sina, out var cosa);");
        sb.AppendLine("        // the axis scaled by the one of the kind beside the cosine of the angle, which every column of the");
        sb.AppendLine("        // matrix is scaled by beside the sine and the cosine of it");
        sb.AppendLine("        var u_inv_cosa = math.fsm(axis, axis, cosa); // axis - axis * cosa");
        sb.AppendLine($"        var t = new {vec4}(axis * sina, cosa);");
        sb.AppendLine("        // the sign of the component of the axis that follows the one of the column is the negative one and the");
        sb.AppendLine("        // one of the component of the axis of the column itself is the positive one");
        sb.AppendLine("        return new(");
        sb.AppendLine($"            math.fma(axis.xxx, u_inv_cosa, math.chg_sign(t.wzy, new {vec}({one}, {one}, -{one}))),");
        sb.AppendLine($"            math.fma(axis.yyy, u_inv_cosa, math.chg_sign(t.zwx, new {vec}(-{one}, {one}, {one}))),");
        sb.AppendLine($"            math.fma(axis.zzz, u_inv_cosa, math.chg_sign(t.yxw, new {vec}({one}, -{one}, {one})))");
        sb.AppendLine("        );");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the member that reaches the matrix of the rotation of the space that looks along a value while another
    /// one stays over it to the square matrix <paramref name="name"/>, which is the rotation of the two values of
    /// it as they are: the member reads the two of them where they are of the length one and are not collinear, so
    /// the value that is of another length or the one that is collinear with the other one reaches the member of the
    /// safe rotation instead.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void LookRotation(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Size, false);
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix of the rotation of the space that looks along <paramref name=\"forward\"/> while the");
        sb.AppendLine("    /// value of <paramref name=\"up\"/> stays over it");
        sb.AppendLine("    /// <para>Every one of the two values of the member has to be of the length one and the two of them must not");
        sb.AppendLine("    /// be collinear, so a value that is of another length or that is collinear with the other one reaches the");
        sb.AppendLine("    /// member of the safe rotation, which takes the two of them to the length one first</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"forward\">The value the rotation looks along, which is of the length one</param>");
        sb.AppendLine("    /// <param name=\"up\">The value that stays over the one that is looked along, which is of the length one</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookRotation({vec} forward, {vec} up)");
        sb.AppendLine("    {");
        sb.AppendLine("        var t = math.normalize(math.cross(up, forward));");
        sb.AppendLine("        return new(t, math.cross(forward, t), forward);");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the member that reaches the matrix of the rotation of the space that looks along a value while another
    /// one stays over it, which takes the two values of it to the length one first, to the square matrix
    /// <paramref name="name"/>: the two values are read where the length of them and the one of the axis that is at
    /// a right angle with the two of them are the ones the kind of the component reads, so the identity of the
    /// matrix is reached where the two of them are collinear or where one of them is so short or so long that the
    /// member cannot read it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the square matrix</param>
    /// <param name="typ">The kind of the component of the square matrix</param>
    private static void LookRotationSafe(StringBuilder sb, string name, Typ typ)
    {
        var vec = VectorGenShared.VecName(typ, Size, false);
        var scalar = typ.compType;
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the matrix of the rotation of the space that looks along <paramref name=\"forward\"/> while the");
        sb.AppendLine("    /// value of <paramref name=\"up\"/> stays over it, where the two values of it are taken to the length one");
        sb.AppendLine("    /// first");
        sb.AppendLine("    /// <para>Every one of the two values is accepted whatever its length is where the length of the two of them");
        sb.AppendLine("    /// and the one of the axis that is at a right angle with them are the ones the kind of the component reads,");
        sb.AppendLine("    /// and the identity of the matrix is reached where the two of them are collinear or where one of them is so");
        sb.AppendLine("    /// short or so long that the member cannot read it</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"forward\">The value the rotation looks along</param>");
        sb.AppendLine("    /// <param name=\"up\">The value that stays over the one that is looked along</param>");
        sb.AppendLine("    /// <returns>The matrix of the rotation, or the identity of the matrix where the two values of it cannot be");
        sb.AppendLine("    /// read</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookRotationSafe({vec} forward, {vec} up)");
        sb.AppendLine("    {");
        sb.AppendLine("        var forwardLengthSq = math.dot(forward, forward);");
        sb.AppendLine("        var upLengthSq = math.dot(up, up);");
        sb.AppendLine();
        sb.AppendLine("        forward *= math.rsqrt(forwardLengthSq);");
        sb.AppendLine("        up *= math.rsqrt(upLengthSq);");
        sb.AppendLine();
        sb.AppendLine("        var t = math.cross(up, forward);");
        sb.AppendLine("        var tLengthSq = math.dot(t, t);");
        sb.AppendLine("        t *= math.rsqrt(tLengthSq);");
        sb.AppendLine();
        sb.AppendLine("        // the length of the two values, the one of the axis that is at a right angle with them and the one of");
        sb.AppendLine("        // the axis of the rotation, which the two values that are collinear do not reach");
        sb.AppendLine($"        if (math.min(math.min(forwardLengthSq, upLengthSq), tLengthSq) <= math.MinRotateSafe<{scalar}>() ||");
        sb.AppendLine($"            math.max(math.max(forwardLengthSq, upLengthSq), tLengthSq) >= math.MaxRotateSafe<{scalar}>() ||");
        sb.AppendLine($"            tLengthSq <= math.MinRotateCollinearSq<{scalar}>() ||");
        sb.AppendLine("            !math.is_finite(forwardLengthSq) || !math.is_finite(upLengthSq) || !math.is_finite(tLengthSq))");
        sb.AppendLine("            return Identity;");
        sb.AppendLine("        return new(t, math.cross(forward, t), forward);");
        sb.AppendLine("    }");
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
