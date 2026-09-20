namespace Coplt.Mathematics.Generics;

public interface IVectorArithmetic<Self, Scalar> :
    IVector<Self, Scalar>,
    IUnaryPlusOperators<Self, Self>,
    IAdditionOperators<Self, Self, Self>,
    ISubtractionOperators<Self, Self, Self>,
    IMultiplyOperators<Self, Self, Self>,
    IDivisionOperators<Self, Self, Self>,
    IModulusOperators<Self, Self, Self>
    where Self : unmanaged, IVectorArithmetic<Self, Scalar>
    where Scalar : unmanaged
{
    #region Sign

    public Self abs();
    public Self sign();

    #endregion

    #region Min Max Clamp

    public Self min(in Self other);
    public Self max(in Self other);

    public Self clamp(in Self min, in Self max);
    public Self clamp(Scalar min, Scalar max);

    #endregion

    #region Lerp Unlerp Remap

    public Self lerp(in Self start, in Self end);
    public Self lerp(Scalar start, Scalar end);
    public static abstract Self lerp(Scalar t, in Self start, in Self end);

    public Self unlerp(in Self start, in Self end);
    public Self unlerp(Scalar start, Scalar end);
    public static abstract Self unlerp(Scalar a, in Self start, in Self end);

    public Self remap(in Self src_start, in Self src_end, in Self dst_start, in Self dst_end);
    public Self remap(Scalar src_start, Scalar src_end, Scalar dst_start, Scalar dst_end);

    #endregion

    #region Dot LengthSq DistanceSq Square

    public Scalar dot(in Self other);

    public Scalar length_sq();
    public Scalar distance_sq(in Self to);

    public Self square();

    #endregion

    #region Fma

    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    public static abstract Self fma(in Self a, in Self b, in Self c);
    /// <summary>
    /// Fusion Subtraction and Multiplication
    /// <code>(a * b) - c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Subtrahend c</param>
    public static abstract Self fms(in Self a, in Self b, in Self c);
    /// <summary>
    /// Fusion Multiplication and Subtraction
    /// <code>c - (a * b)</code> or <code>-(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Minuend c</param>
    public static abstract Self fnma(in Self a, in Self b, in Self c);
    /// <summary>
    /// Fusion Multiplication and Subtraction
    /// <code>c - (a * b)</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Minuend c</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual Self fsm(in Self c, in Self a, in Self b) => Self.fnma(a, b, c);
    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>c + (a * b)</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual Self fam(in Self c, in Self a, in Self b) => Self.fma(a, b, c);
    /// <summary>
    /// Fusion Addition and Multiplication
    /// <code>(a * b) + c</code>
    /// </summary>
    /// <param name="a">Multiplier a</param>
    /// <param name="b">Multiplier b</param>
    /// <param name="c">Addend c</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual Self mad(in Self a, in Self b, in Self c) => Self.fma(a, b, c);

    #endregion

    #region CSum CMin CMax

    public Scalar csum();

    public Scalar cmin();
    public Scalar cmax();

    public Scalar cmin_safe();
    public Scalar cmax_safe();

    #endregion
}

public interface ISignedVectorArithmetic<Self, Scalar> :
    IVectorArithmetic<Self, Scalar>,
    IUnaryNegationOperators<Self, Self>
    where Self : unmanaged, ISignedVectorArithmetic<Self, Scalar>
    where Scalar : unmanaged;

public interface IVector3Arithmetic<Self, Scalar> :
    IVectorArithmetic<Self, Scalar>
    where Self : unmanaged, IVector3Arithmetic<Self, Scalar>
    where Scalar : unmanaged
{
    #region Cross

    public Self cross(in Self other);

    #endregion
}
