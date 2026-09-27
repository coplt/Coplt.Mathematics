namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The dispatch of the value of a floating point vector through the members of a vector of it
/// <para>The member of a visitor that maps a value reaches the members of the kind of a single component of the
/// value and the ones of a vector of it, so the map of a value is the member of a dispatch of a vector, which a
/// vector of a floating point kind implements beside the dispatch of the value: the value of a matrix is the one
/// of its columns, so a matrix has no map of it.</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface IFloatingPointVectorDispatch<TSelf> :
    IFloatingPointAlgebraDispatch<TSelf>,
    IFloatingPointVector<TSelf>
    where TSelf : unmanaged, IFloatingPointVectorDispatch<TSelf>
{
    /// <summary>
    /// Hands the value of <paramref name="self"/> to <typeparamref name="V"/>, which is the visitor of the map
    /// of a value
    /// <para>The visitor of a member of the floating point kind and the one of the map of a value reach the same
    /// shapes, so they are two families: the members of a shape are the ones of the family it belongs to, which
    /// is why the map of a value has a member of its own beside the one that reaches the visitor of the kind of
    /// the value</para>
    /// </summary>
    /// <typeparam name="V">The type of the visitor that reaches the value</typeparam>
    /// <param name="self">The value to hand over</param>
    /// <returns>The value the visitor built</returns>
    public static abstract TSelf Map_Self<V>(in TSelf self)
        where V : IFloatingPointAlgebraVisitor_Map_Self_Self<V>;

    /// <summary>
    /// Hands the value of <paramref name="a"/> and <paramref name="b"/> to <typeparamref name="V"/>, which is
    /// the visitor of the map of a value
    /// </summary>
    /// <typeparam name="V">The type of the visitor that reaches the values</typeparam>
    /// <param name="a">The first value to hand over</param>
    /// <param name="b">The second value to hand over</param>
    /// <returns>The value the visitor built</returns>
    public static abstract TSelf Map_Self<V>(in TSelf a, in TSelf b)
        where V : IFloatingPointAlgebraVisitor_Map_Self_Self_Self<V>;
}
