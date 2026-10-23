using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The one of the kind of a component where the component of a value is not less than the matching component of
/// a threshold and the zero of it where it is less.
/// </summary>
public class TestStep
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well. The member of the math class takes the threshold first,
    /// the one of a value takes it last.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        _ = value.step(value);
        _ = math.step(value, value);
    }

    [Test]
    public void Value()
    {
        var v = new float3(0f, 1f, 2f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.step(new float3(1f)), Is.EqualTo(new float3(0f, 1f, 1f)));
            Assert.That(new float3(1f, 0f, 0f).step(new float3(1f)), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(new double3(0, 2, 0).step(new double3(1)), Is.EqualTo(new double3(0, 1, 0)));
            // the member of the math class takes the threshold first
            Assert.That(math.step(new float3(1f), v), Is.EqualTo(new float3(0f, 1f, 1f)));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)2f).step(new half3((half)1f)),
                Is.EqualTo(new half3((half)0f, (half)1f, (half)1f)));
        }
    }

    /// <summary>
    /// The step of the two zeroes of a padding lane holds, the result of the member masks the padding lanes of
    /// the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f, 1f, 2f).step(new float3(1f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(0, 1, 2).step(new double3(1)).vector.GetElement(3), Is.EqualTo(0d));
            // the register of a value of 2 components is wider than the value
            Assert.That(new float2(0f, 1f).step(new float2(1f)).vector.GetElement(2), Is.EqualTo(0f));
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
