using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Rounds every component to the nearest integral value, a value that is exactly between two of them is
        /// rounded away from zero
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value rounded</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T round_away<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_round_away>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.round_away{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T round_away<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_round_away>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is rounded to the nearest integral value, a component that is exactly
    /// between two integral values is rounded away from zero
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, the value of every other vector reaches the member of the scalar for every
    /// component of it and the value of a matrix reaches it for every component of every one of its columns. A
    /// padding lane holds no component and its bits are zero, which is what the rounding of it is as well, so the
    /// register of the value can be built from it directly</para>
    /// <para>The member of the hardware rounds a component that is exactly between two integral values to the
    /// even one of them, so the member of the register names the midpoint rounding of this member beside the
    /// member of the hardware</para>
    /// </summary>
    internal struct impl_round_away : IFloatingPointAlgebraVisitor_Self_Self<impl_round_away>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self<impl_round_away>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.Round(value, MidpointRounding.AwayFromZero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_round_away>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.Round(vector.AsSingle(), MidpointRounding.AwayFromZero).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_round_away>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector128.Round(vector.AsSingle(), MidpointRounding.AwayFromZero).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.Round(vector.AsDouble(), MidpointRounding.AwayFromZero).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_round_away>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.Round(vector.AsDouble(), MidpointRounding.AwayFromZero).AsByte());
            throw new NotSupportedException();
        }
    }
}
