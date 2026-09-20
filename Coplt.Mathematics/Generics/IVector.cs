namespace Coplt.Mathematics.Generics;

public interface IVector<TSelf, TScalar> :
    IEquatable<TSelf>, IComparable<TSelf>, IComparable,
    IComparisonOperators<TSelf, TSelf, bool>,
    IBitwiseOperators<TSelf, TSelf, TSelf>,
    IShiftOperators<TSelf, TSelf, TScalar>
    where TSelf : unmanaged, IVector<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Meta

    public static abstract bool IsSimdAccelerated { get; }
    public static abstract int Length { get; }
    public static abstract int SizeByte { get; }
    public static abstract int SizeBit { get; }

    #endregion

    #region Constants

    public static abstract TSelf Zero { get; }
    public static abstract TSelf One { get; }
    public static abstract TSelf Two { get; }

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
