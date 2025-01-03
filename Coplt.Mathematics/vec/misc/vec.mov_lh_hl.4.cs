// generated by template, do not modify manually

namespace Coplt.Mathematics;

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static float4 movelh([This] float4 a, float4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static float4 movehl([This] float4 a, float4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static double4 movelh([This] double4 a, double4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static double4 movehl([This] double4 a, double4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static short4 movelh([This] short4 a, short4 b)
    {
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static short4 movehl([This] short4 a, short4 b)
    {
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static ushort4 movelh([This] ushort4 a, ushort4 b)
    {
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static ushort4 movehl([This] ushort4 a, ushort4 b)
    {
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static int4 movelh([This] int4 a, int4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static int4 movehl([This] int4 a, int4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static uint4 movelh([This] uint4 a, uint4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static uint4 movehl([This] uint4 a, uint4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static long4 movelh([This] long4 a, long4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static long4 movehl([This] long4 a, long4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static ulong4 movelh([This] ulong4 a, ulong4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static ulong4 movehl([This] ulong4 a, ulong4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static decimal4 movelh([This] decimal4 a, decimal4 b)
    {
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static decimal4 movehl([This] decimal4 a, decimal4 b)
    {
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static half4 movelh([This] half4 a, half4 b)
    {
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static half4 movehl([This] half4 a, half4 b)
    {
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static b16v4 movelh([This] b16v4 a, b16v4 b)
    {
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static b16v4 movehl([This] b16v4 a, b16v4 b)
    {
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static b32v4 movelh([This] b32v4 a, b32v4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static b32v4 movehl([This] b32v4 a, b32v4 b)
    {
        if (Vector128.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}

[Ex]
public static partial class math
{
    /// <summary>
    /// Move the low part of b to the high part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (x0, y0, x1, y1) (l0, l1)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static b64v4 movelh([This] b64v4 a, b64v4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveLowToHigh(a.vector, b.vector));
        return new(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Move the high part of b to the low part of a
    /// <code>
    /// a (x0, y0, z0, w0) (l0, h0)
    /// b (x1, y1, z1, w1) (l1, h1)
    /// r (z1, w1, z0, w0) (h1, h0)
    /// </code>
    /// </summary>
    [MethodImpl(256 | 512)]
    public static b64v4 movehl([This] b64v4 a, b64v4 b)
    {
        if (Vector256.IsHardwareAccelerated)
            return new(simd.MoveHighToLow(a.vector, b.vector));
        return new(b.z, b.w, a.z, a.w);
    }
}
