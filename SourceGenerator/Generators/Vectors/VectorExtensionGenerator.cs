using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the members of the vector types of the <c>math</c> class that forward to a member of it that is
/// marked with <see cref="Attribute"/>. The type of a vector of the value of a member is not a part of the type
/// of the value, so the compiler cannot infer it from the arguments and a call that does not name it reaches the
/// member of that vector type, which names the type itself and forwards the call to the marked member.
/// <para>The members of a vector type are emitted into two classes: <c>ex_*</c> adds them to the <c>math</c>
/// class, so the type of a vector of the value is named where the member is called and the other types of it are
/// still inferred, and <c>math_ex_*</c> adds them to one of the values, so the call reads like the member of the
/// matrix. The parameter a member of the value is called on is the first one of the marked member unless the
/// attribute names another one, see <see cref="ThisParameterProperty"/>. A member of a class cannot be
/// overloaded on its constraints alone, so every vector type of the same kind has a class of its own. The marked
/// member names the type of a vector of the value with a type parameter of its own, which is <c>TVector</c>
/// unless the attribute says otherwise, and the generated member is the one of the marked member with that type
/// spelled out and its body is the forwarding of the call. The marked member decides whether the generated
/// members reach the overload resolution with a priority of their own, see <see cref="PriorityProperty"/>.</para>
/// </summary>
[Generator]
public class VectorExtensionGenerator : IIncrementalGenerator
{
    /// <summary>The namespace of the generated members, which is the one of the <c>math</c> class.</summary>
    public const string Namespace = "Coplt.Mathematics";

    /// <summary>The name of the attribute a member is marked with.</summary>
    public const string Attribute = "VectorExtensionAttribute";

