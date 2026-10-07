namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the inverse of the value, which is the matrix of the same shape that multiplies with the value
        /// to the identity of the kind of it: the value of it is the adjugate of the value divided by the
        /// determinant of it, and a component of the adjugate of it is the minor of the component signed by the
        /// position of the component, which is the determinant of the matrix of 3 rows and 3 columns that the
        /// value reaches where the row and the column of the component are left out
        /// <para>The minors of the value come in pairs whose products agree beside the signs of the components of
        /// the second one, so the products of two pairs are held in the lanes of a single vector and the sign of
        /// every minor is folded into the reciprocal of the determinant: the components of the determinant of the
        /// value are not the same value, which the four columns of the result turn around where they are scaled
        /// by the reciprocal of it.</para>
        /// <para>A matrix whose determinant is the zero of the kind of a component of it has no inverse: the
        /// division of the adjugate of the value by the zero of the kind reaches the infinity of it, and a
        /// component whose adjugate is the zero of the kind as well is not a number.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 4 rows and 4 columns</param>
        /// <returns>The inverse of the value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4x4 inverse(float4x4 m)
        {
            var (c0, c1, c2, c3) = m;

            // a pair of the halves of the registers of two columns of the value holds two of the components of the
            // rows of the value that the minors of it stand on
            var r0y_r1y_r0x_r1x = math.movelh(c1, c0); // (x1, y1, x0, y0)
            var r0z_r1z_r0w_r1w = math.movelh(c2, c3); // (x2, y2, x3, y3)
            var r2y_r3y_r2x_r3x = math.movehl(c0, c1); // (z1, w1, z0, w0)
            var r2z_r3z_r2w_r3w = math.movehl(c3, c2); // (z2, w2, z3, w3)

            var r0_wzyx = float4.shuffle_zx_xz(r0z_r1z_r0w_r1w, r0y_r1y_r0x_r1x); // x3 x2 x1 x0
            var r1_wzyx = float4.shuffle_wy_yw(r0z_r1z_r0w_r1w, r0y_r1y_r0x_r1x); // y3 y2 y1 y0
            var r2_wzyx = float4.shuffle_zx_xz(r2z_r3z_r2w_r3w, r2y_r3y_r2x_r3x); // z3 z2 z1 z0
            var r3_wzyx = float4.shuffle_wy_yw(r2z_r3z_r2w_r3w, r2y_r3y_r2x_r3x); // w3 w2 w1 w0
            var r0_xyzw = r0_wzyx.wzyx; // x0 x1 x2 x3

            var r1y_r2y_r1x_r2x = float4.shuffle_yz_yz(c1, c0); // (y1, z1, y0, z0)
            var r1z_r2z_r1w_r2w = float4.shuffle_yz_yz(c2, c3); // (y2, z2, y3, z3)
            var r3y_r0y_r3x_r0x = float4.shuffle_wx_wx(c1, c0); // (w1, x1, w0, x0)
            var r3z_r0z_r3w_r0w = float4.shuffle_wx_wx(c2, c3); // (w2, x2, w3, x3)

            // the products of the two components of a pair of the rows of the value: the second product of a pair
            // is the first one of the pair that follows it where the two components of the first one are turned
            // around, so a register holds the products of the two pairs and the sign of a pair is reached by the
            // turn of two of the lanes of the register
            var inner12_23 = math.fsm(r1y_r2y_r1x_r2x * r2z_r3z_r2w_r3w, r1z_r2z_r1w_r2w, r2y_r3y_r2x_r3x);
            var inner02_13 = math.fsm(r0y_r1y_r0x_r1x * r2z_r3z_r2w_r3w, r0z_r1z_r0w_r1w, r2y_r3y_r2x_r3x);
            var inner30_01 = math.fsm(r3z_r0z_r3w_r0w * r0y_r1y_r0x_r1x, r3y_r0y_r3x_r0x, r0z_r1z_r0w_r1w);

            var inner12 = inner12_23.xzzx;
            var inner23 = inner12_23.ywwy;

            var inner02 = inner02_13.xzzx;
            var inner13 = inner02_13.ywwy;

            var minors0 = math.fam(math.fsm(r3_wzyx * inner12, r2_wzyx, inner13), r1_wzyx, inner23);

            var denom = r0_xyzw * minors0;

            // the sum of the products of the four components of the first row of the value with the four minors of
            // that row, which the two turns of the lanes of the value of it reach: the turn of the signs of the
            // two last minors of the row stands in the components of the value beside the sign of the determinant
            denom += denom.yxwz; // x+y        x+y            z+w            z+w
            denom -= denom.zzxx; // x+y-z-w  x+y-z-w        z+w-x-y        z+w-x-y

            var rcp_denom = float4.One / denom;
            var rc0 = minors0 * rcp_denom;

            var inner30 = inner30_01.xzzx;
            var inner01 = inner30_01.ywwy;

            var minors1 = math.fsm(math.fsm(r2_wzyx * inner30, r0_wzyx, inner23), r3_wzyx, inner02);
            var rc1 = minors1 * rcp_denom;

            var minors2 = math.fsm(math.fsm(r0_wzyx * inner13, r1_wzyx, inner30), r3_wzyx, inner01);
            var rc2 = minors2 * rcp_denom;

            var minors3 = math.fam(math.fsm(r1_wzyx * inner02, r0_wzyx, inner12), r2_wzyx, inner01);
            var rc3 = minors3 * rcp_denom;

            return new(rc0, rc1, rc2, rc3);
        }

        /// <inheritdoc cref="inverse(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4x4 inverse(double4x4 m)
        {
            var (c0, c1, c2, c3) = m;

            // a pair of the halves of the registers of two columns of the value holds two of the components of the
            // rows of the value that the minors of it stand on
            var r0y_r1y_r0x_r1x = math.movelh(c1, c0); // (x1, y1, x0, y0)
            var r0z_r1z_r0w_r1w = math.movelh(c2, c3); // (x2, y2, x3, y3)
            var r2y_r3y_r2x_r3x = math.movehl(c0, c1); // (z1, w1, z0, w0)
            var r2z_r3z_r2w_r3w = math.movehl(c3, c2); // (z2, w2, z3, w3)

            var r0_wzyx = double4.shuffle_zx_xz(r0z_r1z_r0w_r1w, r0y_r1y_r0x_r1x); // x3 x2 x1 x0
            var r1_wzyx = double4.shuffle_wy_yw(r0z_r1z_r0w_r1w, r0y_r1y_r0x_r1x); // y3 y2 y1 y0
            var r2_wzyx = double4.shuffle_zx_xz(r2z_r3z_r2w_r3w, r2y_r3y_r2x_r3x); // z3 z2 z1 z0
            var r3_wzyx = double4.shuffle_wy_yw(r2z_r3z_r2w_r3w, r2y_r3y_r2x_r3x); // w3 w2 w1 w0
            var r0_xyzw = r0_wzyx.wzyx; // x0 x1 x2 x3

            var r1y_r2y_r1x_r2x = double4.shuffle_yz_yz(c1, c0); // (y1, z1, y0, z0)
            var r1z_r2z_r1w_r2w = double4.shuffle_yz_yz(c2, c3); // (y2, z2, y3, z3)
            var r3y_r0y_r3x_r0x = double4.shuffle_wx_wx(c1, c0); // (w1, x1, w0, x0)
            var r3z_r0z_r3w_r0w = double4.shuffle_wx_wx(c2, c3); // (w2, x2, w3, x3)

            // the products of the two components of a pair of the rows of the value: the second product of a pair
            // is the first one of the pair that follows it where the two components of the first one are turned
            // around, so a register holds the products of the two pairs and the sign of a pair is reached by the
            // turn of two of the lanes of the register
            var inner12_23 = math.fsm(r1y_r2y_r1x_r2x * r2z_r3z_r2w_r3w, r1z_r2z_r1w_r2w, r2y_r3y_r2x_r3x);
            var inner02_13 = math.fsm(r0y_r1y_r0x_r1x * r2z_r3z_r2w_r3w, r0z_r1z_r0w_r1w, r2y_r3y_r2x_r3x);
            var inner30_01 = math.fsm(r3z_r0z_r3w_r0w * r0y_r1y_r0x_r1x, r3y_r0y_r3x_r0x, r0z_r1z_r0w_r1w);

            var inner12 = inner12_23.xzzx;
            var inner23 = inner12_23.ywwy;

            var inner02 = inner02_13.xzzx;
            var inner13 = inner02_13.ywwy;

            var minors0 = math.fam(math.fsm(r3_wzyx * inner12, r2_wzyx, inner13), r1_wzyx, inner23);

            var denom = r0_xyzw * minors0;

            // the sum of the products of the four components of the first row of the value with the four minors of
            // that row, which the two turns of the lanes of the value of it reach: the turn of the signs of the
            // two last minors of the row stands in the components of the value beside the sign of the determinant
            denom += denom.yxwz; // x+y        x+y            z+w            z+w
            denom -= denom.zzxx; // x+y-z-w  x+y-z-w        z+w-x-y        z+w-x-y

            var rcp_denom = double4.One / denom;
            var rc0 = minors0 * rcp_denom;

            var inner30 = inner30_01.xzzx;
            var inner01 = inner30_01.ywwy;

            var minors1 = math.fsm(math.fsm(r2_wzyx * inner30, r0_wzyx, inner23), r3_wzyx, inner02);
            var rc1 = minors1 * rcp_denom;

            var minors2 = math.fsm(math.fsm(r0_wzyx * inner13, r1_wzyx, inner30), r3_wzyx, inner01);
            var rc2 = minors2 * rcp_denom;

            var minors3 = math.fam(math.fsm(r1_wzyx * inner02, r0_wzyx, inner12), r2_wzyx, inner01);
            var rc3 = minors3 * rcp_denom;

            return new(rc0, rc1, rc2, rc3);
        }

        /// <inheritdoc cref="inverse(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4x4 inverse(half4x4 m)
        {
            var (c0, c1, c2, c3) = m;

            // a pair of the halves of the registers of two columns of the value holds two of the components of the
            // rows of the value that the minors of it stand on
            var r0y_r1y_r0x_r1x = math.movelh(c1, c0); // (x1, y1, x0, y0)
            var r0z_r1z_r0w_r1w = math.movelh(c2, c3); // (x2, y2, x3, y3)
            var r2y_r3y_r2x_r3x = math.movehl(c0, c1); // (z1, w1, z0, w0)
            var r2z_r3z_r2w_r3w = math.movehl(c3, c2); // (z2, w2, z3, w3)

            var r0_wzyx = half4.shuffle_zx_xz(r0z_r1z_r0w_r1w, r0y_r1y_r0x_r1x); // x3 x2 x1 x0
            var r1_wzyx = half4.shuffle_wy_yw(r0z_r1z_r0w_r1w, r0y_r1y_r0x_r1x); // y3 y2 y1 y0
            var r2_wzyx = half4.shuffle_zx_xz(r2z_r3z_r2w_r3w, r2y_r3y_r2x_r3x); // z3 z2 z1 z0
            var r3_wzyx = half4.shuffle_wy_yw(r2z_r3z_r2w_r3w, r2y_r3y_r2x_r3x); // w3 w2 w1 w0
            var r0_xyzw = r0_wzyx.wzyx; // x0 x1 x2 x3

            var r1y_r2y_r1x_r2x = half4.shuffle_yz_yz(c1, c0); // (y1, z1, y0, z0)
            var r1z_r2z_r1w_r2w = half4.shuffle_yz_yz(c2, c3); // (y2, z2, y3, z3)
            var r3y_r0y_r3x_r0x = half4.shuffle_wx_wx(c1, c0); // (w1, x1, w0, x0)
            var r3z_r0z_r3w_r0w = half4.shuffle_wx_wx(c2, c3); // (w2, x2, w3, x3)

            // the products of the two components of a pair of the rows of the value: the second product of a pair
            // is the first one of the pair that follows it where the two components of the first one are turned
            // around, so a register holds the products of the two pairs and the sign of a pair is reached by the
            // turn of two of the lanes of the register
            var inner12_23 = math.fsm(r1y_r2y_r1x_r2x * r2z_r3z_r2w_r3w, r1z_r2z_r1w_r2w, r2y_r3y_r2x_r3x);
            var inner02_13 = math.fsm(r0y_r1y_r0x_r1x * r2z_r3z_r2w_r3w, r0z_r1z_r0w_r1w, r2y_r3y_r2x_r3x);
            var inner30_01 = math.fsm(r3z_r0z_r3w_r0w * r0y_r1y_r0x_r1x, r3y_r0y_r3x_r0x, r0z_r1z_r0w_r1w);

            var inner12 = inner12_23.xzzx;
            var inner23 = inner12_23.ywwy;

            var inner02 = inner02_13.xzzx;
            var inner13 = inner02_13.ywwy;

            var minors0 = math.fam(math.fsm(r3_wzyx * inner12, r2_wzyx, inner13), r1_wzyx, inner23);

            var denom = r0_xyzw * minors0;

            // the sum of the products of the four components of the first row of the value with the four minors of
            // that row, which the two turns of the lanes of the value of it reach: the turn of the signs of the
            // two last minors of the row stands in the components of the value beside the sign of the determinant
            denom += denom.yxwz; // x+y        x+y            z+w            z+w
            denom -= denom.zzxx; // x+y-z-w  x+y-z-w        z+w-x-y        z+w-x-y

            var rcp_denom = half4.One / denom;
            var rc0 = minors0 * rcp_denom;

            var inner30 = inner30_01.xzzx;
            var inner01 = inner30_01.ywwy;

            var minors1 = math.fsm(math.fsm(r2_wzyx * inner30, r0_wzyx, inner23), r3_wzyx, inner02);
            var rc1 = minors1 * rcp_denom;

            var minors2 = math.fsm(math.fsm(r0_wzyx * inner13, r1_wzyx, inner30), r3_wzyx, inner01);
            var rc2 = minors2 * rcp_denom;

            var minors3 = math.fam(math.fsm(r1_wzyx * inner02, r0_wzyx, inner12), r2_wzyx, inner01);
            var rc3 = minors3 * rcp_denom;

            return new(rc0, rc1, rc2, rc3);
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.inverse(float4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4x4 inverse(this float4x4 m) => math.inverse(m);

        /// <inheritdoc cref="math.inverse(double4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4x4 inverse(this double4x4 m) => math.inverse(m);

        /// <inheritdoc cref="math.inverse(half4x4)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static half4x4 inverse(this half4x4 m) => math.inverse(m);
    }
}
