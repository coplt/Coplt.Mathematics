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
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    private static string GenArith(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        // the value of a 64 bit vector is kept in a raw ulong field, the other simd vectors keep the register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var lanes = VectorGenShared.Lanes(typ, size, storeVariant);
        // the register of a 2 or 3 component vector is wider than the vector, the extra lanes are padding
        var pad = VectorGenShared.PadLanes(typ, size, storeVariant) > 0;
        // the bit size of the register of a 2 component vector and of a padded 4 component one, the simd
        // reductions pick their implementation by them
        var bitSize2 = 8 * typ.size * 2;
        var bitSize4 = 8 * typ.size * 4;
        var sig = typ.sig;
        var f = typ.f;
        var i = typ.i;
        var vecName = $"Vector{reg}";
        var cast = typ.arithCast;
        var attr = "[MethodImpl(256)]";

        var comp = VectorGenShared.Components(size);

        // a signed vector also has the negation operator, a 3 component vector also has the cross product
        var ifaces = VectorGenShared.ArithInterfaces(typ, size, storeVariant);

        var sb = new StringBuilder();

        // the members inherit their documentation from the interface they implement
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        string Join(Func<int, string> get, string sep = ", ") => VectorGenShared.Join(size, get, sep);

        // the component wise construction of the result, every component is cast back to the scalar type
        string NewCompWise(Func<int, string> get) => $"new({Join(n => $"({scalar})({get(n)})")})";

        // the 128 bit value of a 64 bit vector and the construction of a 64 bit vector from a 128 bit one
        string Load64(string self) => VectorGenShared.Load64(self, typ.simdComp);
        string From128(string expr) => VectorGenShared.From128(expr);

        // the construction of a simd result, see VectorGenShared.Vector. Only the division and the remainder of a
        // 3 component floating point vector need the mask, they divide the zero padding lane by zero
        string FromVector(string expr, bool masked = false) => VectorGenShared.Vector(simd, pad, expr, masked);

        // emits the accelerated simd fast path, the scalar expression is used when nothing is accelerated,
        // every statement includes the return. The vector type is fixed, a value that is kept in a 64 bit
        // register can only reach 128 bit vectors by widening itself, that is the path for a platform that has
        // no 64 bit hardware support. The accelerated expression is null when the scalar expression is already
        // the best the member can do. The wide expression is null when a wider path does not pay off: the member
        // is built from the operators of the vector, which carry the 128 bit path themselves, or the scalar
        // expression is already a single operation.
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

        // the documentation of the type is carried by the declaration of the base members, only one of the
        // partial declarations of a type may have it
        VectorGenShared.FileHeader(sb, true);
        sb.AppendLine($"public partial struct {type} :");
        for (var n = 0; n < ifaces.Count; n++)
        {
            sb.AppendLine($"    {ifaces[n].Name}<{string.Join(", ", ifaces[n].Args)}>" + (n == ifaces.Count - 1 ? "" : ","));
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
                $"return {From128($"-{Load64("a.")}")};",
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
            $"return {From128($"{Load64("a.")} + {Load64("b.")}")};",
            NewCompWise(n => $"a.{comp[n]} + b.{comp[n]}"));
        EmitBinOp("-", $"return {FromVector("a.vector - b.vector")};",
            $"return {From128($"{Load64("a.")} - {Load64("b.")}")};",
            NewCompWise(n => $"a.{comp[n]} - b.{comp[n]}"));
        EmitBinOp("*", $"return {FromVector("a.vector * b.vector")};",
            $"return {From128($"{Load64("a.")} * {Load64("b.")}")};",
            NewCompWise(n => $"a.{comp[n]} * b.{comp[n]}"));
        // the padding lanes of the register are zero, an integer division by them would throw, so the
        // denominator is patched to one there
        var padOne = "";
        if (simd && i && pad)
        {
            for (var n = size; n < lanes; n++) padOne += $".WithElement({n}, {typ.one})";
        }

        // the padding lanes of a 3 component integer vector are zero, dividing by them would throw
        EmitBinOp("/",
            $"return {FromVector("a.vector / b.vector" + padOne, f)};",
            // the widened value of a 64 bit integer vector has zero padding lanes, the division by them would
            // throw, a floating point vector only produces a nan there and drops it again
            i ? null : $"return {From128($"{Load64("a.")} / {Load64("b.")}")};",
            NewCompWise(n => $"a.{comp[n]} / b.{comp[n]}"));
        // the integer remainder is derived from the division, so it goes through the guarded division operator
        if (f)
            EmitBinOp("%", $"return {FromVector("simd.Rem(a.vector, b.vector)", true)};",
                $"return {From128($"simd.Rem({Load64("a.")}, {Load64("b.")})")};",
                NewCompWise(n => $"a.{comp[n]} % b.{comp[n]}"));
        else
            EmitBinOp("%", "return a - (a / b) * b;", null,
                NewCompWise(n => $"a.{comp[n]} % b.{comp[n]}"));

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region clamp lerp unlerp remap

        sb.AppendLine("    #region clamp lerp unlerp remap");
        sb.AppendLine();

        // the bounds of this member are single components, the dispatch of an algebra does not implement it yet
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} clamp({scalar} min, {scalar} max)");
        sb.AppendLine("    {");
        EmitAccel($"return {FromVector($"{vecName}.Max({vecName}.Create(min), {vecName}.Min({vecName}.Create(max), vector))")};",
            $"return {From128($"Vector128.Max(Vector128.Create(min), Vector128.Min(Vector128.Create(max), {Load64("")}))")};",
            $"return {NewCompWise(n => VectorScalar.Expr(scalar, "clamp", comp[n], "min", "max"))};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the factor of this member is a single component, the dispatch of an algebra does not implement it yet
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} lerp({scalar} t, in {type} start, in {type} end)");
        sb.AppendLine("    {");
        EmitAccel($"return math.fma(new {type}(t), end - start, start);",
            f
                ? $"return {From128($"simd.Fma(Vector128.Create({cast}t), {Load64("end.")} - {Load64("start.")}, {Load64("start.")})")};"
                : $"return {From128($"Vector128.Create({cast}t) * ({Load64("end.")} - {Load64("start.")}) + {Load64("start.")}")};",
            $"return start + new {type}(t) * (end - start);");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the bounds of this member are single components, the dispatch of an algebra does not implement it yet
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} unlerp({scalar} start, {scalar} end) => (this - start) / new {type}({cast}(end - start));");
        sb.AppendLine();

        // the bounds of this member are single components, the dispatch of an algebra does not implement it yet
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {type} remap({scalar} src_start, {scalar} src_end, {scalar} dst_start, {scalar} dst_end) =>");
        sb.AppendLine("        unlerp(src_start, src_end).lerp(dst_start, dst_end);");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region length_sq distance_sq

        sb.AppendLine("    #region length_sq distance_sq");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {scalar} length_sq()");
        sb.AppendLine("    {");
        // dot with itself, the 128 bit path reads the value of the vector twice
        EmitAccel($"return {vecName}.Dot(vector, vector);",
            v64 ? $"return Vector128.Dot({Load64("")}, {Load64("")});" : null,
            "return (this * this).csum();");
        sb.AppendLine("    }");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {scalar} distance_sq(in {type} to) => (to - this).length_sq();");
        sb.AppendLine();

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        #endregion

        #region csum cmin cmax

        sb.AppendLine("    #region csum cmin cmax");
        sb.AppendLine();

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly {scalar} csum()");
        sb.AppendLine("    {");
        // a 64 bit vector sums the two lanes of its own register and falls back to the 128 bit one, the
        // padding lanes of the widened value are zero, so they do not change the sum
        EmitAccel($"return {vecName}.Sum(vector);",
            v64 ? $"return Vector128.Sum({Load64("")});" : null,
            $"return {cast}({Join(n => comp[n], " + ")});");
        sb.AppendLine("    }");
        sb.AppendLine();

        // emits one of the component reductions, when the vector has a padding lane the reduction has to
        // ignore it, the safe variants only use the accelerated path when the padding lane is not a number
        void EmitReduce(string name, string scalarName, string vecOp, bool safeOnly)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public readonly {scalar} {name}()");
            sb.AppendLine("    {");
            // the safe variants of a floating point vector only take the accelerated path when the padding
            // lanes of the value cannot disturb the reduction
            if (simd && (!safeOnly || i))
            {
                var op = size == 3 ? vecOp + "3" : vecOp;
                // the helper of a 64 bit vector reduces the two lanes of its own register, it reads them from
                // a 64 bit register and from the 128 bit one when the platform has no 64 bit hardware support,
                // so the gate accepts either register, the wider types reduce the register of the value itself
                var gate = v64
                    ? $"{vecName}.IsHardwareAccelerated || Vector128.IsHardwareAccelerated"
                    : $"Vector{bitSize4}.IsHardwareAccelerated || Vector{bitSize2}.IsHardwareAccelerated";
                // a 2 component vector whose register is widened to 128 bits keeps its padding lanes at zero,
                // the helper of the 64 bit register reduces the two lanes of it without them
                var operand = pad && size == 2 ? "vector.GetLower()" : "vector";
                sb.AppendLine($"        if ({gate})");
                sb.AppendLine($"            return simd.{op}({operand});");
            }

            var chain = comp[0];
            for (var n = 1; n < size; n++) chain = VectorScalar.Expr(scalar, scalarName, chain, comp[n]);
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
        sb.AppendLine("}");
        sb.AppendLine();

        return VectorDocs.Apply(sb.ToString());
    }
}
