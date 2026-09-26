using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Clamps every component into the range of zero and one
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value clamped into the range of zero
        /// and one</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T saturate<T>(in T value) where T : unmanaged, IFloatingPointAlgebra<T>, INumberAlgebraDispatch<T>
            => clamp(value, T.Zero, T.One);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.saturate{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T saturate<T>(this T value) where T : unmanaged, IFloatingPointAlgebra<T>, INumberAlgebraDispatch<T>
            => math.clamp(value, T.Zero, T.One);
    }
}
