using Coplt.Mathematics.Generics;
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
        public static T abs<T>(in T vector) where T : unmanaged, INumberVector<T>, IDynamicVector<T>
            => T.VisitUnderlyingReturnVector<impl_abs>(vector);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.abs{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T abs<T>([In] this ref T vector) where T : unmanaged, INumberVector<T>, IDynamicVector<T>
            => T.VisitUnderlyingReturnVector<impl_abs>(vector);
    }
}

namespace Coplt.Mathematics.Implements
{
    internal struct impl_abs : IVectorUnderlyingVisitorReturnVector, IVectorDimensionVisitorReturnVector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector64<TScalar> vector)
            => TVector.FromUnderlying(Vector64.Abs(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector128.Abs(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector256.Abs(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitorReturnVector.AcceptSoft<TVector, TScalar>(in TVector vector)
            => TVector.VisitDimensionReturnVector<impl_abs>(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector2<TVector, TScalar>(in TVector vector)
        {
            if (typeof(TScalar) == typeof(byte)
                || typeof(TScalar) == typeof(ushort)
                || typeof(TScalar) == typeof(uint)
                || typeof(TScalar) == typeof(ulong)
                || typeof(TScalar) == typeof(nuint))
            {
                return vector;
            }

            TVector r = default;
            TVector.set_x(ref r, TScalar.Abs(TVector.get_x(vector)));
            TVector.set_y(ref r, TScalar.Abs(TVector.get_y(vector)));
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector3<TVector, TScalar>(in TVector vector)
        {
            if (typeof(TScalar) == typeof(byte)
                || typeof(TScalar) == typeof(ushort)
                || typeof(TScalar) == typeof(uint)
                || typeof(TScalar) == typeof(ulong)
                || typeof(TScalar) == typeof(nuint))
            {
                return vector;
            }

            TVector r = default;
            TVector.set_x(ref r, TScalar.Abs(TVector.get_x(vector)));
            TVector.set_y(ref r, TScalar.Abs(TVector.get_y(vector)));
            TVector.set_z(ref r, TScalar.Abs(TVector.get_z(vector)));
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector4<TVector, TScalar>(in TVector vector)
        {
            if (typeof(TScalar) == typeof(byte)
                || typeof(TScalar) == typeof(ushort)
                || typeof(TScalar) == typeof(uint)
                || typeof(TScalar) == typeof(ulong)
                || typeof(TScalar) == typeof(nuint))
            {
                return vector;
            }

            TVector r = default;
            TVector.set_x(ref r, TScalar.Abs(TVector.get_x(vector)));
            TVector.set_y(ref r, TScalar.Abs(TVector.get_y(vector)));
            TVector.set_z(ref r, TScalar.Abs(TVector.get_z(vector)));
            TVector.set_w(ref r, TScalar.Abs(TVector.get_w(vector)));
            return r;
        }
    }
}
