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
    public static abstract TSelf shuffle(in TSelf a, in TSelf b, Shuffle42 lh);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xx_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xx_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xx_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xx_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xy_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xy_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xy_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xy_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xz_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xz_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xz_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xz_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_xw_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_xw_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_xw_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.x, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_xw_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yx_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yx_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yx_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yx_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yy_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yy_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yy_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yy_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yz_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yz_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yz_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yz_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_yw_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_yw_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_yw_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.y, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_yw_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zx_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zx_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zx_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zx_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zy_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zy_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zy_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zy_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zz_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zz_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zz_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zz_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_zw_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_zw_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_zw_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.z, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_zw_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_wx_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_wx_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_wx_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.x, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_wx_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_wy_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_wy_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_wy_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.y, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_wy_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_wz_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_wz_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_wz_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.z, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_wz_ww(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_xx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_xy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_xz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.x, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_xw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_yx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_yy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_yz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.y, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_yw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_zx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_zy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_zz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.z, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_zw(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.x)</c></summary>
    public static abstract TSelf shuffle_ww_wx(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.y)</c></summary>
    public static abstract TSelf shuffle_ww_wy(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.z)</c></summary>
    public static abstract TSelf shuffle_ww_wz(in TSelf a, in TSelf b);

    /// <summary>Returns <c>(a.w, a.w, b.w, b.w)</c></summary>
    public static abstract TSelf shuffle_ww_ww(in TSelf a, in TSelf b);
}
