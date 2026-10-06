namespace Coplt.Mathematics.Algebras.Generics.Dispatch;

#region Dispatch

public interface IAlgebraDispatch<TSelf>
    where TSelf : IAlgebraDispatch<TSelf>
{
    public static abstract TSelf Self<V>(TSelf a)
        where V : IAlgebraVisitor_T_T<V>;

    /// <summary>
    /// Hands the whole value of <paramref name="self"/> over to <typeparamref name="V"/>, which returns the value
    /// it built out of it
    /// <para>The value hands itself over instead of a component of it, so the visitor decides the type of a
    /// component of the value it reaches and a caller that only knows the value itself reaches the map of it
    /// through this member. It is the dispatch of <see cref="IMapVisitor{V}"/>, which is the map of a value
    /// alone.</para>
    /// </summary>
    /// <typeparam name="V">The type of the visitor that reaches the value</typeparam>
    /// <param name="self">The value that hands itself over</param>
    /// <returns>The value the visitor built out of the value</returns>
    public static abstract TSelf Map_Self<V>(TSelf self)
        where V : IMapVisitor<V>;

    public static abstract TSelf Self<V>(TSelf a, TSelf b)
        where V : IAlgebraVisitor_T_T_T<V>;

    public static abstract TSelf Self<V>(TSelf a, TSelf b, TSelf c)
        where V : IAlgebraVisitor_T_T_T_T<V>;

    public static abstract bool Bool<V>(TSelf a)
        where V : IAlgebraVisitor_T_bool<V>;

    public static abstract bool Bool<V>(TSelf a, TSelf b)
        where V : IAlgebraVisitor_T_T_bool<V>;
}

public interface IFloatDispatch<TSelf> : IAlgebraDispatch<TSelf>
    where TSelf : IFloatDispatch<TSelf>
{
    public static abstract void Self_out<V>(TSelf a, out TSelf b, out TSelf c)
        where V : IFloatVisitor_T_outT_outT_void<V>;
}

public interface IAlgebraDispatch<TSelf, TScalar> : IAlgebraDispatch<TSelf>
    where TSelf : IAlgebraDispatch<TSelf, TScalar>
{
    public static abstract TSelf Self<V>(TSelf a, TScalar b)
        where V : IAlgebraVisitor_T_S_T<V>;

    public static abstract TSelf Self<V>(TSelf a, TScalar b, TScalar c)
        where V : IAlgebraVisitor_T_S_S_T<V>;

    public static abstract TScalar Scalar<V>(TScalar a)
        where V : IAlgebraVisitor_S_S<V>;

    public static abstract TScalar Scalar<V>(TScalar a, TScalar b)
        where V : IAlgebraVisitor_S_S_S<V>;

    public static abstract TScalar Scalar<V>(TScalar a, TScalar b, TScalar c)
        where V : IAlgebraVisitor_S_S_S_S<V>;

    public static abstract TScalar Scalar<V>(TSelf a)
        where V : IAlgebraVisitor_T_S<V>;

    public static abstract TScalar Scalar<V>(TSelf a, TSelf b)
        where V : IAlgebraVisitor_T_T_S<V>;

    public static abstract TScalar Combine<V>(TScalar a, TScalar b)
        where V : IAlgebraCombinator_S_S<V>;

    public static abstract bool Bool<V>(TScalar a)
        where V : IAlgebraVisitor_S_bool<V>;

    public static abstract bool Bool<V>(TScalar a, TScalar b)
        where V : IAlgebraVisitor_T_T_bool<V>;
}

public interface IFloatDispatch<TSelf, TScalar> : IFloatDispatch<TSelf>, IAlgebraDispatch<TSelf, TScalar>
    where TSelf : IFloatDispatch<TSelf, TScalar>
{
    public static abstract void Scalar_out<V>(TScalar a, out TScalar b, out TScalar c)
        where V : IFloatVisitor_S_outS_outS_void<V>;
}

#endregion

#region T -> T

public interface IAlgebraVisitor_S_S<V>
    where V : IAlgebraVisitor_S_S<V>
{
    #region Scalar

    public static virtual TScalar Scalar_Number<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Scalar_Float<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Scalar_Number(value);

    #endregion
}

