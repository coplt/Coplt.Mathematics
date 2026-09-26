using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The floating point members of a vector implement <c>IVectorFloatingPoint</c>: the constants of the component
/// type, the projections and the two sided wrap. The members of a simd vector keep the padding lanes of it at
/// zero.
/// </summary>
public class TestFloatingPoint
{
    /// <summary>
    /// Every floating point vector implements the interface, so every member below is reachable through it and
    /// the constants of the component type are a vector of it.
    /// </summary>
    private static void Check<T, TScalar>(T v)
        where T : unmanaged, IVectorFloatingPoint<T, TScalar>
        where TScalar : unmanaged
    {
        v.project(v);
        v.project_on_plane(v);
        v.project_normalized(v);
        v.project_on_plane_normalized(v);
        v.wrap(v, v);
        v.wrap(default(TScalar), default(TScalar));

        _ = T.E;
        _ = T.Log2;
        _ = T.Log10;
        _ = T.PI;
        _ = T.Tau;
        _ = T.RadToDeg;
        _ = T.DegToRad;
    }

    [Test]
    public void Interface()
    {
        // the type of a single component cannot be inferred from the vector, it has to be spelled out
        Check<float2, float>(new float2(1, 2));
        Check<float3, float>(new float3(1, 2, 3));
        Check<float4, float>(new float4(1, 2, 3, 4));
        Check<double2, double>(new double2(1, 2));
        Check<double3, double>(new double3(1, 2, 3));
        Check<half2, half>(default);
        Check<half4, half>(default);
        Check<float2s, float>(default);
        Check<float3s, float>(default);
        Check<double3s, double>(default);
    }

