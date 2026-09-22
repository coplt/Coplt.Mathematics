using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The bits of the value of a vector are reachable through the underlying interface of the width of its
/// register: the interface of a vector without a register only marks it, the members of the other ones return
/// the bits of the value as a vector of bytes and build a value from them, they tell whether the value fills
/// the register of the vector and give the mask of the lanes of it that the value does not reach. A value that
/// is built from raw bits goes through the constructor of the vector, which keeps the lanes beyond its value at
/// zero, and the unsafe member of the interface writes the bits as they are.
/// </summary>
public class TestVectorUnderlying
{
    /// <summary>Reads the bits of a vector of a 64 bit register and builds the same vector from them</summary>
    private static T Again64<T>(T v) where T : unmanaged, IVector64Underlying<T> =>
        T.FromUnderlying(T.GetUnderlying(v));

    /// <summary>Reads the bits of a vector of a 128 bit register and builds the same vector from them</summary>
    private static T Again128<T>(T v) where T : unmanaged, IVector128Underlying<T> =>
        T.FromUnderlying(T.GetUnderlying(v));

    /// <summary>Reads the bits of a vector of a 256 bit register and builds the same vector from them</summary>
    private static T Again256<T>(T v) where T : unmanaged, IVector256Underlying<T> =>
        T.FromUnderlying(T.GetUnderlying(v));

