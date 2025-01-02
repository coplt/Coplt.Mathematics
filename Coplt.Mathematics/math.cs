namespace Coplt.Mathematics;

public static partial class math_ex;

[Ex, ExTo(typeof(math_ex))]
public static partial class math
{
    #region Consts

    /// <summary>
    /// <code>e</code>
    /// </summary>
    public const float F_E = 2.7182818284590452353602874713526624977572470936999595749669676277f;
    /// <summary>
    /// <code>log(2)</code>
    /// </summary>
    public const float F_Log2 = 0.6931471805599453094172321214581765680755001343602552541206800094f;
    /// <summary>
    /// <code>log(10)</code>
    /// </summary>
    public const float F_Log10 = 2.3025850929940456840179914546843642076011014886287729760333279009f;
    /// <summary>
    /// <code>1 / log(2)</code>
    /// </summary>
    public const float F_1_Div_Log2 = 1.4426950408889634073599246810018921374266459541529859341354494069f;
    /// <summary>
    /// <code>π</code>
    /// </summary>
    public const float F_PI = 3.1415926535897932384626433832795028841971693993751058209749445923f;
    /// <summary>
    /// <code>τ = 2 * π</code>
    /// </summary>
    public const float F_Tau = F_2_PI;
    /// <summary>
    /// <code>2 * π</code>
    /// </summary>
    public const float F_2_PI = 6.2831853071795864769252867665590057683943387987502116419498891846f;
    /// <summary>
    /// <code>4 * π</code>
    /// </summary>
    public const float F_4_PI = 12.566370614359172953850573533118011536788677597500423283899778369f;
    /// <summary>
    /// <code>π / 2</code>
    /// </summary>
    public const float F_Half_PI = 1.5707963267948966192313216916397514420985846996875529104874722961f;
    /// <summary>
    /// <code>π / 4</code>
    /// </summary>
    public const float F_Quarter_PI = 0.7853981633974483096156608458198757210492923498437764552437361480f;
    /// <summary>
    /// <code>1 / π</code>
    /// </summary>
    public const float F_1_Div_PI = 0.3183098861837906715377675267450287240689192914809128974953346881f;
    /// <summary>
    /// <code>1 / (2 * π)</code>
    /// </summary>
    public const float F_1_Div_2_PI = 0.1591549430918953357688837633725143620344596457404564487476673440f;
    /// <summary>
    /// <code>1 / (4 * π)</code>
    /// </summary>
    public const float F_1_Div_4_PI = 0.0795774715459476678844418816862571810172298228702282243738336720f;
    /// <summary>
    /// <code>360 / τ</code>
    /// </summary>
    public const float F_RadToDeg = 57.295779513082320876798154814105170332405472466564321549160243861f;
    /// <summary>
    /// <code>τ / 360</code>
    /// </summary>
    public const float F_DegToRad = 0.0174532925199432957692369076848861271344287188854172545609719144f;

    /// <summary>
    /// <code>e</code>
    /// </summary>
    public const double D_E = 2.7182818284590452353602874713526624977572470936999595749669676277;
    /// <summary>
    /// <code>log(2)</code>
    /// </summary>
    public const double D_Log2 = 0.6931471805599453094172321214581765680755001343602552541206800094;
    /// <summary>
    /// <code>log(10)</code>
    /// </summary>
    public const double D_Log10 = 2.3025850929940456840179914546843642076011014886287729760333279009;
    /// <summary>
    /// <code>1 / log(2)</code>
    /// </summary>
    public const double D_1_Div_Log2 = 1.4426950408889634073599246810018921374266459541529859341354494069;
    /// <summary>
    /// <code>π</code>
    /// </summary>
    public const double D_PI = 3.1415926535897932384626433832795028841971693993751058209749445923;
    /// <summary>
    /// <code>τ = 2 * π</code>
    /// </summary>
    public const double D_Tau = D_2_PI;
    /// <summary>
    /// <code>2 * π</code>
    /// </summary>
    public const double D_2_PI = 6.2831853071795864769252867665590057683943387987502116419498891846;
    /// <summary>
    /// <code>4 * π</code>
    /// </summary>
    public const double D_4_PI = 12.566370614359172953850573533118011536788677597500423283899778369;
    /// <summary>
    /// <code>π / 2</code>
    /// </summary>
    public const double D_Half_PI = 1.5707963267948966192313216916397514420985846996875529104874722961;
    /// <summary>
    /// <code>π / 4</code>
    /// </summary>
    public const double D_Quarter_PI = 0.7853981633974483096156608458198757210492923498437764552437361480;
    /// <summary>
    /// <code>1 / π</code>
    /// </summary>
    public const double D_1_Div_PI = 0.3183098861837906715377675267450287240689192914809128974953346881;
    /// <summary>
    /// <code>1 / (2 * π)</code>
    /// </summary>
    public const double D_1_Div_2_PI = 0.1591549430918953357688837633725143620344596457404564487476673440;
    /// <summary>
    /// <code>1 / (4 * π)</code>
    /// </summary>
    public const double D_1_Div_4_PI = 0.0795774715459476678844418816862571810172298228702282243738336720;
    /// <summary>
    /// <code>360 / τ</code>
    /// </summary>
    public const double D_RadToDeg = 57.295779513082320876798154814105170332405472466564321549160243861;
    /// <summary>
    /// <code>τ / 360</code>
    /// </summary>
    public const double D_DegToRad = 0.0174532925199432957692369076848861271344287188854172545609719144;

