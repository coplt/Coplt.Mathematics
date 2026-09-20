namespace Coplt.Mathematics.Generics;

public interface IVectorFloatingPointIeee754BoolOps<Self, Scalar, out BoolVector> :
    IVectorFloatingPointIeee754<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPointIeee754BoolOps<Self, Scalar, BoolVector>
    where Scalar : unmanaged
{
    #region IsNaN IsFinite IsInfinity IsPositiveInfinity

    public BoolVector is_NaN();
    public BoolVector is_finite();

    public BoolVector is_inf();
    public BoolVector is_pos_inf();
    public BoolVector is_neg_inf();

    #endregion
}

public interface IVectorFloatingPointIeee754<Self, Scalar> :
    IVectorFloatingPoint<Self, Scalar>
    where Self : unmanaged, IVectorFloatingPointIeee754<Self, Scalar>
    where Scalar : unmanaged
{
    #region Log

    public Self log();
    public Self log2();
    public Self log(in Self other);
    public Self log10();

    #endregion

    #region Exp

    public Self exp();
    public Self exp2();
    public Self exp10();

    #endregion

    #region Pow Sqrt RSqrt

    public Self pow(Scalar v);

    public Self sqrt(Scalar v);

    public Self rsqrt(Scalar v);

    #endregion

    #region Length Distance

    public Scalar length();
    public Scalar distance(in Self to);

    #endregion

    #region Normalize

    public Self normalize();

    public Self normalize_safe();

    #endregion

    #region Step Refract

    public Self step(in Self threshold);

    public static abstract Self refract(in Self i, in Self n, Scalar index_of_refraction);

    #endregion

    #region ProjectSafe

    public Self project_safe(in Self onto);
    public Self project_safe(in Self onto, in Self default_value);

    #endregion

    #region FaceForward

    public Self face_forward(in Self i, in Self ng);

    #endregion

    #region Sin Cos Tan

    public Self sin();
    public Self cos();

    public (Self sin, Self cos) sincos();
    public void sincos(out Self sin, out Self cos);

    public Self tan();

    #endregion

    #region ASin ACos ATan ATan2

    public Self asin();
    public Self acos();
    public Self atan();

    public Self atan2(in Self v);

    #endregion

    #region SinH CosH TanH

    public Self sinh();
    public Self cosh();
    public Self tanh();

    #endregion

    #region ASinH ACosH ATanH

    public Self asinh();
    public Self acosh();
    public Self atanh();

    #endregion

    #region ChgSign

    public Self chg_sign(in Self sign);

    #endregion
}
