using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The members that move a half of a value to the other half of it and the ones that interlace the halves of two
/// values reach the halves of the registers of the value with the lanes of 64 bits of them on an arm machine, and
/// the member of a value of a machine that is not an arm one never reaches those paths, so the checks here reach
/// them themselves and hold them against the member of the value. The registers of the columns of the value are
/// taken from the member of the value, so the checks cover the registers of the value as well as the members.
/// </summary>
public class TestMoveUnpackArm
{
    [Test]
    public void OfMove()
    {
        if (!AdvSimd.Arm64.IsSupported) Assert.Ignore("the machine has no arm register");

        using (Assert.EnterMultipleScope())
        {
            var a = new float4(1, 2, 3, 4);
            var b = new float4(5, 6, 7, 8);
            Assert.That(MoveLowToHighArm(a.vector, b.vector), Is.EqualTo(math.movelh(a, b).vector),
                "the low halves of float");
            Assert.That(MoveHighToLowArm(a.vector, b.vector), Is.EqualTo(math.movehl(a, b).vector),
                "the high halves of float");

            var i = new int4(1, 2, 3, 4);
            var j = new int4(5, 6, 7, 8);
            Assert.That(MoveLowToHighArm(i.vector.AsSingle(), j.vector.AsSingle()).AsInt32(),
                Is.EqualTo(math.movelh(i, j).vector), "the low halves of int");
            Assert.That(MoveHighToLowArm(i.vector.AsSingle(), j.vector.AsSingle()).AsInt32(),
                Is.EqualTo(math.movehl(i, j).vector), "the high halves of int");

            var u = new uint4(1, 2, 3, 4);
            var v = new uint4(5, 6, 7, 8);
            Assert.That(MoveLowToHighArm(u.vector.AsSingle(), v.vector.AsSingle()).AsUInt32(),
                Is.EqualTo(math.movelh(u, v).vector), "the low halves of uint");
            Assert.That(MoveHighToLowArm(u.vector.AsSingle(), v.vector.AsSingle()).AsUInt32(),
                Is.EqualTo(math.movehl(u, v).vector), "the high halves of uint");

            // a value of 8 bytes has no register of its own on an arm machine, so the member of it reads the
            // components of the two values
            var d = new double4(1, 2, 3, 4);
            var e = new double4(5, 6, 7, 8);
            Assert.That(math.movelh(d, e), Is.EqualTo(new double4(1, 2, 5, 6)), "the low halves of double");
            Assert.That(math.movehl(d, e), Is.EqualTo(new double4(7, 8, 3, 4)), "the high halves of double");

            var l = new long4(1, 2, 3, 4);
            var n = new long4(5, 6, 7, 8);
            Assert.That(math.movelh(l, n), Is.EqualTo(new long4(1, 2, 5, 6)), "the low halves of long");
            Assert.That(math.movehl(l, n), Is.EqualTo(new long4(7, 8, 3, 4)), "the high halves of long");

            var g = new ulong4(1, 2, 3, 4);
            var w = new ulong4(5, 6, 7, 8);
            Assert.That(math.movelh(g, w), Is.EqualTo(new ulong4(1, 2, 5, 6)), "the low halves of ulong");
            Assert.That(math.movehl(g, w), Is.EqualTo(new ulong4(7, 8, 3, 4)), "the high halves of ulong");
        }
    }

