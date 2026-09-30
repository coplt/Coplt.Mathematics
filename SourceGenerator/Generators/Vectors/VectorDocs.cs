using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// The documentation of the members of the vectors. Every operation of a vector is a member of the vector itself
/// and the interface declares it as a static member as well, so a member of a vector does not implement the
/// interface member of its own name and an <c>inheritdoc</c> of it has nothing to copy. The table below is the
/// documentation of the operation of every name, the parameters of the signature of a generated member select
/// the ones that are emitted for it, so the same name can be a member of the vector, which has no parameter for
/// the vector itself, and a static member of it, which has one.
/// </summary>
internal static class VectorDocs
{
    /// <summary>
    /// The documentation of an operation: the summary, the return value and the documentation of every
    /// parameter, the last one is a list of <c>name=documentation</c> pairs separated by <c>;</c>.
    /// </summary>
    private static readonly Dictionary<string, (string Summary, string Returns, string Params)> Docs = new()
    {
        { "is_NaN", ("Returns a mask that is true where the component is NaN", "The mask", "a=The vector") },
        { "is_finite", ("Returns a mask that is true where the component is finite, so it is neither NaN nor an infinity", "The mask", "a=The vector") },
        { "is_inf", ("Returns a mask that is true where the component is a positive or a negative infinity", "The mask", "a=The vector") },
        { "is_pos_inf", ("Returns a mask that is true where the component is a positive infinity", "The mask", "a=The vector") },
        { "is_neg_inf", ("Returns a mask that is true where the component is a negative infinity", "The mask", "a=The vector") },
        { "pow", ("Returns every component raised to the power of the matching exponent", "The power", "a=The vector;b|other|v=The exponent") },
        { "sqrt", ("Returns the square root of every component", "The square root", "a=The vector") },
        { "rsqrt", ("Returns the reciprocal of the square root of every component, it is the same as <c>rcp(sqrt())</c>", "The reciprocal of the square root", "a=The vector") },
        { "length", ("Returns the length of the vector, it is the same as <c>sqrt(length_sq())</c>", "The length of the vector", "a=The vector") },
        { "distance", ("Returns the distance between the two vectors, it is the same as the length of the difference", "The distance", "a=The vector;b|to=The other vector") },
        { "normalize", ("Returns the vector scaled to a length of 1, the result is a NaN vector when the length is zero", "The normalized vector", "a=The vector to normalize") },
        {
            "normalize_safe", ("Returns the vector scaled to a length of 1, it returns a zero vector when the length is zero", "The normalized vector", "a=The vector to normalize")
        },

        #region IVectorInteger

        {
            "is_pow2",
            ("Returns a value that says where the component is a power of two, a zero and a negative component are not a power of two", "The value of the check", "a=The vector")
        },
        {
            "up2pow2",
            ("Returns every component rounded up to the next power of two, a component that is a power of two already is kept and a zero stays zero", "The rounded up vector",
                "a=The vector")
        },

        #endregion
    };

    /// <summary>
    /// Replaces the <c>inheritdoc</c> of every generated member with the documentation of the operation of its
    /// name. The name and the parameters of the member are read from the signature that follows the comment, so
    /// a member only gets the documentation of the parameters it declares.
    /// </summary>
    /// <param name="file">The file to document</param>
    /// <returns>The documented file</returns>
    public static string Apply(string file)
    {
        var lines = file.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Trim() != "/// <inheritdoc/>") continue;
            // the signature of the member is the next line that declares it
            var sig = -1;
            var name = "";
            for (var j = i + 1; j < lines.Length; j++)
            {
                var next = lines[j].Trim();
                if (!next.StartsWith("public ", StringComparison.Ordinal)) continue;
                var open = next.IndexOf('(');
                if (open < 0) break;
                var head = next[..open].TrimEnd();
                name = head[(head.LastIndexOf(' ') + 1)..];
                sig = j;
                break;
            }

            if (sig < 0 || !Docs.TryGetValue(name, out var doc)) continue;

            var indent = line[..(line.Length - line.TrimStart().Length)];
            // a parameter that has no documentation of its own leaves every parameter to the documentation of
            // the operation itself, a comment that documents only a part of the parameters is not a comment
            var parameters = ParameterNames(lines[sig]);
            var docs = new List<(string Name, string Text)>(ParameterDocs(doc.Params, parameters));
            if (docs.Count != parameters.Length) docs.Clear();

            var sb = new StringBuilder();
            sb.Append(indent).Append("/// <summary>").Append(doc.Summary).AppendLine("</summary>");
            foreach (var (parameter, text) in docs)
            {
                sb.Append(indent).Append("/// <param name=\"").Append(parameter).Append("\">").Append(text).AppendLine("</param>");
            }

            if (doc.Returns.Length != 0)
            {
                sb.Append(indent).Append("/// <returns>").Append(doc.Returns).AppendLine("</returns>");
            }

            lines[i] = sb.ToString().TrimEnd('\r', '\n');
        }

        return string.Join("\n", lines);
    }

    /// <summary>
    /// Returns the names of the parameters of a signature, a parameter that has a default value is read from the
    /// part in front of the equals sign.
    /// </summary>
    private static string[] ParameterNames(string signature)
    {
        var text = signature.Trim();
        var open = text.IndexOf('(');
        var close = open < 0 ? -1 : text.IndexOf(')', open);
        if (close < 0) return Array.Empty<string>();
        var names = new List<string>();
        foreach (var part in text[(open + 1)..close].Split(','))
        {
            var parameter = part.Trim();
            var equals = parameter.IndexOf('=');
            if (equals >= 0) parameter = parameter[..equals].Trim();
            var tokens = parameter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 0) names.Add(tokens[^1]);
        }

        return names.ToArray();
    }

    /// <summary>
    /// Returns the documentation of every parameter of the signature, in the order of it.
    /// </summary>
    private static IEnumerable<(string Name, string Text)> ParameterDocs(string docs, string[] parameters)
    {
        var map = new Dictionary<string, string>();
        // a parameter can have more than one name, the member of the vector names the other operand <c>other</c>
        // where the static member calls it <c>b</c>
        foreach (var part in docs.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var equals = part.IndexOf('=');
            if (equals < 0) continue;
            var text = part[(equals + 1)..];
            foreach (var name in part[..equals].Split('|')) map[name] = text;
        }

        foreach (var parameter in parameters)
        {
            if (map.TryGetValue(parameter, out var text)) yield return (parameter, text);
        }
    }
}
