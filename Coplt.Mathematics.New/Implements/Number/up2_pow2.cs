using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the power of two of the kind of a component that is not less than the component of
        /// <paramref name="a"/> and the smallest one of them at every component of it
        /// <para>The rounding of a component is the one of the framework for the kind of it: the rounding of an
        /// integer is done in the bits of it, which a value whose rounding is above the kind overflows to the zero
        /// of it, and the rounding of a floating point value is the power of two that is not less than it, which a
        /// subnormal reaches as well</para>
        /// </summary>
        /// <param name="a">The value whose every component is rounded up</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the power of two that is not less than it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T up2_pow2<T>(T a) where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>
            => T.Self<impl_up2_pow2>(a);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The rounding up to the next power of two of a value
    /// </summary>
    internal struct impl_up2_pow2 : IAlgebraVisitor_T_T<impl_up2_pow2>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_up2_pow2>.Scalar_Number<TScalar>(TScalar value)
        {
            if (typeof(TScalar) == typeof(ushort))
                return (TScalar)(object)(ushort)BitOperations.RoundUpToPowerOf2((ushort)(object)value);
            if (typeof(TScalar) == typeof(short))
                return (TScalar)(object)(short)BitOperations.RoundUpToPowerOf2((uint)(short)(object)value);
            if (typeof(TScalar) == typeof(uint))
                return (TScalar)(object)BitOperations.RoundUpToPowerOf2((uint)(object)value);
            if (typeof(TScalar) == typeof(int))
                return (TScalar)(object)(int)BitOperations.RoundUpToPowerOf2((uint)(int)(object)value);
            if (typeof(TScalar) == typeof(ulong))
                return (TScalar)(object)BitOperations.RoundUpToPowerOf2((ulong)(object)value);
            if (typeof(TScalar) == typeof(long))
                return (TScalar)(object)(long)BitOperations.RoundUpToPowerOf2((ulong)(long)(object)value);
            if (typeof(TScalar) == typeof(half))
                return (TScalar)(object)(half)simd.RoundUpToPowerOf2(Vector128.CreateScalarUnsafe((float)(half)(object)value).AsSingle()).ToScalar();
            if (typeof(TScalar) == typeof(float))
                return simd.RoundUpToPowerOf2(Vector128.CreateScalarUnsafe(value).AsSingle()).As<float, TScalar>().ToScalar();
            if (typeof(TScalar) == typeof(double))
                return simd.RoundUpToPowerOf2(Vector128.CreateScalarUnsafe(value).AsDouble()).As<double, TScalar>().ToScalar();
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_up2_pow2>.Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(uint) || typeof(TScalar) == typeof(int))
                return TVector.FromUnderlying(simd.RoundUpToPowerOf2(vector.AsUInt32()).AsByte());
            if (typeof(TScalar) == typeof(ulong) || typeof(TScalar) == typeof(long))
                return TVector.FromUnderlying(simd.RoundUpToPowerOf2(vector.AsUInt64()).AsByte());
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.RoundUpToPowerOf2(vector.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.RoundUpToPowerOf2(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_up2_pow2>.Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(ulong) || typeof(TScalar) == typeof(long))
                return TVector.FromUnderlying(simd.RoundUpToPowerOf2(vector.AsUInt64()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.RoundUpToPowerOf2(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }
    }
}
