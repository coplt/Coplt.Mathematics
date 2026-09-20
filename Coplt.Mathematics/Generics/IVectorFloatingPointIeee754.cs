namespace Coplt.Mathematics.Generics;

/// <summary>
/// A floating point vector that can also produce a bool vector, it adds the checks for the special
/// floating point values
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorFloatingPointIeee754BoolOps<Self, Scalar, out BoolVector> :
    IVectorFloatingPointIeee754<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPointIeee754BoolOps<Self, Scalar, BoolVector>
    where Scalar : unmanaged
{
    #region IsNaN IsFinite IsInfinity IsPositiveInfinity

    /// <summary>
    /// Returns a mask that is true where the component is NaN
    /// </summary>
    /// <returns>The mask</returns>
    public BoolVector is_NaN();

    /// <summary>
    /// Returns a mask that is true where the component is finite, so it is neither NaN nor an infinity
    /// </summary>
    /// <returns>The mask</returns>
    public BoolVector is_finite();

    /// <summary>
    /// Returns a mask that is true where the component is a positive or a negative infinity
    /// </summary>
    /// <returns>The mask</returns>
    public BoolVector is_inf();

    /// <summary>
    /// Returns a mask that is true where the component is a positive infinity
    /// </summary>
    /// <returns>The mask</returns>
    public BoolVector is_pos_inf();

    /// <summary>
    /// Returns a mask that is true where the component is a negative infinity
    /// </summary>
    /// <returns>The mask</returns>
    public BoolVector is_neg_inf();

    #endregion
}

