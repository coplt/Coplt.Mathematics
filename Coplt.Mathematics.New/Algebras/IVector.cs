namespace Coplt.Mathematics.Algebras;

#region Core

public interface IVector<TSelf> : IMatrixVector<TSelf, TSelf>
    where TSelf : unmanaged, IVector<TSelf>
{
    #region Meta

    public static abstract int Dimension { get; }

    #endregion
}

public interface IVector<TSelf, TScalar> : IVector<TSelf>, IMatrixScalar<TSelf, TScalar>
    where TSelf : unmanaged, IVector<TSelf, TScalar>
    where TScalar : unmanaged;

#endregion

#region Bool

public interface IBoolVector<TSelf> : IVector<TSelf>, IBoolMatrix<TSelf>
    where TSelf : unmanaged, IBoolVector<TSelf>;

public interface IBoolVector<TSelf, TScalar> : IVector<TSelf, TScalar>, IBoolMatrix<TSelf, TScalar>
    where TSelf : unmanaged, IBoolVector<TSelf, TScalar>
    where TScalar : unmanaged;

#endregion

#region Number

public interface INumberVector<TSelf> : IVector<TSelf>, INumberMatrix<TSelf>
    where TSelf : unmanaged, INumberVector<TSelf>;

public interface INumberVector<TSelf, TScalar> : IVector<TSelf, TScalar>, INumberMatrix<TSelf, TScalar>
    where TSelf : unmanaged, INumberVector<TSelf, TScalar>
    where TScalar : unmanaged, INumberBase<TScalar>;

#endregion

#region Signed

public interface ISignedNumberVector<TSelf> : INumberVector<TSelf>, ISignedNumberMatrix<TSelf>
    where TSelf : unmanaged, ISignedNumberVector<TSelf>;

public interface ISignedNumberVector<TSelf, TScalar> : INumberVector<TSelf, TScalar>, ISignedNumberMatrix<TSelf, TScalar>
    where TSelf : unmanaged, ISignedNumberVector<TSelf, TScalar>
    where TScalar : unmanaged, INumberBase<TScalar>;

#endregion

#region Vector2

public interface IVector2<TSelf> : IVector<TSelf>
    where TSelf : unmanaged, IVector2<TSelf>;

public interface IVector2<TSelf, TScalar> : IVector2<TSelf>, IVector<TSelf, TScalar>
    where TSelf : unmanaged, IVector2<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Create

    /// <summary>
    /// Creates a vector from its components
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, TScalar y);

    #endregion
}

#endregion

#region Vector3

public interface IVector3<TSelf> : IVector<TSelf>
    where TSelf : unmanaged, IVector3<TSelf>;

public interface IVector3<TSelf, TScalar> : IVector3<TSelf>, IVector<TSelf, TScalar>
    where TSelf : unmanaged, IVector3<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Create

    /// <summary>
    /// Creates a vector from its components
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, TScalar y, TScalar z);

    #endregion
}

public interface IVector3CtorFromVector2<TSelf, TScalar, TVector2> : IVector3<TSelf, TScalar>
    where TSelf : unmanaged, IVector3CtorFromVector2<TSelf, TScalar, TVector2>
    where TScalar : unmanaged
{
    #region Create

    /// <summary>
    /// Creates a vector from the pair of the <c>x</c> and <c>y</c> components and the <c>z</c> component
    /// </summary>
    /// <param name="xy">The <c>x</c> and <c>y</c> components</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(in TVector2 xy, TScalar z);

    /// <summary>
    /// Creates a vector from the <c>x</c> component and the pair of the <c>y</c> and <c>z</c> components
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="yz">The <c>y</c> and <c>z</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, in TVector2 yz);

    #endregion

    #region Insert

    /// <summary>
    /// Creates a vector that has the <c>y</c> component of <paramref name="y"/> and the <c>x</c> and
    /// <c>z</c> components of <paramref name="xz"/>
    /// </summary>
    /// <param name="xz">The <c>x</c> and <c>z</c> components</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertY(in TVector2 xz, TScalar y);

    #endregion
}

#endregion

#region Vector4

public interface IVector4<TSelf> : IVector<TSelf>
    where TSelf : unmanaged, IVector4<TSelf>;

public interface IVector4<TSelf, TScalar> : IVector4<TSelf>, IVector<TSelf, TScalar>
    where TSelf : unmanaged, IVector4<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Create

    /// <summary>
    /// Creates a vector from its components
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, TScalar y, TScalar z, TScalar w);

    #endregion
}

