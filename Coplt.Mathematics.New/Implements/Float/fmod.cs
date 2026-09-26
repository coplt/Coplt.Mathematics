using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the remainder of the division of the value by <paramref name="b"/>, it is the same as
        /// <c>a - floor(a / b) * b</c>, so the remainder of a divisor that is negative keeps the sign of the
        /// divisor
        /// </summary>
        /// <param name="a">The value</param>
        /// <param name="b">The divisor</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the remainder of the division of the component of it by
        /// the one of the divisor</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T fmod<T>(in T a, in T b)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_fmod>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.fmod{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T fmod<T>(this T a, in T b)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => T.Visit_Self<impl_fmod>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the remainder of the division of the component of the value by the one
    /// of the divisor, which is the difference of the value and the product of the divisor and the floor of the
    /// quotient of the two
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which fuses the product into the difference, the value of every other vector
    /// reaches the member of the scalar for every component of it and the value of a matrix reaches it for every
    /// component of every one of its columns</para>
    /// <para>The remainder of a padding lane is the one of the zero of it by the zero of the divisor, which is
    /// not a number, so the register of the value is built from the mask of the padding lanes of it, which keeps
    /// them at zero</para>
    /// </summary>
    internal struct impl_fmod : IFloatingPointAlgebraVisitor_Self_Self_Self<impl_fmod>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IFloatingPointAlgebraVisitor_Self_Self_Self<impl_fmod>.AcceptScalar<TScalar>(TScalar a, TScalar b)
            => TScalar.FusedMultiplyAdd(-b, TScalar.Floor(a / b), a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_fmod>.AcceptVector<TVector, TScalar>(
            in Vector64<TScalar> a, in Vector64<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.FusedMultiplyAdd(
                    -b.AsSingle(), Vector64.Floor(a.AsSingle() / b.AsSingle()), a.AsSingle()
                ).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_fmod>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> a, in Vector128<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector128.FusedMultiplyAdd(
                    -b.AsSingle(), Vector128.Floor(a.AsSingle() / b.AsSingle()), a.AsSingle()
                ).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(Vector128.FusedMultiplyAdd(
                    -b.AsDouble(), Vector128.Floor(a.AsDouble() / b.AsDouble()), a.AsDouble()
                ).AsByte());

            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IFloatingPointAlgebraVisitor_Self_Self_Self<impl_fmod>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> a, in Vector256<TScalar> b
        )
        {
            if (typeof(TScalar) == typeof(double))
                return TVector.FromUnderlying(Vector256.FusedMultiplyAdd(
                    -b.AsDouble(), Vector256.Floor(a.AsDouble() / b.AsDouble()), a.AsDouble()
                ).AsByte());

            throw new NotSupportedException();
        }
    }
}
