using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The refraction direction of the incident vector and the normal of a surface, which the index of refraction of
/// the two materials decides. The member is zero where the direction cannot leave the surface.
/// </summary>
public class TestRefract
{
    /// <summary>
    /// The member is one of the visitor of the kind of the value, so a parameter that only knows the interfaces
    /// of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.refract(value, value, TScalar.One);

    [Test]
    public void Value()
    {
        var i = new float3(0f, 0f, -1f);
        var n = new float3(0f, 0f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.refract(i, n, 1f).z, Is.EqualTo(-1f).Within(1e-5f));
            // a direction of a denser medium that is grazing enough cannot leave it
            var g = new float3(0f, -0.5f, -0.8660254f);
            Assert.That(math.refract(g, new float3(0f, 1f, 0f), 1.5f), Is.EqualTo(default(float3)));
            // both sides are the same material and the normal points the same way as the incident direction, so
            // the normal component of the direction is flipped and the other two are kept
            var d = math.normalize(new float3(0.3f, -0.4f, 0.5f));
            var r = math.refract(d, new float3(0f, 0f, 1f), 1f);
            Assert.That(r.x, Is.EqualTo(d.x).Within(1e-5f));
            Assert.That(r.y, Is.EqualTo(d.y).Within(1e-5f));
            Assert.That(r.z, Is.EqualTo(-d.z).Within(1e-5f));
        }

        // a value without a register works on the components
        using (Assert.EnterMultipleScope())
        {
            var h = new half3((half)0f, (half)0f, (half)(-1f));
            Assert.That(math.refract(h, new half3((half)0f, (half)0f, (half)1f), (half)1f).z, Is.EqualTo((half)(-1f)));
        }
    }

    /// <summary>
    /// The refraction of a padding lane is zero, the result of the member keeps the padding lanes of the value
    /// at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        var i = new float3(0f, 0f, -1f);
        var n = new float3(0f, 0f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.refract(i, n, 1f).vector.GetElement(3), Is.EqualTo(0f));
            // the direction that cannot leave the dense medium is the one that is zero
            var g = new float3(0f, -0.5f, -0.8660254f);
            Assert.That(math.refract(g, new float3(0f, 1f, 0f), 1.5f).vector.GetElement(3), Is.EqualTo(0f));
            var di = new double3(0, 0, -1);
            Assert.That(math.refract(di, new double3(0, 0, 1), 1d).vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(0f, 0f, -1f));
        Check<double3, double>(new double3(0, 0, -1));
        Check<half3, half>(new half3((half)0f, (half)0f, (half)(-1f)));
    }
}
