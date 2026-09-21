namespace Coplt.Mathematics.Generics;

/// <summary>
/// An <see cref="IVectorFloatingPointIeee754{Self}"/> that can also produce a bool vector, it adds the checks
/// for the special floating point values
/// <para>It does not name the type of a single component because none of these members needs it, <see
/// cref="IVectorFloatingPointIeee754BoolOps{Self,Scalar,BoolVector}"/> adds the members that do</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorFloatingPointIeee754BoolOps<Self, out BoolVector> :
    IVectorFloatingPointIeee754<Self>
    where Self : unmanaged, IVectorFloatingPointIeee754BoolOps<Self, BoolVector>
{
    #region IsNaN IsFinite IsInfinity IsPositiveInfinity

    /// <summary>
    /// Returns a mask that is true where the component is NaN
    /// </summary>
    /// <returns>The mask</returns>
    public static abstract BoolVector is_NaN(in Self a);

    /// <summary>
    /// Returns a mask that is true where the component is finite, so it is neither NaN nor an infinity
    /// </summary>
    /// <returns>The mask</returns>
    public static abstract BoolVector is_finite(in Self a);

    /// <summary>
    /// Returns a mask that is true where the component is a positive or a negative infinity
    /// </summary>
    /// <returns>The mask</returns>
    public static abstract BoolVector is_inf(in Self a);

    /// <summary>
    /// Returns a mask that is true where the component is a positive infinity
    /// </summary>
    /// <returns>The mask</returns>
    public static abstract BoolVector is_pos_inf(in Self a);

    /// <summary>
    /// Returns a mask that is true where the component is a negative infinity
    /// </summary>
    /// <returns>The mask</returns>
    public static abstract BoolVector is_neg_inf(in Self a);

    #endregion
}

/// <summary>
/// A <see cref="IVectorFloatingPointIeee754{Self,Scalar}"/> that can also produce a bool vector, it adds the
/// checks for the special floating point values
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorFloatingPointIeee754BoolOps<Self, Scalar, out BoolVector> :
    IVectorFloatingPointIeee754BoolOps<Self, BoolVector>,
    IVectorFloatingPointIeee754<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPointIeee754BoolOps<Self, Scalar, BoolVector>
    where Scalar : unmanaged;

