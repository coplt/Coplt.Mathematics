using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The counting of the bits of a value: the count of a component is the one of the bits of the kind of it, so a
/// component of sixteen bits holds a number up to the sixteen of them, and the value of no bit set of the kind
/// holds the whole width of it as the count of the zero bits of either end.
/// </summary>
public class TestBitCount
{
    /// <summary>
    /// The count of the set bits of the value of a component of every whole number kind, in the form of the class
    /// and in the form on a value.
    /// </summary>
    [Test]
    public void PopCount()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.popcnt(new int4(0, 1, 3, -1)), Is.EqualTo(new int4(0, 1, 2, 32)), "the value of an int");
            Assert.That(math.popcnt(new uint4(0u, 0xF0F0u, 0xFFFFu, uint.MaxValue)),
                Is.EqualTo(new uint4(0u, 8u, 16u, 32u)), "the value of a uint");
            Assert.That(math.popcnt(new long2(0L, -1L)), Is.EqualTo(new long2(0L, 64L)), "the value of a long");
            Assert.That(math.popcnt(new ulong2(0xFFFFUL, ulong.MaxValue)), Is.EqualTo(new ulong2(16UL, 64UL)),
                "the value of a ulong");

            // the kind of a component of sixteen bits holds a count up to the sixteen of them
            Assert.That(math.popcnt(new short4(0, 1, -1, 3)), Is.EqualTo(new short4(0, 1, 16, 2)),
                "the value of a short");
            Assert.That(math.popcnt(new ushort4(0, 1, 0x8000, ushort.MaxValue)),
                Is.EqualTo(new ushort4(0, 1, 1, 16)), "the value of a ushort");

            Assert.That(new int4(1, 0, 0, 0).popcnt().x, Is.EqualTo(1), "the form on a value");
        }
    }

    /// <summary>
    /// The count of the zero bits at the front of the value of a component of every whole number kind.
    /// </summary>
    [Test]
    public void LeadingZeroCount()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.lzcnt(new int4(1, 0, -1, 2)), Is.EqualTo(new int4(31, 32, 0, 30)), "the value of an int");
            Assert.That(math.lzcnt(new uint2(1u, 0u)), Is.EqualTo(new uint2(31u, 32u)), "the value of a uint");
            Assert.That(math.lzcnt(new long2(1L, 0L)), Is.EqualTo(new long2(63L, 64L)), "the value of a long");
            Assert.That(math.lzcnt(new ulong2(1UL, ulong.MaxValue)), Is.EqualTo(new ulong2(63UL, 0UL)),
                "the value of a ulong");

            // the value of no bit set of a kind of sixteen bits holds the sixteen of them
            Assert.That(math.lzcnt(new short4(1, 0, -1, 2)), Is.EqualTo(new short4(15, 16, 0, 14)),
                "the value of a short");
            Assert.That(math.lzcnt(new ushort2(0x8000, 0)), Is.EqualTo(new ushort2(0, 16)),
                "the value of a ushort");

            Assert.That(new int4(1, 0, 0, 0).lzcnt().x, Is.EqualTo(31), "the form on a value");
        }
    }

    /// <summary>
    /// The count of the zero bits at the back of the value of a component of every whole number kind.
    /// </summary>
    [Test]
    public void TrailingZeroCount()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.tzcnt(new int4(1, 0, 8, -1)), Is.EqualTo(new int4(0, 32, 3, 0)), "the value of an int");
            Assert.That(math.tzcnt(new uint2(1u, 0u)), Is.EqualTo(new uint2(0u, 32u)), "the value of a uint");
            Assert.That(math.tzcnt(new long2(1L << 40, 0L)), Is.EqualTo(new long2(40L, 64L)), "the value of a long");
            Assert.That(math.tzcnt(new ulong2(1UL << 63, ulong.MaxValue)), Is.EqualTo(new ulong2(63UL, 0UL)),
                "the value of a ulong");

            // the value of no bit set of a kind of sixteen bits holds the sixteen of them
            Assert.That(math.tzcnt(new short4(1, 0, -1, 4)), Is.EqualTo(new short4(0, 16, 0, 2)),
                "the value of a short");
            Assert.That(math.tzcnt(new ushort2(0x8000, 0)), Is.EqualTo(new ushort2(15, 16)),
                "the value of a ushort");

            Assert.That(new int4(8, 0, 0, 0).tzcnt().x, Is.EqualTo(3), "the form on a value");
        }
    }

    /// <summary>
    /// The name of the standard for the count of the set bits of the value of a component, which is the member of
    /// the library that counts them.
    /// </summary>
    [Test]
    public void CountBits()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.countbits(new int4(0, 1, 3, -1)), Is.EqualTo(new int4(0, 1, 2, 32)), "the value of an int");
            Assert.That(math.countbits(new ushort4(0, 1, 0x8000, ushort.MaxValue)),
                Is.EqualTo(new ushort4(0, 1, 1, 16)), "the value of a ushort");
            // the name of the standard is the member of the library itself
            Assert.That(math.countbits(new long2(0xFFFFL, -1L)), Is.EqualTo(math.popcnt(new long2(0xFFFFL, -1L))),
                "the member of the library");
        }
    }

    /// <summary>
    /// The location of the first set bit from the lowest order bit of the value of a component: the standard
    /// answers the all bits set value of the kind for the value that has no bit set, which is the minus one of a
    /// signed kind.
    /// </summary>
    [Test]
    public void FirstBitLow()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.firstbitlow(new int4(1, 8, -1, 0)), Is.EqualTo(new int4(0, 3, 0, -1)),
                "the value of an int");
            Assert.That(math.firstbitlow(new uint2(1u, 0u)), Is.EqualTo(new uint2(0u, uint.MaxValue)),
                "the value of a uint");
            Assert.That(math.firstbitlow(new long2(1L << 40, 0L)), Is.EqualTo(new long2(40L, -1L)),
                "the value of a long");
            Assert.That(math.firstbitlow(new ulong2(1UL << 63, 0UL)), Is.EqualTo(new ulong2(63UL, ulong.MaxValue)),
                "the value of a ulong");

            // the kind of a component of sixteen bits holds a location up to the fifteen of them
            Assert.That(math.firstbitlow(new short4(1, 4, -1, 0)), Is.EqualTo(new short4(0, 2, 0, -1)),
                "the value of a short");
            Assert.That(math.firstbitlow(new ushort4(0x8000, 0, 16, 1)),
                Is.EqualTo(new ushort4(15, ushort.MaxValue, 4, 0)), "the value of a ushort");
        }
    }

    /// <summary>
    /// The location of the first bit of the value of a component from the highest order bit of the kind of it,
    /// which is the first bit of the value of it that is zero when the value of it is negative: the standard
    /// answers the all bits set value of the kind for the value that has no such bit.
    /// </summary>
    [Test]
    public void FirstBitHigh()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.firstbithigh(new int4(1, 2, 0, -1)), Is.EqualTo(new int4(31, 30, -1, -1)),
                "the value of an int");
            // a negative value reaches the first bit of it that is zero, so the sign bit of it answers nothing
            Assert.That(math.firstbithigh(new int4(int.MinValue, -2, 5, 0)), Is.EqualTo(new int4(1, 31, 29, -1)),
                "the value of a negative int");
            Assert.That(math.firstbithigh(new uint2(1u, 0u)), Is.EqualTo(new uint2(31u, uint.MaxValue)),
                "the value of a uint");
            Assert.That(math.firstbithigh(new uint2(0x80000000u, 0x40000000u)), Is.EqualTo(new uint2(0u, 1u)),
                "the value of a uint of the highest bit");
            Assert.That(math.firstbithigh(new long2(1L, -1L)), Is.EqualTo(new long2(63L, -1L)),
                "the value of a long");
            Assert.That(math.firstbithigh(new ulong2(1UL << 63, 0UL)),
                Is.EqualTo(new ulong2(0UL, ulong.MaxValue)), "the value of a ulong");

            // the kind of a component of sixteen bits holds a location up to the fifteen of them
            Assert.That(math.firstbithigh(new short4(1, 2, 0, -1)), Is.EqualTo(new short4(15, 14, -1, -1)),
                "the value of a short");
            Assert.That(math.firstbithigh(new ushort2(0x8000, 0)), Is.EqualTo(new ushort2(0, ushort.MaxValue)),
                "the value of a ushort");
        }
    }

    /// <summary>
    /// The count of a value of three components holds for every one of them and the padding lane of the register
    /// of it holds no component, so the value of the result is one of three components as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        var value = math.lzcnt(new int3(1, 1, 1));
        var register = Unsafe.As<int3, Vector128<int>>(ref value);
        // the value that has no such bit reaches the all bits set value of the kind, which the value of it masks
        // the padding lane of the register of it with, so the result of it holds no bit there either
        var standard = math.firstbithigh(new int3(0, 0, 0));
        var standardRegister = Unsafe.As<int3, Vector128<int>>(ref standard);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((value.x, value.y, value.z), Is.EqualTo((31, 31, 31)), "the components of the value");
            Assert.That(register.GetElement(3), Is.Zero, "the padding lane of the register");
            Assert.That((standard.x, standard.y, standard.z), Is.EqualTo((-1, -1, -1)),
                "the components of the value of the standard");
            Assert.That(standardRegister.GetElement(3), Is.Zero, "the padding lane of the register of the standard");
        }
    }

    /// <summary>
    /// The counting of the bits of a value reaches the values of a whole number kind alone: the interface of the
    /// algebra of a whole number is the constraint of the members of it, so the type of a floating point value
    /// does not satisfy it and the bits of one of them are only counted after the members of its type reinterpret
    /// them, which the <c>asi</c> of the value reaches.
    /// </summary>
    [Test]
    public void WholeNumberKinds()
    {
        using (Assert.EnterMultipleScope())
        {
            foreach (var name in new[] { "popcnt", "lzcnt", "tzcnt" })
            {
                var member = typeof(math).GetMethod(name)!;

                Assert.DoesNotThrow(() => member.MakeGenericMethod(typeof(int4)), $"the value of an int of {name}");
                Assert.DoesNotThrow(() => member.MakeGenericMethod(typeof(ushort4)), $"the value of a ushort of {name}");
                Assert.DoesNotThrow(() => member.MakeGenericMethod(typeof(long2)), $"the value of a long of {name}");
                Assert.DoesNotThrow(() => member.MakeGenericMethod(typeof(int3x3)), $"the value of a matrix of an int of {name}");

                Assert.Throws<ArgumentException>(() => member.MakeGenericMethod(typeof(float4)), $"the value of a float of {name}");
                Assert.Throws<ArgumentException>(() => member.MakeGenericMethod(typeof(half4)), $"the value of a half of {name}");
                Assert.Throws<ArgumentException>(() => member.MakeGenericMethod(typeof(double2)), $"the value of a double of {name}");
                Assert.Throws<ArgumentException>(() => member.MakeGenericMethod(typeof(float3x3)), $"the value of a matrix of a float of {name}");
            }
        }
    }
}
