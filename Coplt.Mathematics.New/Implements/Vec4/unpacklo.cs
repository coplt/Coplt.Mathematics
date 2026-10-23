namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the value whose low half interlaces the low half of <paramref name="a"/> with the low half of
        /// <paramref name="b"/>: a component of the result takes the component of the first value at the index of
        /// the pair of it and the component of the second value at the same index follows it
        /// <code>
        /// a (x0, y0, z0, w0)
        /// b (x1, y1, z1, w1)
        /// r (x0, x1, y0, y1)
        /// </code>
        /// </summary>
        /// <param name="a">The value whose low half leads the pairs of the result</param>
        /// <param name="b">The value whose low half follows the pairs of the result</param>
        /// <returns>The value of the two interlaced low halves</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 unpacklo(float4 a, float4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.UnpackLow(a.vector, b.vector));
            return new(a.x, b.x, a.y, b.y);
        }

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 unpacklo(double4 a, double4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.UnpackLow(a.vector, b.vector));
            return new(a.x, b.x, a.y, b.y);
        }

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 unpacklo(short4 a, short4 b) => new(a.x, b.x, a.y, b.y);

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 unpacklo(ushort4 a, ushort4 b) => new(a.x, b.x, a.y, b.y);

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 unpacklo(int4 a, int4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.UnpackLow(a.vector, b.vector));
            return new(a.x, b.x, a.y, b.y);
        }

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 unpacklo(uint4 a, uint4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.UnpackLow(a.vector, b.vector));
            return new(a.x, b.x, a.y, b.y);
        }

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 unpacklo(long4 a, long4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.UnpackLow(a.vector, b.vector));
            return new(a.x, b.x, a.y, b.y);
        }

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 unpacklo(ulong4 a, ulong4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.UnpackLow(a.vector, b.vector));
            return new(a.x, b.x, a.y, b.y);
        }

        /// <inheritdoc cref="unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 unpacklo(half4 a, half4 b) => new(a.x, b.x, a.y, b.y);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.unpacklo(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 unpacklo(this float4 a, float4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(double4, double4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 unpacklo(this double4 a, double4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(short4, short4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 unpacklo(this short4 a, short4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(ushort4, ushort4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 unpacklo(this ushort4 a, ushort4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(int4, int4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 unpacklo(this int4 a, int4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(uint4, uint4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 unpacklo(this uint4 a, uint4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(long4, long4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 unpacklo(this long4 a, long4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(ulong4, ulong4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 unpacklo(this ulong4 a, ulong4 b) => math.unpacklo(a, b);

        /// <inheritdoc cref="math.unpacklo(half4, half4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 unpacklo(this half4 a, half4 b) => math.unpacklo(a, b);
    }
}
