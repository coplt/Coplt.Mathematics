using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics.Json;

[CpuOnly]
public sealed class MatrixMx3JsonConverter<TMatrix, TVector> : JsonConverter<TMatrix>
    where TMatrix : unmanaged, IMatrixMx3Vector<TMatrix, TVector>
    where TVector : unmanaged, IVector<TVector>
{
    public override TMatrix Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
            Utils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);
        reader.Read();
        var c0 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        var c1 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        var c2 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            Utils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);
        return TMatrix.Create(c0, c1, c2);
    }

    public override void Write(Utf8JsonWriter writer, TMatrix value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, TMatrix.get_c0(value), options);
        JsonSerializer.Serialize(writer, TMatrix.get_c1(value), options);
        JsonSerializer.Serialize(writer, TMatrix.get_c2(value), options);
        writer.WriteEndArray();
    }
}

[CpuOnly]
public sealed class MatrixMx2JsonConverter<TMatrix, TVector> : JsonConverter<TMatrix>
    where TMatrix : unmanaged, IMatrixMx2Vector<TMatrix, TVector>
    where TVector : unmanaged, IVector<TVector>
{
    public override TMatrix Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
            Utils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);
        reader.Read();
        var c0 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        var c1 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            Utils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);
        return TMatrix.Create(c0, c1);
    }

    public override void Write(Utf8JsonWriter writer, TMatrix value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, TMatrix.get_c0(value), options);
        JsonSerializer.Serialize(writer, TMatrix.get_c1(value), options);
        writer.WriteEndArray();
    }
}

[CpuOnly]
public sealed class MatrixMx4JsonConverter<TMatrix, TVector> : JsonConverter<TMatrix>
    where TMatrix : unmanaged, IMatrixMx4Vector<TMatrix, TVector>
    where TVector : unmanaged, IVector<TVector>
{
    public override TMatrix Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
            Utils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);
        reader.Read();
        var c0 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        var c1 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        var c2 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        var c3 = JsonSerializer.Deserialize<TVector>(ref reader, options);
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            Utils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);
        return TMatrix.Create(c0, c1, c2, c3);
    }

    public override void Write(Utf8JsonWriter writer, TMatrix value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, TMatrix.get_c0(value), options);
        JsonSerializer.Serialize(writer, TMatrix.get_c1(value), options);
        JsonSerializer.Serialize(writer, TMatrix.get_c2(value), options);
        JsonSerializer.Serialize(writer, TMatrix.get_c3(value), options);
        writer.WriteEndArray();
    }
}
