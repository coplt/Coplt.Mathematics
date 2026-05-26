using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestTanNew
{
    [Test]
    [Parallelizable]
    public void FloatTestTan()
    {
        Utils.AssertUlpRate(nameof(FloatTestTan), 1000, 0.9, 3000,
            (-10f, 10f),
            x => simd_math.Tan(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Tan
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestTan_vec_2_4([Random(-10f, 10.0f, 1000)] float v)
    {
        var a = simd_math.Tan(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Tan(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    [Parallelizable]
    public void DoubleTestTan()
    {
        Utils.AssertUlpRate(nameof(DoubleTestTan), 1000, 0.9, 8000,
            (-10.0, 10.0),
            x => simd_math.Tan(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Tan
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestTan_vec_2_4([Random(-10.0, 10.0, 1000)] double v)
    {
        var a = simd_math.Tan(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Tan(new double2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }
}

#endif
