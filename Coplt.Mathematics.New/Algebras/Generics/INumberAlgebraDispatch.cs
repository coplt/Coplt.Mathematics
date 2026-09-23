namespace Coplt.Mathematics.Algebras.Generics;

#region Dispatch

public interface INumberAlgebraDispatch<TSelf>
    where TSelf : unmanaged, INumberAlgebraDispatch<TSelf>
{
    public static abstract TSelf Visit_Self<V>(in TSelf self)
        where V : INumberAlgebraVisitor_Self_Self<V>;

    public static abstract TSelf Visit_Self<V>(in TSelf a, in TSelf b)
        where V : INumberAlgebraVisitor_Self_Self_Self<V>;
}

#endregion

#region Self -> Self

public interface INumberAlgebraVisitor_Self_Self<V> where V : INumberAlgebraVisitor_Self_Self<V>
{
    public static abstract TScalar AcceptScalar<TScalar>(TScalar value)
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptVector2<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        TVector r = default;
        TVector.set_x(ref r, V.AcceptScalar(TVector.get_x(vector)));
        TVector.set_y(ref r, V.AcceptScalar(TVector.get_y(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptVector3<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        TVector r = default;
        TVector.set_x(ref r, V.AcceptScalar(TVector.get_x(vector)));
        TVector.set_y(ref r, V.AcceptScalar(TVector.get_y(vector)));
        TVector.set_z(ref r, V.AcceptScalar(TVector.get_z(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptVector4<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        TVector r = default;
        TVector.set_x(ref r, V.AcceptScalar(TVector.get_x(vector)));
        TVector.set_y(ref r, V.AcceptScalar(TVector.get_y(vector)));
        TVector.set_z(ref r, V.AcceptScalar(TVector.get_z(vector)));
        TVector.set_w(ref r, V.AcceptScalar(TVector.get_w(vector)));
        return r;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrixMx2<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(TVector.Visit_Self<V>(TMatrix.get_c0(vector)), TVector.Visit_Self<V>(TMatrix.get_c1(vector)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrixMx3<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(
            TVector.Visit_Self<V>(TMatrix.get_c0(vector)),
            TVector.Visit_Self<V>(TMatrix.get_c1(vector)),
            TVector.Visit_Self<V>(TMatrix.get_c2(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrixMx4<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(
            TVector.Visit_Self<V>(TMatrix.get_c0(vector)),
            TVector.Visit_Self<V>(TMatrix.get_c1(vector)),
            TVector.Visit_Self<V>(TMatrix.get_c2(vector)),
            TVector.Visit_Self<V>(TMatrix.get_c3(vector))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix2x2<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x2Vector<TMatrix, TVector>, IMatrix2x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx2<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix2x3<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x3Vector<TMatrix, TVector>, IMatrix2x3Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx3<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix2x4<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x4Vector<TMatrix, TVector>, IMatrix2x4Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx4<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix3x2<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix3x2Vector<TMatrix, TVector>, IMatrix3x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx2<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix3x3<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix3x3Vector<TMatrix, TVector>, IMatrix3x3Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx3<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix3x4<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix3x4Vector<TMatrix, TVector>, IMatrix3x4Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx4<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix4x2<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix4x2Vector<TMatrix, TVector>, IMatrix4x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx2<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix4x3<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix4x3Vector<TMatrix, TVector>, IMatrix4x3Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx3<TMatrix, TVector, TScalar>(vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix4x4<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix4x4Vector<TMatrix, TVector>, IMatrix4x4Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx4<TMatrix, TVector, TScalar>(vector);
}

#endregion

#region Self, Self -> Self

public interface INumberAlgebraVisitor_Self_Self_Self<V> where V : INumberAlgebraVisitor_Self_Self_Self<V>
{
    public static abstract TScalar AcceptScalar<TScalar>(TScalar a, TScalar b)
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptVector2<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => TVector.Create(
        V.AcceptScalar(TVector.get_x(a), TVector.get_x(b)),
        V.AcceptScalar(TVector.get_y(a), TVector.get_y(b))
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptVector3<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => TVector.Create(
        V.AcceptScalar(TVector.get_x(a), TVector.get_x(b)),
        V.AcceptScalar(TVector.get_y(a), TVector.get_y(b)),
        V.AcceptScalar(TVector.get_z(a), TVector.get_z(b))
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector AcceptVector4<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => TVector.Create(
        V.AcceptScalar(TVector.get_x(a), TVector.get_x(b)),
        V.AcceptScalar(TVector.get_y(a), TVector.get_y(b)),
        V.AcceptScalar(TVector.get_z(a), TVector.get_z(b)),
        V.AcceptScalar(TVector.get_w(a), TVector.get_w(b))
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrixMx2<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrixMx2Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(
            TVector.Visit_Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Visit_Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrixMx3<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrixMx3Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(
            TVector.Visit_Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Visit_Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b)),
            TVector.Visit_Self<V>(TMatrix.get_c2(a), TMatrix.get_c2(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrixMx4<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrixMx4Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(
            TVector.Visit_Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Visit_Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b)),
            TVector.Visit_Self<V>(TMatrix.get_c2(a), TMatrix.get_c2(b)),
            TVector.Visit_Self<V>(TMatrix.get_c3(a), TMatrix.get_c3(b))
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix2x2<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x2Vector<TMatrix, TVector>, IMatrix2x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx2<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix2x3<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x3Vector<TMatrix, TVector>, IMatrix2x3Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx3<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix2x4<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x4Vector<TMatrix, TVector>, IMatrix2x4Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx4<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix3x2<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix3x2Vector<TMatrix, TVector>, IMatrix3x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx2<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix3x3<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix3x3Vector<TMatrix, TVector>, IMatrix3x3Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx3<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix3x4<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix3x4Vector<TMatrix, TVector>, IMatrix3x4Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx4<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix4x2<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix4x2Vector<TMatrix, TVector>, IMatrix4x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx2<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix4x3<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix4x3Vector<TMatrix, TVector>, IMatrix4x3Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx3<TMatrix, TVector, TScalar>(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TMatrix AcceptMatrix4x4<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix4x4Vector<TMatrix, TVector>, IMatrix4x4Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => V.AcceptMatrixMx4<TMatrix, TVector, TScalar>(a, b);
}

#endregion
