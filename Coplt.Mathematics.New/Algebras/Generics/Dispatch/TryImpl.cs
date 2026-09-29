using Coplt.Mathematics.Algebras.Generics.Dispatch;

namespace Coplt.Mathematics;

public partial struct float3 : IAlgebraDispatch<float3, float>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float3 IAlgebraDispatch<float3>.Self<V>(in float3 self) => V.Simd_Float<float3, float>(self.vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float3 IAlgebraDispatch<float3>.Self<V>(in float3 a, in float3 b) => V.Simd_Float<float3, float>(a.vector, b.vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float3 IAlgebraDispatch<float3>.Self<V>(in float3 a, in float3 b, in float3 c) => V.Simd_Float<float3, float>(a.vector, b.vector, c.vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float IAlgebraDispatch<float3, float>.Scalar<V>(float scalar) => V.Scalar_Float(scalar);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float IAlgebraDispatch<float3, float>.Scalar<V>(float a, float b) => V.Scalar_Float(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float IAlgebraDispatch<float3, float>.Scalar<V>(float a, float b, float c) => V.Scalar_Float(a, b, c);
}
