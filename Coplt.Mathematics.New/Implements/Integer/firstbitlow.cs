using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Returns the location of the first set bit starting from the lowest order bit and working upward at
        /// every component of <paramref name="value"/>
        /// <para>The location is a zero based count of the bits, which starts at the lowest order bit of the kind
        /// of a component, so the value of a component that has a bit set is the one of the member of the library
        /// that counts the zero bits at the back of the value of it, the <see cref="math.tzcnt{T}(T)"/> of it.</para>
        /// <para>The value of the result of a component that has no bit set is the all bits set value of the kind
        /// of it, which is the minus one of a signed kind: the function of the standard returns the mask of every
        /// bit set for it, so the result of it is not the whole width of the kind like the one of the member of
        /// the library is.</para>
        /// </summary>
        /// <param name="value">The value whose every component is queried</param>
        /// <typeparam name="T">The type of the value, which is a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the location of the first set bit of the component</returns>
        /// <remarks>
        /// <para>The function of the standard is the one of the library that counts the zero bits at the back of
        /// the value of a component, which answers the all bits set value of the kind for the value that has no bit
        /// set, and the all bits set value of the kind is the mask that the comparison of the value of a component
        /// with the zero of the kind answers:</para>
        /// <code>
        /// firstbitlow(value) = tzcnt(value) | (value == 0)
        /// </code>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/firstbitlow"/></para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T firstbitlow<T>(T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
            => tzcnt(value) | (value == T.Zero);
    }
}
