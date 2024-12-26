using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

[Parallelizable]
public class TestAtan2
{
    [Test]
    [Parallelizable]
    public void FloatTestATan2([Random(-10f, 10f, 50)] float x, [Random(-10f, 10f, 50)] float y)
    {
        var a = simd_math.Atan2(new float4(x).UnsafeGetInner(), new float4(y).UnsafeGetInner()).GetElement(0);
        var b = MathF.Atan2(x, y);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(2).Ulps);
    }

    [Test]
    [Parallelizable]
    [TestCase(0f, -0f, float.Pi)]
    [TestCase(0f, 0f, 0f)]
    [TestCase(float.PositiveInfinity, 0f, float.Pi / 2)]
    [TestCase(float.PositiveInfinity, float.NegativeInfinity, 3 * float.Pi / 4)]
    [TestCase(float.PositiveInfinity, float.PositiveInfinity, float.Pi / 4)]
    [TestCase(-1f, 0f, -float.Pi / 2)]
    [TestCase(1f, 0f, float.Pi / 2)]
    [TestCase(1f, float.NegativeInfinity, float.Pi)]
    [TestCase(-1f, float.NegativeInfinity, -float.Pi)]
    [TestCase(1f, float.PositiveInfinity, 0f)]
    [TestCase(-1f, float.PositiveInfinity, -0f)]
    [TestCase(float.NaN, float.NaN, float.NaN)]
    public void FloatTestATan2Error1(float y, float x, float r)
    {
        var a = simd_math.Atan2(new float4(y).UnsafeGetInner(), new float4(x).UnsafeGetInner()).GetElement(0);
        Console.WriteLine($"{a}");
        Assert.That(a, Is.EqualTo(r));
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestATan2([Random(-10.0, 10.0, 50)] double x, [Random(-10.0, 10.0, 50)] double y)
    {
        var a = simd_math.Atan2(new double4(x).UnsafeGetInner(), new double4(y).UnsafeGetInner()).GetElement(0);
        var b = Math.Atan2(x, y);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a).Within(2).Ulps);
    }

    [Test]
    [Parallelizable]
    [TestCase(0.0, -0.0, double.Pi)]
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(double.PositiveInfinity, 0.0, double.Pi / 2)]
    [TestCase(double.PositiveInfinity, double.NegativeInfinity, 3 * double.Pi / 4)]
    [TestCase(double.PositiveInfinity, double.PositiveInfinity, double.Pi / 4)]
    [TestCase(-1.0, 0.0, -double.Pi / 2)]
    [TestCase(1.0, 0.0, double.Pi / 2)]
    [TestCase(1.0, double.NegativeInfinity, double.Pi)]
    [TestCase(-1.0, double.NegativeInfinity, -double.Pi)]
    [TestCase(1.0, double.PositiveInfinity, 0.0)]
    [TestCase(-1.0, double.PositiveInfinity, -0.0)]
    [TestCase(double.NaN, double.NaN, double.NaN)]
    public void DoubleTestATan2Error1(double y, double x, double r)
    {
        var a = simd_math.Atan2(new double4(y).UnsafeGetInner(), new double4(x).UnsafeGetInner()).GetElement(0);
        Console.WriteLine($"{a}");
        Assert.That(a, Is.EqualTo(r));
    }
}

#endif
