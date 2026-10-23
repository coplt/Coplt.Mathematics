namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the value whose low half holds the high half of <paramref name="b"/> and whose high half holds
        /// the high half of <paramref name="a"/>, which moves the high half of the second value to the low half of
        /// the first one
        /// <code>
        /// a (x0, y0, z0, w0)
        /// b (x1, y1, z1, w1)
        /// r (z1, w1, z0, w0)
        /// </code>
        /// </summary>
        /// <param name="a">The value whose high half the high half of the result holds</param>
        /// <param name="b">The value whose high half the low half of the result holds</param>
        /// <returns>The value of the two high halves</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 movehl(float4 a, float4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.MoveHighToLow(a.vector, b.vector));
            return new(b.z, b.w, a.z, a.w);
        }

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 movehl(double4 a, double4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.MoveHighToLow(a.vector, b.vector));
            return new(b.z, b.w, a.z, a.w);
        }

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 movehl(short4 a, short4 b) => new(b.z, b.w, a.z, a.w);

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 movehl(ushort4 a, ushort4 b) => new(b.z, b.w, a.z, a.w);

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 movehl(int4 a, int4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.MoveHighToLow(a.vector, b.vector));
            return new(b.z, b.w, a.z, a.w);
        }

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 movehl(uint4 a, uint4 b)
        {
            if (Vector128.IsHardwareAccelerated)
                return new(simd.MoveHighToLow(a.vector, b.vector));
            return new(b.z, b.w, a.z, a.w);
        }

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 movehl(long4 a, long4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.MoveHighToLow(a.vector, b.vector));
            return new(b.z, b.w, a.z, a.w);
        }

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 movehl(ulong4 a, ulong4 b)
        {
            if (Vector256.IsHardwareAccelerated)
                return new(simd.MoveHighToLow(a.vector, b.vector));
            return new(b.z, b.w, a.z, a.w);
        }

        /// <inheritdoc cref="movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 movehl(half4 a, half4 b) => new(b.z, b.w, a.z, a.w);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.movehl(float4, float4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 movehl(this float4 a, float4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(double4, double4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 movehl(this double4 a, double4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(short4, short4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short4 movehl(this short4 a, short4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(ushort4, ushort4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort4 movehl(this ushort4 a, ushort4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(int4, int4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 movehl(this int4 a, int4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(uint4, uint4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 movehl(this uint4 a, uint4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(long4, long4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long4 movehl(this long4 a, long4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(ulong4, ulong4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong4 movehl(this ulong4 a, ulong4 b) => math.movehl(a, b);

        /// <inheritdoc cref="math.movehl(half4, half4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4 movehl(this half4 a, half4 b) => math.movehl(a, b);
    }
}
