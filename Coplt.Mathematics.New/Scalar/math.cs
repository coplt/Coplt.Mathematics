using static Coplt.Mathematics.math;
namespace Coplt.Mathematics;

public static partial class math
{
    [MethodImpl(256)]
    public static int min(int a, int b) => Math.Min(a, b);
}

public static partial class ScalarExtensions
{
    extension(int value)
    {
        [MethodImpl(256)]
        public int min(int other) => Math.Min(value, other);
    }
}


public static class TestUseExt
{
    public static void Foo()
    {
        var a = 1;
        var b = a.min(2);
        var c = min(b, 3);
    }
}
