using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestTanhNew
{
    [Test]
    [Parallelizable]
    public void FloatTestTanh()
    {
        Utils.AssertUlpRate(nameof(FloatTestTanh), 1000, 0.9, 4,
            (-10f, 10f),
            x => simd_math.Tanh(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Tanh
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestTanh_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Tanh(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Tanh(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestTanh()
    {
        Utils.AssertUlpRate(nameof(DoubleTestTanh), 1000, 0.9, 4,
            (-10.0, 10.0),
            x => simd_math.Tanh(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Tanh
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestTanh_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Tanh(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Tanh(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }
}

#endif
