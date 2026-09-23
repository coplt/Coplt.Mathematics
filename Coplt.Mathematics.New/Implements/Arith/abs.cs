using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the absolute value of every component of the value
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the absolute value of the component of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>(in T value) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_abs>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.abs{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>([In] this ref T value) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_abs>(value);
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
    internal struct impl_abs : INumberAlgebraVisitor_Self_Self<impl_abs>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self<impl_abs>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.Abs(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self<impl_abs>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
            => TVector.FromUnderlying(Vector64.Abs(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self<impl_abs>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector128.Abs(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self<impl_abs>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector256.Abs(vector).AsByte());
    }
}
