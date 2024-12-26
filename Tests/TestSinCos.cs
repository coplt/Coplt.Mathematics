using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;
namespace Tests;

[Parallelizable]
public class TestSinCos
{
    [Test]
    [Parallelizable]
    public void FloatTestSin([Random(-10f, 10.0f, 100)] float x)
    {
        var a = simd.Sin(new float4(x).UnsafeGetInner()).GetElement(0);
        var b = MathF.Sin(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(5000).Ulps);
    }

    [Test]
    [Parallelizable]
    public void DoubleTestSin([Random(-10, 100.0, 100)] double x)
    {
        var a = simd.Sin(new double4(x).UnsafeGetInner()).GetElement(0);
        var b = Math.Sin(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(5000).Ulps);
    }
    
    [Test]
    [Parallelizable]
    public void FloatTestCos([Random(-10f, 10.0f, 100)] float x)
    {
        var a = simd.Cos(new float4(x).UnsafeGetInner()).GetElement(0);
        var b = MathF.Cos(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(5000).Ulps);
    }

    [Test]
    [Parallelizable]
    public void DoubleTestCos([Random(-10, 100.0, 100)] double x)
    {
        var a = simd.Cos(new double4(x).UnsafeGetInner()).GetElement(0);
        var b = Math.Cos(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(5000).Ulps);
    }
}

#endif
