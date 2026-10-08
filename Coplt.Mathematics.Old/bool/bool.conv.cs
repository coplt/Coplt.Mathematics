namespace Coplt.Mathematics;

public readonly partial record struct b16
{
    [MethodImpl(256 | 512)]
    public static explicit operator b16(b32 v) => v ? True : False;
    [MethodImpl(256 | 512)]
    public static explicit operator b16(b64 v) => v ? True : False;
}

public readonly partial record struct b32
{
    [MethodImpl(256 | 512)]
    public static explicit operator b32(b16 v) => v ? True : False;
    [MethodImpl(256 | 512)]
    public static explicit operator b32(b64 v) => v ? True : False;
}

public readonly partial record struct b64
{
    [MethodImpl(256 | 512)]
    public static explicit operator b64(b16 v) => v ? True : False;
    [MethodImpl(256 | 512)]
    public static explicit operator b64(b32 v) => v ? True : False;
}
