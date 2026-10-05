using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The members of a matrix that take the value of a quaternion: the matrix of 3 rows and 3 columns of the rotation
/// of it, the one of 4 rows and 4 columns of the rotation of it beside the translation of the origin of the space,
/// the one of the transform of a translation, a rotation and a scale and the one of the transform of a translation
/// and a rotation.
/// </summary>
public class TestMatrixQuaternion
{
    /// <summary>Returns the nine components of the matrix of 3 rows and 3 columns.</summary>
    private static (float, float, float, float, float, float, float, float, float) Components(float3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <summary>Returns the four components of a column of the matrix of 4 rows and 4 columns.</summary>
    private static (float, float, float, float) Column(float4 c) => (c.x, c.y, c.z, c.w);

    /// <summary>The value the origin of the space is moved by, which the members of the translation read.</summary>
    private static readonly float3 Translation = new(1f, 2f, 3f);

    /// <summary>The value every axis of the space is scaled by, which the member of the scale reads.</summary>
    private static readonly float3 Scale = new(2f, 3f, 4f);

    [Test]
    public void Ctor()
    {
        var angle = 0.7f;
        var axis = math.normalize(new float3(1f, 2f, 3f));
        var angles = new float3(0.3f, -0.7f, 1.1f);

        using (Assert.EnterMultipleScope())
        {
            // the matrix of 3 rows and 3 columns of the value of a quaternion is the rotation of it, which the
            // member of the matrix of the same name reaches
            Assert.That(Components(new float3x3(quaternion.RotateX(angle))),
                Is.EqualTo(Components(float3x3.RotateX(angle))).Within(1e-5f), "the x axis");
            Assert.That(Components(new float3x3(quaternion.RotateY(angle))),
                Is.EqualTo(Components(float3x3.RotateY(angle))).Within(1e-5f), "the y axis");
            Assert.That(Components(new float3x3(quaternion.RotateZ(angle))),
                Is.EqualTo(Components(float3x3.RotateZ(angle))).Within(1e-5f), "the z axis");
            Assert.That(Components(new float3x3(quaternion.AxisAngle(axis, angle))),
                Is.EqualTo(Components(float3x3.AxisAngle(axis, angle))).Within(1e-5f), "the axis of the value");
            Assert.That(Components(new float3x3(quaternion.EulerZXY(angles))),
                Is.EqualTo(Components(float3x3.EulerZXY(angles))).Within(1e-5f), "the angles of the value");

            // the matrix of 4 rows and 4 columns of the value of a quaternion is the rotation of it beside the
            // translation of the origin of the space
            var q = quaternion.AxisAngle(axis, angle);
            var r = new float3x3(q);
            var m = new float4x4(q, Translation);
            Assert.That(Column(m.c0), Is.EqualTo((r.m00, r.m10, r.m20, 0f)).Within(1e-5f), "the first column");
            Assert.That(Column(m.c1), Is.EqualTo((r.m01, r.m11, r.m21, 0f)).Within(1e-5f), "the second column");
            Assert.That(Column(m.c2), Is.EqualTo((r.m02, r.m12, r.m22, 0f)).Within(1e-5f), "the third column");
            Assert.That(Column(m.c3), Is.EqualTo((Translation.x, Translation.y, Translation.z, 1f)), "the translation");

            // the kind of a component of the value is named by the member that reaches the matrix of it
            Assert.That(new double3x3(quaternion_d.RotateZ(angle)).m00,
                Is.EqualTo(double3x3.RotateZ(angle).m00).Within(1e-12), "double");
        }
    }

    [Test]
    public void Transform()
    {
        var angle = 0.7f;
        var q = quaternion.AxisAngle(math.normalize(new float3(1f, 2f, 3f)), angle);
        var v = new float3(1f, 2f, 3f);

        using (Assert.EnterMultipleScope())
        {
            // the member of the transform of a translation and a rotation is the rotation of the value of the
            // quaternion beside the translation of the origin of the space
            var tr = float4x4.TR(Translation, q);
            var trValue = math.transform(tr, v);
            var trExpected = Translation + math.rotate(q, v);
            Assert.That(Column(tr.c3), Is.EqualTo((Translation.x, Translation.y, Translation.z, 1f)),
                "the translation of the value");
            Assert.That((trValue.x, trValue.y, trValue.z),
                Is.EqualTo((trExpected.x, trExpected.y, trExpected.z)).Within(1e-5f), "the value of the space");

            // the member of the transform of a translation, a rotation and a scale is the scale of the space
            // beside the rotation of it and the translation of the origin of it
            var r = new float4x4(q, default);
            var m = float4x4.TRS(Translation, q, Scale);
            Assert.That(Column(m.c0), Is.EqualTo(Column(r.c0 * Scale.x)).Within(1e-5f), "the first column of the scale");
            Assert.That(Column(m.c1), Is.EqualTo(Column(r.c1 * Scale.y)).Within(1e-5f), "the second column of the scale");
            Assert.That(Column(m.c2), Is.EqualTo(Column(r.c2 * Scale.z)).Within(1e-5f), "the third column of the scale");
            Assert.That(Column(m.c3), Is.EqualTo((Translation.x, Translation.y, Translation.z, 1f)),
                "the translation of the transform");
            var value = math.transform(m, v);
            var expected = Translation + math.rotate(q, v * Scale);
            Assert.That((value.x, value.y, value.z),
                Is.EqualTo((expected.x, expected.y, expected.z)).Within(1e-5f), "the value of the space of the transform");
        }
    }
}
