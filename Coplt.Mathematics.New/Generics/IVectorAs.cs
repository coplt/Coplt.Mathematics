namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the floating point component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the floating point component</typeparam>
public interface IVectorAsF<TSelf, out T>
    where TSelf : unmanaged, IVectorAsF<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the floating point component
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of <paramref name="source"/></returns>
    public static abstract T asf(in TSelf source);
}

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the signed component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the signed component</typeparam>
public interface IVectorAsI<TSelf, out T>
    where TSelf : unmanaged, IVectorAsI<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the signed component
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of <paramref name="source"/></returns>
    public static abstract T asi(in TSelf source);
}

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the unsigned component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the unsigned component</typeparam>
public interface IVectorAsU<TSelf, out T>
    where TSelf : unmanaged, IVectorAsU<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the unsigned component
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of <paramref name="source"/></returns>
    public static abstract T asu(in TSelf source);
}

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the boolean component of the same width
/// <para>The vector of a storage variant reinterprets its bits as the regular bool vector, the bool vector has no
/// storage variant of its own, so a storage variant that has no bool vector of its own width has no bool member
/// at all and does not implement this interface</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the boolean component</typeparam>
public interface IVectorAsB<TSelf, out T>
    where TSelf : unmanaged, IVectorAsB<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the vector of the boolean component
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of <paramref name="source"/></returns>
    public static abstract T asb(in TSelf source);
}
