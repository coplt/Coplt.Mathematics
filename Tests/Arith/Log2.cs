using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The base 2 logarithm of every component of a value.
/// </summary>
public class TestLog2
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.log2(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 2f, 4f).log2().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(new float3(1f, 2f, 4f).log2().z, Is.EqualTo(2f).Within(1e-5f));
            Assert.That(new double3(1, 2, 1024).log2().z, Is.EqualTo(10d).Within(1e-9));
            // a value without a register works on the components
            Assert.That(new half3((half)1f, (half)2f, (half)4f).log2().y, Is.EqualTo((half)1f));
        }
    }

    /// <summary>
    /// The base 2 logarithm of the zero of a padding lane is a negative infinity, the result of the member masks
    /// the padding lanes of the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        var v = new float3(2f, 4f, 8f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.log2().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(2, 4, 8).log2().vector.GetElement(3), Is.EqualTo(0d));
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
