namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the value whose low half holds the low half of <paramref name="a"/> and whose high half holds the
        /// low half of <paramref name="b"/>, which moves the low half of the second value to the high half of the
        /// first one
        /// <code>
        /// a (x0, y0, z0, w0)
        /// b (x1, y1, z1, w1)
        /// r (x0, y0, x1, y1)
        /// </code>
        /// </summary>
        /// <param name="a">The value whose low half the low half of the result holds</param>
        /// <param name="b">The value whose low half the high half of the result holds</param>
        /// <returns>The value of the two low halves</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 movelh(float4 a, float4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.MoveLowToHigh(a.vector, b.vector));
            return new(a.x, a.y, b.x, b.y);
        }

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 movelh(double4 a, double4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.MoveLowToHigh(a.vector, b.vector));
            return new(a.x, a.y, b.x, b.y);
        }

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 movelh(short4 a, short4 b) => new(a.x, a.y, b.x, b.y);

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 movelh(ushort4 a, ushort4 b) => new(a.x, a.y, b.x, b.y);

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 movelh(int4 a, int4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.MoveLowToHigh(a.vector, b.vector));
            return new(a.x, a.y, b.x, b.y);
        }

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 movelh(uint4 a, uint4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.MoveLowToHigh(a.vector, b.vector));
            return new(a.x, a.y, b.x, b.y);
        }

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 movelh(long4 a, long4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.MoveLowToHigh(a.vector, b.vector));
            return new(a.x, a.y, b.x, b.y);
        }

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 movelh(ulong4 a, ulong4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.MoveLowToHigh(a.vector, b.vector));
            return new(a.x, a.y, b.x, b.y);
        }

        /// <inheritdoc cref="movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 movelh(half4 a, half4 b) => new(a.x, a.y, b.x, b.y);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.movelh(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 movelh(this float4 a, float4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(double4, double4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 movelh(this double4 a, double4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(short4, short4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 movelh(this short4 a, short4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(ushort4, ushort4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 movelh(this ushort4 a, ushort4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(int4, int4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 movelh(this int4 a, int4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(uint4, uint4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 movelh(this uint4 a, uint4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(long4, long4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 movelh(this long4 a, long4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(ulong4, ulong4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 movelh(this ulong4 a, ulong4 b) => math.movelh(a, b);

        /// <inheritdoc cref="math.movelh(half4, half4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 movelh(this half4 a, half4 b) => math.movelh(a, b);
    }
}
