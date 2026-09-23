using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the converter that reads and writes the vector described by <paramref name="typ"/> as json: the
    /// value is an array of its components in order, a component of a bool vector is written as a bool and every
    /// other one as a number, the name of the type is not a part of the value. The converter is emitted into its
    /// own file and into <c>Coplt.Mathematics.Json</c> instead of the namespace of the vector, the declaration of
    /// the vector carries the attribute that binds it, see <c>Gen</c>. A value that is not an array and one that
    /// does not hold the components of the vector are both rejected by <c>VectorUtils.ThrowJsonToken</c>, the
    /// message of the exception is not repeated in every converter.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The converter of the vector</returns>
    private static string GenJson(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var comp = VectorGenShared.Components(size);
        // a component of a bool vector is a json bool and every other one a json number, the type of a component
        // is the one the reader returns and the cast of it is the one the vector takes
        var getter = $"reader.Get{typ.jsonType}()";

        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, false, true, VectorGenerator.JsonNamespace);
        // the converter reads and writes the members of the vector, it can only run on the cpu
        sb.AppendLine("[CpuOnly]");
        sb.AppendLine($"public class {type}JsonConverter : JsonConverter<{type}>");
        sb.AppendLine("{");
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    public override {type} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)");
        sb.AppendLine("    {");
        // a json value that is not an array cannot hold the components of the vector
        sb.AppendLine("        if (reader.TokenType is not JsonTokenType.StartArray)");
        sb.AppendLine("            VectorUtils.ThrowJsonToken(JsonTokenType.StartArray, reader.TokenType);");
        for (var i = 0; i < size; i++)
        {
            sb.AppendLine("        reader.Read();");
            sb.AppendLine($"        var {comp[i]} = {typ.jsonCastBack}{getter};");
        }

        // reading a component leaves the reader on it, the array is closed by the token after the last component
        sb.AppendLine("        reader.Read();");
        sb.AppendLine("        if (reader.TokenType is not JsonTokenType.EndArray)");
        sb.AppendLine("            VectorUtils.ThrowJsonToken(JsonTokenType.EndArray, reader.TokenType);");
        sb.AppendLine($"        return new({VectorGenShared.Join(size, i => comp[i])});");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    public override void Write(Utf8JsonWriter writer, {type} value, JsonSerializerOptions options)");
        sb.AppendLine("    {");
        sb.AppendLine("        writer.WriteStartArray();");
        for (var i = 0; i < size; i++)
        {
            sb.AppendLine(typ.bol
                ? $"        writer.WriteBooleanValue(value.{comp[i]});"
                : $"        writer.WriteNumberValue({typ.jsonCast}value.{comp[i]});");
        }

        sb.AppendLine("        writer.WriteEndArray();");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
