using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The members of the class of the math members that read the value of a quaternion: the conjugate of it, the
/// inverse of it, the product of two of them beside the one of a value of 3 components, the one of the angle
/// between two rotations, the ones that read the length of the value, the ones of the interpolation of two of
/// them and the ones that read the value as the one of the exponential and of the logarithm of it.
/// </summary>
public class TestQuaternionMath
{
    /// <summary>Returns the four components of the value of the quaternion.</summary>
    private static (float X, float Y, float Z, float W) Axis(quaternion q) =>
        (q.value.x, q.value.y, q.value.z, q.value.w);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the conjugate of the value turns the three components of the space of it around
            Assert.That(Axis(math.conjugate(new quaternion(1f, 2f, 3f, 4f))), Is.EqualTo((-1f, -2f, -3f, 4f)),
                "the conjugate");

            // the value of the dot product of two quaternions is the one of the four components of them
            var a = new quaternion(1f, 2f, 3f, 4f);
            var b = new quaternion(5f, 6f, 7f, 8f);
            Assert.That(math.dot(a, b), Is.EqualTo(70f), "the dot product");
            Assert.That(math.length_sq(a), Is.EqualTo(30f), "the square of the length");
            Assert.That(math.length(a), Is.EqualTo(MathF.Sqrt(30f)).Within(1e-6f), "the length");

            // the value taken to the length one is the value of it scaled by the reciprocal of the length of it
            Assert.That(Axis(math.normalize(new quaternion(0f, 0f, 3f, 4f))),
                Is.EqualTo((0f, 0f, 0.6f, 0.8f)).Within(1e-6f), "the value taken to the length one");
            Assert.That(math.length(math.normalize(a)), Is.EqualTo(1f).Within(1e-6f), "the length of the value");

            // the inverse of a value of the length one is the conjugate of it
            var q = quaternion.RotateZ(0.7f);
            Assert.That(Axis(math.inverse(q)), Is.EqualTo(Axis(math.conjugate(q))).Within(1e-6f), "the inverse");
            Assert.That(Axis(math.mul(q, math.inverse(q))), Is.EqualTo((0f, 0f, 0f, 1f)).Within(1e-6f),
                "the value beside the inverse of it");

