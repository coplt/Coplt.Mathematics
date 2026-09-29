namespace Coplt.Mathematics.Algebras.Generics;

/// <summary>
/// The dispatch of the value of a number to the visitor that reaches it
/// <para>The dispatch of a value is the interface of the new design,
/// <c>Coplt.Mathematics.Algebras.Generics.Dispatch.IAlgebraDispatch</c>. The two interfaces of the reduction
/// of a matrix, <see cref="INumberMatrixColumnDispatch{TSelf,TVector}"/> and
/// <see cref="INumberMatrixRowDispatch{TSelf,TVector}"/>, still name this one, so it is kept as the marker of
/// the values they reach until the reduction of a matrix moves to the new design as well.</para>
/// </summary>
/// <typeparam name="TSelf">The type of the value itself</typeparam>
public interface INumberAlgebraDispatch<TSelf> : INumberAlgebra<TSelf>
    where TSelf : unmanaged, INumberAlgebraDispatch<TSelf>;
