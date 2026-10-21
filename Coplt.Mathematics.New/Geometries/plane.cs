namespace Coplt.Mathematics;

/// <summary>
/// A plane of the space of 3 dimensions, which is the set of the points that the value of the plane takes to the
/// zero of the kind
/// <para>A plane splits the space in half: the normal of it points to the positive half and the other half is the
/// negative one. The coefficients of the plane are the components of the normal of it beside the distance of it
/// from the origin, so the value of the plane at a point is the dot product of the coefficients of it with the
/// point beside the one of the kind, which is the distance of the point from the plane signed by the half it lies
/// in.</para>
/// <para>A plane whose normal is of the length of one is a normalized plane, which every member that reads the
/// distance of a point from the plane asks for: the constructors of the plane normalize the coefficients that they
/// are handed, and a plane that is built out of the four coefficients of the plane equation through the one that
/// takes the value of them holds them as they are.</para>
/// </summary>
public struct plane : IEquatable<plane>
{
    /// <summary>
    /// The coefficients <c>A</c>, <c>B</c>, <c>C</c> and <c>D</c> of the plane equation
    /// <c>Ax + By + Cz + Dw = 0</c>, in that order
    /// <para>The three first components are the normal of the plane, which is of the length of one where the plane
    /// is normalized, and the fourth one is the distance of the plane from the origin, which may be a negative
    /// value.</para>
    /// </summary>
    public float4 normal_and_distance;

