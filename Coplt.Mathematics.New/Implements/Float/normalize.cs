using Coplt.Mathematics.Algebras;
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
        /// of the hardware is <see cref="normalize_a{T}(in T)"/> instead</para>
        /// <para>A value whose length is zero cannot be scaled to one, so the length of the result is not a
        /// number: <see cref="normalize_safe{T}(in T)"/> returns the zero of the kind of the value
        /// instead</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of
        /// one</returns>
        // the value hands itself over to the visitor of the map, which decides the kind of the map of its own, so
        // a call that names the type of the value alone reaches the level of the kind of it
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize<T>(in T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Map_Self<impl_normalize>(value);

        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, which is the estimate of the hardware
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact result takes <see cref="normalize{T}(in T)"/></para>
        /// <para>A value whose length is zero cannot be scaled to one, so the length of the result is not a
        /// number: <see cref="normalize_safe_a{T}(in T)"/> returns the zero of the kind of the value
        /// instead</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of
        /// one</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_a<T>(in T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Map_Self<impl_normalize_a>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.normalize{T}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize<T>(this T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.normalize(value);

        /// <inheritdoc cref="math.normalize_a{T}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_a<T>(this T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.normalize_a(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value of a vector scaled to a length of one
    /// <para>Only the level of the map of a floating point number is implemented, a value that hands itself over
    /// to any other level of the map of it reaches the member of the level of a number, which throws because
    /// neither a whole number nor a value that is not a number has a reciprocal square root</para>
    /// </summary>
    internal struct impl_normalize : IMapVisitor<impl_normalize>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMapVisitor<impl_normalize>.Map_Float<TVector, TScalar>(in TVector value)
            => value * (TScalar.One / TScalar.Sqrt(math.dot<TVector, TScalar>(value, value)));
    }

    /// <summary>
    /// The value of a vector scaled to a length of one through the estimate of the hardware
    /// <para>See <see cref="impl_normalize"/> for the exact form, the estimate of the hardware is the loose
    /// one</para>
    /// </summary>
    internal struct impl_normalize_a : IMapVisitor<impl_normalize_a>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMapVisitor<impl_normalize_a>.Map_Float<TVector, TScalar>(in TVector value)
            => value * TScalar.ReciprocalSqrtEstimate(math.dot<TVector, TScalar>(value, value));
    }
}
