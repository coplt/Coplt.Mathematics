using System.Runtime.Intrinsics;

namespace Coplt.Mathematics;

public static class Foo
{
    public static Vec32x3<float> Some(float a)
    {
        return new(Vector128.Create(a, a, a, 0));
    }
}
