using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The transpose of a matrix: the rows of the transpose of a value are the columns of it and the columns of the
/// transpose are the rows of it, so the type of the result is the type of the value with the two counts of it
/// swapped and the compiler cannot infer it from the value. A call of the member reaches the member of the math
/// class that names the type of the result and the member of the value that names it, and the value of a shape
/// that keeps its columns in a register reaches the member of the simd library for the shape of it where the shape
/// of it is one the member of the library covers.
/// </summary>
public class TestMatrixTranspose
{
    [Test]
    public void OfSquare()
    {
        using (Assert.EnterMultipleScope())
        {
            // the component of the result at the row r and the column c is the component of the value at the row c
            // and the column r
            var m2 = new float2x2(1f, 2f, 3f, 4f);
            var t2 = math.transpose(m2);
            Assert.That((t2.m00, t2.m01, t2.m10, t2.m11), Is.EqualTo((1f, 3f, 2f, 4f)), "a matrix of 2 rows");
            Assert.That(m2.transpose(), Is.EqualTo(t2), "the member of the value");

            var m3 = new float3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f);
            var t3 = math.transpose(m3);
            Assert.That((t3.m00, t3.m01, t3.m02, t3.m10, t3.m11, t3.m12, t3.m20, t3.m21, t3.m22),
                Is.EqualTo((1f, 4f, 7f, 2f, 5f, 8f, 3f, 6f, 9f)), "a matrix of 3 rows");
            Assert.That(m3.transpose(), Is.EqualTo(t3), "the member of the value");

            var m4 = new float4x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f);
            var t4 = math.transpose(m4);
            Assert.That(
                (t4.m00, t4.m01, t4.m02, t4.m03, t4.m10, t4.m11, t4.m12, t4.m13,
                    t4.m20, t4.m21, t4.m22, t4.m23, t4.m30, t4.m31, t4.m32, t4.m33),
                Is.EqualTo((1f, 5f, 9f, 13f, 2f, 6f, 10f, 14f, 3f, 7f, 11f, 15f, 4f, 8f, 12f, 16f)),
                "a matrix of 4 rows");
            Assert.That(m4.transpose(), Is.EqualTo(t4), "the member of the value");

