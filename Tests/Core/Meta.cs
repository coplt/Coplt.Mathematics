using Coplt.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

public class TestVectorMeta
{
    [Test]
    public void LengthSizeAndAcceleration()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That((float2.Length, float2.SizeByte, float2.SizeBit, float2.IsSimdAccelerated), Is.EqualTo((2, 8, 64, true)));
            Assert.That((float3.Length, float3.SizeByte, float3.SizeBit, float3.IsSimdAccelerated), Is.EqualTo((3, 16, 128, true)));
            Assert.That((float4.Length, float4.SizeByte, float4.SizeBit, float4.IsSimdAccelerated), Is.EqualTo((4, 16, 128, true)));

            Assert.That((double2.Length, double2.SizeByte, double2.SizeBit, double2.IsSimdAccelerated), Is.EqualTo((2, 16, 128, true)));
            Assert.That((double3.Length, double3.SizeByte, double3.SizeBit, double3.IsSimdAccelerated), Is.EqualTo((3, 32, 256, true)));
            Assert.That((double4.Length, double4.SizeByte, double4.SizeBit, double4.IsSimdAccelerated), Is.EqualTo((4, 32, 256, true)));

            Assert.That((short2.Length, short2.SizeByte, short2.SizeBit, short2.IsSimdAccelerated), Is.EqualTo((2, 4, 32, false)));
            Assert.That((short3.Length, short3.SizeByte, short3.SizeBit, short3.IsSimdAccelerated), Is.EqualTo((3, 8, 64, false)));
            Assert.That((short4.Length, short4.SizeByte, short4.SizeBit, short4.IsSimdAccelerated), Is.EqualTo((4, 8, 64, false)));

            Assert.That((ushort2.Length, ushort2.SizeByte, ushort2.SizeBit, ushort2.IsSimdAccelerated), Is.EqualTo((2, 4, 32, false)));
            Assert.That((ushort3.Length, ushort3.SizeByte, ushort3.SizeBit, ushort3.IsSimdAccelerated), Is.EqualTo((3, 8, 64, false)));
            Assert.That((ushort4.Length, ushort4.SizeByte, ushort4.SizeBit, ushort4.IsSimdAccelerated), Is.EqualTo((4, 8, 64, false)));

            Assert.That((int2.Length, int2.SizeByte, int2.SizeBit, int2.IsSimdAccelerated), Is.EqualTo((2, 8, 64, true)));
            Assert.That((int3.Length, int3.SizeByte, int3.SizeBit, int3.IsSimdAccelerated), Is.EqualTo((3, 16, 128, true)));
            Assert.That((int4.Length, int4.SizeByte, int4.SizeBit, int4.IsSimdAccelerated), Is.EqualTo((4, 16, 128, true)));

            Assert.That((uint2.Length, uint2.SizeByte, uint2.SizeBit, uint2.IsSimdAccelerated), Is.EqualTo((2, 8, 64, true)));
            Assert.That((uint3.Length, uint3.SizeByte, uint3.SizeBit, uint3.IsSimdAccelerated), Is.EqualTo((3, 16, 128, true)));
            Assert.That((uint4.Length, uint4.SizeByte, uint4.SizeBit, uint4.IsSimdAccelerated), Is.EqualTo((4, 16, 128, true)));

            Assert.That((long2.Length, long2.SizeByte, long2.SizeBit, long2.IsSimdAccelerated), Is.EqualTo((2, 16, 128, true)));
            Assert.That((long3.Length, long3.SizeByte, long3.SizeBit, long3.IsSimdAccelerated), Is.EqualTo((3, 32, 256, true)));
            Assert.That((long4.Length, long4.SizeByte, long4.SizeBit, long4.IsSimdAccelerated), Is.EqualTo((4, 32, 256, true)));

            Assert.That((ulong2.Length, ulong2.SizeByte, ulong2.SizeBit, ulong2.IsSimdAccelerated), Is.EqualTo((2, 16, 128, true)));
            Assert.That((ulong3.Length, ulong3.SizeByte, ulong3.SizeBit, ulong3.IsSimdAccelerated), Is.EqualTo((3, 32, 256, true)));
            Assert.That((ulong4.Length, ulong4.SizeByte, ulong4.SizeBit, ulong4.IsSimdAccelerated), Is.EqualTo((4, 32, 256, true)));

            Assert.That((half2.Length, half2.SizeByte, half2.SizeBit, half2.IsSimdAccelerated), Is.EqualTo((2, 4, 32, false)));
            Assert.That((half3.Length, half3.SizeByte, half3.SizeBit, half3.IsSimdAccelerated), Is.EqualTo((3, 8, 64, false)));
            Assert.That((half4.Length, half4.SizeByte, half4.SizeBit, half4.IsSimdAccelerated), Is.EqualTo((4, 8, 64, false)));

            Assert.That((b16v2.Length, b16v2.SizeByte, b16v2.SizeBit, b16v2.IsSimdAccelerated), Is.EqualTo((2, 4, 32, false)));
            Assert.That((b16v3.Length, b16v3.SizeByte, b16v3.SizeBit, b16v3.IsSimdAccelerated), Is.EqualTo((3, 8, 64, false)));
            Assert.That((b16v4.Length, b16v4.SizeByte, b16v4.SizeBit, b16v4.IsSimdAccelerated), Is.EqualTo((4, 8, 64, false)));

