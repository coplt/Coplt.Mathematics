using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Tests.Arith;

/// <summary>
/// The members that map the value of a floating point vector with the members of the kind of a single component
/// of it: the visitor of a member of the floating point kind and the one of the map of a value reach the same
/// shapes, so they are two families, and the dispatch of a vector reaches the map of a value with a member of
/// its own beside the ones that reach the visitor of the kind of the value. The visitor of a map declares a
/// single member, which reaches the type of the value and the type of a single component of it, so the shape of
/// the value is not a part of it and every value is handed over the same way.
/// <para>The value of a matrix is the one of its columns, so a matrix has no map of it: the dispatch of a vector
/// is the one the map belongs to.</para>
/// </summary>
public class TestMapSelf
{
    /// <summary>
    /// Returns the value whose every component is the one of the value added to itself.
    /// </summary>
    private struct impl_twice : IFloatingPointAlgebraVisitor_Map_Self_Self<impl_twice>
    {
        /// <summary>The value whose every component is the one of the value added to itself</summary>
        public static TSelf Map<TSelf, TScalar>(in TSelf value)
            where TSelf : unmanaged, IFloatingPointAlgebraDispatch<TSelf>, INumberAlgebraDispatch<TSelf, TScalar>, IFloatingPointAlgebra<TSelf, TScalar>, INumberVector<TSelf, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => value + value;
    }

    /// <summary>
    /// Returns the value whose every component is the sum of the matching components of the two values of the
    /// arguments.
    /// </summary>
    private struct impl_add : IFloatingPointAlgebraVisitor_Map_Self_Self_Self<impl_add>
    {
        /// <summary>The value whose every component is the sum of the matching components of two values</summary>
        public static TSelf Map<TSelf, TScalar>(in TSelf a, in TSelf b)
            where TSelf : unmanaged, IFloatingPointAlgebraDispatch<TSelf>, INumberAlgebraDispatch<TSelf, TScalar>, IFloatingPointAlgebra<TSelf, TScalar>, INumberVector<TSelf, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => a + b;
    }

    /// <summary>The map of a single value, the caller knows neither the shape of the value nor a component.</summary>
    private static T Twice<T>(in T value)
        where T : unmanaged, IFloatingPointVectorDispatch<T>
        => T.Map_Self<impl_twice>(value);

    /// <summary>The map of two values.</summary>
    private static T Add<T>(in T a, in T b)
        where T : unmanaged, IFloatingPointVectorDispatch<T>
        => T.Map_Self<impl_add>(a, b);

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the members of every width of a register and the ones of every count of a component
            Assert.That(Twice(new float4(1f, 2f, 3f, 4f)), Is.EqualTo(new float4(2f, 4f, 6f, 8f)), "float4");
            Assert.That(Twice(new float3(1f, 2f, 3f)), Is.EqualTo(new float3(2f, 4f, 6f)), "float3, a padded register");
            Assert.That(Twice(new double4(1d, 2d, 3d, 4d)), Is.EqualTo(new double4(2d, 4d, 6d, 8d)), "double4, a register of 256 bits");
            Assert.That(Twice(new double3(1d, 2d, 3d)), Is.EqualTo(new double3(2d, 4d, 6d)), "double3");
            Assert.That(Twice(new float2s(1f, 2f)), Is.EqualTo(new float2s(2f, 4f)), "float2s, a register of 64 bits");

            // a value without a register reaches the member of the count of its components
            Assert.That(Twice(new float3s(1f, 2f, 3f)), Is.EqualTo(new float3s(2f, 4f, 6f)), "float3s");
            Assert.That(Twice(new half4((Half)1f, (Half)2f, (Half)3f, (Half)4f)),
                Is.EqualTo(new half4((Half)2f, (Half)4f, (Half)6f, (Half)8f)), "half4, a value without a register");
        }
    }

    [Test]
    public void Pair()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Add(new float4(1f, 2f, 3f, 4f), new float4(4f, 3f, 2f, 1f)),
                Is.EqualTo(new float4(5f, 5f, 5f, 5f)), "float4");
            Assert.That(Add(new float2(1f, 2f), new float2(3f, 5f)), Is.EqualTo(new float2(4f, 7f)), "float2");
            Assert.That(Add(new double3(1d, 2d, 3d), new double3(3d, 2d, 1d)),
                Is.EqualTo(new double3(4d, 4d, 4d)), "double3");
            Assert.That(Add(new float3s(1f, 2f, 3f), new float3s(3f, 2f, 1f)),
                Is.EqualTo(new float3s(4f, 4f, 4f)), "float3s");
        }
    }
}
