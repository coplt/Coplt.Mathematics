using System.Numerics;
using Coplt.Mathematics.Generics;

namespace Tests.Arith;

/// <summary>
/// The arithmetic checks that only use the members declared on the <c>IVectorArithmetic</c> family of
/// interfaces, so every generated number vector is checked through the same code. The accelerated simd path and
/// the scalar fallback are not selected here, both have to pass the checks below, the fallback is covered by the
/// element types that are not simd backed (half, short, ushort).
/// </summary>
internal static class ArithCheck
{
    #region helpers

    /// <summary>
    /// Builds a vector from the components of <paramref name="values"/>, a shorter vector only uses the leading
    /// ones. A simd backed 3 component vector reads a whole simd register, so its padding lane is not part of the
    /// vector and stays zero.
    /// </summary>
    private static T Vec<T, TScalar>(int length, bool simd, params TScalar[] values)
        where T : unmanaged, IVector<T, TScalar>
        where TScalar : unmanaged
    {
        var data = new TScalar[simd && length == 3 ? 4 : length];
        for (var i = 0; i < values.Length && i < data.Length; i++) data[i] = values[i];
        return T.Load(data);
    }

    /// <summary>
    /// Checks every component of the vector, the padding lane of a 3 component vector is not part of the vector.
    /// </summary>
    private static void AllEqual<T, TScalar>(T actual, T expected, string what)
        where T : unmanaged, IVector<T, TScalar>
        where TScalar : unmanaged
    {
        for (var i = 0; i < T.Length; i++)
            Assert.That(actual[i], Is.EqualTo(expected[i]), $"{what}, component {i}");
    }

    private static void ScalarEqual<TScalar>(TScalar actual, TScalar expected, string what)
        => Assert.That(actual, Is.EqualTo(expected), what);

    #endregion

    #region IVectorArithmetic

