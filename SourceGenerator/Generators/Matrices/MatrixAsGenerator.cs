using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the as members of every matrix of <see cref="Typ"/>. A matrix reinterprets its bits as the matrix
/// of every other component type of its own group, which is the shape of the matrix whose component is as wide
/// as its own one and whose columns keep their components in the same storage: the components of every member of
/// a group are covered by the bits of every other one, so a bit cast between 2 of them keeps every component.
/// The member of a matrix keeps the bits of the matrix itself, which is the value of the columns of it, and the
/// short spelling of the name of the target is the one of the member, see the as members of the generator of
/// the vectors.
/// <para>The member of the kind of the matrix itself is left out: reinterpreting the bits of a matrix as the
/// matrix of the kind of its own component is the matrix itself, so the interface of that kind is what marks the
/// matrix alone. The as member of the <c>math</c> class reaches the member of the matrix through the interface of
/// the kind of the target, which is the one of a value whose bits can be reinterpreted, never of a vector alone,
/// so the forwarding of a matrix is the forwarding of a vector.</para>
/// <para>The member of a matrix is emitted beside the member of the interface of every kind of the group of it,
/// which the matrix implements in the declaration of the as file: the members of the other parts of the matrix
/// are not touched by this generator.</para>
/// </summary>
[Generator]
public class MatrixAsGenerator : IIncrementalGenerator
{
    /// <summary>The name of the as member of every kind of the as members, the kind is the index of the name</summary>
    private static readonly string[] AsNames = { "asf", "asi", "asu" };

    /// <summary>The interface of every kind of the as members, the kind is the index of the interface</summary>
    private static readonly string[] AsInterfaces = { "IAsF", "IAsI", "IAsU" };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (var typ in Typ.Typs)
            {
                for (var rows = 2; rows <= 4; rows++)
                {
                    for (var cols = 2; cols <= 4; cols++)
                    {
                        // a matrix has a storage variant for the shapes of the column vectors that have one, its
                        // as members are the ones of the regular matrix alone: the storage variant of a matrix
                        // holds the columns of it as they are and only converts to the regular one
                        var name = MatrixGenerator.Name(typ, rows, cols, false);
                        ctx.AddSource(
                            $"{VectorGenerator.VecNamespace}.{name}.as.g.cs",
                            SourceText.From(Gen(typ, rows, cols), Encoding.UTF8));
                    }
                }
            }
        });
    }

    /// <summary>
    /// Returns the kind of the as member of a component type: the floating point kind, which every floating point
    /// component reaches, the signed kind, which every signed component reaches, and the unsigned kind, which
    /// every other component reaches.
    /// </summary>
    /// <param name="typ">The type of the component</param>
    /// <returns>The kind of the as member of the component type</returns>
    private static int Kind(Typ typ) => typ.f ? 0 : typ.sig ? 1 : 2;

    /// <summary>
    /// Returns the component types the bits of a matrix can be reinterpreted as, which is the matrix of every
    /// component type of its own group: the types that are as wide as the type of its component and that keep
    /// their components in the same storage. The members of a group are ordered by the kind of the as member of
    /// them.
    /// </summary>
    /// <param name="typ">The type of the component of the matrix</param>
    /// <returns>The type of the component of every member of the group</returns>
    private static List<Typ> Targets(Typ typ)
    {
        var targets = new List<Typ>();
        foreach (var target in Typ.Typs)
        {
            if (target.size != typ.size) continue;

            targets.Add(target);
        }

        targets.Sort(static (a, b) => Kind(a).CompareTo(Kind(b)));

        return targets;
    }

    /// <summary>
    /// Generates the as members of the matrix and the forwarding of them in the <c>math</c> class. Every member
    /// reinterprets the bits of the matrix as the matrix of another component type of the same group and the kind
    /// of the matrix itself carries no member, because reinterpreting the bits of a matrix as the matrix of the
    /// kind of its own component is the matrix itself: the interface of that kind is what marks the matrix alone.
    /// The forwarding of the matrix is the one of the target alone, which reaches it through the interface, so a
    /// generic member can reach the member of the matrix without naming the matrix at the call site.
    /// </summary>
    /// <param name="typ">The type of the component of the matrix</param>
    /// <param name="rows">The number of rows of the matrix</param>
    /// <param name="cols">The number of columns of the matrix</param>
    /// <returns>The file of the as members of the matrix</returns>
    private static string Gen(Typ typ, int rows, int cols)
    {
        var type = MatrixGenerator.Name(typ, rows, cols, false);
        var targets = Targets(typ);

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);

        // the matrix implements the interface of the kind of the as member of every member of its group
        var ifaces = new List<string>();
        foreach (var target in targets)
        {
            var targetName = MatrixGenerator.Name(target, rows, cols, false);
            ifaces.Add($"{AsInterfaces[Kind(target)]}<{type}, {targetName}>");
        }

        sb.AppendLine($"public partial struct {type} :");
        for (var i = 0; i < ifaces.Count; i++)
        {
            sb.AppendLine($"    {ifaces[i]}{(i == ifaces.Count - 1 ? "" : ",")}");
        }

        sb.AppendLine("{");
        var first = true;
        foreach (var target in targets)
        {
            var targetName = MatrixGenerator.Name(target, rows, cols, false);
            if (targetName == type) continue;

            if (!first) sb.AppendLine();
            first = false;

            // the member reinterprets the bits of the matrix as the matrix of the target, the name of it is the
            // short spelling of the name of the kind of the target
            sb.AppendLine($"    /// <summary>Reinterprets the bits of the matrix as <see cref=\"{targetName}\"/></summary>");
            sb.AppendLine($"    /// <returns>The matrix of <see cref=\"{targetName}\"/> that has the bits of the matrix</returns>");
            sb.AppendLine($"    public readonly {targetName} {AsNames[Kind(target)]}");
            sb.AppendLine("    {");
            sb.AppendLine("        [MethodImpl(256)]");
            sb.AppendLine($"        get => Unsafe.BitCast<{type}, {targetName}>(this);");
            sb.AppendLine("    }");
        }

        sb.AppendLine("}");

        // the member of a component kind reaches the matrix through the interface of the kind of the target, which
        // only names the type of the result: the matrix and the vector of the kind reach the same member and the
        // type of the result cannot be inferred from the source by the compiler of today, so the forwarding of
        // every target is a member of a class of its own
        var kind = Kind(typ);
        var iface = AsInterfaces[kind];
        var name = AsNames[kind];

        sb.AppendLine();
        sb.AppendLine($"public static partial class ex_{type}");
        sb.AppendLine("{");
        sb.AppendLine("    extension(math)");
        sb.AppendLine("    {");
        sb.AppendLine($"        /// <summary>Reinterprets the bits of <paramref name=\"source\"/> as <see cref=\"{type}\"/></summary>");
        sb.AppendLine("        /// <param name=\"source\">The matrix to reinterpret</param>");
        sb.AppendLine($"        /// <returns>The matrix of <see cref=\"{type}\"/> that has the bits of <paramref name=\"source\"/></returns>");
        sb.AppendLine("        [MethodImpl(256)]");
        sb.AppendLine($"        public static {type} {name}<T>(T source) where T : unmanaged, {iface}<T, {type}>");
        sb.AppendLine($"            => Unsafe.As<T, {type}>(ref Unsafe.AsRef(in source));");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}
