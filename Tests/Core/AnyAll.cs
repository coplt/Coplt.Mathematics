using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Core;

/// <summary>
/// The queries that reduce a value to a single one: a component of a value holds when the bits of it are not all
/// zero, so the query of <c>any</c> of a value holds when one of its components does and the one of <c>all</c>
/// holds when every one of them does. The value of a vector that keeps it in a register reaches the member of
/// the visitor that matches the width of the register, the value of a vector without a register reaches the
/// member of a single component for every one of them and the values of the queries are combined, and the value
/// of a matrix reaches it for every one of the columns of it. The member of the <c>math</c> class and the one
/// that is called on a value are the two forms of the same query and both of them answer the same. A padding
/// lane holds no component and it is kept at zero, so it never decides a query.
/// </summary>
public class TestAnyAll
{
    /// <summary>
    /// Checks the two forms of both queries of a value against the two answers it has: the checks of a test are
    /// run in a single scope, so every check of it reports its own failure instead of the first one hiding the
    /// rest of them.
    /// </summary>
    private static void Check<T>(T value, bool any, bool all) where T : unmanaged, IAlgebraDispatch<T>
    {
        Assert.That(math.any(value), Is.EqualTo(any), $"any of {typeof(T).Name} {value} in the form of the class");
        Assert.That(value.any(), Is.EqualTo(any), $"any of {typeof(T).Name} {value} in the form on a value");
        var r = math.all(value);
        Assert.That(math.all(value), Is.EqualTo(all), $"all of {typeof(T).Name} {value} in the form of the class");
        Assert.That(value.all(), Is.EqualTo(all), $"all of {typeof(T).Name} {value} in the form on a value");
    }

    /// <summary>
    /// The value of a vector that keeps it in a register reaches the member of the visitor that matches the
    /// width of the register, which is 128 bits or 256 bits, and the components of the value are the ones that
    /// decide the query.
    /// </summary>
    [Test]
    public void Register()
    {
        using (Assert.EnterMultipleScope())
        {
            // no component of the value is the zero of its kind
            Check<float2>(new(0f, 0f), false, false);
            Check<float3>(new(0f, 0f, 0f), false, false);
            Check<float4>(new(0f, 0f, 0f, 0f), false, false);
            Check<double2>(new(0d, 0d), false, false);
            Check<double3>(new(0d, 0d, 0d), false, false);
            Check<double4>(new(0d, 0d, 0d, 0d), false, false);
            Check<int2>(new(0, 0), false, false);
            Check<int3>(new(0, 0, 0), false, false);
            Check<int4>(new(0, 0, 0, 0), false, false);
            Check<uint2>(new(0u, 0u), false, false);
            Check<uint4>(new(0u, 0u, 0u, 0u), false, false);
            Check<long2>(new(0L, 0L), false, false);
            Check<long3>(new(0L, 0L, 0L), false, false);
            Check<ulong3>(new(0UL, 0UL, 0UL), false, false);
            Check<ulong4>(new(0UL, 0UL, 0UL, 0UL), false, false);

            // a single component of the value is not the zero of its kind
            Check<float2>(new(0f, 1f), true, false);
            Check<float3>(new(1f, 0f, 0f), true, false);
            Check<float4>(new(0f, 0f, 0f, 1f), true, false);
            Check<double3>(new(0d, 1d, 0d), true, false);
            Check<int2>(new(0, -1), true, false);
            Check<int4>(new(1, 2, 3, 0), true, false);
            Check<uint3>(new(0u, 0u, uint.MaxValue), true, false);
            Check<long2>(new(0L, long.MinValue), true, false);
            Check<ulong2>(new(1UL, 0UL), true, false);

            // every component of the value is not the zero of its kind, which the all bits set one of the kind
            // is a value of as well
            Check<float2>(new(1f, 2f), true, true);
            Check<float3>(new(-1f, -2f, -3f), true, true);
            Check<float4>(new(1f, 2f, 3f, 4f), true, true);
            Check<double2>(new(1d, 2d), true, true);
            Check<double3>(new(-1d, -2d, -3d), true, true);
            Check<int2>(new(-1, -2), true, true);
            Check<int3>(new(-1, -1, -1), true, true);
            Check<int4>(new(int.MinValue, -1, 1, int.MaxValue), true, true);
            Check<uint2>(new(uint.MaxValue, uint.MaxValue), true, true);
            Check<uint4>(new(1u, 2u, 3u, 4u), true, true);
            Check<long2>(new(long.MinValue, long.MaxValue), true, true);
            Check<long3>(new(-1L, -2L, -3L), true, true);
            Check<ulong2>(new(ulong.MaxValue, ulong.MaxValue), true, true);

            // a nan is not the zero of its kind, so a value that holds one is a value that the query of any
            // holds
            Check<float2>(new(0f, float.NaN), true, false);
            Check<double3>(new(double.NaN, 1d, 2d), true, true);
        }
    }

