using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Coplt.Mathematics;

#region Soft

public struct Vec2<TScalar>
{
    #region Fields

    public TScalar x;
    public TScalar y;

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec2(TScalar x, TScalar y)
    {
        this.x = x;
        this.y = y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec2(TScalar broadcast) : this(broadcast, broadcast) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec2<TScalar>(TScalar broadcast) => new(broadcast);

    #endregion
}

public struct Vec3<TScalar>
{
    #region Fields

    public TScalar x;
    public TScalar y;
    public TScalar z;

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3(TScalar x, TScalar y, TScalar z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3(TScalar broadcast) : this(broadcast, broadcast, broadcast) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec3<TScalar>(TScalar broadcast) => new(broadcast);

    #endregion
}

public struct Vec4<TScalar>
{
    #region Fields

    public TScalar x;
    public TScalar y;
    public TScalar z;
    public TScalar w;

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec4(TScalar x, TScalar y, TScalar z, TScalar w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec4(TScalar broadcast) : this(broadcast, broadcast, broadcast, broadcast) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec4<TScalar>(TScalar broadcast) => new(broadcast);

    #endregion
}

#endregion

#region Simd

#region Vec32

public struct Vec32x2<TScalar>
    where TScalar : unmanaged
{
    #region Fields

    public Vector128<TScalar> simd;

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x2(Vector128<TScalar> simd)
    {
        this.simd = (simd.AsInt32() & Vector128.Create(-1, -1, 0, 0)).As<int, TScalar>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x2(TScalar x, TScalar y)
    {
        simd = Vector128.CreateScalar(x).WithElement(1, y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x2(TScalar broadcast) : this(Vector128.Create(broadcast)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec32x2<TScalar>(TScalar broadcast) => new(broadcast);

    #endregion
}

public struct Vec32x3<TScalar>
    where TScalar : unmanaged
{
    #region Fields

    public Vector128<TScalar> simd;

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x3(Vector128<TScalar> simd)
    {
        this.simd = (simd.AsInt32() & Vector128.Create(-1, -1, -1, 0)).As<int, TScalar>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x3(TScalar x, TScalar y, TScalar z)
    {
        simd = Vector128.CreateScalar(x).WithElement(1, y).WithElement(2, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x3(TScalar broadcast) : this(Vector128.Create(broadcast)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec32x3<TScalar>(TScalar broadcast) => new(broadcast);

    #endregion
}

public struct Vec32x4<TScalar>
    where TScalar : unmanaged
{
    #region Fields

    public Vector128<TScalar> simd;

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x4(Vector128<TScalar> simd)
    {
        this.simd = simd;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x4(TScalar x, TScalar y, TScalar z, TScalar w)
    {
        simd = Vector128.CreateScalar(x).WithElement(1, y).WithElement(2, z).WithElement(3, w);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec32x4(TScalar broadcast) : this(Vector128.Create(broadcast)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec32x4<TScalar>(TScalar broadcast) => new(broadcast);

    #endregion
}

#endregion

#region Vec64

public struct Vec64x2<TScalar>
    where TScalar : unmanaged
{
    #region Fields

    public Vector128<TScalar> simd;

    #endregion
}

public struct Vec64x3<TScalar>
    where TScalar : unmanaged
{
    #region Fields

    public Vector256<TScalar> simd;

    #endregion
}

public struct Vec64x4<TScalar>
    where TScalar : unmanaged
{
    #region Fields

    public Vector256<TScalar> simd;

    #endregion
}

#endregion

#endregion
