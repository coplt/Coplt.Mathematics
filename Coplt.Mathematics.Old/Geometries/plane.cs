using Coplt.Mathematics.Geometries;

namespace Coplt.Mathematics.Geometries
{
    /// <summary>
    /// A plane represented by a normal vector and a distance along the normal from the origin.
    /// </summary>
    /// <remarks>
    /// A plane splits the 3D space in half.  The normal vector points to the positive half and the other half is considered negative.
    /// </remarks>
    public record struct plane
    {
        /// <summary>
        /// A plane in the form Ax + By + Cz + Dw = 0.
        /// </summary>
        /// <remarks>
        /// Stores the plane coefficients A, B, C, D where (A, B, C) is a unit normal vector and D is the distance from the origin.
        /// A plane stored with a unit normal vector is called a normalized plane.
        /// </remarks>
        public float4 normal_and_distance;

        /// <summary>
        /// Get/set the normal vector of the plane.
        /// </summary>
        /// <remarks>
        /// It is assumed that the normal is unit length.  If you set a new plane such that Ax + By + Cz + Dw = 0 but
        /// (A, B, C) is not unit length, then you must normalize the plane by calling <see cref="math.normalize(plane)"/>.
        /// </remarks>
        public float3 normal
        {
            [MethodImpl(256 | 512)]
            readonly get => normal_and_distance.xyz;
            [MethodImpl(256 | 512)]
            set => normal_and_distance.xyz = value;
        }

        /// <summary>
        /// Get/set the distance of the plane from the origin.  May be a negative value.
        /// </summary>
        /// <remarks>
        /// It is assumed that the normal is unit length.  If you set a new plane such that Ax + By + Cz + Dw = 0 but
        /// (A, B, C) is not unit length, then you must normalize the plane by calling <see cref="math.normalize(plane)"/>.
        /// </remarks>
        public float distance
        {
            [MethodImpl(256 | 512)]
            readonly get => normal_and_distance.w;
            [MethodImpl(256 | 512)]
            set => normal_and_distance.w = value;
        }

        /// <summary>
        /// Flips the plane so the normal points in the opposite direction.
        /// </summary>
        public readonly plane flipped
        {
            [MethodImpl(256 | 512)]
            get => new() { normal_and_distance = -normal_and_distance };
        }

        /// <summary>
        /// A plane represented by a normal vector and a distance along the normal from the origin.
        /// </summary>
        /// <remarks>
        /// A plane splits the 3D space in half.  The normal vector points to the positive half and the other half is considered negative.
        /// </remarks>
        [MethodImpl(256 | 512)]
        public plane(float4 normal_and_distance) =>
            this.normal_and_distance = normal_and_distance;

        /// <summary>
        /// Constructs a Plane from arbitrary coefficients A, B, C, D of the plane equation Ax + By + Cz + Dw = 0.
        /// </summary>
        /// <remarks>
        /// The constructed plane will be the normalized form of the plane specified by the given coefficients.
        /// </remarks>
        /// <param name="coefficientA">Coefficient A from plane equation.</param>
        /// <param name="coefficientB">Coefficient B from plane equation.</param>
        /// <param name="coefficientC">Coefficient C from plane equation.</param>
        /// <param name="coefficientD">Coefficient D from plane equation.</param>
        [MethodImpl(256 | 512)]
        public plane(float coefficientA, float coefficientB, float coefficientC, float coefficientD) =>
            normal_and_distance = normalize(new float4(coefficientA, coefficientB, coefficientC, coefficientD));

        /// <summary>
        /// Constructs a plane with a normal vector and distance from the origin.
        /// </summary>
        /// <remarks>
        /// The constructed plane will be the normalized form of the plane specified by the inputs.
        /// </remarks>
        /// <param name="normal">A non-zero vector that is perpendicular to the plane.  It may be any length.</param>
        /// <param name="distance">
        /// Distance from the origin along the normal.
        /// A negative value moves the plane in the same direction as the normal while a positive value moves it in the opposite direction.
        /// </param>
        [MethodImpl(256 | 512)]
        public plane(float3 normal, float distance) =>
            normal_and_distance = normalize(new float4(normal, distance));

