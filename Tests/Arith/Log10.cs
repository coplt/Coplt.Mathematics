using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The base 10 logarithm of every component of a value.
/// </summary>
public class TestLog10
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.log10(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 2f, 4f).log10().z, Is.EqualTo(MathF.Log10(4f)).Within(1e-5f));
            Assert.That(new float3(1f, 10f, 1000f).log10().z, Is.EqualTo(3f).Within(1e-5f));
            Assert.That(new double3(1, Math.E, 100).log10().y, Is.EqualTo(Math.Log10(Math.E)).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)1f, (half)10f, (half)100f).log10().z, Is.EqualTo((half)2f));
        }
    }

    /// <summary>
    /// The base 10 logarithm of the zero of a padding lane is a negative infinity, the result of the member
    /// masks the padding lanes of the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        var v = new float3(2f, 4f, 8f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.log10().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(2, 4, 8).log10().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(1f, 10f, 100f));
        Check<double3, double>(new double3(1, 10, 100));
        Check<half3, half>(new half3((half)1f, (half)10f, (half)100f));
    }
}
