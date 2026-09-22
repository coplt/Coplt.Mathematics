using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The replace members of a vector implement <c>IVectorReplace</c>: a member takes the value of the components
/// its name holds and the vector keeps the value of the other ones. The member of a simd backed vector goes
/// through the swizzle setter of the components it replaces and the member of a vector without a register writes
/// the fields of a copy of the vector, a vector of 2 components replaces its two components, a longer one the
/// components its own size reaches.
/// </summary>
public class TestVectorReplace
{
    [Test]
    public void Components2()
    {
        var f = new float2(1, 2);
        var i = new int2(1, 2);
        var s = new short2(1, 2);
        var t = new b32v2(true, false);
        var st = new float2s(1, 2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float2.Rx(f, 3), Is.EqualTo(new float2(3, 2)));
            Assert.That(float2.Ry(f, 3), Is.EqualTo(new float2(1, 3)));
            Assert.That((int2.Rx(i, 3).x, int2.Rx(i, 3).y), Is.EqualTo((3, 2)));
            Assert.That((int2.Ry(i, 3).x, int2.Ry(i, 3).y), Is.EqualTo((1, 3)));
            Assert.That((short2.Rx(s, 3).x, short2.Rx(s, 3).y), Is.EqualTo(((short)3, (short)2)));
            Assert.That(((bool)b32v2.Rx(t, false).x, (bool)b32v2.Rx(t, false).y), Is.EqualTo((false, false)));
            Assert.That(float2s.Rx(st, 3), Is.EqualTo(new float2s(3, 2)));
        }
    }

    [Test]
    public void Components3()
    {
        var f = new float3(1, 2, 3);
        var d = new double3(1, 2, 3);
        var u = new uint3(1, 2, 3);
        var h = new half3((Half)1, (Half)2, (Half)3);
        var t = new b32v3(true, false, true);
        var st = new float3s(1, 2, 3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.Rx(f, 4), Is.EqualTo(new float3(4, 2, 3)));
            Assert.That(float3.Ry(f, 4), Is.EqualTo(new float3(1, 4, 3)));
            Assert.That(float3.Rz(f, 4), Is.EqualTo(new float3(1, 2, 4)));
            Assert.That(float3.Rxy(f, new float2(4, 5)), Is.EqualTo(new float3(4, 5, 3)));
            Assert.That(float3.Ryz(f, new float2(4, 5)), Is.EqualTo(new float3(1, 4, 5)));
            Assert.That(float3.Rxz(f, new float2(4, 5)), Is.EqualTo(new float3(4, 2, 5)));
            Assert.That((double3.Rz(d, 4).z, double3.Rx(d, 4).x), Is.EqualTo((4d, 4d)));
            Assert.That((uint3.Ry(u, 4u).x, uint3.Ry(u, 4u).y, uint3.Ry(u, 4u).z), Is.EqualTo((1u, 4u, 3u)));
            Assert.That(half3.Rz(h, (Half)4), Is.EqualTo(new half3((Half)1, (Half)2, (Half)4)));
            Assert.That(((bool)b32v3.Rz(t, false).z, (bool)b32v3.Rxy(t, new b32v2(false, true)).x),
                Is.EqualTo((false, false)));
            Assert.That(float3s.Ryz(st, new float2(4, 5)), Is.EqualTo(new float3s(1, 4, 5)));
        }
    }

    [Test]
    public void Components4()
    {
        var f = new float4(1, 2, 3, 4);
        var d = new double4(1, 2, 3, 4);
        var l = new long4(1, 2, 3, 4);
        var h = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var t = new b64v4(true, false, true, false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float4.Rx(f, 5), Is.EqualTo(new float4(5, 2, 3, 4)));
            Assert.That(float4.Ry(f, 5), Is.EqualTo(new float4(1, 5, 3, 4)));
            Assert.That(float4.Rz(f, 5), Is.EqualTo(new float4(1, 2, 5, 4)));
            Assert.That(float4.Rw(f, 5), Is.EqualTo(new float4(1, 2, 3, 5)));
            Assert.That(float4.Rxy(f, new float2(5, 6)), Is.EqualTo(new float4(5, 6, 3, 4)));
            Assert.That(float4.Ryz(f, new float2(5, 6)), Is.EqualTo(new float4(1, 5, 6, 4)));
            Assert.That(float4.Rzw(f, new float2(5, 6)), Is.EqualTo(new float4(1, 2, 5, 6)));
            Assert.That(float4.Rxz(f, new float2(5, 6)), Is.EqualTo(new float4(5, 2, 6, 4)));
            Assert.That(float4.Ryw(f, new float2(5, 6)), Is.EqualTo(new float4(1, 5, 3, 6)));
            Assert.That(float4.Rxw(f, new float2(5, 6)), Is.EqualTo(new float4(5, 2, 3, 6)));
            Assert.That(float4.Rxyz(f, new float3(5, 6, 7)), Is.EqualTo(new float4(5, 6, 7, 4)));
            Assert.That(float4.Ryzw(f, new float3(5, 6, 7)), Is.EqualTo(new float4(1, 5, 6, 7)));
            Assert.That(float4.Rxyw(f, new float3(5, 6, 7)), Is.EqualTo(new float4(5, 6, 3, 7)));
            Assert.That(float4.Rxzw(f, new float3(5, 6, 7)), Is.EqualTo(new float4(5, 2, 6, 7)));
            Assert.That(double4.Rzw(d, new double2(5, 6)), Is.EqualTo(new double4(1, 2, 5, 6)));
            Assert.That((long4.Ryzw(l, new long3(5, 6, 7)).x, long4.Ryzw(l, new long3(5, 6, 7)).y),
                Is.EqualTo((1L, 5L)));
            Assert.That(half4.Rxw(h, new half2((Half)5, (Half)6)), Is.EqualTo(new half4((Half)5, (Half)2, (Half)3, (Half)6)));
            Assert.That((bool)b64v4.Rxw(t, new b64v2(true, true)).w, Is.True);
        }
    }
}
