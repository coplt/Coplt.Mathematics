namespace Coplt.Mathematics.Generics;

/// <summary>
/// An arithmetic vector of floating point components, it adds the floating point math functions
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorFloatingPoint<Self, Scalar> :
    ISignedVectorArithmetic<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPoint<Self, Scalar>
    where Scalar : unmanaged
{
    #region Math Constants

    /// <summary>
    /// <code>e</code>
    /// </summary>
    public static abstract Self E { get; }
    /// <summary>
    /// <code>log(2)</code>
    /// </summary>
    public static abstract Self Log2 { get; }
    /// <summary>
    /// <code>log(10)</code>
    /// </summary>
    public static abstract Self Log10 { get; }
    /// <summary>
    /// <code>π</code>
    /// </summary>
    public static abstract Self PI { get; }
    /// <summary>
    /// <code>τ = 2 * π</code>
    /// </summary>
    public static abstract Self Tau { get; }
    /// <summary>
    /// <code>360 / τ</code>
    /// </summary>
    public static abstract Self RadToDeg { get; }
    /// <summary>
    /// <code>τ / 360</code>
    /// </summary>
    public static abstract Self DegToRad { get; }

    #endregion

    #region Mod Modf

    /// <summary>
    /// Returns the component wise remainder of the division of the two vectors
    /// </summary>
    /// <param name="other">The divisor</param>
    /// <returns>The remainder</returns>
    public Self mod(in Self other);

    /// <summary>
    /// Splits the vector into its integral and its fractional part,
    /// it returns the fractional part and stores the integral part into
    /// <paramref name="i"/>
    /// </summary>
    /// <param name="i">Receives the integral part</param>
    /// <returns>The fractional part</returns>
    public Self modf(out Self i);

    #endregion

    #region Ceil Floor Round Trunc Frac

    /// <summary>
    /// Rounds every component up to the smallest integral value that is not less than it
    /// </summary>
    /// <returns>The rounded vector</returns>
    public Self ceil();

    /// <summary>
    /// Rounds every component down to the largest integral value that is not greater than it
    /// </summary>
    /// <returns>The rounded vector</returns>
    public Self floor();

    /// <summary>
    /// Rounds every component to the nearest integral value
    /// </summary>
    /// <returns>The rounded vector</returns>
    public Self round();

    /// <summary>
    /// Rounds every component towards zero
    /// </summary>
    /// <returns>The rounded vector</returns>
    public Self trunc();

    /// <summary>
    /// Returns the fractional part of every component, it is the same as <c>mod(1)</c>
    /// </summary>
    /// <returns>The fractional part</returns>
    public Self frac();

    #endregion

    #region Rcp Saturate SmoothStep Reflect

    /// <summary>
    /// Returns the reciprocal of every component, it is the same as <c>1 / self</c>
    /// </summary>
    /// <returns>The reciprocal</returns>
    public Self rcp();

    /// <summary>
    /// Clamps every component to the range 0 to 1
    /// </summary>
    /// <returns>The clamped vector</returns>
    public Self saturate();

    /// <summary>
    /// Interpolates smoothly between <paramref name="min"/> and <paramref name="max"/>, this vector is the value,
    /// the result is 0 when the value is below the minimum, 1 when it is above the maximum and a smooth
    /// hermite curve in between
    /// </summary>
    /// <param name="min">The value at 0</param>
    /// <param name="max">The value at 1</param>
    /// <returns>The interpolated vector</returns>
    public Self smoothstep(in Self min, in Self max);

    /// <summary>
    /// Returns this vector reflected around the normal <paramref name="n"/>,
    /// <paramref name="n"/> has to be normalized
    /// </summary>
    /// <param name="n">The normalized normal of the surface</param>
    /// <returns>The reflected vector</returns>
    public Self reflect(in Self n);

    #endregion

    #region Project

    /// <summary>
    /// Returns the projection of this vector onto <paramref name="onto"/>,
    /// it is the component of this vector that is parallel to <paramref name="onto"/>
    /// </summary>
    /// <param name="onto">The vector to project onto, it does not have to be normalized</param>
    /// <returns>The projected vector</returns>
    public Self project(in Self onto);

    /// <summary>
    /// Returns the projection of this vector onto the plane that has <paramref name="plane_normal"/> as its
    /// normal, it is the component of this vector that is inside the plane
    /// </summary>
    /// <param name="plane_normal">The normal of the plane, it does not have to be normalized</param>
    /// <returns>The projected vector</returns>
    public Self project_on_plane(in Self plane_normal);

    /// <summary>
    /// Returns the projection of this vector onto <paramref name="onto"/>, it is the same as
    /// <see cref="project(in Self)"/> but <paramref name="onto"/> is assumed to be normalized
    /// </summary>
    /// <param name="onto">The normalized vector to project onto</param>
    /// <returns>The projected vector</returns>
    public Self project_normalized(in Self onto);

    /// <summary>
    /// Returns the projection of this vector onto the plane that has <paramref name="plane_normal"/> as its
    /// normal, it is the same as <see cref="project_on_plane(in Self)"/> but
    /// <paramref name="plane_normal"/> is assumed to be normalized
    /// </summary>
    /// <param name="plane_normal">The normalized normal of the plane</param>
    /// <returns>The projected vector</returns>
    public Self project_on_plane_normalized(in Self plane_normal);

    #endregion

    #region Radians Degrees

    /// <summary>
    /// Degrees -> Radians
    /// </summary>
    public Self radians();

    /// <summary>
    /// Radians -> Degrees
    /// </summary>
    public Self degrees();

    #endregion

    #region Wrap

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    /// <param name="min">The lower bound of the range</param>
    /// <param name="max">The upper bound of the range</param>
    /// <returns>The wrapped vector</returns>
    public Self wrap(in Self min, in Self max);

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    /// <param name="min">The lower bound of the range</param>
    /// <param name="max">The upper bound of the range</param>
    /// <returns>The wrapped vector</returns>
    public Self wrap(Scalar min, Scalar max);

    #endregion
}
