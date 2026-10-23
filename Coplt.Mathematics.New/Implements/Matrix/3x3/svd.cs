namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the rotation of the value, which is the rotation of the singular value decomposition of the
        /// value
        /// <para>The singular value decomposition of a value of 3 rows and 3 columns is the product of a rotation
        /// of the space, a value that scales the three axes of it and another rotation of it, so the rotation of
        /// the value is the product of the two rotations of it: it is the rotation that the value holds where the
        /// value keeps the distances of the space and the angles of it, and the one closest to the value
        /// otherwise.</para>
        /// <para>The member reaches the answer with a fixed number of steps of the Jacobi iteration and of the
        /// Givens factorization, so the rotation of a value that is not a rotation of the space within the
        /// rounding of the arithmetic is an answer of the length of one that the value is near to.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <returns>The rotation of the value, which is a value of the length of one</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion svdRotation(float3x3 m) => svd.svdRotation(m);

        /// <inheritdoc cref="svdRotation(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_d svdRotation(double3x3 m) => svd.svdRotation(m);

        /// <inheritdoc cref="svdRotation(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_h svdRotation(half3x3 m) => svd.svdRotation(m);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.svdRotation(float3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion svdRotation(this float3x3 m) => math.svdRotation(m);

        /// <inheritdoc cref="math.svdRotation(double3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_d svdRotation(this double3x3 m) => math.svdRotation(m);

        /// <inheritdoc cref="math.svdRotation(half3x3)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion_h svdRotation(this half3x3 m) => math.svdRotation(m);
    }

    /// <summary>
    /// The singular value decomposition of a value of 3 rows and 3 columns, which is the product of a rotation of
    /// the space, a value that scales the three axes of it and another rotation of it
    /// <para>The symmetric value that the transpose of the value multiplied by the value is reaches a diagonal
    /// value through the Jacobi iteration, which turns the value around the three pairs of its axes in turn with
    /// the Givens rotation of the pair that makes the two components of the pair the same, and the quaternion of
    /// every turn is kept beside the value: the diagonal of the value that it reaches holds the squares of the
    /// singular values of the value, which the columns of the product of the value and the rotation of the turns
    /// hold in their lengths.</para>
    /// <para>The columns of the product are sorted by their lengths, which turns the second rotation of the value
    /// around with the signs of the columns that the sort swaps, and the Givens factorization of the product, which
    /// turns the three components of the first column of it and then the two components of the second column of
    /// the value that it reaches to the zero of the kind, is the first rotation of the value.</para>
    /// </summary>
    internal static class svd
    {
        /// <summary>
        /// The least absolute value of the determinant of a value that the member of the rotation of the value
        /// reads of the kind of a single precision component
        /// </summary>
        internal static float EpsilonDeterminantFloat => 1e-6f;

        /// <summary>
        /// The least length of the pair of the components that the member of the Givens rotation of a value of a
        /// single precision component divides of the kind of it, below which the pair is the zero of the kind
        /// </summary>
        internal static float EpsilonNormalSqrtFloat => 1e-15f;

        /// <inheritdoc cref="EpsilonDeterminantFloat"/>
        internal static double EpsilonDeterminantDouble => 1e-50;

        /// <inheritdoc cref="EpsilonNormalSqrtFloat"/>
        internal static double EpsilonNormalSqrtDouble => 1e-150;

        /// <inheritdoc cref="EpsilonDeterminantFloat"/>
        internal static half EpsilonDeterminantHalf => (half)1e-2f;

        /// <inheritdoc cref="EpsilonNormalSqrtFloat"/>
        internal static half EpsilonNormalSqrtHalf => (half)1e-3f;

        #region float

        internal static quaternion svdRotation(float3x3 a)
        {
            singularValuesDecomposition(a, out var u, out var v);
            return math.mul(u, math.conjugate(v));
        }

        private static float3 singularValuesDecomposition(float3x3 a, out quaternion u, out quaternion v)
        {
            var s = math.mul(math.transpose(a), a);
            v = jacobiIteration(ref s);
            var b = math.mul(a, new float3x3(v));
            sortSingularValues(ref b, ref v);
            u = givensQRFactorization(b, out var sigma);
            return new(sigma.c0.x, sigma.c1.y, sigma.c2.z);
        }

        private static quaternion jacobiIteration(ref float3x3 s, int iterations = 5)
        {
            var v = quaternion.Identity;

            for (var i = 0; i < iterations; ++i)
            {
                var q = approxGivensQuat(new float3(s.c0.x, s.c1.y, s.c0.y), new float4(0f, 0f, 1f, 1f));
                v = math.mul(v, q);
                var qm = new float3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);

                q = approxGivensQuat(new float3(s.c1.y, s.c2.z, s.c1.z), new float4(1f, 0f, 0f, 1f));
                v = math.mul(v, q);
                qm = new float3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);

                q = approxGivensQuat(new float3(s.c2.z, s.c0.x, s.c2.x), new float4(0f, 1f, 0f, 1f));
                v = math.mul(v, q);
                qm = new float3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);
            }

            return v;
        }

        private static quaternion approxGivensQuat(float3 pq, float4 mask)
        {
            const float c8 = 0.9238795325112867561281831893967882868224166258636424861150977312f; // cos(pi/8)
            const float s8 = 0.3826834323650897717284599840303988667613445624856270414338006356f; // sin(pi/8)
            const float g = 5.8284271247461900976033774484193961571393437507538961463533594759f; // 3 + 2 * sqrt(2)

            var ch = 2f * (pq.x - pq.y); // approx cos(a/2)
            var sh = pq.z; // approx sin(a/2)
            var r = (g * sh * sh < ch * ch ? new float4(sh, sh, sh, ch) : new float4(s8, s8, s8, c8)) * mask;
            return math.normalize(r);
        }

        private static void sortSingularValues(ref float3x3 b, ref quaternion v)
        {
            var l0 = math.length_sq(b.c0);
            var l1 = math.length_sq(b.c1);
            var l2 = math.length_sq(b.c2);

            var c = l0 < l1;
            condNegSwap(c, ref b.c0, ref b.c1);
            v = condNegSwapQuat(c, v, new float4(0f, 0f, 1f, 1f));
            condSwap(c, ref l0, ref l1);

            c = l0 < l2;
            condNegSwap(c, ref b.c0, ref b.c2);
            v = condNegSwapQuat(c, v, new float4(0f, -1f, 0f, 1f));
            condSwap(c, ref l0, ref l2);

            c = l1 < l2;
            condNegSwap(c, ref b.c1, ref b.c2);
            v = condNegSwapQuat(c, v, new float4(1f, 0f, 0f, 1f));
        }

        private static void condNegSwap(bool c, ref float3 x, ref float3 y)
        {
            var tmp = -x;
            x = c ? y : x;
            y = c ? tmp : y;
        }

        private static void condSwap(bool c, ref float x, ref float y)
        {
            var tmp = x;
            x = c ? y : x;
            y = c ? tmp : y;
        }

        private static quaternion condNegSwapQuat(bool c, quaternion q, float4 mask)
        {
            const float halfSqrt2 = 0.7071067811865475244008443621048490392848359376884740365883398689f;
            return math.mul(q, c ? mask * halfSqrt2 : quaternion.Identity.value);
        }

        private static quaternion givensQRFactorization(float3x3 b, out float3x3 r)
        {
            var u = qrGivensQuat(new float2(b.c0.x, b.c0.y), new float4(0f, 0f, 1f, 1f));
            var qmt = new float3x3(math.conjugate(u));
            r = math.mul(qmt, b);

            var q = qrGivensQuat(new float2(r.c0.x, r.c0.z), new float4(0f, -1f, 0f, 1f));
            u = math.mul(u, q);
            qmt = new float3x3(math.conjugate(q));
            r = math.mul(qmt, r);

            q = qrGivensQuat(new float2(r.c1.y, r.c1.z), new float4(1f, 0f, 0f, 1f));
            u = math.mul(u, q);
            qmt = new float3x3(math.conjugate(q));
            r = math.mul(qmt, r);

            return u;
        }

        private static quaternion qrGivensQuat(float2 pq, float4 mask)
        {
            var l = math.sqrt(pq.x * pq.x + pq.y * pq.y);
            var sh = l > EpsilonNormalSqrtFloat ? pq.y : default;
            var ch = math.abs(pq.x) + math.max(l, EpsilonNormalSqrtFloat);
            condSwap(pq.x < 0f, ref sh, ref ch);

            return math.normalize(new float4(sh, sh, sh, ch) * mask);
        }

        #endregion

        #region double

        internal static quaternion_d svdRotation(double3x3 a)
        {
            singularValuesDecomposition(a, out var u, out var v);
            return math.mul(u, math.conjugate(v));
        }

        private static double3 singularValuesDecomposition(double3x3 a, out quaternion_d u, out quaternion_d v)
        {
            var s = math.mul(math.transpose(a), a);
            v = jacobiIteration(ref s);
            var b = math.mul(a, new double3x3(v));
            sortSingularValues(ref b, ref v);
            u = givensQRFactorization(b, out var sigma);
            return new(sigma.c0.x, sigma.c1.y, sigma.c2.z);
        }

        private static quaternion_d jacobiIteration(ref double3x3 s, int iterations = 5)
        {
            var v = quaternion_d.Identity;

            for (var i = 0; i < iterations; ++i)
            {
                var q = approxGivensQuat(new double3(s.c0.x, s.c1.y, s.c0.y), new double4(0d, 0d, 1d, 1d));
                v = math.mul(v, q);
                var qm = new double3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);

                q = approxGivensQuat(new double3(s.c1.y, s.c2.z, s.c1.z), new double4(1d, 0d, 0d, 1d));
                v = math.mul(v, q);
                qm = new double3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);

                q = approxGivensQuat(new double3(s.c2.z, s.c0.x, s.c2.x), new double4(0d, 1d, 0d, 1d));
                v = math.mul(v, q);
                qm = new double3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);
            }

            return v;
        }

        private static quaternion_d approxGivensQuat(double3 pq, double4 mask)
        {
            const double c8 = 0.9238795325112867561281831893967882868224166258636424861150977312; // cos(pi/8)
            const double s8 = 0.3826834323650897717284599840303988667613445624856270414338006356; // sin(pi/8)
            const double g = 5.8284271247461900976033774484193961571393437507538961463533594759; // 3 + 2 * sqrt(2)

            var ch = 2d * (pq.x - pq.y); // approx cos(a/2)
            var sh = pq.z; // approx sin(a/2)
            var r = (g * sh * sh < ch * ch ? new double4(sh, sh, sh, ch) : new double4(s8, s8, s8, c8)) * mask;
            return math.normalize(r);
        }

        private static void sortSingularValues(ref double3x3 b, ref quaternion_d v)
        {
            var l0 = math.length_sq(b.c0);
            var l1 = math.length_sq(b.c1);
            var l2 = math.length_sq(b.c2);

            var c = l0 < l1;
            condNegSwap(c, ref b.c0, ref b.c1);
            v = condNegSwapQuat(c, v, new double4(0d, 0d, 1d, 1d));
            condSwap(c, ref l0, ref l1);

            c = l0 < l2;
            condNegSwap(c, ref b.c0, ref b.c2);
            v = condNegSwapQuat(c, v, new double4(0d, -1d, 0d, 1d));
            condSwap(c, ref l0, ref l2);

            c = l1 < l2;
            condNegSwap(c, ref b.c1, ref b.c2);
            v = condNegSwapQuat(c, v, new double4(1d, 0d, 0d, 1d));
        }

        private static void condNegSwap(bool c, ref double3 x, ref double3 y)
        {
            var tmp = -x;
            x = c ? y : x;
            y = c ? tmp : y;
        }

        private static void condSwap(bool c, ref double x, ref double y)
        {
            var tmp = x;
            x = c ? y : x;
            y = c ? tmp : y;
        }

        private static quaternion_d condNegSwapQuat(bool c, quaternion_d q, double4 mask)
        {
            const double halfSqrt2 = 0.7071067811865475244008443621048490392848359376884740365883398689;
            return math.mul(q, c ? mask * halfSqrt2 : quaternion_d.Identity.value);
        }

        private static quaternion_d givensQRFactorization(double3x3 b, out double3x3 r)
        {
            var u = qrGivensQuat(new double2(b.c0.x, b.c0.y), new double4(0d, 0d, 1d, 1d));
            var qmt = new double3x3(math.conjugate(u));
            r = math.mul(qmt, b);

            var q = qrGivensQuat(new double2(r.c0.x, r.c0.z), new double4(0d, -1d, 0d, 1d));
            u = math.mul(u, q);
            qmt = new double3x3(math.conjugate(q));
            r = math.mul(qmt, r);

            q = qrGivensQuat(new double2(r.c1.y, r.c1.z), new double4(1d, 0d, 0d, 1d));
            u = math.mul(u, q);
            qmt = new double3x3(math.conjugate(q));
            r = math.mul(qmt, r);

            return u;
        }

        private static quaternion_d qrGivensQuat(double2 pq, double4 mask)
        {
            var l = math.sqrt(pq.x * pq.x + pq.y * pq.y);
            var sh = l > EpsilonNormalSqrtDouble ? pq.y : default;
            var ch = math.abs(pq.x) + math.max(l, EpsilonNormalSqrtDouble);
            condSwap(pq.x < 0d, ref sh, ref ch);

            return math.normalize(new double4(sh, sh, sh, ch) * mask);
        }

        #endregion

        #region half

        internal static quaternion_h svdRotation(half3x3 a)
        {
            singularValuesDecomposition(a, out var u, out var v);
            return math.mul(u, math.conjugate(v));
        }

        private static half3 singularValuesDecomposition(half3x3 a, out quaternion_h u, out quaternion_h v)
        {
            var s = math.mul(math.transpose(a), a);
            v = jacobiIteration(ref s);
            var b = math.mul(a, new half3x3(v));
            sortSingularValues(ref b, ref v);
            u = givensQRFactorization(b, out var sigma);
            return new(sigma.c0.x, sigma.c1.y, sigma.c2.z);
        }

        private static quaternion_h jacobiIteration(ref half3x3 s, int iterations = 5)
        {
            var v = quaternion_h.Identity;

            for (var i = 0; i < iterations; ++i)
            {
                var q = approxGivensQuat(new half3(s.c0.x, s.c1.y, s.c0.y), new half4((half)0f, (half)0f, (half)1f, (half)1f));
                v = math.mul(v, q);
                var qm = new half3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);

                q = approxGivensQuat(new half3(s.c1.y, s.c2.z, s.c1.z), new half4((half)1f, (half)0f, (half)0f, (half)1f));
                v = math.mul(v, q);
                qm = new half3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);

                q = approxGivensQuat(new half3(s.c2.z, s.c0.x, s.c2.x), new half4((half)0f, (half)1f, (half)0f, (half)1f));
                v = math.mul(v, q);
                qm = new half3x3(q);
                s = math.mul(math.mul(math.transpose(qm), s), qm);
            }

            return v;
        }

        private static quaternion_h approxGivensQuat(half3 pq, half4 mask)
        {
            var c8 = (half)0.9238795325112867561281831893967882868224166258636424861150977312f; // cos(pi/8)
            var s8 = (half)0.3826834323650897717284599840303988667613445624856270414338006356f; // sin(pi/8)
            var g = (half)5.8284271247461900976033774484193961571393437507538961463533594759f; // 3 + 2 * sqrt(2)

            var ch = (half)2f * (pq.x - pq.y); // approx cos(a/2)
            var sh = pq.z; // approx sin(a/2)
            var r = (g * sh * sh < ch * ch ? new half4(sh, sh, sh, ch) : new half4(s8, s8, s8, c8)) * mask;
            return math.normalize(r);
        }

        private static void sortSingularValues(ref half3x3 b, ref quaternion_h v)
        {
            var l0 = math.length_sq(b.c0);
            var l1 = math.length_sq(b.c1);
            var l2 = math.length_sq(b.c2);

            var c = l0 < l1;
            condNegSwap(c, ref b.c0, ref b.c1);
            v = condNegSwapQuat(c, v, new half4((half)0f, (half)0f, (half)1f, (half)1f));
            condSwap(c, ref l0, ref l1);

            c = l0 < l2;
            condNegSwap(c, ref b.c0, ref b.c2);
            v = condNegSwapQuat(c, v, new half4((half)0f, (half)(-1f), (half)0f, (half)1f));
            condSwap(c, ref l0, ref l2);

            c = l1 < l2;
            condNegSwap(c, ref b.c1, ref b.c2);
            v = condNegSwapQuat(c, v, new half4((half)1f, (half)0f, (half)0f, (half)1f));
        }

        private static void condNegSwap(bool c, ref half3 x, ref half3 y)
        {
            var tmp = -x;
            x = c ? y : x;
            y = c ? tmp : y;
        }

        private static void condSwap(bool c, ref half x, ref half y)
        {
            var tmp = x;
            x = c ? y : x;
            y = c ? tmp : y;
        }

        private static quaternion_h condNegSwapQuat(bool c, quaternion_h q, half4 mask)
        {
            var halfSqrt2 = (half)0.7071067811865475244008443621048490392848359376884740365883398689f;
            return math.mul(q, c ? mask * halfSqrt2 : quaternion_h.Identity.value);
        }

        private static quaternion_h givensQRFactorization(half3x3 b, out half3x3 r)
        {
            var u = qrGivensQuat(new half2(b.c0.x, b.c0.y), new half4((half)0f, (half)0f, (half)1f, (half)1f));
            var qmt = new half3x3(math.conjugate(u));
            r = math.mul(qmt, b);

            var q = qrGivensQuat(new half2(r.c0.x, r.c0.z), new half4((half)0f, (half)(-1f), (half)0f, (half)1f));
            u = math.mul(u, q);
            qmt = new half3x3(math.conjugate(q));
            r = math.mul(qmt, r);

            q = qrGivensQuat(new half2(r.c1.y, r.c1.z), new half4((half)1f, (half)0f, (half)0f, (half)1f));
            u = math.mul(u, q);
            qmt = new half3x3(math.conjugate(q));
            r = math.mul(qmt, r);

            return u;
        }

        private static quaternion_h qrGivensQuat(half2 pq, half4 mask)
        {
            var l = math.sqrt(pq.x * pq.x + pq.y * pq.y);
            var sh = l > EpsilonNormalSqrtHalf ? pq.y : default;
            var ch = math.abs(pq.x) + math.max(l, EpsilonNormalSqrtHalf);
            condSwap(pq.x < (half)0f, ref sh, ref ch);

            return math.normalize(new half4(sh, sh, sh, ch) * mask);
        }

        #endregion
    }
}
