namespace Coplt.Mathematics.Generics;

public interface IVector<TSelf, TScalar> :
    IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>,
    IBitwiseOperators<TSelf, TSelf, TSelf>
    where TSelf : unmanaged, IVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Meta

    public static abstract bool IsSimdAccelerated { get; }
    public static abstract int Length { get; }
    public static abstract int SizeByte { get; }
    public static abstract int SizeBit { get; }

    #endregion

    #region Ctor

    public static abstract TSelf Broadcast(TScalar scalar);
    public static abstract TSelf Scalar(TScalar scalar);
    public static abstract TSelf Load(ReadOnlySpan<TScalar> span);
    public static abstract unsafe TSelf Load(TScalar* ptr);

    #endregion

    #region Index

    public TScalar this[int index] { get; set; }

    #endregion
}

/// <summary>
/// A vector of numbers, it has the numeric constants and the ordering and shift operators.
/// </summary>
public interface INumberVector<TSelf, TScalar> :
    IVector<TSelf, TScalar>,
    IComparable<TSelf>, IComparable,
    IComparisonOperators<TSelf, TSelf, bool>,
    IShiftOperators<TSelf, int, TSelf>
    where TSelf : unmanaged, INumberVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Constants

    public static abstract TSelf Zero { get; }
    public static abstract TSelf One { get; }
    public static abstract TSelf Two { get; }

    /// <summary>
    /// The scalar zero
    /// </summary>
    public static abstract TScalar ScalarZero { get; }

    /// <summary>
    /// The scalar one
    /// </summary>
    public static abstract TScalar ScalarOne { get; }

    /// <summary>
    /// The scalar two
    /// </summary>
    public static abstract TScalar ScalarTwo { get; }

    #endregion
}

/// <summary>
/// A vector of booleans, it is a mask, so it has no numeric constants, no ordering and no shifts.
/// </summary>
public interface IBoolVector<TSelf, TScalar> :
    IVector<TSelf, TScalar>
    where TSelf : unmanaged, IBoolVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Constants

    public static abstract TSelf True { get; }
    public static abstract TSelf False { get; }

    #endregion
}
