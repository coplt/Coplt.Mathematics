using Coplt.Mathematics;

namespace Tests.Geometries;

/// <summary>
/// A plane of the space of 3 dimensions, which is the set of the points that the value of the plane takes to the
/// zero of the kind: the coefficients of the plane are the components of its normal beside its distance from the
/// origin, so the value of the plane at a point is the distance of the point from the plane signed by the half of
/// the space it lies in, and the plane whose normal is of the length of one reads that distance itself. The
/// constructors of the plane normalize the coefficients that they are handed beside the one that takes the four
/// coefficients of the plane equation, which holds them as they are.
/// </summary>
public class TestPlane
{
    [Test]
    public void OfTheCoefficients()
    {
        using (Assert.EnterMultipleScope())
        {
            // (0, 2, 0, 4) is the plane equation 2y + 4 = 0 normalized, which is the plane y = -2
            var p = new plane(0f, 2f, 0f, 4f);
            Assert.That(p.normal, Is.EqualTo(new float3(0f, 1f, 0f)), "the normal of the plane");
            Assert.That(p.distance, Is.EqualTo(2f), "the distance of the plane");

            // the distance of a point from the plane is signed by the half of the space that the point lies in
            Assert.That(p.distance_to_point(new float3(0f, 3f, 0f)), Is.EqualTo(5f),
                "the distance of a point of the half the normal points to");
            Assert.That(p.distance_to_point(new float3(0f, -2f, 0f)), Is.EqualTo(0f),
                "the distance of a point that lies in the plane");
            Assert.That(p.distance_to_point(new float3(0f, -5f, 0f)), Is.EqualTo(-3f),
                "the distance of a point of the other half");

            // the closest point of the plane to a point is the point where the normal of the plane is moved along
            Assert.That(p.projection(new float3(1f, 3f, 2f)), Is.EqualTo(new float3(1f, -2f, 2f)),
                "the projection of a point");
            Assert.That(p.closest_point(new float3(1f, 3f, 2f)), Is.EqualTo(new float3(1f, -2f, 2f)),
                "the closest point of the plane");

            // the flipped plane is the plane of the same points whose normal points in the opposite direction
            Assert.That(p.flipped.normal, Is.EqualTo(new float3(0f, -1f, 0f)), "the normal of the flipped plane");
            Assert.That(p.flipped.distance, Is.EqualTo(-2f), "the distance of the flipped plane");
            Assert.That(p.flipped.distance_to_point(new float3(0f, 3f, 0f)), Is.EqualTo(-5f),
                "the distance of a point from the flipped plane");

            // the plane is the four coefficients of the plane equation
            Assert.That((float4)p, Is.EqualTo(new float4(0f, 1f, 0f, 2f)), "the coefficients of the plane");
        }
    }

    [Test]
    public void OfTheConstructors()
    {
        using (Assert.EnterMultipleScope())
        {
            // the normal of any length is normalized by the constructor
            Assert.That(new plane(new float3(0f, 2f, 0f), 4f), Is.EqualTo(new plane(0f, 2f, 0f, 4f)),
                "the normal and the distance");

            // the distance of the plane of a normal and a point that lies in it is the one that takes the point
            // to the zero of the kind: -y + 5 = 0 is the plane y = 5
            var point = new plane(new float3(0f, -3f, 0f), new float3(1f, 5f, 2f));
            Assert.That(point.normal, Is.EqualTo(new float3(0f, -1f, 0f)), "the normal of the plane of a point");
            Assert.That(point.distance, Is.EqualTo(5f), "the distance of the plane of a point");
            Assert.That(point.distance_to_point(new float3(0f, 5f, 0f)), Is.EqualTo(0f),
                "the point that the plane was built out of");

            // the normal of the plane of two vectors that lie in it is the cross product of them, which is the
            // vector perpendicular to the both: -y + 2 = 0 is the plane y = 2
            var vectors = new plane(new float3(1f, 0f, 0f), new float3(0f, 0f, 1f), new float3(0f, 2f, 0f));
            Assert.That(vectors.normal, Is.EqualTo(new float3(0f, -1f, 0f)), "the normal of the plane of vectors");
            Assert.That(vectors.distance, Is.EqualTo(2f), "the distance of the plane of vectors");
            Assert.That(vectors.distance_to_point(new float3(3f, 2f, 4f)), Is.EqualTo(0f),
                "the point that the plane was built out of");

            // the two members of a normal of the length of one hold the coefficients as they are
            Assert.That(plane.CreateFromUnitNormalAndDistance(new float3(0f, 1f, 0f), 3f),
                Is.EqualTo(new plane(new float4(0f, 1f, 0f, 3f))), "the unit normal and the distance");
            Assert.That(plane.CreateFromUnitNormalAndPointInPlane(new float3(0f, 1f, 0f), new float3(0f, -4f, 0f)),
                Is.EqualTo(new plane(new float4(0f, 1f, 0f, 4f))), "the unit normal and the point");
        }
    }

    [Test]
    public void Normalize()
    {
        using (Assert.EnterMultipleScope())
        {
            // the plane of a value that is not normalized is the plane of the same points whose normal is of the
            // length of one
            var p = new plane(new float4(0f, 2f, 0f, 4f));
            Assert.That(math.normalize(p), Is.EqualTo(new plane(0f, 1f, 0f, 2f)), "math.normalize of a plane");
            Assert.That(p.normalize(), Is.EqualTo(new plane(0f, 1f, 0f, 2f)), "the member of the plane");

            // the plane of a normalized plane is the plane itself
            var q = new plane(0f, 3f, 0f, -6f);
            Assert.That(math.normalize(q), Is.EqualTo(q), "the normalized plane");
        }
    }

    [Test]
    public void Equality()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new plane(0f, 2f, 0f, 4f) == new plane(0f, 1f, 0f, 2f), Is.True, "the equality");
            Assert.That(new plane(0f, 2f, 0f, 4f) != new plane(0f, 1f, 0f, 3f), Is.True, "the inequality");
            Assert.That(new plane(0f, 2f, 0f, 4f).Equals(new plane(0f, 1f, 0f, 2f)), Is.True,
                "the equality of the value");
            Assert.That(new plane(0f, 2f, 0f, 4f).GetHashCode(),
                Is.EqualTo(new plane(0f, 1f, 0f, 2f).GetHashCode()), "the hash");
        }
    }
}
