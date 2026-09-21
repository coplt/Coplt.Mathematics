using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

public class TestVectorCtor
{
    [Test]
    public void FromComponents()
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
    }

    [Test]
    public void BroadcastAndScalar()
    {
        var f2 = new float2(3);
        var f3 = float3.Broadcast(3);
        var f4 = new float4(3);
        var d2 = new double2(3);
        var d3 = double3.Broadcast(3);
        var d4 = new double4(3);
        var s2 = new short2(3);
        var s3 = short3.Broadcast(3);
        var s4 = new short4(3);
        var us2 = new ushort2(3);
        var us3 = ushort3.Broadcast(3);
        var us4 = new ushort4(3);
        var i2 = new int2(3);
        var i3 = int3.Broadcast(3);
        var i4 = new int4(3);
        var u2 = new uint2(3);
        var u3 = uint3.Broadcast(3);
        var u4 = new uint4(3);
        var l2 = new long2(3);
        var l3 = long3.Broadcast(3);
        var l4 = new long4(3);
        var ul2 = new ulong2(3);
        var ul3 = ulong3.Broadcast(3);
        var ul4 = new ulong4(3);
        var h2 = new half2((Half)3);
        var h3 = half3.Broadcast((Half)3);
        var h4 = new half4((Half)3);
        var e2 = new b16v2(B16.True);
        var e3 = b16v3.Broadcast(B16.True);
        var t3 = b32v3.Broadcast(B32.True);
        var q3 = b64v3.Broadcast(B64.True);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.x, f2.y), Is.EqualTo((3f, 3f)));
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((3f, 3f, 3f)));
            Assert.That((f4.x, f4.y, f4.z, f4.w), Is.EqualTo((3f, 3f, 3f, 3f)));
            Assert.That((d2.x, d2.y), Is.EqualTo((3d, 3d)));
            Assert.That((d3.x, d3.y, d3.z), Is.EqualTo((3d, 3d, 3d)));
            Assert.That((d4.x, d4.y, d4.z, d4.w), Is.EqualTo((3d, 3d, 3d, 3d)));
            Assert.That((s2.x, s2.y), Is.EqualTo(((short)3, (short)3)));
            Assert.That((s3.x, s3.y, s3.z), Is.EqualTo(((short)3, (short)3, (short)3)));
            Assert.That((s4.x, s4.y, s4.z, s4.w), Is.EqualTo(((short)3, (short)3, (short)3, (short)3)));
            Assert.That((us2.x, us2.y), Is.EqualTo(((ushort)3, (ushort)3)));
            Assert.That((us3.x, us3.y, us3.z), Is.EqualTo(((ushort)3, (ushort)3, (ushort)3)));
            Assert.That((us4.x, us4.y, us4.z, us4.w), Is.EqualTo(((ushort)3, (ushort)3, (ushort)3, (ushort)3)));
            Assert.That((i2.x, i2.y), Is.EqualTo((3, 3)));
            Assert.That((i3.x, i3.y, i3.z), Is.EqualTo((3, 3, 3)));
            Assert.That((i4.x, i4.y, i4.z, i4.w), Is.EqualTo((3, 3, 3, 3)));
            Assert.That((u2.x, u2.y), Is.EqualTo((3u, 3u)));
            Assert.That((u3.x, u3.y, u3.z), Is.EqualTo((3u, 3u, 3u)));
            Assert.That((u4.x, u4.y, u4.z, u4.w), Is.EqualTo((3u, 3u, 3u, 3u)));
            Assert.That((l2.x, l2.y), Is.EqualTo((3L, 3L)));
            Assert.That((l3.x, l3.y, l3.z), Is.EqualTo((3L, 3L, 3L)));
            Assert.That((l4.x, l4.y, l4.z, l4.w), Is.EqualTo((3L, 3L, 3L, 3L)));
            Assert.That((ul2.x, ul2.y), Is.EqualTo((3UL, 3UL)));
            Assert.That((ul3.x, ul3.y, ul3.z), Is.EqualTo((3UL, 3UL, 3UL)));
            Assert.That((ul4.x, ul4.y, ul4.z, ul4.w), Is.EqualTo((3UL, 3UL, 3UL, 3UL)));
            Assert.That((h2.x, h2.y), Is.EqualTo(((Half)3, (Half)3)));
            Assert.That((h3.x, h3.y, h3.z), Is.EqualTo(((Half)3, (Half)3, (Half)3)));
            Assert.That((h4.x, h4.y, h4.z, h4.w), Is.EqualTo(((Half)3, (Half)3, (Half)3, (Half)3)));
            Assert.That((e2.x, e2.y), Is.EqualTo((B16.True, B16.True)));
            Assert.That((e3.x, e3.y, e3.z), Is.EqualTo((B16.True, B16.True, B16.True)));
            Assert.That((t3.x, t3.y, t3.z), Is.EqualTo((B32.True, B32.True, B32.True)));
            Assert.That((q3.x, q3.y, q3.z), Is.EqualTo((B64.True, B64.True, B64.True)));

            // Scalar only writes the first component
            Assert.That((float4.Scalar(9).x, float4.Scalar(9).y, float4.Scalar(9).z, float4.Scalar(9).w),
                Is.EqualTo((9f, 0f, 0f, 0f)));
            Assert.That((int3.Scalar(9).x, int3.Scalar(9).y, int3.Scalar(9).z), Is.EqualTo((9, 0, 0)));
            Assert.That((short2.Scalar(9).x, short2.Scalar(9).y), Is.EqualTo(((short)9, (short)0)));
            Assert.That((half3.Scalar((Half)9).x, half3.Scalar((Half)9).y, half3.Scalar((Half)9).z),
                Is.EqualTo(((Half)9, (Half)0, (Half)0)));
            Assert.That((b32v3.Scalar(B32.True).x, b32v3.Scalar(B32.True).y, b32v3.Scalar(B32.True).z),
                Is.EqualTo((B32.True, B32.False, B32.False)));
            Assert.That((b64v2.Scalar(B64.True).x, b64v2.Scalar(B64.True).y), Is.EqualTo((B64.True, B64.False)));
        }
    }

    [Test]
    public void FromTupleAndScalar()
    {
        float2 f2 = (1, 2);
        float3 f3 = (1, 2, 3);
        float4 f4 = (1, 2, 3, 4);
        double2 d2 = (1, 2);
        double3 d3 = (1, 2, 3);
        double4 d4 = (1, 2, 3, 4);
        short2 s2 = (1, 2);
        short3 s3 = (1, 2, 3);
        short4 s4 = (1, 2, 3, 4);
        ushort2 us2 = (1, 2);
        ushort3 us3 = (1, 2, 3);
        ushort4 us4 = (1, 2, 3, 4);
        int2 i2 = (1, 2);
        int3 i3 = (1, 2, 3);
        int4 i4 = (1, 2, 3, 4);
        uint2 u2 = (1, 2);
        uint3 u3 = (1, 2, 3);
        uint4 u4 = (1, 2, 3, 4);
        long2 l2 = (1, 2);
        long3 l3 = (1, 2, 3);
        long4 l4 = (1, 2, 3, 4);
        ulong2 ul2 = (1, 2);
        ulong3 ul3 = (1, 2, 3);
        ulong4 ul4 = (1, 2, 3, 4);
        half2 h2 = ((Half)1, (Half)2);
        half3 h3 = ((Half)1, (Half)2, (Half)3);
        half4 h4 = ((Half)1, (Half)2, (Half)3, (Half)4);
        b16v2 e2 = (B16.True, B16.False);
        b16v3 e3 = (B16.True, B16.False, B16.True);
        b32v2 t2 = (B32.True, B32.False);
        b32v4 t4 = (B32.True, B32.False, B32.True, B32.False);
        b64v3 q3 = (B64.True, B64.False, B64.True);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.y, f3.z, f4.w), Is.EqualTo((2f, 3f, 4f)));
            Assert.That((d2.y, d3.z, d4.w), Is.EqualTo((2d, 3d, 4d)));
            Assert.That((s2.y, s3.z, s4.w), Is.EqualTo(((short)2, (short)3, (short)4)));
            Assert.That((us2.y, us3.z, us4.w), Is.EqualTo(((ushort)2, (ushort)3, (ushort)4)));
            Assert.That((i2.y, i3.z, i4.w), Is.EqualTo((2, 3, 4)));
            Assert.That((u2.y, u3.z, u4.w), Is.EqualTo((2u, 3u, 4u)));
            Assert.That((l2.y, l3.z, l4.w), Is.EqualTo((2L, 3L, 4L)));
            Assert.That((ul2.y, ul3.z, ul4.w), Is.EqualTo((2UL, 3UL, 4UL)));
            Assert.That((h2.y, h3.z, h4.w), Is.EqualTo(((Half)2, (Half)3, (Half)4)));
            Assert.That((e2.y, e3.z), Is.EqualTo((B16.False, B16.True)));
            Assert.That((t2.y, t4.w), Is.EqualTo((B32.False, B32.False)));
            Assert.That((bool)q3.z, Is.True);

            // the tuple constructor is the same as the component constructor
            Assert.That(new float3((1, 2, 3)), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(new int2((1, 2)), Is.EqualTo(new int2(1, 2)));
            Assert.That(new b32v2((B32.True, B32.False)), Is.EqualTo(new b32v2(B32.True, B32.False)));

            // a scalar converts to a broadcast vector
            float4 sf = 5;
            int3 si = 5;
            short2 ss = 5;
            half3 sh = (Half)5;
            b32v4 sb = B32.True;
            Assert.That((sf.x, sf.w), Is.EqualTo((5f, 5f)));
            Assert.That((si.x, si.z), Is.EqualTo((5, 5)));
            Assert.That((ss.x, ss.y), Is.EqualTo(((short)5, (short)5)));
            Assert.That((sh.x, sh.z), Is.EqualTo(((Half)5, (Half)5)));
            Assert.That((sb.x, sb.w), Is.EqualTo((B32.True, B32.True)));
        }
    }

    [Test]
    public void FromVectorAndPaddingLane()
    {
        var f2 = new float2(Vector128.Create(1f, 2f, 99f, 99f));
        var f3 = new float3(Vector128.Create(1f, 2f, 3f, 99f));
        var f4 = new float4(Vector128.Create(1f, 2f, 3f, 4f));
        var d2 = new double2(Vector128.Create(1d, 2d));
        var d3 = new double3(Vector256.Create(1d, 2d, 3d, 99d));
        var d4 = new double4(Vector256.Create(1d, 2d, 3d, 4d));
        var i2 = new int2(Vector128.Create(1, 2, 99, 99));
        var i3 = new int3(Vector128.Create(1, 2, 3, 99));
        var i4 = new int4(Vector128.Create(1, 2, 3, 4));
        var u2 = new uint2(Vector128.Create(1u, 2u, 99u, 99u));
        var u3 = new uint3(Vector128.Create(1u, 2u, 3u, 99u));
        var u4 = new uint4(Vector128.Create(1u, 2u, 3u, 4u));
        var l2 = new long2(Vector128.Create(1L, 2L));
        var l3 = new long3(Vector256.Create(1L, 2L, 3L, 99L));
        var l4 = new long4(Vector256.Create(1L, 2L, 3L, 4L));
        var ul2 = new ulong2(Vector128.Create(1UL, 2UL));
        var ul3 = new ulong3(Vector256.Create(1UL, 2UL, 3UL, 99UL));
        var ul4 = new ulong4(Vector256.Create(1UL, 2UL, 3UL, 4UL));
        var t2 = new b32v2(Vector128.Create(~0u, 0u, ~0u, ~0u));
        var t3 = new b32v3(Vector128.Create(~0u, 0u, ~0u, ~0u));
        var t4 = new b32v4(Vector128.Create(~0u, 0u, ~0u, 0u));
        var q2 = new b64v2(Vector128.Create(~0UL, 0UL));
        var q3 = new b64v3(Vector256.Create(~0UL, 0UL, ~0UL, ~0UL));
        var q4 = new b64v4(Vector256.Create(~0UL, 0UL, ~0UL, 0UL));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.x, f2.y), Is.EqualTo((1f, 2f)));
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((f4.x, f4.y, f4.z, f4.w), Is.EqualTo((1f, 2f, 3f, 4f)));
            Assert.That((d2.x, d2.y), Is.EqualTo((1d, 2d)));
            Assert.That((d3.x, d3.y, d3.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((d4.x, d4.y, d4.z, d4.w), Is.EqualTo((1d, 2d, 3d, 4d)));
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
            Assert.That((t2.x, t2.y), Is.EqualTo((B32.True, B32.False)));
            Assert.That((t3.x, t3.y, t3.z), Is.EqualTo((B32.True, B32.False, B32.True)));
            Assert.That((t4.x, t4.y, t4.z, t4.w), Is.EqualTo((B32.True, B32.False, B32.True, B32.False)));
            Assert.That((q2.x, q2.y), Is.EqualTo((B64.True, B64.False)));
            Assert.That((q3.x, q3.y, q3.z), Is.EqualTo((B64.True, B64.False, B64.True)));
            Assert.That((q4.x, q4.y, q4.z, q4.w), Is.EqualTo((B64.True, B64.False, B64.True, B64.False)));

            // a 2 component vector whose register is widened to 128 bits zeroes its padding lanes
            Assert.That(f2.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(i2.vector.GetElement(3), Is.EqualTo(0));
            Assert.That(u2.vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(t2.vector.GetElement(3), Is.EqualTo(0u));

            // a 3 component vector zeroes its padding lane, the other components are kept
            Assert.That(f3.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(d3.vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(i3.vector.GetElement(3), Is.EqualTo(0));
            Assert.That(u3.vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(l3.vector.GetElement(3), Is.EqualTo(0L));
            Assert.That(ul3.vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(t3.vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(q3.vector.GetElement(3), Is.EqualTo(0UL));
        }
    }

    [Test]
    public void LoadFromSpan()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float2.Load(new[] { 1f, 2f }), Is.EqualTo(new float2(1, 2)));
            Assert.That(float3.Load(new[] { 1f, 2f, 3f, 9f }), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(float4.Load(new[] { 1f, 2f, 3f, 4f }), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(double2.Load(new[] { 1d, 2d }), Is.EqualTo(new double2(1, 2)));
            Assert.That(double3.Load(new[] { 1d, 2d, 3d, 9d }), Is.EqualTo(new double3(1, 2, 3)));
            Assert.That(double4.Load(new[] { 1d, 2d, 3d, 4d }), Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That(int2.Load(new[] { 1, 2 }), Is.EqualTo(new int2(1, 2)));
            Assert.That(int3.Load(new[] { 1, 2, 3, 9 }), Is.EqualTo(new int3(1, 2, 3)));
            Assert.That(int4.Load(new[] { 1, 2, 3, 4 }), Is.EqualTo(new int4(1, 2, 3, 4)));
            Assert.That(uint2.Load(new[] { 1u, 2u }), Is.EqualTo(new uint2(1, 2)));
            Assert.That(uint3.Load(new[] { 1u, 2u, 3u, 9u }), Is.EqualTo(new uint3(1, 2, 3)));
            Assert.That(uint4.Load(new[] { 1u, 2u, 3u, 4u }), Is.EqualTo(new uint4(1, 2, 3, 4)));
            Assert.That(long2.Load(new[] { 1L, 2L }), Is.EqualTo(new long2(1, 2)));
            Assert.That(long3.Load(new[] { 1L, 2L, 3L, 9L }), Is.EqualTo(new long3(1, 2, 3)));
            Assert.That(long4.Load(new[] { 1L, 2L, 3L, 4L }), Is.EqualTo(new long4(1, 2, 3, 4)));
            Assert.That(ulong2.Load(new[] { 1UL, 2UL }), Is.EqualTo(new ulong2(1, 2)));
            Assert.That(ulong3.Load(new[] { 1UL, 2UL, 3UL, 9UL }), Is.EqualTo(new ulong3(1, 2, 3)));
            Assert.That(ulong4.Load(new[] { 1UL, 2UL, 3UL, 4UL }), Is.EqualTo(new ulong4(1, 2, 3, 4)));

            // these are not vector types, they load the components one by one
            Assert.That(short2.Load(new short[] { 1, 2 }), Is.EqualTo(new short2(1, 2)));
            Assert.That(short3.Load(new short[] { 1, 2, 3 }), Is.EqualTo(new short3(1, 2, 3)));
            Assert.That(short4.Load(new short[] { 1, 2, 3, 4 }), Is.EqualTo(new short4(1, 2, 3, 4)));
            Assert.That(ushort2.Load(new ushort[] { 1, 2 }), Is.EqualTo(new ushort2(1, 2)));
            Assert.That(ushort3.Load(new ushort[] { 1, 2, 3 }), Is.EqualTo(new ushort3(1, 2, 3)));
            Assert.That(ushort4.Load(new ushort[] { 1, 2, 3, 4 }), Is.EqualTo(new ushort4(1, 2, 3, 4)));
            Assert.That(half2.Load(new[] { (Half)1, (Half)2 }), Is.EqualTo(new half2((Half)1, (Half)2)));
            Assert.That(half3.Load(new[] { (Half)1, (Half)2, (Half)3 }), Is.EqualTo(new half3((Half)1, (Half)2, (Half)3)));
            Assert.That(half4.Load(new[] { (Half)1, (Half)2, (Half)3, (Half)4 }), Is.EqualTo(new half4((Half)1, (Half)2, (Half)3, (Half)4)));
        }
    }

    [Test]
    public void LoadFromBoolSpan()
    {
        var ones = new[] { B16.True, B16.True, B16.True, B16.True };
        var twos = new[] { B32.True, B32.False, B32.True, B32.False };
        var fours = new[] { B64.True, B64.False, B64.True, B64.False };

        using (Assert.EnterMultipleScope())
        {
            Assert.That(b16v2.Load(ones.AsSpan(0, 2)), Is.EqualTo(new b16v2(B16.True, B16.True)));
            Assert.That(b16v3.Load(ones.AsSpan(0, 3)), Is.EqualTo(new b16v3(B16.True, B16.True, B16.True)));
            Assert.That(b16v4.Load(ones), Is.EqualTo(new b16v4(B16.True, B16.True, B16.True, B16.True)));
            Assert.That(b32v2.Load(twos.AsSpan(0, 2)), Is.EqualTo(new b32v2(B32.True, B32.False)));
            Assert.That(b32v3.Load(twos), Is.EqualTo(new b32v3(B32.True, B32.False, B32.True)));
            Assert.That(b32v4.Load(twos), Is.EqualTo(new b32v4(B32.True, B32.False, B32.True, B32.False)));
            Assert.That(b64v2.Load(fours.AsSpan(0, 2)), Is.EqualTo(new b64v2(B64.True, B64.False)));
            Assert.That(b64v3.Load(fours), Is.EqualTo(new b64v3(B64.True, B64.False, B64.True)));
            Assert.That(b64v4.Load(fours), Is.EqualTo(new b64v4(B64.True, B64.False, B64.True, B64.False)));
        }
    }
}
