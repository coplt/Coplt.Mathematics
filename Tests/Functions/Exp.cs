using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests.Functions;

[Parallelizable]
public class TestExp
{
    [Test, Parallelizable]
    public void TestLogFloat4([Random(0.000_1f, 1_000_000.0f, 1000)] float x)
    {
        var a = math.exp(new float4(x));
        var b = MathF.Exp(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }

    [Test, Parallelizable]
    public void TestLogDouble4([Random(0.000_1, 1_000_000.0, 1000)] double x)
    {
        var a = math.exp(new double4(x));
        var b = Math.Exp(x);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a.x).Within(1).Ulps);
    }
}
