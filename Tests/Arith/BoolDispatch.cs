using System.Numerics;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The bool value of a value has the same shape as it: the mask of a vector is a vector of the same count and
/// the mask of a matrix is a matrix of the same shape, and the mask of a mask is the mask itself. The dispatch
/// of the bool value of a value names the type of the bool value, so a caller that does not know the shape of
/// the value reaches the member that builds it, and the visitor of it reaches the component of the value alone:
/// the member of a register, of a vector that has no register and of a matrix is the one of the shape and every
/// one of them reaches the member of the scalar of the visitor for the component of the shape.
/// </summary>
public class TestBoolDispatch
{
    /// <summary>
    /// The visitor that says whether a component is greater than zero. The mask of a component is a mask of the
    /// kind of the component, which the type of a single component of the mask names, so the member of a scalar
    /// reaches it through the type of the mask it built the value of.
    /// </summary>
    internal struct impl_is_positive : IFloatingPointAlgebraVisitor_Self_Bool<impl_is_positive>
    {
        public static TBoolScalar AcceptScalar<TScalar, TBoolScalar>(TScalar value)
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            where TBoolScalar : unmanaged, IMask<TBoolScalar>
        {
            var positive = value > TScalar.Zero;
            if (typeof(TBoolScalar) == typeof(b16)) return (TBoolScalar)(object)(positive ? b16.True : b16.False);
            if (typeof(TBoolScalar) == typeof(b32)) return (TBoolScalar)(object)(positive ? b32.True : b32.False);
            if (typeof(TBoolScalar) == typeof(b64)) return (TBoolScalar)(object)(positive ? b64.True : b64.False);
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// The member of the dispatch is the one of the interface, which a value of the kind of it implements, so a
    /// caller that only knows a type parameter reaches it.
    /// </summary>
    private static TBool BoolOf<T, TBool>(in T value)
        where T : unmanaged, IFloatingPointAlgebraBoolDispatch<T, TBool>
        where TBool : unmanaged, IBoolMatrix<TBool>
        => T.Visit_Self_Bool<impl_is_positive>(value);

    /// <summary>
    /// The mask of a vector has the count of its components and the member of the dispatch of the vector
    /// reaches the member of the visitor of the count of them or the one of its register, which agree.
    /// </summary>
    [Test]
    public void Vector()
    {
        using (Assert.EnterMultipleScope())
        {
            // a register of 128 bits holds a vector of 3 components and one of 4
            var f3 = BoolOf<float3, b32v3>(new float3(1f, -2f, 0f));
            Assert.That((bool)f3.x, Is.True);
            Assert.That((bool)f3.y, Is.False);
            Assert.That((bool)f3.z, Is.False);

            var f4 = BoolOf<float4, b32v4>(new float4(1f, -2f, 0f, 3f));
            Assert.That((bool)f4.x, Is.True);
            Assert.That((bool)f4.y, Is.False);
            Assert.That((bool)f4.z, Is.False);
            Assert.That((bool)f4.w, Is.True);

            // the register of a vector of 2 components is 64 bits wide and the one of a double is 128 bits wide
            var f2 = BoolOf<float2, b32v2>(new float2(-1f, 2f));
            Assert.That((bool)f2.x, Is.False);
            Assert.That((bool)f2.y, Is.True);

            var d3 = BoolOf<double3, b64v3>(new double3(-1d, 2d, 0d));
            Assert.That((bool)d3.x, Is.False);
            Assert.That((bool)d3.y, Is.True);
            Assert.That((bool)d3.z, Is.False);

            // a vector without a register reaches the member of the component type
            var h3 = BoolOf<half3, b16v3>(new half3((half)1f, (half)(-1f), (half)0f));
            Assert.That((bool)h3.x, Is.True);
            Assert.That((bool)h3.y, Is.False);
            Assert.That((bool)h3.z, Is.False);

            // the value of a storage variant is kept in fields, the mask of it is the one of the vector
            var f3s = BoolOf<float3s, b32v3>(new float3s(1f, -2f, 0f));
            Assert.That((bool)f3s.x, Is.True);
            Assert.That((bool)f3s.y, Is.False);
            Assert.That((bool)f3s.z, Is.False);
        }
    }

    /// <summary>
    /// The mask of a matrix has the shape of it, which the member of the count of the columns of the visitor
    /// reaches through the columns of the value.
    /// </summary>
    [Test]
    public void Matrix()
    {
        var v = new float3x3(
            new float3(1f, -2f, 0f),
            new float3(-3f, 4f, -5f),
            new float3(6f, 7f, 8f));

        var m = BoolOf<float3x3, b32m3x3>(v);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)m.c0.x, Is.True);
            Assert.That((bool)m.c0.y, Is.False);
            Assert.That((bool)m.c0.z, Is.False);

