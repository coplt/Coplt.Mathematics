using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Interpolates between the components <paramref name="start"/> and <paramref name="end"/>, the value
        /// of every component of <paramref name="t"/> is the interpolation factor of it
        /// </summary>
        /// <param name="start">The component at t = 0</param>
        /// <param name="end">The component at t = 1</param>
        /// <param name="t">The value, every component of it is the interpolation factor of the component</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The interpolated value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lerp<T, TScalar>(TScalar start, TScalar end, in T t) where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Visit_Self<impl_lerp<TScalar>>(t, start, end);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.lerp{T, TScalar}(TScalar, TScalar, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lerp<T, TScalar>([In] this ref T t, TScalar start, TScalar end) where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Visit_Self<impl_lerp<TScalar>>(t, start, end);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is interpolated between the two components
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_lerp<TScalar> : INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>.AcceptScalar(
            TScalar t, TScalar start, TScalar end
        ) => start + t * (end - start);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>.AcceptVector<TVector>(
            in Vector64<TScalar> t, TScalar start, TScalar end
        )
        {
            var offset = end - start;

            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.FusedMultiplyAdd(
                    t.AsSingle(),
                    Vector64.Shuffle(Vector64.CreateScalarUnsafe(offset).AsSingle(), default),
                    Vector64.Shuffle(Vector64.CreateScalarUnsafe(start).AsSingle(), default)
                ).AsByte());

            return TVector.FromUnderlying((t * Vector64.Create(offset) + Vector64.Create(start)).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>.AcceptVector<TVector>(
            in Vector128<TScalar> t, TScalar start, TScalar end
        )
        {
            // the value of a vector of 2 components fills the lower half of its register, so its padding
            // lanes are the upper two, the one of a vector of 3 components fills all but the last one
            var broadcast = TVector.HavePaddingLanes
                ? TVector.Rows == 2 ? Vector128.Create(0, 0, 3, 3) : Vector128.Create(0, 0, 0, 3)
                : default;

            var offset = end - start;

            if (typeof(TScalar) == typeof(float))
            {
                return TVector.UnsafeFromUnderlying(Vector128.FusedMultiplyAdd(
                    t.AsSingle(),
                    Vector128.Shuffle(Scalar.Register128(offset).AsSingle(), broadcast),
                    Vector128.Shuffle(Scalar.Register128(start).AsSingle(), broadcast)
                ).AsByte());
            }

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.FusedMultiplyAdd(
                    t.AsDouble(),
                    Vector128.Shuffle(Scalar.Register128(offset).AsDouble(), default),
                    Vector128.Shuffle(Scalar.Register128(start).AsDouble(), default)
                ).AsByte());

            return TVector.UnsafeFromUnderlying((
                t *
                Vector128.Shuffle(Scalar.Register128(offset).AsInt32(), broadcast).As<int, TScalar>() +
                Vector128.Shuffle(Scalar.Register128(start).AsInt32(), broadcast).As<int, TScalar>()
            ).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>.AcceptVector<TVector>(
            in Vector256<TScalar> t, TScalar start, TScalar end
        )
        {
            var broadcast = TVector.HavePaddingLanes ? Vector256.Create(0, 0, 0, 3) : default;

            var offset = end - start;

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.FusedMultiplyAdd(
                    t.AsDouble(),
                    Vector256.Shuffle(Scalar.Register256(offset).AsDouble(), broadcast),
                    Vector256.Shuffle(Scalar.Register256(start).AsDouble(), broadcast)
                ).AsByte());

            return TVector.UnsafeFromUnderlying((
                t *
                Vector256.Shuffle(Scalar.Register256(offset).AsInt64(), broadcast).As<long, TScalar>() +
                Vector256.Shuffle(Scalar.Register256(start).AsInt64(), broadcast).As<long, TScalar>()
            ).AsByte());
        }
    }
}
