using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Arith;

/// <summary>
/// The visitors that reduce the values of their arguments to a single component: the value of a vector that
/// keeps it in a register reaches the member of the visitor that matches the width of the register, the one of a
/// vector without a register reaches the member of the count of its components and the value of a matrix reduces
/// every column of it and combines the values of the reductions. The visitors are only written for a test, the
/// members that reduce a value that has no register are the ones the operation of a visitor decides. The
/// members of the interfaces are implemented explicitly, so the constraints of the members are inherited from
/// the interfaces and are not repeated here.
/// </summary>
public class TestScalarDispatch
{
    /// <summary>
    /// Returns the sum of the components of the value, which is the reduction of a single component itself and
    /// the sum of the components of a vector without a register.
    /// </summary>
    private struct impl_sum : IAlgebraVisitor_T_S<impl_sum>
    {
        static TScalar IAlgebraVisitor_S_S<impl_sum>.Scalar_Number<TScalar>(TScalar value) => value;

        static TScalar IAlgebraCombinator_S_S<impl_sum>.Combine_Number<TScalar>(TScalar a, TScalar b) => a + b;

        static TScalar IAlgebraVisitor_T_S<impl_sum>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            // the padding lanes of the register follow the components of the vector and are left out
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += vector.GetElement(i);
            return r;
        }

        static TScalar IAlgebraVisitor_T_S<impl_sum>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += vector.GetElement(i);
            return r;
        }

        static TScalar IAlgebraVisitor_T_S<impl_sum>.Vector2_Number<TVector, TScalar>(in TVector vector)
            => TVector.get_x(vector) + TVector.get_y(vector);

        static TScalar IAlgebraVisitor_T_S<impl_sum>.Vector3_Number<TVector, TScalar>(in TVector vector)
            => TVector.get_x(vector) + TVector.get_y(vector) + TVector.get_z(vector);

        static TScalar IAlgebraVisitor_T_S<impl_sum>.Vector4_Number<TVector, TScalar>(in TVector vector)
            => TVector.get_x(vector) + TVector.get_y(vector) + TVector.get_z(vector) + TVector.get_w(vector);
    }

    /// <summary>
    /// Returns the dot product of the two values, which is the product of the components of a single one of them
    /// and the sum of the products of the components of two vectors without a register.
    /// </summary>
    private struct impl_dot : IAlgebraVisitor_T_T_S<impl_dot>
    {
        static TScalar IAlgebraVisitor_S_S_S<impl_dot>.Scalar_Number<TScalar>(TScalar a, TScalar b) => a * b;

        static TScalar IAlgebraCombinator_S_S<impl_dot>.Combine_Number<TScalar>(TScalar a, TScalar b) => a + b;

        static TScalar IAlgebraVisitor_T_T_S<impl_dot>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
        {
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += a.GetElement(i) * b.GetElement(i);
            return r;
        }

        static TScalar IAlgebraVisitor_T_T_S<impl_dot>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
        {
            var r = TScalar.Zero;
            for (var i = 0; i < TVector.Rows; i++) r += a.GetElement(i) * b.GetElement(i);
            return r;
        }

        static TScalar IAlgebraVisitor_T_T_S<impl_dot>.Vector2_Number<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b);

        static TScalar IAlgebraVisitor_T_T_S<impl_dot>.Vector3_Number<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b) + TVector.get_z(a) * TVector.get_z(b);

        static TScalar IAlgebraVisitor_T_T_S<impl_dot>.Vector4_Number<TVector, TScalar>(in TVector a, in TVector b)
            => TVector.get_x(a) * TVector.get_x(b) + TVector.get_y(a) * TVector.get_y(b) +
               TVector.get_z(a) * TVector.get_z(b) + TVector.get_w(a) * TVector.get_w(b);
    }

    private static TScalar Sum<T, TScalar>(in T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => T.Scalar<impl_sum>(value);

    private static TScalar Dot<T, TScalar>(in T a, in T b)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
        => T.Scalar<impl_dot>(a, b);

    [Test]
    public void Sum()
    {
        using (Assert.EnterMultipleScope())
        {
            // a vector that fills its register and one that has padding lanes
            Assert.That(Sum<float4, float>(new float4(1f, 2f, 3f, 4f)), Is.EqualTo(10f), "float4");
            Assert.That(Sum<float3, float>(new float3(1f, 2f, 3f)), Is.EqualTo(6f), "float3");
            Assert.That(Sum<float2, float>(new float2(1f, 2f)), Is.EqualTo(3f), "float2");
            Assert.That(Sum<double3, double>(new double3(1d, 2d, 3d)), Is.EqualTo(6d), "double3");
            Assert.That(Sum<double2, double>(new double2(1d, 2d)), Is.EqualTo(3d), "double2");
            Assert.That(Sum<int3, int>(new int3(1, 2, 3)), Is.EqualTo(6), "int3");

            // a vector without a register reaches the member of the count of its components
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
