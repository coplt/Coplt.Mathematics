using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Coplt.Analyzers.Generators.Templates;
using Coplt.Analyzers.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

[Generator]
public class MathExGenerator : IIncrementalGenerator
{
    public static RefKind GetRefKind(ParameterSyntax p)
    {
        var kind = RefKind.None;
        foreach (var modifier in p.Modifiers)
        {
            if (modifier.IsKind(SyntaxKind.InKeyword))
            {
                kind = RefKind.In;
            }
            if (modifier.IsKind(SyntaxKind.OutKeyword))
            {
                kind = RefKind.Out;
            }
            if (modifier.IsKind(SyntaxKind.ReadOnlyKeyword))
            {
                kind = RefKind.RefReadOnlyParameter;
            }
            if (modifier.IsKind(SyntaxKind.RefKeyword))
            {
                kind = RefKind.Ref;
            }
        }
        return kind;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var root_path = context.AnalyzerConfigOptionsProvider.Select(static (a, _) => { return a.GlobalOptions.TryGetValue("build_property.projectdir", out var v) ? v : null; });
        var sources = context.SyntaxProvider.ForAttributeWithMetadataName("Coplt.Mathematics.ExAttribute",
                static (syntax, _) => syntax is ClassDeclarationSyntax,
                static (ctx, _) =>
                {
                    var diagnostics = new List<Diagnostic>();
                    var syntax = (TypeDeclarationSyntax)ctx.TargetNode;
                    var symbol = (INamedTypeSymbol)ctx.TargetSymbol;
                    var nullable = ctx.SemanticModel.Compilation.Options.NullableContextOptions;

                    var src_name = symbol.Name;

                    var ex_to = symbol.GetAttributes()
                        .FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString() == "Coplt.Mathematics.ExToAttribute");

                    if (ex_to == null) return default;

                    symbol = (INamedTypeSymbol)ex_to.ConstructorArguments[0].Value!;
                    var rawFullName = symbol.ToDisplayString();
                    var nameWraps = symbol.WrapNames();
                    var nameWrap = symbol.WrapName();

                    var usings = new HashSet<string>();
                    Utils.GetUsings(syntax, usings);
                    var genBase = new GenBase(rawFullName, nullable, usings, nameWraps, nameWrap);

                    return (syntax, symbol.Name, src_name, genBase, diagnostics: AlwaysEq.Create(diagnostics));
                }
            )
            .Where(static a => a.syntax?.GetLocation().SourceTree != null)
            .Combine(root_path)
            .Select(static (input, _) =>
            {
                var ((syntax, name, src_name, genBase, diagnostics), root_path) = input;
                var loc = syntax.GetLocation();
                var src_path = loc.SourceTree?.FilePath;
                if (root_path == null || src_path == null) return default;
                var path = Uri.UnescapeDataString(new Uri(root_path).MakeRelativeUri(new Uri(src_path)).ToString().Replace('/', Path.DirectorySeparatorChar));
                return (syntax, name, src_name, genBase, path, loc.SourceSpan.Start, diagnostics);
            })
            .Where(static a => a.syntax != null)
            .Select(static (input, _) =>
            {
                var (syntax, name, src_name, genBase, path, start, diagnostics) = input;
                var fns = syntax.Members
                    .Where(static m => m is MethodDeclarationSyntax)
                    .Cast<MethodDeclarationSyntax>()
                    .Where(static m => m.ParameterList.Parameters.Count > 0)
                    .Where(static m => m.Modifiers.Any(static mn => mn.IsKind(SyntaxKind.StaticKeyword)))
                    .Where(static m => m.Modifiers.Any(static mn => mn.IsKind(SyntaxKind.PublicKeyword)))
                    .Where(static m => m.ParameterList.Parameters
                        .SelectMany(static a => a.AttributeLists)
                        .SelectMany(static a => a.Attributes)
                        .Any(static a => a.Name is IdentifierNameSyntax { Identifier.ValueText: "This" })
                    )
                    .Select(static m =>
                    {
                        var raw_args = m.ParameterList.Parameters
                            .Select(p => new Arg(p.Identifier.ValueText, p.Type?.ToString() ?? "void", GetRefKind(p)))
                            .ToImmutableArray();
                        var args = m.ParameterList.Parameters.Select((p, i) =>
                            {
                                var self = false;
                                var order = uint.MaxValue;
                                foreach (var attribute in p.AttributeLists.SelectMany(static a => a.Attributes))
                                {
                                    if (attribute is { Name: IdentifierNameSyntax { Identifier.ValueText: "This" } })
                                    {
                                        self = true;
                                    }
                                    if (attribute is
                                        {
                                            Name: IdentifierNameSyntax { Identifier.ValueText: "Arg" },
                                            ArgumentList.Arguments: [{ Expression: LiteralExpressionSyntax { } }],
                                        })
                                    {
                                        // todo
                                    }
                                }
                                var arg = new Arg(p.Identifier.ValueText, p.Type?.ToString() ?? "void", GetRefKind(p));
                                return (arg, self, order, i);
                            })
                            .OrderBy(static a => a.self ? 0 : 1)
                            .ThenBy(static a => a.order)
                            .ThenBy(static a => a.i)
                            .Select(static a => a.arg)
                            .ToImmutableArray();
                        var fn = new Fn(m.Identifier.ValueText, m.ReturnType.ToString(), args, raw_args);
                        return fn;
                    })
                    .ToImmutableArray();
                return (fns, name, src_name, genBase, path, start, diagnostics);
            });
        context.RegisterSourceOutput(sources, static (ctx, input) =>
        {
            var (fns, name, src_name, genBase, path, start, diagnostics) = input;
            if (diagnostics.Value.Count > 0)
            {
                foreach (var diagnostic in diagnostics.Value)
                {
                    ctx.ReportDiagnostic(diagnostic);
                }
            }
            var code = new MathExTemplate(genBase, name, src_name, fns).Gen();
            var sourceText = SourceText.From(code, Encoding.UTF8);
            var rawSourceFileName = genBase.FileFullName;
            var sourceFileName = $"{rawSourceFileName}.[for {path} {start}].ex.g.cs";
            ctx.AddSource(sourceFileName, sourceText);
        });
    }
}
