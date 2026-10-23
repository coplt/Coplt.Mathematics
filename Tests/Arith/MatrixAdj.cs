using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The adjugate of a matrix of 3 rows and 3 columns and the inverse that it reaches beside the determinant of it:
/// the adjugate of a value is the transpose of the value whose columns are the cross products of the columns of
/// the value taken in a cycle, so the product of the value and the adjugate of it is the determinant of it times
/// the identity of the kind of it, and the value has an inverse where the absolute value of the determinant of it
/// is above the epsilon that the member takes. The member that does not take an epsilon names the epsilon of the
/// kind of a component of the value.
/// </summary>
public class TestMatrixAdj
{
    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // the adjugate of [[1, 2, 3], [0, 1, 4], [5, 6, 0]] is the transpose of the value whose components are
            // the cofactors of it, which is [[-24, 18, 5], [20, -15, -4], [-5, 4, 1]], and the determinant of it
            // is the one of the kind, so the adjugate of it is the inverse of it as well
            var m = new float3x3(1f, 2f, 3f, 0f, 1f, 4f, 5f, 6f, 0f);
            var expected = new float3x3(-24f, 18f, 5f, 20f, -15f, -4f, -5f, 4f, 1f);
            var adjugate = math.adj(m, out float det);
            Assert.That(det, Is.EqualTo(1f), "the determinant of the value");
            Assert.That(adjugate, Is.EqualTo(expected), "math.adj of a value");
            Assert.That(math.mul(m, adjugate), Is.EqualTo(det * float3x3.Identity),
                "the product of the value and the adjugate of it");
            Assert.That(m.adj(out float detOfTheValue), Is.EqualTo(expected), "the member of the value");
            Assert.That(detOfTheValue, Is.EqualTo(1f), "the determinant of the value reached with the value");

            // the adjugate of a diagonal value is the diagonal value whose component at an index is the product of
            // the two components of the value at the other two indices: the adjugate of the value whose diagonal
            // is (one, two, four) is the value whose diagonal is (eight, four, two) and the determinant of it is
            // eight
            var diagonal = new float3x3(1f, 0f, 0f, 0f, 2f, 0f, 0f, 0f, 4f);
            Assert.That(math.adj(diagonal, out float diagonalDet),
                Is.EqualTo(new float3x3(8f, 0f, 0f, 0f, 4f, 0f, 0f, 0f, 2f)), "the adjugate of a diagonal value");
            Assert.That(diagonalDet, Is.EqualTo(8f), "the determinant of a diagonal value");

            // the adjugate of the identity of a kind is the identity of it
            Assert.That(math.adj(float3x3.Identity, out float identityDet), Is.EqualTo(float3x3.Identity),
                "the adjugate of the identity");
            Assert.That(identityDet, Is.EqualTo(1f), "the determinant of the identity");

            // the kind of a component of the value is named by the member that reaches the adjugate of it
            Assert.That(math.adj(new double3x3(1d, 2d, 3d, 0d, 1d, 4d, 5d, 6d, 0d), out double doubleDet).m00,
                Is.EqualTo(-24d), "double");
            Assert.That(doubleDet, Is.EqualTo(1d), "double");
            Assert.That(math.adj(new half3x3((half)1f, (half)2f, (half)3f, (half)0f, (half)1f, (half)4f,
                (half)5f, (half)6f, (half)0f), out half halfDet).m00, Is.EqualTo((half)(-24f)), "half");
            Assert.That(halfDet, Is.EqualTo((half)1f), "half");
        }
    }

    [Test]
    public void Inverse()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value has an inverse, which is the adjugate of it divided by the determinant of it
            var m = new float3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 10f);
            Assert.That(math.adj(m, out float3x3 i), Is.True, "the value has an inverse");
            Assert.That(i, Is.EqualTo(math.inverse(m)), "the inverse of the value");
            Assert.That(m.adj(out float3x3 iOfTheValue), Is.True, "the member of the value");
            Assert.That(iOfTheValue, Is.EqualTo(i), "the inverse of the value reached with the value");
            Assert.That(math.mul(m, i), Is.EqualTo(float3x3.Identity), "the product of the value and the inverse of it");

            // the second row of [[1, 2, 3], [2, 4, 6], [1, 1, 1]] is the first one scaled by two, so the
            // determinant of the value is the zero of the kind and the value has no inverse: the adjugate of it is
            // the value that the member stores, which is [[-2, 1, 0], [4, -2, 0], [-2, 1, 0]]
            var singular = new float3x3(1f, 2f, 3f, 2f, 4f, 6f, 1f, 1f, 1f);
            Assert.That(math.adj(singular, out float3x3 singularInverse), Is.False,
                "the determinant of the value is zero");
            Assert.That(singularInverse, Is.EqualTo(new float3x3(-2f, 1f, 0f, 4f, -2f, 0f, -2f, 1f, 0f)),
                "the adjugate of a value whose determinant is zero");
            math.adj(singular, out float singularDet);
            Assert.That(singularDet, Is.EqualTo(0f), "the determinant of the value");

            // the member that takes an epsilon answers whether the determinant of the value is above it, so a
            // value whose determinant is eight has an inverse where the epsilon is below eight and no inverse
            // where it is above it
            var diagonal = new float3x3(1f, 0f, 0f, 0f, 2f, 0f, 0f, 0f, 4f);
            Assert.That(math.adj(diagonal, out float3x3 above, 9f), Is.False, "the epsilon is above the determinant");
            Assert.That(above, Is.EqualTo(new float3x3(8f, 0f, 0f, 0f, 4f, 0f, 0f, 0f, 2f)),
                "the adjugate of the value where the epsilon is above the determinant of it");
            Assert.That(math.adj(diagonal, out float3x3 below, 7f), Is.True, "the epsilon is below the determinant");
            Assert.That(below, Is.EqualTo(new float3x3(1f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.25f)),
                "the inverse of the value where the epsilon is below the determinant of it");
            Assert.That(diagonal.adj(out float3x3 belowOfTheValue, 7f), Is.True, "the member of the value");
            Assert.That(belowOfTheValue, Is.EqualTo(below), "the inverse of the value reached with the value");

            // the kind of a component of the value names its own epsilon as well
            Assert.That(math.adj(new double3x3(1d, 0d, 0d, 0d, 2d, 0d, 0d, 0d, 4d), out double3x3 doubleInverse),
                Is.True, "double");
            Assert.That(doubleInverse.m11, Is.EqualTo(0.5d), "double");
            Assert.That(math.adj(new half3x3((half)1f, (half)0f, (half)0f, (half)0f, (half)2f, (half)0f,
                (half)0f, (half)0f, (half)4f), out half3x3 halfInverse), Is.True, "half");
            Assert.That(halfInverse.m11, Is.EqualTo((half)0.5f), "half");
        }
    }
}
