using System.Diagnostics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns a reflection vector using an incident ray and a surface normal, which is the value of
        /// <paramref name="incident"/> reflected around the one of <paramref name="normal"/>
        /// </summary>
        /// <remarks>
        /// It is the <c>reflect</c> intrinsic of hlsl, which computes the reflection vector with the code below,
        /// and the normal <b>has to be</b> normalized
        /// <code>
        /// return incident - 2 * normal * dot(incident, normal);
        /// </code>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reflect"/></para>
        /// </remarks>
        /// <param name="incident">The value to reflect</param>
        /// <param name="normal">The normalized normal of the surface</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value reflected around the one of
        /// the normal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T reflect<T>(in T incident, in T normal)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Visit_Self<impl_reflect>(incident, normal);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.reflect{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T reflect<T>(this T incident, in T normal)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Visit_Self<impl_reflect>(incident, normal);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is reflected around the one of the normal, which is the difference of the
    /// value and the product of twice the normal and the dot product of the two
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which fuses the product into the difference, and the value of every other
    /// vector reaches the member of the components of it, which builds every one of them with the fused
    /// member</para>
    /// <para>The reflection of a single component has no meaning, the normal of it is the value itself, so the
    /// member of the scalar is not reached</para>
    /// <para>A padding lane holds no component and both the value of the lane and the one of the normal of it are
    /// zero, so the reflection of it is zero as well and the register of the value can be built from it
    /// directly</para>
    /// </summary>
    internal struct impl_reflect : IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>
    {
        // the reflection of a value needs the dot product of it and the normal, which a single component cannot
        // hold
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptScalar<TScalar>(TScalar i, TScalar n)
            => throw new UnreachableException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptVector<TVector, TScalar>(
            in Vector64<TScalar> i, in Vector64<TScalar> n
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(
                    Vector64.FusedMultiplyAdd(
                        Vector64.Create(-2f) * n.AsSingle(),
                        Vector64.Create(Vector64.Dot(i.AsSingle(), n.AsSingle())),
                        i.AsSingle()
                    ).AsByte()
                );

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> i, in Vector128<TScalar> n
        )
        {
            if (typeof(TScalar) == typeof(float))
            {
                var d = TVector.Broadcast(Vector128.Dot(i, n));
                return TVector.UnsafeFromUnderlying(
                    Vector128.FusedMultiplyAdd(
                        TVector.GetUnderlying(TVector.NegativeTwo).AsSingle() * n.AsSingle(),
                        TVector.GetUnderlying(d).AsSingle(),
                        i.AsSingle()
                    ).AsByte()
                );
            }

            if (typeof(TScalar) == typeof(double))
            {
                var d = TVector.Broadcast(Vector128.Dot(i, n));
                return TVector.UnsafeFromUnderlying(
                    Vector128.FusedMultiplyAdd(
                        TVector.GetUnderlying(TVector.NegativeTwo).AsDouble() * n.AsDouble(),
                        TVector.GetUnderlying(d).AsDouble(),
                        i.AsDouble()
                    ).AsByte()
                );
            }

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> i, in Vector256<TScalar> n
        )
        {
            if (typeof(TScalar) == typeof(double))
            {
                var d = TVector.Broadcast(Vector256.Dot(i, n));
                return TVector.UnsafeFromUnderlying(
                    Vector256.FusedMultiplyAdd(
                        TVector.GetUnderlying(TVector.NegativeTwo).AsDouble() * n.AsDouble(),
                        TVector.GetUnderlying(d).AsDouble(),
                        i.AsDouble()
                    ).AsByte()
                );
            }

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptVector2<TVector, TScalar>(
            in TVector i, in TVector n
        )
        {
            // i - 2 * n * dot(i, n)
            var n2 = -TVector.ScalarTwo;

            var ix = TVector.get_x(i);
            var iy = TVector.get_y(i);
            var nx = TVector.get_x(n);
            var ny = TVector.get_y(n);

            var d = GenericMath.FloatDot2(ix, iy, nx, ny);

            var x = TScalar.FusedMultiplyAdd(n2 * nx, d, ix);
            var y = TScalar.FusedMultiplyAdd(n2 * ny, d, iy);

            return TVector.Create(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptVector3<TVector, TScalar>(
            in TVector i, in TVector n
        )
        {
            var n2 = -TVector.ScalarTwo;

            var ix = TVector.get_x(i);
            var iy = TVector.get_y(i);
            var iz = TVector.get_z(i);
            var nx = TVector.get_x(n);
            var ny = TVector.get_y(n);
            var nz = TVector.get_z(n);

            var d = GenericMath.FloatDot3(ix, iy, iz, nx, ny, nz);

            var x = TScalar.FusedMultiplyAdd(n2 * nx, d, ix);
            var y = TScalar.FusedMultiplyAdd(n2 * ny, d, iy);
            var z = TScalar.FusedMultiplyAdd(n2 * nz, d, iz);

            return TVector.Create(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_reflect>.AcceptVector4<TVector, TScalar>(
            in TVector i, in TVector n
        )
        {
            var n2 = -TVector.ScalarTwo;

            var ix = TVector.get_x(i);
            var iy = TVector.get_y(i);
            var iz = TVector.get_z(i);
            var iw = TVector.get_w(i);
            var nx = TVector.get_x(n);
            var ny = TVector.get_y(n);
            var nz = TVector.get_z(n);
            var nw = TVector.get_w(n);

            var d = GenericMath.FloatDot4(ix, iy, iz, iw, nx, ny, nz, nw);

            var x = TScalar.FusedMultiplyAdd(n2 * nx, d, ix);
            var y = TScalar.FusedMultiplyAdd(n2 * ny, d, iy);
            var z = TScalar.FusedMultiplyAdd(n2 * nz, d, iz);
            var w = TScalar.FusedMultiplyAdd(n2 * nw, d, iw);

            return TVector.Create(x, y, z, w);
        }
    }
}
