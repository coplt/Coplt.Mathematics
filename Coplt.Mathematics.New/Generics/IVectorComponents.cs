namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of 2 components whose components are reached by name
/// <para>Every component has two names: the <c>xyzw</c> spelling of its position and the <c>rgba</c> spelling
/// of a color. The two names reach the same component, so writing one of them is the same as writing the
/// other one</para>
/// <para>A member of this interface is static because the components of a vector are reached through the
/// vector itself, the properties of the type stay beside the members of the interface for the code that names
/// the type of the vector</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector2Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector2Components<TSelf, TScalar>
    where TScalar : unmanaged
{
    /// <summary>
    /// Returns the <c>x</c> component of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The <c>x</c> component</returns>
    public static abstract TScalar get_x(in TSelf self);

    /// <summary>
    /// Sets the <c>x</c> component of <paramref name="self"/> to <paramref name="value"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the <c>x</c> component</param>
    public static abstract void set_x(ref TSelf self, TScalar value);

    /// <summary>
    /// Returns the <c>y</c> component of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The <c>y</c> component</returns>
    public static abstract TScalar get_y(in TSelf self);

    /// <summary>
    /// Sets the <c>y</c> component of <paramref name="self"/> to <paramref name="value"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the <c>y</c> component</param>
    public static abstract void set_y(ref TSelf self, TScalar value);

    /// <summary>
    /// Returns the red component of <paramref name="self"/>
    /// <para>It is the same as <see cref="get_x"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The red component</returns>
    public static abstract TScalar get_r(in TSelf self);

    /// <summary>
    /// Sets the red component of <paramref name="self"/> to <paramref name="value"/>
    /// <para>It is the same as <see cref="set_x"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the red component</param>
    public static abstract void set_r(ref TSelf self, TScalar value);

    /// <summary>
    /// Returns the green component of <paramref name="self"/>
    /// <para>It is the same as <see cref="get_y"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The green component</returns>
    public static abstract TScalar get_g(in TSelf self);

    /// <summary>
    /// Sets the green component of <paramref name="self"/> to <paramref name="value"/>
    /// <para>It is the same as <see cref="set_y"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the green component</param>
    public static abstract void set_g(ref TSelf self, TScalar value);
}

/// <summary>
/// A vector of 3 components whose components are reached by name
/// <para>Every component has two names: the <c>xyzw</c> spelling of its position and the <c>rgba</c> spelling
/// of a color. The two names reach the same component, so writing one of them is the same as writing the
/// other one</para>
/// <para>A member of this interface is static because the components of a vector are reached through the
/// vector itself, the properties of the type stay beside the members of the interface for the code that names
/// the type of the vector</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector3Components<TSelf, TScalar> : IVector2Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector3Components<TSelf, TScalar>
    where TScalar : unmanaged
{
    /// <summary>
    /// Returns the <c>z</c> component of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The <c>z</c> component</returns>
    public static abstract TScalar get_z(in TSelf self);

    /// <summary>
    /// Sets the <c>z</c> component of <paramref name="self"/> to <paramref name="value"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the <c>z</c> component</param>
    public static abstract void set_z(ref TSelf self, TScalar value);

    /// <summary>
    /// Returns the blue component of <paramref name="self"/>
    /// <para>It is the same as <see cref="get_z"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The blue component</returns>
    public static abstract TScalar get_b(in TSelf self);

    /// <summary>
    /// Sets the blue component of <paramref name="self"/> to <paramref name="value"/>
    /// <para>It is the same as <see cref="set_z"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the blue component</param>
    public static abstract void set_b(ref TSelf self, TScalar value);
}

/// <summary>
/// A vector of 4 components whose components are reached by name
/// <para>Every component has two names: the <c>xyzw</c> spelling of its position and the <c>rgba</c> spelling
/// of a color. The two names reach the same component, so writing one of them is the same as writing the
/// other one</para>
/// <para>A member of this interface is static because the components of a vector are reached through the
/// vector itself, the properties of the type stay beside the members of the interface for the code that names
/// the type of the vector</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector4Components<TSelf, TScalar> : IVector3Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector4Components<TSelf, TScalar>
    where TScalar : unmanaged
{
    /// <summary>
    /// Returns the <c>w</c> component of <paramref name="self"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The <c>w</c> component</returns>
    public static abstract TScalar get_w(in TSelf self);

    /// <summary>
    /// Sets the <c>w</c> component of <paramref name="self"/> to <paramref name="value"/>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the <c>w</c> component</param>
    public static abstract void set_w(ref TSelf self, TScalar value);

    /// <summary>
    /// Returns the alpha component of <paramref name="self"/>
    /// <para>It is the same as <see cref="get_w"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <returns>The alpha component</returns>
    public static abstract TScalar get_a(in TSelf self);

    /// <summary>
    /// Sets the alpha component of <paramref name="self"/> to <paramref name="value"/>
    /// <para>It is the same as <see cref="set_w"/></para>
    /// </summary>
    /// <param name="self">The vector</param>
    /// <param name="value">The value of the alpha component</param>
    public static abstract void set_a(ref TSelf self, TScalar value);
}
