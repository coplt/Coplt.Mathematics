using Coplt.Mathematics.Generics;

namespace Coplt.Mathematics;

// The floating point members of the vectors are members of the vector itself, a member of the math class reaches
// them as well. The parameters of every member below are the ones of the interface of its operation in the same
// order, which is the order of the hlsl counterpart of the operation as well. None of the members below needs the
// type of a single component, so they do not have to name it: the compiler infers the vector type from the
// argument.
// The constants of the vector (E, PI, Tau, ...) are not forwarded, a property cannot be a member of the math
// class: float3.PI reaches the one of a known type and T.PI the one of a generic type.
public static partial class math
{
    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <returns>The wrapped vector</returns>
    [MethodImpl(256)]
    public static T wrap<T>(in T a, in T min, in T max) where T : unmanaged, IVectorFloatingPoint<T> => T.wrap(a, min, max);

    /// <summary>
    /// Wraps every component into the range of <paramref name="min"/> and <paramref name="max"/>, the bounds are
    /// the same for every component
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="min">The lower bound of every component</param>
    /// <param name="max">The upper bound of every component</param>
    /// <typeparam name="T">The type of the vector</typeparam>
    /// <typeparam name="TScalar">The type of a single component</typeparam>
    /// <returns>The wrapped vector</returns>
    [MethodImpl(256)]
    public static T wrap<T, TScalar>(in T a, TScalar min, TScalar max)
        where T : unmanaged, IVectorFloatingPoint<T, TScalar>
        where TScalar : unmanaged => T.wrap(a, min, max);
}
