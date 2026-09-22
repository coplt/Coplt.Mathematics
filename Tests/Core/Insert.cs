using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The members of the legacy insert api implement <c>IVector2Insert</c> and <c>IVector3Insert</c>: the member is
/// called on the short vector that holds the components that the longer one does not take, so
/// <c>pair.Iz(z)</c> of a vector of 2 components builds the vector of 3 components and <c>pair.Iyz(yz)</c> the
/// one of 4 components, and the member of a vector of 3 components builds the one of 4 components. Every member
/// builds the same vector as the member of the create that takes the same arguments; a vector of 4 components
/// has no member of this kind at all.
/// </summary>
public class TestVectorInsert
{
    /// <summary>
    /// A member of the interface of a vector of 2 components is reached through a type parameter, the interface
    /// declares both of the vectors that the members of the pair build and the implementation forwards to the
    /// member of the create of the one it builds.
    /// </summary>
    private static TVector3 CallIx<T, TScalar, TVector3, TVector4>(in T yz, TScalar x)
        where T : unmanaged, IVector2Insert<T, TScalar, TVector3, TVector4>
        where TScalar : unmanaged
        where TVector3 : unmanaged
        where TVector4 : unmanaged
        => T.Ix(yz, x);

    [Test]
    public void Insert3()
    {
        var h2 = new half2((Half)2, (Half)3);
        var t2 = new b32v2(true, false);

        using (Assert.EnterMultipleScope())
        {
            // the name of a member is the name of the component that it inserts
            Assert.That(float2.Ix(new float2(2, 3), 1), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(float2.Iy(new float2(1, 3), 2), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(float2.Iz(new float2(1, 2), 3), Is.EqualTo(new float3(1, 2, 3)));
            // the member builds the same vector as the member of the create that takes the same arguments
            Assert.That(float2.Ix(new float2(2, 3), 1), Is.EqualTo(float3.Create(1, new float2(2, 3))));
            Assert.That(float2.Iy(new float2(1, 3), 2), Is.EqualTo(float3.InsertY(new float2(1, 3), 2)));
            Assert.That(float2.Iz(new float2(1, 2), 3), Is.EqualTo(float3.Create(new float2(1, 2), 3)));
            Assert.That(double2.Iz(new double2(1, 2), 3), Is.EqualTo(new double3(1, 2, 3)));
            Assert.That(uint2.Iy(new uint2(1, 3), 2), Is.EqualTo(new uint3(1, 2, 3)));
            Assert.That(half2.Ix(h2, (Half)1), Is.EqualTo(new half3((Half)1, (Half)2, (Half)3)));
            Assert.That((bool)b32v2.Iz(t2, true).z, Is.True);
            // the storage variant of a 2 component vector has the member as well
            Assert.That(float2s.Iz(new float2s(1, 2), 3), Is.EqualTo(new float3(1, 2, 3)));
            // the member of the interface is reached through the type parameter of the generic member
            Assert.That(CallIx<float2, float, float3, float4>(new float2(2, 3), 1), Is.EqualTo(new float3(1, 2, 3)));
        }
    }

    [Test]
    public void Insert4FromPair()
    {
        var t2 = new b32v2(true, false);

        using (Assert.EnterMultipleScope())
        {
            // the pair of the arguments and the pair behind it take the components their name holds
            Assert.That(float2.Izw(new float2(1, 2), new float2(3, 4)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Izw(new float2(1, 2), 3, 4), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Ixy(new float2(3, 4), new float2(1, 2)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Ixy(new float2(3, 4), 1, 2), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Iyz(new float2(1, 4), new float2(2, 3)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Iyz(new float2(1, 4), 2, 3), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Ixw(new float2(2, 3), new float2(1, 4)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Ixw(new float2(2, 3), 1, 4), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Iyw(new float2(1, 3), new float2(2, 4)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Iyw(new float2(1, 3), 2, 4), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Ixz(new float2(2, 4), new float2(1, 3)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float2.Ixz(new float2(2, 4), 1, 3), Is.EqualTo(new float4(1, 2, 3, 4)));
            // the member builds the same vector as the member of the create that takes the same arguments
            Assert.That(float2.Iyz(new float2(1, 4), new float2(2, 3)),
                Is.EqualTo(float4.InsertYZ(new float2(1, 4), new float2(2, 3))));
            Assert.That(float2.Ixw(new float2(2, 3), 1, 4), Is.EqualTo(float4.Create(1, new float2(2, 3), 4)));
            Assert.That(double2.Ixz(new double2(2, 4), new double2(1, 3)), Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That((bool)b64v2.Ixy(new b64v2(true, false), true, false).x, Is.True);
            // the storage variant of a 2 component vector has the member as well
            Assert.That(float2s.Izw(new float2s(1, 2), new float2s(3, 4)), Is.EqualTo(new float4(1, 2, 3, 4)));
        }
    }

    /// <summary>
    /// Every member has two forms: the member that is called on the vector builds the vector, which is the form
    /// that a caller uses instead of the extension member of the legacy library, and the static one that a
    /// generic caller is constrained by forwards to it.
    /// </summary>
    [Test]
    public void InstanceForm()
    {
        var p = new float2(1, 2);
        var d = new double2(1, 2);
        var t = new float3(1, 2, 3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(2, 3).Ix(1), Is.EqualTo(float2.Ix(new float2(2, 3), 1)));
            Assert.That(new float2(2, 3).Ix(1), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(p.Iy(3), Is.EqualTo(new float3(1, 3, 2)));
            Assert.That(p.Iz(3), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(p.Izw(new float2(3, 4)), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(p.Ixz(new float2(3, 4)), Is.EqualTo(new float4(3, 1, 4, 2)));
            Assert.That(p.Iyw(3, 4), Is.EqualTo(float2.Iyw(p, 3, 4)));
            Assert.That(d.Ixz(new double2(3, 4)), Is.EqualTo(double2.Ixz(d, new double2(3, 4))));
            Assert.That(new float2s(1, 2).Iz(3), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(t.Ix(4), Is.EqualTo(new float4(4, 1, 2, 3)));
            Assert.That(t.Iw(4), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(new float3(1, 3, 4).Iy(2), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(new float3(1, 2, 4).Iz(3), Is.EqualTo(float3.Iz(new float3(1, 2, 4), 3)));
            Assert.That(new float3s(1, 3, 4).Iy(2), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(new b32v2(true, false).Iz(true), Is.EqualTo(b32v2.Iz(new b32v2(true, false), true)));
        }
    }

    [Test]
    public void Insert4FromTriple()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.Ix(new float3(2, 3, 4), 1), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float3.Iy(new float3(1, 3, 4), 2), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float3.Iz(new float3(1, 2, 4), 3), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float3.Iw(new float3(1, 2, 3), 4), Is.EqualTo(new float4(1, 2, 3, 4)));
            Assert.That(float3.Iy(new float3(1, 3, 4), 2), Is.EqualTo(float4.InsertY(new float3(1, 3, 4), 2)));
            Assert.That(float3.Iz(new float3(1, 2, 4), 3), Is.EqualTo(float4.InsertZ(new float3(1, 2, 4), 3)));
            Assert.That(double3.Iw(new double3(1, 2, 3), 4), Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That(half3.Ix(new half3((Half)2, (Half)3, (Half)4), (Half)1),
                Is.EqualTo(new half4((Half)1, (Half)2, (Half)3, (Half)4)));
            // the storage variant of a 3 component vector has the member as well
            Assert.That(float3s.Iy(new float3s(1, 3, 4), 2), Is.EqualTo(new float4(1, 2, 3, 4)));
        }
    }
}
