using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The remainder of the division of a value is the member of the algebra of the ieee 754 kind, which every floating
/// point type of the library names, a half as well: it is the difference of the value and the product of the divisor
/// and the floor of the quotient of the two, so the remainder of a divisor that is negative keeps the sign of the
/// divisor. The value of a vector that keeps it in a register reaches the member of the visitor that matches the
/// width of the register, which fuses the product into the difference, the value of every other vector reaches the
/// member of the scalar for every component of it and the value of a matrix reaches it for every component of every
/// one of its columns. The remainder of a padding lane is the one of the zero of it by the zero of the divisor, which
/// is not a number, so the register of the value keeps the padding lanes of it at zero.
/// </summary>
public class TestFmod
{
    /// <summary>
    /// The member is the one of the algebra of the ieee 754 kind, so a parameter that only knows the interface
    /// reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T a, T b)
        where T : unmanaged, IFloatingPointIeee754AlgebraDispatch<T>
    {
        var r = math.fmod(a, b);
        Assert.That(a.fmod(b), Is.EqualTo(r));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(5.5f, -5.5f), new float2(2f, 2f));
        Check(new float3(5.5f, -5.5f, 1.25f), new float3(2f, 2f, 0.5f));
        Check(new float4(5.5f, -5.5f, 1.25f, -1.25f), new float4(2f, 2f, 0.5f, 0.5f));
        Check(new double2(5.5, -5.5), new double2(2, 2));
        Check(new double3(5.5, -5.5, 1.25), new double3(2, 2, 0.5));
        Check(new double4(5.5, -5.5, 1.25, -1.25), new double4(2, 2, 0.5, 0.5));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)5.5f, (half)(-5.5f)), new half2((half)2f, (half)2f));
        Check(new half3((half)5.5f, (half)(-5.5f), (half)1.25f), new half3((half)2f, (half)2f, (half)0.5f));
        Check(new float2s(5.5f, -5.5f), new float2s(2f, 2f));
        Check(new float3s(5.5f, -5.5f, 1.25f), new float3s(2f, 2f, 0.5f));
        Check(new double3s(5.5, -5.5, 1.25), new double3s(2, 2, 0.5));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(5.5f, -5.5f), new float2(1.25f, -1.25f)),
            new float2x2(new float2(2f, 2f), new float2(0.5f, 0.5f)));
        Check(new float2x2s(new float2s(5.5f, -5.5f), new float2s(1.25f, -1.25f)),
            new float2x2s(new float2s(2f, 2f), new float2s(0.5f, 0.5f)));
        Check(new float2x3(new float2(5.5f, -5.5f), new float2(1.25f, -1.25f), new float2(2f, 0.5f)),
            new float2x3(new float2(2f, 2f), new float2(0.5f, 0.5f), new float2(1f, 1f)));
        Check(new double2x2(new double2(5.5, -5.5), new double2(1.25, -1.25)),
            new double2x2(new double2(2, 2), new double2(0.5, 0.5)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the member of the register
            Assert.That(math.fmod(new float3(5.5f, -5.5f, 1.25f), new float3(2f, 2f, 0.5f)),
                Is.EqualTo(new float3(1.5f, 0.5f, 0.25f)));
            Assert.That(math.fmod(new float4(5.5f, -5.5f, 1.25f, -1.25f), new float4(2f, 2f, 0.5f, 0.5f)),
                Is.EqualTo(new float4(1.5f, 0.5f, 0.25f, 0.25f)));

            // the quotient of the division is floored, so the remainder of a divisor that is negative keeps the
            // sign of the divisor
            Assert.That(math.fmod(new float3(5.5f, -5.5f, 5.5f), new float3(2f, 2f, -2f)),
                Is.EqualTo(new float3(1.5f, 0.5f, -0.5f)));

            // the remainder of the division of a value by one is the fraction of it
            Assert.That(math.fmod(new float3(1.25f, -1.25f, 2f), new float3(1f)), Is.EqualTo(new float3(0.25f, 0.75f, 0f)));
            Assert.That(math.fmod(new float3(1.4f, -1.5f, 2.6f), new float3(1f)).x, Is.EqualTo(0.4f).Within(1e-6f));
            Assert.That(math.fmod(new float3(1.4f, -1.5f, 2.6f), new float3(1f)).y, Is.EqualTo(0.5f));
            Assert.That(math.fmod(new float3(1.4f, -1.5f, 2.6f), new float3(1f)).z, Is.EqualTo(0.6f).Within(1e-6f));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            Assert.That(math.fmod(new double2(5.5, -5.5), new double2(2, 2)), Is.EqualTo(new double2(1.5, 0.5)));
            Assert.That(math.fmod(new double3(5.5, -5.5, 1.25), new double3(2, 2, 0.5)),
                Is.EqualTo(new double3(1.5, 0.5, 0.25)));
            Assert.That(math.fmod(new double4(5.5, -5.5, 1.25, -1.25), new double4(2, 2, 0.5, 0.5)),
                Is.EqualTo(new double4(1.5, 0.5, 0.25, 0.25)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.fmod(new float2s(5.5f, -5.5f), new float2s(2f, 2f)), Is.EqualTo(new float2s(1.5f, 0.5f)));

            // a vector without a register reaches the member of the component type for every component, which a
            // value of half components is one of as well
            Assert.That(math.fmod(new half3((half)5.5f, (half)(-5.5f), (half)1.25f),
                    new half3((half)2f, (half)2f, (half)0.5f)),
                Is.EqualTo(new half3((half)1.5f, (half)0.5f, (half)0.25f)));
            Assert.That(math.fmod(new float3s(5.5f, -5.5f, 1.25f), new float3s(2f, 2f, 0.5f)),
                Is.EqualTo(new float3s(1.5f, 0.5f, 0.25f)));
            Assert.That(math.fmod(new float3s(1.4f, -1.5f, 2.6f), new float3s(1f)).z, Is.EqualTo(0.6f).Within(1e-6f));
            Assert.That(math.fmod(new double3s(5.5, -5.5, 1.25), new double3s(2, 2, 0.5)),
                Is.EqualTo(new double3s(1.5, 0.5, 0.25)));

            // the value of a matrix is reached through the value of every one of its columns
            Assert.That(
                math.fmod(new float2x2(new float2(5.5f, -5.5f), new float2(1.25f, -1.25f)),
                    new float2x2(new float2(2f, 2f), new float2(0.5f, 0.5f))),
                Is.EqualTo(new float2x2(new float2(1.5f, 0.5f), new float2(0.25f, 0.25f))));
            Assert.That(
                math.fmod(new float2x2s(new float2s(5.5f, -5.5f), new float2s(1.25f, -1.25f)),
                    new float2x2s(new float2s(2f, 2f), new float2s(0.5f, 0.5f))),
                Is.EqualTo(new float2x2s(new float2s(1.5f, 0.5f), new float2s(0.25f, 0.25f))));
            Assert.That(
                math.fmod(new double2x2(new double2(5.5, -5.5), new double2(1.25, -1.25)),
                    new double2x2(new double2(2, 2), new double2(0.5, 0.5))),
                Is.EqualTo(new double2x2(new double2(1.5, 0.5), new double2(0.25, 0.25))));
        }
    }

    /// <summary>
    /// The remainder of the division of a value by one is the fraction of it, which the member of <c>frac</c>
    /// reaches as the value above the floor of it.
    /// </summary>
    [Test]
    public void IsTheFractionOfTheValue()
    {
        using (Assert.EnterMultipleScope())
        {
            var v = new float3(1.25f, -1.25f, 2f);
            Assert.That(math.fmod(v, new float3(1f)), Is.EqualTo(math.frac(v)));

            var d = new double2(2.75, -2.75);
            Assert.That(math.fmod(d, new double2(1)), Is.EqualTo(math.frac(d)));
        }
    }

    /// <summary>
    /// The remainder of a padding lane of a register is the one of the zero of it by the zero of the divisor,
    /// which is not a number, so the member has to mask the register of the value to keep its bits at zero.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.fmod(new float2(5.5f, -5.5f), new float2(2f, 2f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.fmod(new float2(5.5f, -5.5f), new float2(2f, 2f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.fmod(new float3(5.5f, -5.5f, 1.25f), new float3(2f, 2f, 0.5f)).vector.GetElement(3),
                Is.EqualTo(0f));
            Assert.That(math.fmod(new double3(5.5, -5.5, 1.25), new double3(2, 2, 0.5)).vector.GetElement(3),
                Is.EqualTo(0d));
        }
    }
}
