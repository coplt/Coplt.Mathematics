namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector that selects between two vectors of its own shape with a mask.
/// <para>The select is not a member of the arithmetic of a vector, it is a member of its own: every vector has a
/// mask vector and can be selected with it, whether its components are numbers or bools, so the interface only
/// asks for the mask.</para>
/// </summary>
/// <typeparam name="Self">The vector type itself</typeparam>
/// <typeparam name="BoolVector">The bool vector type that has the same shape as this vector</typeparam>
public interface IVectorSelect<Self, BoolVector> :
    IVector<Self>
    where Self : unmanaged, IVectorSelect<Self, BoolVector>
{
    #region Select

    /// <summary>
    /// Returns the vector with the component of <c>t</c> where the mask is true and the component of <c>f</c>
    /// where it is false
    /// </summary>
    /// <param name="c">The mask</param>
    /// <param name="t">The vector that the true components are taken from</param>
    /// <param name="f">The vector that the false components are taken from</param>
    /// <returns>The selected vector</returns>
    public static abstract Self select(in BoolVector c, in Self t, in Self f);

    #endregion
}
