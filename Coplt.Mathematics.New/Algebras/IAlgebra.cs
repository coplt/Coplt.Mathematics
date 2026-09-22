namespace Coplt.Mathematics.Algebras;

#region Core

public interface IAlgebra<TSelf> :
    IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>,
    IBitwiseOperators<TSelf, TSelf, TSelf>,
    ISpanFormattable, IUtf8SpanFormattable
    where TSelf : unmanaged, IAlgebra<TSelf>
{
    #region Meta

    public static abstract bool IsSimdAccelerated { get; }
    public static abstract int SizeByte { get; }
    public static abstract int SizeBit { get; }

    #endregion
}

public interface IAlgebra<TSelf, TScalar> : IAlgebra<TSelf>
    where TSelf : unmanaged, IAlgebra<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Ctor

    public static abstract TSelf Broadcast(TScalar scalar);
    public static abstract TSelf Scalar(TScalar scalar);
    public static abstract TSelf Load(ReadOnlySpan<TScalar> span);
    public static abstract unsafe TSelf Load(TScalar* ptr);

    #endregion

    #region Index

    public static abstract TScalar get(in TSelf self, int index);
    public static abstract void set(ref TSelf self, int index, TScalar value);

    #endregion
}

#endregion

#region Bool

public interface IBoolAlgebra<TSelf> : IAlgebra<TSelf>
    where TSelf : unmanaged, IBoolAlgebra<TSelf>
{
    #region Constants

    public static abstract TSelf True { get; }

    public static abstract TSelf False { get; }

    #endregion
}

public interface IBoolAlgebra<TSelf, TScalar> :
    IBoolAlgebra<TSelf>,
    IAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IBoolAlgebra<TSelf, TScalar>
    where TScalar : unmanaged;

#endregion

#region Number

public interface INumberAlgebra<TSelf> : IAlgebra<TSelf>,
    IComparable<TSelf>, IComparable,
    IComparisonOperators<TSelf, TSelf, bool>,
    IShiftOperators<TSelf, int, TSelf>,
    IUnaryPlusOperators<TSelf, TSelf>,
    IAdditionOperators<TSelf, TSelf, TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IModulusOperators<TSelf, TSelf, TSelf>
    where TSelf : unmanaged, INumberAlgebra<TSelf>
{
    #region Constants

    public static abstract TSelf Zero { get; }

    public static abstract TSelf One { get; }

    public static abstract TSelf Two { get; }

    #endregion
}

public interface INumberAlgebra<TSelf, TScalar> :
    INumberAlgebra<TSelf>,
    IAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, INumberAlgebra<TSelf, TScalar>
    where TScalar : unmanaged, INumberBase<TScalar>
{
    #region Constants

    public static abstract TScalar ScalarZero { get; }

    public static abstract TScalar ScalarOne { get; }

    public static abstract TScalar ScalarTwo { get; }

    #endregion
}

#endregion

#region Signed

public interface ISignedAlgebra<TSelf> :
    INumberAlgebra<TSelf>,
    IUnaryNegationOperators<TSelf, TSelf>
    where TSelf : unmanaged, ISignedAlgebra<TSelf>;

public interface ISignedAlgebra<TSelf, TScalar> :
    INumberAlgebra<TSelf, TScalar>,
    ISignedAlgebra<TSelf>
    where TSelf : unmanaged, ISignedAlgebra<TSelf, TScalar>
    where TScalar : unmanaged, INumberBase<TScalar>;

#endregion
