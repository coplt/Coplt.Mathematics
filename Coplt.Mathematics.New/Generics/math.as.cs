using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// The as members of the vectors are members of the vector itself, a member of the math class reaches them as
// well. The type of the result of one of the members below cannot be inferred from the source vector by the
// compiler of today, so it has to be spelled out: math.asf<int2, float2>(v). Once the compiler can infer it from
// the constraint of the member, the forwarding of every target vector (the extension members of the ex_* classes)
// can be dropped and only these members have to stay.
//
// The interface a member is constrained by only names the type of its result, the bits of the source are
// reinterpreted by the member itself, so a call does not reach the interface.
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
        => Unsafe.As<T, TResult>(ref Unsafe.AsRef(in source));

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the floating point component, which
    /// is the name of the member in HLSL
    /// </summary>
    /// <inheritdoc cref="asf{T, TResult}(in T)"/>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asfloat<T, TResult>(in T source) where T : unmanaged, IVectorAsF<T, TResult>
        => Unsafe.As<T, TResult>(ref Unsafe.AsRef(in source));

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
        => Unsafe.As<T, TResult>(ref Unsafe.AsRef(in source));

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the signed component, which is the
    /// name of the member in HLSL
    /// </summary>
    /// <inheritdoc cref="asi{T, TResult}(in T)"/>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asint<T, TResult>(in T source) where T : unmanaged, IVectorAsI<T, TResult>
        => Unsafe.As<T, TResult>(ref Unsafe.AsRef(in source));

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
        => Unsafe.As<T, TResult>(ref Unsafe.AsRef(in source));

    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the unsigned component, which is the
    /// name of the member in HLSL
    /// </summary>
    /// <inheritdoc cref="asu{T, TResult}(in T)"/>
    [MethodImpl(256)]
    [OverloadResolutionPriority(1000)]
    public static TResult asuint<T, TResult>(in T source) where T : unmanaged, IVectorAsU<T, TResult>
        => Unsafe.As<T, TResult>(ref Unsafe.AsRef(in source));
}
