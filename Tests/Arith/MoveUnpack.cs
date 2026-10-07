using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The members that move a half of a value to the other half of it and the ones that interlace the halves of two
/// values: the low half of <c>movelh</c> and the high half of <c>movehl</c> hold the half of the second value
/// that the name of the member names, and <c>unpacklo</c> and <c>unpackhi</c> interlace the low and the high
/// halves of the two values. The member of a simd backed value moves the halves of its register and the member
/// of a value without a register reads the components of it, both have to agree on the answer.
/// </summary>
public class TestMoveUnpack
{
    [Test]
    public void Move()
    {
        var a = new float4(1, 2, 3, 4);
        var b = new float4(5, 6, 7, 8);
        var d = new double4(1, 2, 3, 4);
        var e = new double4(5, 6, 7, 8);
        var i = new int4(1, 2, 3, 4);
        var j = new int4(5, 6, 7, 8);
        var l = new long4(1, 2, 3, 4);
        var n = new long4(5, 6, 7, 8);
        var h = new half4((Half)1, (Half)2, (Half)3, (Half)4);
        var k = new half4((Half)5, (Half)6, (Half)7, (Half)8);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.movelh(a, b), Is.EqualTo(new float4(1, 2, 5, 6)), "the low halves of float");
            Assert.That(math.movehl(a, b), Is.EqualTo(new float4(7, 8, 3, 4)), "the high halves of float");
            Assert.That(a.movelh(b), Is.EqualTo(new float4(1, 2, 5, 6)), "the member of the value");
            Assert.That(a.movehl(b), Is.EqualTo(new float4(7, 8, 3, 4)), "the member of the value");

            // the register of a 4 component vector of a double is 256 bits wide
            Assert.That(math.movelh(d, e), Is.EqualTo(new double4(1, 2, 5, 6)), "the low halves of double");
            Assert.That(math.movehl(d, e), Is.EqualTo(new double4(7, 8, 3, 4)), "the high halves of double");

            // the register of a whole number of 4 bytes is read as floats by the simd library
            Assert.That(math.movelh(i, j), Is.EqualTo(new int4(1, 2, 5, 6)), "the low halves of int");
            Assert.That(math.movehl(i, j), Is.EqualTo(new int4(7, 8, 3, 4)), "the high halves of int");
            Assert.That(math.movelh(l, n), Is.EqualTo(new long4(1, 2, 5, 6)), "the low halves of long");
            Assert.That(math.movehl(l, n), Is.EqualTo(new long4(7, 8, 3, 4)), "the high halves of long");

            // the half has no register, its components are read one by one
            Assert.That(math.movelh(h, k), Is.EqualTo(new half4((Half)1, (Half)2, (Half)5, (Half)6)),
                "the low halves of half");
            Assert.That(math.movehl(h, k), Is.EqualTo(new half4((Half)7, (Half)8, (Half)3, (Half)4)),
                "the high halves of half");
        }
    }

    [Test]
    public void Unpack()
    {
        var a = new float4(1, 2, 3, 4);
        var b = new float4(5, 6, 7, 8);
        var d = new double4(1, 2, 3, 4);
        var e = new double4(5, 6, 7, 8);
        var u = new uint4(1, 2, 3, 4);
        var v = new uint4(5, 6, 7, 8);
        var g = new ulong4(1, 2, 3, 4);
        var w = new ulong4(5, 6, 7, 8);
        var s = new short4(1, 2, 3, 4);
        var t = new short4(5, 6, 7, 8);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.unpacklo(a, b), Is.EqualTo(new float4(1, 5, 2, 6)), "the low halves of float");
            Assert.That(math.unpackhi(a, b), Is.EqualTo(new float4(3, 7, 4, 8)), "the high halves of float");
            Assert.That(a.unpacklo(b), Is.EqualTo(new float4(1, 5, 2, 6)), "the member of the value");
            Assert.That(a.unpackhi(b), Is.EqualTo(new float4(3, 7, 4, 8)), "the member of the value");

            Assert.That(math.unpacklo(d, e), Is.EqualTo(new double4(1, 5, 2, 6)), "the low halves of double");
            Assert.That(math.unpackhi(d, e), Is.EqualTo(new double4(3, 7, 4, 8)), "the high halves of double");

            Assert.That(math.unpacklo(u, v), Is.EqualTo(new uint4(1, 5, 2, 6)), "the low halves of uint");
            Assert.That(math.unpackhi(u, v), Is.EqualTo(new uint4(3, 7, 4, 8)), "the high halves of uint");
            Assert.That(math.unpacklo(g, w), Is.EqualTo(new ulong4(1, 5, 2, 6)), "the low halves of ulong");
            Assert.That(math.unpackhi(g, w), Is.EqualTo(new ulong4(3, 7, 4, 8)), "the high halves of ulong");

            // the short has no register, its components are read one by one
            Assert.That(math.unpacklo(s, t), Is.EqualTo(new short4(1, 5, 2, 6)), "the low halves of short");
            Assert.That(math.unpackhi(s, t), Is.EqualTo(new short4(3, 7, 4, 8)), "the high halves of short");
        }
    }
}
