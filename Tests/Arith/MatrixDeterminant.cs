using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The determinant of a square matrix: the type of a single component of a matrix is not a part of the type of it
/// and the type of a column of it is not one either, so the compiler cannot infer them from the value, and a call
/// of the member that does not name them reaches the member of the matrix type of the value. The members of the
/// shapes that the generator emits the members of reach the member of the class of the shape, and the members of
/// the shapes that name the matrix type themselves reach the member of the math class that names it and the member
/// of the value that names it. The determinant of a matrix of 2 rows and 2 columns is the area of the parallelogram
/// the columns of it are the sides of, the one of a matrix of 3 rows and 3 columns is the volume of the
/// parallelepiped the columns of it are the sides of and the one of a matrix of 4 rows and 4 columns is the four
/// dimensional volume of the parallelepiped the columns of it are the sides of, every one of them signed by the
/// order of the columns, so the kind of a component of the value reaches the determinant of it as well.
/// </summary>
public class TestMatrixDeterminant
{
    [Test]
    public void Of2x2()
    {
        using (Assert.EnterMultipleScope())
        {
            // the determinant of a matrix of 2 rows and 2 columns is the difference of the products of the two
            // diagonals of it: [[1, 3], [2, 4]] is 1 * 4 - 3 * 2
            var m = new float2x2(1f, 3f, 2f, 4f);
            Assert.That(math.determinant(m), Is.EqualTo(-2f), "math.determinant of a value");
            Assert.That(m.determinant(), Is.EqualTo(-2f), "the member of the value");

            // the columns of the value are the same line, so the area of the parallelogram they are the sides of
            // is the zero of the kind and the determinant of the value is the zero of it as well
            Assert.That(math.determinant(new float2x2(1f, 2f, 2f, 4f)), Is.EqualTo(0f),
                "the columns are the same line");

            // the order of the rows of the value signs the determinant of it: swapping the two rows of it swaps
            // the sign of the value
            Assert.That(math.determinant(new float2x2(3f, 1f, 4f, 2f)), Is.EqualTo(2f), "the rows are swapped");

            // the kind of a component of the value is named by the member that reaches the determinant of it
            Assert.That(math.determinant(float2x2.Identity), Is.EqualTo(1f), "the identity of the value");
            Assert.That(math.determinant(new double2x2(1d, 3d, 2d, 4d)), Is.EqualTo(-2d), "double");
            Assert.That(math.determinant(new half2x2((half)1f, (half)3f, (half)2f, (half)4f)),
                Is.EqualTo((half)(-2f)), "half");
        }
    }

    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // the determinant of a matrix of 3 rows and 3 columns is the sum of the products of a component of the
            // first row of it with the determinant of the matrix of 2 rows and 2 columns that is left when the row
            // and the column of that component are taken out of the value, every product signed by the position of
            // the component: [[1, 4, 7], [2, 5, 8], [3, 6, 10]] is 1 * (5 * 10 - 6 * 8) - 4 * (2 * 10 - 3 * 8)
            // + 7 * (2 * 6 - 3 * 5)
            var m = new float3x3(1f, 4f, 7f, 2f, 5f, 8f, 3f, 6f, 10f);
            Assert.That(math.determinant(m), Is.EqualTo(-3f), "math.determinant of a value");
            Assert.That(m.determinant(), Is.EqualTo(-3f), "the member of the value");

            // the columns of the value are within the same plane, so the volume of the parallelepiped they are the
            // sides of is the zero of the kind and the determinant of the value is the zero of it as well
            Assert.That(math.determinant(new float3x3(1f, 4f, 7f, 2f, 5f, 8f, 3f, 6f, 9f)), Is.EqualTo(0f),
                "the columns are within the same plane");

            Assert.That(math.determinant(float3x3.Identity), Is.EqualTo(1f), "the identity of the value");
            Assert.That(math.determinant(new double3x3(1d, 4d, 7d, 2d, 5d, 8d, 3d, 6d, 10d)), Is.EqualTo(-3d), "double");
            Assert.That(math.determinant(new half3x3(
                    (half)1f, (half)4f, (half)7f, (half)2f, (half)5f, (half)8f, (half)3f, (half)6f, (half)10f)),
                Is.EqualTo((half)(-3f)), "half");
        }
    }

    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            // the determinant of a matrix of 4 rows and 4 columns is the sum of the products of the determinants
            // of the matrices of 2 rows and 2 columns the two first columns of the value name with the ones the
            // two last columns of it name, every product signed. The matrix below is the one whose components
            // above the diagonal of it are zero, so the determinant of it is the product of the four components
            // of that diagonal:
            // [[2, 0, 0, 0], [4, 3, 0, 0], [6, 6, 4, 0], [8, 9, 8, 5]] is 2 * 3 * 4 * 5
            var m = new float4x4(
                new float4(2f, 4f, 6f, 8f),
                new float4(0f, 3f, 6f, 9f),
                new float4(0f, 0f, 4f, 8f),
                new float4(0f, 0f, 0f, 5f));
            Assert.That(math.determinant(m), Is.EqualTo(120f), "math.determinant of a value");
            Assert.That(m.determinant(), Is.EqualTo(120f), "the member of the value");

            // the fourth column of the value is the sum of the two first ones of it, so the columns of it are
            // within the same three dimensional space and the four dimensional volume of the parallelepiped they
            // are the sides of is the zero of the kind
            Assert.That(math.determinant(new float4x4(
                new float4(1f, 0f, 0f, 0f),
                new float4(0f, 1f, 0f, 0f),
                new float4(0f, 0f, 1f, 0f),
                new float4(1f, 1f, 0f, 0f))), Is.EqualTo(0f), "the columns are within the same space");

            // the order of two columns of the value signs the volume the columns of it are the sides of
            Assert.That(math.determinant(new float4x4(
                new float4(1f, 0f, 0f, 0f),
                new float4(0f, 0f, 1f, 0f),
                new float4(0f, 1f, 0f, 0f),
                new float4(0f, 0f, 0f, 1f))), Is.EqualTo(-1f), "two columns are swapped");

            Assert.That(math.determinant(float4x4.Identity), Is.EqualTo(1f), "the identity of the value");
            Assert.That(math.determinant(new double4x4(
                new double4(2d, 4d, 6d, 8d),
                new double4(0d, 3d, 6d, 9d),
                new double4(0d, 0d, 4d, 8d),
                new double4(0d, 0d, 0d, 5d))), Is.EqualTo(120d), "double");
            Assert.That(math.determinant(new half4x4(
                new half4((half)2f, (half)4f, (half)6f, (half)8f),
                new half4((half)0f, (half)3f, (half)6f, (half)9f),
                new half4((half)0f, (half)0f, (half)4f, (half)8f),
                new half4((half)0f, (half)0f, (half)0f, (half)5f))), Is.EqualTo((half)120f), "half");
        }
    }
}