    /// <summary>The name of the type parameter of a marked member that names the type of a vector of the value.</summary>
    public const string DefaultVectorTypeParameter = "TVector";

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
    /// The format of the name of a type in a generated member: the namespace and the containing types of a type
    /// are spelled out, so a member names the type of the marked member itself instead of the one a name reaches
    /// through the usings of the generated file, and a type of the language is named by the keyword of it.
    /// </summary>
    private static readonly SymbolDisplayFormat TypeFormat = SymbolDisplayFormat.FullyQualifiedFormat
        .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)
        .WithMiscellaneousOptions(
            SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions | SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

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
    /// Generates the attribute a member of the <c>math</c> class is marked with.
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
        sb.AppendLine("/// Marks a member of the <c>math</c> class that names the type of a vector of the value it works on with");
        sb.AppendLine("/// a type parameter of its own. The generator emits the member of every vector type of the algebra that");
        sb.AppendLine("/// names the type itself and forwards the call to the marked member, so a call that does not name the type");
        sb.AppendLine("/// of that vector reaches the member of the vector type of the value.");
        sb.AppendLine("/// <para><see cref=\"ThisParameter\"/> names the parameter of the marked member that a member of");
        sb.AppendLine("/// the value is called on, the first one is the receiver when it is empty. The value of");
        sb.AppendLine("/// <see cref=\"OverloadResolutionPriority\"/> decides whether the generated members carry the");
        sb.AppendLine("/// priority of the same name, <see cref=\"NoPriority\"/> leaves it off.</para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("[global::System.AttributeUsage(global::System.AttributeTargets.Method)]");
        sb.AppendLine($"public sealed class {Attribute} : global::System.Attribute");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>The value of <see cref=\"OverloadResolutionPriority\"/> that emits no attribute</summary>");
        sb.AppendLine("    public const int NoPriority = int.MinValue;");
        sb.AppendLine();
        sb.AppendLine("    /// <param name=\"vectorTypeParameter\">The name of the type parameter of the marked member that names the type of a vector of the value</param>");
        sb.AppendLine($"    public {Attribute}(string vectorTypeParameter = \"{DefaultVectorTypeParameter}\") => VectorTypeParameter = vectorTypeParameter;");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>The name of the type parameter of the marked member that names the type of a vector of the value</summary>");
        sb.AppendLine("    public string VectorTypeParameter { get; }");
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
        sb.AppendLine("    /// same name. The priority puts the member of the vector type of a value in front of a member that only");
        sb.AppendLine("    /// reaches the value through a conversion.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <seealso cref=\"NoPriority\"/>");
        sb.AppendLine("    public int OverloadResolutionPriority { get; set; } = NoPriority;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Emits the members of every vector type of the algebra for a marked member.
    /// </summary>
    /// <param name="context">The context of the generation</param>
    /// <param name="method">The marked member</param>
    private static void Emit(SourceProductionContext context, IMethodSymbol method)
    {
        var vectorTypeParameter = VectorTypeParameter(method);
        // the member is marked for the type of a vector of the value, so it has to name it with a type parameter
        // of its own
        var vector = method.TypeParameters.FirstOrDefault(p => p.Name == vectorTypeParameter);
        if (vector is null) return;

        // the type of a vector of the value is spelled out by the generated member, the rest of the type
        // parameters are kept and inferred as they are
        var typeParameters = method.TypeParameters.Where(p => !SymbolEqualityComparer.Default.Equals(p, vector)).ToArray();
        var priority = Priority(method);
        var receiver = Receiver(method);

        foreach (var typ in Typ.Typs)
        {
            // only a number has arithmetic, a bool vector of an algebra has no arithmetic at all
            if (!typ.arith || typ.bol) continue;
            for (var size = 2; size <= 4; size++)
            {
                // only a vector whose register is wider than its value has a storage variant
                var variants = VectorGenShared.HasStorageVariant(typ, size) ? 2 : 1;
                for (var variant = 0; variant < variants; variant++)
                {
                    var storeVariant = variant == 1;
                    var name = VectorGenShared.VecName(typ, size, storeVariant);
                    context.AddSource(
                        $"{Namespace}.{method.Name}.{name}.g.cs",
                        SourceText.From(Gen(name, method, vectorTypeParameter, typeParameters, priority, receiver), Encoding.UTF8));
                }
            }
        }
    }

    /// <summary>
    /// Returns the name of the type parameter of a marked member that names the type of a vector of the value,
    /// which is the one the attribute names.
    /// </summary>
    /// <param name="method">The marked member</param>
    /// <returns>The name of the type parameter</returns>
    private static string VectorTypeParameter(IMethodSymbol method)
    {
        foreach (var attribute in method.GetAttributes())
        {
            if (attribute.AttributeClass?.Name != Attribute) continue;
            if (attribute.ConstructorArguments.Length == 0) continue;
            if (attribute.ConstructorArguments[0].Value is string name && name.Length != 0) return name;
        }

        return DefaultVectorTypeParameter;
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
    /// Generates the members of the vector type <paramref name="vector"/> that forward to <paramref name="method"/>.
    /// </summary>
    /// <param name="vector">The type of a vector of the value</param>
    /// <param name="method">The marked member</param>
    /// <param name="vectorTypeParameter">The name of the type parameter of the marked member that names the type of a vector of the value</param>
    /// <param name="typeParameters">The type parameters of the marked member beside the one of a vector</param>
    /// <param name="priority">The <c>OverloadResolutionPriority</c> of the generated members, <see cref="NoPriority"/> for none</param>
    /// <param name="receiver">The index of the parameter of the marked member a member of the value is called on, -1 for none</param>
    /// <returns>The file of the members</returns>
    private static string Gen(
        string vector, IMethodSymbol method, string vectorTypeParameter, ITypeParameterSymbol[] typeParameters,
        int priority, int receiver)
    {
        // the marked member names the type of a vector of the value, a generated member names the type itself, so
        // every type that carries the type parameter of it is written with the vector type
        string Type(ITypeSymbol type) => type.ToDisplayString(TypeFormat).Replace(vectorTypeParameter, vector);

        // every type parameter of the marked member beside the one of a vector is inferred from the arguments, the
        // one of a vector is named by the generated member
        var typeArguments = string.Join(", ", method.TypeParameters.Select(p => p.Name == vectorTypeParameter ? vector : p.Name));
        var typeParametersText = typeParameters.Length == 0 ? "" : $"<{string.Join(", ", typeParameters.Select(p => p.Name))}>";

        // the parameters of a member of the math class are the ones of the marked member
        var parameters = string.Join(", ", method.Parameters.Select(p => $"{RefKindText(p.RefKind)}{Type(p.Type)} {p.Name}"));

        // the parameters of a member of the value are the ones of the marked member as well, the parameter that
        // the member is called on is the first one of them and it is passed by value because the receiver of an
        // extension method of a type parameter cannot be an in parameter
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

        // the constraints of a type parameter of the marked member are the ones of the generated member, the type
        // of a vector of the value that they carry is spelled out as well
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

        // the member the math class carries, the type of a vector of the value is named where the member is called
        sb.AppendLine($"public static partial class ex_{vector}");
        sb.AppendLine("{");
        sb.AppendLine("    extension(math)");
        sb.AppendLine("    {");
        sb.AppendLine($"        /// <inheritdoc cref=\"{cref}\"/>");
        PriorityAttr("        ");
        sb.AppendLine($"        {Attr}");
        sb.AppendLine($"        public static {Type(method.ReturnType)} {method.Name}{typeParametersText}({parameters}){Constraints("            ")}");
        sb.AppendLine($"            => {target}({arguments});");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();

        // the member the value itself carries, it reads like the member of the value, a marked member whose
        // receiver names no parameter of it has no member of the value at all
        if (receiver >= 0)
        {
            sb.AppendLine($"public static partial class math_ex_{vector}");
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
