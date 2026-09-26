using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// The as members of the vectors are members of the vector itself, a member of the math class reaches them as
// well. The type of the result of one of the members below cannot be inferred from the source vector by the
// compiler of today, so it has to be spelled out: math.asf<int2, float2>(v). Once the compiler can infer it from
// the constraint of the member, the forwarding of every target vector (the extension members of the ex_* classes)
// can be dropped and only these members have to stay.
public static partial class math
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the floating point component
    /// </summary>
    /// <typeparam name="T">The type of the vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The vector of the floating point component</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="TResult"/> that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asf<T, TResult>(in T source) where T : unmanaged, IVectorAsF<T, TResult>
        => T.asf(source);

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the signed component
    /// </summary>
    /// <typeparam name="T">The type of the vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The vector of the signed component</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="TResult"/> that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asi<T, TResult>(in T source) where T : unmanaged, IVectorAsI<T, TResult>
        => T.asi(source);

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the unsigned component
    /// </summary>
    /// <typeparam name="T">The type of the vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The vector of the unsigned component</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="TResult"/> that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asu<T, TResult>(in T source) where T : unmanaged, IVectorAsU<T, TResult>
        => T.asu(source);

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the boolean component
    /// </summary>
    /// <typeparam name="T">The type of the vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The vector of the boolean component</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="TResult"/> that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asb<T, TResult>(in T source) where T : unmanaged, IVectorAsB<T, TResult>
        => T.asb(source);

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the 2 component vector of the same component type
    /// <para>The components behind the second one are dropped, so they have to be zero</para>
    /// </summary>
    /// <typeparam name="T">The type of the vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The 2 component vector of the same component type</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The 2 component vector that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    public static TResult as2<T, TResult>(in T source) where T : unmanaged, IVectorAs2<T, TResult>
        => T.as2(source);

    /// <summary>
    /// Reinterprets the bits of the 4 component <paramref name="source"/> as the 3 component vector of the same
    /// component type
    /// <para>The <c>w</c> component of <paramref name="source"/> is dropped, so it has to be zero</para>
    /// </summary>
    /// <typeparam name="T">The type of the 4 component vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The 3 component vector of the same component type</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The 3 component vector that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    public static TResult as3<T, TResult>(in T source) where T : unmanaged, IVectorAs3<T, TResult>
        => T.as3(source);

    /// <summary>
    /// Reinterprets the bits of the 3 component <paramref name="source"/> as the 4 component vector of the same
    /// component type
    /// <para>The added <c>w</c> component is zero</para>
    /// </summary>
    /// <typeparam name="T">The type of the 3 component vector to reinterpret</typeparam>
    /// <typeparam name="TResult">The 4 component vector of the same component type</typeparam>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The 4 component vector that has the bits of <paramref name="source"/></returns>
    [MethodImpl(256)]
    public static TResult as4<T, TResult>(in T source) where T : unmanaged, IVectorAs4<T, TResult>
        => T.as4(source);
}
