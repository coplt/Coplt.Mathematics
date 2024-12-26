// using System.Runtime.Intrinsics;
// using Coplt.Mathematics;
//
// #if NET8_0_OR_GREATER
// using Coplt.Mathematics.Simd;
// namespace Tests;
//
// [Parallelizable]
// public class TestTan
// {
//     [Test]
//     [Parallelizable]
//     public void FloatTestTan([Random(-10f, 10.0f, 100)] float x)
//     {
//         var a = simd_math.Tan(new float4(x).UnsafeGetInner()).GetElement(0);
//         var b = MathF.Tan(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(8000).Ulps);
//     }
//
//     [Test]
//     [Parallelizable]
//     public void DoubleTestTan([Random(-10, 10.0, 100)] double x)
//     {
//         var a = simd_math.Tan(new double4(x).UnsafeGetInner()).GetElement(0);
//         var b = Math.Tan(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(7000).Ulps);
//     }
// }
//
// #endif
