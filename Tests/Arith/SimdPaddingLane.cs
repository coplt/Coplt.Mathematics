using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The padding lane checks of the simd backed vectors whose register is wider than the vector. A 3 component
/// vector stores its components in a 128 or 256 bit register and a 2 component one in a 128 bit register, so
/// both have to keep their padding lanes at zero, the comparison masks, the reductions and the equality checks
/// rely on it. The vectors that are not simd backed do not have such lanes.
/// </summary>
public class TestSimdPaddingLane
{
    [Test]
    public void ConstructorsMaskThePaddingLane()
    {
        Assert.That(new float2(1, 2).vector.GetElement(2), Is.EqualTo(0f), "float2 padding from components");
        Assert.That(new float2(1, 2).vector.GetElement(3), Is.EqualTo(0f), "float2 padding from components");
        Assert.That(new float2(Vector128.Create(1f, 2f, 42f, 42f)).vector.GetElement(3), Is.EqualTo(0f),
            "float2 from a raw simd value");
        Assert.That(new int2(Vector128.Create(1, 2, 42, 42)).vector.GetElement(2), Is.EqualTo(0),
            "int2 from a raw simd value");
        Assert.That(new uint2(Vector128.Create(1u, 2u, 42u, 42u)).vector.GetElement(3), Is.EqualTo(0u),
            "uint2 from a raw simd value");
        Assert.That(new float2(1).vector.GetElement(3), Is.EqualTo(0f), "float2 broadcast");
        Assert.That(new float2(new[] { 1f, 2f, 42f, 42f }).vector.GetElement(3), Is.EqualTo(0f),
            "float2 loaded from a span");

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

    /// <summary>
    /// Builds a nan out of a value that is handed over: the division is not a constant of the compiler, so the
    /// creation of a vector from the value it returns is the one of a value that is only known at run time
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float Nan(float value) => value / value;

    /// <inheritdoc cref="Nan(float)"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static double Nan(double value) => value / value;

    [Test]
    public void BroadcastOfComputedNan()
    {
        // the creation of a vector from a component broadcasts it with a shuffle of the register of the scalar,
        // which holds the scalar in the first of its lanes and leaves the ones that follow it unset: a nan that
        // is only known at run time tells whether the lanes of the result that are not a component stay zero
        var nan = Nan(0f);
        var nanD = Nan(0d);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float.IsNaN(nan), Is.True, "the computed value is a nan");
            Assert.That(double.IsNaN(nanD), Is.True, "the computed value is a nan");

            var f2 = new float2(nan);
            Assert.That(float.IsNaN(f2.x) && float.IsNaN(f2.y), Is.True, "float2 broadcast keeps the nan");
            Assert.That(f2.vector.GetElement(2), Is.EqualTo(0f), "float2 broadcast padding");
            Assert.That(f2.vector.GetElement(3), Is.EqualTo(0f), "float2 broadcast padding");

            var f3 = new float3(nan);
            Assert.That(float.IsNaN(f3.x) && float.IsNaN(f3.y) && float.IsNaN(f3.z), Is.True,
                "float3 broadcast keeps the nan");
            Assert.That(f3.vector.GetElement(3), Is.EqualTo(0f), "float3 broadcast padding");

            var d3 = new double3(nanD);
            Assert.That(double.IsNaN(d3.x) && double.IsNaN(d3.y) && double.IsNaN(d3.z), Is.True,
                "double3 broadcast keeps the nan");
            Assert.That(d3.vector.GetElement(3), Is.EqualTo(0d), "double3 broadcast padding");

            // only the x component of a scalar is set, the ones after it are zero
            var s3 = float3.Scalar(nan);
            Assert.That(float.IsNaN(s3.x), Is.True, "float3 scalar keeps the nan");
            Assert.That(s3.y, Is.EqualTo(0f), "float3 scalar zeroes the components after it");
            Assert.That(s3.z, Is.EqualTo(0f), "float3 scalar zeroes the components after it");
            Assert.That(s3.vector.GetElement(3), Is.EqualTo(0f), "float3 scalar padding");
        }
    }

    /// <summary>
    /// The platforms whose hardware zeroes the lanes that follow the one of the register of a scalar: an
    /// avx encoded scalar instruction and the one of arm64 write only the lane of the scalar and leave the ones
    /// that follow the one of it at zero, which is what the unsafe members are for. The zeroing is the one of
    /// the 128 bit register of the scalar, the upper half of a 256 bit one is the one the platform decides
    /// </summary>
    private static bool ScalarLanesAreZeroed => Avx.IsSupported || AdvSimd.Arm64.IsSupported;

    [Test]
    public void BroadcastUnsafeOfComputedNan()
    {
        // the unsafe members leave the lanes that follow the one of the register of a scalar as the register of
        // the scalar does, which only leaves them at zero on a platform whose hardware does: the check is the
        // one the caller of an unsafe member makes
        if (!ScalarLanesAreZeroed)
            Assert.Ignore("the hardware of the platform does not zero the lanes that follow the one of a scalar");

        var nan = Nan(0f);
        var nanD = Nan(0d);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float.IsNaN(nan), Is.True, "the computed value is a nan");
            Assert.That(double.IsNaN(nanD), Is.True, "the computed value is a nan");

            var f2 = float2.BroadcastUnsafe(nan);
            Assert.That(float.IsNaN(f2.x) && float.IsNaN(f2.y), Is.True, "float2 broadcast unsafe keeps the nan");
            Assert.That(f2.vector.GetElement(2), Is.EqualTo(0f), "float2 broadcast unsafe padding");
            Assert.That(f2.vector.GetElement(3), Is.EqualTo(0f), "float2 broadcast unsafe padding");

            var f3 = float3.BroadcastUnsafe(nan);
            Assert.That(float.IsNaN(f3.x) && float.IsNaN(f3.y) && float.IsNaN(f3.z), Is.True,
                "float3 broadcast unsafe keeps the nan");
            Assert.That(f3.vector.GetElement(3), Is.EqualTo(0f), "float3 broadcast unsafe padding");

            // the register of the value of 3 doubles is 256 bits wide, the zeroing that follows the scalar is
            // the one of the 128 bit register of it, so the padding lane of the upper half is the one the
            // platform decides and only the components are checked here
            var d3 = double3.BroadcastUnsafe(nanD);
            Assert.That(double.IsNaN(d3.x) && double.IsNaN(d3.y) && double.IsNaN(d3.z), Is.True,
                "double3 broadcast unsafe keeps the nan");

            // the broadcast of a value is the one of the unsafe broadcast and the scalar of a value is the one
            // of the unsafe scalar on every platform
            Assert.That(float2.BroadcastUnsafe(2f), Is.EqualTo(float2.Broadcast(2f)), "float2 broadcast unsafe");
            Assert.That(float3.BroadcastUnsafe(2f), Is.EqualTo(float3.Broadcast(2f)), "float3 broadcast unsafe");
            var b3 = double3.BroadcastUnsafe(2d);
            Assert.That(b3.x, Is.EqualTo(2d), "double3 broadcast unsafe");
            Assert.That(b3.y, Is.EqualTo(2d), "double3 broadcast unsafe");
            Assert.That(b3.z, Is.EqualTo(2d), "double3 broadcast unsafe");
            Assert.That(int3.BroadcastUnsafe(2), Is.EqualTo(int3.Broadcast(2)), "int3 broadcast unsafe");

            var s3 = float3.ScalarUnsafe(nan);
            Assert.That(float.IsNaN(s3.x), Is.True, "float3 scalar unsafe keeps the nan");
            Assert.That(s3.y, Is.EqualTo(0f), "float3 scalar unsafe zeroes the components after the one");
            Assert.That(s3.z, Is.EqualTo(0f), "float3 scalar unsafe zeroes the components after the one");
            Assert.That(s3.vector.GetElement(3), Is.EqualTo(0f), "float3 scalar unsafe padding");
            Assert.That(float3.ScalarUnsafe(2f), Is.EqualTo(float3.Scalar(2f)), "float3 scalar unsafe");
            var u3 = double3.ScalarUnsafe(2d);
            Assert.That(u3.x, Is.EqualTo(2d), "double3 scalar unsafe");
            Assert.That(int3.ScalarUnsafe(2), Is.EqualTo(int3.Scalar(2)), "int3 scalar unsafe");
        }
    }

    [Test]
    public void Float()
        => ArithCheck.PaddingStaysZero<float3, float>(true, static v => v.vector.GetElement(3));

    [Test]
    public void PaddingOfFloat2Low()
        => ArithCheck.PaddingStaysZero2<float2, float>(true, static v => v.vector.GetElement(2));

    [Test]
    public void PaddingOfFloat2High()
        => ArithCheck.PaddingStaysZero2<float2, float>(true, static v => v.vector.GetElement(3));

    [Test]
    public void PaddingOfInt2()
        => ArithCheck.PaddingStaysZero2<int2, int>(true, static v => v.vector.GetElement(3));

    [Test]
    public void PaddingOfUInt2()
        => ArithCheck.PaddingStaysZero2<uint2, uint>(true, static v => v.vector.GetElement(3));

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