    /// <summary>
    /// <code>The smallest positive normal number representable in a float</code>
    /// </summary>
    public const float F_MinNormal = 1.175494351e-38f;

    /// <summary>
    /// <code>The smallest positive normal number representable in a double</code>
    /// </summary>
    public const double D_MinNormal = 2.2250738585072014e-308;
    
    
    /// <summary>
    /// <code>360 / τ</code>
    /// </summary>
    public const decimal M_RadToDeg = 57.295779513082320876798154814105170332405472466564321549160243861m;
    /// <summary>
    /// <code>τ / 360</code>
    /// </summary>
    public const decimal M_DegToRad = 0.0174532925199432957692369076848861271344287188854172545609719144m;

    #endregion

    #region rcp fast

    [MethodImpl(256 | 512)]
    public static float2 rcp_fast([This] float2 v) => new(simd.Rcp(v.vector));

    [MethodImpl(256 | 512)]
    public static float3 rcp_fast([This] float3 v) => new(simd.Rcp(v.vector));

    [MethodImpl(256 | 512)]
    public static float4 rcp_fast([This] float4 v) => new(simd.Rcp(v.vector));

    [MethodImpl(256 | 512)]
    public static double2 rcp_fast([This] double2 v) => new(simd.Rcp(v.vector));

    [MethodImpl(256 | 512)]
    public static double3 rcp_fast([This] double3 v) => new(simd.Rcp(v.vector));

    [MethodImpl(256 | 512)]
    public static double4 rcp_fast([This] double4 v) => new(simd.Rcp(v.vector));

    #endregion

    #region rsqrt fast

    [MethodImpl(256 | 512)]
    public static float2 rsqrt_fast([This] float2 v) => new(simd.RSqrt(v.vector));

    [MethodImpl(256 | 512)]
    public static float3 rsqrt_fast([This] float3 v) => new(simd.RSqrt(v.vector));

    [MethodImpl(256 | 512)]
    public static float4 rsqrt_fast([This] float4 v) => new(simd.RSqrt(v.vector));

    [MethodImpl(256 | 512)]
    public static double2 rsqrt_fast([This] double2 v) => new(simd.RSqrt(v.vector));

    [MethodImpl(256 | 512)]
    public static double3 rsqrt_fast([This] double3 v) => new(simd.RSqrt(v.vector));

    [MethodImpl(256 | 512)]
    public static double4 rsqrt_fast([This] double4 v) => new(simd.RSqrt(v.vector));

    #endregion

    #region Generic Consts

