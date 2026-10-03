using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Arith;

/// <summary>
/// The projection of a value onto a vector is the member of the algebra of the floating point kind: it is the
/// component of the value that is parallel to the vector, which is the vector scaled by the quotient of the dot
/// product of the two and the dot product of the vector with itself. The value of a vector that keeps it in a
/// register reaches the member of the register, which takes the dot product of the register itself, the value of
/// every other vector reaches the member of the components of it. The projection of the value onto the plane of
/// the vector is the difference of the value and the projection of it onto the vector, the component of it that
/// is inside the plane.
/// </summary>
public class TestProject
{
    /// <summary>
    /// The members are the ones of the algebra of the floating point kind, so a parameter that only knows the
    /// interface reaches them, which is the form the members of the library use. The vector below is not the unit
    /// length one of the pair of it, so the two forms of every member are only checked to agree here, what they
    /// hold is checked below.
    /// </summary>
    private static void Check<T>(T value, T onto)
        where T : unmanaged, IAlgebraDispatch<T>, IFloatingPointVector<T>
    {
        Assert.That(value.project(onto), Is.EqualTo(math.project(value, onto)));
        Assert.That(value.project_unsafe(onto), Is.EqualTo(math.project_unsafe(value, onto)));
        Assert.That(value.project_unit(onto), Is.EqualTo(math.project_unit(value, onto)));
        Assert.That(value.project_on_plane(onto), Is.EqualTo(math.project_on_plane(value, onto)));
        Assert.That(value.project_on_plane_unit(onto), Is.EqualTo(math.project_on_plane_unit(value, onto)));
    }

    [Test]
    public void Interface()
    {
        Check(new float2(1f, 2f), new float2(2f, 0f));
        Check(new float3(1f, 2f, 3f), new float3(2f, 0f, 0f));
        Check(new float4(1f, 2f, 3f, 4f), new float4(0f, 1f, 0f, 0f));
        Check(new double2(1, 2), new double2(2, 0));
        Check(new double3(1, 2, 3), new double3(2, 0, 0));
        Check(new double4(1, 2, 3, 4), new double4(0, 1, 0, 0));
        // a vector without a register reaches the member of the components of it
        Check(new half2((Half)1f, (Half)2f), new half2((Half)2f, (Half)0f));
        Check(new half3((Half)1f, (Half)2f, (Half)3f), new half3((Half)2f, (Half)0f, (Half)0f));
    }

