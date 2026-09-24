using Coplt.Mathematics;
using static Coplt.Mathematics.math;
using static Coplt.Mathematics.math_ex;
using static Coplt.Mathematics.ex_float;

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
    }
}
