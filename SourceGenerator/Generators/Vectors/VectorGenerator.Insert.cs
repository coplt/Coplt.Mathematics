using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members of the legacy insert api of the vector described by <paramref name="typ"/>, they
    /// implement the <c>IVector2Insert</c> interface of a vector of 2 components and the <c>IVector3Insert</c>
    /// interface of one of 3 components: the member is called on the short vector that holds the components that
    /// the longer one does not take and builds it, so <c>Iz(z)</c> of a pair and <c>Iy(y)</c> of a triple are the
    /// legacy spelling of <c>Create(xy, z)</c> and of <c>InsertY(xzw, y)</c>.
    /// <para>Every member has two forms: the member that is called on the vector builds it, which is the form that
    /// a caller uses instead of the extension members of the legacy library, and the static one implements the
    /// interface that a generic caller is constrained by and forwards to it</para>
    /// <para>The two forms forward to the member of the create of the vector they build, which holds the
    /// accelerated form and the component wise one, so the members of this kind are a second name of the members
    /// of the create alone. They are emitted into their own file, so they stay separate from the members they
    /// forward to</para>
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file, null when the vector has no member at all</returns>
    private static string? GenInsert(Typ typ, int size, bool storeVariant)
    {
        // a vector of 4 components is the one that every other one inserts into, it has no component to insert
        // into a longer one and no member of this kind
        if (size != 2 && size != 3) return null;

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        // the vector that a member builds is always the regular one, a storage variant exists for the regular
        // one only
        var type3 = VectorGenShared.VecName(typ, 3, false);
        var type4 = VectorGenShared.VecName(typ, 4, false);

        // the members of the vector: the name of the member, the parameters of the form that is called on the
        // vector, the arguments of the static form behind them and the member of the create of the vector they
        // build that takes the same arguments. The members are grouped by the vector they build
        var third = new List<(string Name, string Instance, string Args, string Body)>();
        var fourth = new List<(string Name, string Instance, string Args, string Body)>();
        if (size == 2)
        {
            third.Add(("Ix", $"{scalar} x", "x", $"{type3}.Create(x, this)"));
            third.Add(("Iy", $"{scalar} y", "y", $"{type3}.InsertY(this, y)"));
            third.Add(("Iz", $"{scalar} z", "z", $"{type3}.Create(this, z)"));
            fourth.Add(("Izw", $"in {type} zw", "zw", $"{type4}.Create(this, zw)"));
            fourth.Add(("Izw", $"{scalar} z, {scalar} w", "z, w", $"{type4}.Create(this, z, w)"));
            fourth.Add(("Ixy", $"in {type} xy", "xy", $"{type4}.Create(xy, this)"));
            fourth.Add(("Ixy", $"{scalar} x, {scalar} y", "x, y", $"{type4}.Create(x, y, this)"));
            fourth.Add(("Iyz", $"in {type} yz", "yz", $"{type4}.InsertYZ(this, yz)"));
            fourth.Add(("Iyz", $"{scalar} y, {scalar} z", "y, z", $"{type4}.InsertYZ(this, y, z)"));
            fourth.Add(("Ixw", $"in {type} xw", "xw", $"{type4}.InsertXW(this, xw)"));
            fourth.Add(("Ixw", $"{scalar} x, {scalar} w", "x, w", $"{type4}.InsertXW(this, x, w)"));
            fourth.Add(("Iyw", $"in {type} yw", "yw", $"{type4}.InsertYW(this, yw)"));
            fourth.Add(("Iyw", $"{scalar} y, {scalar} w", "y, w", $"{type4}.InsertYW(this, y, w)"));
            fourth.Add(("Ixz", $"in {type} xz", "xz", $"{type4}.InsertXZ(this, xz)"));
            fourth.Add(("Ixz", $"{scalar} x, {scalar} z", "x, z", $"{type4}.InsertXZ(this, x, z)"));
        }
        else
        {
            fourth.Add(("Ix", $"{scalar} x", "x", $"{type4}.Create(x, this)"));
            fourth.Add(("Iy", $"{scalar} y", "y", $"{type4}.InsertY(this, y)"));
            fourth.Add(("Iz", $"{scalar} z", "z", $"{type4}.InsertZ(this, z)"));
            fourth.Add(("Iw", $"{scalar} w", "w", $"{type4}.Create(this, w)"));
        }

        // the interface the members implement, it is declared by the part that holds the members instead of the
        // base part because the base part has no member of them
        var ifaces = size == 2
            ? new List<string> { $"IVector2Insert<{type}, {scalar}, {type3}, {type4}>" }
            : new List<string> { $"IVector3Insert<{type}, {scalar}, {type4}>" };

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        for (var i = 0; i < ifaces.Count; i++)
        {
            sb.AppendLine($"    {ifaces[i]}" + (i == ifaces.Count - 1 ? "" : ","));
        }

        sb.AppendLine("{");

        foreach (var (name, instance, args, body) in third)
        {
            sb.AppendLine();
            sb.AppendLine("    /// <summary>Returns the vector of 3 components that has the components of the vector and the value that the member takes</summary>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public readonly {type3} {name}({instance}) => {body};");
            sb.AppendLine();
            // the static member implements one of the members of the interface above and inherits its
            // documentation, it forwards to the member that is called on the vector
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {type3} {name}(in {type} self, {instance}) => self.{name}({args});");
        }

        foreach (var (name, instance, args, body) in fourth)
        {
            sb.AppendLine();
            sb.AppendLine("    /// <summary>Returns the vector of 4 components that has the components of the vector and the value or the pair that the member takes</summary>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public readonly {type4} {name}({instance}) => {body};");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {type4} {name}(in {type} self, {instance}) => self.{name}({args});");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