            // the transpose of a matrix of 2 rows and 2 columns is the swap of the two components that are not on
            // the diagonal of it, and the member of the simd library answers it out of the two registers of the
            // value rather than with four reads of a component
            Assert.That(math.transpose(float2x2.Identity), Is.EqualTo(float2x2.Identity), "the identity of the value");
        }
    }

    [Test]
    public void OfWide()
    {
        using (Assert.EnterMultipleScope())
        {
            var m2x3 = new float2x3(1f, 2f, 3f, 4f, 5f, 6f);
            var t2x3 = math.transpose(m2x3);
            Assert.That((t2x3.m00, t2x3.m01, t2x3.m10, t2x3.m11, t2x3.m20, t2x3.m21),
                Is.EqualTo((1f, 4f, 2f, 5f, 3f, 6f)), "2 rows and 3 columns");
            Assert.That(m2x3.transpose(), Is.EqualTo(t2x3), "the member of the value");

            var m2x4 = new float2x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f);
            var t2x4 = math.transpose(m2x4);
            Assert.That((t2x4.m00, t2x4.m01, t2x4.m10, t2x4.m11, t2x4.m20, t2x4.m21, t2x4.m30, t2x4.m31),
                Is.EqualTo((1f, 5f, 2f, 6f, 3f, 7f, 4f, 8f)), "2 rows and 4 columns");
            Assert.That(m2x4.transpose(), Is.EqualTo(t2x4), "the member of the value");

            var m3x4 = new float3x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f);
            var t3x4 = math.transpose(m3x4);
            Assert.That(
                (t3x4.m00, t3x4.m01, t3x4.m02, t3x4.m10, t3x4.m11, t3x4.m12, t3x4.m20, t3x4.m21, t3x4.m22,
                    t3x4.m30, t3x4.m31, t3x4.m32),
                Is.EqualTo((1f, 5f, 9f, 2f, 6f, 10f, 3f, 7f, 11f, 4f, 8f, 12f)), "3 rows and 4 columns");
            Assert.That(m3x4.transpose(), Is.EqualTo(t3x4), "the member of the value");
        }
    }

    [Test]
    public void OfTall()
    {
        using (Assert.EnterMultipleScope())
        {
            var m3x2 = new float3x2(1f, 2f, 3f, 4f, 5f, 6f);
            var t3x2 = math.transpose(m3x2);
            Assert.That((t3x2.m00, t3x2.m01, t3x2.m02, t3x2.m10, t3x2.m11, t3x2.m12),
                Is.EqualTo((1f, 3f, 5f, 2f, 4f, 6f)), "3 rows and 2 columns");
            Assert.That(m3x2.transpose(), Is.EqualTo(t3x2), "the member of the value");

            var m4x2 = new float4x2(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f);
            var t4x2 = math.transpose(m4x2);
            Assert.That((t4x2.m00, t4x2.m01, t4x2.m02, t4x2.m03, t4x2.m10, t4x2.m11, t4x2.m12, t4x2.m13),
                Is.EqualTo((1f, 3f, 5f, 7f, 2f, 4f, 6f, 8f)), "4 rows and 2 columns");
            Assert.That(m4x2.transpose(), Is.EqualTo(t4x2), "the member of the value");

            var m4x3 = new float4x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f);
            var t4x3 = math.transpose(m4x3);
            Assert.That(
                (t4x3.m00, t4x3.m01, t4x3.m02, t4x3.m03, t4x3.m10, t4x3.m11, t4x3.m12, t4x3.m13,
                    t4x3.m20, t4x3.m21, t4x3.m22, t4x3.m23),
                Is.EqualTo((1f, 4f, 7f, 10f, 2f, 5f, 8f, 11f, 3f, 6f, 9f, 12f)), "4 rows and 3 columns");
            Assert.That(m4x3.transpose(), Is.EqualTo(t4x3), "the member of the value");
        }
    }

    [Test]
    public void OfKinds()
    {
        using (Assert.EnterMultipleScope())
        {
            // a matrix of 8 byte components keeps a column of 3 components in a 256 bit register
            var d3 = math.transpose(new double3x3(1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d));
            Assert.That((d3.m00, d3.m01, d3.m02, d3.m10, d3.m11, d3.m12, d3.m20, d3.m21, d3.m22),
                Is.EqualTo((1d, 4d, 7d, 2d, 5d, 8d, 3d, 6d, 9d)), "double");

            // a matrix of a kind without a register reaches the scalar path
            var h2 = math.transpose(new half2x3((half)1f, (half)2f, (half)3f, (half)4f, (half)5f, (half)6f));
            Assert.That((h2.m00, h2.m01, h2.m10, h2.m11, h2.m20, h2.m21),
                Is.EqualTo(((half)1f, (half)4f, (half)2f, (half)5f, (half)3f, (half)6f)), "half");

            var s3 = math.transpose(new short3x3(1, 2, 3, 4, 5, 6, 7, 8, 9));
            Assert.That((s3.m00, s3.m01, s3.m02, s3.m10, s3.m11, s3.m12, s3.m20, s3.m21, s3.m22),
                Is.EqualTo((1, 4, 7, 2, 5, 8, 3, 6, 9)), "short");

            // a matrix of a 4 byte kind keeps a column of 2 components in a 128 bit register whose two upper lanes
            // are padding, which the member of the simd library takes as it is
            var i4x2 = math.transpose(new int4x2(1, 2, 3, 4, 5, 6, 7, 8));
            Assert.That((i4x2.m00, i4x2.m01, i4x2.m02, i4x2.m03, i4x2.m10, i4x2.m11, i4x2.m12, i4x2.m13),
                Is.EqualTo((1, 3, 5, 7, 2, 4, 6, 8)), "int");

            var u2x4 = math.transpose(new uint2x4(1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u));
            Assert.That((u2x4.m00, u2x4.m01, u2x4.m10, u2x4.m11, u2x4.m20, u2x4.m21, u2x4.m30, u2x4.m31),
                Is.EqualTo((1u, 5u, 2u, 6u, 3u, 7u, 4u, 8u)), "uint");

            // the member of the simd library of a shape of 2 rows and 2 columns takes a column in a register of
            // the exact 128 bits of the 2 components of it
            var l2 = math.transpose(new long2x2(1L, 2L, 3L, 4L));
            Assert.That((l2.m00, l2.m01, l2.m10, l2.m11), Is.EqualTo((1L, 3L, 2L, 4L)),
                "long of 2 rows and 2 columns");

            // a matrix of 8 byte components keeps a column of 2 components in a 128 bit register exactly
            var l3x4 = math.transpose(new long3x4(
                1L, 2L, 3L, 4L, 5L, 6L, 7L, 8L, 9L, 10L, 11L, 12L));
            Assert.That(
                (l3x4.m00, l3x4.m01, l3x4.m02, l3x4.m10, l3x4.m11, l3x4.m12, l3x4.m20, l3x4.m21, l3x4.m22,
                    l3x4.m30, l3x4.m31, l3x4.m32),
                Is.EqualTo((1L, 5L, 9L, 2L, 6L, 10L, 3L, 7L, 11L, 4L, 8L, 12L)), "long");
        }
    }
}
