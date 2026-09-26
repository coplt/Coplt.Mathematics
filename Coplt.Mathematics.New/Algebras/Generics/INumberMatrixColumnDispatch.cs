namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The dispatch of the value of a matrix to the visitor that reduces its columns to a single vector
/// <para>A matrix is the only value that is made of vectors, so it is the only one that hands its value over to
/// the visitor that returns a vector: a vector hands its value over to the visitor that reduces the components
/// of it to a single component, which is the reduction of the value itself, and a scalar has no reduction of it
/// at all. The columns of a matrix are the vectors it is made of, so the visitor of the columns of it is the one
/// of this interface: it combines the columns of the matrix, see
/// <see cref="INumberMatrixRowDispatch{TSelf,TVector}"/> for the rows of it, which the visitor of the rows
/// reduces to a component every one of them. The reduction of the columns is the one of every column of it that
/// the visitor combines, so what it is comes from the visitor and not from the matrix.</para>
/// <para>The shape of a matrix is a part of its type, so the type of a column of it is a part of the interface of
/// the dispatch of it as well, which is the only type beside the matrix itself that a member of it needs: the
/// type of a single component is the one of a column of the matrix, so it is not declared by the interface and a
/// member that reduces the columns of a value to a vector does not name it either, which is what the member of
/// the visitor it reaches names.</para>
/// </summary>
/// <typeparam name="TSelf">The matrix type itself</typeparam>
/// <typeparam name="TVector">The type of a column of the matrix</typeparam>
public interface INumberMatrixColumnDispatch<TSelf, TVector> : INumberAlgebraDispatch<TSelf>
    where TSelf : unmanaged, INumberMatrixColumnDispatch<TSelf, TVector>
    where TVector : unmanaged, INumberVector<TVector>
{
    /// <summary>
    /// Hands the columns of <paramref name="self"/> over to <typeparamref name="V"/>, which reduces them to a
    /// single vector
    /// <para>The count of the columns of the matrix decides the member of the visitor that reaches them</para>
    /// </summary>
    /// <typeparam name="V">The type of the visitor that reaches the columns</typeparam>
    /// <param name="self">The matrix whose columns are handed over</param>
    /// <returns>The vector the visitor built out of the columns</returns>
    public static abstract TVector Visit_Vector<V>(in TSelf self)
        where V : INumberAlgebraVisitor_Self_ColumnVector<V>;
}
