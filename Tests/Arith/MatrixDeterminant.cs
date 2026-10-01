using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The determinant of a square matrix: the type of a single component of a matrix is not a part of the type of it
/// and the type of a column of it is not one either, so the compiler cannot infer them from the value. A call of
/// the member that does not name them reaches the member of the matrix type of the value, which the ex_ classes
/// add to the math class and which the math_ex_ classes add to the value itself. The determinant of a matrix of 2
/// rows and 2 columns is the area of the parallelogram the columns of it are the sides of and the one of a matrix
/// of 3 rows and 3 columns is the volume of the parallelepiped the columns of it are the sides of, signed by the
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
}