        /// <summary>
        /// Constructs a plane with a normal vector and a point that lies in the plane.
        /// </summary>
        /// <remarks>
        /// The constructed plane will be the normalized form of the plane specified by the inputs.
        /// </remarks>
        /// <param name="normal">A non-zero vector that is perpendicular to the plane.  It may be any length.</param>
        /// <param name="pointInPlane">A point that lies in the plane.</param>
        [MethodImpl(256 | 512)]
        public plane(float3 normal, float3 pointInPlane) : this(normal, -normal.dot(pointInPlane)) { }

        /// <summary>
        /// Constructs a plane with two vectors and a point that all lie in the plane.
        /// </summary>
        /// <remarks>
        /// The constructed plane will be the normalized form of the plane specified by the inputs.
        /// </remarks>
        /// <param name="vector1InPlane">A non-zero vector that lies in the plane.  It may be any length.</param>
        /// <param name="vector2InPlane">A non-zero vector that lies in the plane.  It may be any length and must not be a scalar multiple of <paramref name="vector1InPlane"/>.</param>
        /// <param name="pointInPlane">A point that lies in the plane.</param>
        [MethodImpl(256 | 512)]
        public plane(float3 vector1InPlane, float3 vector2InPlane, float3 pointInPlane)
            : this(vector1InPlane.cross(vector2InPlane), pointInPlane) { }

        /// <summary>
        /// Creates a normalized Plane directly without normalization cost.
        /// </summary>
        /// <remarks>
        /// If you have a unit length normal vector, you can create a Plane faster than using one of its constructors
        /// by calling this function.
        /// </remarks>
        /// <param name="unitNormal">A non-zero vector that is perpendicular to the plane.  It must be unit length.</param>
        /// <param name="distance">Distance from the origin along the normal.  A negative value moves the plane in the
        /// same direction as the normal while a positive value moves it in the opposite direction.</param>
        /// <returns>Normalized Plane constructed from given inputs.</returns>
        [MethodImpl(256 | 512)]
        public static plane CreateFromUnitNormalAndDistance(float3 unitNormal, float distance) =>
            new() { normal_and_distance = new float4(unitNormal, distance) };

        /// <summary>
        /// Creates a normalized Plane without normalization cost.
        /// </summary>
        /// <remarks>
        /// If you have a unit length normal vector, you can create a Plane faster than using one of its constructors
        /// by calling this function.
        /// </remarks>
        /// <param name="unitNormal">A non-zero vector that is perpendicular to the plane.  It must be unit length.</param>
        /// <param name="pointInPlane">A point that lies in the plane.</param>
        /// <returns>Normalized Plane constructed from given inputs.</returns>
        [MethodImpl(256 | 512)]
        public static plane CreateFromUnitNormalAndPointInPlane(float3 unitNormal, float3 pointInPlane) =>
            new() { normal_and_distance = new float4(unitNormal, -unitNormal.dot(pointInPlane)) };

        /// <summary>
        /// Normalizes the plane represented by the given plane coefficients.
        /// </summary>
        /// <remarks>
        /// The plane coefficients are A, B, C, D and stored in that order in the <see cref="float4"/>.
        /// </remarks>
        /// <param name="planeCoefficients">Plane coefficients A, B, C, D stored in x, y, z, w (respectively).</param>
        /// <returns>Normalized plane coefficients.</returns>
        [MethodImpl(256 | 512)]
        public static float4 normalize(float4 planeCoefficients)
        {
            var recipLength = planeCoefficients.xyz.lengthsq().rsqrt();
            return planeCoefficients * recipLength;
        }

