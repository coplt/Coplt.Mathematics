namespace Coplt.Mathematics.Simt;

public static partial class math_mt_ex;

[Ex, ExTo(typeof(math_mt_ex))]
public static partial class math_mt
{
    

    #region Generic Consts

    [MethodImpl(256 | 512)]
    internal static T MinNormal<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return math.F_MinNormal.BitCast<float, T>();
        if (typeof(T) == typeof(double)) return math.D_MinNormal.BitCast<double, T>();
        if (typeof(T) == typeof(half)) return ((short)0x0400).BitCast<short, T>();
        throw new NotSupportedException();
    }

    [MethodImpl(256 | 512)]
    internal static T MinRotateSafe<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return 1e-35f.BitCast<float, T>();
        if (typeof(T) == typeof(double)) return 1e-290.BitCast<double, T>();
        if (typeof(T) == typeof(half)) return ((half)1e-5f).BitCast<half, T>();
        throw new NotSupportedException();
    }

    [MethodImpl(256 | 512)]
    internal static T MaxRotateSafe<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return 1e35f.BitCast<float, T>();
        if (typeof(T) == typeof(double)) return 1e290.BitCast<double, T>();
        if (typeof(T) == typeof(half)) return ((half)1e5f).BitCast<half, T>();
        throw new NotSupportedException();
    }

    #endregion
}
