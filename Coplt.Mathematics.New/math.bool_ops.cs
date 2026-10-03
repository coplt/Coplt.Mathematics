namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Whether the whole of two values is equal
    /// </summary>
    /// <remarks>
    /// It answers with a bool, it is the equality of the framework, and the whole of a vector is what the answer is
    /// about: every component of one of the values has to be equal to the component of the other one of it.
    /// <para>The counterpart of it that answers with the mask of the kind of the values is
    /// <see cref="ceq{T}(T, T)"/>. The mask one is the one the algebra of a value reaches, a type parameter that
    /// names the interface of the algebra beside the one of the framework does not reach this member at all: the
    /// two answers of the comparison are ambiguous to it.</para>
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>True when the whole of the two values is equal</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool eq<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a == b;

    /// <summary>
    /// Whether the whole of two values is not equal
    /// </summary>
    /// <remarks>
    /// It answers with a bool, it is the inequality of the framework. The mask answering counterpart of it, the
    /// member of the algebra that reaches the comparison and the type parameter that reaches this one are the ones
    /// <see cref="eq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>True when the whole of the two values is not equal</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ne<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a != b;

    /// <summary>
    /// Whether every component of the left value is less than the matching component of the right one
    /// </summary>
    /// <remarks>
    /// It answers with a bool, it is the comparison of the framework, and the whole of a vector is what the answer
    /// is about. The mask answering counterpart of it, the member of the algebra that reaches the comparison and
    /// the type parameter that reaches this one are the ones <see cref="eq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>True when every component of <paramref name="a"/> is less than the matching one of
    /// <paramref name="b"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool lt<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a < b;

    /// <summary>
    /// Whether every component of the left value is greater than the matching component of the right one
    /// </summary>
    /// <remarks>
    /// It answers with a bool, it is the comparison of the framework, and the whole of a vector is what the answer
    /// is about. The mask answering counterpart of it, the member of the algebra that reaches the comparison and
    /// the type parameter that reaches this one are the ones <see cref="eq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>True when every component of <paramref name="a"/> is greater than the matching one of
    /// <paramref name="b"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool gt<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a > b;

    /// <summary>
    /// Whether every component of the left value is not greater than the matching component of the right one
    /// </summary>
    /// <remarks>
    /// It answers with a bool, it is the comparison of the framework, and the whole of a vector is what the answer
    /// is about. The mask answering counterpart of it, the member of the algebra that reaches the comparison and
    /// the type parameter that reaches this one are the ones <see cref="eq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>True when every component of <paramref name="a"/> is not greater than the matching one of
    /// <paramref name="b"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool le<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a <= b;

    /// <summary>
    /// Whether every component of the left value is not less than the matching component of the right one
    /// </summary>
    /// <remarks>
    /// It answers with a bool, it is the comparison of the framework, and the whole of a vector is what the answer
    /// is about. The mask answering counterpart of it, the member of the algebra that reaches the comparison and
    /// the type parameter that reaches this one are the ones <see cref="eq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>True when every component of <paramref name="a"/> is not less than the matching one of
    /// <paramref name="b"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ge<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a >= b;

    /// <summary>
    /// The mask of the equality of two values: the all bits set value of the type at every component that the two
    /// values are equal at and the all bits zero value of it at every one that they are not
    /// </summary>
    /// <remarks>
    /// It answers with a value of the type of the values and not with a bool, it is the comparison of the algebra,
    /// so the members that build a value out of two of them, as the <see cref="scalar_ex.select{T}(T, T, T)"/> of a mask
    /// does, reach it. The mask that a comparison of simd answers with is one of the two values that it is written
    /// for, see the remarks of <see cref="scalar_ex.select{T}(T, T, T)"/>.
    /// <para>The counterpart of it that answers with a bool is <see cref="eq{T}(T, T)"/>, which is the one the code
    /// whose type parameter names the interface of the framework alone reaches.</para>
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>The mask of the equality of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ceq<T>(T a, T b) where T : IEqualityOperators<T, T, T>
        => a == b;

    /// <summary>
    /// The mask of the inequality of two values: the all bits set value of the type at every component that the two
    /// values are not equal at and the all bits zero value of it at every one that they are
    /// </summary>
    /// <remarks>
    /// It answers with a value of the type of the values and not with a bool. Its bool answering counterpart, the
    /// member of the framework and the type parameter that reaches it are the ones
    /// <see cref="ceq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>The mask of the inequality of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T cne<T>(T a, T b) where T : IEqualityOperators<T, T, T>
        => a != b;

    /// <summary>
    /// The mask of the comparison of every component of the left value to the matching one of the right one, which
    /// is the comparison of it that answers with the all bits set value of the type at every component of the left
    /// value that is less than the matching one of the right one and with the all bits zero value of it at every
    /// one that it is not
    /// </summary>
    /// <remarks>
    /// It answers with a value of the type of the values and not with a bool. Its bool answering counterpart, the
    /// member of the framework and the type parameter that reaches it are the ones
    /// <see cref="ceq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>The mask of the comparison of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T clt<T>(T a, T b) where T : IComparisonOperators<T, T, T>
        => a < b;

    /// <summary>
    /// The mask of the comparison of every component of the left value to the matching one of the right one, which
    /// is the comparison of it that answers with the all bits set value of the type at every component of the left
    /// value that is greater than the matching one of the right one and with the all bits zero value of it at every
    /// one that it is not
    /// </summary>
    /// <remarks>
    /// It answers with a value of the type of the values and not with a bool. Its bool answering counterpart, the
    /// member of the framework and the type parameter that reaches it are the ones
    /// <see cref="ceq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>The mask of the comparison of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T cgt<T>(T a, T b) where T : IComparisonOperators<T, T, T>
        => a > b;

    /// <summary>
    /// The mask of the comparison of every component of the left value to the matching one of the right one, which
    /// is the comparison of it that answers with the all bits set value of the type at every component of the left
    /// value that is not greater than the matching one of the right one and with the all bits zero value of it at
    /// every one that it is
    /// </summary>
    /// <remarks>
    /// It answers with a value of the type of the values and not with a bool. Its bool answering counterpart, the
    /// member of the framework and the type parameter that reaches it are the ones
    /// <see cref="ceq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>The mask of the comparison of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T cle<T>(T a, T b) where T : IComparisonOperators<T, T, T>
        => a <= b;

    /// <summary>
    /// The mask of the comparison of every component of the left value to the matching one of the right one, which
    /// is the comparison of it that answers with the all bits set value of the type at every component of the left
    /// value that is not less than the matching one of the right one and with the all bits zero value of it at
    /// every one that it is
    /// </summary>
    /// <remarks>
    /// It answers with a value of the type of the values and not with a bool. Its bool answering counterpart, the
    /// member of the framework and the type parameter that reaches it are the ones
    /// <see cref="ceq{T}(T, T)"/> names.
    /// </remarks>
    /// <typeparam name="T">The type of the two values</typeparam>
    /// <param name="a">The left value</param>
    /// <param name="b">The right value</param>
    /// <returns>The mask of the comparison of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T cge<T>(T a, T b) where T : IComparisonOperators<T, T, T>
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
    public static T select<T>(bool c, T t, T f) => c ? t : f;
}

/// <summary>
/// The members of the scalar kinds that the member of the value of the same name reaches, they live in a class
/// of their own so that the member of the value and the one of the scalar of the same shape do not collide
/// </summary>
public static partial class scalar_ex
{
    extension(math)
    {
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
        /// <para>It is not the selection of a whole value, which the <see cref="math.select{T}(bool, T, T)"/> does
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
}
