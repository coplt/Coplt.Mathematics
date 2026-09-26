using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// A vector implements the interface of its size, which names the size of the vector, the members every vector
/// has and the components it reaches by name: the component interfaces declare the <c>xyzw</c> spelling of a
/// component and the <c>rgba</c> one, and the two of them reach the same component. The size is a type of its
/// own as well, so generic code can constrain the shape of a vector without naming its components, and every
/// member of the interfaces is static, so generic code reaches the components with the static form.
/// </summary>
public class TestVectorComponentInterfaces
{
    /// <summary>
    /// Writes every component of <paramref name="start"/> through the interface of the components and reads
    /// them back: the <c>rgba</c> spelling of a component is written and the <c>xyzw</c> one is read.
    /// </summary>
    private static (S x, S y, S z, S w) RoundTrip4<T, S>(T start, S x, S y, S z, S w)
        where T : unmanaged, IVector4Components<T, S>
        where S : unmanaged
    {
        var v = start;
        T.set_x(ref v, x);
        T.set_y(ref v, y);
        T.set_z(ref v, z);
        T.set_w(ref v, w);
        // the rgba spelling of a component reaches the same one
        T.set_a(ref v, T.get_w(v));
        T.set_b(ref v, T.get_z(v));
        T.set_g(ref v, T.get_y(v));
        T.set_r(ref v, T.get_x(v));
        return (T.get_x(v), T.get_y(v), T.get_z(v), T.get_w(v));
    }

    /// <summary>
    /// Writes every component of <paramref name="start"/> through the interface of the components and reads
    /// them back: the <c>rgba</c> spelling of a component is written and the <c>xyzw</c> one is read.
    /// </summary>
    private static (S x, S y, S z) RoundTrip3<T, S>(T start, S x, S y, S z)
        where T : unmanaged, IVector3Components<T, S>
        where S : unmanaged
    {
        var v = start;
        T.set_x(ref v, x);
        T.set_y(ref v, y);
        T.set_z(ref v, z);
        T.set_b(ref v, T.get_z(v));
        T.set_g(ref v, T.get_y(v));
        T.set_r(ref v, T.get_x(v));
        return (T.get_x(v), T.get_y(v), T.get_z(v));
    }

    /// <summary>
    /// Writes every component of <paramref name="start"/> through the interface of the components and reads
    /// them back: the <c>rgba</c> spelling of a component is written and the <c>xyzw</c> one is read.
    /// </summary>
    private static (S x, S y) RoundTrip2<T, S>(T start, S x, S y)
        where T : unmanaged, IVector2Components<T, S>
        where S : unmanaged
    {
        var v = start;
        T.set_x(ref v, x);
        T.set_y(ref v, y);
        T.set_g(ref v, T.get_y(v));
        T.set_r(ref v, T.get_x(v));
        return (T.get_x(v), T.get_y(v));
    }

