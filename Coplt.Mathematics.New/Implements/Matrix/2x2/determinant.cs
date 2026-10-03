namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the determinant of the value, which is the difference of the products of the two diagonals of
        /// it: the value of it is <c>a * d - b * c</c>, where the value is the matrix of 2 rows and 2 columns
        /// <c>[[a, b], [c, d]]</c>
        /// <para>The determinant of the value is the area of the parallelogram the two columns of it are the sides
        /// of, signed by the order of them, so it is the zero of the kind of a component of the value where the
        /// columns of it are the same line.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 2 rows and 2 columns</param>
        /// <returns>The determinant of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(float2x2 m) => math.fsm(m.m00 * m.m11, m.m01, m.m10);

        /// <inheritdoc cref="determinant(float2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double determinant(double2x2 m) => math.fsm(m.m00 * m.m11, m.m01, m.m10);

        /// <inheritdoc cref="determinant(float2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half determinant(half2x2 m) => math.fsm(m.m00 * m.m11, m.m01, m.m10);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.determinant(float2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(this float2x2 m) => math.determinant(m);

        /// <inheritdoc cref="math.determinant(double2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double determinant(this double2x2 m) => math.determinant(m);

        /// <inheritdoc cref="math.determinant(half2x2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half determinant(this half2x2 m) => math.determinant(m);
    }
}
