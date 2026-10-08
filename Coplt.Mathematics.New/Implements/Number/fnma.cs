using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class ex_math
    {
        extension(math)
        {
            /// <summary>
            /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the subtraction of it
            /// from <paramref name="c"/>: <code>c - (a * b)</code> or <code>-(a * b) + c</code>
            /// </summary>
            /// <param name="a">The value that is multiplied</param>
            /// <param name="b">The value that multiplies <paramref name="a"/></param>
            /// <param name="c">The value that the product is subtracted from</param>
            /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
            /// <returns>The fused result</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T fnma<T>(T a, T b, T c) where T : unmanaged, IAlgebraDispatch<T>
                => T.Self<impl_fnma>(a, b, c);

            /// <summary>
            /// Fuses the multiplication of <paramref name="a"/> and <paramref name="b"/> with the subtraction of it
            /// from <paramref name="c"/>, the operands are named in the order of the subtraction:
            /// <code>c - (a * b)</code> or <code>-(a * b) + c</code>
            /// </summary>
            /// <param name="c">The value that the product is subtracted from</param>
            /// <param name="a">The value that is multiplied</param>
            /// <param name="b">The value that multiplies <paramref name="a"/></param>
            /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
            /// <returns>The fused result</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T fsm<T>(T c, T a, T b) where T : unmanaged, IAlgebraDispatch<T>
                => math.fnma(a, b, c);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="ex_math.fnma{T}(T, T, T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T fnma<T>(this T a, T b, T c) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_fnma>(a, b, c);

        /// <inheritdoc cref="ex_math.fsm{T}(T, T, T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T fsm<T>(this T c, T a, T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_fnma>(a, b, c);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component fuses the multiplication of two values with the subtraction of it from a
    /// third one: <code>c - (a * b)</code> or <code>-(a * b) + c</code>
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_fnma : IAlgebraVisitor_T_T_T_T<impl_fnma>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_fnma>.Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c)
            => c - a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_fnma>.Scalar_Float<TScalar>(TScalar a, TScalar b, TScalar c)
            => math.fnma(a, b, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_fnma>.Simd_Number<TVector, TScalar>(
            Vector128<TScalar> a, Vector128<TScalar> b, Vector128<TScalar> c
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(simd.Fnma(a.AsSingle(), b.AsSingle(), c.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Fnma(a.AsDouble(), b.AsDouble(), c.AsDouble()).AsByte());

            // the padding lanes of the register are zero on both sides, so the fused value of them stays zero
            return TVector.UnsafeFromUnderlying((c - a * b).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_fnma>.Simd_Number<TVector, TScalar>(
            Vector256<TScalar> a, Vector256<TScalar> b, Vector256<TScalar> c
        )
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Fnma(a.AsDouble(), b.AsDouble(), c.AsDouble()).AsByte());

            return TVector.UnsafeFromUnderlying((c - a * b).AsByte());
        }
    }
}
