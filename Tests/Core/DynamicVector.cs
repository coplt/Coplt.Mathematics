using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The kind of a vector is only known from its type: the width of its register and the count of its components
/// are a part of the type, so the type itself dispatches a call to the member of a visitor that matches them
/// and hands the value of it over to that member.
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
}
