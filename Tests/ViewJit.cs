using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;

namespace Tests;

public static class ViewJit
{
    // public static float3 Some1(float3 a, float3 b, float3 c) => math.lerp(a, b, c);
    //
    // public static int3 Some1(int3 a, int3 b, int3 c) => math.lerp(a, b, c);
    //
    public static float3 Some1(float a, float b, float3 c) => math.lerp(a, b, c);
    //
    // public static float2 Some1(float a, float b, float2 c) => math.lerp(a, b, c);
}
