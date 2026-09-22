using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that replace the components of the vector described by <paramref name="typ"/>, they
    /// implement the <c>IVectorReplace</c> interfaces: a member takes the value of the components its name holds
    /// and the vector keeps the value of the other ones. The value is written into a copy of the vector, so the
    /// member of a simd backed vector goes through the swizzle setter of the components it replaces, which writes
    /// its register, and the member of a vector without a register through the fields of it.
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

        // the members of the vector: the signature of the member and the name of the components it replaces,
        // which is also the name of the value behind it
        var members = new List<(string Signature, string Components)>
        {
            ($"Rx(in {type} self, {scalar} x)", "x"),
            ($"Ry(in {type} self, {scalar} y)", "y"),
        };
        if (size >= 3)
        {
            members.Add(($"Rz(in {type} self, {scalar} z)", "z"));
            members.Add(($"Rxy(in {type} self, in {type2} xy)", "xy"));
            members.Add(($"Ryz(in {type} self, in {type2} yz)", "yz"));
            members.Add(($"Rxz(in {type} self, in {type2} xz)", "xz"));
        }

        if (size >= 4)
        {
            members.Add(($"Rw(in {type} self, {scalar} w)", "w"));
            members.Add(($"Rzw(in {type} self, in {type2} zw)", "zw"));
            members.Add(($"Rxw(in {type} self, in {type2} xw)", "xw"));
            members.Add(($"Ryw(in {type} self, in {type2} yw)", "yw"));
            members.Add(($"Rxyz(in {type} self, in {type3} xyz)", "xyz"));
            members.Add(($"Ryzw(in {type} self, in {type3} yzw)", "yzw"));
            members.Add(($"Rxyw(in {type} self, in {type3} xyw)", "xyw"));
            members.Add(($"Rxzw(in {type} self, in {type3} xzw)", "xzw"));
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

        foreach (var (signature, components) in members)
        {
            sb.AppendLine();
            // the member implements one of the members of the interfaces above and inherits its documentation
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {type} {signature}");
            sb.AppendLine("    {");
            sb.AppendLine("        var result = self;");
            sb.AppendLine($"        result.{components} = {components};");
            sb.AppendLine("        return result;");
            sb.AppendLine("    }");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
