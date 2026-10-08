using System.Runtime.Intrinsics;
using Coplt.Mathematics;

namespace Tests.Arith;

/// <summary>
/// The three Euler angles of the rotation of a quaternion are read out of the value of it: every order of the
/// three angles has a member of its own, which the member that takes the order of them reaches at the call of it,
/// and the member that takes no order reads the angles of the default one of the z-x-y order. The angles of an
/// order hold the rotation of the value they were read out of again, so the angles the members answer with are
/// the ones the rotation of the value is the one of.
/// <para>The three angles of an order whose middle rotation is of a right angle cannot be told apart from each
/// other, so the member answers with the angle of the axis in the middle of the order at the right angle, with
/// the sum or the difference of the two angles beside it at one of the two axes beside it and with the zero of
/// the kind at the other one of them.</para>
/// </summary>
public class TestQuaternionEuler
{
    /// <summary>
    /// The tolerance of the angles of the kind of a single precision number, which is the rounding of the two
    /// rotations of the read back of a value.
    /// </summary>
    private const float Tolerance = 1e-5f;

    /// <inheritdoc cref="Tolerance"/>
    private const double DoubleTolerance = 1e-10;

    /// <inheritdoc cref="Tolerance"/>
    private const float HalfTolerance = 0.02f;

    /// <summary>The six orders of the three Euler angles, in the order of the axes of them.</summary>
    private static readonly string[] Orders = { "XYZ", "XZY", "YXZ", "YZX", "ZXY", "ZYX" };

    private static quaternion Euler(string order, float3 xyz) => order switch
    {
        "XYZ" => quaternion.EulerXYZ(xyz),
        "XZY" => quaternion.EulerXZY(xyz),
        "YXZ" => quaternion.EulerYXZ(xyz),
        "YZX" => quaternion.EulerYZX(xyz),
        "ZYX" => quaternion.EulerZYX(xyz),
        _ => quaternion.EulerZXY(xyz),
    };

    private static float3 Read(quaternion q, string order) => order switch
    {
        "XYZ" => q.ToEulerXYZ(),
        "XZY" => q.ToEulerXZY(),
        "YXZ" => q.ToEulerYXZ(),
        "YZX" => q.ToEulerYZX(),
        "ZYX" => q.ToEulerZYX(),
        _ => q.ToEulerZXY(),
    };

    private static bool Defined(string order, RotationOrder rotationOrder) => rotationOrder switch
    {
        RotationOrder.XYZ => order == "XYZ",
        RotationOrder.XZY => order == "XZY",
        RotationOrder.YXZ => order == "YXZ",
        RotationOrder.YZX => order == "YZX",
        RotationOrder.ZYX => order == "ZYX",
        _ => order == "ZXY",
    };

    /// <summary>The axis of the rotation in the middle of an order.</summary>
    private static int Middle(string order) => "xyz".IndexOf(char.ToLowerInvariant(order[1]));

    private static float3 With(float3 angles, int axis, float value) => axis switch
    {
        0 => new float3(value, angles.y, angles.z),
        1 => new float3(angles.x, value, angles.z),
        _ => new float3(angles.x, angles.y, value),
    };

    private static float Component(float3 v, int axis) => axis switch
    {
        0 => v.x,
        1 => v.y,
        _ => v.z,
    };

    private static float Diff(float3 a, float3 b) => math.sum(math.abs(a - b));

    private static double Diff(double3 a, double3 b) => math.sum(math.abs(a - b));

    private static double Diff(double4 a, double4 b) => math.sum(math.abs(a - b));

    private static float Diff(half3 a, half3 b) => (float)math.sum(math.abs(a - b));

    private static float Diff(half4 a, half4 b) => (float)math.sum(math.abs(a - b));

    private static quaternion_d EulerD(string order, double3 xyz) => order switch
    {
        "XYZ" => quaternion_d.EulerXYZ(xyz),
        "XZY" => quaternion_d.EulerXZY(xyz),
        "YXZ" => quaternion_d.EulerYXZ(xyz),
        "YZX" => quaternion_d.EulerYZX(xyz),
        "ZYX" => quaternion_d.EulerZYX(xyz),
        _ => quaternion_d.EulerZXY(xyz),
    };

