using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the smaller of the two values component by component
        /// <para>It is the minimum the platform computes itself, which is the one of <see cref="min{T}(in T, in T)"/>
        /// beside the way it handles a nan and a negative zero: every platform is free to handle the two of them
        /// in a way of its own</para>
        /// </summary>
        /// <param name="a">The first of the two values</param>
        /// <param name="b">The second of the two values</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The smaller of the two values component by component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T min_native<T>(in T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_min_native>(a, b);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.min_native{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T min_native<T>(this T a, in T b) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_min_native>(a, b);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The smaller of two values, the one the platform computes itself
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_min_native : INumberAlgebraVisitor_Self_Self_Self<impl_min_native>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self<impl_min_native>.AcceptScalar<TScalar>(TScalar a, TScalar b)
            => TScalar.MinNative(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_min_native>.AcceptVector<TVector, TScalar>(in Vector64<TScalar> a, in Vector64<TScalar> b)
            => TVector.FromUnderlying(Vector64.MinNative(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_min_native>.AcceptVector<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector128.MinNative(a, b).AsByte());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self<impl_min_native>.AcceptVector<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
            => TVector.UnsafeFromUnderlying(Vector256.MinNative(a, b).AsByte());
    }
}
