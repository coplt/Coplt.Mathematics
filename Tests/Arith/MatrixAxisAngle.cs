using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rotation of the space around an axis of it: the matrix of it is the product of the axis with the one of the
/// kind beside the sine and the cosine of the angle, so the axis of the rotation keeps the value it is handed, the
/// length of the value is the one it was handed where the axis is of the length one, and the rotation of one of the
/// three axes of the space is the rotation of that single axis. The member is a member of a floating point kind
/// alone, since the angle of a rotation is one.
/// </summary>
public class TestMatrixAxisAngle
{
    /// <summary>The axis of the checks, which is of the length one and no two of its components are the same.</summary>
    private static readonly float3 Axis = math.normalize(new float3(1f, -2f, 3f));

    /// <summary>
    /// Returns the matrix of the manual scalar computation of the rotation around the axis of <paramref name="axis"/>
    /// by <paramref name="angle"/>, which is the product of the axis with the one of the kind beside the sine and the
    /// cosine of the angle: the two of them are read out of the three components of the axis and the two of the sine
    /// and the cosine of the angle, and every column of the matrix is the product of the axis with the one of the
    /// component of it the column names beside the cosine of the angle and the two other components of the axis
    /// beside the sine of it.
    /// </summary>
    private static float3x3 AxisAngle(float3 axis, float angle)
    {
        math.sincos(angle, out var s, out var c);
        var t = 1f - c;

        var x = axis.x;
        var y = axis.y;
        var z = axis.z;

        var c0 = new float3(t * x * x + c, t * x * y + s * z, t * x * z - s * y);
        var c1 = new float3(t * x * y - s * z, t * y * y + c, t * y * z + s * x);
        var c2 = new float3(t * x * z + s * y, t * y * z - s * x, t * z * z + c);
        return new float3x3(c0, c1, c2);
    }

    /// <summary>
    /// Returns the nine components of the matrix, which the one of a kind is compared with through the components
    /// of the kind of a single precision number.
    /// </summary>
    private static (float, float, float, float, float, float, float, float, float) Components(float3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <inheritdoc cref="Components(float3x3)"/>
    private static (double, double, double, double, double, double, double, double, double) Components(double3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // the angle of the zero of the kind keeps the space where it is
            Assert.That(Components(float3x3.AxisAngle(Axis, 0f)), Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)),
                "the zero of the angle");

            // the matrix of the rotation around one of the three axes of the space is the one of the rotation of
            // that single axis
            var angle = 0.7f;
            Assert.That(Components(float3x3.AxisAngle(new float3(1f, 0f, 0f), angle)),
                Is.EqualTo(Components(float3x3.RotateX(angle))).Within(1e-5f), "the x axis");
            Assert.That(Components(float3x3.AxisAngle(new float3(0f, 1f, 0f), angle)),
                Is.EqualTo(Components(float3x3.RotateY(angle))).Within(1e-5f), "the y axis");
            Assert.That(Components(float3x3.AxisAngle(new float3(0f, 0f, 1f), angle)),
                Is.EqualTo(Components(float3x3.RotateZ(angle))).Within(1e-5f), "the z axis");

            // the matrix of the rotation around an axis is the one of the manual scalar computation of it
            Assert.That(Components(float3x3.AxisAngle(Axis, angle)),
                Is.EqualTo(Components(AxisAngle(Axis, angle))).Within(1e-5f), "the axis of the value");

            // the axis of a rotation keeps the value it is handed and the length of it is the one of the value
            var axis = math.mul(float3x3.AxisAngle(Axis, angle), Axis);
            Assert.That((axis.x, axis.y, axis.z), Is.EqualTo((Axis.x, Axis.y, Axis.z)).Within(1e-5f),
                "the axis of the rotation");
            Assert.That(math.length(math.mul(float3x3.AxisAngle(Axis, angle), new float3(3f, 4f, 12f))),
                Is.EqualTo(13f).Within(1e-4f), "the length of the value of a rotation");

            // the kind of a component of the value is named by the member that reaches the rotation of it
            Assert.That(Components(double3x3.AxisAngle(new double3(0d, 0d, 1d), Math.PI / 2)),
                Is.EqualTo(Components(double3x3.RotateZ(Math.PI / 2))).Within(1e-12), "double");
            Assert.That((float)half3x3.AxisAngle(new half3((half)1f, (half)0f, (half)0f), (half)0f).m00,
                Is.EqualTo(1f), "half");
        }
    }

    /// <summary>
    /// Returns the value of the upper left of a matrix of 4 rows and 4 columns, which is the nine components of the
    /// 3 rows and the 3 columns of it.
    /// </summary>
    private static (float, float, float, float, float, float, float, float, float) UpperLeft(float4x4 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <inheritdoc cref="UpperLeft(float4x4)"/>
    private static (double, double, double, double, double, double, double, double, double) UpperLeft(double4x4 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <summary>
    /// The rotation of the space around an axis of it of a matrix of 4 rows and 4 columns: the value of the matrix
    /// is the one of the shape of 3 rows and 3 columns of it beside the fourth axis of the space, which the rotation
    /// leaves where it is.
    /// </summary>
    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            var angle = 0.7f;

            // the value of the matrix of the rotation around an axis is the one of the shape of 3 rows and 3 columns
            // of it beside the fourth axis of the space
            Assert.That(float4x4.AxisAngle(Axis, angle),
                Is.EqualTo(new float4x4(float3x3.AxisAngle(Axis, angle), default)), "the axis of the value");

            // the matrix of the rotation around one of the three axes of the space is the one of the rotation of
            // that single axis, which the value of the upper left of the matrix holds
            Assert.That(UpperLeft(float4x4.AxisAngle(new float3(1f, 0f, 0f), angle)),
                Is.EqualTo(Components(float3x3.RotateX(angle))).Within(1e-5f), "the x axis");
            Assert.That(UpperLeft(float4x4.AxisAngle(new float3(0f, 0f, 1f), angle)),
                Is.EqualTo(Components(float3x3.RotateZ(angle))).Within(1e-5f), "the z axis");

            // the fourth axis of the space keeps the value it is handed
            var m = float4x4.AxisAngle(Axis, angle);
            Assert.That((m.c3.x, m.c3.y, m.c3.z, m.c3.w), Is.EqualTo((0f, 0f, 0f, 1f)), "the fourth axis");

            // the zero of the angle keeps the space where it is and the kind of a component of the value is named by
            // the member that reaches the rotation of it
            Assert.That(float4x4.AxisAngle(Axis, 0f), Is.EqualTo(float4x4.Identity), "the zero of the angle");
            Assert.That(half4x4.AxisAngle(new half3((half)1f, (half)0f, (half)0f), (half)0f),
                Is.EqualTo(half4x4.Identity), "half");
            Assert.That(UpperLeft(double4x4.AxisAngle(new double3(0d, 0d, 1d), Math.PI / 2)),
                Is.EqualTo(Components(double3x3.RotateZ(Math.PI / 2))).Within(1e-12), "double");
        }
    }
}
