namespace Coplt.Mathematics;

internal static class GenericMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar FloatDot2<TScalar>(TScalar ax, TScalar ay, TScalar bx, TScalar by)
        where TScalar : IBinaryFloatingPointIeee754<TScalar> =>
        TScalar.FusedMultiplyAdd(ay, by, ax * bx);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar FloatDot3<TScalar>(TScalar ax, TScalar ay, TScalar az, TScalar bx, TScalar by, TScalar bz)
        where TScalar : IBinaryFloatingPointIeee754<TScalar> =>
        TScalar.FusedMultiplyAdd(az, bz, TScalar.FusedMultiplyAdd(ay, by, ax * bx));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar FloatDot4<TScalar>(TScalar ax, TScalar ay, TScalar az, TScalar aw, TScalar bx, TScalar by, TScalar bz, TScalar bw)
        where TScalar : IBinaryFloatingPointIeee754<TScalar> =>
        TScalar.FusedMultiplyAdd(ay, by, ax * bx) +
        TScalar.FusedMultiplyAdd(aw, bw, az * bz);
}
