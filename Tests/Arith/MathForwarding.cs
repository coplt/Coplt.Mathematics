using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The arithmetic members of a vector are members of the vector itself and the members of the math class reach
/// them as well, the members that dispatch the value of a value reach the value of a matrix as well. None of the
/// forwarded members needs the type of a single component, so the compiler infers the vector type from the
/// argument and a call of them does not have to name it.
/// </summary>
public class TestMathArithForwarding
{
    /// <summary>
    /// The members that only need the vector type are reachable with the vector type alone, so the constraint of
    /// every one of them is the interface that has no component type.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IVectorArithmetic<T>, INumberAlgebraDispatch<T>
    {
        math.abs(v);
        math.sign(v);
        math.min(v, v);
        math.max(v, v);
        math.clamp(v, v, v);
        // the range of the interpolation members has to be wider than the value, an integer division by the
        // zero width of a degenerate range would not be defined
        math.lerp(v + v, v + v + v, v);
        math.unlerp(v + v, v, v + v + v);
        math.remap(v, v, v + v, v, v + v);
        math.square(v);
        math.fma(v, v, v);
        math.fms(v, v, v);
        math.fnma(v, v, v);
        math.fsm(v, v, v);
        math.fam(v, v, v);
        math.mad(v, v, v);
    }

    /// <summary>
    /// The members that name the type of a single component are reachable as well: a member that takes a
    /// component infers the type of it from the argument, a member that only returns one names both types.
    /// </summary>
    private static void CheckScalar<T, TScalar>(T v, TScalar s0, TScalar s1)
        where T : unmanaged, IVectorArithmetic<T, TScalar>, INumberAlgebra<T, TScalar>, INumberAlgebraDispatch<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        // the width of the range of the members that divide by it has to be wider than zero
        var b = v + v;
        math.lerp(s0, s1, v);
        _ = math.dot<T, TScalar>(v, v);
        _ = math.length_sq<T, TScalar>(v);
        _ = math.distance_sq<T, TScalar>(v, v);
        _ = math.csum<T, TScalar>(v);
        _ = math.cmin<T, TScalar>(v);
        _ = math.cmax<T, TScalar>(v);
        _ = math.cmin_safe<T, TScalar>(v);
        _ = math.cmax_safe<T, TScalar>(v);
    }

    /// <summary>
    /// The members that only need the type of the value reach a matrix as well: the shape of a matrix is a part
    /// of its type, so the value of it is dispatched to the member that matches the shape or to the columns of
    /// it, and the constraint of every one of the members is the same as the one of a vector.
    /// </summary>
    private static void CheckMatrix<T>(T m)
        where T : unmanaged, INumberAlgebraDispatch<T>
    {
        math.abs(m);
        math.sign(m);
        math.min(m, m);
        math.max(m, m);
        math.clamp(m, m, m);
        math.lerp(m, m, m);
    }

    [Test]
    public void Forwarding()
    {
        // the members of the math class reach the ones of the vector, the call is the same as the member
        Check(new float2(1, 2));
        Check(new float3(1, 2, 3));
        Check(new double2(1, 2));
        Check(new int2(1, 2));
        Check(new uint3(1, 2, 3));
        Check(new long4(1, 2, 3, 4));
        Check(new int3s(1, 2, 3));
        Check(new half2((half)1f, (half)2f));
        CheckScalar(new float3(1, 2, 3), 1f, 2f);
        CheckScalar(new double2(1, 2), 1d, 2d);
        CheckScalar(new half3((half)1f, (half)2f, (half)3f), (half)1f, (half)2f);
        CheckScalar(new int3(1, 2, 3), 1, 2);
        CheckScalar(new uint2(1, 2), 1u, 2u);
        CheckScalar(new long3s(1, 2, 3), 1L, 2L);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.abs(new int3(-1, 2, -3)), Is.EqualTo(new int3(1, 2, 3)));
            Assert.That(math.sign(new int3(-5, 0, 5)), Is.EqualTo(new int3(-1, 0, 1)));
            Assert.That(math.min(new int3(1, 2, 3), new int3(3, 2, 1)), Is.EqualTo(new int3(1, 2, 1)));
            Assert.That(math.max(new int3(1, 2, 3), new int3(3, 2, 1)), Is.EqualTo(new int3(3, 2, 3)));
            Assert.That(math.clamp(new int3(-1, 2, 5), new int3(0), new int3(3)), Is.EqualTo(new int3(0, 2, 3)));
            // a vector whose register is 64 bits wide and one that has no register reach the member of their
            // own kind
            Assert.That(math.clamp(new int2(-1, 2), new int2(0), new int2(1)), Is.EqualTo(new int2(0, 1)));
            Assert.That(math.clamp(new float3s(-1f, 2f, 5f), new float3s(0f, 0f, 0f), new float3s(3f, 3f, 3f)),
                Is.EqualTo(new float3s(0f, 2f, 3f)));
            Assert.That(math.square(new int3(2, 3, 4)), Is.EqualTo(new int3(4, 9, 16)));
            Assert.That(math.lerp(default(float3), new float3(2f), new float3(0.5f)), Is.EqualTo(new float3(1f)));
            // the factor scales the difference, so it only places a value between the two of them inside the
            // range of zero and one, the interpolation of an integer vector has no fraction
            Assert.That(math.lerp(new int3(1, 2, 3), new int3(5, 6, 7), new int3(0)), Is.EqualTo(new int3(1, 2, 3)));
            Assert.That(math.lerp(new int3(1, 2, 3), new int3(5, 6, 7), new int3(1)), Is.EqualTo(new int3(5, 6, 7)));
            Assert.That(math.lerp(new int3(0), new int3(4), new int3(2)), Is.EqualTo(new int3(8)));
            // a vector whose register is 64 bits wide and one that has no register reach the member of their
            // own kind
            Assert.That(math.lerp(new int2(1, 3), new int2(3, 5), new int2(1)), Is.EqualTo(new int2(3, 5)));
            Assert.That(math.lerp(new float3s(0f), new float3s(1f), new float3s(0.5f)),
                Is.EqualTo(new float3s(0.5f)));
            Assert.That(math.unlerp(new float3(1f), default, new float3(2f)), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.remap(new float3(0.5f), default, new float3(1f), default, new float3(10f)),
                Is.EqualTo(new float3(5f)));
            // the member of the value is the member of the class, a value without a register reaches the same
            // members
            Assert.That(new int3(2, 3, 4).square(), Is.EqualTo(new int3(4, 9, 16)));
            Assert.That(new float3s(2f, 3f, 4f).square(), Is.EqualTo(new float3s(4f, 9f, 16f)));
            Assert.That(new float3(1f).unlerp(default, new float3(2f)), Is.EqualTo(new float3(0.5f)));
            Assert.That(new float3(0.5f).remap(default, new float3(1f), default, new float3(10f)),
                Is.EqualTo(new float3(5f)));
        }
    }

    /// <summary>
    /// The members that dispatch the value of a vector dispatch the value of a matrix as well: the value of
    /// every column of it is dispatched the same way as the value of a vector, so a matrix of any shape reaches
    /// the members and a matrix without a register reaches them through the columns of it as well.
    /// </summary>
    [Test]
    public void Matrix()
    {
        // every shape reaches the members, with and without a register in the columns of the matrix
        CheckMatrix(new float2x2(new float2(1f, 2f), new float2(3f, 4f)));
        CheckMatrix(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)));
        CheckMatrix(new double2x4(new double2(1d, 2d), new double2(3d, 4d), new double2(5d, 6d),
            new double2(7d, 8d)));
        CheckMatrix(new int4x4(new int4(1), new int4(2), new int4(3), new int4(4)));
        CheckMatrix(new float3x3s(new float3s(1f, 2f, 3f), new float3s(4f, 5f, 6f), new float3s(7f, 8f, 9f)));

        using (Assert.EnterMultipleScope())
        {
            // a shape without a member of the visitor of its own hands the value of every column over, so the
            // columns of the matrix are the ones that reach the member of their own kind
            Assert.That(math.abs(new float3x2(new float3(-1f, 2f, -3f), new float3(4f, -5f, 6f))),
                Is.EqualTo(new float3x2(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f))));
            Assert.That(math.abs(new float2x2(new float2(-1f, 2f), new float2(3f, -4f))),
                Is.EqualTo(new float2x2(new float2(1f, 2f), new float2(3f, 4f))));
            Assert.That(math.sign(new int2x2(new int2(-5, 0), new int2(0, 5))),
                Is.EqualTo(new int2x2(new int2(-1, 0), new int2(0, 1))));
            Assert.That(math.sign(new int3x3(new int3(-2, 0, 2), new int3(3, -4, 0), new int3(0, 5, -6))),
                Is.EqualTo(new int3x3(new int3(-1, 0, 1), new int3(1, -1, 0), new int3(0, 1, -1))));
            var a = new float3x3s(new float3s(1f, 5f, 3f), new float3s(4f, 2f, 6f), new float3s(7f, 8f, 9f));
            var b = new float3x3s(new float3s(9f, 2f, 1f), new float3s(4f, 8f, 6f), new float3s(1f, 2f, 3f));
            Assert.That(math.min(a, b),
                Is.EqualTo(new float3x3s(new float3s(1f, 2f, 1f), new float3s(4f, 2f, 6f), new float3s(1f, 2f, 3f))));
            Assert.That(math.max(a, b),
                Is.EqualTo(new float3x3s(new float3s(9f, 5f, 3f), new float3s(4f, 8f, 6f), new float3s(7f, 8f, 9f))));
            var c = new float3x2(new float3(1f, 5f, 3f), new float3(4f, 2f, 6f));
            var d = new float3x2(new float3(9f, 2f, 1f), new float3(4f, 8f, 6f));
            Assert.That(math.min(c, d), Is.EqualTo(new float3x2(new float3(1f, 2f, 1f), new float3(4f, 2f, 6f))));
            Assert.That(math.max(c, d), Is.EqualTo(new float3x2(new float3(9f, 5f, 3f), new float3(4f, 8f, 6f))));
            // a member that takes three values reaches the value of every column of a matrix as well
            Assert.That(math.clamp(c, new float3x2(new float3(1f, 2f, 2f), new float3(2f, 2f, 2f)),
                    new float3x2(new float3(8f, 8f, 8f), new float3(8f, 4f, 4f))),
                Is.EqualTo(new float3x2(new float3(1f, 5f, 3f), new float3(4f, 2f, 4f))));
            var e = new float3x3s(new float3s(1f, 5f, 3f), new float3s(4f, 2f, 6f), new float3s(7f, 8f, 9f));
            Assert.That(math.clamp(e, new float3x3s(new float3s(2f, 2f, 2f), new float3s(2f, 2f, 2f),
                    new float3s(2f, 2f, 2f)), new float3x3s(new float3s(6f, 6f, 6f), new float3s(6f, 6f, 6f),
                    new float3s(6f, 6f, 6f))),
                Is.EqualTo(new float3x3s(new float3s(2f, 5f, 3f), new float3s(4f, 2f, 6f),
                    new float3s(6f, 6f, 6f))));
            Assert.That(math.clamp(new int2x2(new int2(-1, 2), new int2(5, 0)), new int2x2(new int2(0), new int2(0)),
                    new int2x2(new int2(3), new int2(3))),
                Is.EqualTo(new int2x2(new int2(0, 2), new int2(3, 0))));
            // a member that takes three values reaches the value of every column of a matrix as well
            Assert.That(math.lerp(new float3x2(new float3(0f), new float3(2f)),
                    new float3x2(new float3(4f), new float3(8f)),
                    new float3x2(new float3(0.5f), new float3(0.25f))),
                Is.EqualTo(new float3x2(new float3(2f), new float3(3.5f))));
        }
    }

    /// <summary>
    /// The members that name the type of a single component reach the ones of the vector as well. The type of a
    /// component that is an argument is inferred from it, the type that is only the result of the member is
    /// spelled out by the caller.
    /// </summary>
    [Test]
    public void Scalar()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.clamp(new float3(-1f, 2f, 5f), 0f, 3f), Is.EqualTo(new float3(0f, 2f, 3f)));
            Assert.That(math.lerp(0f, 1f, new float3(0.5f)), Is.EqualTo(new float3(0.5f)));
            // the bounds as single components reach a vector without a register, a matrix and a value whose
            // component is not a floating point one as well
            Assert.That(math.lerp(0f, 1f, new float3s(0.5f)), Is.EqualTo(new float3s(0.5f)));
            Assert.That(math.lerp(0f, 2f, new float2x2(new float2(0.5f), new float2(0.25f))),
                Is.EqualTo(new float2x2(new float2(1f), new float2(0.5f))));
            Assert.That(math.lerp(1, 3, new int3(0)), Is.EqualTo(new int3(1)));
            Assert.That(math.lerp(1, 3, new int3(1)), Is.EqualTo(new int3(3)));
            Assert.That(math.lerp(1, 3, new int3(2)), Is.EqualTo(new int3(5)));
            // the member of the value puts the factor first
            Assert.That(new float3(0.5f).lerp(0f, 1f), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.lerp(default(float3), new float3(1f), 0.5f), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.unlerp(new float3(1.5f), 1f, 2f), Is.EqualTo(new float3(0.5f)));
            Assert.That(math.unlerp(1.5f, default, new float3(2f)), Is.EqualTo(new float3(0.75f)));
            Assert.That(math.remap(new float3(0.5f), 0f, 1f, 0f, 10f), Is.EqualTo(new float3(5f)));
            // the component that the member is called on is the one that the inverse places
            Assert.That(1.5f.unlerp(new float3(1f), new float3(2f)), Is.EqualTo(new float3(0.5f)));
            Assert.That(1.5f.remap(new float3(1f), new float3(2f), default, new float3(10f)),
                Is.EqualTo(new float3(5f)));
            Assert.That(math.dot<float3, float>(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)), Is.EqualTo(32f));
            Assert.That(math.length_sq<float3, float>(new float3(3f, 4f, 0f)), Is.EqualTo(25f));
            Assert.That(math.distance_sq<float3, float>(new float3(1f), new float3(4f, 5f, 1f)), Is.EqualTo(25f));
            Assert.That(math.csum<float3, float>(new float3(1f, 2f, 3f)), Is.EqualTo(6f));
            // the padding component of a vector is zero, so the minimum of a vector is only the component itself
            // when every component is less than it
            Assert.That(math.cmin<float3, float>(new float3(-3f, -1f, -2f)), Is.EqualTo(-3f));
            Assert.That(math.cmax<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(3f));
            Assert.That(math.cmin_safe<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(1f));
            Assert.That(math.cmax_safe<float3, float>(new float3(3f, 1f, 2f)), Is.EqualTo(3f));
        }
    }

    /// <summary>
    /// The cross product is only reachable with a vector of 3 components, so the constraint of it is the
    /// interface of the 3 component arithmetic.
    /// </summary>
    [Test]
    public void Cross()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.cross(new int3(1, 0, 0), new int3(0, 1, 0)), Is.EqualTo(new int3(0, 0, 1)));
            Assert.That(math.cross(new float3(1f, 0f, 0f), new float3(0f, 1f, 0f)), Is.EqualTo(new float3(0f, 0f, 1f)));
            Assert.That(math.cross(new double3s(1, 0, 0), new double3s(0, 1, 0)), Is.EqualTo(new double3s(0, 0, 1)));
        }
    }

    /// <summary>
    /// The fused members are the ones of the vector, the multiplication and the addition are only fused by the
    /// operand order of the name.
    /// </summary>
    [Test]
    public void Fused()
    {
        var a = new float3(2f, 3f, 4f);
        var b = new float3(5f, 6f, 7f);
        var c = new float3(10f, 20f, 30f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.fma(a, b, c), Is.EqualTo(a * b + c));
            Assert.That(math.fms(a, b, c), Is.EqualTo(a * b - c));
            Assert.That(math.fnma(a, b, c), Is.EqualTo(c - a * b));
            Assert.That(math.fsm(c, a, b), Is.EqualTo(c - a * b));
            Assert.That(math.fam(c, a, b), Is.EqualTo(c + a * b));
            Assert.That(math.mad(a, b, c), Is.EqualTo(a * b + c));
        }
    }
}
