using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The visitors that reduce the vectors a matrix is made of to a single vector: only a matrix is made of
/// vectors, so only a matrix hands its value over to them. The columns of a matrix are the vectors it is made of,
/// so the visitor of the columns combines them and the member of a count of them holds the way two of the columns
/// combine, which a visitor that maps a column before they are combined overrides. The rows of a matrix have no
/// value of their own, so the visitor of the rows reduces every column of it to a single component and builds the
/// vector of the values of the reductions. The visitors are only written for a test, the members that reduce the
/// vectors of a matrix are the ones the operation of a visitor decides, and they are implemented explicitly, so
/// the constraints of the members are inherited from the interfaces and are not repeated here.
/// </summary>
public class TestVectorDispatch
{
    /// <summary>
    /// Returns the sum of the columns of a matrix, which is the sum of every one of them component by component.
    /// </summary>
    private struct impl_sum_columns : IMatrixColumnVisitor<impl_sum_columns>
    {
        static TVector IMatrixColumnVisitor<impl_sum_columns>.Combine_Number<TVector, TScalar>(in TVector a, in TVector b)
            => a + b;
    }

    /// <summary>
    /// Returns the sum of the rows of a matrix, which is the sum of every column of it: the columns of the matrix
    /// are handed over, every one of them is reduced by the sum and the values of the reductions are the
    /// components of the row vector they build.
    /// </summary>
    private struct impl_sum_rows : IMatrixRowVisitor<impl_sum_rows>
    {
        static TRow IMatrixRowVisitor<impl_sum_rows>.Row2_Number<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1)
            => TRow.Create(math.sum<TColumn, TScalar>(c0), math.sum<TColumn, TScalar>(c1));