/// <summary>
/// A <see cref="IVectorFloatingPoint{Self}"/> with the ieee 754 math functions
/// <para>It does not name the type of a single component because none of these members needs it, <see
/// cref="IVectorFloatingPointIeee754{Self,Scalar}"/> adds the members that do</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVectorFloatingPointIeee754<Self> :
    IVectorFloatingPoint<Self>
    where Self : unmanaged, IVectorFloatingPointIeee754<Self>
{
    #region Log

    /// <summary>
    /// Returns the natural logarithm of every component
    /// </summary>
    /// <returns>The natural logarithm</returns>
    public static abstract Self log(in Self a);

    /// <summary>
    /// Returns the base 2 logarithm of every component
    /// </summary>
    /// <returns>The base 2 logarithm</returns>
    public static abstract Self log2(in Self a);

    /// <summary>
    /// Returns the logarithm of every component with <paramref name="b"/> as the base
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The base of the logarithm</param>
    /// <returns>The logarithm</returns>
    public static abstract Self log(in Self a, in Self b);

    /// <summary>
    /// Returns the base 10 logarithm of every component
    /// </summary>
    /// <returns>The base 10 logarithm</returns>
    public static abstract Self log10(in Self a);

    #endregion

    #region Exp

    /// <summary>
    /// Returns <c>e</c> raised to the power of every component
    /// </summary>
    /// <returns>The exponential</returns>
    public static abstract Self exp(in Self a);

    /// <summary>
    /// Returns 2 raised to the power of every component
    /// </summary>
    /// <returns>The exponential</returns>
    public static abstract Self exp2(in Self a);

    /// <summary>
    /// Returns 10 raised to the power of every component
    /// </summary>
    /// <returns>The exponential</returns>
    public static abstract Self exp10(in Self a);

    #endregion

    #region Pow Sqrt RSqrt

    /// <summary>
    /// Returns every component raised to the power of the matching component of <paramref name="b"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The exponent of every component</param>
    /// <returns>The power</returns>
    public static abstract Self pow(in Self a, in Self b);

    /// <summary>
    /// Returns the square root of every component
    /// </summary>
    /// <returns>The square root</returns>
    public static abstract Self sqrt(in Self a);

    /// <summary>
    /// Returns the reciprocal of the square root of every component, it is the same as <c>rcp(sqrt())</c>
    /// </summary>
    /// <returns>The reciprocal of the square root</returns>
    public static abstract Self rsqrt(in Self a);

    #endregion

    #region Normalize

    /// <summary>
    /// Returns <paramref name="a"/> scaled to a length of 1, the result is a NaN vector when the length is zero
    /// </summary>
    /// <param name="a">The vector to normalize</param>
    /// <returns>The normalized vector</returns>
    public static abstract Self normalize(in Self a);

    /// <summary>
    /// Returns <paramref name="a"/> scaled to a length of 1, it returns a zero vector when the length is zero
    /// </summary>
    /// <param name="a">The vector to normalize</param>
    /// <returns>The normalized vector</returns>
    public static abstract Self normalize_safe(in Self a);

    #endregion

    #region Step Refract

    /// <summary>
    /// Returns 1 where the component is not less than the corresponding component of
    /// <paramref name="threshold"/> and 0 where it is less
    /// </summary>
    /// <param name="threshold">The threshold</param>
    /// <returns>The step vector</returns>
    /// <param name="a">The vector</param>
    public static abstract Self step(in Self threshold, in Self a);

    #endregion

    #region ProjectSafe

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto <paramref name="onto"/>, it returns
    /// <paramref name="default_value"/> when the projection is not finite
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="onto">The vector to project onto</param>
    /// <param name="default_value">The value that is returned when the projection is not finite</param>
    /// <returns>The projected vector</returns>
    public static abstract Self project_safe(in Self a, in Self onto, in Self default_value = default);

    #endregion

    #region FaceForward

    /// <summary>
    /// Returns <paramref name="a"/> with the sign chosen so that it faces away from the incident vector
    /// <paramref name="i"/>, it is the same as flipping the sign when the dot product of
    /// <paramref name="ng"/> and <paramref name="i"/> is not negative
    /// </summary>
    /// <param name="a">The vector to orient</param>
    /// <param name="i">The incident vector</param>
    /// <param name="ng">The normal that is used to choose the sign</param>
    /// <returns>The oriented vector</returns>
    public static abstract Self face_forward(in Self a, in Self i, in Self ng);

    #endregion

    #region Sin Cos Tan

    /// <summary>
    /// Returns the sine of every component in radians
    /// </summary>
    /// <returns>The sine</returns>
    public static abstract Self sin(in Self a);

    /// <summary>
    /// Returns the cosine of every component in radians
    /// </summary>
    /// <returns>The cosine</returns>
    public static abstract Self cos(in Self a);

    /// <summary>
    /// Returns the sine and the cosine of every component in radians
    /// </summary>
    /// <returns>The sine and the cosine</returns>
    public static abstract (Self sin, Self cos) sincos(in Self a);

    /// <summary>
    /// Computes the sine and the cosine of every component in radians
    /// </summary>
    /// <param name="sin">Receives the sine</param>
    /// <param name="a">The vector</param>
    /// <param name="cos">Receives the cosine</param>
    public static abstract void sincos(in Self a, out Self sin, out Self cos);

    /// <summary>
    /// Returns the tangent of every component in radians
    /// </summary>
    /// <returns>The tangent</returns>
    public static abstract Self tan(in Self a);

    #endregion

    #region ASin ACos ATan ATan2

    /// <summary>
    /// Returns the arc sine of every component, the result is in radians
    /// </summary>
    /// <returns>The arc sine</returns>
    public static abstract Self asin(in Self a);

    /// <summary>
    /// Returns the arc cosine of every component, the result is in radians
    /// </summary>
    /// <returns>The arc cosine</returns>
    public static abstract Self acos(in Self a);

    /// <summary>
    /// Returns the arc tangent of every component, the result is in radians
    /// </summary>
    /// <returns>The arc tangent</returns>
    public static abstract Self atan(in Self a);

    /// <summary>
    /// Returns the arc tangent of <paramref name="a"/> divided by <paramref name="b"/>, the signs of both are
    /// used to find the quadrant of the result
    /// </summary>
    /// <param name="a">The numerator</param>
    /// <param name="b">The divisor</param>
    /// <returns>The arc tangent, it is in radians</returns>
    public static abstract Self atan2(in Self a, in Self b);

    #endregion

    #region SinH CosH TanH

    /// <summary>
    /// Returns the hyperbolic sine of every component
    /// </summary>
    /// <returns>The hyperbolic sine</returns>
    public static abstract Self sinh(in Self a);

    /// <summary>
    /// Returns the hyperbolic cosine of every component
    /// </summary>
    /// <returns>The hyperbolic cosine</returns>
    public static abstract Self cosh(in Self a);

    /// <summary>
    /// Returns the hyperbolic tangent of every component
    /// </summary>
    /// <returns>The hyperbolic tangent</returns>
    public static abstract Self tanh(in Self a);

    #endregion

    #region ASinH ACosH ATanH

    /// <summary>
    /// Returns the inverse hyperbolic sine of every component
    /// </summary>
    /// <returns>The inverse hyperbolic sine</returns>
    public static abstract Self asinh(in Self a);

    /// <summary>
    /// Returns the inverse hyperbolic cosine of every component
    /// </summary>
    /// <returns>The inverse hyperbolic cosine</returns>
    public static abstract Self acosh(in Self a);

    /// <summary>
    /// Returns the inverse hyperbolic tangent of every component
    /// </summary>
    /// <returns>The inverse hyperbolic tangent</returns>
    public static abstract Self atanh(in Self a);

    #endregion

    #region ChgSign

    /// <summary>
    /// Returns a vector that has the magnitude of <paramref name="a"/> and the sign of <paramref name="sign"/>
    /// </summary>
    /// <param name="a">The vector that provides the magnitude of every component</param>
    /// <param name="sign">The vector that provides the sign of every component</param>
    /// <returns>The vector with the changed sign</returns>
    public static abstract Self chg_sign(in Self a, in Self sign);

    #endregion
}

