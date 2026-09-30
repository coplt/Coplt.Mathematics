using System.Numerics;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// The checks that only use the members declared on the generic interfaces, so every generated vector can be
/// checked through the same code. The members that are not part of the interfaces are checked separately.
/// </summary>
internal static class GenericCheck
{
    /// <summary>
    /// Everything that is declared on <see cref="IVector{TSelf,TScalar}"/>.
    /// </summary>
    public static void Vector<T, TScalar>(int length, int sizeByte, bool simd, TScalar one, TScalar two)
        where T : unmanaged, IVector<T, TScalar>
        where TScalar : unmanaged
    {
        // Constants
        var zero = default(TScalar);

        // Meta
        Assert.That(T.Length, Is.EqualTo(length));
        Assert.That(T.SizeByte, Is.EqualTo(sizeByte));
        Assert.That(T.SizeBit, Is.EqualTo(sizeByte * 8));
        Assert.That(T.IsSimdAccelerated, Is.EqualTo(simd));

        // Ctors
        var broadcast = T.Broadcast(one);
        var firstOnly = T.Scalar(one);
        for (var i = 0; i < length; i++)
        {
            Assert.That(T.get(broadcast, i), Is.EqualTo(one), $"Broadcast component {i}");
            Assert.That(T.get(firstOnly, i), Is.EqualTo(i == 0 ? one : zero), $"Scalar component {i}");
        }

        // Index
        var v = T.Broadcast(one);
        for (var i = 0; i < length; i++)
        {
            Assert.That(T.get(v, i), Is.EqualTo(one), $"index get {i}");
            T.set(ref v, i, two);
            Assert.That(T.get(v, i), Is.EqualTo(two), $"index set {i}");
        }

        Assert.Throws<IndexOutOfRangeException>(() => _ = T.get(v, length), "index get out of range");
        Assert.Throws<IndexOutOfRangeException>(() => T.set(ref v, length, one), "index set out of range");

        // Load, the padding elements of a 3 component vector are not part of the vector
        var loadCount = simd && length == 3 ? 4 : length;
        var data = new TScalar[loadCount];
        for (var i = 0; i < loadCount; i++) data[i] = i < length ? one : two;
        var loaded = T.Load(data);
        for (var i = 0; i < length; i++) Assert.That(T.get(loaded, i), Is.EqualTo(one), $"Load component {i}");

        // IEquatable
        Assert.That(loaded.Equals(broadcast), Is.True);
        Assert.That(broadcast.Equals(T.Broadcast(one)), Is.True);
        Assert.That(broadcast.Equals(firstOnly), Is.False);
        Assert.That(broadcast.Equals((object)loaded), Is.True);
        Assert.That(broadcast.Equals(null), Is.False);
        Assert.That(loaded.GetHashCode(), Is.EqualTo(broadcast.GetHashCode()));

        // IBitwiseOperators, these identities have to hold for every element type
        var x = T.Broadcast(one);
        var y = T.Broadcast(two);
        Assert.That((x & default(T)).Equals(default(T)), Is.True, "& zero");
        Assert.That((x | default(T)).Equals(x), Is.True, "| zero");
        Assert.That((x ^ default(T)).Equals(x), Is.True, "^ zero");
        Assert.That((x & x).Equals(x), Is.True, "& self");
        Assert.That((x | x).Equals(x), Is.True, "| self");
        Assert.That((~~x).Equals(x), Is.True, "double not");
        Assert.That(((x ^ y) ^ y).Equals(x), Is.True, "xor inverse");
        Assert.That((x & y).Equals(y & x), Is.True, "& is commutative");
    }

