namespace Coplt.Mathematics.Algebras;

#region Core

public interface IMatrix<TSelf> : IAlgebra<TSelf>
    where TSelf : unmanaged, IMatrix<TSelf>
{
    #region Meta

    public static abstract int Width { get; }
    public static abstract int Height { get; }

    #endregion

    #region Constants

    public static abstract TSelf Identity { get; }

    #endregion
}

public interface IMatrixVector<TSelf, TVector> :
    IMatrix<TSelf>
    where TSelf : unmanaged, IMatrixVector<TSelf, TVector>
    where TVector : unmanaged, IVector<TVector>
{
    #region Constants

    public static abstract TVector VectorZero { get; }
    public static abstract TVector VectorOne { get; }
    public static abstract TVector VectorTwo { get; }

    #endregion

    #region Ctor

    public static abstract TSelf Broadcast(TVector scalar);
    public static abstract TSelf Vector(TVector scalar);
    public static abstract TSelf Load(ReadOnlySpan<TVector> span);
    public static abstract unsafe TSelf Load(TVector* ptr);

    #endregion

    #region Index

    public static abstract TVector get_vector(in TSelf self, int index);
    public static abstract void set_vector(ref TSelf self, int index, TVector value);

    #endregion
}

public interface IMatrixScalar<TSelf, TScalar> :
    IMatrix<TSelf>, IAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IMatrixScalar<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Index

    public static abstract TScalar get(in TSelf self, int column, int row);
    public static abstract void set(ref TSelf self, int column, int row, TScalar value);

    #endregion
}

#endregion

#region Bool

public interface IBoolMatrix<TSelf> : IMatrix<TSelf>, IBoolAlgebra<TSelf>
    where TSelf : unmanaged, IBoolMatrix<TSelf>;

public interface IBoolMatrix<TSelf, TScalar> : IMatrixScalar<TSelf, TScalar>, IBoolAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IBoolMatrix<TSelf, TScalar>
    where TScalar : unmanaged;

#endregion

#region Number

public interface INumberMatrix<TSelf> : IMatrix<TSelf>, INumberAlgebra<TSelf>
    where TSelf : unmanaged, INumberMatrix<TSelf>;

public interface INumberMatrix<TSelf, TScalar> : IMatrixScalar<TSelf, TScalar>, INumberAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, INumberMatrix<TSelf, TScalar>
    where TScalar : unmanaged, INumberBase<TScalar>;

#endregion

#region Signed

public interface ISignedNumberMatrix<TSelf> : INumberMatrix<TSelf>
    where TSelf : unmanaged, ISignedNumberMatrix<TSelf>;

public interface ISignedNumberMatrix<TSelf, TScalar> : INumberMatrix<TSelf, TScalar>
    where TSelf : unmanaged, ISignedNumberMatrix<TSelf, TScalar>
    where TScalar : unmanaged, INumberBase<TScalar>;

#endregion

#region Matrix2x2

public interface IMatrix2x2<TSelf> : IMatrix<TSelf>
    where TSelf : unmanaged, IMatrix2x2<TSelf>;

public interface IMatrix2x2Vector<TSelf, TVector> : IMatrix2x2<TSelf>, IMatrixVector<TSelf, TVector>
    where TSelf : unmanaged, IMatrix2x2Vector<TSelf, TVector>
    where TVector : unmanaged, IVector<TVector>
{
    #region Create

    public static abstract TSelf Create(in TVector c0, in TVector c1);

    #endregion
}

public interface IMatrix2x2Scalar<TSelf, TScalar> : IMatrix2x2<TSelf>, IMatrixScalar<TSelf, TScalar>
    where TSelf : unmanaged, IMatrix2x2Scalar<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Create

    public static abstract TSelf Create(TScalar m00, TScalar m01, TScalar m10, TScalar m11);

    #endregion
}

#endregion
