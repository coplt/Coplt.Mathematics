// using System.Runtime.Intrinsics;
// using Coplt.Mathematics;
//
// #if NET8_0_OR_GREATER
// using Coplt.Mathematics.Simd;
// namespace Tests;
//
// [Parallelizable]
// public class TestAsinhAcosh
// {
//     [Test]
//     [Parallelizable]
//     public void FloatTestAsinh([Random(-10f, 10.0f, 100)] float x)
//     {
//         var a = simd_math.Asinh(new float4(x).UnsafeGetInner()).GetElement(0);
//         var b = MathF.Asinh(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(50).Ulps);
//     }
//
//     [Test]
//     [Parallelizable]
//     public void DoubleTestAsinh([Random(-10, 10.0, 100)] double x)
//     {
//         var a = simd_math.Asinh(new double4(x).UnsafeGetInner()).GetElement(0);
//         var b = Math.Asinh(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(100).Ulps);
//     }
//     
//     [Test]
//     [Parallelizable]
//     public void FloatTestAcosh([Random(1, 10.0f, 100)] float x)
//     {
//         var a = simd_math.Acosh(new float4(x).UnsafeGetInner()).GetElement(0);
//         var b = MathF.Acosh(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(10).Ulps);
//     }
//
//     [Test]
//     [Parallelizable]
//     public void DoubleTestAcosh([Random(1, 10.0, 100)] double x)
//     {
//         var a = simd_math.Acosh(new double4(x).UnsafeGetInner()).GetElement(0);
//         var b = Math.Acosh(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(10).Ulps);
//     }
//     
//     [Test]
//     [Parallelizable]
//     public void FloatTestAtanh([Random(-1.1f, 1.1f, 100)] float x)
//     {
//         var a = simd_math.Atanh(new float4(x).UnsafeGetInner()).GetElement(0);
//         var b = MathF.Atanh(x);
//         Console.WriteLine($"{a}");
//         Console.WriteLine($"{b}");
//         Assert.That(b, Is.EqualTo(a).Within(50).Ulps);
//     }
// }
//
// #endif
