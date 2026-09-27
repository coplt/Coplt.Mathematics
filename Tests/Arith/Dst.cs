using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The distance vector of two vectors is the member of the math class that is the counterpart of the <c>dst</c>
/// intrinsic of hlsl: it is one in the first component, the product of the second components of the two in the
/// second one, the third component of the first vector in the third one and the fourth component of the second one
/// in the fourth one, so the other components of the two do not reach the result.
/// </summary>
public class TestDst
{
    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.dst(new float4(1f, 2f, 3f, 4f), new float4(5f, 6f, 7f, 8f)),
                Is.EqualTo(new float4(1f, 12f, 3f, 8f)));
            Assert.That(math.dst(new double4(1, 2, 3, 4), new double4(5, 6, 7, 8)),
                Is.EqualTo(new double4(1d, 12d, 3d, 8d)));
            Assert.That(
                math.dst(new half4((Half)1f, (Half)2f, (Half)3f, (Half)4f),
                    new half4((Half)5f, (Half)6f, (Half)7f, (Half)8f)),
                Is.EqualTo(new half4(Half.One, (Half)12f, (Half)3f, (Half)8f)));

            // the second component of the result is the product of the two, which is negative for a vector that
            // is negative itself
            Assert.That(math.dst(new float4(9f, -2f, -3f, -4f), new float4(9f, 3f, -7f, -8f)),
                Is.EqualTo(new float4(1f, -6f, -3f, -8f)));

            // the first component is one even for the zero vector
            Assert.That(math.dst(default(float4), default(float4)), Is.EqualTo(new float4(1f, 0f, 0f, 0f)));
            Assert.That(math.dst(default(double4), default(double4)), Is.EqualTo(new double4(1d, 0d, 0d, 0d)));
        }
    }

    /// <summary>
    /// The vector reaches the second component of the two and the third one of the first vector and the fourth one
    /// of the second: the first component of the result is one and the other ones are the components of the
    /// vectors that are handed over, which the other components of them do not reach.
    /// </summary>
    [Test]
    public void IgnoredComponents()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.dst(new float4(100f, 2f, 3f, 4f), new float4(-100f, 6f, -7f, 8f)),
                Is.EqualTo(new float4(1f, 12f, 3f, 8f)));
            Assert.That(
                math.dst(new half4((Half)100f, (Half)2f, (Half)3f, (Half)4f),
                    new half4((Half)(-100f), (Half)6f, (Half)(-7f), (Half)8f)),
                Is.EqualTo(new half4(Half.One, (Half)12f, (Half)3f, (Half)8f)));
        }
    }
}
