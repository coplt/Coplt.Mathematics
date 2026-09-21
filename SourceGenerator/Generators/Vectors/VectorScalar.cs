using System;
using System.Collections.Generic;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// The expression of an operation on a single component. The BCL exposes the members of the numeric interfaces
/// as the static members of the numeric types themselves, so the operation of a component is a call of the type
/// of the component: <c>float.Sqrt(x)</c>, <c>half.Log(x)</c> and <c>int.Clamp(x, min, max)</c>. An operation
/// that the numeric interfaces do not name is built from the ones they do: the remainder is
/// <c>x - y * floor(x / y)</c> and the reciprocal is <c>one / x</c>.
/// </summary>
internal static class VectorScalar
{
    /// <summary>
    /// The expression of every operation, <c>{s}</c> is the type of the component and <c>{0}</c>, <c>{1}</c> and
    /// <c>{2}</c> are the operands of the operation in the order of the interface member.
    /// </summary>
    private static readonly Dictionary<string, string> Templates = new()
    {
        { "abs", "{s}.Abs({0})" },
        { "sign", "{s}.Sign({0})" },
        { "min", "{s}.Min({0}, {1})" },
        { "max", "{s}.Max({0}, {1})" },
        { "clamp", "{s}.Clamp({0}, {1}, {2})" },
        { "saturate", "{s}.Clamp({0}, {z}, {o})" },
        { "mod", "{0} - {1} * {s}.Floor({0} / {1})" },
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
        { "fma", "{s}.FusedMultiplyAdd({0}, {1}, {2})" },
        { "fms", "{s}.FusedMultiplyAdd({0}, {1}, -{2})" },
        { "fnma", "{s}.FusedMultiplyAdd(-{0}, {1}, {2})" },
    };

    /// <summary>
    /// Returns the expression of the operation <paramref name="op"/> on the operands of <paramref name="args"/>.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <param name="op">The name of the operation</param>
    /// <param name="args">The operand of every parameter of the operation</param>
    /// <returns>The expression</returns>
    /// <exception cref="InvalidOperationException">When the operation is not known</exception>
    public static string Expr(string scalar, string op, params string[] args)
    {
        // the absolute value of a value that cannot be negative is the value itself, the BCL does not expose the
        // member of an unsigned type
        if (op == "abs" && !Signed(scalar)) return args[0];
        if (!Templates.TryGetValue(op, out var template))
            throw new InvalidOperationException($"unknown scalar operation {op}");
        var text = template
            .Replace("{s}", scalar)
            .Replace("{z}", Zero(scalar))
            .Replace("{o}", One(scalar));
        for (var i = 0; i < args.Length; i++) text = text.Replace($"{{{i}}}", args[i]);
        return text;
    }

    /// <summary>
    /// Returns the unsigned form of an integer component that is narrower than an int. The shift of a narrower
    /// value is done on the bits of the int it widens to, so the value has to reach its unsigned form first for
    /// the bits above it to stay out of the result.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <param name="x">The expression of the component</param>
    /// <returns>The expression of the unsigned form</returns>
    public static string Unsigned(string scalar, string x) => scalar switch
    {
        "sbyte" => $"(byte){x}",
        "short" => $"(ushort){x}",
        _ => x,
    };

    /// <summary>
    /// True when the value of the component type can be negative, so it has an absolute value and a sign.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <returns>True when the type is signed</returns>
    public static bool Signed(string scalar) => scalar is "short" or "int" or "long" or "float" or "double" or "half";

    /// <summary>
    /// Returns the literal of zero of the component type. A half has no conversion from an int and only has
    /// conversions from the other numeric types, so its literals are written with the float suffix.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <returns>The literal</returns>
    public static string Zero(string scalar) => scalar switch
    {
        "float" => "0f",
        "half" => "(half)0f",
        _ => "0",
    };

    /// <summary>
    /// Returns the literal of one of the component type, see <see cref="Zero"/>.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <returns>The literal</returns>
    public static string One(string scalar) => scalar switch
    {
        "float" => "1f",
        "half" => "(half)1f",
        _ => "1",
    };

    /// <summary>
    /// Returns the value of a component as its bits, a component that is an integer already is its bits and the
    /// bits of a floating point component are reached through the bit conversions of the BCL.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <param name="x">The expression of the component</param>
    /// <returns>The expression of the bits</returns>
    public static string Bits(string scalar, string x) => scalar switch
    {
        "float" => $"BitConverter.SingleToUInt32Bits({x})",
        "double" => $"BitConverter.DoubleToUInt64Bits({x})",
        "half" => $"BitConverter.HalfToUInt16Bits({x})",
        _ => x,
    };

    /// <summary>
    /// Returns the component of the bits of <paramref name="x"/>, the inverse of <see cref="Bits"/>.
    /// </summary>
    /// <param name="scalar">The type of a single component of the vector</param>
    /// <param name="x">The expression of the bits</param>
    /// <returns>The expression of the component</returns>
    public static string FromBits(string scalar, string x) => scalar switch
    {
        "float" => $"BitConverter.UInt32BitsToSingle({x})",
        "double" => $"BitConverter.UInt64BitsToDouble({x})",
        // the bits of a half are narrower than an int, an operation on them is widened by the language
        "half" => $"BitConverter.UInt16BitsToHalf((ushort)({x}))",
        _ => x,
    };
}
