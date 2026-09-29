using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Generics;

namespace Tests;

/// <summary>
/// The members of the interfaces of a vector are static, so a generic helper that only knows a type parameter
/// calls them with the static form: <c>T.log(a)</c>. A helper that wants the member form on the value
/// has to take the value as the first parameter itself, which is what the members below do, they are only here
/// because a type parameter cannot reach the member of the vector itself. The value is passed by value because
/// the receiver of an extension method of a type parameter cannot be an <c>in</c> parameter.
/// <para>A member that the dispatch of an algebra implements is not helped here: it is the member of the
/// <c>math</c> class and the extension of a value, and a type parameter that satisfies the constraint of it
/// reaches both of them.</para>
/// </summary>
internal static class VectorExtensions
{
    #region IVectorFloatingPointIeee754

    // the checks of the special values and the check of a power of two are not forwarded: the type of the mask
    // of a value only appears in the constraint of the member of the dispatch of its bool value, so the
    // compiler cannot infer it from the arguments and a caller has to name it or reach the member of the
    // type of the mask of its value: math.is_NaN<T, TBool>(v) or v.is_NaN()
    public static T log<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log(a);
    public static T log2<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log2(a);
    public static T log<T>(this T a, in T other) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log(a, other);
    public static T log10<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log10(a);
    public static T exp<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp(a);
    public static T exp2<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp2(a);
    public static T exp10<T>(this T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp10(a);

    // the power, the square root and its reciprocal and the normalization are not forwarded: they are the
    // members of the algebra of the kind of the value, which a type parameter reaches with the members of the
    // math class and of the extension of a value

    // the length and the distance of two vectors are the members of the algebra as well now

    public static T step<T>(this T a, in T threshold) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.step(threshold, a);

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
    // value of the check is a value of the kind of the vector itself

    #endregion
}
