namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The dispatch of the value of an algebra of a floating point number to the visitor that reaches it
/// <para>It is the dispatch of its own beside the one of a number: the visitors of a floating point kind reach
/// the members the kind of it has of its own, which the visitors of a number do not, and the visitors of a
/// number reach the members of the kind of it of a floating point value as well</para>
/// <para>The dispatch of a floating point kind reaches the values of the kind of it alone, so it is named with
/// the type of the value and not with the type of a component of it, which every member of it reaches the type
/// of through the visitor of it</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface IFloatingPointAlgebraDispatch<TSelf> :
    IFloatingPointAlgebra<TSelf>
    where TSelf : unmanaged, IFloatingPointAlgebraDispatch<TSelf>
{
    /// <summary>Hands the value of <paramref name="self"/> to <typeparamref name="V"/></summary>
    /// <typeparam name="V">The type of the visitor that reaches the value</typeparam>
    /// <param name="self">The value to hand over</param>
    /// <returns>The value the visitor built</returns>
    public static abstract TSelf Visit_Self<V>(in TSelf self)
        where V : IFloatingPointAlgebraVisitor_Self_Self<V>;

    /// <summary>Hands the value of <paramref name="a"/> and <paramref name="b"/> to <typeparamref name="V"/></summary>
    /// <typeparam name="V">The type of the visitor that reaches the values</typeparam>
    /// <param name="a">The first value to hand over</param>
    /// <param name="b">The second value to hand over</param>
    /// <returns>The value the visitor built</returns>
    public static abstract TSelf Visit_Self<V>(in TSelf a, in TSelf b)
        where V : IFloatingPointAlgebraVisitor_Self_Self_Self<V>;

    /// <summary>Hands the value of <paramref name="a"/>, <paramref name="b"/> and <paramref name="c"/> to <typeparamref name="V"/></summary>
    /// <typeparam name="V">The type of the visitor that reaches the values</typeparam>
    /// <param name="a">The first value to hand over</param>
    /// <param name="b">The second value to hand over</param>
    /// <param name="c">The third value to hand over</param>
    /// <returns>The value the visitor built</returns>
    public static abstract TSelf Visit_Self<V>(in TSelf a, in TSelf b, in TSelf c)
        where V : IFloatingPointAlgebraVisitor_Self_Self_Self_Self<V>;
}
