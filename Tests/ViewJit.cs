using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static double2 Some1(double2 a) => math.hmin(a);
}
