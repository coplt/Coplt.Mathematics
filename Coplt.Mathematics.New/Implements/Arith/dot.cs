using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the dot product of the two vectors, which is the sum of the products of the components of
        /// them
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <typeparam name="T">The type of the vectors</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The sum of the products of the components</returns>
        // the type of a single component is only a part of the result of the member, so the compiler cannot
        // infer it from the arguments and a call that does not name it reaches the member of the scalar type
        // of the vector instead, which the generator emits for every scalar type: the attribute marks this
        // member for it and the priority keeps this member in front of the one of them when the caller names
        // both types
        [ScalarExtension]
        [OverloadResolutionPriority(10000)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar dot<T, TScalar>(in T a, in T b)
            where T : unmanaged, INumberAlgebraDispatch<T, TScalar>, INumberVector<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Visit_Scalar<impl_dot>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value of the sum of the products of the components of two values
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the count of its
    /// components and the values of a matrix reach the member that combines the sum of the products of every
    /// column of it</para>
    /// </summary>
    internal struct impl_dot : INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptScalar<TScalar>(TScalar a, TScalar b)
            => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptCombine<TScalar>(TScalar a, TScalar b)
            => a + b;

        // the sum of the products of every lane of the register: the padding lane of both operands is zero, so
        // the product of it is zero as well and the sum of the whole register is the one of the components
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptVector<TVector, TScalar>(
            in Vector64<TScalar> a, in Vector64<TScalar> b
        ) => Vector64.Dot(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> a, in Vector128<TScalar> b
        ) => Vector128.Dot(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> a, in Vector256<TScalar> b
        ) => Vector256.Dot(a, b);

        // a vector without a register hands no register over, so the sum is the one of the components of it
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptVector2<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptVector3<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b) + TVector.get_z(a) * TVector.get_z(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>.AcceptVector4<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b) +
               TVector.get_z(a) * TVector.get_z(b) + TVector.get_w(a) * TVector.get_w(b);
    }
}
