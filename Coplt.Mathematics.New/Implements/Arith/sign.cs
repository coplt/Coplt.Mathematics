using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <c>-1</c>, <c>0</c> or <c>1</c> for every component depending on its sign
        /// </summary>
        /// <param name="vector">The vector</param>
        /// <returns>The sign of every component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sign<T>(in T vector) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_sign>(vector);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.sign{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sign<T>([In] this ref T vector) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_sign>(vector);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The sign of a value: the register of a vector is handed to the helper that matches its width and the
    /// default members of the visitor reach <see cref="Scalar.Sign{T}"/> for every component of a value that is
    /// handed over as a vector
    /// </summary>
    internal struct impl_sign : INumberAlgebraVisitor_Self_Self<impl_sign>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self<impl_sign>.AcceptScalar<TScalar>(TScalar value)
            => Scalar.Sign(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self<impl_sign>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float) || typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.SignFloat(vector).AsByte());
            if (typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(long))
                return TVector.FromUnderlying(simd.SignInt(vector).AsByte());
            if (typeof(TScalar) == typeof(uint) || typeof(TScalar) == typeof(ulong))
                return TVector.FromUnderlying(simd.SignUInt(vector).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self<impl_sign>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float) || typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.SignFloat(vector).AsByte());
            if (typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(long))
                return TVector.FromUnderlying(simd.SignInt(vector).AsByte());
            if (typeof(TScalar) == typeof(uint) || typeof(TScalar) == typeof(ulong))
                return TVector.FromUnderlying(simd.SignUInt(vector).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self<impl_sign>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float) || typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.SignFloat(vector).AsByte());
            if (typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(long))
                return TVector.FromUnderlying(simd.SignInt(vector).AsByte());
            if (typeof(TScalar) == typeof(uint) || typeof(TScalar) == typeof(ulong))
                return TVector.FromUnderlying(simd.SignUInt(vector).AsByte());
            throw new NotSupportedException();
        }
    }
}
