using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using static Coplt.Mathematics.math;

namespace Tests.Core;

/// <summary>
/// The select of a value is hand written and it is a mux of the bits of the two values with the mask: the bit of
/// the result at a position is the bit of the one of the two values that the bit of the mask at that position
/// holds, so the all bits set value of the kind takes the whole of the one of them and the all bits zero value
/// of it takes the whole of the other one, which is what a comparison of two values builds. The member of the
/// <c>math</c> class and the one that is called on a value are the two forms of the same operation and the three
/// values of a call have the same type.
/// </summary>
public class TestSelect
{
    /// <summary>
    /// Checks the two forms of the select of a value, the mask and the two values have the same type.
    /// </summary>
    private static void Check<T, TScalar>(T t, T f, T c, T expected)
        where T : unmanaged, IAlgebraDispatch<T>
    {
        Assert.That(select(c, t, f).Equals(expected), Is.True, "the static form");
        Assert.That(t.select(c, f).Equals(expected), Is.True, "the form on a value");
    }

    [Test]
    public void Mask()
    {
        var t = new float2(1, 2);
        var f = new float2(3, 4);
        // the comparison operators of a value produce a mask of its own kind
        var c = t < f;
        Assert.That(select(c, t, f), Is.EqualTo(t));
        Assert.That(t.select(c, f), Is.EqualTo(t));
        c = t > f;
        Assert.That(t.select(c, f), Is.EqualTo(f));
    }

    [Test]
    public void Register()
    {
        Check<float2, float>(new float2(1, 2), new float2(3, 4), new float2(0, 1) < new float2(1, 1), new float2(1, 4));
        Check<float4, float>(new(1, 2, 3, 4), new(5, 6, 7, 8), new float4(0, 1, 0, 1) < new float4(1, 1, 1, 1), new(1, 6, 3, 8));
        Check<double2, double>(new(1, 2), new(3, 4), new double2(1, 0) < new double2(1, 1), new(3, 2));
        Check<double3, double>(new(1, 2, 3), new(4, 5, 6), new double3(0, 1, 0) < new double3(1, 1, 1), new(1, 5, 3));
        Check<int4, int>(new(1, 2, 3, 4), new(5, 6, 7, 8), new int4(1, 0, 1, 0) < new int4(1, 1, 1, 1), new(5, 2, 7, 4));
        Check<uint2, uint>(new(1u, 2u), new(3u, 4u), new uint2(0, 1) < new uint2(1, 1), new(1u, 4u));
        Check<ulong2, ulong>(new(1ul, 2ul), new(3ul, 4ul), new ulong2(1, 1) < new ulong2(1, 1), new(3ul, 4ul));
    }

    /// <summary>
    /// A value that has no register selects its components one by one.
    /// </summary>
    [Test]
    public void Component()
    {
        Check<half2, Half>(new((Half)1, (Half)2), new((Half)3, (Half)4),
            new half2((Half)0, (Half)1) < new half2((Half)1, (Half)1), new((Half)1, (Half)4));
        Check<short3, short>(new(1, 2, 3), new(4, 5, 6), new short3(1, 0, 1) < new short3(1, 1, 1), new(4, 2, 6));
        Check<ushort2, ushort>(new(1, 2), new(3, 4), new ushort2(0, 0) < new ushort2(1, 1), new(1, 2));
    }

    /// <summary>
    /// The padding lanes of the register of a 2 component value are selected by the zero mask of them, so they
    /// stay zero.
    /// </summary>
    [Test]
    public void PaddingStaysZero()
    {
        var selected = new float2(1, 2).select(new float2(0, 1) < new float2(1, 1), new float2(3, 4));
        Assert.That(selected.vector.GetElement(2), Is.EqualTo(0f));
        Assert.That(selected.vector.GetElement(3), Is.EqualTo(0f));
    }
}
