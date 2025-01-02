namespace Coplt.Mathematics.Geometries
{
    public struct ray
    {
        /// <summary>The origin point of the ray.</summary>
        public float3 origin;

        /// <summary>The direction of the ray.</summary>
        public float3 direction;
        
        [MethodImpl(256 | 512)]
        public ray(float3 origin, float3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        /// <summary>
        /// Returns a point at distance units along the ray.
        /// </summary>
        [MethodImpl(256 | 512)]
        public readonly float3 point_at(float distance) => direction.fma(distance, origin);
    }
}
