using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The member of the simd library that transposes a matrix reaches the columns of the result with the transpose
/// of the even and the odd lanes of the registers of the value beside the low and the high combinations of them
/// wherever the machine is an arm one, and the member of the value of a machine that is not an arm one never
/// reaches that path, so the checks here reach it themselves and hold it against the member of the value. The
/// registers of the columns of the value are taken from the member of the value of the shape, so the checks cover
/// the registers of the columns of the value as well as the transpose of them.
/// </summary>
public class TestMatrixTransposeArm
{
    [Test]
    public void OfTwoByTwo()
    {
        if (!AdvSimd.Arm64.IsSupported) Assert.Ignore("the machine has no arm register");

        using (Assert.EnterMultipleScope())
        {
            var mf = new float2x2(1f, 2f, 3f, 4f);
            var tf = math.transpose(mf);
            Assert.That(Transpose2x2Arm(mf.c0.vector, mf.c1.vector),
                Is.EqualTo((tf.c0.vector, tf.c1.vector)), "float");

            var mi = new int2x2(1, 2, 3, 4);
            var ti = math.transpose(mi);
            var (i0, i1) = Transpose2x2Arm(mi.c0.vector.AsSingle(), mi.c1.vector.AsSingle());
            Assert.That((i0.AsInt32(), i1.AsInt32()), Is.EqualTo((ti.c0.vector, ti.c1.vector)), "int");

            var mu = new uint2x2(1u, 2u, 3u, 4u);
            var tu = math.transpose(mu);
            var (u0, u1) = Transpose2x2Arm(mu.c0.vector.AsSingle(), mu.c1.vector.AsSingle());
            Assert.That((u0.AsUInt32(), u1.AsUInt32()), Is.EqualTo((tu.c0.vector, tu.c1.vector)), "uint");

            // a column of the value of a kind of 8 bytes is the register of it itself
            var md = new double2x2(1d, 2d, 3d, 4d);
            var td = math.transpose(md);
            Assert.That(Transpose2x2Arm(md.c0.vector, md.c1.vector),
                Is.EqualTo((td.c0.vector, td.c1.vector)), "double");

            var ml = new long2x2(1L, 2L, 3L, 4L);
            var tl = math.transpose(ml);
            var (l0, l1) = Transpose2x2Arm(ml.c0.vector.AsDouble(), ml.c1.vector.AsDouble());
            Assert.That((l0.AsInt64(), l1.AsInt64()), Is.EqualTo((tl.c0.vector, tl.c1.vector)), "long");

            var mg = new ulong2x2(1UL, 2UL, 3UL, 4UL);
            var tg = math.transpose(mg);
            var (g0, g1) = Transpose2x2Arm(mg.c0.vector.AsDouble(), mg.c1.vector.AsDouble());
            Assert.That((g0.AsUInt64(), g1.AsUInt64()), Is.EqualTo((tg.c0.vector, tg.c1.vector)), "ulong");
        }
    }

