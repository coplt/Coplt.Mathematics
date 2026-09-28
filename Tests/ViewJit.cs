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
    public static float3 Some1(in float3 a) => foo.abs(a);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T abs<T>(in T a) where T : IAlgebraDispatch<T>
        => T.Self<impl_abs>(a);
}

public struct impl_swizzle_yyy : IAlgebraDispatch_Self_Self<impl_swizzle_yyy>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraDispatch_Self_Self<impl_swizzle_yyy>.Simd_Any<TVector, TScalar>(in Vector128<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector128.Shuffle(vector.AsInt32(), Vector128.Create(1, 1, 1, 3)).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraDispatch_Self_Self<impl_swizzle_yyy>.Simd_Any<TVector, TScalar>(in Vector256<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector256.Shuffle(vector.AsInt64(), Vector256.Create(1, 1, 1, 3)).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraDispatch_Self_Self<impl_swizzle_yyy>.Vector3_Any<TVector, TScalar>(in TVector vector)
        => TVector.Create(TVector.get_y(vector), TVector.get_y(vector), TVector.get_y(vector));
}

public struct impl_abs : IAlgebraDispatch_Self_Self<impl_abs>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TScalar IAlgebraDispatch_Scalar_Scalar<impl_abs>.Scalar_Number<TScalar>(TScalar value)
        => TScalar.Abs(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraDispatch_Self_Self<impl_abs>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector128.Abs(vector).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraDispatch_Self_Self<impl_abs>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector256.Abs(vector).AsByte());
}