    /// <summary>
    /// Everything that is declared on <see cref="IVectorArithmetic{Self,Scalar}"/>. Every value is a small
    /// integer, so no result is rounded and the check works for the floating point and the integer types alike.
    /// </summary>
    public static void Arithmetic<T, TScalar>(int length, bool simd)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged, INumber<TScalar>
    {
        var zero = TScalar.Zero;
        var one = TScalar.One;
        var two = TScalar.CreateChecked(2);
        var three = TScalar.CreateChecked(3);
        var four = TScalar.CreateChecked(4);
        var five = TScalar.CreateChecked(5);

        var vZero = T.Zero;
        var allOne = T.One;
        var allTwo = T.Two;
        var allFive = T.Broadcast(five);
        // ascending, so every component is smaller than the matching component of allFour and allFive
        var asc = Vec<T, TScalar>(length, simd, one, two, three, four);

        using (Assert.EnterMultipleScope())
        {
            #region operators

            AllEqual<T, TScalar>(+asc, asc, "unary +");

            AllEqual<T, TScalar>(asc + vZero, asc, "x + 0");
            AllEqual<T, TScalar>(asc + allOne, Vec<T, TScalar>(length, simd, one + one, two + one, three + one, four + one), "x + 1");
            AllEqual<T, TScalar>(asc - vZero, asc, "x - 0");
            AllEqual<T, TScalar>(asc - asc, vZero, "x - x");
            AllEqual<T, TScalar>(asc - allOne, Vec<T, TScalar>(length, simd, one - one, two - one, three - one, four - one), "x - 1");

            AllEqual<T, TScalar>(asc * T.One, asc, "x * 1");
            AllEqual<T, TScalar>(asc * vZero, vZero, "x * 0");
            AllEqual<T, TScalar>(asc * allTwo, Vec<T, TScalar>(length, simd, one * two, two * two, three * two, four * two), "x * 2");
            AllEqual<T, TScalar>((asc * allTwo) / allTwo, asc, "(x * 2) / 2");

            AllEqual<T, TScalar>(asc / allOne, asc, "x / 1");
            AllEqual<T, TScalar>(asc / allTwo, Vec<T, TScalar>(length, simd, one / two, two / two, three / two, four / two), "x / 2");

            AllEqual<T, TScalar>(asc % allOne, vZero, "x % 1");
            AllEqual<T, TScalar>(asc % allTwo, Vec<T, TScalar>(length, simd, one % two, two % two, three % two, four % two), "x % 2");

            #endregion

            #region sign

            AllEqual<T, TScalar>(asc.abs(), asc, "abs of a positive vector");
            AllEqual<T, TScalar>(asc.abs().abs(), asc, "abs is idempotent");
            AllEqual<T, TScalar>(vZero.abs(), vZero, "abs of zero");
            // abs(x) is x scaled by the sign of x, for the unsigned types the sign is a mask of zero and one
            AllEqual<T, TScalar>(asc * asc.sign(), asc.abs(), "x * sign(x)");
            AllEqual<T, TScalar>(vZero.sign(), vZero, "sign of zero");
            AllEqual<T, TScalar>(allOne.sign(), allOne, "sign of one");

            #endregion

            #region min max clamp

            AllEqual<T, TScalar>(asc.min(allFive), asc, "min with a larger vector");
            AllEqual<T, TScalar>(allFive.min(asc), asc, "min is commutative");
            AllEqual<T, TScalar>(asc.min(asc), asc, "min with itself");
            AllEqual<T, TScalar>(asc.max(allFive), allFive, "max with a larger vector");
            AllEqual<T, TScalar>(allFive.max(asc), allFive, "max is commutative");
            AllEqual<T, TScalar>(asc.max(asc), asc, "max with itself");

            AllEqual<T, TScalar>(asc.clamp(allOne, allFive), asc, "clamp keeps a value in the range");
            AllEqual<T, TScalar>(allFive.clamp(allOne, allTwo), allTwo, "clamp to the upper bound");
            AllEqual<T, TScalar>(vZero.clamp(allOne, allTwo), allOne, "clamp to the lower bound");
            AllEqual<T, TScalar>(asc.clamp(one, five), asc, "scalar clamp keeps a value in the range");
            AllEqual<T, TScalar>(allFive.clamp(one, two), allTwo, "scalar clamp to the upper bound");
            AllEqual<T, TScalar>(vZero.clamp(one, two), allOne, "scalar clamp to the lower bound");

            #endregion

            #region lerp unlerp remap

            AllEqual<T, TScalar>(vZero.lerp(asc, allFive), asc, "lerp at t = 0");
            AllEqual<T, TScalar>(allOne.lerp(asc, allFive), allFive, "lerp at t = 1");
            AllEqual<T, TScalar>(asc.lerp(allOne, allFive),
                Vec<T, TScalar>(length, simd, one + one * four, one + two * four, one + three * four, one + four * four),
                "lerp with the vector as t");

            AllEqual<T, TScalar>(T.lerp(zero, asc, allFive), asc, "static lerp at t = 0");
            AllEqual<T, TScalar>(T.lerp(one, asc, allFive), allFive, "static lerp at t = 1");

            AllEqual<T, TScalar>(allOne.lerp(one, five), allFive, "scalar lerp at t = 1");
            AllEqual<T, TScalar>(allOne.unlerp(one, five), vZero, "scalar unlerp at the lower bound");
            AllEqual<T, TScalar>(allFive.unlerp(one, five), allOne, "scalar unlerp at the upper bound");

            AllEqual<T, TScalar>(asc.unlerp(asc, allFive), vZero, "unlerp at the lower bound");
            AllEqual<T, TScalar>(allFive.unlerp(asc, allFive), allOne, "unlerp at the upper bound");
            // the static unlerp places a scalar between the two vectors
            AllEqual<T, TScalar>(T.unlerp(one, allOne, allFive), vZero, "static unlerp at the lower bound");
            AllEqual<T, TScalar>(T.unlerp(five, allOne, allFive), allOne, "static unlerp at the upper bound");
            // unlerp is the inverse of lerp
            AllEqual<T, TScalar>(asc.unlerp(asc, allFive).lerp(asc, allFive), asc, "unlerp is the inverse of lerp");

            AllEqual<T, TScalar>(asc.remap(asc, allFive, allOne, allTwo), allOne, "remap the lower bound");
            AllEqual<T, TScalar>(allFive.remap(asc, allFive, allOne, allTwo), allTwo, "remap the upper bound");
            AllEqual<T, TScalar>(asc.remap(asc, allFive, asc, allFive), asc, "remap onto the same range");
            AllEqual<T, TScalar>(allOne.remap(one, five, one, five), allOne, "scalar remap the lower bound");
            AllEqual<T, TScalar>(allFive.remap(one, five, one, five), allFive, "scalar remap the upper bound");

            #endregion

            #region dot length_sq distance_sq square

            var dotAscFive = zero;
            var lengthSq = zero;
            var distanceSq = zero;
            var sum = zero;
            for (var i = 0; i < length; i++)
            {
                dotAscFive += asc[i] * allFive[i];
                lengthSq += asc[i] * asc[i];
                distanceSq += (five - asc[i]) * (five - asc[i]);
                sum += asc[i];
            }

            ScalarEqual(asc.dot(allFive), dotAscFive, "dot");
            ScalarEqual(asc.dot(asc), lengthSq, "dot with itself");
            ScalarEqual(asc.length_sq(), lengthSq, "length_sq");
            ScalarEqual(asc.distance_sq(allFive), distanceSq, "distance_sq");
            ScalarEqual(asc.distance_sq(asc), zero, "distance_sq with itself");
            ScalarEqual(asc.square().dot(allOne), lengthSq, "square");

            AllEqual<T, TScalar>(asc.square(), asc * asc, "square is x * x");

            #endregion

            #region fma

            AllEqual<T, TScalar>(T.fma(asc, allTwo, allOne), asc * allTwo + allOne, "fma");
            AllEqual<T, TScalar>(T.fms(asc, allTwo, allOne), asc * allTwo - allOne, "fms");
            AllEqual<T, TScalar>(T.fnma(asc, allTwo, allOne), allOne - asc * allTwo, "fnma");
            AllEqual<T, TScalar>(T.fsm(allOne, asc, allTwo), T.fnma(asc, allTwo, allOne), "fsm");
            AllEqual<T, TScalar>(T.fam(allOne, asc, allTwo), T.fma(asc, allTwo, allOne), "fam");
            AllEqual<T, TScalar>(T.mad(asc, allTwo, allOne), T.fma(asc, allTwo, allOne), "mad");

            #endregion

            #region csum cmin cmax

            ScalarEqual(asc.csum(), sum, "csum");
            ScalarEqual(vZero.csum(), zero, "csum of zero");
            ScalarEqual(allOne.csum(), TScalar.CreateChecked(length), "csum of a broadcast vector");

            ScalarEqual(asc.cmin(), one, "cmin");
            ScalarEqual(asc.cmax(), TScalar.CreateChecked(length), "cmax");
            ScalarEqual(asc.cmin_safe(), one, "cmin_safe");
            ScalarEqual(asc.cmax_safe(), TScalar.CreateChecked(length), "cmax_safe");
            ScalarEqual(allFive.cmin(), five, "cmin of a broadcast vector");
            ScalarEqual(allFive.cmax(), five, "cmax of a broadcast vector");
            ScalarEqual(allFive.cmin_safe(), five, "cmin_safe of a broadcast vector");
            ScalarEqual(allFive.cmax_safe(), five, "cmax_safe of a broadcast vector");

            #endregion
        }
    }

