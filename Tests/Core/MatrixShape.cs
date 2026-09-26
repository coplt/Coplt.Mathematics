using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;

namespace Tests.Core;

/// <summary>
/// The columns of a matrix are the value of it, so a caller that does not know the number of the rows of a
/// matrix reaches them through the interface of the number of its columns. The shape of a matrix implements the
/// interface of the number of its columns beside the interface of its own shape, so the interface of a count
/// reaches every shape that has that count.
/// </summary>
public class TestMatrixShape
{
    /// <summary>Reads and writes the columns of a matrix of 2 columns</summary>
    private static void Check2<T, TVector>(T m, TVector a, TVector b)
        where T : unmanaged, IMatrixMx2Vector<T, TVector>
        where TVector : unmanaged, IVector<TVector>
    {
        Assert.That(T.get_c0(m).Equals(a), Is.True);
        Assert.That(T.get_c1(m).Equals(b), Is.True);
        Assert.That(T.Create(a, b).Equals(m), Is.True);
        var r = m;
        T.set_c0(ref r, b);
        T.set_c1(ref r, a);
        Assert.That(T.get_c0(r).Equals(b), Is.True);
        Assert.That(T.get_c1(r).Equals(a), Is.True);
    }

    /// <summary>Reads the columns of a matrix of 3 columns</summary>
    private static void Check3<T, TVector>(T m, TVector a, TVector b, TVector c)
        where T : unmanaged, IMatrixMx3Vector<T, TVector>
        where TVector : unmanaged, IVector<TVector>
    {
        Assert.That(T.get_c0(m).Equals(a), Is.True);
        Assert.That(T.get_c1(m).Equals(b), Is.True);
        Assert.That(T.get_c2(m).Equals(c), Is.True);
        Assert.That(T.Create(a, b, c).Equals(m), Is.True);
    }

    /// <summary>Reads the columns of a matrix of 4 columns</summary>
    private static void Check4<T, TVector>(T m, TVector a, TVector b, TVector c, TVector d)
        where T : unmanaged, IMatrixMx4Vector<T, TVector>
        where TVector : unmanaged, IVector<TVector>
    {
        Assert.That(T.get_c0(m).Equals(a), Is.True);
        Assert.That(T.get_c1(m).Equals(b), Is.True);
        Assert.That(T.get_c2(m).Equals(c), Is.True);
        Assert.That(T.get_c3(m).Equals(d), Is.True);
        Assert.That(T.Create(a, b, c, d).Equals(m), Is.True);
    }

    [Test]
    public void Columns()
    {
        using (Assert.EnterMultipleScope())
        {
            // a matrix of a number reaches the interface of the number of its columns, the vector of a column is
            // the vector of the number of the rows of the matrix
            Check2(new float2x2(new float2(1, 2), new float2(3, 4)), new float2(1, 2), new float2(3, 4));
            Check2(new float3x2(new float3(1, 2, 3), new float3(4, 5, 6)),
                new float3(1, 2, 3), new float3(4, 5, 6));
            Check2(new float4x2(new float4(1, 2, 3, 4), new float4(5, 6, 7, 8)),
                new float4(1, 2, 3, 4), new float4(5, 6, 7, 8));
            Check2(new double2x2(new double2(1, 2), new double2(3, 4)), new double2(1, 2), new double2(3, 4));
            Check2(new int3x2(new int3(1, 2, 3), new int3(4, 5, 6)), new int3(1, 2, 3), new int3(4, 5, 6));
            // a matrix without a register in its columns reaches them the same way
            Check2(new float3x2s(new float3s(1, 2, 3), new float3s(4, 5, 6)),
                new float3s(1, 2, 3), new float3s(4, 5, 6));
            Check2(new double3x2s(new double3s(1, 2, 3), new double3s(4, 5, 6)),
                new double3s(1, 2, 3), new double3s(4, 5, 6));
            Check3(new float2x3(new float2(1, 2), new float2(3, 4), new float2(5, 6)),
                new float2(1, 2), new float2(3, 4), new float2(5, 6));
            Check3(new double4x3(new double4(1), new double4(2), new double4(3)),
                new double4(1), new double4(2), new double4(3));
            Check3(new float3x3s(new float3s(1, 2, 3), new float3s(4, 5, 6), new float3s(7, 8, 9)),
                new float3s(1, 2, 3), new float3s(4, 5, 6), new float3s(7, 8, 9));
            Check4(new float2x4(new float2(1, 2), new float2(3, 4), new float2(5, 6), new float2(7, 8)),
                new float2(1, 2), new float2(3, 4), new float2(5, 6), new float2(7, 8));
            Check4(new int4x4(new int4(1), new int4(2), new int4(3), new int4(4)),
                new int4(1), new int4(2), new int4(3), new int4(4));

            // the shape of a matrix reaches the interface of the number of its columns, whichever count it has
            Assert.That(typeof(IMatrixMx2<float3x2>).IsAssignableFrom(typeof(float3x2)), Is.True);
            Assert.That(typeof(IMatrixMx2<float3x2s>).IsAssignableFrom(typeof(float3x2s)), Is.True);
            Assert.That(typeof(IMatrixMx3<float2x3>).IsAssignableFrom(typeof(float2x3)), Is.True);
            Assert.That(typeof(IMatrixMx3<double4x3>).IsAssignableFrom(typeof(double4x3)), Is.True);
            Assert.That(typeof(IMatrixMx4<int4x4>).IsAssignableFrom(typeof(int4x4)), Is.True);
            // a matrix of a mask reaches the interface of the count of its columns as well
            Assert.That(typeof(IMatrixMx2<b32m3x2>).IsAssignableFrom(typeof(b32m3x2)), Is.True);
        }
    }
}
