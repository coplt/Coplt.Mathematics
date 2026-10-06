using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Implements;

namespace Coplt.Mathematics
{
    public static partial class math
    {
        /// <summary>
        /// Gets the location of the first set bit starting from the highest order bit and working downward at
        /// every component of <paramref name="value"/>
        /// <para>The location is a zero based count of the bits, which starts at the highest order bit of the kind
        /// of a component, so the value of a component that has no sign and has a bit set is the one of the member
        /// of the library that counts the zero bits at the front of the value of it, the
        /// <see cref="math.lzcnt{T}(T)"/> of it, and the one of a component of a signed kind that is negative is
        /// the one of the first bit of the value of it that is zero, which the search of the function of the
        /// standard reaches for a negative value.</para>
        /// <para>The value of the result of a component that has no such bit is the all bits set value of the kind
        /// of it, which is the minus one of a signed kind: the function of the standard returns the mask of every
        /// bit set for it, so the result of it is not the whole width of the kind like the one of the member of
        /// the library is.</para>
        /// </summary>
        /// <param name="value">The value whose every component is queried</param>
        /// <typeparam name="T">The type of the value, which is a vector or a matrix</typeparam>
        /// <returns>The value whose every component is the location of the first set bit of the component</returns>
        /// <remarks>
        /// <para>The function of the standard searches the bits of a component from the highest order bit of the
        /// kind of it downward, so the bit it answers the location of is the first bit of the value of it that is
        /// zero when the value of it is negative, which is the bit the sign of it differs from:</para>
        /// <code>
        /// bits = value &lt; 0 ? ~value : value
        /// firstbithigh(value) = bits == 0 ? all_bits_set : lzcnt(bits)
        /// </code>
        /// <para>The all bits set value of the kind is the mask that the comparison of the value of a component with
        /// the zero of the kind answers, which is the minus one of a signed kind, so the member of the library that
        /// counts the zero bits at the front of the value of it answers the location of every other value:</para>
        /// <code>
        /// firstbithigh(value) = lzcnt(bits) | (bits == 0)
        /// </code>
        /// <para><see href="https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/firstbithigh"/></para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T firstbithigh<T>(T value) where T : unmanaged, IAlgebraDispatch<T>, IIntegerAlgebra<T>
        {
            // the count of the zero bits at the front of the complement of the value of a component that is
            // negative is the location of the first bit of it that is zero, which the search of the standard
            // reaches for a negative value, and the value of a component of a kind that has no sign is reached as
            // it is, because no value of such a component is negative
            var negative = value < T.Zero;
            var bits = (~value & negative) | (value & ~negative);
            return lzcnt(bits) | (bits == T.Zero);
        }
    }
}
