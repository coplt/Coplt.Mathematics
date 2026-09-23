using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Interpolates between <paramref name="start"/> and <paramref name="end"/>, <paramref name="t"/> is the
        /// interpolation factor
        /// </summary>
        /// <param name="start">The value at t = 0</param>
        /// <param name="end">The value at t = 1</param>
        /// <param name="t">The interpolation factor, 0 is <paramref name="start"/> and 1 is <paramref name="end"/></param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The interpolated value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lerp<T>(in T start, in T end, in T t) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_lerp>(start, end, t);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.lerp{T}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lerp<T>([In] this ref T t, in T start, in T end) where T : unmanaged, INumberAlgebraDispatch<T>
            => T.Visit_Self<impl_lerp>(start, end, t);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The value interpolated between two values
    /// <para>The values of the vectors that keep them in a register reach the member of the visitor that matches
    /// the width of the register, the values of every other vector reach the member of the scalar for every
    /// component of them and the values of a matrix reach it for every component of every one of its columns</para>
    /// </summary>
    internal struct impl_lerp : INumberAlgebraVisitor_Self_Self_Self_Self<impl_lerp>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar INumberAlgebraVisitor_Self_Self_Self_Self<impl_lerp>.AcceptScalar<TScalar>(TScalar start, TScalar end, TScalar t)
            => start + t * (end - start);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_lerp>.AcceptVector<TVector, TScalar>(
            in Vector64<TScalar> start, in Vector64<TScalar> end, in Vector64<TScalar> t)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.FromUnderlying(Vector64.Lerp(start.AsSingle(), end.AsSingle(), t.AsSingle()).AsByte());
            return TVector.FromUnderlying((start + t * (end - start)).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_lerp>.AcceptVector<TVector, TScalar>(
            in Vector128<TScalar> start, in Vector128<TScalar> end, in Vector128<TScalar> t)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector128.Lerp(start.AsSingle(), end.AsSingle(), t.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector128.Lerp(start.AsDouble(), end.AsDouble(), t.AsDouble()).AsByte());
            return TVector.UnsafeFromUnderlying((start + t * (end - start)).AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector INumberAlgebraVisitor_Self_Self_Self_Self<impl_lerp>.AcceptVector<TVector, TScalar>(
            in Vector256<TScalar> start, in Vector256<TScalar> end, in Vector256<TScalar> t)
        {
            if (typeof(TScalar) == typeof(float))
                return TVector.UnsafeFromUnderlying(Vector256.Lerp(start.AsSingle(), end.AsSingle(), t.AsSingle()).AsByte());
            if (typeof(TScalar) == typeof(double))
                return TVector.UnsafeFromUnderlying(Vector256.Lerp(start.AsDouble(), end.AsDouble(), t.AsDouble()).AsByte());
            return TVector.UnsafeFromUnderlying((start + t * (end - start)).AsByte());
        }
    }
}
