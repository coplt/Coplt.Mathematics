using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coplt.Mathematics.Json;

/// <summary>
/// The converter of a quaternion of a single precision kind, which is the four components of the value of it
/// <para>The converter reads and writes the value of the four components of the quaternion with the converter of
/// a value of 4 components of the kind, so the value of the json of a quaternion is an array of the four
/// components of it</para>
/// </summary>
[CpuOnly]
public sealed class quaternionJsonConverter : JsonConverter<quaternion>
{
    /// <inheritdoc/>
    public override quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(JsonSerializer.Deserialize<float4>(ref reader, options));

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, quaternion value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value.value, options);
}

/// <summary>
/// The converter of a quaternion of a double precision kind, which is the four components of the value of it
/// <para>The converter reads and writes the value of the four components of the quaternion with the converter of
/// a value of 4 components of the kind, so the value of the json of a quaternion is an array of the four
/// components of it</para>
/// </summary>
[CpuOnly]
public sealed class quaternion_dJsonConverter : JsonConverter<quaternion_d>
{
    /// <inheritdoc/>
    public override quaternion_d Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(JsonSerializer.Deserialize<double4>(ref reader, options));

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, quaternion_d value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value.value, options);
}

/// <summary>
/// The converter of a quaternion of a half precision kind, which is the four components of the value of it
/// <para>The converter reads and writes the value of the four components of the quaternion with the converter of
/// a value of 4 components of the kind, so the value of the json of a quaternion is an array of the four
/// components of it</para>
/// </summary>
[CpuOnly]
public sealed class quaternion_hJsonConverter : JsonConverter<quaternion_h>
{
    /// <inheritdoc/>
    public override quaternion_h Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(JsonSerializer.Deserialize<half4>(ref reader, options));

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, quaternion_h value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value.value, options);
}
