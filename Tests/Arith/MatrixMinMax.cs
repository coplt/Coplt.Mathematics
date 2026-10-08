using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The minimum of the columns and of the rows of a matrix: the type of a column of a matrix and the type of a row
/// of it are not a part of the type of the value, so the compiler cannot infer them from the value. A call of the
/// member that does not name the type of the vector it reduces reaches the member of the vector type of the
/// value, which the ex_ classes add to the math class and which the math_ex_ classes add to the value itself, and
/// the minimum of the columns of a value is the minimum of every row of it while the minimum of the rows of it is
/// the minimum of every column of it. The maximum of the 2 of them is the member of the other name. The minimum
/// of the platform, whose name carries the mark of it, holds the same value for a value of the numbers that are
/// not a nan and not a negative zero, which every platform is free to handle in a way of its own.
/// </summary>
public class TestMatrixMinMax
{
    [Test]
    public void Columns()
    {
        using (Assert.EnterMultipleScope())
        {
            // every column of the value is held against the one before it component by component, so the
            // component of the result at the index of a row of the value is the minimum of the components of that
            // row of it
            Assert.That(math.cmin<float2x3, float2>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float2(1f, 4f)), "2x3");
            Assert.That(math.cmax<float2x3, float2>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float2(3f, 6f)), "2x3");

            Assert.That(math.cmin<float3x3, float3>(new float3x3(new float3(1f, 9f, 2f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 3f))),
                Is.EqualTo(new float3(1f, 5f, 2f)), "3x3");
            Assert.That(math.cmax<float3x3, float3>(new float3x3(new float3(1f, 9f, 2f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 3f))),
                Is.EqualTo(new float3(7f, 9f, 6f)), "3x3");

            Assert.That(math.cmin<int2x2, int2>(new int2x2(new int2(4, 1), new int2(3, 6))),
                Is.EqualTo(new int2(3, 1)), "int2");
            Assert.That(math.cmax<long2x4, long2>(new long2x4(new long2(1L, 8L), new long2(3L, 4L),
                    new long2(5L, 2L), new long2(7L, 6L))),
                Is.EqualTo(new long2(7L, 8L)), "long2");
            Assert.That(math.cmin<half3x2, half3>(new half3x2(new half3((half)9f, (half)2f, (half)3f),
                    new half3((half)4f, (half)1f, (half)6f))),
                Is.EqualTo(new half3((half)4f, (half)1f, (half)3f)), "half3, a value without a register");

            // the minimum of the platform holds the same value for the numbers that are not a nan
            Assert.That(math.cmin_native<float2x3, float2>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float2(1f, 4f)), "the minimum of the platform");
            Assert.That(math.cmax_native<float2x3, float2>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float2(3f, 6f)), "the maximum of the platform");
            Assert.That(math.cmin_native<int2x2, int2>(new int2x2(new int2(4, 1), new int2(3, 6))),
                Is.EqualTo(new int2(3, 1)), "int2");
        }
    }

    [Test]
    public void Rows()
    {
        using (Assert.EnterMultipleScope())
        {
            // every column of the value is reduced to the minimum of its components, so the component of the
            // result at the index of a column of the value is the minimum of the components of that column of it
            Assert.That(math.rmin<float2x3, float3>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float3(1f, 2f, 3f)), "2x3");
            Assert.That(math.rmax<float2x3, float3>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float3(4f, 5f, 6f)), "2x3");

            Assert.That(math.rmin<float3x3, float3>(new float3x3(new float3(1f, 9f, 2f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 3f))),
                Is.EqualTo(new float3(1f, 4f, 3f)), "3x3");
            Assert.That(math.rmax<float3x3, float3>(new float3x3(new float3(1f, 9f, 2f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 3f))),
                Is.EqualTo(new float3(9f, 6f, 8f)), "3x3");

            Assert.That(math.rmin<int2x2, int2>(new int2x2(new int2(4, 1), new int2(3, 6))),
                Is.EqualTo(new int2(1, 3)), "int2");
            Assert.That(math.rmax<long2x4, long4>(new long2x4(new long2(1L, 8L), new long2(3L, 4L),
                    new long2(5L, 2L), new long2(7L, 6L))),
                Is.EqualTo(new long4(8L, 4L, 5L, 7L)), "long4, a 256 bit register");
            Assert.That(math.rmax<half3x2, half2>(new half3x2(new half3((half)9f, (half)2f, (half)3f),
                    new half3((half)4f, (half)1f, (half)6f))),
                Is.EqualTo(new half2((half)9f, (half)6f)), "half2, a value without a register");

            // the minimum of the platform holds the same value for the numbers that are not a nan
            Assert.That(math.rmin_native<float2x3, float3>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float3(1f, 2f, 3f)), "the minimum of the platform");
            Assert.That(math.rmax_native<float2x3, float3>(new float2x3(new float2(1f, 4f), new float2(2f, 5f),
                    new float2(3f, 6f))),
                Is.EqualTo(new float3(4f, 5f, 6f)), "the maximum of the platform");
            Assert.That(math.rmax_native<long2x4, long4>(new long2x4(new long2(1L, 8L), new long2(3L, 4L),
                    new long2(5L, 2L), new long2(7L, 6L))),
                Is.EqualTo(new long4(8L, 4L, 5L, 7L)), "long4, a 256 bit register");
        }
    }

    /// <summary>
    /// The type of a column of a matrix is not a part of the type of a column of it either, so a call that does
    /// not name it reaches the member of the vector type of the value: the minimum of the columns of the value
    /// reaches the member of the type of a column of it and the minimum of the rows of it reaches the member of
    /// the type of a row of it.
    /// </summary>
    [Test]
    public void ReachedByTheVectorTypeOfTheValue()
    {
        using (Assert.EnterMultipleScope())
        {
            // the columns of a matrix of 2 rows are vectors of 2 components, which the member of the math class
            // and the member of the value itself name as well
            var a = new float2x3(new float2(1f, 4f), new float2(2f, 5f), new float2(3f, 6f));
            Assert.That(math.cmin(a), Is.EqualTo(new float2(1f, 4f)), "math.cmin of a value");
            Assert.That(a.cmin(), Is.EqualTo(new float2(1f, 4f)), "the member of the value");
            Assert.That(math.cmax(a), Is.EqualTo(new float2(3f, 6f)), "math.cmax of a value");

            // the rows of a matrix of 3 columns are vectors of 3 components
            Assert.That(math.rmin(a), Is.EqualTo(new float3(1f, 2f, 3f)), "math.rmin of a value");
            Assert.That(a.rmax(), Is.EqualTo(new float3(4f, 5f, 6f)), "the member of the value");

            // the vectors of every other kind and count reach the member of their own type
            Assert.That(math.cmin(new int2x2(new int2(4, 1), new int2(3, 6))), Is.EqualTo(new int2(3, 1)),
                "int2");
            Assert.That(math.rmax(new half3x2(new half3((half)9f, (half)2f, (half)3f),
                    new half3((half)4f, (half)1f, (half)6f))),
                Is.EqualTo(new half2((half)9f, (half)6f)), "half2, a value without a register");

            // the member of the platform of the value reaches the member of the vector type of it as well
            Assert.That(math.rmin_native(a), Is.EqualTo(new float3(1f, 2f, 3f)),
                "math.rmin_native of a value");
            Assert.That(a.cmax_native(), Is.EqualTo(new float2(3f, 6f)), "the member of the value");
        }
    }
}
