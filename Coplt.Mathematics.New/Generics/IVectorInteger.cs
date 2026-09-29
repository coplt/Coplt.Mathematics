namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of integers, it adds the check of a power of two
/// <para>It does not name the type of a single component because none of its members needs it, so a caller of a
/// member of it does not have to spell the component type out</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVectorInteger<Self>
    where Self : unmanaged, IVectorInteger<Self>
{
    #region IsPow2

    /// <summary>
    /// Returns a value whose every component says whether the component is a power of two: the component of the
    /// result is the all bits set value of its kind where the component is a power of two and the zero of its
    /// kind where it is not
    /// <para>A zero and a negative component are not a power of two</para>
    /// </summary>
    /// <returns>The value of the check</returns>
    public static abstract Self is_pow2(in Self a);

    #endregion
}

/// <summary>
/// An <see cref="IVectorInteger{Self}"/> that has no sign, it adds the rounding up to the next power of two,
/// which is only meaningful for a value that cannot be negative
/// <para>Only the vectors without a sign implement it, a vector that does not simply does not satisfy a
/// constraint that requires it</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
public interface IVectorUnsignedInteger<Self> : IVectorInteger<Self>
    where Self : unmanaged, IVectorUnsignedInteger<Self>
{
    #region Up2Pow2

    /// <summary>
    /// Returns every component rounded up to the next power of two, a component that is a power of two already
    /// is kept and a zero stays zero
    /// </summary>
    /// <returns>The rounded up vector</returns>
    public static abstract Self up2pow2(in Self a);

    #endregion
}
