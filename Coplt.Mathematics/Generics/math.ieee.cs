using Coplt.Mathematics.Generics;

namespace Coplt.Experimental.Mathematics;

// The ieee 754 members of the vectors are members of the vector itself, a member of the math class reaches them
// as well. The parameters of every member below are the ones of the interface of its operation in the same order,
// which is the order of the hlsl counterpart of the operation as well: math.sin(v) and math.step(threshold, v).
// Most of the members do not name the type of a single component, the compiler infers the vector type from the
// argument. The members that do name it or the type of the mask they return can only be reached when the caller
// spells the extra type out, which is what the members of math.as do as well: math.length<float3, float>(v) and
// math.is_NaN<float3, b32v3>(v). The members below that take a scalar infer it from the argument.
public static partial class math
{
    /// <summary>
    /// Returns the natural logarithm of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The natural logarithm</returns>
    [MethodImpl(256)]
    public static T log<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log(a);

    /// <summary>
    /// Returns the base 2 logarithm of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The base 2 logarithm</returns>
    [MethodImpl(256)]
    public static T log2<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log2(a);

    /// <summary>
    /// Returns the logarithm of every component with <paramref name="b"/> as the base
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The base of the logarithm</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The logarithm</returns>
    [MethodImpl(256)]
    public static T log<T>(in T a, in T b) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log(a, b);

    /// <summary>
    /// Returns the base 10 logarithm of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The base 10 logarithm</returns>
    [MethodImpl(256)]
    public static T log10<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.log10(a);

    /// <summary>
    /// Returns <c>e</c> raised to the power of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The exponential</returns>
    [MethodImpl(256)]
    public static T exp<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp(a);

    /// <summary>
    /// Returns 2 raised to the power of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The exponential</returns>
    [MethodImpl(256)]
    public static T exp2<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp2(a);

    /// <summary>
    /// Returns 10 raised to the power of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The exponential</returns>
    [MethodImpl(256)]
    public static T exp10<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.exp10(a);

    /// <summary>
    /// Returns every component raised to the power of the matching component of <paramref name="b"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The exponent of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The power</returns>
    [MethodImpl(256)]
    public static T pow<T>(in T a, in T b) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.pow(a, b);

    /// <summary>
    /// Returns the square root of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The square root</returns>
    [MethodImpl(256)]
    public static T sqrt<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sqrt(a);

    /// <summary>
    /// Returns the reciprocal of the square root of every component, it is the same as <c>rcp(sqrt())</c>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The reciprocal of the square root</returns>
    [MethodImpl(256)]
    public static T rsqrt<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.rsqrt(a);

    /// <summary>
    /// Returns <paramref name="a"/> scaled to a length of 1, the result is a NaN vector when the length is zero
    /// </summary>
    /// <param name="a">The vector to normalize</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The normalized vector</returns>
    [MethodImpl(256)]
    public static T normalize<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.normalize(a);

    /// <summary>
    /// Returns <paramref name="a"/> scaled to a length of 1, it returns a zero vector when the length is zero
    /// </summary>
    /// <param name="a">The vector to normalize</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The normalized vector</returns>
    [MethodImpl(256)]
    public static T normalize_safe<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.normalize_safe(a);