public interface IAlgebraVisitor_T_T<V> : IAlgebraVisitor_S_S<V>
    where V : IAlgebraVisitor_T_T<V>
{
    #region Vector Simd Any

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Any<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(vector)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Any<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(vector)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(vector)));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Any<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(vector)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(vector)));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(vector)));
        TVector.set_w(ref r, TVector.Scalar<V>(TVector.get_w(vector)));
        return r;
    }

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector2_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector3_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector4_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(vector);

    #endregion

    #region Matrix Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Any<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(vector)),
            TVector.Self<V>(TMatrix.get_c1(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Any<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(vector)),
            TVector.Self<V>(TMatrix.get_c1(vector)),
            TVector.Self<V>(TMatrix.get_c2(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Any<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(vector)),
            TVector.Self<V>(TMatrix.get_c1(vector)),
            TVector.Self<V>(TMatrix.get_c2(vector)),
            TVector.Self<V>(TMatrix.get_c3(vector))
        );

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(vector);

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(vector);

    #endregion
}

#endregion

#region T -> T -> T

public interface IAlgebraVisitor_S_S_S<V>
    where V : IAlgebraVisitor_S_S_S<V>
{
    #region Scalar

    public static virtual TScalar Scalar_Number<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Scalar_Float<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Scalar_Number(a, b);

    #endregion
}

public interface IAlgebraVisitor_T_T_T<V> : IAlgebraVisitor_S_S_S<V>
    where V : IAlgebraVisitor_T_T_T<V>
{
    #region Vector Simd Any

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    #endregion

    #region Vector Soft Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Any<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Any<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b)));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), TVector.get_z(b)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Any<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b)));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), TVector.get_z(b)));
        TVector.set_w(ref r, TVector.Scalar<V>(TVector.get_w(a), TVector.get_w(b)));
        return r;
    }

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector2_Any<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector3_Any<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector4_Any<TVector, TScalar>(a, b);

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(a, b);

    #endregion

    #region Matrix Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Any<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Any<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b)),
            TVector.Self<V>(TMatrix.get_c2(a), TMatrix.get_c2(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Any<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b)),
            TVector.Self<V>(TMatrix.get_c2(a), TMatrix.get_c2(b)),
            TVector.Self<V>(TMatrix.get_c3(a), TMatrix.get_c3(b))
        );

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(a, b);

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(a, b);

    #endregion
}

#endregion

#region T -> T -> T -> T

public interface IAlgebraVisitor_S_S_S_S<V>
    where V : IAlgebraVisitor_S_S_S_S<V>
{
    #region Scalar

    public static virtual TScalar Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c)
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Scalar_Float<TScalar>(TScalar a, TScalar b, TScalar c)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Scalar_Number(a, b, c);

    #endregion
}

public interface IAlgebraVisitor_T_T_T_T<V> : IAlgebraVisitor_S_S_S_S<V>
    where V : IAlgebraVisitor_T_T_T_T<V>
{
    #region Vector Simd Any

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b, Vector128<TScalar> c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b, Vector256<TScalar> c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b, Vector128<TScalar> c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b, Vector256<TScalar> c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b, c);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b, Vector128<TScalar> c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b, Vector256<TScalar> c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b, c);

    #endregion

    #region Vector Soft Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Any<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b), TVector.get_x(c)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b), TVector.get_y(c)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Any<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b), TVector.get_x(c)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b), TVector.get_y(c)));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), TVector.get_z(b), TVector.get_z(c)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Any<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b), TVector.get_x(c)));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b), TVector.get_y(c)));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), TVector.get_z(b), TVector.get_z(c)));
        TVector.set_w(ref r, TVector.Scalar<V>(TVector.get_w(a), TVector.get_w(b), TVector.get_w(c)));
        return r;
    }

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Number<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector2_Any<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Number<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector3_Any<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Number<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector4_Any<TVector, TScalar>(a, b, c);

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Float<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Float<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Float<TVector, TScalar>(TVector a, TVector b, TVector c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(a, b, c);

    #endregion

    #region Matrix Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Any<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b), TMatrix.get_c0(c)),
            TVector.Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b), TMatrix.get_c1(c))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Any<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b), TMatrix.get_c0(c)),
            TVector.Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b), TMatrix.get_c1(c)),
            TVector.Self<V>(TMatrix.get_c2(a), TMatrix.get_c2(b), TMatrix.get_c2(c))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Any<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b), TMatrix.get_c0(c)),
            TVector.Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b), TMatrix.get_c1(c)),
            TVector.Self<V>(TMatrix.get_c2(a), TMatrix.get_c2(b), TMatrix.get_c2(c)),
            TVector.Self<V>(TMatrix.get_c3(a), TMatrix.get_c3(b), TMatrix.get_c3(c))
        );

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(a, b, c);

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b, TMatrix c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(a, b, c);

    #endregion
}

