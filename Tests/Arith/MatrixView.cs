using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The view of the space: the matrix of the view reads a value of the space as the value of the eye of a view, which
/// is turned towards the value the view looks along, so the value of the third axis of the space of the view is the
/// one that is looked along and the value of the fourth column of the matrix is the one of the eye of it. The member
/// is a member of a floating point kind alone, since the value of a view is the one of a floating point component.
/// </summary>
public class TestMatrixView
{
    /// <summary>The value of the eye position of the checks, which is a point of the space.</summary>
    private static readonly float3 Eye = new(1f, 2f, 3f);

    /// <summary>The value that stays over the one the view looks along, which is of the length one.</summary>
    private static readonly float3 Up = new(0f, 1f, 0f);

    /// <summary>The value of the third axis of the space, which the view of the left of it looks along.</summary>
    private static readonly float3 Forward = new(0f, 0f, 1f);

    /// <summary>The value of the target of the checks, which is a point of the space apart from the eye of the view.</summary>
    private static readonly float3 Target = new(-2f, 5f, 1f);

    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            // the view of the space of the origin of it that looks along the third axis of it while the second axis of
            // it stays over the value is the identity of the matrix, and the view of the right of it looks along the
            // value with the third axis of the space of the view turned around
            Assert.That(float4x4.LookAt(default, Forward, Up), Is.EqualTo(float4x4.Identity), "the origin of the value");
            Assert.That(float4x4.LookTo(default, Forward, Up), Is.EqualTo(float4x4.Identity), "the origin of the direction");
            Assert.That(float4x4.LookTo_RH(default, -Forward, Up), Is.EqualTo(float4x4.Identity),
                "the origin of the right of it");
            Assert.That(float4x4.LookTo_RH(default, Forward, Up), Is.EqualTo(new float4x4(
                -1f, 0f, 0f, 0f,
                0f, 1f, 0f, 0f,
                0f, 0f, -1f, 0f,
                0f, 0f, 0f, 1f)), "the third axis of the right of it");

            // the value of the eye position of the view is the one of the fourth column of the matrix, so the view of
            // the space that looks along the third axis of it is the space read from the eye of it
            Assert.That(float4x4.LookAt(Eye, Eye + Forward, Up), Is.EqualTo(float4x4.Translate(-Eye)),
                "the eye position of the value");
            Assert.That(float4x4.LookTo(Eye, Forward, Up), Is.EqualTo(float4x4.Translate(-Eye)),
                "the eye position of the direction");
            Assert.That(float4x4.LookTo_RH(Eye, -Forward, Up), Is.EqualTo(float4x4.Translate(-Eye)),
                "the eye position of the right of it");

            // the member of the value of an eye position and the one of the value of a target it looks at read the
            // member of the value of the direction between the two points of them
            Assert.That(float4x4.LookAt(Eye, Target, Up), Is.EqualTo(float4x4.LookTo(Eye, Target - Eye, Up)),
                "the value of the target");
            Assert.That(float4x4.LookAt_RH(Eye, Target, Up), Is.EqualTo(float4x4.LookTo_RH(Eye, Target - Eye, Up)),
                "the value of the target of the right of it");
            Assert.That(float4x4.LookAt_Row(Eye, Target, Up), Is.EqualTo(float4x4.LookTo_Row(Eye, Target - Eye, Up)),
                "the value of the target of the rows");
            Assert.That(float4x4.LookAt_RH_Row(Eye, Target, Up),
                Is.EqualTo(float4x4.LookTo_RH_Row(Eye, Target - Eye, Up)), "the value of the target of the rows of the right");

            // the value of the eye position of the view reads as the origin of the space of the view and the value the
            // view looks along reads on the third axis of it
            var v = float4x4.LookAt(Eye, Target, Up);
            var origin = math.mul(v, new float4(Eye, 1f)).xyz;
            Assert.That((origin.x, origin.y, origin.z), Is.EqualTo((0f, 0f, 0f)).Within(1e-5f),
                "the value of the eye position");
            var look = math.normalize(Target - Eye);
            var read = math.mul(v, new float4(Eye + look, 1f)).xyz;
            Assert.That((read.x, read.y, read.z), Is.EqualTo((0f, 0f, 1f)).Within(1e-5f),
                "the value the view looks along");

            // the rows of the matrix are the axes of the space of the view, which are of the length one and at a
            // right angle with one another, whatever the value the view looks along is
            var a = float4x4.LookTo_RH(Eye, Target, Up);
            Assert.That(math.length(new float3(a.m00, a.m01, a.m02)), Is.EqualTo(1f).Within(1e-5f),
                "the length of the first axis");
            Assert.That(math.dot(new float3(a.m00, a.m01, a.m02), new float3(a.m10, a.m11, a.m12)),
                Is.EqualTo(0f).Within(1e-5f), "the first axis beside the second one");
            Assert.That(math.dot(new float3(a.m00, a.m01, a.m02), new float3(a.m20, a.m21, a.m22)),
                Is.EqualTo(0f).Within(1e-5f), "the first axis beside the third one");

            // the columns of the view whose rows are the axes of it are the axes of the space of the view, which the
            // member that names the rows of the matrix reaches, and the value of the space is read by the product of
            // it with the matrix, which reads the value of the eye position as the one of the origin of the space
            var row = float4x4.LookTo_RH_Row(Eye, Target, Up);
            Assert.That(math.length(row.c0.xyz), Is.EqualTo(1f).Within(1e-5f), "the first axis of the rows");
            Assert.That(math.dot(row.c0.xyz, row.c1.xyz), Is.EqualTo(0f).Within(1e-5f),
                "the first axis of the rows beside the second one");
            var rowOrigin = math.mul(new float4(Eye, 1f), row).xyz;
            Assert.That((rowOrigin.x, rowOrigin.y, rowOrigin.z), Is.EqualTo((0f, 0f, 0f)).Within(1e-5f),
                "the value of the eye position of the rows");

            // the kind of a component of the value is named by the member that reaches the view of it
            Assert.That(half4x4.LookAt(default, new half3((half)0f, (half)0f, (half)1f),
                new half3((half)0f, (half)1f, (half)0f)), Is.EqualTo(half4x4.Identity), "half");
            Assert.That(double4x4.LookTo(default, new double3(0d, 0d, 1d), new double3(0d, 1d, 0d)),
                Is.EqualTo(double4x4.Identity), "double");
        }
    }
}
