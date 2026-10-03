namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of 4 components that can be combined with another vector of the same type
/// <para>Every member returns the vector whose low half is taken from <c>a</c> and whose high half is taken
/// from <c>b</c>. The four digits of the name of a member are the indices of the components of the source of
/// each half, the first two name the components of <c>a</c> and the last two the ones of <c>b</c>, so
/// <c>shuffle_yy_zw</c> returns <c>(a.y, a.y, b.z, b.w)</c></para>
/// <para>Every member is static because it combines two vectors, and it is a member of its own because the
/// pattern of a shuffle is a part of the call site: the pattern is a constant of the generated member, so an
/// accelerated vector shuffles its two registers with a single instruction. The sources are passed by readonly
/// reference, a shuffle never changes them and the members of this interface do not have to copy a vector
/// twice at every call</para>
/// <para>The <c>shuffle</c> member is the one member whose pattern is a value instead of a part of its name,
/// it is the entry point of the code that picks the pattern at run time</para>
/// </summary>
/// <typeparam name="TSelf">The type of the vector itself</typeparam>
public interface IVectorShuffle<TSelf>
    where TSelf : unmanaged, IVectorShuffle<TSelf>
{
    /// <summary>
    /// Returns the shuffle of the pattern <paramref name="lh"/>
    /// <para>The pattern of every other member is a constant of the call site, the one of this member is only
    /// known at run time, so it cannot be compiled to a single instruction of the simd register</para>
    /// </summary>
    /// <param name="a">The vector the low half of the result takes its components from</param>
    /// <param name="b">The vector the high half of the result takes its components from</param>
    /// <param name="lh">The pattern of the shuffle, it is the name of the member that shuffles it</param>
    /// <returns>The vector that the pattern <paramref name="lh"/> names</returns>
    public static abstract TSelf shuffle(TSelf a, TSelf b, Shuffle42 lh);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_ww(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_xx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_xy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_xz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_xw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_yx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_yy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_yz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_yw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_zx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_zy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_zz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_zw(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_wx(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_wy(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_wz(TSelf a, TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_ww(TSelf a, TSelf b);
}
