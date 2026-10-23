using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The skew of a plane: the matrix of it holds the one of the kind of the value on the diagonal of it and the
/// tangents of the two angles everywhere else, so the product of the matrix with a value of 2 components keeps
/// every component of it and adds the product of the tangent of an angle with the other component to it. The axis
/// of a component of the value is the one the tangent of the angle of that component reaches, and the member is a
/// member of a floating point kind alone, since the tangent of an angle is one.
/// </summary>
public class TestMatrixSkew
{
    [Test]
    public void Of2x2()
    {
        using (Assert.EnterMultipleScope())
        {
            // the angles of the zero of the kind keep the plane where it is
            var identity = float2x2.Skew(0f, 0f);
            Assert.That((identity.m00, identity.m01, identity.m10, identity.m11), Is.EqualTo((1f, 0f, 0f, 1f)),
                "the zero of the angles");

            // the tangent of the first angle is the component of the first row of the matrix that is not on the
            // diagonal of it and the one of the second angle is the one of the second row
            var quarter = float2x2.Skew(MathF.PI / 4f, 0f);
            Assert.That(quarter.m00, Is.EqualTo(1f), "the one of the diagonal");
            Assert.That(quarter.m01, Is.EqualTo(1f).Within(1e-5f), "the tangent of the first angle");
            Assert.That(quarter.m10, Is.EqualTo(0f), "the tangent of the second angle");
            Assert.That(quarter.m11, Is.EqualTo(1f), "the one of the diagonal");

            // the angle of a skew is the one the axis of a component of the value turns by: the axis of the
            // second component of the value is the one the first angle of the member reaches
            var axis = math.mul(quarter, new float2(0f, 1f));
            Assert.That(axis.x, Is.EqualTo(1f).Within(1e-5f), "the first component of the axis skewed");
            Assert.That(axis.y, Is.EqualTo(1f), "the second component of the axis skewed");

            // the axis of the first component of the value is the one the second angle of the member reaches
            var other = math.mul(float2x2.Skew(0f, MathF.PI / 4f), new float2(1f, 0f));
            Assert.That(other.x, Is.EqualTo(1f), "the first component of the other axis");
            Assert.That(other.y, Is.EqualTo(1f).Within(1e-5f), "the second component of the other axis");

            // the value whose components the angles are is the two of them, so both forms of the member reach
            // the same matrix
            Assert.That(float2x2.Skew(new float2(0.5f, 0.25f)), Is.EqualTo(float2x2.Skew(0.5f, 0.25f)),
                "the skew of a column");

            // the kind of a component of the value is named by the member that reaches the skew of it
            var halfIdentity = half2x2.Skew((half)0f, (half)0f);
            Assert.That((halfIdentity.m00, halfIdentity.m01, halfIdentity.m10, halfIdentity.m11),
                Is.EqualTo(((half)1f, (half)0f, (half)0f, (half)1f)), "half");
            var doubleSkew = double2x2.Skew(Math.PI / 4, 0d);
            Assert.That(doubleSkew.m00, Is.EqualTo(1d), "the one of the diagonal of the double");
            Assert.That(doubleSkew.m01, Is.EqualTo(1d).Within(1e-15), "the tangent of the first angle of the double");
            Assert.That(double2x2.Skew(new double2(0.5d, 0.25d)), Is.EqualTo(double2x2.Skew(0.5d, 0.25d)),
                "the skew of a column of the double");
        }
    }
}
