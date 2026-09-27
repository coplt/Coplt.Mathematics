using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the square root of every component
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which is the one of the simd library, the value of every other
        /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches
        /// it for every component of every one of its columns</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the square root of the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sqrt<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_sqrt>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.sqrt{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T sqrt<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_sqrt>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the square root of the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the one of the simd library, the value of every other vector reaches
    /// the member of the scalar for every component of it, which is the one of the component type, and the
    /// value of a matrix reaches it for every component of every one of its columns</para>
    /// <para>The square root of the zero of a padding lane is the zero of the kind of the component, so the
    /// result of a register that is wider than the value is taken from the value without masking the padding
    /// lanes of it, and the 64 bit register of a value is exactly as wide as the value and has no padding
    /// lane</para>
    /// </summary>
    internal struct impl_sqrt : IFloatingPointAlgebraVisitor_Self_Self<impl_sqrt>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self<impl_sqrt>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.Sqrt(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_sqrt>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
            => TVector.FromUnderlying(Vector64.Sqrt(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_sqrt>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector128.Sqrt(vector).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_sqrt>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.UnsafeFromUnderlying(Vector256.Sqrt(vector).AsByte());
    }
}
