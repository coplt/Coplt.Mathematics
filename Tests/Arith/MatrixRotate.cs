using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rotation of a plane around the origin: the matrix of it holds the cosine of the angle beside the negative
/// of the sine of it in the first row and the sine beside the cosine in the second one, so the product of the
/// matrix with a value of 2 components is the value of the rotation of it and the first axis of the plane reaches
/// the second one where the angle of the rotation is a right angle. The member is a member of a floating point
/// kind alone, since the angle of a rotation is one.
/// </summary>
public class TestMatrixRotate
{
    [Test]
    public void Of2x2()
    {
        using (Assert.EnterMultipleScope())
        {
            // the angle of the zero of the kind keeps the plane where it is
            var identity = float2x2.Rotate(0f);
            Assert.That((identity.m00, identity.m01, identity.m10, identity.m11), Is.EqualTo((1f, 0f, 0f, 1f)),
                "the zero of the angle");

            // the sine of a right angle is the one of the kind and the cosine of it is the zero of it
            var quarter = float2x2.Rotate(MathF.PI / 2f);
            Assert.That(quarter.m00, Is.EqualTo(0f).Within(1e-6f), "the cosine of a right angle");
            Assert.That(quarter.m01, Is.EqualTo(-1f).Within(1e-6f), "the negative of the sine of a right angle");
            Assert.That(quarter.m10, Is.EqualTo(1f).Within(1e-5f), "the sine of a right angle");
            Assert.That(quarter.m11, Is.EqualTo(0f).Within(1e-6f), "the cosine of a right angle");

            // the axis of the first component of the value is the one that reaches the second axis of the plane
            var axis = math.mul(quarter, new float2(1f, 0f));
            Assert.That(axis.x, Is.EqualTo(0f).Within(1e-6f), "the first component of the axis reached");
            Assert.That(axis.y, Is.EqualTo(1f).Within(1e-5f), "the second component of the axis reached");

            // the rotation of two angles is the rotation of the sum of them
            var eighth = float2x2.Rotate(MathF.PI / 4f);
            var composed = math.mul(eighth, eighth);
            Assert.That(composed.m00, Is.EqualTo(quarter.m00).Within(1e-5f), "the composition of two rotations");
            Assert.That(composed.m01, Is.EqualTo(quarter.m01).Within(1e-5f), "the composition of two rotations");
            Assert.That(composed.m10, Is.EqualTo(quarter.m10).Within(1e-5f), "the composition of two rotations");
            Assert.That(composed.m11, Is.EqualTo(quarter.m11).Within(1e-5f), "the composition of two rotations");

            // the length of the value is the one it was handed, which the rotation keeps
            Assert.That(math.length(math.mul(quarter, new float2(3f, 4f))), Is.EqualTo(5f).Within(1e-5f),
                "the length of the value of a rotation");

            // the kind of a component of the value is named by the member that reaches the rotation of it
            var halfIdentity = half2x2.Rotate((half)0f);
            Assert.That((halfIdentity.m00, halfIdentity.m01, halfIdentity.m10, halfIdentity.m11),
                Is.EqualTo(((half)1f, (half)0f, (half)0f, (half)1f)), "half");
            var doubleQuarter = double2x2.Rotate(Math.PI / 2);
            Assert.That(doubleQuarter.m10, Is.EqualTo(1d).Within(1e-15), "double");
            Assert.That(doubleQuarter.m01, Is.EqualTo(-1d).Within(1e-15), "double");
        }
    }
}
