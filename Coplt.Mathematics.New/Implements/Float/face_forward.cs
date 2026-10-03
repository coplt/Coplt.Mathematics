using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <paramref name="a"/> with the sign chosen so that it faces away from the incident vector
        /// <paramref name="i"/>, which flips the sign of it when the dot product of <paramref name="ng"/> and
        /// <paramref name="i"/> is not negative
        /// <para>The condition of the member is the sign of the dot product of two whole vectors, which the
        /// value of a single component holds and the members of the dispatch of a value that reach a component
        /// of it do not: the whole of the member lives in the visitor of the kind of the value, which decides
        /// the type of a component of the value it is handed over, so the type of the value is the only type
        /// parameter of the member</para>
        /// </summary>
        /// <param name="a">The value to orient</param>
        /// <param name="i">The incident vector</param>
        /// <param name="ng">The vector that is used to choose the sign</param>
        /// <typeparam name="T">The type of the vectors</typeparam>
        /// <returns>The oriented vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T face_forward<T>(T a, T i, T ng)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Self<impl_face_forward>(a, i, ng);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.face_forward{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T face_forward<T>(this T a, T i, T ng)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Self<impl_face_forward>(a, i, ng);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value whose every component is the one of the value with the sign of it flipped when the dot product
    /// of the incident vector and the normal is not negative
    /// <para>The value of a vector that keeps it in a register reaches the member of the visitor that matches
    /// the width of the register, which takes the dot product of the register itself, and the value of every
    /// other vector reaches the member of the count of its components, which takes the dot product of the
    /// members of it</para>
    /// <para>The two padding lanes of a value are zero, so the dot product of them is zero as well and it is
    /// not negative: the sign of the padding lane follows the sign of the components of the value, which keeps
    /// the zero of it</para>
    /// </summary>
    internal struct impl_face_forward : IAlgebraVisitor_T_T_T_T<impl_face_forward>
    {
        #region Vector Simd Float

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_face_forward>.Simd_Float<TVector, TScalar>(
            Vector128<TScalar> a, Vector128<TScalar> i, Vector128<TScalar> ng
        ) => TVector.UnsafeFromUnderlying((Vector128.Dot(ng, i) >= TScalar.Zero ? -a : a).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_face_forward>.Simd_Float<TVector, TScalar>(
            Vector256<TScalar> a, Vector256<TScalar> i, Vector256<TScalar> ng
        ) => TVector.UnsafeFromUnderlying((Vector256.Dot(ng, i) >= TScalar.Zero ? -a : a).AsByte());

        #endregion

        #region Vector Soft Float

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_face_forward>.Vector2_Float<TVector, TScalar>(
            TVector a, TVector i, TVector ng
        ) => TVector.Scalar<impl_dot>(ng, i) >= TScalar.Zero ? -a : a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_face_forward>.Vector3_Float<TVector, TScalar>(
            TVector a, TVector i, TVector ng
        ) => TVector.Scalar<impl_dot>(ng, i) >= TScalar.Zero ? -a : a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T_T_T<impl_face_forward>.Vector4_Float<TVector, TScalar>(
            TVector a, TVector i, TVector ng
        ) => TVector.Scalar<impl_dot>(ng, i) >= TScalar.Zero ? -a : a;

        #endregion
    }
}
