using System;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// The name of the constant of the smallest positive normal value of the ieee 754 standard. The component type
    /// of a vector does not carry it, its value is written out, see <see cref="VectorGenShared.MinNormalValue"/>.
    /// </summary>
    internal const string MinNormalName = "MinNormal";

    /// <summary>
    /// The name of every constant of the ieee 754 standard, the value of one of them is the member of the scalar
    /// type of the component of a vector of the same name. The smallest positive normal value is the only one of
    /// them whose value is not held by a member of the kind of it.
    /// </summary>
    internal static readonly string[] IeeeConsts =
    {
        "Epsilon",
        MinNormalName,
        "NaN",
        "NegativeInfinity",
        "NegativeZero",
        "PositiveInfinity",
    };

    /// <summary>
    /// Generates the members of the legacy <c>IVectorFloatingPointIeee754</c> interface of the vector described
    /// by <paramref name="typ"/>: the logarithm, the exponential, the step and the refraction, the face forward,
    /// the trigonometry, the hyperbolics and the change of the sign. The part is the one the migration removes
    /// once the family of the interface is gone, so it is emitted into the file of the value itself under the
    /// region of the interface it implements.
    /// <para>The constants of the kind of a component are the only members of the part that do not belong to the
    /// interface and stay, they are emitted into a region of their own, which the plain floating point members
    /// carry as well: they name the representation of the kind and not an operation of it.</para>
    /// <para>The members whose op is reached through the algebra of the kind of the value do not live here: the
    /// power, the square root and its reciprocal, the normalization and the length and the distance of two
    /// vectors are the ones of the <c>math</c> class and of the extension of a value.</para>
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenIeee(Typ typ, int size, bool storeVariant)
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
                $"return {NewCompWise(n => VectorScalar.Expr(scalar, scalarName, comp[n]))};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }


        // the members of the legacy ieee 754 interface of the kind of the value, the constants of the kind are
        // emitted beside the value itself, see GenIeeeConsts
        sb.AppendLine();
        sb.AppendLine("    #region IVectorFloatingPointIeee754");

        #region log

        sb.AppendLine();
        sb.AppendLine("    #region log");
        sb.AppendLine();

        Unary("log", "Log", "log");
        Unary("log2", "Log2", "log2");

        // the logarithm of any base is the quotient of the two logarithms. The receiver is the first parameter
        // because the interface declares the member as a static one, and a member of the vector that takes the
        // vector itself as its only parameter would have the same parameter list as the one of the natural
        // logarithm
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} log(in {type} a, in {type} b)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector("simd.Log(a.vector) / simd.Log(b.vector)")};",
            $"return {From128($"simd.Log({Load64("a.")}) / simd.Log({Load64("b.")})")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "log_base", $"a.{comp[n]}", $"b.{comp[n]}"))};");
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
        sb.AppendLine($"        return k >= {VectorScalar.Zero(scalar)}");
        sb.AppendLine("            ? index_of_refraction * i - (index_of_refraction * ni + " + VectorScalar.Expr(scalar, "sqrt", "k") + ") * n");
        sb.AppendLine("            : default;");
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
        sb.AppendLine($"    public readonly {type} face_forward(in {type} i, in {type} ng) => ng.dot(i) >= {VectorScalar.Zero(scalar)} ? -this : this;");
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

        for (var i = 0; i < size; i++) sb.AppendLine($"        var (s{i}, c{i}) = {VectorScalar.Expr(scalar, "sincos", $"this.{comp[i]}")};");
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
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "atan2", comp[n], $"v.{comp[n]}"))};");
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

        sb.AppendLine("    #endregion");

        return VectorDocs.Apply(sb.ToString());
    }

    /// <summary>
    /// Generates the file of the members of the legacy interfaces of the vector described by
    /// <paramref name="typ"/>: the forwarding of the families of the interfaces of the value and the members of
    /// the legacy ieee 754 interface of it. They are emitted into a file of their own, which the migration
    /// removes as a whole once the families of the interfaces are gone. The constants of the kind of a component
    /// are not a part of it, they are the representation of the kind and are emitted beside the value itself.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenLegacy(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, true);
        sb.AppendLine($"public partial struct {type}");
        sb.AppendLine("{");
        sb.Append(GenIface(typ, size, storeVariant).Trim('\r', '\n'));
        // the ieee 754 interface is the one of a floating point kind of a number alone
        if (typ.f)
        {
            sb.AppendLine();
            sb.Append(GenIeee(typ, size, storeVariant).Trim('\r', '\n'));
        }

        sb.AppendLine();
        sb.AppendLine("}");

        return VectorDocs.Apply(VectorGenShared.Normalize(sb.ToString()));
    }

    /// <summary>
    /// Generates the constants of the ieee 754 standard of the vector described by <paramref name="typ"/>: the
    /// smallest positive value, the smallest positive normal value, the nan, the two infinities and the negative
    /// zero of the kind of a component. They are the representation of the kind and not a member of the legacy
    /// interface of it, so they are emitted beside the value itself.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The constants</returns>
    private static string GenIeeeConsts(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var attr = "[MethodImpl(256)]";

        var sb = new StringBuilder();

        // the members inherit their documentation from the constants of the kind of a component
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        sb.AppendLine();
        sb.AppendLine("    #region constants");
        sb.AppendLine();

        // every component of the value is the constant of the component type, the one of the standard itself. The
        // smallest positive normal value of the kind of the component is not held by a member of it, so the value
        // of that constant is written out beside the name of the member the other ones come from
        foreach (var name in IeeeConsts)
        {
            var value = name == MinNormalName ? VectorGenShared.MinNormalValue(scalar) : $"{scalar}.{name}";
            InheritDoc();
            sb.AppendLine($"    public static {scalar} Scalar{name}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {value};");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {type} {name}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => new({value});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");

        return sb.ToString();
    }
}
