using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestSinCosNew
{
    [Test]
    [Parallelizable]
    public void FloatTestSin()
    {
        Utils.AssertUlpRate(nameof(FloatTestSin), 1000, 0.9, 500,
            (-10f, 10f),
            x => simd_math.Sin(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Sin
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestSin_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Sin(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Sin(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestSin()
    {
        Utils.AssertUlpRate(nameof(DoubleTestSin), 1000, 0.9, 200,
            (-10.0, 10.0),
            x => simd_math.Sin(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Sin
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestSin_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Sin(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Sin(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void FloatTestCos()
    {
        Utils.AssertUlpRate(nameof(FloatTestCos), 1000, 0.9, 700,
            (-10f, 10f),
            x => simd_math.Cos(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Cos
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestCos_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Cos(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Cos(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestCos()
    {
        Utils.AssertUlpRate(nameof(DoubleTestCos), 1000, 0.9, 1500,
            (-10.0, 10.0),
            x => simd_math.Cos(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Cos
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestCos_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Cos(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Cos(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }
}

#endif
