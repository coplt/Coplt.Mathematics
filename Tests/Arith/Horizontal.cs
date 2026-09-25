using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The smallest and the largest component of a value: the type of a single component is only a part of the
/// result of the members that name it, so the compiler cannot infer it from the argument and a call that does
/// not name it reaches the member of the component type of the value instead, which the ex_ classes add to the
/// math class and which the math_ex_ classes add to the value itself.
/// </summary>
public class TestHorizontal
{
    [Test]
    public void Values()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component type of a number vector reaches the member of its own kind
            Assert.That(math.hmin(new float2(3f, 1f)), Is.EqualTo(1f), "float2");
            Assert.That(math.hmax(new float2(3f, 1f)), Is.EqualTo(3f), "float2");
            Assert.That(math.hmin(new float3(3f, 1f, 2f)), Is.EqualTo(1f), "float3");
            Assert.That(math.hmax(new float3(3f, 1f, 2f)), Is.EqualTo(3f), "float3");
            Assert.That(math.hmin(new float4(3f, 1f, 2f, 4f)), Is.EqualTo(1f), "float4");
            Assert.That(math.hmax(new float4(3f, 1f, 2f, 4f)), Is.EqualTo(4f), "float4");
            Assert.That(math.hmin(new double2(3d, 1d)), Is.EqualTo(1d), "double2");
            Assert.That(math.hmax(new double2(3d, 1d)), Is.EqualTo(3d), "double2");
            Assert.That(math.hmin(new double3(3d, 1d, 2d)), Is.EqualTo(1d), "double3");
            Assert.That(math.hmax(new double3(3d, 1d, 2d)), Is.EqualTo(3d), "double3");
            Assert.That(math.hmin(new double4(3d, 1d, 2d, 4d)), Is.EqualTo(1d), "double4");
            Assert.That(math.hmax(new double4(3d, 1d, 2d, 4d)), Is.EqualTo(4d), "double4");
            Assert.That(math.hmin(new half3((half)3f, (half)1f, (half)2f)), Is.EqualTo((half)1f), "half3");
            Assert.That(math.hmax(new half3((half)3f, (half)1f, (half)2f)), Is.EqualTo((half)3f), "half3");
            Assert.That(math.hmin(new short3(3, 1, 2)), Is.EqualTo((short)1), "short3");
            Assert.That(math.hmax(new short3(3, 1, 2)), Is.EqualTo((short)3), "short3");
            Assert.That(math.hmin(new ushort3(3, 1, 2)), Is.EqualTo((ushort)1), "ushort3");
            Assert.That(math.hmax(new ushort3(3, 1, 2)), Is.EqualTo((ushort)3), "ushort3");
            Assert.That(math.hmin(new int3(3, 1, 2)), Is.EqualTo(1), "int3");
            Assert.That(math.hmax(new int3(3, 1, 2)), Is.EqualTo(3), "int3");
            Assert.That(math.hmin(new uint3(3u, 1u, 2u)), Is.EqualTo(1u), "uint3");
            Assert.That(math.hmax(new uint3(3u, 1u, 2u)), Is.EqualTo(3u), "uint3");
            Assert.That(math.hmin(new long3(3L, 1L, 2L)), Is.EqualTo(1L), "long3");
            Assert.That(math.hmax(new long3(3L, 1L, 2L)), Is.EqualTo(3L), "long3");
            Assert.That(math.hmin(new ulong3(3UL, 1UL, 2UL)), Is.EqualTo(1UL), "ulong3");
            Assert.That(math.hmax(new ulong3(3UL, 1UL, 2UL)), Is.EqualTo(3UL), "ulong3");

            // the negative components of a signed vector reach the member of the same kind
            Assert.That(math.hmin(new float3(-3f, -1f, -2f)), Is.EqualTo(-3f), "float3 of negative components");
            Assert.That(math.hmax(new float3(-3f, -1f, -2f)), Is.EqualTo(-1f), "float3 of negative components");
            Assert.That(math.hmin(new int3(-3, -1, -2)), Is.EqualTo(-3), "int3 of negative components");
            Assert.That(math.hmax(new int3(-3, -1, -2)), Is.EqualTo(-1), "int3 of negative components");

