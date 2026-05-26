namespace Benchmarks;

/// <summary>Benchmarks for simd_math.Exp2 — float and double, all SIMD widths.</summary>
[DisassemblyDiagnoser]
public class BenchExp2
{
    // 1 024 random-ish floats in [-60, 60]
    private static readonly float[] _f32 = GenerateF32();
    private static readonly double[] _f64 = GenerateF64();

    private static float[] GenerateF32()
    {
        const int n = 1024;
        var a = new float[n];
        for (var i = 0; i < n; i++)
            a[i] = (i - n / 2) * (120f / n);   // spans [-60, 60]
        return a;
    }

    private static double[] GenerateF64()
    {
        const int n = 1024;
        var a = new double[n];
        for (var i = 0; i < n; i++)
            a[i] = (i - n / 2) * (1200.0 / n); // spans [-600, 600]
        return a;
    }

    // ── float ──────────────────────────────────────────────────────────────

    [Benchmark(Description = "Exp2 float×2 (Vector64)")]
    public float Exp2_f32x2()
    {
        var acc = Vector64<float>.Zero;
        for (var i = 0; i < _f32.Length - 1; i += 2)
        {
            var v = Vector64.Create(_f32[i], _f32[i + 1]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }

    [Benchmark(Description = "Exp2 float×4 (Vector128)", Baseline = true)]
    public float Exp2_f32x4()
    {
        var acc = Vector128<float>.Zero;
        for (var i = 0; i < _f32.Length - 3; i += 4)
        {
            var v = Vector128.Create(_f32[i], _f32[i + 1], _f32[i + 2], _f32[i + 3]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }

    [Benchmark(Description = "Exp2 float×8 (Vector256)")]
    public float Exp2_f32x8()
    {
        var acc = Vector256<float>.Zero;
        for (var i = 0; i < _f32.Length - 7; i += 8)
        {
            var v = Vector256.Create(
                _f32[i], _f32[i + 1], _f32[i + 2], _f32[i + 3],
                _f32[i + 4], _f32[i + 5], _f32[i + 6], _f32[i + 7]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }

    [Benchmark(Description = "Exp2 float×16 (Vector512)")]
    public float Exp2_f32x16()
    {
        var acc = Vector512<float>.Zero;
        for (var i = 0; i < _f32.Length - 15; i += 16)
        {
            var v = Vector512.Create(
                _f32[i],      _f32[i + 1],  _f32[i + 2],  _f32[i + 3],
                _f32[i + 4],  _f32[i + 5],  _f32[i + 6],  _f32[i + 7],
                _f32[i + 8],  _f32[i + 9],  _f32[i + 10], _f32[i + 11],
                _f32[i + 12], _f32[i + 13], _f32[i + 14], _f32[i + 15]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }

    // ── double ─────────────────────────────────────────────────────────────

    [Benchmark(Description = "Exp2 double×2 (Vector128)")]
    public double Exp2_f64x2()
    {
        var acc = Vector128<double>.Zero;
        for (var i = 0; i < _f64.Length - 1; i += 2)
        {
            var v = Vector128.Create(_f64[i], _f64[i + 1]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }

    [Benchmark(Description = "Exp2 double×4 (Vector256)")]
    public double Exp2_f64x4()
    {
        var acc = Vector256<double>.Zero;
        for (var i = 0; i < _f64.Length - 3; i += 4)
        {
            var v = Vector256.Create(_f64[i], _f64[i + 1], _f64[i + 2], _f64[i + 3]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }

    [Benchmark(Description = "Exp2 double×8 (Vector512)")]
    public double Exp2_f64x8()
    {
        var acc = Vector512<double>.Zero;
        for (var i = 0; i < _f64.Length - 7; i += 8)
        {
            var v = Vector512.Create(
                _f64[i], _f64[i + 1], _f64[i + 2], _f64[i + 3],
                _f64[i + 4], _f64[i + 5], _f64[i + 6], _f64[i + 7]);
            acc += simd_math.Exp2(v);
        }
        return acc.GetElement(0);
    }
}
