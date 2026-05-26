using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestAsinhAcoshNew
{
    [Test]
    [Parallelizable]
    public void FloatTestAsinh()
    {
        Utils.AssertUlpRate(nameof(FloatTestAsinh), 1000, 0.9, 100,
            (-10f, 10f),
            x => simd_math.Asinh(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Asinh
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestAsinh_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Asinh(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Asinh(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestAsinh()
    {
        Utils.AssertUlpRate(nameof(DoubleTestAsinh), 1000, 0.9, 700,
            (-10.0, 10.0),
            x => simd_math.Asinh(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Asinh
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestAsinh_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Asinh(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Asinh(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void FloatTestAcosh()
    {
        Utils.AssertUlpRate(nameof(FloatTestAcosh), 1000, 0.9, 2,
            (1f, 10f),
            x => simd_math.Acosh(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Acosh
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestAcosh_vec_2_4([Random(1f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Acosh(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Acosh(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestAcosh()
    {
        Utils.AssertUlpRate(nameof(DoubleTestAcosh), 1000, 0.9, 16,
            (1.0, 10.0),
            x => simd_math.Acosh(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Acosh
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestAcosh_vec_2_4([Random(1.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Acosh(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Acosh(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void FloatTestAtanh()
    {
        Utils.AssertUlpRate(nameof(FloatTestAtanh), 1000, 0.9, 4,
            (-1f, 1f),
            x => simd_math.Atanh(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Atanh
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestAtanh_vec_2_4([Random(-1f, 1.0f, 1000)] float v)
    {
        var a = simd_math.Atanh(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Atanh(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestAtanh()
    {
        Utils.AssertUlpRate(nameof(DoubleTestAtanh), 1000, 0.9, 4,
            (-1.0, 1.0),
            x => simd_math.Atanh(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Atanh
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestAtanh_vec_2_4([Random(-1.0, 1.0, 1000)] double v)
    {
        var a = simd_math.Atanh(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Atanh(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }
}

#endif
