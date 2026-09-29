using System.Diagnostics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the projection of <paramref name="value"/> onto <paramref name="onto"/> without checking the dot
        /// product of the vector with itself, which is the same as <see cref="math.project{T}"/> but the value of
        /// it is only meaningful when the dot product of <paramref name="onto"/> with itself is not below the
        /// denominator epsilon of the kind of it
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
        /// <para>The dot product of the vector with itself is the squared length of it: the quotient of the value
        /// by the zero of the kind of it is a value of the kind that is not a number, which the member takes as it
        /// is, so the caller of it has to know that the vector of it is not the zero of the kind of it</para>
        /// </remarks>
        /// <param name="value">The value to project</param>
        /// <param name="onto">The vector to project onto, it does <b>not</b> have to be of unit length</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value that is parallel to the
        /// vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_unsafe<T>(in T value, in T onto)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Self<impl_project_unsafe>(value, onto);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.project_unsafe{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_unsafe<T>(this T value, in T onto)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.project_unsafe(value, onto);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the component of it that is parallel to the one of the vector, which is
    /// the vector scaled by the quotient of the dot product of the two and the dot product of the vector with
    /// itself, without the check of the dot product of the vector with itself
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches the
    /// width of the register, which takes the dot product of the register itself, and the value of every other
    /// vector reaches the member of the components of it, which builds the dot product from the members of
    /// them</para>
    /// <para>The projection of a single component has no meaning, the vector of it is the value itself, so the
    /// member of the scalar is not reached</para>
    /// <para>The dot product of the vector with itself is the zero of the kind of the component when every
    /// component of the vector is zero, the quotient of the value by it is a value of the kind that is not a
    /// number, which the member takes as it is: <see cref="math.project{T}"/> is the one that checks the dot
    /// product and returns the zero of the kind of it for the value</para>
    /// <para>A padding lane holds no component and both the value of the lane and the one of the vector of it are
    /// zero, so the projection of it is zero as well and the register of the value can be built from it
    /// directly</para>
    /// </summary>
    internal struct impl_project_unsafe : IAlgebraVisitor_T_T_T<impl_project_unsafe>
    {
        // the projection of a value needs the dot product of it and the vector, which a single component cannot
        // hold
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_project_unsafe>.Scalar_Float<TScalar>(TScalar value, TScalar onto)
            => throw new UnreachableException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project_unsafe>.Simd_Float<TVector, TScalar>(
            in Vector128<TScalar> value, in Vector128<TScalar> onto
        )
        {
            var d = Vector128.Dot(onto, onto);
            return TVector.UnsafeFromUnderlying(
                (TVector.GetUnderlying(TVector.Broadcast(
                    Vector128.Dot(value, onto) / d
                )).As<byte, TScalar>() * onto).AsByte()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project_unsafe>.Simd_Float<TVector, TScalar>(
            in Vector256<TScalar> value, in Vector256<TScalar> onto
        )
        {
            var d = Vector256.Dot(onto, onto);
            return TVector.UnsafeFromUnderlying(
                (TVector.GetUnderlying(TVector.Broadcast(
                    Vector256.Dot(value, onto) / d
                )).As<byte, TScalar>() * onto).AsByte()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project_unsafe>.Vector2_Float<TVector, TScalar>(
            in TVector value, in TVector onto
        )
        {
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);

            var b = GenericMath.FloatDot2(ox, oy, ox, oy);

            var vx = TVector.get_x(value);
            var vy = TVector.get_y(value);

            var a = GenericMath.FloatDot2(vx, vy, ox, oy);
            var c = a / b;

            var x = c * ox;
            var y = c * oy;

            return TVector.Create(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_project_unsafe>.Vector3_Float<TVector, TScalar>(
            in TVector value, in TVector onto
        )
        {
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);
            var oz = TVector.get_z(onto);

            var b = GenericMath.FloatDot3(ox, oy, oz, ox, oy, oz);

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
        static TVector IAlgebraVisitor_T_T_T<impl_project_unsafe>.Vector4_Float<TVector, TScalar>(
            in TVector value, in TVector onto
        )
        {
            var ox = TVector.get_x(onto);
            var oy = TVector.get_y(onto);
            var oz = TVector.get_z(onto);
            var ow = TVector.get_w(onto);

            var b = GenericMath.FloatDot4(ox, oy, oz, ow, ox, oy, oz, ow);

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
