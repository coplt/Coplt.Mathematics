using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The fractional part of a value is the member of the algebra of the floating point kind: it is the value above
/// the floor of it, so it is the same as the remainder of the division of the value by one and the fraction of a
/// component that is negative is the value of it above the floor as well, which is never negative. The value of
/// a vector that keeps it in a register reaches the member of the register, the value of every other vector
/// reaches the member of the scalar for every component of it and the value of a matrix reaches it for every
/// component of every one of its columns.
/// </summary>
public class TestFrac
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>
    {
        var r = math.frac(v);
        Assert.That(v.frac(), Is.EqualTo(r));
        // the fraction of a value is the value above the floor of it, so the two of them add up to the value
        Assert.That(r + math.floor(v), Is.EqualTo(v));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(1.25f, -1.25f));
        Check(new float3(1.25f, -1.25f, 2.5f));
        Check(new float4(1.25f, -1.25f, 2.5f, -2.5f));
        Check(new double2(1.25, -1.25));
        Check(new double3(1.25, -1.25, 2.5));
        Check(new double4(1.25, -1.25, 2.5, -2.5));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)1.25f, (half)(-1.25f)));
        Check(new half3((half)1.25f, (half)(-1.25f), (half)2.5f));
        Check(new float2s(1.25f, -1.25f));
        Check(new float3s(1.25f, -1.25f, 2.5f));
        Check(new double3s(1.25, -1.25, 2.5));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(1.25f, -1.25f), new float2(2.75f, -2.75f)));
        Check(new float2x2s(new float2s(1.25f, -1.25f), new float2s(2.75f, -2.75f)));
        Check(new float2x3(new float2(1.25f, -1.25f), new float2(2.75f, -2.75f), new float2(0.5f, 4f)));
        Check(new double2x2(new double2(1.25, -1.25), new double2(2.75, -2.75)));
        Check(new half3x3(new half3((half)1.25f, (half)(-1.25f), (half)2.5f),
            new half3((half)(-2.5f), (half)0.5f, (half)4f), new half3((half)1f, (half)(-1f), (half)0f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the member of the register
            Assert.That(math.frac(new float2(1.25f, -1.25f)), Is.EqualTo(new float2(0.25f, 0.75f)));
            Assert.That(math.frac(new float3(1.25f, -1.25f, 2.5f)), Is.EqualTo(new float3(0.25f, 0.75f, 0.5f)));
            Assert.That(math.frac(new float4(1.25f, -1.25f, 2.5f, -2.5f)),
                Is.EqualTo(new float4(0.25f, 0.75f, 0.5f, 0.5f)));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            Assert.That(math.frac(new double2(2.75, -2.75)), Is.EqualTo(new double2(0.75, 0.25)));
            Assert.That(math.frac(new double3(2.75, -2.75, 1.5)), Is.EqualTo(new double3(0.75, 0.25, 0.5)));
            Assert.That(math.frac(new double4(2.75, -2.75, 1.5, -1.5)),
                Is.EqualTo(new double4(0.75, 0.25, 0.5, 0.5)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.frac(new float2s(1.25f, -1.25f)), Is.EqualTo(new float2s(0.25f, 0.75f)));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.frac(new float3s(1.25f, -1.25f, 2.5f)), Is.EqualTo(new float3s(0.25f, 0.75f, 0.5f)));
            Assert.That(math.frac(new half3((half)1.25f, (half)(-1.25f), (half)2.5f)),
                Is.EqualTo(new half3((half)0.25f, (half)0.75f, (half)0.5f)));
            Assert.That(math.frac(new double3s(2.75, -2.75, 1.5)), Is.EqualTo(new double3s(0.75, 0.25, 0.5)));

            // the fraction of a value that is integral is zero
            Assert.That(math.frac(new float3(1f, -2f, 3f)), Is.EqualTo(new float3(0f, 0f, 0f)));
            Assert.That(math.frac(new double2(-1, 2)), Is.EqualTo(new double2(0d, 0d)));

            // the fraction of a value is the value of it above its floor, so it is the remainder of the division
            // of the value by one as well
            var v = new float3(1.25f, -1.25f, 2.5f);
            Assert.That(math.frac(v), Is.EqualTo(math.fmod(v, new float3(1f))));
            // the fraction of a component that is negative is the value of it above the floor, so it is never
            // negative
            Assert.That(math.frac(new float3(-1.25f, -0.5f, -2f)).z, Is.EqualTo(0f));

            // the value of a matrix is reached through the value of every one of its columns
            Assert.That(math.frac(new float2x2(new float2(1.25f, -1.25f), new float2(2.75f, -2.75f))),
                Is.EqualTo(new float2x2(new float2(0.25f, 0.75f), new float2(0.75f, 0.25f))));
            Assert.That(math.frac(new double2x2(new double2(2.75, -2.75), new double2(1.25, -1.25))),
                Is.EqualTo(new double2x2(new double2(0.75, 0.25), new double2(0.25, 0.75))));
        }
    }

    /// <summary>
    /// The rounding of a register and the one of a single component are the same operation, so the value of a
    /// vector that keeps it in a register is the value a vector of the same components without a register has.
    /// </summary>
    [Test]
    public void AgreesWithTheComponentType()
    {
        var soft = math.frac(new float3s(1.25f, -1.25f, 2.75f));
        var hardware = math.frac(new float3(1.25f, -1.25f, 2.75f));
        Assert.That(hardware, Is.EqualTo(new float3(soft.x, soft.y, soft.z)));

        var softDouble = math.frac(new double3s(1.25, -1.25, 2.75));
        var hardwareDouble = math.frac(new double4(1.25, -1.25, 2.75, -2.75));
        Assert.That(new double3(hardwareDouble.x, hardwareDouble.y, hardwareDouble.z),
            Is.EqualTo(new double3(softDouble.x, softDouble.y, softDouble.z)));

        // the member of the scalar is the one of the component type, which rounds the value through the floor of
        // the kind of it
        var v = new float4(1.25f, -1.25f, 2.75f, -2.75f);
        Assert.That(math.frac(v), Is.EqualTo(new float4(
            v.x - float.Floor(v.x), v.y - float.Floor(v.y), v.z - float.Floor(v.z), v.w - float.Floor(v.w))));
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component, the fraction of it has to keep its bits at
    /// zero, which is what the fraction of the zero of a lane is as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.frac(new float2(1.25f, -1.25f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.frac(new float2(1.25f, -1.25f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.frac(new float3(1.25f, -1.25f, 2.5f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.frac(new double3(1.25, -1.25, 2.5)).vector.GetElement(3), Is.EqualTo(0d));
        }
    }
}
