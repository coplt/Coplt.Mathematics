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
}
