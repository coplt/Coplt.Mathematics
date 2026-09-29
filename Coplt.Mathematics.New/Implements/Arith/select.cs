using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Selects between the two values component by component: the component of <paramref name="t"/> where the
        /// component of <paramref name="c"/> is not the zero of its kind and the component of <paramref name="f"/>
        /// where it is the zero of its kind
        /// <para>A mask is a value of the kind of the values to select, so the all bits set value of the kind is
        /// the conventional true one and every value that is not the zero of the kind holds just as well</para>
        /// </summary>
        /// <param name="c">The mask, a value of the kind of the values to select</param>
        /// <param name="t">The value that the components the mask holds are taken from</param>
        /// <param name="f">The value that the components the mask does not hold are taken from</param>
        /// <typeparam name="T">The type of the values to select, the three of them have it</typeparam>
        /// <returns>The value of the kind of <typeparamref name="T"/> that has the component of
        /// <paramref name="t"/> where <paramref name="c"/> holds and the one of <paramref name="f"/> where it does
        /// not</returns>
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
    /// The selection between two values component by component
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns. A
    /// mask is the all bits set value of the kind of a component, so the bits of it decide which of the two
    /// values every bit of the result comes from: the register path hands the mask over to the member of the
    /// member of the platform that selects with it and the path of a component selects the bits of it with the
    /// same rule. A padding lane holds no component and its bits are zero, so the mask of it selects the value
    /// that the mask does not hold, whose padding lane is zero as well, which is why the register of the result
    /// is built from it directly</para>
    /// </summary>
    internal struct impl_select : IAlgebraVisitor_T_T_T_T<impl_select>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_select>.Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c)
            => (b & a) | (c & ~a);

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
