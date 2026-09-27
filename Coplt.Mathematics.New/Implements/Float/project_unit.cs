using System.Diagnostics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the projection of <paramref name="value"/> onto <paramref name="onto"/>, which is the same as
        /// <see cref="math.project{T}"/> but <paramref name="onto"/> is assumed to be of unit length
        /// <para>It is the component of <paramref name="value"/> that is parallel to <paramref name="onto"/></para>
        /// </summary>
        /// <remarks>
        /// The dot product of a vector of unit length with itself is one, so there is no quotient to take and the
        /// value is only scaled by the dot product of the two:
        /// <code>
        /// return dot(value, onto) * onto;
        /// </code>
        /// <para>The projection is meaningless when <paramref name="onto"/> is not of unit length</para>
        /// </remarks>
        /// <param name="value">The value to project</param>
        /// <param name="onto">The vector of unit length to project onto</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value that is parallel to the
        /// vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_unit<T>(in T value, in T onto)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Visit_Self<impl_project_unit>(value, onto);

        /// <summary>
        /// Returns the projection of <paramref name="value"/> onto the plane that has
        /// <paramref name="plane_normal"/> as its normal, which is the same as
        /// <see cref="math.project_on_plane{T}"/> but <paramref name="plane_normal"/> is assumed to be of unit
        /// length
        /// <para>It is the component of <paramref name="value"/> that is inside the plane</para>
        /// </summary>
        /// <remarks>
        /// It is the difference of the value and the projection of it onto the normal:
        /// <code>
        /// return value - project_unit(value, plane_normal);
        /// </code>
        /// <para>The projection is meaningless when <paramref name="plane_normal"/> is not of unit length</para>
        /// </remarks>
        /// <param name="value">The value to project</param>
        /// <param name="plane_normal">The normal of the plane, of unit length</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value that is inside the
        /// plane</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_on_plane_unit<T>(in T value, in T plane_normal)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
            => value - project_unit(value, plane_normal);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.project_unit{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_unit<T>(this T value, in T onto)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.project_unit(value, onto);

        /// <inheritdoc cref="math.project_on_plane_unit{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_on_plane_unit<T>(this T value, in T plane_normal)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.project_on_plane_unit(value, plane_normal);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the component of it that is parallel to the one of the vector of unit
    /// length, which is the value scaled by the dot product of the two
    /// <para>The dot product of a vector of unit length with itself is one, so there is no quotient to take and
    /// the value of a vector that keeps it in a register reaches the member of the visitor that matches the width
    /// of the register, the value of every other vector reaches the member of the components of it</para>
    /// <para>The projection of a single component has no meaning, the vector of it is the value itself, so the
    /// member of the scalar is not reached</para>
    /// <para>A padding lane holds no component and both the value of the lane and the one of the vector of it are
    /// zero, so the projection of it is zero as well and the register of the value can be built from it
    /// directly</para>
    /// </summary>
    internal struct impl_project_unit : IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>
    {
        // the projection of a value needs the dot product of it and the vector, which a single component cannot
        // hold
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptScalar<TScalar>(TScalar value, TScalar onto)
            => throw new UnreachableException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptVector<TVector, TScalar>(
            in Vector64<TScalar> value, in Vector64<TScalar> onto
        ) => TVector.FromUnderlying((Vector64.Create(Vector64.Dot(value, onto)) * onto).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> value, in Vector128<TScalar> onto
        ) => TVector.FromUnderlying((Vector128.Create(Vector128.Dot(value, onto)) * onto).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> value, in Vector256<TScalar> onto
        ) => TVector.FromUnderlying((Vector256.Create(Vector256.Dot(value, onto)) * onto).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptVector2<TVector, TScalar>(
            in TVector value, in TVector onto
        )
        {
            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);

            var d = GenericMath.FloatDot2(vx, vy, ox, oy);

            var x = d * ox;
            var y = d * oy;

            return TVector.Create(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptVector3<TVector, TScalar>(
            in TVector value, in TVector onto
        )
        {
            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);
            var vz = TVector.get_z(value);
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);
            var oz = TVector.get_z(onto);

            var d = GenericMath.FloatDot3(vx, vy, vz, ox, oy, oz);

            var x = d * ox;
            var y = d * oy;
            var z = d * oz;

            return TVector.Create(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_project_unit>.AcceptVector4<TVector, TScalar>(
            in TVector value, in TVector onto
        )
        {
            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);
            var vz = TVector.get_z(value);
            var vw = TVector.get_w(value);
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);
            var oz = TVector.get_z(onto);
            var ow = TVector.get_w(onto);

            var d = GenericMath.FloatDot4(vx, vy, vz, vw, ox, oy, oz, ow);

            var x = d * ox;
            var y = d * oy;
            var z = d * oz;
            var w = d * ow;

            return TVector.Create(x, y, z, w);
        }
    }
}
