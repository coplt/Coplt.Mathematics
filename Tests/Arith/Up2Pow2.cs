using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rounding up to the next power of two of every component of a number, which is the power of two of the kind
/// of a component that is not less than the component and the smallest one of them. A power of two answers with
/// itself, the zero and the negative of a value answer with the zero of the kind, the infinity answers with itself
/// and the nan with itself.
/// </summary>
public class TestUp2Pow2
{
    /// <summary>
    /// The member is one of the algebra of the kind of the value, so a parameter that only knows the interfaces
    /// of it reaches the member as well.
    /// </summary>
    private static void Check<T>(T value)
        where T : unmanaged, IAlgebraDispatch<T>, INumberAlgebra<T>
        => _ = math.up2_pow2(value);

    [Test]
    public void FloatingPoint()
    {
        using (Assert.EnterMultipleScope())
        {
            // a power of two answers with itself
            Assert.That(math.up2_pow2(new float3(1f, 2f, 4f)), Is.EqualTo(new float3(1f, 2f, 4f)));
            Assert.That(math.up2_pow2(new double3(1, 2, 4)), Is.EqualTo(new double3(1, 2, 4)));

            // a value between two powers of two answers with the larger one of them, above the one as well as
            // below it
            Assert.That(math.up2_pow2(new float3(3f, 5f, 0.75f)), Is.EqualTo(new float3(4f, 8f, 1f)));
            Assert.That(math.up2_pow2(new double3(3, 5, 0.75)), Is.EqualTo(new double3(4, 8, 1)));
            Assert.That(math.up2_pow2(new float3(0.3f, 1.5f, 6f)), Is.EqualTo(new float3(0.5f, 2f, 8f)));

            // a subnormal answers with the power of two that is not less than it, which the smallest one of them
            // is as well
            Assert.That(math.up2_pow2(new float3(float.Epsilon, float.Epsilon * 3f, float.PositiveInfinity)),
                Is.EqualTo(new float3(float.Epsilon, float.Epsilon * 4f, float.PositiveInfinity)));

            // the zero, the negative of a value and the negative infinity answer with the zero, the nan answers
            // with itself
            Assert.That(math.up2_pow2(new float3(0f, float.NegativeInfinity, -0f)), Is.EqualTo(new float3(0f, 0f, 0f)));
            Assert.That(math.up2_pow2(new float3(-4f, -0.75f, 0.5f)), Is.EqualTo(new float3(0f, 0f, 0.5f)));
            Assert.That(float.IsNaN(math.up2_pow2(new float3(float.NaN, 1f, 2f)).x), Is.True);

            // a value without a register rounds the components with the kind of them
            Assert.That(math.up2_pow2(new half3((half)1f, (half)3f, (half)4f)),
                Is.EqualTo(new half3((half)1f, (half)4f, (half)4f)));
            Assert.That(math.up2_pow2(new half3((half)0f, (half)(-3f), (half)5f)),
                Is.EqualTo(new half3((half)0f, (half)0f, (half)8f)));

            // a matrix is a value of the same algebra
            Assert.That(math.up2_pow2(new float2x2(new float2(1f, 3f), new float2(4f, 6f))),
                Is.EqualTo(new float2x2(new float2(1f, 4f), new float2(4f, 8f))));
        }
    }

