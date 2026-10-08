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

    /// <summary>
    /// Returns the squared length of the axis of the rotation of two values of the length one below which the two
    /// of them are collinear for the kind of a floating point component, which is <c>1e-12</c> of a single
    /// precision component, <c>1e-30</c> of a double precision one and <c>1e-5</c> of a half precision one
    /// <para>The axis of the rotation of two values of the length one is the product of the two of them and the
    /// sine of the angle between them, so the direction of it is one the kind of a component cannot read where the
    /// squared length of it is not above the value of this member, which is what the squared length of it is where
    /// the two values of it are collinear as far as the kind reads the two of them</para>
    /// </summary>
    /// <typeparam name="T">The kind of the component of the value, which is a floating point kind</typeparam>
    /// <returns>The smallest squared length of the axis of a rotation the kind of the component reads</returns>
    [MethodImpl(256 | 512)]
    internal static T MinRotateCollinearSq<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(float)) return Unsafe.BitCast<float, T>(1e-12f);
        if (typeof(T) == typeof(double)) return Unsafe.BitCast<double, T>(1e-30);
        if (typeof(T) == typeof(half)) return Unsafe.BitCast<half, T>((half)1e-5f);
        throw new NotSupportedException();
    }
}
