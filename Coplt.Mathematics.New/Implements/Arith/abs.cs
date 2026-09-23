using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the absolute value of every component
        /// </summary>
        /// <param name="vector">The vector</param>
        /// <typeparam name="T">The type of the vector</typeparam>
        /// <returns>The absolute value of the vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>(in T vector) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_abs>(vector);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.abs{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>([In] this ref T vector) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_abs>(vector);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The absolute value of a value: the register of a vector is handed to the member of the visitor that
    /// matches the width of it and the default members of the visitor reach the member of the scalar for every
    /// component of a value that is handed over as a vector
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