    [Test]
    public void Width()
    {
        var b = Again128(new b32v4(true, false, true, false));
        var q = Again256(new b64v4(true, false, true, false));
        using (Assert.EnterMultipleScope())
        {
            // a vector of 2 32 bit components keeps its value in a 64 bit register, it is the storage variant
            // of the regular vector of 2 components
            Assert.That(Again64(new float2s(1, 2)), Is.EqualTo(new float2s(1, 2)));
            Assert.That(Again64(new int2s(1, 2)), Is.EqualTo(new int2s(1, 2)));
            // the register of a vector of 4 32 bit components is 128 bits wide
            Assert.That(Again128(new float2(1, 2)), Is.EqualTo(new float2(1, 2)));
            Assert.That(Again128(new float3(1, 2, 3)), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(Again128(new float4(1, 2, 3, 4)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(Again128(new int4(1, 2, 3, 4)), Is.EqualTo(new int4(1, 2, 3, 4)));
            Assert.That(Again128(new uint4(1, 2, 3, 4)), Is.EqualTo(new uint4(1, 2, 3, 4)));
            Assert.That(((bool)b.x, (bool)b.y, (bool)b.z, (bool)b.w), Is.EqualTo((true, false, true, false)));
            // the register of a vector of 4 64 bit components is 256 bits wide
            Assert.That(Again256(new double4(1, 2, 3, 4)), Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That(Again256(new long4(1, 2, 3, 4)), Is.EqualTo(new long4(1, 2, 3, 4)));
            Assert.That(Again256(new ulong4(1, 2, 3, 4)), Is.EqualTo(new ulong4(1, 2, 3, 4)));
            Assert.That(((bool)q.x, (bool)q.y, (bool)q.z, (bool)q.w), Is.EqualTo((true, false, true, false)));
        }
    }

    [Test]
    public void Bits()
    {
        // 1f is 0x3F800000, 2f 0x40000000, 3f 0x40400000 and 4f 0x40800000
        var bits = float4.GetUnderlying(new float4(1, 2, 3, 4));
        using (Assert.EnterMultipleScope())
        {
            // the bits of the value are the bits of its register, so a vector of another component type of the
            // same width reads them as they are
            Assert.That(uint4.FromUnderlying(bits),
                Is.EqualTo(new uint4(0x3F800000u, 0x40000000u, 0x40400000u, 0x40800000u)));
            Assert.That(float4.FromUnderlying(bits), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float4.UnsafeFromUnderlying(bits), Is.EqualTo(new float4(1, 2, 3, 4)));
            // the bits of a value are readable as bytes, so the bytes of the register are the bytes of it
            Assert.That(bits.As<byte, uint>()[0], Is.EqualTo(0x3F800000u));
            Assert.That(bits.As<byte, uint>()[3], Is.EqualTo(0x40800000u));
            // the register of a vector of 3 components has a padding lane that its value does not reach
            Assert.That(float3.GetUnderlying(float3.FromUnderlying(bits)).As<byte, uint>()[3], Is.EqualTo(0u));
            Assert.That((float3.FromUnderlying(bits).x, float3.FromUnderlying(bits).z), Is.EqualTo((1f, 3f)));
            // the bytes of a value of a 256 bit register are readable as well
            Assert.That(double4.GetUnderlying(new double4(1, 2, 3, 4)).As<byte, double>()[0], Is.EqualTo(1d));
            Assert.That(long4.UnsafeFromUnderlying(double4.GetUnderlying(new double4(1, 2, 3, 4))),
                Is.EqualTo(new long4(0x3FF0000000000000L, 0x4000000000000000L, 0x4008000000000000L, 0x4010000000000000L)));
        }
    }

    [Test]
    public void Mask()
    {
        // the last lane of the bits of a vector of 4 components is the one the value of a vector of 3
        // components does not reach
        var bits = float4.GetUnderlying(new float4(1, 2, 3, 4));
        var masked = float3.GetUnderlying(float3.FromUnderlying(bits)).As<byte, uint>();
        var raw = float3.GetUnderlying(float3.UnsafeFromUnderlying(bits)).As<byte, uint>();
        using (Assert.EnterMultipleScope())
        {
            // the constructor of the vector keeps the padding lane at zero
            Assert.That(masked[3], Is.EqualTo(0u));
            Assert.That(raw[3], Is.EqualTo(0x40800000u));
            // the lane is the only difference, the components of the value are the ones of the bits
            Assert.That((masked[0], masked[1], masked[2]), Is.EqualTo((0x3F800000u, 0x40000000u, 0x40400000u)));
            Assert.That((raw[0], raw[1], raw[2]), Is.EqualTo((0x3F800000u, 0x40000000u, 0x40400000u)));
        }
    }

    [Test]
    public void PaddingLanes()
    {
        // the bits of a vector of 4 components hold a value of 4 components in a register of 3 lanes
        var bits = float4.GetUnderlying(new float4(1, 2, 3, 4));
        var raw = float3.GetUnderlying(float3.UnsafeFromUnderlying(bits)).As<byte, uint>();
        var mask = float3.PaddingLanesMask.As<byte, uint>();
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector of 2 32 bit components does not fill the register of 128 bits it is kept in
            Assert.That(float2.HavePaddingLanes, Is.True);
            Assert.That(b32v2.HavePaddingLanes, Is.True);
            // the value of a vector of 4 32 bit components fills its register, so it has no padding lane
            Assert.That(float4.HavePaddingLanes, Is.False);
            Assert.That(b32v4.HavePaddingLanes, Is.False);
            // a register of 64 bits is exactly as wide as the value it keeps
            Assert.That(float2s.HavePaddingLanes, Is.False);
            // the register of a vector of 3 components is as wide as the one of a vector of 4 components
            Assert.That(float3.HavePaddingLanes, Is.True);
            Assert.That(double3.HavePaddingLanes, Is.True);
            Assert.That(double4.HavePaddingLanes, Is.False);
            // every bit of a lane of the value is set in the mask and the lane the value does not reach is zero
            Assert.That((mask[0], mask[1], mask[2]), Is.EqualTo((uint.MaxValue, uint.MaxValue, uint.MaxValue)));
            Assert.That(mask[3], Is.EqualTo(0u));
            // the mask keeps the value as it is and drops what the unsafe member wrote beyond it
            Assert.That(raw[3], Is.EqualTo(0x40800000u));
            Assert.That((raw[0] & mask[0], raw[1] & mask[1], raw[2] & mask[2]),
                Is.EqualTo((raw[0], raw[1], raw[2])));
            Assert.That(raw[3] & mask[3], Is.EqualTo(0u));
            // the mask of a vector that has no padding lane keeps every bit of a value
            Assert.That(float4.PaddingLanesMask.As<byte, uint>()[3], Is.EqualTo(uint.MaxValue));
            Assert.That(double4.PaddingLanesMask.As<byte, ulong>()[3], Is.EqualTo(ulong.MaxValue));
        }
    }

    [Test]
    public void Soft()
    {
        using (Assert.EnterMultipleScope())
        {
            // a vector without a register has no bits to reach, it is only marked
            Assert.That(typeof(IVectorSoftUnderlying).IsAssignableFrom(typeof(half4)), Is.True);
            Assert.That(typeof(IVectorSoftUnderlying).IsAssignableFrom(typeof(double3s)), Is.True);
            Assert.That(typeof(IVectorSoftUnderlying).IsAssignableFrom(typeof(b16v4)), Is.True);
            // the other ones carry the underlying members of the width of their register
            Assert.That(typeof(IVectorSoftUnderlying).IsAssignableFrom(typeof(float2s)), Is.False);
            Assert.That(typeof(IVectorSoftUnderlying).IsAssignableFrom(typeof(float4)), Is.False);
            Assert.That(typeof(IVector256Underlying<double4>).IsAssignableFrom(typeof(double4)), Is.True);
            Assert.That(typeof(IVector64Underlying<float2s>).IsAssignableFrom(typeof(float2s)), Is.True);
        }
    }
}
