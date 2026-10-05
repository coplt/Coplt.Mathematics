using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The projection of the space: the matrix of the projection reads the value of the volume of the view, which is the
/// space between the two planes at the distance of the near value and the one of the far value of it from the eye of
/// read as the value of the cube of the view, whose value on every axis of the space is between the negative one of
/// the kind and the one of it. The range of the ±z axis of the cube is the reversed one, so the value of the near
/// plane of the volume is the one of the kind on that axis and the one of the far plane of it is the zero of it, which
/// holds the most of the precision of the kind beside the near plane of the volume, and the depth of a graphics api
/// that reads the value of the cube is cleared as the one of the kind and tested with the value that is greater than
/// the other one. The member is a member of a floating point kind alone, since the value of a projection is the one of
/// a floating point component.
/// </summary>
public class TestMatrixProjection
{
    /// <summary>
    /// Returns the value of the cube of the view the point <paramref name="p"/> of the space is read as by the
    /// projection <paramref name="m"/>, which is the value of the projection of the point beside the value of the
    /// fourth component of it.
    /// </summary>
    private static (float x, float y, float z) Cube(float4x4 m, float3 p)
    {
        var v = math.mul(m, new float4(p, 1f));
        return (v.x / v.w, v.y / v.w, v.z / v.w);
    }

    [Test]
    public void Ortho()
    {
        using (Assert.EnterMultipleScope())
        {
            // the projection of the left of it reads the value of the volume of the origin of the space of the view as
            // the one of the cube of the view, whose range of the ±z axis is the reversed one: the value
            // of the near plane is the one of the kind and the one of the far plane is the zero of it
            var o = float4x4.Ortho(2f, 4f, 1f, 5f);
            Assert.That(Cube(o, new float3(0f, 0f, 1f)), Is.EqualTo((0f, 0f, 1f)).Within(1e-6f), "the near plane");
            Assert.That(Cube(o, new float3(0f, 0f, 5f)), Is.EqualTo((0f, 0f, 0f)).Within(1e-6f), "the far plane");
            Assert.That(Cube(o, new float3(1f, 2f, 3f)), Is.EqualTo((1f, 1f, 0.5f)).Within(1e-6f),
                "the corner of the volume");
            Assert.That(Cube(o, new float3(-1f, -2f, 3f)), Is.EqualTo((-1f, -1f, 0.5f)).Within(1e-6f),
                "the other corner of the volume");

            // the projection of the right of it reads the value of the volume of the negative z axis of the space,
            // which the near plane of the volume reads as the one of the origin of the cube of the view
            var rh = float4x4.Ortho_RH(2f, 4f, 1f, 5f);
            Assert.That(Cube(rh, new float3(0f, 0f, -1f)), Is.EqualTo((0f, 0f, 1f)).Within(1e-6f),
                "the near plane of the right of it");
            Assert.That(Cube(rh, new float3(0f, 0f, -5f)), Is.EqualTo((0f, 0f, 0f)).Within(1e-6f),
                "the far plane of the right of it");

            // the projection of the value of a view volume of the two planes of every axis of the space reads the
            // value of the center of the volume as the one of the origin of the space of the cube
            var off = float4x4.Ortho(-1f, 3f, -4f, 4f, 1f, 5f);
            Assert.That(Cube(off, new float3(1f, 0f, 1f)), Is.EqualTo((0f, 0f, 1f)).Within(1e-6f),
                "the center of the volume");
            Assert.That(Cube(off, new float3(-1f, -4f, 5f)), Is.EqualTo((-1f, -1f, 0f)).Within(1e-6f),
                "the corner of the volume");
            var offRh = float4x4.Ortho_RH(-1f, 3f, -4f, 4f, 1f, 5f);
            Assert.That(Cube(offRh, new float3(1f, 0f, -1f)), Is.EqualTo((0f, 0f, 1f)).Within(1e-6f),
                "the center of the volume of the right of it");
            Assert.That(Cube(offRh, new float3(3f, 4f, -5f)), Is.EqualTo((1f, 1f, 0f)).Within(1e-6f),
                "the corner of the volume of the right of it");
        }
    }

