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
        public static T clamp<T>(in T value, in T min, in T max) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_clamp>(value, min, max);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.clamp{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T clamp<T>(this T value, in T min, in T max) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_clamp>(value, min, max);
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
    internal struct impl_clamp : INumberAlgebraVisitor_Self_Self_Self_Self<impl_clamp>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self_Self<impl_clamp>.AcceptScalar<TScalar>(TScalar a, TScalar b, TScalar c)
            => TScalar.Clamp(a, b, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_clamp>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b, in Vector64<TScalar> c)
            => TVector.FromUnderlying(Vector64.Clamp(a, b, c).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_clamp>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> a, in Vector128<TScalar> b, in Vector128<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector128.Clamp(a, b, c).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_clamp>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> a, in Vector256<TScalar> b, in Vector256<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector256.Clamp(a, b, c).AsByte());
    }
}
