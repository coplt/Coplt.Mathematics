using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns a smooth hermite interpolation between zero and one of a component of <paramref name="value"/>
        /// that is in the range of the component of <paramref name="min"/> and the one of
        /// <paramref name="max"/>: the result is zero below the minimum, one above the maximum and a value
        /// between them when the component is in the range
        /// </summary>
        /// <remarks>
        /// It creates a smooth transition between two values, which blends two of them smoothly.
        /// <para>It is the counterpart of the <c>smoothstep</c> intrinsic of hlsl, which computes the value the
        /// same way:
        /// <code>
        /// var t = saturate((value - min) / (max - min));
        /// return t * t * (3 - (2 * t));
        /// </code></para>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-smoothstep"/></para>
        /// </remarks>
        /// <param name="min">The minimum of the range of <paramref name="value"/></param>
        /// <param name="max">The maximum of the range of <paramref name="value"/></param>
        /// <param name="value">The value to interpolate</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value interpolated between the
        /// bounds of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T smoothstep<T>(in T min, in T max, in T value)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
        {
            // the position of the component between the bounds is clamped into the range of zero and one, which
            // is what the position of a component outside of them is as well
            var t = saturate((value - min) / (max - min));
            // the doubled position is taken from the three by the fused member, which keeps the product of it
            // exact
            return t * t * fnma(T.Two, t, T.Three);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.smoothstep{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T smoothstep<T>(this T value, in T min, in T max)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => math.smoothstep(min, max, value);
    }
}
