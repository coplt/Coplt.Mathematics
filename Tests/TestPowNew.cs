using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestPowNew
{
    [Test]
    [Parallelizable]
    public void FloatTestPow()
    {
        Utils.AssertUlpRate(nameof(FloatTestPow), 1000, 0.9, 16,
            (0.001f, 10f), (-10f, 10f),
            (x, y) => simd_math.Pow(new float4(x).UnsafeGetInner(), new float4(y).UnsafeGetInner()).GetElement(0),
            MathF.Pow
        );
    }

    [Test]
    [Parallelizable]
    public void FloatTestPowOf2()
    {
        Utils.AssertUlpRate(nameof(FloatTestPowOf2), 1000, 0.9, 2,
            (0f, 32f),
            x => simd_math.Pow(new float4(2).UnsafeGetInner(), new float4(x).UnsafeGetInner()).GetElement(0),
            x => MathF.Pow(2, x)
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestPow()
    {
        Utils.AssertUlpRate(nameof(DoubleTestPow), 1000, 0.9, 16,
            (0.001, 10.0), (-10.0, 10.0),
            (x, y) => simd_math.Pow(new double4(x).UnsafeGetInner(), new double4(y).UnsafeGetInner()).GetElement(0),
            Math.Pow
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestPowOf2()
    {
        Utils.AssertUlpRate(nameof(DoubleTestPowOf2), 1000, 0.9, 700,
            (0.0, 32.0),
            x => simd_math.Pow(new double4(2).UnsafeGetInner(), new double4(x).UnsafeGetInner()).GetElement(0),
            x => Math.Pow(2, x)
        );
    }
}

#endif
