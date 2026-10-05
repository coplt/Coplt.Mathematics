using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The matrix of the rotation of the space that looks along a value while another one stays over it: the columns of
/// it are the axis that is at a right angle with the two of them, the value that is looked along crossed with that
/// axis, and the value that is looked along, so the matrix reaches the value that looks along the first of the two
/// while the second one stays over it. The member is a member of a floating point kind alone, since the axis of the
/// rotation is read out of the two values of it.
/// </summary>
public class TestMatrixLookRotation
{
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
            // the value that is looked along is the third axis of the space and the one that stays over it is the
            // second one, so the axis that is at a right angle with the two of them is the first one and the matrix
            // of the rotation is the identity of it
            Assert.That(Components(float3x3.LookRotation(new float3(0f, 0f, 1f), new float3(0f, 1f, 0f))),
                Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)).Within(1e-6f), "the axes of the identity");

            // the third column of the matrix is the value that is looked along, and the second one is the value
            // that stays over it where it is taken to the plane the value that is looked along is at a right angle
            // with
            var forward = math.normalize(new float3(1f, 2f, -3f));
            var up = new float3(0f, 1f, 0f);
            var m = float3x3.LookRotation(forward, up);
            Assert.That((m.m02, m.m12, m.m22), Is.EqualTo((forward.x, forward.y, forward.z)).Within(1e-5f),
                "the value that is looked along");
            var over = math.normalize(up - forward * math.dot(up, forward));
            Assert.That((m.m01, m.m11, m.m21), Is.EqualTo((over.x, over.y, over.z)).Within(1e-5f),
                "the value that stays over the one that is looked along");

            // the columns of the matrix are the three axes of the space that are at a right angle with one another,
            // so the rotation keeps the length of the value it is handed
            Assert.That(math.length(m.c0), Is.EqualTo(1f).Within(1e-5f), "the length of the first column");
            Assert.That(math.dot(m.c0, m.c1), Is.EqualTo(0f).Within(1e-5f), "the first column and the second one");
            Assert.That(math.dot(m.c1, m.c2), Is.EqualTo(0f).Within(1e-5f), "the second column and the third one");
            Assert.That(math.length(math.mul(m, new float3(3f, 4f, 12f))), Is.EqualTo(13f).Within(1e-4f),
                "the length of the value of a rotation");

            // the member of the safe rotation takes the two values to the length one first, so a value that is of
            // another length reaches the same rotation
            Assert.That(Components(float3x3.LookRotationSafe(new float3(0f, 0f, 3f), new float3(0f, 5f, 0f))),
                Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)).Within(1e-5f), "the axes of the identity");
            var safe = float3x3.LookRotationSafe(forward * 2f, up * 3f);
            Assert.That((safe.m02, safe.m12, safe.m22), Is.EqualTo((forward.x, forward.y, forward.z)).Within(1e-5f),
                "the value that is looked along of the safe rotation");
            Assert.That((safe.m01, safe.m11, safe.m21), Is.EqualTo((over.x, over.y, over.z)).Within(1e-5f),
                "the value that stays over the one that is looked along of the safe rotation");

            // the identity of the matrix is reached where the two values are collinear or where one of them is so
            // short or so long that the member cannot read it
            Assert.That(Components(float3x3.LookRotationSafe(new float3(0f, 0f, 1f), new float3(0f, 0f, 2f))),
                Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)), "the two values that are collinear");
            Assert.That(Components(float3x3.LookRotationSafe(new float3(0f, 0f, 1e30f), new float3(0f, 1f, 0f))),
                Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)), "the value that is too long");

            // the kind of a component of the value is named by the member that reaches the rotation of it
            Assert.That(Components(double3x3.LookRotation(new double3(0d, 0d, 1d), new double3(0d, 1d, 0d))),
                Is.EqualTo((1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 1d)).Within(1e-15), "double");
            Assert.That(Components(double3x3.LookRotationSafe(new double3(0d, 0d, 1d), new double3(0d, 0d, 2d))),
                Is.EqualTo((1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 1d)), "the safe rotation of a double");
        }
    }
}
