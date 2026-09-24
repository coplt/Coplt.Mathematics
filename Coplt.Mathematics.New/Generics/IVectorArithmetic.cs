namespace Coplt.Mathematics.Generics;

/// <summary>
/// A <see cref="INumberVector{Self}"/> that also supports the element wise arithmetic operators
/// <c>+</c>, <c>-</c>, <c>*</c>, <c>/</c> and <c>%</c>
/// <para>The members that work on a whole value instead of a component of it are no longer declared here: they
/// are the members of the <c>math</c> class that dispatch the value of an algebra, see <c>math.abs</c> and the
/// extension of a value that reaches the same member</para>
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
    #region Clamp Lerp Unlerp Remap

    /// <summary>
    /// Clamps every component of <paramref name="a"/> to the inclusive range of <paramref name="min"/> and
    /// <paramref name="max"/>
    /// <para>The bounds are the same for every component, the dispatch of an algebra does not implement this
    /// member yet</para>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <returns>The clamped vector</returns>
    public static abstract Self clamp(in Self a, Scalar min, Scalar max);

    /// <summary>
    /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, <paramref name="t"/> is the
    /// interpolation factor
    /// <para>The factor is the same for every component, the dispatch of an algebra does not implement this
    /// member yet</para>
    /// </summary>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
    /// <returns>The interpolated vector</returns>
    public static abstract Self lerp(in Self start, in Self end, Scalar t);

    /// <summary>
    /// Returns where <paramref name="a"/> is between <paramref name="start"/> and <paramref name="end"/>
    /// <para>The bounds are the same for every component, the dispatch of an algebra does not implement this
    /// member yet</para>
    /// </summary>
    /// <param name="a">The value to place</param>
    /// <param name="start">The value at t = 0</param>
    /// <param name="end">The value at t = 1</param>
    /// <returns>The position of <paramref name="a"/> between the two values</returns>
    public static abstract Self unlerp(in Self a, Scalar start, Scalar end);

    /// <summary>
    /// Remaps <paramref name="a"/> from the range <paramref name="src_start"/> to <paramref name="src_end"/> into
    /// the range <paramref name="dst_start"/> to <paramref name="dst_end"/>
    /// <para>The bounds are the same for every component, the dispatch of an algebra does not implement this
    /// member yet</para>
    /// </summary>
    /// <param name="a">The vector to remap</param>
    /// <param name="src_start">The lower bound of the source range</param>
    /// <param name="src_end">The upper bound of the source range</param>
    /// <param name="dst_start">The lower bound of the destination range</param>
    /// <param name="dst_end">The upper bound of the destination range</param>
    /// <returns>The remapped vector</returns>
    public static abstract Self remap(in Self a, Scalar src_start, Scalar src_end, Scalar dst_start, Scalar dst_end);

    #endregion

    #region LengthSq DistanceSq

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
/// An <see cref="IVectorArithmetic{Self}"/> of 3 components
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVector3Arithmetic<Self> :
    IVectorArithmetic<Self>
    where Self : unmanaged, IVector3Arithmetic<Self>
{
}

/// <summary>
/// An <see cref="IVectorArithmetic{Self,Scalar}"/> of 3 components
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVector3Arithmetic<Self, Scalar> :
    IVector3Arithmetic<Self>,
    IVectorArithmetic<Self, Scalar>
    where Self : unmanaged, IVector3Arithmetic<Self, Scalar>
    where Scalar : unmanaged, INumberBase<Scalar>;
