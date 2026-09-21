using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// The names of the as members of a component type: the short name, the long name and the kind of the
    /// component. A vector reinterprets its bits as the component type of every other member of its own group, so
    /// the vector of a 16 bit component reaches <c>asf</c> beside <c>as_half</c> and <c>asi</c> beside
    /// <c>as_short</c>. The short name is the name the interface of the kind declares, the long one names the
    /// component type itself.
    /// </summary>
    private static readonly Dictionary<string, (string Short, string Long, int Kind)> AsNames = new()
    {
        { "float", ("asf", "as_float", 0) },
        { "double", ("asf", "as_double", 0) },
        { "half", ("asf", "as_half", 0) },
        { "short", ("asi", "as_short", 1) },
        { "int", ("asi", "as_int", 1) },
        { "long", ("asi", "as_long", 1) },
        { "ushort", ("asu", "as_ushort", 2) },
        { "uint", ("asu", "as_uint", 2) },
        { "ulong", ("asu", "as_ulong", 2) },
        { "b16v", ("asb", "as_b16", 3) },
        { "b32v", ("asb", "as_b32", 3) },
        { "b64v", ("asb", "as_b64", 3) },
    };

    /// <summary>
    /// The interface of every kind of the as members, the kind of a component is the index of the interface.
    /// </summary>
    private static readonly string[] AsInterfaces = { "IVectorAsF", "IVectorAsI", "IVectorAsU", "IVectorAsB" };

    /// <summary>
    /// Returns the types the bits of a vector can be reinterpreted as, it can reach the vector of every component
    /// type of its own group: the types that have as many components as it has, whose components are as wide as
    /// its ones and that keep their components in the same storage. A storage variant reaches the other storage
    /// variants and a regular vector the other regular ones, the components of every member of a group are
    /// covered by the bits of every other one, so a bit cast between them keeps every component. A bool vector
    /// has no storage variant, it joins a group by the width of its value, so a storage variant that has no bool
    /// vector of its own width has no bool member at all. The members of a group are ordered by their kind.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The type of the vector of every member of the group</returns>
    private static List<Typ> AsTargets(Typ typ, int size, bool storeVariant)
    {
        var byteSize = VectorGenShared.ByteSize(typ, size, storeVariant);
        var targets = new List<Typ>();
        foreach (var target in Typ.Typs)
        {
            if (target.size != typ.size) continue;
            if (target.bol)
            {
                // a bool vector has no storage variant, it joins the group by the width of its value
                if (VectorGenShared.ByteSize(target, size, false) != byteSize) continue;
            }
            // a type whose storage variant does not exist has no storage variant to reach, its own name is the
            // one of a regular vector
            else if (storeVariant && !VectorGenShared.HasStorageVariant(target, size)) continue;

            targets.Add(target);
        }

        targets.Sort(static (a, b) => AsNames[a.name].Kind.CompareTo(AsNames[b.name].Kind));

        return targets;
    }

    /// <summary>
    /// Generates the as members of the vector described by <paramref name="typ"/> and the conversion between the
    /// vector and its storage variant. The as members reinterpret the bits of the vector as the vector of another
    /// component type of the same width, the short spelling of the name of a target and the long one are emitted
    /// side by side, every member of the group implements the interface of its own kind. The regular vector and
    /// its storage variant convert into each other with <c>to_storage</c> and <c>to_compute</c>: the first one is
    /// emitted beside the as members of the regular vector, the second one beside the as members of the storage
    /// variant. Every member is a member of the vector itself, they are emitted into their own file, so they stay
    /// separate from the members of the base type and the arithmetic.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file, null when the vector has no member at all</returns>
    private static string? GenAs(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var targets = AsTargets(typ, size, storeVariant);
        // a regular vector that has a storage variant also converts to it, the as members of a storage variant
        // always exist and the conversion of it always exists as well
        var store = storeVariant ? null : VectorGenShared.HasStorageVariant(typ, size) ? VectorGenShared.VecName(typ, size, true) : null;
        var regular = storeVariant ? VectorGenShared.VecName(typ, size, false) : null;
        if (targets.Count == 0 && store == null && regular == null) return null;

        var sb = new StringBuilder();
        var first = true;
        var ifaces = new List<string>();

        // a bool vector has no storage variant, the name of it is always the regular one
        string TargetName(Typ target) => VectorGenShared.VecName(target, size, storeVariant && !target.bol);

        // emits a member with its documentation, every member beside the first one is separated from the one
        // before it by an empty line
        void Member(string summary, string returns, string signature)
        {
            if (!first) sb.AppendLine();
            first = false;
            sb.AppendLine($"    /// <summary>{summary}</summary>");
            sb.AppendLine($"    /// <returns>{returns}</returns>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    {signature}");
        }

        VectorGenShared.FileHeader(sb, false);
        // the members of the group implement the interfaces of the vector, every member of the group has one of
        // them, the short name of a member is the name its interface declares
        foreach (var target in targets)
        {
            var targetName = TargetName(target);
            var (_, _, kind) = AsNames[target.name];
            ifaces.Add($"{AsInterfaces[kind]}<{type}, {targetName}>");
        }

        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    {string.Join(",\n    ", ifaces)}");
        sb.AppendLine("{");

        // the as members reinterpret the bits of the vector as another vector of the same group, the name of
        // every member is built from the components of the target. The vector keeps the member of every target on
        // itself and the interface declares it as a static member that takes the vector as its parameter, so
        // every target also has the static member that forwards to the one of the vector
        foreach (var target in targets)
        {
            var targetName = TargetName(target);
            var (shortName, longName, _) = AsNames[target.name];
            var summary = $"Reinterprets the bits of <paramref name=\"source\"/> as <see cref=\"{targetName}\"/>";
            var returns = $"The vector of <see cref=\"{targetName}\"/> that has the bits of <paramref name=\"source\"/>";
            Member(summary, returns, $"public static {targetName} {shortName}(in {type} source) => source.{shortName}();");
            Member(summary, returns, $"public static {targetName} {longName}(in {type} source) => source.{longName}();");
            foreach (var name in new[] { shortName, longName })
            {
                Member($"Reinterprets the bits of the vector as <see cref=\"{targetName}\"/>",
                    $"The vector of <see cref=\"{targetName}\"/> that has the bits of the vector",
                    $"public readonly {targetName} {name}() => Unsafe.BitCast<{type}, {targetName}>(this);");
            }
        }

        if (store != null)
        {
            Member($"Converts the vector to its storage variant <see cref=\"{store}\"/>",
                "The storage variant of the vector",
                $"public readonly {store} to_storage() => ({store})this;");
        }

        if (regular != null)
        {
            Member($"Converts the storage variant of the vector to the regular <see cref=\"{regular}\"/>",
                "The regular vector",
                $"public readonly {regular} to_compute() => ({regular})this;");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Generates the as member of the <c>math</c> class for the vector described by <paramref name="typ"/>. The
    /// vector itself is the target of the kind of its own component, so a generic member that is constrained by
    /// the interface of that kind reaches the vector from every member of its group: <c>math.asf(float2)</c> and
    /// <c>math.asf(int2)</c> both reach the floating point vector of the group. The member forwards the call to
    /// the as member of the target vector itself. The forwarding of every target has the same name and the same
    /// parameters, a constraint does not take part in the signature of a member, so every one of them is an
    /// extension member of a class of its own and is emitted into its own file.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenMathAs(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var (name, _, kind) = AsNames[typ.name];
        var iface = AsInterfaces[kind];

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public static partial class ex_{type}");
        sb.AppendLine("{");
        sb.AppendLine("    extension(math)");
        sb.AppendLine("    {");
        sb.AppendLine($"        /// <summary>Reinterprets the bits of <paramref name=\"source\"/> as <see cref=\"{type}\"/></summary>");
        sb.AppendLine("        /// <param name=\"source\">The vector to reinterpret</param>");
        sb.AppendLine($"        /// <returns>The vector of <see cref=\"{type}\"/> that has the bits of <paramref name=\"source\"/></returns>");
        sb.AppendLine("        [MethodImpl(256)]");
        sb.AppendLine($"        public static {type} {name}<T>(in T source) where T : unmanaged, {iface}<T, {type}>");
        sb.AppendLine($"            => T.{name}(source);");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}
