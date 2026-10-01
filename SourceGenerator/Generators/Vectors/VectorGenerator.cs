using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the vector structs described by <see cref="Typ"/>. The base members are emitted by
/// <c>Gen</c>, which reaches the members that dispatch the value of the vector through <c>GenDispatch</c>,
/// the members that reach the raw bits of the value by <c>GenUnderlying</c>, the members that
/// create a vector out of another one by <c>GenCtor</c>, the members that replace
/// the components of the vector by <c>GenReplace</c>, the members of the legacy insert api by <c>GenInsert</c>,
/// the members that combine two vectors by <c>GenShuffle</c> and the helper that shuffles a vector without a
/// register by <c>GenShuffleSoft</c>, the swizzle members by <c>GenSwizzle</c>, the
/// arithmetic members by <c>GenArith</c>, the as members and the
/// conversions between a vector and its storage variant by <c>GenAs</c>, the conversions between two vectors by
/// <c>GenConv</c> and the as members of the math class by <c>GenMathAs</c>. The members that replace the components, the ones of the legacy
/// insert api, the swizzle members and the shuffle members are emitted into a file of their own, the members of
/// every other part are emitted into the file of the type itself, so the constants of a floating point kind and
/// the ones of the ieee 754 standard are a part of the base members as well, see <c>Initialize</c>. The converter of a
/// vector is not generated: every vector of a count of components carries the same generic converter, which
/// names the type
/// of the vector and the type of a single component of it. The converters live in <c>Coplt.Mathematics.Json</c>
/// so that the types of <c>System.Text.Json</c> are not a part of the surface of <c>Coplt.Mathematics</c>.
/// </summary>
[Generator]
public partial class VectorGenerator : IIncrementalGenerator
{
    public const string VecNamespace = "Coplt.Mathematics";

    /// <summary>
    /// The namespace the json converters of the vectors are emitted into, the converters are kept out of
    /// <c>Coplt.Mathematics</c> so that the types of <c>System.Text.Json</c> are not a part of its surface.
    /// </summary>
    public const string JsonNamespace = "Coplt.Mathematics.Json";

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
                        // the storage variant of a vector is a plain holder of the components of it: it keeps the
                        // fields of them, the constructors, the conversions, the equality and the comparison, the
                        // format of the value and the interfaces the json converter of it is constrained by, every
                        // other member of the kind of the vector lives on the regular vector of it alone
                        if (storeVariant)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.g.cs",
                                SourceText.From(Gen(typ, size, storeVariant), Encoding.UTF8));
                            continue;
                        }

                        // the members of the type that are a part of the value itself are emitted into one file,
                        // which is the file of the type, see Gen
                        ctx.AddSource(
                            $"{VecNamespace}.{name}.g.cs",
                            SourceText.From(Gen(typ, size, storeVariant), Encoding.UTF8));
                        // the as member of the kind of the vector is a member of the math class as well, it
                        // forwards the call to the value itself, the forwarding of every target vector lives in
                        // the extension class of its own
                        var mathAs = GenMathAs(typ, size, storeVariant);
                        if (mathAs != null)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.ex_{name}.g.cs",
                                SourceText.From(mathAs, Encoding.UTF8));
                        }

                        // the members that replace the components of the vector implement the IVectorReplace
                        // interfaces, they are long enough to be kept in a file of their own
                        ctx.AddSource(
                            $"{VecNamespace}.{name}.replace.g.cs",
                            SourceText.From(GenReplace(typ, size, storeVariant), Encoding.UTF8));
                        // the members of the legacy insert api implement the IVector2Insert and the
                        // IVector3Insert interfaces, they forward to the members of the create of the longer
                        // vectors and a vector of 4 components has no member of them
                        var insert = GenInsert(typ, size, storeVariant);
                        if (insert != null)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.insert.g.cs",
                                SourceText.From(insert, Encoding.UTF8));
                        }

                        // the swizzle members implement the swizzle interfaces, they are kept in a file of their own
                        ctx.AddSource(
                            $"{VecNamespace}.{name}.swizzle.g.cs",
                            SourceText.From(GenSwizzle(typ, size, storeVariant), Encoding.UTF8));
                        // the shuffle members implement the IVectorShuffle interface, they combine two vectors of
                        // 4 components and a shorter vector has no member of them
                        var shuffle = GenShuffle(typ, size, storeVariant);
                        if (shuffle != null)
                        {
                            ctx.AddSource(
                                $"{VecNamespace}.{name}.shuffle.g.cs",
                                SourceText.From(shuffle, Encoding.UTF8));
                        }
                    }
                }
            }

            // the helper that shuffles a vector that has no register is the same for every vector type, it is
            // emitted once for all of them
            ctx.AddSource(
                $"{VecNamespace}.shuffle_soft.g.cs",
                SourceText.From(GenShuffleSoft(), Encoding.UTF8));
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
