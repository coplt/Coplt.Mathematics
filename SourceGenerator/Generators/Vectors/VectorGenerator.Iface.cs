using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that implement the interfaces of the vector described by <paramref name="typ"/>.
    /// The vector keeps the member of every operation on itself, so a caller of it does not have to name the
    /// type of the vector at all, and the interface declares the same operations as static members that take
    /// the vector as a parameter, which is the form a caller that only knows a type parameter can use. Every
    /// member below is the forwarding of the one of the vector to the other one, they are emitted into the file
    /// of the value itself, grouped by the legacy interface of the family of the member.
    /// <para>The parameter list of every member follows the legacy implementation, so the parameter that the
    /// member of the vector is called on is the first one of it, beside the few members whose legacy form has
    /// it in another position</para>
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The file</returns>
    private static string GenIface(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;

        // the members that implement the interfaces, the kind tells which vector implements them:
        // a = every arithmetic vector, 3 = the 3 component one, f = the floating point one,
        // i = the integer one, u = the integer one without a sign
        var members = new List<(char Kind, string Member)>
        {
            #region IVectorFloatingPointIeee754

            // the legacy form of the step puts the threshold first and the value last
            ('f', "{type} step(in {type} threshold, in {type} a) => a.step(threshold);"),
            // the legacy form of the safe projection carries the default as an optional parameter, which the
            // caller of a type parameter can drop, so there is a single member for the two cases of it
            ('f', "{type} face_forward(in {type} a, in {type} i, in {type} ng) => a.face_forward(i, ng);"),
            ('f', "{type} sinh(in {type} a) => a.sinh();"),
            ('f', "{type} cosh(in {type} a) => a.cosh();"),
            ('f', "{type} tanh(in {type} a) => a.tanh();"),
            ('f', "{type} asinh(in {type} a) => a.asinh();"),
            ('f', "{type} acosh(in {type} a) => a.acosh();"),
            ('f', "{type} atanh(in {type} a) => a.atanh();"),
            ('f', "{type} chg_sign(in {type} a, in {type} sign) => a.chg_sign(sign);"),

            #endregion

            #region IVectorInteger

            ('i', "{type} is_pow2(in {type} a) => a.is_pow2();"),
            // the rounding up to the next power of two is only meaningful for a vector that has no sign
            ('u', "{type} up2pow2(in {type} a) => a.up2pow2();"),

            #endregion
        };

        var sb = new StringBuilder();

        // the members are the forwarding of the legacy interfaces of the kind of the value, so every family of
        // them is named by the region of the interface it implements
        var family = "";
        var first = true;
        foreach (var (kind, member) in members)
        {
            if (!Implements(typ, size, kind)) continue;
            var name = Family(kind);
            if (name != family)
            {
                if (family.Length != 0) sb.AppendLine("    #endregion");
                family = name;
                first = true;
                sb.AppendLine();
                sb.AppendLine($"    #region {name}");
            }

            if (!first) sb.AppendLine();
            first = false;

            var text = member.Replace("{type}", type).Replace("{scalar}", scalar);
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {text}");
        }

        if (family.Length != 0) sb.AppendLine("    #endregion");

        return VectorDocs.Apply(sb.ToString());
    }

    /// <summary>
    /// Returns the name of the legacy interface a member of the kind belongs to.
    /// </summary>
    private static string Family(char kind) => kind switch
    {
        'f' => "IVectorFloatingPointIeee754",
        _ => "IVectorInteger",
    };

    /// <summary>
    /// True when the vector described by <paramref name="typ"/> implements the kind of the interface a member
    /// belongs to.
    /// </summary>
    private static bool Implements(Typ typ, int size, char kind) => kind switch
    {
        'a' => typ.arith,
        '3' => typ.arith && size == 3,
        'f' => typ.f,
        'i' => typ.i,
        'u' => typ.i && !typ.sig,
        _ => false,
    };
}
