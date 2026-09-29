using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Selects between the two values bit by bit, which is a mux of them: the bit of <paramref name="t"/> where
        /// the bit of <paramref name="c"/> at the position of it is set and the bit of <paramref name="f"/> where
        /// it is not
        /// </summary>
        /// <remarks>
        /// A mask is a value of the kind of the values to select and every bit of it decides the bit of the
        /// result at the position of it: the all bits set value of the kind selects the value of
        /// <paramref name="t"/> as it is, the all bits zero value of it selects the value of
        /// <paramref name="f"/> as it is, and a mask that is neither of the two of them muxes the bits of the
        /// two values.
        /// <para>The <c>select</c> intrinsic of hlsl is the counterpart of it, which differs from it in the
        /// mask: the mask of the intrinsic is about a whole component of the values to select, so it takes the
        /// whole of a component from the one of the two values that the component of the mask at the position
        /// of it holds, where the mask of this member is a value of the kind of the values to select itself and
        /// every bit of it decides the bit of the result at the position of it.</para>
        /// <para>It is nothing that a computation that is about a whole component notices: simd takes the all
        /// bits set value of the kind as the true one and the all bits zero value of it as the false one, so the
        /// mask of a comparison of simd values is the one or the other of them, and both of them take the whole
        /// of a component from one of the two values with this member as they do with the intrinsic.</para>
        /// <para>A mask that is neither of the two of them is not one that the member is written for: the bits
        /// of the value alone deciding the bits of the result is the answer of this library, where a platform
        /// may mux the bits of the two values with the mask of it or take the whole of a component with the sign
        /// bit of the mask of it, so the answer of the member that is handed such a mask is not one that a
        /// caller may rely on, on a cpu as much as on a gpu.</para>
        /// </remarks>
        /// <param name="c">The mask, a value of the kind of the values to select</param>
        /// <param name="t">The value that the bits the mask sets are taken from</param>
        /// <param name="f">The value that the bits the mask does not set are taken from</param>
        /// <typeparam name="T">The type of the values to select, the three of them have it</typeparam>
        /// <returns>The value of the kind of <typeparamref name="T"/> whose every bit is the one of
        /// <paramref name="t"/> where the bit of <paramref name="c"/> at the position of it is set and the one of
        /// <paramref name="f"/> where it is not</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T select<T>(in T c, in T t, in T f) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_select>(c, t, f);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.select{T}(in T, in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T select<T>(this T t, in T c, in T f) where T : unmanaged, IAlgebraDispatch<T>
            => math.select(c, t, f);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The mux of two values with a mask
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns. A
    /// mask is the all bits set value of the kind of a component, so the bits of it decide which of the two
    /// values every bit of the result comes from: the register path hands the mask over to the member of the
    /// platform that selects with it and the path of a component selects the bits of it with the
    /// same rule. A padding lane holds no component and its bits are zero, so the mask of it selects the value
    /// that the mask does not hold, whose padding lane is zero as well, which is why the register of the result
    /// is built from it directly</para>
    /// </summary>
    internal struct impl_select : IAlgebraVisitor_T_T_T_T<impl_select>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_select>.Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c) =>
            math.select(a, b, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_select>.Simd_Any<TVector, TScalar>(
            in Vector128<TScalar> a, in Vector128<TScalar> b, in Vector128<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector128.ConditionalSelect(a, b, c).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_select>.Simd_Any<TVector, TScalar>(
            in Vector256<TScalar> a, in Vector256<TScalar> b, in Vector256<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector256.ConditionalSelect(a, b, c).AsByte());
    }
}
