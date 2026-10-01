using Coplt.Mathematics.Algebras;

namespace Coplt.Mathematics
{
    internal static partial class matrix2x2_math
    {
        /// <summary>
        /// Returns the inverse of the value, which is the matrix of the same shape that multiplies with the value
        /// to the identity of the kind of it: the value of it is the adjugate of the value divided by the
        /// determinant of it, which is <c>1 / (a * d - b * c)</c> times <c>[[d, -b], [-c, a]]</c>, where the value
        /// is the matrix of 2 rows and 2 columns <c>[[a, b], [c, d]]</c>
        /// <para>A matrix whose determinant is the zero of the kind of a component of it has no inverse: the
        /// division of the adjugate of the value by the zero of the kind reaches the infinity of it, and a
        /// component whose adjugate is the zero of the kind as well is not a number.</para>
        /// </summary>
        /// <param name="m">The value, a matrix of 2 rows and 2 columns</param>
        /// <typeparam name="T">The type of the value, a matrix of 2 rows and 2 columns</typeparam>
        /// <typeparam name="TScalar">The type of a single component of the value</typeparam>
        /// <returns>The inverse of the value</returns>
        // the type of a single component of the value is only a part of the type of the result of the member, so
        // the compiler cannot infer it from the arguments and a call of the member names it, which the attribute
        // marks this member for: a call that does not name it reaches the member of the matrix type of the value
        [SquareMatrixExtension]
        [OverloadResolutionPriority(-1)]
        public static T inverse<T, TScalar>(in T m)
            where T : unmanaged, IMatrix2x2Scalar<T, TScalar>, IFloatingPointMatrix<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        {
            var a = T.get_m00(m);
            var b = T.get_m01(m);
            var c = T.get_m10(m);
            var d = T.get_m11(m);

            // var det = a * d - b * c;
            var det = math.fsm(a * d, b, c);

            return T.Create(d, -b, -c, a) * (TScalar.One / det);
        }
    }
}
