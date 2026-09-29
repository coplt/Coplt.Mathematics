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
    /// Selects between the two values bit by bit, which is a mux of them: the bit of <paramref name="t"/> where
    /// the bit of <paramref name="c"/> at the position of it is set and the bit of <paramref name="f"/> where it
    /// is not
    /// </summary>
    /// <remarks>
    /// A mask is a value of the type of the values to select and every bit of it decides the bit of the result
    /// at the position of it: the all bits set value of the type selects the value of <paramref name="t"/> as it
    /// is, the all bits zero value of it selects the value of <paramref name="f"/> as it is, and a mask that is
    /// neither of the two of them muxes the bits of the two values.
    /// <para>It is not the selection of a whole value, which the <see cref="select{T}(bool, in T, in T)"/> does
    /// with a condition that is about the whole of it, and the <c>select</c> intrinsic of hlsl is its
    /// counterpart, which differs from it in the mask: the mask of the intrinsic is about a whole component of
    /// the values to select, so it takes the whole of a component from the one of the two values that the
    /// component of the mask at the position of it holds, where the mask of this member is a value of the type
    /// of the values to select itself and every bit of it decides the bit of the result at the position of
    /// it.</para>
    /// <para>It is nothing that a computation that is about a whole value notices: simd takes the all bits set
    /// value of the type as the true one and the all bits zero value of it as the false one, so the mask of a
    /// comparison of simd values is the one or the other of them, and both of them take the value of one of the
    /// two values with this member as they do with the intrinsic.</para>
    /// <para>A mask that is neither of the two of them is not one that the member is written for: the bits of
    /// the value alone deciding the bits of the result is the answer of this library, where a platform may mux
    /// the bits of the two values with the mask of it or take the whole of a value with the sign bit of the mask
    /// of it, so the answer of the member that is handed such a mask is not one that a caller may rely on, on a
    /// cpu as much as on a gpu.</para>
    /// </remarks>
    /// <typeparam name="T">The type of the values to select, the three of them have it</typeparam>
    /// <param name="c">The mask, a value of the type of the values to select</param>
    /// <param name="t">The value that the bits the mask sets are taken from</param>
    /// <param name="f">The value that the bits the mask does not set are taken from</param>
    /// <returns>The value of <typeparamref name="T"/> whose every bit is the one of <paramref name="t"/> where
    /// the bit of <paramref name="c"/> at the position of it is set and the one of <paramref name="f"/> where it
    /// is not</returns>
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
