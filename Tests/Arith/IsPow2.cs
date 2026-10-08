using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The check of a power of two of every component of a number. The answer is a value of the kind of the value
/// itself: the all bits set value of the kind is the true component of it and the zero of the kind is the false
/// one, so a component that holds is not zero and one that does not is zero.
/// <para>The check of a component is the one of the framework for the kind of it: a floating point component holds
/// when it is a positive finite power of two, so the negative of a power of two does not hold and a subnormal that
/// has a single bit does, and an integer component holds when it is not negative</para>
/// </summary>
public class TestIsPow2
{
    /// <summary>
    /// The member is one of the algebra of the kind of the value, so a parameter that only knows the interfaces
    /// of it reaches the member as well.
    /// </summary>
    private static void Check<T>(T value)
        where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>
        => _ = math.is_pow2(value);

    [Test]
    public void Value()
    {
        var v = new float3(1f, 3f, 4f);
        var m = math.is_pow2(v);

        using (Assert.EnterMultipleScope())
        {
            // one is a power of two
            Assert.That(m.x != 0f, Is.True);
            Assert.That(m.y != 0f, Is.False);
            Assert.That(m.z != 0f, Is.True);
        }

        // the negative of a power of two does not hold, the sign of a value is part of the check
        var n = math.is_pow2(new float3(-2f, -3f, 0.5f));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.x != 0f, Is.False);
            Assert.That(n.y != 0f, Is.False);
            Assert.That(n.z != 0f, Is.True);
        }

        // the zero, the infinity and the nan of a value are not a power of two
        var s = math.is_pow2(new float3(-0f, float.PositiveInfinity, float.NaN));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(s.x != 0f, Is.False);
            Assert.That(s.y != 0f, Is.False);
            Assert.That(s.z != 0f, Is.False);
        }

        // the smallest subnormal of a kind has a single bit and holds, the one that has two of them does not
        var e = math.is_pow2(new float3(float.Epsilon, float.Epsilon * 2f, float.Epsilon * 3f));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(e.x != 0f, Is.True);
            Assert.That(e.y != 0f, Is.True);
            Assert.That(e.z != 0f, Is.False);
        }

        using (Assert.EnterMultipleScope())
        {
            var d = math.is_pow2(new double2(2, 0.5));
            Assert.That(d.x != 0d, Is.True);
            Assert.That(d.y != 0d, Is.True);
            Assert.That(math.is_pow2(new double2(3, 0)).x != 0d, Is.False);
            Assert.That(math.is_pow2(new double2(3, 0)).y != 0d, Is.False);

            // a value without a register works on the components
            var h = math.is_pow2(new half3((half)4f, (half)3f, (half)(-8f)));
            Assert.That(h.x != (half)0, Is.True);
            Assert.That(h.y != (half)0, Is.False);
            Assert.That(h.z != (half)0, Is.False);

            // a matrix is a value of the same algebra
            var c = math.is_pow2(new float2x2(new float2(1f, 3f), new float2(4f, 6f)));
            Assert.That(c.c0.x != 0f, Is.True);
            Assert.That(c.c0.y != 0f, Is.False);
            Assert.That(c.c1.x != 0f, Is.True);
            Assert.That(c.c1.y != 0f, Is.False);
        }
    }

    /// <summary>
    /// The check of the integer kinds: a zero and a negative value are not a power of two, and a value without a
    /// sign that has the top bit set is one.
    /// </summary>
    [Test]
    public void Integer()
    {
        var i = math.is_pow2(new int3(1, 2, 3));

        using (Assert.EnterMultipleScope())
        {
            // one is a power of two
            Assert.That(i.x != 0, Is.True);
            Assert.That(i.y != 0, Is.True);
            Assert.That(i.z != 0, Is.False);
        }

        var n = math.is_pow2(new int3(0, -4, int.MinValue));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.x != 0, Is.False);
            Assert.That(n.y != 0, Is.False);
            Assert.That(n.z != 0, Is.False);
        }

        var u = math.is_pow2(new uint3(0x8000_0000u, 0u, 3u));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(u.x != 0u, Is.True);
            Assert.That(u.y != 0u, Is.False);
            Assert.That(u.z != 0u, Is.False);
        }

        using (Assert.EnterMultipleScope())
        {
            // a value of a kind without a register reaches the member of the framework for a component
            var s = math.is_pow2(new short3(1, 2, 4));
            Assert.That(s.x != 0, Is.True);
            Assert.That(s.y != 0, Is.True);
            Assert.That(s.z != 0, Is.True);

            var sn = math.is_pow2(new short3(0, 3, -1));
            Assert.That(sn.x != 0, Is.False);
            Assert.That(sn.y != 0, Is.False);
            Assert.That(sn.z != 0, Is.False);

            var us = math.is_pow2(new ushort3(1, 2, 3));
            Assert.That(us.x != 0, Is.True);
            Assert.That(us.y != 0, Is.True);
            Assert.That(us.z != 0, Is.False);

            var l = math.is_pow2(new long3(1L, 0x4000_0000_0000_0000L, 6L));
            Assert.That(l.x != 0L, Is.True);
            Assert.That(l.y != 0L, Is.True);
            Assert.That(l.z != 0L, Is.False);

            var ul = math.is_pow2(new ulong3(1ul, 0x8000_0000_0000_0000ul, 6ul));
            Assert.That(ul.x != 0ul, Is.True);
            Assert.That(ul.y != 0ul, Is.True);
            Assert.That(ul.z != 0ul, Is.False);
        }
    }

    /// <summary>
    /// Checks the answer of the member against the one of the framework for the kind of a component, which the
    /// member reaches for a value of a kind without a register: the values are the components of a vector in order
    /// and every one of them is put to the member of the framework of the kind of it
    /// </summary>
    private static void CheckFramework<T, TScalar>(TScalar[] values, string what)
        where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>, IVector<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        // a vector that keeps its value in a register reads the whole of it, so the padding lane is given one
        var count = T.IsSimdAccelerated && T.Length == 3 ? 4 : T.Length;
        var data = new TScalar[count];
        for (var i = 0; i < values.Length && i < count; i++) data[i] = values[i];

        var value = T.Load(data);
        var mask = math.is_pow2(value);

        for (var i = 0; i < values.Length && i < count; i++)
        {
            Assert.That(
                T.get(mask, i) != TScalar.Zero,
                Is.EqualTo(TScalar.IsPow2(values[i])),
                $"{what}, the framework says {TScalar.IsPow2(values[i])} of the component {i}");
        }
    }

    /// <summary>
    /// The member says the same about a component as the member of the framework for the kind of it, which is the
    /// one it follows for a value of a kind that has no register of its own and the one the helper of the simd
    /// library follows for a value that has one.
    /// </summary>
    [Test]
    public void Framework()
    {
        CheckFramework<float4, float>([2f, -2f, float.Epsilon, float.NaN], "float4");
        CheckFramework<float3, float>([3f, float.PositiveInfinity, -0f], "float3");
        CheckFramework<double4, double>([2, -2, double.Epsilon, double.NaN], "double4");
        CheckFramework<double3, double>([3, double.PositiveInfinity, -0.0], "double3");
        CheckFramework<half4, Half>([(Half)2f, (Half)(-2f), (Half)3f, (Half)(-0f)], "half4");
        CheckFramework<short4, short>([2, -2, 0, 3], "short4");
        CheckFramework<ushort4, ushort>([2, 0x8000, 0, 3], "ushort4");
        CheckFramework<int4, int>([2, -2, 0, int.MinValue], "int4");
        CheckFramework<uint4, uint>([2, 0x8000_0000u, 0u, 3u], "uint4");
        CheckFramework<long4, long>([2L, -2L, 0L, long.MinValue], "long4");
        CheckFramework<ulong4, ulong>([2ul, 0x8000_0000_0000_0000ul, 0ul, 3ul], "ulong4");
    }

    /// <summary>
    /// The zero of a padding lane is not a power of two, so the lane keeps its zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_pow2(new float3(1f, 2f, 4f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.is_pow2(new double3(1, 2, 4)).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(math.is_pow2(new int3(1, 2, 4)).vector.GetElement(3), Is.EqualTo(0));
            Assert.That(math.is_pow2(new long3(1, 2, 4)).vector.GetElement(3), Is.EqualTo(0L));
        }
    }

    [Test]
    public void Interface()
    {
        Check(new float3(1f, 2f, 4f));
        Check(new double3(1, 2, 4));
        Check(new half3((half)1f, (half)2f, (half)4f));
        Check(new short3(1, 2, 4));
        Check(new ushort3(1, 2, 4));
        Check(new int3(1, 2, 4));
        Check(new uint3(1, 2, 4));
        Check(new long3(1, 2, 4));
        Check(new ulong3(1, 2, 4));
    }
}
