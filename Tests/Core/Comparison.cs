using System.Numerics;
using Coplt.Mathematics;

namespace Tests.Core;

/// <summary>
/// A comparison of two values builds the value of the kind of the values itself: the all bits set value of the
/// kind is the conventional true component and the zero of it is the conventional false one, so the value of a
/// comparison is compared by its bits, which the members below read.
/// </summary>
public class TestVectorComparison
{
    // only one comparison result type may be in scope here, otherwise the operators are ambiguous
    private static bool AllLess<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a < b;
    private static bool AllGreater<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a > b;
    private static bool AllLessOrEqual<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a <= b;
    private static bool AllGreaterOrEqual<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a >= b;
    private static bool IsEqual<T>(T a, T b) where T : unmanaged, IEqualityOperators<T, T, bool> => a == b;

    /// <summary>The bits of the component of a comparison, the all bits set value is the conventional true one</summary>
    private static uint Bits(float x) => BitConverter.SingleToUInt32Bits(x);

    /// <inheritdoc cref="Bits(float)"/>
    private static ulong Bits(double x) => BitConverter.DoubleToUInt64Bits(x);

    /// <inheritdoc cref="Bits(float)"/>
    private static ushort Bits(Half x) => BitConverter.HalfToUInt16Bits(x);

    /// <summary>The bits of the component of a floating point comparison that holds</summary>
    private const uint TrueF = uint.MaxValue;
    private const ulong TrueD = ulong.MaxValue;
    private const ushort TrueH = 0xFFFF;

