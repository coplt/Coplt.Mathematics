namespace Coplt.Mathematics.Algebras.Generics.Dispatch;

#region Column Dispatch

/// <summary>
/// The dispatch of the value of a matrix to the visitor that combines the columns of it into a single vector
/// <para>A matrix is made of its columns, so the vectors it is made of are the ones a visitor combines into the
/// value of a single vector, which is the combination of the columns of the matrix. The count of the columns of a
/// matrix is a part of its type, so the type of a column of it is a part of this interface as well: a member that
/// combines the columns of a value does not name the type of a single component, which is the one the member of
/// the visitor it reaches names.</para>
/// </summary>
/// <typeparam name="TSelf">The matrix type itself</typeparam>
/// <typeparam name="TVector">The type of a column of the matrix</typeparam>
public interface IMatrixColumnDispatch<TSelf, TVector> : IAlgebraDispatch<TSelf>
    where TSelf : unmanaged, IMatrixColumnDispatch<TSelf, TVector>
    where TVector : unmanaged, INumberVector<TVector>
{
    /// <summary>
    /// Hands the columns of <paramref name="self"/> over to <typeparamref name="V"/>, which combines them into a
    /// single vector
    /// <para>The count of the columns of the matrix decides the member of the visitor that reaches them</para>
    /// </summary>
    /// <typeparam name="V">The type of the visitor that combines the columns</typeparam>
    /// <param name="self">The matrix whose columns are handed over</param>
    /// <returns>The vector the visitor built out of the columns</returns>
    public static abstract TVector Combine<V>(in TSelf self)
        where V : IMatrixColumnVisitor<V>;
}

#endregion

#region Row Dispatch

/// <summary>
/// The dispatch of the value of a matrix to the visitor that reduces the rows of it to a single vector
/// <para>A row of a matrix has no value of its own, so the columns of the matrix are handed over as the vectors
/// they are and every one of them is reduced to a single component: the component of the result at the index of a
/// column of the matrix is the value of the reduction of that column of it. The row of a matrix of a shape is not
/// a part of the type of the matrix itself, so this interface names the type of a row of it, which a matrix of
/// the transposed shape shares with it.</para>
/// </summary>
/// <typeparam name="TSelf">The matrix type itself</typeparam>
/// <typeparam name="TVector">The type of a row of the matrix</typeparam>
public interface IMatrixRowDispatch<TSelf, TVector> : IAlgebraDispatch<TSelf>
    where TSelf : unmanaged, IMatrixRowDispatch<TSelf, TVector>
    where TVector : unmanaged, INumberVector<TVector>
{
    /// <summary>
    /// Hands the columns of <paramref name="self"/> over to <typeparamref name="V"/>, which reduces every one of
    /// them to a single component and builds the vector of the values of the reductions
    /// <para>The count of the columns of the matrix decides the member of the visitor that reaches them</para>
    /// </summary>
    /// <typeparam name="V">The type of the visitor that reduces the columns</typeparam>
    /// <param name="self">The matrix whose columns are handed over</param>
    /// <returns>The vector of the value of the reduction of every column</returns>
    public static abstract TVector Reduce<V>(in TSelf self)
        where V : IMatrixRowVisitor<V>;
}

#endregion

#region Column Visitor

/// <summary>
/// The visitor that combines the columns of the value of a matrix into a single vector
/// <para>The columns of a matrix are handed over as the vectors they are, so the member of the count of the
/// columns is the one that reaches them and the member that combines two of the vectors decides what the
/// combination of the columns of the matrix is. It is the visitor of
/// <see cref="IMatrixColumnDispatch{TSelf,TVector}"/>.</para>
/// </summary>
/// <typeparam name="V">The type of the visitor itself</typeparam>
public interface IMatrixColumnVisitor<V> where V : IMatrixColumnVisitor<V>
{
    #region Scalar

    /// <summary>Combines the value of two columns of a matrix</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="a">The first column</param>
    /// <param name="b">The second column</param>
    /// <returns>The value of the two columns combined</returns>
    public static virtual TVector Combine_Number<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    /// <summary>Combines the value of two columns of a matrix of a floating point number</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="a">The first column</param>
    /// <param name="b">The second column</param>
    /// <returns>The value of the two columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine_Float<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Combine_Number<TVector, TScalar>(a, b);

    #endregion

    #region Columns

    /// <summary>Combines the two columns of a matrix of 2 columns</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine2_Number<TVector, TScalar>(in TVector c0, in TVector c1)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine_Number<TVector, TScalar>(c0, c1);

