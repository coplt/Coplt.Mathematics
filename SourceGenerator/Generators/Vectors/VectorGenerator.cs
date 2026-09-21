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
                    // a simd backed 2 or 3 component vector also has a storage variant beside the regular one
                    var variants = VectorGenShared.HasStorageVariant(typ, size) ? 2 : 1;
                    for (var variant = 0; variant < variants; variant++)
                    {
                        var storeVariant = variant == 1;
                        var name = VectorGenShared.VecName(typ, size, storeVariant);
                        ctx.AddSource(
                            $"{VecNamespace}.{name}.g.cs",
                            SourceText.From(Gen(typ, size, storeVariant), Encoding.UTF8));
                        // the swizzle members implement the swizzle interfaces, they are emitted into their own file
                        ctx.AddSource(
                            $"{VecNamespace}.{name}.swizzle.g.cs",
                            SourceText.From(GenSwizzle(typ, size, storeVariant), Encoding.UTF8));
                        // the arithmetic members implement the IVectorArithmetic interfaces, they are emitted
                        // into their own file so they stay separate from the members of the base type
                        if (typ.arith)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.arith.g.cs",
                                SourceText.From(GenArith(typ, size, storeVariant), Encoding.UTF8));
                        }
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
