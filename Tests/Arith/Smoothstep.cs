using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The smooth interpolation between two bounds is the member of the algebra of the floating point kind: it clamps
/// the position of every component of the value between the bounds into the range of zero and one and turns it
/// into the hermite curve of it, which is the one the <c>smoothstep</c> intrinsic of hlsl computes. The result is
/// zero below the minimum, one above the maximum and a value between them when the component is in the range. The
/// value of a vector that keeps it in a register reaches the member of the register, the value of every other
/// vector reaches the member of the scalar for every component of it and the value of a matrix reaches it for
/// every component of every one of its columns.
/// </summary>
public class TestSmoothstep
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T min, T max, T value)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>
    {
        var r = math.smoothstep(min, max, value);
        Assert.That(value.smoothstep(min, max), Is.EqualTo(r));
        // the member is the curve of the position of the value between the bounds, which the clamping of it into
        // the range of zero and one is
        var t = math.saturate((value - min) / (max - min));
        Assert.That(r, Is.EqualTo(t * t * math.fnma(T.Two, t, T.Three)));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(0f, 0f), new float2(1f, 3f), new float2(0.5f, 1.5f));
        Check(new float3(0f, 0f, 0f), new float3(1f, 3f, 6f), new float3(0.5f, 1.5f, 3f));
        Check(new float4(0f, 0f, 0f, 0f), new float4(1f, 3f, 6f, 1f), new float4(0.5f, 1.5f, 3f, 0.25f));
        Check(new double2(0, 0), new double2(1, 3), new double2(0.5, 1.5));
        Check(new double3(0, 0, 0), new double3(1, 3, 6), new double3(0.5, 1.5, 3));
        Check(new double4(0, 0, 0, 0), new double4(1, 3, 6, 1), new double4(0.5, 1.5, 3, 0.25));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)0f, (half)0f), new half2((half)1f, (half)3f), new half2((half)0.5f, (half)1.5f));
        Check(new half3((half)0f, (half)0f, (half)0f), new half3((half)1f, (half)3f, (half)6f),
            new half3((half)0.5f, (half)1.5f, (half)3f));
        Check(new float2s(0f, 0f), new float2s(1f, 3f), new float2s(0.5f, 1.5f));
        Check(new float3s(0f, 0f, 0f), new float3s(1f, 3f, 6f), new float3s(0.5f, 1.5f, 3f));
        Check(new double3s(0, 0, 0), new double3s(1, 3, 6), new double3s(0.5, 1.5, 3));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(0f), new float2(0f)), new float2x2(new float2(1f), new float2(3f)),
            new float2x2(new float2(0.5f), new float2(1.5f)));
        Check(new float2x2s(new float2s(0f), new float2s(0f)), new float2x2s(new float2s(1f), new float2s(3f)),
            new float2x2s(new float2s(0.5f), new float2s(1.5f)));
        Check(new double2x2(new double2(0), new double2(0)), new double2x2(new double2(1), new double2(3)),
            new double2x2(new double2(0.5), new double2(1.5)));
        Check(new half3x3(new half3((half)0f), new half3((half)0f), new half3((half)0f)),
            new half3x3(new half3((half)1f), new half3((half)3f), new half3((half)6f)),
            new half3x3(new half3((half)0.5f), new half3((half)1.5f), new half3((half)3f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register
            Assert.That(math.smoothstep(new float4(0f), new float4(1f), new float4(0.5f)),
                Is.EqualTo(new float4(0.5f)));
            // a component outside of the bounds is the nearest bound of it
            Assert.That(math.smoothstep(new float3(1f), new float3(3f), new float3(-1f, 1f, 5f)),
                Is.EqualTo(new float3(0f, 0f, 1f)));
            // the curve of a quarter of the range is 0.15625 and the one of the middle of it is a half
            Assert.That(math.smoothstep(new float3(0f), new float3(1f), new float3(0.25f, 0.5f, 0.75f)),
                Is.EqualTo(new float3(0.15625f, 0.5f, 0.84375f)));
            // the curve of the bounds in the reverse order is the curve of the bounds in their order reversed
            Assert.That(math.smoothstep(new float2(1f), new float2(0f), new float2(0.25f, 0.75f)),
                Is.EqualTo(new float2(0.84375f, 0.15625f)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.smoothstep(new float2s(0f), new float2s(1f), new float2s(0.5f)),
                Is.EqualTo(new float2s(0.5f)));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.smoothstep(new float3s(0f), new float3s(1f), new float3s(0.5f)),
                Is.EqualTo(new float3s(0.5f)));
            Assert.That(math.smoothstep(new half3((half)0f), new half3((half)1f), new half3((half)0.5f)),
                Is.EqualTo(new half3((half)0.5f)));
            Assert.That(math.smoothstep(new double3s(0), new double3s(1), new double3s(0.5)),
                Is.EqualTo(new double3s(0.5d)));

            // the value of a matrix is interpolated through the value of every one of its columns
            Assert.That(math.smoothstep(new float2x2(new float2(0f), new float2(0f)),
                    new float2x2(new float2(1f), new float2(3f)),
                    new float2x2(new float2(0.5f), new float2(1.5f))),
                Is.EqualTo(new float2x2(new float2(0.5f), new float2(0.5f))));
            Assert.That(math.smoothstep(new float2x2s(new float2s(0f), new float2s(0f)),
                    new float2x2s(new float2s(1f), new float2s(3f)),
                    new float2x2s(new float2s(0.5f), new float2s(1.5f))),
                Is.EqualTo(new float2x2s(new float2s(0.5f), new float2s(0.5f))));
            Assert.That(math.smoothstep(new double2x2(new double2(0), new double2(0)),
                    new double2x2(new double2(1), new double2(3)),
                    new double2x2(new double2(0.5), new double2(1.5))),
                Is.EqualTo(new double2x2(new double2(0.5d), new double2(0.5d))));
        }
    }

    /// <summary>
    /// The interpolation of a register and the one of a single component are the same operation, so the value of
    /// a vector that keeps it in a register is the one the member of the component type builds for every
    /// component of it.
    /// </summary>
    [Test]
    public void AgreesWithTheComponentType()
    {
        var f = new float4(0f, 1f, 2f, 3f);

        Assert.That(math.smoothstep(default, new float4(1f), f), Is.EqualTo(new float4(
            math.smoothstep(0f, 1f, f.x), math.smoothstep(0f, 1f, f.y),
            math.smoothstep(0f, 1f, f.z), math.smoothstep(0f, 1f, f.w))));

        var d = new double3(-1, 0.5, 2);

        Assert.That(math.smoothstep(new double3(-2), new double3(2), d), Is.EqualTo(new double3(
            math.smoothstep(-2d, 2d, d.x), math.smoothstep(-2d, 2d, d.y), math.smoothstep(-2d, 2d, d.z))));

        var h = new half3((half)(-1f), (half)0.5f, (half)2f);

        Assert.That(math.smoothstep(new half3((half)(-2f)), new half3((half)2f), h), Is.EqualTo(new half3(
            math.smoothstep((half)(-2f), (half)2f, h.x), math.smoothstep((half)(-2f), (half)2f, h.y),
            math.smoothstep((half)(-2f), (half)2f, h.z))));
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component, the interpolation of the zero of a lane is
    /// the zero of it, so it has to keep its bits at zero.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.smoothstep(new float2(0f), new float2(1f), new float2(1f)).vector.GetElement(2),
                Is.EqualTo(0f));
            Assert.That(math.smoothstep(new float2(0f), new float2(1f), new float2(1f)).vector.GetElement(3),
                Is.EqualTo(0f));
            Assert.That(math.smoothstep(new float3(0f), new float3(1f), new float3(1f)).vector.GetElement(3),
                Is.EqualTo(0f));
            Assert.That(math.smoothstep(new double3(0), new double3(1), new double3(1)).vector.GetElement(3),
                Is.EqualTo(0d));
        }
    }
}
