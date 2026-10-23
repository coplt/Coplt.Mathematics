using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The vector with the sign chosen so that it faces away from the incident vector, which the normal decides.
/// </summary>
public class TestFaceForward
{
    /// <summary>
    /// The member is one of the visitor of the kind of the value, so a parameter that only knows the interfaces
    /// of it reaches the member as well.
    /// </summary>
    private static void Check<T, TScalar>(T value)
        where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
        where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        => _ = math.face_forward(value, value, value);

    [Test]
    public void Value()
    {
        var v = new float3(1f, 2f, 3f);
        var ng = new float3(0f, 0f, 1f);

        using (Assert.EnterMultipleScope())
        {
            // the incident vector points into the direction of the normal
            Assert.That(v.face_forward(new float3(0f, 0f, 1f), ng), Is.EqualTo(new float3(-1f, -2f, -3f)));
            // the incident vector points against the normal
            Assert.That(v.face_forward(new float3(0f, 0f, -1f), ng), Is.EqualTo(v));
            Assert.That(new double2(1, 2).face_forward(new double2(1, 0), new double2(1, 0)),
                Is.EqualTo(new double2(-1, -2)));
            // the dot product of the normal and the incident vector is zero, so it is not negative
            Assert.That(new double2(1, 2).face_forward(new double2(0, 1), new double2(1, 0)),
                Is.EqualTo(new double2(-1, -2)));
        }

        // a value without a register works on the components
        using (Assert.EnterMultipleScope())
        {
            var h = new half3((half)1f, (half)2f, (half)3f);
            Assert.That(h.face_forward(new half3((half)0f, (half)0f, (half)1f), new half3((half)0f, (half)0f, (half)1f)),
                Is.EqualTo(new half3((half)(-1f), (half)(-2f), (half)(-3f))));
        }
    }

    /// <summary>
    /// The sign of the padding lane of a value follows the sign of the components of it, which keeps the zero
    /// of the lane.
    /// </summary>
    [Test]
    public void Padding()
    {
        var v = new float3(1f, 2f, 3f);
        var ng = new float3(0f, 0f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.face_forward(new float3(0f, 0f, 1f), ng).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.face_forward(new float3(0f, 0f, -1f), ng).vector.GetElement(3), Is.EqualTo(0f));
            // the register of a value of 2 components is wider than the value
            var n = new float2(1f, 2f);
            Assert.That(n.face_forward(n, n).vector.GetElement(2), Is.EqualTo(0f));
        }
    }

    [Test]
    public void Interface()
    {
        Check<float3, float>(new float3(1f, 2f, 3f));
        Check<double3, double>(new double3(1, 2, 3));
        Check<half3, half>(new half3((half)1f, (half)2f, (half)3f));
    }
}
