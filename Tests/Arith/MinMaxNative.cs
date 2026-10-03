using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The smaller and the larger of two values, the ones the platform computes itself: they are the values of
/// <see cref="math.min{T}(T, T)"/> and <see cref="math.max{T}(T, T)"/> for every value this class
/// checks, the two of them only differ in the way they handle a nan and a negative zero, which every platform
/// is free to do in a way of its own and which is not checked here.
/// </summary>
public class TestMinMaxNative
{
    [Test]
    public void Values()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component type of a number vector reaches the member of its own kind
            Assert.That(math.min_native(new float2(4f, 1f), new float2(2f, 5f)), Is.EqualTo(new float2(2f, 1f)),
                "float2");
            Assert.That(math.max_native(new float2(4f, 1f), new float2(2f, 5f)), Is.EqualTo(new float2(4f, 5f)),
                "float2");
            Assert.That(math.min_native(new float3(4f, 1f, 3f), new float3(2f, 5f, 0.5f)),
                Is.EqualTo(new float3(2f, 1f, 0.5f)), "float3");
            Assert.That(math.max_native(new float3(4f, 1f, 3f), new float3(2f, 5f, 0.5f)),
                Is.EqualTo(new float3(4f, 5f, 3f)), "float3");
            Assert.That(math.min_native(new float4(4f, 1f, 3f, 2f), new float4(2f, 5f, 0.5f, 1f)),
                Is.EqualTo(new float4(2f, 1f, 0.5f, 1f)), "float4");
            Assert.That(math.max_native(new float4(4f, 1f, 3f, 2f), new float4(2f, 5f, 0.5f, 1f)),
                Is.EqualTo(new float4(4f, 5f, 3f, 2f)), "float4");
            Assert.That(math.min_native(new double3(4d, 1d, 3d), new double3(2d, 5d, 0.5d)),
                Is.EqualTo(new double3(2d, 1d, 0.5d)), "double3");
            Assert.That(math.max_native(new double3(4d, 1d, 3d), new double3(2d, 5d, 0.5d)),
                Is.EqualTo(new double3(4d, 5d, 3d)), "double3");
            Assert.That(math.min_native(new half3((half)4f, (half)1f, (half)3f),
                    new half3((half)2f, (half)5f, (half)0.5f)),
                Is.EqualTo(new half3((half)2f, (half)1f, (half)0.5f)), "half3");
            Assert.That(math.max_native(new half3((half)4f, (half)1f, (half)3f),
                    new half3((half)2f, (half)5f, (half)0.5f)),
                Is.EqualTo(new half3((half)4f, (half)5f, (half)3f)), "half3");
            Assert.That(math.min_native(new short3(4, 1, 3), new short3(2, 5, 0)),
                Is.EqualTo(new short3(2, 1, 0)), "short3");
            Assert.That(math.max_native(new short3(4, 1, 3), new short3(2, 5, 0)),
                Is.EqualTo(new short3(4, 5, 3)), "short3");
            Assert.That(math.min_native(new ushort3(4, 1, 3), new ushort3(2, 5, 0)),
                Is.EqualTo(new ushort3(2, 1, 0)), "ushort3");
            Assert.That(math.min_native(new int3(4, 1, 3), new int3(2, 5, 0)), Is.EqualTo(new int3(2, 1, 0)),
                "int3");
            Assert.That(math.max_native(new int3(4, 1, 3), new int3(2, 5, 0)), Is.EqualTo(new int3(4, 5, 3)),
                "int3");
            Assert.That(math.min_native(new uint3(4u, 1u, 3u), new uint3(2u, 5u, 0u)),
                Is.EqualTo(new uint3(2u, 1u, 0u)), "uint3");
            Assert.That(math.max_native(new uint3(4u, 1u, 3u), new uint3(2u, 5u, 0u)),
                Is.EqualTo(new uint3(4u, 5u, 3u)), "uint3");
            Assert.That(math.min_native(new long3(4L, 1L, 3L), new long3(2L, 5L, 0L)),
                Is.EqualTo(new long3(2L, 1L, 0L)), "long3");
            Assert.That(math.max_native(new long3(4L, 1L, 3L), new long3(2L, 5L, 0L)),
                Is.EqualTo(new long3(4L, 5L, 3L)), "long3");
            Assert.That(math.min_native(new ulong3(4UL, 1UL, 3UL), new ulong3(2UL, 5UL, 0UL)),
                Is.EqualTo(new ulong3(2UL, 1UL, 0UL)), "ulong3");
            Assert.That(math.max_native(new ulong3(4UL, 1UL, 3UL), new ulong3(2UL, 5UL, 0UL)),
                Is.EqualTo(new ulong3(4UL, 5UL, 3UL)), "ulong3");

