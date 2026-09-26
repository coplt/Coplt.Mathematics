namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of 2 components that the legacy members of the insert are called on
/// <para>The name of a member is the name of the components that it takes from the pair, the pair behind it holds
/// the other ones and the vector it builds has the components of the two of them: <c>Iz(xy, z)</c> builds the
/// vector of 3 components whose <c>x</c> and <c>y</c> components are the pair, and <c>Iyz(xw, yz)</c> builds the
/// vector of 4 components whose <c>y</c> and <c>z</c> components are the second pair and whose <c>x</c> and
/// <c>w</c> ones are the first one</para>
/// <para>Every member builds the same vector as the member of the create of the vector it returns, which is the
/// one that the implementation of a member of this kind forwards to</para>
/// </summary>
/// <typeparam name="TSelf">The 2 component vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector3">The 3 component vector of the same component type</typeparam>
/// <typeparam name="TVector4">The 4 component vector of the same component type</typeparam>
public interface IVector2Insert<TSelf, TScalar, TVector3, TVector4>
    where TSelf : unmanaged
    where TScalar : unmanaged
    where TVector3 : unmanaged
    where TVector4 : unmanaged
{
    #region Insert

    /// <summary>
    /// Returns the vector of 3 components that has the <c>y</c> and <c>z</c> components of
    /// <paramref name="self"/> and the <c>x</c> component of <paramref name="x"/>
    /// </summary>
    /// <param name="self">The <c>y</c> and <c>z</c> components of the vector</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <returns>The vector of 3 components</returns>
    public static abstract TVector3 Ix(in TSelf self, TScalar x);

    /// <summary>
    /// Returns the vector of 3 components that has the <c>x</c> and <c>z</c> components of
    /// <paramref name="self"/> and the <c>y</c> component of <paramref name="y"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>z</c> components of the vector</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <returns>The vector of 3 components</returns>
    public static abstract TVector3 Iy(in TSelf self, TScalar y);

    /// <summary>
    /// Returns the vector of 3 components that has the <c>x</c> and <c>y</c> components of
    /// <paramref name="self"/> and the <c>z</c> component of <paramref name="z"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>y</c> components of the vector</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector of 3 components</returns>
    public static abstract TVector3 Iz(in TSelf self, TScalar z);

    #endregion

    #region Insert Pair

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>y</c> components of
    /// <paramref name="self"/> and the <c>z</c> and <c>w</c> components of <paramref name="zw"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>y</c> components of the vector</param>
    /// <param name="zw">The <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Izw(in TSelf self, in TSelf zw);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>y</c> components of
    /// <paramref name="self"/> and the <c>z</c> and <c>w</c> components of the two values
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>y</c> components of the vector</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Izw(in TSelf self, TScalar z, TScalar w);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>y</c> components of
    /// <paramref name="xy"/> and the <c>z</c> and <c>w</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>z</c> and <c>w</c> components of the vector</param>
    /// <param name="xy">The <c>x</c> and <c>y</c> components</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ixy(in TSelf self, in TSelf xy);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>y</c> components of the two values and
    /// the <c>z</c> and <c>w</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>z</c> and <c>w</c> components of the vector</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ixy(in TSelf self, TScalar x, TScalar y);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>y</c> and <c>z</c> components of
    /// <paramref name="yz"/> and the <c>x</c> and <c>w</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>w</c> components of the vector</param>
    /// <param name="yz">The <c>y</c> and <c>z</c> components</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iyz(in TSelf self, in TSelf yz);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>y</c> and <c>z</c> components of the two values and
    /// the <c>x</c> and <c>w</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>w</c> components of the vector</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iyz(in TSelf self, TScalar y, TScalar z);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>w</c> components of
    /// <paramref name="xw"/> and the <c>y</c> and <c>z</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>y</c> and <c>z</c> components of the vector</param>
    /// <param name="xw">The <c>x</c> and <c>w</c> components</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ixw(in TSelf self, in TSelf xw);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>w</c> components of the two values and
    /// the <c>y</c> and <c>z</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>y</c> and <c>z</c> components of the vector</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ixw(in TSelf self, TScalar x, TScalar w);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>y</c> and <c>w</c> components of
    /// <paramref name="yw"/> and the <c>x</c> and <c>z</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>z</c> components of the vector</param>
    /// <param name="yw">The <c>y</c> and <c>w</c> components</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iyw(in TSelf self, in TSelf yw);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>y</c> and <c>w</c> components of the two values and
    /// the <c>x</c> and <c>z</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>x</c> and <c>z</c> components of the vector</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iyw(in TSelf self, TScalar y, TScalar w);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>z</c> components of
    /// <paramref name="xz"/> and the <c>y</c> and <c>w</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>y</c> and <c>w</c> components of the vector</param>
    /// <param name="xz">The <c>x</c> and <c>z</c> components</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ixz(in TSelf self, in TSelf xz);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c> and <c>z</c> components of the two values and
    /// the <c>y</c> and <c>w</c> components of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The <c>y</c> and <c>w</c> components of the vector</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ixz(in TSelf self, TScalar x, TScalar z);

    #endregion
}

/// <summary>
/// A vector of 3 components that the legacy members of the insert are called on
/// <para>The name of a member is the name of the component that it takes from the triple, the triple holds the
/// other ones and the vector it builds has the components of the two of them: <c>Iy(xzw, y)</c> builds the vector
/// of 4 components whose <c>y</c> component is the value and whose <c>x</c>, <c>z</c> and <c>w</c> ones are the
/// triple</para>
/// <para>Every member builds the same vector as the member of the create of the vector it returns, which is the
/// one that the implementation of a member of this kind forwards to</para>
/// </summary>
/// <typeparam name="TSelf">The 3 component vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector4">The 4 component vector of the same component type</typeparam>
public interface IVector3Insert<TSelf, TScalar, TVector4>
    where TSelf : unmanaged
    where TScalar : unmanaged
    where TVector4 : unmanaged
{
    #region Insert

    /// <summary>
    /// Returns the vector of 4 components that has the <c>y</c>, <c>z</c> and <c>w</c> components of
    /// <paramref name="self"/> and the <c>x</c> component of <paramref name="x"/>
    /// </summary>
    /// <param name="self">The <c>y</c>, <c>z</c> and <c>w</c> components of the vector</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Ix(in TSelf self, TScalar x);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c>, <c>z</c> and <c>w</c> components of
    /// <paramref name="self"/> and the <c>y</c> component of <paramref name="y"/>
    /// </summary>
    /// <param name="self">The <c>x</c>, <c>z</c> and <c>w</c> components of the vector</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iy(in TSelf self, TScalar y);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c>, <c>y</c> and <c>w</c> components of
    /// <paramref name="self"/> and the <c>z</c> component of <paramref name="z"/>
    /// </summary>
    /// <param name="self">The <c>x</c>, <c>y</c> and <c>w</c> components of the vector</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iz(in TSelf self, TScalar z);

    /// <summary>
    /// Returns the vector of 4 components that has the <c>x</c>, <c>y</c> and <c>z</c> components of
    /// <paramref name="self"/> and the <c>w</c> component of <paramref name="w"/>
    /// </summary>
    /// <param name="self">The <c>x</c>, <c>y</c> and <c>z</c> components of the vector</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector of 4 components</returns>
    public static abstract TVector4 Iw(in TSelf self, TScalar w);

    #endregion
}