        /// <summary>
        /// Implicitly converts a <see cref="plane"/> to <see cref="float4"/>.
        /// </summary>
        /// <param name="plane">Plane to convert.</param>
        /// <returns>A <see cref="float4"/> representing the plane.</returns>
        [MethodImpl(256 | 512)]
        public static implicit operator float4(plane plane) => plane.normal_and_distance;

        /// <summary>
        /// Get the signed distance from the point to the plane.
        /// </summary>
        /// <remarks>
        /// Plane must be normalized prior to calling this function.  Distance is positive if point is on side of the plane the normal points to,
        /// negative if on the opposite side and zero if the point lies in the plane.
        /// Avoid comparing equality with 0.0f when testing if a point lies exactly in the plane and use an approximate comparison instead.
        /// </remarks>
        /// <param name="point">Point to find the signed distance with.</param>
        /// <returns>Signed distance of the point from the plane.</returns>
        [MethodImpl(256 | 512)]
        public readonly float distance_to_point(float3 point) => normal_and_distance.dot(new float4(point, 1.0f));

        /// <summary>
        /// Projects the given point onto the plane.
        /// </summary>
        /// <remarks>
        /// Plane must be normalized prior to calling this function.  The result is the position closest to the point
        /// that still lies in the plane.
        /// </remarks>
        /// <param name="point">Point to project onto the plane.</param>
        /// <returns>Projected point that's inside the plane.</returns>
        [MethodImpl(256 | 512)]
        public readonly float3 projection(float3 point) => point - normal * distance_to_point(point);

        /// <summary>
        /// Ray cast to the plane
        /// </summary>
        /// <param name="ray">The ray</param>
        /// <param name="distance">Intersection distance</param>
        /// <returns>Is intersect</returns>
        [MethodImpl(256 | 512)]
        public readonly bool ray_cast(ray ray, out float distance)
        {
            var normal = this.normal;
            var de_nom = normal.dot(ray.direction);
            distance = (this.distance - normal.dot(ray.origin)) / de_nom;
            return de_nom.abs() > 0.0001f && distance >= 0.0f;
        }

        /// <summary>
        /// Ray cast to the plane
        /// </summary>
        /// <param name="ray">The ray</param>
        /// <param name="distance">Intersection distance</param>
        /// <param name="point">Intersection</param>
        /// <returns>Is intersect</returns>
        [MethodImpl(256 | 512)]
        public readonly bool ray_cast(ray ray, out float distance, out float3 point)
        {
            var normal = this.normal;
            var de_nom = normal.dot(ray.direction);
            distance = (this.distance - normal.dot(ray.origin)) / de_nom;
            var intersects = de_nom.abs() > 0.0001f && distance >= 0.0f;
            point = ray.point_at(distance) * (intersects ? 1 : 0);
            return intersects;
        }

        /// <summary>
        /// For a given point returns the closest point on the plane.
        /// </summary>
        /// <param name="point">The point to project onto the plane.</param>
        /// <returns>A point on the plane that is closest to point.</returns>
        [MethodImpl(256 | 512)]
        public readonly float3 closest_point(float3 point)
        {
            var normal = this.normal;
            return point - normal * (normal.dot(point) + distance);
        }

        #region Record

        public readonly bool Equals(plane other) => normal_and_distance.Equals(other.normal_and_distance);

        public readonly override int GetHashCode() => normal_and_distance.GetHashCode();

        private bool PrintMembers(System.Text.StringBuilder sb)
        {
            sb.Append($"normal = {normal}, distance = {distance}");
            return true;
        }

        #endregion
    }
}

namespace Coplt.Mathematics
{
    [Ex]
    public static partial class math
    {
        /// <summary>
        /// Normalizes the plane represented by the given plane coefficients.
        /// </summary>
        public static plane normalize([This] plane plane) => new(plane.normalize(plane.normal_and_distance));
    }
}
