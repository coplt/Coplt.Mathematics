using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The arc tangent of every component of a value, the result of the member is in radians.
/// </summary>
public class TestAtan
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.atan(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f, 1f, -1f).atan().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(new float3(0f, 1f, -1f).atan().y, Is.EqualTo(MathF.PI / 4f).Within(1e-5f));
            Assert.That(new float3(0f, 1f, -1f).atan().z, Is.EqualTo(-MathF.PI / 4f).Within(1e-5f));
            Assert.That(new double3(0, 1, -1).atan().y, Is.EqualTo(Math.PI / 4).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)(-1f)).atan().x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The arc tangent of the zero of a padding lane is the zero, the result of the member keeps the padding
    /// lanes of the value at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 2f, 3f).atan().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 2, 3).atan().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(0f, 1f, -1f));
        Check<double3, double>(new double3(0, 1, -1));
        Check<half3, half>(new half3((half)0f, (half)1f, (half)(-1f)));
    }
}
