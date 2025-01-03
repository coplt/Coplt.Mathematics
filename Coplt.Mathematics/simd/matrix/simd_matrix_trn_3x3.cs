// generated by template, do not modify manually

using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace Coplt.Mathematics.Simd;

public static partial class simd_matrix
{
    #region Vector128<float>

    [MethodImpl(256 | 512)]
    public static (Vector128<float> c0, Vector128<float> c1, Vector128<float> c2) Transpose3x3(
        Vector128<float> c0, Vector128<float> c1, Vector128<float> c2
    )
    {
        if (Sse.IsSupported)
        {
            var a = Sse.Shuffle(c0, c1, 0x44); // a0 a1 b0 b1 => (c0.xy, c1.xy)
            var c = Sse.Shuffle(c0, c1, 0xEE); // a2 a3 b2 b3 => (c0.yz, c1.yz)
            var oc0 = Sse.Shuffle(a, c2, 0xC8); // a0 a2 b0 b3 => (a.xz, c2.xw) => (c0.x, c1.x, c2.x, 0)         
            var oc1 = Sse.Shuffle(a, c2, 0xDD); // a1 a3 a1 a3 => (a.yw, c2.yw) => (c0.y, c1.y, c2.y, 0)            
            var oc2 = Sse.Shuffle(c, c2, 0xE8); // a0 a2 b2 b3 => (c.xz, c2.zw) => (c0.z, c1.z, c2.z, 0)
            return (oc0, oc1, oc2);
        }
        {
            var oc0 = Vector128.Create(c0.GetElement(0), c1.GetElement(0), c2.GetElement(0), 0);
            var oc1 = Vector128.Create(c0.GetElement(1), c1.GetElement(1), c2.GetElement(1), 0);
            var oc2 = Vector128.Create(c0.GetElement(2), c1.GetElement(2), c2.GetElement(2), 0);
            return (oc0, oc1, oc2);
        }
    }

    #endregion
    #region Vector256<double>

    [MethodImpl(256 | 512)]
    public static (Vector256<double> c0, Vector256<double> c1, Vector256<double> c2) Transpose3x3(
        Vector256<double> c0, Vector256<double> c1, Vector256<double> c2
    )
    {
        if (Avx.IsSupported)
        {
            var a = Avx.Shuffle(c0, c1, 0x0); // a0 b0 a2 b2  => (c0.x, c1.x, c0.z, c1.z)
            var c = Avx.Shuffle(c0, c1, 0xF); // a1 b1 a3 b3  => (c0.y, c1.y, 0, 0)
            var b = Vector256.Shuffle(c2, Vector256.Create(0, 3, 2, 3)); // (c2.x, 0, c2.z, 0)
            var d = Vector256.Shuffle(c2, Vector256.Create(1, 3, 3, 3)); // (c2.y, 0, 0, 0)
            var oc0 = Avx.Permute2x128(a, b, 0x20); // a01 b01 => (c0.x, c1.x, c2.x, 0)
            var oc1 = Avx.Permute2x128(c, d, 0x20); // a01 b01 => (c0.y, c1.y, c2.y, 0)
            var oc2 = Avx.Permute2x128(a, b, 0x31); // a23 b23 => (c0.z, c1.z, c2.z, 0)
            return (oc0, oc1, oc2);
        }
        {
            var oc0 = Vector256.Create(c0.GetElement(0), c1.GetElement(0), c2.GetElement(0), 0);
            var oc1 = Vector256.Create(c0.GetElement(1), c1.GetElement(1), c2.GetElement(1), 0);
            var oc2 = Vector256.Create(c0.GetElement(2), c1.GetElement(2), c2.GetElement(2), 0);
            return (oc0, oc1, oc2);
        }
    }

    #endregion
    #region Vector128<int>

    [MethodImpl(256 | 512)]
    public static (Vector128<int> c0, Vector128<int> c1, Vector128<int> c2) Transpose3x3(
        Vector128<int> c0, Vector128<int> c1, Vector128<int> c2
    )
    {
        if (Sse.IsSupported)
        {
            var ic0 = c0.AsSingle();
            var ic1 = c1.AsSingle();
            var ic2 = c2.AsSingle();
            var a = Sse.Shuffle(ic0, ic1, 0x44); // a0 a1 b0 b1 => (c0.xy, c1.xy)
            var c = Sse.Shuffle(ic0, ic1, 0xEE); // a2 a3 b2 b3 => (c0.yz, c1.yz)
            var oc0 = Sse.Shuffle(a, ic2, 0xC8); // a0 a2 b0 b3 => (a.xz, c2.xw) => (c0.x, c1.x, c2.x, 0)         
            var oc1 = Sse.Shuffle(a, ic2, 0xDD); // a1 a3 a1 a3 => (a.yw, c2.yw) => (c0.y, c1.y, c2.y, 0)            
            var oc2 = Sse.Shuffle(c, ic2, 0xE8); // a0 a2 b2 b3 => (c.xz, c2.zw) => (c0.z, c1.z, c2.z, 0)
            return (oc0.AsInt32(), oc1.AsInt32(), oc2.AsInt32());
        }
        {
            var oc0 = Vector128.Create(c0.GetElement(0), c1.GetElement(0), c2.GetElement(0), 0);
            var oc1 = Vector128.Create(c0.GetElement(1), c1.GetElement(1), c2.GetElement(1), 0);
            var oc2 = Vector128.Create(c0.GetElement(2), c1.GetElement(2), c2.GetElement(2), 0);
            return (oc0, oc1, oc2);
        }
    }

    #endregion
    #region Vector128<uint>

