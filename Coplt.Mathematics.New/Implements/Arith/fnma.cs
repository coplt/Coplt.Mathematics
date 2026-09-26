using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
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
        public static T fnma<T>(in T a, in T b, in T c) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_fnma>(a, b, c);

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
        public static T fsm<T>(in T c, in T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => fnma(a, b, c);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.fnma{T}(in T, in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T fnma<T>(this T a, in T b, in T c) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_fnma>(a, b, c);

        /// <inheritdoc cref="math.fsm{T}(in T, in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T fsm<T>(this T c, in T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_fnma>(a, b, c);
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
    internal struct impl_fnma : INumberAlgebraVisitor_Self_Self_Self_Self<impl_fnma>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self_Self<impl_fnma>.AcceptScalar<TScalar>(TScalar a, TScalar b, TScalar c)
        {
            if (Vector128.IsHardwareAccelerated || Vector64.IsHardwareAccelerated)
            {
                if (typeof(TScalar) == typeof(float))
                {
                    return (TScalar)(object)float.FusedMultiplyAdd(-(float)(object)a, (float)(object)b, (float)(object)c);
                }

                if (typeof(TScalar) == typeof(double))
                {
                    return (TScalar)(object)double.FusedMultiplyAdd(-(double)(object)a, (double)(object)b, (double)(object)c);
                }
            }

            return c - a * b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_fnma>.AcceptVector<TVector, TScalar>(
            in Vector64<TScalar> a, in Vector64<TScalar> b, in Vector64<TScalar> c
        )
        {
            // a floating point value has a fused member in the hardware, the one of every other value multiplies
            // and subtracts, a 64 bit register is exactly as wide as the value of the vector, so it has no
            // padding lane and the interface of it does not mask one either way
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Fnma(a.AsSingle(), b.AsSingle(), c.AsSingle()).AsByte());

            return TVector.FromUnderlying((c - a * b).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_fnma>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> a, in Vector128<TScalar> b, in Vector128<TScalar> c
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
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_fnma>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> a, in Vector256<TScalar> b, in Vector256<TScalar> c
        )
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(simd.Fnma(a.AsDouble(), b.AsDouble(), c.AsDouble()).AsByte());

            return TVector.UnsafeFromUnderlying((c - a * b).AsByte());
        }
    }
}
