using Coplt.Experimental.Mathematics;

namespace Tests.Arith;

/// <summary>
/// Runs the arithmetic interface checks over every generated number vector, see <see cref="ArithCheck"/>.
/// </summary>
public class TestGenericVectorArithmetic
{
    [Test]
    public void Float()
    {
        ArithCheck.Arithmetic<float2, float>(2, true);
        ArithCheck.Arithmetic<float3, float>(3, true);
        ArithCheck.Arithmetic<float4, float>(4, true);

        ArithCheck.Negation<float2, float>(2, true);
        ArithCheck.Negation<float3, float>(3, true);
        ArithCheck.Negation<float4, float>(4, true);

        ArithCheck.Cross<float3, float>(true);
        ArithCheck.SignedCross<float3, float>(true);
    }

    [Test]
    public void Double()
    {
        ArithCheck.Arithmetic<double2, double>(2, true);
        ArithCheck.Arithmetic<double3, double>(3, true);
        ArithCheck.Arithmetic<double4, double>(4, true);

        ArithCheck.Negation<double2, double>(2, true);
        ArithCheck.Negation<double3, double>(3, true);
        ArithCheck.Negation<double4, double>(4, true);

        ArithCheck.Cross<double3, double>(true);
        ArithCheck.SignedCross<double3, double>(true);
    }

    [Test]
    public void Half()
    {
        ArithCheck.Arithmetic<half2, Half>(2, false);
        ArithCheck.Arithmetic<half3, Half>(3, false);
        ArithCheck.Arithmetic<half4, Half>(4, false);

        ArithCheck.Negation<half2, Half>(2, false);
        ArithCheck.Negation<half3, Half>(3, false);
        ArithCheck.Negation<half4, Half>(4, false);

        ArithCheck.Cross<half3, Half>(false);
        ArithCheck.SignedCross<half3, Half>(false);
    }

    [Test]
    public void Short()
    {
        ArithCheck.Arithmetic<short2, short>(2, false);
        ArithCheck.Arithmetic<short3, short>(3, false);
        ArithCheck.Arithmetic<short4, short>(4, false);

        ArithCheck.Negation<short2, short>(2, false);
        ArithCheck.Negation<short3, short>(3, false);
        ArithCheck.Negation<short4, short>(4, false);

        ArithCheck.Cross<short3, short>(false);
        ArithCheck.SignedCross<short3, short>(false);
    }

    [Test]
    public void Int()
    {
        ArithCheck.Arithmetic<int2, int>(2, true);
        ArithCheck.Arithmetic<int3, int>(3, true);
        ArithCheck.Arithmetic<int4, int>(4, true);

        ArithCheck.Negation<int2, int>(2, true);
        ArithCheck.Negation<int3, int>(3, true);
        ArithCheck.Negation<int4, int>(4, true);

        ArithCheck.Cross<int3, int>(true);
        ArithCheck.SignedCross<int3, int>(true);
    }

    [Test]
    public void Long()
    {
        ArithCheck.Arithmetic<long2, long>(2, true);
        ArithCheck.Arithmetic<long3, long>(3, true);
        ArithCheck.Arithmetic<long4, long>(4, true);

        ArithCheck.Negation<long2, long>(2, true);
        ArithCheck.Negation<long3, long>(3, true);
        ArithCheck.Negation<long4, long>(4, true);

        ArithCheck.Cross<long3, long>(true);
        ArithCheck.SignedCross<long3, long>(true);
    }

    [Test]
    public void UShort()
    {
        ArithCheck.Arithmetic<ushort2, ushort>(2, false);
        ArithCheck.Arithmetic<ushort3, ushort>(3, false);
        ArithCheck.Arithmetic<ushort4, ushort>(4, false);

        ArithCheck.Cross<ushort3, ushort>(false);
    }

    [Test]
    public void UInt()
    {
        ArithCheck.Arithmetic<uint2, uint>(2, true);
        ArithCheck.Arithmetic<uint3, uint>(3, true);
        ArithCheck.Arithmetic<uint4, uint>(4, true);

        ArithCheck.Cross<uint3, uint>(true);
    }

    [Test]
    public void ULong()
    {
        ArithCheck.Arithmetic<ulong2, ulong>(2, true);
        ArithCheck.Arithmetic<ulong3, ulong>(3, true);
        ArithCheck.Arithmetic<ulong4, ulong>(4, true);

        ArithCheck.Cross<ulong3, ulong>(true);
    }
}
