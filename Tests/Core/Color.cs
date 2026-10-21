using System.Numerics;
using Coplt.Mathematics;

namespace Tests.Core;

public class TestColor
{
    private const float Tolerance = 1e-6f;

    private static bool WholeEquals<T>(T a, T b) where T : IEqualityOperators<T, T, bool> => a == b;

    private static uint Bits(float value) => BitConverter.SingleToUInt32Bits(value);

    private static void AssertRgba(float4 actual, float4 expected, string because)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.x, Is.EqualTo(expected.x).Within(Tolerance), because + ": red");
            Assert.That(actual.y, Is.EqualTo(expected.y).Within(Tolerance), because + ": green");
            Assert.That(actual.z, Is.EqualTo(expected.z).Within(Tolerance), because + ": blue");
            Assert.That(actual.w, Is.EqualTo(expected.w).Within(Tolerance), because + ": alpha");
        }
    }

    [Test]
    public void ComponentsAndConversions()
    {
        var color = new Color(0.1f, 0.2f, 0.3f, 0.4f);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(color.rgba, Is.EqualTo(new float4(0.1f, 0.2f, 0.3f, 0.4f)));
            Assert.That((color.r, color.g, color.b, color.a), Is.EqualTo((0.1f, 0.2f, 0.3f, 0.4f)));

            var fromRgb = new Color(new float3(0.25f, 0.5f, 0.75f));
            Assert.That(fromRgb, Is.EqualTo(new Color(0.25f, 0.5f, 0.75f, 1f)));
            Assert.That(new Color(new float3(1f, 2f, 3f), 0.5f),
                Is.EqualTo(new Color(1f, 2f, 3f, 0.5f)));

            color.r = 0.5f;
            color.g = 0.6f;
            color.b = 0.7f;
            color.a = 0.8f;
            Assert.That(color, Is.EqualTo(new Color(0.5f, 0.6f, 0.7f, 0.8f)));
        }

        AssertRgba(Color.FromByte(51, 102, 153, 204),
            new float4(0.2f, 0.4f, 0.6f, 0.8f), "the byte color");

        float4 vector = new Color(1f, 2f, 3f, 4f);
        Color converted = vector;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector, Is.EqualTo(new float4(1f, 2f, 3f, 4f)), "the conversion to a vector");
            Assert.That(converted, Is.EqualTo(new Color(1f, 2f, 3f, 4f)), "the conversion from a vector");
            Assert.That(Color.White, Is.EqualTo(new Color(float4.One)));
            Assert.That(Color.Transparent, Is.EqualTo(new Color(float4.Zero)));
        }
    }

    [Test]
    public void HsvFactories()
    {
        using (Assert.EnterMultipleScope())
        {
            AssertRgba(Color.Hsv(0f, 1f, 1f), new float4(1f, 0f, 0f, 1f), "red");
            AssertRgba(Color.Hsv(1f / 3f, 1f, 1f), new float4(0f, 1f, 0f, 1f), "green");
            AssertRgba(Color.Hsv(2f / 3f, 1f, 1f), new float4(0f, 0f, 1f, 1f), "blue");
            AssertRgba(Color.Hsv(0.5f, 1f, 1f), new float4(0f, 1f, 1f, 1f), "cyan");
            AssertRgba(Color.Hsv(0.25f, 0f, 0.4f), new float4(0.4f, 0.4f, 0.4f, 1f), "gray");

            AssertRgba(Color.Hsv(new float3(1f / 3f, 1f, 0.5f), 0.25f),
                new float4(0f, 0.5f, 0f, 0.25f), "the vector of the components");
            AssertRgba(Color.Hsv(new float4(2f / 3f, 0.5f, 0.8f, 0.75f)),
                new float4(0.4f, 0.4f, 0.8f, 0.75f), "the vector of the components and alpha");
        }
    }

    [Test]
    public void ToHsv()
    {
        AssertRgba(Color.Red.ToHsv, new float4(0f, 1f, 1f, 1f), "red");
        AssertRgba(Color.Lime.ToHsv, new float4(1f / 3f, 1f, 1f, 1f), "green");
        AssertRgba(Color.Blue.ToHsv, new float4(2f / 3f, 1f, 1f, 1f), "blue");
        AssertRgba(new Color(0f, 1f, 1f, 0.25f).ToHsv,
            new float4(0.5f, 1f, 1f, 0.25f), "cyan with alpha");
        AssertRgba(new Color(0.4f, 0.4f, 0.4f, 0.75f).ToHsv,
            new float4(0f, 0f, 0.4f, 0.75f), "gray");

        var source = new float4(0.2f, 0.7f, 0.35f, 0.6f);
        var restored = Color.Hsv(new Color(source).ToHsv);
        AssertRgba(restored, source, "the round trip through HSV");
    }

    [Test]
    public void Operators()
    {
        var a = new Color(1f, 2f, 3f, 4f);
        var b = new Color(0.5f, 0.25f, 2f, 0.5f);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(+a, Is.EqualTo(a));
            Assert.That(-a, Is.EqualTo(new Color(-1f, -2f, -3f, -4f)));
            Assert.That(a + b, Is.EqualTo(new Color(1.5f, 2.25f, 5f, 4.5f)));
            Assert.That(a - b, Is.EqualTo(new Color(0.5f, 1.75f, 1f, 3.5f)));
            Assert.That(a * b, Is.EqualTo(new Color(0.5f, 0.5f, 6f, 2f)));
            Assert.That(a / b, Is.EqualTo(new Color(2f, 8f, 1.5f, 8f)));
            Assert.That(a % b, Is.EqualTo(new Color(0f, 0f, 1f, 0f)));

            Assert.That(a + 0.5f, Is.EqualTo(new Color(1.5f, 2.5f, 3.5f, 4.5f)));
            Assert.That(0.5f - a, Is.EqualTo(new Color(-0.5f, -1.5f, -2.5f, -3.5f)));
            Assert.That(a * 2f, Is.EqualTo(new Color(2f, 4f, 6f, 8f)));
            Assert.That(2f / a, Is.EqualTo(new Color(2f, 1f, 2f / 3f, 0.5f)));

            Assert.That((a & a).Equals(a), Is.True, "bitwise and");
            Assert.That((a | a).Equals(a), Is.True, "bitwise or");
            Assert.That((a ^ a).Equals(Color.Transparent), Is.True, "bitwise xor");
        }
    }

    [Test]
    public void EqualityAndComparisons()
    {
        var color = new Color(1f, 2f, 3f, 0.5f);
        var same = new Color(1f, 2f, 3f, 0.5f);
        var other = new Color(0f, 2f, 4f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(color.Equals(same), Is.True);
            Assert.That(WholeEquals(color, same), Is.True);
            Assert.That(WholeEquals(color, other), Is.False);
            Assert.That(color.GetHashCode(), Is.EqualTo(same.GetHashCode()));
            var equal = color == same;
            Assert.That(
                (Bits(equal.x), Bits(equal.y), Bits(equal.z), Bits(equal.w)),
                Is.EqualTo((uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue))
            );

            var less = color < other;
            var greater = color > other;
            Assert.That((Bits(less.r), Bits(less.g), Bits(less.b), Bits(less.a)),
                Is.EqualTo((0u, 0u, uint.MaxValue, uint.MaxValue)));
            Assert.That((Bits(greater.r), Bits(greater.g), Bits(greater.b), Bits(greater.a)),
                Is.EqualTo((uint.MaxValue, 0u, 0u, 0u)));
            Assert.That(color.ToString(), Is.EqualTo("Color(1, 2, 3, 0.5)"));
        }
    }

    [Test]
    public void MathMembers()
    {
        var color = new Color(-1f, 0.5f, 1.5f, -0.25f);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.abs(color), Is.EqualTo(new Color(1f, 0.5f, 1.5f, 0.25f)), "abs");
            Assert.That(math.sign(color), Is.EqualTo(new float4(-1f, 1f, 1f, -1f)), "sign");
            Assert.That(math.min(color, Color.Transparent),
                Is.EqualTo(new Color(-1f, 0f, 0f, -0.25f)), "min");
            Assert.That(math.max(color, Color.Transparent),
                Is.EqualTo(new Color(0f, 0.5f, 1.5f, 0f)), "max");
            Assert.That(math.clamp(color, 0f, 1f),
                Is.EqualTo(new Color(0f, 0.5f, 1f, 0f)), "scalar clamp");
            Assert.That(math.saturate(color),
                Is.EqualTo(new Color(0f, 0.5f, 1f, 0f)), "saturate");
            Assert.That(math.floor(color),
                Is.EqualTo(new Color(-1f, 0f, 1f, -1f)), "floor");
            Assert.That(math.ceil(color),
                Is.EqualTo(new Color(-1f, 1f, 2f, 0f)), "ceil");

            var mask = new float4(-1f, 0f, -1f, 0f);
            Assert.That(math.select(mask, Color.Red, Color.Blue),
                Is.EqualTo(new Color(1f, 0f, 0f, 1f)), "select");
            Assert.That(math.lerp(0f, 2f, new Color(0.5f)),
                Is.EqualTo(new Color(1f)), "lerp between scalars");
        }

        // the color overloads must not take the calls of a float4 away from the generic vector members
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.abs(new float4(-1f)), Is.TypeOf<float4>());
            Assert.That(math.min(float4.Zero, float4.One), Is.TypeOf<float4>());
            Assert.That(math.clamp(new float4(0.5f), 0f, 1f), Is.TypeOf<float4>());
            Assert.That(math.select(new float4(-1f), float4.Zero, float4.One), Is.TypeOf<float4>());
            Assert.That(math.saturate(new float4(0.5f)), Is.TypeOf<float4>());
            Assert.That(math.lerp(float4.Zero, float4.One, new float4(0.5f)), Is.TypeOf<float4>());
            Assert.That(math.unlerp(new float4(0.5f), float4.Zero, float4.One), Is.TypeOf<float4>());
            Assert.That(math.remap(new float4(0.5f), float4.Zero, float4.One, float4.One, new float4(3f)),
                Is.TypeOf<float4>());
            Assert.That(math.wrap(new float4(1.5f), float4.Zero, float4.One), Is.TypeOf<float4>());
        }
    }
}
