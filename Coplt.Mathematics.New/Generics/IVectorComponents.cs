namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector of 2 components whose components are reached by name
/// <para>Every component has two names: the <c>xyzw</c> spelling of its position and the <c>rgba</c> spelling
/// of a color. The two names reach the same component, so writing one of them is the same as writing the
/// other one</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector2Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector2Components<TSelf, TScalar>
    where TScalar : unmanaged
{
    /// <summary>The <c>x</c> component</summary>
    public TScalar x { get; set; }

    /// <summary>The <c>y</c> component</summary>
    public TScalar y { get; set; }

    /// <summary>The red component, it is the same as <see cref="x"/></summary>
    public TScalar r { get; set; }

    /// <summary>The green component, it is the same as <see cref="y"/></summary>
    public TScalar g { get; set; }
}

/// <summary>
/// A vector of 3 components whose components are reached by name
/// <para>Every component has two names: the <c>xyzw</c> spelling of its position and the <c>rgba</c> spelling
/// of a color. The two names reach the same component, so writing one of them is the same as writing the
/// other one</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector3Components<TSelf, TScalar> : IVector2Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector3Components<TSelf, TScalar>
    where TScalar : unmanaged
{
    /// <summary>The <c>z</c> component</summary>
    public TScalar z { get; set; }

    /// <summary>The blue component, it is the same as <see cref="z"/></summary>
    public TScalar b { get; set; }
}

/// <summary>
/// A vector of 4 components whose components are reached by name
/// <para>Every component has two names: the <c>xyzw</c> spelling of its position and the <c>rgba</c> spelling
/// of a color. The two names reach the same component, so writing one of them is the same as writing the
/// other one</para>
/// </summary>
/// <typeparam name="TSelf">The vector type itself</typeparam>
/// <typeparam name="TScalar">The type of a single component</typeparam>
public interface IVector4Components<TSelf, TScalar> : IVector3Components<TSelf, TScalar>
    where TSelf : unmanaged, IVector4Components<TSelf, TScalar>
    where TScalar : unmanaged
{
    /// <summary>The <c>w</c> component</summary>
    public TScalar w { get; set; }

    /// <summary>The alpha component, it is the same as <see cref="w"/></summary>
    public TScalar a { get; set; }
}
