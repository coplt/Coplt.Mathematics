using System.Runtime.Intrinsics.Arm;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the largest component of the value
        /// <para>It is the maximum of every component of it: the value of a matrix is the largest component of
        /// every one of the columns of it</para>
        /// </summary>
        /// <param name="value">The value, a vector or a matrix</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The largest component of the value</returns>
        // the type of a single component is only a part of the result of the member, so the compiler cannot
        // infer it from the arguments and a call that does not name it reaches the member of the scalar type
        // of the value instead, which the generator emits for every scalar type: the attribute marks this
        // member for it
        [ScalarExtension]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TScalar hmax<T, TScalar>(in T value)
            where T : unmanaged, INumberAlgebraDispatch<T, TScalar>
            where TScalar : unmanaged, IBinaryNumber<TScalar>
            => T.Visit_Scalar<impl_horizontal_max>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The largest component of a value
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, which reduces the lanes of it, the values of every other vector reach the
    /// member of the count of its components and the values of a matrix reach the member that combines the
    /// largest component of every column of it</para>
    /// </summary>
    internal struct impl_horizontal_max : INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptScalar<TScalar>(TScalar value) => value;

        // the largest component of a matrix is the one of every column of it combined with the one of the next
        // column
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptCombine<TScalar>(TScalar a, TScalar b)
            => TScalar.Max(a, b);

        // every lane of a 64 bit register is a component of the value
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> vector)
        {
            return TScalar.Max(vector[0], vector[1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> vector)
        {
            if (TVector.Rows == 2) return TScalar.Max(vector[0], vector[1]);

            if (Vector128.IsHardwareAccelerated)
            {
                if (AdvSimd.Arm64.IsSupported)
                {
                    if (typeof(TScalar) == typeof(float) || typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(uint))
                    {
                        var s = TVector.Rows == 3
                            ? Vector128.Shuffle(vector.AsInt32(), Vector128.Create(0, 1, 2, 2)).As<int, TScalar>()
                            : vector;

                        if (typeof(TScalar) == typeof(float))
                            return AdvSimd.Arm64.MaxNumberAcross(s.AsSingle()).As<float, TScalar>().ToScalar();

                        if (typeof(TScalar) == typeof(int))
                            return AdvSimd.Arm64.MaxAcross(s.AsInt32()).As<int, TScalar>().ToScalar();

                        if (typeof(TScalar) == typeof(uint))
                            return AdvSimd.Arm64.MaxAcross(s.AsUInt32()).As<uint, TScalar>().ToScalar();
                    }
                }

                if (TVector.Rows == 3)
                {
                    var s1 = Vector128.Shuffle(vector.AsInt32(), Vector128.Create(1, 2, 0, 0)).As<int, TScalar>();
                    var r1 = Vector128.Max(vector, s1);
                    var s2 = Vector128.Shuffle(r1.AsInt32(), Vector128.Create(1, 0, 0, 0)).As<int, TScalar>();
                    return Vector128.Max(r1, s2).ToScalar();
                }
                else
                {
                    var s1 = Vector128.Shuffle(vector.AsInt32(), Vector128.Create(2, 3, 0, 1)).As<int, TScalar>();
                    var r1 = Vector128.Max(vector, s1);
                    var s2 = Vector128.Shuffle(r1.AsInt32(), Vector128.Create(1, 0, 3, 2)).As<int, TScalar>();
                    return Vector128.Max(r1, s2).ToScalar();
                }
            }

            if (TVector.Rows == 3) return TScalar.Max(TScalar.Max(vector[0], vector[1]), vector[2]);
            return TScalar.Max(TScalar.Max(vector[0], vector[1]), TScalar.Max(vector[2], vector[3]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> vector)
        {
            if (Vector256.IsHardwareAccelerated)
            {
                if (TVector.Rows == 3)
                {
                    var s1 = Vector256.Shuffle(vector.AsInt64(), Vector256.Create(1, 2, 0, 0)).As<long, TScalar>();
                    var r1 = Vector256.Max(vector, s1);
                    var s2 = Vector256.Shuffle(r1.AsInt64(), Vector256.Create(1, 0, 0, 0)).As<long, TScalar>();
                    return Vector256.Max(r1, s2).ToScalar();
                }
                else
                {
                    var s1 = Vector256.Shuffle(vector.AsInt64(), Vector256.Create(2, 3, 0, 1)).As<long, TScalar>();
                    var r1 = Vector256.Max(vector, s1);
                    var s2 = Vector256.Shuffle(r1.AsInt64(), Vector256.Create(1, 0, 3, 2)).As<long, TScalar>();
                    return Vector256.Max(r1, s2).ToScalar();
                }
            }

            if (TVector.Rows == 3) return TScalar.Max(TScalar.Max(vector[0], vector[1]), vector[2]);
            return TScalar.Max(TScalar.Max(vector[0], vector[1]), TScalar.Max(vector[2], vector[3]));
        }

        // a vector without a register has no register to reduce, so the components of it are the ones that are
        // combined
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptVector2<TVector, TScalar>(in TVector vector)
            => TScalar.Max(TVector.get_x(vector), TVector.get_y(vector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptVector3<TVector, TScalar>(in TVector vector)
            => TScalar.Max(TScalar.Max(TVector.get_x(vector), TVector.get_y(vector)), TVector.get_z(vector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Scalar<impl_horizontal_max>.AcceptVector4<TVector, TScalar>(in TVector vector)
            => TScalar.Max(TScalar.Max(TVector.get_x(vector), TVector.get_y(vector)), TScalar.Max(TVector.get_z(vector), TVector.get_w(vector)));
    }
}
