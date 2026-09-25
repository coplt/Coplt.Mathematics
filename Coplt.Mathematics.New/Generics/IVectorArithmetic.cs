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
    #region CMin CMax

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
