using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The split of a value into its integral part and its fractional part is the member of the algebra of the
/// floating point kind: the integral part of a value is its truncation, so the fractional part of it keeps the
/// sign of the value, which the one of <c>frac</c> does not, because that member is the value above the floor of
/// it. The split of a vector that keeps its value in a register reaches the members of the register, the one of
/// every other vector reaches the member of the scalar for every component of it and the one of a matrix reaches
/// the split of every component of every one of its columns.
/// </summary>
public class TestModf
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>
    {
        var (fraction, integral) = math.modf(v);
        var r = math.modf(v, out var i);
        var (fractionOfValue, integralOfValue) = v.modf();
        Assert.That(r, Is.EqualTo(fraction));
        Assert.That(i, Is.EqualTo(integral));
        Assert.That(v.modf(out var j), Is.EqualTo(r));
        Assert.That(j, Is.EqualTo(integral));
        Assert.That(fractionOfValue, Is.EqualTo(fraction));
        Assert.That(integralOfValue, Is.EqualTo(integral));
        // the fractional part keeps the sign of the value and the integral part is the value above it, so the
        // two of them add up to the value
        Assert.That(fraction + integral, Is.EqualTo(v));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(1.25f, -1.25f));
        Check(new float3(1.25f, -1.25f, 2f));
        Check(new float4(1.25f, -1.25f, 2f, -2.75f));
        Check(new double2(1.25, -1.25));
        Check(new double3(1.25, -1.25, 2));
        Check(new double4(1.25, -1.25, 2, -2.75));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)1.25f, (half)(-1.25f)));
        Check(new half3((half)1.25f, (half)(-1.25f), (half)2f));
        Check(new float2s(1.25f, -1.25f));
        Check(new float3s(1.25f, -1.25f, 2f));
        Check(new double3s(1.25, -1.25, 2));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(1.25f, -1.25f), new float2(2.75f, -2.75f)));
        Check(new float2x2s(new float2s(1.25f, -1.25f), new float2s(2.75f, -2.75f)));
        Check(new float2x3(new float2(1.25f, -1.25f), new float2(2.75f, -2.75f), new float2(0.5f, 4f)));
        Check(new double2x2(new double2(1.25, -1.25), new double2(2.75, -2.75)));
        Check(new half3x3(new half3((half)1.25f, (half)(-1.25f), (half)2f),
            new half3((half)(-2f), (half)0.5f, (half)4f), new half3((half)1f, (half)(-1f), (half)0f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the integral part of a value is its truncation and the fractional part is what is left of it
            var v = new float3(1.25f, -1.25f, 2f);
            var r = math.modf(v, out var i);
            Assert.That(r, Is.EqualTo(new float3(0.25f, -0.25f, 0f)));
            Assert.That(i, Is.EqualTo(new float3(1f, -1f, 2f)));

            // the tuple of the member that takes no part to store the integral part into keeps the order of the
            // fractional part and the integral part, which is the order of the members of it
            var (fraction, integral) = math.modf(v);
            Assert.That(fraction, Is.EqualTo(r));
            Assert.That(integral, Is.EqualTo(i));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            var d = math.modf(new double4(1.25, -1.25, 2, -2.75), out var di);
            Assert.That(d, Is.EqualTo(new double4(0.25, -0.25, 0, -0.75)));
            Assert.That(di, Is.EqualTo(new double4(1, -1, 2, -2)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            var s = math.modf(new float2s(1.25f, -1.25f), out var si);
            Assert.That(s, Is.EqualTo(new float2s(0.25f, -0.25f)));
            Assert.That(si, Is.EqualTo(new float2s(1f, -1f)));

            // a vector without a register reaches the member of the component type for every component
            var h = math.modf(new half3((half)1.25f, (half)(-1.25f), (half)2f), out var hi);
            Assert.That(h, Is.EqualTo(new half3((half)0.25f, (half)(-0.25f), (half)0f)));
            Assert.That(hi, Is.EqualTo(new half3((half)1f, (half)(-1f), (half)2f)));
            var ds = math.modf(new double3s(1.25, -1.25, 2), out var dsi);
            Assert.That(ds, Is.EqualTo(new double3s(0.25, -0.25, 0)));
            Assert.That(dsi, Is.EqualTo(new double3s(1, -1, 2)));

            // the split of a value that is integral is the value itself and the zero of the fractional part
            var integralValue = math.modf(new float3(1f, -2f, 3f), out var integralValuePortion);
            Assert.That(integralValue, Is.EqualTo(new float3(0f, 0f, 0f)));
            Assert.That(integralValuePortion, Is.EqualTo(new float3(1f, -2f, 3f)));

            // the value of a matrix is split through the value of every one of its columns
            var m = math.modf(new float2x2(new float2(1.25f, -1.25f), new float2(2.75f, -2.75f)), out var mi);
            Assert.That(m, Is.EqualTo(new float2x2(new float2(0.25f, -0.25f), new float2(0.75f, -0.75f))));
            Assert.That(mi, Is.EqualTo(new float2x2(new float2(1f, -1f), new float2(2f, -2f))));
            var ms = math.modf(new float2x2s(new float2s(1.25f, -1.25f), new float2s(2.75f, -2.75f)), out var msi);
            Assert.That(ms, Is.EqualTo(new float2x2s(new float2s(0.25f, -0.25f), new float2s(0.75f, -0.75f))));
            Assert.That(msi, Is.EqualTo(new float2x2s(new float2s(1f, -1f), new float2s(2f, -2f))));
        }
    }

    /// <summary>
    /// The fractional part of a value keeps the sign of the value, which the fraction of <c>frac</c> does not,
    /// because that member is the value above the floor of it: the two of them are the same for the components
    /// that are not negative and they differ by one for the components that are.
    /// </summary>
    [Test]
    public void SignedFractionOfTheValue()
    {
        var v = new float3(-1.25f, -2.5f, 1.25f);

        using (Assert.EnterMultipleScope())
        {
            var (fraction, integral) = math.modf(v);
            Assert.That(fraction, Is.EqualTo(new float3(-0.25f, -0.5f, 0.25f)));
            Assert.That(integral, Is.EqualTo(new float3(-1f, -2f, 1f)));
            Assert.That(math.frac(v), Is.EqualTo(new float3(0.75f, 0.5f, 0.25f)));
            // the fraction of a value and the fractional part of it add up to one for a component that is not
            // integral, so the two of them are the one of the value and the one above it
            Assert.That(fraction + math.frac(v), Is.EqualTo(new float3(0.5f, 0f, 0.5f)));
            Assert.That(math.frac(v) - fraction, Is.EqualTo(new float3(1f, 1f, 0f)));
        }
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component and the split of the zero of it is the zero
    /// of the fractional part and the zero of the integral part, so the lanes stay zero.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.modf(new float2(1.25f, -1.25f), out _).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.modf(new float2(1.25f, -1.25f), out _).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.modf(new float3(1.25f, -1.25f, 2f), out _).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.modf(new double3(1.25, -1.25, 2), out _).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(math.modf(new float3(1.25f, -1.25f, 2f), out var i).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(i.vector.GetElement(3), Is.EqualTo(0f));
        }
    }
}
