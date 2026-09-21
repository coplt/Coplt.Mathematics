using Coplt.Mathematics;
using Coplt.Mathematics.Generics;
using B32 = Coplt.Mathematics.b32;

namespace Tests.Arith;

/// <summary>
/// The floating point and the ieee 754 members of a vector are members of the vector itself and the members of
/// the math class reach them as well. The parameters of a forwarded member are the ones of the interface of its
/// operation in the same order, which is the order of the hlsl counterpart of the operation as well, so
/// <c>math.smoothstep(min, max, v)</c> reaches what <c>v.smoothstep(min, max)</c> does.
/// </summary>
public class TestMathFloatForwarding
{
    /// <summary>
    /// The floating point members only need the vector type, so the constraint of every one of them is the
    /// interface that has no component type.
    /// </summary>
    private static void Check<T>(T v)
        where T : unmanaged, IVectorFloatingPoint<T>
    {
        math.mod(v, v);
        math.modf(v, out var i);
        math.ceil(v);
        math.floor(v);
        math.round(v);
        math.trunc(v);
        math.frac(v);
        math.rcp(v);
        math.saturate(v);
        math.smoothstep(v, v, v);
        math.reflect(v, v);
        math.project(v, v);
        math.project_on_plane(v, v);
        math.project_normalized(v, v);
        math.project_on_plane_normalized(v, v);
        math.radians(v);
        math.degrees(v);
        math.wrap(v, v, v);
    }

    /// <summary>
    /// The ieee 754 members only need the vector type as well.
    /// </summary>
    private static void CheckIeee<T>(T v)
        where T : unmanaged, IVectorFloatingPointIeee754<T>
    {
        math.log(v);
        math.log2(v);
        math.log(v, v);
        math.log10(v);
        math.exp(v);
        math.exp2(v);
        math.exp10(v);
        math.pow(v, v);
        math.sqrt(v);
        math.rsqrt(v);
        math.normalize(v);
        math.normalize_safe(v);
        math.step(v, v);
        math.project_safe(v, v);
        math.project_safe(v, v, v);
        math.face_forward(v, v, v);
        math.sin(v);
        math.cos(v);
        _ = math.sincos(v);
        math.sincos(v, out var sin, out var cos);
        math.tan(v);
        math.asin(v);
        math.acos(v);
        math.atan(v);
        math.atan2(v, v);
        math.sinh(v);
        math.cosh(v);
        math.tanh(v);
        math.asinh(v);
        math.acosh(v);
        math.atanh(v);
        math.chg_sign(v, v);
    }

    /// <summary>
    /// The members that return a mask or a single component name the type that is not the one of the vector, so
    /// the caller spells it out. The members that take a component infer it from the argument.
    /// </summary>
    private static void CheckExtra<T, TScalar, TBool>(T v)
        where T : unmanaged, IVectorFloatingPointIeee754BoolOps<T, TScalar, TBool>
        where TScalar : unmanaged
    {
        math.pow(v, default(TScalar));
        math.wrap(v, default(TScalar), default(TScalar));
        math.refract(v, v, default(TScalar));
        _ = math.length<T, TScalar>(v);
        _ = math.distance<T, TScalar>(v, v);
        _ = math.is_NaN<T, TBool>(v);
        _ = math.is_finite<T, TBool>(v);
        _ = math.is_inf<T, TBool>(v);
        _ = math.is_pos_inf<T, TBool>(v);
        _ = math.is_neg_inf<T, TBool>(v);
    }

    [Test]
    public void Forwarding()
    {
        // the members of the math class reach the ones of the vector, the call is the same as the member
        Check(new float2(1, 2));
        Check(new float3(1, 2, 3));
        Check(new double2(1, 2));
        Check(new half3((Half)1f, (Half)2f, (Half)3f));
        Check(new float2s(1, 2));
        Check(new double3s(1, 2, 3));
        CheckIeee(new float3(1, 2, 3));
        CheckIeee(new double4(1, 2, 3, 4));
        CheckIeee(new half2((Half)1f, (Half)2f));
        CheckIeee(new float3s(1, 2, 3));
        CheckExtra<float3, float, b32v3>(new float3(1, 2, 3));
        CheckExtra<double3s, double, b64v3>(new double3s(1, 2, 3));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.ceil(new float3(1.2f, -1.2f, 2.5f)), Is.EqualTo(new float3(2f, -1f, 3f)));
            Assert.That(math.floor(new float3(1.2f, -1.2f, 2.5f)), Is.EqualTo(new float3(1f, -2f, 2f)));
            Assert.That(math.trunc(new float3(1.8f, -1.8f, 0.5f)), Is.EqualTo(new float3(1f, -1f, 0f)));
            // the fractional part is the value minus its floor, so it is never negative
            Assert.That(math.frac(new float3(1.25f, -1.25f, 0.5f)), Is.EqualTo(new float3(0.25f, 0.75f, 0.5f)));
            Assert.That(math.saturate(new float3(-1f, 0.5f, 2f)), Is.EqualTo(new float3(0f, 0.5f, 1f)));
            Assert.That(math.wrap(new float3(1.5f), new float3(0f), new float3(1f)), Is.EqualTo(new float3(0.5f)));
            // the bounds of the wrap that are the same for every component infer their type from the argument
            Assert.That(math.wrap(new float3(1.5f), 0f, 1f), Is.EqualTo(new float3(0.5f)));
            // the vector of the smoothstep is the value, the bounds come before it
            Assert.That(math.smoothstep(default(float3), new float3(1f), new float3(0.5f)), Is.EqualTo(new float3(0.5f)));
        }
    }

    [Test]
    public void Ieee()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.sqrt(new float3(1f, 4f, 9f)), Is.EqualTo(new float3(1f, 2f, 3f)));
            Assert.That(math.rsqrt(new float3(4f, 16f, 0.25f)), Is.EqualTo(new float3(0.5f, 0.25f, 2f)));
            Assert.That(math.pow(new float3(2f, 3f, 4f), 2f), Is.EqualTo(new float3(4f, 9f, 16f)));
            Assert.That(math.sin(new float4(0f)), Is.EqualTo(new float4(0f)));
            Assert.That(math.normalize(new float3(3f, 4f, 0f)), Is.EqualTo(new float3(0.6f, 0.8f, 0f)));
            // the vector of the step is the value, the threshold comes before it
            Assert.That(math.step(new float3(1f), new float3(0.5f)), Is.EqualTo(new float3(0f)));
            Assert.That(math.step(new float3(1f), new float3(2f)), Is.EqualTo(new float3(1f)));
            Assert.That(math.length<float3, float>(new float3(3f, 4f, 0f)), Is.EqualTo(5f));
            Assert.That(math.distance<float3, float>(new float3(1f), new float3(4f, 5f, 1f)), Is.EqualTo(5f));
            Assert.That(math.is_NaN<float3, b32v3>(new float3(float.NaN, 1f, 0f)),
                Is.EqualTo(new b32v3(B32.True, B32.False, B32.False)));
            Assert.That(math.is_finite<float3, b32v3>(new float3(float.NaN, float.PositiveInfinity, 0f)),
                Is.EqualTo(new b32v3(B32.False, B32.False, B32.True)));
        }
    }
}
