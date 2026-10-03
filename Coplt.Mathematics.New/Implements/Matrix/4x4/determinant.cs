using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the determinant of the value, which is the four dimensional volume of the parallelepiped the
        /// four columns of it are the sides of, signed by the order of them
        /// <para>The determinant of the value is the sum of the products of the determinants of the matrices of 2
        /// rows and 2 columns that the two first columns of the value name with the ones that the two last columns
        /// of it name, every product signed by the positions of the two columns it reaches: the determinant of a
        /// matrix whose four columns are within the same three dimensional space is the zero of the kind of a
        /// component of it.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 4 rows and 4 columns</param>
        /// <returns>The determinant of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(float4x4 m)
        {
            var r0 = m.c0;
            var r1 = m.c1;
            var r2 = m.c2;
            var r3 = m.c3;

            var p_0123 = math.fsm(r0.xxxy * r1.yzwz, r0.yzwz, r1.xxxy);
            var p_45 = math.fsm(r0.yz * r1.ww, r0.ww, r1.yz);
            var q_0123 = math.fsm(r2.xxxy * r3.yzwz, r2.yzwz, r3.xxxy);
            var q_45 = math.fsm(r2.yz * r3.ww, r2.ww, r3.yz);

            var p_vec1 = chg_sign(p_0123, new(1f, -1f, 1f, 1f));
            float4 q_vec1 = new(q_45.yx, q_0123.wz);
            var p_vec2 = chg_sign(p_45, new(-1f, 1f));
            var q_vec2 = q_0123.yx;

            return math.dot(p_vec1, q_vec1) + math.dot(p_vec2, q_vec2);
        }

        /// <inheritdoc cref="determinant(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double determinant(double4x4 m)
        {
            var r0 = m.c0;
            var r1 = m.c1;
            var r2 = m.c2;
            var r3 = m.c3;

            var p_0123 = math.fsm(r0.xxxy * r1.yzwz, r0.yzwz, r1.xxxy);
            var p_45 = math.fsm(r0.yz * r1.ww, r0.ww, r1.yz);
            var q_0123 = math.fsm(r2.xxxy * r3.yzwz, r2.yzwz, r3.xxxy);
            var q_45 = math.fsm(r2.yz * r3.ww, r2.ww, r3.yz);

            var p_vec1 = chg_sign(p_0123, new(1f, -1f, 1f, 1f));
            double4 q_vec1 = new(q_45.yx, q_0123.wz);
            var p_vec2 = chg_sign(p_45, new(-1f, 1f));
            var q_vec2 = q_0123.yx;

            return math.dot(p_vec1, q_vec1) + math.dot(p_vec2, q_vec2);
        }

        /// <inheritdoc cref="determinant(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half determinant(half4x4 m)
        {
            var r0 = m.c0;
            var r1 = m.c1;
            var r2 = m.c2;
            var r3 = m.c3;

            var p_0123 = math.fsm(r0.xxxy * r1.yzwz, r0.yzwz, r1.xxxy);
            var p_45 = math.fsm(r0.yz * r1.ww, r0.ww, r1.yz);
            var q_0123 = math.fsm(r2.xxxy * r3.yzwz, r2.yzwz, r3.xxxy);
            var q_45 = math.fsm(r2.yz * r3.ww, r2.ww, r3.yz);

            var p_vec1 = chg_sign(p_0123, new(half.One, -half.One, half.One, half.One));
            half4 q_vec1 = new(q_45.yx, q_0123.wz);
            var p_vec2 = chg_sign(p_45, new(-half.One, half.One));
            var q_vec2 = q_0123.yx;

            return math.dot(p_vec1, q_vec1) + math.dot(p_vec2, q_vec2);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.determinant(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(this float4x4 m) => math.determinant(m);

        /// <inheritdoc cref="math.determinant(double4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double determinant(this double4x4 m) => math.determinant(m);

        /// <inheritdoc cref="math.determinant(half4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half determinant(this half4x4 m) => math.determinant(m);
    }
}