    [Test]
    public void Constants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.E.x, Is.EqualTo(MathF.E));
            // the constant of the natural logarithm of two and of ten
            Assert.That(float3.Log2.x, Is.EqualTo(MathF.Log(2f)).Within(1e-7f));
            Assert.That(float3.Log10.x, Is.EqualTo(MathF.Log(10f)).Within(1e-7f));
            Assert.That(float3.PI.x, Is.EqualTo(MathF.PI));
            Assert.That(float3.Tau.x, Is.EqualTo(MathF.Tau));
            Assert.That(float3.RadToDeg.x, Is.EqualTo(180f / MathF.PI).Within(1e-4f));
            Assert.That(float3.DegToRad.x, Is.EqualTo(MathF.PI / 180f).Within(1e-7f));

            Assert.That(double3.PI.x, Is.EqualTo(Math.PI));
            Assert.That(double3.Tau.x, Is.EqualTo(Math.Tau));
            Assert.That(double3.Log2.x, Is.EqualTo(Math.Log(2)).Within(1e-15));
            Assert.That((float)half3.PI.x, Is.EqualTo(MathF.PI).Within(0.002f));

            // the constant is a broadcast of the value
            Assert.That(float3.PI, Is.EqualTo(new float3(float.Pi)));
        }
    }

    /// <summary>
    /// The constants of the generated vectors have to keep the values of the legacy library, the digits of a
    /// literal are the digits of the legacy constant of the component type and the value of a half is the cast of
    /// the float literal of it.
    /// </summary>
    [Test]
    public void ConstantsMatchTheLegacyConstants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.E.x, Is.EqualTo(Coplt.Mathematics.math.F_E));
            Assert.That(float3.Log2.x, Is.EqualTo(Coplt.Mathematics.math.F_Log2));
            Assert.That(float3.Log10.x, Is.EqualTo(Coplt.Mathematics.math.F_Log10));
            Assert.That(float3.PI.x, Is.EqualTo(Coplt.Mathematics.math.F_PI));
            Assert.That(float3.Tau.x, Is.EqualTo(Coplt.Mathematics.math.F_Tau));
            Assert.That(float3.RadToDeg.x, Is.EqualTo(Coplt.Mathematics.math.F_RadToDeg));
            Assert.That(float3.DegToRad.x, Is.EqualTo(Coplt.Mathematics.math.F_DegToRad));

            Assert.That(double3.E.x, Is.EqualTo(Coplt.Mathematics.math.D_E));
            Assert.That(double3.Log2.x, Is.EqualTo(Coplt.Mathematics.math.D_Log2));
            Assert.That(double3.Log10.x, Is.EqualTo(Coplt.Mathematics.math.D_Log10));
            Assert.That(double3.PI.x, Is.EqualTo(Coplt.Mathematics.math.D_PI));
            Assert.That(double3.Tau.x, Is.EqualTo(Coplt.Mathematics.math.D_Tau));
            Assert.That(double3.RadToDeg.x, Is.EqualTo(Coplt.Mathematics.math.D_RadToDeg));
            Assert.That(double3.DegToRad.x, Is.EqualTo(Coplt.Mathematics.math.D_DegToRad));

            Assert.That((float)half3.E.x, Is.EqualTo((float)(half)Coplt.Mathematics.math.F_E));
            Assert.That((float)half3.PI.x, Is.EqualTo((float)(half)Coplt.Mathematics.math.F_PI));
        }
    }

    [Test]
    public void Project()
    {
        var a = new float3(1f, 2f, 3f);
        var onto = new float3(2f, 0f, 0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(a.project(onto), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(a.project_normalized(new float3(1f, 0f, 0f)), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(a.project_on_plane(onto), Is.EqualTo(new float3(0f, 2f, 3f)));
            Assert.That(a.project_on_plane_normalized(new float3(1f, 0f, 0f)), Is.EqualTo(new float3(0f, 2f, 3f)));
            // the projection of a vector onto itself is the vector
            Assert.That(a.project(a), Is.EqualTo(a));
            Assert.That(a.project_on_plane(a), Is.EqualTo(default(float3)));
        }
    }

    [Test]
    public void Wrap()
    {
        var v = new float3(2.5f, -1.5f, 0.25f);

        using (Assert.EnterMultipleScope())
        {
            // a value that is not negative is wrapped above the lower bound, a negative one below the upper one
            Assert.That(v.wrap(default, new float3(1f)), Is.EqualTo(new float3(0.5f, 0.5f, 0.25f)));
            Assert.That(v.wrap(0f, 1f), Is.EqualTo(new float3(0.5f, 0.5f, 0.25f)));
            Assert.That(new float3(2.5f, -0.5f, 0f).wrap(0f, 2f), Is.EqualTo(new float3(0.5f, 1.5f, 0f)));
            Assert.That(new double2(2.5, -0.5).wrap(0d, 2d), Is.EqualTo(new double2(0.5, 1.5)));
        }

        var h = new half2((half)2.5f, (half)(-0.5f));

        using (Assert.EnterMultipleScope())
        {
            Assert.That((float)h.wrap((half)0f, (half)2f).x, Is.EqualTo(0.5f));
            Assert.That((float)h.wrap((half)0f, (half)2f).y, Is.EqualTo(1.5f));
        }
    }

    [Test]
    public void StorageVariant()
    {
        var v = new float3s(1.4f, -1.5f, 2.6f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.ceil(), Is.EqualTo(new float3s(2f, -1f, 3f)));
            Assert.That(v.trunc(), Is.EqualTo(new float3s(1f, -1f, 2f)));
            Assert.That(v.frac().y, Is.EqualTo(0.5f));
            Assert.That(math.fmod(v, new float3s(1f, 1f, 1f)).z, Is.EqualTo(0.6f).Within(1e-6f));
            Assert.That(v.wrap(0f, 1f).x, Is.EqualTo(0.4f).Within(1e-6f));
        }

        var n = new float2s(1.4f, 2.6f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.ceil(), Is.EqualTo(new float2s(2f, 3f)));
            Assert.That(n.floor(), Is.EqualTo(new float2s(1f, 2f)));
            // the fraction of a value keeps the rounding of the value, it is not exact
            Assert.That(n.frac().x, Is.EqualTo(0.4f).Within(1e-6f));
            Assert.That(n.frac().y, Is.EqualTo(0.6f).Within(1e-6f));
        }

        var d = new double3s(1.4, -1.5, 2.6);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(d.ceil(), Is.EqualTo(new double3s(2d, -1d, 3d)));
            Assert.That(d.trunc(), Is.EqualTo(new double3s(1d, -1d, 2d)));
        }
    }

    /// <summary>
    /// The members that read the zero padding lane of a simd register leave something else than zero there, the
    /// result of the member has to be masked to keep the padding of the vector.
    /// </summary>
    [Test]
    public void PaddingStaysZero()
    {
        var v = new float3(1.4f, -1.5f, 2.6f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.ceil().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.floor().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.round().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.trunc().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.frac().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.fmod(v, v).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.rcp().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.saturate().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.wrap(v, v).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.radians().vector.GetElement(3), Is.EqualTo(0f));
        }

        var n = new float2(1.4f, 2.6f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.ceil().vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(n.ceil().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(math.fmod(n, n).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(n.rcp().vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(n.rcp().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(n.wrap(n, n).vector.GetElement(2), Is.EqualTo(0f));
        }
    }
}
