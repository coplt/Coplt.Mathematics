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
    /// writes, the member is left out for a 64 bit register because it is exactly as wide as the value and has
    /// no lane to mask. The lanes that are beyond the value are the padding lanes of the register, the width of
    /// them is told by <c>HavePaddingLanes</c> and <c>PaddingLanesMask</c>. The members are emitted into their
    /// own file.
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
            // the mask that keeps the lanes of the register that are beyond the value at zero, it is all bits
            // set when the value fills the register of the vector and a register that is wider than the value
            // is masked out of the components of the vector itself
            var lanes = VectorGenShared.Lanes(typ, size, storeVariant);
            var mask = $"{vecName}.Create(" +
                       VectorGenShared.Join(lanes, i => i < size
                           ? typ.size == 4 ? "-1" : "-1L"
                           : typ.size == 4 ? "0" : "0L") +
                       $").{AsMethod(comp)}().As<{comp}, byte>()";
            // the lanes that are beyond the value are kept at zero by the ctor of the vector, they are the
            // padding lanes of the register and the interface of the width describes them
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    public static bool HavePaddingLanes");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {(VectorGenShared.PadLanes(typ, size, storeVariant) > 0 ? "true" : "false")};");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    public static {vecName}<byte> PaddingLanesMask");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {mask};");
            sb.AppendLine("    }");
            sb.AppendLine();
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
            // a 64 bit register is exactly as wide as the value of the vector it keeps, so there is no padding
            // lane to leave as they are and the interface of the width does not declare the member
            if (reg != 64)
            {
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    public static {type} UnsafeFromUnderlying({vecName}<byte> vector) => " +
                              $"new() {{ vector = vector.As<byte, {comp}>() }};");
            }
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
