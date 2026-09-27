namespace Coplt.Mathematics.Generics;

/// <summary>
/// An <see cref="ISignedVectorArithmetic{Self}"/> of floating point components, it adds the floating point math
/// functions
/// <para>It does not name the type of a single component because none of these members needs it, <see
/// cref="IVectorFloatingPoint{Self,Scalar}"/> is the form that names it</para>
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
}

/// <summary>
/// An <see cref="ISignedVectorArithmetic{Self,Scalar}"/> of floating point components, which is the form of
/// <see cref="IVectorFloatingPoint{Self}"/> that names the type of a single component
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
public interface IVectorFloatingPoint<Self, Scalar> :
    IVectorFloatingPoint<Self>,
    ISignedVectorArithmetic<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPoint<Self, Scalar>
    where Scalar : unmanaged
{
}
