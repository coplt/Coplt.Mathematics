using Coplt.Mathematics;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The arithmetic members of a vector are members of the vector itself and the members of the math class reach
/// them as well. None of the forwarded members needs the type of a single component, so the compiler infers the
/// vector type from the argument and a call of them does not have to name it.
/// </summary>
public class TestMathArithForwarding
{
    /// <summary>
    /// The members that only need the vector type are reachable with the vector type alone, so the constraint of
    /// every one of them is the interface that has no component type.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IVectorArithmetic<T>, IDynamicVector<T>
    {
        math.abs(v);
        math.sign(v);
        math.min(v, v);
        math.max(v, v);
        math.clamp(v, v, v);
        // the range of the interpolation members has to be wider than the value, an integer division by the
        // zero width of a degenerate range would not be defined
        math.lerp(v + v, v + v + v, v);
        math.unlerp(v + v, v, v + v + v);
        math.remap(v, v, v + v, v, v + v);
        math.square(v);
        math.fma(v, v, v);
        math.fms(v, v, v);
        math.fnma(v, v, v);
        math.fsm(v, v, v);
        math.fam(v, v, v);
        math.mad(v, v, v);
    }

    /// <summary>
    /// The members that name the type of a single component are reachable as well: a member that takes a
    /// component infers the type of it from the argument, a member that only returns one names both types.
    /// </summary>
    private static void CheckScalar<T, TScalar>(T v, TScalar s0, TScalar s1)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged
    {
        // the width of the range of the members that divide by it has to be wider than zero
        var b = v + v;
        math.clamp(v, s0, s1);
        math.lerp(s0, s1, v);
        math.lerp(v, b, s0);
        math.unlerp(v, s0, s1);
        math.unlerp(s0, v, b);
        math.remap(v, s0, s1, s0, s1);
        _ = math.dot<T, TScalar>(v, v);
        _ = math.length_sq<T, TScalar>(v);
        _ = math.distance_sq<T, TScalar>(v, v);
        _ = math.csum<T, TScalar>(v);
        _ = math.cmin<T, TScalar>(v);
        _ = math.cmax<T, TScalar>(v);
        _ = math.cmin_safe<T, TScalar>(v);
        _ = math.cmax_safe<T, TScalar>(v);
    }

    [Test]
    public void Forwarding()
    {
        // the members of the math class reach the ones of the vector, the call is the same as the member
        Check(new float2(1, 2));
        Check(new float3(1, 2, 3));
        Check(new double2(1, 2));
        Check(new int2(1, 2));
        Check(new uint3(1, 2, 3));
        Check(new long4(1, 2, 3, 4));
        Check(new int3s(1, 2, 3));
        Check(new half2((half)1f, (half)2f));
        CheckScalar(new float3(1, 2, 3), 1f, 2f);
        CheckScalar(new double2(1, 2), 1d, 2d);
        CheckScalar(new half3((half)1f, (half)2f, (half)3f), (half)1f, (half)2f);
        CheckScalar(new int3(1, 2, 3), 1, 2);
        CheckScalar(new uint2(1, 2), 1u, 2u);
        CheckScalar(new long3s(1, 2, 3), 1L, 2L);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.abs(new int3(-1, 2, -3)), Is.EqualTo(new int3(1, 2, 3)));
            Assert.That(math.sign(new int3(-5, 0, 5)), Is.EqualTo(new int3(-1, 0, 1)));
            Assert.That(math.min(new int3(1, 2, 3), new int3(3, 2, 1)), Is.EqualTo(new int3(1, 2, 1)));
            Assert.That(math.max(new int3(1, 2, 3), new int3(3, 2, 1)), Is.EqualTo(new int3(3, 2, 3)));
            Assert.That(math.clamp(new int3(-1, 2, 5), new int3(0), new int3(3)), Is.EqualTo(new int3(0, 2, 3)));
            Assert.That(math.square(new int3(2, 3, 4)), Is.EqualTo(new int3(4, 9, 16)));
            Assert.That(math.lerp(default(float3), new float3(2f), new float3(0.5f)), Is.EqualTo(new float3(1f)));
            Assert.That(math.unlerp(new float3(1f), default, new float3(2f)), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.remap(new float3(0.5f), default, new float3(1f), default, new float3(10f)),
                Is.EqualTo(new float3(5f)));
        }
    }

    /// <summary>
    /// The members that name the type of a single component reach the ones of the vector as well. The type of a
    /// component that is an argument is inferred from it, the type that is only the result of the member is
    /// spelled out by the caller.
    /// </summary>
    [Test]
    public void Scalar()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.clamp(new float3(-1f, 2f, 5f), 0f, 3f), Is.EqualTo(new float3(0f, 2f, 3f)));
            Assert.That(math.lerp(0f, 1f, new float3(0.5f)), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.lerp(default(float3), new float3(1f), 0.5f), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.unlerp(new float3(1.5f), 1f, 2f), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.unlerp(1.5f, default, new float3(2f)), Is.EqualTo(new float3(0.75f)));
            Assert.That(math.remap(new float3(0.5f), 0f, 1f, 0f, 10f), Is.EqualTo(new float3(5f)));
            Assert.That(math.dot<float3, float>(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)), Is.EqualTo(32f));
            Assert.That(math.length_sq<float3, float>(new float3(3f, 4f, 0f)), Is.EqualTo(25f));
            Assert.That(math.distance_sq<float3, float>(new float3(1f), new float3(4f, 5f, 1f)), Is.EqualTo(25f));
            Assert.That(math.csum<float3, float>(new float3(1f, 2f, 3f)), Is.EqualTo(6f));
            // the padding component of a vector is zero, so the minimum of a vector is only the component itself
            // when every component is less than it
            Assert.That(math.cmin<float3, float>(new float3(-3f, -1f, -2f)), Is.EqualTo(-3f));
            Assert.That(math.cmax<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(3f));
            Assert.That(math.cmin_safe<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(1f));
            Assert.That(math.cmax_safe<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(3f));
        }
    }

    /// <summary>
    /// The cross product is only reachable with a vector of 3 components, so the constraint of it is the
    /// interface of the 3 component arithmetic.
    /// </summary>
    [Test]
    public void Cross()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.cross(new int3(1, 0, 0), new int3(0, 1, 0)), Is.EqualTo(new int3(0, 0, 1)));
            Assert.That(math.cross(new float3(1f, 0f, 0f), new float3(0f, 1f, 0f)), Is.EqualTo(new float3(0f, 0f, 1f)));
            Assert.That(math.cross(new double3s(1, 0, 0), new double3s(0, 1, 0)), Is.EqualTo(new double3s(0, 0, 1)));
        }
    }

    /// <summary>
    /// The fused members are the ones of the vector, the multiplication and the addition are only fused by the
    /// operand order of the name.
    /// </summary>
    [Test]
    public void Fused()
    {
        var a = new float3(2f, 3f, 4f);
        var b = new float3(5f, 6f, 7f);
        var c = new float3(10f, 20f, 30f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.fma(a, b, c), Is.EqualTo(a * b + c));
            Assert.That(math.fms(a, b, c), Is.EqualTo(a * b - c));
            Assert.That(math.fnma(a, b, c), Is.EqualTo(c - a * b));
            Assert.That(math.fsm(c, a, b), Is.EqualTo(c - a * b));
            Assert.That(math.fam(c, a, b), Is.EqualTo(c + a * b));
            Assert.That(math.mad(a, b, c), Is.EqualTo(a * b + c));
        }
    }
}
