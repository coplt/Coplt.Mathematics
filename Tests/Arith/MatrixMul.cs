using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The product of a matrix: the product of a matrix of <c>n</c> rows and <c>m</c> columns with a matrix of
/// <c>m</c> rows and <c>v</c> columns is the matrix of <c>n</c> rows and <c>v</c> columns whose component at a
/// row and a column is the sum of the products of the components of that row of the first value with the
/// components of that column of the second value. A matrix is laid out by columns, so the product of a matrix
/// with a vector is the product of the value with the vector as a matrix of a single column and the product of a
/// vector with a matrix is the product of the vector as a matrix of a single row with the value. The type of a
/// component of a matrix is not a part of the type of a column of it, so the compiler cannot infer it from the
/// value and the members of a shape and a kind name the types themselves, which the generator emits into the math
/// class and into the class of the value itself. The kinds of the value are the ones of a number, the whole
/// numbers have no fused multiply and add, so the sum of the products of the kind of a component of the value is
/// the fused one for a floating point kind and the one of the products and the additions for a whole number kind.
/// </summary>
public class TestMatrixMul
{
    [Test]
    public void OfMatrix()
    {
        using (Assert.EnterMultipleScope())
        {
            // the columns of [[1, 3], [2, 4]] are (1, 2) and (3, 4) and the ones of [[5, 7], [6, 8]] are (5, 6)
            // and (7, 8): the product of the two values is [[23, 31], [34, 46]], which the dot product of the
            // row of the first value with the column of the second one reaches
            var a = new float2x2(new float2(1f, 2f), new float2(3f, 4f));
            var b = new float2x2(new float2(5f, 6f), new float2(7f, 8f));
            var ab = math.mul(a, b);
            Assert.That(ab, Is.EqualTo(new float2x2(23f, 31f, 34f, 46f)), "math.mul of two values");
            Assert.That(a.mul(b), Is.EqualTo(ab), "the member of the value");

            // the product of the identity of a kind with a value is the value itself
            Assert.That(math.mul(a, float2x2.Identity), Is.EqualTo(a), "the identity is on the right");
            Assert.That(math.mul(float2x2.Identity, a), Is.EqualTo(a), "the identity is on the left");

            // the product of a matrix of 2 rows and 3 columns with a matrix of 3 rows and 2 columns is a matrix
            // of 2 rows and 2 columns
            var wide = new float2x3(new float2(1f, 2f), new float2(3f, 4f), new float2(5f, 6f));
            var tall = new float3x2(new float3(7f, 8f, 9f), new float3(10f, 11f, 12f));
            Assert.That(math.mul(wide, tall), Is.EqualTo(new float2x2(76f, 103f, 100f, 136f)),
                "a wide value with a tall one");

            // the product of a matrix of 3 rows and 2 columns with a matrix of 2 rows and 3 columns is a matrix
            // of 3 rows and 3 columns
            Assert.That(math.mul(tall, wide), Is.EqualTo(new float3x3(
                    new float3(27f, 30f, 33f),
                    new float3(61f, 68f, 75f),
                    new float3(95f, 106f, 117f))),
                "a tall value with a wide one");

            // the product of a matrix by the columns of it of a matrix of 4 rows and 4 columns
            var m = new float4x4(
                new float4(1f, 2f, 3f, 4f),
                new float4(5f, 6f, 7f, 8f),
                new float4(9f, 10f, 11f, 12f),
                new float4(13f, 14f, 15f, 16f));
            Assert.That(math.mul(m, float4x4.Identity), Is.EqualTo(m), "a value of 4 rows and 4 columns");
        }
    }

    [Test]
    public void OfVector()
    {
        using (Assert.EnterMultipleScope())
        {
            // the product of a matrix of 2 rows and 3 columns with a vector of 3 components is the sum of the
            // columns of the value scaled by the components of the vector: (1, 2) * 1 + (3, 4) * 2 + (5, 6) * 3
            var m = new float2x3(new float2(1f, 2f), new float2(3f, 4f), new float2(5f, 6f));
            var v = new float3(1f, 2f, 3f);
            var mv = math.mul(m, v);
            Assert.That(mv, Is.EqualTo(new float2(22f, 28f)), "math.mul of a matrix and a vector");
            Assert.That(m.mul(v), Is.EqualTo(mv), "the member of the value");

            // the product of a vector of 2 components with a matrix of 2 rows and 3 columns is the vector of the
            // dot products of the value with the columns of the matrix
            var u = new float2(1f, 2f);
            var um = math.mul(u, m);
            Assert.That(um, Is.EqualTo(new float3(5f, 11f, 17f)), "math.mul of a vector and a matrix");
            Assert.That(u.mul(m), Is.EqualTo(um), "the member of the value");

            // the operator of the value scales the columns of the matrix by the components of the vector
            Assert.That(m * v, Is.EqualTo(new float2x3(
                    new float2(1f, 2f),
                    new float2(6f, 8f),
                    new float2(15f, 18f))),
                "the columns of the value are scaled");

            // the operator of the value scales the rows of the matrix by the components of the vector, which is
            // the vector of the rows of the value
            Assert.That(u * m, Is.EqualTo(new float2x3(
                    new float2(1f, 4f),
                    new float2(3f, 8f),
                    new float2(5f, 12f))),
                "the rows of the value are scaled");
        }
    }

    [Test]
    public void OfKinds()
    {
        using (Assert.EnterMultipleScope())
        {
            // the kind of a component of the value is named by the member that reaches the product of it: the
            // product of the value of a floating point kind uses the fused multiply and add of the kind
            Assert.That(math.mul(
                    new double2x3(new double2(1d, 2d), new double2(3d, 4d), new double2(5d, 6d)),
                    new double3x2(new double3(7d, 8d, 9d), new double3(10d, 11d, 12d))),
                Is.EqualTo(new double2x2(76d, 103d, 100d, 136d)), "double");

            Assert.That(math.mul(
                    new half2x3(
                        new half2((half)1f, (half)2f), new half2((half)3f, (half)4f), new half2((half)5f, (half)6f)),
                    new half3x2(
                        new half3((half)7f, (half)8f, (half)9f), new half3((half)10f, (half)11f, (half)12f))),
                Is.EqualTo(new half2x2((half)76f, (half)103f, (half)100f, (half)136f)), "half");

            // the whole numbers have no fused multiply and add, so the sum of the products of the kind of a
            // whole number is the one of the products and the additions
            Assert.That(math.mul(new int2x2(1, 2, 3, 4), new int2x2(5, 6, 7, 8)),
                Is.EqualTo(new int2x2(19, 22, 43, 50)), "int");

            Assert.That(math.mul(new uint2x2(1u, 2u, 3u, 4u), new uint2x2(5u, 6u, 7u, 8u)),
                Is.EqualTo(new uint2x2(19u, 22u, 43u, 50u)), "uint");
        }
    }
}
