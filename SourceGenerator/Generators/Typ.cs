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
    bool bin = true,
    bool bol = false
)
{
    public string compType { get; set; } = name;
    public string simdComp { get; set; } = name;
    public string maskType { get; set; } = "";
    public string sigMaskType { get; set; } = "";

    public string jsonType { get; set; } = Type;
    public string jsonCast { get; set; } = "";
    public string jsonCastBack { get; set; } = "";

    public string maskNeg { get; set; } = "";
    public string maskPos { get; set; } = "";
    public string maskAll { get; set; } = "";

    public string arithCast { get; set; } = "";

    public string two { get; set; } = $"({one} + {one})";
    public string half { get; set; } = "";

    public static string[] xyzw = { "x", "y", "z", "w" };
    public static string[] rgba = { "r", "g", "b", "a" };

    public string structSuffix = "";

    public static Typ[] Typs =
    {
        new("float", nameof(Single), sizeof(float), "f", "1.0f", arith: true, sig: true, f: true, simd: true)
        {
            two = "2.0f",
            half = "0.5f",
            maskType = "uint",
            sigMaskType = "int",
            maskNeg = "0x80000000",
            maskAll = "0xFFFFFFFF"
        },
        new("double", nameof(Double), sizeof(double), "", "1.0", arith: true, sig: true, f: true, simd: true)
        {
            two = "2.0",
            half = "0.5",
            maskType = "ulong",
            sigMaskType = "long",
            maskNeg = "0x8000000000000000",
            maskAll = "0xFFFFFFFFFFFFFFFF",
            structSuffix = "_d",
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
            maskType = "ushort",
            sigMaskType = "short",
            maskNeg = "0x8000",
            maskAll = "0xFFFF",
            structSuffix = "_h",
        },
        new("b16v", nameof(UInt16), sizeof(ushort), "", "true", shift: false, bol: true) { compType = "b16", jsonType = "Boolean" },
        new("b32v", nameof(UInt32), sizeof(uint), "", "true", simd: true, shuffleCast: "(uint)", shift: false, bol: true)
        {
            compType = "b32",
            simdComp = "uint",
            jsonType = "Boolean"
        },
        new("b64v", nameof(UInt64), sizeof(ulong), "", "true", simd: true, shuffleCast: "(ulong)", shift: false, bol: true)
        {
            compType = "b64",
            simdComp = "ulong",
            jsonType = "Boolean"
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
        { "b16v", ["uint", "int", "ulong", "long", "float", "double", "half", "b32v", "b64v"] },
        { "b32v", ["uint", "int", "ulong", "long", "float", "double", "half", "b16v", "b64v"] },
        { "b64v", ["uint", "int", "ulong", "long", "float", "double", "half", "b16v", "b32v"] },
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

    /// <summary>
    /// The scalar types that convert to a vector of a component type implicitly beside the type of the component
    /// itself, the value of one of them is broadcast into every component of the vector. The type of the
    /// component of the vector is not one of them, the conversion of it is always emitted.
    /// <para>The compiler applies a single user defined conversion, so a scalar that converts to the type of a
    /// component <c>float</c> needs an operator of its own as well, the two conversions of it would not chain
    /// otherwise</para>
    /// </summary>
    public static Dictionary<string, string[]> ScalarConverts = new()
    {
        { "float", ["int", "uint"] },
        { "double", ["long", "ulong"] },
    };
}
