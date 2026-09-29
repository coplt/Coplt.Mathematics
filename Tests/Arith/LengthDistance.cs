using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Arith;

/// <summary>
/// The length of a vector is the square root of the dot product of it with itself and the distance between two
/// vectors is the length of the difference of them. The type of a single component is a part of the result of the
/// member, so the compiler cannot infer it from the arguments: a call either names it as well,
/// <c>math.length&lt;float3, float&gt;(v)</c>, or reaches the member of the scalar type of the vector, which the
/// generator emits for the type of a single component, and then the type of the vector is inferred:
/// <c>math.length(v)</c>.
/// </summary>
public class TestLengthDistance
{
    /// <summary>
    /// The members are the ones of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches them, which is the form the members of the library use.
    /// </summary>
    private static void Check<T, TScalar>(T v)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
    {
        _ = math.length<T, TScalar>(v);
        _ = math.distance<T, TScalar>(v, v);
    }

    [Test]
    public void Interface()
    {
        Check<float2, float>(new float2(3f, 4f));
        Check<float3, float>(new float3(3f, 4f, 0f));
        Check<float4, float>(new float4(1f, 2f, 3f, 4f));
        Check<double2, double>(new double2(3, 4));
        Check<double3, double>(new double3(3, 4, 0));
        Check<double4, double>(new double4(1, 2, 3, 4));
        // a vector without a register works on the components
        Check<half2, Half>(new half2((half)3f, (half)4f));
        Check<half3, Half>(new half3((half)3f, (half)4f, (half)0f));
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // the length of the vector is the square root of the dot product of it with itself
            Assert.That(math.length(new float2(3f, 4f)), Is.EqualTo(5f));
            Assert.That(math.length(new float3(3f, 4f, 0f)), Is.EqualTo(5f));
            Assert.That(math.length(new float4(1f, 2f, 3f, 4f)), Is.EqualTo(MathF.Sqrt(30f)));
            Assert.That(math.length(new double3(3, 4, 0)), Is.EqualTo(5d));
            Assert.That(math.length(new double4(1, 2, 3, 4)), Is.EqualTo(Math.Sqrt(30d)));

            // the type of a single component can be named beside the one of the vector
            Assert.That(math.length<float3, float>(new float3(3f, 4f, 0f)), Is.EqualTo(5f));
            Assert.That(math.distance<float3, float>(new float3(1f, 1f, 1f), new float3(4f, 5f, 1f)),
                Is.EqualTo(5f));

            // a vector without a register works on the components
            Assert.That((float)math.length(new half2((half)3f, (half)4f)), Is.EqualTo(5f));
            Assert.That((float)math.length(new half3((half)1f, (half)2f, (half)2f)), Is.EqualTo(3f));

            // the length of the zero vector is zero
            Assert.That(math.length(default(float3)), Is.EqualTo(0f));
            Assert.That(math.length(default(double2)), Is.EqualTo(0d));
            Assert.That((float)math.length(default(half3)), Is.EqualTo(0f));

            // the distance between the two vectors is the length of the difference of them
            Assert.That(math.distance(new float3(1f, 1f, 1f), new float3(4f, 5f, 1f)), Is.EqualTo(5f));
            Assert.That(math.distance(new double2(0, 0), new double2(1, 1)), Is.EqualTo(Math.Sqrt(2d)));
            Assert.That((float)math.distance(new half2((half)0f, (half)0f), new half2((half)3f, (half)4f)),
                Is.EqualTo(5f));
            // the distance of a vector and itself is zero
            Assert.That(math.distance(new float3(1f, 2f, 3f), new float3(1f, 2f, 3f)), Is.EqualTo(0f));
        }
    }
}