    /// <summary>Combines the three columns of a matrix of 3 columns</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine3_Number<TVector, TScalar>(in TVector c0, in TVector c1, in TVector c2)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine_Number<TVector, TScalar>(V.Combine_Number<TVector, TScalar>(c0, c1), c2);

    /// <summary>Combines the four columns of a matrix of 4 columns</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <param name="c3">The fourth column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine4_Number<TVector, TScalar>(
        in TVector c0, in TVector c1, in TVector c2, in TVector c3
    )
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine_Number<TVector, TScalar>(
            V.Combine_Number<TVector, TScalar>(V.Combine_Number<TVector, TScalar>(c0, c1), c2), c3);

    /// <summary>Combines the two columns of a matrix of 2 columns of a floating point number</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine2_Float<TVector, TScalar>(in TVector c0, in TVector c1)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Combine2_Number<TVector, TScalar>(c0, c1);

    /// <summary>Combines the three columns of a matrix of 3 columns of a floating point number</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine3_Float<TVector, TScalar>(in TVector c0, in TVector c1, in TVector c2)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Combine3_Number<TVector, TScalar>(c0, c1, c2);

    /// <summary>Combines the four columns of a matrix of 4 columns of a floating point number</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <param name="c3">The fourth column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Combine4_Float<TVector, TScalar>(
        in TVector c0, in TVector c1, in TVector c2, in TVector c3
    )
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Combine4_Number<TVector, TScalar>(c0, c1, c2, c3);

    #endregion
}

#endregion

#region Row Visitor

/// <summary>
/// The visitor that reduces the rows of the value of a matrix to a single vector
/// <para>The rows of a matrix have no value of their own, so the columns of it are handed over as the vectors
/// they are and every one of them is reduced to a single component: the member of the count of the columns is the
/// one that reaches them and the values of the reductions are the components of the vector it builds. A matrix
/// has at least two columns, so the members of the counts floor at two of them. It is the visitor of
/// <see cref="IMatrixRowDispatch{TSelf,TVector}"/>.</para>
/// </summary>
/// <typeparam name="V">The type of the visitor itself</typeparam>
public interface IMatrixRowVisitor<V> where V : IMatrixRowVisitor<V>
{
    #region Rows

    /// <summary>Reduces the two columns of a matrix of 2 columns to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <returns>The row of the matrix</returns>
    public static virtual TRow Row2_Number<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1)
        where TColumn : unmanaged, IAlgebraDispatch<TColumn, TScalar>, INumberVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector2<TRow, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    /// <summary>Reduces the three columns of a matrix of 3 columns to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <returns>The row of the matrix</returns>
    public static virtual TRow Row3_Number<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1, in TColumn c2)
        where TColumn : unmanaged, IAlgebraDispatch<TColumn, TScalar>, INumberVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector3<TRow, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    /// <summary>Reduces the four columns of a matrix of 4 columns to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <param name="c3">The fourth column</param>
    /// <returns>The row of the matrix</returns>
    public static virtual TRow Row4_Number<TColumn, TRow, TScalar>(
        in TColumn c0, in TColumn c1, in TColumn c2, in TColumn c3
    )
        where TColumn : unmanaged, IAlgebraDispatch<TColumn, TScalar>, INumberVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector4<TRow, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    /// <summary>Reduces the two columns of a matrix of 2 columns of a floating point number to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <returns>The row of the matrix</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TRow Row2_Float<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1)
        where TColumn : unmanaged, IAlgebraDispatch<TColumn, TScalar>, IFloatingPointVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector2<TRow, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Row2_Number<TColumn, TRow, TScalar>(c0, c1);

    /// <summary>Reduces the three columns of a matrix of 3 columns of a floating point number to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <returns>The row of the matrix</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TRow Row3_Float<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1, in TColumn c2)
        where TColumn : unmanaged, IAlgebraDispatch<TColumn, TScalar>, IFloatingPointVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector3<TRow, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Row3_Number<TColumn, TRow, TScalar>(c0, c1, c2);

    /// <summary>Reduces the four columns of a matrix of 4 columns of a floating point number to the row of it</summary>
    /// <typeparam name="TColumn">The type of a column of the matrix</typeparam>
    /// <typeparam name="TRow">The type of a row of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <param name="c3">The fourth column</param>
    /// <returns>The row of the matrix</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TRow Row4_Float<TColumn, TRow, TScalar>(
        in TColumn c0, in TColumn c1, in TColumn c2, in TColumn c3
    )
        where TColumn : unmanaged, IAlgebraDispatch<TColumn, TScalar>, IFloatingPointVector<TColumn, TScalar>
        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector4<TRow, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Row4_Number<TColumn, TRow, TScalar>(c0, c1, c2, c3);

    #endregion
}

#endregion
