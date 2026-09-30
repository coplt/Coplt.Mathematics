using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the value that has the magnitude of every component of <paramref name="a"/> and the sign of
        /// the matching component of <paramref name="sign"/>
        /// <para>The sign of the kind of a component and no other bit of it is the mask of the algebra of the
        /// kind of the value, so the member takes that bit of the other one with the mask and the value without
        /// that bit with the mask of it that is not set, and it puts the two of them together: unlike
        /// <see cref="chg_sign{T}(in T, in T)"/>, which flips the sign of the value where the other one is
        /// negative, the sign of the value is the one of the other one whatever it is</para>
        /// <para>It is the member of the C standard of the two, see
        /// <see href="https://en.cppreference.com/w/c/numeric/math/copysign"/></para>
        /// </summary>
        /// <param name="a">The value that provides the magnitude of every component</param>
        /// <param name="sign">The value that provides the sign of every component</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value with the magnitude of one of the two and the sign of the other one of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T copy_sign<T>(in T a, in T sign) where T : unmanaged, IFloatingPointAlgebra<T>
            => (sign & T.SignMask) | (a & ~T.SignMask);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.copy_sign{T}(in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T copy_sign<T>(this T a, in T sign) where T : unmanaged, IFloatingPointAlgebra<T>
            => (sign & T.SignMask) | (a & ~T.SignMask);
    }
}
