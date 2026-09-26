using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

/// <summary>
/// Checks the conversions between the vectors of the same size. A component of a float vector is converted and
/// the other ones only keep the bits of the same width, the padding lanes of a result that has them stay zero.
/// </summary>
public class TestVectorConversions
{
    [Test]
    public void Implicit()
    {
        long2 l2 = new int2(1, 2);
        long4 l4 = new int4(1, 2, 3, 4);
        ulong2 ul2 = new uint2(1, 2);
        float2 f2 = new int2(1, 2);
        float3 f3 = new half3((Half)1, (Half)2, (Half)3);
        double2 d2 = new float2(1, 2);
        double3 d3 = new int3(1, 2, 3);
        double4 d4 = new uint4(1, 2, 3, 4);
        float3s s3 = new int3s(1, 2, 3);
        double3s sd3 = new float3s(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((l2.x, l2.y), Is.EqualTo((1L, 2L)));
            Assert.That((l4.x, l4.y, l4.z, l4.w), Is.EqualTo((1L, 2L, 3L, 4L)));
            Assert.That((ul2.x, ul2.y), Is.EqualTo((1UL, 2UL)));
            Assert.That((f2.x, f2.y), Is.EqualTo((1f, 2f)));
            Assert.That(f3.x, Is.EqualTo(1f));
            Assert.That(f3.y, Is.EqualTo(2f));
            Assert.That(f3.z, Is.EqualTo(3f));
            Assert.That((d2.x, d2.y), Is.EqualTo((1d, 2d)));
            Assert.That((d3.x, d3.y, d3.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((d4.x, d4.y, d4.z, d4.w), Is.EqualTo((1d, 2d, 3d, 4d)));
            Assert.That((s3.x, s3.y, s3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((sd3.x, sd3.y, sd3.z), Is.EqualTo((1d, 2d, 3d)));
        }
    }

    [Test]
    public void Explicit()
    {
        // a component of a float vector is converted toward zero, the other ones keep the bits of their value
        int2 i2 = (int2)new float2(1.9f, -2.9f);
        int3 i3 = (int3)new double3(1.9, -2.9, 3.9);
        uint2 u2 = (uint2)new int2(-1, 2);
        int2 ni2 = (int2)new uint2(4294967295, 2);
        ulong2 ul2 = (ulong2)new int2(-1, 2);
        long2 nl2 = (long2)new ulong2(18446744073709551615UL, 2);
        float2 f2 = (float2)new double2(1.5, -2.5);
        half2 h2 = (half2)new int2(1, 2);
        int2 bi2 = (int2)new b32v2(B32.True, B32.False);
        int2 si2 = (int2)new b16v2(B16.True, B16.False);
        float2 bf2 = (float2)new b32v2(B32.True, B32.False);
        b64v2 bb2 = (b64v2)new b32v2(B32.True, B32.False);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((i2.x, i2.y), Is.EqualTo((1, -2)));
            Assert.That((i3.x, i3.y, i3.z), Is.EqualTo((1, -2, 3)));
            Assert.That((u2.x, u2.y), Is.EqualTo((4294967295u, 2u)));
            Assert.That((ni2.x, ni2.y), Is.EqualTo((-1, 2)));
            Assert.That((ul2.x, ul2.y), Is.EqualTo((unchecked((ulong)(-1L)), 2UL)));
            Assert.That((nl2.x, nl2.y), Is.EqualTo((-1L, 2L)));
            Assert.That(f2.x, Is.EqualTo(1.5f));
            Assert.That(f2.y, Is.EqualTo(-2.5f));
            Assert.That(h2.x, Is.EqualTo((Half)1));
            Assert.That(h2.y, Is.EqualTo((Half)2));
            Assert.That((bi2.x, bi2.y), Is.EqualTo((-1, 0)));
            // the component of a bool vector that has no register is cast as the value of its bits
            Assert.That((si2.x, si2.y), Is.EqualTo((65535, 0)));
            Assert.That((bf2.x, bf2.y), Is.EqualTo((4294967296f, 0f)));
            Assert.That(((bool)bb2.x, (bool)bb2.y), Is.EqualTo((true, false)));
        }
    }

    [Test]
    public void StorageVariant()
    {
        int2s i2 = (int2s)new float2s(1.9f, -2.9f);
        int3s i3 = (int3s)new float3s(1.9f, -2.9f, 3.9f);
        uint3s u3 = (uint3s)new int3s(-1, 2, 3);
        double3s d3 = new int3s(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((i2.x, i2.y), Is.EqualTo((1, -2)));
            Assert.That((i3.x, i3.y, i3.z), Is.EqualTo((1, -2, 3)));
            Assert.That((u3.x, u3.y, u3.z), Is.EqualTo((4294967295u, 2u, 3u)));
            Assert.That((d3.x, d3.y, d3.z), Is.EqualTo((1d, 2d, 3d)));
        }
    }

    [Test]
    public void Padding()
    {
        var i3 = (int3)new float3(1, 2, 3);
        var l3 = (long3)new int3(1, 2, 3);
        var d3 = (double3)new float3(1, 2, 3);
        var i2 = (int2)new float2(1, 2);
        var l2 = (long2)new int2(1, 2);
        var d2 = (double2)new int2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Unsafe.As<int3, Vector128<int>>(ref i3).GetElement(3), Is.EqualTo(0));
            Assert.That(Unsafe.As<long3, Vector256<long>>(ref l3).GetElement(3), Is.EqualTo(0L));
            Assert.That(Unsafe.As<double3, Vector256<double>>(ref d3).GetElement(3), Is.EqualTo(0d));
            Assert.That(Unsafe.As<int2, Vector128<int>>(ref i2).GetElement(2), Is.EqualTo(0));
            Assert.That(Unsafe.As<int2, Vector128<int>>(ref i2).GetElement(3), Is.EqualTo(0));
            Assert.That(Unsafe.As<long2, Vector128<long>>(ref l2).GetElement(1), Is.EqualTo(2L));
            Assert.That(Unsafe.As<double2, Vector128<double>>(ref d2).GetElement(1), Is.EqualTo(2d));
        }
    }
}
