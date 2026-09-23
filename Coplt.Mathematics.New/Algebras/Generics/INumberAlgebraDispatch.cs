namespace Coplt.Mathematics.Algebras.Generics;

#region Dispatch

public interface IAlgebraDispatch_Number<TSelf>
    where TSelf : unmanaged, IAlgebraDispatch_Number<TSelf>
{
    public static abstract TSelf Visit_Self<V>(in TSelf self)
        where V : INumberAlgebraVisitor_Self_Self;


    public static abstract TSelf Visit_Self<V>(in TSelf a, in TSelf b)
        where V : IAlgebraVisitor_Self_Self_Self;


    public static abstract TSelf Visit_Vector_Self<V>(in TSelf self)
        where V : IVectorVisitor_Self;


    public static abstract TSelf Visit_Vector_Self<V>(in TSelf a, in TSelf b)
        where V : IVectorVisitor_Self_Self_Self;
}

#endregion

#region Self -> Self

public interface INumberAlgebraVisitor_Self_Self
{
    public static virtual TMatrix AcceptMatrix<TMatrix, TVector, TScalar>(in TMatrix matrix)
        where TMatrix : unmanaged, IAlgebraDispatch_Number<TMatrix>, INumberMatrix<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();

    public static virtual TVector AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();

    public static virtual TVector AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();

    public static virtual TVector AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();

    public static virtual TVector AcceptVector<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVectorSoftUnderlying
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();
}

public interface IVectorVisitor_Self
{
    public static virtual TVector AcceptVector2<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();


    public static virtual TVector AcceptVector3<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();


    public static virtual TVector AcceptVector4<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();
}

public interface IMatrixVisitor_Self
{
    public static virtual TMatrix AcceptMatrix2x2<TMatrix, TVector, TScalar>(in TMatrix vector)
        where TMatrix : unmanaged, IAlgebraDispatch_Number<TMatrix>, INumberMatrix<TMatrix, TScalar>, IMatrix2x2Vector<TMatrix, TVector>, IMatrix2x2Scalar<TMatrix, TScalar>
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => throw new NotSupportedException();
}

#endregion

#region Self, Self -> Self

public interface IAlgebraVisitor_Self_Self_Self
{
    public static abstract TVector AcceptSoft<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVectorSoftUnderlying
        where TScalar : unmanaged, IBinaryNumber<TScalar>;


    public static abstract TVector Accept<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;


    public static abstract TVector Accept<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;


    public static abstract TVector Accept<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;
}

public interface IVectorVisitor_Self_Self_Self
{
    public static abstract TVector AcceptVector2<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;


    public static abstract TVector AcceptVector3<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;

    public static abstract TVector AcceptVector4<TVector, TScalar>(in TVector a, in TVector b)
        where TVector : unmanaged, IAlgebraDispatch_Number<TVector>,
        INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>;
}

#endregion
