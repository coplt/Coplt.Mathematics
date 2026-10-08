using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The natural logarithm of every component of a value and the logarithm of it with another value as the base.
/// </summary>
public class TestLog
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.log(value);

    [Test]
    public void Value()
    {
        var v = new float3(1f, 2f, 4f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.log().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(v.log().y, Is.EqualTo(MathF.Log(2f)).Within(1e-5f));
            Assert.That(new double3(1, Math.E, 100).log().y, Is.EqualTo(1d).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)1f, (half)2f, (half)4f).log().z, Is.EqualTo((half)MathF.Log(4f)));
        }
    }

    [Test]
    public void Base()
    {
        using (Assert.EnterMultipleScope())
        {
            // the logarithm of any base is the quotient of the two logarithms
            Assert.That(new float3(1f, 2f, 4f).log(new float3(2f)).y, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(new float3(1f, 8f, 1024f).log(new float3(2f)).z, Is.EqualTo(10f).Within(1e-4f));
            Assert.That(new double3(8, 100, 2).log(new double3(2)).x, Is.EqualTo(3d).Within(1e-9));
        }
    }

    /// <summary>
    /// The natural logarithm of the zero of a padding lane is a negative infinity, the result of the member
    /// masks the padding lanes of the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        var v = new float3(2f, 4f, 8f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.log().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.log(new float3(2f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(2, 4, 8).log().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(1f, 2f, 4f));
        Check<double3, double>(new double3(1, 2, 4));
        Check<half3, half>(new half3((half)1f, (half)2f, (half)4f));
    }
}
