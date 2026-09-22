using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The kind of a vector is only known from its type: the width of its register and the count of its components
/// are a part of the type, so the type itself dispatches a call to the member of a visitor that matches them
/// and hands the value of it over to that member. A visitor that builds a vector itself returns a value of the
/// type of the vector that dispatched the call.
/// </summary>
public class TestDynamicVector
{
    /// <summary>Counts the lanes of the register of the vector that dispatches to it</summary>
    private readonly struct LaneVisitor : IVectorUnderlyingVisitor<int>
    {
        /// <summary>A vector without a register has no lane at all</summary>
        public static int AcceptSoft<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>, IVectorSoftUnderlying
            where TScalar : unmanaged => 0;

        /// <summary>The lanes of a register of 64 bits</summary>
        public static int Accept<TVector, TScalar>(in Vector64<TScalar> vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>,
            IVector64Underlying<TVector>
            where TScalar : unmanaged => 64 / (8 * Unsafe.SizeOf<TScalar>());

        /// <summary>The lanes of a register of 128 bits</summary>
        public static int Accept<TVector, TScalar>(in Vector128<TScalar> vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>,
            IVector128Underlying<TVector>
            where TScalar : unmanaged => 128 / (8 * Unsafe.SizeOf<TScalar>());

        /// <summary>The lanes of a register of 256 bits</summary>
        public static int Accept<TVector, TScalar>(in Vector256<TScalar> vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>,
            IVector256Underlying<TVector>
            where TScalar : unmanaged => 256 / (8 * Unsafe.SizeOf<TScalar>());
    }

    /// <summary>Reads the bits of the first component of the value the vector hands over</summary>
    private readonly struct FirstVisitor : IVectorDimensionVisitor<uint>
    {
        /// <summary>Reads the first component of a vector of 2 components</summary>
        public static uint AcceptVector2<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>,
            IVector2<TVector, TScalar>
            where TScalar : unmanaged
        {
            var x = TVector.get_x(vector);
            return Unsafe.As<TScalar, uint>(ref x);
        }

        /// <summary>Reads the first component of a vector of 3 components</summary>
        public static uint AcceptVector3<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>,
            IVector3<TVector, TScalar>
            where TScalar : unmanaged
        {
            var x = TVector.get_x(vector);
            return Unsafe.As<TScalar, uint>(ref x);
        }

        /// <summary>Reads the first component of a vector of 4 components</summary>
        public static uint AcceptVector4<TVector, TScalar>(in TVector vector)
            where TVector : unmanaged, IDynamicVector<TVector>, INumberVector<TVector, TScalar>,
            IVector4<TVector, TScalar>
            where TScalar : unmanaged
        {
            var x = TVector.get_x(vector);
            return Unsafe.As<TScalar, uint>(ref x);
        }
    }

    /// <summary>Builds the vector that dispatched the call out of the value it is handed</summary>
    private readonly struct AgainVisitor : IVectorUnderlyingVisitorReturnVector
    {
        /// <summary>Builds a vector without a register, its value is handed over as it is</summary>
        static TVector IVectorUnderlyingVisitorReturnVector.AcceptSoft<TVector, TScalar>(in TVector vector) => vector;

        /// <summary>Builds a vector out of the register of 64 bits that keeps its value</summary>
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector64<TScalar> vector) => TVector.FromUnderlying(vector.As<TScalar, byte>());

        /// <summary>Builds a vector out of the register of 128 bits that keeps its value</summary>
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector128<TScalar> vector) => TVector.FromUnderlying(vector.As<TScalar, byte>());

        /// <summary>Builds a vector out of the register of 256 bits that keeps its value</summary>
        static TVector IVectorUnderlyingVisitorReturnVector.Accept<TVector, TScalar>(in Vector256<TScalar> vector) => TVector.FromUnderlying(vector.As<TScalar, byte>());
    }

    /// <summary>Returns the value it is handed, which tells which member the dispatch reached</summary>
    private readonly struct SameVisitor : IVectorDimensionVisitorReturnVector
    {
        /// <summary>Returns the value of a vector of 2 components</summary>
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector2<TVector, TScalar>(in TVector vector) => vector;

        /// <summary>Returns the value of a vector of 3 components</summary>
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector3<TVector, TScalar>(in TVector vector) => vector;

        /// <summary>Returns the value of a vector of 4 components</summary>
        static TVector IVectorDimensionVisitorReturnVector.AcceptVector4<TVector, TScalar>(in TVector vector) => vector;
    }

    /// <summary>Builds the vector that dispatched the call out of the second of the two values it is handed</summary>
    private readonly struct SecondVisitor2 : IVectorUnderlyingVisitor2ReturnVector
    {
        /// <summary>Builds the second value of a vector that has no register</summary>
        static TVector IVectorUnderlyingVisitor2ReturnVector.AcceptSoft<TVector, TScalar>(in TVector a, in TVector b) => b;

        /// <summary>Builds the second value out of its register of 64 bits</summary>
        static TVector IVectorUnderlyingVisitor2ReturnVector.Accept<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b) => TVector.FromUnderlying(b.As<TScalar, byte>());

        /// <summary>Builds the second value out of its register of 128 bits</summary>
        static TVector IVectorUnderlyingVisitor2ReturnVector.Accept<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b) => TVector.FromUnderlying(b.As<TScalar, byte>());

