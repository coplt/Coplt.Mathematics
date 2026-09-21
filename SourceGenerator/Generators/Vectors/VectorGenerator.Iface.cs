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
    /// member below is the forwarding of the one of the vector to the other one, they are emitted into their
    /// own file.
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
        // the bool vector that has the same number of components as the vector
        var boolType = $"b{typ.size * 8}v{size}";

        // the members that implement the interfaces, the kind tells which vector implements them:
        // a = every arithmetic vector, 3 = the 3 component one, f = the floating point one,
        // i = the integer one, u = the integer one without a sign
        var members = new List<(char Kind, string Member)>
        {
            #region IVectorArithmetic

            ('a', "{type} abs(in {type} a) => a.abs();"),
            ('a', "{type} sign(in {type} a) => a.sign();"),
            ('a', "{type} min(in {type} a, in {type} b) => a.min(b);"),
            ('a', "{type} max(in {type} a, in {type} b) => a.max(b);"),
            ('a', "{type} clamp(in {type} a, in {type} min, in {type} max) => a.clamp(min, max);"),
            ('a', "{type} clamp(in {type} a, {scalar} min, {scalar} max) => a.clamp(min, max);"),
            // the legacy form of the interpolation puts the factor last and the member of the vector is called
            // on it, a factor that is a scalar keeps the vector as the last parameter as well
            ('a', "{type} lerp(in {type} start, in {type} end, in {type} t) => t.lerp(start, end);"),
            ('a', "{type} lerp({scalar} start, {scalar} end, in {type} t) => t.lerp(start, end);"),
            ('a', "{type} lerp(in {type} start, in {type} end, {scalar} t) => {type}.lerp(t, start, end);"),
            ('a', "{type} unlerp(in {type} a, in {type} start, in {type} end) => a.unlerp(start, end);"),
            ('a', "{type} unlerp(in {type} a, {scalar} start, {scalar} end) => a.unlerp(start, end);"),
            ('a', "{type} remap(in {type} a, in {type} src_start, in {type} src_end, in {type} dst_start, in {type} dst_end) => " +
                  "a.remap(src_start, src_end, dst_start, dst_end);"),
            ('a', "{type} remap(in {type} a, {scalar} src_start, {scalar} src_end, {scalar} dst_start, {scalar} dst_end) => " +
                  "a.remap(src_start, src_end, dst_start, dst_end);"),
            ('a', "{type} square(in {type} a) => a.square();"),
            ('a', "{scalar} dot(in {type} a, in {type} b) => a.dot(b);"),
            ('a', "{scalar} length_sq(in {type} a) => a.length_sq();"),
            ('a', "{scalar} distance_sq(in {type} a, in {type} b) => a.distance_sq(b);"),
            ('a', "{scalar} csum(in {type} a) => a.csum();"),
            ('a', "{scalar} cmin(in {type} a) => a.cmin();"),
            ('a', "{scalar} cmax(in {type} a) => a.cmax();"),
            ('a', "{scalar} cmin_safe(in {type} a) => a.cmin_safe();"),
            ('a', "{scalar} cmax_safe(in {type} a) => a.cmax_safe();"),
            ('3', "{type} cross(in {type} a, in {type} b) => a.cross(b);"),

            #endregion

            #region IVectorFloatingPoint

            ('f', "{type} mod(in {type} a, in {type} b) => a.mod(b);"),
            ('f', "{type} modf(in {type} a, out {type} i) => a.modf(out i);"),
            ('f', "{type} ceil(in {type} a) => a.ceil();"),
            ('f', "{type} floor(in {type} a) => a.floor();"),
            ('f', "{type} round(in {type} a) => a.round();"),
            ('f', "{type} trunc(in {type} a) => a.trunc();"),
            ('f', "{type} frac(in {type} a) => a.frac();"),
            ('f', "{type} rcp(in {type} a) => a.rcp();"),
            ('f', "{type} saturate(in {type} a) => a.saturate();"),
            // the legacy form of the interpolation between the two bounds puts the value last
            ('f', "{type} smoothstep(in {type} min, in {type} max, in {type} a) => a.smoothstep(min, max);"),
            ('f', "{type} reflect(in {type} a, in {type} n) => a.reflect(n);"),
            ('f', "{type} project(in {type} a, in {type} onto) => a.project(onto);"),
            ('f', "{type} project_on_plane(in {type} a, in {type} plane_normal) => a.project_on_plane(plane_normal);"),
            ('f', "{type} project_normalized(in {type} a, in {type} onto) => a.project_normalized(onto);"),
            ('f', "{type} project_on_plane_normalized(in {type} a, in {type} plane_normal) => a.project_on_plane_normalized(plane_normal);"),
            ('f', "{type} radians(in {type} a) => a.radians();"),
            ('f', "{type} degrees(in {type} a) => a.degrees();"),
            ('f', "{type} wrap(in {type} a, in {type} min, in {type} max) => a.wrap(min, max);"),
            ('f', "{type} wrap(in {type} a, {scalar} min, {scalar} max) => a.wrap(min, max);"),

            #endregion

            #region IVectorFloatingPointIeee754

            ('f', "{bool} is_NaN(in {type} a) => a.is_NaN();"),
            ('f', "{bool} is_finite(in {type} a) => a.is_finite();"),
            ('f', "{bool} is_inf(in {type} a) => a.is_inf();"),
            ('f', "{bool} is_pos_inf(in {type} a) => a.is_pos_inf();"),
            ('f', "{bool} is_neg_inf(in {type} a) => a.is_neg_inf();"),
            ('f', "{type} log(in {type} a) => a.log();"),
            ('f', "{type} log2(in {type} a) => a.log2();"),
            ('f', "{type} log10(in {type} a) => a.log10();"),
            ('f', "{type} exp(in {type} a) => a.exp();"),
            ('f', "{type} exp2(in {type} a) => a.exp2();"),
            ('f', "{type} exp10(in {type} a) => a.exp10();"),
            ('f', "{type} pow(in {type} a, in {type} b) => a.pow(b);"),
            ('f', "{type} pow(in {type} a, {scalar} b) => a.pow(b);"),
            ('f', "{type} sqrt(in {type} a) => a.sqrt();"),
            ('f', "{type} rsqrt(in {type} a) => a.rsqrt();"),
            ('f', "{scalar} length(in {type} a) => a.length();"),
            ('f', "{scalar} distance(in {type} a, in {type} b) => a.distance(b);"),
            ('f', "{type} normalize(in {type} a) => a.normalize();"),
            ('f', "{type} normalize_safe(in {type} a) => a.normalize_safe();"),
            // the legacy form of the step puts the threshold first and the value last
            ('f', "{type} step(in {type} threshold, in {type} a) => a.step(threshold);"),
            // the legacy form of the safe projection carries the default as an optional parameter, which the
            // caller of a type parameter can drop, so there is a single member for the two cases of it
            ('f', "{type} project_safe(in {type} a, in {type} onto, in {type} default_value = default) => a.project_safe(onto, default_value);"),
            ('f', "{type} face_forward(in {type} a, in {type} i, in {type} ng) => a.face_forward(i, ng);"),
            ('f', "{type} sin(in {type} a) => a.sin();"),
            ('f', "{type} cos(in {type} a) => a.cos();"),
            ('f', "({type} sin, {type} cos) sincos(in {type} a) => a.sincos();"),
            ('f', "void sincos(in {type} a, out {type} sin, out {type} cos) => a.sincos(out sin, out cos);"),
            ('f', "{type} tan(in {type} a) => a.tan();"),
            ('f', "{type} asin(in {type} a) => a.asin();"),
            ('f', "{type} acos(in {type} a) => a.acos();"),
            ('f', "{type} atan(in {type} a) => a.atan();"),
            ('f', "{type} atan2(in {type} a, in {type} b) => a.atan2(b);"),
            ('f', "{type} sinh(in {type} a) => a.sinh();"),
            ('f', "{type} cosh(in {type} a) => a.cosh();"),
            ('f', "{type} tanh(in {type} a) => a.tanh();"),
            ('f', "{type} asinh(in {type} a) => a.asinh();"),
            ('f', "{type} acosh(in {type} a) => a.acosh();"),
            ('f', "{type} atanh(in {type} a) => a.atanh();"),
            ('f', "{type} chg_sign(in {type} a, in {type} sign) => a.chg_sign(sign);"),

            #endregion

            #region IVectorInteger

            ('i', "{bool} is_pow2(in {type} a) => a.is_pow2();"),
            // the rounding up to the next power of two is only meaningful for a vector that has no sign
            ('u', "{type} up2pow2(in {type} a) => a.up2pow2();"),

            #endregion
        };

        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, false);
        sb.AppendLine($"public partial struct {type}");
        sb.AppendLine("{");

        var first = true;
        foreach (var (kind, member) in members)
        {
            if (!Implements(typ, size, kind)) continue;
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine();
            }

            var text = member.Replace("{type}", type).Replace("{scalar}", scalar).Replace("{bool}", boolType);
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {text}");
        }

        sb.AppendLine("}");
        return VectorDocs.Apply(sb.ToString());
    }

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
