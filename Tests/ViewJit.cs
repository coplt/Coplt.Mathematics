using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static float3 Some1(in float3 a, in float3 b) => math.reflect(a, b);
    public static float4 Some1(in float4 a, in float4 b) => math.reflect(a, b);
}
