using System.Reflection;
using System.Runtime.CompilerServices;
using Coplt.Mathematics;

namespace Tests.Core;

/// <summary>
/// The check of a single component of a value: a component holds when the bits of it are not all zero, so the
/// zero of a kind is the only value of it that does not hold. The zero of the two floating point kinds that has
/// the sign bit of it set is not the zero of the bits of a component, and the bits of the value of a kind are
/// the ones the check reads, which the least value of them tells as well. The member is internal, so the test
/// reaches it through the assembly of the library with the kind of the value it is checked with.
/// </summary>
public class TestUtils
{
    /// <summary>The internal member that the checks reach</summary>
    private static readonly MethodInfo IsTrue = typeof(math).Assembly
        .GetType("Coplt.Mathematics.Utils")!
        .GetMethod("IsTrue", BindingFlags.Public | BindingFlags.Static)!;

    /// <summary>Reaches the internal member with the kind of the value that is checked</summary>
    private static bool IsTrueOf<T>(T value) where T : unmanaged =>
        (bool)IsTrue.MakeGenericMethod(typeof(T)).Invoke(null, [value])!;

    [Test]
    public void Integer()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(IsTrueOf(0), Is.False, "the zero of an int");
            Assert.That(IsTrueOf(1), Is.True, "the one of an int");
            Assert.That(IsTrueOf(-1), Is.True, "the minus one of an int");
            Assert.That(IsTrueOf(int.MinValue), Is.True, "the least value of an int");
            Assert.That(IsTrueOf(int.MaxValue), Is.True, "the greatest value of an int");
            Assert.That(IsTrueOf(0u), Is.False, "the zero of a uint");
            Assert.That(IsTrueOf(1u), Is.True, "the one of a uint");
            Assert.That(IsTrueOf(uint.MaxValue), Is.True, "the all bits set value of a uint");
            Assert.That(IsTrueOf(0L), Is.False, "the zero of a long");
            Assert.That(IsTrueOf(-1L), Is.True, "the minus one of a long");
            Assert.That(IsTrueOf(long.MinValue), Is.True, "the least value of a long");
            Assert.That(IsTrueOf(0UL), Is.False, "the zero of a ulong");
            Assert.That(IsTrueOf(ulong.MaxValue), Is.True, "the all bits set value of a ulong");
            Assert.That(IsTrueOf((short)0), Is.False, "the zero of a short");
            Assert.That(IsTrueOf((short)-1), Is.True, "the minus one of a short");
            Assert.That(IsTrueOf(short.MinValue), Is.True, "the least value of a short");
            Assert.That(IsTrueOf((ushort)0), Is.False, "the zero of a ushort");
            Assert.That(IsTrueOf(ushort.MaxValue), Is.True, "the all bits set value of a ushort");
        }
    }

    /// <summary>
    /// The bits of a value of a floating point kind are the ones the check reads, so the value whose bits are
    /// not all zero holds, which the zero of the kind that has the sign bit of it set is one of.
    /// </summary>
    [Test]
    public void Float()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(IsTrueOf(0f), Is.False, "the zero of a float");
            Assert.That(IsTrueOf(1f), Is.True, "the one of a float");
            Assert.That(IsTrueOf(-1f), Is.True, "the minus one of a float");
            Assert.That(IsTrueOf(float.Epsilon), Is.True, "the least positive value of a float");
            Assert.That(IsTrueOf(float.MaxValue), Is.True, "the greatest value of a float");
            Assert.That(IsTrueOf(float.NaN), Is.True, "a nan");
            Assert.That(IsTrueOf(float.PositiveInfinity), Is.True, "the positive infinity");
            // a value of the kind whose bits are the least of the ones it has, so only the bits of it decide
            Assert.That(IsTrueOf(BitConverter.UInt32BitsToSingle(1)), Is.True, "the least value of the bits");
            // the zero of the kind that only the sign bit of it differs from the zero does not hold as the zero
            Assert.That(IsTrueOf(-0f), Is.True, "the zero of the kind that has the sign bit of it set");
        }
    }

    /// <inheritdoc cref="Float"/>
    [Test]
    public void Double()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(IsTrueOf(0d), Is.False, "the zero of a double");
            Assert.That(IsTrueOf(1d), Is.True, "the one of a double");
            Assert.That(IsTrueOf(-1d), Is.True, "the minus one of a double");
            Assert.That(IsTrueOf(double.Epsilon), Is.True, "the least positive value of a double");
            Assert.That(IsTrueOf(double.MaxValue), Is.True, "the greatest value of a double");
            Assert.That(IsTrueOf(double.NaN), Is.True, "a nan");
            Assert.That(IsTrueOf(double.PositiveInfinity), Is.True, "the positive infinity");
            Assert.That(IsTrueOf(BitConverter.UInt64BitsToDouble(1UL)), Is.True, "the least value of the bits");
            Assert.That(IsTrueOf(-0d), Is.True, "the zero of the kind that has the sign bit of it set");
            // a value of the kind whose upper half is all bits set
            Assert.That(IsTrueOf(BitConverter.UInt64BitsToDouble(0xFFFFFFFF_00000000UL)), Is.True,
                "the value of the bits whose upper half is the all bits set one");
        }
    }

    /// <inheritdoc cref="Float"/>
    [Test]
    public void Half()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(IsTrueOf((Half)0f), Is.False, "the zero of a half");
            Assert.That(IsTrueOf((Half)1f), Is.True, "the one of a half");
            Assert.That(IsTrueOf((Half)(-1f)), Is.True, "the minus one of a half");
            Assert.That(IsTrueOf(Unsafe.BitCast<ushort, Half>(1)), Is.True, "the least value of the bits");
            Assert.That(IsTrueOf(Unsafe.BitCast<ushort, Half>(0x7C00)), Is.True, "the positive infinity");
            Assert.That(IsTrueOf(Unsafe.BitCast<ushort, Half>(0x7E00)), Is.True, "a nan");
            Assert.That(IsTrueOf(Unsafe.BitCast<ushort, Half>(0x8000)), Is.True,
                "the zero of the kind that has the sign bit of it set");
        }
    }
}
