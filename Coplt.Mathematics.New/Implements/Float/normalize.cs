using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, the result is not a number when the
        /// length of the value is zero
        /// <para>It is the value multiplied by the reciprocal of the length of it, which is exact, the estimate
        /// of the hardware is <see cref="normalize_a{T, TScalar}(in T)"/> instead</para>
        /// <para>A value whose length is zero cannot be scaled to one, so the length of the result is not a
        /// number: <see cref="normalize_safe{T, TScalar}(in T)"/> returns the zero of the kind of the value
        /// instead</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of
        /// one</returns>
        // the type of a single component is only a part of the result of the member, so the compiler cannot
        // infer it from the arguments and a call that does not name it reaches the member of the scalar type
        // of the value instead, which the generator emits for every scalar type: the attribute marks this
        // member for it
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize<T, TScalar>(in T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => value * (TScalar.One / TScalar.Sqrt(dot<T, TScalar>(value, value)));

        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, which is the estimate of the hardware
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact result takes <see cref="normalize{T, TScalar}(in T)"/></para>
        /// <para>A value whose length is zero cannot be scaled to one, so the length of the result is not a
        /// number: <see cref="normalize_safe_a{T, TScalar}(in T)"/> returns the zero of the kind of the value
        /// instead</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of
        /// one</returns>
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_a<T, TScalar>(in T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => value * TScalar.ReciprocalSqrtEstimate(dot<T, TScalar>(value, value));
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.normalize{T, TScalar}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize<T, TScalar>(this T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.normalize<T, TScalar>(value);

        /// <inheritdoc cref="math.normalize_a{T, TScalar}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_a<T, TScalar>(this T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.normalize_a<T, TScalar>(value);
    }
}
