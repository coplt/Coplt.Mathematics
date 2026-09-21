namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the floating point component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a round trip through any of them keeps every component</para>
/// </summary>
/// <typeparam name="T">The vector of the floating point component</typeparam>
public interface IVectorAsF<out T>
{
    /// <summary>
    /// Reinterprets the bits of the vector as the vector of the floating point component
    /// </summary>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of the vector</returns>
    public T asf();
}

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the signed component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a round trip through any of them keeps every component</para>
/// </summary>
/// <typeparam name="T">The vector of the signed component</typeparam>
public interface IVectorAsI<out T>
{
    /// <summary>
    /// Reinterprets the bits of the vector as the vector of the signed component
    /// </summary>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of the vector</returns>
    public T asi();
}

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the unsigned component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a round trip through any of them keeps every component</para>
/// </summary>
/// <typeparam name="T">The vector of the unsigned component</typeparam>
public interface IVectorAsU<out T>
{
    /// <summary>
    /// Reinterprets the bits of the vector as the vector of the unsigned component
    /// </summary>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of the vector</returns>
    public T asu();
}

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the boolean component of the same width
/// <para>The vector of a storage variant reinterprets its bits as the regular bool vector, the bool vector has no storage variant of its own</para>
/// </summary>
/// <typeparam name="T">The vector of the boolean component</typeparam>
public interface IVectorAsB<out T>
{
    /// <summary>
    /// Reinterprets the bits of the vector as the vector of the boolean component
    /// </summary>
    /// <returns>The vector of <typeparamref name="T"/> that has the bits of the vector</returns>
    public T asb();
}
