namespace Coplt.Mathematics.SignedDistances;

public interface ISdfShape<out T, in V> where V : IVector<T>
{
    public T calc(V target);
}

public interface ISdfShape2d<out T, in V> : ISdfShape<T, V> where V : IVector2<T> { }
public interface ISdfShape3d<out T, in V> : ISdfShape<T, V> where V : IVector3<T> { }
