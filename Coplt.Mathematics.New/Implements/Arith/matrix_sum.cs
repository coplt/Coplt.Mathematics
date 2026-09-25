using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the sum of the columns of the value, which is the sum of every column of it component by
        /// component
        /// <para>It is the sum of every row of the value as well: the component of the result at the index of a
        /// row is the sum of the components of that row of it, so the result is the vector of the count of the
        /// rows of the value. See <see cref="rsum{T,TVector}(in T)"/> for the sum of the rows of it, which is
        /// the sum of every column of it and the vector of the count of the columns of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
        /// <returns>The sum of the columns of the value, which is the vector of the count of the rows of it</returns>
        // the type of a column of the matrix is not a part of the type of the value, so the compiler cannot
        // infer it from the arguments and a call of the member names it, see the interface of the dispatch of
        // the columns of a matrix
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector csum<T, TVector>(in T value)
            where T : unmanaged, INumberMatrixColumnDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Visit_Vector<impl_matrix_column_sum>(value);

        /// <summary>
        /// Returns the sum of the rows of the value, which is the sum of every column of it
        /// <para>The component of the result at the index of a column of the value is the sum of the components
        /// of that column of it, so the result is the vector of the count of the columns of the value. See
        /// <see cref="csum{T,TVector}(in T)"/> for the sum of the columns of it, which is the sum of every row
        /// of it and the vector of the count of the rows of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a row of the matrix</typeparam>
        /// <returns>The sum of the rows of the value, which is the vector of the count of the columns of it</returns>
        // the type of a row of the matrix is not a part of the type of the value either, so a call of the member
        // names it as well. A row of a matrix has no value of its own, so the reduction of the rows is the one of
        // every column of it: the visitor reduces every column with the sum and builds the row vector of them
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector rsum<T, TVector>(in T value)
            where T : unmanaged, INumberMatrixRowDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Visit_Vector<impl_matrix_row_sum>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The sum of the columns of a matrix, which is the sum of every column of it component by component
    /// <para>The columns of the matrix are handed over as the vectors they are and the sum of two of them is the
    /// sum of their components, so the component of the result at the index of a row of the matrix is the sum of
    /// the components of that row of it</para>
    /// </summary>
    internal struct impl_matrix_column_sum : INumberAlgebraVisitor_Self_ColumnVector<impl_matrix_column_sum>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_ColumnVector<impl_matrix_column_sum>.AcceptCombine<TVector, TScalar>(
            in TVector a, in TVector b
        ) => a + b;
    }

    /// <summary>
    /// The sum of the rows of a matrix, which is the sum of every column of it
    /// <para>A row of a matrix has no value of its own, so every column of it is reduced by the sum, see
    /// <see cref="math.sum{T,TScalar}(in T)"/>, and the values of the reductions are the components of the row
    /// vector, so the component of the result at the index of a column of the matrix is the sum of the
    /// components of that column of it</para>
    /// </summary>
    internal struct impl_matrix_row_sum : INumberAlgebraVisitor_Self_RowVector<impl_matrix_row_sum>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow INumberAlgebraVisitor_Self_RowVector<impl_matrix_row_sum>.AcceptMatrixRow2<TColumn, TRow, TScalar>(
            in TColumn c0, in TColumn c1
        ) => TRow.Create(math.sum<TColumn, TScalar>(c0), math.sum<TColumn, TScalar>(c1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow INumberAlgebraVisitor_Self_RowVector<impl_matrix_row_sum>.AcceptMatrixRow3<TColumn, TRow, TScalar>(
            in TColumn c0, in TColumn c1, in TColumn c2
        ) => TRow.Create(math.sum<TColumn, TScalar>(c0), math.sum<TColumn, TScalar>(c1), math.sum<TColumn, TScalar>(c2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow INumberAlgebraVisitor_Self_RowVector<impl_matrix_row_sum>.AcceptMatrixRow4<TColumn, TRow, TScalar>(
            in TColumn c0, in TColumn c1, in TColumn c2, in TColumn c3
        ) => TRow.Create(math.sum<TColumn, TScalar>(c0), math.sum<TColumn, TScalar>(c1), math.sum<TColumn, TScalar>(c2),
            math.sum<TColumn, TScalar>(c3));
    }
}
