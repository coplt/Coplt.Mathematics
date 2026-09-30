using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the natural logarithm of every component
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// <para>The member of the simd library of the register of a value exists for a float and a double, the
        /// value of every other kind of a floating point number, which is a half, has no register and reaches
        /// the member of the component type</para>
        /// <para>The logarithm of the zero of a padding lane is the negative infinity, so the register of the
        /// result is built from the mask of the padding lanes of the value, which keeps them at zero</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the natural logarithm of the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T log<T>(in T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_log>(value);

        /// <summary>
        /// Returns the logarithm of every component with <paramref name="b"/> as the base
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// <para>The logarithm of any base is the quotient of the two natural logarithms, which the member of
        /// the simd library of the register of a value computes for a float and a double and the member of the
        /// component type computes for every kind of a floating point number</para>
        /// </summary>
        /// <param name="a">The value</param>
        /// <param name="b">The base of the logarithm</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the logarithm of the component of it with the matching
        /// component of the base as its base</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T log<T>(in T a, in T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_log_base>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.log{T}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T log<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_log>(value);

        /// <inheritdoc cref="math.log{T}(in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T log<T>(this T a, in T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_log_base>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the natural logarithm of the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The natural logarithm of the zero of a padding lane is the negative infinity, so the register of
    /// the result is built from the mask of the padding lanes of the value, which keeps them at zero</para>
    /// </summary>
    internal struct impl_log : IAlgebraVisitor_T_T<impl_log>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_log>.Scalar_Float<TScalar>(TScalar value)
            => TScalar.Log(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_log>.Simd_Float<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Log(vector.AsSingle()).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Log(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_log>.Simd_Float<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Log(vector.AsDouble()).AsByte());

            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// The value whose every component is the logarithm of the component of it with the matching component of
    /// the base as its base
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The quotient of the two logarithms is not a value of the register of a vector of 2 or 3
    /// components where the components of the base are zero, so the register of the result is built from the
    /// mask of the padding lanes of the value, which keeps them at zero</para>
    /// </summary>
    internal struct impl_log_base : IAlgebraVisitor_T_T_T<impl_log_base>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_log_base>.Scalar_Float<TScalar>(TScalar a, TScalar b)
            => TScalar.Log(a) / TScalar.Log(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_log_base>.Simd_Float<TVector, TScalar>(
            in Vector128<TScalar> a, in Vector128<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying((simd.Log(a.AsSingle()) / simd.Log(b.AsSingle())).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying((simd.Log(a.AsDouble()) / simd.Log(b.AsDouble())).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_log_base>.Simd_Float<TVector, TScalar>(
            in Vector256<TScalar> a, in Vector256<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying((simd.Log(a.AsDouble()) / simd.Log(b.AsDouble())).AsByte());

            throw new NotSupportedException();
        }
    }
}
