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
            Assert.That((float3.Three.x, float3.Three.y, float3.Three.z), Is.EqualTo((3f, 3f, 3f)));
            Assert.That((double4.One.w, double4.Two.w, double4.Three.w), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((int2.One.y, int2.Two.y, int2.Three.y), Is.EqualTo((1, 2, 3)));
            Assert.That((uint4.One.w, uint4.Two.w, uint4.Three.w), Is.EqualTo((1u, 2u, 3u)));
            Assert.That((long3.One.z, long3.Two.z, long3.Three.z), Is.EqualTo((1L, 2L, 3L)));
            Assert.That((ulong2.One.x, ulong2.Two.x, ulong2.Three.x), Is.EqualTo((1UL, 2UL, 3UL)));
            Assert.That(((float)half2.One.y, (float)half2.Two.y, (float)half2.Three.y), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((bool)b32v2.True.y, Is.True);

            // the scalar constants are plain scalars, they are not vectors
            Assert.That((float2.ScalarZero, float2.ScalarOne, float2.ScalarTwo, float2.ScalarThree),
                Is.EqualTo((0f, 1f, 2f, 3f)));
            Assert.That((double3.ScalarZero, double3.ScalarOne, double3.ScalarTwo, double3.ScalarThree),
                Is.EqualTo((0d, 1d, 2d, 3d)));
            Assert.That((int4.ScalarZero, int4.ScalarOne, int4.ScalarTwo, int4.ScalarThree), Is.EqualTo((0, 1, 2, 3)));
            Assert.That((uint2.ScalarZero, uint2.ScalarOne, uint2.ScalarTwo, uint2.ScalarThree),
                Is.EqualTo((0u, 1u, 2u, 3u)));
            Assert.That((long2.ScalarZero, long2.ScalarOne, long2.ScalarTwo, long2.ScalarThree),
                Is.EqualTo((0L, 1L, 2L, 3L)));
            Assert.That((ulong3.ScalarZero, ulong3.ScalarOne, ulong3.ScalarTwo, ulong3.ScalarThree),
                Is.EqualTo((0UL, 1UL, 2UL, 3UL)));
            Assert.That((short2.ScalarZero, short2.ScalarOne, short2.ScalarTwo, short2.ScalarThree),
                Is.EqualTo(((short)0, (short)1, (short)2, (short)3)));
            Assert.That((ushort4.ScalarZero, ushort4.ScalarOne, ushort4.ScalarTwo, ushort4.ScalarThree),
                Is.EqualTo(((ushort)0, (ushort)1, (ushort)2, (ushort)3)));
            Assert.That((half2.ScalarZero, half2.ScalarOne, half2.ScalarTwo, half2.ScalarThree),
                Is.EqualTo(((Half)0, (Half)1, (Half)2, (Half)3)));
            Assert.That((float4.ScalarZero, float4.ScalarOne, float4.ScalarTwo, float4.ScalarThree),
                Is.EqualTo((0f, 1f, 2f, 3f)));

            // every whole number up to ten is a constant of the value beside the one of a single component of it
            Assert.That((float3.Four.x, float3.Five.y, float3.Six.z, float3.Ten.x), Is.EqualTo((4f, 5f, 6f, 10f)));
            Assert.That((double2.Seven.x, double2.Eight.y, double2.Nine.x, double2.Ten.y),
                Is.EqualTo((7d, 8d, 9d, 10d)));
            Assert.That((int4.Four.w, int4.Ten.w), Is.EqualTo((4, 10)));
            Assert.That((uint2.Five.x, uint2.Ten.x), Is.EqualTo((5u, 10u)));
            Assert.That((long3.Six.z, long3.Ten.z), Is.EqualTo((6L, 10L)));
            Assert.That((ulong2.Seven.x, ulong2.Ten.x), Is.EqualTo((7UL, 10UL)));
            Assert.That(((float)half2.Eight.y, (float)half2.Ten.y), Is.EqualTo((8f, 10f)));
            Assert.That((short2.Nine.x, short2.Ten.x), Is.EqualTo(((short)9, (short)10)));
            Assert.That((ushort4.Four.x, ushort4.Ten.x), Is.EqualTo(((ushort)4, (ushort)10)));

            Assert.That((float2.ScalarFour, float2.ScalarTen), Is.EqualTo((4f, 10f)));
            Assert.That((double3.ScalarFive, double3.ScalarTen), Is.EqualTo((5d, 10d)));
            Assert.That((int4.ScalarSix, int4.ScalarTen), Is.EqualTo((6, 10)));
            Assert.That((uint2.ScalarSeven, uint2.ScalarTen), Is.EqualTo((7u, 10u)));
            Assert.That((long2.ScalarEight, long2.ScalarTen), Is.EqualTo((8L, 10L)));
            Assert.That((ulong3.ScalarNine, ulong3.ScalarTen), Is.EqualTo((9UL, 10UL)));
            Assert.That((short2.ScalarNine, short2.ScalarTen), Is.EqualTo(((short)9, (short)10)));
            Assert.That((half2.ScalarEight, half2.ScalarTen), Is.EqualTo(((Half)8, (Half)10)));

            // a signed value reaches the negative of every whole number beside the zero as well
            Assert.That((float3.NegativeOne.x, float3.NegativeTwo.y, float3.NegativeTen.z),
                Is.EqualTo((-1f, -2f, -10f)));
            Assert.That((double2.NegativeThree.x, double2.NegativeTen.y), Is.EqualTo((-3d, -10d)));
            Assert.That((int4.NegativeFour.w, int4.NegativeTen.w), Is.EqualTo((-4, -10)));
            Assert.That((long3.NegativeFive.z, long3.NegativeTen.z), Is.EqualTo((-5L, -10L)));
            Assert.That(((float)half2.NegativeSix.y, (float)half2.NegativeTen.y), Is.EqualTo((-6f, -10f)));
            Assert.That((short2.NegativeSeven.x, short2.NegativeTen.x), Is.EqualTo(((short)(-7), (short)(-10))));

            Assert.That((float4.ScalarNegativeOne, float4.ScalarNegativeTen), Is.EqualTo((-1f, -10f)));
            Assert.That((double3.ScalarNegativeTwo, double3.ScalarNegativeTen), Is.EqualTo((-2d, -10d)));
            Assert.That((int2.ScalarNegativeThree, int2.ScalarNegativeTen), Is.EqualTo((-3, -10)));
            Assert.That((long2.ScalarNegativeFour, long2.ScalarNegativeTen), Is.EqualTo((-4L, -10L)));
            Assert.That((half2.ScalarNegativeFive, half2.ScalarNegativeTen),
                Is.EqualTo(((Half)(-5), (Half)(-10))));
            Assert.That((short2.ScalarNegativeSix, short2.ScalarNegativeTen),
                Is.EqualTo(((short)(-6), (short)(-10))));
        }
    }

    /// <summary>
    /// A matrix reaches the whole numbers of the algebra like a vector does, beside the ones of the vector a
    /// column of it is.
    /// </summary>
    [Test]
    public void MatrixConstants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That((float3x2.Four.c0.x, float3x2.Ten.c1.z), Is.EqualTo((4f, 10f)));
            Assert.That((float3x2.ScalarFour, float3x2.ScalarTen), Is.EqualTo((4f, 10f)));
            Assert.That((double2x2.Three.c1.y, double2x2.ScalarTen), Is.EqualTo((3d, 10d)));
            Assert.That((int3x3.Five.c2.z, int3x3.ScalarTen), Is.EqualTo((5, 10)));
            Assert.That((half2x2.Six.c0.x, half2x2.ScalarTen), Is.EqualTo(((Half)6, (Half)10)));

            // a signed matrix reaches the negative of every whole number beside the zero as well
            Assert.That((float3x2.NegativeThree.c0.x, float3x2.NegativeTen.c1.z), Is.EqualTo((-3f, -10f)));
            Assert.That((float3x2.ScalarNegativeFour, float3x2.ScalarNegativeTen), Is.EqualTo((-4f, -10f)));
            Assert.That((double2x2.NegativeOne.c0.x, double2x2.ScalarNegativeTen), Is.EqualTo((-1d, -10d)));
            Assert.That((int3x3.NegativeSeven.c0.x, int3x3.ScalarNegativeTen), Is.EqualTo((-7, -10)));

            // every whole number of the vector a column of a matrix is
            Assert.That(float3x2.VectorZero, Is.EqualTo(default(float3)));
            Assert.That(float3x2.VectorOne, Is.EqualTo(new float3(1f)));
            Assert.That(float3x2.VectorTen, Is.EqualTo(new float3(10f)));
            Assert.That(double4.VectorSix, Is.EqualTo(new double4(6d)));
            Assert.That(int2s.VectorNine, Is.EqualTo(new int2s(9, 9)));

            // a signed value reaches the negative of every one of them as well
            Assert.That(float3x2.VectorNegativeThree, Is.EqualTo(new float3(-3f)));
            Assert.That(float3x2.VectorNegativeTen, Is.EqualTo(new float3(-10f)));
            Assert.That(half2x2.VectorNegativeSix, Is.EqualTo(new half2((Half)(-6), (Half)(-6))));
            Assert.That(int3x3.VectorNegativeTen, Is.EqualTo(new int3(-10, -10, -10)));
            Assert.That(float3.VectorNegativeTen, Is.EqualTo(new float3(-10f)));
            Assert.That(double2.VectorNegativeTwo, Is.EqualTo(new double2(-2d, -2d)));
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
