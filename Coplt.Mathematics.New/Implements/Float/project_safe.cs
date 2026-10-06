using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the projection of <paramref name="value"/> onto <paramref name="onto"/>, and it returns
        /// <paramref name="default_value"/> when the projection of it is not a value
        /// <para>It is the component of <paramref name="value"/> that is parallel to <paramref name="onto"/>, the
        /// rest of the value is the component of it that is inside the plane that has <paramref name="onto"/> as
        /// its normal</para>
        /// </summary>
        /// <remarks>
        /// The projection of the value onto the vector is the vector scaled by the quotient of the dot product of
        /// the two and the dot product of the vector with itself, and the value of it is taken as it is when it is
        /// a value of the kind of the component and the one the caller names when it is not:
        /// <code>
        /// var proj = project_unsafe(value, onto);
        /// return all(is_finite(proj)) ? proj : default_value;
        /// </code>
        /// <para>A vector that is the zero of the kind of it has no direction to be projected onto: the quotient
        /// of the projection of the value onto it is a value that is not a number and every component of the
        /// result is taken from <paramref name="default_value"/>.
        /// <see cref="math.project{T}(T, T)"/> answers with the zero of the kind of the value for such a vector
        /// and <see cref="math.project_unsafe{T}(T, T)"/> hands the value that is not a number over to the
        /// caller.</para>
        /// </remarks>
        /// <param name="value">The value to project</param>
        /// <param name="onto">The vector to project onto, it does <b>not</b> have to be of unit length</param>
        /// <param name="default_value">The value that is returned when the projection of the value onto the
        /// vector is not a value</param>
        /// <typeparam name="T">The type of the value, a vector</typeparam>
        /// <returns>The value whose every component is the component of the value that is parallel to the
        /// vector, and the one of <paramref name="default_value"/> where the projection of it is not a
        /// value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_safe<T>(T value, T onto, T default_value = default)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
        {
            var proj = project_unsafe(value, onto);
            return all(is_finite(proj)) ? proj : default_value;
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.project_safe{T}(T, T, T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T project_safe<T>(this T value, T onto, T default_value = default)
            where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
            => math.project_safe(value, onto, default_value);
    }
}
