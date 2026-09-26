namespace Coplt.Mathematics.Implements;

internal static class Scalar
{
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
