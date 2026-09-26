using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The algebra of a floating point value: a vector and a matrix of a floating point number reach the math
/// constants of the kind of them and the ones of the standard the component type names, which are the ones of
/// every component of the value.
/// </summary>
public class TestFloatingPointAlgebra
{
    /// <summary>
    /// The constants of a value that names the ieee 754 standard are reachable through the interface of it,
    /// which a type parameter that only knows the interface uses.
    /// </summary>
    private static void CheckIeee754Vector<T, TScalar>(T v)
        where T : unmanaged, IFloatingPointIeee754Vector<T, TScalar>, IFloatingPointIeee754AlgebraDispatch<T>
        where TScalar : unmanaged, IBinaryNumber<TScalar>, IFloatingPointIeee754<TScalar>
    {
        _ = T.E / T.PI;
        _ = T.Log2 + T.Log10 + T.Tau + T.RadToDeg + T.DegToRad;
        _ = T.Epsilon * T.NaN * T.NegativeInfinity * T.NegativeZero * T.PositiveInfinity;
        _ = T.ScalarE / T.ScalarPI;
        _ = T.ScalarLog2 + T.ScalarLog10 + T.ScalarTau + T.ScalarRadToDeg + T.ScalarDegToRad;
        _ = T.ScalarEpsilon * T.ScalarNaN * T.ScalarNegativeInfinity * T.ScalarNegativeZero *
            T.ScalarPositiveInfinity;
        _ = v + T.PI;
        _ = T.Broadcast(T.ScalarPI);
    }

    /// <inheritdoc cref="CheckIeee754Vector{T,TScalar}(T)"/>
    private static void CheckIeee754Matrix<T, TScalar>(T m)
        where T : unmanaged, IFloatingPointIeee754Matrix<T, TScalar>, IFloatingPointIeee754AlgebraDispatch<T>
        where TScalar : unmanaged, IBinaryNumber<TScalar>, IFloatingPointIeee754<TScalar>
    {
        _ = T.E / T.PI;
        _ = T.Log2 + T.Log10 + T.Tau + T.RadToDeg + T.DegToRad;
        _ = T.Epsilon * T.NaN * T.NegativeInfinity * T.NegativeZero * T.PositiveInfinity;
        _ = T.ScalarE / T.ScalarPI;
        _ = T.ScalarLog2 + T.ScalarLog10 + T.ScalarTau + T.ScalarRadToDeg + T.ScalarDegToRad;
        _ = T.ScalarEpsilon * T.ScalarNaN * T.ScalarNegativeInfinity * T.ScalarNegativeZero *
            T.ScalarPositiveInfinity;
        _ = m + T.PI;
        _ = T.Broadcast(T.ScalarPI);
    }

    /// <summary>
    /// The constants of a half are the ones of a floating point number that does not name the standard itself,
    /// so it reaches the ones of the kind of it alone.
    /// </summary>
    private static void CheckFloatingPointVector<T, TScalar>(T v)
        where T : unmanaged, IFloatingPointVector<T, TScalar>, IFloatingPointAlgebraDispatch<T>
        where TScalar : unmanaged, IBinaryNumber<TScalar>, IFloatingPoint<TScalar>
    {
        _ = T.E / T.PI;
        _ = T.Log2 + T.Log10 + T.Tau + T.RadToDeg + T.DegToRad;
        _ = T.ScalarE / T.ScalarPI;
        _ = T.ScalarLog2 + T.ScalarLog10 + T.ScalarTau + T.ScalarRadToDeg + T.ScalarDegToRad;
        _ = v + T.PI;
        _ = T.Broadcast(T.ScalarE);
    }

    /// <inheritdoc cref="CheckFloatingPointVector{T,TScalar}(T)"/>
    private static void CheckFloatingPointMatrix<T, TScalar>(T m)
        where T : unmanaged, IFloatingPointMatrix<T, TScalar>, IFloatingPointAlgebraDispatch<T>
        where TScalar : unmanaged, IBinaryNumber<TScalar>, IFloatingPoint<TScalar>
    {
        _ = T.E / T.PI;
        _ = T.Log2 + T.Log10 + T.Tau + T.RadToDeg + T.DegToRad;
        _ = T.ScalarE / T.ScalarPI;
        _ = T.ScalarLog2 + T.ScalarLog10 + T.ScalarTau + T.ScalarRadToDeg + T.ScalarDegToRad;
        _ = m + T.PI;
        _ = T.Broadcast(T.ScalarE);
    }

