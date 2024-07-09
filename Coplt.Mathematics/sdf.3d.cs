// generated by template, do not modify manually

namespace Coplt.Mathematics.Sdf;

public static partial class sdf
{

    #region float

    /// <summary>
    /// Calculate the distance to the sphere
    /// </summary>
    /// <param name="p">Origin</param>
    /// <param name="c">Center of the sphere</param>
    /// <param name="r">Radius of the sphere</param>
    /// <returns>The distance from the origin to the sphere</returns>
    [MethodImpl(256 | 512)]
    public static float sphere(float3 p, float3 c, float r) => (p - c).length() - r;

    /// <summary>
    /// Calculate the distance to the capsule
    /// </summary>
    /// <param name="p">Origin</param>
    /// <param name="a">Point A of the capsule</param>
    /// <param name="b">Point B of the capsule</param>
    /// <param name="r">Radius of the capsule</param>
    /// <returns>The distance from the origin to the capsule</returns>
    public static float capsule(float3 p, float3 a, float3 b, float r)
    {
        var pa = p - a;
        var ba = b - a;
        var h = (pa.dot(ba) / ba.dot(ba)).clamp(0.0f, 1.0f);
        return (pa - ba * h).length() - r;
    }

    #endregion // float

    #region double

    /// <summary>
    /// Calculate the distance to the sphere
    /// </summary>
    /// <param name="p">Origin</param>
    /// <param name="c">Center of the sphere</param>
    /// <param name="r">Radius of the sphere</param>
    /// <returns>The distance from the origin to the sphere</returns>
    [MethodImpl(256 | 512)]
    public static double sphere(double3 p, double3 c, double r) => (p - c).length() - r;

    /// <summary>
    /// Calculate the distance to the capsule
    /// </summary>
    /// <param name="p">Origin</param>
    /// <param name="a">Point A of the capsule</param>
    /// <param name="b">Point B of the capsule</param>
    /// <param name="r">Radius of the capsule</param>
    /// <returns>The distance from the origin to the capsule</returns>
    public static double capsule(double3 p, double3 a, double3 b, double r)
    {
        var pa = p - a;
        var ba = b - a;
        var h = (pa.dot(ba) / ba.dot(ba)).clamp(0.0, 1.0);
        return (pa - ba * h).length() - r;
    }

    #endregion // double

}
