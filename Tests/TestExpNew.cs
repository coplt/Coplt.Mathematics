using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestExpNew
{
    [Test]
    [Parallelizable]
    public void FloatTestExp()
    {
        Utils.AssertUlpRate(nameof(FloatTestExp), 1000, 0.9, 64,
            (-60f, 60f),
            x => simd_math.Exp(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Exp
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestExp_vec_2_4([Random(-60f, 60.0f, 1000)] float v)
    {
        var a = simd_math.Exp(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Exp(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void FloatTestExpErr([Values(0f, float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float v)
    {
        var a = simd_math.Exp(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = MathF.Exp(v);
        Assert.That(b, Is.EqualTo(a));
    }

    [Test]
    [Parallelizable]
    public void FloatTestExp2()
    {
        Utils.AssertUlpRate(nameof(FloatTestExp2), 1000, 0.9, 2,
            (-60f, 60f),
            x => simd_math.Exp2(new float4(x).UnsafeGetInner()).GetElement(0),
            x => MathF.Pow(2, x)
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestExp2_vec_2_4([Random(-60f, 60.0f, 1000)] float v)
    {
        var a = simd_math.Exp2(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Exp2(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void FloatTestExp2Err([Values(0f, float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float v)
    {
        var a = simd_math.Exp2(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = MathF.Pow(2, v);
        Assert.That(b, Is.EqualTo(a));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestExp()
    {
        Utils.AssertUlpRate(nameof(DoubleTestExp), 1000, 0.9, 300,
            (-600.0, 600.0),
            x => simd_math.Exp(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Exp
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestExp_vec_2_4([Random(-600.0, 600.0, 1000)] double v)
    {
        var a = simd_math.Exp(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Exp(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestExpErr([Values(0.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double v)
    {
        var a = simd_math.Exp(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = Math.Exp(v);
        Assert.That(b, Is.EqualTo(a));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestExp2()
    {
        Utils.AssertUlpRate(nameof(DoubleTestExp2), 1000, 0.9, 300,
            (-600.0, 600.0),
            x => simd_math.Exp2(new double4(x).UnsafeGetInner()).GetElement(0),
            x => Math.Pow(2, x)
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestExp2_vec_2_4([Random(-600.0, 600.0, 1000)] double v)
    {
        var a = simd_math.Exp2(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Exp2(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestExp2Err([Values(0.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double v)
    {
        var a = simd_math.Exp2(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = Math.Pow(2, v);
        Assert.That(b, Is.EqualTo(a));
    }
}

#endif
