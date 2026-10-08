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
        /// <para>It is the minimum the platform computes itself, which is the one of
        /// <see cref="cmin{T,TVector}(T)"/> beside the way it handles a nan and a negative zero: every platform
        /// is free to handle the two of them in a way of its own</para>
        /// <para>It is the minimum of every row of the value as well: the component of the result at the index of
        /// a row is the minimum of the components of that row of it, so the result is the vector of the count of
        /// the rows of the value. See <see cref="rmin_native{T,TVector}(T)"/> for the minimum of the rows of it,
        /// which is the minimum of every column of it and the vector of the count of the columns of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
        /// <returns>The minimum of the columns of the value, which is the vector of the count of the rows of it</returns>
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector cmin_native<T, TVector>(T value)
            where T : unmanaged, IMatrixColumnDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Combine<impl_matrix_column_min_native>(value);

        /// <summary>
        /// Returns the maximum of the columns of the value, which is the maximum of every column of it component
        /// by component
        /// <para>It is the maximum the platform computes itself, which is the one of
        /// <see cref="cmax{T,TVector}(T)"/> beside the way it handles a nan and a negative zero: every platform
        /// is free to handle the two of them in a way of its own</para>
        /// <para>It is the maximum of every row of the value as well: the component of the result at the index of
        /// a row is the maximum of the components of that row of it, so the result is the vector of the count of
        /// the rows of the value. See <see cref="rmax_native{T,TVector}(T)"/> for the maximum of the rows of it,
        /// which is the maximum of every column of it and the vector of the count of the columns of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a column of the matrix</typeparam>
        /// <returns>The maximum of the columns of the value, which is the vector of the count of the rows of it</returns>
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector cmax_native<T, TVector>(T value)
            where T : unmanaged, IMatrixColumnDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Combine<impl_matrix_column_max_native>(value);

        /// <summary>
        /// Returns the minimum of the rows of the value, which is the minimum of every column of it
        /// <para>It is the minimum the platform computes itself, which is the one of
        /// <see cref="rmin{T,TVector}(T)"/> beside the way it handles a nan and a negative zero: every platform
        /// is free to handle the two of them in a way of its own</para>
        /// <para>The component of the result at the index of a column of the value is the minimum of the
        /// components of that column of it, so the result is the vector of the count of the columns of the value.
        /// See <see cref="cmin_native{T,TVector}(T)"/> for the minimum of the columns of it, which is the minimum
        /// of every row of it and the vector of the count of the rows of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a row of the matrix</typeparam>
        /// <returns>The minimum of the rows of the value, which is the vector of the count of the columns of it</returns>
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector rmin_native<T, TVector>(T value)
            where T : unmanaged, IMatrixRowDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Reduce<impl_matrix_row_min_native>(value);

        /// <summary>
        /// Returns the maximum of the rows of the value, which is the maximum of every column of it
        /// <para>It is the maximum the platform computes itself, which is the one of
        /// <see cref="rmax{T,TVector}(T)"/> beside the way it handles a nan and a negative zero: every platform
        /// is free to handle the two of them in a way of its own</para>
        /// <para>The component of the result at the index of a column of the value is the maximum of the
        /// components of that column of it, so the result is the vector of the count of the columns of the value.
        /// See <see cref="cmax_native{T,TVector}(T)"/> for the maximum of the columns of it, which is the maximum
        /// of every row of it and the vector of the count of the rows of it</para>
        /// </summary>
        /// <param name="value">The value, a matrix</param>
        /// <typeparam name="T">The type of the value, a matrix</typeparam>
        /// <typeparam name="TVector">The type of a row of the matrix</typeparam>
        /// <returns>The maximum of the rows of the value, which is the vector of the count of the columns of it</returns>
        [VectorExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TVector rmax_native<T, TVector>(T value)
            where T : unmanaged, IMatrixRowDispatch<T, TVector>
            where TVector : unmanaged, INumberVector<TVector>
            => T.Reduce<impl_matrix_row_max_native>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The minimum of the columns of a matrix, the one the platform computes itself
    /// <para>The columns of the matrix are handed over as the vectors they are and the minimum of two of them is
    /// the minimum of their components, which every platform is free to reach the way it holds, so the component
    /// of the result at the index of a row of the matrix is the minimum of the components of that row of it</para>
    /// </summary>
    internal struct impl_matrix_column_min_native : IMatrixColumnVisitor<impl_matrix_column_min_native>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMatrixColumnVisitor<impl_matrix_column_min_native>.Combine_Number<TVector, TScalar>(
            TVector a, TVector b
        ) => math.min_native(a, b);
    }

    /// <summary>
    /// The maximum of the columns of a matrix, the one the platform computes itself
    /// <para>The columns of the matrix are handed over as the vectors they are and the maximum of two of them is
    /// the maximum of their components, which every platform is free to reach the way it holds, so the component
    /// of the result at the index of a row of the matrix is the maximum of the components of that row of it</para>
    /// </summary>
    internal struct impl_matrix_column_max_native : IMatrixColumnVisitor<impl_matrix_column_max_native>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMatrixColumnVisitor<impl_matrix_column_max_native>.Combine_Number<TVector, TScalar>(
            TVector a, TVector b
        ) => math.max_native(a, b);
    }

    /// <summary>
    /// The minimum of the rows of a matrix, the one the platform computes itself
    /// <para>A row of a matrix has no value of its own, so every column of it is reduced by the minimum of the
    /// platform, see <see cref="math.hmin_native{T,TScalar}(T)"/>, and the values of the reductions are the
    /// components of the row vector, so the component of the result at the index of a column of the matrix is the
    /// minimum of the components of that column of it</para>
    /// </summary>
    internal struct impl_matrix_row_min_native : IMatrixRowVisitor<impl_matrix_row_min_native>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_min_native>.Row2_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1
        ) => TRow.Create(math.hmin_native<TColumn, TScalar>(c0), math.hmin_native<TColumn, TScalar>(c1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_min_native>.Row3_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2
        ) => TRow.Create(math.hmin_native<TColumn, TScalar>(c0), math.hmin_native<TColumn, TScalar>(c1),
            math.hmin_native<TColumn, TScalar>(c2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_min_native>.Row4_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2, TColumn c3
        ) => TRow.Create(math.hmin_native<TColumn, TScalar>(c0), math.hmin_native<TColumn, TScalar>(c1),
            math.hmin_native<TColumn, TScalar>(c2), math.hmin_native<TColumn, TScalar>(c3));
    }

    /// <summary>
    /// The maximum of the rows of a matrix, the one the platform computes itself
    /// <para>A row of a matrix has no value of its own, so every column of it is reduced by the maximum of the
    /// platform, see <see cref="math.hmax_native{T,TScalar}(T)"/>, and the values of the reductions are the
    /// components of the row vector, so the component of the result at the index of a column of the matrix is the
    /// maximum of the components of that column of it</para>
    /// </summary>
    internal struct impl_matrix_row_max_native : IMatrixRowVisitor<impl_matrix_row_max_native>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_max_native>.Row2_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1
        ) => TRow.Create(math.hmax_native<TColumn, TScalar>(c0), math.hmax_native<TColumn, TScalar>(c1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_max_native>.Row3_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2
        ) => TRow.Create(math.hmax_native<TColumn, TScalar>(c0), math.hmax_native<TColumn, TScalar>(c1),
            math.hmax_native<TColumn, TScalar>(c2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TRow IMatrixRowVisitor<impl_matrix_row_max_native>.Row4_Number<TColumn, TRow, TScalar>(
            TColumn c0, TColumn c1, TColumn c2, TColumn c3
        ) => TRow.Create(math.hmax_native<TColumn, TScalar>(c0), math.hmax_native<TColumn, TScalar>(c1),
            math.hmax_native<TColumn, TScalar>(c2), math.hmax_native<TColumn, TScalar>(c3));
    }
}
