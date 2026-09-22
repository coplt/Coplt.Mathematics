namespace Coplt.Mathematics.Generics;

/// <summary>
/// A <see cref="IVector{TSelf}"/> of 2 components
/// <para>It names the size of a vector whose component type is not known, so that generic code can constrain
/// the shape of a vector without constraining its components</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
public interface IVector2<TSelf> : IVector<TSelf> where TSelf : unmanaged, IVector2<TSelf>;

/// <summary>
/// A <see cref="IVector{TSelf}"/> of 3 components
/// <para>It names the size of a vector whose component type is not known, so that generic code can constrain
/// the shape of a vector without constraining its components</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
public interface IVector3<TSelf> : IVector<TSelf> where TSelf : unmanaged, IVector3<TSelf>;

/// <summary>
/// A <see cref="IVector{TSelf}"/> of 4 components
/// <para>It names the size of a vector whose component type is not known, so that generic code can constrain
/// the shape of a vector without constraining its components</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
public interface IVector4<TSelf> : IVector<TSelf> where TSelf : unmanaged, IVector4<TSelf>;

/// <summary>
/// A <see cref="IVector{TSelf,TScalar}"/> of 2 components, its components are reached by name
/// <para>It is the size of the vector, the members every vector has and the two components of it together, a
/// vector of 2 components implements this interface and no other one of its size</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector2<TSelf, TScalar> :
    IVector2<TSelf>, IVector<TSelf, TScalar>,
    IVector2Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector2<TSelf, TScalar>
    where TScalar : unmanaged;

/// <summary>
/// A <see cref="IVector{TSelf,TScalar}"/> of 3 components, its components are reached by name
/// <para>It is the size of the vector, the members every vector has and the three components of it together,
/// a vector of 3 components implements this interface and no other one of its size</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector3<TSelf, TScalar> :
    IVector3<TSelf>, IVector<TSelf, TScalar>,
    IVector3Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector3<TSelf, TScalar>
    where TScalar : unmanaged;

/// <summary>
/// A <see cref="IVector{TSelf,TScalar}"/> of 4 components, its components are reached by name
/// <para>It is the size of the vector, the members every vector has and the four components of it together, a
/// vector of 4 components implements this interface and no other one of its size</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector4<TSelf, TScalar> :
    IVector4<TSelf>, IVector<TSelf, TScalar>,
    IVector4Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector4<TSelf, TScalar>
    where TScalar : unmanaged;
