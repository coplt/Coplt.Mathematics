using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static float3x3 Some(float3x3 m)
    {
        var (t0, t1, t2) = math.transpose(new float3x3(m.c1, m.c2, m.c0));

        var m0 = math.fsm(t1 * t2.yzx, t1.yzx, t2);
        var m1 = math.fsm(t0.yzx * t2, t0, t2.yzx);
        var m2 = math.fsm(t0 * t1.yzx, t0.yzx, t1);

        float3 rcp_det = (1.0f / math.sum(t0.zxy * m0));
        return new float3x3(m0, m1, m2) * rcp_det;
    }

    public static float3x3 inverse(float3x3 m)
    {
        var r0 = math.cross(m.c1, m.c2);
        var r1 = math.cross(m.c2, m.c0);
        var r2 = math.cross(m.c0, m.c1);

        var det = math.dot(m.c0, r0);
        float3 invDet = 1.0f / det;

        return math.transpose(new float3x3(r0 * invDet, r1 * invDet, r2 * invDet));
    }
}
