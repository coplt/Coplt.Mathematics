using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The arc sine of every component of a value, the result of the member is in radians.
/// </summary>
public class TestAsin
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.asin(value);

    [Test]
    public void Value()
    {
        var v = new float3(0f, 0.5f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.asin().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(v.asin().y, Is.EqualTo(MathF.PI / 6f).Within(1e-5f));
            Assert.That(v.asin().z, Is.EqualTo(MathF.PI / 2f).Within(1e-5f));
            Assert.That(new double3(0, 0.5, 1).asin().y, Is.EqualTo(Math.PI / 6).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)2f).asin().x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The arc sine of the zero of a padding lane is the zero, the result of the member keeps the padding lanes
    /// of the value at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f, 0.5f, 1f).asin().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(0, 0.5, 1).asin().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(0f, 0.5f, 1f));
        Check<double3, double>(new double3(0, 0.5, 1));
        Check<half3, half>(new half3((half)0f, (half)1f, (half)2f));
    }
}
