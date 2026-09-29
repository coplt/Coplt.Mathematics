namespace Coplt.Mathematics;

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool eq<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a == b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ne<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a != b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool lt<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a < b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool gt<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a > b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool le<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a <= b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ge<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a >= b;

    /// <summary>
    /// Selects between the two values: it returns <paramref name="t"/> when the condition is true and
    /// <paramref name="f"/> when it is false
    /// </summary>
    /// <typeparam name="T">The type of the values to select</typeparam>
    /// <param name="c">The condition</param>
    /// <param name="t">The value that is selected when the condition is true</param>
    /// <param name="f">The value that is selected when the condition is false</param>
    /// <returns>The value of <paramref name="t"/> when <paramref name="c"/> is true and the one of
    /// <paramref name="f"/> when it is false</returns>
    [MethodImpl(256)]
    public static T select<T>(bool c, in T t, in T f) => c ? t : f;

    /// <summary>
    /// Selects between the two values with a mask
    /// </summary>
    /// <remarks>
    /// A mask is a value of the type of the values to select: the all bits set value of the type takes the whole
    /// of the result from <paramref name="t"/> and the all bits zero value of it takes the whole of it from
    /// <paramref name="f"/>, which are the two of them that the member is written for.
    /// <para>A mask that is neither of the two of them answers with the one of the platform: which of the two
    /// values a bit of the result comes from is the trait of the platform that selects with the mask, which may
    /// mux the bits of the two values with it or take the whole of a value with the sign bit of it, so the
    /// answer of the member that is handed such a mask is not one that a caller may rely on, on a cpu as much as
    /// on a gpu.</para>
    /// <para>It is not the selection of a whole value, which the <see cref="select{T}(bool, in T, in T)"/> does
    /// with a condition that is about the whole of it, and the <c>select</c> intrinsic of hlsl is its
    /// counterpart, which selects with a condition of the kind of the values and not with a mask: the whole of a
    /// value is taken from <paramref name="t"/> where the condition of it holds and from <paramref name="f"/>
    /// where it does not, which is the <c>c ? t : f</c> of it, and the truth of the condition of it is the one
    /// of hlsl, which is a value that is not the zero of its kind. The truth of the mask of this member is the
    /// one of the platform instead.</para>
    /// <para>It is nothing that a computation that is about a whole value notices: the output of a bool
    /// operation of simd is the all bits set value of the type where it holds and the all bits zero value of it
    /// where it does not, so the mask of such a computation is one of the two of them, and both of them take the
    /// value of one of the two values with this member as they do with the intrinsic.</para>
    /// </remarks>
    /// <typeparam name="T">The type of the values to select, the three of them have it</typeparam>
    /// <param name="c">The mask, a value of the type of the values to select</param>
    /// <param name="t">The value that is selected where the mask holds</param>
    /// <param name="f">The value that is selected where the mask does not hold</param>
    /// <returns>The value that the mask selects out of the two values</returns>
    [MethodImpl(256)]
    public static T select<T>(T c, T t, T f) where T : unmanaged, IBinaryNumber<T>
    {
        if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
        {
            return Vector128.ConditionalSelect(
                Vector128.CreateScalarUnsafe(c),
                Vector128.CreateScalarUnsafe(t),
                Vector128.CreateScalarUnsafe(f)
            ).ToScalar();
        }

        return Utils.IsTrue(c) ? t : f;
    }
}
