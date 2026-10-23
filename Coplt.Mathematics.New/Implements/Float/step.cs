using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the one of the kind of a component where the component of <paramref name="value"/> is not
        /// less than the matching component of <paramref name="threshold"/> and the zero of it where it is less
        /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that
        /// matches the width of the register, which compares the two of them with the member of the framework
        /// for a register, the value of every other vector reaches the member of the scalar for every component
        /// of it and the value of a matrix reaches it for every component of every one of its columns</para>
        /// <para>The step of the two zeroes of a padding lane holds, so the register of the result is built from
        /// the mask of the padding lanes of the value, which keeps them at zero</para>
        /// </summary>
        /// <param name="threshold">The threshold of every component</param>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the one of the kind of it or the zero of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T step<T>(T threshold, T value) where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>
            => math.select(value >= threshold, T.One, T.Zero);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.step{T}(T, T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T step<T>(this T value, T threshold) where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>
            => math.step(threshold, value);
    }
}
