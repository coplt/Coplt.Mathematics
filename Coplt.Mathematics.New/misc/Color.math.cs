namespace Coplt.Mathematics;

public static partial class ex_color
{
    extension(math)
    {
        /// <summary>
        /// Returns the color whose every component is the component of one of the two colors that the mask of the
        /// same component reaches
        /// <para>The mask is a vector of the kind of the components of the colors: a component of it that is the
        /// all bits set value of the kind takes the component of <paramref name="a"/> and a component of it that is
        /// the all bits zero value of it takes the component of <paramref name="b"/>.</para>
        /// </summary>
        /// <param name="condition">The mask of the components to select</param>
        /// <param name="a">The color whose components the mask takes where it holds</param>
        /// <param name="b">The color whose components the mask takes where it does not hold</param>
        /// <returns>The color of the components that the mask selects</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color select(float4 condition, Color a, Color b)
            => new(a.rgba.select(condition, b.rgba));

        /// <summary>Returns the color whose every component is the absolute value of the component of the value at the index of it</summary>
        /// <param name="a">The value</param>
        /// <returns>The color whose every component is the absolute value of the component of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color abs(Color a) => new(a.rgba.abs());

        /// <summary>Returns <c>-1</c>, <c>0</c> or <c>1</c> for every component of the color depending on the sign of it</summary>
        /// <param name="a">The value</param>
        /// <returns>The vector whose every component is the sign of the component of the color at the index of it</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 sign(Color a) => a.rgba.sign();

        /// <summary>Returns the color whose every component is the smaller of the components of the two values at the index of it</summary>
        /// <param name="a">The first of the two values</param>
        /// <param name="b">The second of the two values</param>
        /// <returns>The color whose every component is the smaller of the two components</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color min(Color a, Color b) => new(a.rgba.min(b.rgba));

        /// <summary>Returns the color whose every component is the larger of the components of the two values at the index of it</summary>
        /// <param name="a">The first of the two values</param>
        /// <param name="b">The second of the two values</param>
        /// <returns>The color whose every component is the larger of the two components</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color max(Color a, Color b) => new(a.rgba.max(b.rgba));

        /// <summary>Returns the color whose every component is the component of the value at the index of it clamped into the range of the components of the two other values at the index of it</summary>
        /// <param name="value">The value</param>
        /// <param name="min">The color of the smallest components of the range</param>
        /// <param name="max">The color of the largest components of the range</param>
        /// <returns>The color whose every component is within the range of the two colors</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color clamp(Color value, Color min, Color max)
            => new(value.rgba.clamp(min.rgba, max.rgba));

        /// <summary>Returns the color whose every component is the component of the value at the index of it clamped into the range of the two values, which are the same for every channel</summary>
        /// <param name="value">The value</param>
        /// <param name="min">The smallest value of the range</param>
        /// <param name="max">The largest value of the range</param>
        /// <returns>The color whose every component is within the range of the two values</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color clamp(Color value, float min, float max)
            => new(value.rgba.clamp(new float4(min), new float4(max)));

        /// <summary>Returns the color of the interpolation between the two colors that the two vectors hold, which every component of the value scales</summary>
        /// <param name="start">The color that the interpolation starts from</param>
        /// <param name="end">The color that the interpolation reaches</param>
        /// <param name="t">The position of the interpolation, whose every component is the position of the component of the result at the index of it</param>
        /// <returns>The color of the interpolation at the position of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color lerp(float4 start, float4 end, Color t)
            => new(t.rgba.lerp(start, end));

        /// <summary>Returns the color of the interpolation between the two values, which are the same for every channel and which every component of the value scales</summary>
        /// <param name="start">The value that the interpolation starts from</param>
        /// <param name="end">The value that the interpolation reaches</param>
        /// <param name="t">The position of the interpolation, whose every component is the position of the component of the result at the index of it</param>
        /// <returns>The color of the interpolation at the position of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color lerp(float start, float end, Color t)
            => new(t.rgba.lerp(start, end));

