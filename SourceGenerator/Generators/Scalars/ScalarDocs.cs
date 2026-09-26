using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// The documentation of the operations of the scalar types. The functions of the <c>math</c> class implement no
/// interface, so they have nothing to inherit their documentation from and the text of every operation lives in
/// the table below. The parameters of the signature of a member select the documentation that is emitted for
/// it, so the same entry serves the function, which takes the value it works on as its first parameter, and the
/// member that is called on a value, which receives it.
/// </summary>
internal static class ScalarDocs
{
    /// <summary>
    /// The documentation of an operation: the summary, the return value and the documentation of every
    /// parameter, the last one is a list of <c>name=documentation</c> pairs separated by <c>;</c>.
    /// </summary>
    private static readonly Dictionary<string, (string Summary, string Returns, string Params)> Docs = new()
    {
        #region arithmetic

        { "abs", ("Returns the value without its sign", "The absolute value", "a=The value") },
        { "sign", ("Returns <c>-1</c>, <c>0</c> or <c>1</c> depending on the sign of the value", "The sign of the value", "a=The value") },
        { "min", ("Returns the smaller of the two values", "The smaller value", "a=The value;b|other=The other value") },
        { "max", ("Returns the larger of the two values", "The larger value", "a=The value;b|other=The other value") },
        { "clamp", ("Clamps the value into the inclusive range of <c>min</c> and <c>max</c>", "The clamped value", "a=The value;min=The lower bound;max=The upper bound") },
        { "lerp", ("Interpolates between <c>start</c> and <c>end</c>", "The interpolated value", "start=The value at t = 0;end=The value at t = 1;t=The interpolation factor") },
        {
            "unlerp",
            ("Returns where the value is between <c>start</c> and <c>end</c>", "The position of the value between the two bounds",
                "a=The value to place;start=The value at t = 0;end=The value at t = 1")
        },
        {
            "remap",
            ("Remaps the value from the source range into the destination range", "The remapped value",
                "a=The value to remap;src_start=The lower bound of the source range;src_end=The upper bound of the source range;dst_start=The lower bound of the destination range;dst_end=The upper bound of the destination range")
        },
        { "square", ("Returns the value multiplied by itself", "The squared value", "a=The value") },
        { "fma", ("Fuses the multiplication and the addition of the three values, <c>(a * b) + c</c>", "The fused result", "a=Multiplier a;b=Multiplier b;c=Addend c") },
        { "fms", ("Fuses the multiplication and the subtraction of the three values, <c>(a * b) - c</c>", "The fused result", "a=Multiplier a;b=Multiplier b;c=Subtrahend c") },
        { "fnma", ("Fuses the multiplication and the subtraction of the three values, <c>c - (a * b)</c>", "The fused result", "a=Multiplier a;b=Multiplier b;c=Minuend c") },
        {
            "dot",
            ("Returns the product of the two values, it is the dot product of a vector of one component", "The product of the two values", "a=The value;b|other=The other value")
        },
        { "length_sq", ("Returns the squared length of the value, it is the value multiplied by itself without the square root", "The squared length", "a=The value") },
        { "distance_sq", ("Returns the squared distance between the two values", "The squared distance", "a=The value;b|to=The other value") },

        #endregion

        #region integer

        {
            "is_pow2", ("Returns true when the value is a power of two, a zero and a negative value are not a power of two", "True when the value is a power of two", "a=The value")
        },
        {
            "up2pow2",
            ("Returns the value rounded up to the next power of two, a value that is a power of two already is kept and a zero stays zero", "The rounded up value", "a=The value")
        },

        #endregion

        #region floating point

        {
            "mod",
            ("Returns the remainder of the division of the two values, it is the same as <c>a - b * floor(a / b)</c> with the product fused into a single rounding",
                "The remainder", "a=The value;b|other=The divisor")
        },
        { "modf", ("Splits the value into its integral and its fractional part", "The fractional part", "a=The value;i=Receives the integral part") },
        { "ceil", ("Returns the smallest integer that is not less than the value", "The rounded up value", "a=The value") },
        { "floor", ("Returns the largest integer that is not greater than the value", "The rounded down value", "a=The value") },
        { "round", ("Rounds the value to the nearest integer, a value that is exactly between two of them goes to the even one", "The rounded value", "a=The value") },
        { "trunc", ("Returns the integral part of the value", "The truncated value", "a=The value") },
        { "frac", ("Returns the fractional part of the value", "The fractional part", "a=The value") },
        { "rcp", ("Returns the reciprocal of the value", "The reciprocal", "a=The value") },
        { "saturate", ("Clamps the value into the range of zero and one", "The saturated value", "a=The value") },
        { "step", ("Returns 1 when the value is not less than the <c>threshold</c> and 0 when it is less", "The step value", "threshold=The threshold;a=The value") },
        {
            "smoothstep",
            ("Interpolates smoothly between <c>min</c> and <c>max</c>, the result is 0 below the minimum, 1 above the maximum and a smooth hermite curve in between",
                "The interpolated value", "min=The value at 0;max=The value at 1;a=The value to place between the two bounds")
        },
        {
            "reflect",
            ("Returns the value reflected around the normal <c>n</c>, it has to be normalized", "The reflected value",
                "a=The value to reflect;n=The normalized normal of the surface")
        },
        {
            "project",
            ("Returns the projection of the value onto <c>onto</c>, it is the component that is parallel to it", "The projected value",
                "a=The value to project;onto=The value to project onto, it does not have to be normalized")
        },
        {
            "project_normalized",
            ("Returns the projection of the value onto the normalized <c>onto</c>", "The projected value",
                "a=The value to project;onto=The normalized value to project onto")
        },
        {
            "project_on_plane",
            ("Returns the projection of the value onto the plane that has <c>plane_normal</c> as its normal, it is the component that is inside the plane", "The projected value",
                "a=The value to project;plane_normal=The normal of the plane, it does not have to be normalized")
        },
        {
            "project_on_plane_normalized",
            ("Returns the projection of the value onto the plane that has the normalized <c>plane_normal</c> as its normal", "The projected value",
                "a=The value to project;plane_normal=The normalized normal of the plane")
        },
        {
            "project_safe",
            ("Returns the projection of the value onto <c>onto</c>, it returns <c>default_value</c> when the projection is not finite", "The projected value",
                "a=The value to project;onto=The value to project onto;default_value=The value that is returned when the projection is not finite")
        },
        { "radians", ("Converts the value from degrees to radians", "The value in radians", "a=The value in degrees") },
        { "degrees", ("Converts the value from radians to degrees", "The value in degrees", "a=The value in radians") },
        { "wrap", ("Wraps the value into the range of <c>min</c> and <c>max</c>", "The wrapped value", "a=The value;min=The lower bound;max=The upper bound") },

        #endregion

        #region ieee754

        { "sqrt", ("Returns the square root of the value", "The square root", "a=The value") },
        { "rsqrt", ("Returns the reciprocal of the square root of the value, it is the same as <c>rcp(sqrt())</c>", "The reciprocal of the square root", "a=The value") },
        { "length", ("Returns the length of the value, it is the same as its absolute value", "The length of the value", "a=The value") },
        {
            "distance",
            ("Returns the distance between the two values, it is the same as the absolute value of their difference", "The distance", "a=The value;b|to=The other value")
        },
        { "normalize", ("Returns the value scaled to a length of 1, the result is a NaN when the length is zero", "The normalized value", "a=The value to normalize") },
        { "normalize_safe", ("Returns the value scaled to a length of 1, it returns zero when the length is zero", "The normalized value", "a=The value to normalize") },
        {
            "refract",
            ("Returns the refraction direction, the incident value has to be normalized and the normal has to point against it", "The refracted direction",
                "i=The normalized value of the incoming direction;n=The normalized normal that points against <c>i</c>;index_of_refraction=The ratio between the index of refraction of the two materials")
        },
        { "is_NaN", ("Returns true when the value is NaN", "True when the value is NaN", "a=The value") },
        { "is_finite", ("Returns true when the value is finite, so it is neither NaN nor an infinity", "True when the value is finite", "a=The value") },
        { "is_inf", ("Returns true when the value is a positive or a negative infinity", "True when the value is an infinity", "a=The value") },
        { "is_pos_inf", ("Returns true when the value is a positive infinity", "True when the value is a positive infinity", "a=The value") },
        { "is_neg_inf", ("Returns true when the value is a negative infinity", "True when the value is a negative infinity", "a=The value") },
        { "log", ("Returns the natural logarithm of the value, the second one is the base of the logarithm", "The logarithm", "a=The value;b|other=The base of the logarithm") },
        { "log2", ("Returns the base 2 logarithm of the value", "The base 2 logarithm", "a=The value") },
        { "log10", ("Returns the base 10 logarithm of the value", "The base 10 logarithm", "a=The value") },
        { "exp", ("Returns <c>e</c> raised to the power of the value", "The exponential", "a=The value") },
        { "exp2", ("Returns 2 raised to the power of the value", "The exponential", "a=The value") },
        { "exp10", ("Returns 10 raised to the power of the value", "The exponential", "a=The value") },
        { "pow", ("Returns the value raised to the power of the exponent", "The power", "a=The value;b|other|v=The exponent") },
        { "sin", ("Returns the sine of the value in radians", "The sine", "a=The value") },
        { "cos", ("Returns the cosine of the value in radians", "The cosine", "a=The value") },
        { "tan", ("Returns the tangent of the value in radians", "The tangent", "a=The value") },
        { "asin", ("Returns the arc sine of the value, the result is in radians", "The arc sine", "a=The value") },
        { "acos", ("Returns the arc cosine of the value, the result is in radians", "The arc cosine", "a=The value") },
        { "atan", ("Returns the arc tangent of the value, the result is in radians", "The arc tangent", "a=The value") },
        {
            "atan2",
            ("Returns the arc tangent of the quotient of the two values, the signs of both are used to find the quadrant of the result", "The arc tangent in radians",
                "a=The numerator;b|v=The divisor")
        },
        { "sinh", ("Returns the hyperbolic sine of the value", "The hyperbolic sine", "a=The value") },
        { "cosh", ("Returns the hyperbolic cosine of the value", "The hyperbolic cosine", "a=The value") },
        { "tanh", ("Returns the hyperbolic tangent of the value", "The hyperbolic tangent", "a=The value") },
        { "asinh", ("Returns the inverse hyperbolic sine of the value", "The inverse hyperbolic sine", "a=The value") },
        { "acosh", ("Returns the inverse hyperbolic cosine of the value", "The inverse hyperbolic cosine", "a=The value") },
        { "atanh", ("Returns the inverse hyperbolic tangent of the value", "The inverse hyperbolic tangent", "a=The value") },
        { "sincos", ("Returns the sine and the cosine of the value in radians", "The sine and the cosine", "a=The value;sin=Receives the sine;cos=Receives the cosine") },

        #endregion

        #region select

        {
            "select",
            ("Returns the value of the second parameter where the condition is true and the value of the third where it is false", "The selected value",
                "c=The condition;t|a=The value that is returned when the condition is true;f|b=The value that is returned when the condition is false")
        },

        #endregion
    };

