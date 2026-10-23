namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the value whose high half interlaces the high half of <paramref name="a"/> with the high half of
        /// <paramref name="b"/>: a component of the result takes the component of the first value at the index of
        /// the pair of it and the component of the second value at the same index follows it
        /// <code>
        /// a (x0, y0, z0, w0)
        /// b (x1, y1, z1, w1)
        /// r (z0, z1, w0, w1)
        /// </code>
        /// </summary>
        /// <param name="a">The value whose high half leads the pairs of the result</param>
        /// <param name="b">The value whose high half follows the pairs of the result</param>
        /// <returns>The value of the two interlaced high halves</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 unpackhi(float4 a, float4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.UnpackHigh(a.vector, b.vector));
            return new(a.z, b.z, a.w, b.w);
        }

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 unpackhi(double4 a, double4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.UnpackHigh(a.vector, b.vector));
            return new(a.z, b.z, a.w, b.w);
        }

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 unpackhi(short4 a, short4 b) => new(a.z, b.z, a.w, b.w);

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 unpackhi(ushort4 a, ushort4 b) => new(a.z, b.z, a.w, b.w);

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 unpackhi(int4 a, int4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.UnpackHigh(a.vector, b.vector));
            return new(a.z, b.z, a.w, b.w);
        }

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 unpackhi(uint4 a, uint4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.UnpackHigh(a.vector, b.vector));
            return new(a.z, b.z, a.w, b.w);
        }

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 unpackhi(long4 a, long4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.UnpackHigh(a.vector, b.vector));
            return new(a.z, b.z, a.w, b.w);
        }

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 unpackhi(ulong4 a, ulong4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.UnpackHigh(a.vector, b.vector));
            return new(a.z, b.z, a.w, b.w);
        }

        /// <inheritdoc cref="unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 unpackhi(half4 a, half4 b) => new(a.z, b.z, a.w, b.w);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.unpackhi(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 unpackhi(this float4 a, float4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(double4, double4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 unpackhi(this double4 a, double4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(short4, short4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 unpackhi(this short4 a, short4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(ushort4, ushort4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 unpackhi(this ushort4 a, ushort4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(int4, int4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 unpackhi(this int4 a, int4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(uint4, uint4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 unpackhi(this uint4 a, uint4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(long4, long4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 unpackhi(this long4 a, long4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(ulong4, ulong4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 unpackhi(this ulong4 a, ulong4 b) => math.unpackhi(a, b);

        /// <inheritdoc cref="math.unpackhi(half4, half4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 unpackhi(this half4 a, half4 b) => math.unpackhi(a, b);
    }
}
