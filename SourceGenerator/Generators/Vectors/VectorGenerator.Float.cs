using System;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// The name and the value of every math constant of <c>IVectorFloatingPoint</c>, the value is the literal of
    /// a double, the literal of the component type of a vector is built from it. The values are the ones of
    /// <see cref="SourceGenerator.MathConstants"/>.
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
    /// Generates the floating point members of the vector described by <paramref name="typ"/>, they implement
    /// <c>IVectorFloatingPoint</c>: the math constants, the rounding, the split of a value into its integral and
    /// its fractional part, the reciprocal, the saturation, the smooth interpolation between two bounds, the
    /// reflection around a normal, the projections, the angle conversions and the two sided wrap. The members of
    /// the group of a floating point vector are the same functions, they are emitted into their own file so they
    /// stay separate from the base members and the plain arithmetic.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenFloat(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        // the value of a 64 bit vector is kept in a raw ulong field, the other simd vectors keep the register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var pad = VectorGenShared.PadLanes(typ, size, storeVariant) > 0;
        var vecName = $"Vector{reg}";
        var attr = "[MethodImpl(256)]";

        var comp = VectorGenShared.Components(size);

        var sb = new StringBuilder();

        // the members implement the interface, they inherit their documentation from it
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        string Join(Func<int, string> get, string sep = ", ") => VectorGenShared.Join(size, get, sep);

        // the component wise construction of the result, every component is cast back to the scalar type
        string NewCompWise(Func<int, string> get) => $"new({Join(n => $"({scalar})({get(n)})")})";

        // the 128 bit value of a 64 bit vector and the construction of a 64 bit vector from a 128 bit one
        string Load64(string self) => VectorGenShared.Load64(self, typ.simdComp);
        string From128(string expr) => VectorGenShared.From128(expr);

        // the construction of a simd result, see VectorGenShared.Vector. The mask keeps the padding lanes of a
        // result at zero, an expression that reads the zero padding lane of the vector can leave something else
        // there and has to be masked
        string FromVector(string expr, bool masked = false) => VectorGenShared.Vector(simd, pad, expr, masked);

        // emits the accelerated simd fast path, the scalar expression is used when nothing is accelerated,
        // every statement includes the return. The vector type is fixed, a value that is kept in a 64 bit
        // register can only reach 128 bit vectors by widening itself, that is the path for a platform that has
        // no 64 bit hardware support.
        void EmitAccel(string? accelerated, string? wide, string fallback)
        {
            if (simd && accelerated != null)
            {
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            {accelerated}");
                if (v64 && wide != null)
                {
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    sb.AppendLine($"            {wide}");
                }
            }

            sb.AppendLine($"        {fallback}");
        }

        // the literal of a value of the component type of the vector, half is a cast of the float literal
        string Lit(string value) => typ.name switch
        {
            "float" => $"{value}f",
            "double" => value,
            _ => $"({scalar})({value}f)",
        };

        VectorGenShared.FileHeader(sb, true);
        // the members of the floating point functions implement the interface of them
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    IVectorFloatingPoint<{type}, {scalar}>");
        sb.AppendLine("{");

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

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region mod modf

        sb.AppendLine("    #region mod modf");
        sb.AppendLine();

        // the modulo of the library truncates the quotient towards zero on both the simd path and the scalar one
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} mod(in {type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd.Mod(vector, other.vector)", true)};",
            $"return {From128($"simd.Mod({Load64("")}, {Load64("other.")})")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "mod", comp[n], $"other.{comp[n]}"))};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the integral part of a value is its truncation, the fractional part is what is left of it
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} modf(out {type} i)");
        sb.AppendLine("    {");
        sb.AppendLine("        i = trunc();");
        sb.AppendLine("        return this - i;");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region ceil floor round trunc frac

        sb.AppendLine("    #region ceil floor round trunc frac");
        sb.AppendLine();

        void Rounding(string name, string op)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public readonly {type} {name}()");
            sb.AppendLine("    {");
            EmitAccel($"return {FromVector($"{vecName}.{op}(vector)")};",
                $"return {From128($"Vector128.{op}({Load64("")})")};",
                $"return {NewCompWise(n => VectorScalar.Expr(scalar, name, comp[n]))};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        Rounding("ceil", "Ceiling");
        Rounding("floor", "Floor");
        Rounding("round", "Round");
        Rounding("trunc", "Truncate");

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} frac()");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"vector - {vecName}.Floor(vector)")};",
            $"return {From128($"{Load64("")} - Vector128.Floor({Load64("")})")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "frac", comp[n]))};");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region rcp saturate smoothstep reflect

        sb.AppendLine("    #region rcp saturate smoothstep reflect");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} rcp()");
        sb.AppendLine("    {");
        // the reciprocal of the zero padding lane is an infinity, the mask keeps it at zero
        EmitAccel($"return {FromVector("simd.Rcp(vector)", true)};",
            $"return {From128($"simd.Rcp({Load64("")})")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "rcp", comp[n]))};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the vector is clamped into the range of zero and one, the clamp carries the simd fast path itself
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} saturate() => math.clamp(this, default, {type}.One);");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} smoothstep(in {type} min, in {type} max)");
        sb.AppendLine("    {");
        sb.AppendLine("        var t = ((this - min) / (max - min)).saturate();");
        // (3 - (2 * t)) is the hermite curve of the interpolation, it is fused by the fma helper
        sb.AppendLine($"        return t * t * math.fnma(new {type}({Lit("2")}), t, new {type}({Lit("3")}));");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} reflect(in {type} n)");
        sb.AppendLine("    {");
        // this - (2 * n * dot(this, n)), the dot product is broadcast into the vector by the fma helper
        sb.AppendLine($"        return math.fnma(new {type}({Lit("2")}) * n, this.dot(n), this);");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region project

        sb.AppendLine("    #region project");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} project(in {type} onto) => this.dot(onto) / onto.dot(onto) * onto;");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} project_normalized(in {type} onto) => this.dot(onto) * onto;");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} project_on_plane(in {type} plane_normal) => this - this.project(plane_normal);");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} project_on_plane_normalized(in {type} plane_normal) => this - this.project_normalized(plane_normal);");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region radians degrees

        sb.AppendLine("    #region radians degrees");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} radians() => this * {type}.DegToRad;");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} degrees() => this * {type}.RadToDeg;");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region wrap

        sb.AppendLine("    #region wrap");
        sb.AppendLine();

        // a value that is not negative is wrapped into the range above the lower bound, a negative one into the
        // range below the upper bound, the offset is the remainder of the width of the range
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} wrap(in {type} min, in {type} max)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd_math.Wrap(vector, min.vector, max.vector)", true)};",
            $"return {From128($"simd_math.Wrap({Load64("")}, {Load64("min.")}, {Load64("max.")})")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "wrap", comp[n], $"min.{comp[n]}", $"max.{comp[n]}"))};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} wrap({scalar} min, {scalar} max)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd_math.Wrap(vector, min, max)", true)};",
            $"return {From128($"simd_math.Wrap({Load64("")}, min, max)")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "wrap", comp[n], "min", "max"))};");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");

        #endregion

        sb.AppendLine("}");
        return VectorDocs.Apply(sb.ToString());
    }
}
