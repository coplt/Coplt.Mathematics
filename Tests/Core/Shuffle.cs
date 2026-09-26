using System.Reflection;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The shuffle members of a vector implement <c>IVectorShuffle</c>: a member combines two vectors, the low
/// half of its result is taken from the first vector and the high half from the second one, and the four
/// digits of its name are the indices of the components of each half, so <c>shuffle_wz_yx</c> is
/// <c>(a.w, a.z, b.y, b.x)</c>. The member of a simd backed vector shuffles the two registers and the member
/// of a vector without a register reads the components of its sources, both have to agree on the pattern.
/// </summary>
public class TestVectorShuffle
{
    [Test]
    public void Members()
    {
        var a = new float4(1, 2, 3, 4);
        var b = new float4(5, 6, 7, 8);
        var d = new double4(1, 2, 3, 4);
        var e = new double4(5, 6, 7, 8);
        var i = new int4(1, 2, 3, 4);
        var j = new int4(5, 6, 7, 8);
        var h = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var k = new half4((Half)5, (Half)6, (Half)7, (Half)8);
        var s = new b32v4(true, false, true, false);
        var t = new b32v4(false, true, false, true);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float4.shuffle_xx_xx(a, b), Is.EqualTo(new float4(1, 1, 5, 5)));
            Assert.That(float4.shuffle_xy_zw(a, b), Is.EqualTo(new float4(1, 2, 7, 8)));
            Assert.That(float4.shuffle_wz_yx(a, b), Is.EqualTo(new float4(4, 3, 6, 5)));
            Assert.That(float4.shuffle_ww_wx(a, b), Is.EqualTo(new float4(4, 4, 8, 5)));
            Assert.That(float4.shuffle_zw_xy(a, b), Is.EqualTo(new float4(3, 4, 5, 6)));
            // the register of a 4 component vector of a double is 256 bits wide
            Assert.That(double4.shuffle_xx_xx(d, e), Is.EqualTo(new double4(1, 1, 5, 5)));
            Assert.That(double4.shuffle_xy_zw(d, e), Is.EqualTo(new double4(1, 2, 7, 8)));
            Assert.That(double4.shuffle_yx_wz(d, e), Is.EqualTo(new double4(2, 1, 8, 7)));
            Assert.That(int4.shuffle_xx_yy(i, j), Is.EqualTo(new int4(1, 1, 6, 6)));
            Assert.That(int4.shuffle_xw_yz(i, j), Is.EqualTo(new int4(1, 4, 6, 7)));
            // a vector without a register reads the components its pattern names
            Assert.That(half4.shuffle_xx_xx(h, k), Is.EqualTo(new half4((Half)1, (Half)1, (Half)5, (Half)5)));
            Assert.That(half4.shuffle_wz_yx(h, k), Is.EqualTo(new half4((Half)4, (Half)3, (Half)6, (Half)5)));
            // a bool vector keeps the bits of its components, its shuffle is the one of the unsigned vector
            Assert.That(((bool)b32v4.shuffle_xy_zw(s, t).x, (bool)b32v4.shuffle_xy_zw(s, t).w),
                Is.EqualTo((true, true)));
            Assert.That(((bool)b32v4.shuffle_wz_yx(s, t).x, (bool)b32v4.shuffle_wz_yx(s, t).y),
                Is.EqualTo((false, true)));
        }
    }

    [Test]
    public void Table()
    {
        var a = new float4(1, 2, 3, 4);
        var b = new float4(5, 6, 7, 8);
        var d = new double4(1, 2, 3, 4);
        var e = new double4(5, 6, 7, 8);
        var h = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var k = new half4((Half)5, (Half)6, (Half)7, (Half)8);
        using (Assert.EnterMultipleScope())
        {
            // the pattern of this member is only known at run time, it dispatches to the member of its pattern
            Assert.That(float4.shuffle(a, b, Shuffle42.xx_xx), Is.EqualTo(new float4(1, 1, 5, 5)));
            Assert.That(float4.shuffle(a, b, Shuffle42.xy_zw), Is.EqualTo(new float4(1, 2, 7, 8)));
            Assert.That(float4.shuffle(a, b, Shuffle42.wz_yx), Is.EqualTo(new float4(4, 3, 6, 5)));
            Assert.That(double4.shuffle(d, e, Shuffle42.wz_yx), Is.EqualTo(new double4(4, 3, 6, 5)));
            Assert.That(half4.shuffle(h, k, Shuffle42.xy_zw), Is.EqualTo(new half4((Half)1, (Half)2, (Half)7, (Half)8)));
        }
    }

    [Test]
    public void Forwarding()
    {
        var a = new float4(1, 2, 3, 4);
        var b = new float4(5, 6, 7, 8);
        var h = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var k = new half4((Half)5, (Half)6, (Half)7, (Half)8);
        using (Assert.EnterMultipleScope())
        {
            // the math class forwards the call to the member of the vector itself
            Assert.That(math.shuffle_xy_zw(a, b), Is.EqualTo(new float4(1, 2, 7, 8)));
            Assert.That(math.shuffle_wz_yx(a, b), Is.EqualTo(new float4(4, 3, 6, 5)));
            Assert.That(math.shuffle_ww_wx(a, b), Is.EqualTo(new float4(4, 4, 8, 5)));
            Assert.That(math.shuffle_xx_xx(h, k), Is.EqualTo(new half4((Half)1, (Half)1, (Half)5, (Half)5)));
            Assert.That(math.shuffle(a, b, Shuffle42.yx_wz), Is.EqualTo(new float4(2, 1, 8, 7)));
            Assert.That(math.shuffle(h, k, Shuffle42.yx_wz), Is.EqualTo(new half4((Half)2, (Half)1, (Half)8, (Half)7)));
        }
    }

    /// <summary>
    /// The members are static, so a vector can be shuffled by generic code that constrains it to
    /// <c>IVectorShuffle</c>: the constraint carries every pattern and the pattern of the call site is the
    /// member of the type it is called with.
    /// </summary>
    private static T Combine<T>(T a, T b)
        where T : unmanaged, IVectorShuffle<T> => T.shuffle_xy_zw(a, b);

    [Test]
    public void Interface()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Combine(new float4(1, 2, 3, 4), new float4(5, 6, 7, 8)), Is.EqualTo(new float4(1, 2, 7, 8)));
            Assert.That(Combine(new double4(1, 2, 3, 4), new double4(5, 6, 7, 8)), Is.EqualTo(new double4(1, 2, 7, 8)));
            Assert.That(Combine(new int4(1, 2, 3, 4), new int4(5, 6, 7, 8)), Is.EqualTo(new int4(1, 2, 7, 8)));
            Assert.That(Combine(new half4((Half)1, (Half)2, (Half)3, (Half)4), new half4((Half)5, (Half)6, (Half)7, (Half)8)),
                Is.EqualTo(new half4((Half)1, (Half)2, (Half)7, (Half)8)));
            Assert.That(Combine(new long4(1, 2, 3, 4), new long4(5, 6, 7, 8)), Is.EqualTo(new long4(1, 2, 7, 8)));
            Assert.That(Combine(new ulong4(1, 2, 3, 4), new ulong4(5, 6, 7, 8)), Is.EqualTo(new ulong4(1, 2, 7, 8)));
            Assert.That(Combine(new short4(1, 2, 3, 4), new short4(5, 6, 7, 8)), Is.EqualTo(new short4(1, 2, 7, 8)));
            Assert.That(Combine(new ushort4(1, 2, 3, 4), new ushort4(5, 6, 7, 8)), Is.EqualTo(new ushort4(1, 2, 7, 8)));
            Assert.That(Combine(new uint4(1, 2, 3, 4), new uint4(5, 6, 7, 8)), Is.EqualTo(new uint4(1, 2, 7, 8)));
            Assert.That(Combine(new b16v4(true, false, true, false), new b16v4(false, true, false, true)),
                Is.EqualTo(new b16v4(true, false, false, true)));
            Assert.That(Combine(new b64v4(true, false, true, false), new b64v4(false, true, false, true)),
                Is.EqualTo(new b64v4(true, false, false, true)));
        }
    }

    /// <summary>
    /// Shuffles every pattern of <typeparamref name="T"/> with the member of the type and checks the result
    /// against the pattern of the member. The component of the index <c>i</c> of <paramref name="a"/> has the
    /// value <c>i</c> and the one of <paramref name="b"/> the value <c>i + offset</c>, so the result of a
    /// member is the vector of the indices its name holds.
    /// </summary>
    /// <param name="a">The first vector, the component of the index <c>i</c> has the value <c>i</c></param>
    /// <param name="b">The second vector, the component of the index <c>i</c> has the value <c>i + offset</c></param>
    /// <param name="offset">The value of the first component of <paramref name="b"/></param>
    /// <param name="create">Builds a vector from the value of its 4 components</param>
    /// <typeparam name="T">The type of the vectors</typeparam>
    private static void EveryMember<T>(T a, T b, int offset, Func<int, int, int, int, T> create)
        where T : unmanaged, IVectorShuffle<T>
    {
        var members = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name.StartsWith("shuffle_", StringComparison.Ordinal))
            .ToList();
        using (Assert.EnterMultipleScope())
        {
            // every combination of the components of the two vectors is a member of its own
            Assert.That(members.Count, Is.EqualTo(256));
            foreach (var member in members)
            {
                var digits = member.Name["shuffle_".Length..].Replace("_", "");
                var expected = create(
                    Index(digits[0]),
                    Index(digits[1]),
                    Index(digits[2]) + offset,
                    Index(digits[3]) + offset);
                var actual = (T)member.Invoke(null, new object[] { a, b })!;
                Assert.That(actual, Is.EqualTo(expected), member.Name);
            }
        }

        static int Index(char component) => "xyzw".IndexOf(component);
    }

    /// <summary>
    /// Shuffles every pattern of the enum of the patterns with the member of the table of
    /// <typeparamref name="T"/> and checks the result against the member of the name of the value: the name
    /// of a value of the enum is the name of the pattern of a member, so the table and the members agree on
    /// the pattern of every value. See <see cref="EveryMember"/> for the value of the components.
    /// </summary>
    /// <param name="a">The first vector, the component of the index <c>i</c> has the value <c>i</c></param>
    /// <param name="b">The second vector, the component of the index <c>i</c> has the value <c>i + offset</c></param>
    /// <param name="offset">The value of the first component of <paramref name="b"/></param>
    /// <param name="create">Builds a vector from the value of its 4 components</param>
    /// <typeparam name="T">The type of the vectors</typeparam>
    private static void EveryPatternOfTheTable<T>(T a, T b, int offset, Func<int, int, int, int, T> create)
        where T : unmanaged, IVectorShuffle<T>
    {
        var members = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name.StartsWith("shuffle_", StringComparison.Ordinal))
            .ToDictionary(m => m.Name);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Enum.GetValues<Shuffle42>().Length, Is.EqualTo(256));
            for (var i = 0; i < 256; i++)
            {
                var value = (Shuffle42)i;
                var expected = (T)members[$"shuffle_{value}"].Invoke(null, new object[] { a, b })!;
                Assert.That(T.shuffle(a, b, value), Is.EqualTo(expected), value.ToString());
            }
        }
    }

    [Test]
    public void EveryMemberOfASimdVector()
    {
        EveryMember(new float4(0, 1, 2, 3), new float4(4, 5, 6, 7), 4,
            static (x, y, z, w) => new float4(x, y, z, w));
        EveryPatternOfTheTable(new float4(0, 1, 2, 3), new float4(4, 5, 6, 7), 4,
            static (x, y, z, w) => new float4(x, y, z, w));
    }

    [Test]
    public void EveryMemberOfAVectorWithoutARegister()
    {
        EveryMember(new half4((Half)0, (Half)1, (Half)2, (Half)3), new half4((Half)4, (Half)5, (Half)6, (Half)7), 4,
            static (x, y, z, w) => new half4((Half)x, (Half)y, (Half)z, (Half)w));
        EveryPatternOfTheTable(new half4((Half)0, (Half)1, (Half)2, (Half)3), new half4((Half)4, (Half)5, (Half)6, (Half)7), 4,
            static (x, y, z, w) => new half4((Half)x, (Half)y, (Half)z, (Half)w));
    }
}