            Assert.That((b32v2.Length, b32v2.SizeByte, b32v2.SizeBit, b32v2.IsSimdAccelerated), Is.EqualTo((2, 8, 64, true)));
            Assert.That((b32v3.Length, b32v3.SizeByte, b32v3.SizeBit, b32v3.IsSimdAccelerated), Is.EqualTo((3, 16, 128, true)));
            Assert.That((b32v4.Length, b32v4.SizeByte, b32v4.SizeBit, b32v4.IsSimdAccelerated), Is.EqualTo((4, 16, 128, true)));

            Assert.That((b64v2.Length, b64v2.SizeByte, b64v2.SizeBit, b64v2.IsSimdAccelerated), Is.EqualTo((2, 16, 128, true)));
            Assert.That((b64v3.Length, b64v3.SizeByte, b64v3.SizeBit, b64v3.IsSimdAccelerated), Is.EqualTo((3, 32, 256, true)));
            Assert.That((b64v4.Length, b64v4.SizeByte, b64v4.SizeBit, b64v4.IsSimdAccelerated), Is.EqualTo((4, 32, 256, true)));
        }
    }

    [Test]
    public void NumericConstants()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component of the broadcast constants
            Assert.That((float3.Zero.x, float3.Zero.y, float3.Zero.z), Is.EqualTo((0f, 0f, 0f)));
            Assert.That((float3.One.x, float3.One.y, float3.One.z), Is.EqualTo((1f, 1f, 1f)));
            Assert.That((float3.Two.x, float3.Two.y, float3.Two.z), Is.EqualTo((2f, 2f, 2f)));
            Assert.That((double4.One.w, double4.Two.w), Is.EqualTo((1d, 2d)));
            Assert.That((int2.One.y, int2.Two.y), Is.EqualTo((1, 2)));
            Assert.That((uint4.One.w, uint4.Two.w), Is.EqualTo((1u, 2u)));
            Assert.That((long3.One.z, long3.Two.z), Is.EqualTo((1L, 2L)));
            Assert.That((ulong2.One.x, ulong2.Two.x), Is.EqualTo((1UL, 2UL)));
            Assert.That(((float)half2.One.y, (float)half2.Two.y), Is.EqualTo((1f, 2f)));
            Assert.That((bool)b32v2.True.y, Is.True);

            // the scalar constants are plain scalars, they are not vectors
            Assert.That((float2.ScalarZero, float2.ScalarOne, float2.ScalarTwo), Is.EqualTo((0f, 1f, 2f)));
            Assert.That((double3.ScalarZero, double3.ScalarOne, double3.ScalarTwo), Is.EqualTo((0d, 1d, 2d)));
            Assert.That((int4.ScalarZero, int4.ScalarOne, int4.ScalarTwo), Is.EqualTo((0, 1, 2)));
            Assert.That((uint2.ScalarZero, uint2.ScalarOne, uint2.ScalarTwo), Is.EqualTo((0u, 1u, 2u)));
            Assert.That((long2.ScalarZero, long2.ScalarOne, long2.ScalarTwo), Is.EqualTo((0L, 1L, 2L)));
            Assert.That((ulong3.ScalarZero, ulong3.ScalarOne, ulong3.ScalarTwo), Is.EqualTo((0UL, 1UL, 2UL)));
            Assert.That((short2.ScalarZero, short2.ScalarOne, short2.ScalarTwo), Is.EqualTo(((short)0, (short)1, (short)2)));
            Assert.That((ushort4.ScalarZero, ushort4.ScalarOne, ushort4.ScalarTwo), Is.EqualTo(((ushort)0, (ushort)1, (ushort)2)));
            Assert.That((half2.ScalarZero, half2.ScalarOne, half2.ScalarTwo), Is.EqualTo(((Half)0, (Half)1, (Half)2)));
            Assert.That((float4.ScalarZero, float4.ScalarOne, float4.ScalarTwo), Is.EqualTo((0f, 1f, 2f)));
        }
    }

    [Test]
    public void BoolConstants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That((b16v2.True.x, b16v2.True.y), Is.EqualTo((B16.True, B16.True)));
            Assert.That((b16v2.False.x, b16v2.False.y), Is.EqualTo((B16.False, B16.False)));
            Assert.That(b16v3.False.Equals(default(b16v3)), Is.True);

            Assert.That((b32v3.True.x, b32v3.True.z), Is.EqualTo((B32.True, B32.True)));
            Assert.That((b32v3.False.x, b32v3.False.z), Is.EqualTo((B32.False, B32.False)));
            Assert.That((b32v4.True.x, b32v4.True.w), Is.EqualTo((B32.True, B32.True)));
            Assert.That(b32v2.True.Equals(b32v2.True), Is.True);
            Assert.That(b32v2.True.Equals(b32v2.False), Is.False);

            Assert.That((b64v2.True.y, b64v2.False.y), Is.EqualTo((B64.True, B64.False)));
            Assert.That((b64v3.True.z, b64v3.False.z), Is.EqualTo((B64.True, B64.False)));
            Assert.That(b64v4.False.Equals(default(b64v4)), Is.True);
            Assert.That((b16v4.True.a, b16v4.False.a), Is.EqualTo((B16.True, B16.False)));
        }
    }
}