#endregion

#region T -> out T -> out T -> void

public interface IFloatVisitor_S_outS_outS_void<V>
    where V : IFloatVisitor_S_outS_outS_void<V>
{
    #region Scalar

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void Scalar_Float<TScalar>(TScalar a, out TScalar b, out TScalar c)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => throw null!;

    #endregion
}

public interface IFloatVisitor_T_outT_outT_void<V> : IFloatVisitor_S_outS_outS_void<V>
    where V : IFloatVisitor_T_outT_outT_void<V>
{
    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void Simd_Float<TVector, TScalar>(Vector128<TScalar> a, out TVector b, out TVector c)
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void Simd_Float<TVector, TScalar>(Vector256<TScalar> a, out TVector b, out TVector c)
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => throw null!;

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void Vector2_Float<TVector, TScalar>(TVector a, out TVector b, out TVector c)
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        TVector.Scalar_out<V>(TVector.get_x(a), out var bx, out var cx);
        TVector.Scalar_out<V>(TVector.get_y(a), out var by, out var cy);
        b = TVector.Create(bx, by);
        c = TVector.Create(cx, cy);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void Vector3_Float<TVector, TScalar>(TVector a, out TVector b, out TVector c)
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        TVector.Scalar_out<V>(TVector.get_x(a), out var bx, out var cx);
        TVector.Scalar_out<V>(TVector.get_y(a), out var by, out var cy);
        TVector.Scalar_out<V>(TVector.get_z(a), out var bz, out var cz);
        b = TVector.Create(bx, by, bz);
        c = TVector.Create(cx, cy, cz);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void Vector4_Float<TVector, TScalar>(TVector a, out TVector b, out TVector c)
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        TVector.Scalar_out<V>(TVector.get_x(a), out var bx, out var cx);
        TVector.Scalar_out<V>(TVector.get_y(a), out var by, out var cy);
        TVector.Scalar_out<V>(TVector.get_z(a), out var bz, out var cz);
        TVector.Scalar_out<V>(TVector.get_w(a), out var bw, out var cw);
        b = TVector.Create(bx, by, bz, bw);
        c = TVector.Create(cx, cy, cz, cw);
    }

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, out TMatrix b, out TMatrix c)
        where TMatrix : unmanaged, IFloatDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        TVector.Self_out<V>(TMatrix.get_c0(a), out var b0, out var c0);
        TVector.Self_out<V>(TMatrix.get_c1(a), out var b1, out var c1);
        b = TMatrix.Create(b0, b1);
        c = TMatrix.Create(c0, c1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, out TMatrix b, out TMatrix c)
        where TMatrix : unmanaged, IFloatDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        TVector.Self_out<V>(TMatrix.get_c0(a), out var b0, out var c0);
        TVector.Self_out<V>(TMatrix.get_c1(a), out var b1, out var c1);
        TVector.Self_out<V>(TMatrix.get_c2(a), out var b2, out var c2);
        b = TMatrix.Create(b0, b1, b2);
        c = TMatrix.Create(c0, c1, c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual void MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, out TMatrix b, out TMatrix c)
        where TMatrix : unmanaged, IFloatDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IFloatDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        TVector.Self_out<V>(TMatrix.get_c0(a), out var b0, out var c0);
        TVector.Self_out<V>(TMatrix.get_c1(a), out var b1, out var c1);
        TVector.Self_out<V>(TMatrix.get_c2(a), out var b2, out var c2);
        TVector.Self_out<V>(TMatrix.get_c3(a), out var b3, out var c3);
        b = TMatrix.Create(b0, b1, b2, b3);
        c = TMatrix.Create(c0, c1, c2, c3);
    }

    #endregion
}

#endregion