    [Test]
    public void Value()
    {
        var value = new float3(1f, 2f, 3f);
        var axis = new float3(2f, 0f, 0f);
        var unit = new float3(1f, 0f, 0f);

        using (Assert.EnterMultipleScope())
        {
            // the projection of the value onto an axis is the component of the axis of it, the projection onto
            // the plane of the axis is the rest of the value
            Assert.That(math.project(value, axis), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(math.project_unit(value, unit), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(math.project_on_plane(value, axis), Is.EqualTo(new float3(0f, 2f, 3f)));
            Assert.That(math.project_on_plane_unit(value, unit), Is.EqualTo(new float3(0f, 2f, 3f)));

            // the projection of a value onto itself is the value, the one of it onto the plane of itself is the
            // zero of it
            Assert.That(math.project(value, value), Is.EqualTo(value));
            Assert.That(math.project_unit(unit, unit), Is.EqualTo(unit));
            Assert.That(math.project_on_plane(value, value), Is.EqualTo(default(float3)));

            // the dot product of the zero vector with itself is zero, which the quotient of the projection
            // cannot take, so the projection of the value onto it is the zero of it
            Assert.That(math.project(value, default), Is.EqualTo(default(float3)));
            Assert.That(math.project_on_plane(value, default), Is.EqualTo(value));
            // the unit length pair takes no quotient, the dot product of the value and the zero vector is zero
            Assert.That(math.project_unit(value, default), Is.EqualTo(default(float3)));
            Assert.That(math.project_on_plane_unit(value, default), Is.EqualTo(value));

            Assert.That(math.project(new float2(1f, 2f), new float2(2f, 0f)), Is.EqualTo(new float2(1f, 0f)));
            Assert.That(math.project(new float4(1f, 2f, 3f, 4f), new float4(0f, 2f, 0f, 0f)),
                Is.EqualTo(new float4(0f, 2f, 0f, 0f)));
            Assert.That(math.project(new double2(1, 2), new double2(2, 0)), Is.EqualTo(new double2(1d, 0d)));

            // a vector without a register reaches the member of the component type for every component
            Assert.That(math.project(new half3((Half)1f, (Half)2f, (Half)3f), new half3((Half)2f, (Half)0f, (Half)0f)),
                Is.EqualTo(new half3((Half)1f, (Half)0f, (Half)0f)));
            Assert.That(math.project_on_plane_unit(new double3(1, 2, 3), new double3(0, 1, 0)),
                Is.EqualTo(new double3(1d, 0d, 3d)));
        }
    }

    /// <summary>
    /// The projection of a value onto a vector and the one of it onto the plane of the vector are the two
    /// components of the value, the sum of the two is the value itself. The projection of the value onto a vector
    /// of unit length is the same as the one onto the vector itself, the two are the same one for it.
    /// </summary>
    [Test]
    public void Decomposition()
    {
        var value = new float3(1.5f, -2.5f, 3.5f);
        var onto = new float3(1.25f, 2.75f, -0.5f);

        var d = (onto.x * onto.x) + (onto.y * onto.y) + (onto.z * onto.z);
        var c = ((value.x * onto.x) + (value.y * onto.y) + (value.z * onto.z)) / d;

        var p = math.project(value, onto);
        var q = math.project_on_plane(value, onto);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(p.x + q.x, Is.EqualTo(value.x).Within(1e-5f));
            Assert.That(p.y + q.y, Is.EqualTo(value.y).Within(1e-5f));
            Assert.That(p.z + q.z, Is.EqualTo(value.z).Within(1e-5f));

            // the value built from the members of the components is the value of the vector
            Assert.That(p.x, Is.EqualTo(c * onto.x).Within(1e-5f));
            Assert.That(p.y, Is.EqualTo(c * onto.y).Within(1e-5f));
            Assert.That(p.z, Is.EqualTo(c * onto.z).Within(1e-5f));

            // the vector of unit length is the vector divided by the length of it
            var length = MathF.Sqrt(d);
            var unit = new float3(onto.x / length, onto.y / length, onto.z / length);
            var pu = math.project_unit(value, unit);
            var qu = math.project_on_plane_unit(value, unit);
            Assert.That(pu.x, Is.EqualTo(p.x).Within(1e-5f));
            Assert.That(pu.y, Is.EqualTo(p.y).Within(1e-5f));
            Assert.That(pu.z, Is.EqualTo(p.z).Within(1e-5f));
            Assert.That(qu.x, Is.EqualTo(q.x).Within(1e-5f));
            Assert.That(qu.y, Is.EqualTo(q.y).Within(1e-5f));
            Assert.That(qu.z, Is.EqualTo(q.z).Within(1e-5f));
        }
    }

    /// <summary>
    /// The projection without the check of the denominator is the same operation as the one with the check for
    /// every vector whose dot product with itself is not below the denominator epsilon of the kind of it, the two
    /// only part for a vector that is close to the zero of the kind of it.
    /// </summary>
    [Test]
    public void Unsafe()
    {
        var value = new float3(1f, 2f, 3f);
        var axis = new float3(2f, 0f, 0f);

        using (Assert.EnterMultipleScope())
        {
            // the two members agree on a vector whose dot product with itself is not below the constant
            Assert.That(math.project_unsafe(value, axis), Is.EqualTo(math.project(value, axis)));
            Assert.That(math.project_unsafe(value, value), Is.EqualTo(math.project(value, value)));

            // the dot product of the zero vector with itself is zero, which is below the constant of the kind of
            // it: the checked projection returns the zero of it and the unchecked one takes the quotient of zero
            // by zero, which is not a number
            Assert.That(math.project(value, default), Is.EqualTo(default(float3)));
            Assert.That(float.IsNaN(math.project_unsafe(value, default).x), Is.True);
            Assert.That(float.IsNaN(math.project_unsafe(value, default).z), Is.True);

            // a vector whose dot product with itself is below the constant is not the zero of the kind of it for
            // the checked projection: the constant of a float is 1e-8, so the dot product of a vector of the
            // component 1e-5 with itself is below it
            var tiny = new float3(1e-5f, 0f, 0f);
            Assert.That(math.project(value, tiny), Is.EqualTo(default(float3)));
            Assert.That(math.project_unsafe(value, tiny).x, Is.EqualTo(1f).Within(1e-4f));

            // the member of the components of a vector without a register checks the same constant
            // the constant of a half is 1e-3, which is far above the one of a float
            Assert.That(math.project(new half3((Half)1f, (Half)2f, (Half)3f),
                    new half3((Half)0.01f, (Half)0f, (Half)0f)),
                Is.EqualTo(default(half3)));
        }
    }

    /// <summary>
    /// A padding lane of the register of a vector holds no component and both the value of the lane and the one
    /// of the vector of it are zero, so the projection of it is the zero of it as well.
    /// </summary>
    [Test]
    public void PaddingLanes()
    {
        var f2 = new float2(1f, 2f);
        var n2 = new float2(2f, 0f);
        var f3 = new float3(1f, 2f, 3f);
        var n3 = new float3(2f, 0f, 0f);
        var d3 = new double3(1, 2, 3);
        var dy3 = new double3(2, 0, 0);

        using (Assert.EnterMultipleScope())
        {
            // the register of a vector of two components is 128 bits wide for the value of it alone, the one of a
            // vector of three components holds the fourth lane as padding
            Assert.That(math.project(f2, n2).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.project(f2, n2).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project(f3, n3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project(d3, dy3).vector.GetElement(3), Is.EqualTo(0d));

            Assert.That(math.project_unit(f2, new float2(1f, 0f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.project_unit(f2, new float2(1f, 0f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project_unit(f3, new float3(1f, 0f, 0f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project_unit(d3, new double3(1, 0, 0)).vector.GetElement(3), Is.EqualTo(0d));

            // the projection onto the plane is the difference of two values, so the padding lane of it is the
            // difference of the ones of them, which are zero
            Assert.That(math.project_on_plane(f2, n2).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project_on_plane(f3, n3).vector.GetElement(3), Is.EqualTo(0f));

            // the projection without the check of the denominator takes the same members
            Assert.That(math.project_unsafe(f2, n2).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(math.project_unsafe(f2, n2).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project_unsafe(f3, n3).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.project_unsafe(d3, dy3).vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(math.project_on_plane_unit(f3, new float3(1f, 0f, 0f)).vector.GetElement(3),
                Is.EqualTo(0f));
        }
    }
}
