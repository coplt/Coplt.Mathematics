using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the larger of the two values component by component
        /// </summary>
        /// <param name="a">The first of the two values</param>
        /// <param name="b">The second of the two values</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The larger of the two values component by component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T max<T>(in T a, in T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_max>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.max{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T max<T>(this T a, in T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Self<impl_max>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The larger of two values
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_max : IAlgebraVisitor_T_T_T<impl_max>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S_S<impl_max>.Scalar_Number<TScalar>(TScalar a, TScalar b)
            => TScalar.Max(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_max>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector128.Max(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T<impl_max>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector256.Max(a, b).AsByte());
    }
}
