namespace Coplt.Mathematics.Generics;

public interface IVector<Self, Scalar> :
    IEquatable<Self>, IComparable<Self>, IComparable,
    IComparisonOperators<Self, Self, bool>,
    IBitwiseOperators<Self, Self, Self>,
    IShiftOperators<Self, Self, Scalar>
    where Self : unmanaged, IVector<Self, Scalar>
    where Scalar : unmanaged
{
    #region Meta

    public static abstract bool IsSimdAccelerated { get; }
    public static abstract int Length { get; }
    public static abstract int SizeByte { get; }
    public static abstract int SizeBit { get; }

    #endregion

    #region Constants

    public static abstract Self Zero { get; }
    public static abstract Self One { get; }
    public static abstract Self Two { get; }

    #endregion

    #region Ctor

    public static abstract Self CreateBroadcast(Scalar scalar);
    public static abstract Self CreateScalar(Scalar scalar);
    public static abstract Self Load(ReadOnlySpan<Scalar> span);
    public static abstract Self Load(Scalar* ptr);

    #endregion

    #region Index

    public Scalar this[int index] { get; set; }

    #endregion
}
