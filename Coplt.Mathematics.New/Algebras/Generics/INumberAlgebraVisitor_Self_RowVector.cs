namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The visitor that reduces the rows of the value to a single vector
/// <para>The rows of a matrix have no value of their own, so the columns of it are handed over as the vectors
/// they are and every one of them is reduced to a single component: the member of a count of the columns is the
/// one that reaches them and the values of the reductions are the components of the vector it builds</para>
/// <para>It is the visitor of <see cref="INumberMatrixRowDispatch{TSelf,TVector}"/>, which the new design does
/// not name yet: the value of a column is handed over as a vector and the visitor reaches it through the
/// dispatch of the value of the vector.</para>
/// </summary>
/// <typeparam name="V">The type of the visitor itself</typeparam>
public interface INumberAlgebraVisitor_Self_RowVector<V> where V : INumberAlgebraVisitor_Self_RowVector<V>
{
    /// <summary>Reduces the two columns of a matrix of 2 columns to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <returns>The row of the matrix</returns>
    public static abstract TRow AcceptMatrixRow2<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1)
        where TColumn : unmanaged, Dispatch.IAlgebraDispatch<TColumn, TScalar>, INumberVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector2<TRow, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    /// <summary>Reduces the three columns of a matrix of 3 columns to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <returns>The row of the matrix</returns>
    public static abstract TRow AcceptMatrixRow3<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1, in TColumn c2)
        where TColumn : unmanaged, Dispatch.IAlgebraDispatch<TColumn, TScalar>, INumberVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector3<TRow, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    /// <summary>Reduces the four columns of a matrix of 4 columns to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <param name="c3">The fourth column</param>
    /// <returns>The row of the matrix</returns>
    public static abstract TRow AcceptMatrixRow4<TColumn, TRow, TScalar>(
        in TColumn c0, in TColumn c1, in TColumn c2, in TColumn c3)
        where TColumn : unmanaged, Dispatch.IAlgebraDispatch<TColumn, TScalar>, INumberVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector4<TRow, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;
}
