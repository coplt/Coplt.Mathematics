using System.Runtime.Intrinsics;
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

    /// <summary>
    /// Returns the nine components of the matrix, which the one of a kind is compared with through the components
    /// of the kind of a single precision number.
    /// </summary>
    private static (float, float, float, float, float, float, float, float, float) Components(float3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <inheritdoc cref="Components(float3x3)"/>
    private static (float, float, float, float, float, float, float, float, float) Components(half3x3 m) =>
        ((float)m.m00, (float)m.m01, (float)m.m02, (float)m.m10, (float)m.m11, (float)m.m12, (float)m.m20,
            (float)m.m21, (float)m.m22);

    /// <inheritdoc cref="Components(float3x3)"/>
    private static (double, double, double, double, double, double, double, double, double) Components(double3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <summary>
    /// The rotation of the space around a single axis: the matrix of it is the one of the plain formula of the
    /// rotation, which holds the cosine of the angle on the diagonal of the matrix and the sine of it beside it, so
    /// the axis of the rotation keeps the value it is handed and the length of it. The member is a member of a
    /// floating point kind alone, since the angle of a rotation is one.
    /// </summary>
    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // the angle of the zero of the kind keeps the space where it is, whatever the axis of the rotation is
            Assert.That(Components(float3x3.RotateX(0f)), Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)),
                "the zero of the angle of the x axis");
            Assert.That(Components(float3x3.RotateY(0f)), Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)),
                "the zero of the angle of the y axis");
            Assert.That(Components(float3x3.RotateZ(0f)), Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)),
                "the zero of the angle of the z axis");

            // the matrix of the rotation of an angle around one axis is the one of the plain formula of it, which
            // holds the sine and the cosine of the angle of the kind: the member of the axis builds the matrix of
            // the two of them without computing anything else, so the two of them agree up to the rounding of the
            // member that reads them
            var angle = 0.3f;
            math.sincos(angle, out var s, out var c);
            Assert.That(Components(float3x3.RotateX(angle)), Is.EqualTo((1f, 0f, 0f, 0f, c, -s, 0f, s, c)).Within(1e-5f),
                "the x axis");
            Assert.That(Components(float3x3.RotateY(angle)), Is.EqualTo((c, 0f, s, 0f, 1f, 0f, -s, 0f, c)).Within(1e-5f),
                "the y axis");
            Assert.That(Components(float3x3.RotateZ(angle)), Is.EqualTo((c, -s, 0f, s, c, 0f, 0f, 0f, 1f)).Within(1e-5f),
                "the z axis");

            // the axis that follows the one of the rotation reaches the one that follows the two of them where the
            // angle of the rotation is a right angle, and the length of the value is the one it was handed
            var y = math.mul(float3x3.RotateX(MathF.PI / 2f), new float3(0f, 1f, 0f));
            Assert.That(y.x, Is.EqualTo(0f).Within(1e-6f), "the first component of the y axis of the x axis");
            Assert.That(y.y, Is.EqualTo(0f).Within(1e-6f), "the second component of the y axis of the x axis");
            Assert.That(y.z, Is.EqualTo(1f).Within(1e-5f), "the y axis of the x axis reached the z axis");
            Assert.That(math.length(math.mul(float3x3.RotateY(MathF.PI / 2f), new float3(3f, 4f, 12f))),
                Is.EqualTo(13f).Within(1e-4f), "the length of the value of a rotation");

            // the kind of a component of the value is named by the member that reaches the rotation of it, which
            // builds the matrix of the sine and the cosine of the angle of that kind
            var hAngle = (half)0.7f;
            math.sincos(hAngle, out var hs, out var hc);
            Assert.That(Components(half3x3.RotateX(hAngle)),
                Is.EqualTo((1f, 0f, 0f, 0f, (float)hc, (float)(-hs), 0f, (float)hs, (float)hc)).Within(1e-2f),
                "the x axis of the half kind");
            var dAngle = 0.7d;
            math.sincos(dAngle, out var ds, out var dc);
            Assert.That(Components(double3x3.RotateX(dAngle)),
                Is.EqualTo((1d, 0d, 0d, 0d, dc, -ds, 0d, ds, dc)).Within(1e-12), "the x axis of the double kind");

            // a column of the matrix is a view of 3 components of the value of 4 components the sine and the cosine
            // of the angle are shuffled into, so the padding lane of it is the zero of that value
            Assert.That(float3x3.RotateZ(angle).c1.vector.GetElement(3), Is.EqualTo(0f),
                "the padding lane of the column of a single precision kind");
            Assert.That(double3x3.RotateZ(dAngle).c1.vector.GetElement(3), Is.EqualTo(0d),
                "the padding lane of the column of a double precision kind");
        }
    }

    /// <summary>
    /// Returns the value of the upper left of a matrix of 4 rows and 4 columns, which is the nine components of the
    /// 3 rows and the 3 columns of it.
    /// </summary>
    private static (float, float, float, float, float, float, float, float, float) UpperLeft(float4x4 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <summary>
    /// The rotation of the space around a single axis of a matrix of 4 rows and 4 columns: the value of the matrix
    /// is the one of the shape of 3 rows and 3 columns of it beside the fourth axis of the space, which the rotation
    /// leaves where it is.
    /// </summary>
    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of the upper left of the matrix is the matrix of the plain formula of the rotation of the
            // axis of the member
            var angle = 0.3f;
            math.sincos(angle, out var s, out var c);
            Assert.That(UpperLeft(float4x4.RotateX(angle)),
                Is.EqualTo((1f, 0f, 0f, 0f, c, -s, 0f, s, c)).Within(1e-5f), "the x axis");
            Assert.That(UpperLeft(float4x4.RotateY(angle)),
                Is.EqualTo((c, 0f, s, 0f, 1f, 0f, -s, 0f, c)).Within(1e-5f), "the y axis");
            Assert.That(UpperLeft(float4x4.RotateZ(angle)),
                Is.EqualTo((c, -s, 0f, s, c, 0f, 0f, 0f, 1f)).Within(1e-5f), "the z axis");

            // the fourth axis of the space keeps the value it is handed, so the last column of the matrix is the
            // zero of the kind beside the one of it and the padding lane of every other column of it is the zero of
            // it as well
            var r = float4x4.RotateZ(angle);
            Assert.That((r.c3.x, r.c3.y, r.c3.z, r.c3.w), Is.EqualTo((0f, 0f, 0f, 1f)), "the fourth axis");
            Assert.That((r.m30, r.m31, r.m32, r.m33), Is.EqualTo((0f, 0f, 0f, 1f)), "the last row");
            Assert.That((r.c0.w, r.c1.w, r.c2.w), Is.EqualTo((0f, 0f, 0f)), "the padding lanes of the axes");

            // the rotation of the space reaches the value of the shape of 3 rows and 3 columns of it beside the
            // fourth axis of the space
            Assert.That(float4x4.RotateZ(angle), Is.EqualTo(new float4x4(float3x3.RotateZ(angle), default)),
                "the rotation of 3 rows and 3 columns");

            // the rotation turns the three axes of the space and leaves the fourth one where it is
            var v = math.mul(float4x4.RotateZ(MathF.PI / 2f), new float4(3f, 4f, 12f, 5f));
            Assert.That((v.x, v.y, v.z, v.w), Is.EqualTo((-4f, 3f, 12f, 5f)).Within(1e-4f),
                "the value of the rotation");

            // the kind of a component of the value is named by the member that reaches the rotation of it
            Assert.That(float4x4.RotateY(0f), Is.EqualTo(float4x4.Identity), "the zero of the angle");
            Assert.That(half4x4.RotateX((half)0f), Is.EqualTo(half4x4.Identity), "half");
            Assert.That(double4x4.RotateZ(0d), Is.EqualTo(double4x4.Identity), "double");
        }
    }
}
