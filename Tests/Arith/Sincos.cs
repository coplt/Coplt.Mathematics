using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The sine and the cosine of every component of a value, which the member answers as a pair of values, the
/// angle of it is in radians. The pair and the member that receives the two values are the two forms of the
/// single member of the dispatch of the value that hands it over once.
/// </summary>
public class TestSincos
{
    /// <summary>
    /// The member is one of the dispatch of the algebra of the kind of the value, so a parameter that only knows
    /// the interfaces of it reaches the member as well.
    /// </summary>
    private static void Check<T>(T value)
        where T : unmanaged, IFloatDispatch<T>
        => _ = math.sincos(value);

    [Test]
    public void Value()
    {
        var v = new float3(0f, MathF.PI / 2f, MathF.PI);

        using (Assert.EnterMultipleScope())
        {
            var (sin, cos) = v.sincos();
            Assert.That(sin.x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(sin.y, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(cos.x, Is.EqualTo(1f).Within(1e-6f));
            Assert.That(cos.z, Is.EqualTo(-1f).Within(1e-5f));
        }

        using (Assert.EnterMultipleScope())
        {
            // the pair of the member that receives the two values is the same pair
            var d = new double3(0, Math.PI / 3, Math.PI);
            d.sincos(out var s, out var c);
            Assert.That(s.y, Is.EqualTo(Math.Sin(Math.PI / 3)).Within(1e-12));
            Assert.That(c.y, Is.EqualTo(0.5d).Within(1e-12));
            Assert.That(d.sincos().sin.Equals(s), Is.True);
            Assert.That(d.sincos().cos.Equals(c), Is.True);
        }

        using (Assert.EnterMultipleScope())
        {
            // a value without a register works on the components
            var (sin, cos) = new half3((half)0f, (half)1f, (half)2f).sincos();
            Assert.That(sin.x, Is.EqualTo((half)0f));
            Assert.That(cos.x, Is.EqualTo((half)1f));
        }
    }

    /// <summary>
    /// The cosine of the zero of a padding lane is the one, the results of the member mask the padding lanes of
    /// the value and keep them at zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            var (sin, cos) = new float3(1f, 2f, 3f).sincos();
            Assert.That(sin.vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(cos.vector.GetElement(3), Is.EqualTo(0f));
        }
    }

    [Test]
    public void Interface()
    {
        Check(new float3(0f, 1f, 2f));
        Check(new double3(0, 1, 2));
        Check(new half3((half)0f, (half)1f, (half)2f));
    }
}
