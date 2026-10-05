namespace Coplt.Mathematics;

public static partial class math
{
    /// <summary>
    /// Returns the smallest magnitude the member of the safe rotation reads of the kind of a floating point
    /// component, which is <c>1e-35</c> of a single precision component, <c>1e-290</c> of a double precision one and
    /// <c>1e-5</c> of a half precision one
    /// <para>A length of a value that is below the value of this member or above the one of
    /// <see cref="MaxRotateSafe{T}"/> is one the member of the safe rotation cannot read, so the member reaches the
    /// identity of the matrix instead of the rotation.</para>
    /// </summary>
    /// <typeparam name="T">The kind of the component of the value, which is a floating point kind</typeparam>
    /// <returns>The smallest magnitude the member of the safe rotation reads</returns>
    [MethodImpl(256 | 512)]
    internal static T MinRotateSafe<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return Unsafe.BitCast<float, T>(1e-35f);
        if (typeof(T) == typeof(double)) return Unsafe.BitCast<double, T>(1e-290);
        if (typeof(T) == typeof(half)) return Unsafe.BitCast<half, T>((half)1e-5f);
        throw new NotSupportedException();
    }

    /// <summary>
    /// Returns the largest magnitude the member of the safe rotation reads of the kind of a floating point
    /// component, which is <c>1e35</c> of a single precision component, <c>1e290</c> of a double precision one and
    /// <c>1e5</c> of a half precision one
    /// <para>A length of a value that is above the value of this member or below the one of
    /// <see cref="MinRotateSafe{T}"/> is one the member of the safe rotation cannot read, so the member reaches the
    /// identity of the matrix instead of the rotation.</para>
    /// </summary>
    /// <typeparam name="T">The kind of the component of the value, which is a floating point kind</typeparam>
    /// <returns>The largest magnitude the member of the safe rotation reads</returns>
    [MethodImpl(256 | 512)]
    internal static T MaxRotateSafe<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return Unsafe.BitCast<float, T>(1e35f);
        if (typeof(T) == typeof(double)) return Unsafe.BitCast<double, T>(1e290);
        if (typeof(T) == typeof(half)) return Unsafe.BitCast<half, T>((half)1e5f);
        throw new NotSupportedException();
    }
}
