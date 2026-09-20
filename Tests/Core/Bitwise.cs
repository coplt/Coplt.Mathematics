using System.Runtime.Intrinsics;
using Coplt.Experimental.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

public class TestVectorBitwise
{
    [Test]
    public void Bitwise()
    {
        var i4 = new int4(0b1100, 0b1010, 1, 2);
        var i4b = new int4(0b1010, 0b0110, 3, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(i4 & i4b, Is.EqualTo(new int4(0b1000, 0b0010, 1, 2)));
            Assert.That(i4 | i4b, Is.EqualTo(new int4(0b1110, 0b1110, 3, 2)));
            Assert.That(i4 ^ i4b, Is.EqualTo(new int4(0b0110, 0b1100, 2, 0)));
            Assert.That(~i4, Is.EqualTo(new int4(~0b1100, ~0b1010, ~1, ~2)));
        }

        var i2 = new int2(0xF0, 0x0F);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(i2 & i2, Is.EqualTo(i2));
            Assert.That(i2 | i2, Is.EqualTo(i2));
            Assert.That(i2 ^ i2, Is.EqualTo(default(int2)));
            Assert.That(i2 & default(int2), Is.EqualTo(default(int2)));
            Assert.That(i2 | default(int2), Is.EqualTo(i2));
        }

        var u4 = new uint4(0xFFFF_0000, 0x0000_FFFF, 1, 2);
        var u4b = new uint4(0x0F0F_0F0F, 0xF0F0_F0F0, 3, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(u4 & u4b, Is.EqualTo(new uint4(0x0F0F_0000, 0x0000_F0F0, 1, 2)));
            Assert.That(u4 | u4b, Is.EqualTo(new uint4(0xFFFF_0F0F, 0xF0F0_FFFF, 3, 2)));
            Assert.That(u4 ^ u4b, Is.EqualTo(new uint4(0xF0F0_0F0F, 0xF0F0_0F0F, 2, 0)));
            Assert.That(~u4.y, Is.EqualTo(0xFFFF_0000u));
        }

        var l4 = new long4(1, -1, 0x0000_0000_FFFF_FFFFL, 2);
        var l4b = new long4(1, 0, -0x1_0000_0000L, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(l4 & l4b, Is.EqualTo(new long4(1, 0, 0, 2)));
            Assert.That(l4 | l4b, Is.EqualTo(new long4(1, -1, -1, 3)));
            Assert.That((l4 ^ l4b).x, Is.EqualTo(0L));
            Assert.That((~l4).y, Is.EqualTo(0L));
        }

        var ul3 = new ulong3(0xFFFF_FFFF_0000_0000UL, 0, 1);
        var ul3b = new ulong3(0x0F0F_0F0F_0F0F_0F0FUL, ~0UL, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((ul3 & ul3b).x, Is.EqualTo(0x0F0F_0F0F_0000_0000UL));
            Assert.That((ul3 | ul3b).x, Is.EqualTo(0xFFFF_FFFF_0F0F_0F0FUL));
            Assert.That((ul3 & ul3b).y, Is.EqualTo(0UL));
        }

