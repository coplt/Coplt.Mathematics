namespace Coplt.Mathematics;

internal static class FormatUtils
{
    #region Utf16

    public static bool TryFormatPart(ref Span<char> dst, ref int nc, ReadOnlySpan<char> part)
    {
        if (dst.Length < part.Length) return false;
        part.CopyTo(dst);
        nc += part.Length;
        dst = dst[part.Length..];
        return true;
    }

    #if NET8_0_OR_GREATER
    public static bool TryFormatPart<T>(ref Span<char> dst, ref int nc, T part, ReadOnlySpan<char> format, IFormatProvider? provider)
        where T : ISpanFormattable
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    #else
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, short part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, ushort part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, half part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, float part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, double part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, decimal part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, int part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, uint part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, long part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, ulong part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, b16 part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, b32 part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    public static bool TryFormatPart(ref Span<char> dst, ref int nc, b64 part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }
    #endif

    public static bool TryFormatPart(ref Span<char> dst, ref int nc, bool part, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var r = part.TryFormat(dst, out var ic);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }

    #endregion

    #region Utf8

    public static bool TryFormatPart(ref Span<byte> dst, ref int nc, ReadOnlySpan<byte> part)
    {
        if (dst.Length < part.Length) return false;
        part.CopyTo(dst);
        nc += part.Length;
        dst = dst[part.Length..];
        return true;
    }

    public static bool TryFormatPart<T>(ref Span<byte> dst, ref int nc, T part, ReadOnlySpan<char> format, IFormatProvider? provider)
        where T : IUtf8SpanFormattable
    {
        var r = part.TryFormat(dst, out var ic, format, provider);
        nc += ic;
        if (!r) return false;
        dst = dst[ic..];
        return true;
    }

    public static bool TryFormatPart(ref Span<byte> dst, ref int nc, bool part, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormatPart(ref dst, ref nc, part ? "True"u8 : "False"u8);

    #endregion
}
