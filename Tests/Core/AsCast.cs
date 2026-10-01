using System.Runtime.CompilerServices;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The as members of a vector reinterpret the bits of it as the vector of another component type of the same
/// width: the members of a group cover the bits of every member of it, so a round trip through any of them keeps
/// every component. A 16 bit group has <c>asf</c> beside <c>asi</c>, the
/// regular vectors of a size reach the other regular ones and the vector is marked by the interface of the kind
/// of every one of them. The regular vector and its storage variant convert into each other with <c>to_storage</c> and
/// <c>to_compute</c> and keep the components of the value.
/// </summary>
public class TestAsCast
{
    /// <summary>
    /// The members of a group keep the bits of every one of them, so all of them are as wide as the vector.
    /// </summary>
    private static void CheckAs<T, F, I, U>()
        where T : unmanaged, IVectorAsF<T, F>, IVectorAsI<T, I>, IVectorAsU<T, U>
    {
        // the interface of every kind of the group marks the type of the member of that kind
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Unsafe.SizeOf<F>(), Is.EqualTo(Unsafe.SizeOf<T>()));
            Assert.That(Unsafe.SizeOf<I>(), Is.EqualTo(Unsafe.SizeOf<T>()));
            Assert.That(Unsafe.SizeOf<U>(), Is.EqualTo(Unsafe.SizeOf<T>()));
        }
    }

    [Test]
    public void AsInterfaces()
    {
        CheckAs<float3, float3, int3, uint3>();
        CheckAs<float2, float2, int2, uint2>();
        CheckAs<short2, half2, short2, ushort2>();
        CheckAs<double2, double2, long2, ulong2>();
    }

    [Test]
    public void MathForwarding()
    {
        // a generic member of the math class is constrained by the interface of the kind of the target vector,
        // so it reaches the target vector from every member of its group
        var f = math.asf(new int2(0x3F800000, 0x40000000));
        var h = math.asf(new short2(0x3C00, 0x4000));
        var i = math.asi(new float2(1, 2));
        var u = math.asu(new float2(1, 2));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f.x, f.y), Is.EqualTo((1f, 2f)));
            Assert.That(((float)h.x, (float)h.y), Is.EqualTo((1f, 2f)));
            Assert.That((i.x, i.y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((u.x, u.y), Is.EqualTo((0x3F800000u, 0x40000000u)));
        }
    }

    [Test]
    public void MathGenericForwarding()
    {
        // the type of the result of the double generic member of the math class cannot be inferred from the
        // source vector by the compiler of today, so it has to be spelled out
        var f = math.asf<int2, float2>(new int2(0x3F800000, 0x40000000));
        var h = math.asf<short2, half2>(new short2(0x3C00, 0x4000));
        var u = math.asu<float3, uint3>(new float3(1, 2, 3));

        // the spelling of the intrinsic of HLSL reaches the same member
        var hf = math.asfloat<int2, float2>(new int2(0x3F800000, 0x40000000));
        var hi = math.asint<float2, int2>(new float2(1, 2));
        var hu = math.asuint<float3, uint3>(new float3(1, 2, 3));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f.x, f.y), Is.EqualTo((1f, 2f)));
            Assert.That(((float)h.x, (float)h.y), Is.EqualTo((1f, 2f)));
            Assert.That((u.x, u.y, u.z), Is.EqualTo((0x3F800000u, 0x40000000u, 0x40400000u)));
            Assert.That((hf.x, hf.y), Is.EqualTo((1f, 2f)));
            Assert.That((hi.x, hi.y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((hu.x, hu.y, hu.z), Is.EqualTo((0x3F800000u, 0x40000000u, 0x40400000u)));
        }
    }

    [Test]
    public void Float2()
    {
        var v = new float2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi.x, v.asi.y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((v.asu.x, v.asu.y), Is.EqualTo((0x3F800000u, 0x40000000u)));
            Assert.That((v.asi.asf.x, v.asi.asf.y), Is.EqualTo((1f, 2f)));
            Assert.That((v.asu.asf.x, v.asu.asf.y), Is.EqualTo((1f, 2f)));
        }
    }

    [Test]
    public void Short2()
    {
        var v = new short2(-1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asu.x, v.asu.y), Is.EqualTo((ushort.MaxValue, (ushort)2)));
            Assert.That(Unsafe.SizeOf<half2>(), Is.EqualTo(Unsafe.SizeOf<short2>()));
            // the 16 bit group is covered by the bits of every one of its members as well
            Assert.That((v.asf.asi.x, v.asf.asi.y), Is.EqualTo((-1, 2)));
            Assert.That((v.asf.asi.x, v.asf.asi.y), Is.EqualTo((-1, 2)));
        }
    }

    [Test]
    public void Float3()
    {
        var v = new float3(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi.x, v.asi.y, v.asi.z), Is.EqualTo((0x3F800000, 0x40000000, 0x40400000)));
        }
    }

    [Test]
    public void Double2()
    {
        var v = new double2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi.x, v.asi.y), Is.EqualTo((0x3FF0000000000000L, 0x4000000000000000L)));
            Assert.That((v.asu.asf.x, v.asu.asf.y), Is.EqualTo((1d, 2d)));
        }
    }

    [Test]
    public void ToStorage()
    {
        var v = new float3(1, 2, 3);
        var s = v.to_storage;
        var back = s.to_compute;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((s.x, s.y, s.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((back.x, back.y, back.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That(Unsafe.SizeOf<float3s>(), Is.EqualTo(Unsafe.SizeOf<float3>()));
        }
    }

    [Test]
    public void ToStorageNarrower()
    {
        var i = new int2(-1, 2).to_storage;
        var d = new double3(1, 2, 3).to_storage;
        var l = new long3(-1, 2, 3).to_storage.to_compute;

        using (Assert.EnterMultipleScope())
        {
            // the storage variant of a 2 component vector keeps the exact 64 bits of its value
            Assert.That(Unsafe.SizeOf<int2s>(), Is.EqualTo(8));
            Assert.That((i.to_compute.x, i.to_compute.y), Is.EqualTo((-1, 2)));
            Assert.That((d.to_compute.x, d.to_compute.y, d.to_compute.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((l.x, l.y, l.z), Is.EqualTo((-1L, 2L, 3L)));
        }
    }

    /// <summary>
    /// A vector of 3 components and one of 4 components convert into each other with <c>as3</c> and <c>as4</c>
    /// and into the 2 component one with <c>as2</c>: the two of them keep their components in a register of the
    /// same width as the one of the 2 component vector, so the components that the target vector does not hold
    /// are zero.
    /// </summary>
    [Test]
    public void SizeConversion()
    {
        var f3 = new float3(1, 2, 3);
        var d3 = new double3(1, 2, 3);
        var l3 = new long3(1, 2, 3);
        var s3 = new short3(1, 2, 3);
        var h3 = new half3((Half)1, (Half)2, (Half)3);

        using (Assert.EnterMultipleScope())
        {
            // the components behind the second one are the padding lanes of the 2 component vector
            Assert.That(new float4(1, 2, 3, 4).as2, Is.EqualTo(new float2(1, 2)));
            Assert.That(f3.as2, Is.EqualTo(new float2(1, 2)));
            Assert.That(d3.as2, Is.EqualTo(new double2(1, 2)));
            Assert.That(d3.as4.as2, Is.EqualTo(new double2(1, 2)));
            Assert.That(l3.as2, Is.EqualTo(new long2(1, 2)));
            Assert.That(s3.as2, Is.EqualTo(new short2(1, 2)));
            Assert.That(h3.as2, Is.EqualTo(new half2((Half)1, (Half)2)));
            // the 4 component vector that a 3 component one converts into has a w component of zero
            var f4 = f3.as4;
            Assert.That((f4.x, f4.y, f4.z, f4.w), Is.EqualTo((1f, 2f, 3f, 0f)));
            Assert.That((d3.as4.x, d3.as4.w), Is.EqualTo((1d, 0d)));
            Assert.That((l3.as4.z, l3.as4.w), Is.EqualTo((3L, 0L)));
            Assert.That((s3.as4.z, s3.as4.w), Is.EqualTo(((short)3, (short)0)));
            Assert.That((h3.as4.z, h3.as4.w), Is.EqualTo(((Half)3, (Half)0)));
            Assert.That(h3.as4, Is.EqualTo(new half4((Half)1, (Half)2, (Half)3, (Half)0)));
            // the component that the 3 component vector cannot hold is dropped
            Assert.That(f4.as3, Is.EqualTo(f3));
            Assert.That(new float4(1, 2, 3, 4).as3, Is.EqualTo(f3));
            Assert.That(d3.as4.as3, Is.EqualTo(d3));
            Assert.That(l3.as4.as3, Is.EqualTo(l3));
            Assert.That(s3.as4.as3, Is.EqualTo(s3));
            Assert.That(h3.as4.as3, Is.EqualTo(h3));
        }
    }
}