/// <summary>
/// A floating point vector with the ieee 754 math functions
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorFloatingPointIeee754<Self, Scalar> :
    IVectorFloatingPoint<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPointIeee754<Self, Scalar>
    where Scalar : unmanaged
{
    #region Log

    /// <summary>
    /// Returns the natural logarithm of every component
    /// </summary>
    /// <returns>The natural logarithm</returns>
    public Self log();

    /// <summary>
    /// Returns the base 2 logarithm of every component
    /// </summary>
    /// <returns>The base 2 logarithm</returns>
    public Self log2();

    /// <summary>
    /// Returns the logarithm of every component with <paramref name="other"/> as the base
    /// </summary>
    /// <param name="other">The base of the logarithm</param>
    /// <returns>The logarithm</returns>
    public Self log(in Self other);

    /// <summary>
    /// Returns the base 10 logarithm of every component
    /// </summary>
    /// <returns>The base 10 logarithm</returns>
    public Self log10();

    #endregion

    #region Exp

    /// <summary>
    /// Returns <c>e</c> raised to the power of every component
    /// </summary>
    /// <returns>The exponential</returns>
    public Self exp();

    /// <summary>
    /// Returns 2 raised to the power of every component
    /// </summary>
    /// <returns>The exponential</returns>
    public Self exp2();

    /// <summary>
    /// Returns 10 raised to the power of every component
    /// </summary>
    /// <returns>The exponential</returns>
    public Self exp10();

    #endregion

    #region Pow Sqrt RSqrt

    /// <summary>
    /// Returns every component raised to the power of <paramref name="v"/>
    /// </summary>
    /// <param name="v">The exponent</param>
    /// <returns>The power</returns>
    public Self pow(Scalar v);

    /// <summary>
    /// Returns the square root of every component
    /// </summary>
    /// <returns>The square root</returns>
    public Self sqrt();

    /// <summary>
    /// Returns the reciprocal of the square root of every component, it is the same as <c>rcp(sqrt())</c>
    /// </summary>
    /// <returns>The reciprocal of the square root</returns>
    public Self rsqrt();

    #endregion

    #region Length Distance

    /// <summary>
    /// Returns the length of the vector, it is the same as <c>sqrt(length_sq())</c>
    /// </summary>
    /// <returns>The length of the vector</returns>
    public Scalar length();

    /// <summary>
    /// Returns the distance between the two vectors, it is the same as the length of the difference
    /// </summary>
    /// <param name="to">The other vector</param>
    /// <returns>The distance</returns>
    public Scalar distance(in Self to);

    #endregion

    #region Normalize

    /// <summary>
    /// Returns this vector scaled to a length of 1, the result is a NaN vector when the length is zero
    /// </summary>
    /// <returns>The normalized vector</returns>
    public Self normalize();

    /// <summary>
    /// Returns this vector scaled to a length of 1, it returns a zero vector when the length is zero
    /// </summary>
    /// <returns>The normalized vector</returns>
    public Self normalize_safe();

    #endregion

    #region Step Refract

    /// <summary>
    /// Returns 1 where the component is not less than the corresponding component of
    /// <paramref name="threshold"/> and 0 where it is less
    /// </summary>
    /// <param name="threshold">The threshold</param>
    /// <returns>The step vector</returns>
    public Self step(in Self threshold);

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

    #region ProjectSafe

    /// <summary>
    /// Returns the projection of this vector onto <paramref name="onto"/>, it returns a zero vector when
    /// <paramref name="onto"/> is zero
    /// </summary>
    /// <param name="onto">The vector to project onto</param>
    /// <returns>The projected vector</returns>
    public Self project_safe(in Self onto);

    /// <summary>
    /// Returns the projection of this vector onto <paramref name="onto"/>, it returns
    /// <paramref name="default_value"/> when <paramref name="onto"/> is zero
    /// </summary>
    /// <param name="onto">The vector to project onto</param>
    /// <param name="default_value">The value that is returned when <paramref name="onto"/> is zero</param>
    /// <returns>The projected vector</returns>
    public Self project_safe(in Self onto, in Self default_value);

    #endregion

    #region FaceForward

    /// <summary>
    /// Returns this vector with the sign chosen so that it faces away from the incident vector
    /// <paramref name="i"/>, it is the same as flipping the sign when the dot product of
    /// <paramref name="ng"/> and <paramref name="i"/> is negative
    /// </summary>
    /// <param name="i">The incident vector</param>
    /// <param name="ng">The normal that is used to choose the sign</param>
    /// <returns>The oriented vector</returns>
    public Self face_forward(in Self i, in Self ng);

    #endregion

    #region Sin Cos Tan

    /// <summary>
    /// Returns the sine of every component in radians
    /// </summary>
    /// <returns>The sine</returns>
    public Self sin();

    /// <summary>
    /// Returns the cosine of every component in radians
    /// </summary>
    /// <returns>The cosine</returns>
    public Self cos();

    /// <summary>
    /// Returns the sine and the cosine of every component in radians
    /// </summary>
    /// <returns>The sine and the cosine</returns>
    public (Self sin, Self cos) sincos();

    /// <summary>
    /// Computes the sine and the cosine of every component in radians
    /// </summary>
    /// <param name="sin">Receives the sine</param>
    /// <param name="cos">Receives the cosine</param>
    public void sincos(out Self sin, out Self cos);

    /// <summary>
    /// Returns the tangent of every component in radians
    /// </summary>
    /// <returns>The tangent</returns>
    public Self tan();

    #endregion

    #region ASin ACos ATan ATan2

    /// <summary>
    /// Returns the arc sine of every component, the result is in radians
    /// </summary>
    /// <returns>The arc sine</returns>
    public Self asin();

    /// <summary>
    /// Returns the arc cosine of every component, the result is in radians
    /// </summary>
    /// <returns>The arc cosine</returns>
    public Self acos();

    /// <summary>
    /// Returns the arc tangent of every component, the result is in radians
    /// </summary>
    /// <returns>The arc tangent</returns>
    public Self atan();

    /// <summary>
    /// Returns the arc tangent of this vector divided by <paramref name="v"/>, the signs of both are used to
    /// find the quadrant of the result
    /// </summary>
    /// <param name="v">The divisor</param>
    /// <returns>The arc tangent, it is in radians</returns>
    public Self atan2(in Self v);

    #endregion

    #region SinH CosH TanH

    /// <summary>
    /// Returns the hyperbolic sine of every component
    /// </summary>
    /// <returns>The hyperbolic sine</returns>
    public Self sinh();

    /// <summary>
    /// Returns the hyperbolic cosine of every component
    /// </summary>
    /// <returns>The hyperbolic cosine</returns>
    public Self cosh();

    /// <summary>
    /// Returns the hyperbolic tangent of every component
    /// </summary>
    /// <returns>The hyperbolic tangent</returns>
    public Self tanh();

    #endregion

    #region ASinH ACosH ATanH

    /// <summary>
    /// Returns the inverse hyperbolic sine of every component
    /// </summary>
    /// <returns>The inverse hyperbolic sine</returns>
    public Self asinh();

    /// <summary>
    /// Returns the inverse hyperbolic cosine of every component
    /// </summary>
    /// <returns>The inverse hyperbolic cosine</returns>
    public Self acosh();

    /// <summary>
    /// Returns the inverse hyperbolic tangent of every component
    /// </summary>
    /// <returns>The inverse hyperbolic tangent</returns>
    public Self atanh();

    #endregion

    #region ChgSign

    /// <summary>
    /// Returns a vector that has the magnitude of this vector and the sign of <paramref name="sign"/>
    /// </summary>
    /// <param name="sign">The vector that provides the sign of every component</param>
    /// <returns>The vector with the changed sign</returns>
    public Self chg_sign(in Self sign);

    #endregion
}
