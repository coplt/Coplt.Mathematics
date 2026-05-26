using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestSinhCoshNew
{
    [Test]
    [Parallelizable]
    public void FloatTestSinh()
    {
        Utils.AssertUlpRate(nameof(FloatTestSinh), 1000, 0.9, 64,
            (-10f, 10f),
            x => simd_math.Sinh(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Sinh
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestSinh_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Sinh(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Sinh(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestSinh()
    {
        Utils.AssertUlpRate(nameof(DoubleTestSinh), 1000, 0.9, 256,
            (-10.0, 10.0),
            x => simd_math.Sinh(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Sinh
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestSinh_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Sinh(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Sinh(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void FloatTestCosh()
    {
        Utils.AssertUlpRate(nameof(FloatTestCosh), 1000, 0.9, 16,
            (-10f, 10f),
            x => simd_math.Cosh(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Cosh
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestCosh_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Cosh(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Cosh(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestCosh()
    {
        Utils.AssertUlpRate(nameof(DoubleTestCosh), 1000, 0.9, 16,
            (-10.0, 10.0),
            x => simd_math.Cosh(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Cosh
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestCosh_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Cosh(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Cosh(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }
}

#endif
