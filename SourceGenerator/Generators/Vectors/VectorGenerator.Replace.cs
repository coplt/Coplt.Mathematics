using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that replace the components of the vector described by <paramref name="typ"/>, they
    /// implement the <c>IVectorReplace</c> interfaces: a member takes the value of the components its name holds
    /// and the vector keeps the value of the other ones.
    /// <para>Every member has two forms: the member that is called on the vector writes the components of a copy
    /// of it, which is the form that a caller uses instead of the extension members of the legacy library, and the
    /// static one implements the interface that a generic caller is constrained by and forwards to it. The
    /// components of a simd backed vector are written through the swizzle setter of the ones the member replaces,
    /// which writes its register, and the ones of a vector without a register through the fields of it</para>
    /// <para>The members are emitted into their own file, so they stay separate from the members of the base type
    /// and the ones that create the vector</para>
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenReplace(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        // a pair and a triple are always the regular vectors, a storage variant exists for the regular one only
        var type2 = VectorGenShared.VecName(typ, 2, false);
        var type3 = VectorGenShared.VecName(typ, 3, false);

        // the members of the vector: the name of the member, the parameters of the form that is called on the
        // vector and the name of the components it replaces, which is also the name of the value behind them
        var members = new List<(string Name, string Instance, string Components)>
        {
            ("Rx", $"{scalar} x", "x"),
            ("Ry", $"{scalar} y", "y"),
        };
        if (size >= 3)
        {
            members.Add(("Rz", $"{scalar} z", "z"));
            members.Add(("Rxy", $"in {type2} xy", "xy"));
            members.Add(("Ryz", $"in {type2} yz", "yz"));
            members.Add(("Rxz", $"in {type2} xz", "xz"));
        }

        if (size >= 4)
        {
            members.Add(("Rw", $"{scalar} w", "w"));
            members.Add(("Rzw", $"in {type2} zw", "zw"));
            members.Add(("Rxw", $"in {type2} xw", "xw"));
            members.Add(("Ryw", $"in {type2} yw", "yw"));
            members.Add(("Rxyz", $"in {type3} xyz", "xyz"));
            members.Add(("Ryzw", $"in {type3} yzw", "yzw"));
            members.Add(("Rxyw", $"in {type3} xyw", "xyw"));
            members.Add(("Rxzw", $"in {type3} xzw", "xzw"));
        }

        // the interfaces the members implement, they are declared by the part that holds the members instead of
        // the base part because the base part has no member of them
        var ifaces = size switch
        {
            2 => new List<string>
            {
                $"IVectorReplace<{type}, {scalar}>",
            },
            3 => new List<string>
            {
                $"IVector3Replace<{type}, {scalar}, {type2}>",
            },
            _ => new List<string>
            {
                $"IVector4Replace<{type}, {scalar}, {type2}, {type3}>",
            },
        };

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        for (var i = 0; i < ifaces.Count; i++)
        {
            sb.AppendLine($"    {ifaces[i]}" + (i == ifaces.Count - 1 ? "" : ","));
        }

        sb.AppendLine("{");

        foreach (var (name, instance, components) in members)
        {
            sb.AppendLine();
            sb.AppendLine("    /// <summary>Returns the vector with the components of the member replaced by the value or the pair that the member takes</summary>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public readonly {type} {name}({instance})");
            sb.AppendLine("    {");
            sb.AppendLine("        var result = this;");
            sb.AppendLine($"        result.{components} = {components};");
            sb.AppendLine("        return result;");
            sb.AppendLine("    }");
            sb.AppendLine();
            // the static member implements one of the members of the interfaces above and inherits its
            // documentation, it forwards to the member that is called on the vector
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {type} {name}(in {type} self, {instance}) => self.{name}({components});");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
