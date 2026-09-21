using Coplt.Experimental.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests;

/// <summary>
/// The members of the interfaces of a vector are static, so a generic helper that only knows a type parameter
/// calls them with the static form: <c>T.abs(v)</c>. A helper that wants the member form on the value has to
/// take the value as the first parameter itself, which is what the members below do, they are only here because
/// a type parameter cannot reach the member of the vector itself. The value is passed by value because the
/// receiver of an extension method of a type parameter cannot be an <c>in</c> parameter.
/// </summary>
internal static class VectorExtensions
{
    #region IVectorArithmetic

    public static T abs<T>(this T a) where T : unmanaged, IVectorArithmetic<T> => T.abs(a);
    public static T sign<T>(this T a) where T : unmanaged, IVectorArithmetic<T> => T.sign(a);
    public static T min<T>(this T a, in T other) where T : unmanaged, IVectorArithmetic<T> => T.min(a, other);
    public static T max<T>(this T a, in T other) where T : unmanaged, IVectorArithmetic<T> => T.max(a, other);
    public static T clamp<T>(this T a, in T min, in T max) where T : unmanaged, IVectorArithmetic<T> => T.clamp(a, min, max);

    public static T clamp<T, TScalar>(this T a, TScalar min, TScalar max)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.clamp(a, min, max);

    public static T lerp<T>(this T t, in T start, in T end) where T : unmanaged, IVectorArithmetic<T> => T.lerp(start, end, t);

    public static T lerp<T, TScalar>(this T t, TScalar start, TScalar end)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.lerp(start, end, t);

    public static T unlerp<T>(this T a, in T start, in T end) where T : unmanaged, IVectorArithmetic<T> => T.unlerp(a, start, end);

    public static T unlerp<T, TScalar>(this T a, TScalar start, TScalar end)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.unlerp(a, start, end);

    public static T remap<T>(this T a, in T src_start, in T src_end, in T dst_start, in T dst_end)
        where T : unmanaged, IVectorArithmetic<T> => T.remap(a, src_start, src_end, dst_start, dst_end);

    public static T remap<T, TScalar>(this T a, TScalar src_start, TScalar src_end, TScalar dst_start, TScalar dst_end)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged =>
        T.remap(a, src_start, src_end, dst_start, dst_end);

    public static T square<T>(this T a) where T : unmanaged, IVectorArithmetic<T> => T.square(a);
    public static T cross<T>(this T a, in T other) where T : unmanaged, IVector3Arithmetic<T> => T.cross(a, other);

    public static TScalar dot<T, TScalar>(this T a, in T other)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.dot(a, other);

    public static TScalar length_sq<T, TScalar>(this T a)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.length_sq(a);

    public static TScalar distance_sq<T, TScalar>(this T a, in T to)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.distance_sq(a, to);

    public static TScalar csum<T, TScalar>(this T a)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.csum(a);

    public static TScalar cmin<T, TScalar>(this T a)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.cmin(a);

    public static TScalar cmax<T, TScalar>(this T a)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.cmax(a);

    public static TScalar cmin_safe<T, TScalar>(this T a)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.cmin_safe(a);

    public static TScalar cmax_safe<T, TScalar>(this T a)
        where T : unmanaged, IVectorArithmetic<T, TScalar> where TScalar : unmanaged => T.cmax_safe(a);

    #endregion

    #region IVectorFloatingPoint

    public static T mod<T>(this T a, in T other) where T : unmanaged, IVectorFloatingPoint<T> => T.mod(a, other);
    public static T modf<T>(this T a, out T i) where T : unmanaged, IVectorFloatingPoint<T> => T.modf(a, out i);
    public static T ceil<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.ceil(a);
    public static T floor<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.floor(a);
    public static T round<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.round(a);
    public static T trunc<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.trunc(a);
    public static T frac<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.frac(a);
    public static T rcp<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.rcp(a);
    public static T saturate<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.saturate(a);

