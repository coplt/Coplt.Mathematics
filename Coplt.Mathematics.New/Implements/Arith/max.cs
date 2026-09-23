using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the larger of the two vectors component by component
        /// </summary>
        /// <param name="a">The vector a</param>
        /// <param name="b">The vector b</param>
        /// <typeparam name="T">The type of the vector</typeparam>
        /// <returns>The component wise maximum</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T max<T>(in T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_max>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.max{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T max<T>([In] this ref T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_max>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The larger of two values: the registers of the vectors are handed to the member of the visitor that
    /// matches their width and the default members of the visitor reach the member of the scalar for every
    /// component of a value that is handed over as a vector
    /// </summary>
    internal struct impl_max : INumberAlgebraVisitor_Self_Self_Self<impl_max>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self<impl_max>.AcceptScalar<TScalar>(TScalar a, TScalar b)
            => TScalar.Max(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_max>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            => TVector.FromUnderlying(Vector64.Max(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_max>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector128.Max(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_max>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector256.Max(a, b).AsByte());
    }
}
