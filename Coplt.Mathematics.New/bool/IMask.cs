namespace Coplt.Mathematics;

/// <summary>
/// A mask: it says whether a condition holds, so it is either true or false
/// <para>It is the smallest form of a mask of the library, which the type of a single component of a bool
/// vector and of a bool matrix names as well as the type of the two values of it does, so a member that builds
/// a mask the shape of which it does not name, which is the shape of the value it takes, reaches the two
/// values of a condition through it</para>
/// </summary>
/// <typeparam name="TSelf">The type of the mask itself</typeparam>
public interface IMask<TSelf> : IBitwiseOperators<TSelf, TSelf, TSelf>
    where TSelf : unmanaged, IMask<TSelf>
{
    #region Constants

    /// <summary>The mask of a condition that holds</summary>
    public static abstract TSelf True { get; }

    /// <summary>The mask of a condition that does not hold</summary>
    public static abstract TSelf False { get; }

    #endregion

    #region Operators

    /// <inheritdoc cref="IBitwiseOperators{A,B,C}.operator~"/>
    public static abstract TSelf operator !(TSelf value);

    public static abstract implicit operator bool(TSelf value);
    public static abstract implicit operator TSelf(bool value);

    public static abstract bool operator true(TSelf value);
    public static abstract bool operator false(TSelf value);

    #endregion
}
