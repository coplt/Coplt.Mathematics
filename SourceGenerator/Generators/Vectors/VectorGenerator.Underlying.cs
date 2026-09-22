using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that make the bits of the value of the vector reachable as a raw vector of bytes.
    /// The width of the register of the vector decides the interface: a vector without a register is only
    /// marked, every other one implements the interface of the width of its register. A value that is built
    /// from raw bits goes through the constructor of the vector, which masks the lanes that are beyond the
    /// value, and <c>UnsafeFromUnderlying</c> writes the register itself for the code that knows the value it
    /// writes. The members are emitted into their own file.
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The members of the underlying interfaces of the vector</returns>
    private static string GenUnderlying(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var vecName = $"Vector{reg}";
        var comp = typ.simdComp;
        // the value of a 2 component storage variant is kept behind a raw ulong field, its ctor does not take
        // a register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var attr = "[MethodImpl(256)]";

        var sb = new StringBuilder();

        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type} :");
        // a vector without a register has no bits to reach, its interface only declares that
        sb.AppendLine(reg == 0 ? "    IVectorSoftUnderlying" : $"    IVector{reg}Underlying<{type}>");
        sb.AppendLine("{");

        if (reg != 0)
        {
            // the bits of the value are the bits of its register, the ctor of the vector masks the lanes that
            // are beyond the value and writing the register itself leaves them as they are
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {vecName}<byte> GetUnderlying(in {type} self) => self.vector.As<{comp}, byte>();");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} FromUnderlying({vecName}<byte> vector) => " +
                          (v64
                              ? $"new() {{ vector = vector.As<byte, {comp}>() }};"
                              : $"new(vector.As<byte, {comp}>());"));
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} UnsafeFromUnderlying({vecName}<byte> vector) => " +
                          $"new() {{ vector = vector.As<byte, {comp}>() }};");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
