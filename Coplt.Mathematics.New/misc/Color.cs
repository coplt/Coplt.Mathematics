using System.Numerics;

namespace Coplt.Mathematics;

/// <summary>
/// A color whose red, green, blue and alpha components are floating point values
/// <para>The components follow the convention of red, green and blue color channels: zero is the smallest value
/// of a channel and one is the largest one. The alpha component is the opacity of the color.</para>
/// </summary>
[Serializable]
public struct Color :
    IEquatable<Color>,
    IEqualityOperators<Color, Color, bool>,
    IComparisonOperators<Color, Color, float4>
{
    /// <summary>
    /// The red, green, blue and alpha components, in that order
    /// </summary>
    public float4 rgba;

    /// <summary>The red component</summary>
    public float r
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.r;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.r = value;
    }

    /// <summary>The green component</summary>
    public float g
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.g;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.g = value;
    }

    /// <summary>The blue component</summary>
    public float b
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.b = value;
    }

    /// <summary>The alpha component</summary>
    public float a
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => rgba.a;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => rgba.a = value;
    }

    /// <summary>
    /// Creates a color from its red, green, blue and alpha components
    /// </summary>
    /// <param name="rgba">The red, green, blue and alpha components, in that order</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float4 rgba) => this.rgba = rgba;

    /// <summary>
    /// Creates a color from its red, green and blue components and an alpha component
    /// </summary>
    /// <param name="rgb">The red, green and blue components, in that order</param>
    /// <param name="a">The alpha component, whose default value is one</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float3 rgb, float a = 1f) : this(new float4(rgb, a)) { }

    /// <summary>
    /// Creates a color from its red, green, blue and alpha components
    /// </summary>
    /// <param name="r">The red component</param>
    /// <param name="g">The green component</param>
    /// <param name="b">The blue component</param>
    /// <param name="a">The alpha component, whose default value is one</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float r, float g, float b, float a = 1f) : this(new float4(r, g, b, a)) { }

    /// <summary>
    /// Creates a color from the components of a byte color, where 255 is the full value of a channel
    /// </summary>
    /// <param name="r">The red component</param>
    /// <param name="g">The green component</param>
    /// <param name="b">The blue component</param>
    /// <param name="a">The alpha component, whose default value is 255</param>
    /// <returns>The color whose components are the components of the byte color divided by 255</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color FromByte(byte r, byte g, byte b, byte a = 255)
        => new(new float4(r, g, b, a) * (1f / 255f));

    #region Hsv

    /// <summary>
    /// Creates a color from a hue, saturation, value and alpha
    /// <para>The hue is a fraction of a full turn: zero is red, one third is green, two thirds is blue and one is
    /// red again. The saturation and value are normally in the range of zero and one.</para>
    /// </summary>
    /// <param name="h">The hue</param>
    /// <param name="s">The saturation</param>
    /// <param name="v">The value</param>
    /// <param name="a">The alpha component</param>
    /// <returns>The color</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Hsv(float h, float s, float v, float a = 1f)
        => new(HsvToRgb(h, s, v), a);

    /// <summary>
    /// Creates a color from a hue, saturation, value and alpha
    /// </summary>
    /// <param name="hsv">The hue, saturation and value components, in that order</param>
    /// <param name="a">The alpha component</param>
    /// <returns>The color</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Hsv(float3 hsv, float a = 1f)
        => new(HsvToRgb(hsv.x, hsv.y, hsv.z), a);

    /// <summary>
    /// Creates a color from a hue, saturation, value and alpha
    /// </summary>
    /// <param name="hsva">The hue, saturation, value and alpha components, in that order</param>
    /// <returns>The color</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Hsv(float4 hsva)
        => new(HsvToRgb(hsva.x, hsva.y, hsva.z), hsva.w);

    private static float3 HsvToRgb(float h, float s, float v)
    {
        // A hue is a fraction of a full turn; wrapping it keeps the conversion valid for values outside the
        // conventional range of zero and one as well.
        h = math.frac(h);
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
    /// The hue, saturation, value and alpha components of the color, in that order
    /// <para>The hue is a fraction of a full turn. A color without saturation has a hue of zero.</para>
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

    public static readonly Color Transparent = new(float4.Zero);
    public static readonly Color Black = new(0f, 0f, 0f);
    public static readonly Color Silver = new(0.75f, 0.75f, 0.75f);
    public static readonly Color Gray = new(0.5f, 0.5f, 0.5f);
    public static readonly Color White = new(float4.One);
    public static readonly Color Maroon = new(0.5f, 0f, 0f);
    public static readonly Color Red = new(1f, 0f, 0f);
    public static readonly Color Purple = new(0.5f, 0f, 0.5f);
    public static readonly Color Fuchsia = new(1f, 0f, 1f);
    public static readonly Color Green = new(0f, 0.5f, 0f);
    public static readonly Color Lime = new(0f, 1f, 0f);
    public static readonly Color Olive = new(0.5f, 0.5f, 0f);
    public static readonly Color Yellow = new(1f, 1f, 0f);
    public static readonly Color Navy = new(0f, 0f, 0.5f);
    public static readonly Color Blue = new(0f, 0f, 1f);
    public static readonly Color Teal = new(0f, 0.5f, 0.5f);
    public static readonly Color Aqua = new(0f, 1f, 1f);

    #endregion

    #region Operators

    /// <summary>Converts a vector to the color of its components</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Color(float4 color) => new(color);

    /// <summary>Converts a color to the vector of its components</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float4(Color color) => color.rgba;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a) => a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a) => new(-a.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a, Color b) => new(a.rgba + b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a, float b) => new(a.rgba + new float4(b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(float a, Color b) => new(new float4(a) + b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a, Color b) => new(a.rgba - b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a, float b) => new(a.rgba - new float4(b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(float a, Color b) => new(new float4(a) - b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color a, Color b) => new(a.rgba * b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color a, float b) => new(a.rgba * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(float a, Color b) => new(a * b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(Color a, Color b) => new(a.rgba / b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(Color a, float b) => new(a.rgba / new float4(b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(float a, Color b) => new(new float4(a) / b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator %(Color a, Color b) => new(a.rgba % b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator %(Color a, float b) => new(a.rgba % new float4(b));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator %(float a, Color b) => new(new float4(a) % b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator ~(Color a) => new(~a.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator &(Color a, Color b) => new(a.rgba & b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator |(Color a, Color b) => new(a.rgba | b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator ^(Color a, Color b) => new(a.rgba ^ b.rgba);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator <<(Color a, int b) => new(a.rgba << b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator >>(Color a, int b) => new(a.rgba >> b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator >>>(Color a, int b) => new(a.rgba >>> b);

    /// <summary>Returns a component wise mask of the equality of the two colors</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator ==(Color a, Color b) => a.rgba == b.rgba;

    /// <summary>Returns a component wise mask of the inequality of the two colors</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator !=(Color a, Color b) => a.rgba != b.rgba;

    /// <summary>Returns a component wise mask of the greater comparison of the two colors</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator >(Color a, Color b) => a.rgba > b.rgba;

    /// <summary>Returns a component wise mask of the less comparison of the two colors</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator <(Color a, Color b) => a.rgba < b.rgba;

    /// <summary>Returns a component wise mask of the greater or equal comparison of the two colors</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator >=(Color a, Color b) => a.rgba >= b.rgba;

    /// <summary>Returns a component wise mask of the less or equal comparison of the two colors</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 operator <=(Color a, Color b) => a.rgba <= b.rgba;

    static bool IEqualityOperators<Color, Color, bool>.operator ==(Color left, Color right)
        => left.Equals(right);

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

    /// <inheritdoc/>
    public readonly override string ToString() => $"Color({r}, {g}, {b}, {a})";
}