    /// <summary>
    /// The rounding of an integer kind: a power of two answers with itself, a value between two of them with the
    /// larger one, and the zero and the negative of a value with the zero. The top bit of a kind is the power of
    /// two of the smallest value of it, which a value that has it set answers with, and the rounding of a value
    /// without a sign that has the whole of the kind set overflows it and answers with the zero.
    /// </summary>
    [Test]
    public void Integer()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.up2_pow2(new int3(1, 3, 4)), Is.EqualTo(new int3(1, 4, 4)));
            Assert.That(math.up2_pow2(new int3(5, 8, 0)), Is.EqualTo(new int3(8, 8, 0)));
            Assert.That(math.up2_pow2(new int3(-1, -4, 9)), Is.EqualTo(new int3(0, 0, 16)));
            Assert.That(math.up2_pow2(new int3(int.MinValue, 0, 6)), Is.EqualTo(new int3(int.MinValue, 0, 8)));
            Assert.That(math.up2_pow2(new uint3(3u, 0x8000_0000u, 0u)),
                Is.EqualTo(new uint3(4u, 0x8000_0000u, 0u)));
            Assert.That(math.up2_pow2(new uint3(uint.MaxValue, 1u, 2u)), Is.EqualTo(new uint3(0u, 1u, 2u)));

            // a value of a kind without a register reaches the member of the framework for a component
            Assert.That(math.up2_pow2(new short3(1, 3, 5)), Is.EqualTo(new short3(1, 4, 8)));
            Assert.That(math.up2_pow2(new short3(0, -4, 4)), Is.EqualTo(new short3(0, 0, 4)));
            Assert.That(math.up2_pow2(new ushort3(3, 0x8000, 0xFFFF)), Is.EqualTo(new ushort3(4, 0x8000, 0)));

            Assert.That(math.up2_pow2(new long3(3, -4, 6)), Is.EqualTo(new long3(4, 0, 8)));
            Assert.That(math.up2_pow2(new long3(long.MinValue, 0, 6)),
                Is.EqualTo(new long3(long.MinValue, 0, 8)));
            Assert.That(math.up2_pow2(new ulong3(3ul, 0ul, 0x8000_0000_0000_0000ul)),
                Is.EqualTo(new ulong3(4ul, 0ul, 0x8000_0000_0000_0000ul)));
        }
    }

    /// <summary>
    /// The rounding of a component is the one of the framework for the kind of it, which the member is written with
    /// for a value of a kind without a register and the one the helper of the simd library follows for a value that
    /// has one.
    /// </summary>
    [Test]
    public void Framework()
    {
        using (Assert.EnterMultipleScope())
        {
            var u = math.up2_pow2(new uint4(0u, 3u, 4u, 0x8000_0000u));
            Assert.That(u.x, Is.EqualTo(BitOperations.RoundUpToPowerOf2(0u)));
            Assert.That(u.y, Is.EqualTo(BitOperations.RoundUpToPowerOf2(3u)));
            Assert.That(u.z, Is.EqualTo(BitOperations.RoundUpToPowerOf2(4u)));
            Assert.That(u.w, Is.EqualTo(BitOperations.RoundUpToPowerOf2(0x8000_0000u)));

            var ul = math.up2_pow2(new ulong4(0ul, 3ul, 4ul, 0x8000_0000_0000_0000ul));
            Assert.That(ul.x, Is.EqualTo(BitOperations.RoundUpToPowerOf2(0ul)));
            Assert.That(ul.y, Is.EqualTo(BitOperations.RoundUpToPowerOf2(3ul)));
            Assert.That(ul.z, Is.EqualTo(BitOperations.RoundUpToPowerOf2(4ul)));
            Assert.That(ul.w, Is.EqualTo(BitOperations.RoundUpToPowerOf2(0x8000_0000_0000_0000ul)));

            var us = math.up2_pow2(new ushort4(0, 3, 4, 0x8000));
            Assert.That(us.x, Is.EqualTo(BitOperations.RoundUpToPowerOf2((ushort)0)));
            Assert.That(us.y, Is.EqualTo(BitOperations.RoundUpToPowerOf2((ushort)3)));
            Assert.That(us.z, Is.EqualTo(BitOperations.RoundUpToPowerOf2((ushort)4)));
            Assert.That(us.w, Is.EqualTo(BitOperations.RoundUpToPowerOf2((ushort)0x8000)));
        }
    }

    /// <summary>
    /// The zero of a padding lane rounds up to itself, so the lane keeps its zero.
    /// </summary>
    [Test]
    public void Padding()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.up2_pow2(new float3(3f, 5f, 6f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.up2_pow2(new double3(3, 5, 6)).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(math.up2_pow2(new int3(3, 5, 6)).vector.GetElement(3), Is.EqualTo(0));
            Assert.That(math.up2_pow2(new long3(3, 5, 6)).vector.GetElement(3), Is.EqualTo(0L));
        }
    }

    [Test]
    public void Interface()
    {
        Check(new float3(1f, 3f, 4f));
        Check(new double3(1, 3, 4));
        Check(new half3((half)1f, (half)3f, (half)4f));
        Check(new short3(1, 3, 4));
        Check(new ushort3(1, 3, 4));
        Check(new int3(1, 3, 4));
        Check(new uint3(1, 3, 4));
        Check(new long3(1, 3, 4));
        Check(new ulong3(1, 3, 4));
    }
}
