using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns every component of <paramref name="a"/> raised to the power of the matching component of
        /// <paramref name="b"/>
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// <para>The member of the simd library of the register of a value exists for a float and a double,
        /// the value of every other kind of a floating point number, which is a half, has no register and
        /// reaches the member of the component type</para>
        /// </summary>
        /// <param name="a">The value</param>
        /// <param name="b">The exponent of every component</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value raised to the power of the
        /// matching component of the exponent</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T pow<T>(T a, T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_pow>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.pow{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T pow<T>(this T a, T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_pow>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the component of it raised to the power of the matching component of
    /// the exponent
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it and the value of a matrix reaches it for every
    /// component of every one of its columns</para>
    /// <para>The power of the zero of a padding lane is the one of the kind of the component when the exponent
    /// lane is zero as well, so the register of the result is built from the mask of the padding lanes of the
    /// value, which keeps them at zero</para>
    /// </summary>
    internal struct impl_pow : IAlgebraVisitor_T_T_T<impl_pow>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_pow>.Scalar_Float<TScalar>(TScalar a, TScalar b)
            => TScalar.Pow(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_pow>.Simd_Float<TVector, TScalar>(
            Vector128<TScalar> a, Vector128<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Pow(a.AsSingle(), b.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Pow(a.AsDouble(), b.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_pow>.Simd_Float<TVector, TScalar>(
            Vector256<TScalar> a, Vector256<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Pow(a.AsDouble(), b.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }
}
