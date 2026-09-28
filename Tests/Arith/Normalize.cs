using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The normalization of a value is the member of the map of a vector, so the value of a vector is handed over as
/// a whole: it is the value multiplied by the reciprocal of the length of it, which is exact, the estimate of the
/// hardware of it is <c>normalize_a</c>. A value whose length is zero cannot be scaled to one, so the result of
/// the exact member and the one of the estimate is not a number, and the result of <c>normalize_safe</c> and of
/// <c>normalize_safe_a</c> is the zero of the kind of the value instead. The map of a vector is the one of a
/// vector, so the value of a matrix does not reach these members.
/// </summary>
public class TestNormalize
{
    /// <summary>
    /// The estimate of the hardware is not exact, the one of the arm platform is the loose one of the two, so the
    /// tolerance leaves room for it.
    /// </summary>
    private const float Tolerance = 0.01f;

    /// <inheritdoc cref="Tolerance"/>
    private const double DoubleTolerance = 0.01;

    /// <summary>
    /// The members are the ones of the map of a vector, so a parameter that only knows the interface reaches
    /// them, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointVectorDispatch<T>
    {
        _ = math.normalize(v);
        _ = math.normalize_a(v);
        _ = math.normalize_safe(v);
        _ = math.normalize_safe_a(v);
    }

    [Test]
    public void Interface()
    {
        Check(new float2(3f, 4f));
        Check(new float3(3f, 4f, 0f));
        Check(new float4(1f, 2f, 3f, 4f));
        Check(new double2(3, 4));
        Check(new double3(3, 4, 0));
        Check(new double4(1, 2, 3, 4));
        // a vector without a register reaches the members of the component type
        Check(new half2((half)3f, (half)4f));
        Check(new half3((half)3f, (half)4f, (half)0f));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector is the component of it divided by the length of the vector
            var f3 = math.normalize(new float3(3f, 4f, 0f));
            Assert.That(f3.x, Is.EqualTo(0.6f).Within(1e-6f));
            Assert.That(f3.y, Is.EqualTo(0.8f).Within(1e-6f));
            Assert.That(f3.z, Is.EqualTo(0f));

            // the length of a normalized value is one
            Assert.That(math.length(f3), Is.EqualTo(1f).Within(1e-6f));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            var d3 = math.normalize(new double3(3, 4, 0));
            Assert.That(d3.x, Is.EqualTo(0.6d).Within(1e-15));
            Assert.That(d3.y, Is.EqualTo(0.8d).Within(1e-15));
            Assert.That(d3.z, Is.EqualTo(0d));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.normalize(new half3((half)2f, (half)0f, (half)0f)),
                Is.EqualTo(new half3((half)1f, (half)0f, (half)0f)));
            Assert.That((float)math.normalize(new half3((half)3f, (half)4f, (half)0f)).x,
                Is.EqualTo(0.6f).Within(Tolerance));

            // the padding lanes of a register are kept at zero by the members of the kind of the value
            Assert.That(f3.vector.GetElement(3), Is.EqualTo(0f));

            // the estimate of the hardware is near the exact result, it is not exact
            Assert.That(math.normalize_a(new float3(3f, 4f, 0f)).x, Is.EqualTo(0.6f).Within(Tolerance));
            Assert.That(math.normalize_a(new float3(3f, 4f, 0f)).y, Is.EqualTo(0.8f).Within(Tolerance));
            Assert.That(math.normalize_a(new double3(3, 4, 0)).x, Is.EqualTo(0.6d).Within(DoubleTolerance));
            Assert.That(math.normalize_a(new float3(3f, 4f, 0f)).vector.GetElement(3), Is.EqualTo(0f));
        }
    }

    [Test]
    public void ZeroLength()
    {
        using (Assert.EnterMultipleScope())
        {
            // the zero vector cannot be scaled to a length of one, so the result is not a number
            Assert.That(float.IsNaN(math.normalize(new float3(0f)).x), Is.True);
            Assert.That(double.IsNaN(math.normalize(new double3(0, 0, 0)).x), Is.True);
            Assert.That(float.IsNaN(math.normalize_a(new float3(0f)).x), Is.True);

            // the safe members return the zero of the kind of the value instead
            Assert.That(math.normalize_safe(new float3(3f, 4f, 0f)), Is.EqualTo(math.normalize(new float3(3f, 4f, 0f))));
            Assert.That(math.normalize_safe(new float3(0f)), Is.EqualTo(default(float3)));
            Assert.That(math.normalize_safe(new double3(0, 0, 0)), Is.EqualTo(default(double3)));
            Assert.That(math.normalize_safe(new half3((half)0f)), Is.EqualTo(default(half3)));
            Assert.That(math.normalize_safe_a(new float3(0f)), Is.EqualTo(default(float3)));
            Assert.That(math.normalize_safe_a(new double3(0, 0, 0)), Is.EqualTo(default(double3)));

            // a value that is too short to be scaled is the zero of the kind of the value: the squared length
            // of it is not above the smallest positive normal value of the kind of a component
            Assert.That(math.normalize_safe(new float3(1e-30f)), Is.EqualTo(default(float3)));
            Assert.That(math.normalize_safe_a(new float3(1e-30f)), Is.EqualTo(default(float3)));
            Assert.That(math.normalize_safe(new double3(1e-200)), Is.EqualTo(default(double3)));
            // the value of a length that is meaningful is scaled
            Assert.That(math.normalize_safe(new float3(1f, 0f, 0f)), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(math.normalize_safe(new double3(0, 1e-150, 0)).y, Is.EqualTo(1d));
        }
    }
}
