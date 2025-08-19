namespace Coplt.Mathematics;

public struct Color(float4 rgba) : IEquatable<Color>
{
    #region Fields

    public float4 rgba = rgba;

    #endregion

    #region Properties

    public float r
    {
        get => rgba.r;
        set => rgba.r = value;
    }

    public float g
    {
        get => rgba.g;
        set => rgba.g = value;
    }

    public float b
    {
        get => rgba.b;
        set => rgba.b = value;
    }

    public float a
    {
        get => rgba.a;
        set => rgba.a = value;
    }

    #endregion

    #region Ctor

    [MethodImpl(256)]
    public Color(float3 rgb, float a = 1) : this(new(rgb, a)) { }

    [MethodImpl(256)]
    public Color(float r, float g, float b, float a = 1) : this(new(r, g, b, a)) { }

    [MethodImpl(256)]
    public Color(byte r, byte g, byte b, byte a = 255) : this(new float4(r, g, b, a) * (1f / 255f)) { }

    #endregion

    #region Hsv

    #region From Hsv

    [MethodImpl(256 | 512)]
    public static Color Hsv(float h, float s, float v, float a = 1)
    {
        var p = abs(fms(frac(h + new float3(1, 2f / 3f, 1f / 3f)), 6f, 3f));
        var x = v * lerp(1, saturate(p - 1), s);
        return new(x, a);
    }
    [MethodImpl(256 | 512)]
    public static Color Hsv(float3 hsv, float a = 1)
    {
        var p = abs(fms(frac(hsv.xxx + new float3(1, 2f / 3f, 1f / 3f)), 6f, 3f));
        var x = hsv.zzz * lerp(1, saturate(p - 1), hsv.yyy);
        return new(x, a);
    }
    [MethodImpl(256 | 512)]
    public static Color Hsv(float4 hsva)
    {
        var p = abs(fms(frac(hsva.xxx + new float3(1, 2f / 3f, 1f / 3f)), 6f, 3f));
        var x = hsva.zzz * lerp(1, saturate(p - 1), hsva.yyy);
        return new(new int4(-1, -1, -1, 0).asb().select(x.as4(), hsva));
    }

    #endregion

    #region To Hsv

    public float4 ToHsv
    {
        [MethodImpl(256 | 512)]
        get
        {
            var c = rgba;
            var (cr, cg, cb, ca) = c;
            var p = lerp(new float4(c.bg, -1f, 2f / 3f), new float4(c.gb, 0, -1f / 3f), step(cb, cg));
            var (qx, qy, qz, qw) = lerp(new float4(p.xyw, cr), new float4(cr, p.yzx), step(p.x, cr));

            var d = qx - min(qw, qy);
            var e = 1.0e-10f;
            return new float4(abs(qz + (qw - qy) / fma(6f, d, e)), d / (qx + e), qx, ca);
        }
    }

    #endregion

    #endregion

    #region Predefined

    public static readonly Color Transparent = new(float4.Zero);
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color Silver = new(0.75f, 0.75f, 0.75f);
    public static readonly Color Gray = new(0.5f, 0.5f, 0.5f);
    public static readonly Color White = new(float4.One);
    public static readonly Color Maroon = new(0.5f, 0, 0);
    public static readonly Color Red = new(1, 0, 0);
    public static readonly Color Purple = new(0.5f, 0, 0.5f);
    public static readonly Color Fuchsia = new(1, 0, 1);
    public static readonly Color Green = new(0, 0.5f, 0);
    public static readonly Color Lime = new(0, 1, 0);
    public static readonly Color Olive = new(0.5f, 0.5f, 0);
    public static readonly Color Yellow = new(1, 1, 0);
    public static readonly Color Navy = new(0, 0, 0.5f);
    public static readonly Color Blue = new(0, 0, 1);
    public static readonly Color Teal = new(0, 0.5f, 0.5f);
    public static readonly Color Aqua = new(0, 1, 1);

    #endregion

    #region Operator

    [MethodImpl(256)]
    public static implicit operator Color(float4 color) => new(color);
    [MethodImpl(256)]
    public static implicit operator float4(Color color) => color.rgba;

    [MethodImpl(256)]
    public static Color operator +(Color a) => a;
    [MethodImpl(256)]
    public static Color operator -(Color a) => new(-a.rgba);

    [MethodImpl(256)]
    public static Color operator +(Color a, Color b) => new(a.rgba + b.rgba);
    [MethodImpl(256)]
    public static Color operator +(Color a, float b) => new(a.rgba + b);
    [MethodImpl(256)]
    public static Color operator +(float a, Color b) => new(a + b.rgba);
    [MethodImpl(256)]
    public static Color operator -(Color a, Color b) => new(a.rgba - b.rgba);
    [MethodImpl(256)]
    public static Color operator -(Color a, float b) => new(a.rgba - b);
    [MethodImpl(256)]
    public static Color operator -(float a, Color b) => new(a - b.rgba);
    [MethodImpl(256)]
    public static Color operator *(Color a, Color b) => new(a.rgba * b.rgba);
    [MethodImpl(256)]
    public static Color operator *(Color a, float b) => new(a.rgba * b);
    [MethodImpl(256)]
    public static Color operator *(float a, Color b) => new(a * b.rgba);
    [MethodImpl(256)]
    public static Color operator /(Color a, Color b) => new(a.rgba / b.rgba);
    [MethodImpl(256)]
    public static Color operator /(Color a, float b) => new(a.rgba / b);
    [MethodImpl(256)]
    public static Color operator /(float a, Color b) => new(a / b.rgba);
    [MethodImpl(256)]
    public static Color operator %(Color a, Color b) => new(a.rgba % b.rgba);
    [MethodImpl(256)]
    public static Color operator %(Color a, float b) => new(a.rgba % b);
    [MethodImpl(256)]
    public static Color operator %(float a, Color b) => new(a % b.rgba);

