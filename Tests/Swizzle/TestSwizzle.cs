using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Swizzle;

/// <summary>
/// The swizzle checks of the generated vectors. Every combination of the components is a member, the checks pick
/// one member of every shape: the whole vector, a part of it, a combination that repeats a component, one that
/// widens a shorter vector, one that narrows a longer vector and the same kind of combination on a vector that is
/// not backed by a simd type. The setters are checked for the components they write and for the components they
/// have to keep, and the padding lane of a 3 component vector is checked to stay zero.
/// </summary>
public class TestSwizzle
{
    #region helpers

    /// <summary>
    /// Checks every component of the vector, the padding lane of a 3 component vector is not part of it.
    /// </summary>
    private static void Check<T, TScalar>(IVector<T, TScalar> actual, IVector<T, TScalar> expected, string what)
        where T : unmanaged, IVector<T, TScalar>
        where TScalar : unmanaged
    {
        using (Assert.EnterMultipleScope())
        {
            for (var i = 0; i < T.Length; i++)
            {
                Assert.That(T.get_at((T)actual, i), Is.EqualTo(T.get_at((T)expected, i)), $"{what}, component {i}");
            }
        }
    }

    /// <summary>
    /// Checks the padding lane of a 3 component vector that is backed by a simd type.
    /// </summary>
    private static void CheckLane(float3 v, float expected, string what)
        => Assert.That(v.vector.GetElement(3), Is.EqualTo(expected), what);

    #endregion

    #region get

    [Test]
    public void Whole()
    {
        var v = new float4(1, 2, 3, 4);
        Check(v.xyzw, v, "xyzw");
        Check(v.rgba, v, "rgba");
        Check(v.yxwz, new float4(2, 1, 4, 3), "yxwz");
        Check(v.wzyx, new float4(4, 3, 2, 1), "wzyx");
        Check(new float3(1, 2, 3).xyz, new float3(1, 2, 3), "float3.xyz");
        Check(new float3(1, 2, 3).zyx, new float3(3, 2, 1), "float3.zyx");
    }

    [Test]
    public void Part()
    {
        var v = new float4(1, 2, 3, 4);
        Check(v.xy, new float2(1, 2), "xy");
        Check(v.xyz, new float3(1, 2, 3), "xyz");
        Check(v.zw, new float2(3, 4), "zw");
        Check(v.yw, new float2(2, 4), "yw");
        Check(v.wz, new float2(4, 3), "wz");
        Check(v.rgb, new float3(1, 2, 3), "rgb");
        Check(v.bgra, new float4(3, 2, 1, 4), "bgra");
        Check(new float3(1, 2, 3).yz, new float2(2, 3), "float3.yz");
    }

    [Test]
    public void StorageVariant()
    {
        // the same sized combination is the variant itself, a combination of another size is a regular vector
        var v2 = new float2s(1, 2);
        Check(v2.xy, new float2s(1, 2), "float2s.xy");
        Check(v2.yx, new float2s(2, 1), "float2s.yx");
        Check(v2.xxx, new float3(1, 1, 1), "float2s.xxx");
        Check(v2.xyxy, new float4(1, 2, 1, 2), "float2s.xyxy");

        var v3 = new float3s(1, 2, 3);
        Check(v3.xyz, new float3s(1, 2, 3), "float3s.xyz");
        Check(v3.zyx, new float3s(3, 2, 1), "float3s.zyx");
        Check(v3.xy, new float2(1, 2), "float3s.xy");
        Check(v3.xyzz, new float4(1, 2, 3, 3), "float3s.xyzz");

        // a setter only writes the components of the combination
        var a = new float2s(1, 2);
        a.yx = new float2s(7, 8);
        Check(a, new float2s(8, 7), "float2s.yx setter");

        var b = new float3s(1, 2, 3);
        b.xy = new float2(7, 8);
        Check(b, new float3s(7, 8, 3), "float3s.xy setter");
    }

    [Test]
    public void Repeat()
    {
        var v = new float4(1, 2, 3, 4);
        Check(v.xxxx, new float4(1, 1, 1, 1), "xxxx");
        Check(v.zzyy, new float4(3, 3, 2, 2), "zzyy");
        Check(v.xx, new float2(1, 1), "xx");
        Check(new float3(1, 2, 3).xxx, new float3(1, 1, 1), "float3.xxx");
        // a 4 length combination of a 3 component vector, the padding lane is not part of it
        Check(new float3(1, 2, 3).xyzz, new float4(1, 2, 3, 3), "float3.xyzz");
    }

