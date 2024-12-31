using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestAsin
{
    [Test]
    [Parallelizable]
    public void FloatTestASin()
    {
        Utils.AssertUlpRate(nameof(FloatTestASin), 1000, 0.9, 2,
            (-1.1f, 1.1f),
            x => simd_math.Asin(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Asin
        );
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestASin()
    {
        Utils.AssertUlpRate(nameof(FloatTestASin), 1000, 0.9, 2,
            (-1.1, 1.1),
            x => simd_math.Asin(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Asin
        );
    }
}

#endif
