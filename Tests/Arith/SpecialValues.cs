using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The checks of the special floating point values: the value of a check is a value of the kind of the value it
/// was built from and the bits of a component of it that holds are not all zero.
/// </summary>
public class TestSpecialValues
{
    [Test]
    public void Value()
    {
        var v = new float3(1f, float.NaN, float.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_NaN(v).x != 0f, Is.False);
            Assert.That(math.is_NaN(v).y != 0f, Is.True);
            Assert.That(math.is_NaN(v).z != 0f, Is.False);

            Assert.That(math.is_finite(v).x != 0f, Is.True);
            Assert.That(math.is_finite(v).y != 0f, Is.False);
            Assert.That(math.is_finite(v).z != 0f, Is.False);

            Assert.That(math.is_inf(v).x != 0f, Is.False);
            Assert.That(math.is_inf(v).z != 0f, Is.True);

            Assert.That(math.is_pos_inf(v).z != 0f, Is.True);
            Assert.That(math.is_neg_inf(v).z != 0f, Is.False);
        }

        var n = new float3(float.NegativeInfinity, 0f, -0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_neg_inf(n).x != 0f, Is.True);
            Assert.That(math.is_pos_inf(n).x != 0f, Is.False);
            Assert.That(math.is_inf(n).x != 0f, Is.True);
            // a zero is finite, the sign of it does not matter
            Assert.That(math.is_finite(n).y != 0f, Is.True);
            Assert.That(math.is_finite(n).z != 0f, Is.True);
        }

        var d = new double3(1, double.PositiveInfinity, double.NegativeInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_inf(d).x != 0d, Is.False);
            Assert.That(math.is_pos_inf(d).y != 0d, Is.True);
            Assert.That(math.is_neg_inf(d).z != 0d, Is.True);
        }

        var h = new half3((half)1f, half.NaN, half.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_NaN(h).y != (half)0f, Is.True);
            Assert.That(math.is_inf(h).z != (half)0f, Is.True);
            Assert.That(math.is_finite(h).x != (half)0f, Is.True);
        }
    }

    /// <summary>
    /// The name of a check in HLSL reaches the same member.
    /// </summary>
    [Test]
    public void HlslName()
    {
        var v = new float3(1f, float.NaN, float.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.isnan(v).y != 0f, Is.True);
            Assert.That(math.isinf(v).z != 0f, Is.True);
            Assert.That(math.isfinite(v).x != 0f, Is.True);
        }
    }
}
