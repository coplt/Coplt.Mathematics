namespace Coplt.Mathematics.Generics;

/// <summary>
/// A vector whose kind is only known at run time
/// <para>A vector cannot be named by the type parameter of an interface, so the kind of a vector is reached by
/// dispatching it to a visitor: the members below call the member of the visitor that matches the width of the
/// register of the vector or the count of its components, and the implementation of the visitor reaches the
/// kinds of the vector one by one</para>
/// <para>The member of the visitor reaches both the type of the vector and the value of it: the type declares
/// the width of the register of the vector, the padding lanes of it and the count of its components, and the
/// value that is handed to the member is the register of a vector that keeps its value in one and the vector
/// itself when it has no register</para>
/// </summary>
public interface IDynamicVector<TSelf>
    where TSelf : unmanaged, IDynamicVector<TSelf>
{
    /// <summary>
    /// Dispatches the width of the register of the vector to <typeparamref name="V"/>
    /// </summary>
    /// <typeparam name="V">The type of the visitor to dispatch to</typeparam>
    /// <typeparam name="R">The type of the result of the visitor</typeparam>
    /// <returns>The result of the member of <typeparamref name="V"/> that matches the width of the register of the vector</returns>
    public static abstract R VisitUnderlying<V, R>(in TSelf self)
        where V : IVectorUnderlyingVisitor<R>;

    /// <summary>
    /// Dispatches the count of the components of the vector to <typeparamref name="V"/>
    /// </summary>
    /// <typeparam name="V">The type of the visitor to dispatch to</typeparam>
    /// <typeparam name="R">The type of the result of the visitor</typeparam>
    /// <returns>The result of the member of <typeparamref name="V"/> that matches the count of the components of the vector</returns>
    public static abstract R VisitDimension<V, R>(in TSelf self)
        where V : IVectorDimensionVisitor<R>;
}

/// <summary>
/// A visitor the width of the register of a vector dispatches to
/// <para>A vector of a number type dispatches to the member of the width of its register, a vector without a
/// register dispatches to <see cref="AcceptSoft{TVector,TScalar}"/> and the other ones to the member of the
/// register they keep their value in</para>
/// <para>The member of the visitor reaches the properties of the type of the vector and the value that is
/// handed to it, the register of a vector that keeps its value in one and the vector itself when it has no
/// register</para>
/// </summary>
/// <typeparam name="R">The type of the result of the member of the visitor</typeparam>
public interface IVectorUnderlyingVisitor<out R>
{
    /// <summary>
    /// Returns the result of a vector that has no register
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R AcceptSoft<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVectorSoftUnderlying
        where TScalar : unmanaged;

    /// <summary>
    /// Returns the result of a vector whose register is 64 bits wide
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The register that keeps the value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R Accept<TVector, TScalar>(in Vector64<TScalar> vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVector64Underlying<TVector>
        where TScalar : unmanaged;

    /// <summary>
    /// Returns the result of a vector whose register is 128 bits wide
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The register that keeps the value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R Accept<TVector, TScalar>(in Vector128<TScalar> vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVector128Underlying<TVector>
        where TScalar : unmanaged;

    /// <summary>
    /// Returns the result of a vector whose register is 256 bits wide
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The register that keeps the value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R Accept<TVector, TScalar>(in Vector256<TScalar> vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVector256Underlying<TVector>
        where TScalar : unmanaged;
}

/// <summary>
/// A visitor the count of the components of a vector dispatches to
/// <para>A vector of 2 components dispatches to <see cref="AcceptVector2{TVector,TScalar}"/>, a vector of 3
/// components to the member of 3 and a vector of 4 components to the member of 4</para>
/// <para>The member of the visitor reaches the properties of the type of the vector and the value that is
/// handed to it</para>
/// </summary>
/// <typeparam name="R">The type of the result of the member of the visitor</typeparam>
public interface IVectorDimensionVisitor<out R>
{
    /// <summary>
    /// Returns the result of a vector of 2 components
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R AcceptVector2<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVector2<TVector, TScalar>
        where TScalar : unmanaged;

    /// <summary>
    /// Returns the result of a vector of 3 components
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R AcceptVector3<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVector3<TVector, TScalar>
        where TScalar : unmanaged;

    /// <summary>
    /// Returns the result of a vector of 4 components
    /// </summary>
    /// <typeparam name="TVector">The type of the vector that dispatched the call</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the vector</typeparam>
    /// <param name="vector">The value of the vector that dispatched the call</param>
    /// <returns>The result of the visitor</returns>
    public static abstract R AcceptVector4<TVector, TScalar>(in TVector vector)
        where TVector : unmanaged, IDynamicVector<TVector>,
        INumberVector<TVector, TScalar>, IVector4<TVector, TScalar>
        where TScalar : unmanaged;
}
