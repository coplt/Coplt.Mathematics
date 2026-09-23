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
    public static virtual TMatrix AcceptMatrix2x2<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x2Vector<TMatrix, TVector>, IMatrix2x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(TVector.Visit_Self<V>(TMatrix.get_c0(vector)), TVector.Visit_Self<V>(TMatrix.get_c1(vector)));
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
    public static virtual TMatrix AcceptMatrix2x2<TMatrix, TVector, TScalar>(in TMatrix a, in TMatrix b)
        where TMatrix : unmanaged, INumberAlgebraDispatch<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x2Vector<TMatrix, TVector>, IMatrix2x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => TMatrix.Create(
            TVector.Visit_Self<V>(TMatrix.get_c0(a), TMatrix.get_c0(b)),
            TVector.Visit_Self<V>(TMatrix.get_c1(a), TMatrix.get_c1(b))
        );
}

#endregion
