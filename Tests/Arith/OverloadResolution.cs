using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The calls below are not checks of a value, they are here so that they keep compiling and so that the member
/// every one of them is bound to does not change.
/// <para>A call whose arguments are all of a single component reaches the member that is declared for a single
/// component, which is the member of <c>math</c> that names the type of it, the member that the generator emits
/// for the scalar type of the value or the member of the value. The arguments of such a call are only converted
/// to the type of a component, the member that is reached broadcasts them itself.</para>
/// <para>A call that has a value as one of its arguments as well does not fit those members, it reaches the
/// member that only takes values of the same kind, the arguments of a single component are converted to it and
/// broadcast into every component of the value.</para>
/// <para>The member a call is bound to is not visible in the source, <see cref="TestOverloadResolutionCheck"/> reads
/// the calls below from the assembly of the tests and asserts the member every one of them reaches.</para>
/// </summary>
public class TestOverloadResolution
{
    public void ScalarArguments()
    {
        math.dot(new float3(1, 2, 3), new float3(1, 2, 3));
        math.clamp(new float3(1, 2, 3), 2, 5);
        math.lerp(1, 2, new float3(1, 2, 3));
        math.unlerp(new float3(1, 2, 3), 2, 3);
        math.unlerp(new float3(1, 2, 3), 2, new float3(1, 2, 3));
        math.remap(new float3(1, 2, 3), 1, 2, 3, 4);
        math.remap(new float3(1, 2, 3), new float3(1, 2, 3), 2, 3, 4);
        math.length_sq(new float3(1, 2, 3));
        math.distance_sq(new float3(1, 2, 3), new float3(4, 5, 6));
        math.sum(new float3(1, 2, 3));
        new float3(1, 2, 3).lerp(1, 2);
        new float3(1, 2, 3).lerp(1, new float3(1, 2, 3));
        new float3(1, 2, 3).unlerp(1, 2);
        new float3(1, 2, 3).unlerp(1, new float3(1, 2, 3));
        new float3(1, 2, 3).length_sq();
        new float3(1, 2, 3).distance_sq(new float3(4, 5, 6));
        new float3(1, 2, 3).sum();
    }
}
