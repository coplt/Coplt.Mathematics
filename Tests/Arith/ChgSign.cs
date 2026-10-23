using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The value whose sign bit is flipped at every component that the matching component of another one is
/// negative, which is the value multiplied by the sign of the other one and not the taking of the sign of it.
/// </summary>
public class TestChgSign
{
    /// <summary>
    /// The member is one of the algebra of the kind of the value, so a parameter that only knows the interfaces
    /// of it reaches the member as well.
    /// </summary>
    private static void Check<T>(T value)
        where T : unmanaged, IFloatingPointAlgebra<T>
        => _ = math.chg_sign(value, value);

    [Test]
    public void Value()
    {
        var v = new float3(1f, 2f, 3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.chg_sign(new float3(-1f, 1f, -1f)), Is.EqualTo(new float3(-1f, 2f, -3f)));
            // the sign of the other one is taken where the value is positive
            Assert.That(v.chg_sign(new float3(-1f, -1f, -1f)), Is.EqualTo(new float3(-1f, -2f, -3f)));
            Assert.That(new double2(1, 2).chg_sign(new double2(-1, 1)), Is.EqualTo(new double2(-1, 2)));
        }

        using (Assert.EnterMultipleScope())
        {
            // the sign of the value itself is flipped where the other one is negative, it is not the magnitude of
            // it alone that is kept
            Assert.That(new float3(-1f, -2f, 3f).chg_sign(new float3(-1f, 1f, -1f)),
                Is.EqualTo(new float3(1f, -2f, -3f)));
            Assert.That(new float3(-1f, -2f, 3f).chg_sign(new float3(1f, -1f, 1f)),
                Is.EqualTo(new float3(-1f, 2f, 3f)));
        }

        using (Assert.EnterMultipleScope())
        {
            // a value without a register works on the components
            Assert.That(new half3((half)1f, (half)2f, (half)3f).chg_sign(new half3((half)(-1f), (half)1f, (half)(-1f))),
                Is.EqualTo(new half3((half)(-1f), (half)2f, (half)(-3f))));
            // a matrix is a value of the same algebra
            Assert.That(new float2x2(new float2(1f, 2f), new float2(3f, 4f)).chg_sign(
                    new float2x2(new float2(-1f, 1f), new float2(-1f, 1f))),
                Is.EqualTo(new float2x2(new float2(-1f, 2f), new float2(-3f, 4f))));
        }
    }

    /// <summary>
    /// The sign of the zero of a padding lane is the one of the value it follows, which keeps the zero of the
    /// lane.
    /// </summary>
    [Test]
    public void Padding()
    {
        var v = new float3(-1f, 2f, -3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.chg_sign(v).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(new double3(1, 2, 3).chg_sign(new double3(-1, -2, -3)).vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Interface()
    {
        Check(new float3(1f, 2f, 3f));
        Check(new double3(1, 2, 3));
        Check(new half3((half)1f, (half)2f, (half)3f));
    }
}
