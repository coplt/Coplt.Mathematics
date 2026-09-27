using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the reciprocal of the square root of every component, which is the estimate of the hardware
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact reciprocal of the square root of it takes <see cref="rsqrt{T}"/>, which
        /// divides the one of the kind of the value by the square root of it</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the estimate of the reciprocal of the square root of
        /// the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T rsqrt_a<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_rsqrt_approx>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.rsqrt_a{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T rsqrt_a<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_rsqrt_approx>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the estimate of the hardware of the reciprocal of the square root of
    /// the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the estimate of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a caller
    /// that needs the exact reciprocal of the square root of a component takes <see cref="math.rsqrt{T}"/>, which divides the one of
    /// the kind of it by the square root of it</para>
    /// <para>The reciprocal of the square root of a padding lane is an infinity, so the register of the result
    /// is built from the mask of the padding lanes of the value, which keeps them at zero</para>
    /// </summary>
    internal struct impl_rsqrt_approx : IFloatingPointAlgebraVisitor_Self_Self<impl_rsqrt_approx>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self<impl_rsqrt_approx>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.ReciprocalSqrtEstimate(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_rsqrt_approx>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.RSqrt(vector.AsSingle()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_rsqrt_approx>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.RSqrt(vector.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.RSqrt(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_rsqrt_approx>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.RSqrt(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }
}
