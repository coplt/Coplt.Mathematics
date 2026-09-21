using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

public class TestVectorComponents
{
    [Test]
    public void XYZW()
    {
        var f2 = new float2(1, 2);
        var f3 = new float3(1, 2, 3);
        var f4 = new float4(1, 2, 3, 4);
        var d2 = new double2(1, 2);
        var d3 = new double3(1, 2, 3);
        var d4 = new double4(1, 2, 3, 4);
        var s2 = new short2(1, 2);
        var s3 = new short3(1, 2, 3);
        var s4 = new short4(1, 2, 3, 4);
        var us2 = new ushort2(1, 2);
        var us3 = new ushort3(1, 2, 3);
        var us4 = new ushort4(1, 2, 3, 4);
        var i2 = new int2(1, 2);
        var i3 = new int3(1, 2, 3);
        var i4 = new int4(1, 2, 3, 4);
        var u2 = new uint2(1, 2);
        var u3 = new uint3(1, 2, 3);
        var u4 = new uint4(1, 2, 3, 4);
        var l2 = new long2(1, 2);
        var l3 = new long3(1, 2, 3);
        var l4 = new long4(1, 2, 3, 4);
        var ul2 = new ulong2(1, 2);
        var ul3 = new ulong3(1, 2, 3);
        var ul4 = new ulong4(1, 2, 3, 4);
        var h2 = new half2((Half)1, (Half)2);
        var h3 = new half3((Half)1, (Half)2, (Half)3);
        var h4 = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var e2 = new b16v2(B16.True, B16.False);
        var e3 = new b16v3(B16.True, B16.False, B16.True);
        var e4 = new b16v4(B16.True, B16.False, B16.True, B16.False);
        var t2 = new b32v2(B32.True, B32.False);
        var t3 = new b32v3(B32.True, B32.False, B32.True);
        var t4 = new b32v4(B32.True, B32.False, B32.True, B32.False);
        var q2 = new b64v2(B64.True, B64.False);
        var q3 = new b64v3(B64.True, B64.False, B64.True);
        var q4 = new b64v4(B64.True, B64.False, B64.True, B64.False);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.x, f2.y), Is.EqualTo((1f, 2f)));
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((f4.x, f4.y, f4.z, f4.w), Is.EqualTo((1f, 2f, 3f, 4f)));
            Assert.That((d2.x, d2.y), Is.EqualTo((1d, 2d)));
            Assert.That((d3.x, d3.y, d3.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((d4.x, d4.y, d4.z, d4.w), Is.EqualTo((1d, 2d, 3d, 4d)));
            Assert.That((s2.x, s2.y), Is.EqualTo(((short)1, (short)2)));
            Assert.That((s3.x, s3.y, s3.z), Is.EqualTo(((short)1, (short)2, (short)3)));
            Assert.That((s4.x, s4.y, s4.z, s4.w), Is.EqualTo(((short)1, (short)2, (short)3, (short)4)));
            Assert.That((us2.x, us2.y), Is.EqualTo(((ushort)1, (ushort)2)));
            Assert.That((us3.x, us3.y, us3.z), Is.EqualTo(((ushort)1, (ushort)2, (ushort)3)));
            Assert.That((us4.x, us4.y, us4.z, us4.w), Is.EqualTo(((ushort)1, (ushort)2, (ushort)3, (ushort)4)));
            Assert.That((i2.x, i2.y), Is.EqualTo((1, 2)));
            Assert.That((i3.x, i3.y, i3.z), Is.EqualTo((1, 2, 3)));
            Assert.That((i4.x, i4.y, i4.z, i4.w), Is.EqualTo((1, 2, 3, 4)));
            Assert.That((u2.x, u2.y), Is.EqualTo((1u, 2u)));
            Assert.That((u3.x, u3.y, u3.z), Is.EqualTo((1u, 2u, 3u)));
            Assert.That((u4.x, u4.y, u4.z, u4.w), Is.EqualTo((1u, 2u, 3u, 4u)));
            Assert.That((l2.x, l2.y), Is.EqualTo((1L, 2L)));
            Assert.That((l3.x, l3.y, l3.z), Is.EqualTo((1L, 2L, 3L)));
            Assert.That((l4.x, l4.y, l4.z, l4.w), Is.EqualTo((1L, 2L, 3L, 4L)));
            Assert.That((ul2.x, ul2.y), Is.EqualTo((1UL, 2UL)));
            Assert.That((ul3.x, ul3.y, ul3.z), Is.EqualTo((1UL, 2UL, 3UL)));
            Assert.That((ul4.x, ul4.y, ul4.z, ul4.w), Is.EqualTo((1UL, 2UL, 3UL, 4UL)));
            Assert.That((h2.x, h2.y), Is.EqualTo(((Half)1, (Half)2)));
            Assert.That((h3.x, h3.y, h3.z), Is.EqualTo(((Half)1, (Half)2, (Half)3)));
            Assert.That((h4.x, h4.y, h4.z, h4.w), Is.EqualTo(((Half)1, (Half)2, (Half)3, (Half)4)));
            Assert.That((e2.x, e2.y), Is.EqualTo((B16.True, B16.False)));
            Assert.That((e3.x, e3.y, e3.z), Is.EqualTo((B16.True, B16.False, B16.True)));
            Assert.That((e4.x, e4.y, e4.z, e4.w), Is.EqualTo((B16.True, B16.False, B16.True, B16.False)));
            Assert.That((t2.x, t2.y), Is.EqualTo((B32.True, B32.False)));
            Assert.That((t3.x, t3.y, t3.z), Is.EqualTo((B32.True, B32.False, B32.True)));
            Assert.That((t4.x, t4.y, t4.z, t4.w), Is.EqualTo((B32.True, B32.False, B32.True, B32.False)));
            Assert.That((q2.x, q2.y), Is.EqualTo((B64.True, B64.False)));
            Assert.That((q3.x, q3.y, q3.z), Is.EqualTo((B64.True, B64.False, B64.True)));
            Assert.That((q4.x, q4.y, q4.z, q4.w), Is.EqualTo((B64.True, B64.False, B64.True, B64.False)));
        }

        // writing through a component, a simd vector writes the lane and a plain vector the field
        f4.w = 9;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(f4.vector.GetElement(3), Is.EqualTo(9f));
            Assert.That(f4.x, Is.EqualTo(1f));
        }
        d3.z = 9;
        i3.z = 9;
        u3.z = 9;
        l3.z = 9;
        ul3.z = 9;
        s3.z = 9;
        us3.z = 9;
        h3.z = (Half)9;
        e3.z = B16.False;
        t3.z = B32.True;
        q3.z = B64.True;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(d3.z, Is.EqualTo(9d));
            Assert.That(i3.z, Is.EqualTo(9));
            Assert.That(u3.z, Is.EqualTo(9u));
            Assert.That(l3.z, Is.EqualTo(9L));
            Assert.That(ul3.z, Is.EqualTo(9UL));
            Assert.That(s3.z, Is.EqualTo((short)9));
            Assert.That(us3.z, Is.EqualTo((ushort)9));
            Assert.That(h3.z, Is.EqualTo((Half)9));
            Assert.That(e3.z, Is.EqualTo(B16.False));
            Assert.That(t3.z, Is.EqualTo(B32.True));
            Assert.That(q3.z, Is.EqualTo(B64.True));
        }
    }

    [Test]
    public void ColorAliases()
    {
        var f2 = new float2(1, 2);
        var f3 = new float3(1, 2, 3);
        var f4 = new float4(1, 2, 3, 4);
        var d2 = new double2(1, 2);
        var d3 = new double3(1, 2, 3);
        var d4 = new double4(1, 2, 3, 4);
        var s2 = new short2(1, 2);
        var s3 = new short3(1, 2, 3);
        var s4 = new short4(1, 2, 3, 4);
        var us2 = new ushort2(1, 2);
        var us3 = new ushort3(1, 2, 3);
        var us4 = new ushort4(1, 2, 3, 4);
        var i2 = new int2(1, 2);
        var i3 = new int3(1, 2, 3);
        var i4 = new int4(1, 2, 3, 4);
        var u2 = new uint2(1, 2);
        var u3 = new uint3(1, 2, 3);
        var u4 = new uint4(1, 2, 3, 4);
        var l4 = new long4(1, 2, 3, 4);
        var ul4 = new ulong4(1, 2, 3, 4);
        var h4 = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var e4 = new b16v4(B16.True, B16.False, B16.True, B16.False);
        var t4 = new b32v4(B32.True, B32.False, B32.True, B32.False);
        var q4 = new b64v4(B64.True, B64.False, B64.True, B64.False);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.r, f2.g), Is.EqualTo((1f, 2f)));
            Assert.That((f3.r, f3.g, f3.b), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((f4.r, f4.g, f4.b, f4.a), Is.EqualTo((1f, 2f, 3f, 4f)));
            Assert.That((d2.r, d2.g), Is.EqualTo((1d, 2d)));
            Assert.That((d3.r, d3.g, d3.b), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((d4.r, d4.g, d4.b, d4.a), Is.EqualTo((1d, 2d, 3d, 4d)));
            Assert.That((s2.r, s2.g), Is.EqualTo(((short)1, (short)2)));
            Assert.That((s3.r, s3.g, s3.b), Is.EqualTo(((short)1, (short)2, (short)3)));
            Assert.That((s4.a, s4.b), Is.EqualTo(((short)4, (short)3)));
            Assert.That((us2.r, us2.g), Is.EqualTo(((ushort)1, (ushort)2)));
            Assert.That((us3.b, us3.g), Is.EqualTo(((ushort)3, (ushort)2)));
            Assert.That((us4.a, us4.r), Is.EqualTo(((ushort)4, (ushort)1)));
            Assert.That((i2.r, i2.g), Is.EqualTo((1, 2)));
            Assert.That((i3.r, i3.g, i3.b), Is.EqualTo((1, 2, 3)));
            Assert.That((i4.r, i4.g, i4.b, i4.a), Is.EqualTo((1, 2, 3, 4)));
            Assert.That((u2.r, u2.g), Is.EqualTo((1u, 2u)));
            Assert.That((u3.r, u3.g, u3.b), Is.EqualTo((1u, 2u, 3u)));
            Assert.That((u4.r, u4.g, u4.b, u4.a), Is.EqualTo((1u, 2u, 3u, 4u)));
            Assert.That((l4.r, l4.a), Is.EqualTo((1L, 4L)));
            Assert.That((ul4.b, ul4.g), Is.EqualTo((3UL, 2UL)));
            Assert.That((h4.r, h4.a), Is.EqualTo(((Half)1, (Half)4)));
            Assert.That((e4.r, e4.a), Is.EqualTo((B16.True, B16.False)));
            Assert.That((t4.r, t4.a), Is.EqualTo((B32.True, B32.False)));
            Assert.That((q4.g, q4.b), Is.EqualTo((B64.False, B64.True)));
        }

        // the color aliases are the same storage as xyzw
        f4.g = 9;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(f4.y, Is.EqualTo(9f));
            Assert.That(f4.g, Is.EqualTo(9f));
        }
        f4.z = 8;
        Assert.That(f4.b, Is.EqualTo(8f));
        f2.r = 7;
        Assert.That(f2.x, Is.EqualTo(7f));
        t4.a = B32.True;
        Assert.That((bool)t4.w, Is.True);
        s4.b = 7;
        Assert.That(s4.z, Is.EqualTo((short)7));
        i4.a = 7;
        Assert.That(i4.w, Is.EqualTo(7));
    }

    [Test]
    public void Indexer()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(1, 2)[1], Is.EqualTo(2f));
            Assert.That(new float3(1, 2, 3)[2], Is.EqualTo(3f));
            Assert.That(new float4(1, 2, 3, 4)[3], Is.EqualTo(4f));
            Assert.That(new double2(1, 2)[1], Is.EqualTo(2d));
            Assert.That(new double3(1, 2, 3)[2], Is.EqualTo(3d));
            Assert.That(new double4(1, 2, 3, 4)[3], Is.EqualTo(4d));
            Assert.That(new short2(1, 2)[1], Is.EqualTo((short)2));
            Assert.That(new short3(1, 2, 3)[2], Is.EqualTo((short)3));
            Assert.That(new short4(1, 2, 3, 4)[3], Is.EqualTo((short)4));
            Assert.That(new ushort2(1, 2)[1], Is.EqualTo((ushort)2));
            Assert.That(new ushort3(1, 2, 3)[2], Is.EqualTo((ushort)3));
            Assert.That(new ushort4(1, 2, 3, 4)[3], Is.EqualTo((ushort)4));
            Assert.That(new int2(1, 2)[1], Is.EqualTo(2));
            Assert.That(new int3(1, 2, 3)[2], Is.EqualTo(3));
            Assert.That(new int4(1, 2, 3, 4)[3], Is.EqualTo(4));
            Assert.That(new uint2(1, 2)[1], Is.EqualTo(2u));
            Assert.That(new uint3(1, 2, 3)[2], Is.EqualTo(3u));
            Assert.That(new uint4(1, 2, 3, 4)[3], Is.EqualTo(4u));
            Assert.That(new long2(1, 2)[1], Is.EqualTo(2L));
            Assert.That(new long3(1, 2, 3)[2], Is.EqualTo(3L));
            Assert.That(new long4(1, 2, 3, 4)[3], Is.EqualTo(4L));
            Assert.That(new ulong2(1, 2)[1], Is.EqualTo(2UL));
            Assert.That(new ulong3(1, 2, 3)[2], Is.EqualTo(3UL));
            Assert.That(new ulong4(1, 2, 3, 4)[3], Is.EqualTo(4UL));
            Assert.That(new half2((Half)1, (Half)2)[1], Is.EqualTo((Half)2));
            Assert.That(new half3((Half)1, (Half)2, (Half)3)[2], Is.EqualTo((Half)3));
            Assert.That(new half4((Half)1, (Half)2, (Half)3, (Half)4)[3], Is.EqualTo((Half)4));
            Assert.That(new b16v2(B16.True, B16.False)[1], Is.EqualTo(B16.False));
            Assert.That(new b16v3(B16.True, B16.False, B16.True)[2], Is.EqualTo(B16.True));
            Assert.That(new b16v4(B16.True, B16.False, B16.True, B16.False)[3], Is.EqualTo(B16.False));
            Assert.That(new b32v2(B32.True, B32.False)[1], Is.EqualTo(B32.False));
            Assert.That(new b32v3(B32.True, B32.False, B32.True)[2], Is.EqualTo(B32.True));
            Assert.That(new b32v4(B32.True, B32.False, B32.True, B32.False)[3], Is.EqualTo(B32.False));
            Assert.That(new b64v2(B64.True, B64.False)[1], Is.EqualTo(B64.False));
            Assert.That(new b64v3(B64.True, B64.False, B64.True)[2], Is.EqualTo(B64.True));
            Assert.That(new b64v4(B64.True, B64.False, B64.True, B64.False)[3], Is.EqualTo(B64.False));
        }

        // writing through the indexer writes the component
        var f3 = new float3(0, 0, 0);
        var d2 = new double2(0, 0);
        var i4 = new int4(0, 0, 0, 0);
        var s3 = new short3(0, 0, 0);
        var h2 = new half2((Half)0, (Half)0);
        var t3 = new b32v3(B32.False);
        for (var i = 0; i < 3; i++) f3[i] = i + 1;
        d2[1] = 2;
        for (var i = 0; i < 4; i++) i4[i] = i + 1;
        s3[2] = 3;
        h2[1] = (Half)2;
        t3[0] = B32.True;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((d2.x, d2.y), Is.EqualTo((0d, 2d)));
            Assert.That((i4.x, i4.y, i4.z, i4.w), Is.EqualTo((1, 2, 3, 4)));
            Assert.That(s3.z, Is.EqualTo((short)3));
            Assert.That(h2.y, Is.EqualTo((Half)2));
            Assert.That((bool)t3.x, Is.True);
            Assert.That((bool)t3.y, Is.False);
        }
    }

    [Test]
    public void IndexerOutOfRange()
    {
        var f2 = new float2(1, 2);
        var f3 = new float3(1, 2, 3);
        var f4 = new float4(1, 2, 3, 4);
        var s3 = new short3(1, 2, 3);
        var t3 = new b32v3(B32.True);

        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<IndexOutOfRangeException>(() => _ = f2[2]);
            Assert.Throws<IndexOutOfRangeException>(() => f2[2] = 1);
            Assert.Throws<IndexOutOfRangeException>(() => _ = f3[3]);
            Assert.Throws<IndexOutOfRangeException>(() => f3[3] = 1);
            Assert.Throws<IndexOutOfRangeException>(() => _ = f4[4]);
            Assert.Throws<IndexOutOfRangeException>(() => f4[4] = 1);
            Assert.Throws<IndexOutOfRangeException>(() => _ = f2[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => f2[-1] = 1);
            Assert.Throws<IndexOutOfRangeException>(() => _ = s3[3]);
            Assert.Throws<IndexOutOfRangeException>(() => s3[-1] = 1);
            Assert.Throws<IndexOutOfRangeException>(() => _ = t3[3]);
            Assert.Throws<IndexOutOfRangeException>(() => t3[3] = B32.True);
        }
    }

    [Test]
    public void Deconstruct()
    {
        using (Assert.EnterMultipleScope())
        {
            var (f2x, f2y) = new float2(1, 2);
            Assert.That((f2x, f2y), Is.EqualTo((1f, 2f)));
            var (f3x, f3y, f3z) = new float3(1, 2, 3);
            Assert.That((f3x, f3y, f3z), Is.EqualTo((1f, 2f, 3f)));
            var (f4x, f4y, f4z, f4w) = new float4(1, 2, 3, 4);
            Assert.That((f4x, f4y, f4z, f4w), Is.EqualTo((1f, 2f, 3f, 4f)));

            var (d3x, d3y, d3z) = new double3(1, 2, 3);
            Assert.That((d3x, d3y, d3z), Is.EqualTo((1d, 2d, 3d)));
            var (d4x, d4y, d4z, d4w) = new double4(1, 2, 3, 4);
            Assert.That((d4x, d4y, d4z, d4w), Is.EqualTo((1d, 2d, 3d, 4d)));

            var (s2x, s2y) = new short2(1, 2);
            Assert.That((s2x, s2y), Is.EqualTo(((short)1, (short)2)));
            var (us3x, us3y, us3z) = new ushort3(1, 2, 3);
            Assert.That((us3x, us3y, us3z), Is.EqualTo(((ushort)1, (ushort)2, (ushort)3)));
            var (s4x, s4y, s4z, s4w) = new short4(1, 2, 3, 4);
            Assert.That((s4x, s4y, s4z, s4w), Is.EqualTo(((short)1, (short)2, (short)3, (short)4)));

            var (i2x, i2y) = new int2(1, 2);
            Assert.That((i2x, i2y), Is.EqualTo((1, 2)));
            var (u3x, u3y, u3z) = new uint3(1, 2, 3);
            Assert.That((u3x, u3y, u3z), Is.EqualTo((1u, 2u, 3u)));
            var (i4x, i4y, i4z, i4w) = new int4(1, 2, 3, 4);
            Assert.That((i4x, i4y, i4z, i4w), Is.EqualTo((1, 2, 3, 4)));

            var (l2x, l2y) = new long2(1, 2);
            Assert.That((l2x, l2y), Is.EqualTo((1L, 2L)));
            var (ul4x, ul4y, ul4z, ul4w) = new ulong4(1, 2, 3, 4);
            Assert.That((ul4x, ul4y, ul4z, ul4w), Is.EqualTo((1UL, 2UL, 3UL, 4UL)));

            var (h3x, h3y, h3z) = new half3((Half)1, (Half)2, (Half)3);
            Assert.That((h3x, h3y, h3z), Is.EqualTo(((Half)1, (Half)2, (Half)3)));

            var (e2x, e2y) = new b16v2(B16.True, B16.False);
            Assert.That((e2x, e2y), Is.EqualTo((B16.True, B16.False)));
            var (t4x, t4y, t4z, t4w) = new b32v4(B32.True, B32.False, B32.True, B32.False);
            Assert.That((t4x, t4y, t4z, t4w), Is.EqualTo((B32.True, B32.False, B32.True, B32.False)));
            var (q3x, q3y, q3z) = new b64v3(B64.True, B64.False, B64.True);
            Assert.That((q3x, q3y, q3z), Is.EqualTo((B64.True, B64.False, B64.True)));
        }
    }

    [Test]
    public void VectorFieldIsPublic()
    {
        var f2 = new float2(0, 0);
        var f3 = new float3(0, 0, 0);
        var d2 = new double2(0, 0);
        var i3 = new int3(0, 0, 0);
        var l2 = new long2(0, 0);
        var t3 = new b32v3(B32.False);
        var q2 = new b64v2(B64.False);

        f2.vector = Vector128.Create(1f, 2f, 3f, 4f);
        f3.vector = Vector128.Create(1f, 2f, 3f, 4f);
        d2.vector = Vector128.Create(1d, 2d);
        i3.vector = Vector128.Create(1, 2, 3, 4);
        l2.vector = Vector128.Create(1L, 2L);
        t3.vector = Vector128.Create(~0u, 0u, ~0u, 0u);
        q2.vector = Vector128.Create(~0UL, 0UL);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.x, f2.y), Is.EqualTo((1f, 2f)));
            // writing the field directly bypasses the mask, the padding lanes are whatever was written
            Assert.That((f2.vector.GetElement(2), f2.vector.GetElement(3)), Is.EqualTo((3f, 4f)));
            // writing the field directly bypasses the mask, the padding lane is whatever was written
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That(f3.vector.GetElement(3), Is.EqualTo(4f));
            Assert.That((d2.x, d2.y), Is.EqualTo((1d, 2d)));
            Assert.That((i3.x, i3.y, i3.z), Is.EqualTo((1, 2, 3)));
            Assert.That((l2.x, l2.y), Is.EqualTo((1L, 2L)));
            Assert.That((t3.x, t3.y, t3.z), Is.EqualTo((B32.True, B32.False, B32.True)));
            Assert.That((q2.x, q2.y), Is.EqualTo((B64.True, B64.False)));
        }
    }

    [Test]
    public void PaddingLaneStaysZero()
    {
        var f3 = new float3(1, 2, 3);
        f3.x = 9;
        f3[1] = 8;
        var d3 = new double3(1, 2, 3);
        d3.z = 9;
        var i3 = new int3(1, 2, 3);
        i3[2] = 9;
        var u3 = new uint3(1, 2, 3);
        u3.b = 9;
        var l3 = new long3(1, 2, 3);
        l3[0] = 9;
        var ul3 = new ulong3(1, 2, 3);
        ul3.y = 9;
        var t3 = b32v3.True;
        t3[1] = B32.False;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((9f, 8f, 3f)));
            Assert.That(f3.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(d3.vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(i3.vector.GetElement(3), Is.EqualTo(0));
            Assert.That(u3.vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(l3.vector.GetElement(3), Is.EqualTo(0L));
            Assert.That(ul3.vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(t3.vector.GetElement(3), Is.EqualTo(0u));

            // the constants and the constructors keep it at zero too
            Assert.That(float3.Broadcast(1).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.Scalar(1).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.Zero.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.One.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.Two.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.ScalarZero, Is.EqualTo(0f));
            Assert.That(int3.Broadcast(1).vector.GetElement(3), Is.EqualTo(0));
            Assert.That(double3.Broadcast(1).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(b32v3.True.vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(b64v3.True.vector.GetElement(3), Is.EqualTo(0UL));
        }
    }
}
