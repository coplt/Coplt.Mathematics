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

    /// <summary>
    /// Creates a value with every component set to <paramref name="scalar"/>
    /// <para>It builds the whole register of the value and masks the padding lanes of it, so it does not need
    /// the platform to leave the lanes that follow the one of the register of a scalar at zero</para>
    /// </summary>
    public static abstract TSelf Broadcast(TScalar scalar);

    /// <summary>
    /// Creates a value with every component set to <paramref name="scalar"/>, without building the whole
    /// register of it
    /// <para>It shuffles the register of the scalar into the value, which only leaves the padding lanes of it
    /// at zero on a platform whose hardware zeroes the lanes that follow the one of the register of a scalar,
    /// so it is the one the code that knows the platform of uses instead of <see cref="Broadcast"/></para>
    /// </summary>
    public static abstract TSelf BroadcastUnsafe(TScalar scalar);

    /// <summary>
    /// Creates a value with only the first component set to <paramref name="scalar"/>
    /// <para>It leaves the lanes that follow the one of the register of the scalar as they are, which only
    /// leaves the padding lanes of it at zero on a platform whose hardware zeroes the lanes that follow the one
    /// of the register of a scalar, so it is the one the code that knows the platform of uses instead of
    /// <see cref="Scalar"/></para>
    /// </summary>
    public static abstract TSelf ScalarUnsafe(TScalar scalar);

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