    /// <summary>
    /// A vector that has no register keeps its components in fields, so its value reaches the member of a single
    /// component for every one of them and the answers of the queries of the components are combined.
    /// </summary>
    [Test]
    public void Component()
    {
        using (Assert.EnterMultipleScope())
        {
            // no component of the value is the zero of its kind
            Check<half2>(new((Half)0f, (Half)0f), false, false);
            Check<half3>(new((Half)0f, (Half)0f, (Half)0f), false, false);
            Check<half4>(new((Half)0f, (Half)0f, (Half)0f, (Half)0f), false, false);
            Check<short2>(new(0, 0), false, false);
            Check<short3>(new(0, 0, 0), false, false);
            Check<short4>(new(0, 0, 0, 0), false, false);
            Check<ushort2>(new(0, 0), false, false);
            Check<ushort3>(new(0, 0, 0), false, false);
            Check<ushort4>(new(0, 0, 0, 0), false, false);

            // a single component of the value is not the zero of its kind
            Check<half2>(new((Half)0f, (Half)1f), true, false);
            Check<half3>(new((Half)1f, (Half)0f, (Half)0f), true, false);
            Check<half4>(new((Half)0f, (Half)0f, (Half)0f, (Half)float.NaN), true, false);
            Check<short2>(new(0, -1), true, false);
            Check<short3>(new(1, 2, 0), true, false);
            Check<short4>(new(0, 0, 0, short.MinValue), true, false);
            Check<ushort2>(new(0, 1), true, false);
            Check<ushort3>(new(1, 0, 3), true, false);
            Check<ushort4>(new(0, 2, 0, 0), true, false);

            // every component of the value is not the zero of its kind
            Check<half2>(new((Half)1f, (Half)2f), true, true);
            Check<half3>(new((Half)(-1f), (Half)(-2f), (Half)(-3f)), true, true);
            Check<half4>(new((Half)1f, (Half)2f, (Half)3f, (Half)4f), true, true);
            Check<short2>(new(-1, -2), true, true);
            Check<short3>(new(-1, -1, -1), true, true);
            Check<short4>(new(1, 2, 3, 4), true, true);
            Check<ushort2>(new(ushort.MaxValue, ushort.MaxValue), true, true);
            Check<ushort3>(new(1, 2, 3), true, true);
        }
    }

