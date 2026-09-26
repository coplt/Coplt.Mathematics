using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The conversion of an angle is the member of the algebra of the floating point kind: it multiplies every
/// component of the value by the factor of the kind of it, which is <c>DegToRad</c> for the conversion from
/// degrees and <c>RadToDeg</c> for the one from radians. The value of a vector that keeps it in a register
/// reaches the member of the register, the value of every other vector reaches the member of the scalar for every
/// component of it and the value of a matrix reaches it through the value of every one of its columns.
/// </summary>
public class TestRadiansDegrees
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebra<T>
    {
        Assert.That(v.radians(), Is.EqualTo(math.radians(v)));
        Assert.That(v.degrees(), Is.EqualTo(math.degrees(v)));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(180f, 90f));
        Check(new float3(180f, 90f, 45f));
        Check(new float4(180f, 90f, 45f, 0f));
        Check(new double2(180, 90));
        Check(new double3(180, 90, 45));
        Check(new double4(180, 90, 45, 0));
        // a vector without a register reaches the member of the component type for every component
        Check(new half2((half)180f, (half)90f));
        Check(new half3((half)180f, (half)90f, (half)45f));
        Check(new float2s(180f, 90f));
        Check(new float3s(180f, 90f, 45f));
        Check(new double3s(180, 90, 45));
        // a matrix reaches the member of the kind of it through the value of every one of its columns
        Check(new float2x2(new float2(180f, 90f), new float2(45f, 0f)));
        Check(new float2x2s(new float2s(180f, 90f), new float2s(45f, 0f)));
        Check(new double2x2(new double2(180, 90), new double2(45, 0)));
        Check(new half3x3(new half3((half)180f, (half)90f, (half)45f), new half3((half)0f, (half)1f, (half)2f),
            new half3((half)3f, (half)4f, (half)5f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the conversion of a value that keeps it in a register
            Assert.That(math.radians(new float3(180f, 90f, 0f)).x, Is.EqualTo(MathF.PI).Within(1e-5f));
            Assert.That(math.radians(new float3(180f, 90f, 0f)).y, Is.EqualTo(MathF.PI / 2f).Within(1e-6f));
            Assert.That(math.degrees(new float3(MathF.PI, MathF.PI / 2f, 0f)).x, Is.EqualTo(180f).Within(1e-3f));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            Assert.That(math.radians(new double4(180d, 90d, 45d, 0d)).x, Is.EqualTo(Math.PI).Within(1e-12));
            Assert.That(math.degrees(new double4(Math.PI, Math.PI / 2d, Math.PI / 4d, 0d)).x,
                Is.EqualTo(180d).Within(1e-12));

            // a conversion and the one of the other way around are the value again
            Assert.That(new float3(180f, 90f, 45f).radians().degrees().x, Is.EqualTo(180f).Within(1e-3f));
            Assert.That(new double2(360, 0).radians().degrees().x, Is.EqualTo(360d).Within(1e-9));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.radians(new float2s(180f, 90f)).x, Is.EqualTo(MathF.PI).Within(1e-5f));
            Assert.That(math.degrees(new float2s(MathF.PI, MathF.PI / 2f)).x, Is.EqualTo(180f).Within(1e-3f));

            // a vector without a register reaches the member of the component type for every component
            Assert.That((float)math.radians(new half2((half)180f, (half)90f)).x, Is.EqualTo(MathF.PI).Within(1e-2f));
            Assert.That(math.radians(new float3s(180f, 90f, 45f)).x, Is.EqualTo(MathF.PI).Within(1e-5f));
            Assert.That(math.radians(new double3s(180, 90, 45)).x, Is.EqualTo(Math.PI).Within(1e-12));

            // the value of a matrix is converted through the value of every one of its columns
            Assert.That(math.radians(new float2x2(new float2(180f, 90f), new float2(0f, 0f))).c0.x,
                Is.EqualTo(MathF.PI).Within(1e-5f));
            Assert.That(math.radians(new float2x2s(new float2s(180f, 90f), new float2s(0f, 0f))).c0.x,
                Is.EqualTo(MathF.PI).Within(1e-5f));
        }
    }

    /// <summary>
    /// The conversion of a register and the one of a single component are the same operation, so the value of a
    /// vector that keeps it in a register is the one the member of the component type builds for every component
    /// of it.
    /// </summary>
    [Test]
    public void AgreesWithTheComponentType()
    {
        var f = new float4(180f, 90f, 45f, 0f);

        Assert.That(math.radians(f), Is.EqualTo(new float4(
            math.radians(f.x), math.radians(f.y), math.radians(f.z), math.radians(f.w))));

        var d = new double3(180, 90, 45);

        Assert.That(math.radians(d), Is.EqualTo(new double3(
            math.radians(d.x), math.radians(d.y), math.radians(d.z))));
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component, the conversion of the zero of a lane is the
    /// zero of it, so it has to keep its bits at zero.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.radians(new float2(180f, 90f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.radians(new float2(180f, 90f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.radians(new float3(180f, 90f, 45f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.degrees(new double3(180, 90, 45)).vector.GetElement(3), Is.EqualTo(0d));
        }
    }
}
