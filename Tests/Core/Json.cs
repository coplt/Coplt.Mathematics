using System.Text.Json;
using Coplt.Experimental.Mathematics;
using Coplt.Mathematics.Generics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

namespace Tests.Core;

/// <summary>
/// Checks the json converters of the generated vectors. A vector is written as an array of its components in
/// order, a component of a bool vector is a json bool and every other one a json number, the name of the type is
/// not a part of the value.
/// </summary>
public class TestVectorJson
{
    private static readonly JsonSerializerOptions Options = new();

    [Test]
    public void Text()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(JsonSerializer.Serialize(new float2(1, 2), Options), Is.EqualTo("[1,2]"));
            Assert.That(JsonSerializer.Serialize(new float3(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new float4(1, 2, 3, 4), Options), Is.EqualTo("[1,2,3,4]"));
            Assert.That(JsonSerializer.Serialize(new double2(1, 2), Options), Is.EqualTo("[1,2]"));
            Assert.That(JsonSerializer.Serialize(new short3(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new ushort2(1, 2), Options), Is.EqualTo("[1,2]"));
            Assert.That(JsonSerializer.Serialize(new int4(1, 2, 3, 4), Options), Is.EqualTo("[1,2,3,4]"));
            Assert.That(JsonSerializer.Serialize(new uint2(1, 2), Options), Is.EqualTo("[1,2]"));
            Assert.That(JsonSerializer.Serialize(new long3(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new ulong2(1, 2), Options), Is.EqualTo("[1,2]"));
            // a half is not a number of the writer, its value is written as the float it widens to
            Assert.That(JsonSerializer.Serialize(new half2((Half)1, (Half)2), Options), Is.EqualTo("[1,2]"));
            // a component of a bool vector is a json bool, it is never written as a number
            Assert.That(JsonSerializer.Serialize(new b16v2(B16.True, B16.False), Options), Is.EqualTo("[true,false]"));
            Assert.That(
                JsonSerializer.Serialize(new b32v4(B32.True, B32.False, B32.True, B32.False), Options),
                Is.EqualTo("[true,false,true,false]"));
            Assert.That(JsonSerializer.Serialize(new b64v3(B64.True, B64.False, B64.True), Options), Is.EqualTo("[true,false,true]"));
            // the storage variant of a vector has the json shape of the regular vector
            Assert.That(JsonSerializer.Serialize(new float2s(1, 2), Options), Is.EqualTo("[1,2]"));
            Assert.That(JsonSerializer.Serialize(new float3s(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new double3s(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new int2s(1, 2), Options), Is.EqualTo("[1,2]"));
        }
    }

    [Test]
    public void Read()
    {
        var f3 = JsonSerializer.Deserialize<float3>("[1,2,3]", Options);
        var u2 = JsonSerializer.Deserialize<uint2>("[1,2]", Options);
        var h3 = JsonSerializer.Deserialize<half3>("[1,2,3]", Options);
        var b2 = JsonSerializer.Deserialize<b32v2>("[true,false]", Options);
        var p3 = JsonSerializer.Deserialize<float3>("[ 1 , 2 , 3 ]", Options);
        var s3 = JsonSerializer.Deserialize<float3s>("[1,2,3]", Options);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((u2.x, u2.y), Is.EqualTo((1u, 2u)));
            Assert.That(h3.x, Is.EqualTo((Half)1));
            Assert.That(h3.y, Is.EqualTo((Half)2));
            Assert.That(h3.z, Is.EqualTo((Half)3));
            Assert.That((bool)b2.x, Is.True);
            Assert.That((bool)b2.y, Is.False);
            // the whitespace between the components is skipped by the reader
            Assert.That((p3.x, p3.y, p3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((s3.x, s3.y, s3.z), Is.EqualTo((1f, 2f, 3f)));
        }
    }

    [Test]
    public void BadValue()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a vector is an array, an object cannot hold the components
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<float2>("{\"x\":1}", Options));
            // the array has to hold exactly as many components as the vector has
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<float2>("[1,2,3,4]", Options));
        }
    }

    [Test]
    public void EveryVector()
    {
        Check<float4, float>(new float4(1, 2, 3, 4), "[1,2,3,4]");
        Check<double2, double>(new double2(1, 2), "[1,2]");
        Check<half3, Half>(new half3((Half)1, (Half)2, (Half)3), "[1,2,3]");
        Check<short2, short>(new short2(1, 2), "[1,2]");
        Check<ushort4, ushort>(new ushort4(1, 2, 3, 4), "[1,2,3,4]");
        Check<int3, int>(new int3(1, 2, 3), "[1,2,3]");
        Check<uint2, uint>(new uint2(1, 2), "[1,2]");
        Check<long3, long>(new long3(1, 2, 3), "[1,2,3]");
        Check<ulong3, ulong>(new ulong3(1, 2, 3), "[1,2,3]");
        // the storage variant of a vector is a value of its own, it has a converter of its own as well
        Check<float2s, float>(new float2s(1, 2), "[1,2]");
        Check<uint2s, uint>(new uint2s(1, 2), "[1,2]");
        Check<double3s, double>(new double3s(1, 2, 3), "[1,2,3]");
        Check<long3s, long>(new long3s(1, 2, 3), "[1,2,3]");
        Check<b16v2, B16>(new b16v2(B16.True, B16.False), "[true,false]");
        Check<b32v4, B32>(new b32v4(B32.True, B32.False, B32.True, B32.False), "[true,false,true,false]");
        Check<b64v2, B64>(new b64v2(B64.True, B64.False), "[true,false]");
    }

    /// <summary>
    /// Writes the vector and reads it back, the type parameters of a call cannot be inferred from its constraints
    /// so they are written out.
    /// </summary>
    private static void Check<T, TScalar>(T v, string expected)
        where T : unmanaged, IVector<T, TScalar>
        where TScalar : unmanaged
    {
        var text = JsonSerializer.Serialize(v, Options);
        Assert.That(text, Is.EqualTo(expected));
        Assert.That(JsonSerializer.Deserialize<T>(text, Options), Is.EqualTo(v));
    }
}
