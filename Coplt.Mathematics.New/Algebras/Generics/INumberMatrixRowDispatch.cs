namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The dispatch of the value of a matrix to the visitor that reduces its rows to a single vector
/// <para>See <see cref="INumberMatrixColumnDispatch{TSelf,TVector}"/> for the columns of the matrix, which is the
/// interface of the same form: the value of a matrix is the vectors it is made of and the rows of it are the
/// other axis of them, so the row of a matrix is the vector of the count of the columns of it and the sum of the
/// rows of it is the sum of every column of it. The row of a matrix of a shape is not a part of the type of the
/// matrix itself, so the interface of the dispatch of the rows names the type of a row of it, which a matrix of
/// the same count of the rows and the columns shares with the matrix of the transposed shape.</para>
/// <para>The row of a matrix has no value of its own that the matrix could hand over, so the reduction of the
/// rows is the one of every column of it that the visitor reduces to a single component: the component of the
/// result at the index of a column of the matrix is the value of the reduction of that column of it, which is
/// the member that reduces the value of a vector to a component.</para>
/// </summary>
/// <typeparam name="TSelf">The matrix type itself</typeparam>
/// <typeparam name="TVector">The type of a row of the matrix</typeparam>
public interface INumberMatrixRowDispatch<TSelf, TVector> : INumberAlgebraDispatch<TSelf>
    where TSelf : unmanaged, INumberMatrixRowDispatch<TSelf, TVector>
    where TVector : unmanaged, INumberVector<TVector>
{
    /// <summary>
    /// Hands the columns of <paramref name="self"/> over to <typeparamref name="V"/>, which reduces every one of
    /// them to a single component and builds the vector of the values of the reductions
    /// <para>The count of the columns of the matrix decides the member of the visitor that reaches them</para>
    /// </summary>
    /// <typeparam name="V">The type of the visitor that reaches the columns</typeparam>
    /// <param name="self">The matrix whose columns are handed over</param>
    /// <returns>The vector of the value of the reduction of every column</returns>
    public static abstract TVector Visit_Vector<V>(in TSelf self)
        where V : INumberAlgebraVisitor_Self_RowVector<V>;
}
