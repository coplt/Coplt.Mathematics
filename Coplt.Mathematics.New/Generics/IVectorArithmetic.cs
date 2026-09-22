namespace Coplt.Mathematics.Generics;

/// <summary>
/// A <see cref="INumberVector{Self}"/> that also supports the element wise arithmetic operators
/// <c>+</c>, <c>-</c>, <c>*</c>, <c>/</c> and <c>%</c> and the common arithmetic helpers
/// <para>It does not name the type of a single component because none of these members needs it, <see
/// cref="IVectorArithmetic{Self,Scalar}"/> adds the members that do</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVectorArithmetic<Self> :
    INumberVector<Self>,
    IUnaryPlusOperators<Self, Self>,
    IAdditionOperators<Self, Self, Self>,
    ISubtractionOperators<Self, Self, Self>,
    IMultiplyOperators<Self, Self, Self>,
    IDivisionOperators<Self, Self, Self>,
    IModulusOperators<Self, Self, Self>
    where Self : unmanaged, IVectorArithmetic<Self>
{
    #region Sign

    /// <summary>
    /// Returns the absolute value of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <returns>The absolute value of the vector</returns>
    public static abstract Self abs(in Self a);

    /// <summary>
    /// Returns <c>-1</c>, <c>0</c> or <c>1</c> for every component depending on its sign
    /// </summary>
    /// <param name="a">The vector</param>
    /// <returns>The sign of every component</returns>
    public static abstract Self sign(in Self a);

    #endregion

    #region Min Max Clamp

    /// <summary>
    /// Returns the smaller of the two vectors component by component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <returns>The component wise minimum</returns>
    public static abstract Self min(in Self a, in Self b);

    /// <summary>
    /// Returns the larger of the two vectors component by component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <returns>The component wise maximum</returns>
    public static abstract Self max(in Self a, in Self b);

    /// <summary>
    /// Clamps every component of <paramref name="a"/> to the inclusive range of <paramref name="min"/> and
    /// <paramref name="max"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <returns>The clamped vector</returns>
    public static abstract Self clamp(in Self a, in Self min, in Self max);

    #endregion

    #region Lerp Unlerp Remap

    /// <summary>
    /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, <paramref name="t"/> is the
    /// interpolation factor
    /// <para>The receiver of the legacy member is the factor, so it is the last parameter here</para>
    /// </summary>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <returns>The interpolated vector</returns>
    public static abstract Self lerp(in Self start, in Self end, in Self t);

    /// <summary>
    /// Returns where <paramref name="a"/> is between <paramref name="start"/> and <paramref name="end"/>, it is
    /// the inverse of <see cref="lerp(in Self, in Self, in Self)"/>
    /// </summary>
    /// <param name="a">The vector to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <returns>The position of the vector between the two values</returns>
    public static abstract Self unlerp(in Self a, in Self start, in Self end);

    /// <summary>
    /// Remaps <paramref name="a"/> from the range <paramref name="src_start"/> to <paramref name="src_end"/>
    /// into the range <paramref name="dst_start"/> to <paramref name="dst_end"/>
    /// </summary>
    /// <param name="a">The vector to remap</param>
    /// <param name="src_start">The lower bound of the source range</param>
    /// <param name="src_end">The upper bound of the source range</param>
    /// <param name="dst_start">The lower bound of the destination range</param>
    /// <param name="dst_end">The upper bound of the destination range</param>
    /// <returns>The remapped vector</returns>
    public static abstract Self remap(in Self a, in Self src_start, in Self src_end, in Self dst_start, in Self dst_end);

    #endregion

    #region Square

    /// <summary>
    /// Returns the vector with every component squared
    /// </summary>
    /// <param name="a">The vector</param>
    /// <returns>The squared vector</returns>
    public static abstract Self square(in Self a);

    #endregion

    #region Fma

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    public static abstract Self fma(in Self a, in Self b, in Self c);

    /// <summary>
    /// Fusion Subtraction and Multiplication
    /// <code>(a * b) - c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Subtrahend c</param>
    public static abstract Self fms(in Self a, in Self b, in Self c);

    /// <summary>
    /// Fusion Multiplication and Subtraction
    /// <code>c - (a * b)</code> or <code>-(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Minuend c</param>
    public static abstract Self fnma(in Self a, in Self b, in Self c);

    /// <summary>
    /// Fusion Multiplication and Subtraction
    /// <code>c - (a * b)</code>
    /// </summary>
    /// <param name="c">Minuend c</param>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual Self fsm(in Self c, in Self a, in Self b) => Self.fnma(a, b, c);

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>c + (a * b)</code>
    /// </summary>
    /// <param name="c">Addend c</param>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual Self fam(in Self c, in Self a, in Self b) => Self.fma(a, b, c);

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual Self mad(in Self a, in Self b, in Self c) => Self.fma(a, b, c);

    #endregion
}

