using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that dispatch the value of the vector to a visitor: the width of the register of a
    /// vector and the count of its components are a part of its type, so the type itself is the only one that
    /// knows them and a caller that does not know the type of the vector reaches them by calling the member of a
    /// visitor that matches them. The value of the vector is handed to the member: a vector that keeps its value
    /// in a register hands the register over and a vector without a register hands the vector itself over, which
    /// the member of the visitor reaches the components of through the count of them. The member of the visitor
    /// builds the result out of the type of the vector, which the constraints of the members of the visitor
    /// name, so the same visitor serves a vector and a matrix. The members are implemented explicitly and they
    /// are emitted into the file of the base members of the vector. Every vector of a number type implements the
    /// interface, a mask does not: the constraint of a member of a visitor names the vector interface of a
    /// number and a mask implements the one of a bool instead.
    /// </summary>
    /// <param name="typ">The type of the component of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The members that dispatch the value of the vector, null when the vector implements no dispatch</returns>
    private static string? GenDispatch(Typ typ, int size, bool storeVariant)
    {
        // the members of the visitors name the number vector of the type, a mask is not one of them
        if (typ.bol) return null;

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        // the member of the visitor that matches the kind of the value: the width of the register of the vector
        // when it keeps its value in one, the count of its components when it has no register
        var one = reg == 0
            ? $"V.AcceptVector{size}<{type}, {scalar}>(self)"
            : $"V.AcceptVector<{type}, {scalar}>(self.vector)";
        var two = reg == 0
            ? $"V.AcceptVector{size}<{type}, {scalar}>(a, b)"
            : $"V.AcceptVector<{type}, {scalar}>(a.vector, b.vector)";
        var three = reg == 0
            ? $"V.AcceptVector{size}<{type}, {scalar}>(a, b, c)"
            : $"V.AcceptVector<{type}, {scalar}>(a.vector, b.vector, c.vector)";
        // the interface of a member that takes a component of the value beside it names the type of the
        // component, so the member of the visitor is not named with the type of it again
        var oneComponent = reg == 0
            ? $"V.AcceptVector{size}<{type}>(a, b)"
            : $"V.AcceptVector<{type}>(a.vector, b)";
        var twoComponents = reg == 0
            ? $"V.AcceptVector{size}<{type}>(a, b, c)"
            : $"V.AcceptVector<{type}>(a.vector, b, c)";

        var sb = new StringBuilder();

        sb.AppendLine("    #region dispatch");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {type} Algebras.Generics.INumberAlgebraDispatch<{type}>.Visit_Self<V>(in {type} self)");
        sb.AppendLine($"        => {one};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {type} Algebras.Generics.INumberAlgebraDispatch<{type}>.Visit_Self<V>(in {type} a, in {type} b)");
        sb.AppendLine($"        => {two};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {type} Algebras.Generics.INumberAlgebraDispatch<{type}>.Visit_Self<V>(in {type} a, in {type} b, in {type} c)");
        sb.AppendLine($"        => {three};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {type} Algebras.Generics.INumberAlgebraDispatch<{type}, {scalar}>.Visit_Self<V>(in {type} a, {scalar} b)");
        sb.AppendLine($"        => {oneComponent};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {type} Algebras.Generics.INumberAlgebraDispatch<{type}, {scalar}>.Visit_Self<V>(in {type} a, {scalar} b, {scalar} c)");
        sb.AppendLine($"        => {twoComponents};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {scalar} Algebras.Generics.INumberAlgebraDispatch<{type}, {scalar}>.Visit_Scalar<V>(in {type} self)");
        sb.AppendLine($"        => {one};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    static {scalar} Algebras.Generics.INumberAlgebraDispatch<{type}, {scalar}>.Visit_Scalar<V>(in {type} a, in {type} b)");
        sb.AppendLine($"        => {two};");
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();
        return sb.ToString();
    }
}
