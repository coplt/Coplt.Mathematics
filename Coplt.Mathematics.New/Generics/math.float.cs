using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// The floating point members of the vectors are members of the vector itself, a member of the math class reaches
// them as well. The parameters of every member below are the ones of the interface of its operation in the same
// order, which is the order of the hlsl counterpart of the operation as well: math.smoothstep(min, max, v) names
// the bounds before the value, which is the order of its hlsl counterpart. None of the members below needs the
// type of a single component, so they do not have to name it: the compiler infers the vector type from the
// argument.
// The constants of the vector (E, PI, Tau, ...) are not forwarded, a property cannot be a member of the math
// class: float3.PI reaches the one of a known type and T.PI the one of a generic type.
public static partial class math
{
    /// <summary>
    /// Returns the component wise remainder of the division of the two vectors
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="b">The divisor</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The remainder</returns>
    [MethodImpl(256)]
    public static T mod<T>(in T a, in T b) where T : unmanaged, IVectorFloatingPoint<T> => T.mod(a, b);

    /// <summary>
    /// Splits the vector into its integral and its fractional part
    /// <para>It returns the fractional part and stores the integral part into <paramref name="i"/></para>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="i">Receives the integral part</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The fractional part</returns>
    [MethodImpl(256)]
    public static T modf<T>(in T a, out T i) where T : unmanaged, IVectorFloatingPoint<T> => T.modf(a, out i);
    
    /// <summary>
    /// Interpolates smoothly between <paramref name="min"/> and <paramref name="max"/>, <paramref name="a"/> is
    /// the value
    /// <para>The result is 0 when the value is below the minimum, 1 when it is above the maximum and a smooth
    /// hermite curve in between</para>
    /// </summary>
    /// <param name="min">The value at 0</param>
    /// <param name="max">The value at 1</param>
    /// <param name="a">The value to place between the two bounds</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The interpolated vector</returns>
    [MethodImpl(256)]
    public static T smoothstep<T>(in T min, in T max, in T a) where T : unmanaged, IVectorFloatingPoint<T> => T.smoothstep(min, max, a);

    /// <summary>
    /// Returns <paramref name="a"/> reflected around the normal <paramref name="n"/>
    /// <para><paramref name="n"/> <b>has to be</b> normalized</para>
    /// </summary>
    /// <param name="a">The vector to reflect</param>
    /// <param name="n">The normalized normal of the surface</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The reflected vector</returns>
    [MethodImpl(256)]
    public static T reflect<T>(in T a, in T n) where T : unmanaged, IVectorFloatingPoint<T> => T.reflect(a, n);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto <paramref name="onto"/>
    /// <para>It is the component of <paramref name="a"/> that is parallel to <paramref name="onto"/></para>
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="onto">The vector to project onto, it does <b>not</b> have to be normalized</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The projected vector</returns>
    [MethodImpl(256)]
    public static T project<T>(in T a, in T onto) where T : unmanaged, IVectorFloatingPoint<T> => T.project(a, onto);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto the plane that has <paramref name="plane_normal"/>
    /// as its normal
    /// <para>It is the component of <paramref name="a"/> that is inside the plane</para>
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="plane_normal">The normal of the plane, it does <b>not</b> have to be normalized</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The projected vector</returns>
    [MethodImpl(256)]
    public static T project_on_plane<T>(in T a, in T plane_normal) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.project_on_plane(a, plane_normal);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto <paramref name="onto"/>
    /// <para>It is the same as <see cref="project{T}(in T, in T)"/> but <paramref name="onto"/> is assumed to
    /// be normalized</para>
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="onto">The normalized vector to project onto</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The projected vector</returns>
    [MethodImpl(256)]
    public static T project_normalized<T>(in T a, in T onto) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.project_normalized(a, onto);

    /// <summary>
    /// Returns the projection of <paramref name="a"/> onto the plane that has <paramref name="plane_normal"/>
    /// as its normal, it is the same as <see cref="project_on_plane{T}(in T, in T)"/> but
    /// <paramref name="plane_normal"/> is assumed to be normalized
    /// </summary>
    /// <param name="a">The vector to project</param>
    /// <param name="plane_normal">The normalized normal of the plane</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The projected vector</returns>
    [MethodImpl(256)]
    public static T project_on_plane_normalized<T>(in T a, in T plane_normal) where T : unmanaged, IVectorFloatingPoint<T> =>
        T.project_on_plane_normalized(a, plane_normal);

    /// <summary>
    /// Degrees -> Radians
    /// </summary>
    /// <param name="a">The vector in degrees</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The vector in radians</returns>
    [MethodImpl(256)]
    public static T radians<T>(in T a) where T : unmanaged, IVectorFloatingPoint<T> => T.radians(a);

    /// <summary>
    /// Radians -> Degrees
    /// </summary>
    /// <param name="a">The vector in radians</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The vector in degrees</returns>
    [MethodImpl(256)]
    public static T degrees<T>(in T a) where T : unmanaged, IVectorFloatingPoint<T> => T.degrees(a);

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The wrapped vector</returns>
    [MethodImpl(256)]
    public static T wrap<T>(in T a, in T min, in T max) where T : unmanaged, IVectorFloatingPoint<T> => T.wrap(a, min, max);

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>, the bounds are
    /// the same for every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The wrapped vector</returns>
    [MethodImpl(256)]
    public static T wrap<T, TScalar>(in T a, TScalar min, TScalar max)
        where T : unmanaged, IVectorFloatingPoint<T, TScalar>
        where TScalar : unmanaged => T.wrap(a, min, max);
}
