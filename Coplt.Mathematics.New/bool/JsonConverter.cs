using Coplt.Mathematics.Json;

namespace Coplt.Mathematics
{
    [JsonConverter(typeof(b16JsonConverter))]
    public readonly partial record struct b16;

    [JsonConverter(typeof(b32JsonConverter))]
    public readonly partial record struct b32;

    [JsonConverter(typeof(b64JsonConverter))]
    public readonly partial record struct b64;
}

namespace Coplt.Mathematics.Json
{
    /// <summary>
    /// Reads and writes the mask of a half as json: the value is a json bool, true for every bit of the mask
    /// set and false for a mask that is zero
    /// </summary>
    public sealed class b16JsonConverter : JsonConverter<b16>
    {
        public override b16 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
                Utils.ThrowJsonToken(JsonTokenType.True, reader.TokenType);
            return reader.GetBoolean() ? b16.True : b16.False;
        }

        public override void Write(Utf8JsonWriter writer, b16 value, JsonSerializerOptions options) =>
            writer.WriteBooleanValue((bool)value);
    }

    /// <inheritdoc cref="b16JsonConverter"/>
    public sealed class b32JsonConverter : JsonConverter<b32>
    {
        public override b32 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
                Utils.ThrowJsonToken(JsonTokenType.True, reader.TokenType);
            return reader.GetBoolean() ? b32.True : b32.False;
        }

        public override void Write(Utf8JsonWriter writer, b32 value, JsonSerializerOptions options) =>
            writer.WriteBooleanValue((bool)value);
    }

    /// <inheritdoc cref="b16JsonConverter"/>
    public sealed class b64JsonConverter : JsonConverter<b64>
    {
        public override b64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
                Utils.ThrowJsonToken(JsonTokenType.True, reader.TokenType);
            return reader.GetBoolean() ? b64.True : b64.False;
        }

        public override void Write(Utf8JsonWriter writer, b64 value, JsonSerializerOptions options) =>
            writer.WriteBooleanValue((bool)value);
    }
}
