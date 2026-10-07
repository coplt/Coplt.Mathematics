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

            // the determinant of [[2, 1], [2, 3]] is four, so the inverse of it is 1 / 4 times [[3, -1], [-2, 2]]
            // and every component of the result is a value of the kind exactly
            var general = new float2x2(2f, 1f, 2f, 3f);
            var generalInverse = math.inverse(general);
            Assert.That((generalInverse.m00, generalInverse.m01, generalInverse.m10, generalInverse.m11),
                Is.EqualTo((0.75f, -0.25f, -0.5f, 0.5f)), "a value whose components are not on the diagonal");
            Assert.That(math.mul(general, generalInverse), Is.EqualTo(float2x2.Identity),
                "the product of the value and the inverse of it");

            // a value whose determinant is not a power of two and whose components are not whole numbers: the
            // product of the value and the inverse of it is the identity within the rounding of the arithmetic
            var messy = new float2x2(1.5f, -2.25f, 3.75f, 0.5f);
            Assert.That(math.hmax(math.abs(math.mul(messy, math.inverse(messy)) - float2x2.Identity)),
                Is.LessThan(1e-5f), "the product of a value and the inverse of it");

            // the determinant of [[1, 2], [2, 4]] is the zero of the kind, so the value has no inverse: the
            // components of the result of it are the infinities of the kind
            var singular = math.inverse(new float2x2(1f, 2f, 2f, 4f));
            Assert.That(float.IsFinite(singular.m00), Is.False, "the determinant of the value is zero");
            Assert.That(float.IsFinite(singular.m01), Is.False, "the determinant of the value is zero");
        }
    }

    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // the inverse of a diagonal value is the value whose every component is the one of the kind divided
            // by the component of the value: the inverse of [[1, 0, 0], [0, 2, 0], [0, 0, 4]] is
            // [[1, 0, 0], [0, 0.5, 0], [0, 0, 0.25]]
            var m = new float3x3(1f, 0f, 0f, 0f, 2f, 0f, 0f, 0f, 4f);
            var inverse = math.inverse(m);
            Assert.That((inverse.m00, inverse.m01, inverse.m02), Is.EqualTo((1f, 0f, 0f)),
                "the first row of the value");
            Assert.That((inverse.m10, inverse.m11, inverse.m12), Is.EqualTo((0f, 0.5f, 0f)),
                "the second row of the value");
            Assert.That((inverse.m20, inverse.m21, inverse.m22), Is.EqualTo((0f, 0f, 0.25f)),
                "the third row of the value");
            Assert.That(m.inverse(), Is.EqualTo(inverse), "the member of the value");
            Assert.That(math.mul(m, inverse), Is.EqualTo(float3x3.Identity), "the product with the value");

            // the inverse of a value that keeps the axes of the space, which the transpose of it multiplies to
            // the identity with, is the transpose of it
            var swap = new float3x3(0f, 1f, 0f, 0f, 0f, 1f, 1f, 0f, 0f);
            Assert.That(math.inverse(swap), Is.EqualTo(math.transpose(swap)), "the inverse of the value of the axes");
            Assert.That(math.mul(swap, math.inverse(swap)), Is.EqualTo(float3x3.Identity),
                "the product with the transpose of it");

            // the kind of a component of the value is named by the member that reaches the inverse of it
            Assert.That(math.inverse(new double3x3(1d, 0d, 0d, 0d, 2d, 0d, 0d, 0d, 4d)).m11, Is.EqualTo(0.5d),
                "double");
            Assert.That(math.inverse(new half3x3((half)1f, (half)0f, (half)0f, (half)0f, (half)2f, (half)0f,
                (half)0f, (half)0f, (half)4f)).m11, Is.EqualTo((half)0.5f), "half");

            // the determinant of [[1, 2, 3], [0, 1, 4], [5, 6, 0]] is the one of the kind, so the inverse of it is
            // the adjugate of it, which is [[-24, 18, 5], [20, -15, -4], [-5, 4, 1]] and which every kind of a
            // component holds exactly
            var whole = new float3x3(1f, 2f, 3f, 0f, 1f, 4f, 5f, 6f, 0f);
            var wholeInverse = new float3x3(-24f, 18f, 5f, 20f, -15f, -4f, -5f, 4f, 1f);
            Assert.That(math.inverse(whole), Is.EqualTo(wholeInverse), "the value whose determinant is one");
            Assert.That(math.mul(whole, wholeInverse), Is.EqualTo(float3x3.Identity),
                "the product of the value and the adjugate of it");
            Assert.That(math.inverse(new double3x3(1d, 2d, 3d, 0d, 1d, 4d, 5d, 6d, 0d)).m00, Is.EqualTo(-24d),
                "double");
            Assert.That(math.inverse(new half3x3((half)1f, (half)2f, (half)3f, (half)0f, (half)1f, (half)4f,
                (half)5f, (half)6f, (half)0f)).m00, Is.EqualTo((half)(-24f)), "half");

            // a value whose determinant is not the one of the kind, so the inverse of it is not a value of the
            // kind exactly: the product of the value and the inverse of it is the identity of the kind within the
            // rounding of the arithmetic of it
            var messy = new float3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 10f);
            var messyInverse = math.inverse(messy);
            Assert.That(math.hmax(math.abs(math.mul(messy, messyInverse) - float3x3.Identity)), Is.LessThan(1e-5f),
                "the product of a value and the inverse of it");
            Assert.That(math.determinant(messy) * math.determinant(messyInverse), Is.EqualTo(1f).Within(1e-5f),
                "the determinant of the inverse of a value");
            Assert.That(math.hmax(math.abs(math.inverse(messyInverse) - messy)), Is.LessThan(1e-5f),
                "the inverse of the inverse of a value");
            var messyDouble = new double3x3(1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 10d);
            Assert.That(math.hmax(math.abs(math.mul(messyDouble, math.inverse(messyDouble)) - double3x3.Identity)),
                Is.LessThan(1e-12d), "the product of a value of a double component and the inverse of it");

            // the second row of [[1, 2, 3], [2, 4, 6], [1, 1, 1]] is the first one scaled by two, so the
            // determinant of the value is the zero of the kind and the value has no inverse
            var singular3 = math.inverse(new float3x3(1f, 2f, 3f, 2f, 4f, 6f, 1f, 1f, 1f));
            Assert.That(float.IsFinite(singular3.m00), Is.False, "the determinant of the value is zero");
            Assert.That(float.IsFinite(singular3.m22), Is.False, "the determinant of the value is zero");
        }
    }
}
