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
    private static void Check<T, TScalar, TBool>(T v)
        where T : unmanaged, IVectorInteger<T, TScalar, TBool>
        where TScalar : unmanaged
        where TBool : unmanaged
    {
        var mask = v.is_pow2();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mask.Equals(v.is_pow2()), Is.True);
            Assert.That(Unsafe.SizeOf<TBool>(), Is.GreaterThan(0));
        }
    }

    /// <summary>
    /// A vector without a sign also has the rounding up to the next power of two, which the interface of the
    /// plain integer vector does not declare because it is not meaningful for a negative value.
    /// </summary>
    private static void CheckUnsigned<T, TScalar, TBool>(T v)
        where T : unmanaged, IVectorUnsignedInteger<T, TScalar, TBool>
        where TScalar : unmanaged
        where TBool : unmanaged
    {
        Check<T, TScalar, TBool>(v);
        _ = v.up2pow2();
    }

    [Test]
    public void Interface()
    {
        // the type of a single component and the type of the mask cannot be inferred from the vector, they have
        // to be spelled out
        Check<short2, short, b16v2>(new short2(1, 2));
        Check<short3, short, b16v3>(new short3(1, 2, 3));
        Check<short4, short, b16v4>(new short4(1, 2, 3, 4));
        Check<int2, int, b32v2>(new int2(1, 2));
        Check<int3, int, b32v3>(new int3(1, 2, 3));
        Check<int4, int, b32v4>(new int4(1, 2, 3, 4));
        Check<long2, long, b64v2>(new long2(1, 2));
        Check<long3, long, b64v3>(new long3(1, 2, 3));
        Check<long4, long, b64v4>(new long4(1, 2, 3, 4));

        CheckUnsigned<ushort2, ushort, b16v2>(new ushort2(1, 2));
        CheckUnsigned<ushort3, ushort, b16v3>(new ushort3(1, 2, 3));
        CheckUnsigned<ushort4, ushort, b16v4>(new ushort4(1, 2, 3, 4));
        CheckUnsigned<uint2, uint, b32v2>(new uint2(1, 2));
        CheckUnsigned<uint3, uint, b32v3>(new uint3(1, 2, 3));
        CheckUnsigned<uint4, uint, b32v4>(new uint4(1, 2, 3, 4));
        CheckUnsigned<ulong2, ulong, b64v2>(new ulong2(1, 2));
        CheckUnsigned<ulong3, ulong, b64v3>(new ulong3(1, 2, 3));
        CheckUnsigned<ulong4, ulong, b64v4>(new ulong4(1, 2, 3, 4));

        // the storage variant of a vector implements the same interface as the regular one
        Check<int2s, int, b32v2>(new int2s(1, 2));
        Check<int3s, int, b32v3>(new int3s(1, 2, 3));
        Check<long3s, long, b64v3>(new long3s(1, 2, 3));
        CheckUnsigned<uint2s, uint, b32v2>(new uint2s(1, 2));
        CheckUnsigned<uint3s, uint, b32v3>(new uint3s(1, 2, 3));
        CheckUnsigned<ulong3s, ulong, b64v3>(new ulong3s(1, 2, 3));
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
    /// The members have to keep the values of the legacy implementation, which is checked against it directly.
    /// </summary>
    [Test]
    public void MatchesTheLegacyImplementation()
    {
        uint[] values =
        [
            0u, 1u, 2u, 3u, 4u, 5u, 7u, 8u, 9u, 15u, 16u, 17u, 31u, 32u, 63u, 64u, 65u,
            255u, 256u, 1000u, 4095u, 4096u, 65_535u, 65_536u, 0x4000_0000u, 0x8000_0000u, uint.MaxValue,
        ];

        foreach (var a in values)
        {
            var current = new uint3(a).is_pow2();
            var legacy = Coplt.Mathematics.math.isPow2(new Coplt.Mathematics.uint3(a));
            var currentUp = new uint3(a).up2pow2();
            var legacyUp = Coplt.Mathematics.math.up2pow2(new Coplt.Mathematics.uint3(a));

            using (Assert.EnterMultipleScope())
            {
                Assert.That((bool)current.x, Is.EqualTo((bool)legacy.x), $"is_pow2({a})");
                Assert.That(currentUp.x, Is.EqualTo(legacyUp.x), $"up2pow2({a})");
            }
        }

        int[] signed = [0, 1, 2, 3, 4, -1, -2, -3, -4, int.MinValue, int.MaxValue, 1_000_000];

        foreach (var a in signed)
        {
            var current = new int3(a).is_pow2();
            var legacy = Coplt.Mathematics.math.isPow2(new Coplt.Mathematics.int3(a));

            using (Assert.EnterMultipleScope())
            {
                Assert.That((bool)current.x, Is.EqualTo((bool)legacy.x), $"is_pow2({a})");
            }
        }
    }
}