    [Test]
    public void OperatorsReturnTheValueItself()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(1, 2) == new float2(3, 4), Is.TypeOf<float2>());
            Assert.That(new float3(1, 2, 3) < new float3(3, 4, 5), Is.TypeOf<float3>());
            Assert.That(new float4(1, 2, 3, 4) > new float4(3, 4, 5, 6), Is.TypeOf<float4>());
            Assert.That(new double2(1, 2) == new double2(3, 4), Is.TypeOf<double2>());
            Assert.That(new double3(1, 2, 3) != new double3(3, 4, 5), Is.TypeOf<double3>());
            Assert.That(new double4(1, 2, 3, 4) <= new double4(3, 4, 5, 6), Is.TypeOf<double4>());
            Assert.That(new short2(1, 2) >= new short2(3, 4), Is.TypeOf<short2>());
            Assert.That(new short3(1, 2, 3) == new short3(3, 4, 5), Is.TypeOf<short3>());
            Assert.That(new short4(1, 2, 3, 4) < new short4(3, 4, 5, 6), Is.TypeOf<short4>());
            Assert.That(new ushort2(1, 2) == new ushort2(3, 4), Is.TypeOf<ushort2>());
            Assert.That(new ushort3(1, 2, 3) > new ushort3(3, 4, 5), Is.TypeOf<ushort3>());
            Assert.That(new ushort4(1, 2, 3, 4) == new ushort4(3, 4, 5, 6), Is.TypeOf<ushort4>());
            Assert.That(new int2(1, 2) == new int2(3, 4), Is.TypeOf<int2>());
            Assert.That(new int3(1, 2, 3) < new int3(3, 4, 5), Is.TypeOf<int3>());
            Assert.That(new int4(1, 2, 3, 4) > new int4(3, 4, 5, 6), Is.TypeOf<int4>());
            Assert.That(new uint2(1, 2) == new uint2(3, 4), Is.TypeOf<uint2>());
            Assert.That(new uint3(1, 2, 3) != new uint3(3, 4, 5), Is.TypeOf<uint3>());
            Assert.That(new uint4(1, 2, 3, 4) <= new uint4(3, 4, 5, 6), Is.TypeOf<uint4>());
            Assert.That(new long2(1, 2) == new long2(3, 4), Is.TypeOf<long2>());
            Assert.That(new long3(1, 2, 3) >= new long3(3, 4, 5), Is.TypeOf<long3>());
            Assert.That(new long4(1, 2, 3, 4) < new long4(3, 4, 5, 6), Is.TypeOf<long4>());
            Assert.That(new ulong2(1, 2) == new ulong2(3, 4), Is.TypeOf<ulong2>());
            Assert.That(new ulong3(1, 2, 3) > new ulong3(3, 4, 5), Is.TypeOf<ulong3>());
            Assert.That(new ulong4(1, 2, 3, 4) == new ulong4(3, 4, 5, 6), Is.TypeOf<ulong4>());
            Assert.That(new half2((Half)1, (Half)2) == new half2((Half)3, (Half)4), Is.TypeOf<half2>());
            Assert.That(new half3((Half)1, (Half)2, (Half)3) < new half3((Half)3, (Half)4, (Half)5), Is.TypeOf<half3>());
            Assert.That(new half4((Half)1, (Half)2, (Half)3, (Half)4) > new half4((Half)3, (Half)4, (Half)5, (Half)6), Is.TypeOf<half4>());
        }
    }

    [Test]
    public void MaskValues()
    {
        var f2 = new float2(1, 2);
        var f2b = new float2(1, 3);
        var f2Eq = f2 == f2b;
        var f2Ne = f2 != f2b;
        var f2Lt = f2 < f2b;
        var f2Gt = f2 > f2b;
        var f2Le = f2 <= f2b;
        var f2Ge = f2 >= f2b;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((Bits(f2Eq.x), Bits(f2Eq.y)), Is.EqualTo((TrueF, 0u)));
            Assert.That((Bits(f2Ne.x), Bits(f2Ne.y)), Is.EqualTo((0u, TrueF)));
            Assert.That((Bits(f2Lt.x), Bits(f2Lt.y)), Is.EqualTo((0u, TrueF)));
            Assert.That((Bits(f2Gt.x), Bits(f2Gt.y)), Is.EqualTo((0u, 0u)));
            Assert.That((Bits(f2Le.x), Bits(f2Le.y)), Is.EqualTo((TrueF, TrueF)));
            Assert.That((Bits(f2Ge.x), Bits(f2Ge.y)), Is.EqualTo((TrueF, 0u)));
        }

        var f3 = new float3(1, 2, 3);
        var f3b = new float3(1, 4, 2);
        var f3Eq = f3 == f3b;
        var f3Lt = f3 < f3b;
        var f3Ge = f3 >= f3b;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((Bits(f3Eq.x), Bits(f3Eq.y), Bits(f3Eq.z)), Is.EqualTo((TrueF, 0u, 0u)));
            Assert.That((Bits(f3Lt.x), Bits(f3Lt.y), Bits(f3Lt.z)), Is.EqualTo((0u, TrueF, 0u)));
            Assert.That((Bits(f3Ge.x), Bits(f3Ge.y), Bits(f3Ge.z)), Is.EqualTo((TrueF, 0u, TrueF)));
        }

        var d3 = new double3(1, 2, 3);
        var d3b = new double3(1, 4, 2);
        var d3Eq = d3 == d3b;
        var d3Gt = d3 > d3b;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((Bits(d3Eq.x), Bits(d3Eq.y), Bits(d3Eq.z)), Is.EqualTo((TrueD, 0ul, 0ul)));
            Assert.That((Bits(d3Gt.x), Bits(d3Gt.y), Bits(d3Gt.z)), Is.EqualTo((0ul, 0ul, TrueD)));
        }

        var i4 = new int4(1, 2, 3, 4);
        var i4b = new int4(0, 2, 5, 4);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((i4 > i4b), Is.EqualTo(new int4(-1, 0, 0, 0)));
            Assert.That((i4 == i4b), Is.EqualTo(new int4(0, -1, 0, -1)));
            Assert.That((i4 <= i4b), Is.EqualTo(new int4(0, -1, -1, -1)));
        }

        // the comparison of the unsigned types is unsigned
        var u2 = new uint2(0x8000_0000, 1);
        var u2b = new uint2(1, 0x8000_0000);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((u2 < u2b), Is.EqualTo(new uint2(0u, uint.MaxValue)));
            Assert.That((u2 > u2b), Is.EqualTo(new uint2(uint.MaxValue, 0u)));
        }

        var s4 = new short4(-1, 2, 3, 4);
        var s4b = new short4(0, 2, 5, 4);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((s4 < s4b), Is.EqualTo(new short4(-1, 0, -1, 0)));
            Assert.That((s4 == s4b), Is.EqualTo(new short4(0, -1, 0, -1)));
        }

        var ul2 = new ulong2(1, 2);
        var ul2b = new ulong2(2, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((ul2 < ul2b), Is.EqualTo(new ulong2(ulong.MaxValue, 0ul)));
            Assert.That((ul2 != ul2b), Is.EqualTo(new ulong2(ulong.MaxValue, ulong.MaxValue)));
        }

        var h2 = new half2((Half)1, (Half)2);
        var h2b = new half2((Half)1, (Half)3);
        var h2Eq = h2 == h2b;
        var h2Lt = h2 < h2b;
        using (Assert.EnterMultipleScope())
        {
            Assert.That((Bits(h2Eq.x), Bits(h2Eq.y)), Is.EqualTo((TrueH, (ushort)0)));
            Assert.That((Bits(h2Lt.x), Bits(h2Lt.y)), Is.EqualTo(((ushort)0, TrueH)));
        }
    }
}