    /// <summary>
    /// Emits the documentation of a member: the summary of the operation, the documentation of the parameters of
    /// its signature and its return value.
    /// <para>A parameter that has no documentation of its own leaves every parameter to the summary of the
    /// operation, a comment that documents only a part of the parameters is not a comment.</para>
    /// </summary>
    /// <param name="sb">The builder of the file</param>
    /// <param name="indent">The indentation of the member</param>
    /// <param name="name">The name of the member</param>
    /// <param name="parameters">The names of the parameters of the member</param>
    public static void Emit(StringBuilder sb, string indent, string name, string[] parameters)
    {
        if (!Docs.TryGetValue(name, out var doc)) return;

        sb.Append(indent).Append("/// <summary>").Append(doc.Summary).AppendLine("</summary>");

        var map = new Dictionary<string, string>();
        foreach (var (parameter, text) in ParameterDocs(doc.Params)) map[parameter] = text;

        // the parameters of the signature are emitted in the order of it
        var found = new string[parameters.Length];
        var complete = true;
        for (var i = 0; i < parameters.Length; i++)
        {
            if (map.TryGetValue(parameters[i], out var text)) found[i] = text;
            else complete = false;
        }

        if (complete)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                sb.Append(indent).Append("/// <param name=\"").Append(parameters[i]).Append("\">").Append(found[i]).AppendLine("</param>");
            }
        }

        if (doc.Returns.Length != 0)
        {
            sb.Append(indent).Append("/// <returns>").Append(doc.Returns).AppendLine("</returns>");
        }
    }

    /// <summary>
    /// Returns the documentation of every parameter of an operation, in the order of the table.
    /// </summary>
    /// <param name="docs">The documentation of the parameters</param>
    /// <returns>The name and the documentation of every parameter</returns>
    private static IEnumerable<(string Name, string Text)> ParameterDocs(string docs)
    {
        foreach (var part in docs.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var equals = part.IndexOf('=');
            if (equals < 0) continue;
            var text = part[(equals + 1)..];
            // a parameter can have more than one name, the member that is called on a value names the other
            // operand other where the function calls it b
            foreach (var name in part[..equals].Split('|')) yield return (name, text);
        }
    }
}
