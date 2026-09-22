using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Selects between the two vectors component by component: the component of <paramref name="t"/> where the
    /// mask is true and the component of <paramref name="f"/> where it is false
    /// </summary>
    /// <typeparam name="T">The type of the vectors to select, the two of them have it</typeparam>
    /// <typeparam name="B">The type of the mask, the bool vector that has the same shape as the vectors</typeparam>
    /// <param name="c">The mask</param>
    /// <param name="t">The vector that the true components are taken from</param>
    /// <param name="f">The vector that the false components are taken from</param>
    /// <returns>The vector of <typeparamref name="T"/> that has the component of <paramref name="t"/> where
    /// <paramref name="c"/> is true and the one of <paramref name="f"/> where it is false</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T select<T, B>(in B c, in T t, in T f) where T : unmanaged, IVectorSelect<T, B>
        => T.select(c, t, f);
}