    [Test]
    public void Widen()
    {
        var v = new float2(1, 2);
        Check(v.xxx, new float3(1, 1, 1), "float2.xxx");
        Check(v.xyx, new float3(1, 2, 1), "float2.xyx");
        Check(v.xxxy, new float4(1, 1, 1, 2), "float2.xxxy");
        Check(v.yxyx, new float4(2, 1, 2, 1), "float2.yxyx");
        Check(new double2(1, 2).xyyx, new double4(1, 2, 2, 1), "double2.xyyx");
        Check(new int2(1, 2).yxxy, new int4(2, 1, 1, 2), "int2.yxxy");
    }

    [Test]
    public void OtherTypes()
    {
        Check(new double3(1, 2, 3).zyx, new double3(3, 2, 1), "double3.zyx");
        Check(new double4(1, 2, 3, 4).wz, new double2(4, 3), "double4.wz");
        Check(new int3(1, 2, 3).zxy, new int3(3, 1, 2), "int3.zxy");
        Check(new uint4(1, 2, 3, 4).wzyx, new uint4(4, 3, 2, 1), "uint4.wzyx");
        Check(new long2(1, 2).yx, new long2(2, 1), "long2.yx");
        Check(new ulong3(1, 2, 3).zzz, new ulong3(3, 3, 3), "ulong3.zzz");
        Check(new short3(1, 2, 3).zyx, new short3(3, 2, 1), "short3.zyx");
        Check(new ushort2(1, 2).yyx, new ushort3(2, 2, 1), "ushort2.yyx");
        Check(new half3((half)1.0, (half)2.0, (half)3.0).zyx,
            new half3((half)3.0, (half)2.0, (half)1.0), "half3.zyx");
        Check(new b32v3(true, true, false).zyx, new b32v3(false, true, true), "b32v3.zyx");
        Check(new b16v4(true, false, true, false).wx, new b16v2(false, true), "b16v4.wx");
    }

    #endregion

    #region set

    [Test]
    public void SetWhole()
    {
        var v = new float4(1, 2, 3, 4);
        v.xyzw = new float4(5, 6, 7, 8);
        Check(v, new float4(5, 6, 7, 8), "xyzw");
        v.rgba = new float4(9, 10, 11, 12);
        Check(v, new float4(9, 10, 11, 12), "rgba");
        v.wzyx = new float4(1, 2, 3, 4);
        Check(v, new float4(4, 3, 2, 1), "wzyx");
        v.yxwz = new float4(1, 2, 3, 4);
        Check(v, new float4(2, 1, 4, 3), "yxwz");
    }

    [Test]
    public void SetPart()
    {
        // a shorter combination only writes its components and keeps the others
        var v = new float4(1, 2, 3, 4);
        v.zw = new float2(7, 8);
        Check(v, new float4(1, 2, 7, 8), "zw");
        v.xy = new float2(5, 6);
        Check(v, new float4(5, 6, 7, 8), "xy");
        v.wz = new float2(11, 12);
        Check(v, new float4(5, 6, 12, 11), "wz");
        v.rg = new float2(1, 2);
        Check(v, new float4(1, 2, 12, 11), "rg");

        var v3 = new float3(1, 2, 3);
        v3.xy = new float2(7, 8);
        Check(v3, new float3(7, 8, 3), "float3.xy");
        v3.zx = new float2(1, 2);
        Check(v3, new float3(2, 8, 1), "float3.zx");
    }

    [Test]
    public void SetPermutation()
    {
        var v = new float3(1, 2, 3);
        v.zyx = new float3(4, 5, 6);
        Check(v, new float3(6, 5, 4), "float3.zyx");
        v = new float3(1, 2, 3);
        // a combination that is not its own inverse, it writes the components of the value in its own order
        v.zxy = new float3(4, 5, 6);
        Check(v, new float3(5, 6, 4), "float3.zxy");
        v = new float3(1, 2, 3);
        v.xyz = new float3(4, 5, 6);
        Check(v, new float3(4, 5, 6), "float3.xyz");
        v = new float3(1, 2, 3);
        v.rgb = new float3(4, 5, 6);
        Check(v, new float3(4, 5, 6), "float3.rgb");
    }