    #endregion

    #region ISignedVectorArithmetic

    /// <summary>
    /// Everything that is added by <see cref="ISignedVectorArithmetic{Self,Scalar}"/>.
    /// </summary>
    public static void Negation<T, TScalar>(int length, bool simd)
        where T : unmanaged, ISignedVectorArithmetic<T, TScalar>
        where TScalar : unmanaged, ISignedNumber<TScalar>
    {
        var negOne = -TScalar.One;
        var negAll = T.Broadcast(negOne);
        var allOne = T.One;
        var vZero = T.Zero;
        var negLength = -TScalar.CreateChecked(length);

        using (Assert.EnterMultipleScope())
        {
            AllEqual<T, TScalar>(-allOne, negAll, "negation");
            AllEqual<T, TScalar>(-negAll, allOne, "double negation");
            AllEqual<T, TScalar>(allOne + negAll, vZero, "x + -x");
            AllEqual<T, TScalar>(negAll.abs(), allOne, "abs of the negatives");
            AllEqual<T, TScalar>(negAll.abs(), negAll * negAll.sign(), "abs is x scaled by the sign");
            AllEqual<T, TScalar>(negAll.sign(), negAll, "sign of the negatives");
            AllEqual<T, TScalar>(negAll.square(), allOne, "square of the negatives");

            ScalarEqual(negAll.csum(), negLength, "csum of the negatives");
            // the padding lane of a 3 component vector is zero, a reduction that picks it up would return zero
            // instead of the negative components, the fast and the safe variants are both checked by these
            ScalarEqual(negAll.cmin(), negOne, "cmin of the negatives");
            ScalarEqual(negAll.cmax(), negOne, "cmax of the negatives");
            ScalarEqual(negAll.cmin_safe(), negOne, "cmin_safe of the negatives");
            ScalarEqual(negAll.cmax_safe(), negOne, "cmax_safe of the negatives");
        }
    }

    #endregion

    #region IVector3Arithmetic

