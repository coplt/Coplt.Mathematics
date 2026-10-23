using System.Collections.Generic;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>Holds when every component of <paramref name="a"/> is not greater than the matching component
        /// of <paramref name="b"/></summary>
        /// <param name="a">The value</param>
        /// <param name="b">The value that is compared with it</param>
        /// <typeparam name="T">The type of the value, which is a vector or a matrix</typeparam>
        /// <returns>True when every component of <paramref name="a"/> is not greater than the component of <paramref name="b"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all_le<T>(T a, T b) where T : unmanaged, IAlgebraDispatch<T>
            => T.Bool<impl_all_le>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>The ordering of every component of a value at or below the matching component of another one</summary>
    internal struct impl_all_le : IAlgebraVisitor_T_T_bool<impl_all_le>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Combine(bool a, bool b) => a & b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_S_S_bool<impl_all_le>.Scalar_Number<TScalar>(TScalar a, TScalar b)
            => a <= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_S_S_bool<impl_all_le>.Scalar_Float<TScalar>(TScalar a, TScalar b) => a <= b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_T_T_bool<impl_all_le>.Simd_Number<TVector, TScalar>(Vector128<TScalar> a, Vector128<TScalar> b)
        {
            // the padding lanes of the register of a value of two or three components answer nothing, so they are
            // filled with the first component of their value, which the components answer already
            if (TVector.Length == 3)
            {
                a = Vector128.Shuffle(a.AsInt32(), Vector128.Create(0, 1, 2, 0)).As<int, TScalar>();
                b = Vector128.Shuffle(b.AsInt32(), Vector128.Create(0, 1, 2, 0)).As<int, TScalar>();
            }
            else if (TVector.Length == 2 && TVector.HavePaddingLanes)
            {
                a = Vector128.Shuffle(a.AsInt32(), Vector128.Create(0, 1, 0, 1)).As<int, TScalar>();
                b = Vector128.Shuffle(b.AsInt32(), Vector128.Create(0, 1, 0, 1)).As<int, TScalar>();
            }
            return Vector128.LessThanOrEqualAll(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IAlgebraVisitor_T_T_bool<impl_all_le>.Simd_Number<TVector, TScalar>(Vector256<TScalar> a, Vector256<TScalar> b)
        {
            // the padding lane of the register of a value of three components answers nothing, so it is filled
            // with the first component of its value, which the components answer already
            if (TVector.Length == 3)
            {
                a = Vector256.Shuffle(a.AsInt64(), Vector256.Create(0, 1, 2, 0)).As<long, TScalar>();
                b = Vector256.Shuffle(b.AsInt64(), Vector256.Create(0, 1, 2, 0)).As<long, TScalar>();
            }
            return Vector256.LessThanOrEqualAll(a, b);
        }
    }
}
