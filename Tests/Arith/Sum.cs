using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The sum of the components of a value: the type of a single component is only a part of the result of the
/// member that names it, so the compiler cannot infer it from the argument and a call that does not name it
/// reaches the member of the component type of the value instead, which the ex_ classes add to the math class
/// and which the math_ex_ classes add to the value itself.
/// </summary>
public class TestSum
{
    [Test]
    public void Values()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component type of a number vector reaches the member of its own kind
            Assert.That(math.sum(new float2(1f, 2f)), Is.EqualTo(3f), "float2");
            Assert.That(math.sum(new float3(1f, 2f, 3f)), Is.EqualTo(6f), "float3");
            Assert.That(math.sum(new float4(1f, 2f, 3f, 4f)), Is.EqualTo(10f), "float4");
            Assert.That(math.sum(new double2(1d, 2d)), Is.EqualTo(3d), "double2");
            Assert.That(math.sum(new double3(1d, 2d, 3d)), Is.EqualTo(6d), "double3");
            Assert.That(math.sum(new double4(1d, 2d, 3d, 4d)), Is.EqualTo(10d), "double4");
            Assert.That(math.sum(new half3((half)1f, (half)2f, (half)3f)), Is.EqualTo((half)6f), "half3");
            Assert.That(math.sum(new short3(1, 2, 3)), Is.EqualTo((short)6), "short3");
            Assert.That(math.sum(new ushort3(1, 2, 3)), Is.EqualTo((ushort)6), "ushort3");
            Assert.That(math.sum(new int3(1, 2, 3)), Is.EqualTo(6), "int3");
            Assert.That(math.sum(new uint3(1u, 2u, 3u)), Is.EqualTo(6u), "uint3");
            Assert.That(math.sum(new long3(1L, 2L, 3L)), Is.EqualTo(6L), "long3");
            Assert.That(math.sum(new ulong3(1UL, 2UL, 3UL)), Is.EqualTo(6UL), "ulong3");

            // the member that names the type of a component reaches the same value, a call of it spells the
            // scalar type of the value out instead of reaching the member of the scalar type
            Assert.That(math.sum<float3, float>(new float3(1f, 2f, 3f)), Is.EqualTo(6f),
                "the member that names both types");
            Assert.That(math.sum<int3s, int>(new int3s(1, 2, 3)), Is.EqualTo(6), "the member that names both types");
            Assert.That(math.sum<float3, float>(new float3(1f, 2f, 3f)), Is.EqualTo(math.sum(new float3(1f, 2f, 3f))),
                "both of them are the same");

            // the member of the value itself
            Assert.That(new float3(1f, 2f, 3f).sum(), Is.EqualTo(6f), "the member of float3");
            Assert.That(new double2(1d, 2d).sum(), Is.EqualTo(3d), "the member of double2");
            Assert.That(new int3(1, 2, 3).sum(), Is.EqualTo(6), "the member of int3");
            Assert.That(new half3((half)1f, (half)2f, (half)3f).sum(), Is.EqualTo((half)6f), "the member of half3");
        }
    }

    /// <summary>
    /// The sum of a value that has no register is the sum of every component of it and the padding lane of a
    /// register that is wider than the value is left out of the one of it, so the sum of a value whose every
    /// component is negative is negative as well.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.sum(new float3s(1f, 2f, 3f)), Is.EqualTo(6f), "float3s, a value without a register");
            Assert.That(math.sum(new double3s(1d, 2d, 3d)), Is.EqualTo(6d), "double3s, a value without a register");
            Assert.That(math.sum(new int3s(1, 2, 3)), Is.EqualTo(6), "int3s, a value without a register");
            Assert.That(math.sum(new half4((half)1f, (half)2f, (half)3f, (half)4f)), Is.EqualTo((half)10f),
                "half4, a value without a register");
            Assert.That(math.sum(new float2s(1f, 2f)), Is.EqualTo(3f), "float2s, a 64 bit register");
            Assert.That(math.sum(new int2s(1, 2)), Is.EqualTo(3), "int2s, a 64 bit register");

            // the padding lane of a 3 component vector is zero, so the sum of a vector whose every component is
            // negative is the sum of the components instead of zero
            Assert.That(math.sum(new float3(-1f, -2f, -3f)), Is.EqualTo(-6f), "float3, a padding lane");
            Assert.That(math.sum(new double3(-1d, -2d, -3d)), Is.EqualTo(-6d), "double3, a padding lane");
            Assert.That(math.sum(new int3(-1, -2, -3)), Is.EqualTo(-6), "int3, a padding lane");
            Assert.That(math.sum(new long3(-1L, -2L, -3L)), Is.EqualTo(-6L), "long3, a padding lane");
        }
    }

    /// <summary>
    /// The value of a matrix is the one of its columns, so the reduction of it is the one of every column of it
    /// combined with the one of the next column.
    /// </summary>
    [Test]
    public void Matrix()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.sum(new float3x3(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(45f), "every component of float3x3");
            Assert.That(math.sum(new float3x3s(new float3s(1f, 2f, 3f), new float3s(4f, 5f, 6f),
                    new float3s(7f, 8f, 9f))),
                Is.EqualTo(45f), "every component of float3x3s");
            Assert.That(math.sum(new double2x2(new double2(1d, 2d), new double2(3d, 4d))), Is.EqualTo(10d),
                "every component of double2x2");
            Assert.That(math.sum(new int2x2(new int2(1, 2), new int2(3, 4))), Is.EqualTo(10),
                "every component of int2x2");

            // a call that names the type of a component reaches the value of a matrix as well
            Assert.That(math.sum<float3x3, float>(new float3x3(new float3(1f), new float3(2f), new float3(3f))),
                Is.EqualTo(18f), "the member that names both types");
        }
    }
}
