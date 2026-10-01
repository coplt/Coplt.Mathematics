using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the determinant of the value, which is the volume of the parallelepiped the three columns of it
        /// are the sides of, signed by the order of them
        /// <para>The determinant of the value is the scalar triple product of the columns of it, which is the dot
        /// product of the first column of it with the cross product of the two columns that follow it, so the
        /// determinant of a matrix whose columns are within the same plane is the zero of the kind of a component
        /// of it.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <returns>The determinant of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(in float3x3 m) => math.dot(m.c0, cross(m.c1, m.c2));

        /// <inheritdoc cref="determinant(in float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double determinant(in double3x3 m) => math.dot(m.c0, cross(m.c1, m.c2));

        /// <inheritdoc cref="determinant(in float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half determinant(in half3x3 m) => math.dot(m.c0, cross(m.c1, m.c2));
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.determinant(in float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(this float3x3 m) => math.determinant(m);

        /// <inheritdoc cref="math.determinant(in double3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double determinant(this double3x3 m) => math.determinant(m);

        /// <inheritdoc cref="math.determinant(in half3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half determinant(this half3x3 m) => math.determinant(m);
    }
}
