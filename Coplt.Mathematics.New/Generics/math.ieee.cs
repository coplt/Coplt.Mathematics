using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// The ieee 754 members of the vectors are members of the vector itself, a member of the math class reaches them
// as well. The parameters of every member below are the ones of the interface of its operation in the same order,
// which is the order of the hlsl counterpart of the operation as well: math.sin(v) and math.step(threshold, v).
// Most of the members do not name the type of a single component, the compiler infers the vector type from the
// argument. The members that do name it can only be reached when the caller spells the extra type out, which is
// what the members of math.as do as well: math.length<float3, float>(v) and math.dot<float3, float>(a, b). The
// generator emits a member of every scalar type beside such a member, which a call that does not name the type
// of a single component reaches. The members below that take a scalar infer it from the argument.
public static partial class math
{
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
}
