using System;
using System.Collections.Generic;

namespace Coplt.Analyzers.Generators;

public record struct Typ(
    string name,
    string Type,
    int size,
    string suffix,
    string one,
    bool arith = false,
    bool f = false,
    bool i = false,
    bool simd = false,
    string shuffleCast = "",
    bool formattable = true,
    bool bitop = true,
    bool shift = true,
    bool sig = false,
    bool bin = true
)
{
    public string compType { get; set; } = name;
    public string simdComp { get; set; } = name;

    public string jsonType { get; set; } = Type;
    public string jsonCast { get; set; } = "";
    public string jsonCastBack { get; set; } = "";

    public string arithCast { get; set; } = "";

    public string two { get; set; } = $"({one} + {one})";
    public string half { get; set; } = "";

    public static string[] xyzw = { "x", "y", "z", "w" };
    public static string[] rgba = { "r", "g", "b", "a" };

    public string structSuffix = "";

    /// <summary>The type of the mask of a component, which is the unsigned type of the same width.</summary>
    public string maskType = "";

    /// <summary>The type of the signed mask of a component, which is the signed type of the same width.</summary>
    public string sigMaskType = "";

    /// <summary>The value of the sign bit of a component, which is the mask of the negative one.</summary>
    public string maskNeg = "";

    /// <summary>
    /// The names of the members of the converter of the framework that read the bits of a component of the kind
    /// beside the mask of the kind: the one that reads the bits of a component as the mask of the kind, the one
    /// that reads the mask of the kind as the bits of a component and the one that reads the bits of a component
    /// as the signed mask of the kind.
    /// </summary>
    public readonly (string ToMask, string FromMask, string ToSigMask) Bits => maskType switch
    {
        "uint" => ("SingleToUInt32Bits", "UInt32BitsToSingle", "SingleToInt32Bits"),
        "ulong" => ("DoubleToUInt64Bits", "UInt64BitsToDouble", "DoubleToInt64Bits"),
        "ushort" => ("HalfToUInt16Bits", "UInt16BitsToHalf", "HalfToInt16Bits"),
        _ => ("", "", ""),
    };

    public static Typ[] Typs =
    {
        new("float", nameof(Single), sizeof(float), "f", "1.0f", arith: true, sig: true, f: true, simd: true)
        {
            two = "2.0f",
            half = "0.5f",
            maskType = "uint",
            sigMaskType = "int",
            maskNeg = "0x80000000",
        },
        new("double", nameof(Double), sizeof(double), "", "1.0", arith: true, sig: true, f: true, simd: true)
        {
            two = "2.0",
            half = "0.5",
            structSuffix = "_d",
            maskType = "ulong",
            sigMaskType = "long",
            maskNeg = "0x8000000000000000",
        },
        new("short", nameof(Int16), sizeof(short), "", "(short)1", arith: true, sig: true, i: true) { arithCast = "(short)" },
        new("ushort", nameof(UInt16), sizeof(ushort), "", "(ushort)1", arith: true, i: true) { arithCast = "(ushort)" },
        new("int", nameof(Int32), sizeof(int), "", "1", arith: true, sig: true, i: true, simd: true),
        new("uint", nameof(UInt32), sizeof(uint), "u", "1u", arith: true, i: true, simd: true, shuffleCast: "(uint)"),
        new("long", nameof(Int64), sizeof(long), "L", "1L", arith: true, sig: true, i: true, simd: true),
        new("ulong", nameof(UInt64), sizeof(ulong), "UL", "1UL", arith: true, i: true, simd: true, shuffleCast: "(ulong)"),
        new("half", "Half", sizeof(ushort), "f.half()", "(half)1.0", arith: true, sig: true, f: true)
        {
            two = "(half)2.0f",
            half = "(half)0.5f",
            jsonCast = "(float)",
            jsonCastBack = "(half)",
            arithCast = "(half)",
            jsonType = "Single",
            structSuffix = "_h",
            maskType = "ushort",
            sigMaskType = "short",
            maskNeg = "0x8000",
        },
    };

    public static Dictionary<string, string[]> ExplicitConverts = new()
    {
        { "int", ["uint", "ulong", "half"] },
        { "uint", ["int", "half"] },
        { "long", ["uint", "int", "ulong", "half"] },
        { "ulong", ["uint", "int", "long", "half"] },
        { "float", ["uint", "int", "ulong", "long", "half"] },
        { "double", ["uint", "int", "ulong", "long", "float", "half"] },
        { "half", ["uint", "int", "ulong", "long"] },
    };
    public static Dictionary<string, string[]> ImplicitConverts = new()
    {
        { "int", ["long", "float", "double"] },
        { "uint", ["long", "ulong", "float", "double"] },
        { "long", ["double"] },
        { "ulong", ["double"] },
        { "float", ["double"] },
        { "half", ["float", "double"] },
    };
}