        /// <summary>Builds the second value out of its register of 256 bits</summary>
        static TVector IVectorUnderlyingVisitor2ReturnVector.Accept<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b) => TVector.FromUnderlying(b.As<TScalar, byte>());
    }

    /// <summary>Returns the first of the two values it is handed</summary>
    private readonly struct FirstVisitor2 : IVectorDimensionVisitor2ReturnVector
    {
        /// <summary>Returns the first value of a vector of 2 components</summary>
        static TVector IVectorDimensionVisitor2ReturnVector.AcceptVector2<TVector, TScalar>(in TVector a, in TVector b) => a;

        /// <summary>Returns the first value of a vector of 3 components</summary>
        static TVector IVectorDimensionVisitor2ReturnVector.AcceptVector3<TVector, TScalar>(in TVector a, in TVector b) => a;

        /// <summary>Returns the first value of a vector of 4 components</summary>
        static TVector IVectorDimensionVisitor2ReturnVector.AcceptVector4<TVector, TScalar>(in TVector a, in TVector b) => a;
    }

    [Test]
    public void Underlying()
    {
        using (Assert.EnterMultipleScope())
        {
            // the type of the vector dispatches the width of the register it keeps its value in
            Assert.That(float2.VisitUnderlying<LaneVisitor, int>(new float2(1, 2)), Is.EqualTo(4));
            Assert.That(float3.VisitUnderlying<LaneVisitor, int>(new float3(1, 2, 3)), Is.EqualTo(4));
            Assert.That(double3.VisitUnderlying<LaneVisitor, int>(new double3(1, 2, 3)), Is.EqualTo(4));
            Assert.That(int2s.VisitUnderlying<LaneVisitor, int>(new int2s(1, 2)), Is.EqualTo(2));
            // every member of a visitor names the vector interface of a number, so a mask does not dispatch
            Assert.That(typeof(b32v2).GetMethod("VisitUnderlying"), Is.Null);
            Assert.That(typeof(float2).GetMethod("VisitUnderlying"), Is.Not.Null);
            // a vector without a register reaches the member that only names the vector itself
            Assert.That(half2.VisitUnderlying<LaneVisitor, int>(new half2((Half)1, (Half)2)), Is.EqualTo(0));
            Assert.That(float3s.VisitUnderlying<LaneVisitor, int>(new float3s(1, 2, 3)), Is.EqualTo(0));
        }
    }

    [Test]
    public void Dimension()
    {
        using (Assert.EnterMultipleScope())
        {
            // the type of the vector dispatches the count of its components and hands its value over
            Assert.That(float2.VisitDimension<FirstVisitor, uint>(new float2(1, 2)), Is.EqualTo(0x3F800000u));
            Assert.That(float2s.VisitDimension<FirstVisitor, uint>(new float2s(1, 2)), Is.EqualTo(0x3F800000u));
            Assert.That(float3.VisitDimension<FirstVisitor, uint>(new float3(1, 2, 3)), Is.EqualTo(0x3F800000u));
            Assert.That(float3s.VisitDimension<FirstVisitor, uint>(new float3s(1, 2, 3)), Is.EqualTo(0x3F800000u));
            Assert.That(int4.VisitDimension<FirstVisitor, uint>(new int4(7, 8, 9, 10)), Is.EqualTo(7u));
        }
    }

    [Test]
    public void ReturnVector()
    {
        using (Assert.EnterMultipleScope())
        {
            // the member of the visitor rebuilds the vector out of the register that is handed to it
            Assert.That(float2.VisitUnderlyingReturnVector<AgainVisitor>(new float2(1, 2)), Is.EqualTo(new float2(1, 2)));
            Assert.That(float3.VisitUnderlyingReturnVector<AgainVisitor>(new float3(1, 2, 3)),
                Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(double4.VisitUnderlyingReturnVector<AgainVisitor>(new double4(1, 2, 3, 4)),
                Is.EqualTo(new double4(1, 2, 3, 4)));
            Assert.That(int2s.VisitUnderlyingReturnVector<AgainVisitor>(new int2s(1, 2)),
                Is.EqualTo(new int2s(1, 2)));
            // a vector without a register hands its value over as it is
            Assert.That(half2.VisitUnderlyingReturnVector<AgainVisitor>(new half2((Half)1, (Half)2)),
                Is.EqualTo(new half2((Half)1, (Half)2)));
            Assert.That(float3s.VisitUnderlyingReturnVector<AgainVisitor>(new float3s(1, 2, 3)),
                Is.EqualTo(new float3s(1, 2, 3)));
            // the count of the components is dispatched as well, the value is handed over the same way
            Assert.That(float2.VisitDimensionReturnVector<SameVisitor>(new float2(1, 2)), Is.EqualTo(new float2(1, 2)));
            Assert.That(float3.VisitDimensionReturnVector<SameVisitor>(new float3(1, 2, 3)),
                Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(int4.VisitDimensionReturnVector<SameVisitor>(new int4(7, 8, 9, 10)),
                Is.EqualTo(new int4(7, 8, 9, 10)));
            Assert.That(double3.VisitDimensionReturnVector<SameVisitor>(new double3(1, 2, 3)),
                Is.EqualTo(new double3(1, 2, 3)));
            // the members that take two vectors hand both of them over, the same way as the ones that take one
            Assert.That(float2.VisitUnderlyingReturnVector<SecondVisitor2>(new float2(1, 2), new float2(3, 4)),
                Is.EqualTo(new float2(3, 4)));
            Assert.That(float3.VisitUnderlyingReturnVector<SecondVisitor2>(new float3(1, 2, 3), new float3(4, 5, 6)),
                Is.EqualTo(new float3(4, 5, 6)));
            Assert.That(double4.VisitUnderlyingReturnVector<SecondVisitor2>(new double4(1, 2, 3, 4),
                new double4(5, 6, 7, 8)), Is.EqualTo(new double4(5, 6, 7, 8)));
            Assert.That(int2s.VisitUnderlyingReturnVector<SecondVisitor2>(new int2s(1, 2), new int2s(3, 4)),
                Is.EqualTo(new int2s(3, 4)));
            Assert.That(half2.VisitUnderlyingReturnVector<SecondVisitor2>(new half2((Half)1, (Half)2),
                new half2((Half)3, (Half)4)), Is.EqualTo(new half2((Half)3, (Half)4)));
            Assert.That(float3s.VisitUnderlyingReturnVector<SecondVisitor2>(new float3s(1, 2, 3), new float3s(4, 5, 6)),
                Is.EqualTo(new float3s(4, 5, 6)));
            Assert.That(float2.VisitDimensionReturnVector<FirstVisitor2>(new float2(1, 2), new float2(3, 4)),
                Is.EqualTo(new float2(1, 2)));
            Assert.That(float3.VisitDimensionReturnVector<FirstVisitor2>(new float3(1, 2, 3), new float3(4, 5, 6)),
                Is.EqualTo(new float3(1, 2, 3)));
            Assert.That(int4.VisitDimensionReturnVector<FirstVisitor2>(new int4(1, 2, 3, 4), new int4(5, 6, 7, 8)),
                Is.EqualTo(new int4(1, 2, 3, 4)));
        }
    }
}
