using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The clamping of a value into the range of zero and one is the member of the algebra of the floating point
/// kind: it is the clamp of the value between the zero of the kind of it and the one of it, so it is the member
/// of the algebra of a number and the value of every kind of a floating point number reaches it. The value of a
/// vector that keeps it in a register reaches the member of the register, the value of every other vector reaches
/// the member of the scalar for every component of it and the value of a matrix reaches it for every component of
/// every one of its columns.
/// </summary>
public class TestSaturate
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebra<T>, INumberAlgebraDispatch<T>
    {
        var r = math.saturate(v);
        Assert.That(v.saturate(), Is.EqualTo(r));
        // the range of the clamping is the one of the zero and the one of the kind of the value, so the value
        // of a component that is outside of it is the nearest bound of it
        Assert.That(r, Is.EqualTo(math.clamp(v, T.Zero, T.One)));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(-1f, 0.5f));
        Check(new float3(-1f, 0.5f, 2f));
        Check(new float4(-1f, 0.5f, 2f, 0f));
        Check(new double2(-1, 0.5));
        Check(new double3(-1, 0.5, 2));
        Check(new double4(-1, 0.5, 2, 0));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)(-1f), (half)0.5f));
        Check(new half3((half)(-1f), (half)0.5f, (half)2f));
        Check(new float2s(-1f, 0.5f));
        Check(new float3s(-1f, 0.5f, 2f));
        Check(new double3s(-1, 0.5, 2));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(-1f, 0.5f), new float2(2f, 0f)));
        Check(new float2x2s(new float2s(-1f, 0.5f), new float2s(2f, 0f)));
        Check(new float2x3(new float2(-1f, 0.5f), new float2(2f, 0f), new float2(1f, -2f)));
        Check(new double2x2(new double2(-1, 0.5), new double2(2, 0)));
        Check(new half3x3(new half3((half)(-1f), (half)0.5f, (half)2f),
            new half3((half)0f, (half)1f, (half)(-2f)), new half3((half)0.25f, (half)0.75f, (half)0.5f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the member of the register
            Assert.That(math.saturate(new float2(-1f, 0.5f)), Is.EqualTo(new float2(0f, 0.5f)));
            Assert.That(math.saturate(new float3(-1f, 0.5f, 2f)), Is.EqualTo(new float3(0f, 0.5f, 1f)));
            Assert.That(math.saturate(new float4(-1f, 0.5f, 2f, 0f)), Is.EqualTo(new float4(0f, 0.5f, 1f, 0f)));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            Assert.That(math.saturate(new double2(-1, 0.5)), Is.EqualTo(new double2(0d, 0.5d)));
            Assert.That(math.saturate(new double3(-1, 0.5, 2)), Is.EqualTo(new double3(0d, 0.5d, 1d)));
            Assert.That(math.saturate(new double4(-1, 0.5, 2, 0)), Is.EqualTo(new double4(0d, 0.5d, 1d, 0d)));

            // a value that is outside of the range of a component is the nearest bound of it, which the
            // infinity of the kind of it is as well
            Assert.That(math.saturate(new float3(float.NegativeInfinity, float.PositiveInfinity, -0.5f)),
                Is.EqualTo(new float3(0f, 1f, 0f)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.saturate(new float2s(-1f, 0.5f)), Is.EqualTo(new float2s(0f, 0.5f)));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.saturate(new float3s(-1f, 0.5f, 2f)), Is.EqualTo(new float3s(0f, 0.5f, 1f)));
            Assert.That(math.saturate(new half3((half)(-1f), (half)0.5f, (half)2f)),
                Is.EqualTo(new half3((half)0f, (half)0.5f, (half)1f)));
            Assert.That(math.saturate(new double3s(-1, 0.5, 2)), Is.EqualTo(new double3s(0d, 0.5d, 1d)));

            // the value of a matrix is clamped through the value of every one of its columns
            Assert.That(math.saturate(new float2x2(new float2(-1f, 0.5f), new float2(2f, 0f))),
                Is.EqualTo(new float2x2(new float2(0f, 0.5f), new float2(1f, 0f))));
            Assert.That(math.saturate(new float2x2s(new float2s(-1f, 0.5f), new float2s(2f, 0f))),
                Is.EqualTo(new float2x2s(new float2s(0f, 0.5f), new float2s(1f, 0f))));
            Assert.That(math.saturate(new double2x2(new double2(-1, 0.5), new double2(2, 0))),
                Is.EqualTo(new double2x2(new double2(0d, 0.5d), new double2(1d, 0d))));
        }
    }

    /// <summary>
    /// The clamping of a register and the one of a single component are the same operation, so the value of a
    /// vector that keeps it in a register is the value the member of the component type builds for every
    /// component of it.
    /// </summary>
    [Test]
    public void AgreesWithTheComponentType()
    {
        var v = new float4(-1f, 0.5f, 2f, 0f);

        Assert.That(math.saturate(v), Is.EqualTo(new float4(
            float.Clamp(v.x, 0f, 1f), float.Clamp(v.y, 0f, 1f), float.Clamp(v.z, 0f, 1f), float.Clamp(v.w, 0f, 1f))));

        var d = new double4(-1, 0.5, 2, 0);

        Assert.That(math.saturate(d), Is.EqualTo(new double4(
            double.Clamp(d.x, 0d, 1d), double.Clamp(d.y, 0d, 1d), double.Clamp(d.z, 0d, 1d),
            double.Clamp(d.w, 0d, 1d))));

        var h = new half3((half)(-1f), (half)0.5f, (half)2f);

        Assert.That(math.saturate(h), Is.EqualTo(new half3(
            half.Clamp(h.x, (half)0f, (half)1f), half.Clamp(h.y, (half)0f, (half)1f),
            half.Clamp(h.z, (half)0f, (half)1f))));
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component, the clamping of it has to keep its bits at
    /// zero, which is what the clamping of the zero of a lane is as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.saturate(new float2(-1f, 0.5f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.saturate(new float2(-1f, 0.5f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.saturate(new float3(-1f, 0.5f, 2f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.saturate(new double3(-1, 0.5, 2)).vector.GetElement(3), Is.EqualTo(0d));
        }
    }
}
