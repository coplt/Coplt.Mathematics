using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the swizzle members of the vector described by <paramref name="typ"/>. Every combination of the
    /// components is a member, the digits of the name are the indices of the components so <c>01</c> is <c>xy</c>,
    /// and both spellings of a combination, <c>xyzw</c> and <c>rgba</c>, are declared on the same member. They
    /// are emitted into their own file, so the base members and the arithmetic members stay separate.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    private static string GenSwizzle(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        // the value of a 64 bit vector is kept in a raw ulong field, the other simd vectors keep the register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var cast = typ.shuffleCast;
        var attr = "[MethodImpl(256)]";
        var comp = VectorGenShared.Components(size);
        // the register of the value of the vector, a 2 or 3 component vector has padding lanes in it when its
        // register is wider than the vector itself
        var srcReg = VectorGenShared.Register(typ, size, storeVariant);
        var srcVecName = $"Vector{srcReg}";
        var srcLanes = srcReg == 0 ? size : srcReg / (8 * typ.size);
        // a 64 bit vector is widened to 128 bits with zero upper lanes, a wider register keeps its own padding
        // lanes at zero, so the padding lanes of a result read a lane that is known to be zero
        var srcPad = v64 || srcLanes > size;
        var padIdx = v64 ? 2 : srcLanes - 1;

        var sb = new StringBuilder();

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

        // the documentation of the type is carried by the declaration of the base members, only one of the
        // partial declarations of a type may have it
        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type}");
        sb.AppendLine("{");

        for (var dst = 2; dst <= 4; dst++)
        {
            // the combination with the same size as the vector is the vector itself, the ones of another size
            // are vectors without the storage variant
            var sameSize = dst == size;
            var ret = sameSize ? type : VectorGenShared.VecName(typ, dst, false);
            var dstReg = VectorGenShared.Register(typ, dst, sameSize && storeVariant);
            var dst64 = VectorGenShared.Uses64(typ, dst, sameSize && storeVariant);
            var dstPad = VectorGenShared.PadLanes(typ, dst, sameSize && storeVariant) > 0;
            var reg = Math.Max(srcReg, dstReg);
            // a vector without a register still needs the lanes of its padded value for the arrays of the indices,
            // the shuffle itself is not emitted for it
            if (reg == 0) reg = 8 * typ.size * (Math.Max(size, dst) == 2 ? 2 : 4);
            var vecName = $"Vector{reg}";
            var lanes = reg / (8 * typ.size);
            // the value of the vector has to be created in a wider register because it has more lanes
            var widen = reg > srcReg;
            // the result is taken from the lower lanes of the register
            var narrow = reg > dstReg;
            // the padding lanes of a result hold something else than zero only when the shuffle reads them from
            // a lane that is a component of the source, the padding lanes of the source are zero
            var masked = simd && dstPad && !srcPad;
            // a 64 bit vector has no hardware support on every platform, the shuffle can widen it to 128 bits
            var wide = simd && reg == srcReg && srcReg == 64;

            // the construction of a result, see VectorGenShared.Vector
            string FromVector(string expr) => VectorGenShared.Vector(simd, dstPad, expr, masked);

            // the storage variant of a 2 component vector keeps the lower 64 bits of the register
            string FromRegister(string expr) =>
                dst64 && reg > 64 ? VectorGenShared.From128(expr) : FromVector(expr);

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
                // the padding lanes of the result read a lane that is known to be zero
                for (var i = dst; i < lanes; i++) idx[i] = padIdx;
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
                // the value of the vector is widened to the register of the shuffle, a 64 bit value is loaded
                // from the field of the vector type and its upper lanes are zero
                var widenExpr = srcReg == 64 ? VectorGenShared.Load64("", typ.simdComp) : $"vector.ToVector{reg}()";
                var shuffle = $"{vecName}.Shuffle({(widen ? widenExpr : "vector")}, " +
                              $"{vecName}.Create({Args(idx)}))";
                // the lower lanes of the register are the value of a shorter vector, the value of a 64 bit
                // vector is already the lower half of the wider register
                var expr = narrow && dstReg > 64 ? $"{shuffle}.GetLower()" : shuffle;
                // the inverse of the combination, it permutes the value in the same way as the combination
                // permutes the vector, so a setter can assign the permuted value to the whole vector
                var invName = VectorGenShared.Join(size, i => Typ.xyzw[inv[i]], "");

                sb.AppendLine($"    /// <summary>The <c>{name}</c> swizzle of the vector, it is of the type <see cref=\"{ret}\"/></summary>");
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
                    sb.AppendLine($"                return {FromRegister(expr)};");
                    if (wide)
                    {
                        // a 64 bit vector is widened to a 128 bit one by the shuffle, it reads the two
                        // components from the lower lanes of the wider register
                        var wideIdx = new[] { idx[0], idx[1], 0, 0 };
                        sb.AppendLine("            if (Vector128.IsHardwareAccelerated)");
                        sb.AppendLine($"                return {FromRegister(
                            $"Vector128.GetLower(Vector128.Shuffle({VectorGenShared.Load64("", typ.simdComp)}, " +
                            $"Vector128.Create({Args(wideIdx)})))")};");
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
                            // the value is read in the register of the vector, the value of a 64 bit vector is
                            // loaded from its field instead of ToVector and a wider one is narrowed
                            var setValue = dstReg == srcReg
                                ? "value.vector"
                                : dst64
                                    ? VectorGenShared.Load64("value.", typ.simdComp)
                                    : srcReg == 64
                                        ? "value.vector.GetLower()"
                                        : $"value.vector.ToVector{srcReg}()";
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

                // the rgba spelling of a combination is a spelling of the member itself
                sb.AppendLine($"    /// <summary>The <c>{colorName}</c> swizzle of the vector, it is the same as <see cref=\"{name}\"/></summary>");
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

                sb.AppendLine();
            }

            sb.AppendLine("    #endregion");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
