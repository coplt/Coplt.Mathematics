using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// The type of every vector by its name, the conversions of <see cref="Typ.ExplicitConverts"/> and
    /// <see cref="Typ.ImplicitConverts"/> name their target with it.
    /// </summary>
    private static readonly Dictionary<string, Typ> ConvTypes = BuildConvTypes();

    private static Dictionary<string, Typ> BuildConvTypes()
    {
        var map = new Dictionary<string, Typ>();
        foreach (var typ in Typ.Typs) map[typ.name] = typ;
        return map;
    }

    /// <summary>
    /// Generates the conversions of the vector described by <paramref name="typ"/> into the vectors that have the
    /// same number of components and another component type: the target of every conversion of
    /// <see cref="Typ.ExplicitConverts"/> and <see cref="Typ.ImplicitConverts"/>. A conversion is an operator of
    /// the vector, the operators are emitted into their own file and a type without a conversion has no file at
    /// all. The storage variant of a vector only converts into the storage variants of the same size, so a
    /// conversion that only exists beside one of them is kept on the regular vector.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The conversions of the vector or null when it has none</returns>
    private static string? GenConv(Typ typ, int size, bool storeVariant)
    {
        var targets = new List<(string Kind, Typ Target)>();
        if (Typ.ExplicitConverts.TryGetValue(typ.name, out var explicitTargets))
        {
            foreach (var name in explicitTargets)
            {
                if (ConvTypes.TryGetValue(name, out var target)) targets.Add(("explicit", target));
            }
        }

        if (Typ.ImplicitConverts.TryGetValue(typ.name, out var implicitTargets))
        {
            foreach (var name in implicitTargets)
            {
                if (ConvTypes.TryGetValue(name, out var target)) targets.Add(("implicit", target));
            }
        }

        targets.RemoveAll(a => storeVariant && !VectorGenShared.HasStorageVariant(a.Target, size));
        if (targets.Count == 0) return null;

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var comp = VectorGenShared.Components(size);
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var width = 8 * typ.size;

        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type}");
        sb.AppendLine("{");

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

        sb.AppendLine("}");
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
