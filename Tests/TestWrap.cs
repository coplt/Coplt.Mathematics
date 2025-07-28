using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using Coplt.Mathematics;
#if NET8_0_OR_GREATER
using Coplt.Mathematics.Simd;
#endif

namespace Tests;

[Parallelizable]
public class TestWrap
{
    [MethodImpl(256 | 512)]
    public static float Wrap(float x, float min, float max)
    {
        var add = x >= 0 ? min : max;
        var off = x % (max - min);
        return add + off;
    }

    [Test]
    public void Test1()
    {
        var a = simd_math.Wrap(Vector128.Create(456f), Vector128.Create(-180f), Vector128.Create(180f));
        var b = new float4(456).wrap(-180f, 180f);
        var c = 456f.wrap(-180f, 180f);
        var d = Wrap(456f, -180f, 180f);

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);

        Assert.That(a[0], Is.EqualTo(b.x));
        Assert.That(b.x, Is.EqualTo(c));
        Assert.That(c, Is.EqualTo(d));
    }
}
