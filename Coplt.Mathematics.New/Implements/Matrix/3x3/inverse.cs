namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the inverse of the value, which is the matrix of the same shape that multiplies with the value
        /// to the identity of the kind of it: the value of it is the adjugate of the value divided by the
        /// determinant of it, and the adjugate of the value is the transpose of the value whose rows are the cross
        /// products of the columns of it taken in a cycle, the second with the third, the third with the first and
        /// the first with the second
        /// <para>A matrix whose determinant is the zero of the kind of a component of it has no inverse: the
        /// division of the adjugate of the value by the zero of the kind reaches the infinity of it, and a
        /// component whose adjugate is the zero of the kind as well is not a number.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <returns>The inverse of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 inverse(float3x3 m)
        {
            var (t0, t1, t2) = transpose(new float3x3(m.c1, m.c2, m.c0));

            var m0 = math.fsm(t1 * t2.yzx, t1.yzx, t2);
            var m1 = math.fsm(t0.yzx * t2, t0, t2.yzx);
            var m2 = math.fsm(t0 * t1.yzx, t0.yzx, t1);

            float3 rcp_det = (1.0f / math.sum(t0.zxy * m0));
            return new float3x3(m0, m1, m2) * rcp_det;
        }

        /// <inheritdoc cref="inverse(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3x3 inverse(double3x3 m)
        {
            var (t0, t1, t2) = transpose(new double3x3(m.c1, m.c2, m.c0));

            var m0 = math.fsm(t1 * t2.yzx, t1.yzx, t2);
            var m1 = math.fsm(t0.yzx * t2, t0, t2.yzx);
            var m2 = math.fsm(t0 * t1.yzx, t0.yzx, t1);

            double3 rcp_det = (1.0 / math.sum(t0.zxy * m0));
            return new double3x3(m0, m1, m2) * rcp_det;
        }

        /// <inheritdoc cref="inverse(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half3x3 inverse(half3x3 m)
        {
            var (t0, t1, t2) = transpose(new half3x3(m.c1, m.c2, m.c0));

            var m0 = math.fsm(t1 * t2.yzx, t1.yzx, t2);
            var m1 = math.fsm(t0.yzx * t2, t0, t2.yzx);
            var m2 = math.fsm(t0 * t1.yzx, t0.yzx, t1);

            var rcp_det = (Half.One / math.sum(t0.zxy * m0));
            return new half3x3(m0, m1, m2) * rcp_det;
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.inverse(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 inverse(this float3x3 m) => math.inverse(m);

        /// <inheritdoc cref="math.inverse(double3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3x3 inverse(this double3x3 m) => math.inverse(m);

        /// <inheritdoc cref="math.inverse(half3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half3x3 inverse(this half3x3 m) => math.inverse(m);
    }
}
