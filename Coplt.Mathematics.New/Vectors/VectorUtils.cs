using System.Diagnostics;

namespace Coplt.Mathematics;

internal static class VectorUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Load64<T>(this in Vector64<T> place) =>
        Vector128.CreateScalar(Unsafe.As<Vector64<T>, ulong>(ref Unsafe.AsRef(in place))).As<ulong, T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Load64<T>(this in ulong place) =>
        Vector128.CreateScalar(place).As<ulong, T>();

    /// <summary>
    /// Throws the exception of a json value that is not the token the reader expected
    /// <para>The converters of the vectors share the message, it is not repeated in the generated code</para>
    /// </summary>
    /// <param name="expected">The token the reader expected</param>
    /// <param name="actual">The token the reader found</param>
    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowJsonToken(JsonTokenType expected, JsonTokenType actual) =>
        throw new JsonException($"Expected {expected} but found {actual}");
}
