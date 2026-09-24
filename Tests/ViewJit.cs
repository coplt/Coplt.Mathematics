using System.Runtime.CompilerServices;
using Coplt.Mathematics;

namespace Tests;

public static class ViewJit
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static float NaN(float a, float b) => a / b;

    // public static float3 Some1(float3 a, float3 b, float3 c) => math.lerp(a, b, c);
    //
    // public static int3 Some1(int3 a, int3 b, int3 c) => math.lerp(a, b, c);
    //
    // public static float3 Some1(float a, float b, float3 c) => math.lerp(a, b, c);
    //
    // public static float2 Some1(float a, float b, float2 c) => math.lerp(a, b, c);


    [Test]
    public static void Some1()
    {
        var r = new float3(NaN(0, 0));
        Console.WriteLine(r);
    }
}
