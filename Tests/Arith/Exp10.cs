using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The exponential of every component of a value with ten as the base.
/// </summary>
public class TestExp10
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.exp10(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f).exp10().x, Is.EqualTo(1f).Within(1e-6f));
            Assert.That(new float3(3f).exp10().x, Is.EqualTo(1000f).Within(1e-2f));
            Assert.That(new double3(0, 1, 3).exp10().z, Is.EqualTo(1000d).Within(1e-6d));
            // the base 10 exponential and the base 10 logarithm are the inverse of each other
            Assert.That(new float3(3f).log10().exp10().x, Is.EqualTo(3f).Within(1e-4f));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)2f).exp10().z, Is.EqualTo((half)100f));
        }
    }

    /// <summary>
    /// The exponential of the zero of a padding lane is the one, the result of the member masks the padding
    /// lanes of the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 2f, 3f).exp10().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 2, 3).exp10().vector.GetElement(3), Is.EqualTo(0d));
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
