using Coplt.Mathematics;

namespace Tests.Core;

/// <summary>
/// Runs the interface only checks over every generated vector, see <see cref="GenericCheck"/>.
/// </summary>
public class TestGenericVector
{
    [Test]
    public void Float()
    {
        GenericCheck.Number<float2, float>(2, 8, true, 1, 2);
        GenericCheck.Number<float3, float>(3, 16, true, 1, 2);
        GenericCheck.Number<float4, float>(4, 16, true, 1, 2);
    }

    [Test]
    public void Double()
    {
        GenericCheck.Number<double2, double>(2, 16, true, 1, 2);
        GenericCheck.Number<double3, double>(3, 32, true, 1, 2);
        GenericCheck.Number<double4, double>(4, 32, true, 1, 2);
    }

    [Test]
    public void Short()
    {
        GenericCheck.Number<short2, short>(2, 4, false, 1, 2);
        GenericCheck.Number<short3, short>(3, 8, false, 1, 2);
        GenericCheck.Number<short4, short>(4, 8, false, 1, 2);
    }

    [Test]
    public void UShort()
    {
        GenericCheck.Number<ushort2, ushort>(2, 4, false, 1, 2);
        GenericCheck.Number<ushort3, ushort>(3, 8, false, 1, 2);
        GenericCheck.Number<ushort4, ushort>(4, 8, false, 1, 2);
    }

    [Test]
    public void Int()
    {
        GenericCheck.Number<int2, int>(2, 8, true, 1, 2);
        GenericCheck.Number<int3, int>(3, 16, true, 1, 2);
        GenericCheck.Number<int4, int>(4, 16, true, 1, 2);
    }

    [Test]
    public void UInt()
    {
        GenericCheck.Number<uint2, uint>(2, 8, true, 1, 2);
        GenericCheck.Number<uint3, uint>(3, 16, true, 1, 2);
        GenericCheck.Number<uint4, uint>(4, 16, true, 1, 2);
    }

    [Test]
    public void Long()
    {
        GenericCheck.Number<long2, long>(2, 16, true, 1, 2);
        GenericCheck.Number<long3, long>(3, 32, true, 1, 2);
        GenericCheck.Number<long4, long>(4, 32, true, 1, 2);
    }

    [Test]
    public void ULong()
    {
        GenericCheck.Number<ulong2, ulong>(2, 16, true, 1, 2);
        GenericCheck.Number<ulong3, ulong>(3, 32, true, 1, 2);
        GenericCheck.Number<ulong4, ulong>(4, 32, true, 1, 2);
    }

    [Test]
    public void Half()
    {
        GenericCheck.Number<half2, Half>(2, 4, false, (Half)1, (Half)2);
        GenericCheck.Number<half3, Half>(3, 8, false, (Half)1, (Half)2);
        GenericCheck.Number<half4, Half>(4, 8, false, (Half)1, (Half)2);
    }

}
