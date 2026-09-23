namespace Coplt.Mathematics;

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool eq<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a == b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ne<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a != b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool lt<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a < b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool gt<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a > b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool le<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a <= b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ge<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a >= b;
}
