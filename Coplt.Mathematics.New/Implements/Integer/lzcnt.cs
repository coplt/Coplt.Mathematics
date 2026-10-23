using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the number of the bits that are zero at the front of every component of <paramref name="value"/>
        /// <para>The number of the bits of a component is the one of the bits of the kind of it, so a component of
        /// sixteen bits holds a number up to the sixteen of them and the value of no bit set of the kind is the
        /// one of its whole width, and the bits of a component of a signed kind are the ones of the value of it,
        /// whose sign is a bit of its own.</para>
        /// <para>Only the whole number kinds reach it, which the <see cref="IIntegerAlgebra{TSelf}"/> of the value
        /// names: the bits of a floating point component are the ones of the value of it as well, and the members
        /// of the type of the value reinterpret it, which is what <c>math.lzcnt(value.asi)</c> does for the bits
        /// of a single precision value.</para>
        /// </summary>
        /// <param name="value">The value whose every component is counted</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the number of the zero bits at the front of the component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lzcnt<T>(T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
            => T.Self<impl_lzcnt>(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.lzcnt{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lzcnt<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
            => T.Self<impl_lzcnt>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The number of the zero bits at the front of a value
    /// </summary>
    internal struct impl_lzcnt : IAlgebraVisitor_T_T<impl_lzcnt>
    {
        /// <summary>
        /// Returns the number of the bits that are zero at the front of the whole number <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value whose bits are counted</param>
        /// <typeparam name="TScalar">The whole number kind of the value</typeparam>
        /// <returns>The number of the zero bits at the front of the value of the kind of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TScalar Count<TScalar>(TScalar value) where TScalar : unmanaged
        {
            if (typeof(TScalar) == typeof(ushort))
                return (TScalar)(object)(ushort)(BitOperations.LeadingZeroCount((uint)(ushort)(object)value) - 16);
            if (typeof(TScalar) == typeof(short))
                return (TScalar)(object)(short)(BitOperations.LeadingZeroCount((uint)(ushort)(short)(object)value) - 16);
            if (typeof(TScalar) == typeof(uint))
                return (TScalar)(object)(uint)BitOperations.LeadingZeroCount((uint)(object)value);
            if (typeof(TScalar) == typeof(int))
                return (TScalar)(object)BitOperations.LeadingZeroCount((uint)(int)(object)value);
            if (typeof(TScalar) == typeof(ulong))
                return (TScalar)(object)(ulong)BitOperations.LeadingZeroCount((ulong)(object)value);
            if (typeof(TScalar) == typeof(long))
                return (TScalar)(object)(long)BitOperations.LeadingZeroCount((ulong)(long)(object)value);
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_lzcnt>.Scalar_Number<TScalar>(TScalar value) => Count(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_lzcnt>.Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if ((typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(uint)) && (AdvSimd.IsSupported || Avx512CD.VL.IsSupported || Sse2.IsSupported))
                return TVector.FromUnderlying(simd.LeadingZeroCount(vector.AsUInt32()).AsByte());
            var r = Vector128<TScalar>.Zero;
            r = r.WithElement(0, Count(vector.GetElement(0)));
            r = r.WithElement(1, Count(vector.GetElement(1)));
            if (TVector.Length > 2) r = r.WithElement(2, Count(vector.GetElement(2)));
            if (TVector.Length > 3) r = r.WithElement(3, Count(vector.GetElement(3)));
            return TVector.FromUnderlying(r.AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_lzcnt>.Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if ((typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(uint)) && (Avx512CD.VL.IsSupported || Avx2.IsSupported))
                return TVector.FromUnderlying(simd.LeadingZeroCount(vector.AsUInt32()).AsByte());
            var r = Vector256<TScalar>.Zero;
            r = r.WithElement(0, Count(vector.GetElement(0)));
            r = r.WithElement(1, Count(vector.GetElement(1)));
            if (TVector.Length > 2) r = r.WithElement(2, Count(vector.GetElement(2)));
            if (TVector.Length > 3) r = r.WithElement(3, Count(vector.GetElement(3)));
            return TVector.FromUnderlying(r.AsByte());
        }
    }
}
