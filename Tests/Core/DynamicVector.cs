using System.Reflection;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Tests.Core;

/// <summary>
/// The kind of a value is only known from its type: the width of the register of a vector and the count of its
/// components are a part of the type, so the type itself dispatches a call to the member of a visitor that
/// matches them and hands the value of it over to that member. A vector that keeps its value in a register
/// hands the register over, every other vector hands the vector itself over and the value of it reaches the
/// member of the scalar for every component. The shape of a matrix is a part of its type as well, so the same
/// visitor serves a matrix: the type of the matrix reaches the member of its shape and a shape without a member
/// of its own hands the value of every column of the matrix over. The members that dispatch a value are
/// implemented explicitly, so this file reaches them through the interface of the dispatch of a parameter.
/// <para>The algebra a value belongs to is a part of its type as well: the type of it implements the dispatch
/// of every kind of it, which reaches the members of the kind. A floating point number reaches the members a
/// floating point number has of its own beside the ones of a number, every kind of it dispatching the values of
/// the kind alone.</para>
/// </summary>
public class TestDynamicVector
{
    /// <summary>Builds the value that dispatched the call out of the value it is handed</summary>
    private readonly struct AgainVisitor : IAlgebraVisitor_T_T<AgainVisitor>
    {
        /// <summary>The value of a scalar is handed over as it is</summary>
        static TScalar IAlgebraVisitor_S_S<AgainVisitor>.Scalar_Number<TScalar>(TScalar value) => value;

        /// <summary>Builds the value of a vector out of its register of 128 bits</summary>
        static TVector IAlgebraVisitor_T_T<AgainVisitor>.Simd_Any<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.FromUnderlying(vector.As<TScalar, byte>());

        /// <summary>Builds the value of a vector out of its register of 256 bits</summary>
        static TVector IAlgebraVisitor_T_T<AgainVisitor>.Simd_Any<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.FromUnderlying(vector.As<TScalar, byte>());
    }

    /// <summary>Builds the value that dispatched the call out of the second of the two values it is handed</summary>
    private readonly struct SecondVisitor : IAlgebraVisitor_T_T_T<SecondVisitor>
    {
        /// <summary>The second of the two scalars</summary>
        static TScalar IAlgebraVisitor_S_S_S<SecondVisitor>.Scalar_Number<TScalar>(TScalar a, TScalar b) => b;

        /// <summary>Builds the second value out of its register of 128 bits</summary>
        static TVector IAlgebraVisitor_T_T_T<SecondVisitor>.Simd_Any<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.FromUnderlying(b.As<TScalar, byte>());

        /// <summary>Builds the second value out of its register of 256 bits</summary>
        static TVector IAlgebraVisitor_T_T_T<SecondVisitor>.Simd_Any<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.FromUnderlying(b.As<TScalar, byte>());
    }

    /// <summary>Builds the value that dispatched the call out of the third of the three values it is handed</summary>
    private readonly struct ThirdVisitor : IAlgebraVisitor_T_T_T_T<ThirdVisitor>
    {
        /// <summary>The third of the three scalars</summary>
        static TScalar IAlgebraVisitor_S_S_S_S<ThirdVisitor>.Scalar_Number<TScalar>(TScalar a, TScalar b, TScalar c) => c;

        /// <summary>Builds the third value out of its register of 128 bits</summary>
        static TVector IAlgebraVisitor_T_T_T_T<ThirdVisitor>.Simd_Any<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b,
            in Vector128<TScalar> c)
            => TVector.FromUnderlying(c.As<TScalar, byte>());

