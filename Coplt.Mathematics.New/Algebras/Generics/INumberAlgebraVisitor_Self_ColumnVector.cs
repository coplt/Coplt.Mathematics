namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The visitor that reduces the columns of the value to a single vector
/// <para>The columns of a matrix are handed over as the vectors they are, so the member of a count of the
/// columns is the one that reaches them and the member that combines two of the vectors they are decides what
/// the reduction of the columns is</para>
/// <para>It is the visitor of <see cref="INumberMatrixColumnDispatch{TSelf,TVector}"/>, which the new design
/// does not name yet: the value of a column is handed over as a vector and the visitor reaches it through the
/// dispatch of the value of the vector.</para>
/// </summary>
/// <typeparam name="V">The type of the visitor itself</typeparam>
public interface INumberAlgebraVisitor_Self_ColumnVector<V> where V : INumberAlgebraVisitor_Self_ColumnVector<V>
{
    /// <summary>Combines the value of two columns of a matrix</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="a">The first column</param>
    /// <param name="b">The second column</param>
    /// <returns>The value of the two columns combined</returns>
    public static abstract TVector AcceptCombine<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, Dispatch.IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    /// <summary>Combines the two columns of a matrix of 2 columns</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptMatrixColumns2<TVector, TScalar>(in TVector c0, in TVector c1)
        where TVector : unmanaged, Dispatch.IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptCombine<TVector, TScalar>(c0, c1);

    /// <summary>Combines the three columns of a matrix of 3 columns</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptMatrixColumns3<TVector, TScalar>(in TVector c0, in TVector c1, in TVector c2)
        where TVector : unmanaged, Dispatch.IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        var r = V.AcceptCombine<TVector, TScalar>(c0, c1);
        r = V.AcceptCombine<TVector, TScalar>(r, c2);
        return r;
    }

    /// <summary>Combines the four columns of a matrix of 4 columns</summary>
    /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the matrix</typeparam>
    /// <param name="c0">The first column</param>
    /// <param name="c1">The second column</param>
    /// <param name="c2">The third column</param>
    /// <param name="c3">The fourth column</param>
    /// <returns>The value of the columns combined</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptMatrixColumns4<TVector, TScalar>(
        in TVector c0, in TVector c1, in TVector c2, in TVector c3)
        where TVector : unmanaged, Dispatch.IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        var r = V.AcceptCombine<TVector, TScalar>(c0, c1);
        r = V.AcceptCombine<TVector, TScalar>(r, c2);
        r = V.AcceptCombine<TVector, TScalar>(r, c3);
        return r;
    }
}