    public static T smoothstep<T>(this T a, in T min, in T max) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.smoothstep(min, max, a);

    public static T reflect<T>(this T a, in T n) where T : unmanaged, IVectorFloatingPoint<T> => T.reflect(a, n);
    public static T project<T>(this T a, in T onto) where T : unmanaged, IVectorFloatingPoint<T> => T.project(a, onto);

    public static T project_on_plane<T>(this T a, in T plane_normal) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.project_on_plane(a, plane_normal);

    public static T project_normalized<T>(this T a, in T onto) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.project_normalized(a, onto);

    public static T project_on_plane_normalized<T>(this T a, in T plane_normal)
        where T : unmanaged, IVectorFloatingPoint<T> => T.project_on_plane_normalized(a, plane_normal);

    public static T radians<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.radians(a);
    public static T degrees<T>(this T a) where T : unmanaged, IVectorFloatingPoint<T> => T.degrees(a);

    public static T wrap<T>(this T a, in T min, in T max) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.wrap(a, min, max);

    public static T wrap<T, TScalar>(this T a, TScalar min, TScalar max)
        where T : unmanaged, IVectorFloatingPoint<T, TScalar> where TScalar : unmanaged => T.wrap(a, min, max);

    #endregion

    #region IVectorFloatingPointIeee754

    // the checks of the special values and the check of a power of two are not forwarded: the type of the mask
    // only appears in the constraint, so the compiler cannot infer it from the arguments and a caller has to use
    // the static member of the interface: T.is_NaN(v)
    public static T log<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log(a);
    public static T log2<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log2(a);
    public static T log<T>(this T a, in T other) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log(a, other);
    public static T log10<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log10(a);
    public static T exp<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp(a);
    public static T exp2<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp2(a);
    public static T exp10<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp10(a);
    public static T pow<T>(this T a, in T v) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.pow(a, v);

    public static T pow<T, TScalar>(this T a, TScalar v)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar> where TScalar : unmanaged => T.pow(a, v);

    public static T sqrt<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sqrt(a);
    public static T rsqrt<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.rsqrt(a);

    public static TScalar length<T, TScalar>(this T a)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar> where TScalar : unmanaged => T.length(a);

    public static TScalar distance<T, TScalar>(this T a, in T to)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar> where TScalar : unmanaged => T.distance(a, to);

    public static T normalize<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.normalize(a);
    public static T normalize_safe<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.normalize_safe(a);

    public static T step<T>(this T a, in T threshold) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.step(threshold, a);

    public static T project_safe<T>(this T a, in T onto) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.project_safe(a, onto);

    public static T project_safe<T>(this T a, in T onto, in T default_value)
        where T : unmanaged, IVectorFloatingPointIeee754<T> => T.project_safe(a, onto, default_value);

    public static T face_forward<T>(this T a, in T i, in T ng) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.face_forward(a, i, ng);

    public static T sin<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sin(a);
    public static T cos<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.cos(a);
    public static (T sin, T cos) sincos<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sincos(a);

    public static void sincos<T>(this T a, out T sin, out T cos) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.sincos(a, out sin, out cos);

    public static T tan<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.tan(a);
    public static T asin<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.asin(a);
    public static T acos<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.acos(a);
    public static T atan<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.atan(a);
    public static T atan2<T>(this T a, in T v) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.atan2(a, v);
    public static T sinh<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sinh(a);
    public static T cosh<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.cosh(a);
    public static T tanh<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.tanh(a);
    public static T asinh<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.asinh(a);
    public static T acosh<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.acosh(a);
    public static T atanh<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.atanh(a);

    public static T chg_sign<T>(this T a, in T sign) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.chg_sign(a, sign);

    #endregion

    #region IVectorInteger

    // the check of a power of two and the rounding up to the next power of two are not forwarded either, the
    // type of the mask of the first one cannot be inferred

    #endregion
}
