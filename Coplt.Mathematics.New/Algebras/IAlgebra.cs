namespace Coplt.Mathematics.Algebras;

#region Core

/// <summary>
/// A value of the algebra library: it has a size, it says whether the simd of the platform backs it, and it is
/// comparable, bitwise reachable and formattable
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface IAlgebra<TSelf> :
    IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>,
    IBitwiseOperators<TSelf, TSelf, TSelf>,
    ISpanFormattable, IUtf8SpanFormattable
    where TSelf : unmanaged, IAlgebra<TSelf>
{
    #region Meta

    /// <summary>True when the simd of the platform backs the value</summary>
    public static abstract bool IsSimdAccelerated { get; }

    /// <summary>The size of a value in bytes</summary>
    public static abstract int SizeByte { get; }

    /// <summary>The size of a value in bits</summary>
    public static abstract int SizeBit { get; }

    #endregion
}

/// <summary>
/// An <see cref="IAlgebra{TSelf}"/> that also names the type of a single component, which reaches the
/// construction of the value and the component of it at an index
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IAlgebra<TSelf, TScalar> : IAlgebra<TSelf>
    where TSelf : unmanaged, IAlgebra<TSelf, TScalar>
    where TScalar : unmanaged
{
    #region Ctor

    /// <summary>
    /// Creates a value with every component set to <paramref name="scalar"/>
    /// <para>It builds the whole register of the value and masks the padding lanes of it, so it does not need
    /// the platform to leave the lanes that follow the one of the register of a scalar at zero</para>
    /// </summary>
    public static abstract TSelf Broadcast(TScalar scalar);

    /// <summary>
    /// Creates a value with every component set to <paramref name="scalar"/>, without building the whole
    /// register of it
    /// <para>It shuffles the register of the scalar into the value, which only leaves the padding lanes of it
    /// at zero on a platform whose hardware zeroes the lanes that follow the one of the register of a scalar,
    /// so it is the one the code that knows the platform of uses instead of <see cref="Broadcast"/></para>
    /// </summary>
    public static abstract TSelf BroadcastUnsafe(TScalar scalar);

    /// <summary>
    /// Creates a value with only the first component set to <paramref name="scalar"/>
    /// <para>It leaves the lanes that follow the one of the register of the scalar as they are, which only
    /// leaves the padding lanes of it at zero on a platform whose hardware zeroes the lanes that follow the one
    /// of the register of a scalar, so it is the one the code that knows the platform of uses instead of
    /// <see cref="Scalar"/></para>
    /// </summary>
    public static abstract TSelf ScalarUnsafe(TScalar scalar);

    /// <summary>Creates a value whose first component is <paramref name="scalar"/> and whose other components
    /// are zero</summary>
    public static abstract TSelf Scalar(TScalar scalar);

    /// <summary>Creates a value from the components of a span</summary>
    /// <param name="span">The span of the components</param>
    public static abstract TSelf Load(ReadOnlySpan<TScalar> span);

    /// <summary>Creates a value from the components at a pointer</summary>
    /// <param name="ptr">The pointer to the components</param>
    public static abstract unsafe TSelf Load(TScalar* ptr);

    #endregion

    #region Index

    /// <summary>Returns the component of <paramref name="self"/> at <paramref name="index"/></summary>
    public static abstract TScalar get(in TSelf self, int index);

    /// <summary>Sets the component of <paramref name="self"/> at <paramref name="index"/> to
    /// <paramref name="value"/></summary>
    public static abstract void set(ref TSelf self, int index, TScalar value);

    #endregion
}

#endregion

#region Bool

/// <summary>
/// An <see cref="IAlgebra{TSelf}"/> of a mask: every component of it is a bit that says whether a condition
/// holds
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface IBoolAlgebra<TSelf> : IAlgebra<TSelf>
    where TSelf : unmanaged, IBoolAlgebra<TSelf>
{
    #region Constants

    /// <summary>A value whose every component is true</summary>
    public static abstract TSelf True { get; }

    /// <summary>A value whose every component is false</summary>
    public static abstract TSelf False { get; }

    #endregion
}

