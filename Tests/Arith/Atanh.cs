using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The inverse hyperbolic tangent of every component of a value.
/// </summary>
public class TestAtanh
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.atanh(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the member of the simd library reaches the zero of the kind of the component through a logarithm,
            // which does not answer with it exactly
            Assert.That(new float3(0f).atanh().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(new float3(0.5f).atanh().x, Is.EqualTo(0.5493061f).Within(1e-5f));
            Assert.That(new double3(0, 0.5, -0.5).atanh().z, Is.EqualTo(Math.Atanh(-0.5)).Within(1e-12));
            // the hyperbolics and their inverse are the inverse of each other
            Assert.That(new float3(0.5f).tanh().atanh().x, Is.EqualTo(0.5f).Within(1e-5f));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)0.5f, (half)(-0.5f)).atanh().x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The member of the simd library of a padding lane reaches the zero of the kind of the component through a
    /// logarithm, which does not answer with it exactly, so the result of the member masks the padding lanes of
    /// the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0.5f, 0.25f, 0f).atanh().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(0.5, 0.25, 0).atanh().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(0f, 0.5f, -0.5f));
        Check<double3, double>(new double3(0, 0.5, -0.5));
        Check<half3, half>(new half3((half)0f, (half)0.5f, (half)(-0.5f)));
    }
}
