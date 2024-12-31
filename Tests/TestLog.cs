using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;
namespace Tests;

[Parallelizable]
public class TestLog
{
    [Test]
    [Parallelizable]
    public void FloatTestLog2()
    {
        Utils.AssertUlpRate(nameof(FloatTestLog2), 1000, 0.9, 1,
            (0.000_1f, 1_000_000.0f),
            x => simd_math.Log2(new float4(x).UnsafeGetInner()).GetElement(0),
            MathF.Log2
        );
    }
    
    [Test]
    [Parallelizable]
    public void FloatTestLog2_vec_2_4([Random(0.000_1f, 1_000_000.0f, 1000)] float v)
    {
        var a = simd_math.Log2(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Log2(new float2(v).UnsafeGetInner()).GetElement(0);
        Assert.That(a, Is.EqualTo(b));
    }
    
    [Test]
    [Parallelizable]
    public void FloatTestLog2Err([Values(0.0f, float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float v)
    {
        var a = simd_math.Log2(new float4(v).UnsafeGetInner()).GetElement(0);
        var b = MathF.Log2(v);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a));
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestLog2()
    {
        Utils.AssertUlpRate(nameof(DoubleTestLog2), 1000, 0.9, 1,
            (0.000_1, 1_000_000.0),
            x => simd_math.Log2(new double4(x).UnsafeGetInner()).GetElement(0),
            Math.Log2
        );
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestLog2_vec_2_4([Random(0.000_1, 1_000_000.0, 1000)] double v)
    {
        var a = simd_math.Log2(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = simd_math.Log2(new double2(v).UnsafeGetInner()).GetElement(0);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(a, Is.EqualTo(b));
    }
    
    [Test]
    [Parallelizable]
    public void DoubleTestLog2Err([Values(0.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double v)
    {
        var a = simd_math.Log2(new double4(v).UnsafeGetInner()).GetElement(0);
        var b = Math.Log2(v);
        Console.WriteLine($"{a}");
        Console.WriteLine($"{b}");
        Assert.That(b, Is.EqualTo(a));
    }
}

#endif
