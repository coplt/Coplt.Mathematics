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
    public void FloatTestASin([Random(-1.1f, 1.1f, 1000)] float x)
    {
        var a = simd_math.Asin(new float4(x).UnsafeGetInner()).GetElement(0);
        var b = MathF.Asin(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(2).Ulps);
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestASin([Random(-1.1f, 1.1f, 1000)] double x)
    {
        var a = simd_math.Asin(new double4(x).UnsafeGetInner()).GetElement(0);
        var b = Math.Asin(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(2).Ulps);
    }
}

#endif
