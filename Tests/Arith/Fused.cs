using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The fused members of the dispatch layer: the multiplication of a floating point value and its addition are
/// fused into one instruction of the hardware, the ones of every other value multiply and add. The register of
/// the value decides the member of the visitor that reaches it, so every width of a register and a value that
/// has none of them is covered. The name of every member is the order of the operands of it, which is the order
/// of the hlsl counterpart as well.
/// </summary>
public class TestFused
{
    /// <summary>
    /// The members of a value are compared against the multiply and the add of the value itself, the extension
    /// members reach the same ones.
    /// </summary>
    private static void Check<T>(T a, T b, T c)
        where T : unmanaged, INumberAlgebraDispatch<T>
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.fma(a, b, c), Is.EqualTo(a * b + c), "fma");
            Assert.That(math.mad(a, b, c), Is.EqualTo(a * b + c), "mad");
            Assert.That(math.fam(c, a, b), Is.EqualTo(c + a * b), "fam");
            Assert.That(math.fms(a, b, c), Is.EqualTo(a * b - c), "fms");
            Assert.That(math.fnma(a, b, c), Is.EqualTo(c - a * b), "fnma");
            Assert.That(math.fsm(c, a, b), Is.EqualTo(c - a * b), "fsm");

            Assert.That(a.fma(b, c), Is.EqualTo(a * b + c), "fma extension");
            Assert.That(c.fam(a, b), Is.EqualTo(c + a * b), "fam extension");
            Assert.That(a.fms(b, c), Is.EqualTo(a * b - c), "fms extension");
            Assert.That(a.fnma(b, c), Is.EqualTo(c - a * b), "fnma extension");
            Assert.That(c.fsm(a, b), Is.EqualTo(c - a * b), "fsm extension");
        }
    }

    [Test]
    public void Values()
    {
        // a value that fills its register, one that has padding lanes and one that has none of them, every
        // width of a register and the values that have no register at all
        Check(new float4(2f, 3f, 4f, 5f), new float4(5f, 6f, 7f, 8f), new float4(10f, 20f, 30f, 40f));
        Check(new float3(2f, 3f, 4f), new float3(5f, 6f, 7f), new float3(10f, 20f, 30f));
        Check(new float2(2f, 3f), new float2(5f, 6f), new float2(10f, 20f));
        Check(new double3(2d, 3d, 4d), new double3(5d, 6d, 7d), new double3(10d, 20d, 30d));
        Check(new double2(2d, 3d), new double2(5d, 6d), new double2(10d, 20d));
        Check(new int3(2, 3, 4), new int3(5, 6, 7), new int3(10, 20, 30));
        Check(new uint3(2u, 3u, 4u), new uint3(5u, 6u, 7u), new uint3(10u, 20u, 30u));
        Check(new half3((half)2f, (half)3f, (half)4f), new half3((half)5f, (half)6f, (half)7f),
            new half3((half)10f, (half)20f, (half)30f));
        // a value that has no register reaches the member of the scalar for every component of it, the one of a
        // floating point component goes through the lowest lane of a register
        Check(new float3s(2f, 3f, 4f), new float3s(5f, 6f, 7f), new float3s(10f, 20f, 30f));
        Check(new double3s(2d, 3d, 4d), new double3s(5d, 6d, 7d), new double3s(10d, 20d, 30d));
    }

    /// <summary>
    /// A matrix reaches the member of its shape, which fuses every component of every one of its columns.
    /// </summary>
    [Test]
    public void Matrix()
    {
        var m = new float3x3(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f), new float3(7f, 8f, 9f));
        var a = math.fma(m, m, m);
        var b = math.fms(m, m, m);
        var c = math.fnma(m, m, m);

        using (Assert.EnterMultipleScope())
        {
            for (var row = 0; row < 3; row++)
            for (var column = 0; column < 3; column++)
            {
                var v = m[row, column];
                Assert.That(a[row, column], Is.EqualTo(v * v + v), $"fma at {row}, {column}");
                Assert.That(b[row, column], Is.EqualTo(v * v - v), $"fms at {row}, {column}");
                Assert.That(c[row, column], Is.EqualTo(v - v * v), $"fnma at {row}, {column}");
            }
        }
    }

    /// <summary>
    /// Builds a nan out of a value that is handed over: the division is not a constant of the compiler, so the
    /// creation of a vector from the value it returns is the one of a value that is only known at run time
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float Nan(float value) => value / value;

    /// <summary>
    /// The fused value of a component is written into the register itself, so the padding lanes of it have to
    /// stay zero: the padding lanes of every operand are zero, so the value of them is zero on both sides.
    /// </summary>
    [Test]
    public void PaddingStaysZero()
    {
        var nan = Nan(0f);
        var a = new float3(nan);
        var b = new float3(2f);
        var c = new float3(3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float.IsNaN(a.x), Is.True, "the computed value is a nan");
            Assert.That(math.fma(a, b, c).vector.GetElement(3), Is.EqualTo(0f), "fma padding");
            Assert.That(math.fms(a, b, c).vector.GetElement(3), Is.EqualTo(0f), "fms padding");
            Assert.That(math.fnma(a, b, c).vector.GetElement(3), Is.EqualTo(0f), "fnma padding");
            Assert.That(math.fam(c, a, b).vector.GetElement(3), Is.EqualTo(0f), "fam padding");
            Assert.That(math.fsm(c, a, b).vector.GetElement(3), Is.EqualTo(0f), "fsm padding");
            Assert.That(math.mad(a, b, c).vector.GetElement(3), Is.EqualTo(0f), "mad padding");
        }
    }
}
