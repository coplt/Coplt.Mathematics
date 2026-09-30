using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The exponential of every component of a value with two as the base.
/// </summary>
public class TestExp2
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.exp2(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f).exp2().x, Is.EqualTo(1f).Within(1e-6f));
            Assert.That(new float3(10f).exp2().x, Is.EqualTo(1024f).Within(1e-3f));
            // a register of 256 bits
            Assert.That(new float4(0f, 1f, 2f, 3f).exp2().w, Is.EqualTo(8f).Within(1e-4f));
            Assert.That(new double2(2, 3).exp2().x, Is.EqualTo(4d).Within(1e-9));
            Assert.That(new double2(2, 3).exp2().y, Is.EqualTo(8d).Within(1e-9));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)2f).exp2().x, Is.EqualTo((half)1f));
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
            Assert.That(new float3(1f, 2f, 3f).exp2().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 2, 3).exp2().vector.GetElement(3), Is.EqualTo(0d));
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
