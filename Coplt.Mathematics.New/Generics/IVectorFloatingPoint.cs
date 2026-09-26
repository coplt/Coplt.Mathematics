namespace Coplt.Mathematics.Generics;

/// <summary>
/// An <see cref="ISignedVectorArithmetic{Self}"/> of floating point components, it adds the floating point math
/// functions
/// <para>It does not name the type of a single component because none of these members needs it, <see
/// cref="IVectorFloatingPoint{Self,Scalar}"/> adds the members that do</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVectorFloatingPoint<Self> :
    ISignedVectorArithmetic<Self>
    where Self : unmanaged, IVectorFloatingPoint<Self>
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
    /// <param name="a">The vector</param>
    /// <param name="b">The divisor</param>
    /// <returns>The remainder</returns>
    public static abstract Self mod(in Self a, in Self b);

    /// <summary>
    /// Splits the vector into its integral and its fractional part
    /// <para>It returns the fractional part and stores the integral part into <paramref name="i"/></para>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="i">Receives the integral part</param>
    /// <returns>The fractional part</returns>
    public static abstract Self modf(in Self a, out Self i);

    #endregion

    #region SmoothStep Reflect

    /// <summary>
    /// Interpolates smoothly between <paramref name="min"/> and <paramref name="max"/>, <paramref name="a"/> is
    /// the value
    /// <para>The result is 0 when the value is below the minimum, 1 when it is above the maximum and a smooth
    /// hermite curve in between</para>
    /// </summary>
    /// <param name="min">The value at 0</param>
    /// <param name="max">The value at 1</param>
    /// <param name="a">The value to place between the two bounds</param>
    /// <returns>The interpolated vector</returns>
    public static abstract Self smoothstep(in Self min, in Self max, in Self a);

    /// <summary>
    /// Returns <paramref name="a"/> reflected around the normal <paramref name="n"/>
    /// <para><paramref name="n"/> <b>has to be</b> normalized</para>
    /// </summary>
    /// <param name="a">The vector to reflect</param>
    /// <param name="n">The normalized normal of the surface</param>
    /// <returns>The reflected vector</returns>
    public static abstract Self reflect(in Self a, in Self n);

    #endregion

    #region Project

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto <paramref name="onto"/>
    /// <para>It is the component of <paramref name="a"/> that is parallel to <paramref name="onto"/></para>
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="onto">The vector to project onto, it does <b>not</b> have to be normalized</param>
    /// <returns>The projected vector</returns>
    public static abstract Self project(in Self a, in Self onto);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto the plane that has <paramref name="plane_normal"/> as
    /// its normal
    /// <para>It is the component of <paramref name="a"/> that is inside the plane</para>
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="plane_normal">The normal of the plane, it does <b>not</b> have to be normalized</param>
    /// <returns>The projected vector</returns>
    public static abstract Self project_on_plane(in Self a, in Self plane_normal);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto <paramref name="onto"/>
    /// <para>It is the same as <see cref="project(in Self, in Self)"/> but <paramref name="onto"/> is assumed to
    /// be normalized</para>
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="onto">The normalized vector to project onto</param>
    /// <returns>The projected vector</returns>
    public static abstract Self project_normalized(in Self a, in Self onto);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto the plane that has <paramref name="plane_normal"/> as
    /// its normal, it is the same as <see cref="project_on_plane(in Self, in Self)"/> but
    /// <paramref name="plane_normal"/> is assumed to be normalized
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="plane_normal">The normalized normal of the plane</param>
    /// <returns>The projected vector</returns>
    public static abstract Self project_on_plane_normalized(in Self a, in Self plane_normal);

    #endregion

    #region Radians Degrees

    /// <summary>
    /// Degrees -> Radians
    /// </summary>
    public static abstract Self radians(in Self a);

    /// <summary>
    /// Radians -> Degrees
    /// </summary>
    public static abstract Self degrees(in Self a);

    #endregion

    #region Wrap

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of the range</param>
    /// <param name="max">The upper bound of the range</param>
    /// <returns>The wrapped vector</returns>
    public static abstract Self wrap(in Self a, in Self min, in Self max);

    #endregion
}

/// <summary>
/// An <see cref="ISignedVectorArithmetic{Self,Scalar}"/> of floating point components, it adds the floating
/// point math functions
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorFloatingPoint<Self, Scalar> :
    IVectorFloatingPoint<Self>,
    ISignedVectorArithmetic<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPoint<Self, Scalar>
    where Scalar : unmanaged
{
    #region Wrap

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of the range</param>
    /// <param name="max">The upper bound of the range</param>
    /// <returns>The wrapped vector</returns>
    public static abstract Self wrap(in Self a, Scalar min, Scalar max);

    #endregion
}