        static TRow IMatrixRowVisitor<impl_sum_rows>.Row3_Number<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1,
            in TColumn c2)
            => TRow.Create(math.sum<TColumn, TScalar>(c0), math.sum<TColumn, TScalar>(c1),
                math.sum<TColumn, TScalar>(c2));

        static TRow IMatrixRowVisitor<impl_sum_rows>.Row4_Number<TColumn, TRow, TScalar>(in TColumn c0, in TColumn c1,
            in TColumn c2, in TColumn c3)
            => TRow.Create(math.sum<TColumn, TScalar>(c0), math.sum<TColumn, TScalar>(c1),
                math.sum<TColumn, TScalar>(c2), math.sum<TColumn, TScalar>(c3));
    }

    /// <summary>
    /// Returns the sum of the squares of the columns of a matrix: the members of a count of the columns are the
    /// ones that reach the columns themselves, so a visitor that maps them before they are combined overrides
    /// them and the one that combines two of them.
    /// </summary>
    private struct impl_sum_sq_columns : IMatrixColumnVisitor<impl_sum_sq_columns>
    {
        static TVector IMatrixColumnVisitor<impl_sum_sq_columns>.Combine_Number<TVector, TScalar>(in TVector a, in TVector b)
            => a + b;

        static TVector IMatrixColumnVisitor<impl_sum_sq_columns>.Combine2_Number<TVector, TScalar>(in TVector c0, in TVector c1)
            => (c0 * c0) + (c1 * c1);

        static TVector IMatrixColumnVisitor<impl_sum_sq_columns>.Combine3_Number<TVector, TScalar>(in TVector c0, in TVector c1,
            in TVector c2)
            => ((c0 * c0) + (c1 * c1)) + (c2 * c2);

        static TVector IMatrixColumnVisitor<impl_sum_sq_columns>.Combine4_Number<TVector, TScalar>(in TVector c0, in TVector c1,
            in TVector c2, in TVector c3)
            => (((c0 * c0) + (c1 * c1)) + (c2 * c2)) + (c3 * c3);
    }

    /// <summary>
    /// The type of a vector a matrix is made of is a part of the interface of its dispatch, so a helper hands the
    /// type of it over beside the type of the matrix: the type of a single component of the value is the one of
    /// that vector, so it is not named by the interface of the dispatch of the vectors.
    /// </summary>
    private static TVector SumColumns<T, TVector>(in T value)
        where T : unmanaged, IMatrixColumnDispatch<T, TVector>
        where TVector : unmanaged, INumberVector<TVector>
        => T.Combine<impl_sum_columns>(value);

    private static TVector SumRows<T, TVector>(in T value)
        where T : unmanaged, IMatrixRowDispatch<T, TVector>
        where TVector : unmanaged, INumberVector<TVector>
        => T.Reduce<impl_sum_rows>(value);

    private static TVector SumSqColumns<T, TVector>(in T value)
        where T : unmanaged, IMatrixColumnDispatch<T, TVector>
        where TVector : unmanaged, INumberVector<TVector>
        => T.Combine<impl_sum_sq_columns>(value);

    /// <summary>
    /// The count of the columns of a matrix decides the member of the visitor that reaches them and the type of
    /// a column of it decides how the columns are kept, which the value of a column of every kind covers.
    /// </summary>
    [Test]
    public void Sum()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(SumColumns<float3x2, float3>(new float3x2(new float3(1f, 2f, 3f),
                    new float3(4f, 5f, 6f))),
                Is.EqualTo(new float3(5f, 7f, 9f)), "2 columns");
            Assert.That(SumColumns<float3x3, float3>(new float3x3(new float3(1f, 2f, 3f),
                    new float3(4f, 5f, 6f), new float3(7f, 8f, 9f))),
                Is.EqualTo(new float3(12f, 15f, 18f)), "3 columns");
            Assert.That(SumColumns<float3x4, float3>(new float3x4(new float3(1f, 2f, 3f),
                    new float3(4f, 5f, 6f), new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float3(22f, 26f, 30f)), "4 columns");

            // the columns of a matrix of another component type and another count of the rows
            Assert.That(SumColumns<int2x2, int2>(new int2x2(new int2(1, 2), new int2(3, 4))),
                Is.EqualTo(new int2(4, 6)), "int2, a 128 bit register with padding lanes");
            Assert.That(SumColumns<double3x3, double3>(new double3x3(new double3(1d, 2d, 3d),
                    new double3(4d, 5d, 6d), new double3(7d, 8d, 9d))),
                Is.EqualTo(new double3(12d, 15d, 18d)), "double3, a 256 bit register");
            Assert.That(SumColumns<long2x4, long2>(new long2x4(new long2(1L, 2L), new long2(3L, 4L),
                    new long2(5L, 6L), new long2(7L, 8L))),
                Is.EqualTo(new long2(16L, 20L)), "long2, a 128 bit register");
            Assert.That(SumColumns<half3x3, half3>(new half3x3(new half3((half)1f, (half)2f, (half)3f),
                    new half3((half)4f, (half)5f, (half)6f), new half3((half)7f, (half)8f, (half)9f))),
                Is.EqualTo(new half3((half)12f, (half)15f, (half)18f)), "half3, a value without a register");
        }
    }

    /// <summary>
    /// Every column of a matrix is reduced by the visitor of the dispatch of the rows and the values of the
    /// reductions are the components of the vector they build, so the component of the result at the index of a
    /// column of the matrix is the sum of the components of that column of it.
    /// </summary>
    [Test]
    public void SumRows()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(SumRows<float3x2, float2>(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f))),
                Is.EqualTo(new float2(6f, 15f)), "3 rows of 2 components");
            Assert.That(SumRows<float3x3, float3>(new float3x3(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f))),
                Is.EqualTo(new float3(6f, 15f, 24f)), "3 rows of 3 components");
            Assert.That(SumRows<float3x4, float4>(new float3x4(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f),
                    new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float4(6f, 15f, 24f, 33f)), "3 rows of 4 components");

            // the type of a row of a matrix of another component type and another shape
            Assert.That(SumRows<int2x2, int2>(new int2x2(new int2(1, 2), new int2(3, 4))),
                Is.EqualTo(new int2(3, 7)), "int2, 2 rows of 2 components");
            Assert.That(SumRows<half3x2, half2>(new half3x2(new half3((half)1f, (half)2f, (half)3f),
                    new half3((half)4f, (half)5f, (half)6f))),
                Is.EqualTo(new half2((half)6f, (half)15f)), "half2, a value without a register");
        }
    }

    /// <summary>
    /// A member of a count of the vectors that a visitor overrides is the one that reaches the vectors of a
    /// matrix of that count, so the map of a vector reaches the vectors of every shape of the count.
    /// </summary>
    [Test]
    public void MappedVectors()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(SumSqColumns<float3x2, float3>(new float3x2(new float3(1f, 2f, 3f),
                    new float3(4f, 5f, 6f))),
                Is.EqualTo(new float3(17f, 29f, 45f)), "the sum of the squares of 2 columns");
            Assert.That(SumSqColumns<float3x3, float3>(new float3x3(new float3(1f, 2f, 3f),
                    new float3(4f, 5f, 6f), new float3(7f, 8f, 9f))),
                Is.EqualTo(new float3(66f, 93f, 126f)), "the sum of the squares of 3 columns");
            Assert.That(SumSqColumns<float3x4, float3>(new float3x4(new float3(1f, 2f, 3f),
                    new float3(4f, 5f, 6f), new float3(7f, 8f, 9f), new float3(10f, 11f, 12f))),
                Is.EqualTo(new float3(166f, 214f, 270f)), "the sum of the squares of 4 columns");
            Assert.That(SumSqColumns<int3x3, int3>(new int3x3(new int3(1, 2, 3), new int3(4, 5, 6),
                    new int3(7, 8, 9))),
                Is.EqualTo(new int3(66, 93, 126)), "the sum of the squares of an integer matrix");
        }
    }
}
