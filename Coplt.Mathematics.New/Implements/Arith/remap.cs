using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics;

public static partial class math_ex
{
    extension(math)
    {
        /// <summary>
        /// Remaps <paramref name="value"/> from the range <paramref name="src_start"/> to
        /// <paramref name="src_end"/> into the range <paramref name="dst_start"/> to <paramref name="dst_end"/>
        /// </summary>
        /// <param name="value">The value to remap</param>
        /// <param name="src_start">The lower bound of the source range</param>
        /// <param name="src_end">The upper bound of the source range</param>
        /// <param name="dst_start">The lower bound of the destination range</param>
        /// <param name="dst_end">The upper bound of the destination range</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The remapped value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T remap<T>(in T value, in T src_start, in T src_end, in T dst_start, in T dst_end)
            where T : unmanaged, INumberAlgebra<T>, INumberAlgebraDispatch<T>
            => lerp(dst_start, dst_end, unlerp(value, src_start, src_end));
    }
}

public static partial class math
{
    [ScalarExtension]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T remap<T, TScalar>(in T value, TScalar src_start, TScalar src_end, TScalar dst_start, TScalar dst_end)
        where T : unmanaged, INumberAlgebra<T, TScalar>, INumberAlgebraDispatch<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => lerp(dst_start, dst_end, unlerp(value, src_start, src_end));

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static T remap<T, TScalar>(in T value, in T src_start, in T src_end, TScalar dst_start, TScalar dst_end)
    //     where T : unmanaged, INumberAlgebra<T, TScalar>, INumberAlgebraDispatch<T, TScalar>
    //     where TScalar : unmanaged, IBinaryNumber<TScalar>
    //     => lerp(dst_start, dst_end, unlerp<T>(value, src_start, src_end));
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static T remap<T, TScalar>(in T value, TScalar src_start, TScalar src_end, in T dst_start, in T dst_end)
    //     where T : unmanaged, INumberAlgebra<T, TScalar>, INumberAlgebraDispatch<T, TScalar>
    //     where TScalar : unmanaged, IBinaryNumber<TScalar>
    //     => lerp<T>(dst_start, dst_end, unlerp(value, src_start, src_end));
}

public static partial class math_ex
{
    /// <inheritdoc cref="math_ex.remap{T}(in T, in T, in T, in T, in T)"/>
    [OverloadResolutionPriority(-1)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T remap<T>(this T value, in T src_start, in T src_end, in T dst_start, in T dst_end)
        where T : unmanaged, INumberAlgebra<T>, INumberAlgebraDispatch<T>
        => math.lerp<T>(dst_start, dst_end, math.unlerp<T>(value, src_start, src_end));

    /// <summary>
    /// Remaps the component <paramref name="value"/> from the range <paramref name="src_start"/> to
    /// <paramref name="src_end"/> into the range <paramref name="dst_start"/> to <paramref name="dst_end"/>
    /// </summary>
    /// <param name="value">The component to remap</param>
    /// <param name="src_start">The lower bound of the source range</param>
    /// <param name="src_end">The upper bound of the source range</param>
    /// <param name="dst_start">The lower bound of the destination range</param>
    /// <param name="dst_end">The upper bound of the destination range</param>
    /// <typeparam name="T">The type of the bounds, a vector or a matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The remapped component</returns>
    [OverloadResolutionPriority(-1)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T remap<T, TScalar>(this TScalar value, in T src_start, in T src_end, in T dst_start, in T dst_end)
        where T : unmanaged, INumberAlgebra<T, TScalar>, INumberAlgebraDispatch<T>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => math.lerp<T>(dst_start, dst_end, math.unlerp<T>(T.Broadcast(value), src_start, src_end));
}
