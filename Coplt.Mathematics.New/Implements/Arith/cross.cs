using System.Diagnostics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the cross product of the two vectors
        /// <code>a.yzx * b.zxy - a.zxy * b.yzx</code>
        /// </summary>
        /// <param name="a">The vector a</param>
        /// <param name="b">The vector b</param>
        /// <typeparam name="T">The type of the vector</typeparam>
        /// <returns>The vector that is perpendicular to both vectors</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T cross<T>(in T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>, IVector3<T>
            => T.Visit_Self<impl_cross>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.cross{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T cross<T>([In] this T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>, IVector3<T>
            => T.Visit_Self<impl_cross>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The cross product of two vectors of 3 components: the register of the vectors is handed to the member of
    /// the visitor that matches its width and the product of a vector that is handed over as a vector is
    /// reached component by component. Only the count of 3 has a product, every other one is unreachable.
    /// </summary>
    internal struct impl_cross : INumberAlgebraVisitor_Self_Self_Self<impl_cross>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self<impl_cross>.AcceptScalar<TScalar>(TScalar a, TScalar b)
            => throw new UnreachableException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_cross>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            => throw new UnreachableException();

        // a.yzx * b.zxy - a.zxy * b.yzx;

        // The rotation of the result is folded into the shuffles of the operands, so the four shuffles come
        // before the products and none of them depends on one, they can issue in parallel with the
        // multiplications. The rotated form, fnma(a.yzx, b, a * b.yzx).yzx, leaves a shuffle behind the
        // fused operation instead, so the same instruction count reaches the result one step later. The
        // fourth lane of both products reads the first component, their difference is zero there, so the
        // padding lane of the result stays zero and the masking constructor is not needed, like in the
        // DirectX Math library and the standard library.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_cross>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
        {
            if (typeof(TScalar) == typeof(float))
            {
                return TVector.UnsafeFromUnderlying(
                    simd.Fnma(
                        Vector128.Shuffle(a.AsSingle(), Vector128.Create(2, 0, 1, 0)),
                        Vector128.Shuffle(b.AsSingle(), Vector128.Create(1, 2, 0, 0)),
                        Vector128.Shuffle(a.AsSingle(), Vector128.Create(1, 2, 0, 0)) *
                        Vector128.Shuffle(b.AsSingle(), Vector128.Create(2, 0, 1, 0))
                    ).AsByte()
                );
            }
            if (typeof(TScalar) == typeof(int))
            {
                return TVector.UnsafeFromUnderlying(
                    (
                        Vector128.Shuffle(a.AsInt32(), Vector128.Create(1, 2, 0, 0)) *
                        Vector128.Shuffle(b.AsInt32(), Vector128.Create(2, 0, 1, 0)) -
                        Vector128.Shuffle(a.AsInt32(), Vector128.Create(2, 0, 1, 0)) *
                        Vector128.Shuffle(b.AsInt32(), Vector128.Create(1, 2, 0, 0))
                    ).AsByte()
                );
            }
            if (typeof(TScalar) == typeof(uint))
            {
                return TVector.UnsafeFromUnderlying(
                    (
                        Vector128.Shuffle(a.AsUInt32(), Vector128.Create(1u, 2, 0, 0)) *
                        Vector128.Shuffle(b.AsUInt32(), Vector128.Create(2u, 0, 1, 0)) -
                        Vector128.Shuffle(a.AsUInt32(), Vector128.Create(2u, 0, 1, 0)) *
                        Vector128.Shuffle(b.AsUInt32(), Vector128.Create(1u, 2, 0, 0))
                    ).AsByte()
                );
            }
            throw new UnreachableException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_cross>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
        {
            if (typeof(TScalar) == typeof(double))
            {
                return TVector.UnsafeFromUnderlying(
                    simd.Fnma(
                        Vector256.Shuffle(a.AsDouble(), Vector256.Create(2, 0, 1, 0)),
                        Vector256.Shuffle(b.AsDouble(), Vector256.Create(1, 2, 0, 0)),
                        Vector256.Shuffle(a.AsDouble(), Vector256.Create(1, 2, 0, 0)) *
                        Vector256.Shuffle(b.AsDouble(), Vector256.Create(2, 0, 1, 0))
                    ).AsByte()
                );
            }
            if (typeof(TScalar) == typeof(long))
            {
                return TVector.UnsafeFromUnderlying(
                    (
                        Vector256.Shuffle(a.AsInt64(), Vector256.Create(1, 2, 0, 0)) *
                        Vector256.Shuffle(b.AsInt64(), Vector256.Create(2, 0, 1, 0)) -
                        Vector256.Shuffle(a.AsInt64(), Vector256.Create(2, 0, 1, 0)) *
                        Vector256.Shuffle(b.AsInt64(), Vector256.Create(1, 2, 0, 0))
                    ).AsByte()
                );
            }
            if (typeof(TScalar) == typeof(ulong))
            {
                return TVector.UnsafeFromUnderlying(
                    (
                        Vector256.Shuffle(a.AsUInt64(), Vector256.Create(1UL, 2, 0, 0)) *
                        Vector256.Shuffle(b.AsUInt64(), Vector256.Create(2UL, 0, 1, 0)) -
                        Vector256.Shuffle(a.AsUInt64(), Vector256.Create(2UL, 0, 1, 0)) *
                        Vector256.Shuffle(b.AsUInt64(), Vector256.Create(1UL, 2, 0, 0))
                    ).AsByte()
                );
            }
            throw new UnreachableException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_cross>.AcceptVector3<TVector, TScalar>(in TVector a, in TVector b)
        {
            var a_x = TVector.get_x(a);
            var a_y = TVector.get_y(a);
            var a_z = TVector.get_z(a);

            var b_x = TVector.get_x(b);
            var b_y = TVector.get_y(b);
            var b_z = TVector.get_z(b);

            // a.yzx * b.zxy - a.zxy * b.yzx;
            var r_x = a_y * b_z - a_z * b_y;
            var r_y = a_z * b_x - a_x * b_z;
            var r_z = a_x * b_y - a_y * b_x;

            TVector r = default;
            TVector.set_x(ref r, r_x);
            TVector.set_y(ref r, r_y);
            TVector.set_z(ref r, r_z);
            return r;
        }
    }
}
