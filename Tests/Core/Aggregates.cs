using Coplt.Mathematics;

namespace Tests.Core;

/// <summary>
/// The queries that reduce a comparison of two values to a single one: the comparison of the components of two
/// values is the one of the kind of a component, which the mask of the comparison holds, and the query of it is
/// the one of the components of the mask that hold. A padding lane holds no component, so it never decides a
/// query, and a value that is not a number is not the one of no value.
/// </summary>
public class TestAggregates
{
    /// <summary>
    /// The query of no component holding is the complement of the one of any component holding, and both of them
    /// are answered in the form of the class and in the form on a value.
    /// </summary>
    [Test]
    public void None()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.none(new float4(0f, 0f, 0f, 0f)), Is.True, "the value of no component");
            Assert.That(math.none(new float4(0f, 1f, 0f, 0f)), Is.False, "the value of a component");
            Assert.That(math.none(new float4(0f, 0f, 0f, 0f)).Equals(
                new float4(0f, 0f, 0f, 0f).none()), Is.True, "the form on a value");
            Assert.That(math.none(new float3(0f, 0f, 0f)), Is.True, "the value of a padding lane");
            Assert.That(math.none(new half2((Half)0, (Half)0)), Is.True, "the value without a register");
            Assert.That(math.none(new float2x2(0f, 0f, 0f, 0f)), Is.True, "the value of a matrix");
            Assert.That(math.none(new float2x2(0f, 0f, 1f, 0f)), Is.False, "the value of a matrix with a component");
        }
    }

    /// <summary>
    /// The equality of two values holds where every component of one of them is the one of the matching
    /// component of the other, and the query of any component of them being the one of the kind holds where a
    /// single pair of them is.
    /// </summary>
    [Test]
    public void Equality()
    {
        var a = new float4(1f, 2f, 3f, 4f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.all_eq(a, new float4(1f, 2f, 3f, 4f)), Is.True, "the value of the kind");
            Assert.That(math.all_eq(a, new float4(1f, 2f, 3f, 5f)), Is.False, "the value of another component");
            Assert.That(math.all_eq(a, new float4(9f, 2f, 3f, 4f)), Is.False, "the value of the first component");
            Assert.That(math.any_eq(a, new float4(1f, 2f, 3f, 4f)), Is.True, "the value of the kind");
            Assert.That(math.any_eq(a, new float4(9f, 2f, 9f, 9f)), Is.True, "the value of a component");
            Assert.That(math.any_eq(a, new float4(9f, 9f, 9f, 9f)), Is.False, "the value of no component");

            // a padding lane holds no component and it is not reached, so the value of it is the one of the kind
            Assert.That(math.all_eq(new float3(1f, 2f, 3f), new float3(1f, 2f, 3f)), Is.True,
                "the value of three components");
            Assert.That(math.all_eq(new double3(1d, 2d, 3d), new double3(1d, 2d, 3d)), Is.True,
                "the value of three components of a double");
            Assert.That(math.all_eq(new half3((Half)1, (Half)2, (Half)3), new half3((Half)1, (Half)2, (Half)3)),
                Is.True, "the value without a register");
            Assert.That(math.all_eq(new int2(1, 2), new int2(1, 2)), Is.True, "the value of a whole number");

            // a value that is not a number is the one of no value
            var nan = new float4(float.NaN);
            Assert.That(math.any_eq(nan, nan), Is.False, "the value that is not a number");
            Assert.That(math.all_eq(nan, nan), Is.False, "the value that is not a number");

            Assert.That(math.all_eq(new float2x2(1f, 2f, 3f, 4f), new float2x2(1f, 2f, 3f, 4f)), Is.True,
                "the value of a matrix");
            Assert.That(math.all_eq(new float2x2(1f, 2f, 3f, 4f), new float2x2(1f, 2f, 3f, 5f)), Is.False,
                "the value of a matrix of another component");
            Assert.That(math.any_eq(new float2x2(1f, 2f, 3f, 4f), new float2x2(1f, 2f, 3f, 4f)), Is.True,
                "the value of a matrix of the kind");
        }
    }

    /// <summary>
    /// The ordering of two values holds where every component of one of them is ordered against the matching
    /// component of the other, and the query of any component of them being ordered holds where a single pair of
    /// them is.
    /// </summary>
    [Test]
    public void Ordering()
    {
        var a = new float4(1f, 2f, 3f, 4f);
        var b = new float4(2f, 3f, 4f, 5f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.all_lt(a, b), Is.True, "the value that is less at every component");
            Assert.That(math.all_lt(a, a), Is.False, "the value of the kind");
            Assert.That(math.any_lt(a, a), Is.False, "the value of the kind");
            Assert.That(math.any_lt(a, new float4(9f, 3f, 4f, 5f)), Is.True, "the value of a component");
            Assert.That(math.all_le(a, a), Is.True, "the value of the kind that is not greater");
            Assert.That(math.any_le(a, a), Is.True, "the value of the kind that is not greater");

            Assert.That(math.all_gt(b, a), Is.True, "the value that is greater at every component");
            Assert.That(math.all_gt(a, a), Is.False, "the value of the kind");
            Assert.That(math.any_gt(a, new float4(0f, 1f, 2f, 3f)), Is.True, "the value of a component");
            Assert.That(math.all_ge(a, a), Is.True, "the value of the kind that is not less");
            Assert.That(math.any_ge(a, b), Is.False, "the value that is less at every component");

            Assert.That(math.all_lt(new float3(1f, 2f, 3f), new float3(2f, 3f, 4f)), Is.True,
                "the value of three components");
            Assert.That(math.all_lt(new half2((Half)1, (Half)2), new half2((Half)2, (Half)3)), Is.True,
                "the value without a register");
            Assert.That(math.any_gt(new int4(1, 2, 3, 4), new int4(1, 2, 3, 9)), Is.False,
                "the value of a whole number");

            var m = new float2x2(1f, 2f, 3f, 4f);
            Assert.That(math.all_lt(m, new float2x2(2f, 3f, 4f, 5f)), Is.True, "the value of a matrix");
            Assert.That(math.any_gt(m, new float2x2(1f, 2f, 3f, 4f)), Is.False, "the value of a matrix of the kind");
        }
    }
}
