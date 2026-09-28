namespace Coplt.Mathematics.Algebras.Generics.Dispatch;

#region Dispatch

public interface IAlgebraDispatch<TSelf>
    where TSelf : IAlgebraDispatch<TSelf>
{
    public static abstract TSelf Map_Self<V>(in TSelf self)
        where V : IAlgebraDispatch_Self_Self<V>;
}

public interface IAlgebraDispatch<TSelf, TScalar> : IAlgebraDispatch<TSelf>
    where TSelf : IAlgebraDispatch<TSelf, TScalar>
{
    public static abstract TScalar Map_Scalar<V>(TScalar scalar)
        where V : IAlgebraDispatch_Scalar_Scalar<V>;
}

#endregion

#region T -> T

public interface IAlgebraDispatch_Scalar_Scalar<V>
    where V : IAlgebraDispatch_Scalar_Scalar<V>
{
    #region Scalar

    public static virtual TScalar Scalar_Bool<TScalar>(TScalar value)
        where TScalar : unmanaged, IMask<TScalar> => throw null!;

    public static virtual TScalar Scalar_Number<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Scalar_Float<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Scalar_Number(value);

    #endregion
}

public interface IAlgebraDispatch_Self_Self<V> : IAlgebraDispatch_Scalar_Scalar<V>
    where V : IAlgebraDispatch_Self_Self<V>
{
    #region Vector Simd Any

    public static virtual TVector Simd_Any<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Bool

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Bool<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IBoolVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IMask<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Bool<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IBoolVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IMask<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Bool<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IBoolVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IMask<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Any<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Map_Scalar<V>(TVector.get_x(vector)));
        TVector.set_y(ref r, TVector.Map_Scalar<V>(TVector.get_y(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Any<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Map_Scalar<V>(TVector.get_x(vector)));
        TVector.set_y(ref r, TVector.Map_Scalar<V>(TVector.get_y(vector)));
        TVector.set_z(ref r, TVector.Map_Scalar<V>(TVector.get_z(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Any<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Map_Scalar<V>(TVector.get_x(vector)));
        TVector.set_y(ref r, TVector.Map_Scalar<V>(TVector.get_y(vector)));
        TVector.set_z(ref r, TVector.Map_Scalar<V>(TVector.get_z(vector)));
        return r;
    }

    #endregion

    #region Vector Soft Bool

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Bool<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IBoolVector<TVector, TScalar>
        where TScalar : unmanaged, IMask<TScalar>
        => V.Vector2_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Bool<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IBoolVector<TVector, TScalar>
        where TScalar : unmanaged, IMask<TScalar>
        => V.Vector3_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Bool<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IBoolVector<TVector, TScalar>
        where TScalar : unmanaged, IMask<TScalar>
        => V.Vector4_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Number<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector2_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Number<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector3_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Number<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector4_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Float<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Float<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Float<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(vector);

    #endregion

    #region Matrix Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Any<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Map_Self<V>(TMatrix.get_c0(vector)),
            TVector.Map_Self<V>(TMatrix.get_c1(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Any<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Map_Self<V>(TMatrix.get_c0(vector)),
            TVector.Map_Self<V>(TMatrix.get_c1(vector)),
            TVector.Map_Self<V>(TMatrix.get_c2(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Any<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Map_Self<V>(TMatrix.get_c0(vector)),
            TVector.Map_Self<V>(TMatrix.get_c1(vector)),
            TVector.Map_Self<V>(TMatrix.get_c2(vector)),
            TVector.Map_Self<V>(TMatrix.get_c3(vector))
        );

    #endregion

    #region Matrix Bool

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Bool<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IBoolMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IBoolVector<TVector, TScalar>
        where TScalar : unmanaged, IMask<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Bool<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IBoolMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IBoolVector<TVector, TScalar>
        where TScalar : unmanaged, IMask<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Bool<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IBoolMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IBoolVector<TVector, TScalar>
        where TScalar : unmanaged, IMask<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(vector);

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Number<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Number<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Number<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(vector);

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Float<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Float<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Float<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(vector);

    #endregion
}

#endregion
