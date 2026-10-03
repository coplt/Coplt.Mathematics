using System.Diagnostics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the projection of <paramref name="value"/> onto <paramref name="onto"/>
        /// <para>It is the component of <paramref name="value"/> that is parallel to <paramref name="onto"/>, the
        /// rest of the value is the component of it that is inside the plane that has
        /// <paramref name="onto"/> as its normal</para>
        /// </summary>
        /// <remarks>
        /// The projection of the value onto the vector is the vector scaled by the quotient of the dot product of
        /// the two and the dot product of the vector with itself:
        /// <code>
        /// return dot(value, onto) / dot(onto, onto) * onto;
        /// </code>
        /// <para>The dot product of the vector with itself is the squared length of it: a value below the
        /// denominator epsilon of the kind of it is the zero of the kind for the quotient, the projection of the
        /// value onto such a vector is the zero of it</para>
        /// <para><see cref="math.project_unsafe{T}"/> is the same projection without the check of the dot product
        /// of the vector with itself, which a caller that knows the value of it can reach</para>
        /// </remarks>
        /// <param name="value">The value to project</param>
        /// <param name="onto">The vector to project onto, it does <b>not</b> have to be of unit length</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value that is parallel to the
        /// vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project<T>(T value, T onto)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Self<impl_project>(value, onto);

        /// <summary>
        /// Returns the projection of <paramref name="value"/> onto the plane that has
        /// <paramref name="plane_normal"/> as its normal
        /// <para>It is the component of <paramref name="value"/> that is inside the plane, which is the rest of
        /// the value that the projection of it onto the normal does not hold</para>
        /// </summary>
        /// <remarks>
        /// It is the difference of the value and the projection of it onto the normal:
        /// <code>
        /// return value - project(value, plane_normal);
        /// </code>
        /// </remarks>
        /// <param name="value">The value to project</param>
        /// <param name="plane_normal">The normal of the plane, it does <b>not</b> have to be of unit length</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value that is inside the
        /// plane</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_on_plane<T>(T value, T plane_normal)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => value - project(value, plane_normal);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.project{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project<T>(this T value, T onto)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.project(value, onto);

        /// <inheritdoc cref="math.project_on_plane{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_on_plane<T>(this T value, T plane_normal)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.project_on_plane(value, plane_normal);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the component of it that is parallel to the one of the vector, which is
    /// the vector scaled by the quotient of the dot product of the two and the dot product of the vector with
    /// itself
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches the
    /// width of the register, which takes the dot product of the register itself, and the value of every other
    /// vector reaches the member of the components of it, which builds the dot product from the members of
    /// them</para>
    /// <para>The projection of a single component has no meaning, the vector of it is the value itself, so the
    /// member of the scalar is not reached</para>
    /// <para>The dot product of the vector with itself is the squared length of it, a value below the denominator
    /// epsilon of the kind of the component is the zero of the kind for the quotient, so the projection of the
    /// value onto such a vector is the zero of it</para>
    /// <para>A padding lane holds no component and both the value of the lane and the one of the vector of it are
    /// zero, so the projection of it is zero as well and the register of the value can be built from it
    /// directly</para>
    /// </summary>
    internal struct impl_project : IAlgebraVisitor_T_T_T<impl_project>
    {
        // the projection of a value needs the dot product of it and the vector, which a single component cannot
        // hold
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_project>.Scalar_Float<TScalar>(TScalar value, TScalar onto)
            => throw new UnreachableException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project>.Simd_Float<TVector, TScalar>(
            Vector128<TScalar> value, Vector128<TScalar> onto
        )
        {
            var d = Vector128.Dot(onto, onto);
            if (d < TVector.ScalarDenomEpsilon) return TVector.Zero;
            return TVector.UnsafeFromUnderlying(
                (TVector.GetUnderlying(TVector.Broadcast(
                    Vector128.Dot(value, onto) / d
                )).As<byte, TScalar>() * onto).AsByte()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project>.Simd_Float<TVector, TScalar>(
            Vector256<TScalar> value, Vector256<TScalar> onto
        )
        {
            var d = Vector256.Dot(onto, onto);
            if (d < TVector.ScalarDenomEpsilon) return TVector.Zero;
            return TVector.UnsafeFromUnderlying(
                (TVector.GetUnderlying(TVector.Broadcast(
                    Vector256.Dot(value, onto) / d
                )).As<byte, TScalar>() * onto).AsByte()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project>.Vector2_Float<TVector, TScalar>(
            TVector value, TVector onto
        )
        {
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);

            var b = GenericMath.FloatDot2(ox, oy, ox, oy);
            if (b < TVector.ScalarDenomEpsilon) return TVector.Zero;

            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);

            var a = GenericMath.FloatDot2(vx, vy, ox, oy);
            var c = a / b;

            var x = c * ox;
            var y = c * oy;

            return TVector.Create(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project>.Vector3_Float<TVector, TScalar>(
            TVector value, TVector onto
        )
        {
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);
            var oz = TVector.get_z(onto);

            var b = GenericMath.FloatDot3(ox, oy, oz, ox, oy, oz);
            if (b < TVector.ScalarDenomEpsilon) return TVector.Zero;

            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);
            var vz = TVector.get_z(value);

            var a = GenericMath.FloatDot3(vx, vy, vz, ox, oy, oz);
            var c = a / b;

            var x = c * ox;
            var y = c * oy;
            var z = c * oz;

            return TVector.Create(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project>.Vector4_Float<TVector, TScalar>(
            TVector value, TVector onto
        )
        {
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);
            var oz = TVector.get_z(onto);
            var ow = TVector.get_w(onto);

            var b = GenericMath.FloatDot4(ox, oy, oz, ow, ox, oy, oz, ow);
            if (b < TVector.ScalarDenomEpsilon) return TVector.Zero;

            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);
            var vz = TVector.get_z(value);
            var vw = TVector.get_w(value);

            var a = GenericMath.FloatDot4(vx, vy, vz, vw, ox, oy, oz, ow);
            var c = a / b;

            var x = c * ox;
            var y = c * oy;
            var z = c * oz;
            var w = c * ow;

            return TVector.Create(x, y, z, w);
        }
    }
}
