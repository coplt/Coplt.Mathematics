using Coplt.Mathematics.Algebras;
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
        /// of the hardware is <see cref="normalize_safe_a{T}(T)"/> instead</para>
        /// <para>The value is the zero of the kind of it when the squared length of it is not above the
        /// smallest positive normal value of the kind of a component, which is what the squared length of a
        /// value that is too short to be scaled is as well</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of one,
        /// or the zero of the kind of the value when the length of it is zero</returns>
        // the value hands itself over to the visitor of the map, which decides the kind of the map of its own, so
        // a call that names the type of the value alone reaches the level of the kind of it
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe<T>(T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Map_Self<impl_normalize_safe>(value);

        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, which is the estimate of the hardware,
        /// and it returns the zero of the kind of the value when the length of it is zero
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact result takes <see cref="normalize_safe{T}(T)"/></para>
        /// <para>The value is the zero of the kind of it when the squared length of it is not above the
        /// smallest positive normal value of the kind of a component, which is what the squared length of a
        /// value that is too short to be scaled is as well</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of one,
        /// or the zero of the kind of the value when the length of it is zero</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe_a<T>(T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => T.Map_Self<impl_normalize_safe_a>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.normalize_safe{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe<T>(this T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.normalize_safe(value);

        /// <inheritdoc cref="math.normalize_safe_a{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe_a<T>(this T value)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.normalize_safe_a(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value of a vector scaled to a length of one, the zero of the kind of the value when the squared length
    /// of it is too short to be scaled
    /// <para>Only the level of the map of a floating point number is implemented, a value that hands itself over
    /// to any other level of the map of it reaches the member of the level of a number, which throws because
    /// neither a whole number nor a value that is not a number has a reciprocal square root</para>
    /// </summary>
    internal struct impl_normalize_safe : IMapVisitor<impl_normalize_safe>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMapVisitor<impl_normalize_safe>.Map_Float<TVector, TScalar>(TVector value)
        {
            var len = math.dot<TVector, TScalar>(value, value);
            if (len <= TVector.ScalarMinNormal) return TVector.Zero;
            return value * (TScalar.One / TScalar.Sqrt(len));
        }
    }

    /// <summary>
    /// The value of a vector scaled to a length of one through the estimate of the hardware, the zero of the kind
    /// of the value when the squared length of it is too short to be scaled
    /// <para>See <see cref="impl_normalize_safe"/> for the exact form, the estimate of the hardware is the loose
    /// one</para>
    /// </summary>
    internal struct impl_normalize_safe_a : IMapVisitor<impl_normalize_safe_a>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IMapVisitor<impl_normalize_safe_a>.Map_Float<TVector, TScalar>(TVector value)
        {
            var len = math.dot<TVector, TScalar>(value, value);
            if (len <= TVector.ScalarMinNormal) return TVector.Zero;
            return value * TScalar.ReciprocalSqrtEstimate(len);
        }
    }
}
