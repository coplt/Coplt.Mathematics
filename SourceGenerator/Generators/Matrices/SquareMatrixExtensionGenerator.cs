using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the members of the square matrix types of a shape that forward to a member of the class of a shape of
/// a square matrix that is marked with <see cref="Attribute"/>. The type of the matrix a member works on is only
/// a part of its result, so the compiler cannot infer it from the arguments and a call that does not name it
/// reaches the member of the matrix type of the value, which names the types of it and forwards the call to the
/// marked member.
/// <para>The types a marked member names with a type parameter of its own are the ones the constraints of the type
/// parameter of the matrix name: the interface of a shape names the number of the rows and the number of the
/// columns of the shape, the one that names a single component of the value names the type of it and the one that
/// names a column of it names the type of that column. Every shape the constraints name is emitted and every kind
/// of a number that reaches the type of a single component is, so a marked member that names the component of a
/// floating point kind reaches the matrix types of every floating point kind. The shape of a marked member is not
/// checked: the name of the generator names the shape the algebra of today reaches, which is the square one, and
/// the member of a matrix of any shape the constraints name is emitted.</para>
/// <para>The members are emitted into the classes of the scalar type of the value, which are the classes the
/// scalar generator emits the members of as well: <c>ex_*</c> adds them to the <c>math</c> class, so the types of
/// the value are named where the member is called and the other types of it are still inferred, and
/// <c>math_ex_*</c> adds them to one of the values, so the call reads like the member of a matrix. The parameter a
/// member of the value is called on is the first one of the marked member unless the attribute names another one,
/// see <see cref="ThisParameterProperty"/>. The marked member decides whether the generated members reach the
/// overload resolution with a priority of their own, see <see cref="PriorityProperty"/>.</para>
/// </summary>
[Generator]
public class SquareMatrixExtensionGenerator : IIncrementalGenerator
{
    /// <summary>The namespace of the generated members, which is the one of the <c>math</c> class.</summary>
    public const string Namespace = "Coplt.Mathematics";

    /// <summary>The name of the attribute a member is marked with.</summary>
    public const string Attribute = "SquareMatrixExtensionAttribute";

    /// <summary>
    /// The name of the property of the attribute that names the parameter of a marked member that a member of the
    /// value is called on, which is the receiver of it. The first parameter of the marked member is the one when
    /// the property does not name a parameter.
    /// </summary>
    public const string ThisParameterProperty = "ThisParameter";

    /// <summary>
    /// The name of the property of the attribute that decides whether a generated member carries an
    /// <c>OverloadResolutionPriority</c> and which one it is. The value of it that emits no priority at all is
    /// <see cref="NoPriority"/>.
    /// </summary>
    public const string PriorityProperty = "OverloadResolutionPriority";

    /// <summary>
    /// The value of <see cref="PriorityProperty"/> that leaves the <c>OverloadResolutionPriority</c> of a
    /// generated member off, the generated attribute declares the same value.
    /// </summary>
    public const int NoPriority = int.MinValue;

    /// <summary>The attribute of a member, it is inlined into its caller.</summary>
    private const string Attr = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";

    /// <summary>
    /// The name of the class a marked member is a member of when it is an entry point of the operation it
    /// implements, which is the class the generated members are added to without a part of their own.
    /// </summary>
    private const string MathClass = "math";