    /// <summary>
    /// The value of a matrix reaches the visitor for every one of the columns of it and the answers of the
    /// queries of the columns are combined, so both queries of a matrix are the ones of every component of every
    /// column of it. The columns of a matrix that keeps them without a register reach the member of a single
    /// component for every one of their components.
    /// </summary>
    [Test]
    public void Matrix()
    {
        using (Assert.EnterMultipleScope())
        {
            // every component of the matrix is the zero of its kind
            Check<float2x2>(new(new float2(0f, 0f), new float2(0f, 0f)), false, false);
            Check<float3x3>(new(new float3(0f, 0f, 0f), new float3(0f, 0f, 0f), new float3(0f, 0f, 0f)), false,
                false);
            Check<int2x2>(new(new int2(0, 0), new int2(0, 0)), false, false);
            Check<double3x3>(new(new double3(0d, 0d, 0d), new double3(0d, 0d, 0d), new double3(0d, 0d, 0d)), false,
                false);
            Check<half2x2>(new(new half2((Half)0f, (Half)0f), new half2((Half)0f, (Half)0f)), false, false);
            Check<short3x2>(new(new short3(0, 0, 0), new short3(0, 0, 0)), false, false);

            // the component of a single column of the matrix is not the zero of its kind
            Check<float2x2>(new(new float2(0f, 0f), new float2(0f, 1f)), true, false);
            Check<float3x3>(new(new float3(1f, 0f, 0f), new float3(0f, 0f, 0f), new float3(0f, 0f, 0f)), true,
                false);
            Check<float3x4>(new(new float3(0f, 0f, 0f), new float3(0f, 0f, 0f), new float3(0f, 0f, 0f),
                new float3(0f, 0f, 1f)), true, false);
            Check<int2x2>(new(new int2(0, 0), new int2(3, 0)), true, false);
            Check<uint4x4>(new(new uint4(0u, 0u, 0u, 0u), new uint4(0u, 0u, 0u, 0u), new uint4(0u, 0u, 1u, 0u),
                new uint4(0u, 0u, 0u, 0u)), true, false);
            Check<long2x2>(new(new long2(0L, 0L), new long2(0L, long.MinValue)), true, false);
            Check<half2x2>(new(new half2((Half)0f, (Half)0f), new half2((Half)1f, (Half)0f)), true, false);
            Check<short3x2>(new(new short3(0, 0, 0), new short3(0, -1, 0)), true, false);

            // no component of the matrix is the zero of its kind
            Check<float2x2>(new(new float2(1f, 2f), new float2(3f, 4f)), true, true);
            Check<float3x3>(new(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f), new float3(7f, 8f, 9f)), true,
                true);
            Check<double3x3>(new(new double3(-1d, -2d, -3d), new double3(-4d, -5d, -6d),
                new double3(-7d, -8d, -9d)), true, true);
            Check<int2x2>(new(new int2(-1, -2), new int2(-3, -4)), true, true);
            Check<uint2x3>(new(new uint2(1u, 2u), new uint2(3u, 4u), new uint2(5u, 6u)), true, true);
            Check<ulong4x2>(new(new ulong4(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue),
                new ulong4(1UL, 2UL, 3UL, 4UL)), true, true);
            Check<half3x2>(new(new half3((Half)1f, (Half)2f, (Half)3f), new half3((Half)4f, (Half)5f, (Half)6f)),
                true, true);
            Check<ushort2x4>(new(new ushort2(1, 2), new ushort2(3, 4), new ushort2(5, 6), new ushort2(7, 8)),
                true, true);
        }
    }

    /// <summary>
    /// The value of a vector of 2 or 3 components is kept in a register that is wider than it, so some of its
    /// lanes are padding lanes: they are kept at zero and they hold no component, which is why the query of a
    /// value is decided by its components alone. A value of 4 components fills the register of it and a value of
    /// 2 components of 64 bit components fills the 128 bit register it is kept in, so none of those has a
    /// padding lane.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float2.HavePaddingLanes, Is.True, "the value of a float2 does not fill its register");
            Assert.That(float3.HavePaddingLanes, Is.True, "the value of a float3 does not fill its register");
            Assert.That(double3.HavePaddingLanes, Is.True, "the value of a double3 does not fill its register");
            Assert.That(long3.HavePaddingLanes, Is.True, "the value of a long3 does not fill its register");
            Assert.That(float4.HavePaddingLanes, Is.False, "the value of a float4 fills its register");
            Assert.That(double2.HavePaddingLanes, Is.False, "the value of a double2 fills its register");
            Assert.That(long2.HavePaddingLanes, Is.False, "the value of a long2 fills its register");
            // the padding lane of the register of a value that a constructor built stays zero
            Assert.That(new float2(1f, 2f).vector.GetElement(2), Is.EqualTo(0f), "the padding lane of a float2");
            Assert.That(new float3(1f, 2f, 3f).vector.GetElement(3), Is.EqualTo(0f),
                "the padding lane of a float3");

            // the padding lanes are the zero of their kind, so they do not decide a query: the value of a
            // vector whose components all hold is one that the query of all holds, beside the lanes the value
            // does not reach
            Check<float2>(new(1f, 2f), true, true);
            Check<float3>(new(1f, 2f, 3f), true, true);
            Check<int2>(new(1, 2), true, true);
            Check<int3>(new(1, 2, 3), true, true);
            Check<uint2>(new(1u, 2u), true, true);
            Check<uint3>(new(1u, 2u, 3u), true, true);
            Check<double3>(new(1d, 2d, 3d), true, true);
            Check<long3>(new(1L, 2L, 3L), true, true);
            Check<ulong3>(new(1UL, 2UL, 3UL), true, true);
            // and the value of a vector whose components are all the zero of their kind is one that the query
            // of any does not hold, beside the zeroes beyond it as well
            Check<float2>(new(0f, 0f), false, false);
            Check<float3>(new(0f, 0f, 0f), false, false);
            Check<int2>(new(0, 0), false, false);
            Check<int3>(new(0, 0, 0), false, false);
            Check<long3>(new(0L, 0L, 0L), false, false);
        }
    }
}
