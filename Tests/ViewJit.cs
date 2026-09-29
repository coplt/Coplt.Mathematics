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
    public static float3 Some1(in float3 a, in float3 b) => foo.min(a, b);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T min<T>(in T a, in T b) where T : IAlgebraDispatch<T>
        => T.Self<impl_min>(a, b);
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

public struct impl_abs : IAlgebraVisitor_T_T<impl_abs>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TScalar IAlgebraVisitor_S_S<impl_abs>.Scalar_Number<TScalar>(TScalar value)
        => TScalar.Abs(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T<impl_abs>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector128.Abs(vector).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T<impl_abs>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> vector)
        => TVector.UnsafeFromUnderlying(Vector256.Abs(vector).AsByte());
}

public struct impl_min : IAlgebraVisitor_T_T_T<impl_min>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TScalar IAlgebraVisitor_S_S_S<impl_min>.Scalar_Number<TScalar>(TScalar a, TScalar b)
        => TScalar.Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T_T<impl_min>.Simd_Number<TVector, TScalar>(in Vector128<TScalar> a, in Vector128<TScalar> b)
        => TVector.UnsafeFromUnderlying(Vector128.Min(a, b).AsByte());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TVector IAlgebraVisitor_T_T_T<impl_min>.Simd_Number<TVector, TScalar>(in Vector256<TScalar> a, in Vector256<TScalar> b)
        => TVector.UnsafeFromUnderlying(Vector256.Min(a, b).AsByte());
}