            // a vector whose register is 64 bits wide and one that has no register reach the member of their own
            // kind
            Assert.That(math.hmin(new float2s(3f, 1f)), Is.EqualTo(1f), "float2s, a 64 bit register");
            Assert.That(math.hmax(new float2s(3f, 1f)), Is.EqualTo(3f), "float2s, a 64 bit register");
            Assert.That(math.hmin(new int2s(3, 1)), Is.EqualTo(1), "int2s, a 64 bit register");
            Assert.That(math.hmax(new int2s(3, 1)), Is.EqualTo(3), "int2s, a 64 bit register");
            Assert.That(math.hmin(new float3s(3f, 1f, 2f)), Is.EqualTo(1f), "float3s, a value without a register");
            Assert.That(math.hmax(new float3s(3f, 1f, 2f)), Is.EqualTo(3f), "float3s, a value without a register");
            Assert.That(math.hmin(new double3s(3d, 1d, 2d)), Is.EqualTo(1d), "double3s, a value without a register");
            Assert.That(math.hmax(new double3s(3d, 1d, 2d)), Is.EqualTo(3d), "double3s, a value without a register");
            Assert.That(math.hmin(new int3s(3, 1, 2)), Is.EqualTo(1), "int3s, a value without a register");
            Assert.That(math.hmax(new int3s(3, 1, 2)), Is.EqualTo(3), "int3s, a value without a register");
            Assert.That(math.hmin(new long3s(3L, 1L, 2L)), Is.EqualTo(1L), "long3s, a value without a register");
            Assert.That(math.hmin(new half4((half)3f, (half)1f, (half)2f, (half)4f)), Is.EqualTo((half)1f),
                "half4, a value without a register");
            Assert.That(math.hmax(new half4((half)3f, (half)1f, (half)2f, (half)4f)), Is.EqualTo((half)4f),
                "half4, a value without a register");

            // the member that names the type of a component reaches the same value, a call of it spells the
            // scalar type of the value out instead of reaching the member of the scalar type
            Assert.That(math.hmin<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(1f),
                "the member that names both types");
            Assert.That(math.hmax<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(3f),
                "the member that names both types");
            Assert.That(math.hmin<int3s, int>(new int3s(3, 1, 2)), Is.EqualTo(1), "the member that names both types");
            Assert.That(math.hmin<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(math.hmin(new float3(3f, 1f, 2f))),
                "both of them are the same");

