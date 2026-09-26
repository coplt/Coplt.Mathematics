namespace Coplt.Mathematics.Algebras;

#region Core

/// <summary>
/// A matrix of the algebra library: it has a shape, it has an identity, and the values it is made of are the
/// columns of it
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
public interface IMatrix<TSelf> : IAlgebra<TSelf>
    where TSelf : unmanaged, IMatrix<TSelf>
{
    #region Meta

    public static abstract int Rows { get; }
    public static abstract int Columns { get; }
    public static abstract int Length { get; }

    #endregion

    #region Constants

    public static abstract TSelf Identity { get; }

    #endregion
}

/// <summary>
/// An <see cref="IMatrix{TSelf}"/> that names the type of a column of it
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
/// <typeparam name="TVector">The type of a column of the matrix</typeparam>
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

    public static abstract TSelf Broadcast(in TVector scalar);
    public static abstract TSelf Vector(in TVector scalar);
    public static abstract TSelf Load(ReadOnlySpan<TVector> span);
    public static abstract unsafe TSelf Load(TVector* ptr);

    #endregion

    #region Index

    public static abstract TVector get_vector(in TSelf self, int index);
    public static abstract void set_vector(ref TSelf self, int index, in TVector value);

    #endregion
}

/// <summary>
/// An <see cref="IMatrix{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IMatrixScalar<TSelf, TScalar> :
    IMatrix<TSelf>, IAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IMatrixScalar<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Index

    public static abstract TScalar get(in TSelf self, int row, int column);
    public static abstract void set(ref TSelf self, int row, int column, TScalar value);

    #endregion
}

#endregion

#region Bool

/// <summary>
/// An <see cref="IMatrix{TSelf}"/> of a mask: every component of it is a bit that says whether a condition
/// holds
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
public interface IBoolMatrix<TSelf> : IMatrix<TSelf>, IBoolAlgebra<TSelf>
    where TSelf : unmanaged, IBoolMatrix<TSelf>;

/// <summary>
/// An <see cref="IBoolMatrix{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IBoolMatrix<TSelf, TScalar> :
    IBoolMatrix<TSelf>,
    IMatrixScalar<TSelf, TScalar>,
    IBoolAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IBoolMatrix<TSelf>, IBoolMatrix<TSelf, TScalar>
    where TScalar : unmanaged;

#endregion

#region Number

/// <summary>
/// An <see cref="IMatrix{TSelf}"/> of a number: it is ordered, it shifts, and it adds, subtracts, multiplies,
/// divides and takes the remainder of two values
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
public interface INumberMatrix<TSelf> : IMatrix<TSelf>, INumberAlgebra<TSelf>
    where TSelf : unmanaged, INumberMatrix<TSelf>;

/// <summary>
/// An <see cref="INumberMatrix{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface INumberMatrix<TSelf, TScalar> :
    INumberMatrix<TSelf>,
    IMatrixScalar<TSelf, TScalar>,
    INumberAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, INumberMatrix<TSelf>, INumberMatrix<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryNumber<TScalar>;

#endregion

#region Signed

/// <summary>
/// An <see cref="INumberMatrix{TSelf}"/> of a number that has a sign, so the value of it can be negated
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
public interface ISignedNumberMatrix<TSelf> :
    INumberMatrix<TSelf>,
    ISignedAlgebra<TSelf>
    where TSelf : unmanaged, ISignedNumberMatrix<TSelf>;

/// <summary>
/// An <see cref="ISignedNumberMatrix{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface ISignedNumberMatrix<TSelf, TScalar> :
    ISignedNumberMatrix<TSelf>,
    INumberMatrix<TSelf, TScalar>,
    ISignedAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, ISignedNumberMatrix<TSelf>, ISignedNumberMatrix<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryNumber<TScalar>, ISignedNumber<TScalar>;

#endregion

#region FloatingPoint

/// <summary>
/// An <see cref="ISignedNumberMatrix{TSelf}"/> of a floating point number: it reaches the math constants of the
/// kind of it, which are the ones of every component of the matrix, and the ones of the ieee 754 standard,
/// which every floating point type of the library names
/// </summary>
public interface IFloatingPointMatrix<TSelf> :
    ISignedNumberMatrix<TSelf>,
    IFloatingPointAlgebra<TSelf>
    where TSelf : unmanaged, IFloatingPointMatrix<TSelf>;

/// <summary>
/// An <see cref="IFloatingPointMatrix{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the matrix itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IFloatingPointMatrix<TSelf, TScalar> :
    IFloatingPointMatrix<TSelf>,
    ISignedNumberMatrix<TSelf, TScalar>,
    IFloatingPointAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IFloatingPointMatrix<TSelf>, IFloatingPointMatrix<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>;

#endregion

// The interfaces of the shapes of a matrix are generated by Coplt.Analyzers.Generators.MatrixShapeGenerator.
// Every shape of 2 rows to 4 rows and 2 columns to 4 columns has the interface of its own, the form that
// reaches the columns of the matrix through a vector and the form that reaches the components of it by a
// scalar.
