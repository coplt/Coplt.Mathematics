using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestAcos
{
    [Test]
    [Parallelizable]
    public void FloatTestACos()
    {
        Utils.AssertUlpRate(nameof(FloatTestACos), 1000, 0.9, 2,
            (-1.1f, 1.1f),
            x => simd_math.Acos(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Acos
        );
    }

    [Test]
    [Parallelizable]
    public void DoubleTestACos()
    {
        Utils.AssertUlpRate(nameof(DoubleTestACos), 1000, 0.9, 2,
            (-1.1, 1.1),
            x => simd_math.Acos(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Acos
        );
    }
}

#endif
