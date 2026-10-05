using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The scale of a plane: the matrix of it holds the values the axes of the plane are scaled by on the diagonal of
/// it and the zero of the kind of it everywhere else, so every axis of the plane is scaled on its own and a scale
/// keeps no part of one axis in the other one. The member is a member of every kind a number names, so a matrix of
/// a whole number reaches the scale of it as well.
/// </summary>
public class TestMatrixScale
{
    [Test]
    public void Of2x2()
    {
        using (Assert.EnterMultipleScope())
        {
            // a single value scales both axes of the plane by it
            var uniform = float2x2.Scale(2f);
            Assert.That((uniform.m00, uniform.m01, uniform.m10, uniform.m11), Is.EqualTo((2f, 0f, 0f, 2f)),
                "the scale of a single component");

            // two values scale the first axis of the plane by the first of them and the second one by the second
            var diagonal = float2x2.Scale(2f, 3f);
            Assert.That((diagonal.m00, diagonal.m01, diagonal.m10, diagonal.m11), Is.EqualTo((2f, 0f, 0f, 3f)),
                "the scale of two components");

            // the value whose components the axes of the plane are scaled by is the two of them
            Assert.That(float2x2.Scale(new float2(2f, 3f)), Is.EqualTo(diagonal), "the scale of a column");

            // every axis of the plane is scaled on its own, so the scale of the value of the first axis is the
            // first axis scaled and it keeps no part of the second one
            var scaled = math.mul(diagonal, new float2(1f, 1f));
            Assert.That(scaled.x, Is.EqualTo(2f), "the first axis of the plane");
            Assert.That(scaled.y, Is.EqualTo(3f), "the second axis of the plane");

            // the scales of two values that are the reciprocal of each other keep the plane where it is, and the
            // identity of the kind is the one the scale of the one of it is
            Assert.That(math.mul(float2x2.Scale(2f), float2x2.Scale(0.5f)), Is.EqualTo(float2x2.Identity),
                "the reciprocal of the scale");
            Assert.That(math.mul(float2x2.Scale(2f), float2x2.Identity), Is.EqualTo(float2x2.Scale(2f)),
                "the identity of the scale");

            // the kind of a component of the value is named by the member that reaches the scale of it
            Assert.That(double2x2.Scale(2d, 3d).m11, Is.EqualTo(3d), "double");
            Assert.That(half2x2.Scale((half)2f).m00, Is.EqualTo((half)2f), "half");
        }
    }

    [Test]
    public void OfEveryKind()
    {
        using (Assert.EnterMultipleScope())
        {
            // the scale is the member of every kind a number names, so every whole number kind reaches it: the
            // value of the diagonal of the matrix is the one it is handed and the rest of it is the zero of the
            // kind
            var s = short2x2.Scale((short)2);
            Assert.That((s.m00, s.m01, s.m10, s.m11), Is.EqualTo(((short)2, (short)0, (short)0, (short)2)), "short");
            var us = ushort2x2.Scale((ushort)2);
            Assert.That((us.m00, us.m01, us.m10, us.m11), Is.EqualTo(((ushort)2, (ushort)0, (ushort)0, (ushort)2)),
                "ushort");
            var i = int2x2.Scale(2);
            Assert.That((i.m00, i.m01, i.m10, i.m11), Is.EqualTo((2, 0, 0, 2)), "int");
            var u = uint2x2.Scale(2U);
            Assert.That((u.m00, u.m01, u.m10, u.m11), Is.EqualTo((2U, 0U, 0U, 2U)), "uint");
            var l = long2x2.Scale(2L);
            Assert.That((l.m00, l.m01, l.m10, l.m11), Is.EqualTo((2L, 0L, 0L, 2L)), "long");
            var ul = ulong2x2.Scale(2UL);
            Assert.That((ul.m00, ul.m01, ul.m10, ul.m11), Is.EqualTo((2UL, 0UL, 0UL, 2UL)), "ulong");

            // the scale of the one of a kind is the identity of it and the column of a kind reaches the two
            // values of it as well
            Assert.That(int2x2.Scale(1), Is.EqualTo(int2x2.Identity), "the identity of a whole number kind");
            Assert.That(int2x2.Scale(new int2(2, 3)), Is.EqualTo(int2x2.Scale(2, 3)), "the scale of a column");
            Assert.That(short2x2.Scale(new short2((short)2, (short)3)).m11, Is.EqualTo((short)3), "the column of a short");
        }
    }

