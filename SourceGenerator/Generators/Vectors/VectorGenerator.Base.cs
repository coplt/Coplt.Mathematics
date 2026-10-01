using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// The name and the value of every math constant of the kind of a floating point number, the value is the
    /// literal of a double, the literal of the component type of a vector is built from it. The values are the
    /// ones of <see cref="SourceGenerator.MathConstants"/>.
    /// </summary>
    internal static readonly (string Name, string Value)[] FloatConsts =
    {
        ("E", SourceGenerator.MathConstants.E),
        ("Log2", SourceGenerator.MathConstants.Log2),
        ("Log10", SourceGenerator.MathConstants.Log10),
        ("PI", SourceGenerator.MathConstants.Pi),
        ("Tau", SourceGenerator.MathConstants.Tau),
        ("RadToDeg", SourceGenerator.MathConstants.RadToDeg),
        ("DegToRad", SourceGenerator.MathConstants.DegToRad),
    };

    /// <summary>
    /// The name of the constant of the denominator of a quotient of the kind of a floating point value. Its value
    /// is not a constant of the ieee 754 standard and it is not the same for every kind of it, so it is not one of
    /// the values of <see cref="FloatConsts"/>, the literal of it comes from
    /// <see cref="VectorGenShared.DenomEpsilonValue"/>.
    /// </summary>
    internal const string DenomEpsilonName = "DenomEpsilon";

    /// <summary>
    /// The name of the sign mask of the kind of a floating point value: the sign of the kind of the value set in
    /// every component of it and no other bit of it. It is the negative zero of the kind of the component, which
    /// is a constant of the ieee 754 standard as well, but the member is the mask the members that reach the sign
    /// of a value use and not the value of the standard.
    /// </summary>
    internal const string SignMaskName = "SignMask";

    /// <summary>
    /// The name of the constant of the smallest positive normal value of the ieee 754 standard. The component type
    /// of a vector does not carry it, its value is written out, see <see cref="VectorGenShared.MinNormalValue"/>.
    /// </summary>
    internal const string MinNormalName = "MinNormal";

    /// <summary>
    /// The name of every constant of the ieee 754 standard, the value of one of them is the member of the scalar
    /// type of the component of a vector of the same name. The smallest positive normal value is the only one of
    /// them whose value is not held by a member of the kind of it.
    /// </summary>
    internal static readonly string[] IeeeConsts =
    {
        "Epsilon",
        MinNormalName,
        "NaN",
        "NegativeInfinity",
        "NegativeZero",
        "PositiveInfinity",
    };

    /// <summary>
    /// Generates the base members of the vector described by <paramref name="typ"/>: the meta data, the
    /// constants, the fields, the constructors, the deconstruction, the indexer and the operators. They implement
    /// the interfaces of the algebra of the kind of the vector and the operators of the kind of it.
    /// <para>The constants of the part are the whole numbers of the algebra of the kind of the value and, for a
    /// floating point kind, the constants of the math of the kind of it, the one of the denominator of a quotient
    /// of it, the mask of the sign of it and the ones of the kind the ieee 754 standard names, which the value
    /// reaches through the algebra of its kind.</para>
    /// </summary>
    /// <param name="typ">The type of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    private static string Gen(Typ typ, int size, bool storeVariant)
    {
        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var byteSize = typ.size * (size == 3 ? 4 : size);
        var bitSize = 8 * byteSize;
        var simd = VectorGenShared.Simd(typ, size, storeVariant);
        // the value of a 64 bit vector is kept in a raw ulong field, the other simd vectors keep the register
        var v64 = VectorGenShared.Uses64(typ, size, storeVariant);
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        var lanes = VectorGenShared.Lanes(typ, size, storeVariant);
        // the register of a 2 or 3 component vector is wider than the vector, the extra lanes are padding
        var pad = VectorGenShared.PadLanes(typ, size, storeVariant) > 0;
        var vecName = $"Vector{reg}";
        var vecType = $"{vecName}<{typ.simdComp}>";
        var cast = typ.shuffleCast;
        var attr = "[MethodImpl(256)]";
        var attrCpu = "[MethodImpl(256), CpuOnly]";

        var comp = VectorGenShared.Components(size);

        string Getter(int i) => $"vector.GetElement({i})";
        string Setter(int i) => $"vector = vector.WithElement({i}, value);";

        string Join(Func<int, string> f, string sep = ", ") => VectorGenShared.Join(size, f, sep);

        // every argument of the create call that is not a component is a padding lane and is set to zero
        string Create(params string[] values)
        {
            var all = new List<string>(values);
            for (var i = size; i < lanes; i++) all.Add("default");
            return $"{vecName}.Create({string.Join(", ", all)})";
        }

        string Broadcast(string first)
        {
            if (lanes == size) return $"{vecName}.Create({first})";
            var all = new List<string>();
            for (var i = 0; i < size; i++) all.Add(first);
            for (var i = size; i < lanes; i++) all.Add("default");
            return $"{vecName}.Create({string.Join(", ", all)})";
        }

        // the broadcast of a scalar into a vector whose register is wider than its value: the register of the
        // scalar holds it in the first lane and leaves the ones that follow it at zero, so a shuffle of it
        // reaches every component and fills the padding lanes with zero
        string ShuffleBroadcast(string first)
        {
            var all = new List<string>();
            for (var i = 0; i < lanes; i++) all.Add(i < size ? "0" : "1");
            return $"{vecName}.Shuffle({vecName}.CreateScalarUnsafe({first}), {vecName}.Create({string.Join(", ", all)}))";
        }

        // the construction of a simd result, see VectorGenShared.Vector
        string FromVector(string expr, bool masked = false) => VectorGenShared.Vector(simd, pad, expr, masked);

        // the 128 bit value of a 64 bit vector and the construction of a 64 bit vector from a 128 bit one
        string Load64(string self) => VectorGenShared.Load64(self, typ.simdComp);
        string From128(string expr) => VectorGenShared.From128(expr);

        // the literal of a value of the component type of the vector, half is a cast of the float literal
        string Lit(string value) => typ.name switch
        {
            "float" => $"{value}f",
            "double" => value,
            _ => $"({scalar})({value}f)",
        };

        // the mask that keeps the padding lanes of the register at zero
        var mask = !pad
            ? null
            : typ.size == 4
                ? $"{vecName}.Create({VectorGenShared.Join(lanes, i => i < size ? "-1" : "0")}).{AsMethod(typ.simdComp)}()"
                : $"{vecName}.Create({VectorGenShared.Join(lanes, i => i < size ? "-1L" : "0L")}).{AsMethod(typ.simdComp)}()";
        // the int type that has the same width as the components, the comparison masks use it
        var maskAs = typ.size == 4 ? "AsUInt32" : "AsUInt64";
        // bool components are wrappers, the span/pointer has to be reinterpreted
        var spanLoad = scalar == typ.simdComp
            ? "LoadUnsafe(in MemoryMarshal.GetReference(span))"
            : $"LoadUnsafe(in MemoryMarshal.Cast<{scalar}, {typ.simdComp}>(span)[0])";
        var ptrLoad = scalar == typ.simdComp
            ? "Load(ptr)"
            : $"Load(({typ.simdComp}*)ptr)";

        var sb = new StringBuilder();

        void Doc(string text) => sb.AppendLine($"    /// <summary>{text}</summary>");

        void DocParam(string name, string text) => sb.AppendLine($"    /// <param name=\"{name}\">{text}</param>");

        // the members that implement one of the vector interfaces inherit the documentation from it
        void InheritDoc() => sb.AppendLine("    /// <inheritdoc/>");

        // the name of the interface of the kind of the vector in the algebra library, which is the floating
        // point kind for every floating point type of the library, a half as well
        string AlgebraIface() => typ.f
            ? "IFloatingPointVector"
            : typ.sig
                ? "ISignedNumberVector"
                : "INumberVector";

        // only one of the partial declarations of a type may carry the documentation of the type, so the
        // documentation that names the interfaces of every part lives on the base members
        var type2 = VectorGenShared.VecName(typ, 2, false);
        var type3 = VectorGenShared.VecName(typ, 3, false);
        var type4 = VectorGenShared.VecName(typ, 4, false);
        // the bits of the value are reachable as a raw vector of bytes, the width of the register of the
        // vector decides the interface of them
        var underlying = VectorGenShared.Register(typ, size, storeVariant);
        var parts = new List<string>
        {
            "The members implement " +
            VectorGenShared.IfaceRef($"Algebras.IVector{size}", new List<string> { "TSelf", "TScalar" },
                new List<string> { type, scalar }) +
            (storeVariant
                ? ""
                : " and " +
                  VectorGenShared.IfaceRef($"Algebras.{AlgebraIface()}", new List<string> { "TSelf", "TScalar" },
                      new List<string> { type, scalar })),
        };

        // the storage variant of a vector holds the components of a value of the kind of it, so it keeps the
        // members the interfaces above declare alone and the ones that create it and convert it
        if (!storeVariant)
        {
            parts.Add(underlying == 0
                ? "the value has no register, it is only marked as <see cref=\"Algebras.Generics.IVectorSoftUnderlying\"/>"
                : "the underlying members implement " +
                  VectorGenShared.IfaceRef($"Algebras.Generics.IVector{underlying}Underlying", new List<string> { "TSelf" },
                      new List<string> { type }));
            // the members that create the vector out of another one implement the interfaces of the create members.
            // The interface of a vector of 2 components declares its create itself, which the first sentence names
            if (size == 3)
            {
                parts.Add("the create members implement " +
                          VectorGenShared.IfaceRef("Algebras.IVector3CtorFromVector2",
                              new List<string> { "TSelf", "TScalar", "TVector2" },
                              new List<string> { type, scalar, type2 }));
            }
            else if (size == 4)
            {
                parts.Add("the create members implement " +
                          VectorGenShared.IfaceRef("Algebras.IVector4CtorFromVector2",
                              new List<string> { "TSelf", "TScalar", "TVector2" },
                              new List<string> { type, scalar, type2 }) + " and " +
                          VectorGenShared.IfaceRef("Algebras.IVector4CtorFromVector3",
                              new List<string> { "TSelf", "TScalar", "TVector3" },
                              new List<string> { type, scalar, type3 }));
            }

            // the members that replace the components of the vector implement the interfaces of the replace members
            if (size == 2)
            {
                parts.Add("the replace members implement " +
                          VectorGenShared.IfaceRef("IVectorReplace", new List<string> { "TSelf", "TScalar" },
                              new List<string> { type, scalar }));
            }
            else if (size == 3)
            {
                parts.Add("the replace members implement " +
                          VectorGenShared.IfaceRef("IVector3Replace", new List<string> { "TSelf", "TScalar", "TVector2" },
                              new List<string> { type, scalar, type2 }));
            }
            else
            {
                parts.Add("the replace members implement " +
                          VectorGenShared.IfaceRef("IVector4Replace", new List<string> { "TSelf", "TScalar", "TVector2", "TVector3" },
                              new List<string> { type, scalar, type2, type3 }));
            }

            // the members of the legacy insert api implement the interfaces of the insert as well, they are the
            // legacy spelling of the members of the create of the longer vectors and forward to them
            if (size == 2)
            {
                parts.Add("the insert members implement " +
                          VectorGenShared.IfaceRef("IVector2Insert", new List<string> { "TSelf", "TScalar", "TVector3", "TVector4" },
                              new List<string> { type, scalar, type3, type4 }));
            }
            else if (size == 3)
            {
                parts.Add("the insert members implement " +
                          VectorGenShared.IfaceRef("IVector3Insert", new List<string> { "TSelf", "TScalar", "TVector4" },
                              new List<string> { type, scalar, type4 }));
            }

            // a shuffle combines two vectors of the same type, only a vector of 4 components has its members
            if (size == 4)
            {
                parts.Add("the shuffle members implement " +
                          VectorGenShared.IfaceRef("IVectorShuffle", new List<string> { "TSelf" },
                              new List<string> { type }));
            }
        }

        VectorGenShared.FileHeader(sb, true, true);
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// <c>{type}</c> is a vector of {size} <see cref=\"{scalar}\"/> components");
        if (pad || simd)
        {
            var traits = new List<string>();
            if (pad) traits.Add($"padded to {lanes} components");
            if (simd) traits.Add("backed by a hardware accelerated simd type");
            sb.AppendLine($"/// <para>It is {string.Join(" and it is ", traits)}</para>");
        }
        else if (storeVariant)
        {
            sb.AppendLine("/// <para>It keeps its components in fields, it has no simd register</para>");
        }

        sb.AppendLine($"/// <para>{string.Join(", ", parts)}</para>");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("[Serializable]");
        // the converter of a vector is shared by every vector of the count of its components, it names the type
        // of the vector and the type of a single component of it, and it reads and writes a component with the
        // converter the options name for the type of it
        sb.AppendLine(
            $"[JsonConverter(typeof({VectorGenerator.JsonNamespace}.Vector{size}JsonConverter<{type}, {scalar}>))]");
        // the interface of the size of the vector names the size and the components of the vector beside the
        // members of the kind of the vector, which the algebra library declares
        var ifaces = new List<string>
        {
            $"IEqualityOperators<{type}, {type}, bool>",
            $"Algebras.IVector{size}<{type}, {scalar}>",
        };
        // the storage variant of a vector holds the components of a value of the kind of it, it reaches the
        // equality, the comparison and the members the json converter of it needs and nothing else: the algebra
        // of the kind of the vector, the members that dispatch the value of it, the ones that create it out of
        // another vector and the ones that reduce it are the members of the regular vector of it alone
        if (!storeVariant)
        {
            ifaces.Add($"Algebras.{AlgebraIface()}<{type}, {scalar}>");
            // the comparison of the whole value is a member of the algebra of the kind of a number, which the
            // storage variant of a vector does not reach, so it keeps the value of the comparison alone
            ifaces.Add($"IComparisonOperators<{type}, {type}, bool>");
            // the members that dispatch the value of the vector implement the interface of the dispatch of it.
            // The dispatch of the value names the type of the value and the one that reaches the members that
            // take a single component of it beside it names the type of the component as well
            ifaces.Add(VectorGenShared.DispatchIface(type));
            ifaces.Add(VectorGenShared.DispatchIfaceScalar(type, scalar));
            // the members that build two values out of the one they are handed are written for a floating point
            // kind alone, so a vector of another kind does not reach the interface of the dispatch that reaches
            // them
            if (typ.f)
            {
                ifaces.Add(VectorGenShared.DispatchFloatIface(type));
                ifaces.Add(VectorGenShared.DispatchFloatIfaceScalar(type, scalar));
            }

            if (size >= 3) ifaces.Add($"Algebras.IVector{size}CtorFromVector2<{type}, {scalar}, {type2}>");
            if (size == 4) ifaces.Add($"Algebras.IVector4CtorFromVector3<{type}, {scalar}, {type3}>");
            // a signed vector reaches the negative of every whole number of the vector a column of its matrix
            // view is as well
            if (typ.sig) ifaces.Add($"Algebras.ISignedNumberMatrixVector<{type}, {type}>");
        }

        // the members of the parts that are a part of the value itself are emitted into the declaration of the
        // value as well, so every one of them is asked for its members and the interfaces it implements, its
        // members are appended into the declaration below. The storage variant of a vector keeps the members that
        // create it, the conversions of it and the ones that reach the components of it alone
        var underlyingMembers = storeVariant ? null : GenUnderlying(typ, size, storeVariant);
        if (!storeVariant)
        {
            ifaces.Add(underlying == 0
                ? "Algebras.Generics.IVectorSoftUnderlying"
                : $"Algebras.Generics.IVector{underlying}Underlying<{type}>");
        }

        var ctorMembers = GenCtor(typ, size, storeVariant);
        var arithMembers = !storeVariant && typ.arith ? GenArith(typ, size, storeVariant) : null;
        var asMembers = GenAs(typ, size, storeVariant, ifaces);
        var convMembers = GenConv(typ, size, storeVariant);
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine("    " + string.Join(",\n    ", ifaces));
        sb.AppendLine("{");

        #region Meta

        sb.AppendLine();
        sb.AppendLine("    #region Meta");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public static bool IsSimdAccelerated");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine($"        get => {(simd ? "true" : "false")};");
        sb.AppendLine("    }");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public static int Length");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine($"        get => {size};");
        sb.AppendLine("    }");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public static int SizeByte");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine($"        get => {byteSize};");
        sb.AppendLine("    }");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public static int SizeBit");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine($"        get => {bitSize};");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        #region Constants

        sb.AppendLine();
        sb.AppendLine("    #region Constants");
        sb.AppendLine();
        // every whole number the algebra reaches is a constant of the value beside the one of a single
        // component of it, the zero of a type is the default of it and the one of every other one is the
        // literal of the number of the component type
        for (var i = 0; i < VectorGenShared.NumberNames.Length; i++)
        {
            var name = VectorGenShared.NumberNames[i];
            var value = VectorGenShared.NumberValue(scalar, i);
            Prop($"public static {type} {name}", value == "default" ? "default" : $"new({value})");
            // the whole number of a single component is a member of the algebra of the kind of the vector,
            // so the storage variant of a vector keeps the value of the number alone
            if (!storeVariant) Prop($"public static {scalar} Scalar{name}", value);
        }

        // a signed value reaches the negative of every whole number beside the zero as well
        if (typ.sig && !storeVariant)
        {
            for (var i = 1; i < VectorGenShared.NumberNames.Length; i++)
            {
                var name = $"Negative{VectorGenShared.NumberNames[i]}";
                var value = VectorGenShared.NegativeValue(scalar, i);
                Prop($"public static {type} {name}", $"new({value})");
                Prop($"public static {scalar} Scalar{name}", value);
            }
        }

        // the constants of the algebra of a floating point kind: the ones of the math of the kind of it, the one
        // of the denominator of a quotient of it, the mask of the sign of it and the ones of the kind the ieee
        // 754 standard names. The storage variant of a vector reaches the components of a value and not the
        // algebra of its kind, so it holds none of them
        if (typ.arith && typ.f && !storeVariant)
        {
            foreach (var (name, value) in FloatConsts)
            {
                var lit = Lit(value);
                Prop($"public static {scalar} Scalar{name}", lit);
                Prop($"public static {type} {name}", $"new({lit})");
            }

            var denom = VectorGenShared.DenomEpsilonValue(scalar);
            Prop($"public static {scalar} Scalar{DenomEpsilonName}", denom);
            Prop($"public static {type} {DenomEpsilonName}", $"new({denom})");

            // the sign mask of the kind of the value is the negative zero of the kind of the component, which is
            // the only value of it that holds the sign and no other bit
            var sign = Lit("-0.0");
            Prop($"public static {scalar} Scalar{SignMaskName}", sign);
            Prop($"public static {type} {SignMaskName}", $"new({sign})");

            foreach (var name in IeeeConsts)
            {
                var value = name == MinNormalName ? VectorGenShared.MinNormalValue(scalar) : $"{scalar}.{name}";
                Prop($"public static {scalar} Scalar{name}", value);
                Prop($"public static {type} {name}", $"new({value})");
            }
        }

        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        #region matrix

        // a vector is a matrix of a single column, so the view of it as a matrix is the vector itself: the
        // members below are the ones the matrix interfaces of the vector need, the ones that take a vector
        // reach the value it holds and the width of the matrix is a single column
        void Prop(string decl, string expr)
        {
            InheritDoc();
            sb.AppendLine($"    {decl}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {expr};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        void Method(string decl, string body)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    {decl}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {body}");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        void Fn(string decl, string expr)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    {decl} => {expr};");
            sb.AppendLine();
        }

        // the vector itself is the value of the only column of the matrix
        var one = "One";

        sb.AppendLine();
        sb.AppendLine("    #region matrix");
        sb.AppendLine();
        Prop("public static int Columns", "1");
        Prop("public static int Rows", $"{size}");
        Prop($"public static {type} Identity", one);
        // every whole number the matrix of a single column reaches is the one of the vector itself, which is
        // the zero of it for the first one of them
        for (var i = 0; i < VectorGenShared.NumberNames.Length; i++)
        {
            var name = VectorGenShared.NumberNames[i];
            Prop($"public static {type} Vector{name}", $"{type}.{name}");
        }

        // a signed vector reaches the negative of every whole number beside the zero as well, the storage variant
        // of a vector does not reach the algebra of its kind, so it holds the whole numbers alone
        if (typ.sig && !storeVariant)
        {
            for (var i = 1; i < VectorGenShared.NumberNames.Length; i++)
            {
                var name = VectorGenShared.NumberNames[i];
                Prop($"public static {type} VectorNegative{name}", $"{type}.Negative{name}");
            }
        }

        Fn($"public static {type} Vector(in {type} scalar)", "scalar");
        Fn($"public static {type} Broadcast(in {type} scalar)", "scalar");
        Fn($"public static {type} Load(ReadOnlySpan<{type}> span)", "span[0]");
        Fn($"public static unsafe {type} Load({type}* ptr)", "*ptr");
        Fn($"static {type} Algebras.IMatrixVector<{type}, {type}>.get_vector(in {type} self, int index)", "self");
        Method($"static void Algebras.IMatrixVector<{type}, {type}>.set_vector(ref {type} self, int index, in {type} value)",
            "self = value;");
        Fn($"static {scalar} Algebras.IAlgebra<{type}, {scalar}>.get(in {type} self, int index)", "self[index]");
        Method($"static void Algebras.IAlgebra<{type}, {scalar}>.set(ref {type} self, int index, {scalar} value)",
            "self[index] = value;");
        Fn($"static {scalar} Algebras.IMatrixScalar<{type}, {scalar}>.get(in {type} self, int row, int column)",
            "self[row]");
        Method($"static void Algebras.IMatrixScalar<{type}, {scalar}>.set(ref {type} self, int row, int column, {scalar} value)",
            "self[row] = value;");
        sb.AppendLine("    #endregion");

        #endregion

        #region fields

        sb.AppendLine();
        sb.AppendLine("    #region fields");
        sb.AppendLine();
        if (simd && v64)
        {
            Doc($"The raw 64 bits of the vector, the <c>vector</c> property reinterprets them" +
                "<para>Writing it directly <b>bypasses</b> the property</para>");
            sb.AppendLine($"    internal ulong {VectorGenShared.Vector64Field};");
            sb.AppendLine();
            Doc($"The raw <see cref=\"{vecName}{{T}}\"/> value of the vector");
            sb.AppendLine($"    public {vecType} vector");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        readonly get => Unsafe.BitCast<ulong, {vecType}>({VectorGenShared.Vector64Field});");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        set => {VectorGenShared.Vector64Field} = Unsafe.BitCast<{vecType}, ulong>(value);");
            sb.AppendLine("    }");
        }
        else if (simd)
        {
            Doc($"The raw <see cref=\"{vecName}{{T}}\"/> value of the vector" +
                (pad
                    ? "<para>Writing it directly <b>bypasses</b> the mask that keeps the padding lanes at zero</para>"
                    : ""));
            sb.AppendLine($"    public {vecType} vector;");
        }
        else
        {
            // a vector without a register keeps its components in fields, the fields are its value
            for (var i = 0; i < size; i++)
            {
                Doc($"The <c>{comp[i]}</c> component");
                sb.AppendLine($"    public {scalar} {comp[i]};");
            }

            if (size == 3) sb.AppendLine($"    private {scalar} _align;");
        }

        if (simd)
        {
            // a component of a vector with a register is a view of it
            sb.AppendLine();
            for (var i = 0; i < size; i++)
            {
                Doc($"The <c>{comp[i]}</c> component");
                sb.AppendLine($"    public {scalar} {comp[i]}");
                sb.AppendLine("    {");
                sb.AppendLine($"        {attr}");
                sb.AppendLine($"        readonly get => {Getter(i)};");
                sb.AppendLine($"        {attr}");
                sb.AppendLine($"        set => {Setter(i)}");
                sb.AppendLine("    }");
            }
        }

        var colorName = new[] { "red", "green", "blue", "alpha" };
        for (var i = 0; i < size; i++)
        {
            var xyzw = comp[i];
            var rgba = Typ.rgba[i];
            Doc($"The {colorName[i]} component, it is the same as <see cref=\"{xyzw}\"/>");
            sb.AppendLine($"    public {scalar} {rgba}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        readonly get => {xyzw};");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        set => {xyzw} = value;");
            sb.AppendLine("    }");
        }

        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        #region ctors

        sb.AppendLine();
        sb.AppendLine("    #region ctors");
        sb.AppendLine();
        if (simd)
        {
            Doc(mask == null
                ? $"Creates a vector from a raw <see cref=\"{vecName}{{T}}\"/> value"
                : $"Creates a vector from a raw <see cref=\"{vecName}{{T}}\"/> value" +
                  "<para>The padding lanes of the register are set to zero</para>");
            DocParam("vector", $"The raw <see cref=\"{vecName}{{T}}\"/> value");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public {type}({vecType} vector) => this.vector = " +
                          (mask == null ? "vector;" : $"vector & {mask};"));
            sb.AppendLine();
        }

        Doc("Creates a vector from its components");
        for (var i = 0; i < size; i++) DocParam(comp[i], $"The <c>{comp[i]}</c> component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type}({Join(i => $"{scalar} {comp[i]}")})");
        sb.AppendLine("    {");
        if (simd) sb.AppendLine($"        vector = {Create(comp)};");
        else
            for (var i = 0; i < size; i++)
                sb.AppendLine($"        this.{comp[i]} = {comp[i]};");
        sb.AppendLine("    }");
        sb.AppendLine();
        Doc("Creates a vector from a tuple of its components");
        DocParam("tuple", "The tuple of the components");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type}(({Join(i => $"{scalar} {comp[i]}")}) tuple) : this({Join(i => $"tuple.{comp[i]}")}) {{ }}");
        sb.AppendLine();
        Doc("Converts a tuple of the components to a vector");
        DocParam("tuple", "The tuple of the components");
        sb.AppendLine("    /// <returns>The vector</returns>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static implicit operator {type}(({Join(i => $"{scalar} {comp[i]}")}) tuple) => new(tuple);");
        sb.AppendLine();
        Doc("Converts a scalar to a broadcast vector");
        DocParam("value", "The value of every component");
        sb.AppendLine("    /// <returns>The broadcast vector</returns>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static implicit operator {type}({scalar} value) => new(value);");
        sb.AppendLine();

        // the only scalar conversion of a vector is the one of the type of its component: the conversion of a
        // scalar that reaches the type of a component through a standard conversion is the one of that type, and
        // an operator of its own would make the conversion of an integer that two of them reach ambiguous

        if (storeVariant)
        {
            // the storage variant and the regular vector keep the same components in different storages, the
            // conversion of the regular one into the storage variant is an operator of the vector that is emitted
            // beside the other conversions of it, see GenConv
            var regular = VectorGenShared.VecName(typ, size, false);
            var fromRegular = v64 ? From128("value.vector") : $"new({Join(i => $"value.{comp[i]}")})";
            Doc($"Creates the vector from the regular <see cref=\"{regular}\"/>");
            DocParam("value", "The vector to convert");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public {type}(in {regular} value) => this = {fromRegular};");
            sb.AppendLine();
        }

        Doc("Creates a vector with every component set to <paramref name=\"value\"/>");
        DocParam("value", "The value of every component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public {type}({scalar} value)");
        sb.AppendLine("    {");
        // the broadcast of a floating point component into a vector whose register is wider than its value
        // builds the whole register and masks the padding lanes of it, so it does not need the platform to
        // leave the lanes that follow the one of the register of a scalar at zero, BroadcastUnsafe is the one
        // that saves the mask on a platform that does
        if (simd)
            sb.AppendLine(pad && typ.f
                ? $"        vector = {vecName}.Create({cast}value) & {mask};"
                : $"        vector = {Broadcast($"{cast}value")};");
        else
            for (var i = 0; i < size; i++)
                sb.AppendLine($"        {comp[i]} = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        Doc("Creates a vector with every component set to <paramref name=\"value\"/>");
        DocParam("value", "The value of every component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} Broadcast({scalar} value) => new(value);");
        sb.AppendLine();
        // the broadcast that fills the padding lanes with a shuffle into the lane that follows the one of the
        // register of a scalar instead of masking the register it builds, so the code that knows the platform
        // uses it instead of the broadcast
        Doc(
            "Creates a vector with every component set to <paramref name=\"value\"/><para>It shuffles the register of the scalar into the value, which only leaves the padding lanes of it at zero on a platform whose hardware zeroes the lanes that follow the one of the register of a scalar</para>");
        DocParam("value", "The value of every component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine(simd && pad && typ.f
            ? $"    public static {type} BroadcastUnsafe({scalar} value) => new() {{ vector = {ShuffleBroadcast($"{cast}value")} }};"
            : $"    public static {type} BroadcastUnsafe({scalar} value) => new(value);");
        sb.AppendLine();
        Doc($"Creates a vector with only the <c>{comp[0]}</c> component set to <paramref name=\"value\"/><para>The other components are zero</para>");
        DocParam("value", $"The value of the <c>{comp[0]}</c> component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} Scalar({scalar} value) => " +
                      (simd
                          ? $"new() {{ vector = {vecName}.CreateScalar({cast}value) }};"
                          : $"new() {{ {comp[0]} = value }};"));
        sb.AppendLine();
        // the scalar that leaves the lanes that follow the one of the register of the scalar as they are
        // instead of building the whole register, so the code that knows the platform uses it instead of the
        // scalar
        Doc(
            $"Creates a vector with only the <c>{comp[0]}</c> component set to <paramref name=\"value\"/><para>It leaves the lanes that follow the one of the register of the scalar as they are, which only leaves the padding lanes of it at zero on a platform whose hardware zeroes the lanes that follow the one of the register of a scalar</para>");
        DocParam("value", $"The value of the <c>{comp[0]}</c> component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine(simd && pad
            ? $"    public static {type} ScalarUnsafe({scalar} value) => new() {{ vector = {vecName}.CreateScalarUnsafe({cast}value) }};"
            : $"    public static {type} ScalarUnsafe({scalar} value) => Scalar(value);");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attrCpu}");
        sb.AppendLine($"    public static {type} Load(ReadOnlySpan<{scalar}> span) => new(span);");
        sb.AppendLine();
        Doc(simd
            ? "Creates a vector from the beginning of <paramref name=\"span\"/><para>It reads a whole simd register, so the span has to be at least as long as the padded vector</para>"
            : "Creates a vector from the beginning of <paramref name=\"span\"/>");
        DocParam("span", "The span to load from");
        sb.AppendLine($"    {attrCpu}");
        if (simd)
        {
            // routed through the vector constructor so the padding lane mask is applied
            sb.AppendLine($"    public {type}(ReadOnlySpan<{scalar}> span) : this({vecName}.{spanLoad}) {{ }}");
        }
        else
        {
            sb.AppendLine($"    public {type}(ReadOnlySpan<{scalar}> span)");
            sb.AppendLine("    {");
            for (var i = 0; i < size; i++) sb.AppendLine($"        this.{comp[i]} = span[{i}];");
            sb.AppendLine("    }");
        }

        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attrCpu}");
        sb.AppendLine($"    public static unsafe {type} Load({scalar}* ptr) => new(ptr);");
        sb.AppendLine();
        Doc(simd
            ? "Creates a vector from <paramref name=\"ptr\"/><para>It reads a whole simd register, so the pointer has to point to at least as many components as the padded vector</para>"
            : "Creates a vector from <paramref name=\"ptr\"/>");
        DocParam("ptr", "The pointer to load from");
        sb.AppendLine($"    {attrCpu}");
        if (simd)
        {
            sb.AppendLine($"    public unsafe {type}({scalar}* ptr) : this({vecName}.{ptrLoad}) {{ }}");
        }
        else
        {
            sb.AppendLine($"    public unsafe {type}({scalar}* ptr)");
            sb.AppendLine("    {");
            for (var i = 0; i < size; i++) sb.AppendLine($"        this.{comp[i]} = ptr[{i}];");
            sb.AppendLine("    }");
        }

        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        #region deconstruct

        sb.AppendLine();
        sb.AppendLine("    #region deconstruct");
        sb.AppendLine();
        Doc("Deconstructs the vector into its components");
        for (var i = 0; i < size; i++) DocParam(comp[i], $"Receives the <c>{comp[i]}</c> component");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly void Deconstruct({Join(i => $"out {scalar} {comp[i]}")})");
        sb.AppendLine("    {");
        for (var i = 0; i < size; i++) sb.AppendLine($"        {comp[i]} = this.{comp[i]};");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        #region index

        sb.AppendLine();
        sb.AppendLine("    #region index");
        sb.AppendLine();
        Doc("Returns or sets the component at the position of <paramref name=\"i\"/>");
        DocParam("i", "The position of the component");
        sb.AppendLine($"    public {scalar} this[int i]");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        readonly get => i switch");
        sb.AppendLine("        {");
        for (var i = 0; i < size; i++) sb.AppendLine($"            {i} => {comp[i]},");
        sb.AppendLine("            _ => throw new IndexOutOfRangeException(nameof(i)),");
        sb.AppendLine("        };");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        set");
        sb.AppendLine("        {");
        sb.AppendLine("            switch (i)");
        sb.AppendLine("            {");
        for (var i = 0; i < size; i++)
        {
            sb.AppendLine($"                case {i}:");
            sb.AppendLine($"                    {comp[i]} = value;");
            sb.AppendLine("                    break;");
        }

        sb.AppendLine("                default:");
        sb.AppendLine("                    throw new IndexOutOfRangeException(nameof(i));");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        #region components

        sb.AppendLine();
        sb.AppendLine("    #region components");
        sb.AppendLine();

        // the components are reached through the static members of the interface of the algebra of the vector,
        // the fields and the properties of the type stay beside them for the code that names the type of the
        // vector. The members replace a property of the interface, so they are implemented explicitly and they
        // do not become a part of the surface of the type itself.
        for (var i = 0; i < size; i++)
        {
            // the interface of the count of the components declares the component of the position of it, the
            // interface of a longer vector inherits the component and does not declare it again, so the member
            // reaches the interface of the shortest vector that declares it
            var algebra = $"Algebras.IVector{Math.Max(2, i + 1)}Components<{type}, {scalar}>";
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {scalar} {algebra}.get_{comp[i]}(in {type} self) => self.{comp[i]};");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static void {algebra}.set_{comp[i]}(ref {type} self, {scalar} value) => self.{comp[i]} = value;");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");

        #endregion

        #region ops

        sb.AppendLine();
        sb.AppendLine("    #region ops");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine("    public readonly override int GetHashCode() => HashCode.Combine(" +
                      Join(i => comp[i]) + ");");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly override bool Equals(object? obj) => obj is {type} other && Equals(other);");
        sb.AppendLine();

        // emits the accelerated fast paths, falls back to the scalar expression when no vector is accelerated
        void EmitSimd(string full, string wide, string fallback)
        {
            if (simd)
            {
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {full};");
                if (v64)
                {
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    sb.AppendLine($"            return {wide};");
                }
            }

            sb.AppendLine($"        return {fallback};");
        }

        // emits a comparison operator that returns the value of the kind of the vector itself: every component
        // of the result is the all bits set value of the kind where the comparison of the components holds and
        // the zero of it where it does not, so a bool result is a value of the kind and no mask type is needed
        void EmitMaskOp(string op, string vecOp, bool invert, string scalarOp, string doc)
        {
            Doc(doc);
            DocParam("left", "The left vector");
            DocParam("right", "The right vector");
            sb.AppendLine(
                "    /// <returns>A value whose every component says whether the comparison holds and whose component that holds is the all bits set value of the kind of it</returns>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator {op}({type} left, {type} right)");
            sb.AppendLine("    {");
            // the padding lanes are zero on both sides, so the comparison of them holds and the result of the
            // whole register leaves the padding lanes at all ones for the operators that hold on them, which the
            // mask of the constructor of the value clears
            if (simd)
            {
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {FromVector($"{(invert ? "~" : "")}{vecName}.{vecOp}(left.vector, right.vector)", true)};");
            }

            sb.AppendLine($"        return new({Join(i => $"Utils.AllBits<{scalar}>(left.{comp[i]} {scalarOp} right.{comp[i]})")});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // emits the bool result of a comparison operator required by IComparisonOperators
        void EmitBoolOp(string op, string vecOp, string scalarOp)
        {
            // the padding lanes of a vector whose register is wider than it are zero on both sides, an all
            // comparison over the whole register would be false for them, so the mask is compared against the
            // expected mask instead
            var lessOp = vecOp switch
            {
                "LessThanAll" => "LessThan",
                "GreaterThanAll" => "GreaterThan",
                _ => null,
            };
            var expected = !pad
                ? $"{vecName}<{(typ.size == 4 ? "uint" : "ulong")}>.AllBitsSet"
                : $"{vecName}.Create({VectorGenShared.Join(lanes, i => i < size ? (typ.size == 4 ? "-1" : "-1L") : (typ.size == 4 ? "0" : "0L"))}).AsUInt{(typ.size == 4 ? "32" : "64")}()";

            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static bool IComparisonOperators<{type}, {type}, bool>.operator {op}({type} left, {type} right)");
            sb.AppendLine("    {");
            if (simd)
            {
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                if (lessOp == null)
                    sb.AppendLine($"            return {vecName}.{vecOp}(left.vector, right.vector);");
                else
                    sb.AppendLine($"            return {vecName}.EqualsAll({vecName}.{lessOp}(left.vector, right.vector).{maskAs}(), {expected});");
                if (v64)
                {
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    if (lessOp == null)
                        sb.AppendLine($"            return Vector128.{vecOp}({Load64("left.")}, {Load64("right.")});");
                    else
                        // the upper lanes of the widened operands are zero, only the low 64 bits of the mask are meaningful
                        sb.AppendLine(
                            $"            return Vector128.{lessOp}({Load64("left.")}, {Load64("right.")}).AsUInt64()[0] == ulong.MaxValue;");
                }
            }

            sb.AppendLine($"        return {Join(i => $"left.{comp[i]} {scalarOp} right.{comp[i]}", " && ")};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly bool Equals({type} other)");
        sb.AppendLine("    {");
        EmitSimd(
            $"{vecName}.EqualsAll(vector, other.vector)",
            $"Vector128.EqualsAll({Load64("")}, {Load64("other.")})",
            Join(i => $"{comp[i]} == other.{comp[i]}", " && "));
        sb.AppendLine("    }");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine("    public readonly int CompareTo(object? obj)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (ReferenceEquals(null, obj)) return 1;");
        sb.AppendLine($"        return obj is {type} other ? CompareTo(other) : throw new ArgumentException($\"Object must be of type {{nameof({type})}}\");");
        sb.AppendLine("    }");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly int CompareTo({type} other)");
        sb.AppendLine("    {");
        if (simd)
        {
            sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
            sb.AppendLine("        {");
            sb.AppendLine($"            if ({vecName}.LessThanAny(vector, other.vector)) return -1;");
            sb.AppendLine($"            if ({vecName}.GreaterThanAny(vector, other.vector)) return 1;");
            sb.AppendLine("            return 0;");
            sb.AppendLine("        }");
            if (v64)
            {
                sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                sb.AppendLine("        {");
                sb.AppendLine($"            if (Vector128.LessThanAny({Load64("")}, {Load64("other.")})) return -1;");
                sb.AppendLine($"            if (Vector128.GreaterThanAny({Load64("")}, {Load64("other.")})) return 1;");
                sb.AppendLine("            return 0;");
                sb.AppendLine("        }");
            }
        }

        sb.AppendLine($"        if ({Join(i => $"{comp[i]} < other.{comp[i]}", " || ")}) return -1;");
        sb.AppendLine($"        if ({Join(i => $"{comp[i]} > other.{comp[i]}", " || ")}) return 1;");
        sb.AppendLine("        return 0;");
        sb.AppendLine("    }");
        sb.AppendLine();

        EmitMaskOp("==", "Equals", false, "==",
            "Returns a value that says where the components of the two vectors are equal");
        EmitMaskOp("!=", "Equals", true, "!=",
            "Returns a value that says where the components of the two vectors are not equal");
        EmitMaskOp("<", "LessThan", false, "<",
            "Returns a value that says where a component of the left vector is less than the component of the right vector");
        EmitMaskOp(">", "GreaterThan", false, ">",
            "Returns a value that says where a component of the left vector is greater than the component of the right vector");
        EmitMaskOp("<=", "LessThanOrEqual", false, "<=",
            "Returns a value that says where a component of the left vector is less than or equal to the component of the right vector");
        EmitMaskOp(">=", "GreaterThanOrEqual", false, ">=",
            "Returns a value that says where a component of the left vector is greater than or equal to the component of the right vector");
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static bool IEqualityOperators<{type}, {type}, bool>.operator ==({type} left, {type} right) => left.Equals(right);");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static bool IEqualityOperators<{type}, {type}, bool>.operator !=({type} left, {type} right) => !left.Equals(right);");
        sb.AppendLine();
        if (!storeVariant)
        {
            EmitBoolOp("<", "LessThanAll", "<");
            EmitBoolOp(">", "GreaterThanAll", ">");
            EmitBoolOp("<=", "LessThanOrEqualAll", "<=");
            EmitBoolOp(">=", "GreaterThanOrEqualAll", ">=");
        }

        // a bitwise operator is a member of every value of the algebra library, so the storage variant of a vector
        // reaches it as well
        // emits a bitwise operator, the scalar expression is only used when no vector is accelerated
        void EmitBitOp(string op, string rhs, string vecRhs, string wideRhs, string scalarExpr)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator {op}({type} a, {rhs} b)");
            sb.AppendLine("    {");
            // the padding lane is zero on both sides and every bitwise operator keeps it zero
            if (simd)
            {
                var accel = $"a.vector {op} {vecRhs}";
                var wide = $"{Load64("a.")} {op} {wideRhs}";
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {FromVector(accel)};");
                if (v64)
                {
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    sb.AppendLine($"            return {From128(wide)};");
                }
            }

            sb.AppendLine($"        return new({scalarExpr});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator ~({type} a)");
        sb.AppendLine("    {");
        if (simd)
        {
            // the complement of the zero padding lanes is all ones, this one keeps the mask
            sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
            sb.AppendLine($"            return {FromVector("~a.vector", true)};");
            if (v64)
            {
                sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                sb.AppendLine($"            return {From128($"~{Load64("a.")}")};");
            }
        }

        // a component that is an integer has the bitwise operators of its own, the bits of a floating
        // point one are reached through the bit conversions of the BCL
        var bitwise = !typ.i;

        string BitNot(string x) => bitwise
            ? VectorScalar.FromBits(scalar, $"~{VectorScalar.Bits(scalar, x)}")
            : $"({scalar})~{x}";

        string BitOp(string op, string x, string y) => bitwise
            ? VectorScalar.FromBits(scalar, $"({VectorScalar.Bits(scalar, x)} {op} {VectorScalar.Bits(scalar, y)})")
            : $"({scalar})({x} {op} {y})";

        // the shift of a value is the shift of its bits, a narrow signed value is widened to its unsigned form
        // for the logical one, so the bits above the value do not reach it
        string BitShift(string op, string x, string n) => bitwise
            ? VectorScalar.FromBits(scalar, $"({VectorScalar.Bits(scalar, x)} {op} {n})")
            : $"({scalar})({(op == ">>>" ? VectorScalar.Unsigned(scalar, x) : x)} {op} {n})";

        sb.AppendLine($"        return new({Join(i => BitNot($"a.{comp[i]}"))});");
        sb.AppendLine("    }");
        sb.AppendLine();
        EmitBitOp("&", type, "b.vector", Load64("b."),
            Join(i => BitOp("&", $"a.{comp[i]}", $"b.{comp[i]}")));
        EmitBitOp("|", type, "b.vector", Load64("b."),
            Join(i => BitOp("|", $"a.{comp[i]}", $"b.{comp[i]}")));
        EmitBitOp("^", type, "b.vector", Load64("b."),
            Join(i => BitOp("^", $"a.{comp[i]}", $"b.{comp[i]}")));
        EmitBitOp("<<", "int", "b", "b",
            Join(i => BitShift("<<", $"a.{comp[i]}", "b")));
        EmitBitOp(">>", "int", "b", "b",
            Join(i => BitShift(">>", $"a.{comp[i]}", "b")));
        EmitBitOp(">>>", "int", "b", "b",
            Join(i => BitShift(">>>", $"a.{comp[i]}", "b")));

        // a shift of the value by a vector shifts every component by the amount the component of the same position
        // of the other vector holds, the amount of a component of 4 bytes is a uint and the one of a component of
        // 8 bytes is a ulong, so the amount is the vector of the unsigned whole number of the width of the value
        var amount = VectorGenShared.VecName(
            VectorGenShared.ConvTypes[typ.size == 8 ? "ulong" : "uint"], size, storeVariant);
        var unsigned = typ.size == 8 ? "AsUInt64" : "AsUInt32";
        // the shift of a signed whole number that keeps its sign is the arithmetic one, every other shift reaches
        // the bits of the value: the bits of a floating point component are shifted by its amount
        var signed = typ.size == 8 ? "AsInt64" : "AsInt32";
        var arithmetic = typ.i && typ.sig;
        // the register of a vector of 3 components holds a lane that is not one of them, the helper of the simd
        // library keeps it at zero when it is told that the value is of 3 components
        var three = simd && size == 3;

        // emits a shift by the vector of the amount of every component, the view of the value is the one the bits
        // of it are reached through and is only used when a vector is accelerated
        void EmitShiftOp(string op, string member, string left)
        {
            InheritDoc();
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator {op}({type} a, {amount} b)");
            sb.AppendLine("    {");
            if (simd)
            {
                var shifted = $"simd.{member}(a.vector.{left}(), b.vector{(three ? ", true" : "")})";
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {FromVector($"{shifted}.{AsMethod(typ.simdComp)}()")};");
            }

            sb.AppendLine($"        return new({Join(i => BitShift(op, $"a.{comp[i]}", $"(int)b.{comp[i]}"))});");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitShiftOp("<<", "ShiftLeft", unsigned);
        EmitShiftOp(">>", "ShiftRight", arithmetic ? signed : unsigned);
        EmitShiftOp(">>>", "ShiftRight", unsigned);

        sb.AppendLine("    #endregion");

        #endregion

        #region str

        // the format of a vector is the list of its components between parentheses, the name of its type is not
        // a part of it, and every component is formatted with the format and the provider of the call
        void EmitFormatLiteral(string literal) =>
            sb.AppendLine($"        if (!FormatUtils.TryFormatPart(ref dst, ref n, {literal})) return false;");

        // a component is formatted with the format and the provider of the call
        void EmitFormatComponent(int i) =>
            sb.AppendLine($"        if (!FormatUtils.TryFormatPart(ref dst, ref n, {comp[i]}, format, provider)) return false;");

        // the body of a TryFormat, the literals of the utf8 one are the utf8 literals of the same text
        void EmitFormatBody(string open, string separator, string close)
        {
            sb.AppendLine("    {");
            sb.AppendLine("        nc = 0;");
            sb.AppendLine("        var n = 0;");
            EmitFormatLiteral(open);
            for (var i = 0; i < size; i++)
            {
                if (i != 0) EmitFormatLiteral(separator);
                EmitFormatComponent(i);
            }

            EmitFormatLiteral(close);
            sb.AppendLine("        nc = n;");
            sb.AppendLine("        return true;");
            sb.AppendLine("    }");
        }

        // the text of a component of the ToString members
        string ComponentText(int i, bool formatted) => formatted
            ? $"{{{comp[i]}.ToString(format, formatProvider)}}"
            : $"{{{comp[i]}}}";

        sb.AppendLine();
        sb.AppendLine("    #region str");
        sb.AppendLine();
        Doc($"Formats the vector as <c>({Join(i => comp[i])})</c>");
        sb.AppendLine("    public readonly override string ToString() => $\"(" + Join(i => ComponentText(i, false)) + ")\";");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public readonly string ToString(string? format, IFormatProvider? formatProvider)");
        sb.AppendLine("        => $\"(" + Join(i => ComponentText(i, true)) + ")\";");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public readonly bool TryFormat(Span<char> dst, out int nc, ReadOnlySpan<char> format, IFormatProvider? provider)");
        EmitFormatBody("\"(\"", "\", \"", "\")\"");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public readonly bool TryFormat(Span<byte> dst, out int nc, ReadOnlySpan<char> format, IFormatProvider? provider)");
        EmitFormatBody("\"(\"u8", "\", \"u8", "\")\"u8");
        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        // every part that is emitted into the declaration of the value is separated from the part before it by
        // an empty line and it is named by a region of its own, so the members of the value stay readable
        void Part(string? members, string name)
        {
            if (members == null) return;
            // a part is separated from the one before it by an empty line, the region of it holds an empty line
            // after the name of it and the line of the last member of the part is closed at its end as well
            sb.AppendLine();
            sb.AppendLine($"    #region {name}");
            sb.AppendLine();
            sb.Append(members.Trim('\r', '\n'));
            sb.AppendLine();
            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        // the region of the dispatch is emitted by the part itself, it is separated from the one before it here
        sb.AppendLine();
        var dispatch = GenDispatch(typ, size, storeVariant);
        if (dispatch != null)
        {
            sb.Append(dispatch.Trim('\r', '\n'));
            sb.AppendLine();
        }

        // the members of the parts above are a part of the value itself, so they are appended into its
        // declaration as well
        Part(underlyingMembers, "underlying");
        Part(ctorMembers, "ctor");
        Part(arithMembers, "arith");
        Part(asMembers, "as");
        Part(convMembers, "conv");

        sb.AppendLine("}");

        return VectorGenShared.Normalize(sb.ToString());
    }
}
