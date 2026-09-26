using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The reciprocal of a value is the member of the algebra of the floating point kind: the value of a vector that
/// keeps it in a register reaches the member of the visitor that matches the width of the register, which is the
/// estimate of the hardware, the value of every other vector reaches the member of the scalar for every component
/// of it, which divides the one of the kind of the component by it, and the value of a matrix reaches it for every
/// component of every one of its columns. The reciprocal of a padding lane of a register is an infinity, so the
/// register of the value keeps the padding lanes of it at zero.
/// </summary>
public class TestRcp
{
    /// <summary>
    /// The estimate of the hardware is not exact, the one of the arm platform is the loose one of the three, so
    /// the tolerance leaves room for it. The estimate of a platform is exact for a value its table holds an
    /// entry for, which the one of another platform is not, so the value of a register is always asserted
    /// within it.
    /// </summary>
    private const float Tolerance = 0.01f;

    /// <inheritdoc cref="Tolerance"/>
    private const double DoubleTolerance = 0.01;

    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>
    {
        var r = math.rcp(v);
        Assert.That(v.rcp(), Is.EqualTo(r));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(1f, 2f));
        Check(new float3(1f, 2f, 4f));
        Check(new float4(1f, 2f, 4f, 8f));
        Check(new double2(1, 2));
        Check(new double3(1, 2, 4));
        Check(new double4(1, 2, 4, 8));
        // a vector without a register reaches the member of the scalar, which is the one of the component type
        Check(new half2((half)1f, (half)2f));
        Check(new half3((half)1f, (half)2f, (half)4f));
        Check(new float2s(1f, 2f));
        Check(new float3s(1f, 2f, 4f));
        Check(new double3s(1, 2, 4));
        // a matrix hands the value of every one of its columns over, so it reaches the member of the register or
        // the one of the scalar through the column
        Check(new float2x2(new float2(1f, 2f), new float2(4f, 8f)));
        Check(new float2x2s(new float2s(1f, 2f), new float2s(4f, 8f)));
        Check(new float2x3(new float2(1f, 2f), new float2(4f, 8f), new float2(0.5f, 0.25f)));
        Check(new double2x2(new double2(1, 2), new double2(4, 8)));
        Check(new half3x3(new half3((half)1f, (half)2f, (half)4f),
            new half3((half)0.5f, (half)0.25f, (half)8f), new half3((half)1f, (half)(-2f), (half)(-0.5f))));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector that keeps it in a register reaches the estimate of the hardware
            var f4 = math.rcp(new float4(1f, 2f, 4f, 8f));
            Assert.That(f4.x, Is.EqualTo(1f).Within(Tolerance));
            Assert.That(f4.y, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(f4.z, Is.EqualTo(0.25f).Within(Tolerance));
            Assert.That(f4.w, Is.EqualTo(0.125f).Within(Tolerance));

            // the register of a double is 128 bits wide for two components and 256 bits wide for three and four
            var d3 = math.rcp(new double3(1, 2, 4));
            Assert.That(d3.x, Is.EqualTo(1d).Within(DoubleTolerance));
            Assert.That(d3.y, Is.EqualTo(0.5d).Within(DoubleTolerance));
            Assert.That(d3.z, Is.EqualTo(0.25d).Within(DoubleTolerance));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            var f2s = math.rcp(new float2s(1f, 2f));
            Assert.That(f2s.x, Is.EqualTo(1f).Within(Tolerance));
            Assert.That(f2s.y, Is.EqualTo(0.5f).Within(Tolerance));

            // a vector without a register reaches the member of the component type for every component, which is
            // the exact reciprocal of the component
            Assert.That(math.rcp(new float3s(1f, 2f, 4f)), Is.EqualTo(new float3s(1f, 0.5f, 0.25f)));
            Assert.That(math.rcp(new half3((half)1f, (half)2f, (half)4f)),
                Is.EqualTo(new half3((half)1f, (half)0.5f, (half)0.25f)));
            Assert.That(math.rcp(new double3s(1, 2, 4)), Is.EqualTo(new double3s(1d, 0.5d, 0.25d)));

            // the value of a matrix is reached through the value of every one of its columns
            var m = math.rcp(new float2x2(new float2(1f, 2f), new float2(4f, 8f)));
            Assert.That(m.c0.x, Is.EqualTo(1f).Within(Tolerance));
            Assert.That(m.c0.y, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(m.c1.x, Is.EqualTo(0.25f).Within(Tolerance));
            Assert.That(m.c1.y, Is.EqualTo(0.125f).Within(Tolerance));
            // the columns of the storage variant of a matrix of two components keep their value in a 64 bit
            // register as well, so the estimate of the hardware reaches the value of them
            var m2s = math.rcp(new float2x2s(new float2s(1f, 2f), new float2s(4f, 8f)));
            Assert.That(m2s.c0.x, Is.EqualTo(1f).Within(Tolerance));
            Assert.That(m2s.c0.y, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(m2s.c1.x, Is.EqualTo(0.25f).Within(Tolerance));
            Assert.That(m2s.c1.y, Is.EqualTo(0.125f).Within(Tolerance));
            Assert.That(math.rcp(new half3x3(new half3((half)1f, (half)2f, (half)4f),
                    new half3((half)0.5f, (half)0.25f, (half)8f), new half3((half)1f, (half)(-2f), (half)(-0.5f)))),
                Is.EqualTo(new half3x3(new half3((half)1f, (half)0.5f, (half)0.25f),
                    new half3((half)2f, (half)4f, (half)0.125f), new half3((half)1f, (half)(-0.5f), (half)(-2f)))));
        }
    }

    /// <summary>
    /// The estimate of the hardware is not exact, the member of the scalar is the division of the one of the kind
    /// of the component by it: the value of a vector without a register is the exact reciprocal and the one of a
    /// vector that keeps its value in a register is near it, so the product of a value and its reciprocal is near
    /// one.
    /// </summary>
    [Test]
    public void Estimate()
    {
        var v = new float4(1f, 2f, 4f, 8f);
        var r = math.rcp(v);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.x * r.x, Is.EqualTo(1f).Within(2 * Tolerance));
            Assert.That(v.y * r.y, Is.EqualTo(1f).Within(2 * Tolerance));
            Assert.That(v.z * r.z, Is.EqualTo(1f).Within(2 * Tolerance));
            Assert.That(v.w * r.w, Is.EqualTo(1f).Within(2 * Tolerance));

            // the value of a vector without a register is the exact reciprocal of every component of it
            var s = new float3s(1f, 3f, 7f);
            var exact = math.rcp(s);
            Assert.That(exact.x, Is.EqualTo(1f / 1f));
            Assert.That(exact.y, Is.EqualTo(1f / 3f));
            Assert.That(exact.z, Is.EqualTo(1f / 7f));
        }
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component and the reciprocal of the zero of it is an
    /// infinity, so the member has to mask the register of the value to keep its bits at zero.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.rcp(new float2(1f, 2f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.rcp(new float2(1f, 2f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.rcp(new float3(1f, 2f, 4f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.rcp(new double3(1, 2, 4)).vector.GetElement(3), Is.EqualTo(0d));
        }
    }
}
