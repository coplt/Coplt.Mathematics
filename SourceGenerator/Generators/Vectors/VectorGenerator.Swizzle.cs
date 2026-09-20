using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the swizzle members of the vector described by <paramref name="typ"/>. Every combination of the
    /// components is a member, the digits of the name are the indices of the components so <c>01</c> is <c>xy</c>,
    /// and both spellings of a combination, <c>xyzw</c> and <c>rgba</c>, are declared on the same member. The
    /// members implement the swizzle interfaces of <c>Coplt.Mathematics.Generics.Swizzle</c>. They are emitted
    /// into their own file, so the base members and the arithmetic members stay separate.
    /// </summary>
    private static string GenSwizzle(Typ typ, int size)
    {
        var type = $"{typ.name}{size}";
        var simd = typ.simd;
        var cast = typ.shuffleCast;
        var attr = "[MethodImpl(256)]";
        var comp = VectorGenShared.Components(size);
        // the register of the vector itself, a 3 component vector is padded to 4 lanes
        var srcReg = 8 * typ.size * (size == 2 ? 2 : 4);
        var srcVecName = $"Vector{srcReg}";
        var srcLanes = srcReg / (8 * typ.size);

        // the type parameters of the interfaces, the same sized type is the first one, a setter exists for a
        // combination that is not longer than the vector because its indices have to be distinct
        var getArgs = new List<string> { type };
        var setArgs = new List<string> { type };
        for (var d = 2; d <= 4; d++)
        {
            if (d == size) continue;
            getArgs.Add($"{typ.name}{d}");
            if (d < size) setArgs.Add($"{typ.name}{d}");
        }

        var sb = new StringBuilder();

        // the members inherit their documentation from the interface they implement
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        // the digits of every combination of the components, the last digit is the least significant one
        List<int[]> Combinations(int count)
        {
            var result = new List<int[]>();
            var total = 1;
            for (var i = 0; i < count; i++) total *= size;
            for (var combination = 0; combination < total; combination++)
            {
                var digits = new int[count];
                var rest = combination;
                for (var i = count - 1; i >= 0; i--)
                {
                    digits[i] = rest % size;
                    rest /= size;
                }

                result.Add(digits);
            }

            return result;
        }

        // the arguments of a Vector.Create call, the digits of a uint or a ulong vector need a cast
        string Args(int[] values, bool castDigits = true)
        {
            var b = new StringBuilder();
            for (var i = 0; i < values.Length; i++)
            {
                if (i != 0) b.Append(", ");
                if (castDigits) b.Append(cast);
                b.Append(values[i]);
            }

            return b.ToString();
        }

        VectorGenShared.FileHeader(sb, false, true);
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// The swizzle members of {type}, they implement the swizzle interfaces of the vector");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    IVectorGetSwizzleForVec{size}<{string.Join(", ", getArgs)}>,");
        sb.AppendLine($"    IVectorSetSwizzleForVec{size}<{string.Join(", ", setArgs)}>");
        sb.AppendLine("{");

        for (var dst = 2; dst <= 4; dst++)
        {
            var ret = $"{typ.name}{dst}";
            var dstReg = 8 * typ.size * (dst == 2 ? 2 : 4);
            var reg = Math.Max(srcReg, dstReg);
            var vecName = $"Vector{reg}";
            var lanes = reg / (8 * typ.size);
            // the value of the vector has to be created in a wider register because the register has more lanes
            var widen = reg > srcReg;
            // the result is taken from the lower lanes of the register
            var narrow = reg > dstReg;
            // the padding lane of the result can hold something else than zero only when the shuffle reads it
            // from the fourth lane of a 4 component vector, the lanes that a shorter vector does not have are
            // created as zeros and the padding lane of a 3 component vector is one
            var masked = simd && dst == 3 && size == 4;
            // a 64 bit vector has no hardware support on every platform, the shuffle can widen it to 128 bits
            var wide = simd && reg == srcReg && srcReg == 64;

            string FromVector(string expr) => VectorGenShared.Vector(simd, dst, expr, masked);

            sb.AppendLine();
            sb.AppendLine($"    #region {dst} components");
            sb.AppendLine();

            foreach (var digits in Combinations(dst))
            {
                var name = VectorGenShared.Join(dst, i => Typ.xyzw[digits[i]], "");
                var colorName = VectorGenShared.Join(dst, i => Typ.rgba[digits[i]], "");
                var ordered = true;
                var distinct = true;
                for (var i = 0; i < dst; i++)
                {
                    if (digits[i] != i) ordered = false;
                    for (var j = i + 1; j < dst; j++)
                    {
                        if (digits[i] == digits[j]) distinct = false;
                    }
                }

                // a combination with a repeated index cannot be written, so it is a readonly member
                var writable = distinct;
                // only the whole combination is the vector itself, a shorter combination is its beginning
                var identity = ordered && dst == size;

                // the index of every component inside the combination, and the lanes the combination reads
                var idx = new int[lanes];
                for (var i = 0; i < dst; i++) idx[i] = digits[i];
                for (var i = dst; i < lanes; i++) idx[i] = dst == 3 ? 3 : 0;
                // the index the value of a setter is read from for every lane of the vector, and the lanes the
                // combination writes, a lane the combination does not write keeps the value of the vector
                var inv = new int[srcLanes];
                var sel = new int[srcLanes];
                for (var i = 0; i < srcLanes; i++) sel[i] = -1;
                for (var i = 0; i < dst; i++)
                {
                    inv[digits[i]] = i;
                    sel[digits[i]] = 0;
                }

                var fallback = $"new({VectorGenShared.Join(dst, i => comp[digits[i]])})";
                // the value of a shorter vector is widened with the trailing lanes set to zero, so the shuffle
                // can read them and the padding lane of a 3 component result stays zero
                var shuffle = $"{vecName}.Shuffle({(widen ? $"vector.ToVector{reg}()" : "vector")}, " +
                              $"{vecName}.Create({Args(idx)}))";
                var expr = narrow ? $"{shuffle}.GetLower()" : shuffle;
                // the inverse of the combination, it permutes the value in the same way as the combination
                // permutes the vector, so a setter can assign the permuted value to the whole vector
                var invName = VectorGenShared.Join(size, i => Typ.xyzw[inv[i]], "");

                InheritDoc();
                sb.AppendLine(writable ? $"    public {ret} {name}" : $"    public readonly {ret} {name}");
                sb.AppendLine("    {");
                sb.AppendLine($"        {attr}");
                // a readonly property cannot mark its accessor readonly as well
                var acc = writable ? "readonly get" : "get";
                if (identity)
                {
                    sb.AppendLine($"        {acc} => this;");
                }
                else if (simd)
                {
                    sb.AppendLine($"        {acc}");
                    sb.AppendLine("        {");
                    sb.AppendLine($"            if ({vecName}.IsHardwareAccelerated)");
                    sb.AppendLine($"                return {FromVector(expr)};");
                    if (wide)
                    {
                        // a 64 bit vector is widened to a 128 bit one by the shuffle, it reads the two
                        // components from the lower lanes of the wider register
                        var wideIdx = new[] { idx[0], idx[1], 0, 0 };
                        sb.AppendLine("            if (Vector128.IsHardwareAccelerated)");
                        sb.AppendLine($"                return {FromVector("Vector128.GetLower(" +
                                                                           $"Vector128.Shuffle(vector.ToVector128(), Vector128.Create({Args(wideIdx)})))")};");
                    }

                    sb.AppendLine($"            return {fallback};");
                    sb.AppendLine("        }");
                }
                else
                {
                    sb.AppendLine($"        {acc} => {fallback};");
                }

                if (writable)
                {
                    sb.AppendLine($"        {attr}");
                    if (identity)
                    {
                        sb.AppendLine("        set => this = value;");
                    }
                    else if (dst == size)
                    {
                        sb.AppendLine($"        set => this = value.{invName};");
                    }
                    else
                    {
                        sb.AppendLine("        set");
                        sb.AppendLine("        {");
                        if (simd)
                        {
                            // the vector and the value are mixed by the mask, the lanes the combination does
                            // not write keep their component and the value is read through the inverse of it
                            var setValue = $"value.vector{(dstReg == srcReg ? "" : $".ToVector{srcReg}()")}";
                            sb.AppendLine($"            if ({srcVecName}.IsHardwareAccelerated)");
                            sb.AppendLine($"                vector = {srcVecName}.ConditionalSelect(" +
                                          $"{srcVecName}.Create({Args(sel, false)}).{AsMethod(typ.simdComp)}(), " +
                                          $"vector, {srcVecName}.Shuffle({setValue}, " +
                                          $"{srcVecName}.Create({Args(inv)})));");
                            sb.AppendLine("            else");
                            sb.AppendLine("            {");
                            for (var i = 0; i < dst; i++)
                            {
                                sb.AppendLine($"                {comp[digits[i]]} = value.{comp[i]};");
                            }

                            sb.AppendLine("            }");
                        }
                        else
                        {
                            for (var i = 0; i < dst; i++)
                            {
                                sb.AppendLine($"            {comp[digits[i]]} = value.{comp[i]};");
                            }
                        }

                        sb.AppendLine("        }");
                    }
                }

                sb.AppendLine("    }");
                sb.AppendLine();

                // the two spellings of a combination are the same member, the color one is a forwarder
                InheritDoc();
                sb.AppendLine(writable
                    ? $"    public {ret} {colorName}"
                    : $"    public readonly {ret} {colorName}");
                sb.AppendLine("    {");
                sb.AppendLine($"        {attr}");
                sb.AppendLine(writable ? $"        readonly get => {name};" : $"        get => {name};");
                if (writable)
                {
                    sb.AppendLine($"        {attr}");
                    sb.AppendLine($"        set => {name} = value;");
                }

                sb.AppendLine("    }");
                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
