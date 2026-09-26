using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Core;

/// <summary>
/// The kind of the algebra of a value is only known from its type: the type of a component decides the kind of
/// the value, so every value of the library names the kind of it. A half, a float and a double are floating point
/// numbers of the ieee 754 standard and the floating point kind of the library is the one of the standard, so the
/// type of every one of them names it, the members of it are implemented explicitly, so they are not members that
/// the type spells out.
/// <para>The library names the floating point kind for the value of a half component as well, so the value of a
/// half names it like the one of a float does</para>
/// </summary>
public class TestAlgebraKind
{
    /// <summary>
    /// True when <paramref name="type"/> implements the interface <paramref name="openInterface"/>, which is
    /// named without the type arguments of it, because the constraint of the interface of a kind is the interface
    /// of the kind itself, so a type that does not implement it cannot name it with the type arguments of it.
    /// </summary>
    private static bool Implements(Type type, Type openInterface) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openInterface);

    [Test]
    public void ComponentTypes()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Implements(typeof(float), typeof(IBinaryNumber<>)), Is.True);
            Assert.That(Implements(typeof(float), typeof(IFloatingPoint<>)), Is.True);
            Assert.That(Implements(typeof(float), typeof(IFloatingPointIeee754<>)), Is.True);
            Assert.That(Implements(typeof(double), typeof(IFloatingPoint<>)), Is.True);
            Assert.That(Implements(typeof(double), typeof(IFloatingPointIeee754<>)), Is.True);
            // a half is a floating point number of the standard and the type of it names the kind of the
            // standard as well, which the library does not emit the members of for a half
            Assert.That(Implements(typeof(half), typeof(IBinaryNumber<>)), Is.True);
            Assert.That(Implements(typeof(half), typeof(IFloatingPoint<>)), Is.True);
            Assert.That(Implements(typeof(half), typeof(IFloatingPointIeee754<>)), Is.True);
        }
    }

    [Test]
    public void VectorTypes()
    {
        using (Assert.EnterMultipleScope())
        {
            // a value names the floating point kind of the library when the type of its component is a floating
            // point number, which the type of a half is as well
            Assert.That(Implements(typeof(float3), typeof(IFloatingPointVector<>)), Is.True);
            Assert.That(Implements(typeof(half3), typeof(IFloatingPointVector<>)), Is.True);

            // the dispatch of a value is the one of every kind of the component of it
            Assert.That(Implements(typeof(float3), typeof(IFloatingPointAlgebraDispatch<>)), Is.True);
            Assert.That(Implements(typeof(half3), typeof(IFloatingPointAlgebraDispatch<>)), Is.True);
        }
    }
}
