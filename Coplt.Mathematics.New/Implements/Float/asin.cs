using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the arc sine of every component, the result is in radians
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// <para>The member of the simd library of the register of a value exists for a float and a double, the
        /// value of every other kind of a floating point number, which is a half, has no register and reaches
        /// the member of the component type</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the arc sine of the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T asin<T>(T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_asin>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.asin{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T asin<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_asin>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the arc sine of the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The arc sine of the zero of a padding lane is the zero of the kind of the component, so the result
    /// of a register that is wider than the value is taken from the value without masking the padding lanes of
    /// it</para>
    /// </summary>
    internal struct impl_asin : IAlgebraVisitor_T_T<impl_asin>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_asin>.Scalar_Float<TScalar>(TScalar value)
            => TScalar.Asin(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_asin>.Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(simd.Asin(vector.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Asin(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_asin>.Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Asin(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }
}
