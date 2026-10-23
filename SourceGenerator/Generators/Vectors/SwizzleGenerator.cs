using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the members that name a component of a vector more than once and reach the vector itself: a
/// combination of that kind cannot be written, because two of its components are the same component of the
/// vector, so the member of it is read only and the type of the vector does not have to carry it.
/// <para>The members are emitted as the extension members of a vector of a number of components: every vector
/// of that many components reaches them through the interface of the count of its components, which keeps a
/// name that is not a vector of another count from reaching them as well. The member of a combination dispatches
/// the value of the vector to the visitor of the combination, which is shared by every vector of the same count
/// of components: the visitor reaches the register of the value when the vector keeps it in one and the
/// components of it when it does not, and the type of a component of the value is a type parameter of the member
/// of the visitor, so the same value serves every kind of a component.</para>
/// <para><see cref="VectorGenerator"/> emits the same combination as a read only member of the type of every
/// vector as well, so the combinations this generator carries are left out of it, see <c>GenSwizzle</c>. A
/// combination that names every component of the vector once can be written, so it stays a member of the type
/// and this generator does not emit it.</para>
/// <para>The visitors of the combinations are emitted into a namespace of their own beside the other
/// implementations of the algebra, so the visitors that this generator emits do not sit beside the ones that
/// are written by hand.</para>
/// </summary>
[Generator]
public class SwizzleGenerator : IIncrementalGenerator
{
    /// <summary>The namespace of the extension members, which is the one of the vector types.</summary>
    public const string Namespace = "Coplt.Mathematics";

    /// <summary>
    /// The namespace of the visitors of the combinations, a namespace of its own beside the one of the other
    /// implementations of the algebra.
    /// </summary>
    public const string ImplNamespace = "Coplt.Mathematics.Implements.Swizzle";

