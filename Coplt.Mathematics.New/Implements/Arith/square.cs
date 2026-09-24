using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Returns the value whose every component is squared
    /// </summary>
    /// <param name="value">The value</param>
    /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
    /// <returns>The squared value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T square<T>(in T value) where T : unmanaged, INumberAlgebra<T>
        => value * value;
}

public static partial class math_ex
{
    /// <inheritdoc cref="math.square{T}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T square<T>(this T value) where T : unmanaged, INumberAlgebra<T>
        => value * value;
}
