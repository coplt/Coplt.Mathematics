using System.Numerics;

namespace Coplt.Mathematics;

/// <summary>
/// A color of the red, green, blue and alpha channels, which holds the value of every one of the four channels
/// <para>A color is the value of the three channels that a display mixes beside the opacity of the color, which is
/// the value of the alpha channel of it: the zero of the kind is a channel that holds nothing and the one of the
/// kind is a channel that holds the whole of its value, so a color whose components are within the range of the
/// zero of the kind and the one of it is a color that a display holds as it is.</para>
/// <para>A member that reads a color does not clamp the components of it, so a computation that leaves that range
/// reaches <c>math.saturate</c> for the color that a display holds.</para>
/// </summary>
[Serializable]
public struct Color :
    IEquatable<Color>,
    IEqualityOperators<Color, Color, bool>,
    IComparisonOperators<Color, Color, float4>
{
    /// <summary>
    /// The value of the red, green, blue and alpha channels of the color, in that order
    /// </summary>
    public float4 rgba;

    /// <summary>The value of the red channel, which is the first component of <see cref="rgba"/></summary>
    public float r
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.r;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.r = value;
    }

    /// <summary>The value of the green channel, which is the second component of <see cref="rgba"/></summary>
    public float g
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.g;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.g = value;
    }

    /// <summary>The value of the blue channel, which is the third component of <see cref="rgba"/></summary>
    public float b
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.b = value;
    }

    /// <summary>The value of the alpha channel, which is the fourth component of <see cref="rgba"/></summary>
    public float a
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.a;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.a = value;
    }

    /// <summary>
    /// Constructs the color of the four components of the value
    /// </summary>
    /// <param name="rgba">The value of the red, green, blue and alpha channels of the color, in that order</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float4 rgba) => this.rgba = rgba;

    /// <summary>
    /// Constructs the color of the three components of the channels of it that the value holds and the opacity of
    /// it
    /// </summary>
    /// <param name="rgb">The value of the red, green and blue channels of the color, in that order</param>
    /// <param name="a">The value of the alpha channel, which is the one of the kind where the caller does not name
    /// it</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float3 rgb, float a = 1f) : this(new float4(rgb, a))
    {
    }

    /// <summary>
    /// Constructs the color of the four values of the channels of it
    /// </summary>
    /// <param name="r">The value of the red channel</param>
    /// <param name="g">The value of the green channel</param>
    /// <param name="b">The value of the blue channel</param>
    /// <param name="a">The value of the alpha channel, which is the one of the kind where the caller does not name
    /// it</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float r, float g, float b, float a = 1f) : this(new float4(r, g, b, a))
    {
    }

    /// <summary>
    /// Returns the color of the four components of a byte color, which is the color whose every component is the
    /// component of the byte color at the index of it divided by <c>255</c>
    /// <para>A byte color holds the value of a channel in a byte, whose <c>255</c> is the whole of the value of the
    /// channel and whose zero is the channel that holds nothing, so the color of a byte color is a color whose
    /// components are within the range of the zero of the kind and the one of it.</para>
    /// </summary>
    /// <param name="r">The value of the red channel, a byte</param>
    /// <param name="g">The value of the green channel, a byte</param>
    /// <param name="b">The value of the blue channel, a byte</param>
    /// <param name="a">The value of the alpha channel, a byte, which is <c>255</c> where the caller does not name
    /// it</param>
    /// <returns>The color of the components of the byte color divided by <c>255</c></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color FromByte(byte r, byte g, byte b, byte a = 255)
        => new(new float4(r, g, b, a) * (1f / 255f));

    #region Hsv

    /// <summary>
    /// Returns the color of a hue, a saturation and a value of the channels of it, beside the opacity of it
    /// <para>The hue is a fraction of a full turn of the circle of the hues: the zero of the kind is the red hue,
    /// the third of the kind is the green one, the two thirds of it are the blue one and the one of the kind is
    /// the red one again. The member turns a hue that is not within the range of the zero of the kind and the one
    /// of it around the circle, so the hue of a fraction that the whole turns of the value leave out is the one of
    /// the fraction that it keeps.</para>
    /// <para>The saturation is the fraction of the hue that the color holds: the zero of the kind is the gray of
    /// the value of the color and the one of the kind is the hue itself. The value is the largest component of the
    /// color, so the zero of the kind is the black color.</para>
    /// </summary>
    /// <param name="h">The hue, a fraction of a full turn of the circle of the hues</param>
    /// <param name="s">The saturation, the fraction of the hue that the color holds</param>
    /// <param name="v">The value, the largest component of the color</param>
    /// <param name="a">The value of the alpha channel, which is the one of the kind where the caller does not name
    /// it</param>
    /// <returns>The color of the hue, the saturation and the value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Hsv(float h, float s, float v, float a = 1f)
        => new(HsvToRgb(h, s, v), a);

    /// <inheritdoc cref="Hsv(float, float, float, float)"/>
    /// <param name="hsv">The hue, the saturation and the value of the color, in that order</param>
    /// <param name="a">The value of the alpha channel, which is the one of the kind where the caller does not name
    /// it</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Hsv(float3 hsv, float a = 1f)
        => new(HsvToRgb(hsv.x, hsv.y, hsv.z), a);

    /// <inheritdoc cref="Hsv(float, float, float, float)"/>
    /// <param name="hsva">The hue, the saturation, the value and the value of the alpha channel, in that
    /// order</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Hsv(float4 hsva)
        => new(HsvToRgb(hsva.x, hsva.y, hsva.z), hsva.w);

    private static float3 HsvToRgb(float h, float s, float v)
    {
        // the hue is a fraction of a full turn, which the member turns around the circle of the hues so that a
        // value outside of the range of the zero of the kind and the one of it reaches the hue of the fraction
        // that the whole turns of it leave out
        h = math.frac(h);
        // the six sectors of the circle of the hues hold the red hue, the yellow one, the green one, the cyan
        // one, the blue one and the magenta one in that order, and the fraction within a sector blends the two
        // hues that meet in the middle of it
        var sector_value = h * 6f;
        var sector = Math.Min(5, (int)MathF.Floor(sector_value));
        var f = sector_value - sector;

        var p = v * (1f - s);
        var q = v * (1f - s * f);
        var t = v * (1f - s * (1f - f));
        return sector switch
        {
            0 => new float3(v, t, p),
            1 => new float3(q, v, p),
            2 => new float3(p, v, t),
            3 => new float3(p, q, v),
            4 => new float3(t, p, v),
            _ => new float3(v, p, q),
        };
    }

    /// <summary>
    /// The hue, the saturation, the value and the value of the alpha channel of the color, in that order
    /// <para>The value of the color is the largest of its three components, the saturation is the difference of
    /// the largest component and the smallest one divided by the largest one, and the hue is the position of the
    /// largest component within the circle of the hues, which is a fraction of a full turn of it.</para>
    /// <para>A color whose three components are the same value has a saturation of the zero of the kind and a hue
    /// of it as well, so the hue of a gray color is not a hue of the circle of the hues.</para>
    /// </summary>
    public readonly float4 ToHsv
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var c = rgba;
            const float epsilon = 1e-10f;
            var max = MathF.Max(c.r, MathF.Max(c.g, c.b));
            var min = MathF.Min(c.r, MathF.Min(c.g, c.b));
            var delta = max - min;
            if (delta <= epsilon) return new float4(0f, 0f, max, c.a);

            var hue = max == c.r
                ? (c.g - c.b) / delta
                : max == c.g
                    ? (c.b - c.r) / delta + 2f
                    : (c.r - c.g) / delta + 4f;
            hue /= 6f;
            if (hue < 0f) hue += 1f;
            else if (hue >= 1f) hue -= 1f;

            var saturation = max > epsilon ? delta / max : 0f;
            return new float4(hue, saturation, max, c.a);
        }
    }

    #endregion

    #region Predefined

    /// <summary>The color that holds nothing of any channel, which is <c>(0, 0, 0, 0)</c></summary>
    public static readonly Color Transparent = new(float4.Zero);
    /// <summary>The black color, which is <c>(0, 0, 0)</c> of the full opacity</summary>
    public static readonly Color Black = new(0f, 0f, 0f);
    /// <summary>The gray color of the value <c>0.75</c>, which is <c>(0.75, 0.75, 0.75)</c> of the full opacity</summary>
    public static readonly Color Silver = new(0.75f, 0.75f, 0.75f);
    /// <summary>The gray color of the value <c>0.5</c>, which is <c>(0.5, 0.5, 0.5)</c> of the full opacity</summary>
    public static readonly Color Gray = new(0.5f, 0.5f, 0.5f);
    /// <summary>The white color, which is <c>(1, 1, 1)</c> of the full opacity</summary>
    public static readonly Color White = new(float4.One);
    /// <summary>The dark red color, which is <c>(0.5, 0, 0)</c> of the full opacity</summary>
    public static readonly Color Maroon = new(0.5f, 0f, 0f);
    /// <summary>The red color, which is <c>(1, 0, 0)</c> of the full opacity</summary>
    public static readonly Color Red = new(1f, 0f, 0f);
    /// <summary>The dark magenta color, which is <c>(0.5, 0, 0.5)</c> of the full opacity</summary>
    public static readonly Color Purple = new(0.5f, 0f, 0.5f);
    /// <summary>The magenta color, which is <c>(1, 0, 1)</c> of the full opacity</summary>
    public static readonly Color Fuchsia = new(1f, 0f, 1f);
    /// <summary>The dark green color, which is <c>(0, 0.5, 0)</c> of the full opacity</summary>
    public static readonly Color Green = new(0f, 0.5f, 0f);
    /// <summary>The green color, which is <c>(0, 1, 0)</c> of the full opacity</summary>
    public static readonly Color Lime = new(0f, 1f, 0f);
    /// <summary>The dark yellow color, which is <c>(0.5, 0.5, 0)</c> of the full opacity</summary>
    public static readonly Color Olive = new(0.5f, 0.5f, 0f);
    /// <summary>The yellow color, which is <c>(1, 1, 0)</c> of the full opacity</summary>
    public static readonly Color Yellow = new(1f, 1f, 0f);
    /// <summary>The dark blue color, which is <c>(0, 0, 0.5)</c> of the full opacity</summary>
    public static readonly Color Navy = new(0f, 0f, 0.5f);
    /// <summary>The blue color, which is <c>(0, 0, 1)</c> of the full opacity</summary>
    public static readonly Color Blue = new(0f, 0f, 1f);
    /// <summary>The dark cyan color, which is <c>(0, 0.5, 0.5)</c> of the full opacity</summary>
    public static readonly Color Teal = new(0f, 0.5f, 0.5f);
    /// <summary>The cyan color, which is <c>(0, 1, 1)</c> of the full opacity</summary>
    public static readonly Color Aqua = new(0f, 1f, 1f);

    #endregion

    #region Operators

    /// <summary>Returns the color of the four components of the vector</summary>
    /// <param name="color">The value of the red, green, blue and alpha channels, in that order</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Color(float4 color) => new(color);

    /// <summary>Returns the vector of the four components of the color</summary>
    /// <param name="color">The color</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float4(Color color) => color.rgba;

    /// <summary>Returns the color itself</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a) => a;

    /// <summary>Returns the color whose every component is the component of the value at the index of it negated</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a) => new(-a.rgba);

    /// <summary>Returns the color whose every component is the sum of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a, Color b) => new(a.rgba + b.rgba);

    /// <summary>Returns the color whose every component is the component of the value at the index of it beside the value of the other one, which is the same for every channel</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a, float b) => new(a.rgba + new float4(b));

    /// <summary>Returns the color whose every component is the value of the first operand beside the component of the second one at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(float a, Color b) => new(new float4(a) + b.rgba);

    /// <summary>Returns the color whose every component is the difference of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a, Color b) => new(a.rgba - b.rgba);

    /// <summary>Returns the color whose every component is the component of the value at the index of it less the value of the other one</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a, float b) => new(a.rgba - new float4(b));

    /// <summary>Returns the color whose every component is the value of the first operand less the component of the second one at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(float a, Color b) => new(new float4(a) - b.rgba);

    /// <summary>Returns the color whose every component is the product of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color a, Color b) => new(a.rgba * b.rgba);

    /// <summary>Returns the color whose every component is the component of the value at the index of it times the value of the other one</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color a, float b) => new(a.rgba * b);

    /// <summary>Returns the color whose every component is the value of the first operand times the component of the second one at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(float a, Color b) => new(a * b.rgba);

    /// <summary>Returns the color whose every component is the quotient of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(Color a, Color b) => new(a.rgba / b.rgba);

    /// <summary>Returns the color whose every component is the component of the value at the index of it divided by the value of the other one</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(Color a, float b) => new(a.rgba / new float4(b));

    /// <summary>Returns the color whose every component is the value of the first operand divided by the component of the second one at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(float a, Color b) => new(new float4(a) / b.rgba);

    /// <summary>Returns the color whose every component is the remainder of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator %(Color a, Color b) => new(a.rgba % b.rgba);

    /// <summary>Returns the color whose every component is the remainder of the component of the value at the index of it and the value of the other one</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator %(Color a, float b) => new(a.rgba % new float4(b));

    /// <summary>Returns the color whose every component is the remainder of the value of the first operand and the component of the second one at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator %(float a, Color b) => new(new float4(a) % b.rgba);

    /// <summary>Returns the color whose every component is the bits of the component of the value at the index of it complemented</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator ~(Color a) => new(~a.rgba);

    /// <summary>Returns the color whose every component is the conjunction of the bits of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator &(Color a, Color b) => new(a.rgba & b.rgba);

    /// <summary>Returns the color whose every component is the disjunction of the bits of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator |(Color a, Color b) => new(a.rgba | b.rgba);

    /// <summary>Returns the color whose every component is the exclusive disjunction of the bits of the components of the two values at the index of it</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator ^(Color a, Color b) => new(a.rgba ^ b.rgba);

    /// <summary>Returns the color whose every component is the bits of the component of the value at the index of it shifted to the left by the number of the bits that the other operand names</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator <<(Color a, int b) => new(a.rgba << b);

    /// <summary>Returns the color whose every component is the bits of the component of the value at the index of it shifted to the right by the number of the bits that the other operand names</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator >> (Color a, int b) => new(a.rgba >> b);

    /// <summary>Returns the color whose every component is the bits of the component of the value at the index of it shifted to the right by the number of the bits that the other operand names, which fills the bits that it leaves with the zero of the kind</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator >>> (Color a, int b) => new(a.rgba >>> b);

    /// <summary>Returns the mask of the components of the two colors that are equal, which is the all bits set value of the kind in a component that holds and the zero of the kind in a component that does not</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator ==(Color a, Color b) => a.rgba == b.rgba;

    /// <summary>Returns the mask of the components of the two colors that are not equal, which is the all bits set value of the kind in a component that holds and the zero of the kind in a component that does not</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator !=(Color a, Color b) => a.rgba != b.rgba;

    /// <summary>Returns the mask of the components of the first value that are greater than the components of the second one at the index of them</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator >(Color a, Color b) => a.rgba > b.rgba;

    /// <summary>Returns the mask of the components of the first value that are less than the components of the second one at the index of them</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator <(Color a, Color b) => a.rgba < b.rgba;

    /// <summary>Returns the mask of the components of the first value that are greater than the components of the second one at the index of them or equal to them</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator >=(Color a, Color b) => a.rgba >= b.rgba;

    /// <summary>Returns the mask of the components of the first value that are less than the components of the second one at the index of them or equal to them</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator <=(Color a, Color b) => a.rgba <= b.rgba;

    /// <inheritdoc/>
    static bool IEqualityOperators<Color, Color, bool>.operator ==(Color left, Color right)
        => left.Equals(right);

    /// <inheritdoc/>
    static bool IEqualityOperators<Color, Color, bool>.operator !=(Color left, Color right)
        => !left.Equals(right);

    #endregion

    #region Equality

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Color other) => rgba.Equals(other.rgba);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override bool Equals(object? obj) => obj is Color other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode() => rgba.GetHashCode();

    #endregion

    /// <summary>
    /// Returns the string of the four components of the color, which is <c>Color(r, g, b, a)</c> where the four of
    /// them are the components of the color in the order of the channels
    /// </summary>
    public readonly override string ToString() => $"Color({r}, {g}, {b}, {a})";
}
