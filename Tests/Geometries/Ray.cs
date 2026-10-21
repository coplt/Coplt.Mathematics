using Coplt.Mathematics;

namespace Tests.Geometries;

/// <summary>
/// A ray of the space of 3 dimensions, which is the half of the line that the point that it starts at and the
/// direction that it points to reach: the point of the ray at a distance is the origin of it beside the direction
/// of it scaled by the distance, so the distance along the ray is the one of the space where the direction of it is
/// of the length of one.
/// </summary>
public class TestRay
{
    [Test]
    public void OfThePoint()
    {
        using (Assert.EnterMultipleScope())
        {
            // the point of the ray at a distance is the origin of it beside the direction of it scaled by the
            // distance
            var r = new ray(new float3(1f, 2f, 3f), new float3(1f, 0f, 0f));
            Assert.That(r.point_at(0f), Is.EqualTo(new float3(1f, 2f, 3f)), "the point of the origin");
            Assert.That(r.point_at(2f), Is.EqualTo(new float3(3f, 2f, 3f)), "the point at a distance");
            Assert.That(r.point_at(-1f), Is.EqualTo(new float3(0f, 2f, 3f)), "the point behind the origin");

            // a direction that is not of the length of one scales the distance by the length of it
            var scaled = new ray(new float3(1f, 2f, 3f), new float3(2f, 0f, 0f));
            Assert.That(scaled.point_at(2f), Is.EqualTo(new float3(5f, 2f, 3f)), "the point of the scaled ray");
        }
    }

    [Test]
    public void OfTheValue()
    {
        using (Assert.EnterMultipleScope())
        {
            var r = new ray(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f));
            Assert.That(r.origin, Is.EqualTo(new float3(1f, 2f, 3f)), "the origin of the ray");
            Assert.That(r.direction, Is.EqualTo(new float3(4f, 5f, 6f)), "the direction of the ray");

            Assert.That(r == new ray(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)), Is.True, "the equality");
            Assert.That(r != new ray(new float3(1f, 2f, 3f), new float3(4f, 5f, 7f)), Is.True, "the inequality");
            Assert.That(r.Equals(new ray(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f))), Is.True,
                "the equality of the value");
            Assert.That(r.GetHashCode(),
                Is.EqualTo(new ray(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)).GetHashCode()), "the hash");
        }
    }
}