#region T -> S -> T

public interface IAlgebraVisitor_T_S_T<V> : IAlgebraVisitor_S_S_S<V>
    where V : IAlgebraVisitor_T_S_T<V>
{
    #region Vector Simd Any

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector128<TScalar> a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector256<TScalar> a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector128<TScalar> a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector256<TScalar> a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector128<TScalar> a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector256<TScalar> a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    #endregion

    #region Vector Soft Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Any<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), b));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), b));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Any<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), b));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), b));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), b));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Any<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), b));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), b));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), b));
        TVector.set_w(ref r, TVector.Scalar<V>(TVector.get_w(a), b));
        return r;
    }

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Number<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector2_Any(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Number<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector3_Any(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Number<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector4_Any(a, b);

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Float<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Float<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Float<TVector, TScalar>(TVector a, TScalar b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number(a, b);

    #endregion

    #region Matrix Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Any<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), b),
            TVector.Self<V>(TMatrix.get_c1(a), b)
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Any<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), b),
            TVector.Self<V>(TMatrix.get_c1(a), b),
            TVector.Self<V>(TMatrix.get_c2(a), b)
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Any<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), b),
            TVector.Self<V>(TMatrix.get_c1(a), b),
            TVector.Self<V>(TMatrix.get_c2(a), b),
            TVector.Self<V>(TMatrix.get_c3(a), b)
        );

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(a, b);

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, TScalar b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(a, b);

    #endregion
}

#endregion

#region T -> S -> S -> T

public interface IAlgebraVisitor_T_S_S_T<V> : IAlgebraVisitor_S_S_S_S<V>
    where V : IAlgebraVisitor_T_S_S_T<V>
{
    #region Vector Simd Any

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector128<TScalar> a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TVector Simd_Any<TVector, TScalar>(Vector256<TScalar> a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector128<TScalar> a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Number<TVector, TScalar>(Vector256<TScalar> a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b, c);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector128<TScalar> a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Simd_Float<TVector, TScalar>(Vector256<TScalar> a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b, c);

    #endregion

    #region Vector Soft Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Any<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), b, c));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), b, c));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Any<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), b, c));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), b, c));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), b, c));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Any<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged
    {
        TVector r = default;
        TVector.set_x(ref r, TVector.Scalar<V>(TVector.get_x(a), b, c));
        TVector.set_y(ref r, TVector.Scalar<V>(TVector.get_y(a), b, c));
        TVector.set_z(ref r, TVector.Scalar<V>(TVector.get_z(a), b, c));
        TVector.set_w(ref r, TVector.Scalar<V>(TVector.get_w(a), b, c));
        return r;
    }

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Number<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector2_Any(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Number<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector3_Any(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Number<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Vector4_Any(a, b, c);

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector2_Float<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector3_Float<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Vector4_Float<TVector, TScalar>(TVector a, TScalar b, TScalar c)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number(a, b, c);

    #endregion

    #region Matrix Any

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Any<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), b, c),
            TVector.Self<V>(TMatrix.get_c1(a), b, c)
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Any<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), b, c),
            TVector.Self<V>(TMatrix.get_c1(a), b, c),
            TVector.Self<V>(TMatrix.get_c2(a), b, c)
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Any<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged
        => TMatrix.Create(
            TVector.Self<V>(TMatrix.get_c0(a), b, c),
            TVector.Self<V>(TMatrix.get_c1(a), b, c),
            TVector.Self<V>(TMatrix.get_c2(a), b, c),
            TVector.Self<V>(TMatrix.get_c3(a), b, c)
        );

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx2_Any<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx3_Any<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.MatrixMx4_Any<TMatrix, TVector, TScalar>(a, b, c);

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, TScalar b, TScalar c)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(a, b, c);

    #endregion
}

#endregion

#region Combine

public interface IAlgebraCombinator_S_S<V>
    where V : IAlgebraCombinator_S_S<V>
{
    #region Scalar

    public static virtual TScalar Combine_Number<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Combine_Float<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Combine_Number(a, b);

    #endregion
}

#endregion

#region T -> S

