using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests.Functions;

[Parallelizable]
public class TestLog
{
    [Test, Parallelizable]
    public void TestLogFloat4([Random(0.000_1f, 1_000_000.0f, 1000)] float x)
    {
        var a = math.log(new float4(x));
        var b = MathF.Log(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }

    [Test, Parallelizable]
    public void TestLogDouble4([Random(0.000_1, 1_000_000.0, 1000)] double x)
    {
        var a = math.log(new double4(x));
        var b = Math.Log(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }

    [Test, Parallelizable]
    public void TestLog2Float4([Random(0.000_1f, 1_000_000.0f, 1000)] float x)
    {
        var a = math.log2(new float4(x));
        var b = MathF.Log2(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }

    [Test, Parallelizable]
    public void TestLog2Double4([Random(0.000_1, 1_000_000.0, 1000)] double x)
    {
        var a = math.log2(new double4(x));
        var b = Math.Log2(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }

    [Test, Parallelizable]
    public void TestLog10Float4([Random(0.000_1f, 1_000_000.0f, 1000)] float x)
    {
        var a = math.log10(new float4(x));
        var b = MathF.Log10(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(4).Ulps);
    }

    [Test, Parallelizable]
    public void TestLog10Double4([Random(0.000_1, 1_000_000.0, 1000)] double x)
    {
        var a = math.log10(new double4(x));
        var b = Math.Log10(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(4).Ulps);
    }
}
