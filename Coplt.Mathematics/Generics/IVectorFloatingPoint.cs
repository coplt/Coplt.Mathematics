namespace Coplt.Mathematics.Generics;

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

    public Self mod(in Self other);

    public Self modf(out Self i);

    #endregion

    #region Ceil Floor Round Trunc Frac

    public Self ceil();

    public Self floor();

    public Self round();

    public Self trunc();

    public Self frac();

    #endregion

    #region Rcp Saturate SmoothStep Reflect

    public Self rcp();

    public Self saturate();

    public Self smoothstep(in Self min, in Self max);

    public Self reflect(in Self n);

    #endregion

    #region Project

    public Self project(in Self onto);

    public Self project_on_plane(in Self plane_normal);

    public Self project_normalized(in Self onto);

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

    public Self wrap(in Self min, in Self max);
    public Self wrap(Scalar min, Scalar max);

    #endregion
}