/// <summary>
/// A <see cref="IVectorFloatingPoint{Self,Scalar}"/> with the ieee 754 math functions
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorFloatingPointIeee754<Self, Scalar> :
    IVectorFloatingPointIeee754<Self>,
    IVectorFloatingPoint<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPointIeee754<Self, Scalar>
    where Scalar : unmanaged
{
    #region Pow

    /// <summary>
    /// Returns every component raised to the power of <paramref name="b"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The exponent</param>
    /// <returns>The power</returns>
    public static abstract Self pow(in Self a, Scalar b);

    #endregion

    #region Length Distance

    /// <summary>
    /// Returns the length of the vector, it is the same as <c>sqrt(length_sq())</c>
    /// </summary>
    /// <returns>The length of the vector</returns>
    public static abstract Scalar length(in Self a);

    /// <summary>
    /// Returns the distance between the two vectors, it is the same as the length of the difference
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <returns>The distance</returns>
    public static abstract Scalar distance(in Self a, in Self b);

    #endregion

    #region Refract

    /// <summary>
    /// Returns the refraction direction, <paramref name="i"/> has to be normalized and
    /// <paramref name="n"/> has to point against <paramref name="i"/>
    /// </summary>
    /// <param name="i">The normalized vector of the incoming direction</param>
    /// <param name="n">The normalized normal, it has to point against <paramref name="i"/></param>
    /// <param name="index_of_refraction">The ratio between the index of refraction of the two materials</param>
    /// <returns>The refracted direction</returns>
    public static abstract Self refract(in Self i, in Self n, Scalar index_of_refraction);

    #endregion
}
