using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The inverse of a square matrix: the type of a single component of a matrix is not a part of the type of it, so
/// the compiler cannot infer it from the value. A call of the member that does not name it reaches the member of
/// the matrix type of the value, which the ex_ classes add to the math class and which the math_ex_ classes add to
/// the value itself. The inverse of a matrix is the adjugate of it divided by the determinant of it, so a matrix
/// whose determinant is the zero of the kind of its component has no inverse.
/// </summary>
public class TestMatrixInverse
{
    [Test]
    public void Of2x2()
    {
        using (Assert.EnterMultipleScope())
        {
            // the inverse of [[1, 3], [2, 4]] is 1 / (1 * 4 - 3 * 2) times [[4, -3], [-2, 1]]
            var m = new float2x2(1f, 3f, 2f, 4f);
            var inverse = math.inverse(m);
            Assert.That((inverse.m00, inverse.m01, inverse.m10, inverse.m11), Is.EqualTo((-2f, 1.5f, 1f, -0.5f)),
                "math.inverse of a value");
            Assert.That(m.inverse(), Is.EqualTo(inverse), "the member of the value");

            // the inverse of the identity of a kind is the identity of it
            var identity = math.inverse(float2x2.Identity);
            Assert.That((identity.m00, identity.m01, identity.m10, identity.m11), Is.EqualTo((1f, 0f, 0f, 1f)),
                "the identity of the value");

            // the kind of a component of the value is named by the member that reaches the inverse of it
            Assert.That(math.inverse(new double2x2(1d, 3d, 2d, 4d)).m00, Is.EqualTo(-2d), "double");
            Assert.That(math.inverse(new half2x2((half)1f, (half)3f, (half)2f, (half)4f)).m00, Is.EqualTo((half)(-2f)),
                "half");

            // the determinant of [[1, 2], [2, 4]] is the zero of the kind, so the value has no inverse: the
            // components of the result of it are the infinities of the kind
            var singular = math.inverse(new float2x2(1f, 2f, 2f, 4f));
            Assert.That(float.IsFinite(singular.m00), Is.False, "the determinant of the value is zero");
            Assert.That(float.IsFinite(singular.m01), Is.False, "the determinant of the value is zero");
        }
    }
}
