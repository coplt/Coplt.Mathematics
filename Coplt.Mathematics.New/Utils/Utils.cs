using System.Diagnostics;

namespace Coplt.Mathematics;

internal static class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Load64<T>(this Vector64<T> place) =>
        Vector128.CreateScalar(Unsafe.As<Vector64<T>, ulong>(ref Unsafe.AsRef(in place))).As<ulong, T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Load64<T>(this ulong place) =>
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
    /// True when the bits of a value are not all zero, which is what a component of a mask that says that a
    /// condition holds is
    /// <para>A mask is a value of the kind of the values that a condition is about and the component of it that
    /// says that the condition holds is the all bits set value of the kind, so a component holds when the bits
    /// of it are not all zero and the all bits zero value of the kind is the only one of it that does not. The
    /// bits of the value are the ones that decide it: a floating point kind is read as the bits of the value,
    /// so the value of a kind that has only the sign bit of it set holds just as well, and the kinds that are
    /// not floating point are compared with the all bits zero value of the kind, which a value that has a bit
    /// set never is.</para>
    /// <para>The value of a floating point kind is read as a vector that keeps it in the lane of it and leaves
    /// the ones that follow it at zero: the lane that keeps the value is compared with the all bits zero lane
    /// and the ones that follow it are compared with the all bits set lane, which the zeroes they are do not
    /// match, so the bits of the value alone decide the answer. The value of the half kind is read as the 16
    /// bits of it.</para>
    /// </summary>
    /// <typeparam name="TScalar">The type of the value</typeparam>
    /// <param name="value">The value</param>
    /// <returns>True when the bits of the value are not all zero</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrue<TScalar>(TScalar value) where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        if (typeof(TScalar) == typeof(float))
        {
            return !Vector128.EqualsAny(Vector128.CreateScalar(value).AsUInt32(), Vector128.Create(0, -1, -1, -1).AsUInt32());
        }
        if (typeof(TScalar) == typeof(double))
        {
            return !Vector128.EqualsAny(Vector128.CreateScalar(value).AsUInt64(), Vector128.Create(0, -1).AsUInt64());
        }
        if (typeof(TScalar) == typeof(half))
        {
            return Unsafe.BitCast<TScalar, ushort>(value) != 0;
        }
        return value != TScalar.Zero;
    }
}
