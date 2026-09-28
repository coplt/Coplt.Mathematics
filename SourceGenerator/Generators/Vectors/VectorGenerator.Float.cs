using System;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// The name and the value of every math constant of the kind of a floating point number, the value is the
    /// literal of a double, the literal of the component type of a vector is built from it. The values are the
    /// ones of <see cref="SourceGenerator.MathConstants"/>.
    /// </summary>
    internal static readonly (string Name, string Value)[] FloatConsts =
    {
        ("E", SourceGenerator.MathConstants.E),
        ("Log2", SourceGenerator.MathConstants.Log2),
        ("Log10", SourceGenerator.MathConstants.Log10),
        ("PI", SourceGenerator.MathConstants.Pi),
        ("Tau", SourceGenerator.MathConstants.Tau),
        ("RadToDeg", SourceGenerator.MathConstants.RadToDeg),
        ("DegToRad", SourceGenerator.MathConstants.DegToRad),
    };

    /// <summary>
    /// The name of the constant of the denominator of a quotient of the kind of a floating point value. Its value
    /// is not a constant of the ieee 754 standard and it is not the same for every kind of it, so it is not one of
    /// the values of <see cref="FloatConsts"/>, the literal of it comes from
    /// <see cref="VectorGenShared.DenomEpsilonValue"/>.
    /// </summary>
    internal const string DenomEpsilonName = "DenomEpsilon";

    /// <summary>
    /// Generates the floating point members of the vector described by <paramref name="typ"/>, which are the math
    /// constants of the kind of it. The members of the group of a floating point vector are the same functions,
    /// they are emitted into their own file so they stay separate from the base members and the plain arithmetic.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenFloat(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var attr = "[MethodImpl(256)]";

        var sb = new StringBuilder();

        // the members implement the interface, they inherit their documentation from it
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        // the literal of a value of the component type of the vector, half is a cast of the float literal
        string Lit(string value) => typ.name switch
        {
            "float" => $"{value}f",
            "double" => value,
            _ => $"({scalar})({value}f)",
        };


        #region constants

        sb.AppendLine();
        sb.AppendLine("    #region constants");
        sb.AppendLine();

        foreach (var (name, value) in FloatConsts)
        {
            InheritDoc();
            sb.AppendLine($"    public static {scalar} Scalar{name}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {Lit(value)};");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {type} {name}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => new({Lit(value)});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // the constant of the denominator of a quotient of the kind of the value, the value of it is the one of
        // the kind of the component and not one of the values of the table above
        {
            var denom = VectorGenShared.DenomEpsilonValue(scalar);
            InheritDoc();
            sb.AppendLine($"    public static {scalar} ScalarDenomEpsilon");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {denom};");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {type} DenomEpsilon");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => new({denom});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        return VectorDocs.Apply(sb.ToString());
    }
}
