namespace Coplt.Mathematics;

/// <summary>
/// A ray of the space of 3 dimensions, which is the half of the line that the point that it starts at and the
/// direction that it points to reach
/// <para>The point of the ray at a distance is the origin of it beside the direction of it scaled by the distance,
/// so the distance along the ray is the one of the space where the direction of it is of the length of one, and
/// the direction of a ray that is not of the length of one leaves the distance along the ray scaled by it.</para>
/// </summary>
public struct ray : IEquatable<ray>
{
    /// <summary>
    /// The point that the ray starts at
    /// <para>The distance along the ray is the one of the space from this point.</para>
    /// </summary>
    public float3 origin;

    /// <summary>
    /// The direction that the ray points to
    /// <para>The direction of a ray that is not of the length of one scales the distance along the ray by the
    /// length of it, so a caller that reads the point of the ray at a distance of the space asks for a direction of
    /// the length of one.</para>
    /// </summary>
    public float3 direction;

    /// <summary>
    /// Constructs a ray out of the point that it starts at and the direction that it points to
    /// </summary>
    /// <param name="origin">The point that the ray starts at</param>
    /// <param name="direction">The direction that the ray points to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ray(float3 origin, float3 direction)
    {
        this.origin = origin;
        this.direction = direction;
    }

    /// <summary>
    /// Returns the point of the ray at the distance along it that the value is handed
    /// <para>The point is the origin of the ray beside the direction of it scaled by the distance, which the fused
    /// multiply and add of the value reaches in one step.</para>
    /// </summary>
    /// <param name="distance">The distance along the ray of the point</param>
    /// <returns>The point of the ray at the distance</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float3 point_at(float distance) => math.fma(direction, new float3(distance), origin);

    /// <inheritdoc/>
    public readonly bool Equals(ray other) => origin.Equals(other.origin) && direction.Equals(other.direction);

    /// <inheritdoc/>
    public readonly override bool Equals(object? obj) => obj is ray other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(origin, direction);

    /// <inheritdoc/>
    public readonly override string ToString() => $"ray {{ origin = {origin}, direction = {direction} }}";

    /// <summary>Returns whether the two rays are the ray of the same origin and the same direction</summary>
    public static bool operator ==(ray left, ray right) => left.Equals(right);

    /// <summary>Returns whether the two rays are not the ray of the same origin and the same direction</summary>
    public static bool operator !=(ray left, ray right) => !left.Equals(right);
}
