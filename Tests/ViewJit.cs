using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static double3 Some1(double3 a, double3 b, double3 c) => math.fms(a, b, c);
}
