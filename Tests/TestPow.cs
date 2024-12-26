// using System.Runtime.Intrinsics;
// using Coplt.Mathematics;
//
// #if NET8_0_OR_GREATER
// using Coplt.Mathematics.Simd;
// namespace Tests;
//
// [Parallelizable]
// public class TestPow
// {
//     [Test]
//     [Parallelizable]
//     public void FloatTestPow([Random(-10f, 10.0f, 100)] float x, [Random(-10, 10, 10)] float y)
//     {
//         var a = simd.Pow(new float4(x).UnsafeGetInner(), new float4(y).UnsafeGetInner()).GetElement(0);
//         var b = MathF.Pow(x, y);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(50).Ulps);
//     }
//     
//     [Test]
//     [Parallelizable]
//     public void FloatTestPowOf2([Range(0f, 32f, 1f)] float y)
//     {
//         var a = simd.Pow(new float4(2).UnsafeGetInner(), new float4(y).UnsafeGetInner()).GetElement(0);
//         var b = MathF.Pow(2, y);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(20).Ulps);
//     }
//   
//     [Test]
//     [Parallelizable]
//     public void DoubleTestPow([Random(-10, 100.0, 100)] double x, [Random(-100, 100, 10)] double y)
//     {
//         var a = simd.Pow(new double4(x).UnsafeGetInner(), new double4(y).UnsafeGetInner()).GetElement(0);
//         var b = Math.Pow(x, y);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(2000).Ulps);
//     }
//     
//     [Test]
//     [Parallelizable]
//     public void DoubleTestPowOf2([Range(0.0, 32.0, 1.0)] double y)
//     {
//         var a = simd.Pow(new double4(2).UnsafeGetInner(), new double4(y).UnsafeGetInner()).GetElement(0);
//         var b = Math.Pow(2, y);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(2000).Ulps);
//     }
// }
//
// #endif
