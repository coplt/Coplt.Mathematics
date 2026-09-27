using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static b32m3x3 Some1(in float3x3 a) => math.isinf(a);
}
