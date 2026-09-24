using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The dot product of two vectors: the type of a single component is only a part of the result of the member
/// that names it, so the compiler cannot infer it from the arguments and a call that does not name it reaches the
/// member of the component type of the vector instead, which the ex_ classes add to the math class and which the
/// math_ex_ classes add to the vector itself.
/// </summary>
public class TestDot
{
    [Test]
    public void Values()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component type of a number vector reaches the member of its own kind
            Assert.That(math.dot(new float2(1f, 2f), new float2(2f, 3f)), Is.EqualTo(8f), "float2");
            Assert.That(math.dot(new float3(1f, 2f, 3f), new float3(2f, 3f, 4f)), Is.EqualTo(20f), "float3");
            Assert.That(math.dot(new float4(1f, 2f, 3f, 4f), new float4(2f, 3f, 4f, 5f)), Is.EqualTo(40f), "float4");
            Assert.That(math.dot(new double2(1d, 2d), new double2(2d, 3d)), Is.EqualTo(8d), "double2");
            Assert.That(math.dot(new double3(1d, 2d, 3d), new double3(2d, 3d, 4d)), Is.EqualTo(20d), "double3");
            Assert.That(math.dot(new half3((half)1f, (half)2f, (half)3f), new half3((half)2f, (half)3f, (half)4f)),
                Is.EqualTo((half)20f), "half3");
            Assert.That(math.dot(new short3(1, 2, 3), new short3(2, 3, 4)), Is.EqualTo((short)20), "short3");
            Assert.That(math.dot(new ushort3(1, 2, 3), new ushort3(2, 3, 4)), Is.EqualTo((ushort)20), "ushort3");
            Assert.That(math.dot(new int3(1, 2, 3), new int3(2, 3, 4)), Is.EqualTo(20), "int3");
            Assert.That(math.dot(new uint3(1u, 2u, 3u), new uint3(2u, 3u, 4u)), Is.EqualTo(20u), "uint3");
            Assert.That(math.dot(new long3(1L, 2L, 3L), new long3(2L, 3L, 4L)), Is.EqualTo(20L), "long3");
            Assert.That(math.dot(new ulong3(1UL, 2UL, 3UL), new ulong3(2UL, 3UL, 4UL)), Is.EqualTo(20UL), "ulong3");

            // the member that names the type of a component reaches the same one, the priority of it keeps it in
            // front of the one of a component type when both of them are applicable
            Assert.That(math.dot<float3, float>(new float3(1f, 2f, 3f), new float3(2f, 3f, 4f)), Is.EqualTo(20f),
                "the member that names both types");
            Assert.That(math.dot<int3s, int>(new int3s(1, 2, 3), new int3s(2, 3, 4)), Is.EqualTo(20),
                "the member that names both types");
            Assert.That(math.dot<float3, float>(new float3(1f, 2f, 3f), new float3(2f, 3f, 4f)),
                Is.EqualTo(math.dot(new float3(1f, 2f, 3f), new float3(2f, 3f, 4f))), "both of them are the same");

            // the member of the vector itself
            Assert.That(new float3(1f, 2f, 3f).dot(new float3(2f, 3f, 4f)), Is.EqualTo(20f), "the member of float3");
            Assert.That(new double2(1d, 2d).dot(new double2(2d, 3d)), Is.EqualTo(8d), "the member of double2");
            Assert.That(new int3(1, 2, 3).dot(new int3(2, 3, 4)), Is.EqualTo(20), "the member of int3");
            Assert.That(new half3((half)1f, (half)2f, (half)3f).dot(new half3((half)2f, (half)3f, (half)4f)),
                Is.EqualTo((half)20f), "the member of half3");
        }
    }

    /// <summary>
    /// The dot product of a value that has no register is the sum of the products of every component of it and
    /// the padding lane of a register that is wider than the value is left out of the one of it.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.dot(new float3s(1f, 2f, 3f), new float3s(2f, 3f, 4f)), Is.EqualTo(20f), "float3s");
            Assert.That(math.dot(new double3s(1d, 2d, 3d), new double3s(2d, 3d, 4d)), Is.EqualTo(20d), "double3s");
            Assert.That(math.dot(new half3((half)1f, (half)2f, (half)3f), new half3((half)2f, (half)3f, (half)4f)),
                Is.EqualTo((half)20f), "half3, a value without a register");
            Assert.That(math.dot(new float2s(1f, 2f), new float2s(2f, 3f)), Is.EqualTo(8f), "float2s, a 64 bit register");
            Assert.That(math.dot(new int3(1, 2, 3), new int3(2, 3, 4)), Is.EqualTo(20), "int3, a padding lane");
        }
    }
}