/// <summary>
/// An <see cref="IBoolAlgebra{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IBoolAlgebra<TSelf, TScalar> :
    IBoolAlgebra<TSelf>,
    IAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IBoolAlgebra<TSelf, TScalar>
    where TScalar : unmanaged;

#endregion

#region Number

/// <summary>
/// An <see cref="IAlgebra{TSelf}"/> of a number: it is ordered, it shifts, and it adds, subtracts, multiplies,
/// divides and takes the remainder of two values
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface INumberAlgebra<TSelf> : IAlgebra<TSelf>,
    IComparable<TSelf>, IComparable,
    IComparisonOperators<TSelf, TSelf, bool>,
    IShiftOperators<TSelf, int, TSelf>,
    IUnaryPlusOperators<TSelf, TSelf>,
    IAdditionOperators<TSelf, TSelf, TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IModulusOperators<TSelf, TSelf, TSelf>
    where TSelf : unmanaged, INumberAlgebra<TSelf>
{
    #region Constants

    /// <summary>A value whose every component is zero</summary>
    public static abstract TSelf Zero { get; }

    /// <summary>A value whose every component is one</summary>
    public static abstract TSelf One { get; }

    /// <summary>A value whose every component is two</summary>
    public static abstract TSelf Two { get; }

    #endregion
}

/// <summary>
/// An <see cref="INumberAlgebra{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface INumberAlgebra<TSelf, TScalar> :
    INumberAlgebra<TSelf>,
    IAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, INumberAlgebra<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryNumber<TScalar>
{
    #region Constants

    /// <summary>The zero of a single component</summary>
    public static abstract TScalar ScalarZero { get; }

    /// <summary>The one of a single component</summary>
    public static abstract TScalar ScalarOne { get; }

    /// <summary>The two of a single component</summary>
    public static abstract TScalar ScalarTwo { get; }

    #endregion
}

#endregion

#region Signed

/// <summary>
/// An <see cref="INumberAlgebra{TSelf}"/> of a number that has a sign, so the value of it can be negated
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface ISignedAlgebra<TSelf> :
    INumberAlgebra<TSelf>,
    IUnaryNegationOperators<TSelf, TSelf>
    where TSelf : unmanaged, ISignedAlgebra<TSelf>;

/// <summary>
/// An <see cref="ISignedAlgebra{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface ISignedAlgebra<TSelf, TScalar> :
    INumberAlgebra<TSelf, TScalar>,
    ISignedAlgebra<TSelf>
    where TSelf : unmanaged, ISignedAlgebra<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryNumber<TScalar>, ISignedNumber<TScalar>;

#endregion

#region FloatingPoint

/// <summary>
/// An <see cref="ISignedAlgebra{TSelf}"/> of a floating point number: it reaches the math constants of the
/// kind of it
/// </summary>
public interface IFloatingPointAlgebra<TSelf> :
    ISignedAlgebra<TSelf>
    where TSelf : unmanaged, IFloatingPointAlgebra<TSelf>
{
    #region Math Constants

    /// <summary>
    /// <code>e</code>, the base of the natural logarithm
    /// </summary>
    public static abstract TSelf E { get; }
    /// <summary>
    /// <code>log(2)</code>, the natural logarithm of two
    /// </summary>
    public static abstract TSelf Log2 { get; }
    /// <summary>
    /// <code>log(10)</code>, the natural logarithm of ten
    /// </summary>
    public static abstract TSelf Log10 { get; }
    /// <summary>
    /// <code>π</code>, the ratio of the circumference of a circle to its diameter
    /// </summary>
    public static abstract TSelf PI { get; }
    /// <summary>
    /// <code>τ = 2 * π</code>, the ratio of the circumference of a circle to its radius
    /// </summary>
    public static abstract TSelf Tau { get; }
    /// <summary>
    /// <code>360 / τ</code>, the factor that turns a radian into a degree
    /// </summary>
    public static abstract TSelf RadToDeg { get; }
    /// <summary>
    /// <code>τ / 360</code>, the factor that turns a degree into a radian
    /// </summary>
    public static abstract TSelf DegToRad { get; }

    #endregion
}

