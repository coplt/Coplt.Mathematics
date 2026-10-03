using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the inverse hyperbolic cosine of every component
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
        /// <returns>The value whose every component is the inverse hyperbolic cosine of the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T acosh<T>(T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_acosh>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.acosh{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T acosh<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_acosh>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the inverse hyperbolic cosine of the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The inverse hyperbolic cosine of the zero of a padding lane is not a number, so the register of the
    /// result is built from the mask of the padding lanes of the value, which keeps them at zero</para>
    /// </summary>
    internal struct impl_acosh : IAlgebraVisitor_T_T<impl_acosh>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_acosh>.Scalar_Float<TScalar>(TScalar value)
            => TScalar.Acosh(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_acosh>.Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Acosh(vector.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Acosh(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_acosh>.Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Acosh(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }
}
