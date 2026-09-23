using Coplt.Mathematics.Generics;
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
        public static T min<T>(in T a, in T b) where T : unmanaged, INumberVector<T>, IDynamicVector<T>
            => T.VisitUnderlyingReturnVector<impl_min>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.min{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T min<T>([In] this ref T a, in T b) where T : unmanaged, INumberVector<T>, IDynamicVector<T>
            => T.VisitUnderlyingReturnVector<impl_min>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    internal struct impl_min : IVectorUnderlyingVisitor2ReturnVector, IVectorDimensionVisitor2ReturnVector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitor2ReturnVector.Accept<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            => TVector.FromUnderlying(Vector64.Min(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitor2ReturnVector.Accept<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector128.Min(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitor2ReturnVector.Accept<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector256.Min(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorUnderlyingVisitor2ReturnVector.AcceptSoft<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.VisitDimensionReturnVector<impl_min>(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitor2ReturnVector.AcceptVector2<TVector, TScalar>(in TVector a, in TVector b)
        {
            TVector r = default;
            TVector.set_x(ref r, TScalar.Min(TVector.get_x(a), TVector.get_x(b)));
            TVector.set_y(ref r, TScalar.Min(TVector.get_y(a), TVector.get_y(b)));
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitor2ReturnVector.AcceptVector3<TVector, TScalar>(in TVector a, in TVector b)
        {
            TVector r = default;
            TVector.set_x(ref r, TScalar.Min(TVector.get_x(a), TVector.get_x(b)));
            TVector.set_y(ref r, TScalar.Min(TVector.get_y(a), TVector.get_y(b)));
            TVector.set_z(ref r, TScalar.Min(TVector.get_z(a), TVector.get_z(b)));
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IVectorDimensionVisitor2ReturnVector.AcceptVector4<TVector, TScalar>(in TVector a, in TVector b)
        {
            TVector r = default;
            TVector.set_x(ref r, TScalar.Min(TVector.get_x(a), TVector.get_x(b)));
            TVector.set_y(ref r, TScalar.Min(TVector.get_y(a), TVector.get_y(b)));
            TVector.set_z(ref r, TScalar.Min(TVector.get_z(a), TVector.get_z(b)));
            TVector.set_w(ref r, TScalar.Min(TVector.get_w(a), TVector.get_w(b)));
            return r;
        }
    }
}
