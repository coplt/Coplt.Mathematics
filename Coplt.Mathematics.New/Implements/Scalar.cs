namespace Coplt.Mathematics.Implements;

internal static class Scalar
{
    /// <summary>
    /// Builds the register of a component: the lanes that follow the one of it stay zero, which only
    /// <see cref="simd.ScalarRegisterIsZeroed"/> guarantees
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Register128<T>(T value) where T : unmanaged => simd.ScalarRegisterIsZeroed
        ? Vector128.CreateScalarUnsafe(value)
        : Vector128.CreateScalar(value);

    /// <inheritdoc cref="Register128{T}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<T> Register256<T>(T value) where T : unmanaged => simd.ScalarRegisterIsZeroed
        ? Vector256.CreateScalarUnsafe(value)
        : Vector256.CreateScalar(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sign<T>(T value)
        where T : unmanaged, IBinaryNumber<T>
    {
        if (typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(half))
        {
            var a = value & -T.Zero | T.One;
            var c = value == T.Zero ? T.Zero : T.AllBitsSet;
            return a & c;
        }

        if (value.Equals(T.Zero)) return T.Zero;
        if (typeof(T) == typeof(byte)
            || typeof(T) == typeof(ushort)
            || typeof(T) == typeof(uint)
            || typeof(T) == typeof(ulong)
            || typeof(T) == typeof(nuint)) return T.One;

        if (value < T.Zero) return -T.One;
        return T.One;
    }
}
