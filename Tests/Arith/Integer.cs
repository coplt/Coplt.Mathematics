using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Experimental.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Arith;

/// <summary>
/// The integer members of a vector implement <c>IVectorInteger</c> and, for a vector that has no sign,
/// <c>IVectorUnsignedInteger</c>: the check of a power of two that produces a bool vector of the same shape and
/// the rounding up to the next power of two of every component. The values keep the ones of the legacy
/// implementation.
/// </summary>
public class TestInteger
{
    /// <summary>
    /// Every integer vector implements the check of a power of two, so it is reachable through the interface and
    /// the mask of it is the bool vector of the shape of the vector.
    /// </summary>
    private static void Check<T, TBool>(T v)
        where T : unmanaged, IVectorInteger<T, TBool>
        where TBool : unmanaged
    {
        var mask = T.is_pow2(v);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mask.Equals(T.is_pow2(v)), Is.True);
            Assert.That(Unsafe.SizeOf<TBool>(), Is.GreaterThan(0));
        }
    }

    /// <summary>
    /// A vector without a sign also has the rounding up to the next power of two, which the interface of the
    /// plain integer vector does not declare because it is not meaningful for a negative value.
    /// </summary>
    private static void CheckUnsigned<T, TBool>(T v)
        where T : unmanaged, IVectorUnsignedInteger<T, TBool>
        where TBool : unmanaged
    {
        Check<T, TBool>(v);
        _ = T.up2pow2(v);
    }

    [Test]
    public void Interface()
    {
        // the type of a single component and the type of the mask cannot be inferred from the vector, they have
        // to be spelled out
        Check<short2, b16v2>(new short2(1, 2));
        Check<short3, b16v3>(new short3(1, 2, 3));
        Check<short4, b16v4>(new short4(1, 2, 3, 4));
        Check<int2, b32v2>(new int2(1, 2));
        Check<int3, b32v3>(new int3(1, 2, 3));
        Check<int4, b32v4>(new int4(1, 2, 3, 4));
        Check<long2, b64v2>(new long2(1, 2));
        Check<long3, b64v3>(new long3(1, 2, 3));
        Check<long4, b64v4>(new long4(1, 2, 3, 4));

        CheckUnsigned<ushort2, b16v2>(new ushort2(1, 2));
        CheckUnsigned<ushort3, b16v3>(new ushort3(1, 2, 3));
        CheckUnsigned<ushort4, b16v4>(new ushort4(1, 2, 3, 4));
        CheckUnsigned<uint2, b32v2>(new uint2(1, 2));
        CheckUnsigned<uint3, b32v3>(new uint3(1, 2, 3));
        CheckUnsigned<uint4, b32v4>(new uint4(1, 2, 3, 4));
        CheckUnsigned<ulong2, b64v2>(new ulong2(1, 2));
        CheckUnsigned<ulong3, b64v3>(new ulong3(1, 2, 3));
        CheckUnsigned<ulong4, b64v4>(new ulong4(1, 2, 3, 4));

        // the storage variant of a vector implements the same interface as the regular one
        Check<int2s, b32v2>(new int2s(1, 2));
        Check<int3s, b32v3>(new int3s(1, 2, 3));
        Check<long3s, b64v3>(new long3s(1, 2, 3));
        CheckUnsigned<uint2s, b32v2>(new uint2s(1, 2));
        CheckUnsigned<uint3s, b32v3>(new uint3s(1, 2, 3));
        CheckUnsigned<ulong3s, b64v3>(new ulong3s(1, 2, 3));
    }

    [Test]
    public void IsPow2()
    {
        var v = new int3(1, 2, 3);
        var m = v.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            // one is a power of two
            Assert.That((bool)m.x, Is.True);
            Assert.That((bool)m.y, Is.True);
            Assert.That((bool)m.z, Is.False);
        }

        // a zero and a negative value are not a power of two
        var n = new int3(0, -4, int.MinValue);
        var nm = n.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)nm.x, Is.False);
            Assert.That((bool)nm.y, Is.False);
            Assert.That((bool)nm.z, Is.False);
        }

        // a value without a sign that has the top bit set is a power of two and a zero still is not
        var u = new uint3(0x8000_0000u, 0u, 3u);
        var um = u.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)um.x, Is.True);
            Assert.That((bool)um.y, Is.False);
            Assert.That((bool)um.z, Is.False);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)new short3(1, 2, 4).is_pow2().z, Is.True);
            Assert.That((bool)new short3(0, 3, -1).is_pow2().z, Is.False);
            Assert.That((bool)new long3(1L, 0x4000_0000_0000_0000L, 6L).is_pow2().y, Is.True);
            Assert.That((bool)new int2s(3, 4).is_pow2().y, Is.True);
            Assert.That((bool)new ulong3s(3UL, 8UL, ulong.MaxValue).is_pow2().y, Is.True);
        }
    }

    [Test]
    public void Up2Pow2()
    {
        using (Assert.EnterMultipleScope())
        {
            // the values between two powers of two are rounded up to the next one
            Assert.That(new uint3(3u, 5u, 8u).up2pow2(), Is.EqualTo(new uint3(4u, 8u, 8u)));
            Assert.That(new uint3(1u, 2u, 4u).up2pow2(), Is.EqualTo(new uint3(1u, 2u, 4u)));
            // a zero stays zero
            Assert.That(new uint3(0u, 9u, 17u).up2pow2(), Is.EqualTo(new uint3(0u, 16u, 32u)));
            // the value that needs the last shift of the width of the component
            Assert.That(new ushort3(9).up2pow2(), Is.EqualTo(new ushort3(16)));
            Assert.That(new uint3(65_537u).up2pow2(), Is.EqualTo(new uint3(131_072u)));
            Assert.That(new ulong3(0x1_0000_0001UL).up2pow2(), Is.EqualTo(new ulong3(0x2_0000_0000UL)));
            // a value above the highest power of two wraps around to zero
            Assert.That(new uint3(uint.MaxValue).up2pow2(), Is.EqualTo(new uint3(0u)));
            Assert.That(new ulong3(ulong.MaxValue).up2pow2(), Is.EqualTo(new ulong3(0UL)));
        }

        var v = new uint2s(3u, 5u);
        var d = new ulong3s(3UL, 5UL, 17UL);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.up2pow2(), Is.EqualTo(new uint2s(4u, 8u)));
            Assert.That(d.up2pow2(), Is.EqualTo(new ulong3s(4UL, 8UL, 32UL)));
        }
    }

    /// <summary>
    /// The members are built from the operators of the vector, so the padding lanes of a simd register are kept
    /// at zero by them as well.
    /// </summary>
    [Test]
    public void PaddingStaysZero()
    {
        var v = new uint3(3u, 5u, 9u);
        var l = new ulong3(3UL, 5UL, 9UL);
        var n = new uint2(3u, 9u);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.up2pow2().vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(l.up2pow2().vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(n.up2pow2().vector.GetElement(2), Is.EqualTo(0u));
            Assert.That(n.up2pow2().vector.GetElement(3), Is.EqualTo(0u));
        }
    }

    /// <summary>
    /// The members are checked over the values that are at a boundary of the operation.
    /// </summary>
    [Test]
    public void Values()
    {
        uint[] values =
        [
            0u, 1u, 2u, 3u, 4u, 5u, 7u, 8u, 9u, 15u, 16u, 17u, 31u, 32u, 63u, 64u, 65u,
            255u, 256u, 1000u, 4095u, 4096u, 65_535u, 65_536u, 0x4000_0000u, 0x8000_0000u, uint.MaxValue,
        ];

        foreach (var a in values)
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That((bool)new uint3(a).is_pow2().x, Is.EqualTo(IsPow2(a)), $"is_pow2({a})");
                Assert.That(new uint3(a).up2pow2().x, Is.EqualTo(Up2Pow2(a)), $"up2pow2({a})");
            }
        }

        int[] signed = [0, 1, 2, 3, 4, -1, -2, -3, -4, int.MinValue, int.MaxValue, 1_000_000];

        foreach (var a in signed)
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That((bool)new int3(a).is_pow2().x, Is.EqualTo(a > 0 && (a & (a - 1)) == 0), $"is_pow2({a})");
            }
        }
    }

    /// <summary>
    /// A power of two has exactly one bit set, zero is not one of them.
    /// </summary>
    private static bool IsPow2(uint a) => a != 0 && (a & (a - 1)) == 0;

    /// <summary>
    /// Rounds up to the next power of two, zero stays zero and a value above the top wraps to zero.
    /// </summary>
    private static uint Up2Pow2(uint a)
    {
        if (a == 0) return 0;
        var x = a - 1;
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        return x + 1;
    }
}
