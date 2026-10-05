using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The rotation of the three Euler angles of a value: every one of the six orders of the angles is the product of
/// the rotations of the three axes in an order of its own, so the matrix of one of them is the one of another with
/// the axes of the rotation named by a permutation, and the member of the order a value of a rotation names is the
/// one the value of the angles reaches. The members of the angles are the ones of a floating point kind alone,
/// since an angle is one.
/// </summary>
public class TestMatrixEuler
{
    /// <summary>The angles the members of the shape are checked with, which no two of them are the same.</summary>
    private static readonly float3 Angles = new(0.3f, -0.7f, 1.1f);

    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // every order reaches the product of the rotations of the three axes in the order of it
            AssertEqual(float3x3.EulerXYZ(Angles), XYZ(Angles), "x-y-z");
            AssertEqual(float3x3.EulerXZY(Angles), XZY(Angles), "x-z-y");
            AssertEqual(float3x3.EulerYXZ(Angles), YXZ(Angles), "y-x-z");
            AssertEqual(float3x3.EulerYZX(Angles), YZX(Angles), "y-z-x");
            AssertEqual(float3x3.EulerZXY(Angles), ZXY(Angles), "z-x-y");
            AssertEqual(float3x3.EulerZYX(Angles), ZYX(Angles), "z-y-x");

            // the rotation of the zero of the kind is the identity of the matrix, whatever the order of it is
            AssertEqual(float3x3.EulerXYZ(float3.Zero), float3x3.Identity, "the zero of the angles of the x-y-z order");
            AssertEqual(float3x3.EulerZYX(float3.Zero), float3x3.Identity, "the zero of the angles of the z-y-x order");