    private static double3 ReadD(quaternion_d q, string order) => order switch
    {
        "XYZ" => q.ToEulerXYZ(),
        "XZY" => q.ToEulerXZY(),
        "YXZ" => q.ToEulerYXZ(),
        "YZX" => q.ToEulerYZX(),
        "ZYX" => q.ToEulerZYX(),
        _ => q.ToEulerZXY(),
    };

    private static quaternion_h EulerH(string order, half3 xyz) => order switch
    {
        "XYZ" => quaternion_h.EulerXYZ(xyz),
        "XZY" => quaternion_h.EulerXZY(xyz),
        "YXZ" => quaternion_h.EulerYXZ(xyz),
        "YZX" => quaternion_h.EulerYZX(xyz),
        "ZYX" => quaternion_h.EulerZYX(xyz),
        _ => quaternion_h.EulerZXY(xyz),
    };

    private static half3 ReadH(quaternion_h q, string order) => order switch
    {
        "XYZ" => q.ToEulerXYZ(),
        "XZY" => q.ToEulerXZY(),
        "YXZ" => q.ToEulerYXZ(),
        "YZX" => q.ToEulerYZX(),
        "ZYX" => q.ToEulerZYX(),
        _ => q.ToEulerZXY(),
    };

    private static double3 With(double3 angles, int axis, double value) => axis switch
    {
        0 => new double3(value, angles.y, angles.z),
        1 => new double3(angles.x, value, angles.z),
        _ => new double3(angles.x, angles.y, value),
    };

    private static half3 With(half3 angles, int axis, Half value) => axis switch
    {
        0 => new half3(value, angles.y, angles.z),
        1 => new half3(angles.x, value, angles.z),
        _ => new half3(angles.x, angles.y, value),
    };

    private static double Component(double3 v, int axis) => axis switch
    {
        0 => v.x,
        1 => v.y,
        _ => v.z,
    };

    private static float Component(half3 v, int axis) => (float)(axis switch
    {
        0 => v.x,
        1 => v.y,
        _ => v.z,
    });

    /// <summary>
    /// The angles of every order hold the rotation of the value they were read out of, which is the whole of what
    /// the read back of a value answers for: the value of the angles of an order that the angles of the same
    /// order are built from is the value itself.
    /// </summary>
    [Test]
    public void RoundTrip()
    {
        var angles = new float3(0.3f, 0.5f, 0.7f);

        using (Assert.EnterMultipleScope())
        {
            foreach (var order in Orders)
            {
                var q = Euler(order, angles);
                var back = Read(q, order);
                Assert.That(Diff(back, angles), Is.LessThan(Tolerance), $"the angles of the order {order}");
                // the member is the one of the rotation of the value, so the angles of an order reach the value
                // of the order they were read in again
                Assert.That(Diff(Euler(order, back).value.xyz, q.value.xyz), Is.LessThan(Tolerance),
                    $"the rotation of the order {order}");
            }
        }
    }

    /// <summary>
    /// The member that takes no order reads the angles of the default one of the z-x-y order and the one that
    /// takes an order reads the angles of the one it names.
    /// </summary>
    [Test]
    public void DefaultAndOrder()
    {
        var angles = new float3(0.3f, 0.5f, 0.7f);

        using (Assert.EnterMultipleScope())
        {
            foreach (var order in Orders)
            {
                var q = Euler(order, angles);

                Assert.That(Diff(q.ToEulerAngles(), q.ToEulerZXY()), Is.Zero, "the default order is the z-x-y one");
                Assert.That(Diff(q.ToEulerAngles(RotationOrder.Default), q.ToEulerZXY()), Is.Zero,
                    "the order of the constant of the default one");

                foreach (var rotationOrder in Enum.GetValues<RotationOrder>())
                {
                    if (!Defined(order, rotationOrder)) continue;
                    Assert.That(Diff(q.ToEulerAngles(rotationOrder), Read(q, order)), Is.Zero,
                        $"the order {order} at the call of it");
                }
            }
        }
    }

