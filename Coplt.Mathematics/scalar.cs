namespace Coplt.Mathematics;

[Ex]
public static partial class math
{
    #region asf

    [MethodImpl(256 | 512)]
    public static half asf([This] ushort v) => v.BitCast<ushort, half>();
    [MethodImpl(256 | 512)]
    public static half asf([This] short v) => v.BitCast<short, half>();
    [MethodImpl(256 | 512)]
    public static half asf([This] b16 v) => v.BitCast<b16, half>();
    [MethodImpl(256 | 512)]
    public static float asf([This] uint v) => v.BitCast<uint, float>();
    [MethodImpl(256 | 512)]
    public static float asf([This] int v) => v.BitCast<int, float>();
    [MethodImpl(256 | 512)]
    public static float asf([This] b32 v) => v.BitCast<b32, float>();
    [MethodImpl(256 | 512)]
    public static double asf([This] ulong v) => v.BitCast<ulong, double>();
    [MethodImpl(256 | 512)]
    public static double asf([This] long v) => v.BitCast<long, double>();
    [MethodImpl(256 | 512)]
    public static double asf([This] b64 v) => v.BitCast<b64, double>();
    [MethodImpl(256 | 512)]
    public static half asf([This] half v) => v;
    [MethodImpl(256 | 512)]
    public static float asf([This] float v) => v;
    [MethodImpl(256 | 512)]
    public static double asf([This] double v) => v;

    #endregion

    #region asu

    [MethodImpl(256 | 512)]
    public static ushort asu([This] ushort v) => v;
    [MethodImpl(256 | 512)]
    public static ushort asu([This] short v) => (ushort)v;
    [MethodImpl(256 | 512)]
    public static ushort asu([This] b16 v) => v;
    [MethodImpl(256 | 512)]
    public static uint asu([This] uint v) => v;
    [MethodImpl(256 | 512)]
    public static uint asu([This] int v) => (uint)v;
    [MethodImpl(256 | 512)]
    public static uint asu([This] b32 v) => v;
    [MethodImpl(256 | 512)]
    public static ulong asu([This] ulong v) => v;
    [MethodImpl(256 | 512)]
    public static ulong asu([This] long v) => (ulong)v;
    [MethodImpl(256 | 512)]
    public static ulong asu([This] b64 v) => v;
    [MethodImpl(256 | 512)]
    public static ushort asu([This] half v) => v.BitCast<half, ushort>();
    [MethodImpl(256 | 512)]
    public static uint asu([This] float v) => v.BitCast<float, uint>();
    [MethodImpl(256 | 512)]
    public static ulong asu([This] double v) => v.BitCast<double, ulong>();

    #endregion

    #region asi

    [MethodImpl(256 | 512)]
    public static short asi([This] ushort v) => (short)v;
    [MethodImpl(256 | 512)]
    public static short asi([This] short v) => v;
    [MethodImpl(256 | 512)]
    public static short asi([This] b16 v) => (short)(ushort)v;
    [MethodImpl(256 | 512)]
    public static int asi([This] uint v) => (int)v;
    [MethodImpl(256 | 512)]
    public static int asi([This] int v) => v;
    [MethodImpl(256 | 512)]
    public static uint asi([This] b32 v) => (uint)(int)v;
    [MethodImpl(256 | 512)]
    public static long asi([This] ulong v) => (long)v;
    [MethodImpl(256 | 512)]
    public static long asi([This] long v) => v;
    [MethodImpl(256 | 512)]
    public static long asi([This] b64 v) => (long)(ulong)v;
    [MethodImpl(256 | 512)]
    public static short asi([This] half v) => v.BitCast<half, short>();
    [MethodImpl(256 | 512)]
    public static int asi([This] float v) => v.BitCast<float, int>();
    [MethodImpl(256 | 512)]
    public static long asi([This] double v) => v.BitCast<double, long>();

    #endregion

    #region popcnt

