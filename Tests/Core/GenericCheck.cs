using System.Numerics;
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
            Assert.That(T.get_at(broadcast, i), Is.EqualTo(one), $"Broadcast component {i}");
            Assert.That(T.get_at(firstOnly, i), Is.EqualTo(i == 0 ? one : zero), $"Scalar component {i}");
        }

        // Index
        var v = T.Broadcast(one);
        for (var i = 0; i < length; i++)
        {
            Assert.That(T.get_at(v, i), Is.EqualTo(one), $"index get {i}");
            T.set_at(ref v, i, two);
            Assert.That(T.get_at(v, i), Is.EqualTo(two), $"index set {i}");
        }

        Assert.Throws<IndexOutOfRangeException>(() => _ = T.get_at(v, length), "index get out of range");
        Assert.Throws<IndexOutOfRangeException>(() => T.set_at(ref v, length, one), "index set out of range");

        // Load, the padding elements of a 3 component vector are not part of the vector
        var loadCount = simd && length == 3 ? 4 : length;
        var data = new TScalar[loadCount];
        for (var i = 0; i < loadCount; i++) data[i] = i < length ? one : two;
        var loaded = T.Load(data);
        for (var i = 0; i < length; i++) Assert.That(T.get_at(loaded, i), Is.EqualTo(one), $"Load component {i}");

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
        where TScalar : unmanaged
    {
        Vector<T, TScalar>(length, sizeByte, simd, one, two);

        // Constants
        var zero = T.Zero;
        for (var i = 0; i < length; i++)
        {
            Assert.That(T.get_at(zero, i), Is.EqualTo(default(TScalar)), $"Zero component {i}");
            Assert.That(T.get_at(T.One, i), Is.EqualTo(one), $"One component {i}");
            Assert.That(T.get_at(T.Two, i), Is.EqualTo(two), $"Two component {i}");
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

        // IComparisonOperators, the interface result is the all components result
        Assert.That(x == T.Broadcast(one), Is.True);
        Assert.That(x == y, Is.False);
        Assert.That(x != y, Is.True);
        Assert.That(x < y, Is.True);
        Assert.That(y < x, Is.False);
        Assert.That(y > x, Is.True);
        Assert.That(x > y, Is.False);
        Assert.That(x <= T.Broadcast(one), Is.True);
        Assert.That(y <= x, Is.False);
        Assert.That(y >= x, Is.True);
        Assert.That(x >= y, Is.False);

        // IShiftOperators
        Assert.That((x << 1).Equals(x), Is.False, "<< changes the value");
        Assert.That((x << 1) >> 1, Is.EqualTo(x), "arithmetic shift round trip");
        Assert.That((x << 2) >>> 2, Is.EqualTo(x), "logical shift round trip");
        Assert.That(zero << 1, Is.EqualTo(zero));
        Assert.That(zero >> 1, Is.EqualTo(zero));
        Assert.That(zero >>> 1, Is.EqualTo(zero));
    }

    /// <summary>
    /// Everything that is added by <see cref="IBoolVector{TSelf,TScalar}"/>, <paramref name="one"/> is the true
    /// mask component and <paramref name="two"/> another true mask component.
    /// </summary>
    public static void Bool<T, TScalar>(int length, int sizeByte, bool simd, TScalar one, TScalar two)
        where T : unmanaged, IBoolVector<T, TScalar>
        where TScalar : unmanaged
    {
        Vector<T, TScalar>(length, sizeByte, simd, one, two);

        var allTrue = T.True;
        var allFalse = T.False;
        for (var i = 0; i < length; i++)
        {
            Assert.That(T.get_at(allTrue, i), Is.EqualTo(one), $"True component {i}");
            Assert.That(T.get_at(allFalse, i), Is.EqualTo(default(TScalar)), $"False component {i}");
        }

        Assert.That(allFalse.Equals(default(T)), Is.True);
        Assert.That(allTrue.Equals(allFalse), Is.False);
        Assert.That(allTrue.Equals(T.Broadcast(one)), Is.True);
    }

    /// <summary>
    /// <see cref="IComparisonOperators{TSelf,TOther,TResult}"/> with the bool vector result is implemented on the
    /// vector itself, it is not part of the vector interfaces. Only one comparison result type may be in scope,
    /// otherwise the operators are ambiguous, so this helper is not constrained to the vector interfaces.
    /// </summary>
    public static void Mask<T, TBool, TBoolScalar>(T x, T y)
        where T : unmanaged, IComparisonOperators<T, T, TBool>
        where TBool : unmanaged, IBoolVector<TBool, TBoolScalar>
        where TBoolScalar : unmanaged
    {
        var allTrue = TBool.True;
        var allFalse = TBool.False;
        var same = x;

        Assert.That(x < y, Is.TypeOf<TBool>());
        Assert.That(x > y, Is.TypeOf<TBool>());
        Assert.That(x == y, Is.TypeOf<TBool>());
        Assert.That(x != y, Is.TypeOf<TBool>());
        Assert.That(x <= y, Is.TypeOf<TBool>());
        Assert.That(x >= y, Is.TypeOf<TBool>());

        for (var i = 0; i < TBool.Length; i++)
        {
            Assert.That(TBool.get_at((x < y), i), Is.EqualTo(TBool.get_at(allTrue, i)), $"< component {i}");
            Assert.That(TBool.get_at((x > y), i), Is.EqualTo(TBool.get_at(allFalse, i)), $"> component {i}");
            Assert.That(TBool.get_at((x <= y), i), Is.EqualTo(TBool.get_at(allTrue, i)), $"<= component {i}");
            Assert.That(TBool.get_at((x >= y), i), Is.EqualTo(TBool.get_at(allFalse, i)), $">= component {i}");
            Assert.That(TBool.get_at((x == same), i), Is.EqualTo(TBool.get_at(allTrue, i)), $"== component {i} of itself");
            Assert.That(TBool.get_at((x == y), i), Is.EqualTo(TBool.get_at(allFalse, i)), $"== component {i}");
            Assert.That(TBool.get_at((x != y), i), Is.EqualTo(TBool.get_at(allTrue, i)), $"!= component {i}");
        }
    }

    /// <summary>
    /// A bool vector also implements <see cref="IComparisonOperators{TSelf,TOther,TResult}"/> with itself, the
    /// masks are compared as unsigned integers. The equality operators are ambiguous here, the value of
    /// <c>==</c> is checked by the vector interfaces.
    /// </summary>
    public static void BoolMask<TBool, TBoolScalar>()
        where TBool : unmanaged, IBoolVector<TBool, TBoolScalar>, IComparisonOperators<TBool, TBool, TBool>
        where TBoolScalar : unmanaged
    {
        var allTrue = TBool.True;
        var allFalse = TBool.False;

        Assert.That(allFalse < allTrue, Is.TypeOf<TBool>());
        Assert.That(allTrue < allFalse, Is.TypeOf<TBool>());

        for (var i = 0; i < TBool.Length; i++)
        {
            Assert.That(TBool.get_at((allFalse < allTrue), i), Is.EqualTo(TBool.get_at(allTrue, i)), $"< component {i}");
            Assert.That(TBool.get_at((allTrue < allFalse), i), Is.EqualTo(TBool.get_at(allFalse, i)), $"< component {i}");
            Assert.That(TBool.get_at((allFalse <= allTrue), i), Is.EqualTo(TBool.get_at(allTrue, i)), $"<= component {i}");
            Assert.That(TBool.get_at((allTrue >= allFalse), i), Is.EqualTo(TBool.get_at(allTrue, i)), $">= component {i}");
        }
    }
}
