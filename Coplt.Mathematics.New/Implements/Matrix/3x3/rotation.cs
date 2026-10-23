namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the rotation of the value, which is the rotation of the space that the value holds where the
        /// value keeps the distances of the space and the angles of it, and the rotation closest to the value
        /// otherwise
        /// <para>The columns of a value that keeps the distances of the space and the angles of it are the axes of
        /// the space, so the value is a rotation of the space where the determinant of it is the one of the kind,
        /// and the value whose columns are those axes scaled by other lengths holds the same rotation, which the
        /// member reads by scaling every column of the value to the length of one.</para>
        /// <para>A value whose columns are not the axes of a rotation of the space reaches the rotation of the
        /// singular value decomposition of the value, which is the rotation closest to it.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <returns>The rotation of the value, which is a value of the length of one</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion rotation(float3x3 m)
        {
            var det = math.determinant(m);
            if (math.abs(1f - det) < svd.EpsilonDeterminantFloat)
                return new quaternion(m);

            if (math.abs(det) > svd.EpsilonDeterminantFloat)
            {
                var scaled = m * math.rsqrt(new float3(
                    math.length_sq(m.c0), math.length_sq(m.c1), math.length_sq(m.c2)));
                if (math.abs(1f - math.determinant(scaled)) < svd.EpsilonDeterminantFloat)
                    return new quaternion(scaled);
            }

            return svd.svdRotation(m);
        }

        /// <inheritdoc cref="rotation(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_d rotation(double3x3 m)
        {
            var det = math.determinant(m);
            if (math.abs(1d - det) < svd.EpsilonDeterminantDouble)
                return new quaternion_d(m);

            if (math.abs(det) > svd.EpsilonDeterminantDouble)
            {
                var scaled = m * math.rsqrt(new double3(
                    math.length_sq(m.c0), math.length_sq(m.c1), math.length_sq(m.c2)));
                if (math.abs(1d - math.determinant(scaled)) < svd.EpsilonDeterminantDouble)
                    return new quaternion_d(scaled);
            }

            return svd.svdRotation(m);
        }

        /// <inheritdoc cref="rotation(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_h rotation(half3x3 m)
        {
            var det = math.determinant(m);
            if (math.abs((half)1f - det) < svd.EpsilonDeterminantHalf)
                return new quaternion_h(m);

            if (math.abs(det) > svd.EpsilonDeterminantHalf)
            {
                var scaled = m * math.rsqrt(new half3(
                    math.length_sq(m.c0), math.length_sq(m.c1), math.length_sq(m.c2)));
                if (math.abs((half)1f - math.determinant(scaled)) < svd.EpsilonDeterminantHalf)
                    return new quaternion_h(scaled);
            }

            return svd.svdRotation(m);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.rotation(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion rotation(this float3x3 m) => math.rotation(m);

        /// <inheritdoc cref="math.rotation(double3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_d rotation(this double3x3 m) => math.rotation(m);

        /// <inheritdoc cref="math.rotation(half3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_h rotation(this half3x3 m) => math.rotation(m);
    }
}
