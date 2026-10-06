using System.Runtime.Intrinsics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <paramref name="a"/> whose sign bit is flipped at every component that the matching component
        /// of <paramref name="sign"/> is negative
        /// <para>The sign of the kind of a component and no other bit of it is the mask of the algebra of the
        /// kind of the value, so the member flips that bit of the value with the exclusive or of the mask of the
        /// sign of the other one, which is the mask where the other one is negative and no bit at all where it is
        /// not: the value multiplied by the sign of the other one</para>
        /// <para>The member that takes the magnitude of the value with the sign of the other one instead of
        /// flipping the sign of the value is <see cref="copy_sign{T}(T, T)"/></para>
        /// </summary>
        /// <param name="a">The value whose sign is flipped where the other one is negative</param>
        /// <param name="sign">The value that says whether the sign of every component is flipped</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value with the flipped sign</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T chg_sign<T>(T a, T sign) where T : unmanaged, IFloatingPointAlgebra<T>
            => (sign & T.SignMask) ^ a;

        /// <summary>
        /// Returns <paramref name="a"/> whose sign bit is flipped where <paramref name="sign"/> is negative
        /// <para>The member reads the bits of the value with the register of 128 bits where the hardware
        /// accelerates it, which is the one of the value itself, and with the integer of the same width as the
        /// component where it does not, see <see cref="chg_sign(double, double)"/> and
        /// <see cref="chg_sign(half, half)"/></para>
        /// </summary>
        /// <param name="a">The value whose sign is flipped where the other one is negative</param>
        /// <param name="sign">The value that says whether the sign of the value is flipped</param>
        /// <returns>The value with the flipped sign</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float chg_sign(float a, float sign) =>
            Vector128.IsHardwareAccelerated
                ? (Vector128.CreateScalarUnsafe(a) ^
                   (Vector128.CreateScalarUnsafe(sign) & Vector128.CreateScalarUnsafe(float.NegativeZero))).ToScalar()
                : BitConverter.Int32BitsToSingle(
                    BitConverter.SingleToInt32Bits(a) ^ (BitConverter.SingleToInt32Bits(sign) & int.MinValue));

        /// <inheritdoc cref="chg_sign(float, float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double chg_sign(double a, double sign) =>
            Vector128.IsHardwareAccelerated
                ? (Vector128.CreateScalarUnsafe(a) ^
                   (Vector128.CreateScalarUnsafe(sign) & Vector128.CreateScalarUnsafe(double.NegativeZero))).ToScalar()
                : BitConverter.Int64BitsToDouble(
                    BitConverter.DoubleToInt64Bits(a) ^ (BitConverter.DoubleToInt64Bits(sign) & long.MinValue));

        /// <summary>
        /// Returns <paramref name="a"/> whose sign bit is flipped where <paramref name="sign"/> is negative
        /// <para>The kind of a half has no register of the hardware, so the member reads the bits of the value
        /// with the integer of the same width as the component</para>
        /// </summary>
        /// <param name="a">The value whose sign is flipped where the other one is negative</param>
        /// <param name="sign">The value that says whether the sign of the value is flipped</param>
        /// <returns>The value with the flipped sign</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half chg_sign(half a, half sign) =>
            BitConverter.UInt16BitsToHalf(
                (ushort)(BitConverter.HalfToUInt16Bits(a) ^ (BitConverter.HalfToUInt16Bits(sign) & 0x8000)));
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.chg_sign{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T chg_sign<T>(this T a, T sign) where T : unmanaged, IFloatingPointAlgebra<T>
            => (sign & T.SignMask) ^ a;
        /// <inheritdoc cref="math.chg_sign(float, float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float chg_sign(this float a, float sign) => math.chg_sign(a, sign);

        /// <inheritdoc cref="math.chg_sign(double, double)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double chg_sign(this double a, double sign) => math.chg_sign(a, sign);

        /// <inheritdoc cref="math.chg_sign(half, half)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half chg_sign(this half a, half sign) => math.chg_sign(a, sign);    }
}
