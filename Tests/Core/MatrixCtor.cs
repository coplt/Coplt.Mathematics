using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Core;

/// <summary>
/// A matrix is created from the columns of it and from the components of it. The constructor that takes the
/// components takes them in the order of a row, so the row of a component leads the name of the argument and
/// the column of it decides the column of the matrix that receives it.
/// </summary>
public class TestMatrixCtor
{
    [Test]
    public void Components()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component of the matrix is an argument of the constructor, a row of it is an argument of
            // every column
            Assert.That(new float2x2(1f, 2f, 3f, 4f),
                Is.EqualTo(new float2x2(new float2(1f, 3f), new float2(2f, 4f))));
            Assert.That(new float3x2(1f, 2f, 3f, 4f, 5f, 6f),
                Is.EqualTo(new float3x2(new float3(1f, 3f, 5f), new float3(2f, 4f, 6f))));
            Assert.That(new double2x3(1d, 2d, 3d, 4d, 5d, 6d),
                Is.EqualTo(new double2x3(new double2(1d, 4d), new double2(2d, 5d), new double2(3d, 6d))));
            Assert.That(new int4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
                Is.EqualTo(new int4x4(
                    new int4(1, 5, 9, 13), new int4(2, 6, 10, 14), new int4(3, 7, 11, 15),
                    new int4(4, 8, 12, 16))));
            // a matrix without a register in its columns is created the same way
            Assert.That(new float3x3s(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f),
                Is.EqualTo(new float3x3s(
                    new float3s(1f, 4f, 7f), new float3s(2f, 5f, 8f), new float3s(3f, 6f, 9f))));
        }
    }

    /// <summary>
    /// A matrix of 3 rows and 3 columns is the value of the upper left of a matrix of 4 rows and 4 columns of it,
    /// so the value of one of the two reaches the value of the other one: the member that takes the value of a
    /// matrix of 4 rows and 4 columns reads the 3 rows and the 3 columns of the upper left of it, and the member
    /// that hands the value of a matrix of 4 rows and 4 columns over as the one of the upper left of it is the
    /// conversion of it.
    /// </summary>
    [Test]
    public void UpperLeft()
    {
        using (Assert.EnterMultipleScope())
        {
            // the last row and the last column of the value of 4 rows and 4 columns are not part of the value of
            // the upper left of it
            var m4x4 = new float4x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f);
            var m3x3 = new float3x3(m4x4);
            Assert.That((m3x3.m00, m3x3.m01, m3x3.m02, m3x3.m10, m3x3.m11, m3x3.m12, m3x3.m20, m3x3.m21,
                m3x3.m22), Is.EqualTo((1f, 2f, 3f, 5f, 6f, 7f, 9f, 10f, 11f)), "the value of the upper left");
            Assert.That(m3x3, Is.EqualTo(new float3x3(m4x4.c0.xyz, m4x4.c1.xyz, m4x4.c2.xyz)),
                "the columns of the value of the upper left");

            // the conversion of the value of a matrix of 4 rows and 4 columns is the same member
            Assert.That((float3x3)m4x4, Is.EqualTo(m3x3), "the conversion of the value of 4 rows and 4 columns");

            // the value of the upper left is the member of every kind a number names
            var d = new double3x3(new double4x4(1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d, 10d, 11d, 12d, 13d, 14d, 15d,
                16d));
            Assert.That((d.m12, d.m22), Is.EqualTo((7d, 11d)), "double");
            var i = (int3x3)new int4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.That((i.m02, i.m22), Is.EqualTo((3, 11)), "int");
            var h = new half3x3(new half4x4((half)1f, (half)2f, (half)3f, (half)4f, (half)5f, (half)6f, (half)7f,
                (half)8f, (half)9f, (half)10f, (half)11f, (half)12f, (half)13f, (half)14f, (half)15f, (half)16f));
            Assert.That((h.m00, h.m22), Is.EqualTo(((half)1f, (half)11f)), "half");
        }
    }

    /// <summary>
    /// A matrix of 4 rows and 4 columns is also created from the rotation of a matrix of 3 rows and 3 columns and the
    /// translation of a value of 3 components: the value of the upper left of the matrix is the rotation, the value
    /// of the fourth column of it is the translation and the last row of it is the zero of the kind beside the one
    /// of it.
    /// </summary>
    [Test]
    public void RotationAndTranslation()
    {
        using (Assert.EnterMultipleScope())
        {
            var rotation = float3x3.EulerZXY(new float3(0.3f, -0.7f, 1.1f));
            var translation = new float3(1f, 2f, 3f);
            var m = new float4x4(rotation, translation);

            // the value of the upper left of the matrix is the rotation the member is handed
            Assert.That((m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22),
                Is.EqualTo((rotation.m00, rotation.m01, rotation.m02, rotation.m10, rotation.m11, rotation.m12,
                    rotation.m20, rotation.m21, rotation.m22)), "the value of the upper left");

            // the value of the fourth column of the matrix is the translation and the last row of it is the zero of
            // the kind beside the one of it
            Assert.That((m.c3.x, m.c3.y, m.c3.z, m.c3.w), Is.EqualTo((1f, 2f, 3f, 1f)), "the fourth column");
            Assert.That((m.m30, m.m31, m.m32, m.m33), Is.EqualTo((0f, 0f, 0f, 1f)), "the last row");

            // the value of the upper left is the member of every kind a number names
            var d = new double4x4(double3x3.Identity, new double3(1d, 2d, 3d));
            Assert.That((d.m00, d.m11, d.m22, d.c3.x, d.m33), Is.EqualTo((1d, 1d, 1d, 1d, 1d)), "double");
            var i = new int4x4(int3x3.Identity, new int3(1, 2, 3));
            Assert.That((i.m00, i.m11, i.m22, i.c3.z, i.m33), Is.EqualTo((1, 1, 1, 3, 1)), "int");
            var h = new half4x4(half3x3.Identity, new half3((half)1f, (half)2f, (half)3f));
            Assert.That((h.m00, h.c3.z, h.m33), Is.EqualTo(((half)1f, (half)3f, (half)1f)), "half");
        }
    }
}