    [MethodImpl(256)]
    public static Color operator ~(Color a) => new(~a.rgba);
    [MethodImpl(256)]
    public static Color operator &(Color a, Color b) => new(a.rgba & b.rgba);
    [MethodImpl(256)]
    public static Color operator |(Color a, Color b) => new(a.rgba | b.rgba);
    [MethodImpl(256)]
    public static Color operator ^(Color a, Color b) => new(a.rgba ^ b.rgba);
    [MethodImpl(256)]
    public static Color operator <<(Color a, int b) => new(a.rgba << b);
    [MethodImpl(256)]
    public static Color operator >> (Color a, int b) => new(a.rgba >> b);
    [MethodImpl(256)]
    public static Color operator >>> (Color a, int b) => new(a.rgba >>> b);

    [MethodImpl(256)]
    public static b32v4 operator ==(Color a, Color b) => a.rgba == b.rgba;
    [MethodImpl(256)]
    public static b32v4 operator !=(Color a, Color b) => a.rgba != b.rgba;
    [MethodImpl(256)]
    public static b32v4 operator >(Color a, Color b) => a.rgba > b.rgba;
    [MethodImpl(256)]
    public static b32v4 operator <(Color a, Color b) => a.rgba < b.rgba;
    [MethodImpl(256)]
    public static b32v4 operator >=(Color a, Color b) => a.rgba >= b.rgba;
    [MethodImpl(256)]
    public static b32v4 operator <=(Color a, Color b) => a.rgba <= b.rgba;

    #endregion

    #region Equals

    [MethodImpl(256)]
    public bool Equals(Color other) => rgba.Equals(other.rgba);
    [MethodImpl(256)]
    public override bool Equals(object? obj) => obj is Color other && Equals(other);
    [MethodImpl(256)]
    public override int GetHashCode() => rgba.GetHashCode();

    #endregion

    #region ToString

    public override string ToString() => $"Color({r}, {g}, {b}, {a})";

    #endregion
}

[Ex]
public static partial class math
{
    [MethodImpl(256)]
    public static Color select([This] b32v4 condition, Color a, Color b) => new(select(condition, a.rgba, b.rgba));

    [MethodImpl(256 | 512)]
    public static Color abs([This] Color a) => new(abs(a.rgba));

    [MethodImpl(256 | 512)]
    public static float4 sign([This] Color a) => sign(a.rgba);

    [MethodImpl(256 | 512)]
    public static Color min([This] Color a, Color b) => new(min(a.rgba, b.rgba));

    [MethodImpl(256 | 512)]
    public static Color max([This] Color a, Color b) => new(max(a.rgba, b.rgba));

    [MethodImpl(256 | 512)]
    public static Color clamp([This] Color v, Color min, Color max) => new(clamp(v.rgba, min.rgba, max.rgba));

    [MethodImpl(256 | 512)]
    public static Color clamp([This] Color v, float min, float max) => new(clamp(v.rgba, min, max));

    [MethodImpl(256 | 512)]
    public static Color lerp(float4 start, float4 end, [This] Color t) => new(lerp(start, end, t.rgba));

    [MethodImpl(256 | 512)]
    public static Color lerp(float start, float end, [This] Color t) => new(lerp(start, end, t.rgba));

    [MethodImpl(256 | 512)]
    public static Color unlerp([This] Color a, float4 start, float4 end) => new(unlerp(a.rgba, start, end));

    [MethodImpl(256 | 512)]
    public static Color unlerp([This] Color a, float start, float end) => new(unlerp(a.rgba, start, end));

    [MethodImpl(256 | 512)]
    public static Color remap([This] Color a, float4 srcStart, float4 srcEnd, float4 dstStart, float4 dstEnd) =>
        new(remap(a.rgba, srcStart, srcEnd, dstStart, dstEnd));

    [MethodImpl(256 | 512)]
    public static Color wrap([This] Color x, float4 min, float4 max) => new(wrap(x.rgba, min, max));

    [MethodImpl(256 | 512)]
    public static Color wrap([This] Color x, float min, float max) => new(wrap(x.rgba, min, max));

    [MethodImpl(256 | 512)]
    public static Color saturate([This] Color a) => new(saturate(a.rgba));

    [MethodImpl(256 | 512)]
    public static Color smoothstep(float4 min, float4 max, [This] Color a) => new(smoothstep(min, max, a.rgba));

    [MethodImpl(256 | 512)]
    public static Color rcp([This] Color a) => new(rcp(a.rgba));

    [MethodImpl(256 | 512)]
    public static Color frac([This] Color a) => new(frac(a.rgba));

    [MethodImpl(256 | 512)]
    public static Color trunc([This] Color a) => new(trunc(a.rgba));

    [MethodImpl(256 | 512)]
    public static Color round([This] Color a) => new(round(a.rgba));

    [MethodImpl(256 | 512)]
    public static Color floor([This] Color a) => new(floor(a.rgba));

    [MethodImpl(256 | 512)]
    public static Color ceil([This] Color a) => new(ceil(a.rgba));
}