            Assert.That((bool)m.c1.x, Is.False);
            Assert.That((bool)m.c1.y, Is.True);
            Assert.That((bool)m.c1.z, Is.False);

            Assert.That((bool)m.c2.x, Is.True);
            Assert.That((bool)m.c2.y, Is.True);
            Assert.That((bool)m.c2.z, Is.True);

            // the shape of the matrix is the one of its mask
            Assert.That(b32m3x3.Rows, Is.EqualTo(3));
            Assert.That(b32m3x3.Columns, Is.EqualTo(3));
            Assert.That(typeof(b32m3x3).Name, Is.EqualTo("b32m3x3"));
        }

        // the mask of a matrix without a register is the mask of the component of every column of it
        var h2x2 = BoolOf<half2x2, b16m2x2>(new half2x2(
            new half2((half)1f, (half)(-1f)),
            new half2((half)(-2f), (half)3f)));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)h2x2.c0.x, Is.True);
            Assert.That((bool)h2x2.c0.y, Is.False);
            Assert.That((bool)h2x2.c1.x, Is.False);
            Assert.That((bool)h2x2.c1.y, Is.True);
        }
    }

    /// <summary>
    /// The member of the interface is the one of the type parameter, so the value of a parameter that only
    /// knows the interface reaches it, which is the form the members of the library use.
    /// </summary>
    private static void Check<T, TBool>(T value)
        where T : unmanaged, IFloatingPointAlgebraBoolDispatch<T, TBool>
        where TBool : unmanaged, IBoolMatrix<TBool>
    {
        _ = T.Visit_Self_Bool<impl_is_positive>(value);
    }

    [Test]
    public void Interface()
    {
        Check<float2, b32v2>(new float2(1f, 2f));
        Check<float3, b32v3>(new float3(1f, 2f, 3f));
        Check<float4, b32v4>(new float4(1f, 2f, 3f, 4f));
        Check<double2, b64v2>(new double2(1d, 2d));
        Check<double3, b64v3>(new double3(1d, 2d, 3d));
        Check<double4, b64v4>(new double4(1d, 2d, 3d, 4d));
        Check<half2, b16v2>(new half2((half)1f, (half)2f));
        Check<half3, b16v3>(new half3((half)1f, (half)2f, (half)3f));
        Check<half4, b16v4>(new half4((half)1f, (half)2f, (half)3f, (half)4f));
        Check<float3s, b32v3>(new float3s(1f, 2f, 3f));
        Check<double3s, b64v3>(new double3s(1d, 2d, 3d));
        Check<float2x2, b32m2x2>(new float2x2(new float2(1f, 2f), new float2(3f, 4f)));
        Check<float3x3, b32m3x3>(new float3x3(new float3(1f), new float3(2f), new float3(3f)));
        Check<half2x2, b16m2x2>(new half2x2(new half2((half)1f), new half2((half)2f)));
        Check<float2x2s, b32m2x2>(new float2x2s(new float2s(1f, 2f), new float2s(3f, 4f)));
    }
}
