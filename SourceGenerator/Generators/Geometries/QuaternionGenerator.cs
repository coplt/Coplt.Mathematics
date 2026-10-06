using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the members of every quaternion of <see cref="Typ"/>, which are the ones of a floating point kind
/// alone: <c>half</c>, <c>float</c> and <c>double</c>.
/// <para>A quaternion is the value of the four components of the kind of its component, which holds the rotation
/// of the space of 3 rows and 3 columns of it: the members of the type are the value of it, the rotation of an
/// angle around the axis of a value of 3 components, the ones of the three Euler angles, the ones around a single
/// axis of the space, the ones of the record of the value, which are the equality and the text of it, and the
/// members of the class of the math members are the ones that read the rotation of it, which are the rotation of
/// a value of 3 components, the product of two of them beside the two of a value of 3 components, the members
/// that read the length of the value and the ones that read the value of it as the one of the exponential and of
/// the logarithm of it.</para>
/// <para>The name of the value of a kind is the name of the type beside the suffix of the kind of its component,
/// so the value of a single precision kind carries no suffix and reaches the name of the type itself, which is
/// <c>quaternion</c>, and the ones of a double precision kind and of a half precision kind carry the suffixes
/// <c>_d</c> and <c>_h</c>, which are the suffixes the value of the type carries.</para>
/// <para>So far this generator reaches the value of the type, the rotations of it, the ones that take the value of
/// a matrix of 3 rows and 3 columns and the one of a matrix of 4 rows and 4 columns, the ones of the look
/// rotation of two values of 3 components and the members of the class of the math members that read the value of
/// a quaternion alone: the one of the rotation of a matrix and the ones of the adjugate of one are to come beside
/// the ones of the singular value decomposition of the value, which the member of the rotation of a matrix
/// reaches.</para>
/// </summary>
[Generator]
public class QuaternionGenerator : IIncrementalGenerator
{
    /// <summary>The namespace of the quaternion types, which is the one of every type of the library.</summary>
    public const string Namespace = "Coplt.Mathematics";

    /// <summary>
    /// The kinds the arithmetic of a quaternion reaches, which are the ones of a floating point component: the
    /// angle of a rotation, the value of the axis of it and the value of a view are the ones of that kind alone.
    /// </summary>
    private static readonly Typ[] Kinds = Typ.Typs.Where(static typ => typ.f).ToArray();

    /// <summary>The three axes of the space in the order of them.</summary>
    private static readonly string[] AxesName = { "x", "y", "z" };

    /// <summary>
    /// The three rotations of a single axis, which are the axis of a rotation, the axis that follows it and the
    /// axis that follows the two of them: the axis of a rotation keeps the value it is handed, the axis that
    /// follows the one of the rotation reaches the one that follows the two of them where the angle of the
    /// rotation is a right angle.
    /// </summary>
    private static readonly (string Axis, string Next, string Next2)[] Axes =
    {
        ("x", "y", "z"),
        ("y", "z", "x"),
        ("z", "x", "y"),
    };

