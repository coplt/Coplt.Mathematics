#if NET8_0_OR_GREATER
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.Wasm;
using System.Runtime.Intrinsics.X86;
using X86 = System.Runtime.Intrinsics.X86;
#endif

namespace Coplt.Mathematics;

public static partial class math
{
    #region asf

    [MethodImpl(256 | 512)]
    public static half asf(this ushort v) => v.BitCast<ushort, half>();
    [MethodImpl(256 | 512)]
    public static half asf(this short v) => v.BitCast<short, half>();
    [MethodImpl(256 | 512)]
    public static half asf(this b16 v) => v.BitCast<b16, half>();
    [MethodImpl(256 | 512)]
    public static float asf(this uint v) => v.BitCast<uint, float>();
    [MethodImpl(256 | 512)]
    public static float asf(this int v) => v.BitCast<int, float>();
    [MethodImpl(256 | 512)]
    public static float asf(this b32 v) => v.BitCast<b32, float>();
    [MethodImpl(256 | 512)]
    public static double asf(this ulong v) => v.BitCast<ulong, double>();
    [MethodImpl(256 | 512)]
    public static double asf(this long v) => v.BitCast<long, double>();
    [MethodImpl(256 | 512)]
    public static double asf(this b64 v) => v.BitCast<b64, double>();
    [MethodImpl(256 | 512)]
    public static half asf(this half v) => v;
    [MethodImpl(256 | 512)]
    public static float asf(this float v) => v;
    [MethodImpl(256 | 512)]
    public static double asf(this double v) => v;

    #endregion

    #region asu

    [MethodImpl(256 | 512)]
    public static ushort asu(this ushort v) => v;
    [MethodImpl(256 | 512)]
    public static ushort asu(this short v) => (ushort)v;
    [MethodImpl(256 | 512)]
    public static ushort asu(this b16 v) => v;
    [MethodImpl(256 | 512)]
    public static uint asu(this uint v) => v;
    [MethodImpl(256 | 512)]
    public static uint asu(this int v) => (uint)v;
    [MethodImpl(256 | 512)]
    public static uint asu(this b32 v) => v;
    [MethodImpl(256 | 512)]
    public static ulong asu(this ulong v) => v;
    [MethodImpl(256 | 512)]
    public static ulong asu(this long v) => (ulong)v;
    [MethodImpl(256 | 512)]
    public static ulong asu(this b64 v) => v;
    [MethodImpl(256 | 512)]
    public static ushort asu(this half v) => v.BitCast<half, ushort>();
    [MethodImpl(256 | 512)]
    public static uint asu(this float v) => v.BitCast<float, uint>();
    [MethodImpl(256 | 512)]
    public static ulong asu(this double v) => v.BitCast<double, ulong>();

    #endregion

    #region asi

    [MethodImpl(256 | 512)]
    public static short asi(this ushort v) => (short)v;
    [MethodImpl(256 | 512)]
    public static short asi(this short v) => v;
    [MethodImpl(256 | 512)]
    public static short asi(this b16 v) => (short)(ushort)v;
    [MethodImpl(256 | 512)]
    public static int asi(this uint v) => (int)v;
    [MethodImpl(256 | 512)]
    public static int asi(this int v) => v;
    [MethodImpl(256 | 512)]
    public static uint asi(this b32 v) => (uint)(int)v;
    [MethodImpl(256 | 512)]
    public static long asi(this ulong v) => (long)v;
    [MethodImpl(256 | 512)]
    public static long asi(this long v) => v;
    [MethodImpl(256 | 512)]
    public static long asi(this b64 v) => (long)(ulong)v;
    [MethodImpl(256 | 512)]
    public static short asi(this half v) => v.BitCast<half, short>();
    [MethodImpl(256 | 512)]
    public static int asi(this float v) => v.BitCast<float, int>();
    [MethodImpl(256 | 512)]
    public static long asi(this double v) => v.BitCast<double, long>();

    #endregion

    #region popcnt