    [MethodImpl(256 | 512)]
    public static (Vector128<uint> c0, Vector128<uint> c1, Vector128<uint> c2) Transpose3x3(
        Vector128<uint> c0, Vector128<uint> c1, Vector128<uint> c2
    )
    {
        if (Sse.IsSupported)
        {
            var ic0 = c0.AsSingle();
            var ic1 = c1.AsSingle();
            var ic2 = c2.AsSingle();
            var a = Sse.Shuffle(ic0, ic1, 0x44); // a0 a1 b0 b1 => (c0.xy, c1.xy)
            var c = Sse.Shuffle(ic0, ic1, 0xEE); // a2 a3 b2 b3 => (c0.yz, c1.yz)
            var oc0 = Sse.Shuffle(a, ic2, 0xC8); // a0 a2 b0 b3 => (a.xz, c2.xw) => (c0.x, c1.x, c2.x, 0)         
            var oc1 = Sse.Shuffle(a, ic2, 0xDD); // a1 a3 a1 a3 => (a.yw, c2.yw) => (c0.y, c1.y, c2.y, 0)            
            var oc2 = Sse.Shuffle(c, ic2, 0xE8); // a0 a2 b2 b3 => (c.xz, c2.zw) => (c0.z, c1.z, c2.z, 0)
            return (oc0.AsUInt32(), oc1.AsUInt32(), oc2.AsUInt32());
        }
        {
            var oc0 = Vector128.Create(c0.GetElement(0), c1.GetElement(0), c2.GetElement(0), 0);
            var oc1 = Vector128.Create(c0.GetElement(1), c1.GetElement(1), c2.GetElement(1), 0);
            var oc2 = Vector128.Create(c0.GetElement(2), c1.GetElement(2), c2.GetElement(2), 0);
            return (oc0, oc1, oc2);
        }
    }

    #endregion
    #region Vector256<long>

    [MethodImpl(256 | 512)]
    public static (Vector256<long> c0, Vector256<long> c1, Vector256<long> c2) Transpose3x3(
        Vector256<long> c0, Vector256<long> c1, Vector256<long> c2
    )
    {
        if (Avx.IsSupported)
        {
            var ic0 = c0.AsDouble();
            var ic1 = c1.AsDouble();
            var a = Avx.Shuffle(ic0, ic1, 0x0); // a0 b0 a2 b2  => (c0.x, c1.x, c0.z, c1.z)
            var c = Avx.Shuffle(ic0, ic1, 0xF); // a1 b1 a3 b3  => (c0.y, c1.y, 0, 0)
            var b = Vector256.Shuffle(c2, Vector256.Create(0, 3, 2, 3)).AsDouble(); // (c2.x, 0, c2.z, 0)
            var d = Vector256.Shuffle(c2, Vector256.Create(1, 3, 3, 3)).AsDouble(); // (c2.y, 0, 0, 0)
            var oc0 = Avx.Permute2x128(a, b, 0x20); // a01 b01 => (c0.x, c1.x, c2.x, 0)
            var oc1 = Avx.Permute2x128(c, d, 0x20); // a01 b01 => (c0.y, c1.y, c2.y, 0)
            var oc2 = Avx.Permute2x128(a, b, 0x31); // a23 b23 => (c0.z, c1.z, c2.z, 0)
            return (oc0.AsInt64(), oc1.AsInt64(), oc2.AsInt64());
        }
        {
            var oc0 = Vector256.Create(c0.GetElement(0), c1.GetElement(0), c2.GetElement(0), 0);
            var oc1 = Vector256.Create(c0.GetElement(1), c1.GetElement(1), c2.GetElement(1), 0);
            var oc2 = Vector256.Create(c0.GetElement(2), c1.GetElement(2), c2.GetElement(2), 0);
            return (oc0, oc1, oc2);
        }
    }

    #endregion
    #region Vector256<ulong>

    [MethodImpl(256 | 512)]
    public static (Vector256<ulong> c0, Vector256<ulong> c1, Vector256<ulong> c2) Transpose3x3(
        Vector256<ulong> c0, Vector256<ulong> c1, Vector256<ulong> c2
    )
    {
        if (Avx.IsSupported)
        {
            var ic0 = c0.AsDouble();
            var ic1 = c1.AsDouble();
            var a = Avx.Shuffle(ic0, ic1, 0x0); // a0 b0 a2 b2  => (c0.x, c1.x, c0.z, c1.z)
            var c = Avx.Shuffle(ic0, ic1, 0xF); // a1 b1 a3 b3  => (c0.y, c1.y, 0, 0)
            var b = Vector256.Shuffle(c2, Vector256.Create((ulong)0, 3, 2, 3)).AsDouble(); // (c2.x, 0, c2.z, 0)
            var d = Vector256.Shuffle(c2, Vector256.Create((ulong)1, 3, 3, 3)).AsDouble(); // (c2.y, 0, 0, 0)
            var oc0 = Avx.Permute2x128(a, b, 0x20); // a01 b01 => (c0.x, c1.x, c2.x, 0)
            var oc1 = Avx.Permute2x128(c, d, 0x20); // a01 b01 => (c0.y, c1.y, c2.y, 0)
            var oc2 = Avx.Permute2x128(a, b, 0x31); // a23 b23 => (c0.z, c1.z, c2.z, 0)
            return (oc0.AsUInt64(), oc1.AsUInt64(), oc2.AsUInt64());
        }
        {
            var oc0 = Vector256.Create(c0.GetElement(0), c1.GetElement(0), c2.GetElement(0), 0);
            var oc1 = Vector256.Create(c0.GetElement(1), c1.GetElement(1), c2.GetElement(1), 0);
            var oc2 = Vector256.Create(c0.GetElement(2), c1.GetElement(2), c2.GetElement(2), 0);
            return (oc0, oc1, oc2);
        }
    }

    #endregion
}