public interface IAlgebraVisitor_T_S<V> : IAlgebraVisitor_S_S<V>, IAlgebraCombinator_S_S<V>
    where V : IAlgebraVisitor_T_S<V>
{
    #region Vector Simd Any

    public static virtual TScalar Simd_Any<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TScalar Simd_Any<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector2_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(TVector.Scalar<V>(TVector.get_x(vector)), TVector.Scalar<V>(TVector.get_y(vector)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector3_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(TVector.Scalar<V>(TVector.get_x(vector)), TVector.Scalar<V>(TVector.get_y(vector))),
            TVector.Scalar<V>(TVector.get_z(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector4_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(TVector.Scalar<V>(TVector.get_x(vector)), TVector.Scalar<V>(TVector.get_y(vector))),
            TVector.Combine<V>(TVector.Scalar<V>(TVector.get_z(vector)), TVector.Scalar<V>(TVector.get_w(vector)))
        );

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector2_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector3_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector4_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(vector);

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(TVector.Scalar<V>(TMatrix.get_c0(vector)), TVector.Scalar<V>(TMatrix.get_c1(vector)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(TVector.Scalar<V>(TMatrix.get_c0(vector)), TVector.Scalar<V>(TMatrix.get_c1(vector))),
            TVector.Scalar<V>(TMatrix.get_c2(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(TVector.Scalar<V>(TMatrix.get_c0(vector)), TVector.Scalar<V>(TMatrix.get_c1(vector))),
            TVector.Combine<V>(TVector.Scalar<V>(TMatrix.get_c2(vector)), TVector.Scalar<V>(TMatrix.get_c3(vector)))
        );

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(vector);

    #endregion
}

#endregion

#region T -> T -> S

public interface IAlgebraVisitor_T_T_S<V> : IAlgebraVisitor_S_S_S<V>, IAlgebraCombinator_S_S<V>
    where V : IAlgebraVisitor_T_T_S<V>
{
    #region Vector Simd Any

    public static virtual TScalar Simd_Any<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual TScalar Simd_Any<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Number<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Number<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Float<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Simd_Float<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector2_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b)),
            TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector3_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(
                TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b)),
                TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b))
            ),
            TVector.Scalar<V>(TVector.get_z(a), TVector.get_z(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector4_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(
                TVector.Scalar<V>(TVector.get_x(a), TVector.get_x(b)),
                TVector.Scalar<V>(TVector.get_y(a), TVector.get_y(b))
            ),
            TVector.Combine<V>(
                TVector.Scalar<V>(TVector.get_z(a), TVector.get_z(b)),
                TVector.Scalar<V>(TVector.get_w(a), TVector.get_w(b))
            )
        );

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector2_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector3_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar Vector4_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(a, b);

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Scalar<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Scalar<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(
                TVector.Scalar<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
                TVector.Scalar<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))
            ),
            TVector.Scalar<V>(TMatrix.get_c2(a), TMatrix.get_c2(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TVector.Combine<V>(
            TVector.Combine<V>(
                TVector.Scalar<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
                TVector.Scalar<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))
            ),
            TVector.Combine<V>(
                TVector.Scalar<V>(TMatrix.get_c2(a), TMatrix.get_c2(b)),
                TVector.Scalar<V>(TMatrix.get_c3(a), TMatrix.get_c3(b))
            )
        );

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TScalar MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(a, b);

    #endregion
}

#endregion

#region T -> bool

public interface IAlgebraVisitor_S_bool<V>
    where V : IAlgebraVisitor_S_bool<V>
{
    #region Bool

    public static virtual bool Scalar_Number<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryNumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Scalar_Float<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Scalar_Number(value);

    #endregion
}

public interface IAlgebraCombinator_bool_bool<V>
    where V : IAlgebraCombinator_bool_bool<V>
{
    #region Bool

    public static abstract bool Combine(bool a, bool b);

    #endregion
}