    /// <summary>
    /// The six orders of the three Euler angles beside the signs of the four components the value of 3 components
    /// reaches the quaternion of them through, which are the six permutations of the three axes of the space.
    /// </summary>
    private static readonly (string Order, int X, int Y, int Z, int W)[] Orders =
    {
        ("XYZ", -1, 1, -1, 1),
        ("XZY", 1, 1, -1, -1),
        ("YXZ", -1, 1, 1, -1),
        ("YZX", -1, -1, 1, 1),
        ("ZXY", 1, -1, -1, 1),
        ("ZYX", 1, -1, 1, -1),
    };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => AddSources(ctx));
    }

    /// <summary>
    /// Adds the file of the members of every quaternion the arithmetic reaches, which holds the members of the
    /// value of that kind.
    /// </summary>
    /// <param name="ctx">The context of the generator</param>
    private static void AddSources(IncrementalGeneratorPostInitializationContext ctx)
    {
        foreach (var typ in Kinds)
        {
            var name = Name(typ);
            ctx.AddSource(
                $"{Namespace}.{name}.g.cs",
                SourceText.From(Gen(name, typ), Encoding.UTF8));
        }
    }

    /// <summary>
    /// Returns the name of a quaternion, which is the name of the type beside the suffix of the kind of its
    /// component: the value of a single precision kind carries no suffix, the one of a double precision kind
    /// carries <c>_d</c> and the one of a half precision kind carries <c>_h</c>.
    /// </summary>
    /// <param name="typ">The kind of the component of the quaternion</param>
    /// <returns>The name of the quaternion</returns>
    public static string Name(Typ typ) => $"quaternion{typ.structSuffix}";

    /// <summary>
    /// Generates the file of the members of the quaternion <paramref name="name"/>, which is the type of the
    /// value: the class of the math members reaches the members of a quaternion as well, but it is the class of
    /// every value of the library, so the file of a kind reaches the type of the value of it and the members of
    /// the arithmetic that take the value of a single quaternion.
    /// </summary>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    /// <returns>The file of the members</returns>
    private static string Gen(string name, Typ typ)
    {
        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, simdHelpers: false);
        sb.AppendLine();
        // the type of a quaternion is declared by this generator alone, so the doc of it is written here
        sb.AppendLine("/// <summary>A quaternion type for representing rotations.</summary>");
        // the text of a quaternion is the text of the four components of it, so the converter of it is the one of
        // the kind of a component of it alone, see Coplt.Mathematics.Json
        sb.AppendLine($"[JsonConverter(typeof(Json.{name}JsonConverter))]");
        sb.AppendLine($"public partial struct {name} :");
        // the value of a quaternion is the one of the four components of it: the comparison of two of them is the
        // one of the whole of a value, which the members of the framework that name a bool answer with, and the
        // ordering of it is the one of the four components of it in the order of them
        sb.AppendLine($"    IEquatable<{name}>,");
        sb.AppendLine($"    IEqualityOperators<{name}, {name}, bool>,");
        sb.AppendLine($"    IComparable<{name}>,");
        sb.AppendLine($"    IComparable,");
        sb.AppendLine($"    IComparisonOperators<{name}, {name}, bool>,");
        sb.AppendLine("    ISpanFormattable,");
        sb.AppendLine("    IUtf8SpanFormattable");
        sb.AppendLine("{");
        // the members of the value of the quaternion, the ones of a rotation of it and the ones every value of the
        // library has, which are the ones of the record of it
        sb.AppendLine("    // the members of the rotation of a matrix and the ones of the adjugate of one are to come, so this file");
        sb.AppendLine("    // reaches the value of the type, the rotations of it, the ones that take the value of a matrix and the");
        sb.AppendLine("    // equality and the text of its value");
        sb.AppendLine();
        sb.AppendLine("    #region value");
        sb.AppendLine();
        Value(sb, name, typ);
        sb.AppendLine();
        Matrices(sb, name, typ);
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();
        sb.AppendLine("    #region rotation");
        sb.AppendLine();
        Rotations(sb, name, typ);
        sb.AppendLine();
        EulerAngles(sb, name, typ);
        sb.AppendLine();
        Views(sb, name, typ);
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();
        sb.AppendLine("    #region record");
        sb.AppendLine();
        Records(sb, name, typ);
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine("}");
        sb.AppendLine();
        Math(sb, name, typ);
        return VectorGenShared.Normalize(sb.ToString());
    }

    /// <summary>
    /// Returns the text of a literal of the kind of a component, whose digits are the ones of the kind of a single
    /// precision number and whose suffix is the one of the kind.
    /// </summary>
    /// <param name="typ">The kind of the component</param>
    /// <param name="digits">The digits of the literal</param>
    /// <returns>The text of the literal</returns>
    private static string Lit(Typ typ, string digits) => typ.name switch
    {
        "float" => $"{digits}f",
        "double" => digits,
        _ => $"(half){digits}f",
    };

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

    /// <summary>
    /// Adds the lines of <paramref name="doc"/> to the source, every one of them is a line of the doc of the
    /// member that follows them.
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
    /// Adds the value of the quaternion to <paramref name="name"/>, which is the four components of the kind of
    /// its component beside the identity of it and the conversions of a value of 4 components into it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void Value(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var one = typ.one;
        sb.AppendLine($"    public {vec4} value;");
        sb.AppendLine();
        AddDoc(sb, """
                   Returns the identity of the quaternion, which is the value of no rotation of the space
                   <para>The value of the three components of the space is the zero of the kind and the value of the fourth
                   component is the one of the kind</para>
                   """, "    ");
        sb.AppendLine($"    public static {name} Identity");
        sb.AppendLine("    {");
        sb.AppendLine("        [MethodImpl(256)]");
        sb.AppendLine($"        get => new(default, default, default, {one});");
        sb.AppendLine("    }");
        sb.AppendLine();
        AddDoc(sb, """
                   Returns the quaternion of the four components of it
                   <para>The value of the three components of the space is the axis of the rotation of the value scaled by
                   the sine of the half of the angle of it and the value of the fourth component is the cosine of the half of
                   the angle</para>
                   <param name="x">The value of the first component, which is the one of the x axis of the space</param>
                   <param name="y">The value of the second component, which is the one of the y axis of the space</param>
                   <param name="z">The value of the third component, which is the one of the z axis of the space</param>
                   <param name="w">The value of the fourth component, which is the one of the angle of the rotation</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {name}({scalar} x, {scalar} y, {scalar} z, {scalar} w) => value = new(x, y, z, w);");
        sb.AppendLine();
        AddDoc(sb, """
                   Returns the quaternion of the value of the four components of it
                   <param name="value">The value of the four components, which is the value of the quaternion</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {name}({vec4} value) => this.value = value;");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns the quaternion of the value of the four components of it</summary>");
        sb.AppendLine("    /// <param name=\"value\">The value of the four components, which is the value of the quaternion</param>");
        sb.AppendLine("    /// <returns>The quaternion of the value</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static implicit operator {name}({vec4} value) => new(value);");
    }

    /// <summary>
    /// Adds the members that reach the value of the rotation of a matrix of 3 rows and 3 columns and the one of a
    /// matrix of 4 rows and 4 columns of the space to the quaternion <paramref name="name"/>: the value of the upper
    /// left of a matrix is the rotation of the space of 3 rows and 3 columns of it, so the two members read the four
    /// components of the quaternion out of the three columns of that rotation.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void Matrices(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var one = typ.one;
        var mask = typ.maskType;
        var sigMask = typ.sigMaskType;
        var maskNeg = typ.maskNeg;
        var (toMask, fromMask, toSigMask) = typ.Bits;
        var signShift = typ.size * 8 - 1;
        var mats = new[]
        {
            MatrixGenerator.Name(typ, 3, 3, false),
            MatrixGenerator.Name(typ, 4, 4, false),
        };
        foreach (var mat in mats)
        {
            AddDoc(sb, $"""
                        Returns the quaternion of the rotation of the value of <paramref name="m"/>, which is the rotation of
                        the space of the matrix
                        <para>The three columns of the upper left of the matrix have to be the ones of a rotation of the space,
                        which are of the length one and at a right angle with one another: the member reads the four components
                        of the quaternion out of them</para>
                        <param name="m">The matrix the rotation of the space is read out of</param>
                        """, "    ");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public {name}({mat} m)");
            sb.AppendLine("    {");
            sb.AppendLine("        var u = m.c0;");
            sb.AppendLine("        var v = m.c1;");
            sb.AppendLine("        var w = m.c2;");
            sb.AppendLine();
            sb.AppendLine("        // the sign of the first component of the first column of the matrix names the component of the");
            sb.AppendLine("        // value of the quaternion the value of it is read out of, and it turns the sign of the third");
            sb.AppendLine("        // component of the third column of it around");
            sb.AppendLine($"        var u_sign = ({mask})(BitConverter.{toMask}(u.x) & {maskNeg});");
            sb.AppendLine($"        var t = v.y + BitConverter.{fromMask}(({mask})(BitConverter.{toMask}(w.z) ^ u_sign));");
            sb.AppendLine("        // the sign of the first component of the first column of the matrix widened to the mask whose every");
            sb.AppendLine("        // component is the negative one where the sign of it is the negative one");
            sb.AppendLine($"        var u_mask = new {mask}4(({mask})(({sigMask})u_sign >> {signShift})).asf;");
            sb.AppendLine($"        var t_mask = new {sigMask}4(({sigMask})(BitConverter.{toSigMask}(t) >>" +
                          $" {signShift})).asf;");
            sb.AppendLine();
            sb.AppendLine($"        var tr = {one} + math.abs(u.x);");
            sb.AppendLine();
            sb.AppendLine("        // the sign of every component of the value of the quaternion, which is the negative one where the");
            sb.AppendLine("        // sign of the component of the matrix that reads it is the negative one");
            sb.AppendLine("        var sign_flips =");
            sb.AppendLine($"            new {mask}4(default, {maskNeg}, {maskNeg}, {maskNeg}).asf");
            sb.AppendLine($"            ^ (u_mask & new {mask}4(default, {maskNeg}, default, {maskNeg}).asf)");
            sb.AppendLine($"            ^ (t_mask & new {mask}4({maskNeg}, {maskNeg}, {maskNeg}, default).asf);");
            sb.AppendLine();
            sb.AppendLine($"        value = new {vec4}(tr, u.y, w.x, v.z) + (new {vec4}(t, v.x, u.z, w.y) ^ sign_flips);");
            sb.AppendLine();
            sb.AppendLine("        // the component the value of the quaternion is read out of every component of the result, which the");
            sb.AppendLine("        // sign of the two values the matrix is read through names");
            sb.AppendLine("        value = (value & ~u_mask) | (value.zwxy & u_mask);");
            sb.AppendLine("        value = (value.wzyx & ~t_mask) | (value & t_mask);");
            sb.AppendLine($"        value = math.normalize(value);");
            sb.AppendLine("    }");
            sb.AppendLine();
        }
    }

    /// <summary>
    /// Adds the members that reach the matrix of a rotation of the space to the quaternion
    /// <paramref name="name"/>, which are the rotation of an angle around the axis of a value of 3 components, the
    /// ones of the three Euler angles and the ones of a single axis of the space.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void Rotations(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec3 = VectorGenShared.VecName(typ, 3, false);
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var half = typ.half;
        var one = typ.one;

        AddDoc(sb, """
                   Returns the quaternion of the rotation of the space around the axis of <paramref name="axis"/> by
                   <paramref name="angle"/>
                   <para>Every angle of the rotation is in radians and the rotation of an angle is clockwise where the axis
                   of it is looked along towards the origin, so the length of the value is the one it was handed where the
                   axis of the rotation is of the length one and not the zero of the kind, and the rotation turns the axes
                   of the space</para>
                   <param name="axis">The axis of the rotation, which is of the length one</param>
                   <param name="angle">The angle of the rotation, in radians</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} AxisAngle({vec3} axis, {scalar} angle)");
        sb.AppendLine("    {");
        sb.AppendLine($"        math.sincos({half} * angle, out var sina, out var cosa);");
        sb.AppendLine($"        return new(new {vec4}(axis * sina, cosa));");
        sb.AppendLine("    }");
        sb.AppendLine();

        AddDoc(sb, """
                   Returns the quaternion of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the
                   one of the z-x-y order
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                   is looked along towards the origin, and the rotation turns the axes of the space</para>
                   <param name="xyz">The angles of the x axis, the y axis and the z axis, in radians</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Euler({vec3} xyz) => EulerZXY(xyz);");
        sb.AppendLine();
        AddDoc(sb, """
                   Returns the quaternion of the rotation of the three Euler angles of <paramref name="xyz"/> in the order
                   <paramref name="order"/> names
                   <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                   is looked along towards the origin, and the rotation turns the axes of the space</para>
                   <param name="xyz">The angles of the x axis, the y axis and the z axis, in radians</param>
                   <param name="order">The order the three rotations of the angles are applied in</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} Euler({vec3} xyz, RotationOrder order) => order switch");
        sb.AppendLine("    {");
        foreach (var order in Orders)
            sb.AppendLine($"        RotationOrder.{order.Order} => Euler{order.Order}(xyz),");
        sb.AppendLine("        _ => Identity,");
        sb.AppendLine("    };");
        sb.AppendLine();

        foreach (var order in Orders)
        {
            var axes = order.Order.ToLowerInvariant();
            AddDoc(sb, $"""
                        Returns the quaternion of the rotation of the three Euler angles of <paramref name="xyz"/>, which is the rotation
                        around the {axes[0]} axis by {Word(axes[0])} of them, then the rotation around the {axes[1]} axis by {Word(axes[1])} one
                        and finally the rotation around the {axes[2]} axis by {Word(axes[2])} one
                        <para>Every angle of the value is in radians and the rotation of an angle is clockwise where the axis of it
                        is looked along towards the origin, and the rotation turns the axes of the space</para>
                        <param name="xyz">The angles of the x axis, the y axis and the z axis, in radians</param>
                        """, "    ");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {name} Euler{order.Order}({vec3} xyz)");
            sb.AppendLine("    {");
            sb.AppendLine($"        var (s, c) = math.sincos(xyz * {half});");
            sb.AppendLine("        return new(");
            sb.AppendLine($"            new {vec4}(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * new {vec4}(c.xyz, s.x) *");
            sb.AppendLine($"            new {vec4}({Sign(order.X)}{one}, {Sign(order.Y)}{one}, {Sign(order.Z)}{one}, {Sign(order.W)}{one})");
            sb.AppendLine("        );");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        foreach (var (axis, next, next2) in Axes)
        {
            var index = axis[0] - 'x';
            var args = VectorGenShared.Join(3, i => i == index ? "sina" : "default");
            AddDoc(sb, $"""
                        Returns the quaternion of the rotation of the space around the {axis} axis by <paramref name="angle"/>
                        <para>Every angle of the rotation is in radians and the rotation of an angle is clockwise where the axis of
                        it is looked along towards the origin, so the {axis} axis keeps the value it is handed and the {next} axis
                        reaches the {next2} axis where the angle of the rotation is a right angle</para>
                        <param name="angle">The angle of the rotation, in radians</param>
                        """, "    ");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {name} Rotate{axis.ToUpperInvariant()}({scalar} angle)");
            sb.AppendLine("    {");
            sb.AppendLine($"        var (sina, cosa) = math.sincos(angle * {half});");
            sb.AppendLine($"        return new({args}, cosa);");
            sb.AppendLine("    }");
            sb.AppendLine();
        }
    }

    /// <summary>
    /// Returns the sign of a component the value of 3 components of an Euler rotation reaches the quaternion of
    /// the angles of it through, which is the negative one or nothing at all.
    /// </summary>
    /// <param name="sign">The sign of the component</param>
    /// <returns>The text of the sign</returns>
    private static string Sign(int sign) => sign < 0 ? "-" : "";

    /// <summary>
    /// True when the three axes of an order of the three Euler angles come in the order of the axes of the space,
    /// which is the parity of the permutation of the axes of it: the axes of an order that is an even permutation
    /// of them are the ones of the space turned around and the ones of an odd permutation of them are the ones of
    /// the space turned around and of the space of the other side of them.
    /// </summary>
    /// <param name="order">The name of the order</param>
    /// <returns>True when the permutation of the axes of the order is even</returns>
    private static bool Even(string order)
    {
        var inversions = 0;
        for (var i = 0; i < order.Length; i++)
        for (var j = i + 1; j < order.Length; j++)
        {
            if (char.ToLowerInvariant(order[i]) > char.ToLowerInvariant(order[j])) inversions++;
        }

        return inversions % 2 == 0;
    }

    /// <summary>
    /// Adds the members that read the three Euler angles of the rotation of the quaternion out of the value of it
    /// to the quaternion <paramref name="name"/>, which are the inverse of the ones of the rotations of it: every
    /// order of the three angles has the member of its own and the two members that take the order of them reach
    /// the member of the order they name.
    /// <para>The three angles of an order are read out of the value of the quaternion with the axes of it turned
    /// so that the axis in the middle of the order takes the place of the x axis, which the value of 3 components
    /// of a rotation, the product of the components of the value that name the sine of the angle of an axis and
    /// the squared value of it are the ones of the angles of the order that way. The order of the axes of the
    /// angles is the one of the space for an even permutation of them and the one of the space of the other side
    /// for an odd one, which the signs of the products of the axes of the value are the ones of.</para>
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void EulerAngles(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec3 = VectorGenShared.VecName(typ, 3, false);
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var one = typ.one;
        var two = typ.two;
        // the angle of the middle axis of an order that is a right angle cannot be told apart from the ones of
        // the two axes beside it, which the two of them reach the quaternion of the three angles of the order
        // through: the sum and the difference of the two of them is the whole of what the value holds
        var halfPi = typ.name switch
        {
            "float" => "math.F_Half_PI",
            "double" => "math.D_Half_PI",
            _ => "(half)math.F_Half_PI",
        };
        // the angle of the middle axis of an order that is a right angle is read out of the value with the sine
        // of the angle of it, which the value of the kind of the component holds to the digits of the kind of it:
        // the cutoff is the value that is as close to the one as the kind of it reaches
        var digits = typ.name switch
        {
            "float" => "0.99999",
            "double" => "0.99999999999",
            _ => "0.999",
        };
        var cutoff = Lit(typ, digits);

        // the member of every order holds the whole of the read back of the angles of the value of a quaternion,
        // which is long enough not to be inlined into its callers, while the two members that take the order of
        // the angles are the ones that reach the member of the order they name and nothing else of their own
        AddDoc(sb, $"""
                    Reads the three Euler angles of the rotation of the value out of it, which is the one of the z-x-y
                    order
                    <para>Every angle of the result is in radians and the rotation of an angle is clockwise where the
                    axis of it is looked along towards the origin, so the rotation the angles of the result hold is the
                    one the value holds</para>
                    <returns>The angles of the x axis, the y axis and the z axis of the rotation of the value, in radians</returns>
                    """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {vec3} ToEulerAngles() => ToEulerZXY();");
        sb.AppendLine();

        AddDoc(sb, """
                   Reads the three Euler angles of the rotation of the value out of it, which is the one of the order
                   <paramref name="order"/> names
                   <para>Every angle of the result is in radians and the rotation of an angle is clockwise where the
                   axis of it is looked along towards the origin, so the rotation the angles of the result hold is the
                   one the value holds</para>
                   <param name="order">The order the three rotations of the angles are read in</param>
                   <returns>The angles of the x axis, the y axis and the z axis of the rotation of the value, in radians</returns>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public {vec3} ToEulerAngles(RotationOrder order) => order switch");
        sb.AppendLine("    {");
        foreach (var order in Orders)
            sb.AppendLine($"        RotationOrder.{order.Order} => ToEuler{order.Order}(),");
        sb.AppendLine($"        _ => ToEulerZXY(),");
        sb.AppendLine("    };");
        sb.AppendLine();

        foreach (var order in Orders)
        {
            var axes = order.Order.ToLowerInvariant();
            var middle = "xyz".IndexOf(axes[1]);
            var next = (middle + 1) % 3;
            var next2 = (middle + 2) % 3;
            var even = Even(order.Order);
            var relabel = middle switch
            {
                1 => ".yzxw",
                2 => ".zxyw",
                _ => "",
            };
            var back = middle switch
            {
                1 => ".zxy",
                2 => ".yzx",
                _ => "",
            };
            var cross = even
                ? $"{Sign(-1)}{one}, {one}, {one}, {one}"
                : $"{one}, {Sign(-1)}{one}, {Sign(-1)}{one}, {one}";
            // the pair of the axes of the value that names the sine of the sum and of the difference of the two
            // angles beside the middle one is the difference of the product of the middle axis and the one that
            // follows it with the product of the last one of them and the one of the value itself for the axes
            // of the space, and the sum of the two of them for the axes of the space of the other side
            var sum = even ? "-" : "+";

            AddDoc(sb, $"""
                        Reads the three Euler angles of the rotation of the value out of it, which is the rotation around the {axes[0]} axis by {Word(axes[0])} of them, then the rotation around the {axes[1]} axis by {Word(axes[1])} one and finally the rotation around the {axes[2]} axis by {Word(axes[2])} one
                        <para>Every angle of the result is in radians and the rotation of an angle is clockwise where the axis of it is looked along towards the origin, so the rotation the angles of the result hold is the one the value holds</para>
                        <para>The three angles of an order whose middle rotation is of a right angle are not read out of the value on their own: the member answers with the angle of the {axes[middle]} axis at the right angle, with the sum or the difference of the other two at the {axes[next]} axis one and with the zero of the kind at the {axes[next2]} axis one</para>
                        <returns>The angles of the x axis, the y axis and the z axis of the rotation of the value, in radians</returns>
                        """, "    ");
            sb.AppendLine($"    public {vec3} ToEuler{order.Order}()");
            sb.AppendLine("    {");
            sb.AppendLine($"        var q = value{relabel};");
            sb.AppendLine($"        var cross = math.chg_sign(q.yzxw * q.zxyw, new {vec4}({cross}));");
            sb.AppendLine($"        var num = {two} * math.fma(q.wwww, q, cross);");
            sb.AppendLine("        var q2 = q * q;");
            sb.AppendLine("        var sin = num.x;");
            sb.AppendLine($"        if (math.abs(sin) >= {cutoff})");
            sb.AppendLine("        {");
            sb.AppendLine($"            var angle = math.atan2(");
            sb.AppendLine($"                math.chg_sign({two} * ((q.x * q.y) {sum} (q.z * q.w)), sin),");
            sb.AppendLine($"                math.fsm({one}, {two}, q2.y + q2.z)");
            sb.AppendLine("            );");
            sb.AppendLine($"            return new {vec3}(math.chg_sign({halfPi}, sin), angle, default){back};");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        var den = math.fsm({one}, {two}, q2 + q2.xxxx);");
            sb.AppendLine("        var r = math.atan2(num, den);");
            sb.AppendLine($"        r.x = math.asin(math.clamp(sin, -{one}, {one}));");
            sb.AppendLine($"        return r.xyz{back};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }
    }

    /// <summary>
    /// Adds the members that reach the rotation of the space that looks along a value while another one stays over
    /// it to the quaternion <paramref name="name"/>: the member that reads the two values of it where they are of
    /// the length one is the one a value of another length does not reach, which the member of the safe rotation
    /// takes to the length one first.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void Views(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec3 = VectorGenShared.VecName(typ, 3, false);
        var mat3 = MatrixGenerator.Name(typ, 3, 3, false);
        AddDoc(sb, """
                   Returns the quaternion of the rotation of the space that looks along <paramref name="forward"/> while the
                   value of <paramref name="up"/> stays over it
                   <para>Every one of the two values of the member has to be of the length one and the two of them must not be
                   collinear, so a value that is of another length or that is collinear with the other one reaches the member
                   of the safe rotation, which takes the two of them to the length one first</para>
                   <param name="forward">The value the rotation looks along, which is of the length one</param>
                   <param name="up">The value that stays over the one that is looked along, which is of the length one</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookRotation({vec3} forward, {vec3} up)");
        sb.AppendLine("    {");
        sb.AppendLine("        var t = math.normalize(math.cross(up, forward));");
        sb.AppendLine($"        return new(new {mat3}(t, math.cross(forward, t), forward));");
        sb.AppendLine("    }");
        sb.AppendLine();
        AddDoc(sb, """
                   Returns the quaternion of the rotation of the space that looks along <paramref name="forward"/> while the
                   value of <paramref name="up"/> stays over it, where the two values of it are taken to the length one first
                   <para>Every one of the two values is accepted whatever its length is where the length of the two of them and
                   the one of the axis that is at a right angle with them are the ones the kind of the component reads, and the
                   identity of the quaternion is reached where the two of them are collinear or where one of them is so short
                   or so long that the member cannot read it</para>
                   <param name="forward">The value the rotation looks along</param>
                   <param name="up">The value that stays over the one that is looked along</param>
                   """, "    ");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {name} LookRotationSafe({vec3} forward, {vec3} up)");
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
        sb.AppendLine($"        return new(new {mat3}(t, math.cross(forward, t), forward));");
        sb.AppendLine("    }");
    }

    /// <summary>
    /// Adds the members every value of the library has to the quaternion <paramref name="name"/>, which are the
    /// equality of two values, the ordering of them, the hash of the value of one of them and the text of it.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void Records(StringBuilder sb, string name, Typ typ)
    {
        sb.AppendLine("    /// <summary>Returns whether the value of <paramref name=\"obj\"/> is the one of the quaternion</summary>");
        sb.AppendLine("    /// <param name=\"obj\">The value the quaternion is compared with</param>");
        sb.AppendLine("    /// <returns>True where the value is the one of a quaternion of the same value</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public readonly override bool Equals(object? obj) => obj is {name} other && Equals(other);");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns whether the value of <paramref name=\"other\"/> is the one of the quaternion</summary>");
        sb.AppendLine("    /// <param name=\"other\">The other quaternion</param>");
        sb.AppendLine("    /// <returns>True where the two values are the one of the kind</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public readonly bool Equals({name} other) => value.Equals(other.value);");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns the hash of the value of the quaternion</summary>");
        sb.AppendLine("    /// <returns>The hash of the value of the four components of it</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine("    public readonly override int GetHashCode() => value.GetHashCode();");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns whether the values of the two quaternions are the one of the kind</summary>");
        sb.AppendLine("    /// <param name=\"left\">The quaternion</param>");
        sb.AppendLine("    /// <param name=\"right\">The other quaternion</param>");
        sb.AppendLine("    /// <returns>True where the two values are the one of the kind</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static bool operator ==({name} left, {name} right) => left.Equals(right);");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns whether the values of the two quaternions are not the one of the kind</summary>");
        sb.AppendLine("    /// <param name=\"left\">The quaternion</param>");
        sb.AppendLine("    /// <param name=\"right\">The other quaternion</param>");
        sb.AppendLine("    /// <returns>True where the two values are not the one of the kind</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static bool operator !=({name} left, {name} right) => !left.Equals(right);");
        sb.AppendLine();

        // the ordering of two quaternions is the one of the four components of them in the order of the
        // components, which is the one of a columns of a matrix through the columns of it
        sb.AppendLine("    /// <summary>Returns the position of the value of the quaternion against the one of another quaternion</summary>");
        sb.AppendLine("    /// <param name=\"other\">The other quaternion</param>");
        sb.AppendLine("    /// <returns>A negative number where the value of the quaternion is the one before the value of the other one, the zero where the two are the one and a positive number where it is the one after it</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public readonly int CompareTo({name} other)");
        sb.AppendLine("    {");
        for (var i = 0; i < 4; i++)
        {
            var comp = "xyzw"[i];
            sb.AppendLine($"        {(i == 0 ? "var " : "")}c = value.{comp}.CompareTo(other.value.{comp});");
            sb.AppendLine("        if (c != 0) return c;");
        }

        sb.AppendLine("        return 0;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc cref=\"CompareTo(" + name + ")\"/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    int IComparable.CompareTo(object? obj) => obj is {name} other");
        sb.AppendLine("        ? CompareTo(other)");
        sb.AppendLine("        : throw new ArgumentException(null, nameof(obj));");
        sb.AppendLine();
        foreach (var op in new[] { "<", "<=", ">", ">=" })
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static bool operator {op}({name} left, {name} right) => left.CompareTo(right) {op} 0;");
            sb.AppendLine();
        }

        // the text of a quaternion is the text of the four components of it between parentheses, every part of it
        // is written into the destination without a string in between, so a part that does not fit leaves the
        // count at zero and fails
        var text = VectorGenShared.Join(4, i => "{value." + "xyzw"[i] + "}", ", ");
        sb.AppendLine("    /// <summary>Formats the quaternion as the four components of it between parentheses</summary>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine("    public readonly override string ToString() => $\"(" + text + ")\";");
        sb.AppendLine();
        var textFormat = VectorGenShared.Join(4, i => "{value." + "xyzw"[i] + ".ToString(format, formatProvider)}", ", ");
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine("    public readonly string ToString(string? format, IFormatProvider? formatProvider) => " +
                      "$\"(" + textFormat + ")\";");
        sb.AppendLine();
        List<string> Parts(string suffix)
        {
            var calls = new List<string>
            {
                $"FormatUtils.TryFormatPart(ref d, ref n, \"(\"{suffix})",
            };
            for (var i = 0; i < 4; i++)
            {
                if (i != 0) calls.Add($"FormatUtils.TryFormatPart(ref d, ref n, \", \"{suffix})");
                calls.Add($"FormatUtils.TryFormatPart(ref d, ref n, value.{"xyzw"[i]}, format, provider)");
            }

            calls.Add($"FormatUtils.TryFormatPart(ref d, ref n, \")\"{suffix})");
            return calls;
        }

        void EmitTryFormat(string span, string suffix)
        {
            var calls = Parts(suffix);
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public readonly bool TryFormat({span} destination, out int charsWritten, " +
                          "ReadOnlySpan<char> format, IFormatProvider? provider)");
            sb.AppendLine("    {");
            sb.AppendLine("        var n = 0;");
            sb.AppendLine("        var d = destination;");
            var cond = new StringBuilder();
            for (var i = 0; i < calls.Count; i++)
            {
                cond.Append(i == 0 ? $"if (!{calls[i]}" : $"\n            || !{calls[i]}");
            }

            cond.Append(")");
            sb.AppendLine($"        {cond}");
            sb.AppendLine("        {");
            sb.AppendLine("            charsWritten = 0;");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        charsWritten = n;");
            sb.AppendLine("        return true;");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitTryFormat("Span<char>", "");
        EmitTryFormat("Span<byte>", "u8");
    }

    /// <summary>The text of a member of the class of the math members and the one of the extension member of it.</summary>
    private sealed class Member(
        string summary,
        string signature,
        string body,
        string cref,
        string extSignature,
        string extCall)
    {
        public string Summary { get; } = summary;
        public string Signature { get; } = signature;
        public string Body { get; } = body;
        public string Cref { get; } = cref;
        public string ExtSignature { get; } = extSignature;
        public string ExtCall { get; } = extCall;
    }

    /// <summary>
    /// Adds the members of the class of the math members that read the value of a quaternion to the source, which
    /// are emitted into the class of the math members and into the class of the extension members beside it, where
    /// the value of the quaternion is the value the member is reached on.
    /// </summary>
    /// <param name="sb">The source of the file</param>
    /// <param name="name">The name of the quaternion</param>
    /// <param name="typ">The kind of the component of the quaternion</param>
    private static void Math(StringBuilder sb, string name, Typ typ)
    {
        var scalar = typ.compType;
        var vec3 = VectorGenShared.VecName(typ, 3, false);
        var vec4 = VectorGenShared.VecName(typ, 4, false);
        var one = typ.one;
        var negOne = $"-{one}";
        var two = typ.two;
        var half = typ.half;
        var zero = Lit(typ, "0.0");
        var nearOne = Lit(typ, "0.9995");
        var conjugate = $"new(q.value * new {vec4}({negOne}, {negOne}, {negOne}, {one}))";

        var members = new List<Member>
        {
            new("""
                Returns the conjugate of <paramref name="q"/>, which is the value of the three components of the space of
                it turned around and the value of the fourth one where it is
                <param name="q">The quaternion</param>
                """,
                $"{name} conjugate({name} q)", $"=> {conjugate};",
                $"math.conjugate({name})", $"{name} conjugate(this {name} q)", "math.conjugate(q)"),
            new("""
                Returns the inverse of <paramref name="q"/>, which is the conjugate of it scaled by the reciprocal of the
                value of the dot product of it with itself
                <param name="q">The quaternion</param>
                """,
                $"{name} inverse({name} q)",
                $"=> new(math.rcp(math.dot<{vec4}, {scalar}>(q.value, q.value)) * q.value * new {vec4}({negOne}, {negOne}, {negOne}, {one}));",
                $"math.inverse({name})", $"{name} inverse(this {name} q)", "math.inverse(q)"),
            new("""
                Returns the value of the dot product of the two quaternions
                <param name="a">The quaternion</param>
                <param name="b">The other quaternion</param>
                """,
                $"{scalar} dot({name} a, {name} b)", $"=> math.dot<{vec4}, {scalar}>(a.value, b.value);",
                $"math.dot({name}, {name})", $"{scalar} dot(this {name} a, {name} b)", "math.dot(a, b)"),
            new("""
                Returns the length of <paramref name="q"/>, which is the square root of the value of the dot product of it
                with itself
                <param name="q">The quaternion</param>
                """,
                $"{scalar} length({name} q)", $"=> math.sqrt(math.dot<{vec4}, {scalar}>(q.value, q.value));",
                $"math.length({name})", $"{scalar} length(this {name} q)", "math.length(q)"),
            new("""
                Returns the value of the dot product of <paramref name="q"/> with itself, which is the square of the length
                of it
                <param name="q">The quaternion</param>
                """,
                $"{scalar} length_sq({name} q)", $"=> math.dot<{vec4}, {scalar}>(q.value, q.value);",
                $"math.length_sq({name})", $"{scalar} length_sq(this {name} q)", "math.length_sq(q)"),
            new("""
                Returns <paramref name="q"/> taken to the length one, which is the value of it scaled by the reciprocal of
                the square root of the value of the dot product of it with itself
                <param name="q">The quaternion</param>
                """,
                $"{name} normalize({name} q)",
                $"=> new(math.rsqrt(math.dot<{vec4}, {scalar}>(q.value, q.value)) * q.value);",
                $"math.normalize({name})", $"{name} normalize(this {name} q)", "math.normalize(q)"),
            new("""
                Returns the identity of the quaternion where the length of <paramref name="q"/> is too short for the kind
                of the component, and the value of <paramref name="q"/> taken to the length one everywhere else
                <param name="q">The quaternion</param>
                """,
                $"{name} normalize_safe({name} q)",
                $$"""
                  {
                      var len = length_sq(q);
                      if (len <= math.MinNormal<{{scalar}}>()) return {{name}}.Identity;
                      return new(math.rsqrt(len) * q.value);
                  }
                  """,
                $"math.normalize_safe({name})", $"{name} normalize_safe(this {name} q)", "math.normalize_safe(q)"),
            new("""
                Returns <paramref name="default_value"/> where the length of <paramref name="q"/> is too short for the
                kind of the component, and the value of <paramref name="q"/> taken to the length one everywhere else
                <param name="q">The quaternion</param>
                <param name="default_value">The value that is reached where the length of the quaternion cannot be read</param>
                """,
                $"{name} normalize_safe({name} q, {name} default_value)",
                $$"""
                  {
                      var len = length_sq(q);
                      if (len <= math.MinNormal<{{scalar}}>()) return default_value;
                      return new(math.rsqrt(len) * q.value);
                  }
                  """,
                $"math.normalize_safe({name}, {name})", $"{name} normalize_safe(this {name} q, {name} default_value)",
                "math.normalize_safe(q, default_value)"),
            new("""
                Returns the value of the exponential of the unit quaternion of <paramref name="q"/>, whose value is the one
                of the unit quaternion of the value of it
                <param name="q">The quaternion</param>
                """,
                $"{name} unit_exp({name} q)",
                $$"""
                  {
                      var v_rcp_len = math.rsqrt(math.dot(q.value.xyz, q.value.xyz));
                      var v_len = math.rcp(v_rcp_len);
                      math.sincos(v_len, out var sin_v_len, out var cos_v_len);
                      return new(new {{vec4}}(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len));
                  }
                  """,
                $"math.unit_exp({name})", $"{name} unit_exp(this {name} q)", "math.unit_exp(q)"),
            new("""
                Returns the value of the exponential of <paramref name="q"/>, which is the value of the unit quaternion of
                it scaled by the value of the exponential of the fourth component of it
                <param name="q">The quaternion</param>
                """,
                $"{name} exp({name} q)",
                $$"""
                  {
                      var v_rcp_len = math.rsqrt(math.dot(q.value.xyz, q.value.xyz));
                      var v_len = math.rcp(v_rcp_len);
                      math.sincos(v_len, out var sin_v_len, out var cos_v_len);
                      return new(new {{vec4}}(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len) * math.exp(q.value.w));
                  }
                  """,
                $"math.exp({name})", $"{name} exp(this {name} q)", "math.exp(q)"),
            new("""
                Returns the value of the logarithm of the unit quaternion of <paramref name="q"/>, whose value is the one
                of the unit quaternion of the value of it
                <param name="q">The quaternion</param>
                """,
                $"{name} unit_log({name} q)",
                $$"""
                  {
                      var w = math.clamp(q.value.w, {{negOne}}, {{one}});
                      var s = math.acos(w) * math.rsqrt({{one}} - w * w);
                      return new(new {{vec4}}(q.value.xyz * s, default));
                  }
                  """,
                $"math.unit_log({name})", $"{name} unit_log(this {name} q)", "math.unit_log(q)"),
            new("""
                Returns the value of the logarithm of <paramref name="q"/>, which is the value of the unit quaternion of it
                scaled by the value of the logarithm of the length of it
                <param name="q">The quaternion</param>
                """,
                $"{name} log({name} q)",
                $$"""
                  {
                      var v_len_sq = math.dot(q.value.xyz, q.value.xyz);
                      var q_len_sq = v_len_sq + q.value.w * q.value.w;
                      var s = math.acos(math.clamp(q.value.w * math.rsqrt(q_len_sq), {{negOne}}, {{one}})) *
                          math.rsqrt(v_len_sq);
                      return new(new {{vec4}}(q.value.xyz * s, {{half}} * math.log(q_len_sq)));
                  }
                  """,
                $"math.log({name})", $"{name} log(this {name} q)", "math.log(q)"),
            new("""
                Returns the product of the two quaternions, which is the rotation of the one of them beside the one of the
                other one
                <param name="a">The quaternion</param>
                <param name="b">The other quaternion</param>
                """,
                $"{name} mul({name} a, {name} b)",
                $"""
                 => new(
                     a.value.wwww * b.value + (a.value.xyzx * b.value.wwwx + a.value.yzxy * b.value.zxyy)
                     * new {vec4}({one}, {one}, {one}, {negOne}) - a.value.zxyz * b.value.yzxz
                 );
                 """,
                $"math.mul({name}, {name})", $"{name} mul(this {name} a, {name} b)", "math.mul(a, b)"),
            new("""
                Returns the value of <paramref name="v"/> turned by the rotation of <paramref name="q"/>
                <param name="q">The quaternion of the rotation</param>
                <param name="v">The value the rotation reaches</param>
                """,
                $"{vec3} mul({name} q, {vec3} v)",
                $$"""
                  {
                      var t = {{two}} * math.cross(q.value.xyz, v);
                      return v + q.value.w * t + math.cross(q.value.xyz, t);
                  }
                  """,
                $"math.mul({name}, {vec3})", $"{vec3} mul(this {name} q, {vec3} v)", "math.mul(q, v)"),
            new("""
                Returns the value of <paramref name="v"/> turned by the rotation of <paramref name="q"/>
                <param name="q">The quaternion of the rotation</param>
                <param name="v">The value the rotation reaches</param>
                """,
                $"{vec3} rotate({name} q, {vec3} v)", "=> mul(q, v);",
                $"math.rotate({name}, {vec3})", $"{vec3} rotate(this {name} q, {vec3} v)", "math.rotate(q, v)"),
            new("""
                Returns the value of the linear interpolation of the two quaternions by <paramref name="t"/>, whose value
                is taken to the length one, so the length of the value of the result is the one of the kind
                <para>The value of the other quaternion is turned around where the value of the dot product of the two of
                them is the negative one, so the two values the member reaches are the ones of the closer half of the
                space</para>
                <param name="t">The value of the interpolation, which is between the zero of the kind and the one of it</param>
                <param name="q1">The value of the interpolation at the zero of the kind</param>
                <param name="q2">The value of the interpolation at the one of the kind</param>
                """,
                $"{name} nlerp({scalar} t, {name} q1, {name} q2)",
                $"""
                 => normalize(q1.value + t *
                     (math.chg_sign(q2.value, {vec4}.Broadcast(math.dot<{vec4}, {scalar}>(q1.value, q2.value))) -
                     q1.value));
                 """,
                $"math.nlerp({scalar}, {name}, {name})", $"{name} nlerp(this {scalar} t, {name} q1, {name} q2)",
                "math.nlerp(t, q1, q2)"),
            new("""
                Returns the value of the spherical linear interpolation of the two quaternions by
                <paramref name="t"/>, whose value is taken to the length one
                <para>The value of the other quaternion is turned around where the value of the dot product of the two of
                them is the negative one, so the two values the member reaches are the ones of the closer half of the
                space, and the value of the member where the two of them are close to each other is the one of the linear
                interpolation of them</para>
                <param name="t">The value of the interpolation, which is between the zero of the kind and the one of it</param>
                <param name="q1">The value of the interpolation at the zero of the kind</param>
                <param name="q2">The value of the interpolation at the one of the kind</param>
                """,
                $"{name} slerp({scalar} t, {name} q1, {name} q2)",
                $$"""
                  {
                      var dt = dot(q1, q2);
                      if (dt < {{zero}})
                      {
                          dt = -dt;
                          q2.value = -q2.value;
                      }

                      if (dt < {{nearOne}})
                      {
                          var angle = math.acos(dt);
                          // the one of the sine of the angle, which the two values are scaled by
                          var s = math.rsqrt({{one}} - dt * dt);
                          var w1 = math.sin(angle * ({{one}} - t)) * s;
                          var w2 = math.sin(angle * t) * s;
                          return new(q1.value * w1 + q2.value * w2);
                      }

                      // the two values are close to each other, so the linear interpolation of them is the one of the
                      // spherical one of them
                      return nlerp(t, q1, q2);
                  }
                  """,
                $"math.slerp({scalar}, {name}, {name})", $"{name} slerp(this {scalar} t, {name} q1, {name} q2)",
                "math.slerp(t, q1, q2)"),
            new("""
                Returns the angle between the rotations of the two quaternions
                <param name="q1">The quaternion of the rotation</param>
                <param name="q2">The quaternion of the other rotation</param>
                """,
                $"{scalar} angle({name} q1, {name} q2)",
                $$"""
                  {
                      var v = normalize(mul(conjugate(q1), q2)).value.xyz;
                      var diff = math.asin(math.sqrt(math.dot(v, v)));
                      return diff + diff;
                  }
                  """,
                $"math.angle({name}, {name})", $"{scalar} angle(this {name} q1, {name} q2)", "math.angle(q1, q2)"),
        };

        sb.AppendLine("#region math");
        sb.AppendLine();
        sb.AppendLine("public static partial class math");
        sb.AppendLine("{");
        var first = true;
        foreach (var member in members)
        {
            if (!first) sb.AppendLine();
            first = false;
            AddDoc(sb, member.Summary, "    ");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {member.Signature}");
            foreach (var line in member.Body.Replace("\r\n", "\n").Split('\n'))
                sb.AppendLine(line.Length == 0 ? "" : $"        {line}");
        }

        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("#endregion");
        sb.AppendLine();
        sb.AppendLine("#region math_ex");
        sb.AppendLine();
        sb.AppendLine("public static partial class math_ex");
        sb.AppendLine("{");
        first = true;
        foreach (var member in members)
        {
            if (!first) sb.AppendLine();
            first = false;
            sb.AppendLine($"    /// <inheritdoc cref=\"{member.Cref}\"/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {member.ExtSignature} => {member.ExtCall};");
        }

        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("#endregion");
    }
}
