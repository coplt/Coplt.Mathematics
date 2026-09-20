using System.Runtime.Intrinsics;
using Coplt.Experimental.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

/// <summary>
/// The bool vector specifics, the bitwise operators themselves are in <see cref="TestVectorBitwise"/>.
/// </summary>
public class TestBoolVector
{
    [Test]
    public void Constants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(b16v2.True, Is.EqualTo(new b16v2(B16.True, B16.True)));
            Assert.That(b16v3.True, Is.EqualTo(new b16v3(B16.True, B16.True, B16.True)));
            Assert.That(b16v4.True, Is.EqualTo(new b16v4(B16.True, B16.True, B16.True, B16.True)));
            Assert.That(b16v2.False, Is.EqualTo(default(b16v2)));
            Assert.That(b16v3.False, Is.EqualTo(default(b16v3)));
            Assert.That(b16v4.False, Is.EqualTo(default(b16v4)));

            Assert.That(b32v2.True, Is.EqualTo(new b32v2(B32.True, B32.True)));
            Assert.That(b32v3.True, Is.EqualTo(new b32v3(B32.True, B32.True, B32.True)));
            Assert.That(b32v4.True, Is.EqualTo(new b32v4(B32.True, B32.True, B32.True, B32.True)));
            Assert.That(b32v2.False, Is.EqualTo(default(b32v2)));
            Assert.That(b32v4.False, Is.EqualTo(default(b32v4)));

            Assert.That(b64v2.True, Is.EqualTo(new b64v2(B64.True, B64.True)));
            Assert.That(b64v3.True, Is.EqualTo(new b64v3(B64.True, B64.True, B64.True)));
            Assert.That(b64v4.True, Is.EqualTo(new b64v4(B64.True, B64.True, B64.True, B64.True)));
            Assert.That(b64v3.False, Is.EqualTo(default(b64v3)));
            Assert.That(b64v4.False, Is.EqualTo(default(b64v4)));

