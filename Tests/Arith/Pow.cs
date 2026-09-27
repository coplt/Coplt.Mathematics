using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The power of a value is the member of the algebra of the floating point kind: the value of a vector that keeps
/// it in a register reaches the member of the visitor that matches the width of the register, which is the one of
/// the simd library and is built from the logarithm and the exponential of the value, the value of every other
/// vector reaches the member of the scalar for every component of it and the value of a matrix reaches it for
/// every component of every one of its columns. The power of the zero of a padding lane is the one of the kind of
/// the component when the exponent lane is zero as well, so the register of the result is masked.
/// </summary>
public class TestPow
{
    /// <summary>
    /// The member of the simd library of a register is built from a logarithm and an exponential, which are not
    /// exact, so the value of a register is asserted within this tolerance. The member of the scalar is the one of
    /// the component type, which is exact.
    /// </summary>
    private const float Tolerance = 0.001f;

    /// <inheritdoc cref="Tolerance"/>
    private const double DoubleTolerance = 1e-9;

    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>
    {
        _ = math.pow(v, v);
    }

    [Test]
    public void Interface()
    {
        Check(new float2(2f, 3f));
        Check(new float3(2f, 3f, 4f));
        Check(new float4(2f, 3f, 4f, 5f));
        Check(new double2(2, 3));
        Check(new double3(2, 3, 4));
        Check(new double4(2, 3, 4, 5));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)2f, (half)3f));
        Check(new half3((half)2f, (half)3f, (half)4f));
        Check(new float2s(2f, 3f));
        Check(new float3s(2f, 3f, 4f));
        Check(new double3s(2, 3, 4));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(2f, 3f), new float2(4f, 5f)));
        Check(new float2x2s(new float2s(2f, 3f), new float2s(4f, 5f)));
        Check(new double2x2(new double2(2, 3), new double2(4, 5)));
        Check(new half3x3(new half3((half)2f, (half)3f, (half)4f),
            new half3((half)5f, (half)6f, (half)7f), new half3((half)8f, (half)9f, (half)10f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the member of the register
            var f3 = math.pow(new float3(2f, 3f, 4f), new float3(3f, 2f, 2f));
            Assert.That(f3.x, Is.EqualTo(8f).Within(Tolerance));
            Assert.That(f3.y, Is.EqualTo(9f).Within(Tolerance));
            Assert.That(f3.z, Is.EqualTo(16f).Within(Tolerance));

            // a large exponent keeps the accuracy of the kind of the value
            Assert.That(math.pow(new float3(2f), new float3(10f)).x, Is.EqualTo(1024f).Within(1f));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            var d3 = math.pow(new double3(2, 3, 4), new double3(3, 2, 2));
            Assert.That(d3.x, Is.EqualTo(8d).Within(DoubleTolerance));
            Assert.That(d3.y, Is.EqualTo(9d).Within(DoubleTolerance));
            Assert.That(d3.z, Is.EqualTo(16d).Within(DoubleTolerance));

            // the exponent of a component can be the one of another component, and it can be negative and
            // fractional
            Assert.That(math.pow(new float3(4f, 4f, 4f), new float3(0.5f, -1f, -0.5f)).x,
                Is.EqualTo(2f).Within(Tolerance));
            Assert.That(math.pow(new float3(4f, 4f, 4f), new float3(0.5f, -1f, -0.5f)).y,
                Is.EqualTo(0.25f).Within(Tolerance));
            Assert.That(math.pow(new float3(4f, 4f, 4f), new float3(0.5f, -1f, -0.5f)).z,
                Is.EqualTo(0.5f).Within(Tolerance));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.pow(new float2s(2f, 3f), new float2s(3f, 2f)).x, Is.EqualTo(8f).Within(Tolerance));

            // a vector without a register reaches the member of the component type for every component, which is
            // the exact power of the component type
            Assert.That(math.pow(new float3s(2f, 3f, 4f), new float3s(3f, 2f, 2f)),
                Is.EqualTo(new float3s(8f, 9f, 16f)));
            Assert.That(math.pow(new half3((half)2f, (half)3f, (half)4f),
                    new half3((half)3f, (half)2f, (half)2f)),
                Is.EqualTo(new half3((half)8f, (half)9f, (half)16f)));
            Assert.That(math.pow(new double3s(2, 3, 4), new double3s(3, 2, 2)),
                Is.EqualTo(new double3s(8d, 9d, 16d)));

            // the power of the zero of a padding lane is the one of the kind of the component, so the register
            // of the result is masked
            Assert.That(math.pow(new float3(2f), new float3(0f)).vector.GetElement(3), Is.EqualTo(0f));
        }
    }
}
