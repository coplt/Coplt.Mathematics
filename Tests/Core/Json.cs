using System.Text.Json;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// Checks the json converters of the generated vectors and matrices. A vector is written as an array of its
/// components in order and a matrix as an array of the arrays of the components of its columns, every component
/// is a json number and the name of the type is not a part of the value.
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
            // the storage variant of a vector has the json shape of the regular vector
            Assert.That(JsonSerializer.Serialize(new float2s(1, 2), Options), Is.EqualTo("[1,2]"));
            Assert.That(JsonSerializer.Serialize(new float3s(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new double3s(1, 2, 3), Options), Is.EqualTo("[1,2,3]"));
            Assert.That(JsonSerializer.Serialize(new int2s(1, 2), Options), Is.EqualTo("[1,2]"));
        }
    }

    /// <summary>
    /// The value of a quaternion is the one of the four components of the kind of it, so the json of it is the
    /// array of the four components of the value of 4 components of it.
    /// </summary>
    [Test]
    public void Quaternion()
    {
        var q = new quaternion(1f, 2f, 3f, 4f);
        var d = new quaternion_d(1, 2, 3, 4);
        var h = new quaternion_h((Half)1, (Half)2, (Half)3, (Half)4);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(JsonSerializer.Serialize(q, Options), Is.EqualTo("[1,2,3,4]"));
            Assert.That(JsonSerializer.Serialize(d, Options), Is.EqualTo("[1,2,3,4]"));
            // a half is not a number of the writer, its value is written as the float it widens to
            Assert.That(JsonSerializer.Serialize(h, Options), Is.EqualTo("[1,2,3,4]"));

            var rq = JsonSerializer.Deserialize<quaternion>("[1,2,3,4]", Options);
            Assert.That((rq.value.x, rq.value.y, rq.value.z, rq.value.w), Is.EqualTo((1f, 2f, 3f, 4f)));
            var rd = JsonSerializer.Deserialize<quaternion_d>("[1,2,3,4]", Options);
            Assert.That((rd.value.x, rd.value.y, rd.value.z, rd.value.w), Is.EqualTo((1d, 2d, 3d, 4d)));
            var rh = JsonSerializer.Deserialize<quaternion_h>("[1,2,3,4]", Options);
            Assert.That((float)rh.value.w, Is.EqualTo(4f));
            // the value of a quaternion without the four components of it is not a value it can be read from
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<quaternion>("{\"x\":1}", Options));
        }
    }

    [Test]
    public void Read()
    {
        var f3 = JsonSerializer.Deserialize<float3>("[1,2,3]", Options);
        var u2 = JsonSerializer.Deserialize<uint2>("[1,2]", Options);
        var h3 = JsonSerializer.Deserialize<half3>("[1,2,3]", Options);
        var p3 = JsonSerializer.Deserialize<float3>("[ 1 , 2 , 3 ]", Options);
        var s3 = JsonSerializer.Deserialize<float3s>("[1,2,3]", Options);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((f3.x, f3.y, f3.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((u2.x, u2.y), Is.EqualTo((1u, 2u)));
            Assert.That(h3.x, Is.EqualTo((Half)1));
            Assert.That(h3.y, Is.EqualTo((Half)2));
            Assert.That(h3.z, Is.EqualTo((Half)3));
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
    }

    [Test]
    public void Matrix()
    {
        using (Assert.EnterMultipleScope())
        {
            // the value of a matrix is an array of the arrays of the components of its columns
            Assert.That(
                JsonSerializer.Serialize(new float3x3(new float3(1, 2, 3), new float3(4, 5, 6), new float3(7, 8, 9)), Options),
                Is.EqualTo("[[1,2,3],[4,5,6],[7,8,9]]"));
            Assert.That(
                JsonSerializer.Serialize(new float2x2(new float2(1, 2), new float2(3, 4)), Options),
                Is.EqualTo("[[1,2],[3,4]]"));
            // a matrix of 2 rows and 4 columns has 4 columns of 2 components
            Assert.That(
                JsonSerializer.Serialize(
                    new float2x4(new float2(1, 2), new float2(3, 4), new float2(5, 6), new float2(7, 8)), Options),
                Is.EqualTo("[[1,2],[3,4],[5,6],[7,8]]"));
            // the storage variant of a matrix has the json shape of the regular matrix
            Assert.That(
                JsonSerializer.Serialize(new float2x2s(new float2s(1, 2), new float2s(3, 4)), Options),
                Is.EqualTo("[[1,2],[3,4]]"));
        }

        var m = JsonSerializer.Deserialize<float3x3>("[[1,2,3],[4,5,6],[7,8,9]]", Options);
        var d = JsonSerializer.Deserialize<double2x2>("[[1,2],[3,4]]", Options);
        var ms = JsonSerializer.Deserialize<float2x2s>("[[1,2],[3,4]]", Options);
        using (Assert.EnterMultipleScope())
        {
            Assert.That((m.c0.x, m.c0.y, m.c0.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((m.c2.x, m.c2.y, m.c2.z), Is.EqualTo((7f, 8f, 9f)));
            Assert.That((d.c1.x, d.c1.y), Is.EqualTo((3d, 4d)));
            Assert.That((ms.c1.x, ms.c1.y), Is.EqualTo((3f, 4f)));
        }
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
