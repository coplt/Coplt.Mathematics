using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestAtan
{
    [Test]
    [Parallelizable]
    public void FloatTestATan()
    {
        Utils.AssertUlpRate(nameof(FloatTestATan), 1000, 0.9, 2,
            (-12f, 12f),
            x => simd_math.Atan(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Atan
        );
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestATan()
    {
        Utils.AssertUlpRate(nameof(DoubleTestATan), 1000, 0.9, 2,
            (-12.0, 12.0),
            x => simd_math.Atan(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Atan
        );
    }
}

#endif
