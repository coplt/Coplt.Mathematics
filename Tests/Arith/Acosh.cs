using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The inverse hyperbolic cosine of every component of a value.
/// </summary>
public class TestAcosh
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.acosh(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the member of the simd library reaches the zero of the kind of the component through a logarithm,
            // which does not answer with it exactly
            Assert.That(new float3(1f).acosh().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(new float3(2f).acosh().x, Is.EqualTo(1.3169579f).Within(1e-5f));
            Assert.That(new double3(1, 2, 3).acosh().z, Is.EqualTo(Math.Acosh(3)).Within(1e-12));
            // the hyperbolics and their inverse are the inverse of each other
            Assert.That(new float3(1.5f).cosh().acosh().x, Is.EqualTo(1.5f).Within(1e-5f));
            // a value without a register works on the components
            Assert.That(new half3((half)1f, (half)2f, (half)3f).acosh().x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The inverse hyperbolic cosine of the zero of a padding lane is not a number, the result of the member
    /// masks the padding lanes of the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 2f, 3f).acosh().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 2, 3).acosh().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(1f, 2f, 3f));
        Check<double3, double>(new double3(1, 2, 3));
        Check<half3, half>(new half3((half)1f, (half)2f, (half)3f));
    }
}