    /// <summary>
    /// Every kind of a floating point component has the members, which is the kind of the value of the angles of
    /// the quaternion of it and of its own arithmetic: the digits of the sine of the angle of the axis in the
    /// middle of an order that the kind of the component holds are the ones the member reads the angle of it with
    /// a right angle out of the value of it through.
    /// </summary>
    [Test]
    public void Kinds()
    {
        var dangles = new double3(0.3, 0.5, 0.7);
        var hangles = new half3((Half)0.3f, (Half)0.5f, (Half)0.7f);
        const double dhalf = Math.PI / 2;
        var hhalf = (Half)(MathF.PI / 2);

        using (Assert.EnterMultipleScope())
        {
            foreach (var order in Orders)
            {
                var middle = Middle(order);
                var next2 = (middle + 2) % 3;

                Assert.That(Diff(ReadD(EulerD(order, dangles), order), dangles), Is.LessThan(DoubleTolerance),
                    $"the angles of a double of the order {order}");
                Assert.That(Diff(ReadH(EulerH(order, hangles), order), hangles), Is.LessThan(HalfTolerance),
                    $"the angles of a half of the order {order}");

                var dq = EulerD(order, With(dangles, middle, dhalf));
                var dback = ReadD(dq, order);
                Assert.That(Math.Abs(Component(dback, middle) - dhalf), Is.LessThan(DoubleTolerance),
                    $"the angle of the middle axis of a double of the order {order}");
                Assert.That(Math.Abs(Component(dback, next2)), Is.LessThan(DoubleTolerance),
                    $"the zero of a double of the order {order}");
                Assert.That(Diff(EulerD(order, dback).value, dq.value), Is.LessThan(DoubleTolerance),
                    $"the rotation of a double of the order {order} of a right angle");

                var hq = EulerH(order, With(hangles, middle, hhalf));
                var hback = ReadH(hq, order);
                Assert.That(MathF.Abs(Component(hback, middle) - (float)hhalf), Is.LessThan(HalfTolerance),
                    $"the angle of the middle axis of a half of the order {order}");
                Assert.That(MathF.Abs(Component(hback, next2)), Is.LessThan(HalfTolerance),
                    $"the zero of a half of the order {order}");
                Assert.That(Diff(EulerH(order, hback).value, hq.value), Is.LessThan(HalfTolerance),
                    $"the rotation of a half of the order {order} of a right angle");
            }
        }
    }

    /// <summary>
    /// The three angles of an order whose middle rotation is of a right angle are not read out of the value on
    /// their own: the member answers with the angle of the axis in the middle of the order at the right angle,
    /// with the sum or the difference of the two angles beside it at one of the two axes beside it and with the
    /// zero of the kind at the other one of them, and the three of them hold the rotation of the value again.
    /// </summary>
    [Test]
    public void Locked()
    {
        var angles = new float3(0.4f, 0.5f, 0.2f);
        var half = MathF.PI / 2;

        using (Assert.EnterMultipleScope())
        {
            foreach (var order in Orders)
            {
                var middle = Middle(order);
                var next2 = (middle + 2) % 3;

                foreach (var sign in new[] { 1f, -1f })
                {
                    var locked = With(angles, middle, sign * half);
                    var q = Euler(order, locked);
                    var back = Read(q, order);

                    Assert.That(math.abs(Component(back, middle) - (sign * half)), Is.LessThan(Tolerance),
                        $"the angle of the middle axis of the order {order}");
                    Assert.That(math.abs(Component(back, next2)), Is.LessThan(Tolerance),
                        $"the angle of the axis the member of the order {order} answers with the zero for");
                    Assert.That(Diff(Euler(order, back).value.xyz, q.value.xyz), Is.LessThan(Tolerance),
                        $"the rotation of the order {order} of a right angle");
                }
            }
        }
    }

    /// <summary>
    /// A padding lane of the register of a value of 3 components holds no component and the member reads it out
    /// of the register of the value of 4 components of the quaternion, so it is the zero of the kind as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        var angles = new float3(0.3f, 0.5f, 0.7f);

        using (Assert.EnterMultipleScope())
        {
            foreach (var order in Orders)
            {
                Assert.That(Read(Euler(order, angles), order).vector.GetElement(3), Is.EqualTo(0f),
                    $"the padding lane of the order {order}");
                Assert.That(Read(Euler(order, With(angles, Middle(order), MathF.PI / 2)), order).vector.GetElement(3),
                    Is.EqualTo(0f), $"the padding lane of the order {order} of a right angle");
            }
        }
    }
}
