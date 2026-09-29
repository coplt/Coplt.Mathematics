using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Arith;

/// <summary>
/// The square root of a value is the member of the algebra of the floating point kind: the value of a vector that
/// keeps it in a register reaches the member of the visitor that matches the width of the register, which is the
/// one of the simd library, the value of every other vector reaches the member of the scalar for every component
/// of it and the value of a matrix reaches it for every component of every one of its columns. The register of a
/// vector keeps the padding lanes of it at zero, which the square root of them is as well. The reciprocal of the
/// square root of a value is the one of the kind of it divided by the square root of it, which is exact, the
/// estimate of the hardware of it is <c>rsqrt_a</c>.
/// </summary>
public class TestSqrt
{
    /// <summary>
    /// The estimate of the hardware is not exact, the one of the arm platform is the loose one of the two, so the
    /// tolerance leaves room for it.
    /// </summary>
    private const float Tolerance = 0.01f;

    /// <inheritdoc cref="Tolerance"/>
    private const double DoubleTolerance = 0.01;

    /// <summary>
    /// The members are the ones of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches them, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IAlgebraDispatch<T>, Coplt.Mathematics.Algebras.IFloatingPointAlgebra<T>
    {
        _ = math.sqrt(v);
        _ = math.rsqrt(v);
        _ = math.rsqrt_a(v);
    }

    [Test]
    public void Interface()
    {
        Check(new float2(4f, 9f));
        Check(new float3(4f, 9f, 16f));
        Check(new float4(4f, 9f, 16f, 25f));
        Check(new double2(4, 9));
        Check(new double3(4, 9, 16));
        Check(new double4(4, 9, 16, 25));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)4f, (half)9f));
        Check(new half3((half)4f, (half)9f, (half)16f));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(4f, 9f), new float2(16f, 25f)));
        Check(new float2x3(new float2(4f, 9f), new float2(16f, 25f), new float2(36f, 49f)));
        Check(new double2x2(new double2(4, 9), new double2(16, 25)));
        Check(new half3x3(new half3((half)4f, (half)9f, (half)16f),
            new half3((half)25f, (half)36f, (half)49f), new half3((half)64f, (half)81f, (half)100f)));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the member of the register
            Assert.That(math.sqrt(new float2(4f, 9f)), Is.EqualTo(new float2(2f, 3f)));
            Assert.That(math.sqrt(new float3(4f, 9f, 16f)), Is.EqualTo(new float3(2f, 3f, 4f)));
            Assert.That(math.sqrt(new float4(4f, 9f, 16f, 25f)), Is.EqualTo(new float4(2f, 3f, 4f, 5f)));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            Assert.That(math.sqrt(new double2(4, 9)), Is.EqualTo(new double2(2d, 3d)));
            Assert.That(math.sqrt(new double3(4, 9, 16)), Is.EqualTo(new double3(2d, 3d, 4d)));
            Assert.That(math.sqrt(new double4(4, 9, 16, 25)), Is.EqualTo(new double4(2d, 3d, 4d, 5d)));

            // the square root of the zero of a padding lane is the zero of the kind of the component, so the
            // padding lane of the register of the result is zero as well
            Assert.That(math.sqrt(new float3(4f, 9f, 16f)).vector.GetElement(3), Is.EqualTo(0f));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.sqrt(new half3((half)4f, (half)9f, (half)16f)),
                Is.EqualTo(new half3((half)2f, (half)3f, (half)4f)));

            // the square root of a component below zero is not a number
            Assert.That(float.IsNaN(math.sqrt(new float3(-1f, 4f, 9f)).x), Is.True);
            Assert.That(double.IsNaN(math.sqrt(new double2(-1, 4)).x), Is.True);

            // the value of a matrix is reached through the value of every one of its columns
            Assert.That(math.sqrt(new float2x2(new float2(4f, 9f), new float2(16f, 25f))),
                Is.EqualTo(new float2x2(new float2(2f, 3f), new float2(4f, 5f))));
            Assert.That(math.sqrt(new double2x2(new double2(4, 9), new double2(16, 25))),
                Is.EqualTo(new double2x2(new double2(2d, 3d), new double2(4d, 5d))));
        }
    }

    [Test]
    public void ReciprocalSquareRoot()
    {
        using (Assert.EnterMultipleScope())
        {
            // the reciprocal of the square root is the one of the kind of the value divided by the square root of
            // it, so it is the exact result
            Assert.That(math.rsqrt(new float3(0.25f, 1f, 4f)), Is.EqualTo(new float3(2f, 1f, 0.5f)));
            Assert.That(math.rsqrt(new double2(0.25, 1)), Is.EqualTo(new double2(2d, 1d)));
            Assert.That(math.rsqrt(new half2((half)0.25f, (half)1f)), Is.EqualTo(new half2((half)2f, (half)1f)));

            // the reciprocal of the square root of a value that is not a number is one as well, the one of the
            // zero of the kind of a component is an infinity
            Assert.That(float.IsNaN(math.rsqrt(new float2(-1f, 4f)).x), Is.True);
            Assert.That(float.IsPositiveInfinity(math.rsqrt(new float2(0f, 4f)).x), Is.True);

            // the estimate of the hardware is near the exact result, it is not exact
            Assert.That(math.rsqrt_a(new float3(0.25f, 1f, 4f)).x, Is.EqualTo(2f).Within(Tolerance));
            Assert.That(math.rsqrt_a(new float3(0.25f, 1f, 4f)).y, Is.EqualTo(1f).Within(Tolerance));
            Assert.That(math.rsqrt_a(new float3(0.25f, 1f, 4f)).z, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(math.rsqrt_a(new double3(0.25, 1, 4)).y, Is.EqualTo(1d).Within(DoubleTolerance));
            // a vector without a register takes the estimate of the component type, which is the one of the
            // hardware of the platform as well
            Assert.That((float)math.rsqrt_a(new half3((half)0.25f, (half)1f, (half)4f)).y,
                Is.EqualTo(1f).Within(Tolerance));

            // the reciprocal of the square root of a padding lane is an infinity, so the register of the result
            // is built from the mask of the padding lanes of the value
            Assert.That(math.rsqrt_a(new float3(0.25f, 1f, 4f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.rsqrt(new float3(0.25f, 1f, 4f)).vector.GetElement(3), Is.EqualTo(0f));
        }
    }
}
