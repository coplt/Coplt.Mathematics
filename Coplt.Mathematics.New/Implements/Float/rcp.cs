using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the reciprocal of every component, it is the same as <c>1 / self</c>
        /// <para>The value of a vector that keeps it in a register reaches the estimate of the hardware, which
        /// is not exact, every other value reaches the exact division of the component type</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the reciprocal of the component of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T rcp<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_rcp>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.rcp{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T rcp<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_rcp>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the reciprocal of the component of it
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which is the estimate of the hardware, the value of every other vector reaches
    /// the member of the scalar for every component of it and the value of a matrix reaches it for every
    /// component of every one of its columns</para>
    /// <para>The estimate of the hardware is not exact, the one of the arm platform is the loose one of the
    /// three, so a caller that needs the exact reciprocal takes the member of a vector that keeps its value in a
    /// register or the one of the scalar, which divides the one of the kind of a component by it</para>
    /// <para>The reciprocal of a padding lane is an infinity, so the register of the value is built from the
    /// mask of the padding lanes of it, which keeps them at zero</para>
    /// </summary>
    internal struct impl_rcp : IFloatingPointAlgebraVisitor_Self_Self<impl_rcp>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self<impl_rcp>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.One / value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_rcp>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Rcp(vector.AsSingle()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_rcp>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(simd.Rcp(vector.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Rcp(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_rcp>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(simd.Rcp(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }
    }
}
