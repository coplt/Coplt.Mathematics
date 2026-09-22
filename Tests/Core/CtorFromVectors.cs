using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

/// <summary>
/// The create members of a vector implement <c>IVectorCtor</c>: a vector is created from its own components, a
/// longer one is created by merging a pair of the components of a shorter vector into it as well, and a vector
/// of 4 components is also created by merging a triple of them.
/// <para>The name of a member is the name of the components it takes from its arguments, so the pair of
/// <c>Create(xy, z)</c> holds the <c>x</c> and <c>y</c> components, and the pair of <c>InsertY(xz, y)</c> holds
/// the <c>x</c> and <c>z</c> ones because the <c>y</c> one comes from the value behind it</para>
/// <para>The interface can only be named with its type parameters, so the caller passes them and the components
/// of the result are read through the indexer of the vector interface. Every member has an accelerated form
/// that the register of a simd backed vector takes and a component wise form that a vector without a register
/// takes, both of them are covered by the types below</para>
/// </summary>
public class TestVectorCtorFromVectors
{
    /// <summary>
    /// Checks every member that creates a vector of 3 components out of a pair of components and a value.
    /// </summary>
    /// <param name="xy">The pair that holds the <c>x</c> and <c>y</c> components</param>
    /// <param name="yz">The pair that holds the <c>y</c> and <c>z</c> components</param>
    /// <param name="xz">The pair that holds the <c>x</c> and <c>z</c> components</param>
    /// <param name="a">The value of the component that the pair does not hold</param>
    private static void Check3<T, TScalar, TVector2>(TVector2 xy, TVector2 yz, TVector2 xz, TScalar a)
        where T : unmanaged, IVector<T, TScalar>, IVector3CtorFromVector2<T, TScalar, TVector2>
        where TScalar : unmanaged
        where TVector2 : unmanaged, IVector<TVector2, TScalar>
    {
        using (Assert.EnterMultipleScope())
        {
            var v = T.Create(xy, a);
            Assert.That((v[0], v[1], v[2]), Is.EqualTo((xy[0], xy[1], a)));
            v = T.Create(a, yz);
            Assert.That((v[0], v[1], v[2]), Is.EqualTo((a, yz[0], yz[1])));
            v = T.InsertY(xz, a);
            Assert.That((v[0], v[1], v[2]), Is.EqualTo((xz[0], a, xz[1])));
        }
    }

