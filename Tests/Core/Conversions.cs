using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;

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

/// <summary>
/// Checks the conversions between the matrices of the same shape. Every column of a matrix is converted by the
/// conversion of the vector that the column is, so a component of a float matrix is converted and the one of a
/// matrix that only changes the width of a component keeps the bits of it. The storage variant of a matrix
/// converts into the storage variants of the same shape alone.
/// </summary>
public class TestMatrixConversions
{
    [Test]
    public void Implicit()
    {
        double2x2 d2 = new float2x2(new float2(1, 2), new float2(3, 4));
        double3x3 d3 = new int3x3(new int3(1, 2, 3), new int3(4, 5, 6), new int3(7, 8, 9));
        long3x2 l3 = new int3x2(new int3(1, 2, 3), new int3(4, 5, 6));
        using (Assert.EnterMultipleScope())
        {
            Assert.That((d2.c0.x, d2.c0.y, d2.c1.x, d2.c1.y), Is.EqualTo((1d, 2d, 3d, 4d)));
            Assert.That((d3.c0.x, d3.c2.z), Is.EqualTo((1d, 9d)));
            Assert.That((l3.c0.z, l3.c1.x), Is.EqualTo((3L, 4L)));
        }
    }

    [Test]
    public void Explicit()
    {
        var i2 = (int2x2)new float2x2(new float2(1.9f, -2.9f), new float2(3.9f, -4.9f));
        var u3 = (uint3x3)new int3x3(new int3(-1, 2, 3), new int3(4, 5, 6), new int3(7, 8, 9));
        var h2 = (half2x2)new float2x2(new float2(1, 2), new float2(3, 4));
        using (Assert.EnterMultipleScope())
        {
            // the component of the float matrix is converted toward zero
            Assert.That((i2.c0.x, i2.c0.y, i2.c1.x, i2.c1.y), Is.EqualTo((1, -2, 3, -4)));
            Assert.That((u3.c0.x, u3.c2.z), Is.EqualTo((4294967295u, 9u)));
            Assert.That((h2.c0.x, h2.c1.y), Is.EqualTo(((Half)1, (Half)4)));
        }
    }

    [Test]
    public void StorageVariant()
    {
        var i2 = (int2x2s)new float2x2s(new float2s(1.9f, -2.9f), new float2s(3.9f, -4.9f));
        var i3 = (int3x3s)new float3x3s(new float3s(1.9f, -2.9f, 3.9f), new float3s(4, 5, 6),
            new float3s(7, 8, 9));
        using (Assert.EnterMultipleScope())
        {
            Assert.That((i2.c0.x, i2.c0.y, i2.c1.x, i2.c1.y), Is.EqualTo((1, -2, 3, -4)));
            Assert.That((i3.c0.x, i3.c0.y, i3.c0.z), Is.EqualTo((1, -2, 3)));
        }
    }
}
