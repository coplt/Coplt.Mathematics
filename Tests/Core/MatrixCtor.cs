using Coplt.Mathematics;

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
}
