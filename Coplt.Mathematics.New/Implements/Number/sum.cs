using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the sum of every component of the value
        /// <para>It is the dot product of the value with a value whose every component is one, see
        /// <see cref="dot{T,TScalar}(T, T)"/>, and the sum of a matrix is the one of every component of
        /// every one of its columns</para>
        /// </summary>
        /// <param name="value">The value, a vector or a matrix</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The sum of every component of the value</returns>
        // the type of a single component is only a part of the result of the member, so the compiler cannot
        // infer it from the arguments and a call that does not name it reaches the member of the scalar type
        // of the value instead, which the generator emits for every scalar type: the attribute marks this
        // member for it
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar sum<T, TScalar>(T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Scalar<impl_sum>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value of the sum of every component of a value
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the count of its
    /// components and the values of a matrix reach the member that combines the sum of every column of it</para>
    /// </summary>
    internal struct impl_sum : IAlgebraVisitor_T_S<impl_sum>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_sum>.Scalar_Number<TScalar>(TScalar value)
            => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraCombinator_S_S<impl_sum>.Combine_Number<TScalar>(TScalar a, TScalar b)
            => a + b;

        // the sum of every lane of the register: the padding lane of the value is zero, so the sum of the whole
        // register is the one of the components
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_T_S<impl_sum>.Simd_Number<TVector, TScalar>(
            Vector128<TScalar> value
        ) => Vector128.Sum(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_T_S<impl_sum>.Simd_Number<TVector, TScalar>(
            Vector256<TScalar> value
        ) => Vector256.Sum(value);

        // a vector without a register hands no register over, so the sum is the one of the components of it
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_T_S<impl_sum>.Vector2_Number<TVector, TScalar>(TVector value)
            => TVector.get_x(value) + TVector.get_y(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_T_S<impl_sum>.Vector3_Number<TVector, TScalar>(TVector value)
            => TVector.get_x(value) + TVector.get_y(value) + TVector.get_z(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_T_S<impl_sum>.Vector4_Number<TVector, TScalar>(TVector value)
            => TVector.get_x(value) + TVector.get_y(value) + TVector.get_z(value) + TVector.get_w(value);
    }
}
