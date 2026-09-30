namespace Coplt.Mathematics;

public static partial class math
{
    #region fma

    /// <summary>
    /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the addition of
    /// <paramref name="c"/>: <code>(a * b) + c</code>
    /// <para>It is the fused multiply and add of the kind of the value, which rounds the multiplication and the
    /// addition of it once, and the members of a value of the library reach it as the one of the kind of a
    /// single component of it</para>
    /// </summary>
    /// <param name="a">The value that is multiplied</param>
    /// <param name="b">The value that multiplies <paramref name="a"/></param>
    /// <param name="c">The value that is added to the product</param>
    /// <typeparam name="T">The type of the value, a floating point number</typeparam>
    /// <returns>The fused result</returns>
    public static T fma<T>(T a, T b, T c) where T : IFloatingPointIeee754<T> =>
        T.FusedMultiplyAdd(a, b, c);

    /// <inheritdoc cref="fma{T}(T, T, T)"/>
    public static T mad<T>(T a, T b, T c) where T : IFloatingPointIeee754<T> =>
        T.FusedMultiplyAdd(a, b, c);

    /// <summary>
    /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the addition of
    /// <paramref name="c"/>, the operands are named in the order of the addition: <code>c + (a * b)</code>
    /// </summary>
    /// <param name="c">The value that is added to the product</param>
    /// <param name="a">The value that is multiplied</param>
    /// <param name="b">The value that multiplies <paramref name="a"/></param>
    /// <typeparam name="T">The type of the value, a floating point number</typeparam>
    /// <returns>The fused result</returns>
    public static T fam<T>(T c, T a, T b) where T : IFloatingPointIeee754<T> =>
        T.FusedMultiplyAdd(a, b, c);

    /// <summary>
    /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the subtraction of
    /// <paramref name="c"/>: <code>(a * b) - c</code>
    /// </summary>
    /// <param name="a">The value that is multiplied</param>
    /// <param name="b">The value that multiplies <paramref name="a"/></param>
    /// <param name="c">The value that is subtracted from the product</param>
    /// <typeparam name="T">The type of the value, a floating point number</typeparam>
    /// <returns>The fused result</returns>
    public static T fms<T>(T a, T b, T c) where T : IFloatingPointIeee754<T> =>
        T.FusedMultiplyAdd(a, b, -c);

    /// <summary>
    /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the subtraction of it
    /// from <paramref name="c"/>, the operands are named in the order of the subtraction:
    /// <code>c - (a * b)</code>
    /// </summary>
    /// <param name="c">The value that the product is subtracted from</param>
    /// <param name="a">The value that is multiplied</param>
    /// <param name="b">The value that multiplies <paramref name="a"/></param>
    /// <typeparam name="T">The type of the value, a floating point number</typeparam>
    /// <returns>The fused result</returns>
    public static T fsm<T>(T c, T a, T b) where T : IFloatingPointIeee754<T> =>
        T.FusedMultiplyAdd(-a, b, c);

    /// <summary>
    /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the subtraction of it
    /// from <paramref name="c"/>: <code>c - (a * b)</code> or <code>-(a * b) + c</code>
    /// </summary>
    /// <param name="a">The value that is multiplied</param>
    /// <param name="b">The value that multiplies <paramref name="a"/></param>
    /// <param name="c">The value that the product is subtracted from</param>
    /// <typeparam name="T">The type of the value, a floating point number</typeparam>
    /// <returns>The fused result</returns>
    public static T fnma<T>(T a, T b, T c) where T : IFloatingPointIeee754<T> =>
        T.FusedMultiplyAdd(-a, b, c);

    /// <summary>
    /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the addition of
    /// <paramref name="c"/> and negates the result: <code>-(a * b) - c</code> or <code>-(a * b + c)</code>
    /// </summary>
    /// <param name="a">The value that is multiplied</param>
    /// <param name="b">The value that multiplies <paramref name="a"/></param>
    /// <param name="c">The value that is added to the product before the result is negated</param>
    /// <typeparam name="T">The type of the value, a floating point number</typeparam>
    /// <returns>The fused result</returns>
    public static T fnms<T>(T a, T b, T c) where T : IFloatingPointIeee754<T> =>
        -T.FusedMultiplyAdd(a, b, c);

    #endregion
}