    [Test]
    public void SetOtherTypes()
    {
        var d = new double4(1, 2, 3, 4);
        d.zw = new double2(5, 6);
        Check(d, new double4(1, 2, 5, 6), "double4.zw");
        var i = new int3(1, 2, 3);
        i.zyx = new int3(4, 5, 6);
        Check(i, new int3(6, 5, 4), "int3.zyx");
        var u = new uint4(1, 2, 3, 4);
        u.xy = new uint2(5, 6);
        Check(u, new uint4(5, 6, 3, 4), "uint4.xy");
        var l = new long3(1, 2, 3);
        l.xz = new long2(5, 6);
        Check(l, new long3(5, 2, 6), "long3.xz");
        var s = new short3(1, 2, 3);
        s.zyx = new short3(4, 5, 6);
        Check(s, new short3(6, 5, 4), "short3.zyx");
        var h = new half3((half)1.0, (half)2.0, (half)3.0);
        h.zy = new half2((half)4.0, (half)5.0);
        Check(h, new half3((half)1.0, (half)5.0, (half)4.0), "half3.zy");
    }

    /// <summary>
    /// A 64 bit vector is not accelerated on every platform, its members fall back to a 128 bit shuffle there
    /// and read the two components from the lower lanes of the wider register.
    /// </summary>
    [Test]
    public void Small()
    {
        var v = new float2(1, 2);
        Check(v.xy, new float2(1, 2), "float2.xy");
        Check(v.xx, new float2(1, 1), "float2.xx");
        Check(v.yx, new float2(2, 1), "float2.yx");
        v.yx = new float2(3, 4);
        Check(v, new float2(4, 3), "float2.yx set");
        v.xy = new float2(5, 6);
        Check(v, new float2(5, 6), "float2.xy set");

        var u = new uint2(1, 2);
        Check(u.yx, new uint2(2, 1), "uint2.yx");
        u.yx = new uint2(7, 8);
        Check(u, new uint2(8, 7), "uint2.yx set");

        var b = new b32v2(true, false);
        Check(b.yx, new b32v2(false, true), "b32v2.yx");
        b.yx = new b32v2(true, false);
        Check(b, new b32v2(false, true), "b32v2.yx set");
    }

    #endregion

    #region padding lane

    /// <summary>
    /// A 3 component vector that is backed by a simd type keeps its padding lane at zero, a combination that
    /// reaches across two registers has to mask it, the widened lanes of a shorter vector are not zero.
    /// </summary>
    [Test]
    public void PaddingLane()
    {
        using (Assert.EnterMultipleScope())
        {
            // a 4 component vector narrowed to 3 components
            CheckLane(new float4(1, 2, 3, 42).xyz, 0f, "float4.xyz");
            CheckLane(new float4(1, 2, 3, 42).zyx, 0f, "float4.zyx");
            CheckLane(new float4(1, 2, 3, 42).xxx, 0f, "float4.xxx");
            // a 2 component vector widened to 3 components
            CheckLane(new float2(1, 2).xxx, 0f, "float2.xxx");
            CheckLane(new float2(1, 2).yxy, 0f, "float2.yxy");
            // one of the vectors own combinations
            CheckLane(new float3(1, 2, 3).zyx, 0f, "float3.zyx");
            CheckLane(new float3(1, 2, 3).xxx, 0f, "float3.xxx");

            // the setters keep it zero too
            var v = new float3(1, 2, 3);
            v.xy = new float2(5, 6);
            Assert.That(v.vector.GetElement(3), Is.EqualTo(0f), "float3.xy set");
            v = new float3(1, 2, 3);
            v.zyx = new float3(4, 5, 6);
            Assert.That(v.vector.GetElement(3), Is.EqualTo(0f), "float3.zyx set");
            v = new float3(1, 2, 3);
            v.zx = new float2(4, 5);
            Assert.That(v.vector.GetElement(3), Is.EqualTo(0f), "float3.zx set");

            Assert.That(new double4(1, 2, 3, 42).xyz.vector.GetElement(3), Is.EqualTo(0d), "double4.xyz");
            Assert.That(new long2(1, 2).xxx.vector.GetElement(3), Is.EqualTo(0L), "long2.xxx");
            Assert.That(new int2(1, 2).yxy.vector.GetElement(3), Is.EqualTo(0), "int2.yxy");
        }
    }

    #endregion
}
