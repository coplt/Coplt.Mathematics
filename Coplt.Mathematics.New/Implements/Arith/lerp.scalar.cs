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
        [ScalarExtension(ThisParameter = "t")]
        public static T lerp<T, TScalar>(TScalar start, TScalar end, in T t) where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Visit_Self<impl_lerp<TScalar>>(t, start, end);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.lerp{T, TScalar}(TScalar, TScalar, in T)"/>
        [OverloadResolutionPriority(-2)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lerp<T, TScalar>(this T t, TScalar start, TScalar end) where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
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
            // a 64 bit register is exactly as wide as the value of the vector it keeps, so the broadcast of a
            // component of it has no padding lane
            var offset = end - start;

            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.FusedMultiplyAdd(
                    t.AsSingle(),
                    Vector64.Create(offset).AsSingle(),
                    Vector64.Create(start).AsSingle()
                ).AsByte());

            return TVector.FromUnderlying((t * Vector64.Create(offset) + Vector64.Create(start)).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>.AcceptVector<TVector>(
            in Vector128<TScalar> t, TScalar start, TScalar end
        )
        {
            // the broadcast of a component builds the whole register of it and masks the padding lanes of the
            // vector, so the lanes that follow the components of the vector stay zero: a vector whose value
            // fills its register has none of them and skips the mask
            var mask = TVector.PaddingLanesMask.As<byte, TScalar>();

            var offset = end - start;
            var offsetReg = TVector.HavePaddingLanes ? Vector128.Create(offset) & mask : Vector128.Create(offset);
            var startReg = TVector.HavePaddingLanes ? Vector128.Create(start) & mask : Vector128.Create(start);

            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector128.FusedMultiplyAdd(
                    t.AsSingle(),
                    offsetReg.AsSingle(),
                    startReg.AsSingle()
                ).AsByte());

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.FusedMultiplyAdd(
                    t.AsDouble(),
                    offsetReg.AsDouble(),
                    startReg.AsDouble()
                ).AsByte());

            return TVector.UnsafeFromUnderlying((t * offsetReg + startReg).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Scalar_Scalar_Self<impl_lerp<TScalar>, TScalar>.AcceptVector<TVector>(
            in Vector256<TScalar> t, TScalar start, TScalar end
        )
        {
            // the broadcast of a component builds the whole register of it and masks the padding lanes of the
            // vector, so the lanes that follow the components of the vector stay zero: a vector whose value
            // fills its register has none of them and skips the mask
            var mask = TVector.PaddingLanesMask.As<byte, TScalar>();

            var offset = end - start;
            var offsetReg = TVector.HavePaddingLanes ? Vector256.Create(offset) & mask : Vector256.Create(offset);
            var startReg = TVector.HavePaddingLanes ? Vector256.Create(start) & mask : Vector256.Create(start);

            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.FusedMultiplyAdd(
                    t.AsDouble(),
                    offsetReg.AsDouble(),
                    startReg.AsDouble()
                ).AsByte());

            return TVector.UnsafeFromUnderlying((t * offsetReg + startReg).AsByte());
        }
    }
}
