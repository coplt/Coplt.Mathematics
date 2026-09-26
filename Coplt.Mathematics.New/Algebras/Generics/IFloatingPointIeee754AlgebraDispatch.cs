namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The dispatch of the value of an algebra of a floating point number that names the ieee 754 standard to the
/// visitor that reaches it
/// <para>It is the dispatch of its own beside the one of a floating point number: the visitors of the kind of it
/// reach the members the standard names of a value as well, which the visitors of the kind of a floating point
/// number do not</para>
/// <para>The dispatch of a floating point kind reaches the values of the kind of it alone, so it is named with
/// the type of the value and not with the type of a component of it, which every member of it reaches the type
/// of through the visitor of it</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface IFloatingPointIeee754AlgebraDispatch<TSelf> :
    IFloatingPointIeee754Algebra<TSelf>
    where TSelf : unmanaged, IFloatingPointIeee754AlgebraDispatch<TSelf>
{
    /// <inheritdoc cref="IFloatingPointAlgebraDispatch{TSelf}.Visit_Self{V}(in TSelf)"/>
    public static abstract TSelf Visit_Self<V>(in TSelf self)
        where V : IFloatingPointIeee754AlgebraVisitor_Self_Self<V>;

    /// <inheritdoc cref="IFloatingPointAlgebraDispatch{TSelf}.Visit_Self{V}(in TSelf, in TSelf)"/>
    public static abstract TSelf Visit_Self<V>(in TSelf a, in TSelf b)
        where V : IFloatingPointIeee754AlgebraVisitor_Self_Self_Self<V>;

    /// <inheritdoc cref="IFloatingPointAlgebraDispatch{TSelf}.Visit_Self{V}(in TSelf, in TSelf, in TSelf)"/>
    public static abstract TSelf Visit_Self<V>(in TSelf a, in TSelf b, in TSelf c)
        where V : IFloatingPointIeee754AlgebraVisitor_Self_Self_Self_Self<V>;
}