/// <summary>
/// A <see cref="INumberVector{Self,Scalar}"/> that also supports the element wise arithmetic operators
/// <c>+</c>, <c>-</c>, <c>*</c>, <c>/</c> and <c>%</c> and the common arithmetic helpers
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorArithmetic<Self, Scalar> :
    IVectorArithmetic<Self>,
    INumberVector<Self, Scalar>
    where Self : unmanaged, IVectorArithmetic<Self, Scalar>
    where Scalar : unmanaged
{
    #region Min Max Clamp

    /// <summary>
    /// Clamps every component of <paramref name="a"/> to the inclusive range of <paramref name="min"/> and
    /// <paramref name="max"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <returns>The clamped vector</returns>
    public static abstract Self clamp(in Self a, Scalar min, Scalar max);

    #endregion

    #region Lerp Unlerp Remap

    /// <summary>
    /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, <paramref name="t"/> is the
    /// interpolation factor
    /// </summary>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <returns>The interpolated vector</returns>
    public static abstract Self lerp(Scalar start, Scalar end, in Self t);

    /// <summary>
    /// Interpolates between <paramref name="start"/> and <paramref name="end"/>
    /// </summary>
    /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <returns>The interpolated vector</returns>
    public static abstract Self lerp(in Self start, in Self end, Scalar t);

    /// <summary>
    /// Returns where <paramref name="a"/> is between <paramref name="start"/> and <paramref name="end"/>
    /// </summary>
    /// <param name="a">The value to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <returns>The position of <paramref name="a"/> between the two values</returns>
    public static abstract Self unlerp(in Self a, Scalar start, Scalar end);

    /// <summary>
    /// Returns where <paramref name="a"/> is between <paramref name="start"/> and <paramref name="end"/>
    /// </summary>
    /// <param name="a">The value to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <returns>The position of <paramref name="a"/> between the two values</returns>
    public static abstract Self unlerp(Scalar a, in Self start, in Self end);

    /// <summary>
    /// Remaps <paramref name="a"/> from the range <paramref name="src_start"/> to <paramref name="src_end"/> into
    /// the range <paramref name="dst_start"/> to <paramref name="dst_end"/>
    /// </summary>
    /// <param name="a">The vector to remap</param>
    /// <param name="src_start">The lower bound of the source range</param>
    /// <param name="src_end">The upper bound of the source range</param>
    /// <param name="dst_start">The lower bound of the destination range</param>
    /// <param name="dst_end">The upper bound of the destination range</param>
    /// <returns>The remapped vector</returns>
    public static abstract Self remap(in Self a, Scalar src_start, Scalar src_end, Scalar dst_start, Scalar dst_end);

    #endregion

    #region Dot LengthSq DistanceSq

    /// <summary>
    /// Returns the dot product of the two vectors
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <returns>The sum of the products of the components</returns>
    public static abstract Scalar dot(in Self a, in Self b);

    /// <summary>
    /// Returns the squared length of the vector, it is the same as <c>dot(self)</c> but avoids the square root
    /// </summary>
    /// <returns>The squared length</returns>
    public static abstract Scalar length_sq(in Self a);

    /// <summary>
    /// Returns the squared distance between the two vectors
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <returns>The squared distance</returns>
    public static abstract Scalar distance_sq(in Self a, in Self b);

    #endregion

    #region CSum CMin CMax

    /// <summary>
    /// Returns the sum of all components
    /// </summary>
    /// <returns>The sum of the components</returns>
    public static abstract Scalar csum(in Self a);

    /// <summary>
    /// Returns the smallest component
    /// </summary>
    /// <returns>The smallest component</returns>
    public static abstract Scalar cmin(in Self a);

    /// <summary>
    /// Returns the largest component
    /// </summary>
    /// <returns>The largest component</returns>
    public static abstract Scalar cmax(in Self a);

    /// <summary>
    /// Returns the smallest component
    /// <para>It is safe when the vector has a padding component that is not part of the vector, it is slower than <see cref="cmin"/></para>
    /// </summary>
    /// <returns>The smallest component</returns>
    public static abstract Scalar cmin_safe(in Self a);

    /// <summary>
    /// Returns the largest component
    /// <para>It is safe when the vector has a padding component that is not part of the vector, it is slower than <see cref="cmax"/></para>
    /// </summary>
    /// <returns>The largest component</returns>
    public static abstract Scalar cmax_safe(in Self a);

    #endregion
}

/// <summary>
/// An <see cref="IVectorArithmetic{Self}"/> that also has a negative operator
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface ISignedVectorArithmetic<Self> :
    IVectorArithmetic<Self>,
    IUnaryNegationOperators<Self, Self>
    where Self : unmanaged, ISignedVectorArithmetic<Self>;

/// <summary>
/// An <see cref="IVectorArithmetic{Self,Scalar}"/> that also has a negative operator
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface ISignedVectorArithmetic<Self, Scalar> :
    ISignedVectorArithmetic<Self>,
    IVectorArithmetic<Self, Scalar>
    where Self : unmanaged, ISignedVectorArithmetic<Self, Scalar>
    where Scalar : unmanaged;

/// <summary>
/// An <see cref="IVectorArithmetic{Self}"/> of 3 components, it also has the cross product
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVector3Arithmetic<Self> :
    IVectorArithmetic<Self>
    where Self : unmanaged, IVector3Arithmetic<Self>
{
    #region Cross

    /// <summary>
    /// Returns the cross product of the two vectors
    /// <code>a.yzx * b.zxy - a.zxy * b.yzx</code>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <returns>The vector that is perpendicular to both vectors</returns>
    public static abstract Self cross(in Self a, in Self b);

    #endregion
}

/// <summary>
/// An <see cref="IVectorArithmetic{Self,Scalar}"/> of 3 components, it also has the cross product
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVector3Arithmetic<Self, Scalar> :
    IVector3Arithmetic<Self>,
    IVectorArithmetic<Self, Scalar>
    where Self : unmanaged, IVector3Arithmetic<Self, Scalar>
    where Scalar : unmanaged, INumberBase<Scalar>;
