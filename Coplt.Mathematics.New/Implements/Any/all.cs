using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Holds when the bits of every component of <paramref name="value"/> are not all zero
        /// </summary>
        /// <remarks>
        /// A component of a value holds when the bits of it are not all zero, so the all bits set value of its
        /// kind is the conventional true one and every value of the kind that has a bit set holds just as well.
        /// <para>It is the <c>all</c> intrinsic of hlsl, which determines whether all components of the
        /// specified value are non-zero, and it is the counterpart of the
        /// <see cref="math.any{T}(T)"/> of the library.</para>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-all"/></para>
        /// </remarks>
        /// <param name="value">The value to query</param>
        /// <typeparam name="T">The type of the value, which is a vector or a matrix</typeparam>
        /// <returns>True when every component of <paramref name="value"/> holds</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all<T>(T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Bool<impl_all>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.all{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Bool<impl_all>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The query of a value that holds when every component of it holds
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which holds when no lane of the value is all bits zero: the bits of a
    /// component of the value are the bits of a lane of the register, so a component holds when any of the bits
    /// of it is set. The lanes that the value of a vector of 2 or 3 components does not reach hold no component
    /// and they are zero, so the member compares the lanes of the value with the complement of the mask of the
    /// padding lanes of the register instead of the all bits zero lane: the complement of the mask is the all
    /// bits set lane in the lanes the value does not reach, which a lane of it that is zero never matches, and
    /// it is the all bits zero lane in the lanes the value reaches, which a component whose bits are all zero
    /// matches and a component that holds does not. The value of a vector without a register keeps its components in
    /// fields, so the dispatch of the value hands every one of them over on its own and the answers of them are
    /// the ones of the components that hold combined; the columns of a matrix and the components of those reach
    /// the members the same way. The member of a single component answers for a component that the dispatch of
    /// a value hands over on its own, which is the check of <see cref="Utils.IsTrue{TScalar}(TScalar)"/>: the
    /// kind of the value of a component is the one the member is closed over, the bits of the value of a
    /// floating point kind are the ones that decide the answer, and the other kinds are compared with the all
    /// bits zero value of the kind.</para>
    /// </summary>
    internal struct impl_all : IAlgebraVisitor_T_bool<impl_all>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Combine(bool a, bool b) => a & b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_S_bool<impl_all>.Scalar_Number<TScalar>(TScalar value)
            => Utils.IsTrue(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_T_bool<impl_all>.Simd_Any<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float) || typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(uint))
                return !Vector128.EqualsAny(vector.AsUInt32(), ~TVector.PaddingLanesMask.AsUInt32());

            if (typeof(TScalar) == typeof(double) || typeof(TScalar) == typeof(long) || typeof(TScalar) == typeof(ulong))
                return !Vector128.EqualsAny(vector.AsUInt64(), ~TVector.PaddingLanesMask.AsUInt64());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_T_bool<impl_all>.Simd_Any<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double) || typeof(TScalar) == typeof(long) || typeof(TScalar) == typeof(ulong))
                return !Vector256.EqualsAny(vector.AsUInt64(), ~TVector.PaddingLanesMask.AsUInt64());

            throw new NotSupportedException();
        }
    }
}
