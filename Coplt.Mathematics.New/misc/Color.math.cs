namespace Coplt.Mathematics;

public static partial class ex_color
{
    extension(math)
    {
        /// <summary>Selects the components of two colors with a mask</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color select(float4 condition, Color a, Color b)
            => new(a.rgba.select(condition, b.rgba));

        /// <summary>Returns the absolute value of every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color abs(Color a) => new(a.rgba.abs());

        /// <summary>Returns the sign of every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 sign(Color a) => a.rgba.sign();

        /// <summary>Returns the smaller component of the two colors at every component</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color min(Color a, Color b) => new(a.rgba.min(b.rgba));

        /// <summary>Returns the larger component of the two colors at every component</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color max(Color a, Color b) => new(a.rgba.max(b.rgba));

        /// <summary>Clamps every component of the color into the range of the two colors</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color clamp(Color value, Color min, Color max)
            => new(value.rgba.clamp(min.rgba, max.rgba));

        /// <summary>Clamps every component of the color into the range of the two values</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color clamp(Color value, float min, float max)
            => new(value.rgba.clamp(new float4(min), new float4(max)));

        /// <summary>Interpolates between two vectors with the components of a color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color lerp(float4 start, float4 end, Color t)
            => new(t.rgba.lerp(start, end));

        /// <summary>Interpolates between two values with the components of a color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color lerp(float start, float end, Color t)
            => new(t.rgba.lerp(start, end));

        /// <summary>Returns the position of a color between two vectors</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color unlerp(Color value, float4 start, float4 end)
            => new(value.rgba.unlerp(start, end));

        /// <summary>Returns the position of a color between two values</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color unlerp(Color value, float start, float end)
            => new(value.rgba.unlerp(new float4(start), new float4(end)));

        /// <summary>Remaps every component of the color from one range into another</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color remap(
            Color value, float4 src_start, float4 src_end, float4 dst_start, float4 dst_end
        ) => new(value.rgba.remap(src_start, src_end, dst_start, dst_end));

        /// <summary>Wraps every component of the color into the range of the two vectors</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color wrap(Color value, float4 min, float4 max)
            => new(value.rgba.wrap(min, max));

        /// <summary>Wraps every component of the color into the range of the two values</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color wrap(Color value, float min, float max)
            => new(value.rgba.wrap(min, max));

        /// <summary>Clamps every component of the color into the range of zero and one</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color saturate(Color value) => new(value.rgba.saturate());

        /// <summary>Returns the smooth step of every component of the color between the two vectors</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color smoothstep(float4 min, float4 max, Color value)
            => new(value.rgba.smoothstep(min, max));

        /// <summary>Returns the reciprocal of every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color rcp(Color value) => new(value.rgba.rcp());

        /// <summary>Returns the fractional part of every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color frac(Color value) => new(value.rgba.frac());

        /// <summary>Truncates every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color trunc(Color value) => new(value.rgba.trunc());

        /// <summary>Rounds every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color round(Color value) => new(value.rgba.round());

        /// <summary>Returns the floor of every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color floor(Color value) => new(value.rgba.floor());

        /// <summary>Returns the ceiling of every component of the color</summary>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ceil(Color value) => new(value.rgba.ceil());
    }
}
