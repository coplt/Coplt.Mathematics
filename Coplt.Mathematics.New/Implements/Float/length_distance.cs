using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the length of <paramref name="value"/>, which is the square root of the dot product of the
        /// value with itself
        /// </summary>
        /// <param name="value">The vector</param>
        /// <typeparam name="T">The type of the vector</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The length of the vector</returns>
        // the type of a single component is only a part of the result of the member, so the compiler cannot
        // infer it from the arguments and a call that does not name it reaches the member of the scalar type
        // of the vector instead, which the generator emits for every scalar type: the attribute marks this
        // member for it
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar length<T, TScalar>(in T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => TScalar.Sqrt(dot<T, TScalar>(value, value));

        /// <summary>
        /// Returns the distance between the two vectors, which is the length of the difference of them
        /// </summary>
        /// <param name="from">The vector</param>
        /// <param name="to">The other vector</param>
        /// <typeparam name="T">The type of the vectors</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The distance between the two vectors</returns>
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar distance<T, TScalar>(in T from, in T to)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => length<T, TScalar>(to - from);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.length{T, TScalar}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar length<T, TScalar>(this T value)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.length<T, TScalar>(value);

        /// <inheritdoc cref="math.distance{T, TScalar}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar distance<T, TScalar>(this T from, in T to)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.distance<T, TScalar>(from, to);
    }
}
