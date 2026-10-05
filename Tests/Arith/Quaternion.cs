using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Simd;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The quaternion: the value of the four components of the kind of it, which holds the rotation of the space of 3
/// rows and 3 columns of it. The members of the type are the value of it, the rotation of an angle around the axis
/// of a value of 3 components, the ones of the three Euler angles, the ones around a single axis of the space, the
/// ones that read the value of the rotation of a matrix and the ones of the look rotation of two values of 3
/// components.
/// </summary>
public class TestQuaternion
{
    /// <summary>
    /// Returns the four components of the value of the quaternion, which is the sine of the half of the angle of
    /// the rotation in the three components of the space and the cosine of the half of it in the fourth one.
    /// </summary>
    private static (float X, float Y, float Z, float W) Axis(quaternion q) =>
        (q.value.x, q.value.y, q.value.z, q.value.w);

    /// <summary>Returns the nine components of the matrix, which the one of a kind is compared with.</summary>
    private static (float, float, float, float, float, float, float, float, float) Components(float3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Axis(quaternion.Identity), Is.EqualTo((0f, 0f, 0f, 1f)), "the identity");
            Assert.That(Axis(new quaternion(1f, 2f, 3f, 4f)), Is.EqualTo((1f, 2f, 3f, 4f)), "the components");
            Assert.That(Axis(new quaternion(new float4(1f, 2f, 3f, 4f))), Is.EqualTo((1f, 2f, 3f, 4f)), "the value");

            // the value of four components reaches the quaternion without a member of its own
            quaternion implicitValue = new float4(1f, 2f, 3f, 4f);
            Assert.That(Axis(implicitValue), Is.EqualTo((1f, 2f, 3f, 4f)), "the implicit value");

