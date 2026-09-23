using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;

namespace Tests.Core;

/// <summary>
/// The kind of a value is only known from its type: the width of the register of a vector and the count of its
/// components are a part of the type, so the type itself dispatches a call to the member of a visitor that
/// matches them and hands the value of it over to that member. A vector that keeps its value in a register
/// hands the register over, every other vector hands the vector itself over and the value of it reaches the
/// member of the scalar for every component. The shape of a matrix is a part of its type as well, so the same
/// visitor serves a matrix: the type of the matrix reaches the member of its shape and a shape without a member
/// of its own hands the value of every column of the matrix over.
/// </summary>
public class TestDynamicVector
{
    /// <summary>Builds the value that dispatched the call out of the value it is handed</summary>
    private readonly struct AgainVisitor : INumberAlgebraVisitor_Self_Self<AgainVisitor>
    {
        /// <summary>The value of a scalar is handed over as it is</summary>
        static TScalar INumberAlgebraVisitor_Self_Self<AgainVisitor>.AcceptScalar<TScalar>(TScalar value) => value;

        /// <summary>Builds the value of a vector out of its register of 64 bits</summary>
        static TVector INumberAlgebraVisitor_Self_Self<AgainVisitor>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
            => TVector.FromUnderlying(vector.As<TScalar, byte>());

        /// <summary>Builds the value of a vector out of its register of 128 bits</summary>
        static TVector INumberAlgebraVisitor_Self_Self<AgainVisitor>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.FromUnderlying(vector.As<TScalar, byte>());

        /// <summary>Builds the value of a vector out of its register of 256 bits</summary>
        static TVector INumberAlgebraVisitor_Self_Self<AgainVisitor>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.FromUnderlying(vector.As<TScalar, byte>());
    }

    /// <summary>Builds the value that dispatched the call out of the second of the two values it is handed</summary>
    private readonly struct SecondVisitor : INumberAlgebraVisitor_Self_Self_Self<SecondVisitor>
    {
        /// <summary>The second of the two scalars</summary>
        static TScalar INumberAlgebraVisitor_Self_Self_Self<SecondVisitor>.AcceptScalar<TScalar>(TScalar a, TScalar b) => b;

        /// <summary>Builds the second value out of its register of 64 bits</summary>
        static TVector INumberAlgebraVisitor_Self_Self_Self<SecondVisitor>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            => TVector.FromUnderlying(b.As<TScalar, byte>());

        /// <summary>Builds the second value out of its register of 128 bits</summary>
        static TVector INumberAlgebraVisitor_Self_Self_Self<SecondVisitor>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.FromUnderlying(b.As<TScalar, byte>());

        /// <summary>Builds the second value out of its register of 256 bits</summary>
        static TVector INumberAlgebraVisitor_Self_Self_Self<SecondVisitor>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.FromUnderlying(b.As<TScalar, byte>());
    }

