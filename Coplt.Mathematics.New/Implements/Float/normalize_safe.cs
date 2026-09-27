using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, it returns the zero of the kind of the
        /// value when the length of it is zero
        /// <para>It is the value multiplied by the reciprocal of the length of it, which is exact, the estimate
        /// of the hardware is <see cref="normalize_safe_a{T}"/> instead</para>
        /// <para>The value is the zero of the kind of it when the squared length of it is not above the
        /// smallest positive normal value of the kind of a component, which is what the squared length of a
        /// value that is too short to be scaled is as well</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of one,
        /// or the zero of the kind of the value when the length of it is zero</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe<T>(in T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize_safe>(value);

        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, which is the estimate of the hardware,
        /// and it returns the zero of the kind of the value when the length of it is zero
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact result takes <see cref="normalize_safe{T}"/></para>
        /// <para>The value is the zero of the kind of it when the squared length of it is not above the
        /// smallest positive normal value of the kind of a component, which is what the squared length of a
        /// value that is too short to be scaled is as well</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of one,
        /// or the zero of the kind of the value when the length of it is zero</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe_a<T>(in T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize_safe_approx>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.normalize_safe{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe<T>(this T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize_safe>(value);

        /// <inheritdoc cref="math.normalize_safe_a{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe_a<T>(this T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize_safe_approx>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value scaled to a length of one, which is the zero of the kind of the value when the squared length of
    /// it is not above the smallest positive normal value of the kind of a component:
    /// <code>
    /// var len = dot(value, value);
    /// if (len &lt;= ScalarMinNormal) return Zero;
    /// return value * (1 / sqrt(len));
    /// </code>
    /// <para>The value is handed over as a whole, so the member computes with the members of the kind of it,
    /// which keep the padding lanes of a register at zero</para>
    /// </summary>
    internal struct impl_normalize_safe : IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize_safe>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TSelf IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize_safe>.Map<TSelf, TScalar>(in TSelf vector)
        {
            var len = math.dot<TSelf, TScalar>(vector, vector);
            if (len <= TSelf.ScalarMinNormal) return TSelf.Zero;
            return vector * (TScalar.One / TScalar.Sqrt(len));
        }
    }

    /// <summary>
    /// The value scaled to a length of one, which is the zero of the kind of the value when the squared length of
    /// it is not above the smallest positive normal value of the kind of a component and which is the estimate of
    /// the hardware of the reciprocal of the length of it otherwise:
    /// <code>
    /// var len = dot(value, value);
    /// if (len &lt;= ScalarMinNormal) return Zero;
    /// return value * ReciprocalSqrtEstimate(len);
    /// </code>
    /// <para>The value is handed over as a whole, so the member computes with the members of the kind of it,
    /// which keep the padding lanes of a register at zero</para>
    /// </summary>
    internal struct impl_normalize_safe_approx : IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize_safe_approx>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TSelf IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize_safe_approx>.Map<TSelf, TScalar>(in TSelf vector)
        {
            var len = math.dot<TSelf, TScalar>(vector, vector);
            if (len <= TSelf.ScalarMinNormal) return TSelf.Zero;
            return vector * TScalar.ReciprocalSqrtEstimate(len);
        }
    }
}
