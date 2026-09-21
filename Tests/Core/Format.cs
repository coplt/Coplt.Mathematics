using System.Globalization;
using System.Text;
using Coplt.Experimental.Mathematics;
using Coplt.Mathematics.Generics;
using B16 = Coplt.Mathematics.b16;
using B32 = Coplt.Mathematics.b32;
using B64 = Coplt.Mathematics.b64;

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
            Assert.That(new b32v2(B32.True, B32.False).ToString(), Is.EqualTo("(true, false)"));
            Assert.That(new b16v4(B16.True, B16.False, B16.True, B16.False).ToString(), Is.EqualTo("(true, false, true, false)"));
            Assert.That(new b64v3(B64.True, B64.False, B64.True).ToString(), Is.EqualTo("(true, false, true)"));
        }
    }

    [Test]
    public void FormatAndProvider()
    {
        var v = new float3(1, 2, 3);
        var i = new int3(10, 11, 12);
        var b = new b32v2(B32.True, B32.False);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.ToString("F2", CultureInfo.InvariantCulture), Is.EqualTo("(1.00, 2.00, 3.00)"));
            Assert.That(i.ToString("X4", CultureInfo.InvariantCulture), Is.EqualTo("(000A, 000B, 000C)"));
            Assert.That(((IFormattable)v).ToString("F1", CultureInfo.InvariantCulture), Is.EqualTo("(1.0, 2.0, 3.0)"));
            Assert.That(b.ToString("F2", CultureInfo.InvariantCulture), Is.EqualTo("(true, false)"));
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
        var b = new b64v2(B64.True, B64.False);
        var dst = new byte[64];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(v.TryFormat(dst, out var nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(Encoding.UTF8.GetString(dst.AsSpan(0, nc)), Is.EqualTo("(1, 2, 3)"));
            Assert.That(nc, Is.EqualTo(9));
            Assert.That(v.TryFormat(dst.AsSpan(0, 8), out nc, default, CultureInfo.InvariantCulture), Is.False);
            Assert.That(nc, Is.EqualTo(0));
            Assert.That(b.TryFormat(dst, out nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(Encoding.UTF8.GetString(dst.AsSpan(0, nc)), Is.EqualTo("(true, false)"));
        }
    }

    [Test]
    public void LegacyBool()
    {
        var chars = new char[8];
        var utf8 = new byte[8];
        var v = new Coplt.Mathematics.b32v2(B32.True, B32.False);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(B16.True.ToString(), Is.EqualTo("true"));
            Assert.That(B32.False.ToString(), Is.EqualTo("false"));
            Assert.That(B64.True.ToString("G", CultureInfo.InvariantCulture), Is.EqualTo("true"));
            Assert.That(B16.True.TryFormat(chars, out var nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(chars.AsSpan(0, nc).ToString(), Is.EqualTo("true"));
            Assert.That(B32.False.TryFormat(chars, out nc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(chars.AsSpan(0, nc).ToString(), Is.EqualTo("false"));
            Assert.That(B64.True.TryFormat(utf8, out var bc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(Encoding.UTF8.GetString(utf8.AsSpan(0, bc)), Is.EqualTo("true"));
            Assert.That(B32.False.TryFormat(utf8, out bc, default, CultureInfo.InvariantCulture), Is.True);
            Assert.That(Encoding.UTF8.GetString(utf8.AsSpan(0, bc)), Is.EqualTo("false"));
            // the span that holds the text exactly is the smallest one that is accepted
            Assert.That(B16.False.TryFormat(chars.AsSpan(0, 5), out nc, default, null), Is.True);
            Assert.That(B16.False.TryFormat(chars.AsSpan(0, 4), out nc, default, null), Is.False);
            Assert.That(nc, Is.EqualTo(0));
            // the legacy vectors print the text of their bool components
            Assert.That(v.ToString(), Is.EqualTo("b32v2(true, false)"));
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
        Check<b16v2, B16>(new b16v2(B16.True, B16.False), "(true, false)");
        Check<b32v4, B32>(new b32v4(B32.True, B32.False, B32.True, B32.False), "(true, false, true, false)");
        Check<b64v2, B64>(new b64v2(B64.True, B64.False), "(true, false)");
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
