using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;
using static Coplt.Mathematics.math;

namespace Tests.Core;

/// <summary>
/// The select of a vector implements <c>IVectorSelect</c>: a mask selects the components of two vectors that
/// have its own shape. The member that is called on a vector and the static member that the interface declares
/// are the two forms of the same operation and both type parameters of the interface are inferred from the
/// arguments of the forwarding of the <c>math</c> class.
/// </summary>
public class TestSelect
{
    /// <summary>
    /// Checks the two forms of the select of a vector and the forwarding of the <c>math</c> class. The interface
    /// cannot be named without its type parameters, so the caller passes them.
    /// </summary>
    private static void Check<T, B, TScalar>(T t, T f, B c, T expected)
        where T : unmanaged, IVector<T, TScalar>, IVectorSelect<T, B>
        where B : unmanaged
        where TScalar : unmanaged
    {
        Assert.That(T.select(c, t, f).Equals(expected), Is.True);
        Assert.That(t.select(c, f).Equals(expected), Is.True);
        Assert.That(select(c, t, f).Equals(expected), Is.True);
    }

    [Test]
    public void Mask()
    {
        var t = new float2(1, 2);
        var f = new float2(3, 4);
        // the comparison operators of a vector produce the mask of its shape
        var c = t < f;
        Assert.That(c, Is.EqualTo(new b32v2(true, true)));
        Assert.That(t.select(c, f), Is.EqualTo(t));
        c = t > f;
        Assert.That(t.select(c, f), Is.EqualTo(f));
    }

    [Test]
    public void Register()
    {
        Check<float2, b32v2, float>(new(1, 2), new(3, 4), new b32v2(true, false), new(1, 4));
        Check<float4, b32v4, float>(new(1, 2, 3, 4), new(5, 6, 7, 8), new b32v4(true, false, true, false), new(1, 6, 3, 8));
        Check<double2, b64v2, double>(new(1, 2), new(3, 4), new b64v2(false, true), new(3, 2));
        Check<double3, b64v3, double>(new(1, 2, 3), new(4, 5, 6), new b64v3(true, false, true), new(1, 5, 3));
        Check<int4, b32v4, int>(new(1, 2, 3, 4), new(5, 6, 7, 8), new b32v4(false, true, false, true), new(5, 2, 7, 4));
        Check<uint2, b32v2, uint>(new(1, 2), new(3, 4), new b32v2(true, false), new(1u, 4u));
        Check<ulong2, b64v2, ulong>(new(1, 2), new(3, 4), new b64v2(false, false), new(3ul, 4ul));
    }

    /// <summary>
    /// The value of a 64 bit vector is widened to the 128 bit register of its mask, the storage variant keeps
    /// only the lower half of the selected register.
    /// </summary>
    [Test]
    public void StorageVariant()
    {
        Check<float2s, b32v2, float>(new(1, 2), new(3, 4), new b32v2(true, false), new(1, 4));
        Check<int2s, b32v2, int>(new(1, 2), new(3, 4), new b32v2(false, true), new(3, 2));
        Check<uint2s, b32v2, uint>(new(1, 2), new(3, 4), new b32v2(false, false), new(3u, 4u));
        Check<float3s, b32v3, float>(new(1, 2, 3), new(4, 5, 6), new b32v3(true, false, true), new(1, 5, 3));
        Check<double3s, b64v3, double>(new(1, 2, 3), new(4, 5, 6), new b64v3(true, false, false), new(1, 5, 6));
    }

    /// <summary>
    /// A vector that has no register and a bool vector select their components one by one, a bool vector is
    /// selected with a mask of its own type.
    /// </summary>
    [Test]
    public void Component()
    {
        Check<half2, b16v2, Half>(new((Half)1, (Half)2), new((Half)3, (Half)4), new b16v2(true, false), new((Half)1, (Half)4));
        Check<short3, b16v3, short>(new(1, 2, 3), new(4, 5, 6), new b16v3(false, true, false), new(4, 2, 6));
        Check<ushort2, b16v2, ushort>(new(1, 2), new(3, 4), new b16v2(true, true), new(1, 2));
        Check<int3s, b32v3, int>(new(1, 2, 3), new(4, 5, 6), new b32v3(true, false, true), new(1, 5, 3));
        Check<b32v3, b32v3, b32>(
            new b32v3(true, false, true), new b32v3(false, true, false), new b32v3(true, true, false),
            new b32v3(true, false, false));
        Check<b16v2, b16v2, b16>(new b16v2(true, true), new b16v2(false, false), new b16v2(true, false), new b16v2(true, false));
    }

    /// <summary>
    /// The padding lanes of the register of a 2 component vector are selected by the zero mask of them, so they
    /// stay zero.
    /// </summary>
    [Test]
    public void PaddingStaysZero()
    {
        var selected = new float2(1, 2).select(new b32v2(true, false), new float2(3, 4));
        Assert.That(selected.vector.GetElement(2), Is.EqualTo(0f));
        Assert.That(selected.vector.GetElement(3), Is.EqualTo(0f));
        var widened = new float2s(1, 2).select(new b32v2(false, true), new float2s(3, 4));
        Assert.That(widened.x, Is.EqualTo(3f));
        Assert.That(widened.y, Is.EqualTo(2f));
    }
}
