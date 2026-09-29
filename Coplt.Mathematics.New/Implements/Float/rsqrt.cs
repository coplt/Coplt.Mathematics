using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the reciprocal of the square root of every component, it is the same as
        /// <c>rcp(sqrt(value))</c>
        /// <para>It is the quotient of the one of the kind of the value by the square root of it, so it is the
        /// exact result and the estimate of the hardware is <see cref="rsqrt_a{T}"/> instead</para>
        /// <para>The reciprocal of the square root of the zero of a component is an infinity</para>
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the reciprocal of the square root of the component of
        /// it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T rsqrt<T>(in T value) where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointAlgebra<T>
            => T.One / sqrt(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.rsqrt{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T rsqrt<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointAlgebra<T>
            => T.One / sqrt(value);
    }
}
