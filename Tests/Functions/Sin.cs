using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests.Functions;

public class TestSin
{
    [Test, Parallelizable]
    public void TestFloat4([Random(-10f, 10.0f, 100)] float x)
    {
        var a = math.sin(new float4(x));
        var b = MathF.Sin(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }

    [Test, Parallelizable]
    public void TestDouble4([Random(-10, 10.0, 100)] double x)
    {
        var a = math.sin(new double4(x));
        var b = Math.Sin(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }
}
