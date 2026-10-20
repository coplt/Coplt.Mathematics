using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rotation that a value of 3 rows and 3 columns holds: the columns of a value that keeps the distances of the
/// space and the angles of it are the axes of the space, so the value is a rotation of the space where the
/// determinant of it is the one of the kind, and the value whose columns are those axes scaled by other lengths
/// holds the same rotation, which the member reads by scaling every column of the value to the length of one. A
/// value whose columns are not the axes of a rotation of the space reaches the rotation of the singular value
/// decomposition of the value, which is the rotation closest to it.
/// </summary>
public class TestMatrixRotation
{
    /// <summary>The rotation of the space that the tests turn the values by</summary>
    private static float3x3 Rotation => new(quaternion.AxisAngle(math.normalize(new float3(1f, 2f, 3f)), 0.7f));

    [Test]
    public void OfARotation()
    {
        using (Assert.EnterMultipleScope())
        {
            // a value that keeps the distances of the space and the angles of it is a rotation of it, so the
            // rotation of the value is the value itself
            var m = Rotation;
            Assert.That(math.hmax(math.abs(new float3x3(math.rotation(m)) - m)), Is.LessThan(1e-5f),
                "math.rotation of a value");
            Assert.That(math.hmax(math.abs(new float3x3(m.rotation()) - m)), Is.LessThan(1e-5f),
                "the member of the value");

            // the rotation of a value is a value of a length of one
            Assert.That(math.length(math.rotation(m)), Is.EqualTo(1f).Within(1e-5f), "the length of the rotation");

            // the rotation of the identity of a kind is the identity of it
            Assert.That(math.hmax(math.abs(new float3x3(math.rotation(float3x3.Identity)) - float3x3.Identity)),
                Is.LessThan(1e-5f), "the identity of the value");
        }
    }

    [Test]
    public void OfAScaledRotation()
    {
        using (Assert.EnterMultipleScope())
        {
            // the columns of a rotation scaled by other lengths are the axes of it scaled, so the value holds the
            // rotation of the axes, which the member reads by scaling the columns of the value to the length of one
            var m = Rotation;
            var scaled = new float3x3(m.c0 * 2f, m.c1 * 3f, m.c2 * 5f);
            Assert.That(math.hmax(math.abs(new float3x3(math.rotation(scaled)) - m)), Is.LessThan(1e-5f),
                "the rotation of a value whose columns are scaled");
            Assert.That(math.hmax(math.abs(new float3x3(scaled.rotation()) - m)), Is.LessThan(1e-5f),
                "the member of the scaled value");
        }
    }

    [Test]
    public void OfAValueThatIsNotARotation()
    {
        using (Assert.EnterMultipleScope())
        {
            // the columns of the value are not the axes of a rotation of the space and the determinant of it is not
            // the one of the kind either, so the member reaches the singular value decomposition of the value,
            // which is the rotation closest to it
            var sheared = new float3x3(2f, 0.5f, 0f, 0f, 1f, 0f, 0f, 0.25f, 3f);
            Assert.That(new float3x3(math.rotation(sheared)), Is.EqualTo(new float3x3(math.svdRotation(sheared))),
                "the rotation of the decomposition of the value");
            Assert.That(math.length(math.rotation(sheared)), Is.EqualTo(1f).Within(1e-5f),
                "the length of the rotation");

            // a value whose determinant is the zero of the kind has no columns that are the axes of a rotation of
            // the space, so the member reaches the decomposition of the value as well
            var singular = new float3x3(1f, 2f, 3f, 2f, 4f, 6f, 1f, 1f, 1f);
            Assert.That(math.length(math.rotation(singular)), Is.EqualTo(1f).Within(1e-5f),
                "the length of the rotation of a value whose determinant is zero");
        }
    }

    [Test]
    public void OfAnotherKind()
    {
        using (Assert.EnterMultipleScope())
        {
            // the kind of a component of the value is named by the member that reaches the rotation of it
            var md = new double3x3(quaternion_d.AxisAngle(math.normalize(new double3(1d, 2d, 3d)), 0.7d));
            Assert.That(math.hmax(math.abs(new double3x3(math.rotation(md)) - md)), Is.LessThan(1e-13d), "double");

            var mh = new half3x3(quaternion_h.AxisAngle(
                math.normalize(new half3((half)1f, (half)2f, (half)3f)), (half)0.7f));
            Assert.That(math.hmax(math.abs(new half3x3(math.rotation(mh)) - mh)), Is.LessThan((half)2e-2f), "half");
        }
    }
}