    [MethodImpl(256 | 512)]
    public static short popcnt(this half c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short popcnt(this short c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short popcnt(this ushort c) => (short)popcnt((uint)c);

    [MethodImpl(256 | 512)]
    public static int popcnt(this float c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt(this int c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt(this uint c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.PopCount(c);
        #else
        return SoftwareFallback(c);

        [MethodImpl(256 | 512)]
        static int SoftwareFallback(uint value)
        {
            value -= value >> 1 & 1431655765U;
            value = (uint)(((int)value & 858993459) + ((int)(value >> 2) & 858993459));
            value = (uint)(((int)value + (int)(value >> 4) & 252645135) * 16843009 >>> 24);
            return (int)value;
        }
        #endif
    }

    [MethodImpl(256 | 512)]
    public static int popcnt(this double c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt(this long c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt(this ulong c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.PopCount(c);
        #else
        return SoftwareFallback(c);

        [MethodImpl(256 | 512)]
        static int SoftwareFallback(ulong value)
        {
            value -= value >> 1 & 6148914691236517205UL;
            value = (ulong)(((long)value & 3689348814741910323L) + ((long)(value >> 2) & 3689348814741910323L));
            value = (ulong)(((long)value + (long)(value >> 4) & 1085102592571150095L) * 72340172838076673L >>> 56);
            return (int)value;
        }
        #endif
    }

    #endregion

    #region lzcnt

    [MethodImpl(256 | 512)]
    public static short lzcnt(this half c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short lzcnt(this short c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short lzcnt(this ushort c) => (short)lzcnt((uint)c);

    [MethodImpl(256 | 512)]
    public static int lzcnt(this float c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt(this int c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt(this uint c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.LeadingZeroCount(c);
        #else
        if (c == 0) return 32;
        return 31 ^ BitOps.Log2SoftwareFallback(c);
        #endif
    }

    [MethodImpl(256 | 512)]
    public static int lzcnt(this double c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt(this long c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt(this ulong c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.LeadingZeroCount(c);
        #else
        var hi = (uint)(c >> 32);

        if (hi == 0)
        {
            return 32 + lzcnt((uint)c);
        }

        return lzcnt(hi);
        #endif
    }

    #endregion

    #region tzcnt

    [MethodImpl(256 | 512)]
    public static short tzcnt(this half c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short tzcnt(this short c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short tzcnt(this ushort c) => (short)tzcnt((uint)c);

    [MethodImpl(256 | 512)]
    public static int tzcnt(this float c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt(this int c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt(this uint c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.TrailingZeroCount(c);
        #else
        if (c == 0)
        {
            return 32;
        }

        // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
        return Unsafe.AddByteOffset(
            // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_0111_1100_1011_0101_0011_0001u
            ref MemoryMarshal.GetReference(BitOps.TrailingZeroCountDeBruijn),
            // uint|long -> IntPtr cast on 32-bit platforms does expensive overflow checks not needed here
            (IntPtr)(int)(((c & (uint)-(int)c) * 0x077CB531u) >> 27));
        #endif
    }

    [MethodImpl(256 | 512)]
    public static int tzcnt(this double c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt(this long c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt(this ulong c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.TrailingZeroCount(c);
        #else
        var lo = (uint)c;

        if (lo == 0)
        {
            return 32 + tzcnt((uint)(c >> 32));
        }

        return tzcnt(lo);
        #endif
    }

    #endregion

    #region up2pow2

    [MethodImpl(256 | 512)]
    public static ushort up2pow2(this ushort c)
    {
        #if NET8_0_OR_GREATER
        return (ushort)up2pow2((uint)c);
        #else
        // Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
        var v = (uint)c;
        --v;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        return (ushort)(v + 1);
        #endif
    }

    [MethodImpl(256 | 512)]
    public static uint up2pow2(this uint c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.RoundUpToPowerOf2(c);
        #else
        // Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
        --c;
        c |= c >> 1;
        c |= c >> 2;
        c |= c >> 4;
        c |= c >> 8;
        c |= c >> 16;
        return c + 1;
        #endif
    }

    [MethodImpl(256 | 512)]
    public static ulong up2pow2(this ulong c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.RoundUpToPowerOf2(c);
        #else
        // Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
        --c;
        c |= c >> 1;
        c |= c >> 2;
        c |= c >> 4;
        c |= c >> 8;
        c |= c >> 16;
        c |= c >> 32;
        return c + 1;
        #endif
    }

    #endregion

    #region isPow2

    [MethodImpl(256 | 512)]
    public static bool isPow2(this short c) => isPow2((int)c);

    [MethodImpl(256 | 512)]
    public static bool isPow2(this ushort c) => isPow2((uint)c);

    [MethodImpl(256 | 512)]
    public static bool isPow2(this int c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.IsPow2(c);
        #else
        return (c & (c - 1)) == 0 && c > 0;
        #endif
    }

    [MethodImpl(256 | 512)]
    public static bool isPow2(this uint c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.IsPow2(c);
        #else
        return (c & (c - 1)) == 0 && c != 0;
        #endif
    }

    [MethodImpl(256 | 512)]
    public static bool isPow2(this long c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.IsPow2(c);
        #else
        return (c & (c - 1)) == 0 && c > 0;
        #endif
    }

    [MethodImpl(256 | 512)]
    public static bool isPow2(this ulong c)
    {
        #if NET8_0_OR_GREATER
        return BitOperations.IsPow2(c);
        #else
        return (c & (c - 1)) == 0 && c != 0;
        #endif
    }

    #endregion

    #region select

    [MethodImpl(256 | 512)]
    public static byte select(this bool c, byte t, byte f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static sbyte select(this bool c, sbyte t, sbyte f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static ushort select(this bool c, ushort t, ushort f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static short select(this bool c, short t, short f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static uint select(this bool c, uint t, uint f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static int select(this bool c, int t, int f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static ulong select(this bool c, ulong t, ulong f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static long select(this bool c, long t, long f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static half select(this bool c, half t, half f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static float select(this bool c, float t, float f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static double select(this bool c, double t, double f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static decimal select(this bool c, decimal t, decimal f) => c ? t : f;

    #endregion

    #region BitNot

    [MethodImpl(256 | 512)]
    internal static b16 BitNot(this b16 v) => ~v;
    [MethodImpl(256 | 512)]
    internal static b32 BitNot(this b32 v) => ~v;
    [MethodImpl(256 | 512)]
    internal static b64 BitNot(this b64 v) => ~v;

    [MethodImpl(256 | 512)]
    internal static byte BitNot(this byte v) => (byte)~(uint)v;
    [MethodImpl(256 | 512)]
    internal static sbyte BitNot(this sbyte v) => (sbyte)~(uint)v;
    [MethodImpl(256 | 512)]
    internal static ushort BitNot(this ushort v) => (ushort)~v;
    [MethodImpl(256 | 512)]
    internal static short BitNot(this short v) => (short)~v;
    [MethodImpl(256 | 512)]
    internal static uint BitNot(this uint v) => ~v;
    [MethodImpl(256 | 512)]
    internal static int BitNot(this int v) => ~v;
    [MethodImpl(256 | 512)]
    internal static ulong BitNot(this ulong v) => ~v;
    [MethodImpl(256 | 512)]
    internal static long BitNot(this long v) => ~v;
    [MethodImpl(256 | 512)]
    internal static half BitNot(this half v) => ((ushort)~v.AsUInt16()).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitNot(this float v) => (~v.AsUInt32()).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitNot(this double v) => (~v.AsUInt32()).AsDouble();

    #endregion

    #region BitOr

    [MethodImpl(256 | 512)]
    internal static b16 BitOr(this b16 a, b16 b) => a | b;
    [MethodImpl(256 | 512)]
    internal static b32 BitOr(this b32 a, b32 b) => a | b;
    [MethodImpl(256 | 512)]
    internal static b64 BitOr(this b64 a, b64 b) => a | b;

    [MethodImpl(256 | 512)]
    internal static byte BitOr(this byte a, byte b) => (byte)(a | b);
    [MethodImpl(256 | 512)]
    internal static sbyte BitOr(this sbyte a, sbyte b) => (sbyte)(a | b);
    [MethodImpl(256 | 512)]
    internal static ushort BitOr(this ushort a, ushort b) => (ushort)(a | b);
    [MethodImpl(256 | 512)]
    internal static short BitOr(this short a, short b) => (short)(a | b);
    [MethodImpl(256 | 512)]
    internal static uint BitOr(this uint a, uint b) => a | b;
    [MethodImpl(256 | 512)]
    internal static int BitOr(this int a, int b) => a | b;
    [MethodImpl(256 | 512)]
    internal static ulong BitOr(this ulong a, ulong b) => a | b;
    [MethodImpl(256 | 512)]
    internal static long BitOr(this long a, long b) => a | b;
    [MethodImpl(256 | 512)]
    internal static half BitOr(this half a, half b) => ((ushort)(a.AsUInt16() | b.AsUInt16())).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitOr(this float a, float b) => (a.AsUInt32() | b.AsUInt32()).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitOr(this double a, double b) => (a.AsUInt32() | b.AsUInt32()).AsDouble();

    #endregion

    #region BitAnd

    [MethodImpl(256 | 512)]
    internal static b16 BitAnd(this b16 a, b16 b) => a & b;
    [MethodImpl(256 | 512)]
    internal static b32 BitAnd(this b32 a, b32 b) => a & b;
    [MethodImpl(256 | 512)]
    internal static b64 BitAnd(this b64 a, b64 b) => a & b;

    [MethodImpl(256 | 512)]
    internal static byte BitAnd(this byte a, byte b) => (byte)(a & b);
    [MethodImpl(256 | 512)]
    internal static sbyte BitAnd(this sbyte a, sbyte b) => (sbyte)(a & b);
    [MethodImpl(256 | 512)]
    internal static ushort BitAnd(this ushort a, ushort b) => (ushort)(a & b);
    [MethodImpl(256 | 512)]
    internal static short BitAnd(this short a, short b) => (short)(a & b);
    [MethodImpl(256 | 512)]
    internal static uint BitAnd(this uint a, uint b) => a & b;
    [MethodImpl(256 | 512)]
    internal static int BitAnd(this int a, int b) => a & b;
    [MethodImpl(256 | 512)]
    internal static ulong BitAnd(this ulong a, ulong b) => a & b;
    [MethodImpl(256 | 512)]
    internal static long BitAnd(this long a, long b) => a & b;
    [MethodImpl(256 | 512)]
    internal static half BitAnd(this half a, half b) => ((ushort)(a.AsUInt16() & b.AsUInt16())).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitAnd(this float a, float b) => (a.AsUInt32() & b.AsUInt32()).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitAnd(this double a, double b) => (a.AsUInt32() & b.AsUInt32()).AsDouble();

    #endregion

    #region BitXor

    [MethodImpl(256 | 512)]
    internal static b16 BitXor(this b16 a, b16 b) => a ^ b;
    [MethodImpl(256 | 512)]
    internal static b32 BitXor(this b32 a, b32 b) => a ^ b;
    [MethodImpl(256 | 512)]
    internal static b64 BitXor(this b64 a, b64 b) => a ^ b;

    [MethodImpl(256 | 512)]
    internal static byte BitXor(this byte a, byte b) => (byte)(a ^ b);
    [MethodImpl(256 | 512)]
    internal static sbyte BitXor(this sbyte a, sbyte b) => (sbyte)(a ^ b);
    [MethodImpl(256 | 512)]
    internal static ushort BitXor(this ushort a, ushort b) => (ushort)(a ^ b);
    [MethodImpl(256 | 512)]
    internal static short BitXor(this short a, short b) => (short)(a ^ b);
    [MethodImpl(256 | 512)]
    internal static uint BitXor(this uint a, uint b) => a ^ b;
    [MethodImpl(256 | 512)]
    internal static int BitXor(this int a, int b) => a ^ b;
    [MethodImpl(256 | 512)]
    internal static ulong BitXor(this ulong a, ulong b) => a ^ b;
    [MethodImpl(256 | 512)]
    internal static long BitXor(this long a, long b) => a ^ b;
    [MethodImpl(256 | 512)]
    internal static half BitXor(this half a, half b) => ((ushort)(a.AsUInt16() ^ b.AsUInt16())).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitXor(this float a, float b) => (a.AsUInt32() ^ b.AsUInt32()).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitXor(this double a, double b) => (a.AsUInt32() ^ b.AsUInt32()).AsDouble();

    #endregion

    #region BitAndNot

    [MethodImpl(256 | 512)]
    public static b16 andnot(this b16 a, b16 b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static b32 andnot(this b32 a, b32 b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static b64 andnot(this b64 a, b64 b) => a & ~b;

    [MethodImpl(256 | 512)]
    public static byte andnot(this byte a, byte b) => (byte)(a & ~b);
    [MethodImpl(256 | 512)]
    public static sbyte andnot(this sbyte a, sbyte b) => (sbyte)(a & ~b);
    [MethodImpl(256 | 512)]
    public static ushort andnot(this ushort a, ushort b) => (ushort)(a & ~b);
    [MethodImpl(256 | 512)]
    public static short andnot(this short a, short b) => (short)(a & ~b);
    [MethodImpl(256 | 512)]
    public static uint andnot(this uint a, uint b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static int andnot(this int a, int b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static ulong andnot(this ulong a, ulong b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static long andnot(this long a, long b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static half andnot(this half a, half b) => ((ushort)(a.AsUInt16() & ~b.AsUInt16())).AsHalf();
    [MethodImpl(256 | 512)]
    public static float andnot(this float a, float b) => (a.AsUInt32() & ~b.AsUInt32()).AsSingle();
    [MethodImpl(256 | 512)]
    public static double andnot(this double a, double b) => (a.AsUInt32() & ~b.AsUInt32()).AsDouble();

    #endregion

    #region BitShiftLeft

    [MethodImpl(256 | 512)]
    internal static byte BitShiftLeft(this byte a, int b) => (byte)(a << b);
    [MethodImpl(256 | 512)]
    internal static sbyte BitShiftLeft(this sbyte a, int b) => (sbyte)(a << b);
    [MethodImpl(256 | 512)]
    internal static ushort BitShiftLeft(this ushort a, int b) => (ushort)(a << b);
    [MethodImpl(256 | 512)]
    internal static short BitShiftLeft(this short a, int b) => (short)(a << b);
    [MethodImpl(256 | 512)]
    internal static uint BitShiftLeft(this uint a, int b) => a << b;
    [MethodImpl(256 | 512)]
    internal static int BitShiftLeft(this int a, int b) => a << b;
    [MethodImpl(256 | 512)]
    internal static ulong BitShiftLeft(this ulong a, int b) => a << b;
    [MethodImpl(256 | 512)]
    internal static long BitShiftLeft(this long a, int b) => a << b;
    [MethodImpl(256 | 512)]
    internal static half BitShiftLeft(this half a, int b) => ((ushort)(a.AsUInt16() << b)).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitShiftLeft(this float a, int b) => (a.AsUInt32() << b).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitShiftLeft(this double a, int b) => (a.AsUInt32() << b).AsDouble();

    #endregion

    #region BitShiftRight

    [MethodImpl(256 | 512)]
    internal static byte BitShiftRight(this byte a, int b) => (byte)(a >> b);
    [MethodImpl(256 | 512)]
    internal static sbyte BitShiftRight(this sbyte a, int b) => (sbyte)(a >> b);
    [MethodImpl(256 | 512)]
    internal static ushort BitShiftRight(this ushort a, int b) => (ushort)(a >> b);
    [MethodImpl(256 | 512)]
    internal static short BitShiftRight(this short a, int b) => (short)(a >> b);
    [MethodImpl(256 | 512)]
    internal static uint BitShiftRight(this uint a, int b) => a >> b;
    [MethodImpl(256 | 512)]
    internal static int BitShiftRight(this int a, int b) => a >> b;
    [MethodImpl(256 | 512)]
    internal static ulong BitShiftRight(this ulong a, int b) => a >> b;
    [MethodImpl(256 | 512)]
    internal static long BitShiftRight(this long a, int b) => a >> b;
    [MethodImpl(256 | 512)]
    internal static half BitShiftRight(this half a, int b) => ((ushort)(a.AsUInt16() >> b)).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitShiftRight(this float a, int b) => (a.AsUInt32() >> b).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitShiftRight(this double a, int b) => (a.AsUInt32() >> b).AsDouble();

    #endregion

    #region BitShiftRightUnsigned

    [MethodImpl(256 | 512)]
    internal static byte BitShiftRightUnsigned(this byte a, int b) => (byte)(a >>> b);
    [MethodImpl(256 | 512)]
    internal static sbyte BitShiftRightUnsigned(this sbyte a, int b) => (sbyte)(a >>> b);
    [MethodImpl(256 | 512)]
    internal static ushort BitShiftRightUnsigned(this ushort a, int b) => (ushort)(a >>> b);
    [MethodImpl(256 | 512)]
    internal static short BitShiftRightUnsigned(this short a, int b) => (short)(a >>> b);
    [MethodImpl(256 | 512)]
    internal static uint BitShiftRightUnsigned(this uint a, int b) => a >>> b;
    [MethodImpl(256 | 512)]
    internal static int BitShiftRightUnsigned(this int a, int b) => a >>> b;
    [MethodImpl(256 | 512)]
    internal static ulong BitShiftRightUnsigned(this ulong a, int b) => a >>> b;
    [MethodImpl(256 | 512)]
    internal static long BitShiftRightUnsigned(this long a, int b) => a >>> b;
    [MethodImpl(256 | 512)]
    internal static half BitShiftRightUnsigned(this half a, int b) => ((ushort)(a.AsUInt16() >>> b)).AsHalf();
    [MethodImpl(256 | 512)]
    internal static float BitShiftRightUnsigned(this float a, int b) => (a.AsUInt32() >>> b).AsSingle();
    [MethodImpl(256 | 512)]
    internal static double BitShiftRightUnsigned(this double a, int b) => (a.AsUInt32() >>> b).AsDouble();

    #endregion

    #region isNaN

    [MethodImpl(256 | 512)]
    public static bool isNaN(this half v) => half.IsNaN(v);

    [MethodImpl(256 | 512)]
    public static bool isNaN(this float v) => float.IsNaN(v);

    [MethodImpl(256 | 512)]
    public static bool isNaN(this double v) => double.IsNaN(v);

    #endregion

    #region isFinite

    [MethodImpl(256 | 512)]
    public static bool isFinite(this half v) => half.IsFinite(v);

    [MethodImpl(256 | 512)]
    public static bool isFinite(this float v) => float.IsFinite(v);

    [MethodImpl(256 | 512)]
    public static bool isFinite(this double v) => double.IsFinite(v);

    #endregion

    #region Abs

    [MethodImpl(256 | 512)]
    public static byte abs(this byte v) => v;
    [MethodImpl(256 | 512)]
    public static sbyte abs(this sbyte v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static ushort abs(this ushort v) => v;
    [MethodImpl(256 | 512)]
    public static short abs(this short v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static uint abs(this uint v) => v;
    [MethodImpl(256 | 512)]
    public static int abs(this int v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static ulong abs(this ulong v) => v;
    [MethodImpl(256 | 512)]
    public static long abs(this long v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static half abs(this half v) => (half)Math.Abs((float)v);
    [MethodImpl(256 | 512)]
    public static float abs(this float v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static double abs(this double v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static decimal abs(this decimal v) => Math.Abs(v);

    #endregion

    #region Sign

    [MethodImpl(256 | 512)]
    public static byte sign(this byte v) => (byte)(v == 0 ? 0 : 1);
    [MethodImpl(256 | 512)]
    public static sbyte sign(this sbyte v) => (sbyte)Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static ushort sign(this ushort v) => (ushort)(v == 0 ? 0 : 1);
    [MethodImpl(256 | 512)]
    public static short sign(this short v) => (short)Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static uint sign(this uint v) => v == 0u ? 0u : 1u;
    [MethodImpl(256 | 512)]
    public static int sign(this int v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static ulong sign(this ulong v) => v == 0ul ? 0ul : 1ul;
    [MethodImpl(256 | 512)]
    public static long sign(this long v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static half sign(this half v) => (half)Math.Sign((float)v);
    [MethodImpl(256 | 512)]
    public static float sign(this float v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static double sign(this double v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static decimal sign(this decimal v) => Math.Sign(v);

    #endregion

    #region Ceil

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half ceil(this half v) => half.Ceiling(v);
    #else
    public static half ceil(this half v) => (half)MathF.Ceiling(v);
    #endif
    [MethodImpl(256 | 512)]
    public static float ceil(this float v) => MathF.Ceiling(v);
    [MethodImpl(256 | 512)]
    public static double ceil(this double v) => Math.Ceiling(v);
    [MethodImpl(256 | 512)]
    public static decimal ceil(this decimal v) => Math.Ceiling(v);

    #endregion

    #region Floor

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half floor(this half v) => half.Floor(v);
    #else
    public static half floor(this half v) => (half)MathF.Floor(v);
    #endif
    [MethodImpl(256 | 512)]
    public static float floor(this float v) => MathF.Floor(v);
    [MethodImpl(256 | 512)]
    public static double floor(this double v) => Math.Floor(v);
    [MethodImpl(256 | 512)]
    public static decimal floor(this decimal v) => Math.Floor(v);

    #endregion

    #region Round

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half round(this half v) => half.Round(v);
    #else
    public static half round(this half v) => (half)MathF.Round(v);
    #endif
    [MethodImpl(256 | 512)]
    public static float round(this float v) => MathF.Round(v);
    [MethodImpl(256 | 512)]
    public static double round(this double v) => Math.Round(v);
    [MethodImpl(256 | 512)]
    public static decimal round(this decimal v) => Math.Round(v);

    #endregion

    #region Trunc

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half trunc(this half v) => half.Truncate(v);
    #else
    public static half trunc(this half v) => (half)MathF.Truncate(v);
    #endif
    [MethodImpl(256 | 512)]
    public static float trunc(this float v) => MathF.Truncate(v);
    [MethodImpl(256 | 512)]
    public static double trunc(this double v) => Math.Truncate(v);
    [MethodImpl(256 | 512)]
    public static decimal trunc(this decimal v) => Math.Truncate(v);

    #endregion

    #region Frac

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half frac(this half v) => v - half.Floor(v);
    #else
    public static half frac(this half v) => (half)(v - MathF.Floor(v));
    #endif
    [MethodImpl(256 | 512)]
    public static float frac(this float v) => v - MathF.Floor(v);
    [MethodImpl(256 | 512)]
    public static double frac(this double v) => v - Math.Floor(v);
    [MethodImpl(256 | 512)]
    public static decimal frac(this decimal v) => v - Math.Floor(v);

    #endregion

    #region Modf

    [MethodImpl(256 | 512)]
    public static half modf(this half v, out half i)
    {
        i = trunc(v);
        return (half)(v - i);
    }
    [MethodImpl(256 | 512)]
    public static float modf(this float v, out float i)
    {
        i = trunc(v);
        return v - i;
    }
    [MethodImpl(256 | 512)]
    public static double modf(this double v, out double i)
    {
        i = trunc(v);
        return v - i;
    }
    [MethodImpl(256 | 512)]
    public static decimal modf(this decimal v, out decimal i)
    {
        i = trunc(v);
        return v - i;
    }

    #endregion

    #region Mod

    [MethodImpl(256 | 512)]
    public static half mod(this half a, half b) => (half)mod((float)a, (float)b);
    [MethodImpl(256 | 512)]
    public static float mod(this float a, float b) => a - b * MathF.Floor(a / b);
    [MethodImpl(256 | 512)]
    public static double mod(this double a, double b) => a - b * Math.Floor(a / b);
    [MethodImpl(256 | 512)]
    public static decimal mod(this decimal a, decimal b) => a - b * decimal.Floor(a / b);

    #endregion

    #region Min

    [MethodImpl(256 | 512)]
    public static byte min(this byte a, byte b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static sbyte min(this sbyte a, sbyte b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static ushort min(this ushort a, ushort b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static short min(this short a, short b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static uint min(this uint a, uint b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static int min(this int a, int b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static ulong min(this ulong a, ulong b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static long min(this long a, long b) => Math.Min(a, b);
    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half min(this half a, half b) => half.Min(a, b);
    #else
    [MethodImpl(256 | 512)]
    public static half min(this half a, half b) => (half)Math.Min(a, b);
    #endif
    [MethodImpl(256 | 512)]
    public static float min(this float a, float b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static double min(this double a, double b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static decimal min(this decimal a, decimal b) => Math.Min(a, b);

    #endregion

    #region Max

    [MethodImpl(256 | 512)]
    public static byte max(this byte a, byte b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static sbyte max(this sbyte a, sbyte b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static ushort max(this ushort a, ushort b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static short max(this short a, short b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static uint max(this uint a, uint b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static int max(this int a, int b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static ulong max(this ulong a, ulong b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static long max(this long a, long b) => Math.Max(a, b);
    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half max(this half a, half b) => half.Max(a, b);
    #else
    [MethodImpl(256 | 512)]
    public static half max(this half a, half b) => (half)Math.Max(a, b);
    #endif
    [MethodImpl(256 | 512)]
    public static float max(this float a, float b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static double max(this double a, double b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static decimal max(this decimal a, decimal b) => Math.Max(a, b);

    #endregion

    #region Clamp

    [MethodImpl(256 | 512)]
    public static byte clamp(this byte v, byte min, byte max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static sbyte clamp(this sbyte v, sbyte min, sbyte max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static ushort clamp(this ushort v, ushort min, ushort max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static short clamp(this short v, short min, short max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static uint clamp(this uint v, uint min, uint max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static int clamp(this int v, int min, int max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static ulong clamp(this ulong v, ulong min, ulong max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static long clamp(this long v, long min, long max) => Math.Max(min, Math.Min(max, v));
    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half clamp(this half v, half min, half max) => half.Max(min, half.Min(max, v));
    #else
    [MethodImpl(256 | 512)]
    public static half clamp(this half v, half min, half max) => (half)Math.Max(min, Math.Min(max, v));
    #endif
    [MethodImpl(256 | 512)]
    public static float clamp(this float v, float min, float max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static double clamp(this double v, double min, double max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static decimal clamp(this decimal v, decimal min, decimal max) => Math.Max(min, Math.Min(max, v));

    #endregion

    #region Lerp

    [MethodImpl(256 | 512)]
    public static sbyte lerp(this sbyte t, sbyte start, sbyte end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, (sbyte)(end - start), start);
        #else // NET8_0_OR_GREATER
        return (sbyte)(start + t * (end - start));
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static byte lerp(this byte t, byte start, byte end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, (byte)(end - start), start);
        #else // NET8_0_OR_GREATER
        return (byte)(start + t * (end - start));
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static short lerp(this short t, short start, short end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, (short)(end - start), start);
        #else // NET8_0_OR_GREATER
        return (short)(start + t * (end - start));
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static ushort lerp(this ushort t, ushort start, ushort end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, (ushort)(end - start), start);
        #else // NET8_0_OR_GREATER
        return (ushort)(start + t * (end - start));
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static int lerp(this int t, int start, int end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static uint lerp(this uint t, uint start, uint end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static long lerp(this long t, long start, long end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static ulong lerp(this ulong t, ulong start, ulong end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static half lerp(this half t, half start, half end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return (half)(start + t * (end - start));
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static float lerp(this float t, float start, float end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static double lerp(this double t, double start, double end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    [MethodImpl(256 | 512)]
    public static decimal lerp(this decimal t, decimal start, decimal end)
    {
        #if NET8_0_OR_GREATER
        return fma(t, end - start, start);
        #else // NET8_0_OR_GREATER
        return start + t * (end - start);
        #endif // NET8_0_OR_GREATER
    }

    #endregion

    #region Unlerp

    [MethodImpl(256 | 512)]
    public static byte unlerp(this byte a, byte start, byte end) => (byte)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static sbyte unlerp(this sbyte a, sbyte start, sbyte end) => (sbyte)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static ushort unlerp(this ushort a, ushort start, ushort end) => (ushort)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static short unlerp(this short a, short start, short end) => (short)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static uint unlerp(this uint a, uint start, uint end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static int unlerp(this int a, int start, int end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static ulong unlerp(this ulong a, ulong start, ulong end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static long unlerp(this long a, long start, long end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static half unlerp(this half a, half start, half end) => (half)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static float unlerp(this float a, float start, float end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static double unlerp(this double a, double start, double end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static decimal unlerp(this decimal a, decimal start, decimal end) => (a - start) / (end - start);

    #endregion

    #region Remap

    [MethodImpl(256 | 512)]
    public static byte remap(this byte a, byte srcStart, byte srcEnd, byte dstStart, byte dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static sbyte remap(this sbyte a, sbyte srcStart, sbyte srcEnd, sbyte dstStart, sbyte dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static ushort remap(this ushort a, ushort srcStart, ushort srcEnd, ushort dstStart, ushort dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static short remap(this short a, short srcStart, short srcEnd, short dstStart, short dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static uint remap(this uint a, uint srcStart, uint srcEnd, uint dstStart, uint dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static int remap(this int a, int srcStart, int srcEnd, int dstStart, int dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static ulong remap(this ulong a, ulong srcStart, ulong srcEnd, ulong dstStart, ulong dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static long remap(this long a, long srcStart, long srcEnd, long dstStart, long dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static half remap(this half a, half srcStart, half srcEnd, half dstStart, half dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static float remap(this float a, float srcStart, float srcEnd, float dstStart, float dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static double remap(this double a, double srcStart, double srcEnd, double dstStart, double dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static decimal remap(this decimal a, decimal srcStart, decimal srcEnd, decimal dstStart, decimal dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    #endregion

    #region step

    [MethodImpl(256 | 512)]
    public static byte step(this byte a, byte threshold) => select(a >= threshold, (byte)1, (byte)0);
    
    [MethodImpl(256 | 512)]
    public static sbyte step(this sbyte a, sbyte threshold) => select(a >= threshold, (sbyte)1, (sbyte)0);
    
    [MethodImpl(256 | 512)]
    public static ushort step(this ushort a, ushort threshold) => select(a >= threshold, (ushort)1, (ushort)0);
    
    [MethodImpl(256 | 512)]
    public static short step(this short a, short threshold) => select(a >= threshold, (short)1, (short)0);
    
    [MethodImpl(256 | 512)]
    public static uint step(this uint a, uint threshold) => select(a >= threshold, 1u, 0u);
    
    [MethodImpl(256 | 512)]
    public static int step(this int a, int threshold) => select(a >= threshold, 1, 0);
    
    [MethodImpl(256 | 512)]
    public static ulong step(this ulong a, ulong threshold) => select(a >= threshold, 1UL, 0UL);
    
    [MethodImpl(256 | 512)]
    public static long step(this long a, long threshold) => select(a >= threshold, 1L, 0L);
    
    [MethodImpl(256 | 512)]
    public static half step(this half a, half threshold) => select(a >= threshold, (half)1f, (half)0f);
    
    [MethodImpl(256 | 512)]
    public static float step(this float a, float threshold) => select(a >= threshold, 1f, 0f);
    
    [MethodImpl(256 | 512)]
    public static double step(this double a, double threshold) => select(a >= threshold, 1d, 0d);

    #endregion

    #region SmoothStep

    [MethodImpl(256 | 512)]
    public static half smoothstep(this half a, half min, half max)
    {
        var t = saturate(((float)a - (float)min) / ((float)max - (float)min));
        return (half)(t * t * (3.0f - (2.0f * t)));
    }

    [MethodImpl(256 | 512)]
    public static float smoothstep(this float a, float min, float max)
    {
        var t = saturate((a - min) / (max - min));
        return t * t * (3.0f - (2.0f * t));
    }

    [MethodImpl(256 | 512)]
    public static double smoothstep(this double a, double min, double max)
    {
        var t = saturate((a - min) / (max - min));
        return t * t * (3.0 - (2.0 * t));
    }

    [MethodImpl(256 | 512)]
    public static decimal smoothstep(this decimal a, decimal min, decimal max)
    {
        var t = saturate((a - min) / (max - min));
        return t * t * (3.0m - (2.0m * t));
    }

    #endregion

    #region rcp

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half rcp(this half v) => half.One / v;
    #else
    public static half rcp(this half v) => (half)(1 / v);
    #endif
    [MethodImpl(256 | 512)]
    public static float rcp(this float v) => 1.0f / v;
    [MethodImpl(256 | 512)]
    public static double rcp(this double v) => 1.0 / v;
    [MethodImpl(256 | 512)]
    public static decimal rcp(this decimal v) => 1.0m / v;

    #endregion

    #region fma

    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half fma(this half a, half b, half c) => half.FusedMultiplyAdd(a, b, c);
    [MethodImpl(256 | 512)]
    public static float fma(this float a, float b, float c) => MathF.FusedMultiplyAdd(a, b, c);
    [MethodImpl(256 | 512)]
    public static double fma(this double a, double b, double c) => Math.FusedMultiplyAdd(a, b, c);
    #else
    [MethodImpl(256 | 512)]
    public static half fma(this half a, half b, half c) => (half)(a * b + c);
    [MethodImpl(256 | 512)]
    public static float fma(this float a, float b, float c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static double fma(this double a, double b, double c) => a * b + c;
    #endif

    [MethodImpl(256 | 512)]
    public static byte fma(this byte a, byte b, byte c) => (byte)(a * b + c);
    [MethodImpl(256 | 512)]
    public static sbyte fma(this sbyte a, sbyte b, sbyte c) => (sbyte)(a * b + c);
    [MethodImpl(256 | 512)]
    public static ushort fma(this ushort a, ushort b, ushort c) => (ushort)(a * b + c);
    [MethodImpl(256 | 512)]
    public static short fma(this short a, short b, short c) => (short)(a * b + c);
    [MethodImpl(256 | 512)]
    public static uint fma(this uint a, uint b, uint c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static int fma(this int a, int b, int c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static ulong fma(this ulong a, ulong b, ulong c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static long fma(this long a, long b, long c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static decimal fma(this decimal a, decimal b, decimal c) => a * b + c;

    #endregion

    #region fms

    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half fms(this half a, half b, half c) => (half)fms((float)a, (float)b, (float)c);
    [MethodImpl(256 | 512)]
    public static float fms(this float a, float b, float c) => MathF.FusedMultiplyAdd(a, b, -c);
    [MethodImpl(256 | 512)]
    public static double fms(this double a, double b, double c) => Math.FusedMultiplyAdd(a, b, -c);
    #else
    [MethodImpl(256 | 512)]
    public static half fms(this half a, half b, half c) => (half)(a * b - c);
    [MethodImpl(256 | 512)]
    public static float fms(this float a, float b, float c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static double fms(this double a, double b, double c) => a * b - c;
    #endif

    [MethodImpl(256 | 512)]
    public static ushort fms(this ushort a, ushort b, ushort c) => (ushort)(a * b - c);
    [MethodImpl(256 | 512)]
    public static short fms(this short a, short b, short c) => (short)(a * b - c);
    [MethodImpl(256 | 512)]
    public static uint fms(this uint a, uint b, uint c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static int fms(this int a, int b, int c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static ulong fms(this ulong a, ulong b, ulong c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static long fms(this long a, long b, long c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static decimal fms(this decimal a, decimal b, decimal c) => a * b - c;

    #endregion

    #region fnma

    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half fnma(this half a, half b, half c) => (half)fnma((float)a, (float)b, (float)c);
    [MethodImpl(256 | 512)]
    public static float fnma(this float a, float b, float c) => MathF.FusedMultiplyAdd(-a, b, c);
    [MethodImpl(256 | 512)]
    public static double fnma(this double a, double b, double c) => Math.FusedMultiplyAdd(-a, b, c);
    #else
    [MethodImpl(256 | 512)]
    public static half fnma(this half a, half b, half c) => (half)(c - a * b);
    [MethodImpl(256 | 512)]
    public static float fnma(this float a, float b, float c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static double fnma(this double a, double b, double c) => c - a * b;
    #endif

    [MethodImpl(256 | 512)]
    public static ushort fnma(this ushort a, ushort b, ushort c) => (ushort)(c - a * b);
    [MethodImpl(256 | 512)]
    public static short fnma(this short a, short b, short c) => (short)(c - a * b);
    [MethodImpl(256 | 512)]
    public static uint fnma(this uint a, uint b, uint c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static int fnma(this int a, int b, int c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static ulong fnma(this ulong a, ulong b, ulong c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static long fnma(this long a, long b, long c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static decimal fnma(this decimal a, decimal b, decimal c) => c - a * b;

    #endregion

    #region fsm

    [MethodImpl(256 | 512)]
    public static half fsm(this half c, half a, half b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static float fsm(this float c, float a, float b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static double fsm(this double c, double a, double b) => fnma(a, b, c);

    [MethodImpl(256 | 512)]
    public static ushort fsm(this ushort c, ushort a, ushort b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static short fsm(this short c, short a, short b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static uint fsm(this uint c, uint a, uint b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static int fsm(this int c, int a, int b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ulong fsm(this ulong c, ulong a, ulong b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static long fsm(this long c, long a, long b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static decimal fsm(this decimal c, decimal a, decimal b) => fnma(a, b, c);

    #endregion

    #region fam

    [MethodImpl(256 | 512)]
    public static half fam(this half c, half a, half b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static float fam(this float c, float a, float b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static double fam(this double c, double a, double b) => fma(a, b, c);

    [MethodImpl(256 | 512)]
    public static byte fam(this byte c, byte a, byte b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static sbyte fam(this sbyte c, sbyte a, sbyte b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ushort fam(this ushort c, ushort a, ushort b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static short fam(this short c, short a, short b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static uint fam(this uint c, uint a, uint b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static int fam(this int c, int a, int b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ulong fam(this ulong c, ulong a, ulong b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static long fam(this long c, long a, long b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static decimal fam(this decimal c, decimal a, decimal b) => fma(a, b, c);

    #endregion

    #region mad

    [MethodImpl(256 | 512)]
    public static half mad(this half a, half b, half c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static float mad(this float a, float b, float c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static double mad(this double a, double b, double c) => fma(a, b, c);

    [MethodImpl(256 | 512)]
    public static byte mad(this byte a, byte b, byte c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static sbyte mad(this sbyte a, sbyte b, sbyte c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ushort mad(this ushort a, ushort b, ushort c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static short mad(this short a, short b, short c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static uint mad(this uint a, uint b, uint c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static int mad(this int a, int b, int c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ulong mad(this ulong a, ulong b, ulong c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static long mad(this long a, long b, long c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static decimal mad(this decimal a, decimal b, decimal c) => fma(a, b, c);

    #endregion

    #region Log

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half log(this half a) => half.Log(a);
    #else
    public static half log(this half a) => (half)MathF.Log(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float log(this float a) => MathF.Log(a);
    [MethodImpl(256 | 512)]
    public static double log(this double a) => Math.Log(a);

    #endregion

    #region Log2

    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half log2(this half a) => half.Log2(a);
    [MethodImpl(256 | 512)]
    public static float log2(this float a) => MathF.Log2(a);
    [MethodImpl(256 | 512)]
    public static double log2(this double a) => Math.Log2(a);
    #else
    [MethodImpl(256 | 512)]
    public static half log2(this half a) => (half)(MathF.Log(a) / math.F_Log2);
    [MethodImpl(256 | 512)]
    public static float log2(this float a) => MathF.Log(a) / math.F_Log2;
    [MethodImpl(256 | 512)]
    public static double log2(this double a) => Math.Log(a) / math.D_Log2;
    #endif

    #endregion

    #region Log10

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half log10(this half a) => half.Log10(a);
    #else
    public static half log10(this half a) => (half)MathF.Log10(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float log10(this float a) => MathF.Log10(a);
    [MethodImpl(256 | 512)]
    public static double log10(this double a) => Math.Log10(a);

    #endregion

    #region Log N

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half log(this half a, half b) => half.Log(a, b);
    #else
    public static half log(this half a, half b) => (half)MathF.Log(a, b);
    #endif
    [MethodImpl(256 | 512)]
    public static float log(this float a, float b) => MathF.Log(a, b);
    [MethodImpl(256 | 512)]
    public static double log(this double a, double b) => Math.Log(a, b);

    #endregion

    #region Exp

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half exp(this half a) => half.Exp(a);
    #else
    public static half exp(this half a) => (half)MathF.Exp(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float exp(this float a) => MathF.Exp(a);
    [MethodImpl(256 | 512)]
    public static double exp(this double a) => Math.Exp(a);

    #endregion

    #region Exp2

    [MethodImpl(256 | 512)]
    public static half exp2(this half a) => (half)MathF.Exp((float)a * 0.693147180559945309f);
    [MethodImpl(256 | 512)]
    public static float exp2(this float a) => MathF.Exp(a * 0.693147180559945309f);
    [MethodImpl(256 | 512)]
    public static double exp2(this double a) => Math.Exp(a * 0.693147180559945309);

    #endregion

    #region Exp10

    [MethodImpl(256 | 512)]
    public static half exp10(this half a) => (half)MathF.Exp((float)a * 2.302585092994045684f);
    [MethodImpl(256 | 512)]
    public static float exp10(this float a) => MathF.Exp(a * 2.302585092994045684f);
    [MethodImpl(256 | 512)]
    public static double exp10(this double a) => Math.Exp(a * 2.302585092994045684);

    #endregion

    #region Pow

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half pow(this half a, half b) => half.Pow(a, b);
    #else
    public static half pow(this half a, half b) => (half)MathF.Pow(a, b);
    #endif
    [MethodImpl(256 | 512)]
    public static float pow(this float a, float b) => MathF.Pow(a, b);
    [MethodImpl(256 | 512)]
    public static double pow(this double a, double b) => Math.Pow(a, b);

    #endregion

    #region IsInf

    [MethodImpl(256 | 512)]
    public static bool isInf(this half a) => half.IsInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isInf(this float a) => float.IsInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isInf(this double a) => double.IsInfinity(a);

    #endregion

    #region IsPInf

    [MethodImpl(256 | 512)]
    public static bool isPosInf(this half a) => half.IsPositiveInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isPosInf(this float a) => float.IsPositiveInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isPosInf(this double a) => double.IsPositiveInfinity(a);

    #endregion

    #region IsNegInf

    [MethodImpl(256 | 512)]
    public static bool isNegInf(this half a) => half.IsNegativeInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isNegInf(this float a) => float.IsNegativeInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isNegInf(this double a) => double.IsNegativeInfinity(a);

    #endregion

    #region Saturate

    [MethodImpl(256 | 512)]
    public static half saturate(this half v) => v.clamp((half)0f, (half)1f);
    [MethodImpl(256 | 512)]
    public static float saturate(this float v) => v.clamp(0f, 1f);
    [MethodImpl(256 | 512)]
    public static double saturate(this double v) => v.clamp(0, 1);
    [MethodImpl(256 | 512)]
    public static decimal saturate(this decimal v) => v.clamp(0m, 1m);

    #endregion

    #region Dot

    [MethodImpl(256 | 512)]
    public static byte dot(this byte a, byte b) => (byte)(a * b);
    [MethodImpl(256 | 512)]
    public static sbyte dot(this sbyte a, sbyte b) => (sbyte)(a * b);
    [MethodImpl(256 | 512)]
    public static ushort dot(this ushort a, ushort b) => (ushort)(a * b);
    [MethodImpl(256 | 512)]
    public static short dot(this short a, short b) => (short)(a * b);
    [MethodImpl(256 | 512)]
    public static uint dot(this uint a, uint b) => a * b;
    [MethodImpl(256 | 512)]
    public static int dot(this int a, int b) => a * b;
    [MethodImpl(256 | 512)]
    public static ulong dot(this ulong a, ulong b) => a * b;
    [MethodImpl(256 | 512)]
    public static long dot(this long a, long b) => a * b;
    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static half dot(this half a, half b) => a * b;
    #else
    [MethodImpl(256 | 512)]
    public static half dot(this half a, half b) => (half)(a * b);
    #endif
    [MethodImpl(256 | 512)]
    public static float dot(this float a, float b) => a * b;
    [MethodImpl(256 | 512)]
    public static double dot(this double a, double b) => a * b;
    [MethodImpl(256 | 512)]
    public static decimal dot(this decimal a, decimal b) => a * b;

    #endregion

    #region Sqrt

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half sqrt(this half v) => half.Sqrt(v);
    #else
    public static half sqrt(this half v) => (half)MathF.Sqrt(v);
    #endif
    [MethodImpl(256 | 512)]
    public static float sqrt(this float v) => MathF.Sqrt(v);
    [MethodImpl(256 | 512)]
    public static double sqrt(this double v) => Math.Sqrt(v);

    #endregion

    #region RSqrt

    [MethodImpl(256 | 512)]
    public static half rsqrt(this half v) => (half)(1 / sqrt((float)v));
    [MethodImpl(256 | 512)]
    public static float rsqrt(this float v) => 1 / sqrt(v);
    [MethodImpl(256 | 512)]
    public static double rsqrt(this double v) => 1 / sqrt(v);

    #endregion

    #region LengthSq

    [MethodImpl(256 | 512)]
    public static byte lengthsq(this byte v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static sbyte lengthsq(this sbyte v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static ushort lengthsq(this ushort v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static short lengthsq(this short v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static uint lengthsq(this uint v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static int lengthsq(this int v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static ulong lengthsq(this ulong v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static long lengthsq(this long v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static half lengthsq(this half v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static float lengthsq(this float v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static double lengthsq(this double v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static decimal lengthsq(this decimal v) => dot(v, v);

    #endregion

    #region Length

    [MethodImpl(256 | 512)]
    public static byte length(this byte v) => abs(v);
    [MethodImpl(256 | 512)]
    public static sbyte length(this sbyte v) => abs(v);
    [MethodImpl(256 | 512)]
    public static ushort length(this ushort v) => abs(v);
    [MethodImpl(256 | 512)]
    public static short length(this short v) => abs(v);
    [MethodImpl(256 | 512)]
    public static uint length(this uint v) => abs(v);
    [MethodImpl(256 | 512)]
    public static int length(this int v) => abs(v);
    [MethodImpl(256 | 512)]
    public static ulong length(this ulong v) => abs(v);
    [MethodImpl(256 | 512)]
    public static long length(this long v) => abs(v);
    [MethodImpl(256 | 512)]
    public static half length(this half v) => abs(v);
    [MethodImpl(256 | 512)]
    public static float length(this float v) => abs(v);
    [MethodImpl(256 | 512)]
    public static double length(this double v) => abs(v);
    [MethodImpl(256 | 512)]
    public static decimal length(this decimal v) => abs(v);

    #endregion

    #region DistanceSq

    [MethodImpl(256 | 512)]
    public static byte distancesq(this byte a, byte b) => (byte)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static sbyte distancesq(this sbyte a, sbyte b) => (sbyte)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static ushort distancesq(this ushort a, ushort b) => (ushort)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static short distancesq(this short a, short b) => (short)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static uint distancesq(this uint a, uint b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static int distancesq(this int a, int b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static ulong distancesq(this ulong a, ulong b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static long distancesq(this long a, long b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static half distancesq(this half a, half b) => (half)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static float distancesq(this float a, float b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static double distancesq(this double a, double b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static decimal distancesq(this decimal a, decimal b) => lengthsq(b - a);

    #endregion

    #region Distance

    [MethodImpl(256 | 512)]
    public static byte distance(this byte a, byte b) => (byte)abs(b - a);
    [MethodImpl(256 | 512)]
    public static sbyte distance(this sbyte a, sbyte b) => (sbyte)abs(b - a);
    [MethodImpl(256 | 512)]
    public static ushort distance(this ushort a, ushort b) => (ushort)abs(b - a);
    [MethodImpl(256 | 512)]
    public static short distance(this short a, short b) => (short)abs(b - a);
    [MethodImpl(256 | 512)]
    public static uint distance(this uint a, uint b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static int distance(this int a, int b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static ulong distance(this ulong a, ulong b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static long distance(this long a, long b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static half distance(this half a, half b) => (half)abs(b - a);
    [MethodImpl(256 | 512)]
    public static float distance(this float a, float b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static double distance(this double a, double b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static decimal distance(this decimal a, decimal b) => abs(b - a);

    #endregion

    #region Sin

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half sin(this half a) => half.Sin(a);
    #else
    public static half sin(this half a) => (half)MathF.Sin(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float sin(this float a) => MathF.Sin(a);
    [MethodImpl(256 | 512)]
    public static double sin(this double a) => Math.Sin(a);

    #endregion

    #region Cos

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half cos(this half a) => half.Cos(a);
    #else
    public static half cos(this half a) => (half)MathF.Cos(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float cos(this float a) => MathF.Cos(a);
    [MethodImpl(256 | 512)]
    public static double cos(this double a) => Math.Cos(a);

    #endregion

    #region SinCos

    #if NET8_0_OR_GREATER
    [MethodImpl(256 | 512)]
    public static (half sin, half cos) sincos(this half a) => half.SinCos(a);
    [MethodImpl(256 | 512)]
    public static (float sin, float cos) sincos(this float a) => MathF.SinCos(a);
    [MethodImpl(256 | 512)]
    public static (double sin, double cos) sincos(this double a) => Math.SinCos(a);
    #else
    [MethodImpl(256 | 512)]
    public static (half sin, half cos) sincos(this half a) => ((half)MathF.Sin(a), (half)MathF.Cos(a));
    [MethodImpl(256 | 512)]
    public static (float sin, float cos) sincos(this float a) => (MathF.Sin(a), MathF.Cos(a));
    [MethodImpl(256 | 512)]
    public static (double sin, double cos) sincos(this double a) => (Math.Sin(a), Math.Cos(a));
    #endif

    #endregion

    #region SinCos out

    [MethodImpl(256 | 512)]
    public static void sincos(this half a, out half sin, out half cos) => (sin, cos) = sincos(a);
    [MethodImpl(256 | 512)]
    public static void sincos(this float a, out float sin, out float cos) => (sin, cos) = sincos(a);
    [MethodImpl(256 | 512)]
    public static void sincos(this double a, out double sin, out double cos) => (sin, cos) = sincos(a);

    #endregion

    #region Tan

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half tan(this half a) => half.Tan(a);
    #else
    public static half tan(this half a) => (half)MathF.Tan(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float tan(this float a) => MathF.Tan(a);
    [MethodImpl(256 | 512)]
    public static double tan(this double a) => Math.Tan(a);

    #endregion

    #region Asin

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half asin(this half a) => half.Asin(a);
    #else
    public static half asin(this half a) => (half)MathF.Asin(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float asin(this float a) => MathF.Asin(a);
    [MethodImpl(256 | 512)]
    public static double asin(this double a) => Math.Asin(a);

    #endregion

    #region Acos

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half acos(this half a) => half.Acos(a);
    #else
    public static half acos(this half a) => (half)MathF.Acos(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float acos(this float a) => MathF.Acos(a);
    [MethodImpl(256 | 512)]
    public static double acos(this double a) => Math.Acos(a);

    #endregion

    #region Atan

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half atan(this half a) => half.Atan(a);
    #else
    public static half atan(this half a) => (half)MathF.Atan(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float atan(this float a) => MathF.Atan(a);
    [MethodImpl(256 | 512)]
    public static double atan(this double a) => Math.Atan(a);

    #endregion

    #region Atan2

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half atan2(this half y, half x) => half.Atan2(y, x);
    #else
    public static half atan2(this half y, half x) => (half)MathF.Atan2(y, x);
    #endif
    [MethodImpl(256 | 512)]
    public static float atan2(this float y, float x) => MathF.Atan2(y, x);
    [MethodImpl(256 | 512)]
    public static double atan2(this double y, double x) => Math.Atan2(y, x);

    #endregion

    #region Sinh

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half sinh(this half a) => half.Sinh(a);
    #else
    public static half sinh(this half a) => (half)MathF.Sinh(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float sinh(this float a) => MathF.Sinh(a);
    [MethodImpl(256 | 512)]
    public static double sinh(this double a) => Math.Sinh(a);

    #endregion

    #region Cosh

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half cosh(this half a) => half.Cosh(a);
    #else
    public static half cosh(this half a) => (half)MathF.Cosh(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float cosh(this float a) => MathF.Cosh(a);
    [MethodImpl(256 | 512)]
    public static double cosh(this double a) => Math.Cosh(a);

    #endregion

    #region Tanh

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half tanh(this half a) => half.Tanh(a);
    #else
    public static half tanh(this half a) => (half)MathF.Tanh(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float tanh(this float a) => MathF.Tanh(a);
    [MethodImpl(256 | 512)]
    public static double tanh(this double a) => Math.Tanh(a);

    #endregion

    #region Asinh

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half asinh(this half a) => half.Asinh(a);
    #else
    public static half asinh(this half a) => (half)MathF.Asinh(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float asinh(this float a) => MathF.Asinh(a);
    [MethodImpl(256 | 512)]
    public static double asinh(this double a) => Math.Asinh(a);

    #endregion

    #region Acosh

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half acosh(this half a) => half.Acosh(a);
    #else
    public static half acosh(this half a) => (half)MathF.Acosh(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float acosh(this float a) => MathF.Acosh(a);
    [MethodImpl(256 | 512)]
    public static double acosh(this double a) => Math.Acosh(a);

    #endregion

    #region Atanh

    [MethodImpl(256 | 512)]
    #if NET8_0_OR_GREATER
    public static half atanh(this half a) => half.Atanh(a);
    #else
    public static half atanh(this half a) => (half)MathF.Atanh(a);
    #endif
    [MethodImpl(256 | 512)]
    public static float atanh(this float a) => MathF.Atanh(a);
    [MethodImpl(256 | 512)]
    public static double atanh(this double a) => Math.Atanh(a);

    #endregion

    #region radians

    [MethodImpl(256 | 512)]
    public static half radians(this half a) => (half)((float)a * F_DegToRad);

    [MethodImpl(256 | 512)]
    public static float radians(this float a) => a * F_DegToRad;

    [MethodImpl(256 | 512)]
    public static double radians(this double a) => a * D_DegToRad;

    #endregion

    #region degrees

    [MethodImpl(256 | 512)]
    public static half degrees(this half a) => (half)((float)a * F_RadToDeg);

    [MethodImpl(256 | 512)]
    public static float degrees(this float a) => a * F_RadToDeg;

    [MethodImpl(256 | 512)]
    public static double degrees(this double a) => a * D_RadToDeg;

    #endregion

    #region wrap

    [MethodImpl(256 | 512)]
    public static half wrap(this half x, half min, half max) => (half)wrap((float)x, (float)min, (float)max);

    [MethodImpl(256 | 512)]
    public static float wrap(this float x, float min, float max)
    {
        var range = max - min;
        return min + ((x - min) % range + range) % range;
    }
    
    [MethodImpl(256 | 512)]
    public static double wrap(this double x, double min, double max) 
    {
        var range = max - min;
        return min + ((x - min) % range + range) % range;
    }

    #endregion
}
