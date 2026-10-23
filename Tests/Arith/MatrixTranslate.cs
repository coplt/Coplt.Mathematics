using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The translation of the space: the matrix of it holds the one of the kind on the diagonal of it and the value of
/// the origin of the space in the fourth column of it, so no axis of the space is scaled and no axis of it is turned,
/// and the value of the fourth column of the matrix is the value the origin of the space is moved by. The member is
/// a member of every kind a number names, so a matrix of a whole number reaches the translation of it as well.
/// </summary>
public class TestMatrixTranslate
{
    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            // the matrix of a translation holds the one of the kind on the diagonal of it and the value of the
            // origin of the space in the fourth column of it
            var t = float4x4.Translate(new float3(1f, 2f, 3f));
            Assert.That(t, Is.EqualTo(new float4x4(1f, 0f, 0f, 1f, 0f, 1f, 0f, 2f, 0f, 0f, 1f, 3f, 0f, 0f, 0f, 1f)),
                "the translation of a value of 3 components");

            // the value of the origin of the space is the fourth column of the matrix and the last row of it is the
            // zero of the kind beside the one of it
            Assert.That((t.c3.x, t.c3.y, t.c3.z, t.c3.w), Is.EqualTo((1f, 2f, 3f, 1f)), "the fourth column");
            Assert.That((t.m30, t.m31, t.m32, t.m33), Is.EqualTo((0f, 0f, 0f, 1f)), "the last row");

            // no axis of the space is scaled and no axis of it is turned, so the translation of the origin of the
            // space is the identity of the matrix
            Assert.That(float4x4.Translate(new float3(0f, 0f, 0f)), Is.EqualTo(float4x4.Identity), "the origin");

            // the value of a transform of the space reads the value of the origin of the space beside the three axes
            // of it, which the fourth column of the matrix holds
            Assert.That(math.mul(t, new float4(0f, 0f, 0f, 1f)).xyz, Is.EqualTo(new float3(1f, 2f, 3f)),
                "the value of the origin of the space");

            // the kind of a component of the value is named by the member that reaches the translation of it
            Assert.That((double4x4.Translate(new double3(1d, 2d, 3d)).c3.x, double4x4.Translate(default).m33),
                Is.EqualTo((1d, 1d)), "double");
            Assert.That(int4x4.Translate(new int3(1, 2, 3)).c3.z, Is.EqualTo(3), "int");
            Assert.That((half4x4.Translate(new half3((half)1f, (half)2f, (half)3f)).c3.x,
                half4x4.Translate(default).m33), Is.EqualTo(((half)1f, (half)1f)), "half");
        }
    }
}
