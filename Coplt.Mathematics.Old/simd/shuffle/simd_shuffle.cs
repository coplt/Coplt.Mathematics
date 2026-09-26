namespace Coplt.Mathematics.Simd;

[CpuOnly]
public static partial class simd_shuffle
{
    [MethodImpl(256 | 512)]
    public static Vector128<int> Shuffle(Vector128<int> a, Vector128<int> b, Shuffle42 lh) =>
        Shuffle(a.AsSingle(), b.AsSingle(), lh).AsInt32();

    [MethodImpl(256 | 512)]
    public static Vector128<uint> Shuffle(Vector128<uint> a, Vector128<uint> b, Shuffle42 lh) =>
        Shuffle(a.AsSingle(), b.AsSingle(), lh).AsUInt32();

    [MethodImpl(256 | 512)]
    public static Vector256<long> Shuffle(Vector256<long> a, Vector256<long> b, Shuffle42 lh) =>
        Shuffle(a.AsDouble(), b.AsDouble(), lh).AsInt64();

    [MethodImpl(256 | 512)]
    public static Vector256<ulong> Shuffle(Vector256<ulong> a, Vector256<ulong> b, Shuffle42 lh) =>
        Shuffle(a.AsDouble(), b.AsDouble(), lh).AsUInt64();
}
