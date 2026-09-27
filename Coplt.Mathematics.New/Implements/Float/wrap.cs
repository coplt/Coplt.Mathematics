using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math_ex
    {
        extension(math)
        {
            /// <summary>
            /// Wraps every component of <paramref name="value"/> into the range of <paramref name="min"/> and
            /// <paramref name="max"/>, which puts a component that is outside of the range into it by the width of it
            /// <para>The range holds the lower bound of it and does not hold the upper one, so the components of a value
            /// that is inside the range are the ones of the value itself</para>
            /// </summary>
            /// <remarks>
            /// The offset of a component from the lower bound of the range is the remainder of the width of it, which
            /// is added to the bound:
            /// <code>
            /// var range = max - min;
            /// return min + fmod(value - min, range);
            /// </code>
            /// <para>The remainder is the one of <see cref="math.fmod{T}"/>, so a component that is above the range is
            /// wrapped into it from the lower bound of it and one that is below the range is wrapped into it from the
            /// upper bound</para>
            /// <para>The width of a range whose upper bound is not above its lower bound is not a width that a
            /// component can be wrapped into: the remainder of a component by the zero of the kind of it is not a
            /// number</para>
            /// </remarks>
            /// <param name="value">The value to wrap</param>
            /// <param name="min">The lower bound of the range, which the result holds</param>
            /// <param name="max">The upper bound of the range, which the result does not hold</param>
            /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
            /// <returns>The value whose every component is the component of the value wrapped into the range</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T wrap<T>(in T value, in T min, in T max)
                where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            {
                var range = max - min;
                return min + fmod(value - min, range);
            }
        }
    }

    public static partial class math
    {
        /// <summary>
        /// Wraps every component of <paramref name="value"/> into the range of <paramref name="min"/> and
        /// <paramref name="max"/>, which puts a component that is outside of the range into it by the width of it
        /// <para>The range holds the lower bound of it and does not hold the upper one, and both bounds are the
        /// same for every component of the value</para>
        /// </summary>
        /// <remarks>
        /// The offset of a component from the lower bound of the range is the remainder of the width of it, which
        /// is added to the bound:
        /// <code>
        /// var range = max - min;
        /// return min + fmod(value - min, range);
        /// </code>
        /// <para>The remainder is the one of <see cref="math.fmod{T}"/>, so a component that is above the range is
        /// wrapped into it from the lower bound of it and one that is below the range is wrapped into it from the
        /// upper bound, and every bound is the one of a single component</para>
        /// <para>The width of a range whose upper bound is not above its lower bound is not a width that a
        /// component can be wrapped into: the remainder of a component by the zero of the kind of it is not a
        /// number</para>
        /// </remarks>
        /// <param name="value">The value to wrap</param>
        /// <param name="min">The lower bound of the range, which every component of the result holds</param>
        /// <param name="max">The upper bound of the range, which no component of the result holds</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The value whose every component is the component of the value wrapped into the range</returns>
        [ScalarExtension]
        [OverloadResolutionPriority(-2)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T wrap<T, TScalar>(in T value, TScalar min, TScalar max)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointAlgebra<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        {
            var range = max - min;
            var vm = T.Broadcast(min);
            return vm + fmod(value - vm, T.Broadcast(range));
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math_ex.wrap{T}(in T, in T, in T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T wrap<T>(this T value, in T min, in T max)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>
            => math.wrap(value, min, max);

        /// <inheritdoc cref="math.wrap{T, TScalar}"/>
        [OverloadResolutionPriority(-2)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T wrap<T, TScalar>(this T value, TScalar min, TScalar max)
            where T : unmanaged, IFloatingPointAlgebraDispatch<T>, IFloatingPointAlgebra<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.wrap(value, min, max);
    }
}
