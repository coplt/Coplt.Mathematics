using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The reflection of a value around a normal is the member of the algebra of the floating point kind: it returns
/// a reflection vector using an incident ray and a surface normal, which is the difference of the value and the
/// product of twice the normal and the dot product of the two. The value of a vector that keeps it in a register
/// reaches the member of the register, which fuses the product into the difference, the value of every other
/// vector reaches the member of the components of it. The normal has to be normalized.
/// </summary>
public class TestReflect
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T incident, T normal)
        where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointVector<T>
    {
        var r = math.reflect(incident, normal);
        Assert.That(incident.reflect(normal), Is.EqualTo(r));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(1f, -2f), new float2(0f, 1f));
        Check(new float3(1f, -2f, 3f), new float3(0f, 1f, 0f));
        Check(new float4(1f, -2f, 3f, -4f), new float4(0f, 1f, 0f, 0f));
        Check(new double2(1, -2), new double2(0, 1));
        Check(new double3(1, -2, 3), new double3(0, 1, 0));
        Check(new double4(1, -2, 3, -4), new double4(0, 1, 0, 0));
        // a vector without a register reaches the member of the components of it
        Check(new half2((half)1f, (half)(-2f)), new half2((half)0f, (half)1f));
        Check(new half3((half)1f, (half)(-2f), (half)3f), new half3((half)0f, (half)1f, (half)0f));
        Check(new float2s(1f, -2f), new float2s(0f, 1f));
        Check(new float3s(1f, -2f, 3f), new float3s(0f, 1f, 0f));
        Check(new double3s(1, -2, 3), new double3s(0, 1, 0));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the reflection of a vector that is parallel to the normal is the negative of it, the one of a
            // vector that is perpendicular to the normal is the vector itself
            Assert.That(math.reflect(new float3(0f, 2f, 0f), new float3(0f, 1f, 0f)),
                Is.EqualTo(new float3(0f, -2f, 0f)));
            Assert.That(math.reflect(new float3(1f, 0f, 0f), new float3(0f, 1f, 0f)),
                Is.EqualTo(new float3(1f, 0f, 0f)));
            // the reflection of the value is the one of the component of the normal of it
            Assert.That(math.reflect(new float4(1f, -2f, 3f, -4f), new float4(0f, 1f, 0f, 0f)),
                Is.EqualTo(new float4(1f, 2f, 3f, -4f)));
            Assert.That(math.reflect(new double2(1, -2), new double2(0, 1)), Is.EqualTo(new double2(1d, 2d)));

            // the register of a vector of two components without a value of its own is widened to 128 bits and
            // the value of the storage variant of one of them is exactly 64 bits wide
            Assert.That(math.reflect(new float2(1f, -2f), new float2(0f, 1f)), Is.EqualTo(new float2(1f, 2f)));
            Assert.That(math.reflect(new float2s(1f, -2f), new float2s(0f, 1f)), Is.EqualTo(new float2s(1f, 2f)));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.reflect(new float3s(1f, -2f, 3f), new float3s(0f, 1f, 0f)),
                Is.EqualTo(new float3s(1f, 2f, 3f)));
            Assert.That(math.reflect(new half3((half)1f, (half)(-2f), (half)3f),
                    new half3((half)0f, (half)1f, (half)0f)),
                Is.EqualTo(new half3((half)1f, (half)2f, (half)3f)));
            Assert.That(math.reflect(new double3s(1, -2, 3), new double3s(0, 1, 0)),
                Is.EqualTo(new double3s(1d, 2d, 3d)));
        }
    }

    /// <summary>
    /// The reflection of a value around a normalized normal twice is the value itself, the normal of the second
    /// reflection is the one of the first.
    /// </summary>
    [Test]
    public void Mirror()
    {
        using (Assert.EnterMultipleScope())
        {
            var f3 = new float3(1f, -2f, 3f);
            var n3 = new float3(0f, 1f, 0f);
            Assert.That(math.reflect(math.reflect(f3, n3), n3), Is.EqualTo(f3));

            var f4 = new float4(1f, -2f, 3f, -4f);
            var n4 = new float4(0f, 1f, 0f, 0f);
            Assert.That(math.reflect(math.reflect(f4, n4), n4), Is.EqualTo(f4));

            var d4 = new double4(1, -2, 3, -4);
            var dn4 = new double4(0, 1, 0, 0);
            Assert.That(math.reflect(math.reflect(d4, dn4), dn4), Is.EqualTo(d4));

            var h3 = new half3((half)1f, (half)(-2f), (half)3f);
            var hn3 = new half3((half)0f, (half)1f, (half)0f);
            Assert.That(math.reflect(math.reflect(h3, hn3), hn3), Is.EqualTo(h3));

            var f2s = new float2s(1f, -2f);
            var n2s = new float2s(0f, 1f);
            Assert.That(math.reflect(math.reflect(f2s, n2s), n2s), Is.EqualTo(f2s));

            var f3s = new float3s(1f, -2f, 3f);
            var n3s = new float3s(0f, 1f, 0f);
            Assert.That(math.reflect(math.reflect(f3s, n3s), n3s), Is.EqualTo(f3s));
        }
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component and both the value of the lane and the one
    /// of the normal of it are zero, so the reflection of it is the zero of it as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of
            // a vector of three components holds the fourth lane as padding
            Assert.That(math.reflect(new float2(1f, -2f), new float2(0f, 1f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.reflect(new float2(1f, -2f), new float2(0f, 1f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.reflect(new float3(1f, -2f, 3f), new float3(0f, 1f, 0f)).vector.GetElement(3),
                Is.EqualTo(0f));
            Assert.That(math.reflect(new double3(1, -2, 3), new double3(0, 1, 0)).vector.GetElement(3),
                Is.EqualTo(0d));
        }
    }
}
