using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The member of the simd library that transposes the matrix of 2 rows and 2 columns reaches the two columns of
/// the result with the shuffles of a register of 128 bits wherever the machine has no register of 256 bits, and
/// the member of the value of that shape reaches the wide register of the machine it runs on first, so the check
/// here reaches the narrow path itself and holds it against the member of the value. The two registers of the
/// columns of the value are taken from the member of the value of the shape, so the check covers the register of
/// a column of the value as well as the transpose of it.
/// </summary>
public class TestMatrixTransposeSse
{
    [Test]
    public void OfTwoByTwo()
    {
        if (!Sse2.IsSupported) Assert.Ignore("the machine has no register of 128 bits");

        using (Assert.EnterMultipleScope())
        {
            // a column of the value of a kind of 4 bytes sits in the two lower lanes of the register of it beside
            // two lanes of padding
            var mf = new float2x2(1f, 2f, 3f, 4f);
            var tf = math.transpose(mf);
            Assert.That(Transpose2x2Narrow(mf.c0.vector, mf.c1.vector),
                Is.EqualTo((tf.c0.vector, tf.c1.vector)), "float");

            var mi = new int2x2(1, 2, 3, 4);
            var ti = math.transpose(mi);
            var (i0, i1) = Transpose2x2Narrow(mi.c0.vector.AsSingle(), mi.c1.vector.AsSingle());
            Assert.That((i0.AsInt32(), i1.AsInt32()), Is.EqualTo((ti.c0.vector, ti.c1.vector)), "int");

            var mu = new uint2x2(1u, 2u, 3u, 4u);
            var tu = math.transpose(mu);
            var (u0, u1) = Transpose2x2Narrow(mu.c0.vector.AsSingle(), mu.c1.vector.AsSingle());
            Assert.That((u0.AsUInt32(), u1.AsUInt32()), Is.EqualTo((tu.c0.vector, tu.c1.vector)), "uint");

            // a column of the value of a kind of 8 bytes is the register of it itself
            var md = new double2x2(1d, 2d, 3d, 4d);
            var td = math.transpose(md);
            Assert.That(Transpose2x2Narrow(md.c0.vector, md.c1.vector),
                Is.EqualTo((td.c0.vector, td.c1.vector)), "double");

            var ml = new long2x2(1L, 2L, 3L, 4L);
            var tl = math.transpose(ml);
            var (l0, l1) = Transpose2x2Narrow(ml.c0.vector.AsDouble(), ml.c1.vector.AsDouble());
            Assert.That((l0.AsInt64(), l1.AsInt64()), Is.EqualTo((tl.c0.vector, tl.c1.vector)), "long");

            var mg = new ulong2x2(1UL, 2UL, 3UL, 4UL);
            var tg = math.transpose(mg);
            var (g0, g1) = Transpose2x2Narrow(mg.c0.vector.AsDouble(), mg.c1.vector.AsDouble());
            Assert.That((g0.AsUInt64(), g1.AsUInt64()), Is.EqualTo((tg.c0.vector, tg.c1.vector)), "ulong");
        }
    }

    /// <summary>
    /// The transpose of the two registers of the columns of a matrix of 2 rows and 2 columns of a kind of 4 bytes
    /// reached the way the member of the simd library reaches it where the machine has no register of 256 bits: a
    /// column of the value sits in the two lower lanes of the register of it beside two lanes of padding, so the
    /// two registers are combined into a single register whose halves each take a column of the result out of the
    /// two lanes of the value at the index of that column beside the padding of the first column of the value.
    /// </summary>
    private static (Vector128<float> c0, Vector128<float> c1) Transpose2x2Narrow(
        Vector128<float> c0, Vector128<float> c1
    )
    {
        var a = Sse.MoveLowToHigh(c0, c1); // (c0.x, c0.y, c1.x, c1.y)
        var r0 = Sse.Shuffle(a, Vector128<float>.Zero, 0x08); // (a0, a2, 0, 0) => (c0.x, c1.x, 0, 0)
        var r1 = Sse.Shuffle(a, Vector128<float>.Zero, 0x0D); // (a1, a3, 0, 0) => (c0.y, c1.y, 0, 0)
        return (r0, r1);
    }

    /// <summary>
    /// The transpose of the two registers of the columns of a matrix of 2 rows and 2 columns of a kind of 8 bytes
    /// reached the way the member of the simd library reaches it where the machine has no register of 256 bits: a
    /// column of the value is the register of it itself, so the low lanes of the two registers of the value are
    /// the first column of the result and the high lanes of them are the second.
    /// </summary>
    private static (Vector128<double> c0, Vector128<double> c1) Transpose2x2Narrow(
        Vector128<double> c0, Vector128<double> c1
    ) => (Sse2.UnpackLow(c0, c1), Sse2.UnpackHigh(c0, c1));
}
