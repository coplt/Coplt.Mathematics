using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the base 2 logarithm of every component
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// <para>The member of the simd library of the register of a value exists for a float and a double, the
        /// value of every other kind of a floating point number, which is a half, has no register and reaches
        /// the member of the component type</para>
        /// <para>The logarithm of the zero of a padding lane is the negative infinity, so the register of the
        /// result is built from the mask of the padding lanes of the value, which keeps them at zero</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the base 2 logarithm of the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T log2<T>(T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_log2>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.log2{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T log2<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_log2>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the base 2 logarithm of the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The base 2 logarithm of the zero of a padding lane is the negative infinity, so the register of
    /// the result is built from the mask of the padding lanes of the value, which keeps them at zero</para>
    /// </summary>
    internal struct impl_log2 : IAlgebraVisitor_T_T<impl_log2>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_log2>.Scalar_Float<TScalar>(TScalar value)
            => log2_ieee754.Log2(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_log2>.Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Log2(vector.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Log2(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_log2>.Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Log2(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// The base 2 logarithm of a floating point number of a kind that a type parameter stands for
    /// <para>A type parameter that the ieee 754 interface constrains reaches the base 2 logarithm of its kind
    /// through the member of the numeric interface as well, which is another member of the same name and the
    /// same parameter list, so the compiler cannot tell the two of them apart. The constraint of the member
    /// below names the only interface of the two that declares the base 2 logarithm of a floating point
    /// number, and a caller passes the type parameter it has to it, which infers the kind</para>
    /// </summary>
    file static class log2_ieee754
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Log2<T>(T value) where T : unmanaged, ILogarithmicFunctions<T> => T.Log2(value);
    }
}
