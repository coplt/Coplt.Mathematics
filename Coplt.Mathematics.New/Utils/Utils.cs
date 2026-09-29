using System.Diagnostics;

namespace Coplt.Mathematics;

internal static class Utils
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

    /// <summary>
    /// Returns the mask of a condition of the kind of a value: the all bits set value of the kind when the
    /// condition holds and the zero of it when it does not
    /// <para>A bool result of the library is a value of the kind of the value that the condition is about, so a
    /// comparison of two values builds the result of it out of this member: a component of the result that holds
    /// is the all bits set value of its kind and a component that does not is the zero of it</para>
    /// </summary>
    /// <typeparam name="TScalar">The type of the value</typeparam>
    /// <param name="condition">The condition</param>
    /// <returns>The all bits set value of the kind or the zero of it</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar AllBits<TScalar>(bool condition) where TScalar : unmanaged, IBinaryNumber<TScalar> =>
        condition ? TScalar.AllBitsSet : TScalar.Zero;

    /// <summary>
    /// True when a value is not the zero of its kind, which is what a component of a mask that says that a
    /// condition holds is
    /// </summary>
    /// <typeparam name="TScalar">The type of the value</typeparam>
    /// <param name="value">The value</param>
    /// <returns>True when the value is not the zero of its kind</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrue<TScalar>(TScalar value) where TScalar : unmanaged, IBinaryNumber<TScalar> =>
        !TScalar.IsZero(value);
}