        /// <summary>Returns the color whose every component is the position of the component of the value at the index of it between the components of the two vectors at the index of it</summary>
        /// <param name="value">The value</param>
        /// <param name="start">The value that the range starts at</param>
        /// <param name="end">The value that the range reaches</param>
        /// <returns>The color of the positions of the components of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color unlerp(Color value, float4 start, float4 end)
            => new(value.rgba.unlerp(start, end));

        /// <summary>Returns the color whose every component is the position of the component of the value at the index of it between the two values, which are the same for every channel</summary>
        /// <param name="value">The value</param>
        /// <param name="start">The value that the range starts at</param>
        /// <param name="end">The value that the range reaches</param>
        /// <returns>The color of the positions of the components of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color unlerp(Color value, float start, float end)
            => new(value.rgba.unlerp(new float4(start), new float4(end)));

        /// <summary>Returns the color whose every component is the component of the value at the index of it taken from the range of the source into the range of the destination</summary>
        /// <param name="value">The value</param>
        /// <param name="src_start">The value that the source range starts at</param>
        /// <param name="src_end">The value that the source range reaches</param>
        /// <param name="dst_start">The value that the destination range starts at</param>
        /// <param name="dst_end">The value that the destination range reaches</param>
        /// <returns>The color whose every component is remapped out of the source range into the destination range</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color remap(
            Color value, float4 src_start, float4 src_end, float4 dst_start, float4 dst_end
        ) => new(value.rgba.remap(src_start, src_end, dst_start, dst_end));

        /// <summary>Returns the color whose every component is the component of the value at the index of it wrapped into the range of the components of the two other values at the index of it</summary>
        /// <param name="value">The value</param>
        /// <param name="min">The color of the smallest components of the range</param>
        /// <param name="max">The color of the largest components of the range</param>
        /// <returns>The color whose every component is within the range of the two colors</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color wrap(Color value, float4 min, float4 max)
            => new(value.rgba.wrap(min, max));

        /// <summary>Returns the color whose every component is the component of the value at the index of it wrapped into the range of the two values, which are the same for every channel</summary>
        /// <param name="value">The value</param>
        /// <param name="min">The smallest value of the range</param>
        /// <param name="max">The largest value of the range</param>
        /// <returns>The color whose every component is within the range of the two values</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color wrap(Color value, float min, float max)
            => new(value.rgba.wrap(min, max));

        /// <summary>Returns the color whose every component is the component of the value at the index of it clamped into the range of the zero of the kind and the one of it</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is within the range of the zero of the kind and the one of it</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color saturate(Color value) => new(value.rgba.saturate());

        /// <summary>Returns the color of the smooth step of the components of the value between the components of the two vectors at the index of it</summary>
        /// <param name="min">The value that the range starts at</param>
        /// <param name="max">The value that the range reaches</param>
        /// <param name="value">The value</param>
        /// <returns>The color of the smooth step of the components of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color smoothstep(float4 min, float4 max, Color value)
            => new(value.rgba.smoothstep(min, max));

        /// <summary>Returns the color whose every component is the reciprocal of the component of the value at the index of it</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is the reciprocal of the component of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color rcp(Color value) => new(value.rgba.rcp());

        /// <summary>Returns the color whose every component is the fractional part of the component of the value at the index of it</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is the fractional part of the component of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color frac(Color value) => new(value.rgba.frac());

        /// <summary>Returns the color whose every component is the component of the value at the index of it without the fractional part of it</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is the integral part of the component of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color trunc(Color value) => new(value.rgba.trunc());

        /// <summary>Returns the color whose every component is the component of the value at the index of it rounded to the nearest integral value</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is the component of the value rounded</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color round(Color value) => new(value.rgba.round());

        /// <summary>Returns the color whose every component is the largest integral value that is not above the component of the value at the index of it</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is the floor of the component of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color floor(Color value) => new(value.rgba.floor());

        /// <summary>Returns the color whose every component is the smallest integral value that is not below the component of the value at the index of it</summary>
        /// <param name="value">The value</param>
        /// <returns>The color whose every component is the ceiling of the component of the value</returns>
        [OverloadResolutionPriority(-1)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ceil(Color value) => new(value.rgba.ceil());
    }
}