        /// <summary>Builds the third value out of its register of 256 bits</summary>
        static TVector IAlgebraVisitor_T_T_T_T<ThirdVisitor>.Simd_Any<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b,
            in Vector256<TScalar> c)
            => TVector.FromUnderlying(c.As<TScalar, byte>());
    }

    /// <summary>
    /// Tells which member of the visitor the dispatch reached: the register of a vector is replaced by the one
    /// of the vector, every other value reaches the scalar of the visitor for every one of its components and
    /// adds one to it
    /// </summary>
    private readonly struct WhichVisitor : IAlgebraVisitor_T_T<WhichVisitor>
    {
        /// <summary>Adds one to the value, it tells that the scalar of the visitor was reached</summary>
        static TScalar IAlgebraVisitor_S_S<WhichVisitor>.Scalar_Number<TScalar>(TScalar value)
            => value + TScalar.One;

        /// <summary>Replaces the value of a register of 128 bits by the one of the vector</summary>
        static TVector IAlgebraVisitor_T_T<WhichVisitor>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> vector)
            => TVector.One;

        /// <summary>Replaces the value of a register of 256 bits by the one of the vector</summary>
        static TVector IAlgebraVisitor_T_T<WhichVisitor>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> vector)
            => TVector.One;
    }

    /// <summary>
    /// Dispatches the value of a vector or of a matrix to a visitor: the member that dispatches it is
    /// implemented explicitly, so only the interface of the dispatch of a parameter reaches it
    /// </summary>
    private static T Visit<T, V>(in T value)
        where T : unmanaged, IAlgebraDispatch<T>
        where V : IAlgebraVisitor_T_T<V>
        => T.Self<V>(value);

    /// <summary>Dispatches the two values of a vector or of a matrix to a visitor</summary>
    private static T Visit<T, V>(in T a, in T b)
        where T : unmanaged, IAlgebraDispatch<T>
        where V : IAlgebraVisitor_T_T_T<V>
        => T.Self<V>(a, b);

    /// <summary>Dispatches the three values of a vector or of a matrix to a visitor</summary>
    private static T Visit<T, V>(in T a, in T b, in T c)
        where T : unmanaged, IAlgebraDispatch<T>
        where V : IAlgebraVisitor_T_T_T_T<V>
        => T.Self<V>(a, b, c);

    /// <summary>
    /// Counts the members that dispatch the value of the type: the member of an explicit implementation of an
    /// interface is named by the interface and the member of it, so the count of them tells whether the type
    /// implements the dispatch at all and how many members of it the type reaches
    /// </summary>
    private static int CountDispatch<T>()
    {
        var count = 0;
        foreach (var method in typeof(T).GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
        {
            var name = method.Name;
            if (name.EndsWith(".Self", StringComparison.Ordinal)
                || name.EndsWith(".Scalar", StringComparison.Ordinal)
                || name.EndsWith(".Combine", StringComparison.Ordinal)
                || name.EndsWith(".Map_Self", StringComparison.Ordinal)
                || name.EndsWith(".Reduce", StringComparison.Ordinal))
            {
                count++;
            }
        }

        return count;
    }

    [Test]
    public void Value()
    {
        using (Assert.EnterMultipleScope())
        {
            // a vector that keeps its value in a register hands the register over
            Assert.That(Visit<float2, AgainVisitor>(new float2(1, 2)), Is.EqualTo(new float2(1, 2)));
            Assert.That(Visit<float3, AgainVisitor>(new float3(1, 2, 3)), Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(Visit<double4, AgainVisitor>(new double4(1, 2, 3, 4)), Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That(Visit<int2, AgainVisitor>(new int2(1, 2)), Is.EqualTo(new int2(1, 2)));
            // a vector without a register hands the vector itself over, the value of it reaches the scalar of
            // the visitor for every one of its components
            Assert.That(Visit<half2, AgainVisitor>(new half2((Half)1, (Half)2)),
                Is.EqualTo(new half2((Half)1, (Half)2)));
            // a matrix reaches the member of its shape, which hands the value of every column of it over
            Assert.That(Visit<float2x2, AgainVisitor>(new float2x2(new float2(1, 2), new float2(3, 4))),
                Is.EqualTo(new float2x2(new float2(1, 2), new float2(3, 4))));
            // a shape the visitor has no member for hands the value of every column of the matrix over
            Assert.That(Visit<float3x2, AgainVisitor>(new float3x2(new float3(1, 2, 3), new float3(4, 5, 6))),
                Is.EqualTo(new float3x2(new float3(1, 2, 3), new float3(4, 5, 6))));
            Assert.That(Visit<double2x4, AgainVisitor>(
                    new double2x4(new double2(1, 2), new double2(3, 4), new double2(5, 6), new double2(7, 8))),
                Is.EqualTo(new double2x4(new double2(1, 2), new double2(3, 4), new double2(5, 6), new double2(7, 8))));
            // every value of a kind dispatches the members of the kind: three that take values, two that take a
            // component of the value beside it and the map of the value, and a matrix reaches the two members
            // that combine and reduce its columns on top of them
            Assert.That(CountDispatch<float2>(), Is.EqualTo(12));
            Assert.That(CountDispatch<half2>(), Is.EqualTo(12));
            Assert.That(CountDispatch<int2>(), Is.EqualTo(12));
            Assert.That(CountDispatch<float2x2>(), Is.EqualTo(14));
            Assert.That(CountDispatch<float3x2>(), Is.EqualTo(14));
            Assert.That(CountDispatch<half2x2>(), Is.EqualTo(14));
            Assert.That(CountDispatch<double2x4>(), Is.EqualTo(14));
        }
    }

    [Test]
    public void Second()
    {
        using (Assert.EnterMultipleScope())
        {
            // both of the values are handed over the same way, like the members that take a single one
            Assert.That(Visit<float2, SecondVisitor>(new float2(1, 2), new float2(3, 4)),
                Is.EqualTo(new float2(3, 4)));
            Assert.That(Visit<float3, SecondVisitor>(new float3(1, 2, 3), new float3(4, 5, 6)),
                Is.EqualTo(new float3(4, 5, 6)));
            Assert.That(Visit<double4, SecondVisitor>(new double4(1, 2, 3, 4), new double4(5, 6, 7, 8)),
                Is.EqualTo(new double4(5, 6, 7, 8)));
            Assert.That(Visit<half2, SecondVisitor>(new half2((Half)1, (Half)2), new half2((Half)3, (Half)4)),
                Is.EqualTo(new half2((Half)3, (Half)4)));
            // every column of a matrix is handed over the same way
            var a = new float3x2(new float3(1, 2, 3), new float3(4, 5, 6));
            var b = new float3x2(new float3(7, 8, 9), new float3(10, 11, 12));
            Assert.That(Visit<float3x2, SecondVisitor>(a, b), Is.EqualTo(b));
            var c = new float2x2(new float2(1, 2), new float2(3, 4));
            var d = new float2x2(new float2(5, 6), new float2(7, 8));
            Assert.That(Visit<float2x2, SecondVisitor>(c, d), Is.EqualTo(d));
        }
    }

    [Test]
    public void Kind()
    {
        using (Assert.EnterMultipleScope())
        {
            // the width of the register of the vector decides the member: a value that is handed over as a
            // register is replaced by the one of the vector
            Assert.That(Visit<float2, WhichVisitor>(new float2(1, 2)), Is.EqualTo(new float2(1, 1)));
            Assert.That(Visit<float4, WhichVisitor>(new float4(1, 2, 3, 4)), Is.EqualTo(new float4(1, 1, 1, 1)));
            Assert.That(Visit<int3, WhichVisitor>(new int3(4, 5, 6)), Is.EqualTo(new int3(1, 1, 1)));
            // a vector without a register hands the vector itself over, so the member of the scalar is reached
            // for every component of it
            Assert.That(Visit<half2, WhichVisitor>(new half2((Half)1, (Half)2)),
                Is.EqualTo(new half2((Half)2, (Half)3)));
            // a matrix of a number dispatches the value of its columns, every one of them reaching the member
            // of the width of its register
            Assert.That(Visit<float2x2, WhichVisitor>(new float2x2(new float2(1, 2), new float2(3, 4))),
                Is.EqualTo(new float2x2(new float2(1, 1), new float2(1, 1))));
        }
    }

    [Test]
    public void Third()
    {
        using (Assert.EnterMultipleScope())
        {
            // a visitor of three values reaches the member of three values, of the kind of the value it reaches
            Assert.That(Visit<float2, ThirdVisitor>(new float2(1, 2), new float2(3, 4), new float2(5, 6)),
                Is.EqualTo(new float2(5, 6)));
            Assert.That(Visit<float3, ThirdVisitor>(new float3(1, 2, 3), new float3(4, 5, 6), new float3(7, 8, 9)),
                Is.EqualTo(new float3(7, 8, 9)));
            Assert.That(Visit<double4, ThirdVisitor>(new double4(1, 2, 3, 4), new double4(5, 6, 7, 8),
                    new double4(9, 10, 11, 12)),
                Is.EqualTo(new double4(9, 10, 11, 12)));
            Assert.That(Visit<half2, ThirdVisitor>(new half2((Half)1, (Half)2), new half2((Half)3, (Half)4),
                    new half2((Half)5, (Half)6)),
                Is.EqualTo(new half2((Half)5, (Half)6)));
            // every column of a matrix is handed over the same way
            var a = new float3x2(new float3(1, 2, 3), new float3(4, 5, 6));
            var b = new float3x2(new float3(7, 8, 9), new float3(10, 11, 12));
            var c = new float3x2(new float3(13, 14, 15), new float3(16, 17, 18));
            Assert.That(Visit<float3x2, ThirdVisitor>(a, b, c), Is.EqualTo(c));
        }
    }
}