    [Test]
    public void Interfaces()
    {
        CheckIeee754Vector<float3, float>(new float3(1f, 2f, 3f));
        CheckIeee754Vector<float3s, float>(new float3s(1f, 2f, 3f));
        CheckIeee754Vector<double4, double>(new double4(1d, 2d, 3d, 4d));
        CheckIeee754Matrix<float3x2, float>(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)));
        CheckIeee754Matrix<float2x2s, float>(new float2x2s(new float2s(1f, 2f), new float2s(3f, 4f)));
        CheckIeee754Matrix<double2x2, double>(new double2x2(new double2(1d, 2d), new double2(3d, 4d)));
        CheckFloatingPointVector<half3, Half>(new half3((half)1f, (half)2f, (half)3f));
        CheckFloatingPointVector<half4, Half>(new half4((half)1f, (half)2f, (half)3f, (half)4f));
        CheckFloatingPointMatrix<half2x2, Half>(new half2x2(new half2((half)1f, (half)2f),
            new half2((half)3f, (half)4f)));
        CheckFloatingPointMatrix<half3x3, Half>(new half3x3(new half3((half)1f, (half)2f, (half)3f),
            new half3((half)4f, (half)5f, (half)6f), new half3((half)7f, (half)8f, (half)9f)));
    }

    /// <summary>
    /// The constants of a value are the ones of every component of it.
    /// </summary>
    [Test]
    public void Constants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.PI, Is.EqualTo(new float3(MathF.PI)), "the pi of a vector");
            Assert.That(float3.E, Is.EqualTo(new float3(MathF.E)), "the e of a vector");
            Assert.That(float3.Tau, Is.EqualTo(new float3(MathF.Tau)), "the tau of a vector");
            Assert.That(float4.RadToDeg, Is.EqualTo(new float4((float)(180d / Math.PI))), "the degrees of a vector");
            Assert.That(double2.DegToRad, Is.EqualTo(new double2(Math.PI / 180d)), "the radians of a vector");
            Assert.That(half3.PI, Is.EqualTo(new half3((half)MathF.PI)), "the pi of a vector of halves");
            Assert.That(float3.Epsilon, Is.EqualTo(new float3(float.Epsilon)), "the epsilon of a vector");
            Assert.That(double2.NegativeInfinity, Is.EqualTo(new double2(double.NegativeInfinity)),
                "the negative infinity of a vector");
            Assert.That(float.IsNaN(float3.NaN.x), Is.True, "the nan of a vector");

            Assert.That(float3x2.PI, Is.EqualTo(new float3x2(new float3(MathF.PI), new float3(MathF.PI))),
                "the pi of a matrix");
            Assert.That(half2x2.E, Is.EqualTo(new half2x2(new half2((half)MathF.E), new half2((half)MathF.E))),
                "the e of a matrix of halves");
            Assert.That(double2x2.Epsilon, Is.EqualTo(new double2x2(new double2(double.Epsilon),
                new double2(double.Epsilon))), "the epsilon of a matrix");
            Assert.That(double.IsNaN(float2x4.NaN.c0.x), Is.True, "the nan of a matrix");

            Assert.That(float3.ScalarPI, Is.EqualTo(MathF.PI), "the pi of a component");
            Assert.That(float3.ScalarLog2, Is.EqualTo(0.6931471805599453f), "the log2 of a component");
            Assert.That(half3.ScalarPI, Is.EqualTo((half)MathF.PI), "the pi of a component of halves");
            Assert.That(float3x2.ScalarTau, Is.EqualTo(MathF.Tau), "the tau of a component of a matrix");
            Assert.That(float3.ScalarEpsilon, Is.EqualTo(float.Epsilon), "the epsilon of a component");
            Assert.That(float.IsNaN(float3.ScalarNaN), Is.True, "the nan of a component");
            Assert.That(double2x2.ScalarNegativeInfinity, Is.EqualTo(double.NegativeInfinity),
                "the negative infinity of a component of a matrix");
        }
    }
}