            // the member of the value itself
            Assert.That(new float3(3f, 1f, 2f).hmin(), Is.EqualTo(1f), "the member of float3");
            Assert.That(new float3(3f, 1f, 2f).hmax(), Is.EqualTo(3f), "the member of float3");
            Assert.That(new double2(3d, 1d).hmin(), Is.EqualTo(1d), "the member of double2");
            Assert.That(new int3(3, 1, 2).hmin(), Is.EqualTo(1), "the member of int3");
            Assert.That(new int3(3, 1, 2).hmax(), Is.EqualTo(3), "the member of int3");
            Assert.That(new half3((half)3f, (half)1f, (half)2f).hmin(), Is.EqualTo((half)1f), "the member of half3");
        }
    }

    /// <summary>
    /// The padding lane of a register that is wider than the value it holds is zero, so the reduction of a value
    /// whose components are all positive would return zero instead of the smallest one of them and the one of a
    /// value whose components are all negative would return zero instead of the largest one of them when the
    /// padding lane is not left out.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.hmin(new float3(3f, 1f, 2f)), Is.EqualTo(1f), "float3, a padding lane");
            Assert.That(math.hmin(new float4(3f, 1f, 2f, 4f)), Is.EqualTo(1f), "float4, no padding lane");
            Assert.That(math.hmax(new float3(-3f, -1f, -2f)), Is.EqualTo(-1f), "float3, a padding lane");
            Assert.That(math.hmax(new float4(-3f, -1f, -2f, -4f)), Is.EqualTo(-1f), "float4, no padding lane");
            Assert.That(math.hmin(new double3(3d, 1d, 2d)), Is.EqualTo(1d), "double3, a padding lane");
            Assert.That(math.hmax(new double3(-3d, -1d, -2d)), Is.EqualTo(-1d), "double3, a padding lane");
            Assert.That(math.hmin(new int3(3, 1, 2)), Is.EqualTo(1), "int3, a padding lane");
            Assert.That(math.hmax(new int3(-3, -1, -2)), Is.EqualTo(-1), "int3, a padding lane");
            Assert.That(math.hmin(new long3(3L, 1L, 2L)), Is.EqualTo(1L), "long3, a padding lane");
            Assert.That(math.hmax(new long3(-3L, -1L, -2L)), Is.EqualTo(-1L), "long3, a padding lane");
            // a value without a register has no padding lane at all
            Assert.That(math.hmin(new float3s(3f, 1f, 2f)), Is.EqualTo(1f), "float3s, no padding lane");
            Assert.That(math.hmax(new float3s(-3f, -1f, -2f)), Is.EqualTo(-1f), "float3s, no padding lane");
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
            Assert.That(math.hmin(new float3x3(new float3(4f, 1f, 3f), new float3(2f, 6f, 5f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(1f), "every component of float3x3");
            Assert.That(math.hmax(new float3x3(new float3(4f, 1f, 3f), new float3(2f, 6f, 5f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(9f), "every component of float3x3");
            Assert.That(math.hmin(new float3x3s(new float3s(4f, 1f, 3f), new float3s(2f, 6f, 5f),
                    new float3s(7f, 8f, 9f))),
                Is.EqualTo(1f), "every component of float3x3s");
            Assert.That(math.hmin(new double2x2(new double2(4d, 1d), new double2(2d, 3d))), Is.EqualTo(1d),
                "every component of double2x2");
            Assert.That(math.hmax(new double2x2(new double2(4d, 1d), new double2(2d, 3d))), Is.EqualTo(4d),
                "every component of double2x2");
            Assert.That(math.hmin(new int2x2(new int2(4, 1), new int2(2, 3))), Is.EqualTo(1),
                "every component of int2x2");
            Assert.That(math.hmax(new int2x2(new int2(4, 1), new int2(2, 3))), Is.EqualTo(4),
                "every component of int2x2");

            // a call that names the type of a component reaches the value of a matrix as well
            Assert.That(math.hmin<float3x3, float>(new float3x3(new float3(4f), new float3(2f), new float3(7f))),
                Is.EqualTo(2f), "the member that names both types");
            Assert.That(math.hmax<float3x3, float>(new float3x3(new float3(4f), new float3(2f), new float3(7f))),
                Is.EqualTo(7f), "the member that names both types");
        }
    }

    /// <summary>
    /// The native members reduce the components of a value with the minimum and the maximum the platform
    /// computes itself, which is the one of <see cref="math.hmin{T,TScalar}(in T)"/> and
    /// <see cref="math.hmax{T,TScalar}(in T)"/> for a value that is neither a nan nor a zero of a sign of its
    /// own, so every value that is checked here is the one of the member above it.
    /// </summary>
    [Test]
    public void Native()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component type of a number vector, the width of the register of the value picks the member
            // of the visitor
            Assert.That(math.hmin_native(new float2(3f, 1f)), Is.EqualTo(math.hmin(new float2(3f, 1f))), "float2");
            Assert.That(math.hmax_native(new float2(3f, 1f)), Is.EqualTo(math.hmax(new float2(3f, 1f))), "float2");
            Assert.That(math.hmin_native(new float3(3f, 1f, 2f)), Is.EqualTo(math.hmin(new float3(3f, 1f, 2f))),
                "float3");
            Assert.That(math.hmax_native(new float3(3f, 1f, 2f)), Is.EqualTo(math.hmax(new float3(3f, 1f, 2f))),
                "float3");
            Assert.That(math.hmin_native(new float4(3f, 1f, 2f, 4f)), Is.EqualTo(math.hmin(new float4(3f, 1f, 2f, 4f))),
                "float4");
            Assert.That(math.hmax_native(new float4(3f, 1f, 2f, 4f)), Is.EqualTo(math.hmax(new float4(3f, 1f, 2f, 4f))),
                "float4");
            Assert.That(math.hmin_native(new double3(3d, 1d, 2d)), Is.EqualTo(math.hmin(new double3(3d, 1d, 2d))),
                "double3");
            Assert.That(math.hmax_native(new double4(3d, 1d, 2d, 4d)),
                Is.EqualTo(math.hmax(new double4(3d, 1d, 2d, 4d))), "double4");
            Assert.That(math.hmin_native(new half3((half)3f, (half)1f, (half)2f)),
                Is.EqualTo(math.hmin(new half3((half)3f, (half)1f, (half)2f))), "half3");
            Assert.That(math.hmax_native(new half3((half)3f, (half)1f, (half)2f)),
                Is.EqualTo(math.hmax(new half3((half)3f, (half)1f, (half)2f))), "half3");
            Assert.That(math.hmin_native(new short3(3, 1, 2)), Is.EqualTo(math.hmin(new short3(3, 1, 2))), "short3");
            Assert.That(math.hmax_native(new ushort3(3, 1, 2)), Is.EqualTo(math.hmax(new ushort3(3, 1, 2))),
                "ushort3");
            Assert.That(math.hmin_native(new int3(3, 1, 2)), Is.EqualTo(math.hmin(new int3(3, 1, 2))), "int3");
            Assert.That(math.hmax_native(new uint3(3u, 1u, 2u)), Is.EqualTo(math.hmax(new uint3(3u, 1u, 2u))),
                "uint3");
            Assert.That(math.hmin_native(new long3(3L, 1L, 2L)), Is.EqualTo(math.hmin(new long3(3L, 1L, 2L))), "long3");
            Assert.That(math.hmax_native(new ulong3(3UL, 1UL, 2UL)), Is.EqualTo(math.hmax(new ulong3(3UL, 1UL, 2UL))),
                "ulong3");

            // a vector whose register is 64 bits wide and one that has no register reach the member of their own
            // kind
            Assert.That(math.hmin_native(new float2s(3f, 1f)), Is.EqualTo(math.hmin(new float2s(3f, 1f))),
                "float2s, a 64 bit register");
            Assert.That(math.hmax_native(new int2s(3, 1)), Is.EqualTo(math.hmax(new int2s(3, 1))),
                "int2s, a 64 bit register");
            Assert.That(math.hmin_native(new float3s(3f, 1f, 2f)), Is.EqualTo(math.hmin(new float3s(3f, 1f, 2f))),
                "float3s, a value without a register");
            Assert.That(math.hmax_native(new float3s(3f, 1f, 2f)), Is.EqualTo(math.hmax(new float3s(3f, 1f, 2f))),
                "float3s, a value without a register");
            Assert.That(math.hmin_native(new double3s(3d, 1d, 2d)), Is.EqualTo(math.hmin(new double3s(3d, 1d, 2d))),
                "double3s, a value without a register");
            Assert.That(math.hmin_native(new int3s(3, 1, 2)), Is.EqualTo(math.hmin(new int3s(3, 1, 2))),
                "int3s, a value without a register");
            Assert.That(math.hmax_native(new long3s(3L, 1L, 2L)), Is.EqualTo(math.hmax(new long3s(3L, 1L, 2L))),
                "long3s, a value without a register");
            Assert.That(math.hmin_native(new half4((half)3f, (half)1f, (half)2f, (half)4f)),
                Is.EqualTo(math.hmin(new half4((half)3f, (half)1f, (half)2f, (half)4f))),
                "half4, a value without a register");

            // the padding lane of a value of 3 components is zero, a reduction that picks it up would return
            // zero instead of the smallest or the largest component
            Assert.That(math.hmin_native(new float3(3f, 1f, 2f)), Is.EqualTo(1f), "float3, a padding lane");
            Assert.That(math.hmax_native(new float3(-3f, -1f, -2f)), Is.EqualTo(-1f), "float3, a padding lane");
            Assert.That(math.hmin_native(new double3(3d, 1d, 2d)), Is.EqualTo(1d), "double3, a padding lane");
            Assert.That(math.hmax_native(new double3(-3d, -1d, -2d)), Is.EqualTo(-1d), "double3, a padding lane");
            Assert.That(math.hmin_native(new int3(3, 1, 2)), Is.EqualTo(1), "int3, a padding lane");
            Assert.That(math.hmax_native(new int3(-3, -1, -2)), Is.EqualTo(-1), "int3, a padding lane");
            Assert.That(math.hmin_native(new long3(3L, 1L, 2L)), Is.EqualTo(1L), "long3, a padding lane");
            Assert.That(math.hmax_native(new long3(-3L, -1L, -2L)), Is.EqualTo(-1L), "long3, a padding lane");

            // the member that names the type of a component and the member of the value itself
            Assert.That(math.hmin_native<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(1f),
                "the member that names both types");
            Assert.That(math.hmax_native<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(3f),
                "the member that names both types");
            Assert.That(new float3(3f, 1f, 2f).hmin_native(), Is.EqualTo(1f), "the member of float3");
            Assert.That(new float3(3f, 1f, 2f).hmax_native(), Is.EqualTo(3f), "the member of float3");
            Assert.That(new int3(3, 1, 2).hmin_native(), Is.EqualTo(1), "the member of int3");
            Assert.That(new float3s(3f, 1f, 2f).hmax_native(), Is.EqualTo(3f), "the member of float3s");

            // the value of a matrix is the one of its columns
            Assert.That(math.hmin_native(new float3x3(new float3(4f, 1f, 3f), new float3(2f, 6f, 5f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(1f), "every component of float3x3");
            Assert.That(math.hmax_native(new float3x3(new float3(4f, 1f, 3f), new float3(2f, 6f, 5f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(9f), "every component of float3x3");
            Assert.That(math.hmin_native(new float3x3s(new float3s(4f, 1f, 3f), new float3s(2f, 6f, 5f),
                    new float3s(7f, 8f, 9f))),
                Is.EqualTo(1f), "every component of float3x3s");
            Assert.That(math.hmax_native(new float3x3s(new float3s(4f, 1f, 3f), new float3s(2f, 6f, 5f),
                    new float3s(7f, 8f, 9f))),
                Is.EqualTo(9f), "every component of float3x3s");
            Assert.That(math.hmin_native(new double2x2(new double2(4d, 1d), new double2(2d, 3d))), Is.EqualTo(1d),
                "every component of double2x2");
            Assert.That(math.hmax_native(new double2x2(new double2(4d, 1d), new double2(2d, 3d))), Is.EqualTo(4d),
                "every component of double2x2");
            Assert.That(math.hmin_native(new int2x2(new int2(4, 1), new int2(2, 3))), Is.EqualTo(1),
                "every component of int2x2");
            Assert.That(math.hmax_native(new int2x2(new int2(4, 1), new int2(2, 3))), Is.EqualTo(4),
                "every component of int2x2");
            Assert.That(math.hmax_native<float3x3, float>(new float3x3(new float3(4f), new float3(2f), new float3(7f))),
                Is.EqualTo(7f), "the member that names both types");
        }
    }
}
