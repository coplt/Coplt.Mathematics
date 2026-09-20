namespace Coplt.Experimental.Mathematics;

internal static class VectorUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Load64<T>(this in Vector64<T> place) =>
        Vector128.CreateScalar(Unsafe.As<Vector64<T>, ulong>(ref Unsafe.AsRef(in place))).As<ulong, T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<T> Load64<T>(this in ulong place) =>
        Vector128.CreateScalar(place).As<ulong, T>();
}
