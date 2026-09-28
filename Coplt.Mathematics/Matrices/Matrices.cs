namespace Coplt.Mathematics;

public struct MatC2<TVector, TScalar>
    where TVector : unmanaged
    where TScalar : unmanaged
{
    #region Fields

    public TVector c0;
    public TVector c1;

    #endregion
}

public struct MatC3<TVector, TScalar>
    where TVector : unmanaged
    where TScalar : unmanaged
{
    #region Fields

    public TVector c0;
    public TVector c1;
    public TVector c2;

    #endregion
}

public struct MatC4<TVector, TScalar>
    where TVector : unmanaged
    where TScalar : unmanaged
{
    #region Fields

    public TVector c0;
    public TVector c1;
    public TVector c2;
    public TVector c3;

    #endregion
}
