using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// A shuffle combines two vectors of the same type and the members of the math class forward the call to the
// member of the vector itself. The pattern of a member is a part of its name and the vector is a type parameter
// of the member, so the member of the type can devirtualize and inline and the pattern stays a constant of the
// call site. The type parameter of a call is inferred from its arguments, math.shuffle_xy_zw(a, b).
public static partial class math
{
    /// <summary>
    /// Returns the shuffle of the pattern <paramref name="lh"/>
    /// <para>The pattern of every other member is a constant of the call site, the one of this member is only
    /// known at run time, so it cannot be compiled to a single instruction of the simd register</para>
    /// </summary>
    /// <typeparam name="T">The type of the vectors to shuffle</typeparam>
    /// <param name="a">The vector the low half of the result takes its components from</param>
    /// <param name="b">The vector the high half of the result takes its components from</param>
    /// <param name="lh">The pattern of the shuffle, it is the name of the member that shuffles it</param>
    /// <returns>The vector that the pattern <paramref name="lh"/> names</returns>
    [MethodImpl(256)]
    public static T shuffle<T>(in T a, in T b, Shuffle42 lh) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle(a, b, lh);

    [MethodImpl(256)]
    public static T shuffle_xx_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xx_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xx_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xy_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xy_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xz_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xz_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_xw_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_xw_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yx_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yx_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yy_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yy_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yz_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yz_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_yw_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_yw_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zx_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zx_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zy_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zy_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zz_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zz_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_zw_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_zw_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wx_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wx_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wy_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wy_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_wz_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_wz_ww(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_xx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_xx(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_xy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_xy(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_xz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_xz(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_xw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_xw(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_yx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_yx(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_yy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_yy(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_yz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_yz(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_yw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_yw(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_zx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_zx(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_zy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_zy(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_zz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_zz(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_zw<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_zw(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_wx<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_wx(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_wy<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_wy(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_wz<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_wz(a, b);

    [MethodImpl(256)]
    public static T shuffle_ww_ww<T>(in T a, in T b) where T : unmanaged, IVectorShuffle<T>
        => T.shuffle_ww_ww(a, b);
}
