using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;

namespace Tests.Core;

/// <summary>
/// The storage variants of the vectors keep the same components as the regular vectors in a narrower storage: the
/// variant of a 2 component vector keeps the exact 64 bits of its value instead of a padded 128 bit register, the
/// one of a 3 component vector keeps its components in fields and has no register at all. Both convert into each
/// other and keep the components of the value.
/// </summary>
public class TestStorageVariant
{
    [Test]
    public void Float2()
    {
        var v = new float2s(1, 2);
        float2 regular = v;
        float2s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y), Is.EqualTo((1f, 2f)));
            Assert.That((regular.x, regular.y), Is.EqualTo((1f, 2f)));
            // the regular vector is widened to a 128 bit register with zero padding lanes
            Assert.That((regular.vector.GetElement(2), regular.vector.GetElement(3)), Is.EqualTo((0f, 0f)));
            // the variant keeps the exact 64 bits of its value
            Assert.That(Unsafe.SizeOf<float2s>(), Is.EqualTo(8));
            Assert.That(Unsafe.SizeOf<float2>(), Is.EqualTo(16));
            Assert.That(v.vector, Is.EqualTo(regular.vector.GetLower()));
        }
    }

    [Test]
    public void Int2()
    {
        var v = new int2s(-1, 2);
        int2 regular = v;
        int2s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y), Is.EqualTo((-1, 2)));
            Assert.That((regular.vector.GetElement(2), regular.vector.GetElement(3)), Is.EqualTo((0, 0)));
            Assert.That(Unsafe.SizeOf<int2s>(), Is.EqualTo(8));
            Assert.That(Unsafe.SizeOf<int2>(), Is.EqualTo(16));
        }
    }

    [Test]
    public void UInt2()
    {
        var v = new uint2s(1u, 2u);
        uint2 regular = v;
        uint2s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y), Is.EqualTo((1u, 2u)));
            Assert.That((regular.vector.GetElement(2), regular.vector.GetElement(3)), Is.EqualTo((0u, 0u)));
            Assert.That(Unsafe.SizeOf<uint2s>(), Is.EqualTo(8));
            Assert.That(Unsafe.SizeOf<uint2>(), Is.EqualTo(16));
        }
    }

    [Test]
    public void Float3()
    {
        var v = new float3s(1, 2, 3);
        float3 regular = v;
        float3s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y, back.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That((regular.x, regular.y, regular.z), Is.EqualTo((1f, 2f, 3f)));
            Assert.That(regular.vector.GetElement(3), Is.EqualTo(0f));
        }
    }

    [Test]
    public void Double3()
    {
        var v = new double3s(1, 2, 3);
        double3 regular = v;
        double3s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y, back.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That((regular.x, regular.y, regular.z), Is.EqualTo((1d, 2d, 3d)));
            Assert.That(regular.vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    [Test]
    public void Int3()
    {
        var v = new int3s(-1, 2, 3);
        int3 regular = v;
        int3s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y, back.z), Is.EqualTo((-1, 2, 3)));
            Assert.That((regular.x, regular.y, regular.z), Is.EqualTo((-1, 2, 3)));
            Assert.That(regular.vector.GetElement(3), Is.EqualTo(0));
        }
    }

    [Test]
    public void UInt3()
    {
        var v = new uint3s(1u, 2u, 3u);
        uint3 regular = v;
        uint3s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y, back.z), Is.EqualTo((1u, 2u, 3u)));
            Assert.That((regular.x, regular.y, regular.z), Is.EqualTo((1u, 2u, 3u)));
            Assert.That(regular.vector.GetElement(3), Is.EqualTo(0u));
        }
    }

    [Test]
    public void Long3()
    {
        var v = new long3s(-1, 2, 3);
        long3 regular = v;
        long3s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y, back.z), Is.EqualTo((-1L, 2L, 3L)));
            Assert.That((regular.x, regular.y, regular.z), Is.EqualTo((-1L, 2L, 3L)));
            Assert.That(regular.vector.GetElement(3), Is.EqualTo(0L));
        }
    }

    [Test]
    public void ULong3()
    {
        var v = new ulong3s(1, 2, 3);
        ulong3 regular = v;
        ulong3s back = regular;

        using (Assert.EnterMultipleScope())
        {
            Assert.That((back.x, back.y, back.z), Is.EqualTo((1UL, 2UL, 3UL)));
            Assert.That((regular.x, regular.y, regular.z), Is.EqualTo((1UL, 2UL, 3UL)));
            Assert.That(regular.vector.GetElement(3), Is.EqualTo(0UL));
        }
    }
}
