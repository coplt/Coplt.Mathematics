using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, the result is not a number when the
        /// length of the value is zero
        /// <para>It is the value multiplied by the reciprocal of the length of it, which is exact, the estimate
        /// of the hardware is <see cref="normalize_a{T}"/> instead</para>
        /// <para>A value whose length is zero cannot be scaled to one, so the length of the result is not a
        /// number: <see cref="normalize_safe{T}"/> returns the zero of the kind of the value instead</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of
        /// one</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize<T>(in T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize>(value);

        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, which is the estimate of the hardware
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact result takes <see cref="normalize{T}"/></para>
        /// <para>A value whose length is zero cannot be scaled to one, so the length of the result is not a
        /// number: <see cref="normalize_safe_a{T}"/> returns the zero of the kind of the value instead</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of
        /// one</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_a<T>(in T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize_approx>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.normalize{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize<T>(this T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize>(value);

        /// <inheritdoc cref="math.normalize_a{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_a<T>(this T value) where T : unmanaged, IFloatingPointVectorDispatch<T>
            => T.Map_Self<impl_normalize_approx>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value scaled to a length of one, which is the value multiplied by the reciprocal of the length of it:
    /// <code>
    /// return value * (1 / sqrt(dot(value, value)));
    /// </code>
    /// <para>The value is handed over as a whole, so the member computes with the members of the kind of it,
    /// which keep the padding lanes of a register at zero</para>
    /// </summary>
    internal struct impl_normalize : IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TSelf IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize>.Map<TSelf, TScalar>(in TSelf vector)
            => vector * (TScalar.One / TScalar.Sqrt(math.dot<TSelf, TScalar>(vector, vector)));
    }

    /// <summary>
    /// The value scaled to a length of one, which is the value multiplied by the estimate of the hardware of the
    /// reciprocal of the length of it:
    /// <code>
    /// return value * ReciprocalSqrtEstimate(dot(value, value));
    /// </code>
    /// <para>The value is handed over as a whole, so the member computes with the members of the kind of it,
    /// which keep the padding lanes of a register at zero</para>
    /// </summary>
    internal struct impl_normalize_approx : IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize_approx>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TSelf IFloatingPointAlgebraVisitor_Map_Self_Self<impl_normalize_approx>.Map<TSelf, TScalar>(in TSelf vector)
            => vector * TScalar.ReciprocalSqrtEstimate(math.dot<TSelf, TScalar>(vector, vector));
    }
}
