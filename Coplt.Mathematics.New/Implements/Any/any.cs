using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Holds when the bits of one of the components of <paramref name="value"/> are not all zero
        /// </summary>
        /// <remarks>
        /// A component of a value holds when the bits of it are not all zero, so the all bits set value of its
        /// kind is the conventional true one and every value of the kind that has a bit set holds just as well.
        /// <para>It is the <c>any</c> intrinsic of hlsl, which determines whether any component of the
        /// specified value is non-zero.</para>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-any"/></para>
        /// </remarks>
        /// <param name="value">The value to query</param>
        /// <typeparam name="T">The type of the value, which is a vector or a matrix</typeparam>
        /// <returns>True when one of the components of <paramref name="value"/> holds</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any<T>(in T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Bool<impl_any>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.any{T}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Bool<impl_any>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The query of a value that holds when one of the components of it holds
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which holds when the register is not all bits zero: the bits of a
    /// component of the value are the bits of a lane of the register, so a component holds when any of the bits
    /// of it is set, and the lanes that the value of a vector of 2 or 3 components does not reach are kept at
    /// zero by the constructor of it, so they never hold. The value of a vector without a register keeps its
    /// components in fields, so the dispatch of the value hands every one of them over on its own and the
    /// answers of them are the ones of the components that hold combined; the columns of a matrix and the
    /// components of those reach the members the same way. The member of a single component answers for a
    /// component that the dispatch of a value hands over on its own, which is the check of
    /// <see cref="Utils.IsTrue{TScalar}(TScalar)"/>: the kind of the value of a component is the one the member
    /// is closed over, the bits of the value of a floating point kind are the ones that decide the answer, and
    /// the other kinds are compared with the all bits zero value of the kind.</para>
    /// </summary>
    internal struct impl_any : IAlgebraVisitor_T_bool<impl_any>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Combine(bool a, bool b) => a | b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_S_bool<impl_any>.Scalar_Number<TScalar>(TScalar value)
            => Utils.IsTrue(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_T_bool<impl_any>.Simd_Any<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            return !(vector.AsByte() == default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_T_bool<impl_any>.Simd_Any<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            return !(vector.AsByte() == default);
        }
    }
}
