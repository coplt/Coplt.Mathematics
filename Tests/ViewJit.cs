using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;
using Coplt.Mathematics.Algebras.Generics.Dispatch;
using Coplt.Mathematics.Simd;

namespace Tests;

public static class ViewJit
{
    public static bool Some1(float a) => IsTrue(a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrue<TScalar>(TScalar value) where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        if (typeof(TScalar) == typeof(float))
        {
            return !Vector128.EqualsAny(Vector128.CreateScalar(value).AsUInt32(), Vector128.Create(0, -1, -1, -1).AsUInt32());
        }
        if (typeof(TScalar) == typeof(double))
        {
            return !Vector128.EqualsAny(Vector128.CreateScalar(value).AsUInt64(), Vector128.Create(0, -1).AsUInt64());
        }
        if (typeof(TScalar) == typeof(Half))
        {
            return Unsafe.BitCast<TScalar, ushort>(value) != 0;
        }
        return value != TScalar.Zero;
    }
}

public static class foo
{
    extension<T>(T self) where T : IAlgebraDispatch<T>
    {
        public T yyy1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => T.Self<impl_swizzle_yyy>(self);
        }
    }
}

public struct impl_swizzle_yyy : IAlgebraVisitor_T_T<impl_swizzle_yyy>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T<impl_swizzle_yyy>.Simd_Any<TVector, TScalar>(in Vector128<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector128.Shuffle(vector.AsInt32(), Vector128.Create(1, 1, 1, 3)).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T<impl_swizzle_yyy>.Simd_Any<TVector, TScalar>(in Vector256<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector256.Shuffle(vector.AsInt64(), Vector256.Create(1, 1, 1, 3)).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T<impl_swizzle_yyy>.Vector3_Any<TVector, TScalar>(in TVector vector)
        => TVector.Create(TVector.get_y(vector), TVector.get_y(vector), TVector.get_y(vector));
}
