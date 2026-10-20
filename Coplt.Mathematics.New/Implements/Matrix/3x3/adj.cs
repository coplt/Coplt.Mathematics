namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the adjugate of the value, which is the transpose of the value whose columns are the cross
        /// products of the columns of the value taken in a cycle, the second with the third, the third with the
        /// first and the first with the second, and stores the determinant of the value into
        /// <paramref name="det"/>
        /// <para>The product of the value and the adjugate of it is the determinant of it times the identity of
        /// the kind of it, so the adjugate of the value divided by the determinant of it is the inverse of it
        /// where the determinant of it is not the zero of the kind.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <param name="det">Receives the determinant of the value</param>
        /// <returns>The adjugate of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 adj(float3x3 m, out float det)
        {
            var adjT = new float3x3(cross(m.c1, m.c2), cross(m.c2, m.c0), cross(m.c0, m.c1));
            det = math.dot(m.c0, adjT.c0);
            return transpose(adjT);
        }

        /// <inheritdoc cref="adj(float3x3, out float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3x3 adj(double3x3 m, out double det)
        {
            var adjT = new double3x3(cross(m.c1, m.c2), cross(m.c2, m.c0), cross(m.c0, m.c1));
            det = math.dot(m.c0, adjT.c0);
            return transpose(adjT);
        }

        /// <inheritdoc cref="adj(float3x3, out float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half3x3 adj(half3x3 m, out half det)
        {
            var adjT = new half3x3(cross(m.c1, m.c2), cross(m.c2, m.c0), cross(m.c0, m.c1));
            det = math.dot(m.c0, adjT.c0);
            return transpose(adjT);
        }

        /// <summary>
        /// Returns whether the value has an inverse and stores the inverse of it into <paramref name="i"/>
        /// <para>The value has an inverse when the absolute value of the determinant of it is above the epsilon of
        /// the kind of a component of it, which is <c>1e-30</c> for a value of a float component, <c>1e-300</c> for
        /// one of a double component and <c>1e-5</c> for one of a half component; the value that the member of
        /// the value itself stores where the value has none is the adjugate of it, which is not the inverse of
        /// it.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <param name="i">Receives the inverse of the value, or the adjugate of it where the value has none</param>
        /// <returns><c>true</c> where the value has an inverse and <c>false</c> otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(float3x3 m, out float3x3 i) => adj(m, out i, 1e-30f);

        /// <inheritdoc cref="adj(float3x3, out float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(double3x3 m, out double3x3 i) => adj(m, out i, 1e-300);

        /// <inheritdoc cref="adj(float3x3, out float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(half3x3 m, out half3x3 i) => adj(m, out i, (half)1e-5f);

        /// <summary>
        /// Returns whether the value has an inverse and stores the inverse of it into <paramref name="i"/>
        /// <para>The value has an inverse when the absolute value of the determinant of it is above
        /// <paramref name="epsilon"/>; the value that the member of the value itself stores where the value has
        /// none is the adjugate of it, which is not the inverse of it.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <param name="i">Receives the inverse of the value, or the adjugate of it where the value has none</param>
        /// <param name="epsilon">The least absolute value of the determinant of the value that the value has an
        /// inverse at</param>
        /// <returns><c>true</c> where the value has an inverse and <c>false</c> otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(float3x3 m, out float3x3 i, float epsilon)
        {
            var adjugate = adj(m, out float det);
            var invertible = float.Abs(det) > epsilon;
            i = invertible ? adjugate * (1f / det) : adjugate;
            return invertible;
        }

        /// <inheritdoc cref="adj(float3x3, out float3x3, float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(double3x3 m, out double3x3 i, double epsilon)
        {
            var adjugate = adj(m, out double det);
            var invertible = double.Abs(det) > epsilon;
            i = invertible ? adjugate * (1.0 / det) : adjugate;
            return invertible;
        }

        /// <inheritdoc cref="adj(float3x3, out float3x3, float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(half3x3 m, out half3x3 i, half epsilon)
        {
            var adjugate = adj(m, out half det);
            var invertible = half.Abs(det) > epsilon;
            i = invertible ? adjugate * (Half.One / det) : adjugate;
            return invertible;
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.adj(float3x3, out float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 adj(this float3x3 m, out float det) => math.adj(m, out det);

        /// <inheritdoc cref="math.adj(double3x3, out double)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3x3 adj(this double3x3 m, out double det) => math.adj(m, out det);

        /// <inheritdoc cref="math.adj(half3x3, out half)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half3x3 adj(this half3x3 m, out half det) => math.adj(m, out det);

        /// <inheritdoc cref="math.adj(float3x3, out float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(this float3x3 m, out float3x3 i) => math.adj(m, out i);

        /// <inheritdoc cref="math.adj(double3x3, out double3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(this double3x3 m, out double3x3 i) => math.adj(m, out i);

        /// <inheritdoc cref="math.adj(half3x3, out half3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(this half3x3 m, out half3x3 i) => math.adj(m, out i);

        /// <inheritdoc cref="math.adj(float3x3, out float3x3, float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(this float3x3 m, out float3x3 i, float epsilon) => math.adj(m, out i, epsilon);

        /// <inheritdoc cref="math.adj(double3x3, out double3x3, double)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(this double3x3 m, out double3x3 i, double epsilon) => math.adj(m, out i, epsilon);

        /// <inheritdoc cref="math.adj(half3x3, out half3x3, half)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool adj(this half3x3 m, out half3x3 i, half epsilon) => math.adj(m, out i, epsilon);
    }
}
