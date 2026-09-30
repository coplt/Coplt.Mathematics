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

    // the logarithm, its two other bases and the two exponentials are not forwarded: they are the members of
    // the dispatch of the algebra of the kind of the value, which a type parameter reaches with the members of
    // the math class and of the extension of a value

    // the power, the square root and its reciprocal and the normalization are not forwarded: they are the
    // members of the algebra of the kind of the value, which a type parameter reaches with the members of the
    // math class and of the extension of a value

    // the length and the distance of two vectors are the members of the algebra as well now

    public static T step<T>(this T a, in T threshold) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.step(threshold, a);

    public static T face_forward<T>(this T a, in T i, in T ng) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.face_forward(a, i, ng);

    // the trigonometry is not forwarded either: it is the member of the dispatch of the algebra of the kind of
    // the value as well, the pair of the sine and the cosine of it and the member that receives the two of them
    // are the two forms of the single member of the dispatch that hands the value over once

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