    /// <summary>
    /// The format of the name of a type in a generated member: the namespace and the containing types of a type
    /// are spelled out, so a member names the type of the marked member itself instead of the one a name reaches
    /// through the usings of the generated file, and a type of the language is named by the keyword of it.
    /// </summary>
    private static readonly SymbolDisplayFormat TypeFormat = SymbolDisplayFormat.FullyQualifiedFormat
        .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)
        .WithMiscellaneousOptions(
            SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions | SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    /// <summary>The form of the type a constraint of a matrix names beside the matrix itself.</summary>
    private enum Form
    {
        /// <summary>The constraint names no other type of the matrix</summary>
        None,

        /// <summary>The constraint names the type of a single component of the matrix</summary>
        Scalar,

        /// <summary>The constraint names the type of a column of the matrix</summary>
        Vector,
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
            $"{Namespace}.{Attribute}.g.cs",
            SourceText.From(GenAttribute(), Encoding.UTF8)));

        var members = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{Namespace}.{Attribute}",
                static (node, _) => node is MethodDeclarationSyntax,
                static (ctx, _) => ctx.TargetSymbol as IMethodSymbol)
            .Where(static member => member is not null);

        context.RegisterSourceOutput(members, static (ctx, member) => Emit(ctx, member!));
    }

    /// <summary>
    /// Generates the attribute a member of the class of a shape of a matrix is marked with.
    /// </summary>
    /// <returns>The file of the attribute</returns>
    private static string GenAttribute()
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine($"namespace {Namespace};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Marks a member of the class of a shape of a square matrix that names the type of the matrix it works on, the");
        sb.AppendLine("/// type of a column of it and the type of a single component of it with type parameters of their own. The");
        sb.AppendLine("/// generator emits the member of every matrix type the constraints of those type parameters reach, which");
        sb.AppendLine("/// names the types themselves and forwards the call to the marked member, so a call that does not name");
        sb.AppendLine("/// them reaches the member of the matrix type of the value.");
        sb.AppendLine("/// <para>The shape of a marked member is not checked: the name of the attribute names the shape the");
        sb.AppendLine("/// algebra of today reaches, which is the square one, and the member of a matrix of any shape the");
        sb.AppendLine("/// constraints of it name is emitted.</para>");
        sb.AppendLine("/// <para><see cref=\"ThisParameter\"/> names the parameter of the marked member that a member of");
        sb.AppendLine("/// the value is called on, the first one is the receiver when it is empty. The value of");
        sb.AppendLine("/// <see cref=\"OverloadResolutionPriority\"/> decides whether the generated members carry the");
        sb.AppendLine("/// priority of the same name, <see cref=\"NoPriority\"/> leaves it off.</para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("[global::System.AttributeUsage(global::System.AttributeTargets.Method)]");
        sb.AppendLine($"internal sealed class {Attribute} : global::System.Attribute");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>The value of <see cref=\"OverloadResolutionPriority\"/> that emits no attribute</summary>");
        sb.AppendLine("    public const int NoPriority = int.MinValue;");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// The name of the parameter of the marked member that a member of the value is called on, which is the");
        sb.AppendLine("    /// receiver of it. The first parameter of the marked member is the receiver when the name is empty, a name");
        sb.AppendLine("    /// that matches no parameter leaves the member of the value off.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public string ThisParameter { get; set; } = \"\";");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// The priority of a generated member in the overload resolution, it is emitted as the attribute of the");
        sb.AppendLine("    /// same name. The priority puts the member of the matrix type of a value in front of a member that only");
        sb.AppendLine("    /// reaches the value through a conversion.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <seealso cref=\"NoPriority\"/>");
        sb.AppendLine("    public int OverloadResolutionPriority { get; set; } = NoPriority;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Emits the members of every matrix type of every shape of a marked member.
    /// </summary>
    /// <param name="context">The context of the generation</param>
    /// <param name="method">The marked member</param>
    private static void Emit(SourceProductionContext context, IMethodSymbol method)
    {
        // the type parameter of the matrix is the one a constraint of the shape of a matrix belongs to, and the
        // shape of the matrix is the one that constraint names
        ITypeParameterSymbol? matrix = null;
        var rows = 0;
        var cols = 0;
        foreach (var typeParameter in method.TypeParameters)
        {
            foreach (var constraint in typeParameter.ConstraintTypes)
            {
                if (Shape(constraint.Name) is not { } shape) continue;
                matrix = typeParameter;
                rows = shape.Rows;
                cols = shape.Cols;
                break;
            }

            if (matrix is not null) break;
        }

        // a member that names no shape of a matrix reaches no member of the generator
        if (matrix is null) return;

        // the type of a column of the matrix and the type of a single component of it are the other types the
        // constraints of the type parameter of the matrix name
        ITypeParameterSymbol? column = null;
        ITypeParameterSymbol? scalar = null;
        foreach (var constraint in matrix.ConstraintTypes)
        {
            var form = Shape(constraint.Name)?.Form ?? Companion(constraint.Name);
            if (form == Form.Vector) column ??= Argument(constraint, 1);
            if (form == Form.Scalar) scalar ??= Argument(constraint, 1);
        }

        // the type of a single component is named by a type parameter of its own
        if (scalar is null) return;

        // the marked member decides which types of a single component reach it: the type parameter of the kind of
        // a floating point number is only reached by the floating point types
        var floating = Floating(scalar);

        // the type of the matrix, the type of a column of it and the type of a single component of it are spelled
        // out by the generated member, the rest of the type parameters are kept and inferred as they are
        var typeParameters = method.TypeParameters
            .Where(p => !SymbolEqualityComparer.Default.Equals(p, matrix)
                && !SymbolEqualityComparer.Default.Equals(p, column)
                && !SymbolEqualityComparer.Default.Equals(p, scalar)).ToArray();
        var priority = Priority(method);
        var receiver = Receiver(method);

        foreach (var typ in Typ.Typs)
        {
            // only a number type has arithmetic
            if (!typ.arith) continue;
            if (floating && !typ.f) continue;
            // a matrix reaches the member of the shape of it alone: the storage variant of a matrix keeps the
            // columns of the value and reaches no member of the shape it is one of
            var matrixName = MatrixGenerator.Name(typ, rows, cols, false);
            var names = new List<(string Name, string Value)> { (matrix.Name, matrixName) };
            if (column is not null) names.Add((column.Name, VectorGenShared.VecName(typ, rows, false)));
            names.Add((scalar.Name, typ.name));
            context.AddSource(
                $"{Namespace}.{method.Name}.{matrixName}.{Signature(method)}.g.cs",
                SourceText.From(Gen(typ.name, names, method, typeParameters, priority, receiver), Encoding.UTF8));
        }
    }

    /// <summary>
    /// Returns the form the name of a constraint names beside the shape of it: the interface that names the type
    /// of a single component of a matrix ends with <c>Scalar</c> and the one that names the type of a column of it
    /// ends with <c>Vector</c>. The interface of the kind of a matrix names the type of a single component as
    /// well, the name of it ends with <c>Matrix</c>.
    /// </summary>
    /// <param name="name">The name of a constraint</param>
    /// <returns>The form of the constraint</returns>
    private static Form Companion(string name) =>
        name.EndsWith("Vector") ? Form.Vector
        : name.EndsWith("Scalar") || name.EndsWith("Matrix") ? Form.Scalar
        : Form.None;

    /// <summary>
    /// Returns the shape a name of a constraint names, which is the number of the rows and the number of the
    /// columns of it and the form of the interface beside the shape, or null for a name that is not the one of the
    /// interface of a shape. The name of the interface of a shape is <c>IMatrix{rows}x{cols}</c> beside the suffix
    /// that names the form of it, which is the one that reaches the components of the value, the one that reaches
    /// the columns of it or none of them. The interface of the number of the columns alone names no shape, it
    /// hides the number of the rows.
    /// </summary>
    /// <param name="name">The name of a constraint</param>
    /// <returns>The shape of the constraint or null</returns>
    private static (int Rows, int Cols, Form Form)? Shape(string name)
    {
        const string prefix = "IMatrix";
        if (!name.StartsWith(prefix)) return null;
        var i = prefix.Length;
        if (i >= name.Length || !char.IsDigit(name[i])) return null;
        var rows = 0;
        while (i < name.Length && char.IsDigit(name[i])) rows = rows * 10 + (name[i++] - '0');
        if (i >= name.Length || name[i] != 'x') return null;
        i++;
        var start = i;
        var cols = 0;
        while (i < name.Length && char.IsDigit(name[i])) cols = cols * 10 + (name[i++] - '0');
        if (i == start) return null;
        var suffix = name.Substring(i);
        if (suffix.Length == 0) return (rows, cols, Form.None);
        if (suffix == "Scalar") return (rows, cols, Form.Scalar);
        if (suffix == "Vector") return (rows, cols, Form.Vector);
        return null;
    }

    /// <summary>
    /// Returns the type parameter a constraint names at a position, which is the one a type of the matrix is named
    /// by, or null when the constraint names a type that is not a type parameter at that position.
    /// </summary>
    /// <param name="constraint">The constraint</param>
    /// <param name="index">The position of the type argument</param>
    /// <returns>The type parameter or null</returns>
    private static ITypeParameterSymbol? Argument(ITypeSymbol constraint, int index) =>
        constraint is INamedTypeSymbol named && named.TypeArguments.Length > index
            ? named.TypeArguments[index] as ITypeParameterSymbol
            : null;

    /// <summary>
    /// Returns whether the type of a single component of a marked member is only reached by the floating point
    /// types of a matrix, which the constraint of the type parameter of it says: the constraint of the kind of a
    /// number reaches every number type of a matrix and the one of the kind of a floating point number only
    /// reaches the floating point ones.
    /// </summary>
    /// <param name="scalar">The type parameter of a marked member that names the type of a single component</param>
    /// <returns>True for a member that only the floating point types of a matrix reach</returns>
    private static bool Floating(ITypeParameterSymbol scalar) =>
        scalar.ConstraintTypes.Any(static c => c.Name is "IBinaryFloatingPointIeee754");

    /// <summary>
    /// Returns the part of the name of a generated file that tells the marked members which share a name apart: the
    /// name of the type that contains the member and the types of its parameters. The name of a file has to be
    /// unique within the generator, and the members of two classes reach the same one of them when they share a
    /// name and the shape of the matrix they reach.
    /// </summary>
    /// <param name="method">The marked member</param>
    /// <returns>The name of the member</returns>
    private static string Signature(IMethodSymbol method)
    {
        var sb = new StringBuilder(method.ContainingType.Name);
        foreach (var parameter in method.Parameters)
        {
            // a type is named without its namespace, the name of a file only has to tell the members that share
            // a name apart
            sb.Append('_').Append(FileSafe(parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns a text as a part of the name of a generated file: every character that a name of a file does not
    /// carry is spelled out with an underscore.
    /// </summary>
    /// <param name="text">The text</param>
    /// <returns>The text of the name of a file</returns>
    private static string FileSafe(string text)
    {
        var sb = new StringBuilder(text.Length);
        foreach (var c in text) sb.Append(char.IsLetterOrDigit(c) || c == '_' ? c : '_');
        return sb.ToString();
    }

    /// <summary>
    /// Returns the text of a type with the type parameters of the marked member that name the types of the value
    /// spelled out. The name of a type parameter is only reached where it is the whole name of an identifier, so
    /// a type parameter whose name carries the name of another one is left as it is.
    /// </summary>
    /// <param name="text">The text of a type</param>
    /// <param name="names">The name of every spelled out type parameter and the type it is spelled out with</param>
    /// <returns>The text of the type</returns>
    private static string Substitute(string text, List<(string Name, string Value)> names)
    {
        foreach (var (name, value) in names) text = Regex.Replace(text, $@"\b{name}\b", value);
        return text;
    }

    /// <summary>
    /// Returns the value of the named argument <paramref name="name"/> of the attribute of a marked member, which
    /// is <paramref name="fallback"/> when the attribute does not name it.
    /// </summary>
    /// <param name="method">The marked member</param>
    /// <param name="name">The name of the argument</param>
    /// <param name="fallback">The value of an argument the attribute does not name</param>
    /// <returns>The value of the named argument</returns>
    private static object? NamedArgument(IMethodSymbol method, string name, object? fallback)
    {
        foreach (var attribute in method.GetAttributes())
        {
            if (attribute.AttributeClass?.Name != Attribute) continue;
            foreach (var argument in attribute.NamedArguments)
            {
                if (argument.Key == name) return argument.Value.Value;
            }

            break;
        }

        return fallback;
    }

    /// <summary>
    /// Returns the value of <see cref="PriorityProperty"/> of a marked member, which is <see cref="NoPriority"/>
    /// when the attribute does not name it.
    /// </summary>
    /// <param name="method">The marked member</param>
    /// <returns>The priority of the generated members</returns>
    private static int Priority(IMethodSymbol method)
        => NamedArgument(method, PriorityProperty, NoPriority) is int priority ? priority : NoPriority;

    /// <summary>
    /// Returns the index of the parameter of a marked member that a member of the value is called on, which is the
    /// first parameter unless the attribute names another one. A name that matches no parameter of the marked
    /// member leaves the member of the value off, it would be called on the wrong value otherwise.
    /// </summary>
    /// <param name="method">The marked member</param>
    /// <returns>The index of the parameter or -1 when a member of the value is not called on one of them</returns>
    private static int Receiver(IMethodSymbol method)
    {
        if (NamedArgument(method, ThisParameterProperty, "") is not string name) return -1;
        if (name.Length == 0) return 0;
        for (var i = 0; i < method.Parameters.Length; i++)
        {
            if (method.Parameters[i].Name == name) return i;
        }

        return -1;
    }

    /// <summary>
    /// Returns the reference to the marked member, which is what a generated member inherits its documentation
    /// from: the name of the member, the type parameters of it and the types of its parameters.
    /// </summary>
    /// <param name="method">The marked member</param>
    /// <returns>The reference</returns>
    private static string Cref(IMethodSymbol method)
    {
        var typeParameters = method.TypeParameters.Length == 0
            ? ""
            : $"{{{string.Join(", ", method.TypeParameters.Select(p => p.Name))}}}";
        var parameters = string.Join(", ", method.Parameters.Select(p => $"{RefKindText(p.RefKind)}{p.Type.ToDisplayString(TypeFormat)}"));
        return $"{method.ContainingType.Name}.{method.Name}{typeParameters}({parameters})";
    }

    /// <summary>
    /// Returns the keyword a parameter of a member is declared with.
    /// </summary>
    /// <param name="kind">The kind of the parameter</param>
    /// <returns>The keyword, which is empty for a parameter that is passed by value</returns>
    private static string RefKindText(RefKind kind) => kind switch
    {
        RefKind.In => "in ",
        RefKind.Ref => "ref ",
        RefKind.Out => "out ",
        RefKind.RefReadOnlyParameter => "ref readonly ",
        _ => ""
    };

    /// <summary>
    /// Generates the members of one matrix type of the scalar type <paramref name="scalar"/> that forward to
    /// <paramref name="method"/>.
    /// </summary>
    /// <param name="scalar">The scalar type of a single component</param>
    /// <param name="names">The name of every spelled out type parameter of the marked member and the type it is spelled out with</param>
    /// <param name="method">The marked member</param>
    /// <param name="typeParameters">The type parameters of the marked member that are not spelled out</param>
    /// <param name="priority">The <c>OverloadResolutionPriority</c> of the generated members, <see cref="NoPriority"/> for none</param>
    /// <param name="receiver">The index of the parameter of the marked member a member of the value is called on, -1 for none</param>
    /// <returns>The file of the members</returns>
    private static string Gen(
        string scalar, List<(string Name, string Value)> names, IMethodSymbol method,
        ITypeParameterSymbol[] typeParameters, int priority, int receiver)
    {
        // the marked member names the types of the value with type parameters of its own, a generated member names
        // the types themselves, so every type that carries one of them is written with the type of it
        string Type(ITypeSymbol type) => Substitute(type.ToDisplayString(TypeFormat), names);

        // the type argument of a type parameter of the marked member is the type it is spelled out with, a type
        // parameter that is spelled out with nothing is inferred from the arguments
        string TypeArgument(string name)
        {
            foreach (var (parameter, value) in names)
            {
                if (parameter == name) return value;
            }

            return name;
        }

        var typeArguments = string.Join(", ", method.TypeParameters.Select(p => TypeArgument(p.Name)));
        var typeParametersText = typeParameters.Length == 0 ? "" : $"<{string.Join(", ", typeParameters.Select(p => p.Name))}>";

        // the parameters of a member of the math class are the ones of the marked member
        var parameters = string.Join(", ", method.Parameters.Select(p => $"{RefKindText(p.RefKind)}{Type(p.Type)} {p.Name}"));

        // the parameters of a member of the value are the ones of the marked member as well, the parameter that
        // the member is called on is the first one of them and the rest keep the ref kind of the marked member
        var others = method.Parameters.Where((p, i) => i != receiver).ToArray();
        var calledOn = receiver < 0 ? null : method.Parameters[receiver];
        var receiverParameters = calledOn is null
            ? ""
            : string.Join(", ", new[] { $"this {Type(calledOn.Type)} {calledOn.Name}" }
                .Concat(others.Select(p => $"{RefKindText(p.RefKind)}{Type(p.Type)} {p.Name}")));

        // the arguments of the call of the marked member are the ones of it in its own order
        var arguments = string.Join(", ", method.Parameters.Select(Argument));

        string Argument(IParameterSymbol parameter) => parameter.RefKind switch
        {
            RefKind.Ref => $"ref {parameter.Name}",
            RefKind.Out => $"out {parameter.Name}",
            _ => parameter.Name,
        };

        var target = $"{method.ContainingType.Name}.{method.Name}<{typeArguments}>";
        var cref = Cref(method);

        // the constraints of a type parameter of the marked member are the ones of the generated member, the
        // types that the spelled out type parameters name are spelled out as well
        string Constraints(string indent)
        {
            var text = new StringBuilder();
            foreach (var typeParameter in typeParameters)
            {
                var parts = new List<string>();
                if (typeParameter.HasUnmanagedTypeConstraint) parts.Add("unmanaged");
                else if (typeParameter.HasValueTypeConstraint) parts.Add("struct");
                else if (typeParameter.HasReferenceTypeConstraint) parts.Add("class");
                foreach (var constraint in typeParameter.ConstraintTypes) parts.Add(Type(constraint));
                if (typeParameter.HasConstructorConstraint) parts.Add("new()");
                if (parts.Count == 0) continue;
                text.AppendLine();
                text.Append($"{indent}where {typeParameter.Name} : {string.Join(", ", parts)}");
            }

            return text.ToString();
        }

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine();
        sb.AppendLine($"namespace {Namespace};");
        sb.AppendLine();

        // the priority is only emitted when the attribute of the marked member asks for it
        void PriorityAttr(string indent)
        {
            if (priority == NoPriority) return;
            sb.AppendLine($"{indent}[OverloadResolutionPriority({priority})]");
        }

        // the member the math class carries, the types of the value are named where the member is called
        sb.AppendLine($"public static partial class ex_{scalar}");
        sb.AppendLine("{");
        sb.AppendLine($"    extension({MathClass})");
        sb.AppendLine("    {");
        sb.AppendLine($"        /// <inheritdoc cref=\"{cref}\"/>");
        PriorityAttr("        ");
        sb.AppendLine($"        {Attr}");
        sb.AppendLine($"        public static {Type(method.ReturnType)} {method.Name}{typeParametersText}({parameters}){Constraints("            ")}");
        sb.AppendLine($"            => {target}({arguments});");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();

        // the member the value itself carries, it reads like the member of a matrix, a marked member whose
        // receiver names no parameter of it has no member of the value at all
        if (receiver >= 0)
        {
            sb.AppendLine($"public static partial class math_ex_{scalar}");
            sb.AppendLine("{");
            sb.AppendLine($"    /// <inheritdoc cref=\"{cref}\"/>");
            PriorityAttr("    ");
            sb.AppendLine($"    {Attr}");
            sb.AppendLine($"    public static {Type(method.ReturnType)} {method.Name}{typeParametersText}({receiverParameters}){Constraints("        ")}");
            sb.AppendLine($"        => {target}({arguments});");
            sb.AppendLine("}");
        }

        return sb.ToString();
    }
}
