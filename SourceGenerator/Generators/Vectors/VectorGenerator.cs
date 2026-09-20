using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the vector structs described by <see cref="Typ"/>. The base members are emitted by
/// <c>Gen</c>, the swizzle members by <c>GenSwizzle</c> and the arithmetic members by <c>GenArith</c>,
/// every part lives in its own file.
/// </summary>
[Generator]
public partial class VectorGenerator : IIncrementalGenerator
{
    public const string VecNamespace = "Coplt.Experimental.Mathematics";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (var typ in Typ.Typs)
            {
                for (var size = 2; size <= 4; size++)
                {
                    ctx.AddSource(
                        $"{VecNamespace}.{typ.name}{size}.g.cs",
                        SourceText.From(Gen(typ, size), Encoding.UTF8));
                    // the swizzle members implement the swizzle interfaces, they are emitted into their own file
                    ctx.AddSource(
                        $"{VecNamespace}.{typ.name}{size}.swizzle.g.cs",
                        SourceText.From(GenSwizzle(typ, size), Encoding.UTF8));
                    // the arithmetic members implement the IVectorArithmetic interfaces, they are emitted
                    // into their own file so they stay separate from the members of the base type
                    if (typ.arith)
                    {
                        ctx.AddSource(
                            $"{VecNamespace}.{typ.name}{size}.arith.g.cs",
                            SourceText.From(GenArith(typ, size), Encoding.UTF8));
                    }
                }
            }
        });
    }

    private static string AsMethod(string simdComp) => simdComp switch
    {
        "float" => "AsSingle",
        "double" => "AsDouble",
        "int" => "AsInt32",
        "uint" => "AsUInt32",
        "long" => "AsInt64",
        "ulong" => "AsUInt64",
        _ => throw new InvalidOperationException($"unknown simd comp {simdComp}"),
    };
}