            Assert.That((b16v2.Length, b16v2.SizeByte, b16v2.IsSimdAccelerated), Is.EqualTo((2, 4, false)));
            Assert.That((b16v4.Length, b16v4.SizeByte, b16v4.IsSimdAccelerated), Is.EqualTo((4, 8, false)));
            Assert.That((b32v2.Length, b32v2.SizeByte, b32v2.IsSimdAccelerated), Is.EqualTo((2, 8, true)));
            Assert.That((b32v4.Length, b32v4.SizeByte, b32v4.IsSimdAccelerated), Is.EqualTo((4, 16, true)));
            Assert.That((b64v2.Length, b64v2.SizeByte, b64v2.IsSimdAccelerated), Is.EqualTo((2, 16, true)));
            Assert.That((b64v4.Length, b64v4.SizeByte, b64v4.IsSimdAccelerated), Is.EqualTo((4, 32, true)));
        }
    }

    [Test]
    public void Equality()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(b16v2.True.Equals(b16v2.True), Is.True);
            Assert.That(b16v3.True.Equals(b16v3.False), Is.False);
            Assert.That(b16v4.False.Equals(default(b16v4)), Is.True);

            Assert.That(b32v2.True.Equals(b32v2.True), Is.True);
            Assert.That(b32v3.True.Equals(b32v3.False), Is.False);
            Assert.That(b32v4.True.Equals((object)b32v4.True), Is.True);
            Assert.That(b32v4.True.Equals(null), Is.False);
            Assert.That(b32v2.True.GetHashCode(), Is.EqualTo(b32v2.Broadcast(B32.True).GetHashCode()));
            Assert.That(b32v4.False.Equals(default(b32v4)), Is.True);

            Assert.That(b64v2.True.Equals(b64v2.True), Is.True);
            Assert.That(b64v3.True.Equals(b64v3.False), Is.False);
            Assert.That(b64v4.False.Equals(default(b64v4)), Is.True);

            // the components are compared, not the bits
            Assert.That(new b32v2(B32.True, B32.False).Equals(new b32v2(B32.True, B32.True)), Is.False);
            Assert.That(new b64v3(B64.True, B64.False, B64.True).Equals(new b64v3(B64.True, B64.False, B64.True)), Is.True);
        }
    }

    [Test]
    public void ComparisonReturnsItself()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(b16v2.True == b16v2.False, Is.TypeOf<b16v2>());
            Assert.That(b16v3.True != b16v3.False, Is.TypeOf<b16v3>());
            Assert.That(b16v4.True >= b16v4.False, Is.TypeOf<b16v4>());
            Assert.That(b32v2.True == b32v2.False, Is.TypeOf<b32v2>());
            Assert.That(b32v3.True != b32v3.False, Is.TypeOf<b32v3>());
            Assert.That(b32v4.True >= b32v4.False, Is.TypeOf<b32v4>());
            Assert.That(b64v2.True == b64v2.False, Is.TypeOf<b64v2>());
            Assert.That(b64v3.True != b64v3.False, Is.TypeOf<b64v3>());
            Assert.That(b64v4.True >= b64v4.False, Is.TypeOf<b64v4>());
        }

        var t = b32v3.True;
        var tCopy = b32v3.True;
        var f = b32v3.False;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(t == tCopy, Is.EqualTo(b32v3.True));
            Assert.That(t == f, Is.EqualTo(b32v3.False));
            Assert.That(t != tCopy, Is.EqualTo(b32v3.False));
            Assert.That(t != f, Is.EqualTo(b32v3.True));
            // a mask is compared as an unsigned integer, all bits set is the largest
            Assert.That(f < t, Is.EqualTo(b32v3.True));
            Assert.That(t < f, Is.EqualTo(b32v3.False));
            Assert.That(t > f, Is.EqualTo(b32v3.True));
            Assert.That(f <= t, Is.EqualTo(b32v3.True));
            Assert.That(t >= f, Is.EqualTo(b32v3.True));
        }

        var s = new b16v2(B16.True, B16.False);
        var s2 = new b16v2(B16.False, B16.True);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(s == s2, Is.EqualTo(b16v2.False));
            Assert.That(s != s2, Is.EqualTo(b16v2.True));
            Assert.That(s < s2, Is.EqualTo(new b16v2(B16.False, B16.True)));
        }

        var q = new b64v2(B64.True, B64.False);
        var qCopy = new b64v2(B64.True, B64.False);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(q == qCopy, Is.EqualTo(b64v2.True));
            Assert.That(q < qCopy, Is.EqualTo(b64v2.False));
        }
    }

    [Test]
    public void Components()
    {
        var e4 = new b16v4(B16.True, B16.False, B16.True, B16.False);
        var t4 = new b32v4(B32.True, B32.False, B32.True, B32.False);
        var q4 = new b64v4(B64.True, B64.False, B64.True, B64.False);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((e4.x, e4.y, e4.z, e4.w), Is.EqualTo((B16.True, B16.False, B16.True, B16.False)));
            Assert.That((e4.r, e4.g, e4.b, e4.a), Is.EqualTo((B16.True, B16.False, B16.True, B16.False)));
            Assert.That((t4.x, t4.y, t4.z, t4.w), Is.EqualTo((B32.True, B32.False, B32.True, B32.False)));
            Assert.That((t4.r, t4.g, t4.b, t4.a), Is.EqualTo((B32.True, B32.False, B32.True, B32.False)));
            Assert.That((q4.x, q4.y, q4.z, q4.w), Is.EqualTo((B64.True, B64.False, B64.True, B64.False)));
            Assert.That((q4.r, q4.g, q4.b, q4.a), Is.EqualTo((B64.True, B64.False, B64.True, B64.False)));
            Assert.That(t4[0], Is.EqualTo(B32.True));
            Assert.That(t4[1], Is.EqualTo(B32.False));
            Assert.That(e4[2], Is.EqualTo(B16.True));
            Assert.That(q4[3], Is.EqualTo(B64.False));
        }

        t4.a = B32.True;
        t4[1] = B32.True;
        e4.w = B16.True;
        q4.b = B64.False;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((t4.y, t4.w), Is.EqualTo((B32.True, B32.True)));
            Assert.That((bool)e4.w, Is.True);
            Assert.That((bool)q4.z, Is.False);
        }

        var (x2, y2) = new b32v2(B32.True, B32.False);
        Assert.That((x2, y2), Is.EqualTo((B32.True, B32.False)));
        var (x3, y3, z3) = new b64v3(B64.True, B64.False, B64.True);
        Assert.That((x3, y3, z3), Is.EqualTo((B64.True, B64.False, B64.True)));
        var (x4, y4, z4, w4) = new b16v4(B16.True, B16.False, B16.True, B16.False);
        Assert.That((x4, y4, z4, w4), Is.EqualTo((B16.True, B16.False, B16.True, B16.False)));

        var b2 = new b16v2(B16.True);
        Assert.That((b2.r, b2.g), Is.EqualTo((B16.True, B16.True)));
        Assert.That((bool)b2[1], Is.True);
    }

    [Test]
    public void ComesFromANumericComparison()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(1, 2) < new float2(3, 2), Is.EqualTo(new b32v2(B32.True, B32.False)));
            Assert.That(new float3(1, 2, 3) >= new float3(1, 2, 4), Is.EqualTo(new b32v3(B32.True, B32.True, B32.False)));
            Assert.That(new float4(1, 2, 3, 4) == new float4(0, 2, 0, 4), Is.EqualTo(new b32v4(B32.False, B32.True, B32.False, B32.True)));
            Assert.That(new double2(1, 2) <= new double2(0, 2), Is.EqualTo(new b64v2(B64.False, B64.True)));
            Assert.That(new double3(1, 2, 3) != new double3(1, 0, 3), Is.EqualTo(new b64v3(B64.False, B64.True, B64.False)));
            Assert.That(new short2(1, 2) > new short2(2, 2), Is.EqualTo(new b16v2(B16.False, B16.False)));
            Assert.That(new short3(1, 2, 3) < new short3(2, 3, 4), Is.EqualTo(new b16v3(B16.True, B16.True, B16.True)));
            Assert.That(new ushort4(1, 2, 3, 4) == new ushort4(1, 2, 3, 4), Is.EqualTo(new b16v4(B16.True, B16.True, B16.True, B16.True)));
            Assert.That(new int2(1, 2) >= new int2(2, 2), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That(new uint3(1, 2, 3) < new uint3(1, 2, 4), Is.EqualTo(new b32v3(B32.False, B32.False, B32.True)));
            Assert.That(new long2(1, 2) != new long2(1, 2), Is.EqualTo(new b64v2(B64.False, B64.False)));
            Assert.That(new ulong4(1, 2, 3, 4) <= new ulong4(1, 2, 3, 3), Is.EqualTo(new b64v4(B64.True, B64.True, B64.True, B64.False)));
            Assert.That(new half2((Half)1, (Half)2) > new half2((Half)0, (Half)2), Is.EqualTo(new b16v2(B16.True, B16.False)));
            Assert.That(new half3((Half)1, (Half)2, (Half)3) == new half3((Half)1, (Half)2, (Half)3), Is.EqualTo(new b16v3(B16.True, B16.True, B16.True)));
        }

        // a mask can be used as the operand of the bitwise operators
        var m = new float4(1, 2, 3, 4) > new float4(0, 3, 3, 5);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(m & new b32v4(B32.True, B32.True, B32.False, B32.False),
                Is.EqualTo(new b32v4(B32.True, B32.False, B32.False, B32.False)));
            Assert.That(m | b32v4.False, Is.EqualTo(m));
            Assert.That((bool)(~m).y, Is.True);
        }
    }

    [Test]
    public void PaddingLaneStaysZero()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(b16v3.True.z, Is.EqualTo(B16.True));
            Assert.That(b32v3.True.vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((b32v3.True & b32v3.True).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((~b32v3.True).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((b32v3.True | b32v3.False).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((b32v3.True == b32v3.False).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((b32v3.True >= b32v3.False).vector.GetElement(3), Is.EqualTo(0u));

            Assert.That(b64v3.True.vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That((b64v3.True | b64v3.False).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That((b64v3.True != b64v3.False).vector.GetElement(3), Is.EqualTo(0UL));

            // the components of a 3 component mask are still readable
            Assert.That((bool)b32v3.True.x, Is.True);
            Assert.That((bool)b32v3.True.y, Is.True);
            Assert.That((bool)b64v3.True.z, Is.True);
        }
    }
}
