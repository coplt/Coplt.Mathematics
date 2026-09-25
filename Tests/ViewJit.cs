using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static double2 Some1(double2 a) => math.hmin(a);
    public static double2 Some2(double2 a) => math.hmin_native(a);
}
