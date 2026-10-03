using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class ex_math
    {
        extension(math)
        {
            /// <summary>
            /// Selects between the two values with a mask
            /// </summary>
            /// <remarks>
            /// A mask is a value of the kind of the values to select: the all bits set value of the kind takes the
            /// whole of the result from <paramref name="t"/> and the all bits zero value of it takes the whole of it
            /// from <paramref name="f"/>, which are the two of them that the member is written for.
            /// <para>A mask that is neither of the two of them answers with the one of the platform: which of the
            /// two values a bit of the result comes from is the trait of the platform that selects with the mask,
            /// which may mux the bits of the two values with it or take the whole of a component with the sign bit
            /// of it, so the answer of the member that is handed such a mask is not one that a caller may rely on,
            /// on a cpu as much as on a gpu.</para>
            /// <para>The <c>select</c> intrinsic of hlsl is the counterpart of it, which selects component by
            /// component and not with a mask: the whole of a component is taken from <paramref name="t"/> where the
            /// condition of that component of the intrinsic holds and from <paramref name="f"/> where it does not,
            /// which is the <c>c ? t : f</c> of it, and the truth of a component of the condition of it is the one
            /// of hlsl, which is a component that is not the zero of its kind. The truth of a component of the mask
            /// of this member is the one of the platform instead.</para>
            /// <para>It is nothing that a computation that is about a whole component notices: the output of a bool
            /// operation of simd is the all bits set value of the kind in a component that holds and the all bits
            /// zero value of it in a component that does not, so the mask of such a computation is one of the two of
            /// them in every component of it, and both of them take the whole of a component from one of the two
            /// values with this member as they do with the intrinsic.</para>
            /// </remarks>
            /// <param name="c">The mask, a value of the kind of the values to select</param>
            /// <param name="t">The value that is selected where the mask holds</param>
            /// <param name="f">The value that is selected where the mask does not hold</param>
            /// <typeparam name="T">The type of the values to select, the three of them have it</typeparam>
            /// <returns>The value that the mask selects out of the two values</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T select<T>(T c, T t, T f) where T : unmanaged, IAlgebraDispatch<T>
                => T.Self<impl_select>(c, t, f);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="ex_math.select{T}(T, T, T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T select<T>(this T t, T c, T f) where T : unmanaged, IAlgebraDispatch<T>
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
    /// mask is the all bits set value of the kind of a component or the all bits zero value of it, which are the
    /// two of them that take the whole of a component from one of the two values: the register path hands the
    /// mask over to the member of the platform that selects with it and the path of a component muxes the bits
    /// of the two values with the members of the kind of a component, so a mask that is neither of the two of
    /// them answers with the one of the platform on the register path and with the one of the members of the
    /// kind on the path of a component. A padding lane holds no component and its bits are zero, so the mask of
    /// it selects the value that the mask does not hold, whose padding lane is zero as well, which is why the
    /// register of the result is built from it directly</para>
    /// </summary>
    internal struct impl_select : IAlgebraVisitor_T_T_T_T<impl_select>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_select>.Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c) =>
            math.select(a, b, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_select>.Simd_Any<TVector, TScalar>(
            Vector128<TScalar> a, Vector128<TScalar> b, Vector128<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector128.ConditionalSelect(a, b, c).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_select>.Simd_Any<TVector, TScalar>(
            Vector256<TScalar> a, Vector256<TScalar> b, Vector256<TScalar> c)
            => TVector.UnsafeFromUnderlying(Vector256.ConditionalSelect(a, b, c).AsByte());
    }
}
