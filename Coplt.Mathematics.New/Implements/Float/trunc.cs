using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Rounds every component towards zero, so the integral part of it is kept
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value truncated</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T trunc<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_trunc>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.trunc{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T trunc<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_trunc>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is truncated towards zero
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which truncates it with the simd member of the hardware, the value of every
    /// other vector reaches the member of the scalar for every component of it and the value of a matrix reaches
    /// it for every component of every one of its columns. A padding lane holds no component and its bits are
    /// zero, which is what the truncation of it is as well, so the register of the value can be built from it
    /// directly</para>
    /// </summary>
    internal struct impl_trunc : IFloatingPointAlgebraVisitor_Self_Self<impl_trunc>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self<impl_trunc>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.Truncate(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_trunc>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.Truncate(vector.AsSingle()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_trunc>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector128.Truncate(vector.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.Truncate(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_trunc>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.Truncate(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }
    }
}
