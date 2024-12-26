using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;
namespace Tests;

[Parallelizable]
public class TestSinhCosh
{
    [Test]
    [Parallelizable]
    public void FloatTestSinh([Random(-10f, 10.0f, 100)] float x)
    {
        var a = simd_math.Sinh(new float4(x).UnsafeGetInner()).GetElement(0);
        var b = MathF.Sinh(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(30).Ulps);
    }

    [Test]
    [Parallelizable]
    public void DoubleTestSinh([Random(-10, 10.0, 100)] double x)
    {
        var a = simd_math.Sinh(new double4(x).UnsafeGetInner()).GetElement(0);
        var b = Math.Sinh(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(30).Ulps);
    }
    
    [Test]
    [Parallelizable]
    public void FloatTestCosh([Random(-10f, 10.0f, 100)] float x)
    {
        var a = simd_math.Cosh(new float4(x).UnsafeGetInner()).GetElement(0);
        var b = MathF.Cosh(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(10).Ulps);
    }

    [Test]
    [Parallelizable]
    public void DoubleTestCosh([Random(-10, 10.0, 100)] double x)
    {
        var a = simd_math.Cosh(new double4(x).UnsafeGetInner()).GetElement(0);
        var b = Math.Cosh(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(10).Ulps);
    }
}

#endif
