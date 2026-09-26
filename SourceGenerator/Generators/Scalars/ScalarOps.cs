using System;
using System.Collections.Generic;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// An operation of a scalar type: the function of the <c>math</c> class and the member that is called on a
/// value. The two forms are the same operation, the function takes the value it works on as its first parameter
/// and the member receives it as its receiver.
/// </summary>
/// <param name="Name">The name of the function and of the member</param>
/// <param name="Kind">The types that have the operation, see <see cref="ScalarOps.Matches"/></param>
/// <param name="Params">
/// The declaration of the parameters of the function, the name of a parameter may carry a modifier in front of
/// it or a default value behind it
/// </param>
/// <param name="Self">
/// The index of the parameter that the operation works on, it is the receiver of the member, <c>-1</c> when the
/// operation has no member
/// </param>
/// <param name="Returns">The return type of the operation, the type of the value itself when it is empty</param>
/// <param name="Bcl">
/// The name of the operation in the table of the expressions of the BCL, the name of the operation itself when
/// it is empty, see <see cref="ScalarOps.BclExpr"/>
/// </param>
/// <param name="Body">The body of the operation, see <see cref="ScalarOps.Expand"/></param>
/// <param name="IntBody">The body of an integer type, <paramref name="Body"/> when it is empty</param>
/// <param name="HalfBody">The body of a half, <paramref name="Body"/> when it is empty</param>
/// <param name="Ext">
/// The declaration of the parameters of the member that is called on a value: the parameters of the function
/// without the one the operation works on, see <see cref="Operands"/>
/// </param>
internal sealed record ScalarOp(
    string Name,
    string Kind,
    string[] Params,
    int Self = 0,
    string Returns = "",
    string Bcl = "",
    string Body = "",
    string IntBody = "",
    string HalfBody = "",
    string[]? Ext = null)
{
    /// <summary>
    /// The tokens of the part of a declaration in front of its default value. The last token is the name of the
    /// value, the ones in front of it are its type and its modifier, one of the two at most, see
    /// <see cref="Declaration"/>.
    /// </summary>
    /// <param name="declaration">The declaration of the parameter</param>
    /// <returns>The tokens of the declaration</returns>
    public static string[] Head(string declaration)
    {
        var equals = declaration.IndexOf('=');
        var text = (equals < 0 ? declaration : declaration[..equals]).Trim();
        return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Returns the name of the value of a parameter, see <see cref="Head"/>.
    /// </summary>
    /// <param name="declaration">The declaration of the parameter</param>
    /// <returns>The name of the value</returns>
    public static string ValueName(string declaration) => Head(declaration)[^1];

    /// <summary>
    /// Returns the type of the value of a parameter: the type the operation is generated for unless the
    /// declaration names one of its own, see <see cref="TypeKeywords"/>.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <param name="declaration">The declaration of the parameter</param>
    /// <returns>The type of the value</returns>
    public static string TypeOf(string scalar, string declaration)
    {
        var head = Head(declaration);
        return head.Length > 1 && Array.IndexOf(TypeKeywords, head[0]) >= 0 ? head[0] : scalar;
    }

    /// <summary>
    /// The keywords that a declaration uses as the type of the value of a parameter. A declaration that starts
    /// with one of them takes a value of that type instead of a value of the type the operation is generated for,
    /// which the select of a condition needs.
    /// </summary>
    public static readonly string[] TypeKeywords =
    [
        "bool", "sbyte", "byte", "short", "ushort", "int", "uint", "long", "ulong", "float", "double", "half",
        "decimal",
    ];

    /// <summary>
    /// The type that the member of the operation that is called on a value is called on, empty when the operation
    /// has no such member. It is the type of the parameter the operation works on, which is not the type of the
    /// value itself for an operation that takes a condition.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <returns>The type of the receiver</returns>
    public string Receiver(string scalar) => Self < 0 ? "" : TypeOf(scalar, Params[Self]);

    /// <summary>
    /// The names of the values of the parameters of the member that is called on a value, in the order of the
    /// parameters of the function without the one the operation works on. The other operand of a binary
    /// operation is named after the member of the vector that has the same operation.
    /// </summary>
    private string[] ExtParams()
    {
        if (Ext != null) return Ext;
        var names = new List<string>();
        for (var i = 0; i < Params.Length; i++)
        {
            if (i != Self) names.Add(Params[i]);
        }

        return names.ToArray();
    }

    /// <summary>
    /// The names of the values of the operands of the operation, in the order of the parameters of the function.
    /// The value that the member is called on is named <c>value</c>.
    /// </summary>
    /// <param name="ext">True for the member that is called on a value</param>
    /// <returns>The name of every operand</returns>
    public string[] Operands(bool ext)
    {
        var operands = new string[Params.Length];
        if (!ext)
        {
            for (var i = 0; i < operands.Length; i++) operands[i] = ValueName(Params[i]);
            return operands;
        }

        var rest = ExtParams();
        var n = 0;
        for (var i = 0; i < operands.Length; i++) operands[i] = i == Self ? "value" : ValueName(rest[n++]);
        return operands;
    }

    /// <summary>
    /// The declaration of a parameter of one of the two forms of the operation: its type, which is the type of the
    /// value unless the declaration names another one, its name and the default value that may be behind it.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <param name="parameter">The declaration of the parameter</param>
    /// <returns>The parameter</returns>
    public static string Declaration(string scalar, string parameter)
    {
        var equals = parameter.IndexOf('=');
        var suffix = equals < 0 ? "" : parameter[equals..].Trim();
        var head = Head(parameter);
        var name = head[^1];
        var text = head.Length switch
        {
            // the declaration names the type of the value itself
            2 when Array.IndexOf(TypeKeywords, head[0]) >= 0 => $"{head[0]} {name}",
            // the declaration carries a modifier in front of the type of the value
            2 => $"{head[0]} {scalar} {name}",
            _ => $"{scalar} {name}",
        };
        return suffix.Length == 0 ? text : $"{text} {suffix}";
    }

    /// <summary>
    /// The declaration of the parameters of one of the two forms of the operation.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <param name="ext">True for the member that is called on a value</param>
    /// <returns>The parameter list</returns>
    public string Parameters(string scalar, bool ext)
    {
        var list = new List<string>();
        foreach (var parameter in ext ? ExtParams() : Params) list.Add(Declaration(scalar, parameter));
        return string.Join(", ", list);
    }

    /// <summary>
    /// The names of the parameters of one of the two forms of the operation, they are the names that the
    /// documentation of the operation is looked up by.
    /// </summary>
    /// <param name="ext">True for the member that is called on a value</param>
    /// <returns>The name of every parameter</returns>
    public string[] ParameterNames(bool ext)
    {
        var names = ext ? ExtParams() : Params;
        var result = new string[names.Length];
        for (var i = 0; i < names.Length; i++) result[i] = ValueName(names[i]);
        return result;
    }

    /// <summary>
    /// The body of one of the two forms of the operation for a type.
    /// </summary>
    /// <param name="typ">The type of the value</param>
    /// <param name="operands">The name of every operand</param>
    /// <returns>The expression or the statements of the body</returns>
    public string Source(Typ typ, string[] operands)
    {
        var scalar = typ.compType;
        // the absolute value of a value that cannot be negative is the value itself, the BCL does not name it
        if (Name == "abs" && !typ.sig) return operands[0];
        // the body of an integer type is the one it names, an operation that the BCL only has for the floating
        // point types is written out for them
        var body = typ.f || IntBody.Length == 0 ? Body : IntBody;
        if (body.Length == 0) body = ScalarOps.BclExpr(Bcl.Length == 0 ? Name : Bcl);
        if (typ.name == "half" && HalfBody.Length != 0) body = HalfBody;
        return ScalarOps.Expand(scalar, body, operands);
    }
}

/// <summary>
/// The operations of the scalar types. Every one of them is a member of the <c>math</c> class and a member that
/// is called on a value, the names are the ones of the members of the vectors that have the same operation.
/// <para>The body of an operation is the expression of the BCL or the forwarding to another operation of the
/// same layer: the squared distance is the squared length of the difference, the remainder, the smoothstep and
/// the reflection are fused multiply adds. A forwarding never forms a cycle, a scalar function that would reach
/// itself would not end, and a vector that has no accelerated path computes what the function of the <c>math</c>
/// class computes, see <see cref="VectorScalar"/>.</para>
/// </summary>
internal static class ScalarOps
{
    /// <summary>
    /// The expression of the operations that the BCL names, <c>{s}</c> is the type of the value and the other
    /// placeholders are the ones of <see cref="Expand"/>, see the body of an operation of <see cref="All"/>.
    /// </summary>
    private static readonly Dictionary<string, string> Bcl = new()
    {
        { "abs", "{s}.Abs({0})" },
        // the sign of a value is an int where the type of the value is a narrower one
        { "sign", "{c}{s}.Sign({0})" },
        { "min", "{s}.Min({0}, {1})" },
        { "max", "{s}.Max({0}, {1})" },
        { "clamp", "{s}.Clamp({0}, {1}, {2})" },
        { "saturate", "{s}.Clamp({0}, {z}, {o})" },
        { "frac", "{0} - {s}.Floor({0})" },
        { "ceil", "{s}.Ceiling({0})" },
        { "floor", "{s}.Floor({0})" },
        { "round", "{s}.Round({0})" },
        { "trunc", "{s}.Truncate({0})" },
        { "rcp", "{o} / {0}" },
        { "sqrt", "{s}.Sqrt({0})" },
        { "rsqrt", "{o} / {s}.Sqrt({0})" },
        { "log", "{s}.Log({0})" },
        { "log_base", "{s}.Log({0}, {1})" },
        { "log2", "{s}.Log2({0})" },
        { "log10", "{s}.Log10({0})" },
        { "exp", "{s}.Exp({0})" },
        { "exp2", "{s}.Exp2({0})" },
        { "exp10", "{s}.Exp10({0})" },
        { "pow", "{s}.Pow({0}, {1})" },
        { "sin", "{s}.Sin({0})" },
        { "cos", "{s}.Cos({0})" },
        { "sincos", "{s}.SinCos({0})" },
        { "tan", "{s}.Tan({0})" },
        { "asin", "{s}.Asin({0})" },
        { "acos", "{s}.Acos({0})" },
        { "atan", "{s}.Atan({0})" },
        { "atan2", "{s}.Atan2({0}, {1})" },
        { "sinh", "{s}.Sinh({0})" },
        { "cosh", "{s}.Cosh({0})" },
        { "tanh", "{s}.Tanh({0})" },
        { "asinh", "{s}.Asinh({0})" },
        { "acosh", "{s}.Acosh({0})" },
        { "atanh", "{s}.Atanh({0})" },
        { "is_NaN", "{s}.IsNaN({0})" },
        { "is_finite", "{s}.IsFinite({0})" },
        { "is_inf", "{s}.IsInfinity({0})" },
        { "is_pos_inf", "{s}.IsPositiveInfinity({0})" },
        { "is_neg_inf", "{s}.IsNegativeInfinity({0})" },
        { "wrap", "({0} >= {z} ? ({1}) : ({2})) + {0} % ({2} - {1})" },
        // the fused operation of a floating point value is the one of the BCL, the two operations of an integer
        // are only fused by the operand order of the name
        { "fma", "{s}.FusedMultiplyAdd({0}, {1}, {2})" },
        { "fms", "{s}.FusedMultiplyAdd({0}, {1}, -{2})" },
        { "fnma", "{s}.FusedMultiplyAdd(-{0}, {1}, {2})" },
    };

    /// <summary>
    /// The expression of an operation of the BCL, see <see cref="Bcl"/>.
    /// </summary>
    /// <param name="op">The name of the operation</param>
    /// <returns>The expression</returns>
    /// <exception cref="InvalidOperationException">When the operation is not known</exception>
    public static string BclExpr(string op)
    {
        if (Bcl.TryGetValue(op, out var text)) return text;
        throw new InvalidOperationException($"unknown bcl operation {op}");
    }

    /// <summary>
    /// The operations of every type, in the order they are emitted.
    /// <para>The operations that the BCL names are its own members, the ones that it does not name are built
    /// from the ones it does: the remainder is <c>a - b * floor(a / b)</c> with the product fused into a single
    /// rounding and the power of two operations are the ones of <c>BitOperations</c>.</para>
    /// </summary>
    private static readonly ScalarOp[] All =
    [
        #region arithmetic

        // the members of every numeric type
        Op("abs", "a", ["a"]),
        Op("sign", "a", ["a"]),
        Op("min", "a", ["a", "b"], ext: ["other"]),
        Op("max", "a", ["a", "b"], ext: ["other"]),
        Op("clamp", "a", ["a", "min", "max"]),
        Op("lerp", "a", ["start", "end", "t"], self: 2,
            body: "{s}.FusedMultiplyAdd({2}, {1} - {0}, {0})",
            intBody: "{c}({2} * {c}({1} - {0}) + {0})"),
        Op("unlerp", "a", ["a", "start", "end"],
            body: "({0} - {1}) / ({2} - {1})",
            intBody: "{c}(({0} - {1}) / ({2} - {1}))"),
        // the value is placed between the source bounds and then interpolated between the destination ones, it
        // is the unlerp of the value followed by the lerp of the position, see the two operations above
        Op("remap", "a", ["a", "src_start", "src_end", "dst_start", "dst_end"],
            body: "{s}.FusedMultiplyAdd(({0} - {1}) / ({2} - {1}), {4} - {3}, {3})",
            intBody: "{c}({c}(({0} - {1}) / ({2} - {1})) * {c}({4} - {3}) + {3})"),
        Op("square", "a", ["a"], body: "{0} * {0}", intBody: "{c}({0} * {0})"),
        Op("fma", "a", ["a", "b", "c"], intBody: "{c}({0} * {1} + {2})"),
        Op("fms", "a", ["a", "b", "c"], intBody: "{c}({0} * {1} - {2})"),
        Op("fnma", "a", ["a", "b", "c"], intBody: "{c}({2} - {0} * {1})"),
        Op("dot", "a", ["a", "b"], body: "{0} * {1}", intBody: "{c}({0} * {1})", ext: ["other"]),
        Op("length_sq", "a", ["a"], body: "{0} * {0}", intBody: "{c}({0} * {0})"),
        // the squared distance is the squared length of the difference, see the forwarding below
        Op("distance_sq", "a", ["a", "b"],
            body: "math.length_sq({1} - {0})",
            intBody: "math.length_sq({c}({1} - {0}))",
            ext: ["to"]),

        #endregion

        #region integer

        Op("is_pow2", "i", ["a"], returns: "bool", body: "BitOperations.IsPow2({ci}{0})"),
        // the rounding up to a power of two only has unsigned members
        Op("up2pow2", "u", ["a"], body: "{c}BitOperations.RoundUpToPowerOf2({cu}{0})"),

        #endregion

        #region floating point

        // the remainder is the difference of the value and the product of the divisor with the integer below the
        // quotient, the fused multiply add leaves the product exact
        Op("mod", "f", ["a", "b"],
            body: "{s}.FusedMultiplyAdd(-{1}, {s}.Floor({0} / {1}), {0})",
            ext: ["other"]),
        Op("modf", "f", ["a", "out i"], body: "{1} = {s}.Truncate({0});\nreturn {0} - {1};"),
        Op("ceil", "f", ["a"]),
        Op("floor", "f", ["a"]),
        Op("round", "f", ["a"]),
        Op("trunc", "f", ["a"]),
        Op("frac", "f", ["a"]),
        Op("rcp", "f", ["a"]),
        Op("saturate", "f", ["a"]),
        Op("step", "f", ["threshold", "a"], self: 1, body: "{1} >= {0} ? {o} : {z}"),
        Op("smoothstep", "f", ["min", "max", "a"], self: 2,
            body: "var t = {s}.Clamp(({2} - {0}) / ({1} - {0}), {z}, {o});\n" +
                  "return t * t * math.fnma({two}, t, {three});"),
        // the reflection is the value minus twice the product of the normal with the projection of the value
        // onto it, the fused multiply add leaves the product exact
        Op("reflect", "f", ["a", "n"], body: "math.fnma({two} * {1}, {0} * {1}, {0})"),
        Op("project", "f", ["a", "onto"], body: "({0} * {1}) / ({1} * {1}) * {1}"),
        Op("project_safe", "f", ["a", "onto", "default_value = default"],
            body: "var proj = ({0} * {1}) / ({1} * {1}) * {1};\nreturn {s}.IsFinite(proj) ? proj : {2};"),
        Op("project_normalized", "f", ["a", "onto"], body: "({0} * {1}) * {1}"),
        Op("project_on_plane", "f", ["a", "plane_normal"], body: "{0} - ({0} * {1}) / ({1} * {1}) * {1}"),
        Op("project_on_plane_normalized", "f", ["a", "plane_normal"], body: "{0} - ({0} * {1}) * {1}"),
        Op("radians", "f", ["a"],
            body: "{0} * math.{cf}DegToRad",
            halfBody: "(half)((float){0} * math.F_DegToRad)"),
        Op("degrees", "f", ["a"],
            body: "{0} * math.{cf}RadToDeg",
            halfBody: "(half)((float){0} * math.F_RadToDeg)"),
        Op("wrap", "f", ["a", "min", "max"]),

        #endregion

        #region ieee754

        Op("sqrt", "e", ["a"]),
        Op("rsqrt", "e", ["a"]),
        // the length of a value of one component is its absolute value
        Op("length", "e", ["a"], bcl: "abs"),
        Op("distance", "e", ["a", "b"], body: "{s}.Abs({1} - {0})", ext: ["to"]),
        Op("normalize", "e", ["a"], body: "{0} * ({o} / {s}.Sqrt({0} * {0}))"),
        Op("normalize_safe", "e", ["a"], body: "{0} * {0} > {eps} ? {0} * ({o} / {s}.Sqrt({0} * {0})) : {z}"),
        // the refraction direction has no member, the value it is called on would be the third operand
        Op("refract", "e", ["i", "n", "index_of_refraction"], self: -1,
            body: "var ni = {1} * {0};\nvar k = {o} - {2} * {2} * ({o} - ni * ni);\n" +
                  "return k >= {z} ? {2} * {0} - ({2} * ni + {s}.Sqrt(k)) * {1} : {z};"),
        Op("is_NaN", "e", ["a"], returns: "bool"),
        Op("is_finite", "e", ["a"], returns: "bool"),
        Op("is_inf", "e", ["a"], returns: "bool"),
        Op("is_pos_inf", "e", ["a"], returns: "bool"),
        Op("is_neg_inf", "e", ["a"], returns: "bool"),
        Op("log", "e", ["a"]),
        // the base of the logarithm is the second parameter of the same function
        Op("log", "e", ["a", "b"], bcl: "log_base", ext: ["other"]),
        Op("log2", "e", ["a"]),
        Op("log10", "e", ["a"]),
        Op("exp", "e", ["a"]),
        Op("exp2", "e", ["a"]),
        Op("exp10", "e", ["a"]),
        Op("pow", "e", ["a", "b"], ext: ["other"]),
        Op("sin", "e", ["a"]),
        Op("cos", "e", ["a"]),
        Op("tan", "e", ["a"]),
        Op("asin", "e", ["a"]),
        Op("acos", "e", ["a"]),
        Op("atan", "e", ["a"]),
        Op("atan2", "e", ["a", "b"], ext: ["v"]),
        Op("sinh", "e", ["a"]),
        Op("cosh", "e", ["a"]),
        Op("tanh", "e", ["a"]),
        Op("asinh", "e", ["a"]),
        Op("acosh", "e", ["a"]),
        Op("atanh", "e", ["a"]),
        Op("sincos", "e", ["a"], returns: "({s} sin, {s} cos)"),
        Op("sincos", "e", ["a", "out sin", "out cos"], returns: "void",
            body: "({1}, {2}) = {s}.SinCos({0});"),

        #endregion

        #region select

        // the condition is a bool, the declaration of the parameter names its own type, every value can be
        // selected and the member that is called on a value is called on the condition
        Op("select", "s", ["bool c", "t", "f"], body: "{0} ? {1} : {2}"),

        #endregion
    ];

    /// <summary>
    /// The interface of the table that a generator reads, see <see cref="All"/>.
    /// </summary>
    public static IReadOnlyList<ScalarOp> Operations => All;

    private static ScalarOp Op(
        string name,
        string kind,
        string[] pars,
        int self = 0,
        string returns = "",
        string bcl = "",
        string body = "",
        string intBody = "",
        string halfBody = "",
        string[]? ext = null) =>
        new(name, kind, pars, self, returns, bcl, body, intBody, halfBody, ext);

    /// <summary>
    /// True when the type of a value has the operation.
    /// <para><c>a</c> is every numeric type, <c>i</c> is an integer, <c>u</c> is an integer without a sign,
    /// which is the only kind that can be rounded up to a power of two, <c>f</c> is a floating point type and
    /// <c>e</c> is a floating point type as well, the last two only tell the two groups of the members
    /// apart.</para>
    /// </summary>
    /// <param name="op">The operation</param>
    /// <param name="typ">The type of the value</param>
    /// <returns>True when the type has the operation</returns>
    public static bool Matches(ScalarOp op, Typ typ) => op.Kind switch
    {
        "a" or "s" => typ.arith,
        "i" => typ.i,
        "u" => typ.i && !typ.sig,
        "f" or "e" => typ.f,
        _ => false,
    };

    /// <summary>
    /// The name of the region the members of a kind are emitted into.
    /// </summary>
    /// <param name="kind">The kind of the operation</param>
    /// <returns>The name of the region</returns>
    public static string Region(string kind) => kind switch
    {
        "a" => "arithmetic",
        "i" or "u" => "integer",
        "f" => "floating point",
        "e" => "ieee754",
        "s" => "select",
        _ => kind,
    };

    /// <summary>
    /// Expands the body of an operation: the operands of the operation and the parts that depend on the type of
    /// the value.
    /// <para><c>{0}</c> and the ones after it are the operands in the order of the parameters of the operation,
    /// <c>{s}</c> is the type of the value, <c>{z}</c> and <c>{o}</c> are its zero and its one, <c>{two}</c> and
    /// <c>{three}</c> are its two and its three, <c>{eps}</c> is the smallest normal of a floating point type,
    /// <c>{c}</c> is the cast to the type of the value, which the operations that the BCL returns in a wider
    /// type need, <c>{ci}</c> is the cast to the integer the bits of a narrower integer are computed in,
    /// <c>{cu}</c> is the same for the unsigned integer and <c>{cf}</c> is the prefix of the constants of the
    /// type in the <c>math</c> class.</para>
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <param name="body">The body of the operation</param>
    /// <param name="operands">The name of every operand</param>
    /// <returns>The body</returns>
    public static string Expand(string scalar, string body, string[] operands)
    {
        for (var i = 0; i < operands.Length; i++) body = body.Replace($"{{{i}}}", operands[i]);
        return body
            .Replace("{s}", scalar)
            .Replace("{z}", VectorScalar.Zero(scalar))
            .Replace("{o}", VectorScalar.One(scalar))
            .Replace("{two}", Number(scalar, "2"))
            .Replace("{three}", Number(scalar, "3"))
            .Replace("{eps}", Eps(scalar))
            .Replace("{ci}", IntCast(scalar))
            .Replace("{cu}", UnsignedCast(scalar))
            .Replace("{c}", Cast(scalar))
            .Replace("{cf}", scalar == "double" ? "D_" : "F_");
    }

    /// <summary>
    /// The cast to the type of the value, the operations that the BCL returns in a wider type or that are
    /// computed in it need it to reach the type of the member. Only the two widest floating point types are
    /// their own expression type.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <returns>The cast</returns>
    public static string Cast(string scalar) => scalar is "float" or "double" ? "" : $"({scalar})";

    /// <summary>
    /// The cast to the unsigned integer that the bits of a value are counted in: the rounding up to a power of
    /// two only has unsigned members and the one of a value that is narrower than an int is its uint.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <returns>The cast</returns>
    public static string UnsignedCast(string scalar) => scalar is "short" or "ushort" ? "(uint)" : "";

    /// <summary>
    /// The cast to the integer that the bits of a value are computed in: the arithmetic of an integer that is
    /// narrower than an int is done on the int it widens to.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <returns>The cast</returns>
    public static string IntCast(string scalar) => scalar is "short" or "ushort" ? "(int)" : "";

    /// <summary>
    /// The literal of a number of the type of the value. A half has no conversion from an int, its literals are
    /// written with the float suffix, see <see cref="VectorScalar.Zero"/>.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <param name="value">The number</param>
    /// <returns>The literal</returns>
    public static string Number(string scalar, string value) => scalar switch
    {
        "float" => $"{value}f",
        "half" => $"(half){value}f",
        _ => value,
    };

    /// <summary>
    /// The smallest positive normal of the type of the value, the one of a half is below its own range and
    /// becomes its zero.
    /// </summary>
    /// <param name="scalar">The type of the value</param>
    /// <returns>The literal</returns>
    public static string Eps(string scalar) => scalar switch
    {
        "float" => "1.175494351e-38f",
        "half" => "(half)1.175494351e-38f",
        _ => "1.175494351e-38",
    };
}
