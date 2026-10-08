using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the minimum of the columns of the value, which is the minimum of every column of it component
        /// by component
        /// <para>It is the minimum of every row of the value as well: the component of the result at the index of
        /// a row is the minimum of the components of that row of it, so the result is the vector of the count of
        /// the rows of the value. See <see cref="rmin{T,TVector}(T)"/> for the minimum of the rows of it, which
        /// is the minimum of every column of it and the vector of the count of the columns of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
        /// <returns>The minimum of the columns of the value, which is the vector of the count of the rows of it</returns>
        // the type of a column of the matrix is not a part of the type of the value, so the compiler cannot
        // infer it from the arguments and a call of the member names it, see the interface of the dispatch of
        // the columns of a matrix. A call that does not name it reaches the member of the vector type of the
        // value instead, which the attribute marks this member for
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector cmin<T, TVector>(T value)
            where T : unmanaged, IMatrixColumnDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Combine<impl_matrix_column_min>(value);

        /// <summary>
        /// Returns the maximum of the columns of the value, which is the maximum of every column of it component
        /// by component
        /// <para>It is the maximum of every row of the value as well: the component of the result at the index of
        /// a row is the maximum of the components of that row of it, so the result is the vector of the count of
        /// the rows of the value. See <see cref="rmax{T,TVector}(T)"/> for the maximum of the rows of it, which
        /// is the maximum of every column of it and the vector of the count of the columns of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
        /// <returns>The maximum of the columns of the value, which is the vector of the count of the rows of it</returns>
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector cmax<T, TVector>(T value)
            where T : unmanaged, IMatrixColumnDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Combine<impl_matrix_column_max>(value);

        /// <summary>
        /// Returns the minimum of the rows of the value, which is the minimum of every column of it
        /// <para>The component of the result at the index of a column of the value is the minimum of the
        /// components of that column of it, so the result is the vector of the count of the columns of the value.
        /// See <see cref="cmin{T,TVector}(T)"/> for the minimum of the columns of it, which is the minimum of
        /// every row of it and the vector of the count of the rows of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a row of the matrix</typeparam>
        /// <returns>The minimum of the rows of the value, which is the vector of the count of the columns of it</returns>
        // the type of a row of the matrix is not a part of the type of the value either, so a call of the member
        // names it as well, a call that does not name it reaches the member of the vector type of the value. A
        // row of a matrix has no value of its own, so the reduction of the rows is the one of every column of it:
        // the visitor reduces every column with the minimum and builds the row vector of them, see the minimum of
        // the components of a vector
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector rmin<T, TVector>(T value)
            where T : unmanaged, IMatrixRowDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Reduce<impl_matrix_row_min>(value);

        /// <summary>
        /// Returns the maximum of the rows of the value, which is the maximum of every column of it
        /// <para>The component of the result at the index of a column of the value is the maximum of the
        /// components of that column of it, so the result is the vector of the count of the columns of the value.
        /// See <see cref="cmax{T,TVector}(T)"/> for the maximum of the columns of it, which is the maximum of
        /// every row of it and the vector of the count of the rows of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a row of the matrix</typeparam>
        /// <returns>The maximum of the rows of the value, which is the vector of the count of the columns of it</returns>
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector rmax<T, TVector>(T value)
            where T : unmanaged, IMatrixRowDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Reduce<impl_matrix_row_max>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The minimum of the columns of a matrix, which is the minimum of every column of it component by component
    /// <para>The columns of the matrix are handed over as the vectors they are and the minimum of two of them is
    /// the minimum of their components, so the component of the result at the index of a row of the matrix is the
    /// minimum of the components of that row of it</para>
    /// </summary>
    internal struct impl_matrix_column_min : IMatrixColumnVisitor<impl_matrix_column_min>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMatrixColumnVisitor<impl_matrix_column_min>.Combine_Number<TVector, TScalar>(
            TVector a, TVector b
        ) => math.min(a, b);
    }

    /// <summary>
    /// The maximum of the columns of a matrix, which is the maximum of every column of it component by component
    /// <para>The columns of the matrix are handed over as the vectors they are and the maximum of two of them is
    /// the maximum of their components, so the component of the result at the index of a row of the matrix is the
    /// maximum of the components of that row of it</para>
    /// </summary>
    internal struct impl_matrix_column_max : IMatrixColumnVisitor<impl_matrix_column_max>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMatrixColumnVisitor<impl_matrix_column_max>.Combine_Number<TVector, TScalar>(
            TVector a, TVector b
        ) => math.max(a, b);
    }

    /// <summary>
    /// The minimum of the rows of a matrix, which is the minimum of every column of it
    /// <para>A row of a matrix has no value of its own, so every column of it is reduced by the minimum, see
    /// <see cref="math.hmin{T,TScalar}(T)"/>, and the values of the reductions are the components of the row
    /// vector, so the component of the result at the index of a column of the matrix is the minimum of the
    /// components of that column of it</para>
    /// </summary>
    internal struct impl_matrix_row_min : IMatrixRowVisitor<impl_matrix_row_min>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_min>.Row2_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1
        ) => TRow.Create(math.hmin<TColumn, TScalar>(c0), math.hmin<TColumn, TScalar>(c1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_min>.Row3_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2
        ) => TRow.Create(math.hmin<TColumn, TScalar>(c0), math.hmin<TColumn, TScalar>(c1),
            math.hmin<TColumn, TScalar>(c2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_min>.Row4_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2, TColumn c3
        ) => TRow.Create(math.hmin<TColumn, TScalar>(c0), math.hmin<TColumn, TScalar>(c1),
            math.hmin<TColumn, TScalar>(c2), math.hmin<TColumn, TScalar>(c3));
    }

    /// <summary>
    /// The maximum of the rows of a matrix, which is the maximum of every column of it
    /// <para>A row of a matrix has no value of its own, so every column of it is reduced by the maximum, see
    /// <see cref="math.hmax{T,TScalar}(T)"/>, and the values of the reductions are the components of the row
    /// vector, so the component of the result at the index of a column of the matrix is the maximum of the
    /// components of that column of it</para>
    /// </summary>
    internal struct impl_matrix_row_max : IMatrixRowVisitor<impl_matrix_row_max>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_max>.Row2_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1
        ) => TRow.Create(math.hmax<TColumn, TScalar>(c0), math.hmax<TColumn, TScalar>(c1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_max>.Row3_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2
        ) => TRow.Create(math.hmax<TColumn, TScalar>(c0), math.hmax<TColumn, TScalar>(c1),
            math.hmax<TColumn, TScalar>(c2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_max>.Row4_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2, TColumn c3
        ) => TRow.Create(math.hmax<TColumn, TScalar>(c0), math.hmax<TColumn, TScalar>(c1),
            math.hmax<TColumn, TScalar>(c2), math.hmax<TColumn, TScalar>(c3));
    }
}
