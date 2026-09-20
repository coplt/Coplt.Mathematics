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
        // a scalar operation on a floating point component is a single instruction because the components always
        // live in a simd register, so a 2 component floating point vector gains nothing from a simd reduction
        var scalarOnly = f && bitSize == 64;

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

        // the construction of a simd result, see VectorGenShared.Vector. Only the division and the remainder of a
        // 3 component floating point vector need the mask, they divide the zero padding lane by zero
        string FromVector(string expr, bool masked = false) => VectorGenShared.Vector(simd, size, expr, masked);

        // emits the accelerated simd fast path, the scalar expression is used when nothing is accelerated,
        // every statement includes the return. The vector type is fixed, a 64 bit vector can only reach 128 bit
        // vectors by widening its value, that is the path for a platform that has no 64 bit hardware support.
        // The accelerated expression is null when the scalar expression is already the best the member can do.
        // The wide expression is null when a wider path does not pay off: the member is built from the operators
        // of the vector, which carry the 128 bit path themselves, or the scalar expression is already a single
        // operation.
        void EmitAccel(string? accelerated, string? wide, string fallback)
        {
            if (simd && accelerated != null)
            {
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            {accelerated}");
                if (bitSize == 64 && wide != null)
                {
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    sb.AppendLine($"            {wide}");
                }
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
            EmitAccel($"return {FromVector("-a.vector")};",
                $"return {FromVector("Vector128.GetLower(-a.vector.ToVector128())")};",
                $"return new({Join(n => $"{cast}(-a.{comp[n]})")});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // emits an element wise binary operator
        void EmitBinOp(string op, string accelerated, string? wide, string fallback)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator {op}({type} a, {type} b)");
            sb.AppendLine("    {");
            EmitAccel(accelerated, wide, $"return {fallback};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitBinOp("+", $"return {FromVector("a.vector + b.vector")};",
            $"return {FromVector("Vector128.GetLower(a.vector.ToVector128() + b.vector.ToVector128())")};",
            NewCompWise(n => $"a.{comp[n]} + b.{comp[n]}"));
        EmitBinOp("-", $"return {FromVector("a.vector - b.vector")};",
            $"return {FromVector("Vector128.GetLower(a.vector.ToVector128() - b.vector.ToVector128())")};",
            NewCompWise(n => $"a.{comp[n]} - b.{comp[n]}"));
        EmitBinOp("*", $"return {FromVector("a.vector * b.vector")};",
            $"return {FromVector("Vector128.GetLower(a.vector.ToVector128() * b.vector.ToVector128())")};",
            NewCompWise(n => $"a.{comp[n]} * b.{comp[n]}"));
        // the padding lane of a 3 component integer vector is zero, dividing by it would throw
        EmitBinOp("/",
            $"return {FromVector("a.vector / b.vector" + (simd && i && size == 3 ? $".WithElement(3, {typ.one})" : ""), f)};",
            // the widened value of a 64 bit integer vector has zero padding lanes, the division by them would
            // throw, a floating point vector only produces a nan there and drops it again
            i ? null : $"return {FromVector("Vector128.GetLower(a.vector.ToVector128() / b.vector.ToVector128())")};",
            NewCompWise(n => $"a.{comp[n]} / b.{comp[n]}"));
        // the integer remainder is derived from the division, so it goes through the guarded division operator
        if (f)
            EmitBinOp("%", $"return {FromVector("simd.Rem(a.vector, b.vector)", true)};",
                $"return {FromVector("Vector128.GetLower(simd.Rem(a.vector.ToVector128(), b.vector.ToVector128()))")};",
                NewCompWise(n => $"a.{comp[n]} % b.{comp[n]}"));
        else
            EmitBinOp("%", "return a - (a / b) * b;", null,
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
        EmitAccel($"return {FromVector($"{vecName}.Abs(vector)")};",
            $"return {FromVector("Vector128.GetLower(Vector128.Abs(vector.ToVector128()))")};",
            $"return {NewCompWise(n => $"{comp[n]}.abs()")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // unsigned components can not be negative, their sign is a mask of zero or one
        var signOp = f ? "SignFloat" : sig ? "SignInt" : "SignUInt";
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} sign()");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"simd.{signOp}(vector)")};",
            $"return {FromVector($"Vector128.GetLower(simd.{signOp}(vector.ToVector128()))")};",
            $"return {NewCompWise(n => $"{comp[n]}.sign()")};");
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
        EmitAccel($"return {FromVector($"{vecName}.Min(vector, other.vector)")};",
            $"return {FromVector("Vector128.GetLower(Vector128.Min(vector.ToVector128(), other.vector.ToVector128()))")};",
            $"return {NewCompWise(n => $"{comp[n]}.min(other.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} max(in {type} other)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"{vecName}.Max(vector, other.vector)")};",
            $"return {FromVector("Vector128.GetLower(Vector128.Max(vector.ToVector128(), other.vector.ToVector128()))")};",
            $"return {NewCompWise(n => $"{comp[n]}.max(other.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} clamp(in {type} min, in {type} max)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"{vecName}.Max(min.vector, {vecName}.Min(max.vector, vector))")};",
            $"return {FromVector($"Vector128.GetLower(Vector128.Max(min.vector.ToVector128(), Vector128.Min(max.vector.ToVector128(), vector.ToVector128())))")};",
            $"return {NewCompWise(n => $"{comp[n]}.clamp(min.{comp[n]}, max.{comp[n]})")};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the scalar bounds are broadcast into the operands, the vector clamp is not called with two broadcast
        // vectors, the scalar expression clamps every component on its own
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} clamp({scalar} min, {scalar} max)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"{vecName}.Max({vecName}.Create(min), {vecName}.Min({vecName}.Create(max), vector))")};",
            $"return {FromVector($"Vector128.GetLower(Vector128.Max(Vector128.Create(min), Vector128.Min(Vector128.Create(max), vector.ToVector128())))")};",
            $"return {NewCompWise(n => $"{comp[n]}.clamp(min, max)")};");
        sb.AppendLine("    }");
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
        EmitAccel(f
                ? $"return {FromVector($"{vecName}.Lerp(start.vector, end.vector, vector)")};"
                : "return fma(this, end - start, start);",
            f
                ? $"return {FromVector("Vector128.GetLower(Vector128.Lerp(start.vector.ToVector128(), end.vector.ToVector128(), vector.ToVector128()))")};"
                : $"return {FromVector("Vector128.GetLower(vector.ToVector128() * (end.vector.ToVector128() - start.vector.ToVector128()) + start.vector.ToVector128())")};",
            "return start + this * (end - start);");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} lerp({scalar} t, in {type} start, in {type} end)");
        sb.AppendLine("    {");
        EmitAccel($"return fma(new {type}(t), end - start, start);",
            f
                ? $"return {FromVector($"Vector128.GetLower(simd.Fma(Vector128.Create({cast}t), end.vector.ToVector128() - start.vector.ToVector128(), start.vector.ToVector128()))")};"
                : $"return {FromVector($"Vector128.GetLower(Vector128.Create({cast}t) * (end.vector.ToVector128() - start.vector.ToVector128()) + start.vector.ToVector128())")};",
            $"return start + new {type}(t) * (end - start);");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type} lerp({scalar} start, {scalar} end)");
        sb.AppendLine("    {");
        EmitAccel($"return fma(this, {cast}(end - start), new {type}(start));",
            f
                ? $"return {FromVector($"Vector128.GetLower(simd.Fma(vector.ToVector128(), Vector128.Create({cast}(end - start)), Vector128.Create({cast}start)))")};"
                : $"return {FromVector($"Vector128.GetLower(vector.ToVector128() * Vector128.Create({cast}(end - start)) + Vector128.Create({cast}start))")};",
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
        // the sum of the products is the component sum of the element wise product, the scalar sum and its
        // acceleration are the ones of csum
        EmitAccel($"return {vecName}.Dot(vector, other.vector);", null,
            "return (this * other).csum();");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {scalar} length_sq()");
        sb.AppendLine("    {");
        // dot with itself
        EmitAccel($"return {vecName}.Dot(vector, vector);", null,
            "return (this * this).csum();");
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
        void EmitFma(string name, string accelerated, string? wide, string scalarName)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} {name}(in {type} a, in {type} b, in {type} c)");
            sb.AppendLine("    {");
            EmitAccel(accelerated, wide, $"return {NewCompWise(n => $"a.{comp[n]}.{scalarName}(b.{comp[n]}, c.{comp[n]})")};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitFma("fma",
            f ? $"return {FromVector("simd.Fma(a.vector, b.vector, c.vector)")};" : "return (a * b) + c;",
            f ? $"return {FromVector("Vector128.GetLower(simd.Fma(a.vector.ToVector128(), b.vector.ToVector128(), c.vector.ToVector128()))")};" : null,
            "fma");
        EmitFma("fms",
            f ? $"return {FromVector("simd.Fms(a.vector, b.vector, c.vector)")};" : "return (a * b) - c;",
            f ? $"return {FromVector("Vector128.GetLower(simd.Fms(a.vector.ToVector128(), b.vector.ToVector128(), c.vector.ToVector128()))")};" : null,
            "fms");
        EmitFma("fnma",
            f ? $"return {FromVector("simd.Fnma(a.vector, b.vector, c.vector)")};" : "return c - (a * b);",
            f ? $"return {FromVector("Vector128.GetLower(simd.Fnma(a.vector.ToVector128(), b.vector.ToVector128(), c.vector.ToVector128()))")};" : null,
            "fnma");

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
        EmitAccel(scalarOnly ? null : $"return {vecName}.Sum(vector);", null,
            $"return {cast}({Join(n => comp[n], " + ")});");
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
            // a floating point 2 component vector reduces with a single scalar operation, its components always
            // live in a simd register, so it has no accelerated path at all
            if (simd && (!safeOnly || i) && !scalarOnly)
            {
                var op = size == 3 ? vecOp + "3" : vecOp;
                // the reduction of a 2 component integer vector is a couple of scalar operations, the wider types
                // are not worth checking for it, a wider vector would have to widen the value first
                var gate = bitSize == 64
                    ? $"{vecName}.IsHardwareAccelerated"
                    : $"Vector{bitSize4}.IsHardwareAccelerated || Vector{bitSize2}.IsHardwareAccelerated";
                sb.AppendLine($"        if ({gate})");
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
