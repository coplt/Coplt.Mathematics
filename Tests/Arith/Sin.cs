using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The sine of every component of a value, the angle of it is in radians.
/// </summary>
public class TestSin
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.sin(value);

    [Test]
    public void Value()
    {
        var v = new float3(0f, MathF.PI / 2f, MathF.PI);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.sin().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(v.sin().y, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(v.sin().z, Is.EqualTo(0f).Within(1e-5f));
            Assert.That(new double3(Math.PI / 6, 0, Math.PI / 2).sin().x, Is.EqualTo(0.5d).Within(1e-12));
            // the angle conversion of the floating point members is the input of the trigonometry
            Assert.That(new float3(90f).radians().sin().x, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(new double3(90, 0, 45).radians().sin().x, Is.EqualTo(1d).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)2f).sin().x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The sine of the zero of a padding lane is the zero, the result of the member keeps the padding lanes of
    /// the value at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 2f, 3f).sin().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 2, 3).sin().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(0f, 1f, 2f));
        Check<double3, double>(new double3(0, 1, 2));
        Check<half3, half>(new half3((half)0f, (half)1f, (half)2f));
    }
}
