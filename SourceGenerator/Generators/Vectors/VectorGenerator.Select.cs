using System;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the select members of the vector described by <paramref name="typ"/>, they implement
    /// <c>IVectorSelect</c>: a mask selects the components of two vectors that have its own shape. The member
    /// that is called on a value and the static member that implements the interface are emitted into the same
    /// file because they are the two forms of the same operation, every vector has a mask and can be selected
    /// whether its components are numbers or bools.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenSelect(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        // the mask of a vector has the same number of components and the bit width of a component
        var boolType = $"b{typ.size * 8}v{size}";
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        // the value of a 64 bit vector is kept in a raw ulong field, the other simd vectors keep the register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var pad = VectorGenShared.PadLanes(typ, size, storeVariant) > 0;
        // the register of the mask is 128 bits wide for a mask of 4 byte components and the register of the vector
        // itself for a mask of 8 byte ones, see the bool members of the ieee 754 part, a bool of 2 byte
        // components, a half and a short of them have no register and are selected component by component
        var maskVecName = $"Vector{(typ.size == 4 ? 128 : reg)}";
        // the lanes of the mask hold an unsigned integer of the width of a component
        var maskAs = typ.size == 4 ? "AsUInt32" : "AsUInt64";
        // the bool vectors already keep their components in the lanes of the mask, the other types reach them by
        // the cast of the register of the value and come back by its inverse
        var valueAs = simd && !typ.bol ? AsMethod(typ.simdComp) : "";
        var comp = VectorGenShared.Components(size);

        var sb = new StringBuilder();
        string Join(Func<int, string> get, string sep = ", ") => VectorGenShared.Join(size, get, sep);

        // the lanes of a mask that a value reaches, the value of a bool vector is the mask itself
        string ToMask(string expr) => typ.bol ? expr : $"{expr}.{maskAs}()";

        // the value of the lanes of the register of the mask
        string FromMask(string expr) => valueAs.Length == 0 ? expr : $"{expr}.{valueAs}()";

        // the register of the mask and the register of the value, a value of 64 bits is widened to the mask so
        // that the mask reaches every one of its lanes, the padding lanes of the widened value are zero
        var mask = "c.vector";
        var self = v64 ? VectorGenShared.Load64("", typ.simdComp) : "this.vector";
        var other = v64 ? VectorGenShared.Load64("f.", typ.simdComp) : "f.vector";
        var selected = FromMask($"{maskVecName}.ConditionalSelect({mask}, {ToMask(self)}, {ToMask(other)})");

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    IVectorSelect<{type}, {boolType}>");
        sb.AppendLine("{");

        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public readonly {type} select(in {boolType} c, in {type} f)");
        sb.AppendLine("    {");
        if (simd)
        {
            if (v64)
            {
                sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                sb.AppendLine($"            return {VectorGenShared.From128(selected)};");
            }
            else
            {
                // the selected value keeps the padding lanes at zero, the mask of them is zero as well
                sb.AppendLine($"        if ({maskVecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {VectorGenShared.Vector(simd, pad, selected)};");
            }
        }

        sb.AppendLine($"        return new({Join(n => $"(bool)c.{comp[n]} ? this.{comp[n]} : f.{comp[n]}")});");
        sb.AppendLine("    }");
        sb.AppendLine();

        // the static member of the interface forwards to the member that is called on the vector
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {type} select(in {boolType} c, in {type} t, in {type} f) => t.select(c, f);");
        sb.AppendLine("}");

        // the member implements the interface of the select, its documentation is the one of the operation
        return VectorDocs.Apply(sb.ToString());
    }
}
