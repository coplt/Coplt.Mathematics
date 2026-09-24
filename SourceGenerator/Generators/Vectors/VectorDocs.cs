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
        #region IVectorArithmetic

        {
            "clamp",
            ("Clamps every component into the inclusive range of <c>min</c> and <c>max</c>", "The clamped vector",
                "a=The vector;min=The lower bound of every component;max=The upper bound of every component")
        },
        { "lerp", ("Interpolates between <c>start</c> and <c>end</c>", "The interpolated vector", "start=The value at t = 0;end=The value at t = 1;t=The interpolation factor") },
        {
            "unlerp",
            ("Returns where the value is between <c>start</c> and <c>end</c>", "The position of the value between the two bounds",
                "a=The value to place;start=The value at t = 0;end=The value at t = 1")
        },
        {
            "remap",
            ("Remaps the vector from the source range into the destination range", "The remapped vector",
                "a=The vector to remap;src_start=The lower bound of the source range;src_end=The upper bound of the source range;dst_start=The lower bound of the destination range;dst_end=The upper bound of the destination range")
        },
        { "length_sq", ("Returns the squared length of the vector, it is the same as <c>dot(self)</c> but avoids the square root", "The squared length", "a=The vector") },
        { "distance_sq", ("Returns the squared distance between the two vectors", "The squared distance", "a=The vector;b|to=The other vector") },
        { "csum", ("Returns the sum of all components", "The sum of the components", "a=The vector") },
        { "cmin", ("Returns the smallest component", "The smallest component", "a=The vector") },
        { "cmax", ("Returns the largest component", "The largest component", "a=The vector") },
        { "cmin_safe", ("Returns the smallest component, it is safe when the vector has a padding component", "The smallest component", "a=The vector") },
        { "cmax_safe", ("Returns the largest component, it is safe when the vector has a padding component", "The largest component", "a=The vector") },

        #endregion

        #region IVectorFloatingPoint

        { "mod", ("Returns the component wise remainder of the division of the two vectors", "The remainder", "a=The vector;b|other=The divisor") },
        { "modf", ("Splits the vector into its integral and its fractional part", "The fractional part", "a=The vector;i=Receives the integral part") },
        { "ceil", ("Returns the smallest integer that is not less than every component", "The rounded up vector", "a=The vector") },
        { "floor", ("Returns the largest integer that is not greater than every component", "The rounded down vector", "a=The vector") },
        { "round", ("Rounds every component to the nearest integer, a value that is exactly between two of them goes to the even one", "The rounded vector", "a=The vector") },
        { "trunc", ("Returns the integral part of every component", "The truncated vector", "a=The vector") },
        { "frac", ("Returns the fractional part of every component", "The fractional part of the vector", "a=The vector") },
        { "rcp", ("Returns the reciprocal of every component", "The reciprocal of the vector", "a=The vector") },
        { "saturate", ("Clamps every component into the range of zero and one", "The saturated vector", "a=The vector") },
        {
            "smoothstep",
            ("Interpolates smoothly between <c>min</c> and <c>max</c>, the result is 0 below the minimum, 1 above the maximum and a smooth hermite curve in between",
                "The interpolated vector", "min=The value at 0;max=The value at 1;a=The value to place between the two bounds")
        },
        {
            "reflect",
            ("Returns the vector reflected around the normal <c>n</c>, it has to be normalized", "The reflected vector",
                "a=The vector to reflect;n=The normalized normal of the surface")
        },
        {
            "project",
            ("Returns the projection of the vector onto <c>onto</c>, it is the component that is parallel to it", "The projected vector",
                "a=The vector to project;onto=The vector to project onto, it does not have to be normalized")
        },
        {
            "project_on_plane",
            ("Returns the projection of the vector onto the plane that has <c>plane_normal</c> as its normal, it is the component that is inside the plane", "The projected vector",
                "a=The vector to project;plane_normal=The normal of the plane, it does not have to be normalized")
        },
        {
            "project_normalized",
            ("Returns the projection of the vector onto the normalized <c>onto</c>", "The projected vector", "a=The vector to project;onto=The normalized vector to project onto")
        },
        {
            "project_on_plane_normalized",
            ("Returns the projection of the vector onto the plane that has the normalized <c>plane_normal</c> as its normal", "The projected vector",
                "a=The vector to project;plane_normal=The normalized normal of the plane")
        },
        { "radians", ("Converts every component from degrees to radians", "The vector in radians", "a=The vector in degrees") },
        { "degrees", ("Converts every component from radians to degrees", "The vector in degrees", "a=The vector in radians") },
        {
            "wrap",
            ("Wraps every component into the range of <c>min</c> and <c>max</c>", "The wrapped vector",
                "a=The vector;min=The lower bound of every component;max=The upper bound of every component")
        },

        #endregion

        #region IVectorFloatingPointIeee754

        { "is_NaN", ("Returns a mask that is true where the component is NaN", "The mask", "a=The vector") },
        { "is_finite", ("Returns a mask that is true where the component is finite, so it is neither NaN nor an infinity", "The mask", "a=The vector") },
        { "is_inf", ("Returns a mask that is true where the component is a positive or a negative infinity", "The mask", "a=The vector") },
        { "is_pos_inf", ("Returns a mask that is true where the component is a positive infinity", "The mask", "a=The vector") },
        { "is_neg_inf", ("Returns a mask that is true where the component is a negative infinity", "The mask", "a=The vector") },
        { "log", ("Returns the natural logarithm of every component", "The natural logarithm", "a=The vector;b=The base of the logarithm") },
        { "log2", ("Returns the base 2 logarithm of every component", "The base 2 logarithm", "a=The vector") },
        { "log10", ("Returns the base 10 logarithm of every component", "The base 10 logarithm", "a=The vector") },
        { "exp", ("Returns <c>e</c> raised to the power of every component", "The exponential", "a=The vector") },
        { "exp2", ("Returns 2 raised to the power of every component", "The exponential", "a=The vector") },
        { "exp10", ("Returns 10 raised to the power of every component", "The exponential", "a=The vector") },
        { "pow", ("Returns every component raised to the power of the matching exponent", "The power", "a=The vector;b|other|v=The exponent") },
        { "sqrt", ("Returns the square root of every component", "The square root", "a=The vector") },
        { "rsqrt", ("Returns the reciprocal of the square root of every component, it is the same as <c>rcp(sqrt())</c>", "The reciprocal of the square root", "a=The vector") },
        { "length", ("Returns the length of the vector, it is the same as <c>sqrt(length_sq())</c>", "The length of the vector", "a=The vector") },
        { "distance", ("Returns the distance between the two vectors, it is the same as the length of the difference", "The distance", "a=The vector;b|to=The other vector") },
        { "normalize", ("Returns the vector scaled to a length of 1, the result is a NaN vector when the length is zero", "The normalized vector", "a=The vector to normalize") },
        {
            "normalize_safe", ("Returns the vector scaled to a length of 1, it returns a zero vector when the length is zero", "The normalized vector", "a=The vector to normalize")
        },
        {
            "step",
            ("Returns 1 where the component is not less than the matching component of the <c>threshold</c> and 0 where it is less", "The step vector",
                "threshold=The threshold;a=The vector")
        },
        {
            "project_safe",
            ("Returns the projection of the vector onto <c>onto</c>, it returns <c>default_value</c> when the projection is not finite", "The projected vector",
                "a=The vector to project;onto=The vector to project onto;default_value=The value that is returned when the projection is not finite")
        },
        {
            "face_forward",
            ("Returns the vector with the sign chosen so that it faces away from the incident vector <c>i</c>, the sign is flipped when the dot product of <c>ng</c> and <c>i</c> is not negative",
                "The oriented vector", "a=The vector to orient;i=The incident vector;ng=The normal that is used to choose the sign")
        },
        { "sin", ("Returns the sine of every component in radians", "The sine", "a=The vector") },
        { "cos", ("Returns the cosine of every component in radians", "The cosine", "a=The vector") },
        { "sincos", ("Returns the sine and the cosine of every component in radians", "The sine and the cosine", "a=The vector;sin=Receives the sine;cos=Receives the cosine") },
        { "tan", ("Returns the tangent of every component in radians", "The tangent", "a=The vector") },
        { "asin", ("Returns the arc sine of every component, the result is in radians", "The arc sine", "a=The vector") },
        { "acos", ("Returns the arc cosine of every component, the result is in radians", "The arc cosine", "a=The vector") },
        { "atan", ("Returns the arc tangent of every component, the result is in radians", "The arc tangent", "a=The vector") },
        {
            "atan2",
            ("Returns the arc tangent of the quotient of the two vectors, the signs of both are used to find the quadrant of the result", "The arc tangent in radians",
                "a=The numerator;b|v=The divisor")
        },
        { "sinh", ("Returns the hyperbolic sine of every component", "The hyperbolic sine", "a=The vector") },
        { "cosh", ("Returns the hyperbolic cosine of every component", "The hyperbolic cosine", "a=The vector") },
        { "tanh", ("Returns the hyperbolic tangent of every component", "The hyperbolic tangent", "a=The vector") },
        { "asinh", ("Returns the inverse hyperbolic sine of every component", "The inverse hyperbolic sine", "a=The vector") },
        { "acosh", ("Returns the inverse hyperbolic cosine of every component", "The inverse hyperbolic cosine", "a=The vector") },
        { "atanh", ("Returns the inverse hyperbolic tangent of every component", "The inverse hyperbolic tangent", "a=The vector") },
        {
            "chg_sign",
            ("Returns a vector that has the magnitude of the vector and the sign of <c>sign</c>", "The vector with the changed sign",
                "a=The vector that provides the magnitude of every component;sign=The vector that provides the sign of every component")
        },
        {
            "refract",
            ("Returns the refraction direction, the incident vector has to be normalized and the normal has to point against it", "The refracted direction",
                "i=The normalized vector of the incoming direction;n=The normalized normal that points against <c>i</c>;index_of_refraction=The ratio between the index of refraction of the two materials")
        },

        #endregion

        #region IVectorInteger

        { "is_pow2", ("Returns a mask that is true where the component is a power of two, a zero and a negative component are not a power of two", "The mask", "a=The vector") },
        {
            "up2pow2",
            ("Returns every component rounded up to the next power of two, a component that is a power of two already is kept and a zero stays zero", "The rounded up vector",
                "a=The vector")
        },

        #endregion

        #region IVectorSelect

        {
            "select",
            ("Returns the component of the second vector where the mask is true and the component of the third where it is false", "The selected vector",
                "c=The mask;t|a=The vector that the true components are taken from;f|b=The vector that the false components are taken from")
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
