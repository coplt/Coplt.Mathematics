using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.Wasm;
using System.Runtime.Intrinsics.X86;
using X86 = System.Runtime.Intrinsics.X86;

namespace Coplt.Mathematics.Simd;

[CpuOnly]
public static partial class simd
{
    #region Convert

    [MethodImpl(256 | 512)]
    public static Vector128<int> ToInt32(Vector256<double> v)
    {
        if (Avx.IsSupported)
        {
            return Avx.ConvertToVector128Int32(v);
        }
        return Vector128.Narrow(Vector128.ConvertToInt64(v.GetLower()), Vector128.ConvertToInt64(v.GetUpper()));
    }

    [MethodImpl(256 | 512)]
    public static Vector128<uint> ToUInt32(Vector256<double> v)
    {
        if (Avx.IsSupported)
        {
            return Avx.ConvertToVector128Int32(v).AsUInt32();
        }
        return Vector128.Narrow(Vector128.ConvertToUInt64(v.GetLower()), Vector128.ConvertToUInt64(v.GetUpper()));
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> ToSingle(Vector256<double> v)
    {
        if (Avx.IsSupported)
        {
            return Avx.ConvertToVector128Single(v);
        }
        return Vector128.Narrow(v.GetLower(), v.GetUpper());
    }

    [MethodImpl(256 | 512)]
    public static Vector64<float> ToSingle(Vector128<double> v)
    {
        return Vector64.Narrow(v.GetLower(), v.GetUpper());
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> ToDouble(Vector128<float> v)
    {
        if (Avx.IsSupported)
        {
            return Avx.ConvertToVector256Double(v);
        }
        var (l, u) = Vector128.Widen(v);
        return Vector256.Create(l, u);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> ToDouble(Vector64<float> v)
    {
        var (l, u) = Vector64.Widen(v);
        return Vector128.Create(l, u);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> ToDouble(Vector128<int> v)
    {
        if (Avx.IsSupported)
        {
            return Avx.ConvertToVector256Double(v);
        }
        var (l, u) = Vector128.Widen(v);
        return Vector256.Create(Vector128.ConvertToDouble(l), Vector128.ConvertToDouble(u));
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> ToDouble(Vector128<uint> v)
    {
        if (Avx.IsSupported)
        {
            return Avx.ConvertToVector256Double(v.AsInt32());
        }
        var (l, u) = Vector128.Widen(v);
        return Vector256.Create(Vector128.ConvertToDouble(l), Vector128.ConvertToDouble(u));
    }

    #endregion

    #region MoveLowToHigh

    /// <summary>
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static Vector128<T> MoveLowToHigh<T>(Vector128<T> a, Vector128<T> b)
    {
        if (Sse.IsSupported)
        {
            return Sse.MoveLowToHigh(a.As<T, float>(), b.As<T, float>()).As<float, T>();
        }
        return a.WithUpper(b.GetLower());
    }

    /// <summary>
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static Vector256<T> MoveLowToHigh<T>(Vector256<T> a, Vector256<T> b)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute2x128(a.As<T, double>(), b.As<T, double>(), 0b0010_0000).As<double, T>();
        }
        return a.WithUpper(b.GetLower());
    }

    #endregion

    #region MoveHighToLow

    /// <summary>
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static Vector128<T> MoveHighToLow<T>(Vector128<T> a, Vector128<T> b)
    {
        if (Sse.IsSupported)
        {
            return Sse.MoveHighToLow(a.As<T, float>(), b.As<T, float>()).As<float, T>();
        }
        return a.WithLower(b.GetUpper());
    }

    /// <summary>
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static Vector256<T> MoveHighToLow<T>(Vector256<T> a, Vector256<T> b)
    {
        if (Avx.IsSupported)
        {
            return Avx.Permute2x128(a.As<T, double>(), b.As<T, double>(), 0b0001_0011).As<double, T>();
        }
        return a.WithLower(b.GetUpper());
    }

    #endregion

    #region UnpackLow

    [MethodImpl(256 | 512)]
    public static Vector128<T> UnpackLow<T>(Vector128<T> a, Vector128<T> b) =>
        UnpackLow(a.AsSingle(), b.AsSingle()).As<float, T>();

    [MethodImpl(256 | 512)]
    public static Vector256<T> UnpackLow<T>(Vector256<T> a, Vector256<T> b) =>
        UnpackLow(a.AsDouble(), b.AsDouble()).As<double, T>();

    [MethodImpl(256 | 512)]
    private static Vector128<float> UnpackLow(Vector128<float> a, Vector128<float> b)
    {
        if (Sse.IsSupported)
        {
            return Sse.UnpackLow(a, b);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.ZipLow(a, b);
        }
        return Vector128.Shuffle(MoveLowToHigh(a, b), Vector128.Create(0, 2, 1, 3));
    }
    [MethodImpl(256 | 512)]
    private static Vector256<double> UnpackLow(Vector256<double> a, Vector256<double> b)
    {
        return Vector256.Shuffle(MoveLowToHigh(a, b), Vector256.Create(0, 2, 1, 3));
    }

    #endregion

    #region UnpackHigh

    [MethodImpl(256 | 512)]
    public static Vector128<T> UnpackHigh<T>(Vector128<T> a, Vector128<T> b) =>
        UnpackHigh(a.AsSingle(), b.AsSingle()).As<float, T>();

    [MethodImpl(256 | 512)]
    public static Vector256<T> UnpackHigh<T>(Vector256<T> a, Vector256<T> b) =>
        UnpackHigh(a.AsDouble(), b.AsDouble()).As<double, T>();

    [MethodImpl(256 | 512)]
    private static Vector128<float> UnpackHigh(Vector128<float> a, Vector128<float> b)
    {
        if (Sse.IsSupported)
        {
            return Sse.UnpackHigh(a, b);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.ZipHigh(a, b);
        }
        return Vector128.Shuffle(MoveHighToLow(b, a), Vector128.Create(0, 2, 1, 3));
    }

    [MethodImpl(256 | 512)]
    private static Vector256<double> UnpackHigh(Vector256<double> a, Vector256<double> b)
    {
        return Vector256.Shuffle(MoveHighToLow(b, a), Vector256.Create(0, 2, 1, 3));
    }

    #endregion

    #region Cmp

    [MethodImpl(256 | 512)]
    public static Vector64<float> Ne(Vector64<float> a, Vector64<float> b)
    {
        if (Sse.IsSupported)
        {
            return Sse.CompareNotEqual(a.ToVector128(), b.ToVector128()).GetLower();
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.CompareNotEqual(a.ToVector128(), b.ToVector128()).GetLower();
        }
        return ~Vector64.Equals(a, b);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Ne(Vector128<float> a, Vector128<float> b)
    {
        if (Sse.IsSupported)
        {
            return Sse.CompareNotEqual(a, b);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.CompareNotEqual(a, b);
        }
        return ~Vector128.Equals(a, b);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Ne(Vector256<float> a, Vector256<float> b)
    {
        if (Avx.IsSupported)
        {
            return Avx.CompareNotEqual(a, b);
        }
        return ~Vector256.Equals(a, b);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Ne(Vector512<float> a, Vector512<float> b)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.CompareNotEqual(a, b);
        }
        return ~Vector512.Equals(a, b);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Ne(Vector128<double> a, Vector128<double> b)
    {
        if (Sse2.IsSupported)
        {
            return Sse2.CompareNotEqual(a, b);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.CompareNotEqual(a, b);
        }
        return ~Vector128.Equals(a, b);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Ne(Vector256<double> a, Vector256<double> b)
    {
        if (Avx.IsSupported)
        {
            return Avx.CompareNotEqual(a, b);
        }
        return ~Vector256.Equals(a, b);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Ne(Vector512<double> a, Vector512<double> b)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.CompareNotEqual(a, b);
        }
        return ~Vector512.Equals(a, b);
    }

    #endregion

    #region Shift

    public static bool IsShiftAccelerated
    {
        [MethodImpl(256 | 512)]
        get => Avx2.IsSupported;
    }

    #region ShiftLeft

    [MethodImpl(256 | 512)]
    public static Vector64<uint> ShiftLeft(Vector64<uint> a, Vector64<uint> b)
    {
        if (Vector128.IsHardwareAccelerated) return ShiftLeft(a.ToVector128(), b.ToVector128()).GetLower();
        return Vector64.Create(
            a[0] << (int)b[0],
            a[1] << (int)b[1]
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<uint> ShiftLeft(Vector128<uint> a, Vector128<uint> b, bool _3d = false)
    {
        if (Avx2.IsSupported) return Avx2.ShiftLeftLogicalVariable(a, b);
        if (_3d) return Vector128.Create(a[0] << (int)b[0], a[1] << (int)b[1], a[2] << (int)b[2], 0);
        return Vector128.Create(a[0] << (int)b[0], a[1] << (int)b[1], a[2] << (int)b[2], a[3] << (int)b[3]);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<uint> ShiftLeft(Vector256<uint> a, Vector256<uint> b)
    {
        if (Avx2.IsSupported) return Avx2.ShiftLeftLogicalVariable(a, b);
        return Vector256.Create(
            ShiftLeft(a.GetLower(), b.GetLower()),
            ShiftLeft(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<uint> ShiftLeft(Vector512<uint> a, Vector512<uint> b)
    {
        if (Avx512F.IsSupported) return Avx512F.ShiftLeftLogicalVariable(a, b);
        return Vector512.Create(
            ShiftLeft(a.GetLower(), b.GetLower()),
            ShiftLeft(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<ulong> ShiftLeft(Vector128<ulong> a, Vector128<ulong> b)
    {
        if (Avx2.IsSupported) return Avx2.ShiftLeftLogicalVariable(a, b);
        return Vector128.Create(a[0] << (int)b[0], a[1] << (int)b[1]);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<ulong> ShiftLeft(Vector256<ulong> a, Vector256<ulong> b, bool _3d = false)
    {
        if (Avx2.IsSupported) return Avx2.ShiftLeftLogicalVariable(a, b);
        if (_3d) return Vector256.Create(a[0] << (int)b[0], a[1] << (int)b[1], a[2] << (int)b[2], 0);
        return Vector256.Create(a[0] << (int)b[0], a[1] << (int)b[1], a[2] << (int)b[2], a[3] << (int)b[3]);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<ulong> ShiftLeft(Vector512<ulong> a, Vector512<ulong> b)
    {
        if (Avx512F.IsSupported) return Avx512F.ShiftLeftLogicalVariable(a, b);
        return Vector512.Create(
            ShiftLeft(a.GetLower(), b.GetLower()),
            ShiftLeft(a.GetUpper(), b.GetUpper())
        );
    }

    #endregion

    #region ShiftRight

    [MethodImpl(256 | 512)]
    public static Vector64<uint> ShiftRight(Vector64<uint> a, Vector64<uint> b, bool _3d = false)
    {
        if (Vector128.IsHardwareAccelerated) return ShiftRight(a.ToVector128(), b.ToVector128()).GetLower();
        return Vector64.Create(
            a[0] >> (int)b[0],
            a[1] >> (int)b[1]
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<uint> ShiftRight(Vector128<uint> a, Vector128<uint> b, bool _3d = false)
    {
        if (Avx2.IsSupported) return Avx2.ShiftRightLogicalVariable(a, b);
        if (_3d) return Vector128.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], 0);
        return Vector128.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], a[3] >> (int)b[3]);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<uint> ShiftRight(Vector256<uint> a, Vector256<uint> b)
    {
        if (Avx2.IsSupported) return Avx2.ShiftRightLogicalVariable(a, b);
        return Vector256.Create(
            ShiftRight(a.GetLower(), b.GetLower()),
            ShiftRight(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<uint> ShiftRight(Vector512<uint> a, Vector512<uint> b)
    {
        if (Avx512F.IsSupported) return Avx512F.ShiftRightLogicalVariable(a, b);
        return Vector512.Create(
            ShiftRight(a.GetLower(), b.GetLower()),
            ShiftRight(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<ulong> ShiftRight(Vector128<ulong> a, Vector128<ulong> b)
    {
        if (Avx2.IsSupported) return Avx2.ShiftRightLogicalVariable(a, b);
        return Vector128.Create(a[0] >> (int)b[0], a[1] >> (int)b[1]);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<ulong> ShiftRight(Vector256<ulong> a, Vector256<ulong> b, bool _3d = false)
    {
        if (Avx2.IsSupported) return Avx2.ShiftRightLogicalVariable(a, b);
        if (_3d) return Vector256.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], 0);
        return Vector256.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], a[3] >> (int)b[3]);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<ulong> ShiftRight(Vector512<ulong> a, Vector512<ulong> b)
    {
        if (Avx512F.IsSupported) return Avx512F.ShiftRightLogicalVariable(a, b);
        return Vector512.Create(
            ShiftRight(a.GetLower(), b.GetLower()),
            ShiftRight(a.GetUpper(), b.GetUpper())
        );
    }

    #endregion

    #region ShiftRightSigned

    [MethodImpl(256 | 512)]
    public static Vector64<int> ShiftRight(Vector64<int> a, Vector64<uint> b, bool _3d = false)
    {
        if (Avx2.IsSupported) return ShiftRight(a.ToVector128(), b.ToVector128()).GetLower();
        return Vector64.Create(a[0] >> (int)b[0], a[1] >> (int)b[1]);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<int> ShiftRight(Vector128<int> a, Vector128<uint> b, bool _3d = false)
    {
        if (Avx2.IsSupported) return Avx2.ShiftRightArithmeticVariable(a, b);
        if (_3d) return Vector128.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], 0);
        return Vector128.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], a[3] >> (int)b[3]);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<int> ShiftRight(Vector256<int> a, Vector256<uint> b)
    {
        if (Avx2.IsSupported) return Avx2.ShiftRightArithmeticVariable(a, b);
        return Vector256.Create(
            ShiftRight(a.GetLower(), b.GetLower()),
            ShiftRight(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<int> ShiftRight(Vector512<int> a, Vector512<uint> b)
    {
        if (Avx512F.IsSupported) return Avx512F.ShiftRightArithmeticVariable(a, b);
        return Vector512.Create(
            ShiftRight(a.GetLower(), b.GetLower()),
            ShiftRight(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<long> ShiftRight(Vector128<long> a, Vector128<ulong> b)
    {
        #if NET9_0_OR_GREATER
        if (Avx10v1.IsSupported) return Avx10v1.ShiftRightArithmeticVariable(a, b);
        #endif
        if (Avx512F.VL.IsSupported) return Avx512F.VL.ShiftRightArithmeticVariable(a, b);
        return Vector128.Create(a[0] >> (int)b[0], a[1] >> (int)b[1]);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<long> ShiftRight(Vector256<long> a, Vector256<ulong> b, bool _3d = false)
    {
        #if NET9_0_OR_GREATER
        if (Avx10v1.IsSupported) return Avx10v1.ShiftRightArithmeticVariable(a, b);
        #endif
        if (Avx512F.VL.IsSupported) return Avx512F.VL.ShiftRightArithmeticVariable(a, b);
        if (_3d) return Vector256.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], 0);
        return Vector256.Create(a[0] >> (int)b[0], a[1] >> (int)b[1], a[2] >> (int)b[2], a[3] >> (int)b[3]);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<long> ShiftRight(Vector512<long> a, Vector512<ulong> b)
    {
        if (Avx512F.IsSupported) return Avx512F.ShiftRightArithmeticVariable(a, b);
        return Vector512.Create(
            ShiftRight(a.GetLower(), b.GetLower()),
            ShiftRight(a.GetUpper(), b.GetUpper())
        );
    }

    #endregion

    #endregion

    #region Round

    [MethodImpl(256 | 512)]
    public static Vector64<float> Round(Vector64<float> x)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.RoundToNearestInteger(x.ToVector128()).GetLower();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.RoundToNearest(x);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.RoundToNearest(x.ToVector128()).GetLower();
        }
        return Vector64.Create(
            MathF.Round(x.GetElement(0)),
            MathF.Round(x.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Round(Vector128<float> x)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.RoundToNearestInteger(x);
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.RoundToNearest(x);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.RoundToNearest(x);
        }
        return Vector128.Create(
            Round(x.GetLower()),
            Round(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Round(Vector256<float> x)
    {
        if (Avx.IsSupported)
        {
            return Avx.RoundToNearestInteger(x);
        }
        return Vector256.Create(
            Round(x.GetLower()),
            Round(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Round(Vector512<float> x)
    {
        if (Avx512F.IsSupported)
        {
            // _MM_FROUND_TO_NEAREST_INT | _MM_FROUND_NO_EXC = 8
            return Avx512F.RoundScale(x, 8);
        }
        return Vector512.Create(
            Round(x.GetLower()),
            Round(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Round(Vector128<double> x)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.RoundToNearestInteger(x);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.RoundToNearest(x);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.RoundToNearest(x);
        }
        return Vector128.Create(
            Math.Round(x.GetElement(0)),
            Math.Round(x.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Round(Vector256<double> x)
    {
        if (Avx.IsSupported)
        {
            return Avx.RoundToNearestInteger(x);
        }
        return Vector256.Create(
            Round(x.GetLower()),
            Round(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Round(Vector512<double> x)
    {
        if (Avx512F.IsSupported)
        {
            // _MM_FROUND_TO_NEAREST_INT | _MM_FROUND_NO_EXC = 8
            return Avx512F.RoundScale(x, 8);
        }
        return Vector512.Create(
            Round(x.GetLower()),
            Round(x.GetUpper())
        );
    }

    public static bool IsRoundF256HardwareAccelerated
    {
        [MethodImpl(256 | 512)]
        get => Avx.IsSupported;
    }

    public static bool IsRoundD512HardwareAccelerated
    {
        [MethodImpl(256 | 512)]
        get => Avx512F.IsSupported;
    }

    #endregion

    #region RoundToZero

    [MethodImpl(256 | 512)]
    public static Vector64<float> RoundToZero(Vector64<float> x)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.RoundToZero(x.ToVector128()).GetLower();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.RoundToZero(x);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.Truncate(x.ToVector128()).GetLower();
        }
        return Vector64.Create(
            MathF.Round(x.GetElement(0), MidpointRounding.ToZero),
            MathF.Round(x.GetElement(1), MidpointRounding.ToZero)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> RoundToZero(Vector128<float> x)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.RoundToZero(x);
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.RoundToZero(x);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.Truncate(x);
        }
        return Vector128.Create(
            RoundToZero(x.GetLower()),
            RoundToZero(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> RoundToZero(Vector256<float> x)
    {
        if (Avx.IsSupported)
        {
            return Avx.RoundToZero(x);
        }
        return Vector256.Create(
            RoundToZero(x.GetLower()),
            RoundToZero(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> RoundToZero(Vector512<float> x)
    {
        if (Avx512F.IsSupported)
        {
            // _MM_FROUND_TO_ZERO | _MM_FROUND_NO_EXC = 11
            return Avx512F.RoundScale(x, 11);
        }
        return Vector512.Create(
            RoundToZero(x.GetLower()),
            RoundToZero(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> RoundToZero(Vector128<double> x)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.RoundToZero(x);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.RoundToZero(x);
        }
        if (PackedSimd.IsSupported)
        {
            return PackedSimd.Truncate(x);
        }
        return Vector128.Create(
            Math.Round(x.GetElement(0), MidpointRounding.ToZero),
            Math.Round(x.GetElement(1), MidpointRounding.ToZero)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> RoundToZero(Vector256<double> x)
    {
        if (Avx.IsSupported)
        {
            return Avx.RoundToZero(x);
        }
        return Vector256.Create(
            RoundToZero(x.GetLower()),
            RoundToZero(x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> RoundToZero(Vector512<double> x)
    {
        if (Avx512F.IsSupported)
        {
            // _MM_FROUND_TO_ZERO | _MM_FROUND_NO_EXC = 11
            return Avx512F.RoundScale(x, 11);
        }
        return Vector512.Create(
            RoundToZero(x.GetLower()),
            RoundToZero(x.GetUpper())
        );
    }

    #endregion

    #region Rem

    [MethodImpl(256 | 512)]
    public static Vector64<float> Rem(Vector64<float> a, Vector64<float> b)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Rem(a.ToVector128(), b.ToVector128()).GetLower();
        }

        return Vector64.Create(
            a.GetElement(0) % b.GetElement(0),
            a.GetElement(1) % b.GetElement(1)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Rem(Vector128<float> a, Vector128<float> b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector128.Create(
            Rem(a.GetLower(), b.GetLower()),
            Rem(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Rem(Vector256<float> a, Vector256<float> b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector256.Create(
            Rem(a.GetLower(), b.GetLower()),
            Rem(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Rem(Vector512<float> a, Vector512<float> b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector512.Create(
            Rem(a.GetLower(), b.GetLower()),
            Rem(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Rem(Vector128<double> a, Vector128<double> b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector128.Create(
            a.GetElement(0) % b.GetElement(0),
            a.GetElement(1) % b.GetElement(1)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Rem(Vector256<double> a, Vector256<double> b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector256.Create(
            Rem(a.GetLower(), b.GetLower()),
            Rem(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Rem(Vector512<double> a, Vector512<double> b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector512.Create(
            Rem(a.GetLower(), b.GetLower()),
            Rem(a.GetUpper(), b.GetUpper())
        );
    }

    #endregion

    #region Rem Scalar

    [MethodImpl(256 | 512)]
    public static Vector64<float> Rem(Vector64<float> a, float b)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Rem(a.ToVector128(), b).GetLower();
        }

        return Vector64.Create(
            a.GetElement(0) % b,
            a.GetElement(1) % b
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Rem(Vector128<float> a, float b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector128.Create(
            Rem(a.GetLower(), b),
            Rem(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Rem(Vector256<float> a, float b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector256.Create(
            Rem(a.GetLower(), b),
            Rem(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Rem(Vector512<float> a, float b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector512.Create(
            Rem(a.GetLower(), b),
            Rem(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Rem(Vector128<double> a, double b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector128.Create(
            a.GetElement(0) % b,
            a.GetElement(1) % b
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Rem(Vector256<double> a, double b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector256.Create(
            Rem(a.GetLower(), b),
            Rem(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Rem(Vector512<double> a, double b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Rem(a, b);
        }

        return Vector512.Create(
            Rem(a.GetLower(), b),
            Rem(a.GetUpper(), b)
        );
    }

    #endregion

    #region Mod

    [MethodImpl(256 | 512)]
    public static Vector64<float> Mod(Vector64<float> a, Vector64<float> b)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Mod(a.ToVector128(), b.ToVector128()).GetLower();
        }

        return Vector64.Create(
            a.GetElement(0).mod(b.GetElement(0)),
            a.GetElement(1).mod(b.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Mod(Vector128<float> a, Vector128<float> b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector128.Create(
            Mod(a.GetLower(), b.GetLower()),
            Mod(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Mod(Vector256<float> a, Vector256<float> b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector256.Create(
            Mod(a.GetLower(), b.GetLower()),
            Mod(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Mod(Vector512<float> a, Vector512<float> b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector512.Create(
            Mod(a.GetLower(), b.GetLower()),
            Mod(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Mod(Vector128<double> a, Vector128<double> b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector128.Create(
            a.GetElement(0).mod(b.GetElement(0)),
            a.GetElement(1).mod(b.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Mod(Vector256<double> a, Vector256<double> b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector256.Create(
            Mod(a.GetLower(), b.GetLower()),
            Mod(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Mod(Vector512<double> a, Vector512<double> b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector512.Create(
            Mod(a.GetLower(), b.GetLower()),
            Mod(a.GetUpper(), b.GetUpper())
        );
    }

    #endregion

    #region Mod Scalar

    [MethodImpl(256 | 512)]
    public static Vector64<float> Mod(Vector64<float> a, float b)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Mod(a.ToVector128(), b).GetLower();
        }

        return Vector64.Create(
            a.GetElement(0).mod(b),
            a.GetElement(1).mod(b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Mod(Vector128<float> a, float b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector128.Create(
            Mod(a.GetLower(), b),
            Mod(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Mod(Vector256<float> a, float b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector256.Create(
            Mod(a.GetLower(), b),
            Mod(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Mod(Vector512<float> a, float b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector512.Create(
            Mod(a.GetLower(), b),
            Mod(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Mod(Vector128<double> a, double b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector128.Create(
            a.GetElement(0).mod(b),
            a.GetElement(1).mod(b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Mod(Vector256<double> a, double b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector256.Create(
            Mod(a.GetLower(), b),
            Mod(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Mod(Vector512<double> a, double b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Mod(a, b);
        }

        return Vector512.Create(
            Mod(a.GetLower(), b),
            Mod(a.GetUpper(), b)
        );
    }

    #endregion

    #region ModF

    [MethodImpl(256 | 512)]
    public static Vector64<float> ModF(Vector64<float> d, out Vector64<float> i)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }
        if (Vector128.IsHardwareAccelerated)
        {
            var r128 = ModF(d.ToVector128(), out var i128);
            i = i128.GetLower();
            return r128.GetLower();
        }

        var r = Vector64.Create(
            d.GetElement(0).modf(out var i0),
            d.GetElement(1).modf(out var i1)
        );
        i = Vector64.Create(i0, i1);
        return r;
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> ModF(Vector128<float> d, out Vector128<float> i)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }

        var r = Vector128.Create(
            ModF(d.GetLower(), out var lower),
            ModF(d.GetUpper(), out var upper)
        );
        i = Vector128.Create(lower, upper);
        return r;
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> ModF(Vector256<float> d, out Vector256<float> i)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }

        var r = Vector256.Create(
            ModF(d.GetLower(), out var lower),
            ModF(d.GetUpper(), out var upper)
        );
        i = Vector256.Create(lower, upper);
        return r;
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> ModF(Vector512<float> d, out Vector512<float> i)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }

        var r = Vector512.Create(
            ModF(d.GetLower(), out var lower),
            ModF(d.GetUpper(), out var upper)
        );
        i = Vector512.Create(lower, upper);
        return r;
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> ModF(Vector128<double> d, out Vector128<double> i)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }

        var r = Vector128.Create(
            d.GetElement(0).modf(out var i0),
            d.GetElement(1).modf(out var i1)
        );
        i = Vector128.Create(i0, i1);
        return r;
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> ModF(Vector256<double> d, out Vector256<double> i)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }

        var r = Vector256.Create(
            ModF(d.GetLower(), out var lower),
            ModF(d.GetUpper(), out var upper)
        );
        i = Vector256.Create(lower, upper);
        return r;
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> ModF(Vector512<double> d, out Vector512<double> i)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            i = RoundToZero(d);
            return d - i;
        }

        var r = Vector512.Create(
            ModF(d.GetLower(), out var lower),
            ModF(d.GetUpper(), out var upper)
        );
        i = Vector512.Create(lower, upper);
        return r;
    }

    #endregion

    #region Sign

    [MethodImpl(256 | 512)]
    public static Vector64<T> SignInt<T>(Vector64<T> v)
    {
        var pos = Vector64.GreaterThan(v, default) & Vector64<T>.One;
        var neg = Vector64.LessThan(v, default) & -Vector64<T>.One;
        return pos | neg;
    }

    [MethodImpl(256 | 512)]
    public static Vector64<T> SignUInt<T>(Vector64<T> v)
    {
        return Vector64.GreaterThan(v, default) & Vector64<T>.One;
    }

    [MethodImpl(256 | 512)]
    public static Vector64<T> SignFloat<T>(Vector64<T> v)
    {
        var a = v & -Vector64<T>.Zero | Vector64<T>.One;
        var c = Vector64.Equals(v, default);
        return Vector64.AndNot(a, c);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<T> SignInt<T>(Vector128<T> v)
    {
        var pos = Vector128.GreaterThan(v, default) & Vector128<T>.One;
        var neg = Vector128.LessThan(v, default) & -Vector128<T>.One;
        return pos | neg;
    }

    [MethodImpl(256 | 512)]
    public static Vector128<T> SignUInt<T>(Vector128<T> v)
    {
        return Vector128.GreaterThan(v, default) & Vector128<T>.One;
    }

    [MethodImpl(256 | 512)]
    public static Vector128<T> SignFloat<T>(Vector128<T> v)
    {
        var a = v & -Vector128<T>.Zero | Vector128<T>.One;
        var c = Vector128.Equals(v, default);
        return Vector128.AndNot(a, c);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<T> SignInt<T>(Vector256<T> v)
    {
        var pos = Vector256.GreaterThan(v, default) & Vector256<T>.One;
        var neg = Vector256.LessThan(v, default) & -Vector256<T>.One;
        return pos | neg;
    }

    [MethodImpl(256 | 512)]
    public static Vector256<T> SignUInt<T>(Vector256<T> v)
    {
        return Vector256.GreaterThan(v, default) & Vector256<T>.One;
    }

    [MethodImpl(256 | 512)]
    public static Vector256<T> SignFloat<T>(Vector256<T> v)
    {
        var a = v & -Vector256<T>.Zero | Vector256<T>.One;
        var c = Vector256.Equals(v, default);
        return Vector256.AndNot(a, c);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<T> SignInt<T>(Vector512<T> v)
    {
        var pos = Vector512.GreaterThan(v, default) & Vector512<T>.One;
        var neg = Vector512.LessThan(v, default) & -Vector512<T>.One;
        return pos | neg;
    }

    [MethodImpl(256 | 512)]
    public static Vector512<T> SignUInt<T>(Vector512<T> v)
    {
        return Vector512.GreaterThan(v, default) & Vector512<T>.One;
    }

    [MethodImpl(256 | 512)]
    public static Vector512<T> SignFloat<T>(Vector512<T> v)
    {
        var a = v & -Vector512<T>.Zero | Vector512<T>.One;
        var c = Vector512.Equals(v, default);
        return Vector512.AndNot(a, c);
    }

    #endregion

    #region Fma

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector64<float> Fma(Vector64<float> a, Vector64<float> b, Vector64<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAdd(a.ToVector128(), b.ToVector128(), c.ToVector128()).GetLower();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplyAdd(c, a, b);
        }
        return a * b + c;
    }

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector128<float> Fma(Vector128<float> a, Vector128<float> b, Vector128<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAdd(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplyAdd(c, a, b);
        }
        return a * b + c;
    }

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector128<double> Fma(Vector128<double> a, Vector128<double> b, Vector128<double> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAdd(a, b, c);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.FusedMultiplyAdd(c, a, b);
        }
        return a * b + c;
    }

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector256<float> Fma(Vector256<float> a, Vector256<float> b, Vector256<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAdd(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            return Vector256.Create(
                AdvSimd.FusedMultiplyAdd(c.GetLower(), a.GetLower(), b.GetLower()),
                AdvSimd.FusedMultiplyAdd(c.GetUpper(), a.GetUpper(), b.GetUpper())
            );
        }
        return a * b + c;
    }

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector256<double> Fma(Vector256<double> a, Vector256<double> b, Vector256<double> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAdd(a, b, c);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return Vector256.Create(
                AdvSimd.Arm64.FusedMultiplyAdd(c.GetLower(), a.GetLower(), b.GetLower()),
                AdvSimd.Arm64.FusedMultiplyAdd(c.GetUpper(), a.GetUpper(), b.GetUpper())
            );
        }
        return a * b + c;
    }

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector512<float> Fma(Vector512<float> a, Vector512<float> b, Vector512<float> c)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.FusedMultiplyAdd(a, b, c);
        }
        if (X86.Fma.IsSupported)
        {
            return Vector512.Create(
                X86.Fma.MultiplyAdd(a.GetLower(), b.GetLower(), c.GetLower()),
                X86.Fma.MultiplyAdd(a.GetUpper(), b.GetUpper(), c.GetUpper())
            );
        }
        return a * b + c;
    }

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector512<double> Fma(Vector512<double> a, Vector512<double> b, Vector512<double> c)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.FusedMultiplyAdd(a, b, c);
        }
        if (X86.Fma.IsSupported)
        {
            return Vector512.Create(
                X86.Fma.MultiplyAdd(a.GetLower(), b.GetLower(), c.GetLower()),
                X86.Fma.MultiplyAdd(a.GetUpper(), b.GetUpper(), c.GetUpper())
            );
        }
        return a * b + c;
    }

    #endregion

    #region Fms

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector64<float> Fms(Vector64<float> a, Vector64<float> b, Vector64<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplySubtract(a.ToVector128(), b.ToVector128(), c.ToVector128()).GetLower();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplyAdd(-c, a, b);
        }
        return a * b - c;
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector128<float> Fms(Vector128<float> a, Vector128<float> b, Vector128<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplySubtract(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplyAdd(-c, a, b);
        }
        return a * b - c;
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector128<double> Fms(Vector128<double> a, Vector128<double> b, Vector128<double> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplySubtract(a, b, c);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.FusedMultiplyAdd(-c, a, b);
        }
        return a * b - c;
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector256<float> Fms(Vector256<float> a, Vector256<float> b, Vector256<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplySubtract(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            var nc = -c;
            return Vector256.Create(
                AdvSimd.FusedMultiplyAdd(nc.GetLower(), a.GetLower(), b.GetLower()),
                AdvSimd.FusedMultiplyAdd(nc.GetUpper(), a.GetUpper(), b.GetUpper())
            );
        }
        return a * b - c;
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector256<double> Fms(Vector256<double> a, Vector256<double> b, Vector256<double> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplySubtract(a, b, c);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            var nc = -c;
            return Vector256.Create(
                AdvSimd.Arm64.FusedMultiplyAdd(nc.GetLower(), a.GetLower(), b.GetLower()),
                AdvSimd.Arm64.FusedMultiplyAdd(nc.GetUpper(), a.GetUpper(), b.GetUpper())
            );
        }
        return a * b - c;
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector512<float> Fms(Vector512<float> a, Vector512<float> b, Vector512<float> c)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.FusedMultiplySubtract(a, b, c);
        }
        if (X86.Fma.IsSupported)
        {
            return Vector512.Create(
                X86.Fma.MultiplySubtract(a.GetLower(), b.GetLower(), c.GetLower()),
                X86.Fma.MultiplySubtract(a.GetUpper(), b.GetUpper(), c.GetUpper())
            );
        }
        return a * b - c;
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector512<double> Fms(Vector512<double> a, Vector512<double> b, Vector512<double> c)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.FusedMultiplySubtract(a, b, c);
        }
        if (X86.Fma.IsSupported)
        {
            return Vector512.Create(
                X86.Fma.MultiplySubtract(a.GetLower(), b.GetLower(), c.GetLower()),
                X86.Fma.MultiplySubtract(a.GetUpper(), b.GetUpper(), c.GetUpper())
            );
        }
        return a * b - c;
    }

    #endregion

    #region Fnma

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector64<float> Fnma(Vector64<float> a, Vector64<float> b, Vector64<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAddNegated(a.ToVector128(), b.ToVector128(), c.ToVector128()).GetLower();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplySubtract(c, a, b);
        }
        return c - a * b;
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector128<float> Fnma(Vector128<float> a, Vector128<float> b, Vector128<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAddNegated(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplySubtract(c, a, b);
        }
        return c - a * b;
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector128<double> Fnma(Vector128<double> a, Vector128<double> b, Vector128<double> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAddNegated(a, b, c);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return AdvSimd.Arm64.FusedMultiplySubtract(c, a, b);
        }
        return c - a * b;
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector256<float> Fnma(Vector256<float> a, Vector256<float> b, Vector256<float> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAddNegated(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            return Vector256.Create(
                AdvSimd.FusedMultiplySubtract(c.GetLower(), a.GetLower(), b.GetLower()),
                AdvSimd.FusedMultiplySubtract(c.GetUpper(), a.GetUpper(), b.GetUpper())
            );
        }
        return c - a * b;
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector256<double> Fnma(Vector256<double> a, Vector256<double> b, Vector256<double> c)
    {
        if (X86.Fma.IsSupported)
        {
            return X86.Fma.MultiplyAddNegated(a, b, c);
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            return Vector256.Create(
                AdvSimd.Arm64.FusedMultiplySubtract(c.GetLower(), a.GetLower(), b.GetLower()),
                AdvSimd.Arm64.FusedMultiplySubtract(c.GetUpper(), a.GetUpper(), b.GetUpper())
            );
        }
        return c - a * b;
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector512<float> Fnma(Vector512<float> a, Vector512<float> b, Vector512<float> c)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.FusedMultiplyAddNegated(a, b, c);
        }
        if (AdvSimd.IsSupported)
        {
            return Vector512.Create(
                Fnma(a.GetLower(), b.GetLower(), c.GetLower()),
                Fnma(a.GetUpper(), b.GetUpper(), c.GetUpper())
            );
        }
        return c - a * b;
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector512<double> Fnma(Vector512<double> a, Vector512<double> b, Vector512<double> c)
    {
        if (Avx512F.IsSupported)
        {
            return Avx512F.FusedMultiplyAddNegated(a, b, c);
        }
        return c - a * b;
    }

    #endregion

    #region IsInfinity

    [MethodImpl(256 | 512)]
    public static Vector64<int> IsInfinity(Vector64<float> f)
    {
        var bits = f.AsInt32();
        return Vector64.Equals(bits & Vector64.Create(int.MaxValue), Vector64.Create(0x7F800000));
    }

    [MethodImpl(256 | 512)]
    public static Vector128<int> IsInfinity(Vector128<float> f)
    {
        var bits = f.AsInt32();
        return Vector128.Equals(bits & Vector128.Create(int.MaxValue), Vector128.Create(0x7F800000));
    }

    [MethodImpl(256 | 512)]
    public static Vector256<int> IsInfinity(Vector256<float> f)
    {
        var bits = f.AsInt32();
        return Vector256.Equals(bits & Vector256.Create(int.MaxValue), Vector256.Create(0x7F800000));
    }

    [MethodImpl(256 | 512)]
    public static Vector512<int> IsInfinity(Vector512<float> f)
    {
        var bits = f.AsInt32();
        return Vector512.Equals(bits & Vector512.Create(int.MaxValue), Vector512.Create(0x7F800000));
    }

    [MethodImpl(256 | 512)]
    public static Vector128<long> IsInfinity(Vector128<double> f)
    {
        var bits = f.AsInt64();
        return Vector128.Equals(bits & Vector128.Create(long.MaxValue), Vector128.Create(0x7FF0000000000000L));
    }

    [MethodImpl(256 | 512)]
    public static Vector256<long> IsInfinity(Vector256<double> f)
    {
        var bits = f.AsInt64();
        return Vector256.Equals(bits & Vector256.Create(long.MaxValue), Vector256.Create(0x7FF0000000000000L));
    }

    [MethodImpl(256 | 512)]
    public static Vector512<long> IsInfinity(Vector512<double> f)
    {
        var bits = f.AsInt64();
        return Vector512.Equals(bits & Vector512.Create(long.MaxValue), Vector512.Create(0x7FF0000000000000L));
    }

    #endregion

    #region CMin

    [MethodImpl(256 | 512)]
    public static uint CMin(Vector128<uint> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1).AsUInt32());
            var c = Vector128.Min(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2).AsUInt32());
            var e = Vector128.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), Math.Min(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static int CMin(Vector128<int> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = Vector128.Min(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = Vector128.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), Math.Min(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static float CMin(Vector128<float> a)
    {
        if (Sse.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = Sse.Min(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = Sse.Min(c, d);
            return e[0];
        }
        if (AdvSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = AdvSimd.Min(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = AdvSimd.Min(c, d);
            return e[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = PackedSimd.Min(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = PackedSimd.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), Math.Min(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static ulong CMin(Vector256<ulong> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(2, 3, 0, 1).AsUInt64());
            var c = Vector256.Min(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(1, 0, 3, 2).AsUInt64());
            var e = Vector256.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), Math.Min(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static long CMin(Vector256<long> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(2, 3, 0, 1));
            var c = Vector256.Min(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(1, 0, 3, 2));
            var e = Vector256.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), Math.Min(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static double CMin(Vector256<double> a)
    {
        if (Avx.IsSupported)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(2, 3, 0, 1));
            var c = Avx.Min(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(1, 0, 3, 2));
            var e = Avx.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), Math.Min(a[2], a[3]));
    }

    #endregion

    #region CMax

    [MethodImpl(256 | 512)]
    public static uint CMax(Vector128<uint> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1).AsUInt32());
            var c = Vector128.Max(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2).AsUInt32());
            var e = Vector128.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), Math.Max(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static int CMax(Vector128<int> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = Vector128.Max(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = Vector128.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), Math.Max(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static float CMax(Vector128<float> a)
    {
        if (Sse.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = Sse.Max(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = Sse.Max(c, d);
            return e[0];
        }
        if (AdvSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = AdvSimd.Max(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = AdvSimd.Max(c, d);
            return e[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(2, 3, 0, 1));
            var c = PackedSimd.Max(a, b);
            var d = Vector128.Shuffle(c, Vector128.Create(1, 0, 3, 2));
            var e = PackedSimd.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), Math.Max(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static ulong CMax(Vector256<ulong> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(2, 3, 0, 1).AsUInt64());
            var c = Vector256.Max(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(1, 0, 3, 2).AsUInt64());
            var e = Vector256.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), Math.Max(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static long CMax(Vector256<long> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(2, 3, 0, 1));
            var c = Vector256.Max(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(1, 0, 3, 2));
            var e = Vector256.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), Math.Max(a[2], a[3]));
    }

    [MethodImpl(256 | 512)]
    public static double CMax(Vector256<double> a)
    {
        if (Avx.IsSupported)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(2, 3, 0, 1));
            var c = Avx.Max(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(1, 0, 3, 2));
            var e = Avx.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), Math.Max(a[2], a[3]));
    }

    #endregion

    #region CMin3

    [MethodImpl(256 | 512)]
    public static uint CMin3(Vector128<uint> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1).AsUInt32());
            var c = Vector128.Min(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2).AsUInt32());
            var e = Vector128.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static int CMin3(Vector128<int> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = Vector128.Min(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = Vector128.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static float CMin3(Vector128<float> a)
    {
        if (Sse.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = Sse.Min(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = Sse.Min(c, d);
            return e[0];
        }
        if (AdvSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = AdvSimd.Min(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = AdvSimd.Min(c, d);
            return e[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = PackedSimd.Min(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = PackedSimd.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static ulong CMin3(Vector256<ulong> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(1, 1, 1, 1).AsUInt64());
            var c = Vector256.Min(a, b);
            var d = Vector256.Shuffle(a, Vector256.Create(2, 2, 2, 2).AsUInt64());
            var e = Vector256.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static long CMin3(Vector256<long> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(1, 1, 1, 1));
            var c = Vector256.Min(a, b);
            var d = Vector256.Shuffle(a, Vector256.Create(2, 2, 2, 2));
            var e = Vector256.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static double CMin3(Vector256<double> a)
    {
        if (Avx.IsSupported)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(1, 1, 1, 1));
            var c = Avx.Min(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(2, 2, 2, 2));
            var e = Avx.Min(c, d);
            return e[0];
        }
        return Math.Min(Math.Min(a[0], a[1]), a[2]);
    }

    #endregion

    #region CMax3

    [MethodImpl(256 | 512)]
    public static uint CMax3(Vector128<uint> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1).AsUInt32());
            var c = Vector128.Max(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2).AsUInt32());
            var e = Vector128.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static int CMax3(Vector128<int> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = Vector128.Max(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = Vector128.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static float CMax3(Vector128<float> a)
    {
        if (Sse.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = Sse.Max(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = Sse.Max(c, d);
            return e[0];
        }
        if (AdvSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = AdvSimd.Max(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = AdvSimd.Max(c, d);
            return e[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 1, 1, 1));
            var c = PackedSimd.Max(a, b);
            var d = Vector128.Shuffle(a, Vector128.Create(2, 2, 2, 2));
            var e = PackedSimd.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static ulong CMax3(Vector256<ulong> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(1, 1, 1, 1).AsUInt64());
            var c = Vector256.Max(a, b);
            var d = Vector256.Shuffle(a, Vector256.Create(2, 2, 2, 2).AsUInt64());
            var e = Vector256.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static long CMax3(Vector256<long> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(1, 1, 1, 1));
            var c = Vector256.Max(a, b);
            var d = Vector256.Shuffle(a, Vector256.Create(2, 2, 2, 2));
            var e = Vector256.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), a[2]);
    }

    [MethodImpl(256 | 512)]
    public static double CMax3(Vector256<double> a)
    {
        if (Avx.IsSupported)
        {
            var b = Vector256.Shuffle(a, Vector256.Create(1, 1, 1, 1));
            var c = Avx.Max(a, b);
            var d = Vector256.Shuffle(c, Vector256.Create(2, 2, 2, 2));
            var e = Avx.Max(c, d);
            return e[0];
        }
        return Math.Max(Math.Max(a[0], a[1]), a[2]);
    }

    #endregion

    #region CMin2

    [MethodImpl(256 | 512)]
    public static uint CMin(Vector64<uint> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            var b = Vector64.Shuffle(a, Vector64.Create(1, 0).AsUInt32());
            var c = Vector64.Min(a, b);
            return c[0];
        }
        return Math.Min(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static int CMin(Vector64<int> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            var b = Vector64.Shuffle(a, Vector64.Create(1, 0));
            var c = Vector64.Min(a, b);
            return c[0];
        }
        return Math.Min(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static float CMin(Vector64<float> a)
    {
        if (Sse.IsSupported)
        {
            var b = Vector128.Create(a, a);
            var c = Vector128.Shuffle(b, Vector128.Create(1, 0, 3, 2));
            var d = Sse.Min(c, b);
            return d[0];
        }
        if (AdvSimd.IsSupported)
        {
            var b = Vector64.Shuffle(a, Vector64.Create(1, 0));
            var c = AdvSimd.Min(a, b);
            return c[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Create(a, a);
            var c = Vector128.Shuffle(b, Vector128.Create(1, 0, 3, 2));
            var d = PackedSimd.Min(c, b);
            return d[0];
        }
        return Math.Min(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static ulong CMin(Vector128<ulong> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0).AsUInt64());
            var c = Vector128.Min(a, b);
            return c[0];
        }
        return Math.Min(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static long CMin(Vector128<long> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = Vector128.Min(a, b);
            return c[0];
        }
        return Math.Min(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static double CMin(Vector128<double> a)
    {
        if (Sse2.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = Sse2.Min(a, b);
            return c[0];
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = AdvSimd.Arm64.Min(a, b);
            return c[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = PackedSimd.Min(a, b);
            return c[0];
        }
        return Math.Min(a[0], a[1]);
    }

    #endregion

    #region CMax2

    [MethodImpl(256 | 512)]
    public static uint CMax(Vector64<uint> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            var b = Vector64.Shuffle(a, Vector64.Create(1, 0).AsUInt32());
            var c = Vector64.Max(a, b);
            return c[0];
        }
        return Math.Max(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static int CMax(Vector64<int> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            var b = Vector64.Shuffle(a, Vector64.Create(1, 0));
            var c = Vector64.Max(a, b);
            return c[0];
        }
        return Math.Max(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static float CMax(Vector64<float> a)
    {
        if (Sse.IsSupported)
        {
            var b = Vector128.Create(a, a);
            var c = Vector128.Shuffle(b, Vector128.Create(1, 0, 3, 2));
            var d = Sse.Max(c, b);
            return d[0];
        }
        if (AdvSimd.IsSupported)
        {
            var b = Vector64.Shuffle(a, Vector64.Create(1, 0));
            var c = AdvSimd.Max(a, b);
            return c[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Create(a, a);
            var c = Vector128.Shuffle(b, Vector128.Create(1, 0, 3, 2));
            var d = PackedSimd.Max(c, b);
            return d[0];
        }
        return Math.Max(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static ulong CMax(Vector128<ulong> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0).AsUInt64());
            var c = Vector128.Max(a, b);
            return c[0];
        }
        return Math.Max(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static long CMax(Vector128<long> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = Vector128.Max(a, b);
            return c[0];
        }
        return Math.Max(a[0], a[1]);
    }

    [MethodImpl(256 | 512)]
    public static double CMax(Vector128<double> a)
    {
        if (Sse2.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = Sse2.Max(a, b);
            return c[0];
        }
        if (AdvSimd.Arm64.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = AdvSimd.Arm64.Max(a, b);
            return c[0];
        }
        if (PackedSimd.IsSupported)
        {
            var b = Vector128.Shuffle(a, Vector128.Create(1, 0));
            var c = PackedSimd.Max(a, b);
            return c[0];
        }
        return Math.Max(a[0], a[1]);
    }

    #endregion

    #region Log

    [MethodImpl(256 | 512)]
    public static Vector64<float> Log(Vector64<float> d)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log(d.ToVector128()).GetLower();
        }
        return Vector64.Create(
            d.GetElement(0).log(),
            d.GetElement(1).log()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Log(Vector128<float> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        return Vector128.Create(
            Log(d.GetLower()),
            Log(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Log(Vector256<float> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        return Vector256.Create(
            Log(d.GetLower()),
            Log(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Log(Vector512<float> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        return Vector512.Create(
            Log(d.GetLower()),
            Log(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Log(Vector128<double> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        return Vector128.Create(
            d.GetElement(0).log(),
            d.GetElement(1).log()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Log(Vector256<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        return Vector256.Create(
            Log(d.GetLower()),
            Log(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Log(Vector512<double> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Log(d);
        }
        return Vector512.Create(
            Log(d.GetLower()),
            Log(d.GetUpper())
        );
    }

    #endregion

    #region Log2

    [MethodImpl(256 | 512)]
    public static Vector64<float> Log2(Vector64<float> d)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log2(d.ToVector128()).GetLower();
        }
        return Vector64.Create(
            d.GetElement(0).log2(),
            d.GetElement(1).log2()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Log2(Vector128<float> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        return Vector128.Create(
            Log2(d.GetLower()),
            Log2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Log2(Vector256<float> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        return Vector256.Create(
            Log2(d.GetLower()),
            Log2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Log2(Vector512<float> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        return Vector512.Create(
            Log2(d.GetLower()),
            Log2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Log2(Vector128<double> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        return Vector128.Create(
            d.GetElement(0).log2(),
            d.GetElement(1).log2()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Log2(Vector256<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        return Vector256.Create(
            Log2(d.GetLower()),
            Log2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Log2(Vector512<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log2(d);
        }
        return Vector512.Create(
            Log2(d.GetLower()),
            Log2(d.GetUpper())
        );
    }

    #endregion

    #region Log10

    [MethodImpl(256 | 512)]
    public static Vector64<float> Log10(Vector64<float> d)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log10(d.ToVector128()).GetLower();
        }
        return Vector64.Create(
            d.GetElement(0).log10(),
            d.GetElement(1).log10()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Log10(Vector128<float> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        return Vector128.Create(
            Log10(d.GetLower()),
            Log10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Log10(Vector256<float> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        return Vector256.Create(
            Log10(d.GetLower()),
            Log10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Log10(Vector512<float> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        return Vector512.Create(
            Log10(d.GetLower()),
            Log10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Log10(Vector128<double> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        return Vector128.Create(
            d.GetElement(0).log10(),
            d.GetElement(1).log10()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Log10(Vector256<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        return Vector256.Create(
            Log10(d.GetLower()),
            Log10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Log10(Vector512<double> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Log10(d);
        }
        return Vector512.Create(
            Log10(d.GetLower()),
            Log10(d.GetUpper())
        );
    }

    #endregion

    #region Exp

    [MethodImpl(256 | 512)]
    public static Vector64<float> Exp(Vector64<float> d)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp(d.ToVector128()).GetLower();
        }
        return Vector64.Create(
            d.GetElement(0).exp(),
            d.GetElement(1).exp()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Exp(Vector128<float> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        return Vector128.Create(
            Exp(d.GetLower()),
            Exp(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Exp(Vector256<float> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        return Vector256.Create(
            Exp(d.GetLower()),
            Exp(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Exp(Vector512<float> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        return Vector512.Create(
            Exp(d.GetLower()),
            Exp(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Exp(Vector128<double> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        return Vector128.Create(
            d.GetElement(0).exp(),
            d.GetElement(1).exp()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Exp(Vector256<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        return Vector256.Create(
            Exp(d.GetLower()),
            Exp(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Exp(Vector512<double> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Exp(d);
        }
        return Vector512.Create(
            Exp(d.GetLower()),
            Exp(d.GetUpper())
        );
    }

    #endregion

    #region Exp2

    [MethodImpl(256 | 512)]
    public static Vector64<float> Exp2(Vector64<float> d)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d.ToVector128()).GetLower();
        }
        return Vector64.Create(
            d.GetElement(0).exp2(),
            d.GetElement(1).exp2()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Exp2(Vector128<float> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        return Vector128.Create(
            Exp2(d.GetLower()),
            Exp2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Exp2(Vector256<float> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        return Vector256.Create(
            Exp2(d.GetLower()),
            Exp2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Exp2(Vector512<float> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        return Vector512.Create(
            Exp2(d.GetLower()),
            Exp2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Exp2(Vector128<double> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        return Vector128.Create(
            d.GetElement(0).exp2(),
            d.GetElement(1).exp2()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Exp2(Vector256<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        return Vector256.Create(
            Exp2(d.GetLower()),
            Exp2(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Exp2(Vector512<double> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Exp2(d);
        }
        return Vector512.Create(
            Exp2(d.GetLower()),
            Exp2(d.GetUpper())
        );
    }

    #endregion

    #region Exp10

    [MethodImpl(256 | 512)]
    public static Vector64<float> Exp10(Vector64<float> d)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d.ToVector128()).GetLower();
        }
        return Vector64.Create(
            d.GetElement(0).exp10(),
            d.GetElement(1).exp10()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Exp10(Vector128<float> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        return Vector128.Create(
            Exp10(d.GetLower()),
            Exp10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Exp10(Vector256<float> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        return Vector256.Create(
            Exp10(d.GetLower()),
            Exp10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Exp10(Vector512<float> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        return Vector512.Create(
            Exp10(d.GetLower()),
            Exp10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Exp10(Vector128<double> d)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        return Vector128.Create(
            d.GetElement(0).exp10(),
            d.GetElement(1).exp10()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Exp10(Vector256<double> d)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        return Vector256.Create(
            Exp10(d.GetLower()),
            Exp10(d.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Exp10(Vector512<double> d)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Exp10(d);
        }
        return Vector512.Create(
            Exp10(d.GetLower()),
            Exp10(d.GetUpper())
        );
    }

    #endregion

    #region Pow

    [MethodImpl(256 | 512)]
    public static Vector64<float> Pow(Vector64<float> a, Vector64<float> b)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Pow(a.ToVector128(), b.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).pow(b.GetElement(0)),
            a.GetElement(1).pow(b.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Pow(Vector128<float> a, Vector128<float> b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector128.Create(
            Pow(a.GetLower(), b.GetLower()),
            Pow(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Pow(Vector256<float> a, Vector256<float> b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector256.Create(
            Pow(a.GetLower(), b.GetLower()),
            Pow(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Pow(Vector512<float> a, Vector512<float> b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector512.Create(
            Pow(a.GetLower(), b.GetLower()),
            Pow(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Pow(Vector128<double> a, Vector128<double> b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector128.Create(
            a.GetElement(0).pow(b.GetElement(0)),
            a.GetElement(1).pow(b.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Pow(Vector256<double> a, Vector256<double> b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector256.Create(
            Pow(a.GetLower(), b.GetLower()),
            Pow(a.GetUpper(), b.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Pow(Vector512<double> a, Vector512<double> b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector512.Create(
            Pow(a.GetLower(), b.GetLower()),
            Pow(a.GetUpper(), b.GetUpper())
        );
    }

    #endregion

    #region Pow Scalar

    [MethodImpl(256 | 512)]
    public static Vector64<float> Pow(Vector64<float> a, float b)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Pow(a.ToVector128(), b).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).pow(b),
            a.GetElement(1).pow(b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Pow(Vector128<float> a, float b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector128.Create(
            Pow(a.GetLower(), b),
            Pow(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Pow(Vector256<float> a, float b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector256.Create(
            Pow(a.GetLower(), b),
            Pow(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Pow(Vector512<float> a, float b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector512.Create(
            Pow(a.GetLower(), b),
            Pow(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Pow(Vector128<double> a, double b)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector128.Create(
            a.GetElement(0).pow(b),
            a.GetElement(1).pow(b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Pow(Vector256<double> a, double b)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector256.Create(
            Pow(a.GetLower(), b),
            Pow(a.GetUpper(), b)
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Pow(Vector512<double> a, double b)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Pow(a, b);
        }
        return Vector512.Create(
            Pow(a.GetLower(), b),
            Pow(a.GetUpper(), b)
        );
    }

    #endregion

    #region Rcp

    [MethodImpl(256 | 512)]
    public static Vector64<float> Rcp(Vector64<float> a)
    {
        if (AdvSimd.IsSupported) return AdvSimd.ReciprocalEstimate(a);
        if (Avx512F.VL.IsSupported || Sse.IsSupported) return Rcp(a.ToVector128()).GetLower();

        return Vector64<float>.One / a;
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Rcp(Vector128<float> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.Reciprocal14(a);
        if (Sse.IsSupported) return Sse.Reciprocal(a);
        if (AdvSimd.IsSupported) return AdvSimd.ReciprocalEstimate(a);

        return Vector128<float>.One / a;
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Rcp(Vector256<float> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.Reciprocal14(a);
        if (Avx.IsSupported) return Avx.Reciprocal(a);

        return Vector256<float>.One / a;
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Rcp(Vector512<float> a)
    {
        if (Avx512F.IsSupported) return Avx512F.Reciprocal14(a);
        return Vector512<float>.One / a;
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Rcp(Vector128<double> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.Reciprocal14(a);
        if (AdvSimd.Arm64.IsSupported) return AdvSimd.Arm64.ReciprocalEstimate(a);

        return Vector128<double>.One / a;
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Rcp(Vector256<double> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.Reciprocal14(a);
        if (AdvSimd.Arm64.IsSupported)
        {
            return Vector256.Create(
                AdvSimd.Arm64.ReciprocalEstimate(a.GetLower()),
                AdvSimd.Arm64.ReciprocalEstimate(a.GetUpper())
            );
        }

        return Vector256<double>.One / a;
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Rcp(Vector512<double> a)
    {
        if (Avx512F.IsSupported) return Avx512F.Reciprocal14(a);
        return Vector512<double>.One / a;
    }

    #endregion

    #region RSqrt

    [MethodImpl(256 | 512)]
    public static Vector64<float> RSqrt(Vector64<float> a)
    {
        if (AdvSimd.IsSupported) return AdvSimd.ReciprocalSquareRootEstimate(a);
        if (Avx512F.VL.IsSupported || Sse.IsSupported) return RSqrt(a.ToVector128()).GetLower();

        return Vector64<float>.One / Vector64.Sqrt(a);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> RSqrt(Vector128<float> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.ReciprocalSqrt14(a);
        if (Sse.IsSupported) return Sse.ReciprocalSqrt(a);
        if (AdvSimd.IsSupported) return AdvSimd.ReciprocalSquareRootEstimate(a);

        return Vector128<float>.One / Vector128.Sqrt(a);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> RSqrt(Vector256<float> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.ReciprocalSqrt14(a);

        return Vector256<float>.One / Vector256.Sqrt(a);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> RSqrt(Vector512<float> a)
    {
        if (Avx512F.IsSupported) return Avx512F.ReciprocalSqrt14(a);

        return Vector512<float>.One / Vector512.Sqrt(a);
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> RSqrt(Vector128<double> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.ReciprocalSqrt14(a);
        if (AdvSimd.Arm64.IsSupported) return AdvSimd.Arm64.ReciprocalSquareRootEstimate(a);

        return Vector128<double>.One / Vector128.Sqrt(a);
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> RSqrt(Vector256<double> a)
    {
        if (Avx512F.VL.IsSupported) return Avx512F.VL.ReciprocalSqrt14(a);
        if (AdvSimd.Arm64.IsSupported)
        {
            return Vector256.Create(
                AdvSimd.Arm64.ReciprocalSquareRootEstimate(a.GetLower()),
                AdvSimd.Arm64.ReciprocalSquareRootEstimate(a.GetUpper())
            );
        }

        return Vector256<double>.One / Vector256.Sqrt(a);
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> RSqrt(Vector512<double> a)
    {
        if (Avx512F.IsSupported) return Avx512F.ReciprocalSqrt14(a);

        return Vector512<double>.One / Vector512.Sqrt(a);
    }

    #endregion

    #region Sin

    [MethodImpl(256 | 512)]
    public static Vector64<float> Sin(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Sin(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).sin(),
            a.GetElement(1).sin()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Sin(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        return Vector128.Create(
            Sin(a.GetLower()),
            Sin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Sin(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        return Vector256.Create(
            Sin(a.GetLower()),
            Sin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Sin(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        return Vector512.Create(
            Sin(a.GetLower()),
            Sin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Sin(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        return Vector128.Create(
            a.GetElement(0).sin(),
            a.GetElement(1).sin()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Sin(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        return Vector256.Create(
            Sin(a.GetLower()),
            Sin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Sin(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Sin(a);
        }
        return Vector512.Create(
            Sin(a.GetLower()),
            Sin(a.GetUpper())
        );
    }

    #endregion

    #region Cos

    [MethodImpl(256 | 512)]
    public static Vector64<float> Cos(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Cos(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).cos(),
            a.GetElement(1).cos()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Cos(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        return Vector128.Create(
            Cos(a.GetLower()),
            Cos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Cos(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        return Vector256.Create(
            Cos(a.GetLower()),
            Cos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Cos(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        return Vector512.Create(
            Cos(a.GetLower()),
            Cos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Cos(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        return Vector128.Create(
            a.GetElement(0).cos(),
            a.GetElement(1).cos()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Cos(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        return Vector256.Create(
            Cos(a.GetLower()),
            Cos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Cos(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Cos(a);
        }
        return Vector512.Create(
            Cos(a.GetLower()),
            Cos(a.GetUpper())
        );
    }

    #endregion

    #region SinCos

    [MethodImpl(256 | 512)]
    public static (Vector64<float> sin, Vector64<float> cos) SinCos(Vector64<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            var r = simd_math.SinCos(Vector128.Create(a, a));
            return (r.GetLower(), r.GetUpper());
        }
        if (Vector64.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = a.GetElement(0).sincos();
        var v1 = a.GetElement(1).sincos();
        return (Vector64.Create(v0.sin, v1.sin), Vector64.Create(v0.cos, v1.cos));
    }

    [MethodImpl(256 | 512)]
    public static (Vector128<float> sin, Vector128<float> cos) SinCos(Vector128<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            var r = simd_math.SinCos(Vector256.Create(a, a));
            return (r.GetLower(), r.GetUpper());
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = SinCos(a.GetLower());
        var v1 = SinCos(a.GetUpper());
        return (Vector128.Create(v0.sin, v1.sin), Vector128.Create(v0.cos, v1.cos));
    }

    [MethodImpl(256 | 512)]
    public static (Vector256<float> sin, Vector256<float> cos) SinCos(Vector256<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            var r = simd_math.SinCos(Vector512.Create(a, a));
            return (r.GetLower(), r.GetUpper());
        }
        if (Vector256.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = SinCos(a.GetLower());
        var v1 = SinCos(a.GetUpper());
        return (Vector256.Create(v0.sin, v1.sin), Vector256.Create(v0.cos, v1.cos));
    }

    [MethodImpl(256 | 512)]
    public static (Vector512<float> sin, Vector512<float> cos) SinCos(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = SinCos(a.GetLower());
        var v1 = SinCos(a.GetUpper());
        return (Vector512.Create(v0.sin, v1.sin), Vector512.Create(v0.cos, v1.cos));
    }

    [MethodImpl(256 | 512)]
    public static (Vector128<double> sin, Vector128<double> cos) SinCos(Vector128<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            var r = simd_math.SinCos(Vector256.Create(a, a));
            return (r.GetLower(), r.GetUpper());
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = a.GetElement(0).sincos();
        var v1 = a.GetElement(1).sincos();
        return (Vector128.Create(v0.sin, v1.sin), Vector128.Create(v0.cos, v1.cos));
    }

    [MethodImpl(256 | 512)]
    public static (Vector256<double> sin, Vector256<double> cos) SinCos(Vector256<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            var r = simd_math.SinCos(Vector512.Create(a, a));
            return (r.GetLower(), r.GetUpper());
        }
        if (Vector256.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = SinCos(a.GetLower());
        var v1 = SinCos(a.GetUpper());
        return (Vector256.Create(v0.sin, v1.sin), Vector256.Create(v0.cos, v1.cos));
    }

    [MethodImpl(256 | 512)]
    public static (Vector512<double> sin, Vector512<double> cos) SinCos(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return (Sin(a), Cos(a));
        }
        var v0 = SinCos(a.GetLower());
        var v1 = SinCos(a.GetUpper());
        return (Vector512.Create(v0.sin, v1.sin), Vector512.Create(v0.cos, v1.cos));
    }

    #endregion

    #region Tan

    [MethodImpl(256 | 512)]
    public static Vector64<float> Tan(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Tan(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Tan(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).tan(),
            a.GetElement(1).tan()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Tan(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Tan(a);
        }
        return Vector128.Create(
            Tan(a.GetLower()),
            Tan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Tan(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Tan(a);
        }
        return Vector256.Create(
            Tan(a.GetLower()),
            Tan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Tan(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Tan(a);
        }
        return Vector128.Create(
            a.GetElement(0).tan(),
            a.GetElement(1).tan()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Tan(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Tan(a);
        }
        return Vector256.Create(
            Tan(a.GetLower()),
            Tan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Tan(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Tan(a);
        }
        return Vector512.Create(
            Tan(a.GetLower()),
            Tan(a.GetUpper())
        );
    }

    #endregion

    #region Sinh

    [MethodImpl(256 | 512)]
    public static Vector64<float> Sinh(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).sinh(),
            a.GetElement(1).sinh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Sinh(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        return Vector128.Create(
            Sinh(a.GetLower()),
            Sinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Sinh(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        return Vector256.Create(
            Sinh(a.GetLower()),
            Sinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Sinh(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        return Vector512.Create(
            Sinh(a.GetLower()),
            Sinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Sinh(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        return Vector128.Create(
            a.GetElement(0).sinh(),
            a.GetElement(1).sinh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Sinh(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        return Vector256.Create(
            Sinh(a.GetLower()),
            Sinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Sinh(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Sinh(a);
        }
        return Vector512.Create(
            Sinh(a.GetLower()),
            Sinh(a.GetUpper())
        );
    }

    #endregion

    #region Cosh

    [MethodImpl(256 | 512)]
    public static Vector64<float> Cosh(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).cosh(),
            a.GetElement(1).cosh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Cosh(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        return Vector128.Create(
            Cosh(a.GetLower()),
            Cosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Cosh(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        return Vector256.Create(
            Cosh(a.GetLower()),
            Cosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Cosh(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        return Vector512.Create(
            Cosh(a.GetLower()),
            Cosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Cosh(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        return Vector128.Create(
            a.GetElement(0).cosh(),
            a.GetElement(1).cosh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Cosh(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        return Vector256.Create(
            Cosh(a.GetLower()),
            Cosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Cosh(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Cosh(a);
        }
        return Vector512.Create(
            Cosh(a.GetLower()),
            Cosh(a.GetUpper())
        );
    }

    #endregion

    #region Tanh

    [MethodImpl(256 | 512)]
    public static Vector64<float> Tanh(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).tanh(),
            a.GetElement(1).tanh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Tanh(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        return Vector128.Create(
            Tanh(a.GetLower()),
            Tanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Tanh(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        return Vector256.Create(
            Tanh(a.GetLower()),
            Tanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Tanh(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        return Vector512.Create(
            Tanh(a.GetLower()),
            Tanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Tanh(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        return Vector128.Create(
            a.GetElement(0).tanh(),
            a.GetElement(1).tanh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Tanh(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        return Vector256.Create(
            Tanh(a.GetLower()),
            Tanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Tanh(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Tanh(a);
        }
        return Vector512.Create(
            Tanh(a.GetLower()),
            Tanh(a.GetUpper())
        );
    }

    #endregion

    #region Asinh

    [MethodImpl(256 | 512)]
    public static Vector64<float> Asinh(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).asinh(),
            a.GetElement(1).asinh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Asinh(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        return Vector128.Create(
            Asinh(a.GetLower()),
            Asinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Asinh(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        return Vector256.Create(
            Asinh(a.GetLower()),
            Asinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Asinh(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        return Vector512.Create(
            Asinh(a.GetLower()),
            Asinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Asinh(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        return Vector128.Create(
            a.GetElement(0).asinh(),
            a.GetElement(1).asinh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Asinh(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        return Vector256.Create(
            Asinh(a.GetLower()),
            Asinh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Asinh(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Asinh(a);
        }
        return Vector512.Create(
            Asinh(a.GetLower()),
            Asinh(a.GetUpper())
        );
    }

    #endregion

    #region Acosh

    [MethodImpl(256 | 512)]
    public static Vector64<float> Acosh(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).acosh(),
            a.GetElement(1).acosh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Acosh(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        return Vector128.Create(
            Acosh(a.GetLower()),
            Acosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Acosh(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        return Vector256.Create(
            Acosh(a.GetLower()),
            Acosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Acosh(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        return Vector512.Create(
            Acosh(a.GetLower()),
            Acosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Acosh(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        return Vector128.Create(
            a.GetElement(0).acosh(),
            a.GetElement(1).acosh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Acosh(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        return Vector256.Create(
            Acosh(a.GetLower()),
            Acosh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Acosh(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Acosh(a);
        }
        return Vector512.Create(
            Acosh(a.GetLower()),
            Acosh(a.GetUpper())
        );
    }

    #endregion

    #region Atanh

    [MethodImpl(256 | 512)]
    public static Vector64<float> Atanh(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).atanh(),
            a.GetElement(1).atanh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Atanh(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        return Vector128.Create(
            Atanh(a.GetLower()),
            Atanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Atanh(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        return Vector256.Create(
            Atanh(a.GetLower()),
            Atanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Atanh(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        return Vector512.Create(
            Atanh(a.GetLower()),
            Atanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Atanh(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        return Vector128.Create(
            a.GetElement(0).atanh(),
            a.GetElement(1).atanh()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Atanh(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        return Vector256.Create(
            Atanh(a.GetLower()),
            Atanh(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Atanh(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Atanh(a);
        }
        return Vector512.Create(
            Atanh(a.GetLower()),
            Atanh(a.GetUpper())
        );
    }

    #endregion

    #region Asin

    [MethodImpl(256 | 512)]
    public static Vector64<float> Asin(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Asin(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).asin(),
            a.GetElement(1).asin()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Asin(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        return Vector128.Create(
            Asin(a.GetLower()),
            Asin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Asin(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        return Vector256.Create(
            Asin(a.GetLower()),
            Asin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Asin(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        return Vector512.Create(
            Asin(a.GetLower()),
            Asin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Asin(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        return Vector128.Create(
            a.GetElement(0).asin(),
            a.GetElement(1).asin()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Asin(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        return Vector256.Create(
            Asin(a.GetLower()),
            Asin(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Asin(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Asin(a);
        }
        return Vector512.Create(
            Asin(a.GetLower()),
            Asin(a.GetUpper())
        );
    }

    #endregion

    #region Acos

    [MethodImpl(256 | 512)]
    public static Vector64<float> Acos(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Acos(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).acos(),
            a.GetElement(1).acos()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Acos(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        return Vector128.Create(
            Acos(a.GetLower()),
            Acos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Acos(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        return Vector256.Create(
            Acos(a.GetLower()),
            Acos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Acos(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        return Vector512.Create(
            Acos(a.GetLower()),
            Acos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Acos(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        return Vector128.Create(
            a.GetElement(0).acos(),
            a.GetElement(1).acos()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Acos(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        return Vector256.Create(
            Acos(a.GetLower()),
            Acos(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Acos(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Acos(a);
        }
        return Vector512.Create(
            Acos(a.GetLower()),
            Acos(a.GetUpper())
        );
    }

    #endregion

    #region Atan

    [MethodImpl(256 | 512)]
    public static Vector64<float> Atan(Vector64<float> a)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atan(a.ToVector128()).GetLower();
        }
        return Vector64.Create(
            a.GetElement(0).atan(),
            a.GetElement(1).atan()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Atan(Vector128<float> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        return Vector128.Create(
            Atan(a.GetLower()),
            Atan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Atan(Vector256<float> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        return Vector256.Create(
            Atan(a.GetLower()),
            Atan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Atan(Vector512<float> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        return Vector512.Create(
            Atan(a.GetLower()),
            Atan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Atan(Vector128<double> a)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        return Vector128.Create(
            a.GetElement(0).atan(),
            a.GetElement(1).atan()
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Atan(Vector256<double> a)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        return Vector256.Create(
            Atan(a.GetLower()),
            Atan(a.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Atan(Vector512<double> a)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Atan(a);
        }
        return Vector512.Create(
            Atan(a.GetLower()),
            Atan(a.GetUpper())
        );
    }

    #endregion

    #region Atan2

    [MethodImpl(256 | 512)]
    public static Vector64<float> Atan2(Vector64<float> y, Vector64<float> x)
    {
        if (Vector64.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y.ToVector128(), x.ToVector128()).GetLower();
        }
        return Vector64.Create(
            y.GetElement(0).atan2(x.GetElement(0)),
            y.GetElement(1).atan2(x.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<float> Atan2(Vector128<float> y, Vector128<float> x)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        return Vector128.Create(
            Atan2(y.GetLower(), x.GetLower()),
            Atan2(y.GetUpper(), x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<float> Atan2(Vector256<float> y, Vector256<float> x)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        return Vector256.Create(
            Atan2(y.GetLower(), x.GetLower()),
            Atan2(y.GetUpper(), x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<float> Atan2(Vector512<float> y, Vector512<float> x)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        return Vector512.Create(
            Atan2(y.GetLower(), x.GetLower()),
            Atan2(y.GetUpper(), x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector128<double> Atan2(Vector128<double> y, Vector128<double> x)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        return Vector128.Create(
            y.GetElement(0).atan2(x.GetElement(0)),
            y.GetElement(1).atan2(x.GetElement(1))
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector256<double> Atan2(Vector256<double> y, Vector256<double> x)
    {
        if (Vector256.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        return Vector256.Create(
            Atan2(y.GetLower(), x.GetLower()),
            Atan2(y.GetUpper(), x.GetUpper())
        );
    }

    [MethodImpl(256 | 512)]
    public static Vector512<double> Atan2(Vector512<double> y, Vector512<double> x)
    {
        if (Vector512.IsHardwareAccelerated)
        {
            return simd_math.Atan2(y, x);
        }
        return Vector512.Create(
            Atan2(y.GetLower(), x.GetLower()),
            Atan2(y.GetUpper(), x.GetUpper())
        );
    }

    #endregion
}
