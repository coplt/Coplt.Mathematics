using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the number of the bits that are set at every component of <paramref name="value"/>
        /// <para>The number of the bits of a component is the one of the bits of the kind of it, so a component
        /// of sixteen bits holds a number up to the sixteen of them, and the bits of a component of a signed kind
        /// are the ones of the value of it, whose sign is a bit of its own.</para>
        /// <para>Only the whole number kinds reach it, which the <see cref="IIntegerAlgebra{TSelf}"/> of the value
        /// names: the bits of a floating point component are the ones of the value of it as well, and the members
        /// of the type of the value reinterpret it, which is what <c>math.popcnt(value.asi)</c> does for the bits
        /// of a single precision value.</para>
        /// </summary>
        /// <param name="value">The value whose every component is counted</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the number of the set bits of the component</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T popcnt<T>(T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
            => T.Self<impl_popcnt>(value);

        /// <summary>
        /// Counts the number of bits (per component) set in the input integer
        /// <para>The function of the standard is the one of the library that counts the set bits of the value of
        /// every component, the <see cref="math.popcnt{T}(T)"/> of it, which the whole number kinds of the library
        /// reach alone.</para>
        /// </summary>
        /// <param name="value">The value whose every component is counted</param>
        /// <typeparam name="T">The type of the value, a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the number of the set bits of the component</returns>
        /// <remarks>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/countbits"/></para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T countbits<T>(T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
            => popcnt(value);
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.popcnt{T}(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T popcnt<T>(this T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
            => T.Self<impl_popcnt>(value);
    }
}

namespace Coplt.Mathematics.Implements
{
    /// <summary>
    /// The number of the set bits of a value
    /// </summary>
    internal struct impl_popcnt : IAlgebraVisitor_T_T<impl_popcnt>
    {
        /// <summary>
        /// Returns the number of the bits that are set at the whole number <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value whose bits are counted</param>
        /// <typeparam name="TScalar">The whole number kind of the value</typeparam>
        /// <returns>The number of the set bits of the value of the kind of it</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TScalar Count<TScalar>(TScalar value) where TScalar : unmanaged
        {
            if (typeof(TScalar) == typeof(ushort))
                return (TScalar)(object)(ushort)BitOperations.PopCount((uint)(ushort)(object)value);
            if (typeof(TScalar) == typeof(short))
                return (TScalar)(object)(short)BitOperations.PopCount((uint)(ushort)(short)(object)value);
            if (typeof(TScalar) == typeof(uint))
                return (TScalar)(object)(uint)BitOperations.PopCount((uint)(object)value);
            if (typeof(TScalar) == typeof(int))
                return (TScalar)(object)(int)BitOperations.PopCount((uint)(int)(object)value);
            if (typeof(TScalar) == typeof(ulong))
                return (TScalar)(object)(ulong)BitOperations.PopCount((ulong)(object)value);
            if (typeof(TScalar) == typeof(long))
                return (TScalar)(object)(long)BitOperations.PopCount((ulong)(long)(object)value);
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar IAlgebraVisitor_S_S<impl_popcnt>.Scalar_Number<TScalar>(TScalar value) => Count(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_popcnt>.Simd_Number<TVector, TScalar>(Vector128<TScalar> vector)
        {
            if ((typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(uint)) && (AdvSimd.IsSupported || Ssse3.IsSupported))
                return TVector.FromUnderlying(simd.PopCount(vector.AsUInt32()).AsByte());
            var r = Vector128<TScalar>.Zero;
            r = r.WithElement(0, Count(vector.GetElement(0)));
            r = r.WithElement(1, Count(vector.GetElement(1)));
            if (TVector.Length > 2) r = r.WithElement(2, Count(vector.GetElement(2)));
            if (TVector.Length > 3) r = r.WithElement(3, Count(vector.GetElement(3)));
            return TVector.FromUnderlying(r.AsByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TVector IAlgebraVisitor_T_T<impl_popcnt>.Simd_Number<TVector, TScalar>(Vector256<TScalar> vector)
        {
            if ((typeof(TScalar) == typeof(int) || typeof(TScalar) == typeof(uint)) && (Avx2.IsSupported))
                return TVector.FromUnderlying(simd.PopCount(vector.AsUInt32()).AsByte());
            var r = Vector256<TScalar>.Zero;
            r = r.WithElement(0, Count(vector.GetElement(0)));
            r = r.WithElement(1, Count(vector.GetElement(1)));
            if (TVector.Length > 2) r = r.WithElement(2, Count(vector.GetElement(2)));
            if (TVector.Length > 3) r = r.WithElement(3, Count(vector.GetElement(3)));
            return TVector.FromUnderlying(r.AsByte());
        }
    }
}
