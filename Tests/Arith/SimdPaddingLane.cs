using System.Runtime.Intrinsics;
using Coplt.Experimental.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The padding lane checks of the simd backed 3 component vectors. They store their components in a hardware
/// vector, so they have to keep the fourth lane at zero, the comparison masks, the reductions and the equality
/// checks rely on it. The vectors that are not simd backed do not have such a lane.
/// </summary>
public class TestSimdPaddingLane
{
    [Test]
    public void ConstructorsMaskThePaddingLane()
    {
        Assert.That(new float3(1, 2, 3).vector.GetElement(3), Is.EqualTo(0f), "float3 from components");
        Assert.That(new double3(1, 2, 3).vector.GetElement(3), Is.EqualTo(0d), "double3 from components");
        Assert.That(new int3(1, 2, 3).vector.GetElement(3), Is.EqualTo(0), "int3 from components");
        Assert.That(new uint3(1, 2, 3).vector.GetElement(3), Is.EqualTo(0u), "uint3 from components");
        Assert.That(new long3(1, 2, 3).vector.GetElement(3), Is.EqualTo(0L), "long3 from components");
        Assert.That(new ulong3(1, 2, 3).vector.GetElement(3), Is.EqualTo(0UL), "ulong3 from components");

        Assert.That(new float3(1).vector.GetElement(3), Is.EqualTo(0f), "float3 broadcast");
        Assert.That(float3.Scalar(1).vector.GetElement(3), Is.EqualTo(0f), "float3 scalar");

        // a vector built from a raw simd value is masked too
        Assert.That(new float3(Vector128.Create(1f, 2f, 3f, 42f)).vector.GetElement(3), Is.EqualTo(0f),
            "float3 from a raw simd value");
        Assert.That(new int3(Vector128.Create(1, 2, 3, 42)).vector.GetElement(3), Is.EqualTo(0),
            "int3 from a raw simd value");
        Assert.That(new double3(Vector256.Create(1d, 2d, 3d, 42d)).vector.GetElement(3), Is.EqualTo(0d),
            "double3 from a raw simd value");
        Assert.That(new long3(Vector256.Create(1L, 2L, 3L, 42L)).vector.GetElement(3), Is.EqualTo(0L),
            "long3 from a raw simd value");

        // reading a whole simd register from a load is masked too
        Assert.That(new float3(new[] { 1f, 2f, 3f, 42f }).vector.GetElement(3), Is.EqualTo(0f),
            "float3 loaded from a span");
        Assert.That(new int3(new[] { 1, 2, 3, 42 }).vector.GetElement(3), Is.EqualTo(0),
            "int3 loaded from a span");
    }

    [Test]
    public void Float()
        => ArithCheck.PaddingStaysZero<float3, float>(true, static v => v.vector.GetElement(3));

    [Test]
    public void Double()
        => ArithCheck.PaddingStaysZero<double3, double>(true, static v => v.vector.GetElement(3));

    [Test]
    public void Int()
        => ArithCheck.PaddingStaysZero<int3, int>(true, static v => v.vector.GetElement(3));

    [Test]
    public void UInt()
        => ArithCheck.PaddingStaysZero<uint3, uint>(true, static v => v.vector.GetElement(3));

    [Test]
    public void Long()
        => ArithCheck.PaddingStaysZero<long3, long>(true, static v => v.vector.GetElement(3));

    [Test]
    public void ULong()
        => ArithCheck.PaddingStaysZero<ulong3, ulong>(true, static v => v.vector.GetElement(3));
}