            // the value taken to the length one where the length of it is too short for the kind of the component
            // is the identity of the rotation, which the value of the other member of the length reaches instead
            Assert.That(Axis(math.normalize_safe(new quaternion(1e-30f, 0f, 0f, 0f))), Is.EqualTo((0f, 0f, 0f, 1f)),
                "the value that is too short");
            Assert.That(Axis(math.normalize_safe(new quaternion(0f, 0f, 3f, 4f))),
                Is.EqualTo((0f, 0f, 0.6f, 0.8f)).Within(1e-6f), "the value of a length the kind reads");
            Assert.That(Axis(math.normalize_safe(new quaternion(1e-30f, 0f, 0f, 0f), new quaternion(1f, 2f, 3f, 4f))),
                Is.EqualTo((1f, 2f, 3f, 4f)), "the value of the other one");
        }
    }

    [Test]
    public void Rotation()
    {
        var angle = 0.7f;

        using (Assert.EnterMultipleScope())
        {
            // the product of two quaternions is the rotation of the one of them beside the one of the other one
            var quarter = quaternion.RotateZ(MathF.PI / 2f);
            Assert.That(Axis(math.mul(quarter, quarter)), Is.EqualTo(Axis(quaternion.RotateZ(MathF.PI))).Within(1e-5f),
                "the product of two rotations");
            Assert.That(Axis(math.mul(quarter, quaternion.Identity)), Is.EqualTo(Axis(quarter)),
                "the product with the identity");

            // the value of 3 components a quaternion reaches is the one of the matrix of it
            var q = quaternion.AxisAngle(math.normalize(new float3(1f, 2f, 3f)), angle);
            var v = new float3(1f, 2f, 3f);
            var m = math.mul(new float3x3(q), v);
            var r = math.rotate(q, v);
            Assert.That((r.x, r.y, r.z), Is.EqualTo((m.x, m.y, m.z)).Within(1e-5f), "the value the rotation reaches");
            Assert.That(math.length(r), Is.EqualTo(math.length(v)).Within(1e-5f), "the length of the value");

            // the angle between the rotations of two quaternions is the one of the rotation that takes the one
            // of them to the other one
            Assert.That(math.angle(quaternion.Identity, quaternion.RotateZ(MathF.PI / 2f)),
                Is.EqualTo(MathF.PI / 2f).Within(1e-5f), "the angle of the value");
            Assert.That(math.angle(quaternion.Identity, quaternion.Identity), Is.EqualTo(0f).Within(1e-6f),
                "the angle of the identity");
        }
    }

    [Test]
    public void Interpolation()
    {
        var q1 = quaternion.Identity;
        var q2 = quaternion.RotateZ(MathF.PI / 2f);
        var middle = quaternion.RotateZ(MathF.PI / 4f);

        using (Assert.EnterMultipleScope())
        {
            // the value of the interpolation of two quaternions of a single axis of the space is the rotation of
            // the part of the angle the value of the interpolation names
            Assert.That(Axis(math.nlerp(0.5f, q1, q2)), Is.EqualTo(Axis(middle)).Within(1e-5f), "the linear interpolation");
            Assert.That(Axis(math.nlerp(0f, q1, q2)), Is.EqualTo(Axis(q1)).Within(1e-6f), "the linear interpolation of the zero");
            Assert.That(Axis(math.nlerp(1f, q1, q2)), Is.EqualTo(Axis(q2)).Within(1e-6f), "the linear interpolation of the one");
            Assert.That(Axis(math.slerp(0.5f, q1, q2)), Is.EqualTo(Axis(middle)).Within(1e-5f),
                "the spherical interpolation");
            Assert.That(Axis(math.slerp(0f, q1, q2)), Is.EqualTo(Axis(q1)).Within(1e-6f),
                "the spherical interpolation of the zero");
            Assert.That(Axis(math.slerp(1f, q1, q2)), Is.EqualTo(Axis(q2)).Within(1e-6f),
                "the spherical interpolation of the one");

            // the value of the other quaternion is turned around where the value of the dot product of the two of
            // them is the negative one, so the two values the member reaches are the ones of the closer half
            var turned = new quaternion(-q2.value);
            Assert.That(Axis(math.nlerp(0.5f, q1, turned)), Is.EqualTo(Axis(middle)).Within(1e-5f),
                "the linear interpolation of the value that is turned around");
            Assert.That(Axis(math.slerp(0.5f, q1, turned)), Is.EqualTo(Axis(middle)).Within(1e-5f),
                "the spherical interpolation of the value that is turned around");

            // the value of the interpolation is of the length one
            Assert.That(math.length(math.nlerp(0.3f, q1, q2)), Is.EqualTo(1f).Within(1e-5f), "the length of the linear");
            Assert.That(math.length(math.slerp(0.3f, q1, q2)), Is.EqualTo(1f).Within(1e-5f),
                "the length of the spherical");
        }
    }

    [Test]
    public void ExpLog()
    {
        var q = quaternion.RotateX(0.7f);

        using (Assert.EnterMultipleScope())
        {
            // the logarithm of a unit quaternion is the value of the axis of the rotation of it scaled by the half
            // of the angle of it
            Assert.That(Axis(math.unit_log(q)), Is.EqualTo((0.35f, 0f, 0f, 0f)).Within(1e-5f), "the unit logarithm");
            Assert.That(Axis(math.unit_exp(math.unit_log(q))), Is.EqualTo(Axis(q)).Within(1e-5f), "the unit exponential");

            // the exponential of the logarithm of a quaternion of the length one is the value of it
            Assert.That(Axis(math.log(q)), Is.EqualTo((0.35f, 0f, 0f, 0f)).Within(1e-5f), "the logarithm");
            Assert.That(Axis(math.exp(math.log(q))), Is.EqualTo(Axis(q)).Within(1e-5f), "the exponential");

            // the exponential of a value of the length of it is the value of the unit quaternion of it
            Assert.That(math.exp(math.log(q)).value.w, Is.EqualTo(q.value.w).Within(1e-5f), "the fourth component");
        }
    }
}
