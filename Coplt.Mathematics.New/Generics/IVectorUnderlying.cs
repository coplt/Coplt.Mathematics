namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector that has no register
/// <para>Its value is kept in fields, so its bits are not reachable as a raw vector of bytes</para>
/// </summary>
public interface IVectorSoftUnderlying;

/// <summary>
/// A vector whose bits are reachable as a raw <see cref="Vector64{T}"/> of bytes
/// <para>A vector that is exactly 64 bits wide is the register itself, so every one of its bits is a bit of
/// its value</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
public interface IVector64Underlying<TSelf> where TSelf : unmanaged, IVector64Underlying<TSelf>
{
    /// <summary>
    /// Returns the raw 64 bits of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector to read</param>
    /// <returns>The raw 64 bits of <paramref name="self"/></returns>
    public static abstract Vector64<byte> GetUnderlying(in TSelf self);

    /// <summary>
    /// Creates a vector from the raw 64 bits of <paramref name="vector"/>
    /// <para>The padding lanes of the register are set to zero</para>
    /// </summary>
    /// <param name="vector">The raw 64 bits of the vector</param>
    /// <returns>The vector that has the bits of <paramref name="vector"/></returns>
    public static abstract TSelf FromUnderlying(Vector64<byte> vector);

    /// <summary>
    /// Creates a vector from the raw 64 bits of <paramref name="vector"/> without masking the padding lanes
    /// <para>It is the same as <see cref="FromUnderlying"/> for a vector that has no padding lane, and a value
    /// that leaves something else than zero in one of them breaks the invariant of the vector: the member
    /// exists for the code that knows the value it writes</para>
    /// </summary>
    /// <param name="vector">The raw 64 bits of the vector</param>
    /// <returns>The vector that has the bits of <paramref name="vector"/></returns>
    public static abstract TSelf UnsafeFromUnderlying(Vector64<byte> vector);
}

/// <summary>
/// A vector whose bits are reachable as a raw <see cref="Vector128{T}"/> of bytes
/// <para>A vector that is exactly 128 bits wide is the register itself, so every one of its bits is a bit of
/// its value</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
public interface IVector128Underlying<TSelf> where TSelf : unmanaged, IVector128Underlying<TSelf>
{
    /// <summary>
    /// Returns the raw 128 bits of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector to read</param>
    /// <returns>The raw 128 bits of <paramref name="self"/></returns>
    public static abstract Vector128<byte> GetUnderlying(in TSelf self);

    /// <summary>
    /// Creates a vector from the raw 128 bits of <paramref name="vector"/>
    /// <para>The padding lanes of the register are set to zero</para>
    /// </summary>
    /// <param name="vector">The raw 128 bits of the vector</param>
    /// <returns>The vector that has the bits of <paramref name="vector"/></returns>
    public static abstract TSelf FromUnderlying(Vector128<byte> vector);

    /// <summary>
    /// Creates a vector from the raw 128 bits of <paramref name="vector"/> without masking the padding lanes
    /// <para>It is the same as <see cref="FromUnderlying"/> for a vector that has no padding lane, and a value
    /// that leaves something else than zero in one of them breaks the invariant of the vector: the member
    /// exists for the code that knows the value it writes</para>
    /// </summary>
    /// <param name="vector">The raw 128 bits of the vector</param>
    /// <returns>The vector that has the bits of <paramref name="vector"/></returns>
    public static abstract TSelf UnsafeFromUnderlying(Vector128<byte> vector);
}

/// <summary>
/// A vector whose bits are reachable as a raw <see cref="Vector256{T}"/> of bytes
/// <para>A vector that is exactly 256 bits wide is the register itself, so every one of its bits is a bit of
/// its value</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
public interface IVector256Underlying<TSelf> where TSelf : unmanaged, IVector256Underlying<TSelf>
{
    /// <summary>
    /// Returns the raw 256 bits of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector to read</param>
    /// <returns>The raw 256 bits of <paramref name="self"/></returns>
    public static abstract Vector256<byte> GetUnderlying(in TSelf self);

    /// <summary>
    /// Creates a vector from the raw 256 bits of <paramref name="vector"/>
    /// <para>The padding lanes of the register are set to zero</para>
    /// </summary>
    /// <param name="vector">The raw 256 bits of the vector</param>
    /// <returns>The vector that has the bits of <paramref name="vector"/></returns>
    public static abstract TSelf FromUnderlying(Vector256<byte> vector);

    /// <summary>
    /// Creates a vector from the raw 256 bits of <paramref name="vector"/> without masking the padding lanes
    /// <para>It is the same as <see cref="FromUnderlying"/> for a vector that has no padding lane, and a value
    /// that leaves something else than zero in one of them breaks the invariant of the vector: the member
    /// exists for the code that knows the value it writes</para>
    /// </summary>
    /// <param name="vector">The raw 256 bits of the vector</param>
    /// <returns>The vector that has the bits of <paramref name="vector"/></returns>
    public static abstract TSelf UnsafeFromUnderlying(Vector256<byte> vector);
}
