using Coplt.Mathematics;
using static Coplt.Mathematics.math;
using static Coplt.Mathematics.math_ex;
using static Coplt.Mathematics.ex_float;
using static Coplt.Mathematics.ex_float3;
using static Coplt.Mathematics.ex_float2;
using static Coplt.Mathematics.ex_double;
using static Coplt.Mathematics.ex_half;

namespace Tests.Arith;

/// <summary>
/// The same calls as <see cref="TestOverloadResolution"/>, the members are imported with <c>using static</c>
/// instead of being qualified, so the member of every one of them is reached by a name of its own as well, see
/// the documentation of the class of the calls for what the calls check and <see cref="TestOverloadResolutionCheck"/>
/// for the member every one of them is bound to.
/// </summary>
public class TestOverloadResolutionUsingStatic
{
    public void ScalarArguments()
    {
        var r = dot(new float3(1, 2, 3), new float3(1, 2, 3));
        clamp(new float3(1, 2, 3), 2, 5);
        lerp(1, 2, new float3(1, 2, 3));
        unlerp(new float3(1, 2, 3), 2, 3);
        unlerp(new float3(1, 2, 3), 2, new float3(1, 2, 3));
        remap(new float3(1, 2, 3), 1, 2, 3, 4);
        remap(new float3(1, 2, 3), new float3(1, 2, 3), 2, 3, 4);
        length_sq(new float3(1, 2, 3));
        distance_sq(new float3(1, 2, 3), new float3(4, 5, 6));
        sum(new float3(1, 2, 3));
        hmin(new float3(1, 2, 3));
        hmax(new float3(1, 2, 3));
        hmin_native(new float3(1, 2, 3));
        hmax_native(new float3(1, 2, 3));
        wrap(new float3(1, 2, 3), new float3(0, 0, 0), new float3(1, 1, 1));
        wrap<float3, float>(new float3(1, 2, 3), 0f, 1f);
        wrap(new float3(1, 2, 3), 0f, 1f);
        wrap(new float3(1, 2, 3), 0, 1);
        wrap(new float2(1, 2), 0, 1);
        wrap(new double2(1, 2), 0, 1);
        wrap(new half3((Half)1, (Half)2, (Half)3), (Half)0, (Half)1);
    }

    /// <summary>
    /// The same calls as <see cref="TestOverloadResolution.MatrixArguments"/> that the math class carries, the
    /// member of the type of a column of the value and the one of the type of a row of it are imported with
    /// <c>using static</c> as well, so a name of its own reaches the one of them that fits the value.
    /// </summary>
    public void MatrixArguments()
    {
        var m = new float3x2(new float3(1, 2, 3), new float3(4, 5, 6));
        csum(m);
        rsum(m);
        wrap(m, 0f, 1f);
    }
}
