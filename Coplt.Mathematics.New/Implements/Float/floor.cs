using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Rounds every component down to the largest integral value that is not greater than it
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value rounded down</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T floor<T>(in T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_floor>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.floor{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T floor<T>(this T value) where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_floor>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is rounded down
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which rounds it with the simd member of the hardware, the value of every other
    /// vector reaches the member of the scalar for every component of it and the value of a matrix reaches it
    /// for every component of every one of its columns. A padding lane holds no component and its bits are zero,
    /// which is what the rounding of it is as well, so the register of the value can be built from it directly
    /// </para>
    /// </summary>
    internal struct impl_floor : IFloatingPointAlgebraVisitor_Self_Self<impl_floor>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self<impl_floor>.AcceptScalar<TScalar>(TScalar value)
            => TScalar.Floor(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_floor>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.Floor(vector.AsSingle()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_floor>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector128.Floor(vector.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.Floor(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self<impl_floor>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.Floor(vector.AsDouble()).AsByte());
            throw new NotSupportedException();
        }
    }
}