    [Test]
    public void Components2()
    {
        var b = RoundTrip2<b32v2, b32>(new b32v2(), true, false);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(RoundTrip2(new float2(), 1f, 2f), Is.EqualTo((1f, 2f)));
            Assert.That(RoundTrip2(new double2(), 1d, 2d), Is.EqualTo((1d, 2d)));
            Assert.That(RoundTrip2(new int2(), 1, 2), Is.EqualTo((1, 2)));
            Assert.That(RoundTrip2(new half2(), (Half)1, (Half)2), Is.EqualTo(((Half)1, (Half)2)));
            Assert.That(RoundTrip2(new ushort2(), (ushort)1, (ushort)2), Is.EqualTo(((ushort)1, (ushort)2)));
            // the storage variant of a vector reaches its components as well
            Assert.That(RoundTrip2(new float2s(), 1f, 2f), Is.EqualTo((1f, 2f)));
            // a component of a bool vector is a mask
            Assert.That(((bool)b.x, (bool)b.y), Is.EqualTo((true, false)));
        }
    }

    [Test]
    public void Components3()
    {
        var b = RoundTrip3<b64v3, b64>(new b64v3(), true, false, true);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(RoundTrip3(new float3(), 1f, 2f, 3f), Is.EqualTo((1f, 2f, 3f)));
            Assert.That(RoundTrip3(new double3(), 1d, 2d, 3d), Is.EqualTo((1d, 2d, 3d)));
            Assert.That(RoundTrip3(new int3(), 1, 2, 3), Is.EqualTo((1, 2, 3)));
            Assert.That(RoundTrip3(new half3(), (Half)1, (Half)2, (Half)3), Is.EqualTo(((Half)1, (Half)2, (Half)3)));
            // a storage variant of a vector reaches its components as well, a 3 component one keeps them in
            // fields even when the regular vector has a register
            Assert.That(RoundTrip3(new float3s(), 1f, 2f, 3f), Is.EqualTo((1f, 2f, 3f)));
            Assert.That(RoundTrip3(new double3s(), 1d, 2d, 3d), Is.EqualTo((1d, 2d, 3d)));
            Assert.That(((bool)b.x, (bool)b.y, (bool)b.z), Is.EqualTo((true, false, true)));
        }
    }

    [Test]
    public void Components4()
    {
        var b = RoundTrip4<b16v4, b16>(new b16v4(), true, false, true, false);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(RoundTrip4(new float4(), 1f, 2f, 3f, 4f), Is.EqualTo((1f, 2f, 3f, 4f)));
            Assert.That(RoundTrip4(new double4(), 1d, 2d, 3d, 4d), Is.EqualTo((1d, 2d, 3d, 4d)));
            Assert.That(RoundTrip4(new long4(), 1L, 2L, 3L, 4L), Is.EqualTo((1L, 2L, 3L, 4L)));
            Assert.That(RoundTrip4(new ulong4(), 1UL, 2UL, 3UL, 4UL), Is.EqualTo((1UL, 2UL, 3UL, 4UL)));
            Assert.That(RoundTrip4(new short4(), (short)1, (short)2, (short)3, (short)4),
                Is.EqualTo(((short)1, (short)2, (short)3, (short)4)));
            Assert.That(RoundTrip4(new half4(), (Half)1, (Half)2, (Half)3, (Half)4),
                Is.EqualTo(((Half)1, (Half)2, (Half)3, (Half)4)));
            Assert.That(((bool)b.x, (bool)b.y, (bool)b.z, (bool)b.w), Is.EqualTo((true, false, true, false)));
        }
    }

    /// <summary>
    /// The shape of a vector of 2 components and its two components together, so a member can constrain both
    /// of them at once.
    /// </summary>
    private static (int Length, S x, S y) Size2<T, S>(T v)
        where T : unmanaged, IVector2<T, S>
        where S : unmanaged => (T.Length, T.get_x(v), T.get_y(v));

    /// <summary>
    /// The shape of a vector of 4 components and its four components together, so a member can constrain both
    /// of them at once.
    /// </summary>
    private static (int Length, S x, S y, S z, S w) Size4<T, S>(T v)
        where T : unmanaged, IVector4<T, S>
        where S : unmanaged => (T.Length, T.get_x(v), T.get_y(v), T.get_z(v), T.get_w(v));

    /// <summary>
    /// The size of a vector whose components are not known, the constraint of the member only names the shape
    /// of the vector.
    /// </summary>
    private static int Length4<T>(T v) where T : unmanaged, IVector4<T> => T.Length;

    [Test]
    public void Size()
    {
        // the component type of a member is a part of the constraint of the member of the size, it cannot be
        // inferred from its arguments
        var b = Size4<b32v4, b32>(new b32v4(true, false, true, false));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Size2<float2, float>(new float2(1, 2)), Is.EqualTo((2, 1f, 2f)));
            Assert.That(Size2<half2, Half>(new half2((Half)1, (Half)2)), Is.EqualTo((2, (Half)1, (Half)2)));
            Assert.That(Size4<float4, float>(new float4(1, 2, 3, 4)), Is.EqualTo((4, 1f, 2f, 3f, 4f)));
            Assert.That(Size4<double4, double>(new double4(1, 2, 3, 4)), Is.EqualTo((4, 1d, 2d, 3d, 4d)));
            // the size of a vector does not need the type of its components
            Assert.That(b.Length, Is.EqualTo(4));
            Assert.That(((bool)b.x, (bool)b.y, (bool)b.z, (bool)b.w), Is.EqualTo((true, false, true, false)));
            Assert.That(Length4(new float4(1, 2, 3, 4)), Is.EqualTo(4));
            Assert.That(Length4(new short4(1, 2, 3, 4)), Is.EqualTo(4));
            Assert.That(Length4(new half4((Half)1, (Half)2, (Half)3, (Half)4)), Is.EqualTo(4));
        }
    }
}
