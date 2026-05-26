using System.Runtime.Intrinsics;
using Coplt.Mathematics;

#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;

namespace Tests;

/// <summary>Accuracy tests for simd_math.Exp2 — float and double, all SIMD widths.</summary>
[Parallelizable]
public class TestExp2Accuracy
{
    // ── float Exp2 ─────────────────────────────────────────────────────────

    [Test, Parallelizable]
    public void Float_Exp2_Vector128([Random(-60f, 60f, 1000)] float x)
    {
        var result = simd_math.Exp2(new float4(x).UnsafeGetInner()).GetElement(0);
        Assert.That(result, Is.EqualTo(MathF.Pow(2f, x)).Within(2).Ulps);
    }

    [Test, Parallelizable]
    public void Float_Exp2_Vector64([Random(-60f, 60f, 1000)] float x)
    {
        var result = simd_math.Exp2(new float2(x).UnsafeGetInner()).GetElement(0);
        Assert.That(result, Is.EqualTo(MathF.Pow(2f, x)).Within(2).Ulps);
    }

    [Test, Parallelizable]
    public void Float_Exp2_ConsistentAcrossWidths([Random(-60f, 60f, 500)] float x)
    {
        var r64  = simd_math.Exp2(new float2(x).UnsafeGetInner()).GetElement(0);
        var r128 = simd_math.Exp2(new float4(x).UnsafeGetInner()).GetElement(0);
        Assert.That(r64, Is.EqualTo(r128));
    }

    [Test, Parallelizable]
    public void Float_Exp2_SpecialValues(
        [Values(0f, 1f, -1f, float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float x)
    {
        var result   = simd_math.Exp2(new float4(x).UnsafeGetInner()).GetElement(0);
        var expected = MathF.Pow(2f, x);
        // NaN: both must be NaN; Infinity: must match exactly; others: within 2 ULP
        if (float.IsNaN(expected))
            Assert.That(result, Is.NaN);
        else
            Assert.That(result, Is.EqualTo(expected).Within(2).Ulps);
    }

    // ── double Exp2 ────────────────────────────────────────────────────────

    [Test, Parallelizable]
    public void Double_Exp2_Vector128([Random(-600.0, 600.0, 1000)] double x)
    {
        var result = simd_math.Exp2(new double2(x).UnsafeGetInner()).GetElement(0);
        Assert.That(result, Is.EqualTo(Math.Pow(2.0, x)).Within(2).Ulps);
    }

    [Test, Parallelizable]
    public void Double_Exp2_Vector256([Random(-600.0, 600.0, 1000)] double x)
    {
        var result = simd_math.Exp2(new double4(x).UnsafeGetInner()).GetElement(0);
        Assert.That(result, Is.EqualTo(Math.Pow(2.0, x)).Within(2).Ulps);
    }

    [Test, Parallelizable]
    public void Double_Exp2_ConsistentAcrossWidths([Random(-600.0, 600.0, 500)] double x)
    {
        var r128 = simd_math.Exp2(new double2(x).UnsafeGetInner()).GetElement(0);
        var r256 = simd_math.Exp2(new double4(x).UnsafeGetInner()).GetElement(0);
        Assert.That(r128, Is.EqualTo(r256));
    }

    [Test, Parallelizable]
    public void Double_Exp2_SpecialValues(
        [Values(0.0, 1.0, -1.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double x)
    {
        var result   = simd_math.Exp2(new double4(x).UnsafeGetInner()).GetElement(0);
        var expected = Math.Pow(2.0, x);
        if (double.IsNaN(expected))
            Assert.That(result, Is.NaN);
        else
            Assert.That(result, Is.EqualTo(expected).Within(2).Ulps);
    }
}

#endif