    [MethodImpl(256 | 512)]
    public static short popcnt([This] half c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short popcnt([This] short c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short popcnt([This] ushort c) => (short)popcnt((uint)c);

    [MethodImpl(256 | 512)]
    public static int popcnt([This] float c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt([This] int c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt([This] uint c) => BitOperations.PopCount(c);

    [MethodImpl(256 | 512)]
    public static int popcnt([This] double c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt([This] long c) => popcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int popcnt([This] ulong c) => BitOperations.PopCount(c);

    #endregion

    #region lzcnt

    [MethodImpl(256 | 512)]
    public static short lzcnt([This] half c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short lzcnt([This] short c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short lzcnt([This] ushort c) => (short)lzcnt((uint)c);

    [MethodImpl(256 | 512)]
    public static int lzcnt([This] float c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt([This] int c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt([This] uint c) => BitOperations.LeadingZeroCount(c);

    [MethodImpl(256 | 512)]
    public static int lzcnt([This] double c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt([This] long c) => lzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int lzcnt([This] ulong c) => BitOperations.LeadingZeroCount(c);

    #endregion

    #region tzcnt

    [MethodImpl(256 | 512)]
    public static short tzcnt([This] half c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short tzcnt([This] short c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static short tzcnt([This] ushort c) => (short)tzcnt((uint)c);

    [MethodImpl(256 | 512)]
    public static int tzcnt([This] float c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt([This] int c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt([This] uint c) => BitOperations.TrailingZeroCount(c);

    [MethodImpl(256 | 512)]
    public static int tzcnt([This] double c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt([This] long c) => tzcnt(c.asu());

    [MethodImpl(256 | 512)]
    public static int tzcnt([This] ulong c) => BitOperations.TrailingZeroCount(c);

    #endregion

    #region up2pow2

    [MethodImpl(256 | 512)]
    public static ushort up2pow2([This] ushort c) => (ushort)up2pow2((uint)c);

    [MethodImpl(256 | 512)]
    public static uint up2pow2([This] uint c) => BitOperations.RoundUpToPowerOf2(c);

    [MethodImpl(256 | 512)]
    public static ulong up2pow2([This] ulong c) => BitOperations.RoundUpToPowerOf2(c);

    #endregion

    #region isPow2

    [MethodImpl(256 | 512)]
    public static bool isPow2([This] short c) => isPow2((int)c);

    [MethodImpl(256 | 512)]
    public static bool isPow2([This] ushort c) => isPow2((uint)c);

    [MethodImpl(256 | 512)]
    public static bool isPow2([This] int c) => BitOperations.IsPow2(c);

    [MethodImpl(256 | 512)]
    public static bool isPow2([This] uint c) => BitOperations.IsPow2(c);

    [MethodImpl(256 | 512)]
    public static bool isPow2([This] long c) => BitOperations.IsPow2(c);

    [MethodImpl(256 | 512)]
    public static bool isPow2([This] ulong c) => BitOperations.IsPow2(c);

    #endregion

    #region select

    [MethodImpl(256 | 512)]
    public static byte select([This] bool c, byte t, byte f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static sbyte select([This] bool c, sbyte t, sbyte f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static ushort select([This] bool c, ushort t, ushort f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static short select([This] bool c, short t, short f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static uint select([This] bool c, uint t, uint f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static int select([This] bool c, int t, int f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static ulong select([This] bool c, ulong t, ulong f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static long select([This] bool c, long t, long f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static half select([This] bool c, half t, half f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static float select([This] bool c, float t, float f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static double select([This] bool c, double t, double f) => c ? t : f;
    [MethodImpl(256 | 512)]
    public static decimal select([This] bool c, decimal t, decimal f) => c ? t : f;

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
    public static b16 andnot([This] b16 a, b16 b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static b32 andnot([This] b32 a, b32 b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static b64 andnot([This] b64 a, b64 b) => a & ~b;

    [MethodImpl(256 | 512)]
    public static byte andnot([This] byte a, byte b) => (byte)(a & ~b);
    [MethodImpl(256 | 512)]
    public static sbyte andnot([This] sbyte a, sbyte b) => (sbyte)(a & ~b);
    [MethodImpl(256 | 512)]
    public static ushort andnot([This] ushort a, ushort b) => (ushort)(a & ~b);
    [MethodImpl(256 | 512)]
    public static short andnot([This] short a, short b) => (short)(a & ~b);
    [MethodImpl(256 | 512)]
    public static uint andnot([This] uint a, uint b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static int andnot([This] int a, int b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static ulong andnot([This] ulong a, ulong b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static long andnot([This] long a, long b) => a & ~b;
    [MethodImpl(256 | 512)]
    public static half andnot([This] half a, half b) => ((ushort)(a.AsUInt16() & ~b.AsUInt16())).AsHalf();
    [MethodImpl(256 | 512)]
    public static float andnot([This] float a, float b) => (a.AsUInt32() & ~b.AsUInt32()).AsSingle();
    [MethodImpl(256 | 512)]
    public static double andnot([This] double a, double b) => (a.AsUInt32() & ~b.AsUInt32()).AsDouble();

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
    public static bool isNaN([This] half v) => half.IsNaN(v);

    [MethodImpl(256 | 512)]
    public static bool isNaN([This] float v) => float.IsNaN(v);

    [MethodImpl(256 | 512)]
    public static bool isNaN([This] double v) => double.IsNaN(v);

    #endregion

    #region isFinite

    [MethodImpl(256 | 512)]
    public static bool isFinite([This] half v) => half.IsFinite(v);

    [MethodImpl(256 | 512)]
    public static bool isFinite([This] float v) => float.IsFinite(v);

    [MethodImpl(256 | 512)]
    public static bool isFinite([This] double v) => double.IsFinite(v);

    #endregion

    #region Abs

    [MethodImpl(256 | 512)]
    public static byte abs([This] byte v) => v;
    [MethodImpl(256 | 512)]
    public static sbyte abs([This] sbyte v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static ushort abs([This] ushort v) => v;
    [MethodImpl(256 | 512)]
    public static short abs([This] short v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static uint abs([This] uint v) => v;
    [MethodImpl(256 | 512)]
    public static int abs([This] int v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static ulong abs([This] ulong v) => v;
    [MethodImpl(256 | 512)]
    public static long abs([This] long v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static half abs([This] half v) => (half)Math.Abs((float)v);
    [MethodImpl(256 | 512)]
    public static float abs([This] float v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static double abs([This] double v) => Math.Abs(v);
    [MethodImpl(256 | 512)]
    public static decimal abs([This] decimal v) => Math.Abs(v);

    #endregion

    #region Sign

    [MethodImpl(256 | 512)]
    public static byte sign([This] byte v) => (byte)(v == 0 ? 0 : 1);
    [MethodImpl(256 | 512)]
    public static sbyte sign([This] sbyte v) => (sbyte)Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static ushort sign([This] ushort v) => (ushort)(v == 0 ? 0 : 1);
    [MethodImpl(256 | 512)]
    public static short sign([This] short v) => (short)Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static uint sign([This] uint v) => v == 0u ? 0u : 1u;
    [MethodImpl(256 | 512)]
    public static int sign([This] int v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static ulong sign([This] ulong v) => v == 0ul ? 0ul : 1ul;
    [MethodImpl(256 | 512)]
    public static long sign([This] long v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static half sign([This] half v) => (half)Math.Sign((float)v);
    [MethodImpl(256 | 512)]
    public static float sign([This] float v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static double sign([This] double v) => Math.Sign(v);
    [MethodImpl(256 | 512)]
    public static decimal sign([This] decimal v) => Math.Sign(v);

    #endregion

    #region Ceil

    [MethodImpl(256 | 512)]
    public static half ceil([This] half v) => half.Ceiling(v);
    [MethodImpl(256 | 512)]
    public static float ceil([This] float v) => MathF.Ceiling(v);
    [MethodImpl(256 | 512)]
    public static double ceil([This] double v) => Math.Ceiling(v);
    [MethodImpl(256 | 512)]
    public static decimal ceil([This] decimal v) => Math.Ceiling(v);

    #endregion

    #region Floor

    [MethodImpl(256 | 512)]
    public static half floor([This] half v) => half.Floor(v);
    [MethodImpl(256 | 512)]
    public static float floor([This] float v) => MathF.Floor(v);
    [MethodImpl(256 | 512)]
    public static double floor([This] double v) => Math.Floor(v);
    [MethodImpl(256 | 512)]
    public static decimal floor([This] decimal v) => Math.Floor(v);

    #endregion

    #region Round

    [MethodImpl(256 | 512)]
    public static half round([This] half v) => half.Round(v);
    [MethodImpl(256 | 512)]
    public static float round([This] float v) => MathF.Round(v);
    [MethodImpl(256 | 512)]
    public static double round([This] double v) => Math.Round(v);
    [MethodImpl(256 | 512)]
    public static decimal round([This] decimal v) => Math.Round(v);

    #endregion

    #region Trunc

    [MethodImpl(256 | 512)]
    public static half trunc([This] half v) => half.Truncate(v);
    [MethodImpl(256 | 512)]
    public static float trunc([This] float v) => MathF.Truncate(v);
    [MethodImpl(256 | 512)]
    public static double trunc([This] double v) => Math.Truncate(v);
    [MethodImpl(256 | 512)]
    public static decimal trunc([This] decimal v) => Math.Truncate(v);

    #endregion

    #region Frac

    [MethodImpl(256 | 512)]
    public static half frac([This] half v) => v - half.Floor(v);
    [MethodImpl(256 | 512)]
    public static float frac([This] float v) => v - MathF.Floor(v);
    [MethodImpl(256 | 512)]
    public static double frac([This] double v) => v - Math.Floor(v);
    [MethodImpl(256 | 512)]
    public static decimal frac([This] decimal v) => v - Math.Floor(v);

    #endregion

    #region Modf

    [MethodImpl(256 | 512)]
    public static half modf([This] half v, out half i)
    {
        i = trunc(v);
        return (half)(v - i);
    }
    [MethodImpl(256 | 512)]
    public static float modf([This] float v, out float i)
    {
        i = trunc(v);
        return v - i;
    }
    [MethodImpl(256 | 512)]
    public static double modf([This] double v, out double i)
    {
        i = trunc(v);
        return v - i;
    }
    [MethodImpl(256 | 512)]
    public static decimal modf([This] decimal v, out decimal i)
    {
        i = trunc(v);
        return v - i;
    }

    #endregion

    #region Mod

    [MethodImpl(256 | 512)]
    public static half mod([This] half a, half b) => (half)mod((float)a, (float)b);
    [MethodImpl(256 | 512)]
    public static float mod([This] float a, float b) => a - b * MathF.Floor(a / b);
    [MethodImpl(256 | 512)]
    public static double mod([This] double a, double b) => a - b * Math.Floor(a / b);
    [MethodImpl(256 | 512)]
    public static decimal mod([This] decimal a, decimal b) => a - b * decimal.Floor(a / b);

    #endregion

    #region Min

    [MethodImpl(256 | 512)]
    public static byte min([This] byte a, byte b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static sbyte min([This] sbyte a, sbyte b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static ushort min([This] ushort a, ushort b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static short min([This] short a, short b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static uint min([This] uint a, uint b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static int min([This] int a, int b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static ulong min([This] ulong a, ulong b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static long min([This] long a, long b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static half min([This] half a, half b) => half.Min(a, b);
    [MethodImpl(256 | 512)]
    public static float min([This] float a, float b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static double min([This] double a, double b) => Math.Min(a, b);
    [MethodImpl(256 | 512)]
    public static decimal min([This] decimal a, decimal b) => Math.Min(a, b);

    #endregion

    #region Max

    [MethodImpl(256 | 512)]
    public static byte max([This] byte a, byte b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static sbyte max([This] sbyte a, sbyte b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static ushort max([This] ushort a, ushort b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static short max([This] short a, short b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static uint max([This] uint a, uint b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static int max([This] int a, int b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static ulong max([This] ulong a, ulong b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static long max([This] long a, long b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static half max([This] half a, half b) => half.Max(a, b);
    [MethodImpl(256 | 512)]
    public static float max([This] float a, float b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static double max([This] double a, double b) => Math.Max(a, b);
    [MethodImpl(256 | 512)]
    public static decimal max([This] decimal a, decimal b) => Math.Max(a, b);

    #endregion

    #region Clamp

    [MethodImpl(256 | 512)]
    public static byte clamp([This] byte v, byte min, byte max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static sbyte clamp([This] sbyte v, sbyte min, sbyte max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static ushort clamp([This] ushort v, ushort min, ushort max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static short clamp([This] short v, short min, short max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static uint clamp([This] uint v, uint min, uint max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static int clamp([This] int v, int min, int max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static ulong clamp([This] ulong v, ulong min, ulong max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static long clamp([This] long v, long min, long max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static half clamp([This] half v, half min, half max) => half.Max(min, half.Min(max, v));
    [MethodImpl(256 | 512)]
    public static float clamp([This] float v, float min, float max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static double clamp([This] double v, double min, double max) => Math.Max(min, Math.Min(max, v));
    [MethodImpl(256 | 512)]
    public static decimal clamp([This] decimal v, decimal min, decimal max) => Math.Max(min, Math.Min(max, v));

    #endregion

    #region Lerp

    [MethodImpl(256 | 512)]
    public static sbyte lerp(sbyte start, sbyte end, [This] sbyte t) => fma(t, (sbyte)(end - start), start);

    [MethodImpl(256 | 512)]
    public static byte lerp(byte start, byte end, [This] byte t) => fma(t, (byte)(end - start), start);

    [MethodImpl(256 | 512)]
    public static short lerp(short start, short end, [This] short t) => fma(t, (short)(end - start), start);

    [MethodImpl(256 | 512)]
    public static ushort lerp(ushort start, ushort end, [This] ushort t) => fma(t, (ushort)(end - start), start);

    [MethodImpl(256 | 512)]
    public static int lerp(int start, int end, [This] int t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static uint lerp(uint start, uint end, [This] uint t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static long lerp(long start, long end, [This] long t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static ulong lerp(ulong start, ulong end, [This] ulong t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static half lerp(half start, half end, [This] half t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static float lerp(float start, float end, [This] float t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static double lerp(double start, double end, [This] double t) => fma(t, end - start, start);

    [MethodImpl(256 | 512)]
    public static decimal lerp(decimal start, decimal end, [This] decimal t) => fma(t, end - start, start);

    #endregion

    #region Unlerp

    [MethodImpl(256 | 512)]
    public static byte unlerp([This] byte a, byte start, byte end) => (byte)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static sbyte unlerp([This] sbyte a, sbyte start, sbyte end) => (sbyte)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static ushort unlerp([This] ushort a, ushort start, ushort end) => (ushort)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static short unlerp([This] short a, short start, short end) => (short)((a - start) / (end - start));

    [MethodImpl(256 | 512)]
    public static uint unlerp([This] uint a, uint start, uint end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static int unlerp([This] int a, int start, int end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static ulong unlerp([This] ulong a, ulong start, ulong end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static long unlerp([This] long a, long start, long end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static half unlerp([This] half a, half start, half end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static float unlerp([This] float a, float start, float end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static double unlerp([This] double a, double start, double end) => (a - start) / (end - start);

    [MethodImpl(256 | 512)]
    public static decimal unlerp([This] decimal a, decimal start, decimal end) => (a - start) / (end - start);

    #endregion

    #region Remap

    [MethodImpl(256 | 512)]
    public static byte remap([This] byte a, byte srcStart, byte srcEnd, byte dstStart, byte dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static sbyte remap([This] sbyte a, sbyte srcStart, sbyte srcEnd, sbyte dstStart, sbyte dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static ushort remap([This] ushort a, ushort srcStart, ushort srcEnd, ushort dstStart, ushort dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static short remap([This] short a, short srcStart, short srcEnd, short dstStart, short dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static uint remap([This] uint a, uint srcStart, uint srcEnd, uint dstStart, uint dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static int remap([This] int a, int srcStart, int srcEnd, int dstStart, int dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static ulong remap([This] ulong a, ulong srcStart, ulong srcEnd, ulong dstStart, ulong dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static long remap([This] long a, long srcStart, long srcEnd, long dstStart, long dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static half remap([This] half a, half srcStart, half srcEnd, half dstStart, half dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static float remap([This] float a, float srcStart, float srcEnd, float dstStart, float dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static double remap([This] double a, double srcStart, double srcEnd, double dstStart, double dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    [MethodImpl(256 | 512)]
    public static decimal remap([This] decimal a, decimal srcStart, decimal srcEnd, decimal dstStart, decimal dstEnd) =>
        a.unlerp(srcStart, srcEnd).lerp(dstStart, dstEnd);

    #endregion

    #region step

    [MethodImpl(256 | 512)]
    public static byte step(byte threshold, [This] byte a) => select(a >= threshold, (byte)1, (byte)0);

    [MethodImpl(256 | 512)]
    public static sbyte step(sbyte threshold, [This] sbyte a) => select(a >= threshold, (sbyte)1, (sbyte)0);

    [MethodImpl(256 | 512)]
    public static ushort step(ushort threshold, [This] ushort a) => select(a >= threshold, (ushort)1, (ushort)0);

    [MethodImpl(256 | 512)]
    public static short step(short threshold, [This] short a) => select(a >= threshold, (short)1, (short)0);

    [MethodImpl(256 | 512)]
    public static uint step(uint threshold, [This] uint a) => select(a >= threshold, 1u, 0u);

    [MethodImpl(256 | 512)]
    public static int step(int threshold, [This] int a) => select(a >= threshold, 1, 0);

    [MethodImpl(256 | 512)]
    public static ulong step(ulong threshold, [This] ulong a) => select(a >= threshold, 1UL, 0UL);

    [MethodImpl(256 | 512)]
    public static long step(long threshold, [This] long a) => select(a >= threshold, 1L, 0L);

    [MethodImpl(256 | 512)]
    public static half step(half threshold, [This] half a) => select(a >= threshold, (half)1f, (half)0f);

    [MethodImpl(256 | 512)]
    public static float step(float threshold, [This] float a) => select(a >= threshold, 1f, 0f);

    [MethodImpl(256 | 512)]
    public static double step(double threshold, [This] double a) => select(a >= threshold, 1d, 0d);

    #endregion

    #region SmoothStep

    [MethodImpl(256 | 512)]
    public static half smoothstep(half min, half max, [This] half a)
    {
        var t = saturate(((float)a - (float)min) / ((float)max - (float)min));
        return (half)(t * t * (3.0f - (2.0f * t)));
    }

    [MethodImpl(256 | 512)]
    public static float smoothstep(float min, float max, [This] float a)
    {
        var t = saturate((a - min) / (max - min));
        return t * t * (3.0f - (2.0f * t));
    }

    [MethodImpl(256 | 512)]
    public static double smoothstep(double min, double max, [This] double a)
    {
        var t = saturate((a - min) / (max - min));
        return t * t * (3.0 - (2.0 * t));
    }

    [MethodImpl(256 | 512)]
    public static decimal smoothstep(decimal min, decimal max, [This] decimal a)
    {
        var t = saturate((a - min) / (max - min));
        return t * t * (3.0m - (2.0m * t));
    }

    #endregion

    #region rcp

    [MethodImpl(256 | 512)]
    public static half rcp([This] half v) => half.One / v;
    [MethodImpl(256 | 512)]
    public static float rcp([This] float v) => 1.0f / v;
    [MethodImpl(256 | 512)]
    public static double rcp([This] double v) => 1.0 / v;
    [MethodImpl(256 | 512)]
    public static decimal rcp([This] decimal v) => 1.0m / v;

    #endregion

    #region fma

    [MethodImpl(256 | 512)]
    public static half fma([This] half a, half b, half c) => half.FusedMultiplyAdd(a, b, c);
    [MethodImpl(256 | 512)]
    public static float fma([This] float a, float b, float c) => MathF.FusedMultiplyAdd(a, b, c);
    [MethodImpl(256 | 512)]
    public static double fma([This] double a, double b, double c) => Math.FusedMultiplyAdd(a, b, c);
    [MethodImpl(256 | 512)]
    public static byte fma([This] byte a, byte b, byte c) => (byte)(a * b + c);
    [MethodImpl(256 | 512)]
    public static sbyte fma([This] sbyte a, sbyte b, sbyte c) => (sbyte)(a * b + c);
    [MethodImpl(256 | 512)]
    public static ushort fma([This] ushort a, ushort b, ushort c) => (ushort)(a * b + c);
    [MethodImpl(256 | 512)]
    public static short fma([This] short a, short b, short c) => (short)(a * b + c);
    [MethodImpl(256 | 512)]
    public static uint fma([This] uint a, uint b, uint c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static int fma([This] int a, int b, int c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static ulong fma([This] ulong a, ulong b, ulong c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static long fma([This] long a, long b, long c) => a * b + c;
    [MethodImpl(256 | 512)]
    public static decimal fma([This] decimal a, decimal b, decimal c) => a * b + c;

    #endregion

    #region fms

    [MethodImpl(256 | 512)]
    public static half fms([This] half a, half b, half c) => (half)fms((float)a, (float)b, (float)c);
    [MethodImpl(256 | 512)]
    public static float fms([This] float a, float b, float c) => MathF.FusedMultiplyAdd(a, b, -c);
    [MethodImpl(256 | 512)]
    public static double fms([This] double a, double b, double c) => Math.FusedMultiplyAdd(a, b, -c);
    [MethodImpl(256 | 512)]
    public static ushort fms([This] ushort a, ushort b, ushort c) => (ushort)(a * b - c);
    [MethodImpl(256 | 512)]
    public static short fms([This] short a, short b, short c) => (short)(a * b - c);
    [MethodImpl(256 | 512)]
    public static uint fms([This] uint a, uint b, uint c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static int fms([This] int a, int b, int c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static ulong fms([This] ulong a, ulong b, ulong c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static long fms([This] long a, long b, long c) => a * b - c;
    [MethodImpl(256 | 512)]
    public static decimal fms([This] decimal a, decimal b, decimal c) => a * b - c;

    #endregion

    #region fnma

    [MethodImpl(256 | 512)]
    public static half fnma([This] half a, half b, half c) => (half)fnma((float)a, (float)b, (float)c);
    [MethodImpl(256 | 512)]
    public static float fnma([This] float a, float b, float c) => MathF.FusedMultiplyAdd(-a, b, c);
    [MethodImpl(256 | 512)]
    public static double fnma([This] double a, double b, double c) => Math.FusedMultiplyAdd(-a, b, c);
    [MethodImpl(256 | 512)]
    public static ushort fnma([This] ushort a, ushort b, ushort c) => (ushort)(c - a * b);
    [MethodImpl(256 | 512)]
    public static short fnma([This] short a, short b, short c) => (short)(c - a * b);
    [MethodImpl(256 | 512)]
    public static uint fnma([This] uint a, uint b, uint c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static int fnma([This] int a, int b, int c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static ulong fnma([This] ulong a, ulong b, ulong c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static long fnma([This] long a, long b, long c) => c - a * b;
    [MethodImpl(256 | 512)]
    public static decimal fnma([This] decimal a, decimal b, decimal c) => c - a * b;

    #endregion

    #region fsm

    [MethodImpl(256 | 512)]
    public static half fsm([This] half c, half a, half b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static float fsm([This] float c, float a, float b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static double fsm([This] double c, double a, double b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ushort fsm([This] ushort c, ushort a, ushort b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static short fsm([This] short c, short a, short b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static uint fsm([This] uint c, uint a, uint b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static int fsm([This] int c, int a, int b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ulong fsm([This] ulong c, ulong a, ulong b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static long fsm([This] long c, long a, long b) => fnma(a, b, c);
    [MethodImpl(256 | 512)]
    public static decimal fsm([This] decimal c, decimal a, decimal b) => fnma(a, b, c);

    #endregion

    #region fam

    [MethodImpl(256 | 512)]
    public static half fam([This] half c, half a, half b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static float fam([This] float c, float a, float b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static double fam([This] double c, double a, double b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static byte fam([This] byte c, byte a, byte b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static sbyte fam([This] sbyte c, sbyte a, sbyte b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ushort fam([This] ushort c, ushort a, ushort b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static short fam([This] short c, short a, short b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static uint fam([This] uint c, uint a, uint b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static int fam([This] int c, int a, int b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ulong fam([This] ulong c, ulong a, ulong b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static long fam([This] long c, long a, long b) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static decimal fam([This] decimal c, decimal a, decimal b) => fma(a, b, c);

    #endregion

    #region mad

    [MethodImpl(256 | 512)]
    public static half mad([This] half a, half b, half c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static float mad([This] float a, float b, float c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static double mad([This] double a, double b, double c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static byte mad([This] byte a, byte b, byte c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static sbyte mad([This] sbyte a, sbyte b, sbyte c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ushort mad([This] ushort a, ushort b, ushort c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static short mad([This] short a, short b, short c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static uint mad([This] uint a, uint b, uint c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static int mad([This] int a, int b, int c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static ulong mad([This] ulong a, ulong b, ulong c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static long mad([This] long a, long b, long c) => fma(a, b, c);
    [MethodImpl(256 | 512)]
    public static decimal mad([This] decimal a, decimal b, decimal c) => fma(a, b, c);

    #endregion

    #region Log

    [MethodImpl(256 | 512)]
    public static half log([This] half a) => half.Log(a);
    [MethodImpl(256 | 512)]
    public static float log([This] float a) => MathF.Log(a);
    [MethodImpl(256 | 512)]
    public static double log([This] double a) => Math.Log(a);

    #endregion

    #region Log2

    [MethodImpl(256 | 512)]
    public static half log2([This] half a) => half.Log2(a);
    [MethodImpl(256 | 512)]
    public static float log2([This] float a) => MathF.Log2(a);
    [MethodImpl(256 | 512)]
    public static double log2([This] double a) => Math.Log2(a);

    #endregion

    #region Log10

    [MethodImpl(256 | 512)]
    public static half log10([This] half a) => half.Log10(a);
    [MethodImpl(256 | 512)]
    public static float log10([This] float a) => MathF.Log10(a);
    [MethodImpl(256 | 512)]
    public static double log10([This] double a) => Math.Log10(a);

    #endregion

    #region Log N

    [MethodImpl(256 | 512)]
    public static half log([This] half a, half b) => half.Log(a, b);
    [MethodImpl(256 | 512)]
    public static float log([This] float a, float b) => MathF.Log(a, b);
    [MethodImpl(256 | 512)]
    public static double log([This] double a, double b) => Math.Log(a, b);

    #endregion

    #region Exp

    [MethodImpl(256 | 512)]
    public static half exp([This] half a) => half.Exp(a);
    [MethodImpl(256 | 512)]
    public static float exp([This] float a) => MathF.Exp(a);
    [MethodImpl(256 | 512)]
    public static double exp([This] double a) => Math.Exp(a);

    #endregion

    #region Exp2

    [MethodImpl(256 | 512)]
    public static half exp2([This] half a) => (half)MathF.Exp((float)a * 0.693147180559945309f);
    [MethodImpl(256 | 512)]
    public static float exp2([This] float a) => MathF.Exp(a * 0.693147180559945309f);
    [MethodImpl(256 | 512)]
    public static double exp2([This] double a) => Math.Exp(a * 0.693147180559945309);

    #endregion

    #region Exp10

    [MethodImpl(256 | 512)]
    public static half exp10([This] half a) => (half)MathF.Exp((float)a * 2.302585092994045684f);
    [MethodImpl(256 | 512)]
    public static float exp10([This] float a) => MathF.Exp(a * 2.302585092994045684f);
    [MethodImpl(256 | 512)]
    public static double exp10([This] double a) => Math.Exp(a * 2.302585092994045684);

    #endregion

    #region Pow

    [MethodImpl(256 | 512)]
    public static half pow([This] half a, half b) => half.Pow(a, b);
    [MethodImpl(256 | 512)]
    public static float pow([This] float a, float b) => MathF.Pow(a, b);
    [MethodImpl(256 | 512)]
    public static double pow([This] double a, double b) => Math.Pow(a, b);

    #endregion

    #region IsInf

    [MethodImpl(256 | 512)]
    public static bool isInf([This] half a) => half.IsInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isInf([This] float a) => float.IsInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isInf([This] double a) => double.IsInfinity(a);

    #endregion

    #region IsPInf

    [MethodImpl(256 | 512)]
    public static bool isPosInf([This] half a) => half.IsPositiveInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isPosInf([This] float a) => float.IsPositiveInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isPosInf([This] double a) => double.IsPositiveInfinity(a);

    #endregion

    #region IsNegInf

    [MethodImpl(256 | 512)]
    public static bool isNegInf([This] half a) => half.IsNegativeInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isNegInf([This] float a) => float.IsNegativeInfinity(a);
    [MethodImpl(256 | 512)]
    public static bool isNegInf([This] double a) => double.IsNegativeInfinity(a);

    #endregion

    #region Saturate

    [MethodImpl(256 | 512)]
    public static half saturate([This] half v) => v.clamp((half)0f, (half)1f);
    [MethodImpl(256 | 512)]
    public static float saturate([This] float v) => v.clamp(0f, 1f);
    [MethodImpl(256 | 512)]
    public static double saturate([This] double v) => v.clamp(0, 1);
    [MethodImpl(256 | 512)]
    public static decimal saturate([This] decimal v) => v.clamp(0m, 1m);

    #endregion

    #region Dot

    [MethodImpl(256 | 512)]
    public static byte dot([This] byte a, byte b) => (byte)(a * b);
    [MethodImpl(256 | 512)]
    public static sbyte dot([This] sbyte a, sbyte b) => (sbyte)(a * b);
    [MethodImpl(256 | 512)]
    public static ushort dot([This] ushort a, ushort b) => (ushort)(a * b);
    [MethodImpl(256 | 512)]
    public static short dot([This] short a, short b) => (short)(a * b);
    [MethodImpl(256 | 512)]
    public static uint dot([This] uint a, uint b) => a * b;
    [MethodImpl(256 | 512)]
    public static int dot([This] int a, int b) => a * b;
    [MethodImpl(256 | 512)]
    public static ulong dot([This] ulong a, ulong b) => a * b;
    [MethodImpl(256 | 512)]
    public static long dot([This] long a, long b) => a * b;
    [MethodImpl(256 | 512)]
    public static half dot([This] half a, half b) => a * b;
    [MethodImpl(256 | 512)]
    public static float dot([This] float a, float b) => a * b;
    [MethodImpl(256 | 512)]
    public static double dot([This] double a, double b) => a * b;
    [MethodImpl(256 | 512)]
    public static decimal dot([This] decimal a, decimal b) => a * b;

    #endregion

    #region Sqrt

    [MethodImpl(256 | 512)]
    public static half sqrt([This] half v) => half.Sqrt(v);
    [MethodImpl(256 | 512)]
    public static float sqrt([This] float v) => MathF.Sqrt(v);
    [MethodImpl(256 | 512)]
    public static double sqrt([This] double v) => Math.Sqrt(v);

    #endregion

    #region RSqrt

    [MethodImpl(256 | 512)]
    public static half rsqrt([This] half v) => (half)(1 / sqrt((float)v));
    [MethodImpl(256 | 512)]
    public static float rsqrt([This] float v) => 1 / sqrt(v);
    [MethodImpl(256 | 512)]
    public static double rsqrt([This] double v) => 1 / sqrt(v);

    #endregion

    #region LengthSq

    [MethodImpl(256 | 512)]
    public static byte lengthsq([This] byte v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static sbyte lengthsq([This] sbyte v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static ushort lengthsq([This] ushort v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static short lengthsq([This] short v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static uint lengthsq([This] uint v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static int lengthsq([This] int v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static ulong lengthsq([This] ulong v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static long lengthsq([This] long v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static half lengthsq([This] half v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static float lengthsq([This] float v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static double lengthsq([This] double v) => dot(v, v);
    [MethodImpl(256 | 512)]
    public static decimal lengthsq([This] decimal v) => dot(v, v);

    #endregion

    #region Length

    [MethodImpl(256 | 512)]
    public static byte length([This] byte v) => abs(v);
    [MethodImpl(256 | 512)]
    public static sbyte length([This] sbyte v) => abs(v);
    [MethodImpl(256 | 512)]
    public static ushort length([This] ushort v) => abs(v);
    [MethodImpl(256 | 512)]
    public static short length([This] short v) => abs(v);
    [MethodImpl(256 | 512)]
    public static uint length([This] uint v) => abs(v);
    [MethodImpl(256 | 512)]
    public static int length([This] int v) => abs(v);
    [MethodImpl(256 | 512)]
    public static ulong length([This] ulong v) => abs(v);
    [MethodImpl(256 | 512)]
    public static long length([This] long v) => abs(v);
    [MethodImpl(256 | 512)]
    public static half length([This] half v) => abs(v);
    [MethodImpl(256 | 512)]
    public static float length([This] float v) => abs(v);
    [MethodImpl(256 | 512)]
    public static double length([This] double v) => abs(v);
    [MethodImpl(256 | 512)]
    public static decimal length([This] decimal v) => abs(v);

    #endregion

    #region DistanceSq

    [MethodImpl(256 | 512)]
    public static byte distancesq([This] byte a, byte b) => (byte)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static sbyte distancesq([This] sbyte a, sbyte b) => (sbyte)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static ushort distancesq([This] ushort a, ushort b) => (ushort)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static short distancesq([This] short a, short b) => (short)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static uint distancesq([This] uint a, uint b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static int distancesq([This] int a, int b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static ulong distancesq([This] ulong a, ulong b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static long distancesq([This] long a, long b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static half distancesq([This] half a, half b) => (half)lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static float distancesq([This] float a, float b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static double distancesq([This] double a, double b) => lengthsq(b - a);
    [MethodImpl(256 | 512)]
    public static decimal distancesq([This] decimal a, decimal b) => lengthsq(b - a);

    #endregion

    #region Distance

    [MethodImpl(256 | 512)]
    public static byte distance([This] byte a, byte b) => (byte)abs(b - a);
    [MethodImpl(256 | 512)]
    public static sbyte distance([This] sbyte a, sbyte b) => (sbyte)abs(b - a);
    [MethodImpl(256 | 512)]
    public static ushort distance([This] ushort a, ushort b) => (ushort)abs(b - a);
    [MethodImpl(256 | 512)]
    public static short distance([This] short a, short b) => (short)abs(b - a);
    [MethodImpl(256 | 512)]
    public static uint distance([This] uint a, uint b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static int distance([This] int a, int b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static ulong distance([This] ulong a, ulong b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static long distance([This] long a, long b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static half distance([This] half a, half b) => (half)abs(b - a);
    [MethodImpl(256 | 512)]
    public static float distance([This] float a, float b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static double distance([This] double a, double b) => abs(b - a);
    [MethodImpl(256 | 512)]
    public static decimal distance([This] decimal a, decimal b) => abs(b - a);

    #endregion

    #region Sin

    [MethodImpl(256 | 512)]
    public static half sin([This] half a) => half.Sin(a);
    [MethodImpl(256 | 512)]
    public static float sin([This] float a) => MathF.Sin(a);
    [MethodImpl(256 | 512)]
    public static double sin([This] double a) => Math.Sin(a);

    #endregion

    #region Cos

    [MethodImpl(256 | 512)]
    public static half cos([This] half a) => half.Cos(a);
    [MethodImpl(256 | 512)]
    public static float cos([This] float a) => MathF.Cos(a);
    [MethodImpl(256 | 512)]
    public static double cos([This] double a) => Math.Cos(a);

    #endregion

    #region SinCos

    [MethodImpl(256 | 512)]
    public static (half sin, half cos) sincos([This] half a) => half.SinCos(a);
    [MethodImpl(256 | 512)]
    public static (float sin, float cos) sincos([This] float a) => MathF.SinCos(a);
    [MethodImpl(256 | 512)]
    public static (double sin, double cos) sincos([This] double a) => Math.SinCos(a);

    #endregion

    #region SinCos out

    [MethodImpl(256 | 512)]
    public static void sincos([This] half a, out half sin, out half cos) => (sin, cos) = sincos(a);
    [MethodImpl(256 | 512)]
    public static void sincos([This] float a, out float sin, out float cos) => (sin, cos) = sincos(a);
    [MethodImpl(256 | 512)]
    public static void sincos([This] double a, out double sin, out double cos) => (sin, cos) = sincos(a);

    #endregion

    #region Tan

    [MethodImpl(256 | 512)]
    public static half tan([This] half a) => half.Tan(a);
    [MethodImpl(256 | 512)]
    public static float tan([This] float a) => MathF.Tan(a);
    [MethodImpl(256 | 512)]
    public static double tan([This] double a) => Math.Tan(a);

    #endregion

    #region Asin

    [MethodImpl(256 | 512)]
    public static half asin([This] half a) => half.Asin(a);
    [MethodImpl(256 | 512)]
    public static float asin([This] float a) => MathF.Asin(a);
    [MethodImpl(256 | 512)]
    public static double asin([This] double a) => Math.Asin(a);

    #endregion

    #region Acos

    [MethodImpl(256 | 512)]
    public static half acos([This] half a) => half.Acos(a);
    [MethodImpl(256 | 512)]
    public static float acos([This] float a) => MathF.Acos(a);
    [MethodImpl(256 | 512)]
    public static double acos([This] double a) => Math.Acos(a);

    #endregion

    #region Atan

    [MethodImpl(256 | 512)]
    public static half atan([This] half a) => half.Atan(a);
    [MethodImpl(256 | 512)]
    public static float atan([This] float a) => MathF.Atan(a);
    [MethodImpl(256 | 512)]
    public static double atan([This] double a) => Math.Atan(a);

    #endregion

    #region Atan2

    [MethodImpl(256 | 512)]
    public static half atan2([This] half y, half x) => half.Atan2(y, x);
    [MethodImpl(256 | 512)]
    public static float atan2([This] float y, float x) => MathF.Atan2(y, x);
    [MethodImpl(256 | 512)]
    public static double atan2([This] double y, double x) => Math.Atan2(y, x);

    #endregion

    #region Sinh

    [MethodImpl(256 | 512)]
    public static half sinh([This] half a) => half.Sinh(a);
    [MethodImpl(256 | 512)]
    public static float sinh([This] float a) => MathF.Sinh(a);
    [MethodImpl(256 | 512)]
    public static double sinh([This] double a) => Math.Sinh(a);

    #endregion

    #region Cosh

    [MethodImpl(256 | 512)]
    public static half cosh([This] half a) => half.Cosh(a);
    [MethodImpl(256 | 512)]
    public static float cosh([This] float a) => MathF.Cosh(a);
    [MethodImpl(256 | 512)]
    public static double cosh([This] double a) => Math.Cosh(a);

    #endregion

    #region Tanh

    [MethodImpl(256 | 512)]
    public static half tanh([This] half a) => half.Tanh(a);
    [MethodImpl(256 | 512)]
    public static float tanh([This] float a) => MathF.Tanh(a);
    [MethodImpl(256 | 512)]
    public static double tanh([This] double a) => Math.Tanh(a);

    #endregion

    #region Asinh

    [MethodImpl(256 | 512)]
    public static half asinh([This] half a) => half.Asinh(a);
    [MethodImpl(256 | 512)]
    public static float asinh([This] float a) => MathF.Asinh(a);
    [MethodImpl(256 | 512)]
    public static double asinh([This] double a) => Math.Asinh(a);

    #endregion

    #region Acosh

    [MethodImpl(256 | 512)]
    public static half acosh([This] half a) => half.Acosh(a);
    [MethodImpl(256 | 512)]
    public static float acosh([This] float a) => MathF.Acosh(a);
    [MethodImpl(256 | 512)]
    public static double acosh([This] double a) => Math.Acosh(a);

    #endregion

    #region Atanh

    [MethodImpl(256 | 512)]
    public static half atanh([This] half a) => half.Atanh(a);
    [MethodImpl(256 | 512)]
    public static float atanh([This] float a) => MathF.Atanh(a);
    [MethodImpl(256 | 512)]
    public static double atanh([This] double a) => Math.Atanh(a);

    #endregion

    #region Radians

    [MethodImpl(256 | 512)]
    public static half radians([This] half a) => (half)((float)a * F_DegToRad);

    [MethodImpl(256 | 512)]
    public static float radians([This] float a) => a * F_DegToRad;

    [MethodImpl(256 | 512)]
    public static double radians([This] double a) => a * D_DegToRad;

    [MethodImpl(256 | 512)]
    public static decimal radians([This] decimal a) => a * M_DegToRad;

    #endregion

    #region Degrees

    [MethodImpl(256 | 512)]
    public static half degrees([This] half a) => (half)((float)a * F_RadToDeg);

    [MethodImpl(256 | 512)]
    public static float degrees([This] float a) => a * F_RadToDeg;

    [MethodImpl(256 | 512)]
    public static double degrees([This] double a) => a * D_RadToDeg;

    [MethodImpl(256 | 512)]
    public static decimal degrees([This] decimal a) => a * M_RadToDeg;

    #endregion

    #region Wrap

    [MethodImpl(256 | 512)]
    public static half wrap([This] half x, half min, half max) => (half)wrap((float)x, (float)min, (float)max);

    [MethodImpl(256 | 512)]
    public static float wrap([This] float x, float min, float max)
    {
        var range = max - min;
        return min + ((x - min) % range + range) % range;
    }

    [MethodImpl(256 | 512)]
    public static double wrap([This] double x, double min, double max)
    {
        var range = max - min;
        return min + ((x - min) % range + range) % range;
    }

    [MethodImpl(256 | 512)]
    public static decimal wrap([This] decimal x, decimal min, decimal max)
    {
        var range = max - min;
        return min + ((x - min) % range + range) % range;
    }

    #endregion

    #region Normalize

    [MethodImpl(256 | 512)]
    public static half normalize([This] half a) => a * dot(a, a).rsqrt();
    [MethodImpl(256 | 512)]
    public static float normalize([This] float a) => a * dot(a, a).rsqrt();
    [MethodImpl(256 | 512)]
    public static double normalize([This] double a) => a * dot(a, a).rsqrt();

    #endregion

    #region NormalizeSafe

    [MethodImpl(256 | 512)]
    public static half normalizeSafe([This] half a, half defaultValue = default)
    {
        var len = dot(a, a);
        return select(len > 1.175494351e-38f.half(), a * rsqrt(len), defaultValue);
    }
    
    [MethodImpl(256 | 512)]
    public static float normalizeSafe([This] float a, float defaultValue = default)
    {
        var len = dot(a, a);
        return select(len > 1.175494351e-38f, a * rsqrt(len), defaultValue);
    }

    [MethodImpl(256 | 512)]
    public static double normalizeSafe([This] double a, double defaultValue = default)
    {
        var len = dot(a, a);
        return select(len > 1.175494351e-38, a * rsqrt(len), defaultValue);
    }

    #endregion
    
    #region Reflect

    [MethodImpl(256 | 512)]
    public static half reflect([This] half i, half n) => i - 2f.half() * n * dot(i, n);
    [MethodImpl(256 | 512)]
    public static float reflect([This] float i, float n) => i - 2f * n * dot(i, n);
    [MethodImpl(256 | 512)]
    public static double reflect([This] double i, double n) => i - 2d * n * dot(i, n);
    [MethodImpl(256 | 512)]
    public static decimal reflect([This] decimal i, decimal n) => i - 2m * n * dot(i, n);

    #endregion

    #region Reflect

    [MethodImpl(256 | 512)]
    public static half refract(half i, half n, [This] half indexOfRefraction)
    {
        var ni = dot(n, i);
        var k = 1.0f.half() - indexOfRefraction * indexOfRefraction * (1.0f.half() - ni * ni);
        return select(k >= 0.0f.half(), indexOfRefraction * i - (indexOfRefraction * ni + sqrt(k)) * n, default);
    }
    [MethodImpl(256 | 512)]
    public static float refract(float i, float n, [This] float indexOfRefraction)
    {
        var ni = dot(n, i);
        var k = 1.0f - indexOfRefraction * indexOfRefraction * (1.0f - ni * ni);
        return select(k >= 0.0f, indexOfRefraction * i - (indexOfRefraction * ni + sqrt(k)) * n, default);
    }
    [MethodImpl(256 | 512)]
    public static double refract(double i, double n, [This] double indexOfRefraction)
    {
        var ni = dot(n, i);
        var k = 1.0 - indexOfRefraction * indexOfRefraction * (1.0 - ni * ni);
        return select(k >= 0.0, indexOfRefraction * i - (indexOfRefraction * ni + sqrt(k)) * n, default);
    }

    #endregion

    #region Project

    [MethodImpl(256 | 512)]
    public static half project([This] half a, half onto) => dot(a, onto) / dot(onto, onto) * onto;
    [MethodImpl(256 | 512)]
    public static float project([This] float a, float onto) => dot(a, onto) / dot(onto, onto) * onto;
    [MethodImpl(256 | 512)]
    public static double project([This] double a, double onto) => dot(a, onto) / dot(onto, onto) * onto;
    [MethodImpl(256 | 512)]
    public static decimal project([This] decimal a, decimal onto) => dot(a, onto) / dot(onto, onto) * onto;

    
    [MethodImpl(256 | 512)]
    public static half projectSafe([This] half a, half onto, half defaultValue = default) 
    {
        var proj = project(a, onto);
        return select(isFinite(proj), proj, defaultValue);
    }
    [MethodImpl(256 | 512)]
    public static float projectSafe([This] float a, float onto, float defaultValue = default) 
    {
        var proj = project(a, onto);
        return select(isFinite(proj), proj, defaultValue);
    }
    [MethodImpl(256 | 512)]
    public static double projectSafe([This] double a, double onto, double defaultValue = default) 
    {
        var proj = project(a, onto);
        return select(isFinite(proj), proj, defaultValue);
    }
    

    [MethodImpl(256 | 512)]
    public static half projectNormalized([This] half a, half onto) => dot(a, onto) * onto;
    [MethodImpl(256 | 512)]
    public static float projectNormalized([This] float a, float onto) => dot(a, onto) * onto;
    [MethodImpl(256 | 512)]
    public static double projectNormalized([This] double a, double onto) => dot(a, onto) * onto;
    [MethodImpl(256 | 512)]
    public static decimal projectNormalized([This] decimal a, decimal onto) => dot(a, onto) * onto;


    [MethodImpl(256 | 512)]
    public static half projectOnPlane([This] half a, half plane_normal) => a - project(a, plane_normal);
    [MethodImpl(256 | 512)]
    public static float projectOnPlane([This] float a, float plane_normal) => a - project(a, plane_normal);
    [MethodImpl(256 | 512)]
    public static double projectOnPlane([This] double a, double plane_normal) => a - project(a, plane_normal);
    [MethodImpl(256 | 512)]
    public static decimal projectOnPlane([This] decimal a, decimal plane_normal) => a - project(a, plane_normal);


    [MethodImpl(256 | 512)]
    public static half projectOnPlaneNormalized([This] half a, half plane_normal) => a - projectNormalized(a, plane_normal);
    [MethodImpl(256 | 512)]
    public static float projectOnPlaneNormalized([This] float a, float plane_normal) => a - projectNormalized(a, plane_normal);
    [MethodImpl(256 | 512)]
    public static double projectOnPlaneNormalized([This] double a, double plane_normal) => a - projectNormalized(a, plane_normal);
    [MethodImpl(256 | 512)]
    public static decimal projectOnPlaneNormalized([This] decimal a, decimal plane_normal) => a - projectNormalized(a, plane_normal);

    #endregion
}
