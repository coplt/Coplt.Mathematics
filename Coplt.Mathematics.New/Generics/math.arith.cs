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
    /// Returns <c>-1</c>, <c>0</c> or <c>1</c> for every component depending on its sign
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The sign of every component</returns>
    [MethodImpl(256)]
    public static T sign<T>(in T v) where T : unmanaged, IVectorArithmetic<T> => T.sign(v);

    /// <summary>
    /// Returns the smaller of the two vectors component by component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The component wise minimum</returns>
    [MethodImpl(256)]
    public static T min<T>(in T v, in T other) where T : unmanaged, IVectorArithmetic<T> => T.min(v, other);

    /// <summary>
    /// Returns the larger of the two vectors component by component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The component wise maximum</returns>
    [MethodImpl(256)]
    public static T max<T>(in T v, in T other) where T : unmanaged, IVectorArithmetic<T> => T.max(v, other);

    /// <summary>
    /// Clamps every component of the vector to the inclusive range of <paramref name="min"/> and
    /// <paramref name="max"/>
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The clamped vector</returns>
    [MethodImpl(256)]
    public static T clamp<T>(in T v, in T min, in T max) where T : unmanaged, IVectorArithmetic<T> => T.clamp(v, min, max);

    /// <summary>
    /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, <paramref name="t"/> is the
    /// interpolation factor
    /// </summary>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The interpolated vector</returns>
    [MethodImpl(256)]
    public static T lerp<T>(in T start, in T end, in T t) where T : unmanaged, IVectorArithmetic<T> => T.lerp(start, end, t);

    /// <summary>
    /// Returns where the vector is between <paramref name="start"/> and <paramref name="end"/>, it is the inverse
    /// of <see cref="lerp{T}(in T, in T, in T)"/>
    /// </summary>
    /// <param name="v">The vector to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The position of the vector between the two values</returns>
    [MethodImpl(256)]
    public static T unlerp<T>(in T v, in T start, in T end) where T : unmanaged, IVectorArithmetic<T> => T.unlerp(v, start, end);

    /// <summary>
    /// Remaps the vector from the range <paramref name="src_start"/> to <paramref name="src_end"/> into the range
    /// <paramref name="dst_start"/> to <paramref name="dst_end"/>
    /// </summary>
    /// <param name="v">The vector to remap</param>
    /// <param name="src_start">The lower bound of the source range</param>
    /// <param name="src_end">The upper bound of the source range</param>
    /// <param name="dst_start">The lower bound of the destination range</param>
    /// <param name="dst_end">The upper bound of the destination range</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The remapped vector</returns>
    [MethodImpl(256)]
    public static T remap<T>(in T v, in T src_start, in T src_end, in T dst_start, in T dst_end)
        where T : unmanaged, IVectorArithmetic<T> => T.remap(v, src_start, src_end, dst_start, dst_end);

    /// <summary>
    /// Returns the vector with every component squared
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The squared vector</returns>
    [MethodImpl(256)]
    public static T square<T>(in T v) where T : unmanaged, IVectorArithmetic<T> => T.square(v);

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fused result</returns>
    [MethodImpl(256)]
    public static T fma<T>(in T a, in T b, in T c) where T : unmanaged, IVectorArithmetic<T> => T.fma(a, b, c);

    /// <summary>
    /// Fusion Subtraction and Multiplication
    /// <code>(a * b) - c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Subtrahend c</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fused result</returns>
    [MethodImpl(256)]
    public static T fms<T>(in T a, in T b, in T c) where T : unmanaged, IVectorArithmetic<T> => T.fms(a, b, c);

    /// <summary>
    /// Fusion Multiplication and Subtraction
    /// <code>c - (a * b)</code> or <code>-(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Minuend c</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fused result</returns>
    [MethodImpl(256)]
    public static T fnma<T>(in T a, in T b, in T c) where T : unmanaged, IVectorArithmetic<T> => T.fnma(a, b, c);

    /// <summary>
    /// Fusion Multiplication and Subtraction
    /// <code>c - (a * b)</code>
    /// </summary>
    /// <param name="c">Minuend c</param>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fused result</returns>
    [MethodImpl(256)]
    public static T fsm<T>(in T c, in T a, in T b) where T : unmanaged, IVectorArithmetic<T> => T.fsm(c, a, b);

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>c + (a * b)</code>
    /// </summary>
    /// <param name="c">Addend c</param>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fused result</returns>
    [MethodImpl(256)]
    public static T fam<T>(in T c, in T a, in T b) where T : unmanaged, IVectorArithmetic<T> => T.fam(c, a, b);

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fused result</returns>
    [MethodImpl(256)]
    public static T mad<T>(in T a, in T b, in T c) where T : unmanaged, IVectorArithmetic<T> => T.mad(a, b, c);

    /// <summary>
    /// Clamps every component of the vector to the inclusive range of <paramref name="min"/> and
    /// <paramref name="max"/>, the bounds are the same for every component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The clamped vector</returns>
    [MethodImpl(256)]
    public static T clamp<T, TScalar>(in T v, TScalar min, TScalar max)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.clamp(v, min, max);

    /// <summary>
    /// Interpolates between the two bounds, <paramref name="t"/> is the interpolation factor
    /// </summary>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <param name="t">The interpolation factor, the vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The interpolated vector</returns>
    [MethodImpl(256)]
    public static T lerp<T, TScalar>(TScalar start, TScalar end, in T t)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.lerp(start, end, t);

    /// <summary>
    /// Interpolates between the two vectors, <paramref name="t"/> is the interpolation factor
    /// </summary>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The interpolated vector</returns>
    [MethodImpl(256)]
    public static T lerp<T, TScalar>(in T start, in T end, TScalar t)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.lerp(start, end, t);

    /// <summary>
    /// Returns where the vector is between <paramref name="start"/> and <paramref name="end"/>, it is the inverse
    /// of <see cref="lerp{T,TScalar}(TScalar, TScalar, in T)"/>
    /// </summary>
    /// <param name="v">The vector to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The position of the vector between the two values</returns>
    [MethodImpl(256)]
    public static T unlerp<T, TScalar>(in T v, TScalar start, TScalar end)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.unlerp(v, start, end);

    /// <summary>
    /// Returns where the scalar is between <paramref name="start"/> and <paramref name="end"/>
    /// </summary>
    /// <param name="v">The value to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The position of the value between the two vectors</returns>
    [MethodImpl(256)]
    public static T unlerp<T, TScalar>(TScalar v, in T start, in T end)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.unlerp(v, start, end);

    /// <summary>
    /// Remaps the vector from the range <paramref name="src_start"/> to <paramref name="src_end"/> into the range
    /// <paramref name="dst_start"/> to <paramref name="dst_end"/>, the bounds are the same for every component
    /// </summary>
    /// <param name="v">The vector to remap</param>
    /// <param name="src_start">The lower bound of the source range</param>
    /// <param name="src_end">The upper bound of the source range</param>
    /// <param name="dst_start">The lower bound of the destination range</param>
    /// <param name="dst_end">The upper bound of the destination range</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The remapped vector</returns>
    [MethodImpl(256)]
    public static T remap<T, TScalar>(in T v, TScalar src_start, TScalar src_end, TScalar dst_start, TScalar dst_end)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged => T.remap(v, src_start, src_end, dst_start, dst_end);

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
