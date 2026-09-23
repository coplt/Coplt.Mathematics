using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the smaller of the two vectors component by component
        /// </summary>
        /// <param name="a">The vector a</param>
        /// <param name="b">The vector b</param>
        /// <typeparam name="T">The type of the vector</typeparam>
        /// <returns>The component wise minimum</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T min<T>(in T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_min>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.min{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T min<T>([In] this ref T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_min>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The smaller of two values: the registers of the vectors are handed to the member of the visitor that
    /// matches their width and the default members of the visitor reach the member of the scalar for every
    /// component of a value that is handed over as a vector
    /// </summary>
    internal struct impl_min : INumberAlgebraVisitor_Self_Self_Self<impl_min>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self<impl_min>.AcceptScalar<TScalar>(TScalar a, TScalar b)
            => TScalar.Min(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_min>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            => TVector.FromUnderlying(Vector64.Min(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_min>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector128.Min(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_min>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector256.Min(a, b).AsByte());
    }
}
