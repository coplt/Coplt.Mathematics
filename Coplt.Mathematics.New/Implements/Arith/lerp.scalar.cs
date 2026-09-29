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
        public static T lerp<T, TScalar>(TScalar start, TScalar end, in T t) where T : unmanaged, IAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Self<impl_lerp_scalar>(t, start, end);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.lerp{T, TScalar}(TScalar, TScalar, in T)"/>
        [OverloadResolutionPriority(-2)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lerp<T, TScalar>(this T t, TScalar start, TScalar end) where T : unmanaged, IAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Self<impl_lerp_scalar>(t, start, end);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is interpolated between the two components
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// <para>The visitor does not name the type of a single component: the members of it that reach a value name
    /// it themselves, so one visitor serves every kind of a component</para>
    /// </summary>
    internal struct impl_lerp_scalar : IAlgebraVisitor_T_S_S_T<impl_lerp_scalar>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S_S<impl_lerp_scalar>.Scalar_Number<TScalar>(
            TScalar t, TScalar start, TScalar end
        ) => start + t * (end - start);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_S_S_T<impl_lerp_scalar>.Simd_Number<TVector, TScalar>(
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
        static TVector IAlgebraVisitor_T_S_S_T<impl_lerp_scalar>.Simd_Number<TVector, TScalar>(
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
