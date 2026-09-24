using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// The arithmetic members of the vectors are members of the vector itself, a member of the math class reaches them
// as well. The parameters of every member below are the ones of the interface of its operation in the same order,
// which is the order of the hlsl counterpart of the operation as well: math.abs(v) and math.lerp(start, end, t).
// The members that name the type of a single component are forwarded as well: the compiler infers it from an
// argument when a component of the vector is one of them (math.lerp(0.5f, 1f, v)) and the caller has to spell it
// out when it is only the result (math.dot<float3, float>(a, b)), which is what the members of math.as do too.
public static partial class math
{
    /// <summary>
    /// Returns the dot product of the two vectors
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The sum of the products of the components</returns>
    [MethodImpl(256)]
    public static TScalar dot<T, TScalar>(in T v, in T other)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.dot(v, other);

    /// <summary>
    /// Returns the squared length of the vector, it is the same as <c>dot(self)</c> but avoids the square root
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The squared length</returns>
    [MethodImpl(256)]
    public static TScalar length_sq<T, TScalar>(in T v)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.length_sq(v);

    /// <summary>
    /// Returns the squared distance between the two vectors
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The squared distance</returns>
    [MethodImpl(256)]
    public static TScalar distance_sq<T, TScalar>(in T v, in T other)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.distance_sq(v, other);

    /// <summary>
    /// Returns the sum of all components
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The sum of the components</returns>
    [MethodImpl(256)]
    public static TScalar csum<T, TScalar>(in T v)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.csum(v);

    /// <summary>
    /// Returns the smallest component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The smallest component</returns>
    [MethodImpl(256)]
    public static TScalar cmin<T, TScalar>(in T v)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.cmin(v);

    /// <summary>
    /// Returns the largest component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The largest component</returns>
    [MethodImpl(256)]
    public static TScalar cmax<T, TScalar>(in T v)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.cmax(v);

    /// <summary>
    /// Returns the smallest component
    /// <para>It is safe when the vector has a padding component that is not a part of the vector, it is slower
    /// than <see cref="cmin{T,TScalar}(in T)"/></para>
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The smallest component</returns>
    [MethodImpl(256)]
    public static TScalar cmin_safe<T, TScalar>(in T v)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.cmin_safe(v);

    /// <summary>
    /// Returns the largest component
    /// <para>It is safe when the vector has a padding component that is not a part of the vector, it is slower
    /// than <see cref="cmax{T,TScalar}(in T)"/></para>
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The largest component</returns>
    [MethodImpl(256)]
    public static TScalar cmax_safe<T, TScalar>(in T v)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.cmax_safe(v);
}
