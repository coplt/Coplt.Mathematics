using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics
{
    internal static partial class matrix3x3_math
    {
        /// <summary>
        /// Returns the determinant of the value, which is the sum of the products of a component of the first row
        /// of it with the determinant of the matrix of 2 rows and 2 columns that is left when the row and the
        /// column of that component are taken out of the value, every product signed by the position of the
        /// component: the value of it is <c>m00 * (m11 * m22 - m12 * m21) - m01 * (m10 * m22 - m12 * m20)
        /// + m02 * (m10 * m21 - m11 * m20)</c>
        /// <para>The determinant of the value is the volume of the parallelepiped the three columns of it are the
        /// sides of, signed by the order of them, so it is the zero of the kind of a component of the value where
        /// the columns of it are within the same plane.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 3 rows and 3 columns</param>
        /// <typeparam name="M">The type of the value, a matrix of 3 rows and 3 columns</typeparam>
        /// <typeparam name="V">The type of a column of the value, a vector of 3 components</typeparam>
        /// <typeparam name="S">The type of a single component of the value</typeparam>
        /// <returns>The determinant of the value</returns>
        // the type of a single component of the value and the type of a column of it are only a part of the type of
        // the result of the member, so the compiler cannot infer them from the arguments and a call of the member
        // names them, which the attribute marks this member for: a call that does not name them reaches the member
        // of the matrix type of the value
        [SquareMatrixExtension]
        public static S determinant<M, V, S>(in M m)
            where M : unmanaged, IMatrix3x3Scalar<M, S>, IMatrix3x3Vector<M, V>, IFloatingPointMatrix<M, S>
            where V : unmanaged, IVector3<V, S>, IFloatingPointVector<V, S>
            where S : unmanaged, IBinaryFloatingPointIeee754<S>
        {
            var c0_x = M.get_m00(m);
            var c0_y = M.get_m10(m);
            var c0_z = M.get_m20(m);

            var c1_x = M.get_m01(m);
            var c1_y = M.get_m11(m);
            var c1_z = M.get_m21(m);

            var c2_x = M.get_m02(m);
            var c2_y = M.get_m12(m);
            var c2_z = M.get_m22(m);

            // var m00 = c1.y * c2.z - c1.z * c2.y;
            // var m01 = c0.y * c2.z - c0.z * c2.y;
            // var m02 = c0.y * c1.z - c0.z * c1.y;
            var m00 = math.fsm(c1_y * c2_z, c1_z, c2_y);
            var m01 = math.fsm(c0_y * c2_z, c0_z, c2_y);
            var m02 = math.fsm(c0_y * c1_z, c0_z, c1_y);

            // return c0.x * m00 - c1.x * m01 + c2.x * m02;
            return math.fma(c2_x, m02, math.fsm(c0_x * m00, c1_x, m01));
        }
    }
}
