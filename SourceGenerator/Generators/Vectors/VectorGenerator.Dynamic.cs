using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that dispatch the kind of the vector to a visitor: the width of the register of a
    /// vector and the count of its components are a part of its type, so the type itself is the only one that
    /// knows them and a caller that does not know the type of the vector reaches them by calling the member of
    /// a visitor that matches them. The value of the vector is handed to the member: it receives the register of
    /// a vector that keeps its value in one and the vector itself when it has no register, and the visitor that
    /// returns a vector builds it out of the type of the vector, which the constraints of its members name.
    /// Every vector of a number type implements the interface, a mask does not: the constraint of a member of a
    /// visitor names the vector interface of a number and a mask implements the one of a bool instead.
    /// </summary>
    /// <param name="typ">The type of the component of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The members that dispatch the kind of the vector, null when the vector implements no dispatch</returns>
    private static string? GenDynamic(Typ typ, int size, bool storeVariant)
    {
        // the members of both visitors name the number vector of the type, a mask is not one of them
        if (typ.bol) return null;

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        // a vector without a register hands the vector itself to the visitor, the other ones hand it the
        // register they keep their value in
        var underlying = reg == 0
            ? $"V.AcceptSoft<{type}, {scalar}>(self)"
            : $"V.Accept<{type}, {scalar}>(self.vector)";
        var dimension = $"V.AcceptVector{size}<{type}, {scalar}>(self)";

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine($"    IDynamicVector<{type}>");
        sb.AppendLine("{");
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static R VisitUnderlying<V, R>(in {type} self) where V : IVectorUnderlyingVisitor<R> => " +
                      $"{underlying};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static R VisitDimension<V, R>(in {type} self) where V : IVectorDimensionVisitor<R> => " +
                      $"{dimension};");
        sb.AppendLine();
        // a visitor that returns a vector builds the result itself, the type of the vector is reachable to it
        // through the constraints of its own members
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {type} VisitUnderlyingReturnVector<V>(in {type} self) " +
                      $"where V : IVectorUnderlyingVisitorReturnVector => {underlying};");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {type} VisitDimensionReturnVector<V>(in {type} self) " +
                      $"where V : IVectorDimensionVisitorReturnVector => {dimension};");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
