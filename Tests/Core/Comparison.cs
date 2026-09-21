using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

public class TestVectorComparison
{
    // only one comparison result type may be in scope here, otherwise the operators are ambiguous
    private static bool AllLess<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a < b;
    private static bool AllGreater<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a > b;
    private static bool AllLessOrEqual<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a <= b;
    private static bool AllGreaterOrEqual<T>(T a, T b) where T : unmanaged, IComparisonOperators<T, T, bool> => a >= b;
    private static bool IsEqual<T>(T a, T b) where T : unmanaged, IEqualityOperators<T, T, bool> => a == b;

    [Test]
    public void OperatorsReturnTheBoolVector()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(1, 2) == new float2(3, 4), Is.TypeOf<b32v2>());
            Assert.That(new float3(1, 2, 3) < new float3(3, 4, 5), Is.TypeOf<b32v3>());
            Assert.That(new float4(1, 2, 3, 4) > new float4(3, 4, 5, 6), Is.TypeOf<b32v4>());
            Assert.That(new double2(1, 2) == new double2(3, 4), Is.TypeOf<b64v2>());
            Assert.That(new double3(1, 2, 3) != new double3(3, 4, 5), Is.TypeOf<b64v3>());
            Assert.That(new double4(1, 2, 3, 4) <= new double4(3, 4, 5, 6), Is.TypeOf<b64v4>());
            Assert.That(new short2(1, 2) >= new short2(3, 4), Is.TypeOf<b16v2>());
            Assert.That(new short3(1, 2, 3) == new short3(3, 4, 5), Is.TypeOf<b16v3>());
            Assert.That(new short4(1, 2, 3, 4) < new short4(3, 4, 5, 6), Is.TypeOf<b16v4>());
            Assert.That(new ushort2(1, 2) == new ushort2(3, 4), Is.TypeOf<b16v2>());
            Assert.That(new ushort3(1, 2, 3) > new ushort3(3, 4, 5), Is.TypeOf<b16v3>());
            Assert.That(new ushort4(1, 2, 3, 4) == new ushort4(3, 4, 5, 6), Is.TypeOf<b16v4>());
            Assert.That(new int2(1, 2) == new int2(3, 4), Is.TypeOf<b32v2>());
            Assert.That(new int3(1, 2, 3) < new int3(3, 4, 5), Is.TypeOf<b32v3>());
            Assert.That(new int4(1, 2, 3, 4) > new int4(3, 4, 5, 6), Is.TypeOf<b32v4>());
            Assert.That(new uint2(1, 2) == new uint2(3, 4), Is.TypeOf<b32v2>());
            Assert.That(new uint3(1, 2, 3) != new uint3(3, 4, 5), Is.TypeOf<b32v3>());
            Assert.That(new uint4(1, 2, 3, 4) <= new uint4(3, 4, 5, 6), Is.TypeOf<b32v4>());
            Assert.That(new long2(1, 2) == new long2(3, 4), Is.TypeOf<b64v2>());
            Assert.That(new long3(1, 2, 3) >= new long3(3, 4, 5), Is.TypeOf<b64v3>());
            Assert.That(new long4(1, 2, 3, 4) < new long4(3, 4, 5, 6), Is.TypeOf<b64v4>());
            Assert.That(new ulong2(1, 2) == new ulong2(3, 4), Is.TypeOf<b64v2>());
            Assert.That(new ulong3(1, 2, 3) > new ulong3(3, 4, 5), Is.TypeOf<b64v3>());
            Assert.That(new ulong4(1, 2, 3, 4) == new ulong4(3, 4, 5, 6), Is.TypeOf<b64v4>());
            Assert.That(new half2((Half)1, (Half)2) == new half2((Half)3, (Half)4), Is.TypeOf<b16v2>());
            Assert.That(new half3((Half)1, (Half)2, (Half)3) < new half3((Half)3, (Half)4, (Half)5), Is.TypeOf<b16v3>());
            Assert.That(new half4((Half)1, (Half)2, (Half)3, (Half)4) > new half4((Half)3, (Half)4, (Half)5, (Half)6), Is.TypeOf<b16v4>());
        }
    }

    [Test]
    public void MaskValues()
    {
        var f2 = new float2(1, 2);
        var f2b = new float2(1, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2 == f2b), Is.EqualTo(new b32v2(B32.True, B32.False)));
            Assert.That((f2 != f2b), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That((f2 < f2b), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That((f2 > f2b), Is.EqualTo(new b32v2(B32.False, B32.False)));
            Assert.That((f2 <= f2b), Is.EqualTo(new b32v2(B32.True, B32.True)));
            Assert.That((f2 >= f2b), Is.EqualTo(new b32v2(B32.True, B32.False)));
        }

        var f3 = new float3(1, 2, 3);
        var f3b = new float3(1, 4, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((f3 == f3b), Is.EqualTo(new b32v3(B32.True, B32.False, B32.False)));
            Assert.That((f3 < f3b), Is.EqualTo(new b32v3(B32.False, B32.True, B32.False)));
            Assert.That((f3 >= f3b), Is.EqualTo(new b32v3(B32.True, B32.False, B32.True)));
        }

        var d3 = new double3(1, 2, 3);
        var d3b = new double3(1, 4, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((d3 == d3b), Is.EqualTo(new b64v3(B64.True, B64.False, B64.False)));
            Assert.That((d3 > d3b), Is.EqualTo(new b64v3(B64.False, B64.False, B64.True)));
        }

        var i4 = new int4(1, 2, 3, 4);
        var i4b = new int4(0, 2, 5, 4);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((i4 > i4b), Is.EqualTo(new b32v4(B32.True, B32.False, B32.False, B32.False)));
            Assert.That((i4 == i4b), Is.EqualTo(new b32v4(B32.False, B32.True, B32.False, B32.True)));
            Assert.That((i4 <= i4b), Is.EqualTo(new b32v4(B32.False, B32.True, B32.True, B32.True)));
        }

        // masks are compared as unsigned integers
        var u2 = new uint2(0x8000_0000, 1);
        var u2b = new uint2(1, 0x8000_0000);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((u2 < u2b), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That((u2 > u2b), Is.EqualTo(new b32v2(B32.True, B32.False)));
        }

        var s4 = new short4(-1, 2, 3, 4);
        var s4b = new short4(0, 2, 5, 4);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((s4 < s4b), Is.EqualTo(new b16v4(B16.True, B16.False, B16.True, B16.False)));
            Assert.That((s4 == s4b), Is.EqualTo(new b16v4(B16.False, B16.True, B16.False, B16.True)));
        }

        var ul2 = new ulong2(1, 2);
        var ul2b = new ulong2(2, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((ul2 < ul2b), Is.EqualTo(new b64v2(B64.True, B64.False)));
            Assert.That((ul2 != ul2b), Is.EqualTo(new b64v2(B64.True, B64.True)));
        }

        var h2 = new half2((Half)1, (Half)2);
        var h2b = new half2((Half)1, (Half)3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((h2 == h2b), Is.EqualTo(new b16v2(B16.True, B16.False)));
            Assert.That((h2 < h2b), Is.EqualTo(new b16v2(B16.False, B16.True)));
        }
    }

    [Test]
    public void BoolVectorMaskValues()
    {
        var t2 = new b32v2(B32.True, B32.False);
        var t2b = new b32v2(B32.False, B32.True);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((t2 == t2b), Is.EqualTo(new b32v2(B32.False, B32.False)));
            Assert.That((t2 != t2b), Is.EqualTo(new b32v2(B32.True, B32.True)));
            // the masks are compared as unsigned integers, all bits set is the largest
            Assert.That((t2 < t2b), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That((t2 > t2b), Is.EqualTo(new b32v2(B32.True, B32.False)));
            Assert.That((t2 <= t2b), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That((t2 >= t2b), Is.EqualTo(new b32v2(B32.True, B32.False)));
        }

        var e3 = new b16v3(B16.True, B16.False, B16.True);
        var e3b = new b16v3(B16.False, B16.False, B16.True);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((e3 == e3b), Is.EqualTo(new b16v3(B16.False, B16.True, B16.True)));
            Assert.That((e3 > e3b), Is.EqualTo(new b16v3(B16.True, B16.False, B16.False)));
        }

        var q4 = new b64v4(B64.True, B64.False, B64.True, B64.False);
        var q4Copy = new b64v4(B64.True, B64.False, B64.True, B64.False);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((q4 == q4Copy), Is.EqualTo(new b64v4(B64.True, B64.True, B64.True, B64.True)));
            Assert.That((q4 < q4Copy), Is.EqualTo(new b64v4(B64.False, B64.False, B64.False, B64.False)));
        }
    }

    [Test]
    public void InterfaceComparisonIsAllComponents()
    {
        var a = new float2(1, 2);
        var b = new float2(2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(AllLess(a, b), Is.True);
            Assert.That(AllLess(a, new float2(1, 3)), Is.False);
            Assert.That(AllGreater(b, a), Is.True);
            Assert.That(AllGreater(a, new float2(0, 3)), Is.False);
            Assert.That(AllLessOrEqual(a, a), Is.True);
            Assert.That(AllGreaterOrEqual(a, a), Is.True);
            Assert.That(AllLessOrEqual(b, a), Is.False);
            Assert.That(IsEqual(a, a), Is.True);
            Assert.That(IsEqual(a, b), Is.False);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(AllLess(new float3(1, 2, 3), new float3(2, 3, 4)), Is.True);
            Assert.That(AllLess(new float3(1, 2, 3), new float3(2, 3, 3)), Is.False);
            Assert.That(AllLess(new float4(1, 2, 3, 4), new float4(2, 3, 4, 5)), Is.True);
            Assert.That(AllGreater(new double2(3, 4), new double2(1, 2)), Is.True);
            Assert.That(AllGreater(new double3(4, 5, 6), new double3(1, 2, 3)), Is.True);
            Assert.That(AllGreater(new double4(4, 5, 6, 7), new double4(1, 2, 3, 4)), Is.True);
            Assert.That(AllLess(new short2(1, 2), new short2(2, 3)), Is.True);
            Assert.That(AllLessOrEqual(new ushort3(1, 2, 3), new ushort3(1, 2, 3)), Is.True);
            Assert.That(AllLess(new int4(1, 2, 3, 4), new int4(2, 3, 4, 5)), Is.True);
            Assert.That(AllGreater(new uint2(2, 3), new uint2(1, 2)), Is.True);
            Assert.That(AllLess(new long3(1, 2, 3), new long3(2, 3, 4)), Is.True);
            Assert.That(AllGreater(new ulong4(2, 3, 4, 5), new ulong4(1, 2, 3, 4)), Is.True);
            Assert.That(AllLess(new half2((Half)1, (Half)2), new half2((Half)2, (Half)3)), Is.True);
            Assert.That(IsEqual(new half4((Half)1, (Half)2, (Half)3, (Half)4), new half4((Half)1, (Half)2, (Half)3, (Half)4)), Is.True);
        }
    }

    [Test]
    public void Equality()
    {
        var f2 = new float2(1, 2);
        var f2b = new float2(1, 2);
        var f2c = new float2(1, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(f2.Equals(f2b), Is.True);
            Assert.That(f2.Equals(f2c), Is.False);
            Assert.That(f2.Equals((object)f2b), Is.True);
            Assert.That(f2.Equals(null), Is.False);
            Assert.That(f2.Equals(default), Is.False);
            Assert.That(f2.GetHashCode(), Is.EqualTo(f2b.GetHashCode()));
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(1, 2, 3).Equals(new float3(1, 2, 3)), Is.True);
            Assert.That(new float4(1, 2, 3, 4).Equals(new float4(1, 2, 3, 5)), Is.False);
            Assert.That(new double2(1, 2).Equals(new double2(1, 2)), Is.True);
            Assert.That(new double3(1, 2, 3).Equals(new double3(1, 2, 3)), Is.True);
            Assert.That(new double4(1, 2, 3, 4).Equals(new double4(1, 2, 3, 4)), Is.True);
            Assert.That(new short2(1, 2).Equals(new short2(1, 2)), Is.True);
            Assert.That(new ushort3(1, 2, 3).Equals(new ushort3(1, 2, 3)), Is.True);
            Assert.That(new short4(1, 2, 3, 4).Equals(new short4(1, 2, 3, 4)), Is.True);
            Assert.That(new int2(1, 2).Equals(new int2(1, 2)), Is.True);
            Assert.That(new uint3(1, 2, 3).Equals(new uint3(1, 2, 3)), Is.True);
            Assert.That(new int4(1, 2, 3, 4).Equals(new int4(1, 2, 3, 4)), Is.True);
            Assert.That(new long2(1, 2).Equals(new long2(1, 2)), Is.True);
            Assert.That(new ulong3(1, 2, 3).Equals(new ulong3(1, 2, 3)), Is.True);
            Assert.That(new long4(1, 2, 3, 4).Equals(new long4(1, 2, 3, 4)), Is.True);
            Assert.That(new half2((Half)1, (Half)2).Equals(new half2((Half)1, (Half)2)), Is.True);
            Assert.That(new b16v2(B16.True, B16.False).Equals(new b16v2(B16.True, B16.False)), Is.True);
            Assert.That(new b32v3(B32.True, B32.False, B32.True).Equals(new b32v3(B32.True, B32.False, B32.True)), Is.True);
            Assert.That(new b64v4(B64.True, B64.False, B64.True, B64.False).Equals(new b64v4(B64.True, B64.False, B64.True, B64.False)), Is.True);
            Assert.That(b32v2.True.Equals(b32v2.True), Is.True);
            Assert.That(b32v2.True.Equals(b32v2.False), Is.False);
            Assert.That(b32v2.False.Equals(default(b32v2)), Is.True);
        }
    }

    [Test]
    public void CompareTo()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(1, 2).CompareTo(new float2(2, 3)), Is.EqualTo(-1));
            Assert.That(new float2(2, 3).CompareTo(new float2(1, 2)), Is.EqualTo(1));
            Assert.That(new float2(1, 2).CompareTo(new float2(1, 2)), Is.EqualTo(0));
            // any component less wins over another component being greater, this is the same as the last library
            Assert.That(new float2(1, 5).CompareTo(new float2(2, 1)), Is.EqualTo(-1));
            Assert.That(new float2(2, 1).CompareTo(new float2(1, 5)), Is.EqualTo(-1));

            Assert.That(new float3(1, 2, 3).CompareTo(new float3(1, 2, 3)), Is.EqualTo(0));
            Assert.That(new float4(1, 2, 3, 4).CompareTo(new float4(1, 2, 3, 5)), Is.EqualTo(-1));
            Assert.That(new double2(1, 2).CompareTo(new double2(1, 2)), Is.EqualTo(0));
            Assert.That(new double3(1, 2, 3).CompareTo(new double3(1, 2, 4)), Is.EqualTo(-1));
            Assert.That(new double4(1, 2, 3, 5).CompareTo(new double4(1, 2, 3, 4)), Is.EqualTo(1));
            Assert.That(new short2(1, 2).CompareTo(new short2(1, 3)), Is.EqualTo(-1));
            Assert.That(new ushort3(1, 2, 3).CompareTo(new ushort3(1, 2, 3)), Is.EqualTo(0));
            Assert.That(new short4(1, 2, 3, 4).CompareTo(new short4(1, 2, 3, 5)), Is.EqualTo(-1));
            Assert.That(new int2(1, 2).CompareTo(new int2(1, 2)), Is.EqualTo(0));
            Assert.That(new uint3(1, 2, 3).CompareTo(new uint3(1, 2, 4)), Is.EqualTo(-1));
            Assert.That(new int4(1, 2, 3, 5).CompareTo(new int4(1, 2, 3, 4)), Is.EqualTo(1));
            Assert.That(new long2(1, 2).CompareTo(new long2(1, 3)), Is.EqualTo(-1));
            Assert.That(new ulong3(1, 2, 3).CompareTo(new ulong3(1, 2, 3)), Is.EqualTo(0));
            Assert.That(new long4(1, 2, 3, 4).CompareTo(new long4(1, 2, 3, 5)), Is.EqualTo(-1));
            Assert.That(new half2((Half)1, (Half)2).CompareTo(new half2((Half)1, (Half)3)), Is.EqualTo(-1));
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(((IComparable)new float3(1, 2, 3)).CompareTo((object)new float3(1, 2, 3)), Is.EqualTo(0));
            Assert.That(((IComparable)new float3(1, 2, 3)).CompareTo((object)new float3(1, 2, 4)), Is.EqualTo(-1));
            Assert.That(((IComparable)new int4(1, 2, 3, 4)).CompareTo(null), Is.EqualTo(1));
            Assert.That(((IComparable)new double2(1, 2)).CompareTo((object)new double2(1, 2)), Is.EqualTo(0));
            Assert.Throws<ArgumentException>(() => ((IComparable)new float3(1, 2, 3)).CompareTo("not a vector"));
        }
    }

    [Test]
    public void FloatingPointSpecialValues()
    {
        var nan2 = new float2(float.NaN, 1);
        var nan2b = new float2(float.NaN, 1);
        using (Assert.EnterMultipleScope())
        {
            // NaN is not equal to itself, this has to hold for the widened path too
            Assert.That((nan2 == nan2b), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That((nan2 != nan2b), Is.EqualTo(new b32v2(B32.True, B32.False)));
            Assert.That(nan2.Equals(nan2b), Is.False);
            Assert.That(AllLess(nan2, new float2(1, 1)), Is.False);
            Assert.That(AllLessOrEqual(nan2, nan2b), Is.False);
            Assert.That(AllGreaterOrEqual(nan2, nan2b), Is.False);
        }

        var inf2 = new float2(float.PositiveInfinity, float.NegativeInfinity);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((inf2 > new float2(1, 1)), Is.EqualTo(new b32v2(B32.True, B32.False)));
            Assert.That((inf2 < new float2(1, 1)), Is.EqualTo(new b32v2(B32.False, B32.True)));
            Assert.That(inf2.Equals(inf2), Is.True);
        }

        var nan3 = new float3(float.NaN, 1, 2);
        var nan3b = new float3(float.NaN, 1, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(nan3 == nan3b, Is.EqualTo(new b32v3(B32.False, B32.True, B32.True)));
            Assert.That(nan3.Equals(nan3b), Is.False);
        }

        var nan2d = new double2(double.NaN, 1);
        var nan2db = new double2(double.NaN, 1);
        var inf2d = new double2(double.PositiveInfinity, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(nan2d == nan2db, Is.EqualTo(new b64v2(B64.False, B64.True)));
            Assert.That(nan2d.Equals(nan2db), Is.False);
            Assert.That((inf2d > new double2(1, 1)), Is.EqualTo(new b64v2(B64.True, B64.False)));
            Assert.That(inf2d.Equals(inf2d), Is.True);
        }

        var nan2h = new half2(Half.NaN, (Half)1);
        var nan2hb = new half2(Half.NaN, (Half)1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(nan2h == nan2hb, Is.EqualTo(new b16v2(B16.False, B16.True)));
            Assert.That(nan2h.Equals(nan2hb), Is.False);
        }
    }

    [Test]
    public void PaddingLaneIsZeroInMasks()
    {
        var f3 = new float3(1, 2, 3);
        var f3b = new float3(1, 4, 2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((f3 == f3b).vector.GetElement(3), Is.EqualTo(0u));
            // the padding lane of both operands is zero, without the mask this would be all ones
            Assert.That((f3 >= f3b).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((f3 < f3b).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((f3 != f3b).vector.GetElement(3), Is.EqualTo(0u));
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That((new int3(1, 2, 3) < new int3(4, 5, 6)).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((new uint3(1, 2, 3) >= new uint3(1, 2, 4)).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((new double3(1, 2, 3) == new double3(1, 2, 3)).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That((new long3(1, 2, 3) > new long3(4, 5, 6)).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That((new ulong3(1, 2, 3) <= new ulong3(4, 5, 6)).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That((b32v3.True == b32v3.True).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That((b64v3.True >= b64v3.True).vector.GetElement(3), Is.EqualTo(0UL));
            // a plain vector has no padding lane, the components are checked directly
            Assert.That(new short3(1, 2, 3) < new short3(4, 5, 6), Is.EqualTo(new b16v3(B16.True, B16.True, B16.True)));
            Assert.That(new b16v3(B16.True) >= new b16v3(B16.False), Is.EqualTo(b16v3.True));
        }
    }
}
