namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of integers, it adds the check of a power of two. The mask of a check is a bool vector that has the
/// same shape as this vector, only the vectors without a sign also have <see cref="IVectorUnsignedInteger{Self,Scalar,BoolVector}"/>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorInteger<Self, Scalar, out BoolVector>
    where Self : unmanaged, IVectorInteger<Self, Scalar, BoolVector>
    where Scalar : unmanaged
{
    #region IsPow2

    /// <summary>
    /// Returns a mask that is true where the component is a power of two
    /// <para>A zero and a negative component are not a power of two</para>
    /// </summary>
    /// <returns>The mask</returns>
    public BoolVector is_pow2();

    #endregion
}

/// <summary>
/// An <see cref="IVectorInteger{Self,Scalar,BoolVector}"/> that has no sign, it adds the rounding up to the next
/// power of two, which is only meaningful for a value that cannot be negative
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="Scalar">The type of a single component</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorUnsignedInteger<Self, Scalar, out BoolVector> :
    IVectorInteger<Self, Scalar, BoolVector>
    where Self : unmanaged, IVectorUnsignedInteger<Self, Scalar, BoolVector>
    where Scalar : unmanaged
{
    #region Up2Pow2

    /// <summary>
    /// Returns every component rounded up to the next power of two, a component that is a power of two already
    /// is kept and a zero stays zero
    /// </summary>
    /// <returns>The rounded up vector</returns>
    public Self up2pow2();

    #endregion
}