        var s2 = new short2(0b1100, 0b1010);
        var s2b = new short2(0b1010, 0b0110);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(s2 & s2b, Is.EqualTo(new short2(0b1000, 0b0010)));
            Assert.That(s2 | s2b, Is.EqualTo(new short2(0b1110, 0b1110)));
            Assert.That(s2 ^ s2b, Is.EqualTo(new short2(0b0110, 0b1100)));
            Assert.That(~s2, Is.EqualTo(new short2(~0b1100, ~0b1010)));
        }

        var s3 = new short3(1, 2, 3);
        var s3b = new short3(3, 2, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(s3 & s3b, Is.EqualTo(new short3(1, 2, 1)));
            Assert.That(s3 ^ s3b, Is.EqualTo(new short3(2, 0, 2)));
        }

        var us4 = new ushort4(0xFF00, 0x00FF, 1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(us4 & us4, Is.EqualTo(us4));
            Assert.That((~us4).y, Is.EqualTo((ushort)0xFF00));
            Assert.That(us4 ^ us4, Is.EqualTo(default(ushort4)));
        }

        var t3 = new b32v3(B32.True, B32.True, B32.True);
        var t3b = new b32v3(B32.True, B32.False, B32.True);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(t3 & t3b, Is.EqualTo(t3b));
            Assert.That(t3 | t3b, Is.EqualTo(t3));
            Assert.That(t3 ^ t3b, Is.EqualTo(new b32v3(B32.False, B32.True, B32.False)));
            Assert.That(~t3, Is.EqualTo(new b32v3(B32.False, B32.False, B32.False)));
            Assert.That(~t3b, Is.EqualTo(new b32v3(B32.False, B32.True, B32.False)));
        }

        var e2 = new b16v2(B16.True, B16.False);
        var e2b = new b16v2(B16.True, B16.True);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(e2 & e2b, Is.EqualTo(e2));
            Assert.That(e2 | e2b, Is.EqualTo(e2b));
            Assert.That((bool)(~e2).y, Is.True);
        }

        var q4 = new b64v4(B64.True, B64.False, B64.True, B64.False);
        var q4b = new b64v4(B64.True, B64.True, B64.False, B64.False);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(q4 & q4b, Is.EqualTo(new b64v4(B64.True, B64.False, B64.False, B64.False)));
            Assert.That(q4 | q4b, Is.EqualTo(new b64v4(B64.True, B64.True, B64.True, B64.False)));
            Assert.That(q4 ^ q4b, Is.EqualTo(new b64v4(B64.False, B64.True, B64.True, B64.False)));
        }
    }

    [Test]
    public void BitwiseOfFloatingPointIsBitLevel()
    {
        static uint F(float x) => BitConverter.SingleToUInt32Bits(x);
        static ulong D(double x) => BitConverter.DoubleToUInt64Bits(x);
        static ushort H(Half x) => BitConverter.HalfToUInt16Bits(x);

        var f2 = new float2(1, 2);
        var f2b = new float2(3, 4);
        var f2And = f2 & f2b;
        var f2Or = f2 | f2b;
        var f2Xor = f2 ^ f2b;
        var f2Not = ~f2;
        using (Assert.EnterMultipleScope())
        {
            // the result of a bitwise operator can be NaN, so the bits are compared instead of the values
            Assert.That((F(f2And.x), F(f2And.y)), Is.EqualTo((F(1f) & F(3f), F(2f) & F(4f))));
            Assert.That((F(f2Or.x), F(f2Or.y)), Is.EqualTo((F(1f) | F(3f), F(2f) | F(4f))));
            Assert.That((F(f2Xor.x), F(f2Xor.y)), Is.EqualTo((F(1f) ^ F(3f), F(2f) ^ F(4f))));
            Assert.That((F(f2Not.x), F(f2Not.y)), Is.EqualTo((~F(1f), ~F(2f))));
            // the masked lane of the widened path is not part of the vector
            Assert.That(F(f2Not.y), Is.EqualTo(~F(2f)));
        }

        var f3 = new float3(1, 2, 3);
        var f4 = new float4(1, 2, 3, 4);
        var f3Xor = f3 ^ f3;
        var f4Not = ~f4;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(f3Xor, Is.EqualTo(default(float3)));
            Assert.That((F(f4Not.x), F(f4Not.y), F(f4Not.z), F(f4Not.w)), Is.EqualTo((~F(1f), ~F(2f), ~F(3f), ~F(4f))));
        }

        var d2 = new double2(1, 2);
        var d2b = new double2(3, 4);
        var d2And = d2 & d2b;
        var d2Not = ~d2;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(D(d2And.x), Is.EqualTo(D(1d) & D(3d)));
            Assert.That(D(d2And.y), Is.EqualTo(D(2d) & D(4d)));
            Assert.That((D(d2Not.x), D(d2Not.y)), Is.EqualTo((~D(1d), ~D(2d))));
        }

        var d3 = new double3(1, 2, 3);
        var d3Or = d3 | d3;
        Assert.That((D(d3Or.x), D(d3Or.y), D(d3Or.z)), Is.EqualTo((D(1d), D(2d), D(3d))));

        var h2 = new half2((Half)1, (Half)2);
        var h2b = new half2((Half)3, (Half)4);
        var h2And = h2 & h2b;
        var h2Or = h2 | h2b;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(H(h2And.x), Is.EqualTo((ushort)(H((Half)1) & H((Half)3))));
            Assert.That(H(h2And.y), Is.EqualTo((ushort)(H((Half)2) & H((Half)4))));
            Assert.That(H(h2Or.x), Is.EqualTo((ushort)(H((Half)1) | H((Half)3))));
        }
    }

    [Test]
    public void Shifts()
    {
        var i4 = new int4(1, 2, 3, 4);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(i4 << 1, Is.EqualTo(new int4(2, 4, 6, 8)));
            Assert.That(i4 << 2, Is.EqualTo(new int4(4, 8, 12, 16)));
            Assert.That(i4 >> 1, Is.EqualTo(new int4(0, 1, 1, 2)));
            Assert.That(i4 >>> 1, Is.EqualTo(new int4(0, 1, 1, 2)));
        }

        var i3 = new int3(1, 2, 3);
        Assert.That(i3 << 3, Is.EqualTo(new int3(8, 16, 24)));

        // the arithmetic shift keeps the sign, the logical shift does not
        var i2 = new int2(-8, -1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(i2 >> 1, Is.EqualTo(new int2(-4, -1)));
            Assert.That(i2 >>> 1, Is.EqualTo(new int2(int.MaxValue - 3, int.MaxValue)));
            Assert.That(i2 << 1, Is.EqualTo(new int2(-16, -2)));
        }

        var u2 = new uint2(0x8000_0000, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(u2 >> 1, Is.EqualTo(new uint2(0x4000_0000, 0)));
            Assert.That(u2 >>> 1, Is.EqualTo(new uint2(0x4000_0000, 0)));
            Assert.That(u2 << 1, Is.EqualTo(new uint2(0, 2)));
        }

        var ul2 = new ulong2(0x8000_0000_0000_0000UL, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ul2 >>> 1, Is.EqualTo(new ulong2(0x4000_0000_0000_0000UL, 0)));
            Assert.That(ul2 << 2, Is.EqualTo(new ulong2(0, 4)));
        }

        var l2 = new long2(-1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(l2 >> 1, Is.EqualTo(new long2(-1, 1)));
            Assert.That(l2 >>> 1, Is.EqualTo(new long2(long.MaxValue, 1)));
        }

        var l4 = new long4(1, 2, 3, 4);
        Assert.That(l4 << 4, Is.EqualTo(new long4(16, 32, 48, 64)));

        var u4 = new uint4(1, 2, 3, 4);
        Assert.That(u4 >> 1, Is.EqualTo(new uint4(0, 1, 1, 2)));

        var ul4 = new ulong4(1, 2, 3, 4);
        Assert.That(ul4 >>> 1, Is.EqualTo(new ulong4(0, 1, 1, 2)));
    }

    [Test]
    public void ShiftsOfSmallTypes()
    {
        var s2 = new short2(-8, 4);
        using (Assert.EnterMultipleScope())
        {
            // the logical shift of a small type has to shift the unsigned bits, not the sign extended ones
            Assert.That(s2 >> 1, Is.EqualTo(new short2(-4, 2)));
            Assert.That(s2 >>> 1, Is.EqualTo(new short2(32764, 2)));
            Assert.That(s2 << 1, Is.EqualTo(new short2(-16, 8)));
        }

        var s3 = new short3(1, 2, 3);
        Assert.That(s3 << 2, Is.EqualTo(new short3(4, 8, 12)));

        var us2 = new ushort2(0x8000, 4);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(us2 >> 1, Is.EqualTo(new ushort2(0x4000, 2)));
            Assert.That(us2 >>> 1, Is.EqualTo(new ushort2(0x4000, 2)));
            Assert.That(us2 << 1, Is.EqualTo(new ushort2(0, 8)));
        }

        var us4 = new ushort4(1, 2, 3, 4);
        Assert.That(us4 << 1, Is.EqualTo(new ushort4(2, 4, 6, 8)));
    }

    [Test]
    public void ShiftsOfFloatingPointAreBitLevel()
    {
        var f2 = new float2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(f2 << 1, Is.EqualTo(new float2(
                BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(1f) << 1),
                BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(2f) << 1))));
            Assert.That(f2 >> 1, Is.EqualTo(new float2(
                BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(1f) >> 1),
                BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(2f) >> 1))));
            Assert.That(f2 >>> 1, Is.EqualTo(new float2(
                BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(1f) >>> 1),
                BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(2f) >>> 1))));
        }

        var f4 = new float4(1, 2, 3, 4);
        Assert.That(f4 & f4, Is.EqualTo(f4));

        var d2 = new double2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(d2 << 1, Is.EqualTo(new double2(
                BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(1d) << 1),
                BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(2d) << 1))));
            Assert.That(d2 >>> 1, Is.EqualTo(new double2(
                BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(1d) >>> 1),
                BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(2d) >>> 1))));
        }

        var h2 = new half2((Half)1, (Half)2);
        Assert.That(h2 << 1, Is.EqualTo(new half2(
            BitConverter.UInt16BitsToHalf((ushort)(BitConverter.HalfToUInt16Bits((Half)1) << 1)),
            BitConverter.UInt16BitsToHalf((ushort)(BitConverter.HalfToUInt16Bits((Half)2) << 1)))));
    }

    [Test]
    public void PaddingLaneStaysZero()
    {
        var f3 = new float3(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((~f3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That((f3 & f3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That((f3 | f3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That((f3 ^ f3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That((f3 << 1).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That((f3 >>> 2).vector.GetElement(3), Is.EqualTo(0f));
        }

        var i3 = new int3(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((~i3).vector.GetElement(3), Is.EqualTo(0));
            Assert.That((i3 & i3).vector.GetElement(3), Is.EqualTo(0));
            Assert.That((i3 << 2).vector.GetElement(3), Is.EqualTo(0));
            Assert.That((i3 >> 2).vector.GetElement(3), Is.EqualTo(0));
        }

        var u3 = new uint3(1, 2, 3);
        var d3 = new double3(1, 2, 3);
        var l3 = new long3(1, 2, 3);
        var ul3 = new ulong3(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((~u3).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((~d3).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That((d3 << 3).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That((~l3).vector.GetElement(3), Is.EqualTo(0L));
            Assert.That((ul3 << 1).vector.GetElement(3), Is.EqualTo(0UL));
        }

        var t3 = b32v3.True;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((~t3).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((t3 & t3).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((bool)(~t3).x, Is.False);
            Assert.That((bool)(~t3).z, Is.False);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That((b64v3.True | b64v3.False).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That((b16v3.True & b16v3.True).z, Is.EqualTo(B16.True));
        }
    }
}
