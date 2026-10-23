using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the value that is true where the component of <paramref name="a"/> is finite, so it is neither
        /// NaN nor an infinity
        /// <para>The value has the same type as the one it was built from: a component of it that is true is the
        /// all bits set value of the kind of the component and a component that is false is the zero of it, so
        /// any value that is not zero counts as true</para>
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which builds the value out of the bits of the register of the
        /// mask, the value of every other vector reaches the member of the scalar for every component of it and
        /// the value of a matrix reaches it for every component of every one of its columns</para>
        /// </summary>
        /// <param name="a">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value that is true where the component of it is finite</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T is_finite<T>(T a) where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointAlgebra<T>
            => T.Self<impl_is_finite>(a);

        /// <summary>
        /// Returns the value that is true where the component of <paramref name="a"/> is finite, which is the
        /// name of the member in HLSL
        /// </summary>
        /// <inheritdoc cref="is_finite{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T isfinite<T>(T a) where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointAlgebra<T>
            => is_finite(a);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value that is true where the component of a value is finite, so it is neither NaN nor an infinity
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the comparison of the whole register at once, the value of every
    /// other vector reaches the member of the scalar for every component of it and the value of a matrix
    /// reaches it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_is_finite : IAlgebraVisitor_T_T<impl_is_finite>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_is_finite>.Scalar_Float<TScalar>(TScalar value)
            => TScalar.IsFinite(value) ? TScalar.AllBitsSet : TScalar.Zero;

        // the member of the simd library of the kind of the component finds the finite ones, but the zero of a
        // padding lane is finite as well, so the value of the register leaves the padding lanes out
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_is_finite>.Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
            => TVector.FromUnderlying(Vector128.IsFinite(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_is_finite>.Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
            => TVector.FromUnderlying(Vector256.IsFinite(vector).AsByte());
    }
}
