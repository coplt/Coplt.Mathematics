using Coplt.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

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

    [Test]
    public void Bool16()
    {
        GenericCheck.Bool<b16v2, B16>(2, 4, false, B16.True, B16.True);
        GenericCheck.Bool<b16v3, B16>(3, 8, false, B16.True, B16.True);
        GenericCheck.Bool<b16v4, B16>(4, 8, false, B16.True, B16.True);
    }

    [Test]
    public void Bool32()
    {
        GenericCheck.Bool<b32v2, B32>(2, 8, true, B32.True, B32.True);
        GenericCheck.Bool<b32v3, B32>(3, 16, true, B32.True, B32.True);
        GenericCheck.Bool<b32v4, B32>(4, 16, true, B32.True, B32.True);
    }

    [Test]
    public void Bool64()
    {
        GenericCheck.Bool<b64v2, B64>(2, 16, true, B64.True, B64.True);
        GenericCheck.Bool<b64v3, B64>(3, 32, true, B64.True, B64.True);
        GenericCheck.Bool<b64v4, B64>(4, 32, true, B64.True, B64.True);
    }

    [Test]
    public void NumberMasks()
    {
        GenericCheck.Mask<float2, b32v2, B32>(float2.Broadcast(1), float2.Broadcast(2));
        GenericCheck.Mask<float3, b32v3, B32>(float3.Broadcast(1), float3.Broadcast(2));
        GenericCheck.Mask<float4, b32v4, B32>(float4.Broadcast(1), float4.Broadcast(2));
        GenericCheck.Mask<double2, b64v2, B64>(double2.Broadcast(1), double2.Broadcast(2));
        GenericCheck.Mask<double3, b64v3, B64>(double3.Broadcast(1), double3.Broadcast(2));
        GenericCheck.Mask<double4, b64v4, B64>(double4.Broadcast(1), double4.Broadcast(2));
        GenericCheck.Mask<short2, b16v2, B16>(short2.Broadcast(1), short2.Broadcast(2));
        GenericCheck.Mask<short3, b16v3, B16>(short3.Broadcast(1), short3.Broadcast(2));
        GenericCheck.Mask<short4, b16v4, B16>(short4.Broadcast(1), short4.Broadcast(2));
        GenericCheck.Mask<ushort2, b16v2, B16>(ushort2.Broadcast(1), ushort2.Broadcast(2));
        GenericCheck.Mask<ushort3, b16v3, B16>(ushort3.Broadcast(1), ushort3.Broadcast(2));
        GenericCheck.Mask<ushort4, b16v4, B16>(ushort4.Broadcast(1), ushort4.Broadcast(2));
        GenericCheck.Mask<int2, b32v2, B32>(int2.Broadcast(1), int2.Broadcast(2));
        GenericCheck.Mask<int3, b32v3, B32>(int3.Broadcast(1), int3.Broadcast(2));
        GenericCheck.Mask<int4, b32v4, B32>(int4.Broadcast(1), int4.Broadcast(2));
        GenericCheck.Mask<uint2, b32v2, B32>(uint2.Broadcast(1), uint2.Broadcast(2));
        GenericCheck.Mask<uint3, b32v3, B32>(uint3.Broadcast(1), uint3.Broadcast(2));
        GenericCheck.Mask<uint4, b32v4, B32>(uint4.Broadcast(1), uint4.Broadcast(2));
        GenericCheck.Mask<long2, b64v2, B64>(long2.Broadcast(1), long2.Broadcast(2));
        GenericCheck.Mask<long3, b64v3, B64>(long3.Broadcast(1), long3.Broadcast(2));
        GenericCheck.Mask<long4, b64v4, B64>(long4.Broadcast(1), long4.Broadcast(2));
        GenericCheck.Mask<ulong2, b64v2, B64>(ulong2.Broadcast(1), ulong2.Broadcast(2));
        GenericCheck.Mask<ulong3, b64v3, B64>(ulong3.Broadcast(1), ulong3.Broadcast(2));
        GenericCheck.Mask<ulong4, b64v4, B64>(ulong4.Broadcast(1), ulong4.Broadcast(2));
        GenericCheck.Mask<half2, b16v2, B16>(half2.Broadcast((Half)1), half2.Broadcast((Half)2));
        GenericCheck.Mask<half3, b16v3, B16>(half3.Broadcast((Half)1), half3.Broadcast((Half)2));
        GenericCheck.Mask<half4, b16v4, B16>(half4.Broadcast((Half)1), half4.Broadcast((Half)2));
    }

    [Test]
    public void BoolMasks()
    {
        GenericCheck.BoolMask<b16v2, B16>();
        GenericCheck.BoolMask<b16v3, B16>();
        GenericCheck.BoolMask<b16v4, B16>();
        GenericCheck.BoolMask<b32v2, B32>();
        GenericCheck.BoolMask<b32v3, B32>();
        GenericCheck.BoolMask<b32v4, B32>();
        GenericCheck.BoolMask<b64v2, B64>();
        GenericCheck.BoolMask<b64v3, B64>();
        GenericCheck.BoolMask<b64v4, B64>();
    }
}
