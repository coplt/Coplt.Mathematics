using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the vector structs described by <see cref="Typ"/>. The base members are emitted by
/// <c>Gen</c>, the swizzle members by <c>GenSwizzle</c>, the arithmetic members by <c>GenArith</c>, the
/// integer members by <c>GenInt</c>, the floating point members by <c>GenFloat</c>, the ieee 754 members by
/// <c>GenIeee</c>, the members that implement the interfaces by <c>GenIface</c>, the as members and the
/// conversions between a vector and its storage variant by <c>GenAs</c>, the conversions between two vectors by
/// <c>GenConv</c> and the as members of the math class by <c>GenMathAs</c>, every part lives in its own file.
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

                        // the integer members implement the IVectorInteger interfaces, they are emitted into
                        // their own file as well
                        if (typ.arith && typ.i)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.int.g.cs",
                                SourceText.From(GenInt(typ, size, storeVariant), Encoding.UTF8));
                        }

                        // the members that implement the interfaces are the forwarding of the members of the
                        // vector, they are emitted into their own file as well
                        if (typ.arith)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.iface.g.cs",
                                SourceText.From(GenIface(typ, size, storeVariant), Encoding.UTF8));
                        }

                        // the floating point members implement the IVectorFloatingPoint interface, they are
                        // emitted into their own file as well
                        if (typ.arith && typ.f)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.float.g.cs",
                                SourceText.From(GenFloat(typ, size, storeVariant), Encoding.UTF8));
                            // the ieee 754 members implement the IVectorFloatingPointIeee754 interfaces
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.ieee.g.cs",
                                SourceText.From(GenIeee(typ, size, storeVariant), Encoding.UTF8));
                        }

                        // the as members and the conversion between the vector and its storage variant are
                        // members of the vector, they are emitted into their own file as well, a type that
                        // has neither of them has no file at all
                        var cast = GenAs(typ, size, storeVariant);
                        if (cast != null)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.as.g.cs",
                                SourceText.From(cast, Encoding.UTF8));
                            // the as member of the kind of the vector is also a member of the math class, it
                            // forwards the call to the vector itself, the forwarding of every target vector
                            // lives in the extension class of its own
                            ctx.AddSource(
                                $"{VecNamespace}.ex_{name}.g.cs",
                                SourceText.From(GenMathAs(typ, size, storeVariant), Encoding.UTF8));
                        }

                        // the conversions of the vector into the vectors of the same size that have another
                        // component type are emitted into their own file as well, a type that has no
                        // conversion has no file at all
                        var conv = GenConv(typ, size, storeVariant);
                        if (conv != null)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.conv.g.cs",
                                SourceText.From(conv, Encoding.UTF8));
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
