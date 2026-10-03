using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The arc tangent of the quotient of the two matching components of two values, the result of the member is in
/// radians and the signs of both of the values are used to find the quadrant of it.
/// </summary>
public class TestAtan2
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.atan2(value, value);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f, 1f, 1f).atan2(new float3(1f, 1f, -1f)).x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(new float3(0f, 1f, 1f).atan2(new float3(1f, 1f, -1f)).y, Is.EqualTo(MathF.PI / 4f).Within(1e-5f));
            // the signs of both vectors are used to find the quadrant of the result
            Assert.That(new float3(0f, 1f, 1f).atan2(new float3(1f, 1f, -1f)).z, Is.EqualTo(3f * MathF.PI / 4f).Within(1e-5f));
            Assert.That(new double2(1, -1).atan2(new double2(1, 0)).y, Is.EqualTo(-Math.PI / 2).Within(1e-12));
            // a value without a register works on the components
            Assert.That(new half3((half)0f, (half)1f, (half)0f).atan2(new half3((half)1f)).x, Is.EqualTo((half)0f));
        }
    }

    /// <summary>
    /// The arc tangent of the two zeroes of a padding lane is the zero, the result of the member keeps the
    /// padding lanes of the value at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            var v = new float3(1f, 2f, 3f);
            Assert.That(v.atan2(v).vector.GetElement(3), Is.EqualTo(0f));
            var d = new double3(1, 2, 3);
            Assert.That(d.atan2(d).vector.GetElement(3), Is.EqualTo(0d));
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
