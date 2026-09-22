namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector whose components can be replaced by a value of their own type
/// <para>The name of a member is the name of the components it takes from its argument, the components it does
/// not name keep the value of the vector it is called on: <c>Rx</c> returns the vector whose <c>x</c> component
/// is the value and whose other ones are the ones of the vector</para>
/// <para>The value of the pair that a member of 2 components takes is a 2 component vector, the members that a
/// vector of 3 or 4 components adds name the components it can reach on top of these two</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVectorReplace<TSelf, TScalar>
    where TSelf : unmanaged, IVectorReplace<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Replace

    /// <summary>
    /// Returns the vector whose <c>x</c> component is <paramref name="x"/> and whose other components keep their
    /// value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="x">The value of the <c>x</c> component</param>
    /// <returns>The vector with the replaced component</returns>
    public static abstract TSelf Rx(in TSelf self, TScalar x);

    /// <summary>
    /// Returns the vector whose <c>y</c> component is <paramref name="y"/> and whose other components keep their
    /// value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="y">The value of the <c>y</c> component</param>
    /// <returns>The vector with the replaced component</returns>
    public static abstract TSelf Ry(in TSelf self, TScalar y);

    #endregion
}

/// <summary>
/// A <see cref="IVectorReplace{TSelf,TScalar}"/> of 3 components, it replaces the <c>z</c> component and the
/// three pairs of the components as well
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector2">The 2 component vector of the same component type</typeparam>
public interface IVector3Replace<TSelf, TScalar, TVector2> :
    IVectorReplace<TSelf, TScalar>
    where TSelf : unmanaged, IVectorReplace<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Replace

    /// <summary>
    /// Returns the vector whose <c>z</c> component is <paramref name="z"/> and whose other components keep their
    /// value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="z">The value of the <c>z</c> component</param>
    /// <returns>The vector with the replaced component</returns>
    public static abstract TSelf Rz(in TSelf self, TScalar z);

    #endregion

    #region Replace Pair

    /// <summary>
    /// Returns the vector whose <c>x</c> and <c>y</c> components are the pair and whose <c>z</c> component keeps
    /// its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="xy">The values of the <c>x</c> and <c>y</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rxy(in TSelf self, in TVector2 xy);

    /// <summary>
    /// Returns the vector whose <c>y</c> and <c>z</c> components are the pair and whose <c>x</c> component keeps
    /// its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="yz">The values of the <c>y</c> and <c>z</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Ryz(in TSelf self, in TVector2 yz);

    /// <summary>
    /// Returns the vector whose <c>x</c> and <c>z</c> components are the pair and whose <c>y</c> component keeps
    /// its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="xz">The values of the <c>x</c> and <c>z</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rxz(in TSelf self, in TVector2 xz);

    #endregion
}

/// <summary>
/// A <see cref="IVector3Replace{TSelf,TScalar,TVector2}"/> of 4 components, it replaces the <c>w</c> component,
/// the three remaining pairs of the components and the four triples of them as well
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector2">The 2 component vector of the same component type</typeparam>
/// <typeparam name="TVector3">The 3 component vector of the same component type</typeparam>
public interface IVector4Replace<TSelf, TScalar, TVector2, TVector3> :
    IVector3Replace<TSelf, TScalar, TVector2>
    where TSelf : unmanaged, IVectorReplace<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Replace

    /// <summary>
    /// Returns the vector whose <c>w</c> component is <paramref name="w"/> and whose other components keep their
    /// value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="w">The value of the <c>w</c> component</param>
    /// <returns>The vector with the replaced component</returns>
    public static abstract TSelf Rw(in TSelf self, TScalar w);

    #endregion

    #region Replace Pair

    /// <summary>
    /// Returns the vector whose <c>z</c> and <c>w</c> components are the pair and whose other components keep
    /// their value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="zw">The values of the <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rzw(in TSelf self, in TVector2 zw);

    /// <summary>
    /// Returns the vector whose <c>x</c> and <c>w</c> components are the pair and whose other components keep
    /// their value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="xw">The values of the <c>x</c> and <c>w</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rxw(in TSelf self, in TVector2 xw);

    /// <summary>
    /// Returns the vector whose <c>y</c> and <c>w</c> components are the pair and whose other components keep
    /// their value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="yw">The values of the <c>y</c> and <c>w</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Ryw(in TSelf self, in TVector2 yw);

    #endregion

    #region Replace Triple

    /// <summary>
    /// Returns the vector whose <c>x</c>, <c>y</c> and <c>z</c> components are the triple and whose <c>w</c>
    /// component keeps its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="xyz">The values of the <c>x</c>, <c>y</c> and <c>z</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rxyz(in TSelf self, in TVector3 xyz);

    /// <summary>
    /// Returns the vector whose <c>y</c>, <c>z</c> and <c>w</c> components are the triple and whose <c>x</c>
    /// component keeps its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="yzw">The values of the <c>y</c>, <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Ryzw(in TSelf self, in TVector3 yzw);

    /// <summary>
    /// Returns the vector whose <c>x</c>, <c>y</c> and <c>w</c> components are the triple and whose <c>z</c>
    /// component keeps its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="xyw">The values of the <c>x</c>, <c>y</c> and <c>w</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rxyw(in TSelf self, in TVector3 xyw);

    /// <summary>
    /// Returns the vector whose <c>x</c>, <c>z</c> and <c>w</c> components are the triple and whose <c>y</c>
    /// component keeps its value
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="xzw">The values of the <c>x</c>, <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector with the replaced components</returns>
    public static abstract TSelf Rxzw(in TSelf self, in TVector3 xzw);

    #endregion
}