    /// <summary>
    /// Everything that is added by <see cref="INumberVector{TSelf,TScalar}"/>, <paramref name="one"/> is the one
    /// value and <paramref name="two"/> the two value.
    /// </summary>
    public static void Number<T, TScalar>(int length, int sizeByte, bool simd, TScalar one, TScalar two)
        where T : unmanaged, INumberVector<T, TScalar>
        where TScalar : unmanaged, IBinaryNumber<TScalar>
    {
        Vector<T, TScalar>(length, sizeByte, simd, one, two);

        // Constants
        var zero = T.Zero;
        for (var i = 0; i < length; i++)
        {
            Assert.That(T.get(zero, i), Is.EqualTo(default(TScalar)), $"Zero component {i}");
            Assert.That(T.get(T.One, i), Is.EqualTo(one), $"One component {i}");
            Assert.That(T.get(T.Two, i), Is.EqualTo(two), $"Two component {i}");
        }

        Assert.That(T.ScalarZero, Is.EqualTo(default(TScalar)), "ScalarZero");
        Assert.That(T.ScalarOne, Is.EqualTo(one), "ScalarOne");
        Assert.That(T.ScalarTwo, Is.EqualTo(two), "ScalarTwo");

        var x = T.Broadcast(one);
        var y = T.Broadcast(two);

        // IComparable
        Assert.That(x.CompareTo(y), Is.EqualTo(-1));
        Assert.That(y.CompareTo(x), Is.EqualTo(1));
        Assert.That(x.CompareTo(T.Broadcast(one)), Is.EqualTo(0));
        Assert.That(((IComparable)x).CompareTo((object)y), Is.EqualTo(-1));
        Assert.That(((IComparable)x).CompareTo(null), Is.EqualTo(1));
        Assert.Throws<ArgumentException>(() => ((IComparable)x).CompareTo("not a vector"));

        // IComparisonOperators, the result of the interface is the mask of the comparison of every component
        void CheckMask(T mask, bool expected, string name)
        {
            for (var i = 0; i < length; i++)
            {
                Assert.That(T.get(mask, i) != default(TScalar), Is.EqualTo(expected), $"{name} component {i}");
            }
        }

        CheckMask(x == T.Broadcast(one), true, "==");
        CheckMask(x == y, false, "==");
        CheckMask(x != y, true, "!=");
        CheckMask(x < y, true, "<");
        CheckMask(y < x, false, "<");
        CheckMask(y > x, true, ">");
        CheckMask(x > y, false, ">");
        CheckMask(x <= T.Broadcast(one), true, "<=");
        CheckMask(y <= x, false, "<=");
        CheckMask(y >= x, true, ">=");
        CheckMask(x >= y, false, ">=");

        // the bool result of the comparison is the result of every component, it is the explicit implementation
        // of the interface of the framework of the value, which a value whose type is a type parameter that names
        // the interface of the algebra alone does not reach, see BoolComparison

        // IShiftOperators
        Assert.That((x << 1).Equals(x), Is.False, "<< changes the value");
        Assert.That((x << 1) >> 1, Is.EqualTo(x), "arithmetic shift round trip");
        Assert.That((x << 2) >>> 2, Is.EqualTo(x), "logical shift round trip");
        Assert.That(zero << 1, Is.EqualTo(zero));
        Assert.That(zero >> 1, Is.EqualTo(zero));
        Assert.That(zero >>> 1, Is.EqualTo(zero));
    }

    /// <summary>
    /// The bool result of the comparison of a number vector: the interface of the algebra of the value keeps the
    /// mask of the comparison of every component, the bool result of the whole value is the explicit implementation
    /// of the framework interface of it. A member whose type parameter names the framework interface alone reaches
    /// it, a member that also names the interface of the algebra does not, the two results of the comparison are
    /// ambiguous to it.
    /// </summary>
    public static void BoolComparison<T>(T one, T two)
        where T : unmanaged, IComparisonOperators<T, T, bool>
    {
        Assert.That(one == one, Is.True, "== self");
        Assert.That(one == two, Is.False, "== other");
        Assert.That(one != two, Is.True, "!= other");
        Assert.That(one < two, Is.True, "<");
        Assert.That(two < one, Is.False, "< reversed");
        Assert.That(two > one, Is.True, ">");
        Assert.That(one > two, Is.False, "> reversed");
        Assert.That(one <= one, Is.True, "<=");
        Assert.That(two <= one, Is.False, "<= reversed");
        Assert.That(two >= one, Is.True, ">=");
        Assert.That(one >= two, Is.False, ">= reversed");
    }
}
