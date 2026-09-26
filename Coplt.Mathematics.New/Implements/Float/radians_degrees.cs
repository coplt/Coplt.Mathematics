using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Converts every component from degrees to radians, which is the component of the value multiplied by
        /// <c>DegToRad</c>, the factor of the kind of it
        /// <code>degrees -> radians</code>
        /// </summary>
        /// <param name="value">The value in degrees</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value in radians</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T radians<T>(in T value) where T : unmanaged, IFloatingPointAlgebra<T>
            => value * T.DegToRad;

        /// <summary>
        /// Converts every component from radians to degrees, which is the component of the value multiplied by
        /// <c>RadToDeg</c>, the factor of the kind of it
        /// <code>radians -> degrees</code>
        /// </summary>
        /// <param name="value">The value in radians</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the component of the value in degrees</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T degrees<T>(in T value) where T : unmanaged, IFloatingPointAlgebra<T>
            => value * T.RadToDeg;
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.radians{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T radians<T>(this T value) where T : unmanaged, IFloatingPointAlgebra<T>
            => value * T.DegToRad;

        /// <inheritdoc cref="math.degrees{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T degrees<T>(this T value) where T : unmanaged, IFloatingPointAlgebra<T>
            => value * T.RadToDeg;
    }
}