    [Test]
    public void Perspective()
    {
        using (Assert.EnterMultipleScope())
        {
            // the projection of the value of a field of view reads the value of the volume of the origin of the space
            // of the view as the one of the cube of the view, and the value of the field of view is the
            // angle of the y axis of the space of the view
            var p = float4x4.PerspectiveFov(MathF.PI / 2f, 1f, 1f, 5f);
            Assert.That(Cube(p, new float3(0f, 0f, 1f)), Is.EqualTo((0f, 0f, 1f)).Within(1e-6f), "the near plane");
            Assert.That(Cube(p, new float3(0f, 0f, 5f)), Is.EqualTo((0f, 0f, 0f)).Within(1e-6f), "the far plane");
            Assert.That(Cube(p, new float3(0f, 1f, 1f)), Is.EqualTo((0f, 1f, 1f)).Within(1e-6f),
                "the top of the near plane");
            Assert.That(Cube(p, new float3(1f, 0f, 1f)), Is.EqualTo((1f, 0f, 1f)).Within(1e-6f),
                "the right of the near plane");

            // the aspect of the view is the value of the x axis of the space of it beside the one of the y
            // axis of it, so the value of the volume of a view of an aspect of the value is read as the one of the
            // cube, which the y axis of the space of the view does not reach
            var wide = float4x4.PerspectiveFov(MathF.PI / 2f, 2f, 1f, 5f);
            Assert.That(Cube(wide, new float3(0f, 1f, 1f)), Is.EqualTo((0f, 1f, 1f)).Within(1e-6f),
                "the top of the near plane of the value");
            Assert.That(Cube(wide, new float3(2f, 0f, 1f)), Is.EqualTo((1f, 0f, 1f)).Within(1e-6f),
                "the right of the near plane of the value");

            // the projection of the right of it reads the value of the volume of the negative z axis of the space
            // and the member that names the rows of the matrix reads the value of the projection of the value of the
            // left of it on the rows of the matrix
            var rh = float4x4.PerspectiveFov_RH(MathF.PI / 2f, 1f, 1f, 5f);
            Assert.That(rh, Is.EqualTo(new float4x4(
                1f, 0f, 0f, 0f,
                0f, 1f, 0f, 0f,
                0f, 0f, 0.25f, 1.25f,
                0f, 0f, -1f, 0f)), "the projection of the right of it");
            Assert.That(float4x4.PerspectiveFov_Row(MathF.PI / 2f, 1f, 1f, 5f),
                Is.EqualTo(math.transpose(p)), "the rows of the projection");
            Assert.That(float4x4.PerspectiveFov_RH_Row(MathF.PI / 2f, 1f, 1f, 5f),
                Is.EqualTo(math.transpose(rh)), "the rows of the projection of the right of it");

            // the projection of the value of an infinite field of view has no far plane, so the value of the near plane
            // of it is the one of the kind and the value of the +z axis of a point beyond the near plane of it is the
            // one of the near plane of the volume beside the one of the +z axis of the point
            var infinite = float4x4.PerspectiveFov(MathF.PI / 2f, 1f, 1f);
            Assert.That(Cube(infinite, new float3(0f, 0f, 1f)), Is.EqualTo((0f, 0f, 1f)).Within(1e-6f),
                "the near plane of the infinite value");
            Assert.That(Cube(infinite, new float3(0f, 0f, 5f)).z, Is.EqualTo(0.2f).Within(1e-6f),
                "the value of the +z axis of a point beyond the near plane of the infinite value");
            Assert.That(float4x4.PerspectiveFov_Row(MathF.PI / 2f, 1f, 1f),
                Is.EqualTo(math.transpose(infinite)), "the rows of the infinite value");
            Assert.That(float4x4.PerspectiveFov_RH_Row(MathF.PI / 2f, 1f, 1f),
                Is.EqualTo(math.transpose(float4x4.PerspectiveFov_RH(MathF.PI / 2f, 1f, 1f))),
                "the rows of the infinite value of the right of it");

            // the kind of a component of the value is named by the member that reaches the projection of it
            Assert.That(double4x4.PerspectiveFov(Math.PI / 2, 1d, 1d, 5d).c2.z, Is.EqualTo(-0.25).Within(1e-12),
                "double");
            Assert.That((float)half4x4.Ortho((half)2f, (half)4f, (half)1f, (half)5f).c0.x, Is.EqualTo(1f).Within(1e-2f),
                "half");
        }
    }
}
