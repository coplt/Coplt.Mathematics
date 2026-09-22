namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector that is created from a broadcast value, from a single component or from the beginning of a span or
/// a pointer
/// <para>The members are static members of the vector itself instead of constructors of it, so a caller that
/// only knows a type parameter can create a vector of it as well, see <see cref="IVector{TSelf,TScalar}"/></para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVectorCtor<out TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Ctor

    /// <summary>
    /// Creates a vector with every component set to <paramref name="scalar"/>
    /// </summary>
    /// <param name="scalar">The value of every component</param>
    /// <returns>The broadcast vector</returns>
    public static abstract TSelf Broadcast(TScalar scalar);

    /// <summary>
    /// Creates a vector with only the <c>x</c> component set to <paramref name="scalar"/>
    /// <para>The other components are zero</para>
    /// </summary>
    /// <param name="scalar">The value of the <c>x</c> component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Scalar(TScalar scalar);

    /// <summary>
    /// Loads a vector from the beginning of <paramref name="span"/>
    /// <para>A simd backed vector reads a whole simd register, so the span has to be at least as long as the padded vector</para>
    /// </summary>
    /// <param name="span">The span to load from</param>
    /// <returns>The loaded vector</returns>
    public static abstract TSelf Load(ReadOnlySpan<TScalar> span);

    /// <summary>
    /// Loads a vector from <paramref name="ptr"/>
    /// <para>A simd backed vector reads a whole simd register, so the pointer has to point to at least as many components as the padded vector</para>
    /// </summary>
    /// <param name="ptr">The pointer to load from</param>
    /// <returns>The loaded vector</returns>
    public static abstract unsafe TSelf Load(TScalar* ptr);

    #endregion
}

/// <summary>
/// A <see cref="IVectorCtor{TSelf,TScalar}"/> of 2 components that is also created from its components
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector2Ctor<out TSelf, TScalar> :
    IVectorCtor<TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
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

/// <summary>
/// A <see cref="IVectorCtor{TSelf,TScalar}"/> of 3 components that is also created from its components
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector3Ctor<out TSelf, TScalar> :
    IVectorCtor<TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
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

/// <summary>
/// A <see cref="IVectorCtor{TSelf,TScalar}"/> of 4 components that is also created from its components
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector4Ctor<out TSelf, TScalar> :
    IVectorCtor<TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
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

/// <summary>
/// A <see cref="IVector3Ctor{TSelf,TScalar}"/> of 3 components that is also created by merging a pair of
/// components and a single value
/// <para>The name of a pair is the name of the components it holds and the name of a single value is the name
/// of the component it is: <c>Create(xy, z)</c> takes the <c>x</c> and <c>y</c> components from the pair and
/// the <c>z</c> one from the value, while <c>Create(x, yz)</c> takes the <c>y</c> and <c>z</c> ones from the
/// pair, and <c>InsertY(xz, y)</c> takes the <c>x</c> and <c>z</c> ones from the pair and the <c>y</c> one
/// from the value</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector2">The 2 component vector of the same component type</typeparam>
public interface IVector3CtorFromVector2<out TSelf, TScalar, TVector2> :
    IVector3Ctor<TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
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

/// <summary>
/// A <see cref="IVector4Ctor{TSelf,TScalar}"/> of 4 components that is also created by merging two pairs of
/// components
/// <para>The name of a pair is the name of the components it holds: <c>Create(xy, zw)</c> is the vector whose
/// beginning is the first pair and whose end is the second one, while <c>InsertYZ(xw, yz)</c> is the one that
/// takes the <c>y</c> and <c>z</c> components from the second pair and the <c>x</c> and <c>w</c> ones from the
/// first one</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector2">The 2 component vector of the same component type</typeparam>
public interface IVector4CtorFromVector2<out TSelf, TScalar, TVector2> :
    IVector4Ctor<TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
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

/// <summary>
/// A <see cref="IVector4Ctor{TSelf,TScalar}"/> of 4 components that is also created by merging a triple of
/// components and a single value
/// <para>The name of a triple is the name of the components it holds and the name of a single value is the
/// name of the component it is: <c>Create(xyz, w)</c> is the vector whose beginning is the triple and whose
/// <c>w</c> component is the value, while <c>InsertY(xzw, y)</c> takes the <c>x</c>, <c>z</c> and <c>w</c>
/// components from the triple</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
/// <typeparam name="TVector3">The 3 component vector of the same component type</typeparam>
public interface IVector4CtorFromVector3<out TSelf, TScalar, TVector3> :
    IVector4Ctor<TSelf, TScalar>
    where TSelf : unmanaged, IVectorCtor<TSelf, TScalar>
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
