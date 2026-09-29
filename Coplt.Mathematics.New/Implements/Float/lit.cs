namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Returns a lighting coefficient vector (ambient, diffuse, specular, 1) of the lighting terms of
    /// <paramref name="n_dot_l"/> and <paramref name="n_dot_h"/>: the ambient one and the last one are one, the
    /// diffuse one is the term of the normal and the light vector clamped below by the zero, and the specular one
    /// is the term of the half angle vector clamped the same way and raised to <paramref name="m"/>, which is zero
    /// when the term of the normal and the light vector is not positive
    /// </summary>
    /// <remarks>
    /// It is the counterpart of the <c>lit</c> intrinsic of hlsl, which computes the vector the same way:
    /// <code>
    /// var d = max(0, n_dot_l);
    /// var h = max(0, n_dot_h);
    /// return new(1, d, n_dot_l > 0 ? pow(h, m) : 0, 1);
    /// </code>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-lit"/></para>
    /// </remarks>
    /// <param name="n_dot_l">The dot product of the normalized surface normal and the light vector</param>
    /// <param name="n_dot_h">The dot product of the half angle vector and the surface normal</param>
    /// <param name="m">The specular exponent</param>
    /// <returns>The lighting coefficient vector</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static half4 lit(Half n_dot_l, Half n_dot_h, Half m)
    {
        var d = Half.MaxNative(Half.Zero, n_dot_l);
        var h = half.MaxNative(Half.Zero, n_dot_h);
        var s = n_dot_l > Half.Zero ? half.Pow(h, m) : Half.Zero;
        return new(Half.One, d, s, Half.One);
    }

    /// <summary>
    /// Returns a lighting coefficient vector (ambient, diffuse, specular, 1) of the lighting terms of
    /// <paramref name="n_dot_l"/> and <paramref name="n_dot_h"/>: the ambient one and the last one are one, the
    /// diffuse one is the term of the normal and the light vector clamped below by the zero, and the specular one
    /// is the term of the half angle vector clamped the same way and raised to <paramref name="m"/>, which is zero
    /// when the term of the normal and the light vector is not positive
    /// </summary>
    /// <remarks>
    /// It is the counterpart of the <c>lit</c> intrinsic of hlsl, which computes the vector the same way:
    /// <code>
    /// var d = max(0, n_dot_l);
    /// var h = max(0, n_dot_h);
    /// return new(1, d, n_dot_l > 0 ? pow(h, m) : 0, 1);
    /// </code>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-lit"/></para>
    /// </remarks>
    /// <param name="n_dot_l">The dot product of the normalized surface normal and the light vector</param>
    /// <param name="n_dot_h">The dot product of the half angle vector and the surface normal</param>
    /// <param name="m">The specular exponent</param>
    /// <returns>The lighting coefficient vector</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 lit(float n_dot_l, float n_dot_h, float m)
    {
        var d = float.MaxNative(0, n_dot_l);
        var h = float.MaxNative(0, n_dot_h);
        var s = n_dot_l > 0 ? float.Pow(h, m) : 0;
        return new(1, d, s, 1);
    }

    /// <summary>
    /// Returns a lighting coefficient vector (ambient, diffuse, specular, 1) of the lighting terms of
    /// <paramref name="n_dot_l"/> and <paramref name="n_dot_h"/>: the ambient one and the last one are one, the
    /// diffuse one is the term of the normal and the light vector clamped below by the zero, and the specular one
    /// is the term of the half angle vector clamped the same way and raised to <paramref name="m"/>, which is zero
    /// when the term of the normal and the light vector is not positive
    /// </summary>
    /// <remarks>
    /// It is the counterpart of the <c>lit</c> intrinsic of hlsl, which computes the vector the same way:
    /// <code>
    /// var d = max(0, n_dot_l);
    /// var h = max(0, n_dot_h);
    /// return new(1, d, n_dot_l > 0 ? pow(h, m) : 0, 1);
    /// </code>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-lit"/></para>
    /// </remarks>
    /// <param name="n_dot_l">The dot product of the normalized surface normal and the light vector</param>
    /// <param name="n_dot_h">The dot product of the half angle vector and the surface normal</param>
    /// <param name="m">The specular exponent</param>
    /// <returns>The lighting coefficient vector</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double4 lit(double n_dot_l, double n_dot_h, double m)
    {
        var d = double.MaxNative(0, n_dot_l);
        var h = double.MaxNative(0, n_dot_h);
        var s = n_dot_l > 0 ? double.Pow(h, m) : 0;
        return new(1, d, s, 1);
    }
}
