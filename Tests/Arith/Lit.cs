using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The lighting coefficient vector of the lighting terms of a value is the member of the math class that is the
/// counterpart of the <c>lit</c> intrinsic of hlsl: it is one in the first component, the term of the normal and
/// the light vector clamped below by the zero in the second one, the term of the half angle vector clamped the same
/// way and raised to the specular exponent in the third one, which is the zero of the kind of it when the term of
/// the normal and the light vector is not positive, and one in the fourth one.
/// </summary>
public class TestLit
{
    /// <summary>
    /// The lighting of a surface: the diffuse term of a term of the normal and the light vector that is not
    /// positive is the zero of the kind of it, the specular one is the zero of the kind of it as well when the term
    /// of the half angle vector is not positive, and the term of the half angle vector raised to a large exponent
    /// is a value that a float holds and a half does not.
    /// </summary>
    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // a surface that faces the light and the highlight of it, 0.9 ^ 16 is about 0.185302
            var standard = math.lit(0.8f, 0.9f, 16f);
            Assert.That(standard.x, Is.EqualTo(1f));
            Assert.That(standard.y, Is.EqualTo(0.8f));
            Assert.That(standard.z, Is.EqualTo(0.185302f).Within(1e-6f));
            Assert.That(standard.w, Is.EqualTo(1f));

            // a surface that faces away from the light, so the term of the normal and the light vector is not
            // positive and the diffuse one and the specular one are the zero of the kind of them
            Assert.That(math.lit(-0.5f, 0.9f, 16f), Is.EqualTo(new float4(1f, 0f, 0f, 1f)));

            // a surface that is perpendicular to the light, which is the boundary of the term of it
            Assert.That(math.lit(0f, 0.8f, 32f), Is.EqualTo(new float4(1f, 0f, 0f, 1f)));

            // a surface whose highlight faces away from it, so the term of the half angle vector is not positive
            // and the specular one is the zero of the kind of it
            Assert.That(math.lit(0.5f, -0.2f, 16f), Is.EqualTo(new float4(1f, 0.5f, 0f, 1f)));

            // a surface that faces the light and the highlight of it directly, so the specular one is the peak
            Assert.That(math.lit(1f, 1f, 64f), Is.EqualTo(new float4(1f, 1f, 1f, 1f)));

            // 0.8 ^ 128 is about 2.87e-13, which is a value that a float holds, so the specular term of it is not
            // the zero of the kind of it
            var tight = math.lit(0.6f, 0.8f, 128f);
            Assert.That(tight.x, Is.EqualTo(1f));
            Assert.That(tight.y, Is.EqualTo(0.6f));
            Assert.That(tight.z, Is.EqualTo(0f).Within(1e-12f));
            Assert.That(tight.w, Is.EqualTo(1f));

            // the exponent of zero keeps the whole term of the half angle vector
            Assert.That(math.lit(0.5f, 0.5f, 0f), Is.EqualTo(new float4(1f, 0.5f, 1f, 1f)));
        }
    }

    /// <summary>
    /// The same lighting of a surface for the other kinds of a floating point number: the term of the half angle
    /// vector raised to an exponent that is large enough is below the smallest value a half holds, so it is the
    /// zero of the kind of it for a half.
    /// </summary>
    [Test]
    public void Kinds()
    {
        using (Assert.EnterMultipleScope())
        {
            // 0.9 ^ 16 is about 0.1853020188851841 for a double
            var standard = math.lit(0.8, 0.9, 16d);
            Assert.That((standard.x, standard.y, standard.w), Is.EqualTo((1d, 0.8d, 1d)));
            Assert.That(standard.z, Is.EqualTo(0.1853020188851841d).Within(1e-15d));

            // the term of the half angle vector of an exponent that keeps it above the smallest value of a half
            Assert.That(math.lit((Half)0.5f, (Half)0.5f, (Half)2f),
                Is.EqualTo(new half4(Half.One, (Half)0.5f, (Half)0.25f, Half.One)));

            // 0.8 ^ 128 is about 2.87e-13, which the half holds as the zero of the kind of it
            Assert.That(math.lit((Half)0.6f, (Half)0.8f, (Half)128f),
                Is.EqualTo(new half4(Half.One, (Half)0.6f, Half.Zero, Half.One)));
        }
    }

    /// <summary>
    /// The two terms of the lighting of the vector are clamped below by the zero of the kind of them, so the power
    /// of the term of the half angle vector does not reach the power of a negative value, which is not a number for
    /// an exponent that is not a whole number.
    /// </summary>
    [Test]
    public void Terms()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.lit(1f, -1f, 0.5f), Is.EqualTo(new float4(1f, 1f, 0f, 1f)));
            Assert.That(math.lit(1f, -0.25f, 3f), Is.EqualTo(new float4(1f, 1f, 0f, 1f)));
            Assert.That(math.lit(1d, -0.25, 3.5), Is.EqualTo(new double4(1d, 1d, 0d, 1d)));
            Assert.That(math.lit((Half)1f, (Half)(-0.25f), (Half)3.5f),
                Is.EqualTo(new half4(Half.One, Half.One, Half.Zero, Half.One)));
        }
    }
}