public interface IVector4CtorFromVector2<TSelf, TScalar, TVector2> : IVector4<TSelf, TScalar>
    where TSelf : unmanaged, IVector4CtorFromVector2<TSelf, TScalar, TVector2>
    where TScalar : unmanaged
{
    #region Create

    /// <summary>
    /// Creates a vector from two pairs of components
    /// </summary>
    /// <param name="xy">The <c>x</c> and <c>y</c> components</param>
    /// <param name="zw">The <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(in TVector2 xy, in TVector2 zw);

    /// <summary>
    /// Creates a vector from the pair of the <c>x</c> and <c>y</c> components and the two other components
    /// </summary>
    /// <param name="xy">The <c>x</c> and <c>y</c> components</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(in TVector2 xy, TScalar z, TScalar w);

    /// <summary>
    /// Creates a vector from the two first components and the pair of the <c>z</c> and <c>w</c> ones
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="zw">The <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, TScalar y, in TVector2 zw);

    /// <summary>
    /// Creates a vector from the <c>x</c> and <c>w</c> components and the pair of the <c>y</c> and <c>z</c>
    /// ones
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="yz">The <c>y</c> and <c>z</c> components</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, in TVector2 yz, TScalar w);

    #endregion

    #region Insert

    /// <summary>
    /// Creates a vector that has the <c>y</c> and <c>z</c> components of <paramref name="yz"/> and the
    /// <c>x</c> and <c>w</c> components of <paramref name="xw"/>
    /// </summary>
    /// <param name="xw">The <c>x</c> and <c>w</c> components</param>
    /// <param name="yz">The <c>y</c> and <c>z</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertYZ(in TVector2 xw, in TVector2 yz);

    /// <summary>
    /// Creates a vector that has the <c>y</c> and <c>z</c> components of <paramref name="y"/> and
    /// <paramref name="z"/> and the <c>x</c> and <c>w</c> components of <paramref name="xw"/>
    /// </summary>
    /// <param name="xw">The <c>x</c> and <c>w</c> components</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertYZ(in TVector2 xw, TScalar y, TScalar z);

    /// <summary>
    /// Creates a vector that has the <c>x</c> and <c>w</c> components of <paramref name="xw"/> and the
    /// <c>y</c> and <c>z</c> components of <paramref name="yz"/>
    /// </summary>
    /// <param name="yz">The <c>y</c> and <c>z</c> components</param>
    /// <param name="xw">The <c>x</c> and <c>w</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertXW(in TVector2 yz, in TVector2 xw);

    /// <summary>
    /// Creates a vector that has the <c>x</c> and <c>w</c> components of <paramref name="x"/> and
    /// <paramref name="w"/> and the <c>y</c> and <c>z</c> components of <paramref name="yz"/>
    /// </summary>
    /// <param name="yz">The <c>y</c> and <c>z</c> components</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertXW(in TVector2 yz, TScalar x, TScalar w);

    /// <summary>
    /// Creates a vector that has the <c>y</c> and <c>w</c> components of <paramref name="yw"/> and the
    /// <c>x</c> and <c>z</c> components of <paramref name="xz"/>
    /// </summary>
    /// <param name="xz">The <c>x</c> and <c>z</c> components</param>
    /// <param name="yw">The <c>y</c> and <c>w</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertYW(in TVector2 xz, in TVector2 yw);

    /// <summary>
    /// Creates a vector that has the <c>y</c> and <c>w</c> components of <paramref name="y"/> and
    /// <paramref name="w"/> and the <c>x</c> and <c>z</c> components of <paramref name="xz"/>
    /// </summary>
    /// <param name="xz">The <c>x</c> and <c>z</c> components</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertYW(in TVector2 xz, TScalar y, TScalar w);

    /// <summary>
    /// Creates a vector that has the <c>x</c> and <c>z</c> components of <paramref name="xz"/> and the
    /// <c>y</c> and <c>w</c> components of <paramref name="yw"/>
    /// </summary>
    /// <param name="yw">The <c>y</c> and <c>w</c> components</param>
    /// <param name="xz">The <c>x</c> and <c>z</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertXZ(in TVector2 yw, in TVector2 xz);

    /// <summary>
    /// Creates a vector that has the <c>x</c> and <c>z</c> components of <paramref name="x"/> and
    /// <paramref name="z"/> and the <c>y</c> and <c>w</c> components of <paramref name="yw"/>
    /// </summary>
    /// <param name="yw">The <c>y</c> and <c>w</c> components</param>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertXZ(in TVector2 yw, TScalar x, TScalar z);

    #endregion
}

public interface IVector4CtorFromVector3<TSelf, TScalar, TVector3> : IVector4<TSelf, TScalar>
    where TSelf : unmanaged, IVector4CtorFromVector3<TSelf, TScalar, TVector3>
    where TScalar : unmanaged
{
    #region Create

    /// <summary>
    /// Creates a vector from the triple of the <c>x</c>, <c>y</c> and <c>z</c> components and the <c>w</c>
    /// component
    /// </summary>
    /// <param name="xyz">The <c>x</c>, <c>y</c> and <c>z</c> components</param>
    /// <param name="w">The <c>w</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(in TVector3 xyz, TScalar w);

    /// <summary>
    /// Creates a vector from the <c>x</c> component and the triple of the <c>y</c>, <c>z</c> and <c>w</c>
    /// components
    /// </summary>
    /// <param name="x">The <c>x</c> component</param>
    /// <param name="yzw">The <c>y</c>, <c>z</c> and <c>w</c> components</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Create(TScalar x, in TVector3 yzw);

    #endregion

    #region Insert

    /// <summary>
    /// Creates a vector that has the <c>y</c> component of <paramref name="y"/> and the <c>x</c>, <c>z</c>
    /// and <c>w</c> components of <paramref name="xzw"/>
    /// </summary>
    /// <param name="xzw">The <c>x</c>, <c>z</c> and <c>w</c> components</param>
    /// <param name="y">The <c>y</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertY(in TVector3 xzw, TScalar y);

    /// <summary>
    /// Creates a vector that has the <c>z</c> component of <paramref name="z"/> and the <c>x</c>, <c>y</c>
    /// and <c>w</c> components of <paramref name="xyw"/>
    /// </summary>
    /// <param name="xyw">The <c>x</c>, <c>y</c> and <c>w</c> components</param>
    /// <param name="z">The <c>z</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf InsertZ(in TVector3 xyw, TScalar z);

    #endregion
}

#endregion
