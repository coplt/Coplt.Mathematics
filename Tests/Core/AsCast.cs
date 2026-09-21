using System.Runtime.CompilerServices;
using Coplt.Experimental.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The as members of a vector reinterpret the bits of it as the vector of another component type of the same
/// width: the members of a group cover the bits of every member of it, so a round trip through any of them keeps
/// every component. A 16 bit group has <c>asf</c> / <c>as_half</c> beside <c>asi</c> / <c>as_short</c>, the
/// regular vectors of a size reach the other regular ones and a storage variant the other storage variants,
/// every member implements the interface of its own kind. The regular vector and its storage variant convert
/// into each other with <c>to_storage</c> and <c>to_compute</c> and keep the components of the value.
/// </summary>
public class TestAsCast
{
    /// <summary>
    /// The members of a group keep the bits of every one of them, so all of them are as wide as the vector. A
    /// type whose group has no bool vector of its own width does not implement the bool interface, so only the
    /// types that have one are checked with the four interfaces.
    /// </summary>
    private static void CheckAs<T, F, I, U, B>(T v)
        where T : IVectorAsF<F>, IVectorAsI<I>, IVectorAsU<U>, IVectorAsB<B>
    {
        v.asf();
        v.asi();
        v.asu();
        v.asb();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(Unsafe.SizeOf<F>(), Is.EqualTo(Unsafe.SizeOf<T>()));
            Assert.That(Unsafe.SizeOf<I>(), Is.EqualTo(Unsafe.SizeOf<T>()));
            Assert.That(Unsafe.SizeOf<U>(), Is.EqualTo(Unsafe.SizeOf<T>()));
            Assert.That(Unsafe.SizeOf<B>(), Is.EqualTo(Unsafe.SizeOf<T>()));
        }
    }

    [Test]
    public void AsInterfaces()
    {
        CheckAs<float3, float3, int3, uint3, b32v3>(new float3(1, 2, 3));
        CheckAs<float2, float2, int2, uint2, b32v2>(new float2(1, 2));
        CheckAs<short2, half2, short2, ushort2, b16v2>(new short2(-1, 2));
        CheckAs<double2, double2, long2, ulong2, b64v2>(new double2(1, 2));
        CheckAs<double3s, double3s, long3s, ulong3s, b64v3>(new double3s(1, 2, 3));

        // a 2 component storage variant has no bool member, no bool vector of its own 8 bytes exists
        var v = new float2s(1, 2);
        IVectorAsF<float2s> f = v;
        IVectorAsI<int2s> i = v;
        IVectorAsU<uint2s> u = v;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f.asf().x, f.asf().y), Is.EqualTo((1f, 2f)));
            Assert.That((i.asi().x, i.asi().y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((u.asu().x, u.asu().y), Is.EqualTo((0x3F800000u, 0x40000000u)));
        }
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
        var b = math.asb(new short2(-1, 2));
        var s = math.asf(new float2s(1, 2));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f.x, f.y), Is.EqualTo((1f, 2f)));
            Assert.That(((float)h.x, (float)h.y), Is.EqualTo((1f, 2f)));
            Assert.That((i.x, i.y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((u.x, u.y), Is.EqualTo((0x3F800000u, 0x40000000u)));
            Assert.That((bool)b.x, Is.True);
            Assert.That((bool)b.y, Is.True);
            Assert.That((s.x, s.y), Is.EqualTo((1f, 2f)));
        }
    }

    [Test]
    public void MathGenericForwarding()
    {
        // the type of the result of the double generic member of the math class cannot be inferred from the
        // source vector by the compiler of today, so it has to be spelled out
        var f = math.asf<int2, float2>(new int2(0x3F800000, 0x40000000));
        var h = math.asf<short2, half2>(new short2(0x3C00, 0x4000));
        var i = math.asi<float2s, int2s>(new float2s(1, 2));
        var u = math.asu<float3, uint3>(new float3(1, 2, 3));
        var b = math.asb<double3s, b64v3>(new double3s(1, 2, 3));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f.x, f.y), Is.EqualTo((1f, 2f)));
            Assert.That(((float)h.x, (float)h.y), Is.EqualTo((1f, 2f)));
            Assert.That((i.x, i.y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((u.x, u.y, u.z), Is.EqualTo((0x3F800000u, 0x40000000u, 0x40400000u)));
            Assert.That((bool)b.x, Is.True);
        }
    }

    [Test]
    public void Float2()
    {
        var v = new float2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi().x, v.asi().y), Is.EqualTo((0x3F800000, 0x40000000)));
            Assert.That((v.asu().x, v.asu().y), Is.EqualTo((0x3F800000u, 0x40000000u)));
            Assert.That(Unsafe.SizeOf<b32v2>(), Is.EqualTo(Unsafe.SizeOf<float2>()));
            Assert.That((v.asi().as_float().x, v.asi().as_float().y), Is.EqualTo((1f, 2f)));
            Assert.That((v.asu().asf().x, v.asu().asf().y), Is.EqualTo((1f, 2f)));
            Assert.That((v.asb().as_float().x, v.asb().as_float().y), Is.EqualTo((1f, 2f)));
        }
    }

    [Test]
    public void Short2()
    {
        var v = new short2(-1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asu().x, v.asu().y), Is.EqualTo((ushort.MaxValue, (ushort)2)));
            Assert.That(Unsafe.SizeOf<half2>(), Is.EqualTo(Unsafe.SizeOf<short2>()));
            // the 16 bit group is covered by the bits of every one of its members as well
            Assert.That((v.asf().asi().x, v.asf().asi().y), Is.EqualTo((-1, 2)));
            Assert.That((v.as_half().as_short().x, v.as_half().as_short().y), Is.EqualTo((-1, 2)));
            // the b16 vector of the group keeps the truth of the bits of a value that is not zero
            Assert.That((bool)v.as_ushort().asb().x, Is.True);
            Assert.That((bool)v.as_ushort().asb().y, Is.True);
        }
    }

    [Test]
    public void Float3()
    {
        var v = new float3(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi().x, v.asi().y, v.asi().z), Is.EqualTo((0x3F800000, 0x40000000, 0x40400000)));
            Assert.That((v.asb().asf().x, v.asb().asf().y, v.asb().asf().z), Is.EqualTo((1f, 2f, 3f)));
        }
    }

    [Test]
    public void Double2()
    {
        var v = new double2(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi().x, v.asi().y), Is.EqualTo((0x3FF0000000000000L, 0x4000000000000000L)));
            Assert.That((v.asu().as_double().x, v.asu().as_double().y), Is.EqualTo((1d, 2d)));
            Assert.That(Unsafe.SizeOf<b64v2>(), Is.EqualTo(Unsafe.SizeOf<double2>()));
        }
    }

    [Test]
    public void StorageFloat2()
    {
        var v = new float2s(1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asf().x, v.asf().y), Is.EqualTo((1f, 2f)));
            Assert.That((v.asu().x, v.asu().y), Is.EqualTo((0x3F800000u, 0x40000000u)));
            Assert.That(Unsafe.SizeOf<int2s>(), Is.EqualTo(Unsafe.SizeOf<float2s>()));
            Assert.That((v.asi().as_float().x, v.asi().as_float().y), Is.EqualTo((1f, 2f)));
            Assert.That((v.as_uint().asf().x, v.as_uint().asf().y), Is.EqualTo((1f, 2f)));
        }
    }

    [Test]
    public void StorageFloat3()
    {
        var v = new float3s(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asi().x, v.asi().y, v.asi().z), Is.EqualTo((0x3F800000, 0x40000000, 0x40400000)));
            Assert.That(Unsafe.SizeOf<uint3s>(), Is.EqualTo(Unsafe.SizeOf<float3s>()));
            Assert.That((v.as_int().asf().x, v.as_int().asf().y, v.as_int().asf().z), Is.EqualTo((1f, 2f, 3f)));
            // the bool vector has no storage variant, the storage variant reaches the regular one
            Assert.That((v.asb().asf().x, v.asb().asf().y, v.asb().asf().z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That(Unsafe.SizeOf<b32v3>(), Is.EqualTo(Unsafe.SizeOf<float3s>()));
        }
    }

    [Test]
    public void StorageDouble3()
    {
        var v = new double3s(1, 2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((v.asu().x, v.asu().y, v.asu().z), Is.EqualTo((
                0x3FF0000000000000UL, 0x4000000000000000UL, 0x4008000000000000UL)));
            Assert.That(Unsafe.SizeOf<long3s>(), Is.EqualTo(Unsafe.SizeOf<double3s>()));
            Assert.That((v.as_ulong().as_double().x, v.as_ulong().as_double().y), Is.EqualTo((1d, 2d)));
        }
    }

    [Test]
    public void ToStorage()
    {
        var v = new float3(1, 2, 3);
        var s = v.to_storage();
        var back = s.to_compute();

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
        var i = new int2(-1, 2).to_storage();
        var d = new double3(1, 2, 3).to_storage();
        var l = new long3(-1, 2, 3).to_storage().to_compute();

        using (Assert.EnterMultipleScope())
        {
            // the storage variant of a 2 component vector keeps the exact 64 bits of its value
            Assert.That(Unsafe.SizeOf<int2s>(), Is.EqualTo(8));
            Assert.That((i.to_compute().x, i.to_compute().y), Is.EqualTo((-1, 2)));
            Assert.That((d.to_compute().x, d.to_compute().y, d.to_compute().z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((l.x, l.y, l.z), Is.EqualTo((-1L, 2L, 3L)));
        }
    }
}
