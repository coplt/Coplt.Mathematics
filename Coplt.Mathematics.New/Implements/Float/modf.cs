using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Splits the value into its integral part and its fractional part
        /// <para>It stores the integral part into <paramref name="integer_portion"/> and returns the fractional
        /// part, which keeps the sign of the value, which the fraction of <c>frac</c> does not</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="integer_portion">Receives the integral part</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The fractional part</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T modf<T>(in T value, out T integer_portion)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
        {
            var d = value;
            var i = trunc(d);
            integer_portion = i;
            return d - i;
        }

        /// <summary>
        /// Splits the value into its integral part and its fractional part
        /// <para>The fractional part keeps the sign of the value and the integral part is the value above it, so
        /// the two of them add up to the value</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The fractional part of the value and the integral part of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T SignedFractionalPortion, T IntegerPortion) modf<T>(in T value)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
        {
            var d = value;
            var i = trunc(d);
            return (d - i, i);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.modf{T}(in T, out T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T modf<T>(this T value, out T integer_portion)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => math.modf(value, out integer_portion);

        /// <inheritdoc cref="math.modf{T}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T SignedFractionalPortion, T IntegerPortion) modf<T>(this T value)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => math.modf(value);
    }
}
