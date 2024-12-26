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
    public void FloatTestACos([Random(-1.1f, 1.1f, 1000)] float x)
    {
        var a = simd_math.Acos(new float4(x).UnsafeGetInner()).GetElement(0);
        var b = MathF.Acos(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(2).Ulps);
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestACos([Random(-1.1f, 1.1f, 1000)] double x)
    {
        var a = simd_math.Acos(new double4(x).UnsafeGetInner()).GetElement(0);
        var b = Math.Acos(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(2).Ulps);
    }
}

#endif
