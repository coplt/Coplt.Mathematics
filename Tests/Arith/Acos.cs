using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The arc cosine of every component of a value, the result of the member is in radians.
/// </summary>
public class TestAcos
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.acos(value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 0.5f, 0f).acos().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(new float3(1f, 0.5f, 0f).acos().y, Is.EqualTo(MathF.PI / 3f).Within(1e-5f));
            Assert.That(new float3(1f, 0.5f, 0f).acos().z, Is.EqualTo(MathF.PI / 2f).Within(1e-5f));
            Assert.That(new double3(1, 0.5, 0).acos().y, Is.EqualTo(Math.PI / 3).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)1f, (half)0f, (half)(-1f)).acos().x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The arc cosine of the zero of a padding lane is half of pi, the result of the member masks the padding
    /// lanes of the value and keeps them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1f, 0.5f, 0f).acos().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 0.5, 0).acos().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(1f, 0.5f, 0f));
        Check<double3, double>(new double3(1, 0.5, 0));
        Check<half3, half>(new half3((half)1f, (half)0f, (half)(-1f)));
    }
}
