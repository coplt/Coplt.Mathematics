using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math_ex
    {
        extension(math)
        {
            /// <summary>
            /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, <paramref name="t"/> is the
            /// interpolation factor
            /// </summary>
            /// <param name="start">The value at t = 0</param>
            /// <param name="end">The value at t = 1</param>
            /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
            /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
            /// <returns>The interpolated value</returns>
            [OverloadResolutionPriority(-1)]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T lerp<T>(in T start, in T end, in T t) where T : unmanaged, IAlgebraDispatch<T>
                => T.Self<impl_lerp>(start, end, t);
        }
        
        /// <inheritdoc cref="math_ex.lerp{T}(in T, in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(-2)]
        public static T lerp<T>(this T t, in T start, in T end) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_lerp>(start, end, t);

        /// <inheritdoc cref="math_ex.lerp{T}(in T, in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(-2)]
        public static T lerp<T, TScalar>(this TScalar t, in T start, in T end) where T : unmanaged, IAlgebra<T, TScalar>, IAlgebraDispatch<T>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => math.lerp(start, end, T.Broadcast(t));
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value interpolated between two values
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_lerp : IAlgebraVisitor_T_T_T_T<impl_lerp>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_lerp>.Scalar_Number<TScalar>(TScalar start, TScalar end, TScalar t)
            => start + t * (end - start);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_lerp>.Simd_Number<TVector, TScalar>(
            in Vector128<TScalar> start, in Vector128<TScalar> end, in Vector128<TScalar> t
        )
        {
            var offset = end - start;

            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector128.FusedMultiplyAdd(
                    t.AsSingle(), offset.AsSingle(), start.AsSingle()
                ).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.FusedMultiplyAdd(
                    t.AsDouble(), offset.AsDouble(), start.AsDouble()
                ).AsByte());

            return TVector.UnsafeFromUnderlying((start + t * offset).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_lerp>.Simd_Number<TVector, TScalar>(
            in Vector256<TScalar> start, in Vector256<TScalar> end, in Vector256<TScalar> t
        )
        {
            var offset = end - start;

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.FusedMultiplyAdd(
                    t.AsDouble(), offset.AsDouble(), start.AsDouble()
                ).AsByte());

            return TVector.UnsafeFromUnderlying((start + t * offset).AsByte());
        }
    }
}
