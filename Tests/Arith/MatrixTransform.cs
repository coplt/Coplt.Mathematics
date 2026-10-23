using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The transform of a value of 3 components by the space of a matrix of 4 rows and 4 columns: the value of the member
/// is the sum of the three axes of the space of the matrix scaled by the components of the value, which reads the
/// value of the fourth column of the matrix as the one of the origin of the space, so the rotation of the value holds
/// no part of the origin of the space and the transform of it holds the whole of it. The member is a member of the
/// class of the math members and of the value of the matrix, which the value of a floating point kind alone reaches,
/// since the value of a matrix of another kind is not the one of a rotation and a translation of the space.
/// </summary>
public class TestMatrixTransform
{
    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            var v = new float3(4f, 5f, 6f);

            // the rotation of a value by the space of a matrix reads the three axes of it and the transform of the
            // value reads the one of the origin of the space of the matrix as well
            var t = float4x4.Translate(new float3(1f, 2f, 3f));
            Assert.That(math.rotate(t, v), Is.EqualTo(v), "the rotation of a translation");
            Assert.That(math.transform(t, v), Is.EqualTo(new float3(5f, 7f, 9f)), "the transform of a translation");

            // the rotation of a value by the rotation of the space is the value of that rotation and the length of
            // the value is the one it was handed
            var r = float4x4.RotateZ(MathF.PI / 2f);
            var axis = math.rotate(r, new float3(1f, 0f, 0f));
            Assert.That((axis.x, axis.y, axis.z), Is.EqualTo((0f, 1f, 0f)).Within(1e-5f), "the rotation of a rotation");
            Assert.That(math.length(math.rotate(r, new float3(3f, 4f, 0f))), Is.EqualTo(5f).Within(1e-5f),
                "the length of the value of a rotation");
            Assert.That(math.transform(r, new float3(1f, 0f, 0f)),
                Is.EqualTo(math.rotate(r, new float3(1f, 0f, 0f))), "the transform of a rotation");

            // every axis of the space of the matrix scales the value of the component of it
            var s = float4x4.Scale(new float3(2f, 3f, 4f));
            Assert.That(math.transform(s, new float3(1f, 1f, 1f)), Is.EqualTo(new float3(2f, 3f, 4f)),
                "the transform of a scale");

            // the member of the value of the matrix is the same member as the one of the class of the math members
            Assert.That(r.rotate(new float3(1f, 0f, 0f)), Is.EqualTo(math.rotate(r, new float3(1f, 0f, 0f))),
                "the rotation of the value");
            Assert.That(t.transform(v), Is.EqualTo(math.transform(t, v)), "the transform of the value");

            // the kind of a component of the value is named by the member that reaches the transform of it, which is
            // the member of a floating point kind alone
            Assert.That(math.rotate(double4x4.Scale(new double3(2d, 3d, 4d)), new double3(1d, 1d, 1d)),
                Is.EqualTo(new double3(2d, 3d, 4d)), "double");
            Assert.That(math.transform(half4x4.Translate(new half3((half)1f, (half)2f, (half)3f)), default),
                Is.EqualTo(new half3((half)1f, (half)2f, (half)3f)), "half");
        }
    }
}
