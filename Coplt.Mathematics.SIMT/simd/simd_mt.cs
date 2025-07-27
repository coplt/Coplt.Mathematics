namespace Coplt.Mathematics.SIMT;

public static partial class simd_mt
{
    #region Rem

    [MethodImpl(256 | 512)]
    public static Vector<int> Rem(Vector<int> a, Vector<int> b) => a - (a / b) * b;
    
    [MethodImpl(256 | 512)]
    public static Vector<int> Rem(Vector<int> a, int b) => a - (a / b) * b;

    [MethodImpl(256 | 512)]
    public static Vector<uint> Rem(Vector<uint> a, Vector<uint> b) => a - (a / b) * b;
    
    [MethodImpl(256 | 512)]
    public static Vector<uint> Rem(Vector<uint> a, uint b) => a - (a / b) * b;

    [MethodImpl(256 | 512)]
    public static Vector<float> Rem(Vector<float> a, Vector<float> b)
    {
        #if NET9_0_OR_GREATER
        var div = a / b;
        var flr = RoundToZero(div);
        return Fnma(flr, b, a);
        #else
        if (Vector<float>.Count == Vector128<float>.Count) return simd_math.Rem(a.AsVector128(), b.AsVector128()).AsVector();
        if (Vector<float>.Count == Vector256<float>.Count) return simd_math.Rem(a.AsVector256(), b.AsVector256()).AsVector();
        if (Vector<float>.Count == Vector512<float>.Count) return simd_math.Rem(a.AsVector512(), b.AsVector512()).AsVector();

        // soft
        Span<float> result = stackalloc float[Vector<float>.Count];
        for (var i = 0; i < Vector<float>.Count; i++)
        {
            result[i] = a[i] % b[i];
        }
        return Vector.LoadUnsafe(in result.GetPinnableReference());
        #endif
    }

    [MethodImpl(256 | 512)]
    public static Vector<float> Rem(Vector<float> a, float b)
    {
        #if NET9_0_OR_GREATER
        var div = a / b;
        var flr = RoundToZero(div);
        return Fnma(flr, Vector.Create(b), a);
        #else
        if (Vector<float>.Count == Vector128<float>.Count) return simd_math.Rem(a.AsVector128(), b).AsVector();
        if (Vector<float>.Count == Vector256<float>.Count) return simd_math.Rem(a.AsVector256(), b).AsVector();
        if (Vector<float>.Count == Vector512<float>.Count) return simd_math.Rem(a.AsVector512(), b).AsVector();

        // soft
        Span<float> result = stackalloc float[Vector<float>.Count];
        for (var i = 0; i < Vector<float>.Count; i++)
        {
            result[i] = a[i] % b;
        }
        return Vector.LoadUnsafe(in result.GetPinnableReference());
        #endif
    }

    #endregion

    #region RoundToZero
    
    [MethodImpl(256 | 512)]
    public static Vector<float> RoundToZero(Vector<float> a)
    {
        if (Vector<float>.Count == Vector128<float>.Count) return simd.RoundToZero(a.AsVector128()).AsVector();
        if (Vector<float>.Count == Vector256<float>.Count) return simd.RoundToZero(a.AsVector256()).AsVector();
        if (Vector<float>.Count == Vector512<float>.Count) return simd.RoundToZero(a.AsVector512()).AsVector();
        #if NET9_0_OR_GREATER
        // Vector.Round not inlined in NET9.0，if the new version is inlined later, may consider using it directly
        return Vector.Round(a, MidpointRounding.ToZero);
        #else
        // soft
        Span<float> result = stackalloc float[Vector<float>.Count];
        for (var i = 0; i < Vector<float>.Count; i++)
        {
            result[i] = MathF.Round(a[i], MidpointRounding.ToZero);
        }
        return Vector.LoadUnsafe(in result.GetPinnableReference());
        #endif
    }

    #endregion

    #region Fma

    /// <returns><code>a * b + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector<float> Fma(Vector<float> a, Vector<float> b, Vector<float> c)
    {
        if (Vector<float>.Count == Vector128<float>.Count) return simd.Fma(a.AsVector128(), b.AsVector128(), c.AsVector128()).AsVector();
        if (Vector<float>.Count == Vector256<float>.Count) return simd.Fma(a.AsVector256(), b.AsVector256(), c.AsVector256()).AsVector();
        if (Vector<float>.Count == Vector512<float>.Count) return simd.Fma(a.AsVector512(), b.AsVector512(), c.AsVector512()).AsVector();
        #if NET9_0_OR_GREATER
        return Vector.FusedMultiplyAdd(a, b, c);
        #else
        return a * b + c;
        #endif
    }

    /// <returns><code>a * b - c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector<float> Fms(Vector<float> a, Vector<float> b, Vector<float> c)
    {
        if (Vector<float>.Count == Vector128<float>.Count) return simd.Fms(a.AsVector128(), b.AsVector128(), c.AsVector128()).AsVector();
        if (Vector<float>.Count == Vector256<float>.Count) return simd.Fms(a.AsVector256(), b.AsVector256(), c.AsVector256()).AsVector();
        if (Vector<float>.Count == Vector512<float>.Count) return simd.Fms(a.AsVector512(), b.AsVector512(), c.AsVector512()).AsVector();
        #if NET9_0_OR_GREATER
        return Vector.FusedMultiplyAdd(a, b, -c);
        #else
        return a * b + c;
        #endif
    }

    /// <returns><code>c - a * b</code> or <code>-(a * b) + c</code></returns>
    [MethodImpl(256 | 512)]
    public static Vector<float> Fnma(Vector<float> a, Vector<float> b, Vector<float> c)
    {
        if (Vector<float>.Count == Vector128<float>.Count) return simd.Fnma(a.AsVector128(), b.AsVector128(), c.AsVector128()).AsVector();
        if (Vector<float>.Count == Vector256<float>.Count) return simd.Fnma(a.AsVector256(), b.AsVector256(), c.AsVector256()).AsVector();
        if (Vector<float>.Count == Vector512<float>.Count) return simd.Fnma(a.AsVector512(), b.AsVector512(), c.AsVector512()).AsVector();
        return c - a * b;
    }

    #endregion
}