    [MethodImpl(256 | 512)]
    internal static T MinNormal<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return F_MinNormal.BitCast<float, T>();
        if (typeof(T) == typeof(double)) return D_MinNormal.BitCast<double, T>();
        if (typeof(T) == typeof(half)) return ((short)0x0400).BitCast<short, T>();
        throw new NotSupportedException();
    }

    [MethodImpl(256 | 512)]
    internal static T MinRotateSafe<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return 1e-35f.BitCast<float, T>();
        if (typeof(T) == typeof(double)) return 1e-290.BitCast<double, T>();
        if (typeof(T) == typeof(half)) return ((half)1e-5f).BitCast<half, T>();
        throw new NotSupportedException();
    }

    [MethodImpl(256 | 512)]
    internal static T MaxRotateSafe<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return 1e35f.BitCast<float, T>();
        if (typeof(T) == typeof(double)) return 1e290.BitCast<double, T>();
        if (typeof(T) == typeof(half)) return ((half)1e5f).BitCast<half, T>();
        throw new NotSupportedException();
    }

    #endregion

    #region slerp

    [MethodImpl(256 | 512)]
    internal static float3 FastOrthogonal(float3 v, bool normalize = true)
    {
        var sqr = v.xy.lengthsq();
        if (sqr > 0f)
        {
            // (0,0,1) x (x,y,z)
            var im = normalize ? 1f / sqr.sqrt() : 1f;
            // new(-v.y * im, v.x * im, 0f)
            return v.yxz * new float3(-1, 1, 0) * im;
        }
        else
        {
            // (1,0,0) x (x,y,z)
            sqr = v.yz.lengthsq();
            var im = normalize ? 1f / sqr.sqrt() : 1f;
            // new(0f, -v.z * im, v.y * im);
            return v.xzy * new float3(0, -1, 1) * im;
        }
    }

    [MethodImpl(256 | 512)]
    public static float3 slerp(float3 a, float3 b, [This] float t)
    {
        var it = 1f - t; // inverse of t
        var m0 = a.length();
        var m1 = b.length();
        var mm = m0 * m1; // combined magnitude
        if (mm == 0f) return a * it + b * t; // use lerp if one of the vectors is zero
        var d = a.dot(b) / mm; // unit dot
        if (1f - d.abs() <= 1E-5f)
        {
            // abs(dot) is close to 1
            return d > 0f
                ? a * it + b * t // << use lerp for very small angles
                : quaternion.AxisAngle(FastOrthogonal(a), MathF.PI * t).mul((it + (m1 / m0) * t) * a);
        } // ^^ vectors are antiparallel, apply rotation on orthogonal axis, lerp mag
        var th = acos(d);
        // var s = sin(th) * mm;
        // var j = sin(it * th);
        // var k = sin(t * th);
        var th_it_t_one = new float4(th, it, t, 1);
        var sjk = sin(th_it_t_one.xyz * th_it_t_one.wxx) * new float3(mm, 1, 1);
        // left-hand-side scalar part = mag lerp
        // right-hand-side vector part = actual slerp
        return (m0 * it + m1 * t) * (sjk.y * m1 * a + sjk.z * m0 * b) / sjk.x;
    }

    [MethodImpl(256 | 512)]
    public static float3 slerp_unit(float3 a, float3 b, [This] float t)
    {
        var d = a.dot(b);
        if (1f - d.abs() <= 1e-6f) // smaller epsilon
            return d > 0f
                ? a * (1f - t) + b * t
                : quaternion.AxisAngle(FastOrthogonal(a, normalize: false), MathF.PI * t).mul(a);
        var th = acos(d);
        // var s = sin(th);
        // var j = sin((1f - t) * th);
        // var k = sin(t * th);
        var one_it_t = new float3(1, 1f - t, t);
        var sjk = sin(one_it_t * th);
        return (sjk.y * a + sjk.z * b) / sjk.x;
    }

    [MethodImpl(256 | 512)]
    internal static double3 FastOrthogonal(double3 v, bool normalize = true)
    {
        var sqr = v.xy.lengthsq();
        if (sqr > 0)
        {
            // (0,0,1) x (x,y,z)
            var im = normalize ? 1 / sqr.sqrt() : 1;
            // new(-v.y * im, v.x * im, 0f)
            return v.yxz * new double3(-1, 1, 0) * im;
        }
        else
        {
            // (1,0,0) x (x,y,z)
            sqr = v.yz.lengthsq();
            var im = normalize ? 1 / sqr.sqrt() : 1;
            // new(0f, -v.z * im, v.y * im);
            return v.xzy * new double3(0, -1, 1) * im;
        }
    }

    [MethodImpl(256 | 512)]
    public static double3 slerp(double3 a, double3 b, [This] float t)
    {
        var it = 1 - t; // inverse of t
        var m0 = a.length();
        var m1 = b.length();
        var mm = m0 * m1; // combined magnitude
        if (mm == 0) return a * it + b * t; // use lerp if one of the vectors is zero
        var d = a.dot(b) / mm; // unit dot
        if (1 - d.abs() <= 1E-5)
        {
            // abs(dot) is close to 1
            return d > 0
                ? a * it + b * t // << use lerp for very small angles
                : quaternion_d.AxisAngle(FastOrthogonal(a), Math.PI * t).mul((it + (m1 / m0) * t) * a);
        } // ^^ vectors are antiparallel, apply rotation on orthogonal axis, lerp mag
        var th = acos(d);
        // var s = sin(th) * mm;
        // var j = sin(it * th);
        // var k = sin(t * th);
        var th_it_t_one = new double4(th, it, t, 1);
        var sjk = sin(th_it_t_one.xyz * th_it_t_one.wxx) * new double3(mm, 1, 1);
        // left-hand-side scalar part = mag lerp
        // right-hand-side vector part = actual slerp
        return (m0 * it + m1 * t) * (sjk.y * m1 * a + sjk.z * m0 * b) / sjk.x;
    }

    [MethodImpl(256 | 512)]
    public static double3 slerp_unit(double3 a, double3 b, [This] float t)
    {
        var d = a.dot(b);
        if (1 - d.abs() <= 1e-6) // smaller epsilon
            return d > 0
                ? a * (1 - t) + b * t
                : quaternion_d.AxisAngle(FastOrthogonal(a, normalize: false), Math.PI * t).mul(a);
        var th = acos(d);
        // var s = sin(th);
        // var j = sin((1f - t) * th);
        // var k = sin(t * th);
        var one_it_t = new double3(1, 1 - t, t);
        var sjk = sin(one_it_t * th);
        return (sjk.y * a + sjk.z * b) / sjk.x;
    }

    #endregion
}

public static partial class ctor_ex;

[Ex, ExTo(typeof(ctor_ex))]
public static partial class ctor
{
    [MethodImpl(256 | 512)]
    public static half half([This]int v) => (half)v;
    [MethodImpl(256 | 512)]
    public static half half([This]float v) => (half)v;
    [MethodImpl(256 | 512)]
    public static half half([This]double v) => (half)v;
}
