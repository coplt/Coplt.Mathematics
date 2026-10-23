namespace Coplt.Mathematics.Algebras.Generics.Dispatch;

#region Map Visitor

/// <summary>
/// The visitor that builds a value of the kind of the value it is handed out of it
/// <para>The value hands itself over instead of a component of it, so the visitor decides the type of a
/// component of the value it reaches and a caller that only knows the value itself reaches the map of it. The
/// members are leveled by the kind of a component of the value: the map of a floating point number falls back to
/// the map of a number and that falls back to the map of every value, so a visitor implements the member of the
/// level of the kind it reaches alone. It is the visitor of
/// <see cref="IAlgebraDispatch{TSelf}.Map_Self{V}(TSelf)"/>.</para>
/// </summary>
/// <typeparam name="V">The type of the visitor itself</typeparam>
public interface IMapVisitor<V> where V : IMapVisitor<V>
{
    #region Map

    /// <summary>Maps the whole value of a vector</summary>
    /// <typeparam name="TVector">The type of the value, a vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the value</typeparam>
    /// <param name="value">The value to map</param>
    /// <returns>The value the visitor built out of the value</returns>
    public static virtual TVector Map_Any<TVector, TScalar>(TVector value)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IVector<TVector, TScalar>
        where TScalar : unmanaged => throw null!;

    /// <summary>Maps the whole value of a vector of a number</summary>
    /// <typeparam name="TVector">The type of the value, a vector of a number</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the value</typeparam>
    /// <param name="value">The value to map</param>
    /// <returns>The value the visitor built out of the value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Map_Number<TVector, TScalar>(TVector value)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, INumberVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar> => V.Map_Any<TVector, TScalar>(value);

    /// <summary>Maps the whole value of a vector of a floating point number</summary>
    /// <typeparam name="TVector">The type of the value, a vector of a floating point number</typeparam>
    /// <typeparam name="TScalar">The type of a single component of the value</typeparam>
    /// <param name="value">The value to map</param>
    /// <returns>The value the visitor built out of the value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static virtual TVector Map_Float<TVector, TScalar>(TVector value)
        where TVector : unmanaged, IAlgebraDispatch<TVector, TScalar>, IFloatingPointVector<TVector, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar> => V.Map_Number<TVector, TScalar>(value);

    #endregion
}

#endregion
