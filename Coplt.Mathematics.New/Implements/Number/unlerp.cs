using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics;

public static partial class math_ex
{
    extension(math)
    {
        /// <summary>
        /// Returns where <paramref name="value"/> is between <paramref name="start"/> and <paramref name="end"/>,
        /// it is the inverse of <see cref="math_ex.lerp{T}(T, T, T)"/>
        /// </summary>
        /// <param name="value">The value to place</param>
        /// <param name="start">The value at t = 0</param>
        /// <param name="end">The value at t = 1</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The position of the value between the two values</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T unlerp<T>(T value, T start, T end) where T : unmanaged, INumberAlgebra<T>
            => (value - start) / (end - start);
    }
}

public static partial class math
{
    [ScalarExtension]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [OverloadResolutionPriority(-1)]
    public static T unlerp<T, TScalar>(T value, TScalar start, TScalar end)
        where T : unmanaged, INumberAlgebra<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => (value - T.Broadcast(start)) / T.Broadcast(end - start);
}

public static partial class scalar_math_ex
{
    /// <inheritdoc cref="math_ex.unlerp{T}(T, T, T)"/>
    [OverloadResolutionPriority(-1)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T unlerp<T>(this T value, T start, T end) where T : unmanaged, INumberAlgebra<T>
        => (value - start) / (end - start);

    /// <summary>
    /// Returns where the component <paramref name="value"/> is between the components of
    /// <paramref name="start"/> and <paramref name="end"/>, it is the inverse of
    /// <see cref="math.lerp{T, TScalar}(TScalar, TScalar, T)"/>
    /// </summary>
    /// <param name="value">The component to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <typeparam name="T">The type of the bounds, a vector or a matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The position of the component between the two values</returns>
    [OverloadResolutionPriority(-1)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T unlerp<T, TScalar>(this TScalar value, T start, T end) where T : unmanaged, INumberAlgebra<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => math.unlerp(T.Broadcast(value), start, end);
}
