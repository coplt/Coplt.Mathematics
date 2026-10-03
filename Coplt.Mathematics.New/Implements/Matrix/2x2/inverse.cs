namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the inverse of the value, which is the matrix of the same shape that multiplies with the value
        /// to the identity of the kind of it: the value of it is the adjugate of the value divided by the
        /// determinant of it, which is <c>1 / (a * d - b * c)</c> times <c>[[d, -b], [-c, a]]</c>, where the value
        /// is the matrix of 2 rows and 2 columns <c>[[a, b], [c, d]]</c>
        /// <para>A matrix whose determinant is the zero of the kind of a component of it has no inverse: the
        /// division of the adjugate of the value by the zero of the kind reaches the infinity of it, and a
        /// component whose adjugate is the zero of the kind as well is not a number.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 2 rows and 2 columns</param>
        /// <returns>The inverse of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2x2 inverse(in float2x2 m)
        {
            var det = fsm(m.m00 * m.m11, m.m01, m.m10);
            return new float2x2(m.m11, -m.m01, -m.m10, m.m00) * (1f / det);
        }

        /// <inheritdoc cref="inverse(in float2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x2 inverse(in double2x2 m)
        {
            var det = fsm(m.m00 * m.m11, m.m01, m.m10);
            return new double2x2(m.m11, -m.m01, -m.m10, m.m00) * (1d / det);
        }

        /// <inheritdoc cref="inverse(in float2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half2x2 inverse(in half2x2 m)
        {
            var det = fsm(m.m00 * m.m11, m.m01, m.m10);
            return new half2x2(m.m11, -m.m01, -m.m10, m.m00) * (half.One / det);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.inverse(in float2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2x2 inverse(this float2x2 m) => math.inverse(m);

        /// <inheritdoc cref="math.inverse(in double2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x2 inverse(this double2x2 m) => math.inverse(m);

        /// <inheritdoc cref="math.inverse(in half2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half2x2 inverse(this half2x2 m) => math.inverse(m);
    }
}