            // the negative components of a signed vector reach the member of the same kind
            Assert.That(math.min_native(new float3(-4f, 1f, -3f), new float3(-2f, 5f, -1f)),
                Is.EqualTo(new float3(-4f, 1f, -3f)), "float3 of negative components");
            Assert.That(math.max_native(new float3(-4f, 1f, -3f), new float3(-2f, 5f, -1f)),
                Is.EqualTo(new float3(-2f, 5f, -1f)), "float3 of negative components");
            Assert.That(math.min_native(new int3(-4, 1, -3), new int3(-2, 5, -1)),
                Is.EqualTo(new int3(-4, 1, -3)), "int3 of negative components");
            Assert.That(math.max_native(new int3(-4, 1, -3), new int3(-2, 5, -1)),
                Is.EqualTo(new int3(-2, 5, -1)), "int3 of negative components");
            Assert.That(math.min_native(new long3(-4L, 1L, -3L), new long3(-2L, 5L, -1L)),
                Is.EqualTo(new long3(-4L, 1L, -3L)), "long3 of negative components");
            Assert.That(math.max_native(new long3(-4L, 1L, -3L), new long3(-2L, 5L, -1L)),
                Is.EqualTo(new long3(-2L, 5L, -1L)), "long3 of negative components");

            Assert.That(math.min_native(new half4((half)4f, (half)1f, (half)3f, (half)2f),
                    new half4((half)2f, (half)5f, (half)0f, (half)1f)),
                Is.EqualTo(new half4((half)2f, (half)1f, (half)0f, (half)1f)), "half4, a value without a register");
            Assert.That(math.max_native(new half4((half)4f, (half)1f, (half)3f, (half)2f),
                    new half4((half)2f, (half)5f, (half)0f, (half)1f)),
                Is.EqualTo(new half4((half)4f, (half)5f, (half)3f, (half)2f)), "half4, a value without a register");

            // the members that do not name the platform itself reach the same value
            Assert.That(math.min_native(new float3(4f, 1f, 3f), new float3(2f, 5f, 0.5f)),
                Is.EqualTo(math.min(new float3(4f, 1f, 3f), new float3(2f, 5f, 0.5f))), "the minimum of min");
            Assert.That(math.max_native(new float3(4f, 1f, 3f), new float3(2f, 5f, 0.5f)),
                Is.EqualTo(math.max(new float3(4f, 1f, 3f), new float3(2f, 5f, 0.5f))), "the maximum of max");
            Assert.That(math.min_native(new int3(-4, 1, -3), new int3(-2, 5, -1)),
                Is.EqualTo(math.min(new int3(-4, 1, -3), new int3(-2, 5, -1))), "the minimum of min");
            Assert.That(math.max_native(new double2(4d, 1d), new double2(2d, 5d)),
                Is.EqualTo(math.max(new double2(4d, 1d), new double2(2d, 5d))), "the maximum of max");
        }
    }

    /// <summary>
    /// The members reach the value of a matrix as well: the value of every column of it is dispatched the same
    /// way as the value of a vector.
    /// </summary>
    [Test]
    public void Matrix()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.min_native(new float3x2(new float3(4f, 1f, 3f), new float3(2f, 5f, 0f)),
                    new float3x2(new float3(1f, 6f, 2f), new float3(5f, 4f, 7f))),
                Is.EqualTo(new float3x2(new float3(1f, 1f, 2f), new float3(2f, 4f, 0f))), "the columns of float3x2");
            Assert.That(math.max_native(new float3x2(new float3(4f, 1f, 3f), new float3(2f, 5f, 0f)),
                    new float3x2(new float3(1f, 6f, 2f), new float3(5f, 4f, 7f))),
                Is.EqualTo(new float3x2(new float3(4f, 6f, 3f), new float3(5f, 5f, 7f))), "the columns of float3x2");
            Assert.That(math.min_native(new int2x2(new int2(-4, 1), new int2(3, -2)),
                    new int2x2(new int2(-2, 5), new int2(-3, 2))),
                Is.EqualTo(new int2x2(new int2(-4, 1), new int2(-3, -2))), "the columns of int2x2");
            Assert.That(math.max_native(new int2x2(new int2(-4, 1), new int2(3, -2)),
                    new int2x2(new int2(-2, 5), new int2(-3, 2))),
                Is.EqualTo(new int2x2(new int2(-2, 5), new int2(3, 2))), "the columns of int2x2");
        }
    }

    /// <summary>
    /// The members of the value itself.
    /// </summary>
    [Test]
    public void Member()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(4f, 1f, 3f).min_native(new float3(2f, 5f, 0.5f)),
                Is.EqualTo(new float3(2f, 1f, 0.5f)), "the member of float3");
            Assert.That(new float3(4f, 1f, 3f).max_native(new float3(2f, 5f, 0.5f)),
                Is.EqualTo(new float3(4f, 5f, 3f)), "the member of float3");
            Assert.That(new int3(-4, 1, -3).min_native(new int3(-2, 5, -1)), Is.EqualTo(new int3(-4, 1, -3)),
                "the member of int3");
            Assert.That(new float3x2(new float3(4f, 1f, 3f), new float3(2f, 5f, 0f)).min_native(
                    new float3x2(new float3(1f, 6f, 2f), new float3(5f, 4f, 7f))),
                Is.EqualTo(new float3x2(new float3(1f, 1f, 2f), new float3(2f, 4f, 0f))),
                "the member of float3x2");
        }
    }
}
