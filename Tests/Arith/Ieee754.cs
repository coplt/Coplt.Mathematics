using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Experimental.Mathematics;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The ieee 754 members of a vector implement <c>IVectorFloatingPointIeee754</c> and
/// <c>IVectorFloatingPointIeee754BoolOps</c>: the logarithms, the exponentials, the power, the square root and
/// its reciprocal, the length and the distance, the normalization, the step and the refraction, the safe
/// projection, the face forward, the trigonometry, the hyperbolics, the change of the sign and the checks of the
/// special floating point values that produce a bool vector of the same shape. The members of a simd vector keep
/// the padding lanes of it at zero, the members of a vector without a register work on the components.
/// </summary>
public class TestIeee754
{
    /// <summary>
    /// Every ieee 754 vector implements the interface, so every member below is reachable through it. The mask of
    /// the checks is the bool vector of the same shape and the refraction is a static member of the interface.
    /// </summary>
    private static void Check<T, TScalar, TBool>(T v)
        where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TScalar, TBool>
        where TScalar : unmanaged
        where TBool : unmanaged
    {
        v.log();
        v.log2();
        v.log(v);
        v.log10();
        v.exp();
        v.exp2();
        v.exp10();
        v.pow(default);
        v.pow(v);
        v.sqrt();
        v.rsqrt();
        _ = T.length(v);
        _ = T.distance(v, v);
        v.normalize();
        v.normalize_safe();
        v.step(v);
        T.refract(v, v, default);
        v.project_safe(v);
        v.project_safe(v, v);
        v.face_forward(v, v);
        v.sin();
        v.cos();
        var (sin, cos) = v.sincos();
        v.sincos(out var s, out var c);
        v.tan();
        v.asin();
        v.acos();
        v.atan();
        v.atan2(v);
        v.sinh();
        v.cosh();
        v.tanh();
        v.asinh();
        v.acosh();
        v.atanh();
        v.chg_sign(v);
        T.is_NaN(v);
        T.is_finite(v);
        T.is_inf(v);
        T.is_pos_inf(v);
        T.is_neg_inf(v);

        using (Assert.EnterMultipleScope())
        {
            // the single and the pair form of the sine and the cosine agree and the mask of a vector is the one
            // of its own shape, so it is equal to itself
            Assert.That(v.sin().Equals(sin), Is.True);
            Assert.That(v.cos().Equals(cos), Is.True);
            Assert.That(sin.Equals(s), Is.True);
            Assert.That(cos.Equals(c), Is.True);
            Assert.That(T.is_NaN(v).Equals(T.is_NaN(v)), Is.True);
            Assert.That(Unsafe.SizeOf<TBool>(), Is.GreaterThan(0));
        }
    }

    [Test]
    public void Interface()
    {
        // the type of a single component and the type of the mask cannot be inferred from the vector, they have
        // to be spelled out
        Check<float2, float, b32v2>(new float2(1, 2));
        Check<float3, float, b32v3>(new float3(1, 2, 3));
        Check<float4, float, b32v4>(new float4(1, 2, 3, 4));
        Check<float2s, float, b32v2>(new float2s(1, 2));
        Check<float3s, float, b32v3>(new float3s(1, 2, 3));
        Check<double2, double, b64v2>(new double2(1, 2));
        Check<double3, double, b64v3>(new double3(1, 2, 3));
        Check<double4, double, b64v4>(new double4(1, 2, 3, 4));
        Check<double3s, double, b64v3>(new double3s(1, 2, 3));
        Check<half2, half, b16v2>(new half2((half)1f, (half)2f));
        Check<half3, half, b16v3>(new half3((half)1f, (half)2f, (half)3f));
        Check<half4, half, b16v4>(new half4((half)1f, (half)2f, (half)3f, (half)4f));
    }

    /// <summary>
    /// The checks of the special values produce a mask of the shape of the vector.
    /// </summary>
    [Test]
    public void SpecialValues()
    {
        var v = new float3(1f, float.NaN, float.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)v.is_NaN().x, Is.False);
            Assert.That((bool)v.is_NaN().y, Is.True);
            Assert.That((bool)v.is_NaN().z, Is.False);

            Assert.That((bool)v.is_finite().x, Is.True);
            Assert.That((bool)v.is_finite().y, Is.False);
            Assert.That((bool)v.is_finite().z, Is.False);

            Assert.That((bool)v.is_inf().x, Is.False);
            Assert.That((bool)v.is_inf().z, Is.True);

