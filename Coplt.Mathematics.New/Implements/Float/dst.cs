namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Calculates the distance vector of <paramref name="a"/> and <paramref name="b"/>, which is one in the first
    /// component, the product of the second components of the two in the second one, the third component of
    /// <paramref name="a"/> in the third one and the fourth component of <paramref name="b"/> in the fourth one
    /// </summary>
    /// <remarks>
    /// It is the counterpart of the <c>dst</c> intrinsic of hlsl, which provides the same functionality as the
    /// <c>dst</c> instruction of a vertex shader and computes the vector the same way:
    /// <code>
    /// return new(1, a.y * b.y, a.z, b.w);
    /// </code>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dst"/></para>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dst---vs"/></para>
    /// </remarks>
    /// <param name="a">The first vector</param>
    /// <param name="b">The second vector</param>
    /// <returns>The calculated distance vector</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static half4 dst(in half4 a, in half4 b) => new(
        Half.One,
        a.y * b.y,
        a.z,
        b.w
    );

    /// <summary>
    /// Calculates the distance vector of <paramref name="a"/> and <paramref name="b"/>, which is one in the first
    /// component, the product of the second components of the two in the second one, the third component of
    /// <paramref name="a"/> in the third one and the fourth component of <paramref name="b"/> in the fourth one
    /// </summary>
    /// <remarks>
    /// It is the counterpart of the <c>dst</c> intrinsic of hlsl, which provides the same functionality as the
    /// <c>dst</c> instruction of a vertex shader and computes the vector the same way:
    /// <code>
    /// return new(1, a.y * b.y, a.z, b.w);
    /// </code>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dst"/></para>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dst---vs"/></para>
    /// </remarks>
    /// <param name="a">The first vector</param>
    /// <param name="b">The second vector</param>
    /// <returns>The calculated distance vector</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 dst(in float4 a, in float4 b) => new(
        1,
        a.y * b.y,
        a.z,
        b.w
    );

    /// <summary>
    /// Calculates the distance vector of <paramref name="a"/> and <paramref name="b"/>, which is one in the first
    /// component, the product of the second components of the two in the second one, the third component of
    /// <paramref name="a"/> in the third one and the fourth component of <paramref name="b"/> in the fourth one
    /// </summary>
    /// <remarks>
    /// It is the counterpart of the <c>dst</c> intrinsic of hlsl, which provides the same functionality as the
    /// <c>dst</c> instruction of a vertex shader and computes the vector the same way:
    /// <code>
    /// return new(1, a.y * b.y, a.z, b.w);
    /// </code>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dst"/></para>
    /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dst---vs"/></para>
    /// </remarks>
    /// <param name="a">The first vector</param>
    /// <param name="b">The second vector</param>
    /// <returns>The calculated distance vector</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double4 dst(in double4 a, in double4 b) => new(
        1,
        a.y * b.y,
        a.z,
        b.w
    );
}
