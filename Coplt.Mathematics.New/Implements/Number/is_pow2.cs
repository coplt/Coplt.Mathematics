using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the all bits set value of the kind of a component at every component of <paramref name="a"/>
        /// that is a power of two and the zero of it at every one that is not
        /// <para>The check of a component is the one of the framework for the kind of it: a floating point component
        /// holds when it is a positive finite power of two, so a subnormal that has a single bit holds and one that
        /// has more of them does not, and an integer component holds when it is not negative</para>
        /// </summary>
        /// <param name="a">The value whose every component is checked</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the mask of the check of the component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T is_pow2<T>(in T a) where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>
            => T.Self<impl_is_pow2>(a);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The check of a power of two of a value
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches the
    /// width of the register, the value of every other vector reaches the member of the scalar for every component
    /// of it and the value of a matrix reaches it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_is_pow2 : IAlgebraVisitor_T_T<impl_is_pow2>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_is_pow2>.Scalar_Number<TScalar>(TScalar value)
            => TScalar.IsPow2(value) ? TScalar.AllBitsSet : TScalar.Zero;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_is_pow2>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(long))
                return TVector.UnsafeFromUnderlying(
                    (Vector128.Equals((vector & (vector - Vector128<TScalar>.One)), default)
                     & Vector128.GreaterThan(vector, default)
                    ).AsByte());
            if (typeof(TScalar) == typeof(uint) || typeof(TScalar) == typeof(ulong))
                return TVector.UnsafeFromUnderlying(
                    (Vector128.Equals((vector & (vector - Vector128<TScalar>.One)), default)
                     & ~Vector128.Equals(vector, default)
                    ).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_is_pow2>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(long))
                return TVector.UnsafeFromUnderlying(
                    (Vector256.Equals((vector & (vector - Vector256<TScalar>.One)), default)
                     & Vector256.GreaterThan(vector, default)
                    ).AsByte());
            if (typeof(TScalar) == typeof(uint) || typeof(TScalar) == typeof(ulong))
                return TVector.UnsafeFromUnderlying(
                    (Vector256.Equals((vector & (vector - Vector256<TScalar>.One)), default)
                     & ~Vector256.Equals(vector, default)
                    ).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_is_pow2>.Simd_Float<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(simd.IsPow2(vector.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.IsPow2(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_is_pow2>.Simd_Float<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.IsPow2(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }
    }
}
