using Coplt.Mathematics.Generics;
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
        public static T sign<T>(in T vector) where T : unmanaged, INumberVector<T>, IDynamicVector<T>
            => T.VisitUnderlyingReturnVector<impl_sign>(vector);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.sign{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sign<T>([In] this ref T vector) where T : unmanaged, INumberVector<T>, IDynamicVector<T>
            => T.VisitUnderlyingReturnVector<impl_sign>(vector);
    }
}

namespace Coplt.Mathematics.Implements
{
    internal struct impl_sign : IVectorUnderlyingVisitorReturnVector, IVectorDimensionVisitorReturnVector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector64<TScalar> vector)
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
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector128<TScalar> vector)
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
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector256<TScalar> vector)
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
        static TVector IVectorUnderlyingVisitorReturnVector.AcceptSoft<TVector, TScalar>(in TVector vector)
            => TVector.VisitDimensionReturnVector<impl_sign>(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector2<TVector, TScalar>(in TVector vector)
        {
            TVector r = default;
            TVector.set_x(ref r, Scalar.Sign(TVector.get_x(vector)));
            TVector.set_y(ref r, Scalar.Sign(TVector.get_y(vector)));
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector3<TVector, TScalar>(in TVector vector)
        {
            TVector r = default;
            TVector.set_x(ref r, Scalar.Sign(TVector.get_x(vector)));
            TVector.set_y(ref r, Scalar.Sign(TVector.get_y(vector)));
            TVector.set_z(ref r, Scalar.Sign(TVector.get_z(vector)));
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector4<TVector, TScalar>(in TVector vector)
        {
            TVector r = default;
            TVector.set_x(ref r, Scalar.Sign(TVector.get_x(vector)));
            TVector.set_y(ref r, Scalar.Sign(TVector.get_y(vector)));
            TVector.set_z(ref r, Scalar.Sign(TVector.get_z(vector)));
            TVector.set_w(ref r, Scalar.Sign(TVector.get_w(vector)));
            return r;
        }
    }
}