public interface IAlgebraVisitor_T_bool<V> : IAlgebraVisitor_S_bool<V>, IAlgebraCombinator_bool_bool<V>
    where V : IAlgebraVisitor_T_bool<V>
{
    #region Vector Simd Any

    public static virtual bool Simd_Any<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual bool Simd_Any<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(vector);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Float<TVector, TScalar>(Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Float<TVector, TScalar>(Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(vector);

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector2_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(TVector.Bool<V>(TVector.get_x(vector)), TVector.Bool<V>(TVector.get_y(vector)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector3_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TVector.get_x(vector)), TVector.Bool<V>(TVector.get_y(vector))),
            TVector.Bool<V>(TVector.get_z(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector4_Number<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TVector.get_x(vector)), TVector.Bool<V>(TVector.get_y(vector))),
            V.Combine(TVector.Bool<V>(TVector.get_z(vector)), TVector.Bool<V>(TVector.get_w(vector)))
        );

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector2_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector3_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector4_Float<TVector, TScalar>(TVector vector)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(vector);

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(TVector.Bool<V>(TMatrix.get_c0(vector)), TVector.Bool<V>(TMatrix.get_c1(vector)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TMatrix.get_c0(vector)), TVector.Bool<V>(TMatrix.get_c1(vector))),
            TVector.Bool<V>(TMatrix.get_c2(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TMatrix.get_c0(vector)), TVector.Bool<V>(TMatrix.get_c1(vector))),
            V.Combine(TVector.Bool<V>(TMatrix.get_c2(vector)), TVector.Bool<V>(TMatrix.get_c3(vector)))
        );

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(vector);

    #endregion
}

#endregion

#region T -> T -> bool

public interface IAlgebraVisitor_S_S_bool<V>
    where V : IAlgebraVisitor_S_S_bool<V>
{
    #region Bool

    public static virtual bool Scalar_Number<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, INumber<TScalar> => throw null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Scalar_Float<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Scalar_Number(a, b);

    #endregion
}

public interface IAlgebraVisitor_T_T_bool<V> : IAlgebraVisitor_S_S_bool<V>, IAlgebraCombinator_bool_bool<V>
    where V : IAlgebraVisitor_T_T_bool<V>
{
    #region Vector Simd Any

    public static virtual bool Simd_Any<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    public static virtual bool Simd_Any<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged => throw null!;

    #endregion

    #region Vector Simd Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Number<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Number<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Simd_Any<TVector, TScalar>(a, b);

    #endregion

    #region Vector Simd Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Float<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Simd_Float<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Simd_Number<TVector, TScalar>(a, b);

    #endregion

    #region Vector Soft Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector2_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(TVector.Bool<V>(TVector.get_x(a), TVector.get_x(b)),
            TVector.Bool<V>(TVector.get_y(a), TVector.get_y(b)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector3_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TVector.get_x(a), TVector.get_x(b)),
                TVector.Bool<V>(TVector.get_y(a), TVector.get_y(b))),
            TVector.Bool<V>(TVector.get_z(a), TVector.get_z(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector4_Number<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TVector.get_x(a), TVector.get_x(b)),
                TVector.Bool<V>(TVector.get_y(a), TVector.get_y(b))),
            V.Combine(TVector.Bool<V>(TVector.get_z(a), TVector.get_z(b)),
                TVector.Bool<V>(TVector.get_w(a), TVector.get_w(b)))
        );

    #endregion

    #region Vector Soft Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector2_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector2<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector2_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector3_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector3<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector3_Number<TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool Vector4_Float<TVector, TScalar>(TVector a, TVector b)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector4<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.Vector4_Number<TVector, TScalar>(a, b);

    #endregion

    #region Matrix Number

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx2_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(TVector.Bool<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Bool<V>(TMatrix.get_c1(a), TMatrix.get_c1(b)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx3_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
                TVector.Bool<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))),
            TVector.Bool<V>(TMatrix.get_c2(a), TMatrix.get_c2(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx4_Number<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.Combine(
            V.Combine(TVector.Bool<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
                TVector.Bool<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))),
            V.Combine(TVector.Bool<V>(TMatrix.get_c2(a), TMatrix.get_c2(b)),
                TVector.Bool<V>(TMatrix.get_c3(a), TMatrix.get_c3(b)))
        );

    #endregion

    #region Matrix Float

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx2_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx2_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx3_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx3_Number<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual bool MatrixMx4_Float<TMatrix, TVector, TScalar>(TMatrix a, TMatrix b)
        where TMatrix : unmanaged, IAlgebraDispatch<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IFloatingPointMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => V.MatrixMx4_Number<TMatrix, TVector, TScalar>(a, b);

    #endregion
}

#endregion