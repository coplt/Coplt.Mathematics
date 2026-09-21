using System;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the ieee 754 members of the vector described by <paramref name="typ"/>, they implement
    /// <c>IVectorFloatingPointIeee754BoolOps</c>: the logarithm, the exponential, the power, the square root and
    /// its reciprocal, the length and the distance, the normalization, the step and the refraction, the safe
    /// projection, the face forward, the trigonometry, the hyperbolics and the change of the sign, and the checks
    /// of the special floating point values that produce a bool vector. They are emitted into their own file, so
    /// they stay separate from the floating point members and the plain arithmetic.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenIeee(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        // the bool vector that has the same number of components as the vector
        var boolType = $"b{typ.size * 8}v{size}";
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        // the value of a 64 bit vector is kept in a raw ulong field, the other simd vectors keep the register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var pad = VectorGenShared.PadLanes(typ, size, storeVariant) > 0;
        var vecName = $"Vector{reg}";
        var attr = "[MethodImpl(256)]";
        // the mask of a check is held by the bool vector of the same size, the register of it is 128 bits wide for
        // a bool of 4 byte components and the register of the vector itself for a bool of 8 byte ones
        var boolRegName = $"Vector{(typ.size == 4 ? 128 : reg)}";
        // the mask of a bool of 4 byte components is a 32 bit one and the mask of a bool of 8 byte ones is 64 bits
        var boolAs = typ.size == 4 ? "AsUInt32" : "AsUInt64";
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
        string FromVector(string expr, bool masked = true) => VectorGenShared.Vector(simd, pad, expr, masked);

        // the literal of a value of the component type of the vector, half is a cast of the float literal
        string Lit(string value) => typ.name switch
        {
            "float" => $"{value}f",
            "double" => value,
            _ => $"({scalar})({value}f)",
        };

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

        // emits a member that maps a helper over every component. The helper of the 128 bit register is used for
        // the widened form of a 64 bit vector. A helper of the BCL is called on the register of the vector and
        // has a form for every width, a helper of the simd utilities has a form for every width as well
        void Unary(string name, string method, string scalarName, bool bcl = false)
        {
            var accelFn = bcl ? $"{vecName}.{method}" : $"simd.{method}";
            var wideFn = bcl ? $"Vector128.{method}" : $"simd.{method}";
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public readonly {type} {name}()");
            sb.AppendLine("    {");
            EmitAccel($"return {FromVector($"{accelFn}(vector)")};",
                $"return {From128($"{wideFn}({Load64("")})")};",
                $"return {NewCompWise(n => $"{comp[n]}.{scalarName}()")};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // emits the check of a special value, a register has the mask of the BCL and a type without a register
        // checks every component on its own
        void BoolMember(string name, string fn, string scalarName)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public readonly {boolType} {name}()");
            sb.AppendLine("    {");
            if (simd)
            {
                // the mask of a 64 bit vector is widened to the register of the bool vector, the register of a
                // bool of 4 byte components is 128 bits wide
                if (v64)
                {
                    sb.AppendLine("        if (Vector64.IsHardwareAccelerated)");
                    sb.AppendLine($"            return new(Vector128.Create(Vector64.{fn}(vector).{boolAs}()));");
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    sb.AppendLine($"            return new(Vector128.{fn}({Load64("")}).{boolAs}());");
                }
                else
                {
                    sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                    sb.AppendLine($"            return new {boolType}({boolRegName}.{fn}(vector).{boolAs}());");
                }
            }

            sb.AppendLine($"        return new({Join(n => $"{comp[n]}.{scalarName}()")});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        VectorGenShared.FileHeader(sb, true);
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    IVectorFloatingPointIeee754BoolOps<{type}, {scalar}, {boolType}>");
        sb.AppendLine("{");

        #region log

        sb.AppendLine();
        sb.AppendLine("    #region log");
        sb.AppendLine();

        Unary("log", "Log", "log");
        Unary("log2", "Log2", "log2");

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} log(in {type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd.Log(vector) / simd.Log(other.vector)")};",
            $"return {From128($"simd.Log({Load64("")}) / simd.Log({Load64("other.")})")};",
            $"return {NewCompWise(n => $"{comp[n]}.log(other.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        Unary("log10", "Log10", "log10");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region exp

        sb.AppendLine("    #region exp");
        sb.AppendLine();

        Unary("exp", "Exp", "exp");
        Unary("exp2", "Exp2", "exp2");
        Unary("exp10", "Exp10", "exp10");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region pow sqrt rsqrt

        sb.AppendLine("    #region pow sqrt rsqrt");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} pow({type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd.Pow(vector, other.vector)")};",
            $"return {From128($"simd.Pow({Load64("")}, {Load64("other.")})")};",
            $"return {NewCompWise(n => $"{comp[n]}.pow(other.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} pow({scalar} v)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"simd.Pow(vector, {vecName}.Create(v))")};",
            $"return {From128($"simd.Pow({Load64("")}, Vector128.Create(v))")};",
            $"return {NewCompWise(n => $"{comp[n]}.pow(v)")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        Unary("sqrt", "Sqrt", "sqrt", bcl: true);
        Unary("rsqrt", "RSqrt", "rsqrt");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region length distance

        sb.AppendLine("    #region length distance");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {scalar} length() => length_sq().sqrt();");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {scalar} distance(in {type} to) => (to - this).length();");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region normalize

        sb.AppendLine("    #region normalize");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} normalize() => this * length_sq().rsqrt();");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} normalize_safe()");
        sb.AppendLine("    {");
        sb.AppendLine("        var len = length_sq();");
        // the smallest normal value of the component type is the bound, a shorter vector is zero for the result
        sb.AppendLine($"        return len > {Lit("1.175494351e-38")} ? this * len.rsqrt() : default;");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region step refract

        sb.AppendLine("    #region step refract");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} step(in {type} threshold)");
        sb.AppendLine("    {");
        if (simd)
        {
            sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
            sb.AppendLine($"            return new({vecName}.ConditionalSelect(" +
                          $"{vecName}.GreaterThanOrEqual(vector, threshold.vector), " +
                          $"{vecName}<{typ.simdComp}>.One, {vecName}<{typ.simdComp}>.Zero));");
            if (v64)
            {
                sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                sb.AppendLine($"            return {From128(
                    $"Vector128.ConditionalSelect(Vector128.GreaterThanOrEqual({Load64("")}, {Load64("threshold.")}), " +
                    $"Vector128<{typ.simdComp}>.One, Vector128<{typ.simdComp}>.Zero)")};");
            }
        }

        sb.AppendLine($"        return {NewCompWise(n => $"{comp[n]} >= threshold.{comp[n]} ? {type}.ScalarOne : {type}.ScalarZero")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the refraction is undefined when the root of k is not real, the result is a zero vector there
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} refract(in {type} i, in {type} n, {scalar} index_of_refraction)");
        sb.AppendLine("    {");
        sb.AppendLine("        var ni = i.dot(n);");
        sb.AppendLine($"        var k = ({scalar})({Lit("1")} - index_of_refraction * index_of_refraction * ({Lit("1")} - ni * ni));");
        sb.AppendLine($"        return k >= ({scalar})0");
        sb.AppendLine("            ? index_of_refraction * i - (index_of_refraction * ni + k.sqrt()) * n");
        sb.AppendLine("            : default;");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region project safe

        sb.AppendLine("    #region project safe");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} project_safe(in {type} onto) => project_safe(onto, default);");
        sb.AppendLine();

        // the projection of a degenerate vector is an infinity or a nan, the default is returned for it
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} project_safe(in {type} onto, in {type} default_value)");
        sb.AppendLine("    {");
        sb.AppendLine("        var proj = this.project(onto);");
        sb.AppendLine($"        return {Join(i => $"{scalar}.IsFinite(proj.{comp[i]})", " && ")} ? proj : default_value;");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region face forward

        sb.AppendLine("    #region face forward");
        sb.AppendLine();

        // the vector is flipped when the dot product of the normal and the incident vector is not negative
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} face_forward(in {type} i, in {type} ng) => ng.dot(i) >= ({scalar})0 ? -this : this;");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region sin cos tan

        sb.AppendLine("    #region sin cos tan");
        sb.AppendLine();

        Unary("sin", "Sin", "sin", bcl: true);
        Unary("cos", "Cos", "cos", bcl: true);

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly ({type} sin, {type} cos) sincos()");
        sb.AppendLine("    {");
        sb.AppendLine("        sincos(out var sin, out var cos);");
        sb.AppendLine("        return (sin, cos);");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly void sincos(out {type} sin, out {type} cos)");
        sb.AppendLine("    {");
        if (simd)
        {
            // the pair of the sine and the cosine is computed by the register of the vector
            sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var (s, c) = {vecName}.SinCos(vector);");
            sb.AppendLine($"            sin = {FromVector("s")};");
            sb.AppendLine($"            cos = {FromVector("c")};");
            sb.AppendLine("            return;");
            sb.AppendLine("        }");
            // the 64 bit register of the value is not accelerated on every platform
            if (v64)
            {
                sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                sb.AppendLine("        {");
                sb.AppendLine($"            var (s, c) = Vector128.SinCos({Load64("")});");
                sb.AppendLine($"            sin = {From128("s")};");
                sb.AppendLine($"            cos = {From128("c")};");
                sb.AppendLine("            return;");
                sb.AppendLine("        }");
            }
        }

        for (var i = 0; i < size; i++) sb.AppendLine($"        this.{comp[i]}.sincos(out var s{i}, out var c{i});");
        sb.AppendLine($"        sin = new({Join(i => $"s{i}")});");
        sb.AppendLine($"        cos = new({Join(i => $"c{i}")});");
        sb.AppendLine("    }");
        sb.AppendLine();

        Unary("tan", "Tan", "tan");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region asin acos atan atan2

        sb.AppendLine("    #region asin acos atan atan2");
        sb.AppendLine();

        Unary("asin", "Asin", "asin");
        Unary("acos", "Acos", "acos");
        Unary("atan", "Atan", "atan");

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} atan2(in {type} v)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd.Atan2(vector, v.vector)")};",
            $"return {From128($"simd.Atan2({Load64("")}, {Load64("v.")})")};",
            $"return {NewCompWise(n => $"{comp[n]}.atan2(v.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region sinh cosh tanh

        sb.AppendLine("    #region sinh cosh tanh");
        sb.AppendLine();

        Unary("sinh", "Sinh", "sinh");
        Unary("cosh", "Cosh", "cosh");
        Unary("tanh", "Tanh", "tanh");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region asinh acosh atanh

        sb.AppendLine("    #region asinh acosh atanh");
        sb.AppendLine();

        Unary("asinh", "Asinh", "asinh");
        Unary("acosh", "Acosh", "acosh");
        Unary("atanh", "Atanh", "atanh");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region chg sign

        sb.AppendLine("    #region chg sign");
        sb.AppendLine();

        // the sign bit of the component type, the magnitude of this vector and the sign of the other one are the
        // two parts of the result
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} chg_sign(in {type} sign) => (sign & new {type}({Lit("-0.0")})) ^ this;");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region is_NaN is_finite is_inf is_pos_inf is_neg_inf

        sb.AppendLine("    #region is_NaN is_finite is_inf is_pos_inf is_neg_inf");
        sb.AppendLine();

        // a value is not equal to itself only when it is a nan
        sb.AppendLine("    #pragma warning disable CS1718");
        sb.AppendLine("    // ReSharper disable once EqualExpressionComparison");
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {boolType} is_NaN() => this != this;");
        sb.AppendLine("    #pragma warning restore CS1718");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {boolType} is_finite() => abs() < new {type}({scalar}.PositiveInfinity);");
        sb.AppendLine();

        BoolMember("is_inf", "IsInfinity", "isInf");
        BoolMember("is_pos_inf", "IsPositiveInfinity", "isPosInf");
        BoolMember("is_neg_inf", "IsNegativeInfinity", "isNegInf");

        sb.AppendLine("    #endregion");

        #endregion

        sb.AppendLine("}");
        return sb.ToString();
    }
}
