namespace Coplt.Mathematics;

public static class Test123
{
    private struct Vector3Functor_XYZ : IVector3Functor
    {
        [MethodImpl(256)]
        public static Vector128<T> Map<T>(Vector128<T> vector) => vector;
        [MethodImpl(256)]
        public static Vector256<T> Map<T>(Vector256<T> vector) => vector;
        [MethodImpl(256)]
        public static (T x, T y, T z) Map<T>(T x, T y, T z) => (x, y, z);
    }

    extension<V>(V self) where V : IVector2
    {
        // public Self Swizzle_XYZ => default!;
    }

    public static void Foo(float3 a)
    {
        // var b = a.Swizzle_XYZ
    }
}
