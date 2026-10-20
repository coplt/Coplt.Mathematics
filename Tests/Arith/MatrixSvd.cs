using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rotation that the singular value decomposition of a value of 3 rows and 3 columns reaches: the value is the
/// product of a rotation of the space, a value that scales the three axes of it and another rotation of it, so the
/// rotation of the value is the product of the two rotations of it. A value that keeps the distances of the space
/// and the angles of it is a rotation of it, so the rotation of the value is the value itself, and the columns of a
/// rotation scaled by other lengths hold the same rotation beside the lengths of the columns.
/// </summary>
public class TestMatrixSvd
{
    /// <summary>The rotation of the space that the tests turn the values by</summary>
    private static float3x3 Rotation => new(quaternion.AxisAngle(math.normalize(new float3(1f, 2f, 3f)), 0.7f));

    [Test]
    public void OfARotation()
    {
        using (Assert.EnterMultipleScope())
        {
            // a value that keeps the distances of the space is a rotation of it, so the rotation of the singular
            // value decomposition of it is the value itself
            var m = Rotation;
            Assert.That(math.hmax(math.abs(new float3x3(math.svdRotation(m)) - m)), Is.LessThan(1e-5f),
                "math.svdRotation of a value");
            Assert.That(math.hmax(math.abs(new float3x3(m.svdRotation()) - m)), Is.LessThan(1e-5f),
                "the member of the value");

            // the rotation of the value is a value of a length of one
            Assert.That(math.length(math.svdRotation(m)), Is.EqualTo(1f).Within(1e-5f), "the length of the rotation");

            // the rotation of the identity of a kind is the identity of it
            Assert.That(math.hmax(math.abs(new float3x3(math.svdRotation(float3x3.Identity)) - float3x3.Identity)),
                Is.LessThan(1e-5f), "the identity of the value");
        }
    }

    [Test]
    public void OfAScaledRotation()
    {
        using (Assert.EnterMultipleScope())
        {
            // the columns of a rotation scaled by other lengths are the axes of it scaled, so the singular values
            // of the value are the lengths of the columns and the rotations of the decomposition leave the rotation
            // of the value alone
            var m = Rotation;
            var scaled = new float3x3(m.c0 * 2f, m.c1 * 3f, m.c2 * 5f);
            Assert.That(math.hmax(math.abs(new float3x3(math.svdRotation(scaled)) - m)), Is.LessThan(1e-5f),
                "the rotation of a value whose columns are scaled");

            // the order of the lengths of the columns does not change the rotation either, which the sort of the
            // singular values of the decomposition turns the second rotation of it around for
            var reversed = new float3x3(m.c0 * 5f, m.c1 * 1.5f, m.c2 * 0.25f);
            Assert.That(math.hmax(math.abs(new float3x3(math.svdRotation(reversed)) - m)), Is.LessThan(1e-5f),
                "the rotation of a value whose columns are scaled by other lengths");
        }
    }

    [Test]
    public void OfAnotherKind()
    {
        using (Assert.EnterMultipleScope())
        {
            // the kind of a component of the value is named by the member that reaches the rotation of it
            var md = new double3x3(quaternion_d.AxisAngle(math.normalize(new double3(1d, 2d, 3d)), 0.7d));
            Assert.That(math.hmax(math.abs(new double3x3(math.svdRotation(md)) - md)), Is.LessThan(1e-13d), "double");

            var mh = new half3x3(quaternion_h.AxisAngle(
                math.normalize(new half3((half)1f, (half)2f, (half)3f)), (half)0.7f));
            Assert.That(math.hmax(math.abs(new half3x3(math.svdRotation(mh)) - mh)), Is.LessThan((half)2e-2f), "half");
        }
    }
}
