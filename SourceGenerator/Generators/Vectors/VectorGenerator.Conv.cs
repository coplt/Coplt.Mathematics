using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the conversions of the vector described by <paramref name="typ"/> into the vectors that have the
    /// same number of components and another component type: the target of every conversion of
    /// <see cref="Typ.ExplicitConverts"/> and <see cref="Typ.ImplicitConverts"/>, and the storage variant of the
    /// vector itself. A conversion is an operator of the vector, the operators are emitted into their own file and
    /// a type without a conversion has no file at all. The storage variant of a vector only converts into the
    /// storage variants of the same size, so a conversion that only exists beside one of them is kept on the
    /// regular vector.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The conversions of the vector or null when it has none</returns>
    private static string? GenConv(Typ typ, int size, bool storeVariant)
    {
        var targets = VectorGenShared.ConvTargets(typ);

        targets.RemoveAll(a => storeVariant && !VectorGenShared.HasStorageVariant(a.Target, size));

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        // the conversion between the vector and its storage variant is emitted beside the numeric ones, the two of
        // them are implicit: a storage variant keeps the same components in another storage, so neither of the two
        // conversions drops a component
        var store = !storeVariant && VectorGenShared.HasStorageVariant(typ, size)
            ? VectorGenShared.VecName(typ, size, true)
            : null;
        var regular = storeVariant ? VectorGenShared.VecName(typ, size, false) : null;
        if (targets.Count == 0 && store == null && regular == null) return null;

        var comp = VectorGenShared.Components(size);
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var width = 8 * typ.size;

        var sb = new StringBuilder();

        foreach (var (kind, target) in targets)
        {
            var targetType = VectorGenShared.VecName(target, size, storeVariant);
            var targetSimd = VectorGenShared.Simd(target, size, storeVariant);
            var targetReg = VectorGenShared.Register(target, size, storeVariant);
            var targetPad = VectorGenShared.PadLanes(target, size, storeVariant) > 0;
            var targetWidth = 8 * target.size;
            // a component of a float vector is converted, a component that keeps its kind only reinterprets the
            // bits of its value, the two of them are the same when the width of the component does not change
            var convert = typ.f != target.f;

            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {kind} operator {targetType}({type} self)");
            sb.AppendLine("    {");

            // a 64 bit register is not accelerated on every platform, the conversions of it are component wise
            if (simd && targetSimd && reg >= 128 && targetReg >= 128)
            {
                // the value of a wider or a narrower component is built in two steps, the lanes of a component
                // that keeps its width are converted by a single call
                var setup = new List<string>();
                string expr;
                if (width == targetWidth)
                {
                    expr = convert
                        ? $"Vector{reg}.{ConvertMethod(target.simdComp)}(self.vector)"
                        : $"self.vector.{AsMethod(target.simdComp)}()";
                }
                else if (width < targetWidth)
                {
                    // the widened lanes of a value are the lower half of the result, a target that is twice as
                    // wide takes both halves
                    setup.Add($"var (a, b) = Vector{reg}.Widen(self.vector);");
                    var widenedReg = targetReg > reg ? targetReg : reg;
                    var widened = targetReg > reg ? $"Vector{targetReg}.Create(a, b)" : "a";
                    expr = convert
                        ? $"Vector{widenedReg}.{ConvertMethod(target.simdComp)}({widened})"
                        : $"{widened}.{AsMethod(target.simdComp)}()";
                }
                else
                {
                    // the register of a vector is never wider than two times the one of the target, the two
                    // halves of a 64 bit register feed the padding lanes of the target
                    var halves = reg > targetReg
                        ? "self.vector.GetLower(), self.vector.GetUpper()"
                        : "self.vector, self.vector";
                    setup.Add($"var v = Vector{targetReg}.Narrow({halves});");
                    expr = convert
                        ? $"Vector{targetReg}.{ConvertMethod(target.simdComp)}(v)"
                        : $"v.{AsMethod(target.simdComp)}()";
                }

                sb.AppendLine($"        if (Vector{reg}.IsHardwareAccelerated)");
                sb.AppendLine("        {");
                foreach (var line in setup) sb.AppendLine($"            {line}");
                sb.AppendLine($"            return {VectorGenShared.Vector(targetSimd, targetPad, expr, targetPad)};");
                sb.AppendLine("        }");
            }

            sb.AppendLine($"        return new({VectorGenShared.Join(size, i => $"({target.compType})self.{comp[i]}")});");
            sb.AppendLine("    }");
        }

        if (store != null)
        {
            sb.AppendLine();
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static implicit operator {store}({type} self) => new(self);");
        }

        if (regular != null)
        {
            // the value of the storage variant is widened by the constructor of the regular vector, which zeroes
            // the padding lanes of its register
            var toRegular = VectorGenShared.Uses64(typ, size, storeVariant)
                ? $"new() {{ vector = {VectorGenShared.Load64("self.", typ.simdComp)} }}"
                : $"new({VectorGenShared.Join(size, i => $"self.{comp[i]}")})";
            sb.AppendLine();
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static implicit operator {regular}({type} self) => {toRegular};");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns the name of the helper of the simd types that converts a vector of another component type into
    /// <paramref name="simdComp"/>.
    /// </summary>
    /// <param name="simdComp">The type of a component of the target vector</param>
    /// <returns>The name of the helper</returns>
    private static string ConvertMethod(string simdComp) => simdComp switch
    {
        "float" => "ConvertToSingle",
        "double" => "ConvertToDouble",
        "int" => "ConvertToInt32",
        "uint" => "ConvertToUInt32",
        "long" => "ConvertToInt64",
        "ulong" => "ConvertToUInt64",
        _ => throw new InvalidOperationException($"unknown simd comp {simdComp}"),
    };
}
