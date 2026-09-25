using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The sum of the columns and the sum of the rows of a matrix: the type of a column of a matrix and the type of a
/// row of it are not a part of the type of the value, so the compiler cannot infer them from the value. A call of
/// the member that does not name the type of the vector it reduces reaches the member of the vector type of the
/// value, which the ex_ classes add to the math class and which the math_ex_ classes add to the value itself, and
/// the sum of the columns of a value is the sum of every row of it while the sum of the rows of it is the sum of
/// every column of it.
/// </summary>
public class TestMatrixSum
{
    [Test]
    public void Columns()
    {
        using (Assert.EnterMultipleScope())
        {
            // every column of the value adds up component by component, so the component of the result at the
            // index of a row of the value is the sum of the components of that row
            Assert.That(math.csum<float3x2, float3>(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f))),
                Is.EqualTo(new float3(5f, 7f, 9f)), "3x2");
            Assert.That(math.csum<float3x3, float3>(new float3x3(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(new float3(12f, 15f, 18f)), "3x3");
            Assert.That(math.csum<float3x4, float3>(new float3x4(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float3(22f, 26f, 30f)), "3x4");

            Assert.That(math.csum<int2x2, int2>(new int2x2(new int2(1, 2), new int2(3, 4))),
                Is.EqualTo(new int2(4, 6)), "int2, a 128 bit register with padding lanes");
            Assert.That(math.csum<long2x4, long2>(new long2x4(new long2(1L, 2L), new long2(3L, 4L),
                    new long2(5L, 6L), new long2(7L, 8L))),
                Is.EqualTo(new long2(16L, 20L)), "long2, a 128 bit register");
            Assert.That(math.csum<half3x2, half3>(new half3x2(new half3((half)1f, (half)2f, (half)3f),
                    new half3((half)4f, (half)5f, (half)6f))),
                Is.EqualTo(new half3((half)5f, (half)7f, (half)9f)), "half3, a value without a register");

            // the storage variants of a matrix keep the columns in a narrower storage
            Assert.That(math.csum<double3x3s, double3s>(new double3x3s(new double3s(1d, 2d, 3d),
                    new double3s(4d, 5d, 6d), new double3s(7d, 8d, 9d))),
                Is.EqualTo(new double3s(12d, 15d, 18d)), "double3s, a value without a register");
            Assert.That(math.csum<float3x4s, float3s>(new float3x4s(new float3s(1f, 2f, 3f), new float3s(4f, 5f, 6f),
                    new float3s(7f, 8f, 9f), new float3s(10f, 11f, 12f))),
                Is.EqualTo(new float3s(22f, 26f, 30f)), "float3s, a value without a register");
        }
    }

    [Test]
    public void Rows()
    {
        using (Assert.EnterMultipleScope())
        {
            // every row of the value adds up component by component, so the component of the result at the index
            // of a column of the value is the sum of the components of that column
            Assert.That(math.rsum<float3x2, float2>(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f))),
                Is.EqualTo(new float2(6f, 15f)), "3x2");
            Assert.That(math.rsum<float3x3, float3>(new float3x3(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(new float3(6f, 15f, 24f)), "3x3");
            Assert.That(math.rsum<float3x4, float4>(new float3x4(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float4(6f, 15f, 24f, 33f)), "3x4");

            Assert.That(math.rsum<int2x2, int2>(new int2x2(new int2(1, 2), new int2(3, 4))),
                Is.EqualTo(new int2(3, 7)), "int2");
            Assert.That(math.rsum<long2x4, long4>(new long2x4(new long2(1L, 2L), new long2(3L, 4L),
                    new long2(5L, 6L), new long2(7L, 8L))),
                Is.EqualTo(new long4(3L, 7L, 11L, 15L)), "long4, a 256 bit register");
            Assert.That(math.rsum<half3x2, half2>(new half3x2(new half3((half)1f, (half)2f, (half)3f),
                    new half3((half)4f, (half)5f, (half)6f))),
                Is.EqualTo(new half2((half)6f, (half)15f)), "half2, a value without a register");

            // a row of a matrix keeps the storage variant of its own kind, which the vectors of a count of 4 have
            // none of
            Assert.That(math.rsum<double3x3s, double3s>(new double3x3s(new double3s(1d, 2d, 3d),
                    new double3s(4d, 5d, 6d), new double3s(7d, 8d, 9d))),
                Is.EqualTo(new double3s(6d, 15d, 24d)), "double3s, a value without a register");
            Assert.That(math.rsum<float3x4s, float4>(new float3x4s(new float3s(1f, 2f, 3f), new float3s(4f, 5f, 6f),
                    new float3s(7f, 8f, 9f), new float3s(10f, 11f, 12f))),
                Is.EqualTo(new float4(6f, 15f, 24f, 33f)), "float4, the vectors of 4 components have no storage variant");
        }
    }

    /// <summary>
    /// The type of a column of a matrix is not a part of the type of a column of it either, so a call that does
    /// not name it reaches the member of the vector type of the value: the sum of the columns of the value
    /// reaches the member of the type of a column of it and the sum of the rows of it reaches the member of the
    /// type of a row of it.
    /// </summary>
    [Test]
    public void ReachedByTheVectorTypeOfTheValue()
    {
        using (Assert.EnterMultipleScope())
        {
            // the columns of a matrix of 3 rows are vectors of 3 components, which the member of the math class
            // and the member of the value itself name as well
            var a = new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f));
            Assert.That(math.csum(a), Is.EqualTo(new float3(5f, 7f, 9f)), "math.csum of a value");
            Assert.That(a.csum(), Is.EqualTo(new float3(5f, 7f, 9f)), "the member of the value");

            // the rows of a matrix of 2 columns are vectors of 2 components
            Assert.That(math.rsum(a), Is.EqualTo(new float2(6f, 15f)), "math.rsum of a value");
            Assert.That(a.rsum(), Is.EqualTo(new float2(6f, 15f)), "the member of the value");

            // the vectors of every other kind and count reach the member of their own type
            Assert.That(math.csum(new float3x4(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float3(22f, 26f, 30f)), "4 columns");
            Assert.That(math.rsum(new float3x4(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float4(6f, 15f, 24f, 33f)), "4 components of a row");
            Assert.That(math.csum(new int2x2(new int2(1, 2), new int2(3, 4))), Is.EqualTo(new int2(4, 6)),
                "int2");
            Assert.That(math.rsum(new half3x2(new half3((half)1f, (half)2f, (half)3f),
                    new half3((half)4f, (half)5f, (half)6f))),
                Is.EqualTo(new half2((half)6f, (half)15f)), "half2, a value without a register");

            // a storage variant reaches the member of the vector type of its own storage
            var b = new float3x2s(new float3s(1f, 2f, 3f), new float3s(4f, 5f, 6f));
            Assert.That(math.csum(b), Is.EqualTo(new float3s(5f, 7f, 9f)), "float3s");
            Assert.That(b.rsum(), Is.EqualTo(new float2s(6f, 15f)), "float2s");
        }
    }
}
