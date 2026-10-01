namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the floating point component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// <para>The interface declares no member of its own: the getter that reinterprets the bits of the vector is a
/// member of the vector itself and the interface only names the type of the result of it, so a member that cannot
/// infer that type from the vector it is handed is constrained by the interface of the kind of the result</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the floating point component</typeparam>
public interface IVectorAsF<TSelf, out T>
    where TSelf : unmanaged, IVectorAsF<TSelf, T>;

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the signed component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// <para>The interface declares no member of its own: the getter that reinterprets the bits of the vector is a
/// member of the vector itself and the interface only names the type of the result of it, so a member that cannot
/// infer that type from the vector it is handed is constrained by the interface of the kind of the result</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the signed component</typeparam>
public interface IVectorAsI<TSelf, out T>
    where TSelf : unmanaged, IVectorAsI<TSelf, T>;

/// <summary>
/// A vector whose bits can be reinterpreted as the vector of the unsigned component of the same width
/// <para>Every member of a group keeps the bits of the vector, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// <para>The interface declares no member of its own: the getter that reinterprets the bits of the vector is a
/// member of the vector itself and the interface only names the type of the result of it, so a member that cannot
/// infer that type from the vector it is handed is constrained by the interface of the kind of the result</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="T">The vector of the unsigned component</typeparam>
public interface IVectorAsU<TSelf, out T>
    where TSelf : unmanaged, IVectorAsU<TSelf, T>;