/// <summary>
/// An <see cref="IFloatingPointAlgebra{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IFloatingPointAlgebra<TSelf, TScalar> :
    IFloatingPointAlgebra<TSelf>,
    ISignedAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IFloatingPointAlgebra<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryNumber<TScalar>, IFloatingPoint<TScalar>
{
    #region Math Constants

    /// <summary>
    /// <code>e</code>, the base of the natural logarithm
    /// </summary>
    public static abstract TScalar ScalarE { get; }
    /// <summary>
    /// <code>log(2)</code>, the natural logarithm of two
    /// </summary>
    public static abstract TScalar ScalarLog2 { get; }
    /// <summary>
    /// <code>log(10)</code>, the natural logarithm of ten
    /// </summary>
    public static abstract TScalar ScalarLog10 { get; }
    /// <summary>
    /// <code>π</code>, the ratio of the circumference of a circle to its diameter
    /// </summary>
    public static abstract TScalar ScalarPI { get; }
    /// <summary>
    /// <code>τ = 2 * π</code>, the ratio of the circumference of a circle to its radius
    /// </summary>
    public static abstract TScalar ScalarTau { get; }
    /// <summary>
    /// <code>360 / τ</code>, the factor that turns a radian into a degree
    /// </summary>
    public static abstract TScalar ScalarRadToDeg { get; }
    /// <summary>
    /// <code>τ / 360</code>, the factor that turns a degree into a radian
    /// </summary>
    public static abstract TScalar ScalarDegToRad { get; }

    #endregion
}

#endregion

#region FloatingPoint Ieee

/// <summary>
/// An <see cref="IFloatingPointAlgebra{TSelf}"/> of the values the ieee 754 standard names: the smallest
/// positive value, the value that is not a number and the ones of the infinities
/// </summary>
public interface IFloatingPointIeee754Algebra<TSelf> :
    IFloatingPointAlgebra<TSelf>
    where TSelf : unmanaged, IFloatingPointIeee754Algebra<TSelf>
{
    #region Math Constants

    /// <summary>
    /// <code>Epsilon</code>, the smallest positive value of the component type of the value
    /// </summary>
    public static abstract TSelf Epsilon { get; }

    /// <summary>
    /// <code>NaN</code>, the value that is not a number
    /// </summary>
    public static abstract TSelf NaN { get; }

    /// <summary>
    /// <code>-Infinity</code>, the value below every number
    /// </summary>
    public static abstract TSelf NegativeInfinity { get; }

    /// <summary>
    /// <code>-0</code>, the zero of the negative sign
    /// </summary>
    public static abstract TSelf NegativeZero { get; }

    /// <summary>
    /// <code>Infinity</code>, the value above every number
    /// </summary>
    public static abstract TSelf PositiveInfinity { get; }

    #endregion
}

/// <summary>
/// An <see cref="IFloatingPointIeee754Algebra{TSelf}"/> that also names the type of a single component
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IFloatingPointIeee754Algebra<TSelf, TScalar> :
    IFloatingPointIeee754Algebra<TSelf>,
    IFloatingPointAlgebra<TSelf, TScalar>
    where TSelf : unmanaged, IFloatingPointIeee754Algebra<TSelf, TScalar>
    where TScalar : unmanaged, IBinaryNumber<TScalar>, IFloatingPointIeee754<TScalar>
{
    #region Math Constants

    /// <summary>
    /// <code>Epsilon</code>, the smallest positive value of a single component
    /// </summary>
    public static abstract TScalar ScalarEpsilon { get; }

    /// <summary>
    /// <code>NaN</code>, the value that is not a number
    /// </summary>
    public static abstract TScalar ScalarNaN { get; }

    /// <summary>
    /// <code>-Infinity</code>, the value below every number
    /// </summary>
    public static abstract TScalar ScalarNegativeInfinity { get; }

    /// <summary>
    /// <code>-0</code>, the zero of the negative sign
    /// </summary>
    public static abstract TScalar ScalarNegativeZero { get; }

    /// <summary>
    /// <code>Infinity</code>, the value above every number
    /// </summary>
    public static abstract TScalar ScalarPositiveInfinity { get; }

    #endregion
}

#endregion
