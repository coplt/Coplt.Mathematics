namespace Coplt.Mathematics.Generics;

/// <summary>
/// The base of every vector, it only declares what a number vector and a bool vector have in common:
/// the meta data, the constructors, the indexer and the bitwise operators
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector<TSelf, TScalar> :
    IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>,
    IBitwiseOperators<TSelf, TSelf, TSelf>
    where TSelf : unmanaged, IVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Meta

    /// <summary>
    /// True when the vector is backed by a hardware accelerated simd type, the operators of the vector still
    /// work when it is false, they only fall back to the component wise implementation
    /// </summary>
    public static abstract bool IsSimdAccelerated { get; }

    /// <summary>
    /// The number of components of the vector
    /// </summary>
    public static abstract int Length { get; }

    /// <summary>
    /// The size of the vector in bytes, a 3 component vector is padded to the size of a 4 component one
    /// </summary>
    public static abstract int SizeByte { get; }

    /// <summary>
    /// The size of the vector in bits, a 3 component vector is padded to the size of a 4 component one
    /// </summary>
    public static abstract int SizeBit { get; }

    #endregion

    #region Ctor

    /// <summary>
    /// Creates a vector with every component set to <paramref name="scalar"/>
    /// </summary>
    /// <param name="scalar">The value of every component</param>
    /// <returns>The broadcast vector</returns>
    public static abstract TSelf Broadcast(TScalar scalar);

    /// <summary>
    /// Creates a vector with only the x component set to <paramref name="scalar"/>, the other components are zero
    /// </summary>
    /// <param name="scalar">The value of the x component</param>
    /// <returns>The vector</returns>
    public static abstract TSelf Scalar(TScalar scalar);

    /// <summary>
    /// Loads a vector from the beginning of <paramref name="span"/>, a simd backed vector reads a whole simd
    /// register, so the span has to be at least as long as the padded vector
    /// </summary>
    /// <param name="span">The span to load from</param>
    /// <returns>The loaded vector</returns>
    public static abstract TSelf Load(ReadOnlySpan<TScalar> span);

    /// <summary>
    /// Loads a vector from <paramref name="ptr"/>, a simd backed vector reads a whole simd register, so the
    /// pointer has to point to at least as many components as the padded vector
    /// </summary>
    /// <param name="ptr">The pointer to load from</param>
    /// <returns>The loaded vector</returns>
    public static abstract unsafe TSelf Load(TScalar* ptr);

    #endregion

    #region Index

    /// <summary>
    /// Gets or sets the component at <paramref name="index"/>, 0 is x, 1 is y, 2 is z and 3 is w
    /// </summary>
    /// <param name="index">The index of the component</param>
    /// <returns>The component</returns>
    /// <exception cref="System.IndexOutOfRangeException">When the index is not in the range of the vector</exception>
    public TScalar this[int index] { get; set; }

    #endregion
}

/// <summary>
/// A vector of numbers, it adds the numeric constants, the ordering operators and the shift operators
/// to <see cref="IVector{TSelf,TScalar}"/>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface INumberVector<TSelf, TScalar> :
    IVector<TSelf, TScalar>,
    IComparable<TSelf>, IComparable,
    IComparisonOperators<TSelf, TSelf, bool>,
    IShiftOperators<TSelf, int, TSelf>
    where TSelf : unmanaged, INumberVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Constants

    /// <summary>
    /// A vector with every component set to zero
    /// </summary>
    public static abstract TSelf Zero { get; }

    /// <summary>
    /// A vector with every component set to one
    /// </summary>
    public static abstract TSelf One { get; }

    /// <summary>
    /// A vector with every component set to two
    /// </summary>
    public static abstract TSelf Two { get; }

    /// <summary>
    /// The scalar zero, it is here because <typeparamref name="TScalar"/> is not constrained to a number interface
    /// </summary>
    public static abstract TScalar ScalarZero { get; }

    /// <summary>
    /// The scalar one, it is here because <typeparamref name="TScalar"/> is not constrained to a number interface
    /// </summary>
    public static abstract TScalar ScalarOne { get; }

    /// <summary>
    /// The scalar two, it is here because <typeparamref name="TScalar"/> is not constrained to a number interface
    /// </summary>
    public static abstract TScalar ScalarTwo { get; }

    #endregion
}

/// <summary>
/// A vector of booleans, it is a mask, so it has no numeric constants, no ordering operators and no shift operators
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IBoolVector<TSelf, TScalar> :
    IVector<TSelf, TScalar>
    where TSelf : unmanaged, IBoolVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Constants

    /// <summary>
    /// A mask with every component set to true
    /// </summary>
    public static abstract TSelf True { get; }

    /// <summary>
    /// A mask with every component set to false, it is the same as <c>default</c>
    /// </summary>
    public static abstract TSelf False { get; }

    #endregion
}
