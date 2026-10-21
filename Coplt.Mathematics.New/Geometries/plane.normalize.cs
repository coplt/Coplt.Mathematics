namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Returns the plane of the coefficients of the value, which is the plane of the same points whose normal is
    /// of the length of one
    /// <para>The four components of the value are the coefficients <c>A</c>, <c>B</c>, <c>C</c> and <c>D</c> of
    /// the plane equation, so the plane of the value is the plane whose four components are the components of the
    /// value divided by the length of the normal of it, which is the length of the three first components of
    /// it.</para>
    /// </summary>
    /// <param name="p">The value, a plane</param>
    /// <returns>The plane of the same points whose normal is of the length of one</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static plane normalize(plane p) => new(plane.normalize(p.normal_and_distance));
}

public static partial class math_ex
{
    /// <inheritdoc cref="math.normalize(plane)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static plane normalize(this plane p) => math.normalize(p);
}
