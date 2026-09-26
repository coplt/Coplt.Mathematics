using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the fractional part of every component, it is the same as the remainder of the division of the
        /// component by one
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value above the floor of it, so the
        /// fraction of a component that is negative is the value of it above the floor as well and it is never
        /// negative</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T frac<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => value - floor(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.frac{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T frac<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => value - math.floor(value);
    }
}
