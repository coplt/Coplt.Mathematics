using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the larger of the two values component by component
        /// <para>It is the maximum the platform computes itself, which is the one of <see cref="max{T}(in T, in T)"/>
        /// beside the way it handles a nan and a negative zero: every platform is free to handle the two of them
        /// in a way of its own</para>
        /// </summary>
        /// <param name="a">The first of the two values</param>
        /// <param name="b">The second of the two values</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The larger of the two values component by component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T max_native<T>(in T a, in T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_max_native>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.max_native{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T max_native<T>(this T a, in T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_max_native>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The larger of two values, the one the platform computes itself
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_max_native : IAlgebraVisitor_T_T_T<impl_max_native>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_max_native>.Scalar_Number<TScalar>(TScalar a, TScalar b)
            => TScalar.MaxNative(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_max_native>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector128.MaxNative(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_max_native>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector256.MaxNative(a, b).AsByte());
    }
}
