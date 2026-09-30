using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using Coplt.Mathematics.Generics;
using half = System.Half;

namespace Tests.Arith;

/// <summary>
/// The ieee 754 members of a vector implement <c>IVectorFloatingPointIeee754</c>: the step and the refraction,
/// the face forward, the trigonometry, the hyperbolics and the change of the sign. The logarithms and the
/// exponentials left the interface, they are the members of the dispatch of the algebra of the kind of the
/// value now. The members of a simd vector keep the padding lanes of it at zero, the members of a vector
/// without a register work on the components. The checks of the special floating point values are the members
/// of the <c>math</c> class from <c>is_NaN</c> on, the value of a check is a value of the kind of the value it
/// was built from and the bits of a component of it that holds are not all zero.
/// </summary>
public class TestIeee754
{
    /// <summary>
    /// Every ieee 754 vector implements both the interface and the dispatch of the algebra of the kind of its
    /// value, so every member below is reachable through one of the two, the refraction is a static member of
    /// the interface.
    /// </summary>
    private static void Check<T, TScalar>(T v)
        where T : unmanaged, IVectorFloatingPointIeee754<T, TScalar>, IAlgebraDispatch<T>, IFloatDispatch<T>
        where TScalar : unmanaged, INumberBase<TScalar>
    {
        // the logarithms and the exponentials are not members of the ieee 754 interface any more: they are the
        // members of the dispatch of the algebra of the kind of the value, which a caller reaches with the
        // members of the math class and of the extension of a value
        v.log();
        v.log2();
        v.log(v);
        v.log10();
        v.exp();
        v.exp2();
        v.exp10();
        // the power, the square root and its reciprocal, the normalization and the length and the distance of two
        // vectors are not members of the ieee 754 interface: they are the members of the algebra of the kind of
        // the value, which a caller reaches with the members of the math class and of the extension of a value
        v.step(v);
        T.refract(v, v, default);
        v.face_forward(v, v);
        // the trigonometry left the interface as well, the pair of the sine and the cosine is the member of the
        // dispatch that hands the value over once and receives two values out of it
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

        using (Assert.EnterMultipleScope())
        {
            // the single and the pair form of the sine and the cosine agree
            Assert.That(v.sin().Equals(sin), Is.True);
            Assert.That(v.cos().Equals(cos), Is.True);
            Assert.That(sin.Equals(s), Is.True);
            Assert.That(cos.Equals(c), Is.True);
        }
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
        Check<double4, double>(new double4(1, 2, 3, 4));
        Check<half2, half>(new half2((half)1f, (half)2f));
        Check<half3, half>(new half3((half)1f, (half)2f, (half)3f));
        Check<half4, half>(new half4((half)1f, (half)2f, (half)3f, (half)4f));
    }

    /// <summary>
    /// The checks of the special values produce a value of the shape of the vector: the bits of a component of a
    /// check that holds are not all zero.
    /// </summary>
    [Test]
    public void SpecialValues()
    {
        var v = new float3(1f, float.NaN, float.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_NaN(v).x != 0f, Is.False);
            Assert.That(math.is_NaN(v).y != 0f, Is.True);
            Assert.That(math.is_NaN(v).z != 0f, Is.False);

            Assert.That(math.is_finite(v).x != 0f, Is.True);
            Assert.That(math.is_finite(v).y != 0f, Is.False);
            Assert.That(math.is_finite(v).z != 0f, Is.False);

            Assert.That(math.is_inf(v).x != 0f, Is.False);
            Assert.That(math.is_inf(v).z != 0f, Is.True);

            Assert.That(math.is_pos_inf(v).z != 0f, Is.True);
            Assert.That(math.is_neg_inf(v).z != 0f, Is.False);
        }

        var n = new float3(float.NegativeInfinity, 0f, -0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_neg_inf(n).x != 0f, Is.True);
            Assert.That(math.is_pos_inf(n).x != 0f, Is.False);
            Assert.That(math.is_inf(n).x != 0f, Is.True);
            // a zero is finite, the sign of it does not matter
            Assert.That(math.is_finite(n).y != 0f, Is.True);
            Assert.That(math.is_finite(n).z != 0f, Is.True);
        }

        var d = new double3(1, double.PositiveInfinity, double.NegativeInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_inf(d).x != 0d, Is.False);
            Assert.That(math.is_pos_inf(d).y != 0d, Is.True);
            Assert.That(math.is_neg_inf(d).z != 0d, Is.True);
        }

        var h = new half3((half)1f, half.NaN, half.PositiveInfinity);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.is_NaN(h).y != (half)0f, Is.True);
            Assert.That(math.is_inf(h).z != (half)0f, Is.True);
            Assert.That(math.is_finite(h).x != (half)0f, Is.True);
        }

        // the name of a check in HLSL reaches the same member
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.isnan(v).y != 0f, Is.True);
            Assert.That(math.isinf(v).z != 0f, Is.True);
            Assert.That(math.isfinite(v).x != 0f, Is.True);
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

    /// <summary>
    /// The length and the distance of a vector are the members of the algebra of the kind of the value now, so
    /// the call on the value reaches the generated member of the scalar type of it.
    /// </summary>
    [Test]
    public void LengthDistance()
    {
        var v = new float3(3f, 4f, 0f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.length(), Is.EqualTo(5f).Within(1e-5f));
            Assert.That(v.distance(new float3(3f, 0f, 0f)), Is.EqualTo(4f).Within(1e-5f));
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
            var d = math.normalize(new float3(0.3f, -0.4f, 0.5f));
            var r = float3.refract(d, new float3(0f, 0f, 1f), 1f);
            Assert.That(r.x, Is.EqualTo(d.x).Within(1e-5f));
            Assert.That(r.y, Is.EqualTo(d.y).Within(1e-5f));
            Assert.That(r.z, Is.EqualTo(-d.z).Within(1e-5f));
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
            // the reciprocal of the square root of a value of a component that the padding lane holds is an
            // infinity, the value of the member of the vector keeps the padding lanes at zero
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
        }
    }

    /// <summary>
    /// A vector without a register works on the components of it.
    /// </summary>
    [Test]
    public void WithoutRegister()
    {
        var h = new half2((half)4f, (half)16f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(h.log2().y, Is.EqualTo((half)4f));
        }
    }
}
