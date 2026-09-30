using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Algebras.Generics;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the refraction direction of the incident vector <paramref name="i"/>, which has to be
        /// normalized, and the normal <paramref name="n"/>, which has to point against it
        /// <para>The direction is undefined when the root of the factor of the refraction is not real, which
        /// happens when the incident direction is grazing enough, and the member answers with the zero value of
        /// the kind of it there</para>
        /// <para>The factor of the refraction is the value of a single component, so the member is built out of
        /// the members of the algebra of the kind of the value: the dot product of the two vectors, which the
        /// value itself reaches, and the fused multiply and add of the kind of a component</para>
        /// </summary>
        /// <remarks>
        /// The direction is the incident vector scaled by the index of refraction less the normal scaled by the
        /// sum of the index of refraction and the root of the factor of the refraction:
        /// <code>
        /// eta * i - (eta * dot(n, i) + sqrt(1 - eta * eta * (1 - dot(n, i) * dot(n, i)))) * n
        /// </code>
        /// <para>The member answers with the zero of the kind of the value when the factor of the refraction is
        /// negative, which is the case the root of it has no real value in.</para>
        /// <para>It is the <c>refract</c> intrinsic of hlsl, which is the direction that the incident vector is
        /// refracted into as well.</para>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-refract"/></para>
        /// </remarks>
        /// <param name="i">The normalized vector of the incoming direction</param>
        /// <param name="n">The normalized normal, it has to point against <paramref name="i"/></param>
        /// <param name="index_of_refraction">The ratio between the index of refraction of the two materials</param>
        /// <typeparam name="T">The type of the vectors</typeparam>
        /// <typeparam name="TScalar">The type of a single component</typeparam>
        /// <returns>The refracted direction</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T refract<T, TScalar>(in T i, in T n, TScalar index_of_refraction)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
        {
            var ni = dot<T, TScalar>(n, i);
            var k = fsm(TScalar.One, index_of_refraction * index_of_refraction, fnma(ni, ni, TScalar.One));
            if (k < TScalar.Zero) return default;
            return fms(
                T.Broadcast(index_of_refraction), i,
                n * fma(index_of_refraction, ni, TScalar.Sqrt(k))
            );
        }
    }

    public static partial class math_ex
    {
        /// <inheritdoc cref="math.refract{T, TScalar}"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T refract<T, TScalar>(this T i, in T n, TScalar index_of_refraction)
            where T : unmanaged, IAlgebraDispatch<T, TScalar>, IFloatingPointVector<T, TScalar>
            where TScalar : unmanaged, IBinaryFloatingPointIeee754<TScalar>
            => math.refract(i, n, index_of_refraction);
    }
}
