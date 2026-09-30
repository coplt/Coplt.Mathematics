namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of floating point components with the ieee 754 math functions
/// <para>It does not name the type of a single component because none of these members needs it, <see
/// cref="IVectorFloatingPointIeee754{Self,Scalar}"/> adds the members that do</para>
/// <para>The members of it are the ones the migration has not taken over yet: the step and the refraction, the
/// face forward, the hyperbolics and the change of the sign. The logarithm, the exponential and the
/// trigonometry are the members of the dispatch of the algebra of the kind of the value.</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVectorFloatingPointIeee754<Self>
    where Self : unmanaged, IVectorFloatingPointIeee754<Self>
{
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
/// A <see cref="IVectorFloatingPointIeee754{Self}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorFloatingPointIeee754<Self, Scalar> :
    IVectorFloatingPointIeee754<Self>
    where Self : unmanaged, IVectorFloatingPointIeee754<Self, Scalar>
    where Scalar : unmanaged
{
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
