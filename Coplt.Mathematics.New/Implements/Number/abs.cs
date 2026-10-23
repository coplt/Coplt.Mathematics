using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the absolute value of every component of the value
        /// <para>The absolute value of the smallest value of a signed whole number kind is not a value of the
        /// kind, which the member of the BCL raises for: the member of the library answers the value itself,
        /// which is the value the negation of it wraps into, and the register of a value answers it as well</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the absolute value of the component of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>(T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_abs>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.abs{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_abs>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The absolute value of a value
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, the value of every other vector reaches the member of the scalar for every
    /// component of it and the value of a matrix reaches it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_abs : IAlgebraVisitor_T_T<impl_abs>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_abs>.Scalar_Number<TScalar>(TScalar value)
        {
            // the absolute value of the smallest value of a signed whole number kind is not a value of the kind,
            // which the member of the BCL raises for: the negation of the value of the kind wraps into the value
            // itself, which the register of a value answers as well
            if (typeof(TScalar) == typeof(short) || typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(long))
                return unchecked(TScalar.IsNegative(value) ? -value : value);
            return TScalar.Abs(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_abs>.Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector128.Abs(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_abs>.Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector256.Abs(vector).AsByte());
    }
}
