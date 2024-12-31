using System.Runtime.CompilerServices;
using Coplt.Mathematics;

namespace Tests;

public static class Utils
{
    public static float RandomFloat(float min, float max) => Random.Shared.NextSingle() * (max - min) + min;
    public static double RandomDouble(double min, double max) => Random.Shared.NextDouble() * (max - min) + min;

    public static uint CalcULP(float a, float b)
    {
        if (float.IsNaN(a) && float.IsNaN(b)) return 0;

        var left = a.asu();
        var right = b.asu();

        if (left == right) return 0;

        var leftSignMask = left >> 31;
        var rightSignMask = right >> 31;

        left = ((0x80000000 - left) & leftSignMask) | (left & ~leftSignMask);
        right = ((0x80000000 - right) & rightSignMask) | (right & ~rightSignMask);

        if (leftSignMask != rightSignMask)
        {
            if (left == right) return 0;

            return Math.Max(Math.Abs(left.asi()), Math.Abs(right.asi())).asu();
        }

        return Math.Abs(left.asi() - right.asi()).asu();
    }

    public static ulong CalcULP(double a, double b)
    {
        if (double.IsNaN(a) && double.IsNaN(b)) return 0;

        var left = a.asu();
        var right = b.asu();

        if (left == right) return 0;

        var leftSignMask = left >> 63;
        var rightSignMask = right >> 63;

        left = ((0x8000000000000000 - left) & leftSignMask) | (left & ~leftSignMask);
        right = ((0x8000000000000000 - right) & rightSignMask) | (right & ~rightSignMask);

        if (leftSignMask != rightSignMask)
        {
            if (left == right) return 0;

            return Math.Max(Math.Abs(left.asi()), Math.Abs(right.asi())).asu();
        }

        return Math.Abs(left.asi() - right.asi()).asu();
    }

    public static void AssertUlpRate(string name, int count, double rate, int target_ulp,
        (float min, float max) range,
        Func<float, float> calc_a,
        Func<float, float> calc_b
    )
    {
        var data = ParallelEnumerable.Range(0, count)
            .Select(_ => RandomFloat(range.min, range.max))
            .Select(i =>
            {
                var a = calc_a(i);
                var b = calc_b(i);
                var ulp = CalcULP(a, b);
                return (a, b, ulp, i);
            }).ToArray();
        var pass_rate = data.AsParallel()
            .Select(i =>
            {
                var (a, b, ulp, p) = i;
                if (ulp > (uint)target_ulp)
                {
                    Console.WriteLine($"[{name}] Ulp Error: {{ Ulp = {ulp}, A = {a}, B = {b}, I = {p} }}");
                    return 0;
                }
                return 1;
            })
            .Sum() / (double)count;
        var max_ulp = data.AsParallel().Max(static i => i.ulp);
        var avg_ulp = data.AsParallel().Average(static i => (int)i.ulp);
        Console.WriteLine($"[{name}] Ulp Pass Rate: {pass_rate * 100:P}; Max Ulp: {max_ulp}; Avg Ulp: {avg_ulp}");
        Assert.That(pass_rate, Is.GreaterThanOrEqualTo(rate));
    }

    public static void AssertUlpRate(string name, int count, double rate, int target_ulp,
        (double min, double max) range,
        Func<double, double> calc_a,
        Func<double, double> calc_b
    )
    {
        var data = ParallelEnumerable.Range(0, count)
            .Select(_ => RandomDouble(range.min, range.max))
            .Select(i =>
            {
                var a = calc_a(i);
                var b = calc_b(i);
                var ulp = CalcULP(a, b);
                return (a, b, ulp, i);
            }).ToArray();
        var pass_rate = data.AsParallel()
            .Select(i =>
            {
                var (a, b, ulp, p) = i;
                if (ulp > (uint)target_ulp)
                {
                    Console.WriteLine($"[{name}] Ulp Error: {{ Ulp = {ulp}, A = {a}, B = {b}, I = {p} }}");
                    return 0;
                }
                return 1;
            })
            .Sum() / (double)count;
        var max_ulp = data.AsParallel().Max(static i => i.ulp);
        var avg_ulp = data.AsParallel().Average(static i => (long)i.ulp);
        Console.WriteLine($"[{name}] Ulp Pass Rate: {pass_rate * 100:P}; Max Ulp: {max_ulp}; Avg Ulp: {avg_ulp}");
        Assert.That(pass_rate, Is.GreaterThanOrEqualTo(rate));
    }

    public static void AssertUlpRate(string name, int count, double rate, int target_ulp,
        (float min, float max) range_a,
        (float min, float max) range_b,
        Func<float, float, float> calc_a,
        Func<float, float, float> calc_b
    )
    {
        var data = ParallelEnumerable.Range(0, count)
            .Select(_ => (RandomFloat(range_a.min, range_a.max), RandomFloat(range_b.min, range_b.max)))
            .Select(i =>
            {
                var (ia, ib) = i;
                var a = calc_a(ia, ib);
                var b = calc_b(ia, ib);
                var ulp = CalcULP(a, b);
                return (a, b, ulp, ia, ib);
            }).ToArray();
        var pass_rate = data.AsParallel()
            .Select(i =>
            {
                var (a, b, ulp, ia, ib) = i;
                if (ulp > (uint)target_ulp)
                {
                    Console.WriteLine($"[{name}] Ulp Error: {{ Ulp = {ulp}, A = {a}, B = {b}, IA = {ia}, IB = {ib} }}");
                    return 0;
                }
                return 1;
            })
            .Sum() / (double)count;
        var max_ulp = data.AsParallel().Max(static i => i.ulp);
        var avg_ulp = data.AsParallel().Average(static i => (long)i.ulp);
        Console.WriteLine($"[{name}] Ulp Pass Rate: {pass_rate * 100:P}; Max Ulp: {max_ulp}; Avg Ulp: {avg_ulp}");
        Assert.That(pass_rate, Is.GreaterThanOrEqualTo(rate));
    }

    public static void AssertUlpRate(string name, int count, double rate, long target_ulp,
        (double min, double max) range_a,
        (double min, double max) range_b,
        Func<double, double, double> calc_a,
        Func<double, double, double> calc_b
    )
    {
        var data = ParallelEnumerable.Range(0, count)
            .Select(_ => (RandomDouble(range_a.min, range_a.max), RandomDouble(range_b.min, range_b.max)))
            .Select(i =>
            {
                var (ia, ib) = i;
                var a = calc_a(ia, ib);
                var b = calc_b(ia, ib);
                var ulp = CalcULP(a, b);
                return (a, b, ulp, ia, ib);
            }).ToArray();
        var pass_rate = data.AsParallel()
            .Select(i =>
            {
                var (a, b, ulp, ia, ib) = i;
                if (ulp > (ulong)target_ulp)
                {
                    Console.WriteLine($"[{name}] Ulp Error: {{ Ulp = {ulp}, A = {a}, B = {b}, IA = {ia}, IB = {ib} }}");
                    return 0;
                }
                return 1;
            })
            .Sum() / (double)count;
        var max_ulp = data.AsParallel().Max(static i => i.ulp);
        var avg_ulp = data.AsParallel().Average(static i => (long)i.ulp);
        Console.WriteLine($"[{name}] Ulp Pass Rate: {pass_rate * 100:P}; Max Ulp: {max_ulp}; Avg Ulp: {avg_ulp}");
        Assert.That(pass_rate, Is.GreaterThanOrEqualTo(rate));
    }
}