    /// <summary>
    /// Checks every member that creates a vector of 4 components out of two pairs of components.
    /// </summary>
    /// <param name="xy">The pair that holds the <c>x</c> and <c>y</c> components</param>
    /// <param name="zw">The pair that holds the <c>z</c> and <c>w</c> components</param>
    /// <param name="yz">The pair that holds the <c>y</c> and <c>z</c> components</param>
    /// <param name="xz">The pair that holds the <c>x</c> and <c>z</c> components</param>
    /// <param name="xw">The pair that holds the <c>x</c> and <c>w</c> components</param>
    /// <param name="yw">The pair that holds the <c>y</c> and <c>w</c> components</param>
    /// <param name="a">The value of the <c>y</c> or the <c>x</c> component of the pairs that carry one</param>
    /// <param name="b">The value of the <c>z</c> or the <c>w</c> component of the pairs that carry one</param>
    private static void Check4FromPairs<T, TScalar, TVector2>(
        TVector2 xy, TVector2 zw, TVector2 yz, TVector2 xz, TVector2 xw, TVector2 yw, TScalar a, TScalar b)
        where T : unmanaged, IVector<T, TScalar>, IVector4CtorFromVector2<T, TScalar, TVector2>
        where TScalar : unmanaged
        where TVector2 : unmanaged, IVector<TVector2, TScalar>
    {
        using (Assert.EnterMultipleScope())
        {
            // the first pair of the arguments is the beginning of the vector and the second one its end
            var v = T.Create(xy, zw);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xy[0], xy[1], zw[0], zw[1])));
            v = T.Create(xy, a, b);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xy[0], xy[1], a, b)));
            v = T.Create(a, b, zw);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((a, b, zw[0], zw[1])));
            v = T.Create(a, yz, b);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((a, yz[0], yz[1], b)));
            // the pair of an insert holds the components of the vector that the member names
            v = T.InsertYZ(xw, yz);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xw[0], yz[0], yz[1], xw[1])));
            v = T.InsertYZ(xw, a, b);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xw[0], a, b, xw[1])));
            v = T.InsertXW(yz, xw);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xw[0], yz[0], yz[1], xw[1])));
            v = T.InsertXW(yz, a, b);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((a, yz[0], yz[1], b)));
            v = T.InsertYW(xz, yw);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xz[0], yw[0], xz[1], yw[1])));
            v = T.InsertYW(xz, a, b);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xz[0], a, xz[1], b)));
            v = T.InsertXZ(yw, xz);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xz[0], yw[0], xz[1], yw[1])));
            v = T.InsertXZ(yw, a, b);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((a, yw[0], b, yw[1])));
        }
    }

    /// <summary>
    /// Checks every member that creates a vector of 4 components out of a triple of components and a value.
    /// </summary>
    /// <param name="xyz">The triple that holds the <c>x</c>, <c>y</c> and <c>z</c> components</param>
    /// <param name="yzw">The triple that holds the <c>y</c>, <c>z</c> and <c>w</c> components</param>
    /// <param name="xzw">The triple that holds the <c>x</c>, <c>z</c> and <c>w</c> components</param>
    /// <param name="xyw">The triple that holds the <c>x</c>, <c>y</c> and <c>w</c> components</param>
    /// <param name="a">The value of the component that the triple does not hold</param>
    private static void Check4FromTriples<T, TScalar, TVector3>(
        TVector3 xyz, TVector3 yzw, TVector3 xzw, TVector3 xyw, TScalar a)
        where T : unmanaged, IVector<T, TScalar>, IVector4CtorFromVector3<T, TScalar, TVector3>
        where TScalar : unmanaged
        where TVector3 : unmanaged, IVector<TVector3, TScalar>
    {
        using (Assert.EnterMultipleScope())
        {
            var v = T.Create(xyz, a);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xyz[0], xyz[1], xyz[2], a)));
            v = T.Create(a, yzw);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((a, yzw[0], yzw[1], yzw[2])));
            v = T.InsertY(xzw, a);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xzw[0], a, xzw[1], xzw[2])));
            v = T.InsertZ(xyw, a);
            Assert.That((v[0], v[1], v[2], v[3]), Is.EqualTo((xyw[0], xyw[1], a, xyw[2])));
        }
    }

    /// <summary>
    /// Checks the members that create a vector from its own components, they are the base of every other
    /// create.
    /// </summary>
    [Test]
    public void FromComponents()
    {
        var f2 = float2.Create(1, 2);
        var f3 = float3.Create(1, 2, 3);
        var f4 = float4.Create(1, 2, 3, 4);
        var d2 = double2.Create(1, 2);
        var d3 = double3.Create(1, 2, 3);
        var d4 = double4.Create(1, 2, 3, 4);
        var i3 = int3.Create(1, 2, 3);
        var u4 = uint4.Create(1, 2, 3, 4);
        var h3 = half3.Create((Half)1, (Half)2, (Half)3);
        var t3 = b32v3.Create(B32.True, B32.False, B32.True);
        var e2 = b16v2.Create(B16.True, B16.False);
        var q4 = b64v4.Create(B64.True, B64.False, B64.True, B64.False);
        var f2s = float2s.Create(1, 2);
        var f3s = float3s.Create(1, 2, 3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((f2.x, f2.y), Is.EqualTo((1f, 2f)));
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((f4.x, f4.y, f4.z, f4.w), Is.EqualTo((1f, 2f, 3f, 4f)));
            Assert.That((d2.x, d2.y), Is.EqualTo((1d, 2d)));
            Assert.That((d3.x, d3.y, d3.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((d4.x, d4.y, d4.z, d4.w), Is.EqualTo((1d, 2d, 3d, 4d)));
            Assert.That((i3.x, i3.y, i3.z), Is.EqualTo((1, 2, 3)));
            Assert.That((u4.x, u4.y, u4.z, u4.w), Is.EqualTo((1u, 2u, 3u, 4u)));
            Assert.That((h3.x, h3.y, h3.z), Is.EqualTo(((Half)1, (Half)2, (Half)3)));
            Assert.That((t3.x, t3.y, t3.z), Is.EqualTo((B32.True, B32.False, B32.True)));
            Assert.That((e2.x, e2.y), Is.EqualTo((B16.True, B16.False)));
            Assert.That((q4.x, q4.y, q4.z, q4.w), Is.EqualTo((B64.True, B64.False, B64.True, B64.False)));
            Assert.That((f2s.x, f2s.y), Is.EqualTo((1f, 2f)));
            Assert.That((f3s.x, f3s.y, f3s.z), Is.EqualTo((1f, 2f, 3f)));
        }
    }

    /// <summary>
    /// Covers the merge of a pair and a value of every kind of a 3 component vector: a component of 4 bytes
    /// whose vector is backed by a 128 bit register, one of 8 bytes whose vector is backed by a 256 bit one,
    /// one of 2 bytes whose vector has no register at all, a bool vector and the storage variant of a vector.
    /// </summary>
    [Test]
    public void Merge3()
    {
        Check3<float3, float, float2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<double3, double, double2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<int3, int, int2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<uint3, uint, uint2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<long3, long, long2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<ulong3, ulong, ulong2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<short3, short, short2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<ushort3, ushort, ushort2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<half3, Half, half2>(new((Half)1, (Half)2), new((Half)3, (Half)4), new((Half)5, (Half)6), (Half)7);
        Check3<b32v3, b32, b32v2>(new(B32.True, B32.False), new(B32.False, B32.True), new(B32.True, B32.True), B32.False);
        Check3<b64v3, b64, b64v2>(new(B64.True, B64.False), new(B64.False, B64.True), new(B64.True, B64.True), B64.False);
        Check3<b16v3, b16, b16v2>(new(B16.True, B16.False), new(B16.False, B16.True), new(B16.True, B16.True), B16.False);
        // the storage variant of a vector keeps its components in fields, it has no register to fill
        Check3<float3s, float, float2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<double3s, double, double2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<int3s, int, int2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<uint3s, uint, uint2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<long3s, long, long2>(new(1, 2), new(3, 4), new(5, 6), 7);
        Check3<ulong3s, ulong, ulong2>(new(1, 2), new(3, 4), new(5, 6), 7);
    }

    /// <summary>
    /// Covers the merge of two pairs of every kind of a 4 component vector.
    /// </summary>
    [Test]
    public void Merge4FromPairs()
    {
        Check4FromPairs<float4, float, float2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<double4, double, double2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<int4, int, int2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<uint4, uint, uint2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<long4, long, long2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<ulong4, ulong, ulong2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<short4, short, short2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<ushort4, ushort, ushort2>(new(1, 2), new(3, 4), new(5, 6), new(7, 8), new(9, 10), new(11, 12), 13, 14);
        Check4FromPairs<half4, Half, half2>(new((Half)1, (Half)2), new((Half)3, (Half)4), new((Half)5, (Half)6),
            new((Half)7, (Half)8), new((Half)9, (Half)10), new((Half)11, (Half)12), (Half)13, (Half)14);
        Check4FromPairs<b32v4, b32, b32v2>(new(B32.True, B32.False), new(B32.False, B32.True), new(B32.True, B32.True),
            new(B32.False, B32.False), new(B32.True, B32.False), new(B32.False, B32.True), B32.True, B32.False);
        Check4FromPairs<b64v4, b64, b64v2>(new(B64.True, B64.False), new(B64.False, B64.True), new(B64.True, B64.True),
            new(B64.False, B64.False), new(B64.True, B64.False), new(B64.False, B64.True), B64.True, B64.False);
        Check4FromPairs<b16v4, b16, b16v2>(new(B16.True, B16.False), new(B16.False, B16.True), new(B16.True, B16.True),
            new(B16.False, B16.False), new(B16.True, B16.False), new(B16.False, B16.True), B16.True, B16.False);
    }

    /// <summary>
    /// Covers the merge of a triple and a value of every kind of a 4 component vector.
    /// </summary>
    [Test]
    public void Merge4FromTriples()
    {
        Check4FromTriples<float4, float, float3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<double4, double, double3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<int4, int, int3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<uint4, uint, uint3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<long4, long, long3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<ulong4, ulong, ulong3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<short4, short, short3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<ushort4, ushort, ushort3>(new(1, 2, 3), new(4, 5, 6), new(7, 8, 9), new(10, 11, 12), 13);
        Check4FromTriples<half4, Half, half3>(new((Half)1, (Half)2, (Half)3), new((Half)4, (Half)5, (Half)6),
            new((Half)7, (Half)8, (Half)9), new((Half)10, (Half)11, (Half)12), (Half)13);
        Check4FromTriples<b32v4, b32, b32v3>(new(B32.True, B32.False, B32.True), new(B32.False, B32.True, B32.False),
            new(B32.True, B32.True, B32.False), new(B32.False, B32.False, B32.True), B32.True);
        Check4FromTriples<b64v4, b64, b64v3>(new(B64.True, B64.False, B64.True), new(B64.False, B64.True, B64.False),
            new(B64.True, B64.True, B64.False), new(B64.False, B64.False, B64.True), B64.True);
        Check4FromTriples<b16v4, b16, b16v3>(new(B16.True, B16.False, B16.True), new(B16.False, B16.True, B16.False),
            new(B16.True, B16.True, B16.False), new(B16.False, B16.False, B16.True), B16.True);
    }

    /// <summary>
    /// The padding lane of the register of a 3 component vector is not a component, so it holds zero whatever
    /// the pair of the merge held in the lanes behind its own components.
    /// </summary>
    [Test]
    public void PaddingLane()
    {
        var f2 = new float2(1, 2);
        var d2 = new double2(1, 2);
        var i2 = new int2(1, 2);
        var u2 = new uint2(1, 2);
        var l2 = new long2(1, 2);
        var ul2 = new ulong2(1, 2);
        var t2 = new b32v2(B32.True, B32.False);
        var q2 = new b64v2(B64.True, B64.False);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.Create(f2, 3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.Create(3, f2).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(float3.InsertY(f2, 3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(double3.Create(d2, 3).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(double3.Create(3, d2).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(double3.InsertY(d2, 3).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(int3.Create(i2, 3).vector.GetElement(3), Is.EqualTo(0));
            Assert.That(int3.Create(3, i2).vector.GetElement(3), Is.EqualTo(0));
            Assert.That(int3.InsertY(i2, 3).vector.GetElement(3), Is.EqualTo(0));
            Assert.That(uint3.Create(u2, 3).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(uint3.Create(3, u2).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(uint3.InsertY(u2, 3).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(long3.Create(l2, 3).vector.GetElement(3), Is.EqualTo(0L));
            Assert.That(long3.Create(3, l2).vector.GetElement(3), Is.EqualTo(0L));
            Assert.That(long3.InsertY(l2, 3).vector.GetElement(3), Is.EqualTo(0L));
            Assert.That(ulong3.Create(ul2, 3).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(ulong3.Create(3, ul2).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(ulong3.InsertY(ul2, 3).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(b32v3.Create(t2, B32.True).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(b32v3.Create(B32.True, t2).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(b32v3.InsertY(t2, B32.True).vector.GetElement(3), Is.EqualTo(0u));
            Assert.That(b64v3.Create(q2, B64.True).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(b64v3.Create(B64.True, q2).vector.GetElement(3), Is.EqualTo(0UL));
            Assert.That(b64v3.InsertY(q2, B64.True).vector.GetElement(3), Is.EqualTo(0UL));
        }
    }
}
