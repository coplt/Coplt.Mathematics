using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the sine and the cosine of every component in radians
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the framework and computes the two of them
        /// together, the value of every other vector reaches the member of the scalar for every component of it
        /// and the value of a matrix reaches it for every component of every one of its columns</para>
        /// <para>The pair is the member of the dispatch that hands the value over once and answers with the two
        /// values it built out of it, which is the same member the pair that receives them reaches</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the sine of the component of it and the value whose
        /// every component is the cosine of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T sin, T cos) sincos<T>(T value) where T : unmanaged, IFloatDispatch<T>
        {
            T.Self_out<impl_sincos>(value, out var sin, out var cos);
            return (sin, cos);
        }

        /// <summary>
        /// Computes the sine and the cosine of every component in radians
        /// <para>The two results are the same as the ones of <see cref="sincos{T}(T)"/>, the value of every
        /// component that is no result is not built</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="sin">Receives the value whose every component is the sine of the component of it</param>
        /// <param name="cos">Receives the value whose every component is the cosine of the component of it</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos<T>(T value, out T sin, out T cos) where T : unmanaged, IFloatDispatch<T>
            => T.Self_out<impl_sincos>(value, out sin, out cos);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.sincos{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T sin, T cos) sincos<T>(this T value) where T : unmanaged, IFloatDispatch<T>
            => math.sincos(value);

        /// <inheritdoc cref="math.sincos{T}(T, out T, out T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos<T>(this T value, out T sin, out T cos) where T : unmanaged, IFloatDispatch<T>
            => math.sincos(value, out sin, out cos);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The pair of the value whose every component is the sine of the component of it and of the value whose
    /// every component is the cosine of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the framework and builds the two of them together, the
    /// value of every other vector reaches the member of the scalar for every component of it, which is the one
    /// of the component type, and the value of a matrix reaches it for every component of every one of its
    /// columns</para>
    /// <para>The member of the framework of the register of a value exists for a float and a double, the value
    /// of every other kind of a floating point number, which is a half, has no register and reaches the member
    /// of the component type</para>
    /// <para>The cosine of the zero of a padding lane is the one of the kind of the component, so the registers
    /// of the both results are built from the mask of the padding lanes of the value, which keeps them at zero
    /// </para>
    /// </summary>
    internal struct impl_sincos : IFloatVisitor_T_outT_outT_void<impl_sincos>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void IFloatVisitor_S_outS_outS_void<impl_sincos>.Scalar_Float<TScalar>(
            TScalar a, out TScalar b, out TScalar c
        )
        {
            var (sin, cos) = TScalar.SinCos(a);
            b = sin;
            c = cos;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void IFloatVisitor_T_outT_outT_void<impl_sincos>.Simd_Float<TVector, TScalar>(
            Vector128<TScalar> a, out TVector b, out TVector c
        )
        {
            if (typeof(TScalar) == typeof(float))
            {
                var (s, co) = Vector128.SinCos(a.AsSingle());
                b = TVector.FromUnderlying(s.AsByte());
                c = TVector.FromUnderlying(co.AsByte());
                return;
            }

            if (typeof(TScalar) == typeof(double))
            {
                var (s, co) = Vector128.SinCos(a.AsDouble());
                b = TVector.FromUnderlying(s.AsByte());
                c = TVector.FromUnderlying(co.AsByte());
                return;
            }

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void IFloatVisitor_T_outT_outT_void<impl_sincos>.Simd_Float<TVector, TScalar>(
            Vector256<TScalar> a, out TVector b, out TVector c
        )
        {
            if (typeof(TScalar) == typeof(double))
            {
                var (s, co) = Vector256.SinCos(a.AsDouble());
                b = TVector.FromUnderlying(s.AsByte());
                c = TVector.FromUnderlying(co.AsByte());
                return;
            }

            throw new NotSupportedException();
        }
    }
}
