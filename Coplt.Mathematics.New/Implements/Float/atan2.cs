using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the arc tangent of the quotient of the two values, the signs of both of them are used to find
        /// the quadrant of the result
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// <para>The member of the simd library of the register of a value exists for a float and a double, the
        /// value of every other kind of a floating point number, which is a half, has no register and reaches
        /// the member of the component type</para>
        /// </summary>
        /// <param name="a">The numerator of every component</param>
        /// <param name="b">The divisor of every component</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the arc tangent of the quotient of the two matching
        /// components in radians</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T atan2<T>(T a, T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_atan2>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.atan2{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T atan2<T>(this T a, T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_atan2>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the arc tangent of the quotient of the two matching components
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The arc tangent of the two zeroes of a padding lane is the zero of the kind of the component, so
    /// the result of a register that is wider than the value is taken from the value without masking the
    /// padding lanes of it</para>
    /// </summary>
    internal struct impl_atan2 : IAlgebraVisitor_T_T_T<impl_atan2>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_atan2>.Scalar_Float<TScalar>(TScalar a, TScalar b)
            => TScalar.Atan2(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_atan2>.Simd_Float<TVector, TScalar>(
            Vector128<TScalar> a, Vector128<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(simd.Atan2(a.AsSingle(), b.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Atan2(a.AsDouble(), b.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_atan2>.Simd_Float<TVector, TScalar>(
            Vector256<TScalar> a, Vector256<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Atan2(a.AsDouble(), b.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }
}
