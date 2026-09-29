using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, it returns the zero of the kind of the
        /// value when the length of it is zero
        /// <para>It is the value multiplied by the reciprocal of the length of it, which is exact, the estimate
        /// of the hardware is <see cref="normalize_safe_a{T, TScalar}(in T)"/> instead</para>
        /// <para>The value is the zero of the kind of it when the squared length of it is not above the
        /// smallest positive normal value of the kind of a component, which is what the squared length of a
        /// value that is too short to be scaled is as well</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of one,
        /// or the zero of the kind of the value when the length of it is zero</returns>
        // the type of a single component is only a part of the result of the member, so the compiler cannot
        // infer it from the arguments and a call that does not name it reaches the member of the scalar type
        // of the value instead, which the generator emits for every scalar type: the attribute marks this
        // member for it
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe<T, TScalar>(in T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        {
            var len = dot<T, TScalar>(value, value);
            if (len <= T.ScalarMinNormal) return T.Zero;
            return value * (TScalar.One / TScalar.Sqrt(len));
        }

        /// <summary>
        /// Returns <paramref name="value"/> scaled to a length of one, which is the estimate of the hardware,
        /// and it returns the zero of the kind of the value when the length of it is zero
        /// <para>The estimate is not exact, the one of the arm platform is the loose one of the two, so a value
        /// that needs the exact result takes <see cref="normalize_safe{T, TScalar}(in T)"/></para>
        /// <para>The value is the zero of the kind of it when the squared length of it is not above the
        /// smallest positive normal value of the kind of a component, which is what the squared length of a
        /// value that is too short to be scaled is as well</para>
        /// </summary>
        /// <param name="value">The value to normalize</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The value whose every component is the component of the value scaled to a length of one,
        /// or the zero of the kind of the value when the length of it is zero</returns>
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe_a<T, TScalar>(in T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        {
            var len = dot<T, TScalar>(value, value);
            if (len <= T.ScalarMinNormal) return T.Zero;
            return value * TScalar.ReciprocalSqrtEstimate(len);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.normalize_safe{T, TScalar}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe<T, TScalar>(this T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.normalize_safe<T, TScalar>(value);

        /// <inheritdoc cref="math.normalize_safe_a{T, TScalar}(in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T normalize_safe_a<T, TScalar>(this T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.normalize_safe_a<T, TScalar>(value);
    }
}
