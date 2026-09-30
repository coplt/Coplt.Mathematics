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
        /// flipping the sign of the value is <see cref="copy_sign{T}(in T, in T)"/></para>
        /// </summary>
        /// <param name="a">The value whose sign is flipped where the other one is negative</param>
        /// <param name="sign">The value that says whether the sign of every component is flipped</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value with the flipped sign</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T chg_sign<T>(in T a, in T sign) where T : unmanaged, IFloatingPointAlgebra<T>
            => (sign & T.SignMask) ^ a;
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.chg_sign{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T chg_sign<T>(this T a, in T sign) where T : unmanaged, IFloatingPointAlgebra<T>
            => (sign & T.SignMask) ^ a;
    }
}