    /// <summary>
    /// The normal of the plane, which is the vector perpendicular to every line that lies in the plane
    /// <para>The normal of a normalized plane is of the length of one, which the constructors of the plane reach
    /// for the normal that they are handed; a normal that is not of the length of one leaves a plane whose members
    /// that read the distance of a point from it are not the distance of the point from it.</para>
    /// </summary>
    public float3 normal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => normal_and_distance.xyz;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => normal_and_distance.xyz = value;
    }

    /// <summary>
    /// The distance of the plane from the origin along the normal of it, which may be a negative value
    /// <para>A negative value moves the plane in the same direction as the normal points to and a positive value
    /// moves it in the opposite direction, so the plane whose normal is <c>(0, 1, 0)</c> and whose distance is
    /// <c>2</c> is the plane <c>y = -2</c>.</para>
    /// </summary>
    public float distance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => normal_and_distance.w;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => normal_and_distance.w = value;
    }

    /// <summary>
    /// The plane of the value whose normal points in the opposite direction
    /// <para>The flipped plane is the plane of the same points, whose coefficients are the ones of the value
    /// negated: the half that the value takes to the positive side is the half that the flipped plane takes to the
    /// negative side.</para>
    /// </summary>
    public readonly plane flipped
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new() { normal_and_distance = -normal_and_distance };
    }

    /// <summary>
    /// Constructs a plane out of the four coefficients of the plane equation, which it holds as they are
    /// <para>The normal of the plane is not normalized by the constructor, so a plane that a caller builds out of
    /// the coefficients of a plane that is not normalized reaches <see cref="math.normalize(plane)"/> for the
    /// normalized plane of them.</para>
    /// </summary>
    /// <param name="normal_and_distance">The coefficients <c>A</c>, <c>B</c>, <c>C</c> and <c>D</c> of the plane
    /// equation, in that order</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public plane(float4 normal_and_distance) => this.normal_and_distance = normal_and_distance;

    /// <summary>
    /// Constructs the normalized plane out of the four coefficients <c>A</c>, <c>B</c>, <c>C</c> and <c>D</c> of
    /// the plane equation
    /// </summary>
    /// <param name="coefficientA">The coefficient <c>A</c> of the plane equation</param>
    /// <param name="coefficientB">The coefficient <c>B</c> of the plane equation</param>
    /// <param name="coefficientC">The coefficient <c>C</c> of the plane equation</param>
    /// <param name="coefficientD">The coefficient <c>D</c> of the plane equation</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public plane(float coefficientA, float coefficientB, float coefficientC, float coefficientD)
        : this(normalize(new float4(coefficientA, coefficientB, coefficientC, coefficientD))) { }

    /// <summary>
    /// Constructs the normalized plane out of the normal of the plane and the distance of it from the origin
    /// </summary>
    /// <param name="normal">A vector perpendicular to the plane, which is a vector of the length of one in the
    /// plane that the constructor builds</param>
    /// <param name="distance">The distance of the plane from the origin along the normal of it, where a negative
    /// value moves the plane in the same direction as the normal points to and a positive value moves it in the
    /// opposite direction</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public plane(float3 normal, float distance)
        : this(normalize(new float4(normal, distance))) { }

    /// <summary>
    /// Constructs the normalized plane out of the normal of the plane and a point that lies in it
    /// </summary>
    /// <param name="normal">A vector perpendicular to the plane, which is a vector of the length of one in the
    /// plane that the constructor builds</param>
    /// <param name="point_in_plane">A point that lies in the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public plane(float3 normal, float3 point_in_plane)
        : this(normal, -math.dot(normal, point_in_plane)) { }

    /// <summary>
    /// Constructs the normalized plane out of two vectors that lie in the plane and a point that lies in it
    /// </summary>
    /// <param name="vector1_in_plane">A vector that lies in the plane, which is not the zero of the kind</param>
    /// <param name="vector2_in_plane">Another vector that lies in the plane, which is not the zero of the kind and
    /// which is not a multiple of the first one</param>
    /// <param name="point_in_plane">A point that lies in the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public plane(float3 vector1_in_plane, float3 vector2_in_plane, float3 point_in_plane)
        : this(math.cross(vector1_in_plane, vector2_in_plane), point_in_plane) { }

    /// <summary>
    /// Builds the plane of a normal of the length of one and the distance of the plane from the origin without
    /// normalizing the coefficients of it
    /// </summary>
    /// <param name="unit_normal">A vector perpendicular to the plane, which must be of the length of one</param>
    /// <param name="distance">The distance of the plane from the origin along the normal of it</param>
    /// <returns>The normalized plane of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static plane CreateFromUnitNormalAndDistance(float3 unit_normal, float distance)
        => new() { normal_and_distance = new float4(unit_normal, distance) };

    /// <summary>
    /// Builds the plane of a normal of the length of one and a point that lies in it without normalizing the
    /// coefficients of it
    /// </summary>
    /// <param name="unit_normal">A vector perpendicular to the plane, which must be of the length of one</param>
    /// <param name="point_in_plane">A point that lies in the plane</param>
    /// <returns>The normalized plane of the two values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static plane CreateFromUnitNormalAndPointInPlane(float3 unit_normal, float3 point_in_plane)
        => new() { normal_and_distance = new float4(unit_normal, -math.dot(unit_normal, point_in_plane)) };

    /// <summary>
    /// Returns the coefficients of the normalized plane of the coefficients that the value holds
    /// <para>The four components of the value are the coefficients <c>A</c>, <c>B</c>, <c>C</c> and <c>D</c> of
    /// the plane equation, so the coefficients of the normalized plane are the components of the value divided by
    /// the length of the normal of it.</para>
    /// </summary>
    /// <param name="plane_coefficients">The coefficients <c>A</c>, <c>B</c>, <c>C</c> and <c>D</c> of the plane
    /// equation, in that order</param>
    /// <returns>The coefficients of the normalized plane</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 normalize(float4 plane_coefficients)
    {
        var reciprocal_length = math.rsqrt(math.length_sq(plane_coefficients.xyz));
        return plane_coefficients * reciprocal_length;
    }

    /// <summary>
    /// Converts the plane to the value of the four coefficients of the plane equation, which it holds
    /// </summary>
    /// <param name="plane">The plane to convert</param>
    /// <returns>The coefficients of the plane equation</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float4(plane plane) => plane.normal_and_distance;

    /// <summary>
    /// Returns the distance of the point from the plane, signed by the half of the space that the point lies in
    /// <para>The distance is positive where the point lies in the half that the normal of the plane points to,
    /// negative where it lies in the other half and the zero of the kind where it lies in the plane. The plane
    /// must be normalized for the answer to be the distance of the point, and a caller that asks whether a point
    /// lies in the plane compares the answer with an epsilon of the kind instead of with the zero of it.</para>
    /// </summary>
    /// <param name="point">The point to read the distance of</param>
    /// <returns>The distance of the point from the plane, signed by the half it lies in</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float distance_to_point(float3 point)
        => math.dot(normal_and_distance, new float4(point, 1f));

    /// <summary>
    /// Returns the point of the plane closest to the point that the value is handed, which is the point of the
    /// plane that the point reaches where the normal of the plane is moved along
    /// <para>The plane must be normalized for the answer to be the closest point of it.</para>
    /// </summary>
    /// <param name="point">The point to read the closest point of the plane of</param>
    /// <returns>The closest point of the plane</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float3 projection(float3 point) => point - normal * distance_to_point(point);

    /// <summary>
    /// Returns the point of the plane closest to the point that the value is handed, which is the point of the
    /// plane that the point reaches where the normal of the plane is moved along
    /// <para>The plane must be normalized for the answer to be the closest point of it.</para>
    /// </summary>
    /// <param name="point">The point to read the closest point of the plane of</param>
    /// <returns>The closest point of the plane</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float3 closest_point(float3 point)
        => point - normal * (math.dot(normal, point) + distance);

    /// <inheritdoc/>
    public readonly bool Equals(plane other) => normal_and_distance.Equals(other.normal_and_distance);

    /// <inheritdoc/>
    public readonly override bool Equals(object? obj) => obj is plane other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => normal_and_distance.GetHashCode();

    /// <inheritdoc/>
    public readonly override string ToString() => $"plane {{ normal = {normal}, distance = {distance} }}";

    /// <summary>Returns whether the two planes are the plane of the same coefficients</summary>
    public static bool operator ==(plane left, plane right) => left.Equals(right);

    /// <summary>Returns whether the two planes are not the plane of the same coefficients</summary>
    public static bool operator !=(plane left, plane right) => !left.Equals(right);
}
