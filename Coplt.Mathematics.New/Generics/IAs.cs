namespace Coplt.Mathematics.Generics;

/// <summary>
/// A value whose bits can be reinterpreted as the value of the floating point component of the same width
/// <para>Every member of a group keeps the bits of the value, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// <para>The interface declares no member of its own: the getter that reinterprets the bits of the value is a
/// member of the value itself and the interface only names the type of the result of it, so a member that cannot
/// infer that type from the value it is handed is constrained by the interface of the kind of the result</para>
/// <para>The vectors of a group and the matrices of a group reach the members of it, both of them by their own
/// name: the interface carries the type of the result alone, so a matrix reinterprets its bits as the matrix of
/// the other component type the way a vector reaches the vector of it.</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="T">The value of the floating point component</typeparam>
public interface IAsF<TSelf, out T>
    where TSelf : unmanaged, IAsF<TSelf, T>;

/// <summary>
/// A value whose bits can be reinterpreted as the value of the signed component of the same width
/// <para>Every member of a group keeps the bits of the value, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// <para>The interface declares no member of its own: the getter that reinterprets the bits of the value is a
/// member of the value itself and the interface only names the type of the result of it, so a member that cannot
/// infer that type from the value it is handed is constrained by the interface of the kind of the result</para>
/// <para>The vectors of a group and the matrices of a group reach the members of it, both of them by their own
/// name: the interface carries the type of the result alone, so a matrix reinterprets its bits as the matrix of
/// the other component type the way a vector reaches the vector of it.</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="T">The value of the signed component</typeparam>
public interface IAsI<TSelf, out T>
    where TSelf : unmanaged, IAsI<TSelf, T>;

/// <summary>
/// A value whose bits can be reinterpreted as the value of the unsigned component of the same width
/// <para>Every member of a group keeps the bits of the value, only the type of the components changes, so a
/// round trip through any of them keeps every component</para>
/// <para>The interface declares no member of its own: the getter that reinterprets the bits of the value is a
/// member of the value itself and the interface only names the type of the result of it, so a member that cannot
/// infer that type from the value it is handed is constrained by the interface of the kind of the result</para>
/// <para>The vectors of a group and the matrices of a group reach the members of it, both of them by their own
/// name: the interface carries the type of the result alone, so a matrix reinterprets its bits as the matrix of
/// the other component type the way a vector reaches the vector of it.</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
/// <typeparam name="T">The value of the unsigned component</typeparam>
public interface IAsU<TSelf, out T>
    where TSelf : unmanaged, IAsU<TSelf, T>;
