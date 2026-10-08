using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Arith;

/// <summary>
/// The wrap of a value puts every component of it into the range of two bounds: the offset of a component from the
/// lower bound of the range is the remainder of the division of it by the width of the range, which is the one of
/// <c>fmod</c>, so a component that is above the range is wrapped into it from the lower bound of it and one that
/// is below the range is wrapped into it from the upper bound. The member of the two bounds of the value reaches
/// every kind of it, the member of two of a single component reaches them as well, and the members the generator
/// emits for every scalar type make the member of the value and the one of the math class reach a range of the
/// type of a single component without naming it.
/// </summary>
public class TestWrap
{
    /// <summary>
    /// The member is the one of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T>(T value, T min, T max)
        where T : unmanaged, IAlgebraDispatch<T>, Coplt.Mathematics.Algebras.IFloatingPointAlgebra<T>
    {
        // the member of the two bounds of the value is an extension member of the math class, so its type
        // argument is named to reach it instead of the member of the two bounds of a single component, which
        // the compiler prefers and whose scalar type a value does not satisfy
        var wrapped = math.wrap<T>(value, min, max);
        Assert.That(value.wrap(min, max), Is.EqualTo(wrapped));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(2.5f, -0.5f), new float2(0f, 0f), new float2(2f, 2f));
        Check(new float3(2.5f, -1.5f, 0.25f), new float3(0f), new float3(1f));
        Check(new float4(2.5f, -0.5f, 4.5f, -3.5f), new float4(0f), new float4(4f));
        Check(new double2(2.5, -0.5), new double2(0), new double2(2));
        Check(new double3(2.5, -0.5, 4.5), new double3(0), new double3(4));
        // a vector without a register reaches the member of the components of it
        Check(new half2((Half)2.5f, (Half)(-0.5f)), new half2(Half.Zero, Half.Zero), new half2((Half)2f, (Half)2f));
        Check(new half3((Half)2.5f, (Half)(-0.5f), (Half)0.25f), new half3(Half.Zero), new half3(Half.One));
        // the member of the value reaches every kind of a value, a matrix as well
        Check(new float2x2(new float2(2.5f, 1.25f), new float2(-0.5f, -1.5f)),
            new float2x2(new float2(0f), new float2(0f)), new float2x2(new float2(2f), new float2(2f)));
    }

    [Test]
    public void Value()
    {
        var v = new float3(2.5f, -1.5f, 0.25f);

        using (Assert.EnterMultipleScope())
        {
            // a component that is not negative is wrapped into the range from the lower bound of it and a negative
            // one from the upper bound
            Assert.That(math.wrap(v, default, new float3(1f)), Is.EqualTo(new float3(0.5f, 0.5f, 0.25f)));
            // the bounds that are the same for every component are the ones of a single component
            Assert.That(math.wrap(v, 0f, 1f), Is.EqualTo(new float3(0.5f, 0.5f, 0.25f)));
            Assert.That(v.wrap(0f, 1f), Is.EqualTo(new float3(0.5f, 0.5f, 0.25f)));

            // a value that is inside the range is the value itself, one that is the upper bound of it is wrapped
            // to the lower bound of it
            Assert.That(math.wrap(new float3(0.5f, 1.5f, 0f), 0f, 2f), Is.EqualTo(new float3(0.5f, 1.5f, 0f)));
            Assert.That(math.wrap(new float3(2f, -2f, 4f), 0f, 2f), Is.EqualTo(default(float3)));

            Assert.That(math.wrap(new float3(2.5f, -0.5f, 0f), 0f, 2f), Is.EqualTo(new float3(0.5f, 1.5f, 0f)));
            Assert.That(math.wrap(new double2(2.5, -0.5), 0d, 2d), Is.EqualTo(new double2(0.5, 1.5)));
            Assert.That(math.wrap(new half2((Half)2.5f, (Half)(-0.5f)), (Half)0f, (Half)2f),
                Is.EqualTo(new half2((Half)0.5f, (Half)1.5f)));

            // the member the generator emits for the scalar type of a component reaches the one of the math class
            // with a bound that is not of the type of a single component
            Assert.That(math.wrap(new float3(1.5f), 0, 1), Is.EqualTo(new float3(0.5f)));
            Assert.That(new float3(1.5f).wrap(0, 1), Is.EqualTo(new float3(0.5f)));

            // every component of every column of a matrix is wrapped
            var m = math.wrap(new float2x2(new float2(2.5f, 1.25f), new float2(-0.5f, -1.5f)),
                new float2x2(new float2(0f), new float2(0f)), new float2x2(new float2(2f), new float2(2f)));
            Assert.That(m.c0.x, Is.EqualTo(0.5f));
            Assert.That(m.c0.y, Is.EqualTo(1.25f));
            Assert.That(m.c1.x, Is.EqualTo(1.5f));
            Assert.That(m.c1.y, Is.EqualTo(0.5f));
        }
    }

    /// <summary>
    /// The member of two bounds of a single component is the one of the marked member of the math class when the
    /// type of a component is a parameter of the kind of the value, which a generic member that only knows the kind
    /// of the value reaches.
    /// </summary>
    private static void CheckBounds<T, TScalar>(T value, TScalar min, TScalar max)
        where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointAlgebra<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        var wrapped = math.wrap(value, min, max);
        Assert.That(value.wrap(min, max), Is.EqualTo(wrapped));
    }

    /// <summary>
    /// The overloads of the member and the way a call reaches one of them. The member of the math class that names
    /// the type of a single component is the one a call of it reaches when the bounds are of that type, the member
    /// the generator emits for a scalar type is the one it reaches when they are not, and the member of the two
    /// bounds of the value is the one it reaches when the value is a bound as well.
    /// <para>The member of the two bounds of the value is an extension member of the math class, so a call of the
    /// value itself reaches the member the generator emits for the scalar type of it when its bounds are of that
    /// type, which is the one the priority of the marked member is below, and the marked member when they are
    /// not.</para>
    /// <para>A bound of a type that the kind of the value does not reach converts to nothing, so such a call does
    /// not reach a member at all: the bounds of a half value are not the ones of a float one for example.</para>
    /// </summary>
    [Test]
    public void Resolution()
    {
        using (Assert.EnterMultipleScope())
        {
            // the bounds of the value itself
            Assert.That(math.wrap(new float3(1.5f), new float3(0f), new float3(1f)), Is.EqualTo(new float3(0.5f)));
            Assert.That(new float3(1.5f).wrap(new float3(0f), new float3(1f)), Is.EqualTo(new float3(0.5f)));

            // the bounds of the type of a single component, which a call that names the type args reaches the
            // marked member of the math class with
            Assert.That(math.wrap<float3, float>(new float3(1.5f), 0f, 1f), Is.EqualTo(new float3(0.5f)));

            // the bounds of the type of a component reach the member of the two bounds of a single component, the
            // bounds are broadcast into every component of the value
            Assert.That(math.wrap(new float3(1.5f), 0f, 1f), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.wrap(new double2(1.5), 0d, 1d), Is.EqualTo(new double2(0.5)));
            Assert.That(math.wrap(new half2((Half)1.5f), (Half)0f, (Half)1f), Is.EqualTo(new half2((Half)0.5f)));

            // a call on the value itself reaches the member the generator emits for the scalar type of it
            Assert.That(new float3(1.5f).wrap(0f, 1f), Is.EqualTo(new float3(0.5f)));
            Assert.That(new double2(1.5).wrap(0d, 1d), Is.EqualTo(new double2(0.5)));
            Assert.That(new half2((Half)1.5f).wrap((Half)0f, (Half)1f), Is.EqualTo(new half2((Half)0.5f)));

            // a bound that is not of the type of a component is converted to it, a whole number literal as well as
            // a whole number of its own
            Assert.That(math.wrap(new float3(1.5f), 0, 1), Is.EqualTo(new float3(0.5f)));
            Assert.That(new float3(1.5f).wrap(0, 1), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.wrap(new float2(1.5f, 2.5f), 0, 1), Is.EqualTo(new float2(0.5f, 0.5f)));
            Assert.That(math.wrap(new double2(1.5, 2.5), 0, 1), Is.EqualTo(new double2(0.5, 0.5)));
            Assert.That(new double2(1.5, 2.5).wrap(0, 1), Is.EqualTo(new double2(0.5, 0.5)));

            int min = 0, max = 1;
            Assert.That(math.wrap(new float3(1.5f), min, max), Is.EqualTo(new float3(0.5f)));
            Assert.That(new float3(1.5f).wrap(min, max), Is.EqualTo(new float3(0.5f)));

            // a generic member that only knows the kind of the value reaches the marked member of the two bounds of
            // a single component
            CheckBounds(new float3(1.5f), 0f, 1f);
            CheckBounds(new double2(1.5), 0d, 1d);
            CheckBounds(new half2((Half)1.5f), (Half)0f, (Half)1f);

            // a matrix reaches both forms as well
            var m = new float2x2(new float2(1.5f, 0.5f), new float2(2.5f, -0.5f));
            var min2 = new float2x2(new float2(0f, 0f), new float2(0f, 0f));
            var max2 = new float2x2(new float2(1f, 1f), new float2(1f, 1f));
            Assert.That(math.wrap(m, min2, max2).c0.x, Is.EqualTo(0.5f));
            Assert.That(math.wrap(m, 0f, 1f).c1.y, Is.EqualTo(0.5f));
        }
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component, so the width of the range of it is the zero
    /// of the kind of it: the remainder of a value by it is not a number, which the member of the remainder masks,
    /// so the padding lane of the wrapped value is the zero of the kind of it as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.wrap(new float2(1.4f, 2.6f), 0f, 1f).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.wrap(new float2(1.4f, 2.6f), 0f, 1f).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.wrap(new float3(1.4f, -1.5f, 2.6f), 0f, 1f).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.wrap(new double3(1.4, -1.5, 2.6), 0d, 1d).vector.GetElement(3), Is.EqualTo(0d));

            // the bounds of the value itself
            Assert.That(
                math.wrap(new float3(1.4f, -1.5f, 2.6f), new float3(0f), new float3(1f)).vector.GetElement(3),
                Is.EqualTo(0f));
            Assert.That(
                math.wrap(new float2(1.4f, 2.6f), new float2(0f), new float2(1f)).vector.GetElement(3),
                Is.EqualTo(0f));
        }
    }
}
