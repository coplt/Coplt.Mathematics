namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of integers, it adds the check of a power of two
/// <para>It does not name the type of a single component because none of its members needs it, so a caller of a
/// member of it does not have to spell the component type out</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorInteger<Self, out BoolVector>
    where Self : unmanaged, IVectorInteger<Self, BoolVector>
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
/// An <see cref="IVectorInteger{Self,BoolVector}"/> that has no sign, it adds the rounding up to the next power
/// of two, which is only meaningful for a value that cannot be negative
/// <para>Only the vectors without a sign implement it, a vector that does not simply does not satisfy a
/// constraint that requires it</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorUnsignedInteger<Self, out BoolVector> :
    IVectorInteger<Self, BoolVector>
    where Self : unmanaged, IVectorUnsignedInteger<Self, BoolVector>
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