    /// <summary>
    /// Returns 1 where the component of <paramref name="a"/> is not less than the matching component of
    /// <paramref name="threshold"/> and 0 where it is less
    /// </summary>
    /// <param name="threshold">The threshold</param>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The step vector</returns>
    [MethodImpl(256)]
    public static T step<T>(in T threshold, in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.step(threshold, a);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto <paramref name="onto"/>, it returns
    /// <paramref name="default_value"/> when the projection is not finite
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="onto">The vector to project onto</param>
    /// <param name="default_value">The value that is returned when the projection is not finite</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The projected vector</returns>
    [MethodImpl(256)]
    public static T project_safe<T>(in T a, in T onto, in T default_value = default)
        where T : unmanaged, IVectorFloatingPointIeee754<T> => T.project_safe(a, onto, default_value);

    /// <summary>
    /// Returns <paramref name="a"/> with the sign chosen so that it faces away from the incident vector
    /// <paramref name="i"/>, it is the same as flipping the sign when the dot product of
    /// <paramref name="ng"/> and <paramref name="i"/> is not negative
    /// </summary>
    /// <param name="a">The vector to orient</param>
    /// <param name="i">The incident vector</param>
    /// <param name="ng">The normal that is used to choose the sign</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The oriented vector</returns>
    [MethodImpl(256)]
    public static T face_forward<T>(in T a, in T i, in T ng) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.face_forward(a, i, ng);

    /// <summary>
    /// Returns the sine of every component in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The sine</returns>
    [MethodImpl(256)]
    public static T sin<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sin(a);

    /// <summary>
    /// Returns the cosine of every component in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The cosine</returns>
    [MethodImpl(256)]
    public static T cos<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.cos(a);

    /// <summary>
    /// Returns the sine and the cosine of every component in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The sine and the cosine</returns>
    [MethodImpl(256)]
    public static (T sin, T cos) sincos<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sincos(a);

    /// <summary>
    /// Computes the sine and the cosine of every component in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="sin">Receives the sine</param>
    /// <param name="cos">Receives the cosine</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    [MethodImpl(256)]
    public static void sincos<T>(in T a, out T sin, out T cos) where T : unmanaged, IVectorFloatingPointIeee754<T> =>
        T.sincos(a, out sin, out cos);

    /// <summary>
    /// Returns the tangent of every component in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The tangent</returns>
    [MethodImpl(256)]
    public static T tan<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.tan(a);

    /// <summary>
    /// Returns the arc sine of every component, the result is in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The arc sine</returns>
    [MethodImpl(256)]
    public static T asin<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.asin(a);

    /// <summary>
    /// Returns the arc cosine of every component, the result is in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The arc cosine</returns>
    [MethodImpl(256)]
    public static T acos<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.acos(a);

    /// <summary>
    /// Returns the arc tangent of every component, the result is in radians
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The arc tangent</returns>
    [MethodImpl(256)]
    public static T atan<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.atan(a);

    /// <summary>
    /// Returns the arc tangent of <paramref name="a"/> divided by <paramref name="b"/>, the signs of both are
    /// used to find the quadrant of the result
    /// </summary>
    /// <param name="a">The numerator</param>
    /// <param name="b">The divisor</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The arc tangent, it is in radians</returns>
    [MethodImpl(256)]
    public static T atan2<T>(in T a, in T b) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.atan2(a, b);

    /// <summary>
    /// Returns the hyperbolic sine of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The hyperbolic sine</returns>
    [MethodImpl(256)]
    public static T sinh<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.sinh(a);

    /// <summary>
    /// Returns the hyperbolic cosine of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The hyperbolic cosine</returns>
    [MethodImpl(256)]
    public static T cosh<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.cosh(a);

    /// <summary>
    /// Returns the hyperbolic tangent of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The hyperbolic tangent</returns>
    [MethodImpl(256)]
    public static T tanh<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.tanh(a);

    /// <summary>
    /// Returns the inverse hyperbolic sine of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The inverse hyperbolic sine</returns>
    [MethodImpl(256)]
    public static T asinh<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.asinh(a);

    /// <summary>
    /// Returns the inverse hyperbolic cosine of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The inverse hyperbolic cosine</returns>
    [MethodImpl(256)]
    public static T acosh<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.acosh(a);

    /// <summary>
    /// Returns the inverse hyperbolic tangent of every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The inverse hyperbolic tangent</returns>
    [MethodImpl(256)]
    public static T atanh<T>(in T a) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.atanh(a);

    /// <summary>
    /// Returns a vector that has the magnitude of <paramref name="a"/> and the sign of <paramref name="sign"/>
    /// </summary>
    /// <param name="a">The vector that provides the magnitude of every component</param>
    /// <param name="sign">The vector that provides the sign of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The vector with the changed sign</returns>
    [MethodImpl(256)]
    public static T chg_sign<T>(in T a, in T sign) where T : unmanaged, IVectorFloatingPointIeee754<T> => T.chg_sign(a, sign);

    /// <summary>
    /// Returns every component raised to the power of <paramref name="b"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The exponent</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The power</returns>
    [MethodImpl(256)]
    public static T pow<T, TScalar>(in T a, TScalar b)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar>
        where TScalar : unmanaged => T.pow(a, b);

    /// <summary>
    /// Returns the length of the vector, it is the same as <c>sqrt(length_sq())</c>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The length of the vector</returns>
    [MethodImpl(256)]
    public static TScalar length<T, TScalar>(in T a)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar>
        where TScalar : unmanaged => T.length(a);

    /// <summary>
    /// Returns the distance between the two vectors, it is the same as the length of the difference
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The other vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The distance</returns>
    [MethodImpl(256)]
    public static TScalar distance<T, TScalar>(in T a, in T b)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar>
        where TScalar : unmanaged => T.distance(a, b);

    /// <summary>
    /// Returns the refraction direction, <paramref name="i"/> has to be normalized and
    /// <paramref name="n"/> has to point against <paramref name="i"/>
    /// </summary>
    /// <param name="i">The normalized vector of the incoming direction</param>
    /// <param name="n">The normalized normal, it has to point against <paramref name="i"/></param>
    /// <param name="index_of_refraction">The ratio between the index of refraction of the two materials</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The refracted direction</returns>
    [MethodImpl(256)]
    public static T refract<T, TScalar>(in T i, in T n, TScalar index_of_refraction)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar>
        where TScalar : unmanaged => T.refract(i, n, index_of_refraction);

    /// <summary>
    /// Returns a mask that is true where the component is NaN
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TBool">The type of the mask, the vector that has the same shape as
    /// <typeparamref name="T"/></typeparam>
    /// <returns>The mask</returns>
    [MethodImpl(256)]
    public static TBool is_NaN<T, TBool>(in T a) where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TBool> =>
        T.is_NaN(a);

    /// <summary>
    /// Returns a mask that is true where the component is finite, so it is neither NaN nor an infinity
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TBool">The type of the mask, the vector that has the same shape as
    /// <typeparamref name="T"/></typeparam>
    /// <returns>The mask</returns>
    [MethodImpl(256)]
    public static TBool is_finite<T, TBool>(in T a) where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TBool> =>
        T.is_finite(a);

    /// <summary>
    /// Returns a mask that is true where the component is a positive or a negative infinity
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TBool">The type of the mask, the vector that has the same shape as
    /// <typeparamref name="T"/></typeparam>
    /// <returns>The mask</returns>
    [MethodImpl(256)]
    public static TBool is_inf<T, TBool>(in T a) where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TBool> =>
        T.is_inf(a);

    /// <summary>
    /// Returns a mask that is true where the component is a positive infinity
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TBool">The type of the mask, the vector that has the same shape as
    /// <typeparamref name="T"/></typeparam>
    /// <returns>The mask</returns>
    [MethodImpl(256)]
    public static TBool is_pos_inf<T, TBool>(in T a) where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TBool> =>
        T.is_pos_inf(a);

    /// <summary>
    /// Returns a mask that is true where the component is a negative infinity
    /// </summary>
    /// <param name="a">The vector</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TBool">The type of the mask, the vector that has the same shape as
    /// <typeparamref name="T"/></typeparam>
    /// <returns>The mask</returns>
    [MethodImpl(256)]
    public static TBool is_neg_inf<T, TBool>(in T a) where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TBool> =>
        T.is_neg_inf(a);
}