    /// <summary>
    /// The scale of the space: the matrix of it holds the values the axes of the space are scaled by on the diagonal
    /// of it and the zero of the kind of it everywhere else, so every axis of the space is scaled on its own and the
    /// value of the three axes of it is the one of the scale.
    /// </summary>
    [Test]
    public void Of3x3()
    {
        using (Assert.EnterMultipleScope())
        {
            // a single value scales every axis of the space by it
            Assert.That(float3x3.Scale(2f), Is.EqualTo(new float3x3(2f, 0f, 0f, 0f, 2f, 0f, 0f, 0f, 2f)),
                "the scale of a single component");

            // three values scale every axis of the space by the one of it
            var diagonal = float3x3.Scale(2f, 3f, 4f);
            Assert.That(diagonal, Is.EqualTo(new float3x3(2f, 0f, 0f, 0f, 3f, 0f, 0f, 0f, 4f)),
                "the scale of three components");
            Assert.That(float3x3.Scale(new float3(2f, 3f, 4f)), Is.EqualTo(diagonal),
                "the scale of a value of 3 components");

            // every axis of the space is scaled on its own, so the value of the three axes is the one of the scale
            var scaled = math.mul(diagonal, new float3(1f, 1f, 1f));
            Assert.That((scaled.x, scaled.y, scaled.z), Is.EqualTo((2f, 3f, 4f)), "the axes of the space");

            // the kind of a component of the value is named by the member that reaches the scale of it
            Assert.That((double3x3.Scale(2d).m00, double3x3.Scale(2d, 3d, 4d).m22), Is.EqualTo((2d, 4d)), "double");
            Assert.That(half3x3.Scale((half)2f).m00, Is.EqualTo((half)2f), "half");
            Assert.That((int3x3.Scale(2).m11, long3x3.Scale(new long3(2L, 3L, 4L)).m22), Is.EqualTo((2, 4L)), "whole");

            // the column of the value of a kind that has no register is put together from the components of the
            // value, since only the value of a kind that keeps its value in one has a padding lane to read
            var halfColumn = half3x3.Scale(new half3((half)2f, (half)3f, (half)4f));
            Assert.That((halfColumn.m00, halfColumn.m11, halfColumn.m22), Is.EqualTo(((half)2f, (half)3f, (half)4f)),
                "the scale of a value of 3 components of a kind that has no register");

            // the value of a kind that keeps its value in a register reads the padding lane of it, which is zero,
            // so the padding lane of a column of the matrix is zero as well
            Assert.That(float3x3.Scale(new float3(2f, 3f, 4f)).c0.vector.GetElement(3), Is.EqualTo(0f),
                "the padding lane of the first column");
            Assert.That(double3x3.Scale(new double3(2d, 3d, 4d)).c2.vector.GetElement(3), Is.EqualTo(0d),
                "the padding lane of the third column");
        }
    }

    /// <summary>
    /// The scale of the space: the matrix of it holds the values the axes of the space are scaled by on the diagonal
    /// of it and the zero of the kind of it everywhere else, so every axis of the space is scaled on its own, and the
    /// fourth axis of it keeps the one of the kind, which the scale of the space does not reach.
    /// </summary>
    [Test]
    public void Of4x4()
    {
        using (Assert.EnterMultipleScope())
        {
            // a single value scales every axis of the space by it
            Assert.That(float4x4.Scale(2f), Is.EqualTo(new float4x4(2f, 0f, 0f, 0f, 0f, 2f, 0f, 0f, 0f, 0f, 2f, 0f,
                0f, 0f, 0f, 1f)), "the scale of a single component");

            // three values scale every axis of the space by the one of it
            var diagonal = float4x4.Scale(2f, 3f, 4f);
            Assert.That(diagonal, Is.EqualTo(new float4x4(2f, 0f, 0f, 0f, 0f, 3f, 0f, 0f, 0f, 0f, 4f, 0f, 0f, 0f, 0f,
                1f)), "the scale of three components");
            Assert.That(float4x4.Scale(new float3(2f, 3f, 4f)), Is.EqualTo(diagonal),
                "the scale of a value of 3 components");

            // the fourth axis of the space keeps the one of the kind, so the last column of the matrix is not the
            // zero of the kind alone and the padding lane of every other column of it is the zero of it
            Assert.That((diagonal.m33, diagonal.c3.w, diagonal.m03), Is.EqualTo((1f, 1f, 0f)), "the fourth axis");
            Assert.That((diagonal.c0.w, diagonal.c1.w, diagonal.c2.w), Is.EqualTo((0f, 0f, 0f)),
                "the padding lanes of the columns of a single precision kind");
            Assert.That(double4x4.Scale(new double3(2d, 3d, 4d)).c0.vector.GetElement(3), Is.EqualTo(0d),
                "the padding lane of the column of a double precision kind");

            // the scale of the one of a kind is the identity of it
            Assert.That(float4x4.Scale(1f), Is.EqualTo(float4x4.Identity), "the identity of the scale");
            Assert.That(float4x4.Scale(new float3(1f, 1f, 1f)), Is.EqualTo(float4x4.Identity),
                "the identity of the scale of a value of 3 components");

            // the kind of a component of the value is named by the member that reaches the scale of it
            Assert.That((double4x4.Scale(2d).m00, half4x4.Scale((half)2f).m11, int4x4.Scale(new int3(2, 3, 4)).m22),
                Is.EqualTo((2d, (half)2f, 4)), "the kind of the value");
        }
    }
}
