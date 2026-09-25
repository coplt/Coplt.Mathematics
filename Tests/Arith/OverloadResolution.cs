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
/// <para>The type of a vector a matrix is made of is not a part of the type of the value either, so a call of the
/// member that reduces the vectors of a matrix to a vector reaches the member of the vector type of the value
/// when it does not name it: the calls of <see cref="MatrixArguments"/> pin the member every one of them
/// reaches.</para>
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
        math.hmin(new float3(1, 2, 3));
        math.hmax(new float3(1, 2, 3));
        new float3(1, 2, 3).lerp(1, 2);
        new float3(1, 2, 3).lerp(1, new float3(1, 2, 3));
        new float3(1, 2, 3).unlerp(1, 2);
        new float3(1, 2, 3).unlerp(1, new float3(1, 2, 3));
        new float3(1, 2, 3).length_sq();
        new float3(1, 2, 3).distance_sq(new float3(4, 5, 6));
        new float3(1, 2, 3).sum();
        new float3(1, 2, 3).hmin();
        new float3(1, 2, 3).hmax();
    }

    /// <summary>
    /// The sum of the columns of a matrix is the member of the type of a column of it and the sum of the rows of
    /// a row of it, so a call that does not name that type reaches the member of the vector type of the value,
    /// which the ex_ classes add to the math class and which the math_ex_ classes add to the value itself. The
    /// first two calls are the ones the using static form of them pins as well, see
    /// <see cref="TestOverloadResolutionUsingStatic.MatrixArguments"/>.
    /// </summary>
    public void MatrixArguments()
    {
        var m = new float3x2(new float3(1, 2, 3), new float3(4, 5, 6));
        math.csum(m);
        math.rsum(m);
        math.csum<float3x2, float3>(m);
        math.rsum<float3x2, float2>(m);
        m.csum();
        m.rsum();
    }
}
