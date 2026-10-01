using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics
{
    internal static partial class matrix2x2_math
    {
        /// <summary>
        /// Returns the determinant of the value, which is the difference of the products of the two diagonals of
        /// it: the value of it is <c>a * d - b * c</c>, where the value is the matrix of 2 rows and 2 columns
        /// <c>[[a, b], [c, d]]</c>
        /// <para>The determinant of the value is the area of the parallelogram the two columns of it are the sides
        /// of, signed by the order of them, so it is the zero of the kind of a component of the value where the
        /// columns of it are the same line.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 2 rows and 2 columns</param>
        /// <typeparam name="T">The type of the value, a matrix of 2 rows and 2 columns</typeparam>
        /// <typeparam name="TScalar">The type of a single component of the value</typeparam>
        /// <returns>The determinant of the value</returns>
        // the type of a single component of the value is only a part of the type of the result of the member, so
        // the compiler cannot infer it from the arguments and a call of the member names it, which the attribute
        // marks this member for: a call that does not name it reaches the member of the matrix type of the value
        [SquareMatrixExtension]
        public static TScalar determinant<T, TScalar>(in T m)
            where T : unmanaged, IMatrix2x2Scalar<T, TScalar>, IFloatingPointMatrix<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        {
            var a = T.get_m00(m);
            var b = T.get_m01(m);
            var c = T.get_m10(m);
            var d = T.get_m11(m);

            // var det = a * d - b * c;
            return math.fsm(a * d, b, c);
        }
    }
}
