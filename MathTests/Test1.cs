using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using Coplt.Mathematics;

namespace MathTests;

public class Test1
{
    #if NET8_0_OR_GREATER
    [Test]
    public void Transpose4x4()
    {
        var c0 = new long4(1, 2, 3, 4).UnsafeGetInner();
        var c1 = new long4(5, 6, 7, 8).UnsafeGetInner();
        var c2 = new long4(9, 10, 11, 12).UnsafeGetInner();
        var c3 = new long4(13, 14, 15, 16).UnsafeGetInner();
        var r = simd_matrix.Transpose4x4(c0, c1, c2, c3);
        Console.WriteLine(r.Item1);
        Console.WriteLine(r.Item2);
        Console.WriteLine(r.Item3);
        Console.WriteLine(r.Item4);
    }
    [Test]
    public void Float()
    {
        var a = new float4(9);
        var r = simd_float.AsinhAcosh(a.UnsafeGetInner());
        Console.WriteLine(r);
        Console.WriteLine(MathF.Acosh(9));
    }
    [Test]
    public void Double()
    {
        var a = new double4(9);
        var r = simd_double.AsinhAcosh(a.UnsafeGetInner());
        Console.WriteLine(r);
        Console.WriteLine(Math.Acosh(9.0));
    }
    #endif
}
