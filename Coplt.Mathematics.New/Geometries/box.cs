namespace Coplt.Mathematics;

/// <summary>
/// A box of the space of 3 dimensions, which is the set of the points that lie between the two corners of it
/// <para>The components of a corner of the box are the smallest and the largest value of the components of the
/// points of the box at the index of them, so every component of a point of the box is between the two components
/// of the same index of the corners of it.</para>
/// </summary>
public struct box : IEquatable<box>
{
    /// <summary>
    /// The corner of the box whose every component is the smallest value of the components of the points of the box
    /// at the index of it
    /// </summary>
    public float3 min;

    /// <summary>
    /// The corner of the box whose every component is the largest value of the components of the points of the box
    /// at the index of it
    /// </summary>
    public float3 max;

    /// <inheritdoc/>
    public readonly bool Equals(box other) => min.Equals(other.min) && max.Equals(other.max);

    /// <inheritdoc/>
    public readonly override bool Equals(object? obj) => obj is box other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(min, max);

    /// <inheritdoc/>
    public readonly override string ToString() => $"box {{ min = {min}, max = {max} }}";

    /// <summary>Returns whether the two boxes are the box of the same corners</summary>
    public static bool operator ==(box left, box right) => left.Equals(right);

    /// <summary>Returns whether the two boxes are not the box of the same corners</summary>
    public static bool operator !=(box left, box right) => !left.Equals(right);
}
