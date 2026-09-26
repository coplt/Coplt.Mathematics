namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Selects between the two values: it returns <paramref name="t"/> when the condition is true and
    /// <paramref name="f"/> when it is false
    /// </summary>
    /// <typeparam name="T">The type of the values to select</typeparam>
    /// <param name="c">The condition</param>
    /// <param name="t">The value that is selected when the condition is true</param>
    /// <param name="f">The value that is selected when the condition is false</param>
    /// <returns>The value of <paramref name="t"/> when <paramref name="c"/> is true and the one of
    /// <paramref name="f"/> when it is false</returns>
    [MethodImpl(256)]
    public static T select<T>(bool c, in T t, in T f) => c ? t : f;
}
