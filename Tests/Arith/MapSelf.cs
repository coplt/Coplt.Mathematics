using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Arith;

/// <summary>
/// The members that map the value of a floating point vector with the members of the kind of a single component
/// of it: the visitor of a member of the floating point kind and the one of the map of a value reach the same
/// shapes, so they are two families, and the dispatch of a value reaches the map of a value with a member of its
/// own beside the ones that reach the visitor of the kind of the value. The visitor of a map declares a single
/// member, which reaches the type of the value and the type of a single component of it, so the shape of the
/// value is not a part of it and every value is handed over the same way.
/// <para>The value of a matrix is the one of its columns, so the map of a value reaches every column of it: a
/// matrix maps each of its columns and builds itself out of them.</para>
/// </summary>
public class TestMapSelf
{
    /// <summary>
    /// Returns the value whose every component is the one of the value added to itself.
    /// </summary>
    private struct impl_twice : IMapVisitor<impl_twice>
    {
        /// <summary>The value whose every component is the one of the value added to itself</summary>
        public static TVector Map_Float<TVector, TScalar>(in TVector value)
            where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => value + value;
    }

    /// <summary>The map of a single value, the caller knows neither the shape of the value nor a component.</summary>
    private static T Twice<T>(in T value)
        where T : unmanaged, IAlgebraDispatch<T>
        => T.Map_Self<impl_twice>(value);

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

            // a value without a register reaches the member of the count of its components
            Assert.That(Twice(new half4((Half)1f, (Half)2f, (Half)3f, (Half)4f)),
                Is.EqualTo(new half4((Half)2f, (Half)4f, (Half)6f, (Half)8f)), "half4, a value without a register");

            // the map of a matrix maps every column of it and builds the matrix out of them
            Assert.That(Twice(new float2x2(new float2(1f, 2f), new float2(3f, 4f))),
                Is.EqualTo(new float2x2(new float2(2f, 4f), new float2(6f, 8f))), "float2x2");
            Assert.That(Twice(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f))),
                Is.EqualTo(new float3x2(new float3(2f, 4f, 6f), new float3(8f, 10f, 12f))), "float3x2");
        }
    }
}
