using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Returns the squared length of the value, which is the sum of the squares of its components
    /// <para>It is the dot product of the value with itself, see <see cref="dot{T,TScalar}(in T, in T)"/>, the
    /// length of the value is the square root of it</para>
    /// </summary>
    /// <param name="value">The value, a vector</param>
    /// <typeparam name="T">The type of the value, a vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The squared length of the value</returns>
    [ScalarExtension]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar length_sq<T, TScalar>(in T value)
        where T : unmanaged, INumberAlgebraDispatch<T, TScalar>, INumberVector<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => dot<T, TScalar>(value, value);

    /// <summary>
    /// Returns the squared distance between the two values, which is the squared length of the difference of them,
    /// see <see cref="length_sq{T,TScalar}(in T)"/>
    /// </summary>
    /// <param name="from">The value to measure from</param>
    /// <param name="to">The value to measure to</param>
    /// <typeparam name="T">The type of the values, a vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The squared distance between the two values</returns>
    [ScalarExtension]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar distance_sq<T, TScalar>(in T from, in T to)
        where T : unmanaged, INumberAlgebraDispatch<T, TScalar>, INumberVector<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => length_sq<T, TScalar>(to - from);
}