    [Test]
    public void OfSquare()
    {
        if (!AdvSimd.Arm64.IsSupported) Assert.Ignore("the machine has no arm register");

        using (Assert.EnterMultipleScope())
        {
            // the register of a column of the value of a kind of 4 bytes keeps the three or the four components of
            // it in the lanes of it beside a lane of padding
            var m3 = new float3x3(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f);
            var t3 = math.transpose(m3);
            Assert.That(Transpose3x3Arm(m3.c0.vector, m3.c1.vector, m3.c2.vector),
                Is.EqualTo((t3.c0.vector, t3.c1.vector, t3.c2.vector)), "a matrix of 3 rows of float");

            var i3 = new int3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            var ti3 = math.transpose(i3);
            var (j0, j1, j2) = Transpose3x3Arm(i3.c0.vector.AsSingle(), i3.c1.vector.AsSingle(), i3.c2.vector.AsSingle());
            Assert.That((j0.AsInt32(), j1.AsInt32(), j2.AsInt32()),
                Is.EqualTo((ti3.c0.vector, ti3.c1.vector, ti3.c2.vector)), "a matrix of 3 rows of int");

            var u3 = new uint3x3(1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u);
            var tu3 = math.transpose(u3);
            var (k0, k1, k2) = Transpose3x3Arm(u3.c0.vector.AsSingle(), u3.c1.vector.AsSingle(), u3.c2.vector.AsSingle());
            Assert.That((k0.AsUInt32(), k1.AsUInt32(), k2.AsUInt32()),
                Is.EqualTo((tu3.c0.vector, tu3.c1.vector, tu3.c2.vector)), "a matrix of 3 rows of uint");

            var m4 = new float4x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f);
            var t4 = math.transpose(m4);
            Assert.That(Transpose4x4Arm(m4.c0.vector, m4.c1.vector, m4.c2.vector, m4.c3.vector),
                Is.EqualTo((t4.c0.vector, t4.c1.vector, t4.c2.vector, t4.c3.vector)), "a matrix of 4 rows of float");

            var i4 = new int4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            var ti4 = math.transpose(i4);
            var (q0, q1, q2, q3) = Transpose4x4Arm(
                i4.c0.vector.AsSingle(), i4.c1.vector.AsSingle(), i4.c2.vector.AsSingle(), i4.c3.vector.AsSingle());
            Assert.That((q0.AsInt32(), q1.AsInt32(), q2.AsInt32(), q3.AsInt32()),
                Is.EqualTo((ti4.c0.vector, ti4.c1.vector, ti4.c2.vector, ti4.c3.vector)), "a matrix of 4 rows of int");

            var u4 = new uint4x4(1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u, 10u, 11u, 12u, 13u, 14u, 15u, 16u);
            var tu4 = math.transpose(u4);
            var (r0, r1, r2, r3) = Transpose4x4Arm(
                u4.c0.vector.AsSingle(), u4.c1.vector.AsSingle(), u4.c2.vector.AsSingle(), u4.c3.vector.AsSingle());
            Assert.That((r0.AsUInt32(), r1.AsUInt32(), r2.AsUInt32(), r3.AsUInt32()),
                Is.EqualTo((tu4.c0.vector, tu4.c1.vector, tu4.c2.vector, tu4.c3.vector)),
                "a matrix of 4 rows of uint");
        }
    }

    [Test]
    public void OfWideAndTall()
    {
        if (!AdvSimd.Arm64.IsSupported) Assert.Ignore("the machine has no arm register");

        using (Assert.EnterMultipleScope())
        {
            // a column of the value of a matrix of 2 rows and 4 columns keeps its two components in the two lower
            // lanes of the register of it beside two lanes of padding and a column of the value of a matrix of 4
            // rows and 2 columns is the register of it itself
            var w = new float2x4(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f);
            var tw = math.transpose(w);
            Assert.That(Transpose2x4To4x2Arm(w.c0.vector, w.c1.vector, w.c2.vector, w.c3.vector),
                Is.EqualTo((tw.c0.vector, tw.c1.vector)), "a matrix of 2 rows and 4 columns of float");

            var iw = new int2x4(1, 2, 3, 4, 5, 6, 7, 8);
            var tiw = math.transpose(iw);
            var (s0, s1) = Transpose2x4To4x2Arm(
                iw.c0.vector.AsSingle(), iw.c1.vector.AsSingle(), iw.c2.vector.AsSingle(), iw.c3.vector.AsSingle());
            Assert.That((s0.AsInt32(), s1.AsInt32()), Is.EqualTo((tiw.c0.vector, tiw.c1.vector)),
                "a matrix of 2 rows and 4 columns of int");

            var uw = new uint2x4(1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u);
            var tuw = math.transpose(uw);
            var (v0, v1) = Transpose2x4To4x2Arm(
                uw.c0.vector.AsSingle(), uw.c1.vector.AsSingle(), uw.c2.vector.AsSingle(), uw.c3.vector.AsSingle());
            Assert.That((v0.AsUInt32(), v1.AsUInt32()), Is.EqualTo((tuw.c0.vector, tuw.c1.vector)),
                "a matrix of 2 rows and 4 columns of uint");

            // the two components of a column of the result of a matrix of 4 rows and 2 columns are the two lower
            // lanes of the register of that column and the lanes that follow them are padding, which the member of
            // the shape keeps at zero on every path of it
            var d = new float4x2(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f);
            var td = math.transpose(d);
            Assert.That(Transpose4x2To2x4Arm(d.c0.vector, d.c1.vector),
                Is.EqualTo((td.c0.vector, td.c1.vector, td.c2.vector, td.c3.vector)),
                "a matrix of 4 rows and 2 columns of float");

            var id = new int4x2(1, 2, 3, 4, 5, 6, 7, 8);
            var tid = math.transpose(id);
            var (b0, b1, b2, b3) = Transpose4x2To2x4Arm(id.c0.vector.AsSingle(), id.c1.vector.AsSingle());
            Assert.That((b0.AsInt32(), b1.AsInt32(), b2.AsInt32(), b3.AsInt32()),
                Is.EqualTo((tid.c0.vector, tid.c1.vector, tid.c2.vector, tid.c3.vector)),
                "a matrix of 4 rows and 2 columns of int");

            var ud = new uint4x2(1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u);
            var tud = math.transpose(ud);
            var (c0, c1, c2, c3) = Transpose4x2To2x4Arm(ud.c0.vector.AsSingle(), ud.c1.vector.AsSingle());
            Assert.That((c0.AsUInt32(), c1.AsUInt32(), c2.AsUInt32(), c3.AsUInt32()),
                Is.EqualTo((tud.c0.vector, tud.c1.vector, tud.c2.vector, tud.c3.vector)),
                "a matrix of 4 rows and 2 columns of uint");
        }
    }

    /// <summary>
    /// The transpose of the two registers of the columns of a matrix of 2 rows and 2 columns reached the way the
    /// member of the simd library reaches it on arm: the even lanes of the two registers of the value are the first
    /// column of the result beside the padding of the first column of the value and the odd lanes of them are the
    /// second.
    /// </summary>
    private static (Vector128<float> c0, Vector128<float> c1) Transpose2x2Arm(
        Vector128<float> c0, Vector128<float> c1
    ) => (AdvSimd.Arm64.TransposeEven(c0, c1), AdvSimd.Arm64.TransposeOdd(c0, c1));

    /// <summary>
    /// The transpose of the two registers of the columns of a matrix of 2 rows and 2 columns of a kind of 8 bytes
    /// reached the way the member of the simd library reaches it on arm: the even lanes of the two registers of the
    /// value are the first column of the result and the odd lanes of them are the second.
    /// </summary>
    private static (Vector128<double> c0, Vector128<double> c1) Transpose2x2Arm(
        Vector128<double> c0, Vector128<double> c1
    ) => (AdvSimd.Arm64.TransposeEven(c0, c1), AdvSimd.Arm64.TransposeOdd(c0, c1));

    /// <summary>
    /// The transpose of the registers of the columns of a matrix of 3 rows and 3 columns reached the way the member
    /// of the simd library reaches it on arm, where the transpose of a matrix of 4 rows and 4 columns is taken
    /// beside the register of the padding of the columns of the value.
    /// </summary>
    private static (Vector128<float> c0, Vector128<float> c1, Vector128<float> c2) Transpose3x3Arm(
        Vector128<float> c0, Vector128<float> c1, Vector128<float> c2
    )
    {
        var a = AdvSimd.Arm64.TransposeEven(c0, c2); // (c0.x, c2.x, c0.z, c2.z)
        var b = AdvSimd.Arm64.TransposeEven(c1, Vector128<float>.Zero); // (c1.x, 0, c1.z, 0)
        var c = AdvSimd.Arm64.TransposeOdd(c0, c2); // (c0.y, c2.y, c0.w, c2.w)
        var d = AdvSimd.Arm64.TransposeOdd(c1, Vector128<float>.Zero); // (c1.y, 0, c1.w, 0)
        return (AdvSimd.Arm64.ZipLow(a, b), AdvSimd.Arm64.ZipLow(c, d), AdvSimd.Arm64.ZipHigh(a, b));
    }

    /// <summary>
    /// The transpose of the registers of the columns of a matrix of 4 rows and 4 columns reached the way the member
    /// of the simd library reaches it on arm: the even lanes of the first and the third registers of the value
    /// beside the even lanes of the second and the fourth take the columns of the result at the even indexes and
    /// the odd lanes of the two pairs take the columns at the odd indexes.
    /// </summary>
    private static (Vector128<float> c0, Vector128<float> c1, Vector128<float> c2, Vector128<float> c3) Transpose4x4Arm(
        Vector128<float> c0, Vector128<float> c1, Vector128<float> c2, Vector128<float> c3
    )
    {
        var a = AdvSimd.Arm64.TransposeEven(c0, c2); // (c0.x, c2.x, c0.z, c2.z)
        var b = AdvSimd.Arm64.TransposeEven(c1, c3); // (c1.x, c3.x, c1.z, c3.z)
        var c = AdvSimd.Arm64.TransposeOdd(c0, c2); // (c0.y, c2.y, c0.w, c2.w)
        var d = AdvSimd.Arm64.TransposeOdd(c1, c3); // (c1.y, c3.y, c1.w, c3.w)
        return (
            AdvSimd.Arm64.ZipLow(a, b), // (c0.x, c1.x, c2.x, c3.x)
            AdvSimd.Arm64.ZipLow(c, d), // (c0.y, c1.y, c2.y, c3.y)
            AdvSimd.Arm64.ZipHigh(a, b), // (c0.z, c1.z, c2.z, c3.z)
            AdvSimd.Arm64.ZipHigh(c, d) // (c0.w, c1.w, c2.w, c3.w)
        );
    }

    /// <summary>
    /// The transpose of the registers of the columns of a matrix of 2 rows and 4 columns reached the way the member
    /// of the simd library reaches it on arm: the even lanes of the first and the third registers of the value
    /// beside the even lanes of the second and the fourth take the first column of the result and the odd lanes of
    /// the two pairs take the second.
    /// </summary>
    private static (Vector128<float> c0, Vector128<float> c1) Transpose2x4To4x2Arm(
        Vector128<float> c0, Vector128<float> c1, Vector128<float> c2, Vector128<float> c3
    )
    {
        var a = AdvSimd.Arm64.TransposeEven(c0, c2); // (c0.x, c2.x, c0.z, c2.z)
        var b = AdvSimd.Arm64.TransposeEven(c1, c3); // (c1.x, c3.x, c1.z, c3.z)
        var c = AdvSimd.Arm64.TransposeOdd(c0, c2); // (c0.y, c2.y, c0.w, c2.w)
        var d = AdvSimd.Arm64.TransposeOdd(c1, c3); // (c1.y, c3.y, c1.w, c3.w)
        return (AdvSimd.Arm64.ZipLow(a, b), AdvSimd.Arm64.ZipLow(c, d));
    }

    /// <summary>
    /// The transpose of the two registers of the columns of a matrix of 4 rows and 2 columns reached the way the
    /// member of the simd library reaches it on arm: the low halves of the two registers of the value are the pair
    /// of the first and the second columns of the result and the high halves are the pair of the third and the
    /// fourth, and the pair of lanes of every one of the four columns of the result is widened with the zero
    /// register, which keeps the padding lanes of that column at zero.
    /// </summary>
    private static (Vector128<float> c0, Vector128<float> c1, Vector128<float> c2, Vector128<float> c3) Transpose4x2To2x4Arm(
        Vector128<float> c0, Vector128<float> c1
    )
    {
        var a = AdvSimd.Arm64.ZipLow(c0, c1); // (c0.x, c1.x, c0.y, c1.y)
        var b = AdvSimd.Arm64.ZipHigh(c0, c1); // (c0.z, c1.z, c0.w, c1.w)
        return (
            Vector128.Create(a.GetLower(), Vector64<float>.Zero), // (c0.x, c1.x, 0, 0)
            Vector128.Create(a.GetUpper(), Vector64<float>.Zero), // (c0.y, c1.y, 0, 0)
            Vector128.Create(b.GetLower(), Vector64<float>.Zero), // (c0.z, c1.z, 0, 0)
            Vector128.Create(b.GetUpper(), Vector64<float>.Zero) // (c0.w, c1.w, 0, 0)
        );
    }
}