            // the rotation keeps the length of the value it is handed, whatever the order of the angles is
            Assert.That(math.length(math.mul(float3x3.EulerYZX(Angles), new float3(3f, 4f, 12f))),
                Is.EqualTo(13f).Within(1e-4f), "the length of the value of a rotation");
        }
    }

    [Test]
    public void OfOrder()
    {
        using (Assert.EnterMultipleScope())
        {
            // the member of the angles alone reaches the order of the z-x-y angles, which the default of the orders
            // of a rotation names as well
            Assert.That(float3x3.Euler(Angles), Is.EqualTo(float3x3.EulerZXY(Angles)), "the order of the default");
            Assert.That(float3x3.Euler(Angles, RotationOrder.Default), Is.EqualTo(float3x3.EulerZXY(Angles)),
                "the value of the default order");

            // every order a value of a rotation names reaches the member of the name of it
            Assert.That(float3x3.Euler(Angles, RotationOrder.XYZ), Is.EqualTo(float3x3.EulerXYZ(Angles)), "x-y-z");
            Assert.That(float3x3.Euler(Angles, RotationOrder.XZY), Is.EqualTo(float3x3.EulerXZY(Angles)), "x-z-y");
            Assert.That(float3x3.Euler(Angles, RotationOrder.YXZ), Is.EqualTo(float3x3.EulerYXZ(Angles)), "y-x-z");
            Assert.That(float3x3.Euler(Angles, RotationOrder.YZX), Is.EqualTo(float3x3.EulerYZX(Angles)), "y-z-x");
            Assert.That(float3x3.Euler(Angles, RotationOrder.ZXY), Is.EqualTo(float3x3.EulerZXY(Angles)), "z-x-y");
            Assert.That(float3x3.Euler(Angles, RotationOrder.ZYX), Is.EqualTo(float3x3.EulerZYX(Angles)), "z-y-x");
        }
    }

    /// <summary>
    /// The matrix of the angles of every kind that holds an angle, which is the kind of a floating point component:
    /// the zero of the angles of a kind reaches the identity of the matrix of that kind, and the matrix of the
    /// angles of a kind is the one of the plain formula of it, which builds the components of the matrix from the
    /// sine and the cosine of the angles of the value, so the member of a kind is checked for the way it builds the
    /// matrix of the two of them.
    /// </summary>
    [Test]
    public void OfKinds()
    {
        using (Assert.EnterMultipleScope())
        {
            // the zero of the angles of a kind reaches the identity of the matrix of that kind
            var hZero = half3x3.EulerZXY(new half3((half)0f, (half)0f, (half)0f));
            Assert.That(Components(hZero), Is.EqualTo((1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)),
                "the identity of the zero of the half angles");
            var dZero = double3x3.EulerZYX(new double3(0d, 0d, 0d));
            Assert.That(Components(dZero), Is.EqualTo((1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 1d)),
                "the identity of the zero of the double angles");

            // the matrix of the angles of a kind is the one of the plain formula of that kind, which builds the
            // components of it from the sine and the cosine of the angles of the value
            var h = new half3((half)0.3f, (half)(-0.7f), (half)1.1f);
            Assert.That(Components(half3x3.EulerZXY(h)), Is.EqualTo(Components(ZXY(h))).Within(1e-2f),
                "the z-x-y angles of the half kind");
            var d = new double3(0.3d, -0.7d, 1.1d);
            Assert.That(Components(double3x3.EulerZXY(d)), Is.EqualTo(Components(ZXY(d))).Within(1e-12),
                "the z-x-y angles of the double kind");
        }
    }

    /// <summary>
    /// Returns the nine components of the matrix, which the one of a kind is compared with through the components
    /// of the kind of a single precision number.
    /// </summary>
    private static (float, float, float, float, float, float, float, float, float) Components(float3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <inheritdoc cref="Components(float3x3)"/>
    private static (float, float, float, float, float, float, float, float, float) Components(half3x3 m) =>
        ((float)m.m00, (float)m.m01, (float)m.m02, (float)m.m10, (float)m.m11, (float)m.m12, (float)m.m20,
            (float)m.m21, (float)m.m22);

    /// <inheritdoc cref="Components(float3x3)"/>
    private static (double, double, double, double, double, double, double, double, double) Components(double3x3 m) =>
        (m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

    /// <summary>
    /// Asserts that <paramref name="actual"/> is the matrix <paramref name="expected"/>: the members of the shape
    /// compute the matrix of an order with the members of the vector of the kind, so the two of them agree on every
    /// component of it up to the precision of the kind.
    /// </summary>
    private static void AssertEqual(float3x3 actual, float3x3 expected, string what)
    {
        Assert.That(actual.m00, Is.EqualTo(expected.m00).Within(1e-5f), $"the first component of the first row of {what}");
        Assert.That(actual.m01, Is.EqualTo(expected.m01).Within(1e-5f), $"the second component of the first row of {what}");
        Assert.That(actual.m02, Is.EqualTo(expected.m02).Within(1e-5f), $"the third component of the first row of {what}");
        Assert.That(actual.m10, Is.EqualTo(expected.m10).Within(1e-5f), $"the first component of the second row of {what}");
        Assert.That(actual.m11, Is.EqualTo(expected.m11).Within(1e-5f), $"the second component of the second row of {what}");
        Assert.That(actual.m12, Is.EqualTo(expected.m12).Within(1e-5f), $"the third component of the second row of {what}");
        Assert.That(actual.m20, Is.EqualTo(expected.m20).Within(1e-5f), $"the first component of the third row of {what}");
        Assert.That(actual.m21, Is.EqualTo(expected.m21).Within(1e-5f), $"the second component of the third row of {what}");
        Assert.That(actual.m22, Is.EqualTo(expected.m22).Within(1e-5f), $"the third component of the third row of {what}");
    }

    /// <summary>
    /// Returns the matrix of the plain formula of the rotation of the three Euler angles of the value of
    /// <paramref name="a"/> in the order of the x-y-z angles, which is the product of the rotations of the three
    /// axes of it: the sine and the cosine of the angles are the ones of the value, and the components of the
    /// matrix are the products of them, so the member of the order is checked for the way it builds the matrix.
    /// Every angle of the value is in radians.
    /// </summary>
    private static float3x3 XYZ(float3 a)
    {
        var (s, c) = math.sincos(a);
        return new float3x3(
            c.y * c.z, c.z * s.x * s.y - c.x * s.z, c.x * c.z * s.y + s.x * s.z,
            c.y * s.z, c.x * c.z + s.x * s.y * s.z, c.x * s.y * s.z - c.z * s.x,
            -s.y, c.y * s.x, c.x * c.y);
    }

    /// <inheritdoc cref="XYZ(float3)"/>
    private static float3x3 XZY(float3 a)
    {
        var (s, c) = math.sincos(a);
        return new float3x3(
            c.y * c.z, s.x * s.y - c.x * c.y * s.z, c.x * s.y + c.y * s.x * s.z,
            s.z, c.x * c.z, -c.z * s.x,
            -c.z * s.y, c.y * s.x + c.x * s.y * s.z, c.x * c.y - s.x * s.y * s.z);
    }

    /// <inheritdoc cref="XYZ(float3)"/>
    private static float3x3 YXZ(float3 a)
    {
        var (s, c) = math.sincos(a);
        return new float3x3(
            c.y * c.z - s.x * s.y * s.z, -c.x * s.z, c.z * s.y + c.y * s.x * s.z,
            c.z * s.x * s.y + c.y * s.z, c.x * c.z, s.y * s.z - c.y * c.z * s.x,
            -c.x * s.y, s.x, c.x * c.y);
    }

    /// <inheritdoc cref="XYZ(float3)"/>
    private static float3x3 YZX(float3 a)
    {
        var (s, c) = math.sincos(a);
        return new float3x3(
            c.y * c.z, -s.z, c.z * s.y,
            s.x * s.y + c.x * c.y * s.z, c.x * c.z, c.x * s.y * s.z - c.y * s.x,
            c.y * s.x * s.z - c.x * s.y, c.z * s.x, c.x * c.y + s.x * s.y * s.z);
    }

    /// <inheritdoc cref="XYZ(float3)"/>
    private static float3x3 ZXY(float3 a)
    {
        var (s, c) = math.sincos(a);
        return new float3x3(
            c.y * c.z + s.x * s.y * s.z, c.z * s.x * s.y - c.y * s.z, c.x * s.y,
            c.x * s.z, c.x * c.z, -s.x,
            c.y * s.x * s.z - c.z * s.y, c.y * c.z * s.x + s.y * s.z, c.x * c.y);
    }

    /// <inheritdoc cref="XYZ(float3)"/>
    private static float3x3 ZYX(float3 a)
    {
        var (s, c) = math.sincos(a);
        return new float3x3(
            c.y * c.z, -c.y * s.z, s.y,
            c.z * s.x * s.y + c.x * s.z, c.x * c.z - s.x * s.y * s.z, -c.y * s.x,
            s.x * s.z - c.x * c.z * s.y, c.z * s.x + c.x * s.y * s.z, c.x * c.y);
    }

    /// <summary>
    /// Returns the matrix of the plain formula of the rotation of the three Euler angles of the value of
    /// <paramref name="a"/> of the half kind in the order of the z-x-y angles, which is the same formula computed
    /// with the half of the kind.
    /// </summary>
    private static half3x3 ZXY(half3 a)
    {
        var (s, c) = math.sincos(a);
        return new half3x3(
            c.y * c.z + s.x * s.y * s.z, c.z * s.x * s.y - c.y * s.z, c.x * s.y,
            c.x * s.z, c.x * c.z, -s.x,
            c.y * s.x * s.z - c.z * s.y, c.y * c.z * s.x + s.y * s.z, c.x * c.y);
    }

    /// <inheritdoc cref="ZXY(half3)"/>
    private static double3x3 ZXY(double3 a)
    {
        var (s, c) = math.sincos(a);
        return new double3x3(
            c.y * c.z + s.x * s.y * s.z, c.z * s.x * s.y - c.y * s.z, c.x * s.y,
            c.x * s.z, c.x * c.z, -s.x,
            c.y * s.x * s.z - c.z * s.y, c.y * c.z * s.x + s.y * s.z, c.x * c.y);
    }
}
