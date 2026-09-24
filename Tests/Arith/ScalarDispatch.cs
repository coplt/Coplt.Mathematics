using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Tests.Arith;

/// <summary>
/// The visitors that reduce the values of their arguments to a single component: the value of a vector that
/// keeps it in a register reaches the member of the visitor that matches the width of the register, the one of a
/// vector without a register reaches the member of the count of its components and the value of a matrix has no
/// reduction of it at all. The visitors are only written for a test, the members that reduce a value that has no
/// register are the ones the operation of a visitor decides.
/// </summary>
public class TestScalarDispatch
{
    /// <summary>
    /// Returns the sum of the components of the value, which is the reduction of a single component itself and
    /// the sum of the components of a vector without a register.
    /// </summary>
    private struct impl_sum : INumberAlgebraVisitor_Self_Scalar<impl_sum>
    {
        public static TScalar AcceptScalar<TScalar>(TScalar value)
            where TScalar : unmanaged, IBinaryNumber<TScalar> => value;

        public static TScalar AcceptCombine<TScalar>(TScalar a, TScalar b)
            where TScalar : unmanaged, IBinaryNumber<TScalar> => a + b;

        public static TScalar AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => vector.GetElement(0) + vector.GetElement(1);

        public static TScalar AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
        {
            // the padding lanes of the register follow the components of the vector and are left out
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += vector.GetElement(i);
            return r;
        }

        public static TScalar AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
        {
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += vector.GetElement(i);
            return r;
        }

        public static TScalar AcceptVector2<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => TVector.get_x(vector) + TVector.get_y(vector);

        public static TScalar AcceptVector3<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => TVector.get_x(vector) + TVector.get_y(vector) + TVector.get_z(vector);

        public static TScalar AcceptVector4<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => TVector.get_x(vector) + TVector.get_y(vector) + TVector.get_z(vector) + TVector.get_w(vector);
    }

    /// <summary>
    /// Returns the dot product of the two values, which is the product of the components of a single one of them
    /// and the sum of the products of the components of two vectors without a register.
    /// </summary>
    private struct impl_dot : INumberAlgebraVisitor_Self_Self_Scalar<impl_dot>
    {
        public static TScalar AcceptScalar<TScalar>(TScalar a, TScalar b)
            where TScalar : unmanaged, IBinaryNumber<TScalar> => a * b;

        public static TScalar AcceptCombine<TScalar>(TScalar a, TScalar b)
            where TScalar : unmanaged, IBinaryNumber<TScalar> => a + b;

        public static TScalar AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => a.GetElement(0) * b.GetElement(0) + a.GetElement(1) * b.GetElement(1);

        public static TScalar AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
        {
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += a.GetElement(i) * b.GetElement(i);
            return r;
        }

        public static TScalar AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
        {
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += a.GetElement(i) * b.GetElement(i);
            return r;
        }

        public static TScalar AcceptVector2<TVector, TScalar>(in TVector a, in TVector b)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b);

        public static TScalar AcceptVector3<TVector, TScalar>(in TVector a, in TVector b)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b) + TVector.get_z(a) * TVector.get_z(b);

        public static TScalar AcceptVector4<TVector, TScalar>(in TVector a, in TVector b)
            where TVector : unmanaged, INumberAlgebraDispatch<TVector>, INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b) +
               TVector.get_z(a) * TVector.get_z(b) + TVector.get_w(a) * TVector.get_w(b);
    }

    private static TScalar Sum<T, TScalar>(in T value)
        where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => T.Visit_Scalar<impl_sum>(value);

    private static TScalar Dot<T, TScalar>(in T a, in T b)
        where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => T.Visit_Scalar<impl_dot>(a, b);

    [Test]
    public void Sum()
    {
        using (Assert.EnterMultipleScope())
        {
            // a vector that fills its register, one that has padding lanes and one whose register is 64 bits wide
            Assert.That(Sum<float4, float>(new float4(1f, 2f, 3f, 4f)), Is.EqualTo(10f), "float4");
            Assert.That(Sum<float3, float>(new float3(1f, 2f, 3f)), Is.EqualTo(6f), "float3");
            Assert.That(Sum<float2, float>(new float2(1f, 2f)), Is.EqualTo(3f), "float2");
            Assert.That(Sum<double3, double>(new double3(1d, 2d, 3d)), Is.EqualTo(6d), "double3");
            Assert.That(Sum<double2, double>(new double2(1d, 2d)), Is.EqualTo(3d), "double2");
            Assert.That(Sum<int3, int>(new int3(1, 2, 3)), Is.EqualTo(6), "int3");
            Assert.That(Sum<float2s, float>(new float2s(1f, 2f)), Is.EqualTo(3f), "float2s, a 64 bit register");

            // a vector without a register reaches the member of the count of its components
            Assert.That(Sum<float3s, float>(new float3s(1f, 2f, 3f)), Is.EqualTo(6f), "float3s");
            Assert.That(Sum<double3s, double>(new double3s(1d, 2d, 3d)), Is.EqualTo(6d), "double3s");
            Assert.That(Sum<half4, Half>(new half4((Half)1f, (Half)2f, (Half)3f, (Half)4f)), Is.EqualTo((Half)10f),
                "half4, a value without a register");
        }
    }

    [Test]
    public void DotProduct()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Dot<float4, float>(new float4(1f, 2f, 3f, 4f), new float4(2f, 3f, 4f, 5f)), Is.EqualTo(40f),
                "float4");
            Assert.That(Dot<float3, float>(new float3(1f, 2f, 3f), new float3(2f, 3f, 4f)), Is.EqualTo(20f), "float3");
            Assert.That(Dot<int3, int>(new int3(1, 2, 3), new int3(2, 3, 4)), Is.EqualTo(20), "int3");
            Assert.That(Dot<double3, double>(new double3(1d, 2d, 3d), new double3(2d, 3d, 4d)), Is.EqualTo(20d),
                "double3");
            Assert.That(Dot<float3s, float>(new float3s(1f, 2f, 3f), new float3s(2f, 3f, 4f)), Is.EqualTo(20f),
                "float3s");
        }
    }

    /// <summary>
    /// The value of a matrix is the one of its columns, so the reduction of it is the one of every column of it
    /// combined with the one of the next column.
    /// </summary>
    [Test]
    public void Matrix()
    {
        var m = new float3x3(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f), new float3(7f, 8f, 9f));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(Sum<float3x3, float>(m), Is.EqualTo(45f), "the sum of every component");
            Assert.That(Dot<float3x3, float>(m, m), Is.EqualTo(285f), "the sum of the products of them");
        }
    }
}