    /// <summary>
    /// Everything that is added by <see cref="IVector3Arithmetic{Self,Scalar}"/>. Only the products that stay
    /// positive on every component are checked, so the unsigned types are covered too.
    /// </summary>
    public static void Cross<T, TScalar>(bool simd)
        where T : unmanaged, IVector3Arithmetic<T, TScalar>
        where TScalar : unmanaged, INumber<TScalar>
    {
        var zero = TScalar.Zero;
        var one = TScalar.One;
        var allTwo = T.Broadcast(TScalar.CreateChecked(2));
        var axisX = Vec<T, TScalar>(3, simd, one, zero, zero);
        var axisY = Vec<T, TScalar>(3, simd, zero, one, zero);
        var axisZ = Vec<T, TScalar>(3, simd, zero, zero, one);
        var vZero = Vec<T, TScalar>(3, simd, zero, zero, zero);

        using (Assert.EnterMultipleScope())
        {
            AllEqual<T, TScalar>(axisX.cross(axisY), axisZ, "x cross y");
            AllEqual<T, TScalar>(axisY.cross(axisZ), axisX, "y cross z");
            AllEqual<T, TScalar>(axisZ.cross(axisX), axisY, "z cross x");
            AllEqual<T, TScalar>(axisX.cross(axisX), vZero, "x cross x");
            AllEqual<T, TScalar>(vZero.cross(axisZ), vZero, "0 cross z");
            AllEqual<T, TScalar>(axisX.cross(allTwo * axisY), allTwo * axisZ, "cross is linear");

            ScalarEqual(axisZ.dot(axisX.cross(axisY)), one, "cross of the basis vectors is the third axis");
            ScalarEqual(axisX.dot(axisX.cross(axisY)), zero, "cross is orthogonal to the left operand");
            ScalarEqual(axisY.dot(axisX.cross(axisY)), zero, "cross is orthogonal to the right operand");
        }
    }

    /// <summary>
    /// The cross product of a signed 3 component vector, the parts that need a negative value.
    /// </summary>
    public static void SignedCross<T, TScalar>(bool simd)
        where T : unmanaged, ISignedVectorArithmetic<T, TScalar>, IVector3Arithmetic<T, TScalar>
        where TScalar : unmanaged, ISignedNumber<TScalar>
    {
        var zero = TScalar.Zero;
        var one = TScalar.One;
        var axisX = Vec<T, TScalar>(3, simd, one, zero, zero);
        var axisY = Vec<T, TScalar>(3, simd, zero, one, zero);
        var axisZ = Vec<T, TScalar>(3, simd, zero, zero, one);

        using (Assert.EnterMultipleScope())
        {
            AllEqual<T, TScalar>(axisX.cross(axisY), -axisY.cross(axisX), "cross is anti commutative");
            AllEqual<T, TScalar>((axisX + axisY).cross(axisZ), axisX.cross(axisZ) + axisY.cross(axisZ),
                "cross is linear on the left operand");
        }
    }

    #endregion

    #region padding lane

    /// <summary>
    /// The padding lane of a simd backed 3 component vector has to stay zero, the comparison masks, the
    /// reductions and the equality checks rely on it. <paramref name="padding"/> reads that lane.
    /// </summary>
    public static void PaddingStaysZero<T, TScalar>(bool simd, Func<T, TScalar> padding)
        where T : unmanaged, IVectorArithmetic<T, TScalar>
        where TScalar : unmanaged, INumber<TScalar>
    {
        var one = TScalar.One;
        var two = TScalar.CreateChecked(2);
        var three = TScalar.CreateChecked(3);
        var five = TScalar.CreateChecked(5);
        var a = Vec<T, TScalar>(3, simd, one, two, three);
        // every component of b differs from the matching one of a, so no unlerp divides by zero
        var b = Vec<T, TScalar>(3, simd, three, five, one);

        void StaysZero(T v, string what)
            => Assert.That(padding(v), Is.EqualTo(default(TScalar)), $"padding lane after {what}");

        using (Assert.EnterMultipleScope())
        {
            StaysZero(a, "the load");
            StaysZero(a + b, "+");
            StaysZero(a - b, "-");
            StaysZero(a * b, "*");
            StaysZero(a / b, "/");
            StaysZero(a % b, "%");
            StaysZero(a.abs(), "abs");
            StaysZero(a.sign(), "sign");
            StaysZero(a.min(b), "min");
            StaysZero(a.max(b), "max");
            StaysZero(a.clamp(b, a), "clamp");
            StaysZero(a.square(), "square");
            StaysZero(a.lerp(b, a), "lerp");
            StaysZero(a.unlerp(a, b), "unlerp");
            StaysZero(a.remap(a, b, b, a), "remap");
            StaysZero(T.fma(a, b, a), "fma");
            StaysZero(T.fms(a, b, a), "fms");
            StaysZero(T.fnma(a, b, a), "fnma");
        }
    }

    #endregion
}
