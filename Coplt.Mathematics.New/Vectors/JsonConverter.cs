using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics.Json;

[CpuOnly]
public sealed class Vector3JsonConverter<TVector, TScalar> : JsonConverter<TVector>
    where TVector : unmanaged, IVector3<TVector, TScalar>
    where TScalar : unmanaged
{
    public override TVector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
            Utils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);
        reader.Read();
        var x = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        var y = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        var z = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            Utils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);
        return TVector.Create(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, TVector value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, TVector.get_x(value), options);
        JsonSerializer.Serialize(writer, TVector.get_y(value), options);
        JsonSerializer.Serialize(writer, TVector.get_z(value), options);
        writer.WriteEndArray();
    }
}

[CpuOnly]
public sealed class Vector2JsonConverter<TVector, TScalar> : JsonConverter<TVector>
    where TVector : unmanaged, IVector2<TVector, TScalar>
    where TScalar : unmanaged
{
    public override TVector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
            Utils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);
        reader.Read();
        var x = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        var y = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            Utils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);
        return TVector.Create(x, y);
    }

    public override void Write(Utf8JsonWriter writer, TVector value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, TVector.get_x(value), options);
        JsonSerializer.Serialize(writer, TVector.get_y(value), options);
        writer.WriteEndArray();
    }
}

[CpuOnly]
public sealed class Vector4JsonConverter<TVector, TScalar> : JsonConverter<TVector>
    where TVector : unmanaged, IVector4<TVector, TScalar>
    where TScalar : unmanaged
{
    public override TVector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
            Utils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);
        reader.Read();
        var x = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        var y = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        var z = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        var w = JsonSerializer.Deserialize<TScalar>(ref reader, options);
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            Utils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);
        return TVector.Create(x, y, z, w);
    }

    public override void Write(Utf8JsonWriter writer, TVector value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, TVector.get_x(value), options);
        JsonSerializer.Serialize(writer, TVector.get_y(value), options);
        JsonSerializer.Serialize(writer, TVector.get_z(value), options);
        JsonSerializer.Serialize(writer, TVector.get_w(value), options);
        writer.WriteEndArray();
    }
}
