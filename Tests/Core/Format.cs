using System.Globalization;
using System.Text;
using Coplt.Mathematics;
using Coplt.Mathematics.Algebras;
using Coplt.Mathematics.Generics;

namespace Tests.Core;

/// <summary>
/// Checks the formatting members of the generated vectors, the text is the list of the components between
/// parentheses and the name of the type is not a part of it.
/// </summary>
public class TestVectorFormat
{
    [Test]
    public void Text()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2(1, 2).ToString(), Is.EqualTo("(1, 2)"));
            Assert.That(new float3(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new float4(1, 2, 3, 4).ToString(), Is.EqualTo("(1, 2, 3, 4)"));
            Assert.That(new double3(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new int4(1, 2, 3, 4).ToString(), Is.EqualTo("(1, 2, 3, 4)"));
            Assert.That(new uint2(1, 2).ToString(), Is.EqualTo("(1, 2)"));
            Assert.That(new long3(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new half2((Half)1, (Half)2).ToString(), Is.EqualTo("(1, 2)"));
        }
    }

    [Test]
    public void FormatAndProvider()
    {
        var v = new float3(1, 2, 3);
        var i = new int3(10, 11, 12);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.ToString("F2", CultureInfo.InvariantCulture), Is.EqualTo("(1.00, 2.00, 3.00)"));
            Assert.That(i.ToString("X4", CultureInfo.InvariantCulture), Is.EqualTo("(000A, 000B, 000C)"));
            Assert.That(((IFormattable)v).ToString("F1", CultureInfo.InvariantCulture), Is.EqualTo("(1.0, 2.0, 3.0)"));
        }
    }

    [Test]
    public void StorageVariant()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(new float2s(1, 2).ToString(), Is.EqualTo("(1, 2)"));
            Assert.That(new float3s(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new double3s(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new int2s(1, 2).ToString(), Is.EqualTo("(1, 2)"));
            Assert.That(new uint3s(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new long3s(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new ulong3s(1, 2, 3).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(new float3s(1, 2, 3).ToString("F2", CultureInfo.InvariantCulture), Is.EqualTo("(1.00, 2.00, 3.00)"));
        }
    }

    [Test]
    public void TryFormatChar()
    {
        var v = new float3(1, 2, 3);
        var dst = new char[64];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.TryFormat(dst, out var nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(dst.AsSpan(0, nc).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(nc, Is.EqualTo(9));
            // the span that holds the text exactly is the smallest one that is accepted
            Assert.That(v.TryFormat(dst.AsSpan(0, 9), out nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(dst.AsSpan(0, 9).ToString(), Is.EqualTo("(1, 2, 3)"));
            Assert.That(v.TryFormat(dst.AsSpan(0, 8), out nc, default, CultureInfo.InvariantCulture), Is.False);
            Assert.That(nc, Is.EqualTo(0));
        }
    }

    [Test]
    public void TryFormatUtf8()
    {
        var v = new float3(1, 2, 3);
        var dst = new byte[64];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.TryFormat(dst, out var nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(Encoding.UTF8.GetString(dst.AsSpan(0, nc)), Is.EqualTo("(1, 2, 3)"));
            Assert.That(nc, Is.EqualTo(9));
            Assert.That(v.TryFormat(dst.AsSpan(0, 8), out nc, default, CultureInfo.InvariantCulture), Is.False);
            Assert.That(nc, Is.EqualTo(0));
        }
    }

    [Test]
    public void EveryVector()
    {
        Check<float4, float>(new float4(1, 2, 3, 4), "(1, 2, 3, 4)");
        Check<double2, double>(new double2(1, 2), "(1, 2)");
        Check<half3, Half>(new half3((Half)1, (Half)2, (Half)3), "(1, 2, 3)");
        Check<short2, short>(new short2(1, 2), "(1, 2)");
        Check<ushort4, ushort>(new ushort4(1, 2, 3, 4), "(1, 2, 3, 4)");
        Check<int3, int>(new int3(1, 2, 3), "(1, 2, 3)");
        Check<uint2, uint>(new uint2(1, 2), "(1, 2)");
        Check<long3, long>(new long3(1, 2, 3), "(1, 2, 3)");
        Check<ulong3, ulong>(new ulong3(1, 2, 3), "(1, 2, 3)");
        Check<float2s, float>(new float2s(1, 2), "(1, 2)");
        Check<int3s, int>(new int3s(1, 2, 3), "(1, 2, 3)");
        Check<double3s, double>(new double3s(1, 2, 3), "(1, 2, 3)");
        Check<long3s, long>(new long3s(1, 2, 3), "(1, 2, 3)");
    }

    /// <summary>
    /// Formats the vector with every member of the formatting interfaces, the type parameters of a call cannot
    /// be inferred from its constraints so they are written out.
    /// </summary>
    private static void Check<T, TScalar>(T v, string expected)
        where T : unmanaged, IVector<T, TScalar>, ISpanFormattable, IUtf8SpanFormattable
        where TScalar : unmanaged
    {
        var chars = new char[64];
        var utf8 = new byte[64];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.ToString(), Is.EqualTo(expected));
            Assert.That(v.TryFormat(chars, out var nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(chars.AsSpan(0, nc).ToString(), Is.EqualTo(expected));
            Assert.That(v.TryFormat(utf8, out var bc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(Encoding.UTF8.GetString(utf8.AsSpan(0, bc)), Is.EqualTo(expected));
        }
    }
}
