using System;
using System.Collections.Generic;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// The expression of an operation on a single component. The scalar types have the same operation as their
/// members of the <c>math</c> class, see <see cref="ScalarOps"/>, so the scalar fallback of a component wise
/// operation is the call of the function of the <c>math</c> class: <c>math.sqrt(x)</c>, <c>math.abs(x)</c> and
/// <c>math.clamp(x, min, max)</c>. The scalar functions are the single place that defines what an operation
/// computes, a vector that no hardware supports computes the same values as a single value does.
/// </summary>
internal static class VectorScalar
{
    /// <summary>
    /// The expression of every operation, <c>{0}</c>, <c>{1}</c> and <c>{2}</c> are the operands of the
    /// operation in the order of the interface member.
    /// </summary>
    private static readonly Dictionary<string, string> Templates = new()
    {
        { "abs", "math.abs({0})" },
        { "sign", "math.sign({0})" },
        { "min", "math.min({0}, {1})" },
        { "max", "math.max({0}, {1})" },
        { "clamp", "math.clamp({0}, {1}, {2})" },
        { "sqrt", "math.sqrt({0})" },
        { "rsqrt", "math.rsqrt({0})" },
        { "log", "math.log({0})" },
        // the base of the logarithm is the second parameter of the same function
        { "log_base", "math.log({0}, {1})" },
        { "log2", "math.log2({0})" },
        { "log10", "math.log10({0})" },
        { "exp", "math.exp({0})" },
        { "exp2", "math.exp2({0})" },
        { "exp10", "math.exp10({0})" },
        { "pow", "math.pow({0}, {1})" },
        { "sin", "math.sin({0})" },
        { "cos", "math.cos({0})" },
        { "sincos", "math.sincos({0})" },
        { "tan", "math.tan({0})" },
        { "asin", "math.asin({0})" },
        { "acos", "math.acos({0})" },
        { "atan", "math.atan({0})" },
        { "atan2", "math.atan2({0}, {1})" },
        { "sinh", "math.sinh({0})" },
        { "cosh", "math.cosh({0})" },
        { "tanh", "math.tanh({0})" },
        { "asinh", "math.asinh({0})" },
        { "acosh", "math.acosh({0})" },
        { "atanh", "math.atanh({0})" },
        { "is_NaN", "math.is_NaN({0})" },
        { "is_finite", "math.is_finite({0})" },
        { "is_inf", "math.is_inf({0})" },
        { "is_pos_inf", "math.is_pos_inf({0})" },
        { "is_neg_inf", "math.is_neg_inf({0})" },
        { "wrap", "math.wrap({0}, {1}, {2})" },
        { "fma", "math.fma({0}, {1}, {2})" },
        { "fms", "math.fms({0}, {1}, {2})" },
        { "fnma", "math.fnma({0}, {1}, {2})" },
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
        if (!Templates.TryGetValue(op, out var text))
            throw new InvalidOperationException($"unknown scalar operation {op}");
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
