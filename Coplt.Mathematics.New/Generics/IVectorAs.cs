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

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the same kind that has 2 components
/// <para>The register of a vector of 4 byte components is as wide as the one of its 2 component vector, so the
/// bits of one of them are the bits of the other one whose dropped components are zero</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The 2 component vector of the same component type</typeparam>
public interface IVectorAs2<TSelf, out T>
    where TSelf : unmanaged, IVectorAs2<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the 2 component vector of the same component type
    /// <para>The components behind the second one are dropped, so they have to be zero because the padding lanes
    /// of the register of the 2 component vector are</para>
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The 2 component vector that has the bits of <paramref name="source"/></returns>
    public static abstract T as2(in TSelf source);
}

/// <summary>
/// A vector of 4 components whose bits can be reinterpreted as the vector of the same kind that has 3 of them
/// <para>The 4 component vector and the 3 component one keep their components in the register of the same
/// width, so the bits of one of them are the bits of the other one whose dropped component is zero</para>
/// </summary>
/// <typeparam name="TSelf">The 4 component vector type itself</typeparam>
/// <typeparam name="T">The 3 component vector of the same component type</typeparam>
public interface IVectorAs3<TSelf, out T>
    where TSelf : unmanaged, IVectorAs3<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the 3 component vector of the same component type
    /// <para>The <c>w</c> component of <paramref name="source"/> is dropped, so it has to be zero because the
    /// padding lane of the register of the 3 component vector is</para>
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The 3 component vector that has the bits of <paramref name="source"/></returns>
    public static abstract T as3(in TSelf source);
}

/// <summary>
/// A vector of 3 components whose bits can be reinterpreted as the vector of the same kind that has 4 of them
/// <para>The 4 component vector and the 3 component one keep their components in the register of the same
/// width, so the bits of one of them are the bits of the other one whose added component is zero</para>
/// </summary>
/// <typeparam name="TSelf">The 3 component vector type itself</typeparam>
/// <typeparam name="T">The 4 component vector of the same component type</typeparam>
public interface IVectorAs4<TSelf, out T>
    where TSelf : unmanaged, IVectorAs4<TSelf, T>
{
    /// <summary>
    /// Reinterprets the bits of <paramref name="source"/> as the 4 component vector of the same component type
    /// <para>The added <c>w</c> component is zero, which the padding lane of the register of the 3 component
    /// vector already is</para>
    /// </summary>
    /// <param name="source">The vector to reinterpret</param>
    /// <returns>The 4 component vector that has the bits of <paramref name="source"/></returns>
    public static abstract T as4(in TSelf source);
}
