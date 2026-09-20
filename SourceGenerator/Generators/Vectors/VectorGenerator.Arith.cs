using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the arithmetic members of the vector described by <paramref name="typ"/>, they implement one of
    /// the <c>IVectorArithmetic</c> interfaces. They are emitted into their own file, so the base members and the
    /// arithmetic members stay separate.
    /// </summary>
    private static string GenArith(Typ typ, int size)
    {
        var type = $"{typ.name}{size}";
        var scalar = typ.compType;
        var byteSize = typ.size * (size == 3 ? 4 : size);
        var bitSize = 8 * byteSize;
        // the bit size of a padded 4 component vector and of a 2 component one, the simd reductions
        // pick their implementation by them
        var bitSize2 = 8 * typ.size * 2;
        var bitSize4 = 8 * typ.size * 4;
        var simd = typ.simd;
        var sig = typ.sig;
        var f = typ.f;
        var i = typ.i;
        var vecName = $"Vector{bitSize}";
        var cast = typ.arithCast;
        var attr = "[MethodImpl(256)]";

        var comp = VectorGenShared.Components(size);

        // a signed vector also has the negation operator, a 3 component vector also has the cross product
        var ifaces = new List<string>();
        if (sig) ifaces.Add($"ISignedVectorArithmetic<{type}, {scalar}>");
        else if (size != 3) ifaces.Add($"IVectorArithmetic<{type}, {scalar}>");
        if (size == 3) ifaces.Add($"IVector3Arithmetic<{type}, {scalar}>");

        var sb = new StringBuilder();

        // the members inherit their documentation from the interface they implement
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        string Join(Func<int, string> get, string sep = ", ") => VectorGenShared.Join(size, get, sep);

        // the component wise construction of the result, every component is cast back to the scalar type
        string NewCompWise(Func<int, string> get) => $"new({Join(n => $"({scalar})({get(n)})")})";

        // emits the accelerated simd fast path, the fallback is used when the simd type is not accelerated,
        // both statements include the return
        void EmitAccel(string accelerated, string fallback)
        {
            if (simd)
            {
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            {accelerated}");
            }

            sb.AppendLine($"        {fallback}");
        }

        VectorGenShared.FileHeader(sb, true);
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// The arithmetic members of {type}, they implement {string.Join(" and ", ifaces)}");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public partial struct {type} :");
        for (var n = 0; n < ifaces.Count; n++)
        {
            sb.AppendLine($"    {ifaces[n]}" + (n == ifaces.Count - 1 ? "" : ","));
        }

        sb.AppendLine("{");

        #region operators

        sb.AppendLine();
        sb.AppendLine("    #region operators");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator +({type} a) => a;");
        sb.AppendLine();

        if (sig)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator -({type} a)");
            sb.AppendLine("    {");
            EmitAccel("return new(-a.vector);", $"return new({Join(n => $"{cast}(-a.{comp[n]})")});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // emits an element wise binary operator
        void EmitBinOp(string op, string accelerated, string fallback)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator {op}({type} a, {type} b)");
            sb.AppendLine("    {");
            EmitAccel(accelerated, $"return {fallback};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitBinOp("+", "return new(a.vector + b.vector);",
            NewCompWise(n => $"a.{comp[n]} + b.{comp[n]}"));
        EmitBinOp("-", "return new(a.vector - b.vector);",
            NewCompWise(n => $"a.{comp[n]} - b.{comp[n]}"));
        EmitBinOp("*", "return new(a.vector * b.vector);",
            NewCompWise(n => $"a.{comp[n]} * b.{comp[n]}"));
        // the padding lane of a 3 component integer vector is zero, dividing by it would throw
        EmitBinOp("/",
            "return new(a.vector / b.vector" + (simd && i && size == 3 ? $".WithElement(3, {typ.one})" : "") + ");",
            NewCompWise(n => $"a.{comp[n]} / b.{comp[n]}"));
        // the integer remainder is derived from the division, so it goes through the guarded division operator
        if (f)
            EmitBinOp("%", "return new(simd.Rem(a.vector, b.vector));",
                NewCompWise(n => $"a.{comp[n]} % b.{comp[n]}"));
        else
            EmitBinOp("%", "return a - (a / b) * b;",
                NewCompWise(n => $"a.{comp[n]} % b.{comp[n]}"));

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region sign

        sb.AppendLine("    #region sign");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} abs()");
        sb.AppendLine("    {");
        EmitAccel($"return new({vecName}.Abs(vector));", $"return {NewCompWise(n => $"{comp[n]}.abs()")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // unsigned components can not be negative, their sign is a mask of zero or one
        var signOp = f ? "SignFloat" : sig ? "SignInt" : "SignUInt";
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} sign()");
        sb.AppendLine("    {");
        EmitAccel($"return new(simd.{signOp}(vector));", $"return {NewCompWise(n => $"{comp[n]}.sign()")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region min max clamp

        sb.AppendLine("    #region min max clamp");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} min(in {type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return new({vecName}.Min(vector, other.vector));",
            $"return {NewCompWise(n => $"{comp[n]}.min(other.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} max(in {type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return new({vecName}.Max(vector, other.vector));",
            $"return {NewCompWise(n => $"{comp[n]}.max(other.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} clamp(in {type} min, in {type} max)");
        sb.AppendLine("    {");
        EmitAccel($"return new({vecName}.Max(min.vector, {vecName}.Min(max.vector, vector)));",
            $"return {NewCompWise(n => $"{comp[n]}.clamp(min.{comp[n]}, max.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} clamp({scalar} min, {scalar} max) => clamp(new {type}(min), new {type}(max));");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region lerp unlerp remap

        sb.AppendLine("    #region lerp unlerp remap");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} lerp(in {type} start, in {type} end)");
        sb.AppendLine("    {");
        if (simd)
        {
            sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
            sb.AppendLine(f
                ? $"            return new({vecName}.Lerp(start.vector, end.vector, vector));"
                : "            return fma(this, end - start, start);");
        }

        sb.AppendLine("        return start + this * (end - start);");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} lerp({scalar} t, in {type} start, in {type} end)");
        sb.AppendLine("    {");
        EmitAccel($"return fma(new {type}(t), end - start, start);",
            $"return start + new {type}(t) * (end - start);");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} lerp({scalar} start, {scalar} end)");
        sb.AppendLine("    {");
        EmitAccel($"return fma(this, {cast}(end - start), new {type}(start));",
            $"return new {type}(start) + this * new {type}({cast}(end - start));");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} unlerp(in {type} start, in {type} end) => (this - start) / (end - start);");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} unlerp({scalar} start, {scalar} end) => (this - start) / new {type}({cast}(end - start));");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} unlerp({scalar} a, in {type} start, in {type} end) => (a - start) / (end - start);");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} remap(in {type} src_start, in {type} src_end, in {type} dst_start, in {type} dst_end) =>");
        sb.AppendLine("        unlerp(src_start, src_end).lerp(dst_start, dst_end);");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} remap({scalar} src_start, {scalar} src_end, {scalar} dst_start, {scalar} dst_end) =>");
        sb.AppendLine("        unlerp(src_start, src_end).lerp(dst_start, dst_end);");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region dot length_sq distance_sq square

        sb.AppendLine("    #region dot length_sq distance_sq square");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {scalar} dot(in {type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return {vecName}.Dot(vector, other.vector);",
            $"return ({scalar})({Join(n => $"{comp[n]} * other.{comp[n]}", " + ")});");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {scalar} length_sq()");
        sb.AppendLine("    {");
        EmitAccel($"return {vecName}.Dot(vector, vector);",
            $"return ({scalar})({Join(n => $"{comp[n]} * {comp[n]}", " + ")});");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {scalar} distance_sq(in {type} to) => (to - this).length_sq();");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} square() => this * this;");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region fma

        sb.AppendLine("    #region fma");
        sb.AppendLine();

        // emits one of the fused multiply add variants
        void EmitFma(string name, string accelerated, string scalarName)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} {name}(in {type} a, in {type} b, in {type} c)");
            sb.AppendLine("    {");
            EmitAccel(accelerated, $"return {NewCompWise(n => $"a.{comp[n]}.{scalarName}(b.{comp[n]}, c.{comp[n]})")};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitFma("fma",
            f ? "return new(simd.Fma(a.vector, b.vector, c.vector));" : "return (a * b) + c;", "fma");
        EmitFma("fms",
            f ? "return new(simd.Fms(a.vector, b.vector, c.vector));" : "return (a * b) - c;", "fms");
        EmitFma("fnma",
            f ? "return new(simd.Fnma(a.vector, b.vector, c.vector));" : "return c - (a * b);", "fnma");

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region csum cmin cmax

        sb.AppendLine("    #region csum cmin cmax");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {scalar} csum()");
        sb.AppendLine("    {");
        EmitAccel($"return {vecName}.Sum(vector);", $"return {cast}({Join(n => comp[n], " + ")});");
        sb.AppendLine("    }");
        sb.AppendLine();

        // emits one of the component reductions, when the vector has a padding lane the reduction has to
        // ignore it, the safe variants only use the accelerated path when the padding lane is not a number
        void EmitReduce(string name, string scalarName, string vecOp, bool safeOnly)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public {scalar} {name}()");
            sb.AppendLine("    {");
            if (simd && (!safeOnly || i))
            {
                var op = size == 3 ? vecOp + "3" : vecOp;
                sb.AppendLine($"        if (Vector{bitSize4}.IsHardwareAccelerated || Vector{bitSize2}.IsHardwareAccelerated)");
                sb.AppendLine($"            return simd.{op}(vector);");
            }

            var chain = comp[0];
            for (var n = 1; n < size; n++) chain += $".{scalarName}({comp[n]})";
            sb.AppendLine($"        return {chain};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitReduce("cmin", "min", "CMin", false);
        EmitReduce("cmax", "max", "CMax", false);
        EmitReduce("cmin_safe", "min", "CMin", true);
        EmitReduce("cmax_safe", "max", "CMax", true);

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        if (size == 3)
        {
            #region cross

            sb.AppendLine("    #region cross");
            sb.AppendLine();

            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public {type} cross(in {type} other) => new(");
            sb.AppendLine($"        ({scalar})(y * other.z - z * other.y),");
            sb.AppendLine($"        ({scalar})(z * other.x - x * other.z),");
            sb.AppendLine($"        ({scalar})(x * other.y - y * other.x));");
            sb.AppendLine();

            sb.AppendLine("    #endregion");
            sb.AppendLine();

            #endregion
        }

        sb.AppendLine("}");
        sb.AppendLine();

        return sb.ToString();
    }
}
