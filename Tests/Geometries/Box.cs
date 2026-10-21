using Coplt.Mathematics;

namespace Tests.Geometries;

/// <summary>
/// A box of the space of 3 dimensions, which is the set of the points that lie between the two corners of it: the
/// components of a corner are the smallest and the largest value of the components of the points of the box at the
/// index of them.
/// </summary>
public class TestBox
{
    [Test]
    public void OfTheCorners()
    {
        using (Assert.EnterMultipleScope())
        {
            var b = new box { min = new float3(-1f, -2f, -3f), max = new float3(1f, 2f, 3f) };
            Assert.That(b.min, Is.EqualTo(new float3(-1f, -2f, -3f)), "the corner of the smallest components");
            Assert.That(b.max, Is.EqualTo(new float3(1f, 2f, 3f)), "the corner of the largest components");

            Assert.That(b == new box { min = new float3(-1f, -2f, -3f), max = new float3(1f, 2f, 3f) }, Is.True,
                "the equality");
            Assert.That(b != new box { min = new float3(-1f, -2f, -3f), max = new float3(1f, 2f, 4f) }, Is.True,
                "the inequality");
            Assert.That(b.Equals(new box { min = new float3(-1f, -2f, -3f), max = new float3(1f, 2f, 3f) }), Is.True,
                "the equality of the value");
            Assert.That(b.GetHashCode(),
                Is.EqualTo(new box { min = new float3(-1f, -2f, -3f), max = new float3(1f, 2f, 3f) }.GetHashCode()),
                "the hash");
        }
    }
}