    /// <summary>
    /// Tells which member of the visitor the dispatch reached: the register of a vector is replaced by the one
    /// of the vector, every other value reaches the scalar of the visitor for every one of its components and
    /// adds one to it
    /// </summary>
    private readonly struct WhichVisitor : INumberAlgebraVisitor_Self_Self<WhichVisitor>
    {
        /// <summary>Adds one to the value, it tells that the scalar of the visitor was reached</summary>
        static TScalar INumberAlgebraVisitor_Self_Self<WhichVisitor>.AcceptScalar<TScalar>(TScalar value)
            => value + TScalar.One;

        /// <summary>Replaces the value of a register of 64 bits by the one of the vector</summary>
        static TVector INumberAlgebraVisitor_Self_Self<WhichVisitor>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
            => TVector.One;

        /// <summary>Replaces the value of a register of 128 bits by the one of the vector</summary>
        static TVector INumberAlgebraVisitor_Self_Self<WhichVisitor>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.One;

        /// <summary>Replaces the value of a register of 256 bits by the one of the vector</summary>
        static TVector INumberAlgebraVisitor_Self_Self<WhichVisitor>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.One;
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // a vector that keeps its value in a register hands the register over
            Assert.That(float2.Visit_Self<AgainVisitor>(new float2(1, 2)), Is.EqualTo(new float2(1, 2)));
            Assert.That(float3.Visit_Self<AgainVisitor>(new float3(1, 2, 3)), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(double4.Visit_Self<AgainVisitor>(new double4(1, 2, 3, 4)), Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That(int2.Visit_Self<AgainVisitor>(new int2(1, 2)), Is.EqualTo(new int2(1, 2)));
            // a vector without a register hands the vector itself over, the value of it reaches the scalar of
            // the visitor for every one of its components
            Assert.That(half2.Visit_Self<AgainVisitor>(new half2((Half)1, (Half)2)),
                Is.EqualTo(new half2((Half)1, (Half)2)));
            Assert.That(float3s.Visit_Self<AgainVisitor>(new float3s(1, 2, 3)), Is.EqualTo(new float3s(1, 2, 3)));
            Assert.That(int2s.Visit_Self<AgainVisitor>(new int2s(1, 2)), Is.EqualTo(new int2s(1, 2)));
            // a matrix reaches the member of its shape, which hands the value of every column of it over
            Assert.That(float2x2.Visit_Self<AgainVisitor>(new float2x2(new float2(1, 2), new float2(3, 4))),
                Is.EqualTo(new float2x2(new float2(1, 2), new float2(3, 4))));
            // a shape the visitor has no member for hands the value of every column of the matrix over
            Assert.That(float3x2.Visit_Self<AgainVisitor>(new float3x2(new float3(1, 2, 3), new float3(4, 5, 6))),
                Is.EqualTo(new float3x2(new float3(1, 2, 3), new float3(4, 5, 6))));
            Assert.That(double2x4.Visit_Self<AgainVisitor>(
                    new double2x4(new double2(1, 2), new double2(3, 4), new double2(5, 6), new double2(7, 8))),
                Is.EqualTo(new double2x4(new double2(1, 2), new double2(3, 4), new double2(5, 6), new double2(7, 8))));
            // a matrix without a register hands the value of every one of its columns over the same way
            Assert.That(float3x3s.Visit_Self<AgainVisitor>(
                    new float3x3s(new float3s(1, 2, 3), new float3s(4, 5, 6), new float3s(7, 8, 9))),
                Is.EqualTo(new float3x3s(new float3s(1, 2, 3), new float3s(4, 5, 6), new float3s(7, 8, 9))));
            // every member of a visitor names the algebra of a number, so a mask does not dispatch
            Assert.That(typeof(float2).GetMethod("Visit_Self", new[] { typeof(float2).MakeByRefType() }), Is.Not.Null);
            Assert.That(typeof(b32v2).GetMethod("Visit_Self", new[] { typeof(b32v2).MakeByRefType() }), Is.Null);
            Assert.That(typeof(b16m2x2).GetMethod("Visit_Self", new[] { typeof(b16m2x2).MakeByRefType() }), Is.Null);
        }
    }

    [Test]
    public void Second()
    {
        using (Assert.EnterMultipleScope())
        {
            // both of the values are handed over the same way, like the members that take a single one
            Assert.That(float2.Visit_Self<SecondVisitor>(new float2(1, 2), new float2(3, 4)),
                Is.EqualTo(new float2(3, 4)));
            Assert.That(float3.Visit_Self<SecondVisitor>(new float3(1, 2, 3), new float3(4, 5, 6)),
                Is.EqualTo(new float3(4, 5, 6)));
            Assert.That(double4.Visit_Self<SecondVisitor>(new double4(1, 2, 3, 4), new double4(5, 6, 7, 8)),
                Is.EqualTo(new double4(5, 6, 7, 8)));
            Assert.That(half2.Visit_Self<SecondVisitor>(new half2((Half)1, (Half)2), new half2((Half)3, (Half)4)),
                Is.EqualTo(new half2((Half)3, (Half)4)));
            Assert.That(float3s.Visit_Self<SecondVisitor>(new float3s(1, 2, 3), new float3s(4, 5, 6)),
                Is.EqualTo(new float3s(4, 5, 6)));
            Assert.That(int2s.Visit_Self<SecondVisitor>(new int2s(1, 2), new int2s(3, 4)),
                Is.EqualTo(new int2s(3, 4)));
            // every column of a matrix is handed over the same way
            var a = new float3x2(new float3(1, 2, 3), new float3(4, 5, 6));
            var b = new float3x2(new float3(7, 8, 9), new float3(10, 11, 12));
            Assert.That(float3x2.Visit_Self<SecondVisitor>(a, b), Is.EqualTo(b));
            var c = new float2x2(new float2(1, 2), new float2(3, 4));
            var d = new float2x2(new float2(5, 6), new float2(7, 8));
            Assert.That(float2x2.Visit_Self<SecondVisitor>(c, d), Is.EqualTo(d));
        }
    }

    [Test]
    public void Kind()
    {
        using (Assert.EnterMultipleScope())
        {
            // the width of the register of the vector decides the member: a value that is handed over as a
            // register is replaced by the one of the vector
            Assert.That(float2.Visit_Self<WhichVisitor>(new float2(1, 2)), Is.EqualTo(new float2(1, 1)));
            Assert.That(float4.Visit_Self<WhichVisitor>(new float4(1, 2, 3, 4)), Is.EqualTo(new float4(1, 1, 1, 1)));
            Assert.That(int3.Visit_Self<WhichVisitor>(new int3(4, 5, 6)), Is.EqualTo(new int3(1, 1, 1)));
            // a vector without a register hands the vector itself over, so the member of the scalar is reached
            // for every component of it. A storage variant of a vector of 2 components keeps its value in a
            // register of 64 bits, so it reaches the member of the register like the other ones
            Assert.That(float2s.Visit_Self<WhichVisitor>(new float2s(1, 2)), Is.EqualTo(new float2s(1, 1)));
            Assert.That(float3s.Visit_Self<WhichVisitor>(new float3s(1, 2, 3)), Is.EqualTo(new float3s(2, 3, 4)));
            Assert.That(double3s.Visit_Self<WhichVisitor>(new double3s(1, 2, 3)), Is.EqualTo(new double3s(2, 3, 4)));
            Assert.That(half2.Visit_Self<WhichVisitor>(new half2((Half)1, (Half)2)),
                Is.EqualTo(new half2((Half)2, (Half)3)));
            // a matrix of a number dispatches the value of its columns, every one of them reaching the member
            // of the width of its register, and a mask dispatches nothing at all
            Assert.That(float2x2.Visit_Self<WhichVisitor>(new float2x2(new float2(1, 2), new float2(3, 4))),
                Is.EqualTo(new float2x2(new float2(1, 1), new float2(1, 1))));
        }
    }
}
