using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rounding of a value to the nearest integer is the member of the algebra of the floating point kind: the
/// value of a vector that keeps it in a register reaches the member of the visitor that matches the width of the
/// register, which rounds it with the simd member of the hardware, the value of every other vector reaches the
/// member of the scalar for every component of it and the value of a matrix reaches it for every component of
/// every one of its columns. The member of the hardware and the member of the component type round a value that
/// is exactly between two integers to the even one of them, so both of them agree on the result. The value of a
/// padding lane of a register holds no component and stays zero, which is what the rounding of it is as well.
/// </summary>
public class TestRound
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>
    {
        var r = math.round(v);
        Assert.That(v.round(), Is.EqualTo(r));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(1.4f, -1.5f));
        Check(new float3(1.4f, -1.5f, 2.6f));
        Check(new float4(1.4f, -1.5f, 2.6f, -0.5f));
        Check(new double2(1.4, -1.5));
        Check(new double3(1.4, -1.5, 2.6));
        Check(new double4(1.4, -1.5, 2.6, -0.5));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)1.4f, (half)(-1.5f)));
        Check(new half3((half)1.4f, (half)(-1.5f), (half)2.6f));
        Check(new float2s(1.4f, -1.5f));
        Check(new float3s(1.4f, -1.5f, 2.6f));
        Check(new double3s(1.4, -1.5, 2.6));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(1.4f, -1.5f), new float2(2.6f, -0.5f)));
        Check(new float2x2s(new float2s(1.4f, -1.5f), new float2s(2.6f, -0.5f)));
        Check(new float2x3(new float2(1.4f, -1.5f), new float2(2.6f, -0.5f), new float2(0.4f, 3.5f)));
        Check(new double2x2(new double2(1.4, -1.5), new double2(2.6, -0.5)));
        Check(new half3x3(new half3((half)1.4f, (half)(-1.5f), (half)2.6f),
            new half3((half)(-0.5f), (half)0.4f, (half)3.5f), new half3((half)1f, (half)(-1f), (half)0f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the simd member of the hardware
            Assert.That(math.round(new float2(1.4f, -1.5f)), Is.EqualTo(new float2(1f, -2f)));
            Assert.That(math.round(new float3(1.4f, -1.5f, 2.6f)), Is.EqualTo(new float3(1f, -2f, 3f)));
            Assert.That(math.round(new float4(1.4f, -1.5f, 2.6f, -0.5f)), Is.EqualTo(new float4(1f, -2f, 3f, 0f)));

            // a value that is exactly between two integers goes to the even one of them, the member of the
            // hardware and the one of the component type agree on it
            Assert.That(math.round(new float3(0.5f, 1.5f, 2.5f)), Is.EqualTo(new float3(0f, 2f, 2f)));
            Assert.That(math.round(new float3(-0.5f, -1.5f, 3.5f)), Is.EqualTo(new float3(0f, -2f, 4f)));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            Assert.That(math.round(new double2(1.4, -1.5)), Is.EqualTo(new double2(1d, -2d)));
            Assert.That(math.round(new double3(1.4, -1.5, 2.5)), Is.EqualTo(new double3(1d, -2d, 2d)));
            Assert.That(math.round(new double4(1.4, -1.5, 2.5, -0.5)), Is.EqualTo(new double4(1d, -2d, 2d, 0d)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.round(new float2s(1.4f, -1.5f)), Is.EqualTo(new float2s(1f, -2f)));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.round(new float3s(1.4f, -1.5f, 2.6f)), Is.EqualTo(new float3s(1f, -2f, 3f)));
            Assert.That(math.round(new half3((half)1.5f, (half)(-1.5f), (half)2.5f)),
                Is.EqualTo(new half3((half)2f, (half)(-2f), (half)2f)));
            Assert.That(math.round(new double3s(1.4, -1.5, 2.5)), Is.EqualTo(new double3s(1d, -2d, 2d)));

            // a rounding of a value that is already integral is the value itself
            Assert.That(math.round(new float3(1f, -2f, 3f)), Is.EqualTo(new float3(1f, -2f, 3f)));

            // the value of a matrix is rounded through the value of every one of its columns
            Assert.That(math.round(new float2x2(new float2(1.4f, -1.5f), new float2(2.6f, -0.5f))),
                Is.EqualTo(new float2x2(new float2(1f, -2f), new float2(3f, 0f))));
            Assert.That(math.round(new float2x2s(new float2s(1.4f, -1.5f), new float2s(2.6f, -0.5f))),
                Is.EqualTo(new float2x2s(new float2s(1f, -2f), new float2s(3f, 0f))));
            Assert.That(math.round(new double2x2(new double2(1.4, -1.5), new double2(2.6, -0.5))),
                Is.EqualTo(new double2x2(new double2(1d, -2d), new double2(3d, 0d))));
        }
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component, the rounding of it has to keep its bits at
    /// zero, which is what the rounding of the zero of a lane is as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.round(new float2(1.4f, -1.5f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.round(new float2(1.4f, -1.5f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.round(new float3(1.4f, -1.5f, 2.6f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.round(new double3(1.4, -1.5, 2.5)).vector.GetElement(3), Is.EqualTo(0d));
        }
    }
}
