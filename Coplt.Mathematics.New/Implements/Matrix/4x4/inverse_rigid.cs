namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the inverse of the value, which is the matrix of the same shape that multiplies with the value
        /// to the identity of the kind of it, where the value is a value that keeps the distances of the space: such
        /// a value is the position of a point of the space added to the product of the turn of the value, which is
        /// the matrix of 3 rows and 3 columns of the three first rows and the three first columns of it and whose
        /// rows are of the unit length and perpendicular to each other, so the inverse of the value is the turn of
        /// the value turned around with the position of it turned by the turn of it and negated
        /// <para>The result of this member holds where the value keeps the distances of the space: the inverse of a
        /// value that scales the space is the member of the value instead, and the inverse of a value that turns the
        /// space around a line that is not through the origin of it is the member of the value as well.</para>
        /// <para>The turn of the value is reached through the registers of the columns of the value, which hold four
        /// of the components of the rows of the value at a time: the unpack of the components of two columns holds
        /// the components of two rows of the value where the components of the rows are taken in turn, and the
        /// unpack of the two results holds the components of one row of the kind of the component of the value in
        /// every register.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 4 rows and 4 columns that keeps the distances of the space</param>
        /// <returns>The inverse of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4x4 inverse_rigid(float4x4 m)
        {
            var (c0, c1, c2, pos) = m;

            // the pairs of the rows of the turn of the value: the components of the two first rows of it are the
            // components of the two first columns of it taken in turn, and the components of the two last columns
            // hold the components of the third row of it
            var t0 = math.unpacklo(c0, c2);
            var t1 = math.unpacklo(c1, default);
            var t2 = math.unpackhi(c0, c2);
            var t3 = math.unpackhi(c1, default);

            var r0 = math.unpacklo(t0, t1);
            var r1 = math.unpackhi(t0, t1);
            var r2 = math.unpacklo(t2, t3);

            // the position of the value turned by the rows of the turn of it and negated: the position of the value
            // is a single point of the space, so every component of it scales a whole row of the turn of it
            pos = -math.fam(math.fam(r0 * pos.x, r1, pos.y), r2, pos.z);
            pos.w = 1.0f;

            return new(r0, r1, r2, pos);
        }

        /// <inheritdoc cref="inverse_rigid(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4x4 inverse_rigid(double4x4 m)
        {
            var (c0, c1, c2, pos) = m;

            // the pairs of the rows of the turn of the value: the components of the two first rows of it are the
            // components of the two first columns of it taken in turn, and the components of the two last columns
            // hold the components of the third row of it
            var t0 = math.unpacklo(c0, c2);
            var t1 = math.unpacklo(c1, default);
            var t2 = math.unpackhi(c0, c2);
            var t3 = math.unpackhi(c1, default);

            var r0 = math.unpacklo(t0, t1);
            var r1 = math.unpackhi(t0, t1);
            var r2 = math.unpacklo(t2, t3);

            // the position of the value turned by the rows of the turn of it and negated: the position of the value
            // is a single point of the space, so every component of it scales a whole row of the turn of it
            pos = -math.fam(math.fam(r0 * pos.x, r1, pos.y), r2, pos.z);
            pos.w = 1.0;

            return new(r0, r1, r2, pos);
        }

        /// <inheritdoc cref="inverse_rigid(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4x4 inverse_rigid(half4x4 m)
        {
            var (c0, c1, c2, pos) = m;

            // the pairs of the rows of the turn of the value: the components of the two first rows of it are the
            // components of the two first columns of it taken in turn, and the components of the two last columns
            // hold the components of the third row of it
            var t0 = math.unpacklo(c0, c2);
            var t1 = math.unpacklo(c1, default);
            var t2 = math.unpackhi(c0, c2);
            var t3 = math.unpackhi(c1, default);

            var r0 = math.unpacklo(t0, t1);
            var r1 = math.unpackhi(t0, t1);
            var r2 = math.unpacklo(t2, t3);

            // the position of the value turned by the rows of the turn of it and negated: the position of the value
            // is a single point of the space, so every component of it scales a whole row of the turn of it
            pos = -math.fam(math.fam(r0 * pos.x, r1, pos.y), r2, pos.z);
            pos.w = Half.One;

            return new(r0, r1, r2, pos);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.inverse_rigid(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4x4 inverse_rigid(this float4x4 m) => math.inverse_rigid(m);

        /// <inheritdoc cref="math.inverse_rigid(double4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4x4 inverse_rigid(this double4x4 m) => math.inverse_rigid(m);

        /// <inheritdoc cref="math.inverse_rigid(half4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4x4 inverse_rigid(this half4x4 m) => math.inverse_rigid(m);
    }
}