            // the equality of two quaternions is the one of the four components of the value of them
            var q = new quaternion(1f, 2f, 3f, 4f);
            Assert.That(q == new quaternion(1f, 2f, 3f, 4f), Is.True, "the equality");
            Assert.That(q == new quaternion(1f, 2f, 3f, 5f), Is.False, "the inequality");
            Assert.That(q != new quaternion(1f, 2f, 3f, 5f), Is.True, "the inequality of the other one");
            Assert.That(q.Equals(new quaternion(1f, 2f, 3f, 4f)), Is.True, "the equality of the value");
            Assert.That(q.GetHashCode(), Is.EqualTo(new quaternion(1f, 2f, 3f, 4f).GetHashCode()), "the hash");
            Assert.That(q.ToString(), Is.EqualTo("quaternion(1, 2, 3, 4)"), "the text");
        }
    }

    [Test]
    public void Rotation()
    {
        var angle = 0.7f;
        var half = angle / 2f;
        math.sincos(half, out var sina, out var cosa);

        using (Assert.EnterMultipleScope())
        {
            // the member of a single axis of the space reads the sine and the cosine of the half of the angle
            Assert.That(Axis(quaternion.RotateX(angle)), Is.EqualTo((sina, 0f, 0f, cosa)).Within(1e-6f), "the x axis");
            Assert.That(Axis(quaternion.RotateY(angle)), Is.EqualTo((0f, sina, 0f, cosa)).Within(1e-6f), "the y axis");
            Assert.That(Axis(quaternion.RotateZ(angle)), Is.EqualTo((0f, 0f, sina, cosa)).Within(1e-6f), "the z axis");

            // the member of the axis of a value of 3 components agrees with the member of the matrix of it
            var axis = math.normalize(new float3(1f, 2f, 3f));
            Assert.That(Components(new float3x3(quaternion.AxisAngle(axis, angle))),
                Is.EqualTo(Components(float3x3.AxisAngle(axis, angle))).Within(1e-5f), "the axis of the value");

            // the member of the angles alone is the one of the z-x-y order, and the member that names the order
            // reaches the ones of the orders
            var angles = new float3(0.3f, -0.7f, 1.1f);
            Assert.That(Axis(quaternion.Euler(angles)), Is.EqualTo(Axis(quaternion.EulerZXY(angles))), "the angles alone");
            Assert.That(Axis(quaternion.Euler(angles, RotationOrder.YZX)),
                Is.EqualTo(Axis(quaternion.EulerYZX(angles))), "the order of the value");

            // the order the three rotations of the angles are applied in is the one of the matrix of the same name
            Assert.That(Components(new float3x3(quaternion.EulerXYZ(angles))),
                Is.EqualTo(Components(float3x3.EulerXYZ(angles))).Within(1e-5f), "the x-y-z order");
            Assert.That(Components(new float3x3(quaternion.EulerXZY(angles))),
                Is.EqualTo(Components(float3x3.EulerXZY(angles))).Within(1e-5f), "the x-z-y order");
            Assert.That(Components(new float3x3(quaternion.EulerYXZ(angles))),
                Is.EqualTo(Components(float3x3.EulerYXZ(angles))).Within(1e-5f), "the y-x-z order");
            Assert.That(Components(new float3x3(quaternion.EulerYZX(angles))),
                Is.EqualTo(Components(float3x3.EulerYZX(angles))).Within(1e-5f), "the y-z-x order");
            Assert.That(Components(new float3x3(quaternion.EulerZXY(angles))),
                Is.EqualTo(Components(float3x3.EulerZXY(angles))).Within(1e-5f), "the z-x-y order");
            Assert.That(Components(new float3x3(quaternion.EulerZYX(angles))),
                Is.EqualTo(Components(float3x3.EulerZYX(angles))).Within(1e-5f), "the z-y-x order");

            // the identity of the rotation is the value of an order the member cannot read
            Assert.That(Axis(quaternion.Euler(angles, (RotationOrder)99)), Is.EqualTo((0f, 0f, 0f, 1f)),
                "the order of the value that is not one");

            // the kind of a component of the value is named by the member that reaches the rotation of it
            Assert.That(quaternion_h.Identity.value.w, Is.EqualTo((half)1f), "half");
            Assert.That(quaternion_d.Identity.value.w, Is.EqualTo(1d), "double");
        }
    }

    [Test]
    public void FromMatrix()
    {
        var angle = 0.7f;

        using (Assert.EnterMultipleScope())
        {
            // the value of the rotation of a matrix is the one of the quaternion of it, which the member reads out
            // of the three columns of the matrix
            Assert.That(Axis(new quaternion(float3x3.RotateX(angle))),
                Is.EqualTo(Axis(quaternion.RotateX(angle))).Within(1e-6f), "the x axis");
            Assert.That(Axis(new quaternion(float3x3.RotateY(angle))),
                Is.EqualTo(Axis(quaternion.RotateY(angle))).Within(1e-6f), "the y axis");
            Assert.That(Axis(new quaternion(float3x3.RotateZ(angle))),
                Is.EqualTo(Axis(quaternion.RotateZ(angle))).Within(1e-6f), "the z axis");
            Assert.That(Axis(new quaternion(float3x3.Identity)), Is.EqualTo((0f, 0f, 0f, 1f)), "the identity");

            // the matrix of four rows and four columns reaches the same member, which reads the upper left of it
            var angles = new float3(0.3f, -0.7f, 1.1f);
            Assert.That(Axis(new quaternion(new float4x4(float3x3.EulerYXZ(angles), default))),
                Is.EqualTo(Axis(quaternion.EulerYXZ(angles))).Within(1e-6f), "the matrix of four rows");

            // the value of the matrix of the quaternion is the one the member read it out of
            var m = float3x3.EulerZXY(angles);
            Assert.That(Components(new float3x3(new quaternion(m))),
                Is.EqualTo(Components(m)).Within(1e-5f), "the matrix of the value");

            // the component of the matrix the value of the quaternion is read out of names which of the two
            // signs of the value is the one it reaches, so a rotation of an angle that turns the first
            // component of the matrix around reaches the value of it as well
            var wide = quaternion.AxisAngle(math.normalize(new float3(1f, 2f, 3f)), 2.4f);
            Assert.That(Components(new float3x3(new quaternion(new float3x3(wide)))),
                Is.EqualTo(Components(new float3x3(wide))).Within(1e-5f), "the value of a wide angle");
        }
    }

    [Test]
    public void LookRotation()
    {
        var forward = math.normalize(new float3(1f, 2f, -3f));
        var up = new float3(0f, 1f, 0f);

        using (Assert.EnterMultipleScope())
        {
            // the rotation of the value that is looked along is the one of the matrix of the view of it
            Assert.That(Components(new float3x3(quaternion.LookRotation(forward, up))),
                Is.EqualTo(Components(float3x3.LookRotation(forward, up))).Within(1e-5f), "the value that is looked along");
            Assert.That(Axis(quaternion.LookRotation(new float3(0f, 0f, 1f), up)),
                Is.EqualTo((0f, 0f, 0f, 1f)), "the value that is looked along of the origin");

            // the member of the safe rotation takes the two values of it to the length one first
            Assert.That(Axis(quaternion.LookRotationSafe(forward * 2f, up * 3f)),
                Is.EqualTo(Axis(quaternion.LookRotation(forward, up))).Within(1e-5f), "the two values of another length");
            Assert.That(Components(new float3x3(quaternion.LookRotationSafe(forward * 2f, up * 3f))),
                Is.EqualTo(Components(float3x3.LookRotationSafe(forward * 2f, up * 3f))).Within(1e-5f),
                "the matrix of the safe rotation");

            // the identity of the rotation is reached where the two values cannot be read
            Assert.That(Axis(quaternion.LookRotationSafe(forward, forward)), Is.EqualTo((0f, 0f, 0f, 1f)),
                "the two values that are collinear");
            Assert.That(Axis(quaternion.LookRotationSafe(new float3(0f, 0f, 0f), up)),
                Is.EqualTo((0f, 0f, 0f, 1f)), "the value that is the zero of the kind");
        }
    }
}
