using Coplt.Mathematics.Generics;

namespace Coplt.Experimental.Mathematics;

// The arithmetic members of the vectors are members of the vector itself, a member of the math class reaches them
// as well. None of the members below needs the type of a single component, so they do not have to name it: the
// compiler infers the vector type from the argument and the call is math.abs(v) for a vector of any component
// type. The members that need the component type are the ones of the interfaces that add it and a caller of one of
// them has to spell it out, so they are not forwarded here.
public static partial class math
{
    /// <summary>
    /// Returns the absolute value of every component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The absolute value of the vector</returns>
    [MethodImpl(256)]
    public static T abs<T>(in T v) where T : unmanaged, IVectorArithmetic<T> => v.abs();

    /// <summary>
    /// Returns <c>-1</c>, <c>0</c> or <c>1</c> for every component depending on its sign
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The sign of every component</returns>
    [MethodImpl(256)]
    public static T sign<T>(in T v) where T : unmanaged, IVectorArithmetic<T> => v.sign();

    /// <summary>
    /// Returns the smaller of the two vectors component by component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The component wise minimum</returns>
    [MethodImpl(256)]
    public static T min<T>(in T v, in T other) where T : unmanaged, IVectorArithmetic<T> => v.min(other);

    /// <summary>
    /// Returns the larger of the two vectors component by component
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The component wise maximum</returns>
    [MethodImpl(256)]
    public static T max<T>(in T v, in T other) where T : unmanaged, IVectorArithmetic<T> => v.max(other);

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
    public static T clamp<T>(in T v, in T min, in T max) where T : unmanaged, IVectorArithmetic<T> => v.clamp(min, max);

    /// <summary>
    /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, the vector is the t value
    /// </summary>
    /// <param name="v">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The interpolated vector</returns>
    [MethodImpl(256)]
    public static T lerp<T>(in T v, in T start, in T end) where T : unmanaged, IVectorArithmetic<T> => v.lerp(start, end);

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
    public static T unlerp<T>(in T v, in T start, in T end) where T : unmanaged, IVectorArithmetic<T> => v.unlerp(start, end);

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
        where T : unmanaged, IVectorArithmetic<T> => v.remap(src_start, src_end, dst_start, dst_end);

    /// <summary>
    /// Returns the vector with every component squared
    /// </summary>
    /// <param name="v">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The squared vector</returns>
    [MethodImpl(256)]
    public static T square<T>(in T v) where T : unmanaged, IVectorArithmetic<T> => v.square();

    /// <summary>
    /// Returns the cross product of the two vectors
    /// <code>a.yzx * b.zxy - a.zxy * b.yzx</code>
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="other">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The vector that is perpendicular to both vectors</returns>
    [MethodImpl(256)]
    public static T cross<T>(in T v, in T other) where T : unmanaged, IVector3Arithmetic<T> => v.cross(other);

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
}