    [Test]
    public void OfUnpack()
    {
        if (!AdvSimd.Arm64.IsSupported) Assert.Ignore("the machine has no arm register");

        using (Assert.EnterMultipleScope())
        {
            var a = new float4(1, 2, 3, 4);
            var b = new float4(5, 6, 7, 8);
            Assert.That(UnpackLowArm(a.vector, b.vector), Is.EqualTo(math.unpacklo(a, b).vector),
                "the low halves of float");
            Assert.That(UnpackHighArm(a.vector, b.vector), Is.EqualTo(math.unpackhi(a, b).vector),
                "the high halves of float");

            var i = new int4(1, 2, 3, 4);
            var j = new int4(5, 6, 7, 8);
            Assert.That(UnpackLowArm(i.vector.AsSingle(), j.vector.AsSingle()).AsInt32(),
                Is.EqualTo(math.unpacklo(i, j).vector), "the low halves of int");
            Assert.That(UnpackHighArm(i.vector.AsSingle(), j.vector.AsSingle()).AsInt32(),
                Is.EqualTo(math.unpackhi(i, j).vector), "the high halves of int");

            var u = new uint4(1, 2, 3, 4);
            var v = new uint4(5, 6, 7, 8);
            Assert.That(UnpackLowArm(u.vector.AsSingle(), v.vector.AsSingle()).AsUInt32(),
                Is.EqualTo(math.unpacklo(u, v).vector), "the low halves of uint");
            Assert.That(UnpackHighArm(u.vector.AsSingle(), v.vector.AsSingle()).AsUInt32(),
                Is.EqualTo(math.unpackhi(u, v).vector), "the high halves of uint");

            // a value of 8 bytes has no register of its own on an arm machine, so the member of it reads the
            // components of the two values
            var d = new double4(1, 2, 3, 4);
            var e = new double4(5, 6, 7, 8);
            Assert.That(math.unpacklo(d, e), Is.EqualTo(new double4(1, 5, 2, 6)), "the low halves of double");
            Assert.That(math.unpackhi(d, e), Is.EqualTo(new double4(3, 7, 4, 8)), "the high halves of double");

            var l = new long4(1, 2, 3, 4);
            var n = new long4(5, 6, 7, 8);
            Assert.That(math.unpacklo(l, n), Is.EqualTo(new long4(1, 5, 2, 6)), "the low halves of long");
            Assert.That(math.unpackhi(l, n), Is.EqualTo(new long4(3, 7, 4, 8)), "the high halves of long");

            var g = new ulong4(1, 2, 3, 4);
            var w = new ulong4(5, 6, 7, 8);
            Assert.That(math.unpacklo(g, w), Is.EqualTo(new ulong4(1, 5, 2, 6)), "the low halves of ulong");
            Assert.That(math.unpackhi(g, w), Is.EqualTo(new ulong4(3, 7, 4, 8)), "the high halves of ulong");
        }
    }

    /// <summary>
    /// The combination of the low half of the first register of a value with the low half of the second one
    /// reached the way the member of the simd library reaches it on arm: the two halves of a register of 128 bits
    /// are the two lanes of 64 bits of it, so the interlace of the low halves of the two registers is the low
    /// lane of the two of them.
    /// </summary>
    private static Vector128<float> MoveLowToHighArm(Vector128<float> a, Vector128<float> b) =>
        AdvSimd.Arm64.ZipLow(a.AsInt64(), b.AsInt64()).AsSingle();

    /// <summary>
    /// The combination of the high half of the second register of a value with the high half of the first one
    /// reached the way the member of the simd library reaches it on arm: the interlace of the high halves of the
    /// two registers is the high lane of the two of them, which the other order of the registers turns around.
    /// </summary>
    private static Vector128<float> MoveHighToLowArm(Vector128<float> a, Vector128<float> b) =>
        AdvSimd.Arm64.ZipHigh(b.AsInt64(), a.AsInt64()).AsSingle();

    /// <summary>
    /// The interlace of the low halves of the two registers of a value reached the way the member of the simd
    /// library reaches it on arm: the interlace of the lanes of the low halves of the two registers.
    /// </summary>
    private static Vector128<float> UnpackLowArm(Vector128<float> a, Vector128<float> b) =>
        AdvSimd.Arm64.ZipLow(a, b);

    /// <summary>
    /// The interlace of the high halves of the two registers of a value reached the way the member of the simd
    /// library reaches it on arm: the interlace of the lanes of the high halves of the two registers.
    /// </summary>
    private static Vector128<float> UnpackHighArm(Vector128<float> a, Vector128<float> b) =>
        AdvSimd.Arm64.ZipHigh(a, b);
}
