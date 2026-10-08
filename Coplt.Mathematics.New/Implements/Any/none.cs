using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Holds when the bits of every component of <paramref name="value"/> are all zero
        /// <para>A component of a value holds when the bits of it are not all zero, so the all bits set value of
        /// its kind is the conventional true one and every value of the kind that has a bit set holds just as
        /// well: the value of no component of it holds, which is the complement of the counterpart of it, the
        /// <see cref="math.any{T}(T)"/> of the library.</para>
        /// </summary>
        /// <param name="value">The value to query</param>
        /// <typeparam name="T">The type of the value, which is a vector or a matrix</typeparam>
        /// <returns>True when no component of <paramref name="value"/> holds</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none<T>(T value) where T : unmanaged, IAlgebraDispatch<T>
            => !any(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.none{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => !any(value);
    }
}
