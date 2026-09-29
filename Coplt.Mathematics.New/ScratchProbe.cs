// temporary probe, deleted after the check
using System.Numerics;
using System.Runtime.Intrinsics;

namespace ScratchProbe;

internal static class Probe
{
    public static T All<T>() where T : unmanaged, IBinaryNumber<T> => T.AllBitsSet;

    public static Vector128<byte> Reg128<T>(in Vector128<T> v)
        where T : unmanaged, IFloatingPointIeee754<T> => Vector128.IsNaN(v).AsByte();

    public static Vector256<byte> Reg256<T>(in Vector256<T> v)
        where T : unmanaged, IFloatingPointIeee754<T> => Vector256.IsNaN(v).AsByte();

    public static T Bits<T>(T v) where T : unmanaged, IBinaryNumber<T> => v & ~T.AllBitsSet;
}
