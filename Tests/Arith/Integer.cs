using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Arith;

/// <summary>
/// The integer members of a vector implement <c>IVectorInteger</c> and, for a vector that has no sign,
/// <c>IVectorUnsignedInteger</c>: the check of a power of two and the rounding up to the next power of two of
/// every component. The value of the check is a value of the kind of the vector itself, the all bits set value
/// of a kind is its true component and a zero is its false one, so a component that holds is not zero and a
/// component that does not is zero. The values keep the ones of the legacy implementation.
/// </summary>
public class TestInteger
{
    /// <summary>
    /// Every integer vector implements the check of a power of two, so it is reachable through the interface and
    /// the value of it has the kind of the vector itself.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IVectorInteger<T>
    {
        var mask = T.is_pow2(v);
        Assert.That(mask.Equals(T.is_pow2(v)), Is.True);
    }

    /// <summary>
    /// A vector without a sign also has the rounding up to the next power of two, which the interface of the
    /// plain integer vector does not declare because it is not meaningful for a negative value.
    /// </summary>
    private static void CheckUnsigned<T>(T v)
        where T : unmanaged, IVectorUnsignedInteger<T>
    {
        Check(v);
        _ = T.up2pow2(v);
    }

    [Test]
    public void Interface()
    {
        Check(new short2(1, 2));
        Check(new short3(1, 2, 3));
        Check(new short4(1, 2, 3, 4));
        Check(new int2(1, 2));
        Check(new int3(1, 2, 3));
        Check(new int4(1, 2, 3, 4));
        Check(new long2(1, 2));
        Check(new long3(1, 2, 3));
        Check(new long4(1, 2, 3, 4));

        CheckUnsigned(new ushort2(1, 2));
        CheckUnsigned(new ushort3(1, 2, 3));
        CheckUnsigned(new ushort4(1, 2, 3, 4));
        CheckUnsigned(new uint2(1, 2));
        CheckUnsigned(new uint3(1, 2, 3));
        CheckUnsigned(new uint4(1, 2, 3, 4));
        CheckUnsigned(new ulong2(1, 2));
        CheckUnsigned(new ulong3(1, 2, 3));
        CheckUnsigned(new ulong4(1, 2, 3, 4));
    }

    [Test]
    public void IsPow2()
    {
        var v = new int3(1, 2, 3);
        var m = v.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            // one is a power of two
            Assert.That(m.x != 0, Is.True);
            Assert.That(m.y != 0, Is.True);
            Assert.That(m.z != 0, Is.False);
        }

        // a zero and a negative value are not a power of two
        var n = new int3(0, -4, int.MinValue);
        var nm = n.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(nm.x != 0, Is.False);
            Assert.That(nm.y != 0, Is.False);
            Assert.That(nm.z != 0, Is.False);
        }

        // a value without a sign that has the top bit set is a power of two and a zero still is not
        var u = new uint3(0x8000_0000u, 0u, 3u);
        var um = u.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(um.x != 0, Is.True);
            Assert.That(um.y != 0, Is.False);
            Assert.That(um.z != 0, Is.False);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(new short3(1, 2, 4).is_pow2().z != 0, Is.True);
            Assert.That(new short3(0, 3, -1).is_pow2().z != 0, Is.False);
            Assert.That(new long3(1L, 0x4000_0000_0000_0000L, 6L).is_pow2().y != 0, Is.True);
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
                Assert.That(new uint3(a).is_pow2().x != 0, Is.EqualTo(IsPow2(a)), $"is_pow2({a})");
                Assert.That(new uint3(a).up2pow2().x, Is.EqualTo(Up2Pow2(a)), $"up2pow2({a})");
            }
        }

        int[] signed = [0, 1, 2, 3, 4, -1, -2, -3, -4, int.MinValue, int.MaxValue, 1_000_000];

        foreach (var a in signed)
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(new int3(a).is_pow2().x != 0, Is.EqualTo(a > 0 && (a & (a - 1)) == 0), $"is_pow2({a})");
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