    /// <summary>The attribute of a member, it is inlined into its caller.</summary>
    private const string Attr = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            for (var size = 2; size <= 4; size++)
            {
                ctx.AddSource(
                    $"{Namespace}.ex_swizzle{size}.g.cs",
                    SourceText.From(GenExtension(size), Encoding.UTF8));
                ctx.AddSource(
                    $"{ImplNamespace}.impl_swizzle{size}.g.cs",
                    SourceText.From(GenVisitors(size), Encoding.UTF8));
            }
        });
    }

    /// <summary>
    /// Returns the combinations of <paramref name="size"/> components that name a component of the vector more
    /// than once, which are the combinations that cannot be written. The digits of a combination are the indices
    /// of the components of the vector, so the combination <c>010</c> is <c>xyx</c>.
    /// </summary>
    /// <param name="size">The number of components of the vector</param>
    /// <returns>The combinations</returns>
    private static List<int[]> Combinations(int size)
    {
        var result = new List<int[]>();
        var total = 1;
        for (var i = 0; i < size; i++) total *= size;
        for (var combination = 0; combination < total; combination++)
        {
            var digits = new int[size];
            var rest = combination;
            for (var i = size - 1; i >= 0; i--)
            {
                digits[i] = rest % size;
                rest /= size;
            }

            var distinct = true;
            for (var i = 0; i < size && distinct; i++)
            {
                for (var j = i + 1; j < size; j++)
                {
                    if (digits[i] != digits[j]) continue;
                    distinct = false;
                    break;
                }
            }

            if (!distinct) result.Add(digits);
        }

        return result;
    }

    /// <summary>
    /// Returns the name of a combination, which is the name of every component of it in the order of the digits
    /// of the combination.
    /// </summary>
    /// <param name="digits">The digits of the combination</param>
    /// <param name="names">The name of a single component</param>
    /// <returns>The name of the combination</returns>
    private static string Name(int[] digits, string[] names) =>
        VectorGenShared.Join(digits.Length, i => names[digits[i]], "");

    /// <summary>
    /// Returns the indices of the lanes of the register of a vector for a combination: every lane of the
    /// combination reads the component of the vector the digit of it names, and every lane the register has
    /// beside the combination is a padding lane, which reads the last lane of the register.
    /// <para>The padding lanes of the value of a vector are zero, so the padding lanes of the result are zero
    /// as well: the member of a combination keeps the invariant of the value without masking anything.</para>
    /// </summary>
    /// <param name="digits">The digits of the combination</param>
    /// <param name="lanes">The number of lanes of the register</param>
    /// <param name="suffix">The suffix of a literal of the type of a lane</param>
    /// <returns>The arguments of the creation of the indices</returns>
    private static string Index(int[] digits, int lanes, string suffix) =>
        VectorGenShared.Join(lanes, i => i < digits.Length ? $"{digits[i]}{suffix}" : $"{lanes - 1}{suffix}");

    /// <summary>
    /// Generates the extension members that name a component of a vector of <paramref name="size"/> components
    /// more than once.
    /// </summary>
    /// <param name="size">The number of components of the vector</param>
    /// <returns>The members of the extension</returns>
    private static string GenExtension(int size)
    {
        var patterns = Combinations(size);

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using Coplt.Mathematics.Algebras.Generics.Dispatch;");
        sb.AppendLine($"using {ImplNamespace};");
        sb.AppendLine("using Algebras = Coplt.Mathematics.Algebras;");
        sb.AppendLine();
        sb.AppendLine($"namespace {Namespace};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// The members of a vector of {size} components that name a component of it more than once");
        sb.AppendLine($"/// <para>A combination of that kind cannot be written, so the member of it is read only, and the value");
        sb.AppendLine($"/// of the result is the vector itself, so the type of the vector does not carry the member: it is");
        sb.AppendLine($"/// reached through the interface of the count of the components of the vector, which keeps a name of");
        sb.AppendLine($"/// another count of components and a name that is not an algebra value from reaching it. The member of");
        sb.AppendLine($"/// every other shape of a combination stays a member of the type of the vector, it is emitted by");
        sb.AppendLine($"/// <c>VectorGenerator</c> beside <c>GenSwizzle</c>.</para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public static class ex_swizzle{size}");
        sb.AppendLine("{");
        sb.AppendLine($"    extension<T>(T self)");
        sb.AppendLine($"        where T : unmanaged, IAlgebraDispatch<T>, Algebras.IVector{size}<T>");
        sb.AppendLine("    {");

        // a member of the extension starts on the line after the one the previous member ended on
        var first = true;
        foreach (var digits in patterns)
        {
            var name = Name(digits, Typ.xyzw);
            var colorName = Name(digits, Typ.rgba);

            if (!first) sb.AppendLine();
            first = false;

            sb.AppendLine($"        /// <summary>The <c>{name}</c> swizzle of the vector, it is the vector itself</summary>");
            sb.AppendLine($"        public T {name}");
            sb.AppendLine("        {");
            sb.AppendLine($"            {Attr}");
            sb.AppendLine($"            get => T.Self<impl_swizzle_{name}>(self);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        /// <summary>The <c>{colorName}</c> swizzle of the vector, it is the same swizzle as <c>{name}</c></summary>");
            sb.AppendLine($"        public T {colorName}");
            sb.AppendLine("        {");
            sb.AppendLine($"            {Attr}");
            sb.AppendLine($"            get => T.Self<impl_swizzle_{name}>(self);");
            sb.AppendLine("        }");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Generates the visitors of the combinations of <paramref name="size"/> components.
    /// </summary>
    /// <param name="size">The number of components of the vector</param>
    /// <returns>The visitors</returns>
    private static string GenVisitors(int size)
    {
        var patterns = Combinations(size);

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using System.Runtime.Intrinsics;");
        sb.AppendLine("using Coplt.Mathematics.Algebras.Generics.Dispatch;");
        sb.AppendLine();
        sb.AppendLine($"namespace {ImplNamespace};");

        foreach (var digits in patterns) Visitor(sb, size, digits);

        return sb.ToString();
    }

    /// <summary>
    /// Emits the visitor of a single combination.
    /// <para>The visitor reaches the value of a vector that keeps it in a register through the member that
    /// matches the width of the register, which shuffles the lanes of it, and the value of every other vector
    /// through the member of the count of the components of it, which reads the components the combination names.
    /// A register of 128 bits keeps 4 lanes of a component of 4 bytes and 2 lanes of a component of 8 bytes,
    /// which only a vector of 2 components reaches, and a register of 256 bits only ever keeps a component of 8
    /// bytes, so the member of the 128 bit register of a vector of 3 or more components names the type of a lane
    /// by itself.</para>
    /// </summary>
    /// <param name="sb">The builder of the file</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="digits">The digits of the combination</param>
    private static void Visitor(StringBuilder sb, int size, int[] digits)
    {
        var name = Name(digits, Typ.xyzw);
        var visitor = $"impl_swizzle_{name}";

        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// The <c>{name}</c> swizzle of the value of a vector of {size} components");
        sb.AppendLine("/// <para>The combination names a component of the vector more than once, so the value of the result is");
        sb.AppendLine("/// the vector itself: it is read out of the register of the value when the vector keeps it in one and");
        sb.AppendLine("/// out of the components of it when it does not.</para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"internal struct {visitor} : IAlgebraVisitor_T_T<{visitor}>");
        sb.AppendLine("{");

        if (size == 2)
        {
            // a vector of 2 components of 4 bytes is padded to the 4 lanes of a 128 bit register, the 2 lanes of
            // the register of a vector of 2 components of 8 bytes are the value itself
            sb.AppendLine($"    {Attr}");
            sb.AppendLine($"    static TVector IAlgebraVisitor_T_T<{visitor}>.Simd_Any<TVector, TScalar>(Vector128<TScalar> vector)");
            sb.AppendLine("    {");
            sb.AppendLine("        if (Unsafe.SizeOf<TScalar>() == 8)");
            sb.AppendLine($"            return TVector.UnsafeFromUnderlying(Vector128.Shuffle(vector.AsInt64(), Vector128.Create({Index(digits, 2, "L")})).AsByte());");
            sb.AppendLine($"        return TVector.UnsafeFromUnderlying(Vector128.Shuffle(vector.AsInt32(), Vector128.Create({Index(digits, 4, "")})).AsByte());");
            sb.AppendLine("    }");
        }
        else
        {
            // a vector of 3 or more components keeps 4 lanes in every register it is backed by: a component of 4
            // bytes in a register of 128 bits and a component of 8 bytes in one of 256 bits
            sb.AppendLine($"    {Attr}");
            sb.AppendLine($"    static TVector IAlgebraVisitor_T_T<{visitor}>.Simd_Any<TVector, TScalar>(Vector128<TScalar> vector)");
            sb.AppendLine($"        => TVector.UnsafeFromUnderlying(Vector128.Shuffle(vector.AsInt32(), Vector128.Create({Index(digits, 4, "")})).AsByte());");
            sb.AppendLine();
            sb.AppendLine($"    {Attr}");
            sb.AppendLine($"    static TVector IAlgebraVisitor_T_T<{visitor}>.Simd_Any<TVector, TScalar>(Vector256<TScalar> vector)");
            sb.AppendLine($"        => TVector.UnsafeFromUnderlying(Vector256.Shuffle(vector.AsInt64(), Vector256.Create({Index(digits, 4, "L")})).AsByte());");
        }

        sb.AppendLine();
        sb.AppendLine($"    {Attr}");
        sb.AppendLine($"    static TVector IAlgebraVisitor_T_T<{visitor}>.Vector{size}_Any<TVector, TScalar>(TVector vector)");
        sb.AppendLine($"        => TVector.Create({VectorGenShared.Join(size, i => $"TVector.get_{Typ.xyzw[digits[i]]}(vector)")});");
        sb.AppendLine("}");
    }
}
