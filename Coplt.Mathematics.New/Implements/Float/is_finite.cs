using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns a mask that is true where the component is finite, so it is neither NaN nor an infinity
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which builds the mask out of the register of the mask itself,
        /// the value of every other vector reaches the member of the scalar for every component of it and the
        /// value of a matrix reaches it for every component of every one of its columns</para>
        /// </summary>
        /// <param name="a">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <typeparam name="TBool">The type of the mask, the value that has the same shape as <typeparamref name="T"/></typeparam>
        /// <returns>The mask</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BoolExtension]
        public static TBool is_finite<T, TBool>(in T a)
            where T : unmanaged, IFloatingPointAlgebraBoolDispatch<T, TBool>
            where TBool : unmanaged, IBoolMatrix<TBool>
            => T.Visit_Self_Bool<impl_is_finite>(a);

        /// <summary>
        /// Returns a mask that is true where the component is finite, which is the name of the member in HLSL
        /// </summary>
        /// <inheritdoc cref="is_finite{T,TBool}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BoolExtension]
        public static TBool isfinite<T, TBool>(in T a)
            where T : unmanaged, IFloatingPointAlgebraBoolDispatch<T, TBool>
            where TBool : unmanaged, IBoolMatrix<TBool>
            => is_finite<T, TBool>(a);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The mask of a value that is true where the component of it is finite, so it is neither NaN nor an infinity
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the comparison of the whole register at once, the value of every
    /// other vector reaches the member of the scalar for every component of it and the value of a matrix
    /// reaches it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_is_finite : IFloatingPointAlgebraVisitor_Self_Bool<impl_is_finite>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TBoolScalar IFloatingPointAlgebraVisitor_Self_Bool<impl_is_finite>.AcceptScalar<TScalar, TBoolScalar>(TScalar value)
            => TScalar.IsFinite(value) ? TBoolScalar.True : TBoolScalar.False;

        // the member of the simd library of the kind of the component finds the finite ones, but the zero of a
        // padding lane is finite as well, so the mask of the register leaves the padding lanes out
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TBool IFloatingPointAlgebraVisitor_Self_Bool<impl_is_finite>.AcceptVector<TVector, TScalar, TBool, TBoolScalar>(
            in Vector64<TScalar> vector
        ) => TBool.FromUnderlying(Vector64.IsFinite(vector).AsByte());

        /// <inheritdoc cref="IFloatingPointAlgebraVisitor_Self_Bool{V}.AcceptVector{TVector,TScalar,TBool,TBoolScalar}(in Vector64{TScalar})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TBool IFloatingPointAlgebraVisitor_Self_Bool<impl_is_finite>.AcceptVector<TVector, TScalar, TBool, TBoolScalar>(
            in Vector128<TScalar> vector
        ) => TBool.FromUnderlying(Vector128.IsFinite(vector).AsByte());

        /// <inheritdoc cref="IFloatingPointAlgebraVisitor_Self_Bool{V}.AcceptVector{TVector,TScalar,TBool,TBoolScalar}(in Vector64{TScalar})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TBool IFloatingPointAlgebraVisitor_Self_Bool<impl_is_finite>.AcceptVector<TVector, TScalar, TBool, TBoolScalar>(
            in Vector256<TScalar> vector
        ) => TBool.FromUnderlying(Vector256.IsFinite(vector).AsByte());
    }
}
