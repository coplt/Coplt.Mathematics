using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Clamps every component of the value to the inclusive range of <paramref name="min"/> and
        /// <paramref name="max"/>
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">The lower bound of every component</param>
        /// <param name="max">The upper bound of every component</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is clamped to the range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T clamp<T>(T value, T min, T max) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_clamp>(value, min, max);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.clamp{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T clamp<T>(this T value, T min, T max) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_clamp>(value, min, max);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value that is clamped to the range of two values
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_clamp : IAlgebraVisitor_T_T_T_T<impl_clamp>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_clamp>.Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c)
            => TScalar.Clamp(a, b, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_clamp>.Simd_Number<TVector, TScalar>(
            Vector128<TScalar> a, Vector128<TScalar> b, Vector128<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector128.Clamp(a, b, c).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_clamp>.Simd_Number<TVector, TScalar>(
            Vector256<TScalar> a, Vector256<TScalar> b, Vector256<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector256.Clamp(a, b, c).AsByte());
    }
}
