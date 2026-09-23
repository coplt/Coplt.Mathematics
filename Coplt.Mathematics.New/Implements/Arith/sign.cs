using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <c>-1</c>, <c>0</c> or <c>1</c> for every component of the value depending on the sign of it
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the sign of the component of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sign<T>(in T value) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_sign>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.sign{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sign<T>([In] this ref T value) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_sign>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The sign of a value
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, the value of every other vector reaches <see cref="Scalar.Sign{T}"/> for every
    /// component of it and the value of a matrix reaches it for every component of every one of its columns</para>
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