            Assert.That((bool)v.is_pos_inf().z, Is.True);
            Assert.That((bool)v.is_neg_inf().z, Is.False);
        }

        var n = new float3(float.NegativeInfinity, 0f, -0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)n.is_neg_inf().x, Is.True);
            Assert.That((bool)n.is_pos_inf().x, Is.False);
            Assert.That((bool)n.is_inf().x, Is.True);
            // a zero is finite, the sign of it does not matter
            Assert.That((bool)n.is_finite().y, Is.True);
            Assert.That((bool)n.is_finite().z, Is.True);
        }

        var s = new float2s(float.NaN, float.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)s.is_NaN().x, Is.True);
            Assert.That((bool)s.is_inf().y, Is.True);
            Assert.That((bool)s.is_finite().x, Is.False);
            Assert.That((bool)s.is_finite().y, Is.False);
        }

        // the mask of a bool of 8 byte components is a 64 bit one
        var d = new double3(1, double.PositiveInfinity, double.NegativeInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)d.is_inf().x, Is.False);
            Assert.That((bool)d.is_pos_inf().y, Is.True);
            Assert.That((bool)d.is_neg_inf().z, Is.True);
        }

        var h = new half3((half)1f, half.NaN, half.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)h.is_NaN().y, Is.True);
            Assert.That((bool)h.is_inf().z, Is.True);
            Assert.That((bool)h.is_finite().x, Is.True);
        }
    }

    [Test]
    public void Log()
    {
        var v = new float3(1f, 2f, 4f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.log().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(v.log().y, Is.EqualTo(MathF.Log(2f)).Within(1e-5f));
            Assert.That(v.log2().z, Is.EqualTo(2f).Within(1e-5f));
            Assert.That(v.log10().z, Is.EqualTo(MathF.Log10(4f)).Within(1e-5f));
            // the logarithm of any base is the quotient of the two logarithms
            Assert.That(v.log(v).y, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(new float3(1f, 8f, 1024f).log(new float3(2f)).z, Is.EqualTo(10f).Within(1e-4f));
            Assert.That(new double3(1, Math.E, 100).log10().y, Is.EqualTo(Math.Log10(Math.E)).Within(1e-12));
        }
    }

    [Test]
    public void Exp()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f).exp().x, Is.EqualTo(1f).Within(1e-6f));
            Assert.That(new float3(1f).exp().x, Is.EqualTo(MathF.E).Within(1e-5f));
            Assert.That(new float3(10f).exp2().x, Is.EqualTo(1024f).Within(1e-3f));
            Assert.That(new float3(3f).exp10().x, Is.EqualTo(1000f).Within(1e-2f));
            Assert.That(new float4(0f, 1f, 2f, 3f).exp2().w, Is.EqualTo(8f).Within(1e-4f));
            Assert.That(new double2(2, 3).exp2().x, Is.EqualTo(4d).Within(1e-9));
            Assert.That(new double2(2, 3).exp2().y, Is.EqualTo(8d).Within(1e-9));
            // the exponential and the logarithm are the inverse of each other
            Assert.That(new float3(5f).log().exp().x, Is.EqualTo(5f).Within(1e-4f));
        }
    }

    [Test]
    public void PowSqrtRSqrt()
    {
        var v = new float3(4f, 9f, 16f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.sqrt().x, Is.EqualTo(2f).Within(1e-6f));
            Assert.That(v.sqrt().y, Is.EqualTo(3f).Within(1e-6f));
            Assert.That(v.sqrt().z, Is.EqualTo(4f).Within(1e-6f));
            // the reciprocal of the square root is the reciprocal of the square root, it is an estimate
            Assert.That(v.rsqrt().x, Is.EqualTo(0.5f).Within(1e-3f));
            Assert.That(v.rsqrt().y, Is.EqualTo(1f / 3f).Within(1e-3f));
            Assert.That(new float3(2f).pow(10f).x, Is.EqualTo(1024f).Within(1e-2f));
            Assert.That(new float3(2f).pow(0.5f).x, Is.EqualTo(MathF.Sqrt(2f)).Within(1e-5f));
            // the exponent can be a vector as well
            Assert.That(new float3(2f).pow(new float3(3f)).x, Is.EqualTo(8f).Within(1e-3f));
            Assert.That(new double3(2, 3, 4).pow(new double3(2, 2, 2)).z, Is.EqualTo(16d).Within(1e-9));
            Assert.That(new float2s(3f, 4f).pow(new float2s(2f, 2f)), Is.EqualTo(new float2s(9f, 16f)));
            Assert.That(new double3(9, 16, 25).sqrt().z, Is.EqualTo(5d).Within(1e-12));
        }

        // the element wise power of a value that has a reference
        var nv = new float3(2f, 3f, 4f).pow(new float3(0.5f, 2f, 3f));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(nv.x, Is.EqualTo(1.4142135f).Within(1e-6f));
            Assert.That(nv.y, Is.EqualTo(9f).Within(1e-6f));
            Assert.That(nv.z, Is.EqualTo(64f).Within(1e-6f));
        }
    }

    [Test]
    public void LengthDistanceNormalize()
    {
        var v = new float3(3f, 4f, 0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.length(), Is.EqualTo(5f).Within(1e-5f));
            Assert.That(v.distance(new float3(3f, 0f, 0f)), Is.EqualTo(4f).Within(1e-5f));
            Assert.That(v.normalize().x, Is.EqualTo(0.6f).Within(1e-5f));
            Assert.That(v.normalize().y, Is.EqualTo(0.8f).Within(1e-5f));
            Assert.That(v.normalize().z, Is.EqualTo(0f).Within(1e-6f));
            // the safe normalization of a vector that has no direction is the zero vector
            Assert.That(new float3(0f).normalize_safe(), Is.EqualTo(default(float3)));
            Assert.That(new double3(0, 0, 0).normalize_safe(), Is.EqualTo(default(double3)));
            Assert.That(new float3(0f).normalize().x, Is.EqualTo(float.NaN));
            Assert.That(new half2((half)3f, (half)4f).length(), Is.EqualTo((half)5f));
        }
    }

    [Test]
    public void Step()
    {
        var v = new float3(0f, 1f, 2f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.step(new float3(1f)), Is.EqualTo(new float3(0f, 1f, 1f)));
            Assert.That(new float3(1f, 0f, 0f).step(new float3(1f)), Is.EqualTo(new float3(1f, 0f, 0f)));
            Assert.That(new double3(0, 2, 0).step(new double3(1)), Is.EqualTo(new double3(0, 1, 0)));
            Assert.That(new float2s(0f, 2f).step(new float2s(1f)), Is.EqualTo(new float2s(0f, 1f)));
        }
    }

    /// <summary>
    /// The refraction is zero when the index of refraction does not allow a direction to leave the surface and
    /// the incident direction is kept when both materials are the same.
    /// </summary>
    [Test]
    public void Refract()
    {
        var i = new float3(0f, 0f, -1f);
        var n = new float3(0f, 0f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float3.refract(i, n, 1f).z, Is.EqualTo(-1f).Within(1e-5f));
            // a grazing direction cannot leave a denser medium
            var g = new float3(0f, -0.5f, -0.8660254f);
            Assert.That(float3.refract(g, new float3(0f, 1f, 0f), 1.5f), Is.EqualTo(default(float3)));
            // both sides are the same material, the normal points the same way as the incident direction, so the
            // normal component of the direction is flipped and the other two are kept
            var d = float3.normalize(new float3(0.3f, -0.4f, 0.5f));
            var r = float3.refract(d, new float3(0f, 0f, 1f), 1f);
            Assert.That(r.x, Is.EqualTo(d.x).Within(1e-5f));
            Assert.That(r.y, Is.EqualTo(d.y).Within(1e-5f));
            Assert.That(r.z, Is.EqualTo(-d.z).Within(1e-5f));
        }
    }

    [Test]
    public void ProjectSafe()
    {
        var v = new float3(1f, 2f, 3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.project_safe(new float3(2f, 0f, 0f)), Is.EqualTo(new float3(1f, 0f, 0f)));
            // the projection of a zero vector is not finite, so the default is returned
            Assert.That(v.project_safe(default), Is.EqualTo(default(float3)));
            Assert.That(v.project_safe(default, v), Is.EqualTo(v));
            Assert.That(new double3(1, 2, 3).project_safe(new double3(1, 1, 1)),
                Is.EqualTo(new double3(2, 2, 2)));
        }
    }

    /// <summary>
    /// The sign of the vector is flipped when the dot product of the normal and the incident vector is not
    /// negative, so the result faces away from the incident vector.
    /// </summary>
    [Test]
    public void FaceForward()
    {
        var v = new float3(1f, 2f, 3f);
        var ng = new float3(0f, 0f, 1f);

        using (Assert.EnterMultipleScope())
        {
            // the incident vector points into the direction of the normal
            Assert.That(v.face_forward(new float3(0f, 0f, 1f), ng), Is.EqualTo(new float3(-1f, -2f, -3f)));
            // the incident vector points against the normal
            Assert.That(v.face_forward(new float3(0f, 0f, -1f), ng), Is.EqualTo(v));
            Assert.That(new double2(1, 2).face_forward(new double2(1, 0), new double2(1, 0)),
                Is.EqualTo(new double2(-1, -2)));
        }
    }

    [Test]
    public void SinCosTan()
    {
        var v = new float3(0f, MathF.PI / 2f, MathF.PI);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.sin().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(v.sin().y, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(v.sin().z, Is.EqualTo(0f).Within(1e-5f));
            Assert.That(v.cos().x, Is.EqualTo(1f).Within(1e-6f));
            Assert.That(v.cos().y, Is.EqualTo(0f).Within(1e-5f));
            Assert.That(v.cos().z, Is.EqualTo(-1f).Within(1e-5f));
            Assert.That(new float3(MathF.PI / 4f).tan().x, Is.EqualTo(1f).Within(1e-5f));
        }

        var (sin, cos) = v.sincos();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(sin.y, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(cos.z, Is.EqualTo(-1f).Within(1e-5f));
        }

        v.sincos(out var s, out var c);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(s, Is.EqualTo(sin));
            Assert.That(c, Is.EqualTo(cos));
        }

        // the angle conversions of the floating point members are the input of the trigonometry
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(90f).radians().sin().x, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(new double3(90, 0, 45).radians().sin().x, Is.EqualTo(1d).Within(1e-12));
        }
    }

    [Test]
    public void InverseTrigonometry()
    {
        var v = new float3(0f, 0.5f, 1f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.asin().x, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(v.asin().y, Is.EqualTo(MathF.PI / 6f).Within(1e-5f));
            Assert.That(v.asin().z, Is.EqualTo(MathF.PI / 2f).Within(1e-5f));
            Assert.That(v.acos().y, Is.EqualTo(MathF.PI / 3f).Within(1e-5f));
            Assert.That(new float3(1f).atan().x, Is.EqualTo(MathF.PI / 4f).Within(1e-5f));
            // the signs of both vectors are used to find the quadrant of the result
            Assert.That(new float3(1f).atan2(new float3(-1f)).x, Is.EqualTo(3f * MathF.PI / 4f).Within(1e-5f));
            Assert.That(new double2(1, -1).atan2(new double2(1, 0)).y,
                Is.EqualTo(-Math.PI / 2).Within(1e-12));
        }
    }

    [Test]
    public void Hyperbolics()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float3(0f).sinh().x, Is.EqualTo(0f));
            Assert.That(new float3(0f).cosh().x, Is.EqualTo(1f));
            Assert.That(new float3(1f).sinh().x, Is.EqualTo(1.1752012f).Within(1e-5f));
            Assert.That(new float3(1f).cosh().x, Is.EqualTo(1.5430806f).Within(1e-5f));
            Assert.That(new float3(1f).tanh().x, Is.EqualTo(0.7615942f).Within(1e-5f));
            Assert.That(new float3(1f).asinh().x, Is.EqualTo(0.8813736f).Within(1e-5f));
            Assert.That(new float3(2f).acosh().x, Is.EqualTo(1.3169579f).Within(1e-5f));
            Assert.That(new float3(0.5f).atanh().x, Is.EqualTo(0.5493061f).Within(1e-5f));
            Assert.That(new double3(1, 2, 3).cosh().y, Is.EqualTo(Math.Cosh(2)).Within(1e-12));
            // the hyperbolics and their inverse are the inverse of each other
            Assert.That(new float3(1.5f).sinh().asinh().x, Is.EqualTo(1.5f).Within(1e-5f));
        }
    }

    [Test]
    public void ChgSign()
    {
        var v = new float3(1f, 2f, 3f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.chg_sign(new float3(-1f, 1f, -1f)), Is.EqualTo(new float3(-1f, 2f, -3f)));
            // a positive magnitude keeps the sign of the other one
            Assert.That(v.chg_sign(new float3(-1f, -1f, -1f)), Is.EqualTo(new float3(-1f, -2f, -3f)));
            Assert.That(new double2(1, 2).chg_sign(new double2(-1, 1)), Is.EqualTo(new double2(-1, 2)));
            Assert.That(new float2s(1f, 2f).chg_sign(new float2s(-1f, 1f)), Is.EqualTo(new float2s(-1f, 2f)));
        }
    }

    /// <summary>
    /// The members that read the zero padding lane of a simd register leave something else than zero there, the
    /// result of the member has to be masked to keep the padding of the vector.
    /// </summary>
    [Test]
    public void PaddingStaysZero()
    {
        var v = new float3(2f, 4f, 8f);

        using (Assert.EnterMultipleScope())
        {
            // the natural logarithm of the zero padding lane is a negative infinity
            Assert.That(v.log().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.log2().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.log10().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.log(v).vector.GetElement(3), Is.EqualTo(0f));
            // the exponential of the zero padding lane is one
            Assert.That(v.exp().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.exp2().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.exp10().vector.GetElement(3), Is.EqualTo(0f));
            // the reciprocal of the square root of the zero padding lane is an infinity
            Assert.That(v.rsqrt().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.sqrt().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.saturate().vector.GetElement(3), Is.EqualTo(0f));
            // the cosine and the hyperbolic cosine of the zero padding lane are one
            Assert.That(v.cos().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.cosh().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.sincos().cos.vector.GetElement(3), Is.EqualTo(0f));
            // the arc of the zero padding lane is zero or not a number
            Assert.That(v.acosh().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.atanh().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.asin().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.tan().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.atan2(v).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.sinh().vector.GetElement(3), Is.EqualTo(0f));
            // the step of the zero padding lane is one, the threshold of it is zero as well
            Assert.That(v.step(new float3(1f)).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.pow(2f).vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.normalize().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(v.chg_sign(v).vector.GetElement(3), Is.EqualTo(0f));
        }

        var n = new float2(2f, 4f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.log().vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(n.log().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(n.exp().vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(n.exp().vector.GetElement(3), Is.EqualTo(0f));
            Assert.That(n.step(new float2(1f)).vector.GetElement(2), Is.EqualTo(0f));
            Assert.That(n.face_forward(n, n).vector.GetElement(2), Is.EqualTo(0f));
        }

        // the register of a bool of 8 byte components is 256 bits wide
        var d = new double3(2, 4, 8);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(d.log().vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(d.exp().vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(d.cos().vector.GetElement(3), Is.EqualTo(0d));
            Assert.That(d.rsqrt().vector.GetElement(3), Is.EqualTo(0d));
        }
    }

    /// <summary>
    /// The storage variant of a vector keeps the value in an exact register or in fields, its members work the
    /// same way as the ones of the regular vector.
    /// </summary>
    [Test]
    public void StorageVariant()
    {
        var v = new float3s(1f, 4f, 9f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.sqrt(), Is.EqualTo(new float3s(1f, 2f, 3f)));
            Assert.That(v.log2().y, Is.EqualTo(2f).Within(1e-5f));
            Assert.That(v.exp().z, Is.EqualTo(MathF.Exp(9f)).Within(1e-2f));
            Assert.That(v.length(), Is.EqualTo(MathF.Sqrt(98f)).Within(1e-4f));
            Assert.That(v.normalize_safe().length(), Is.EqualTo(1f).Within(1e-4f));
        }

        var n = new float2s(4f, 16f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(n.sqrt().x, Is.EqualTo(2f).Within(1e-6f));
            Assert.That(n.sqrt().y, Is.EqualTo(4f).Within(1e-6f));
            Assert.That(n.log2().y, Is.EqualTo(4f).Within(1e-4f));
            Assert.That(n.exp2().x, Is.EqualTo(16f).Within(1e-3f));
            Assert.That((bool)n.is_finite().x, Is.True);
            Assert.That((bool)new float2s(float.NaN, 1f).is_NaN().x, Is.True);
            Assert.That(n.chg_sign(new float2s(-1f, 1f)), Is.EqualTo(new float2s(-4f, 16f)));
        }

        var d = new double3s(4, 16, 81);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(d.sqrt().z, Is.EqualTo(9d).Within(1e-9));
            Assert.That(d.log2().x, Is.EqualTo(2d).Within(1e-9));
            Assert.That((bool)new double3s(1, double.NaN, 3).is_NaN().y, Is.True);
        }

        var h = new half2((half)4f, (half)16f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(h.sqrt().x, Is.EqualTo((half)2f));
            Assert.That(h.log2().y, Is.EqualTo((half)4f));
        }
    }
}
