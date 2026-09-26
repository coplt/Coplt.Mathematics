using System;
using System.Collections.Generic;
using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the base members of the vector described by <paramref name="typ"/>: the meta data, the
    /// constants, the fields, the constructors, the deconstruction, the indexer and the operators. They implement
    /// <c>IVector</c> and the operators of <c>INumberVector</c> / <c>IBoolVector</c>.
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
        var bol = typ.bol;
        var iface = bol ? "IBoolVector" : "INumberVector";
        var boolType = $"b{typ.size * 8}v{size}";
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

        // the name of the interface of the kind of the vector in the algebra library, the one of the ieee 754
        // standard is the one of a floating point number whose type names the standard itself
        string AlgebraIface() => bol
            ? "IBoolVector"
            : typ.f
                ? typ.name == "half" ? "IFloatingPointVector" : "IFloatingPointIeee754Vector"
                : typ.sig
                    ? "ISignedNumberVector"
                    : "INumberVector";

        // only one of the partial declarations of a type may carry the documentation of the type, so the
        // documentation that names the interfaces of every part lives on the base members
        var type2 = VectorGenShared.VecName(typ, 2, false);
        var type3 = VectorGenShared.VecName(typ, 3, false);
        var type4 = VectorGenShared.VecName(typ, 4, false);
        var parts = new List<string>
        {
            "The base members implement " +
            VectorGenShared.IfaceRef($"IVector{size}", new List<string> { "TSelf", "TScalar" },
                new List<string> { type, scalar }) + " and " +
            VectorGenShared.IfaceRef(iface, new List<string> { "TSelf", "TScalar" }, new List<string> { type, scalar }),
        };

        // the bits of the value are reachable as a raw vector of bytes, the width of the register of the
        // vector decides the interface of them
        var underlying = VectorGenShared.Register(typ, size, storeVariant);
        parts.Add(underlying == 0
            ? "the value has no register, it is only marked as <see cref=\"Algebras.Generics.IVectorSoftUnderlying\"/>"
            : "the underlying members implement " +
              VectorGenShared.IfaceRef($"Algebras.Generics.IVector{underlying}Underlying", new List<string> { "TSelf" },
                  new List<string> { type }));
        // the members that create the vector out of another one implement the interfaces of the create members
        if (size == 2)
        {
            parts.Add("the create members implement " +
                      VectorGenShared.IfaceRef("IVector2Ctor", new List<string> { "TSelf", "TScalar" },
                          new List<string> { type, scalar }));
        }
        else if (size == 3)
        {
            parts.Add("the create members implement " +
                      VectorGenShared.IfaceRef("IVector3CtorFromVector2", new List<string> { "TSelf", "TScalar", "TVector2" },
                          new List<string> { type, scalar, type2 }));
        }
        else
        {
            parts.Add("the create members implement " +
                      VectorGenShared.IfaceRef("IVector4CtorFromVector2", new List<string> { "TSelf", "TScalar", "TVector2" },
                          new List<string> { type, scalar, type2 }) + " and " +
                      VectorGenShared.IfaceRef("IVector4CtorFromVector3", new List<string> { "TSelf", "TScalar", "TVector3" },
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

        if (typ.arith)
        {
            var arith = new List<string>();
            foreach (var i in VectorGenShared.ArithInterfaces(typ, size, storeVariant))
            {
                arith.Add(VectorGenShared.IfaceRef(i.Name, new List<string> { "Self", "Scalar" }, i.Args));
            }

            parts.Add($"the arithmetic members implement {string.Join(" and ", arith)}");
        }

        VectorGenShared.FileHeader(sb, false, true);
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
        // the converter of the vector is generated beside it, it lives in the namespace the converters share
        // and the attribute names it in full
        sb.AppendLine($"[JsonConverter(typeof({VectorGenerator.JsonNamespace}.{type}JsonConverter))]");
        // the interface of the size of the vector names the size and the components of the vector beside the
        // members of the kind of the vector, the algebra interfaces are implemented beside the older ones and
        // the two families stay beside each other until the older one is migrated away
        var ifaces = new List<string>
        {
            $"IVector{size}<{type}, {scalar}>",
            $"{iface}<{type}, {scalar}>",
            $"IEqualityOperators<{type}, {type}, {boolType}>",
            $"IComparisonOperators<{type}, {type}, {boolType}>",
            $"Algebras.IVector{size}<{type}, {scalar}>",
            $"Algebras.{AlgebraIface()}<{type}, {scalar}>",
        };
        // the members that dispatch the value of the vector implement the interface of the dispatch of it, a
        // mask does not dispatch: the members of the visitors name the number vector of a type. The dispatch of
        // the kind of a number names the type of a component of the value beside it, the one of a floating
        // point kind reaches the values of the kind of it alone, so it names the type of the value
        if (!bol)
            foreach (var family in VectorGenShared.DispatchIfaces(typ))
                ifaces.Add(VectorGenShared.DispatchIface(family, type, scalar));
        if (size >= 3) ifaces.Add($"Algebras.IVector{size}CtorFromVector2<{type}, {scalar}, {type2}>");
        if (size == 4) ifaces.Add($"Algebras.IVector4CtorFromVector3<{type}, {scalar}, {type3}>");
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
        if (bol)
        {
            InheritDoc();
            sb.AppendLine($"    public static {type} True");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => new({typ.one});");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {type} False");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine("        get => default;");
            sb.AppendLine("    }");
        }
        else
        {
            InheritDoc();
            sb.AppendLine($"    public static {type} Zero");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine("        get => default;");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {type} One");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => new({typ.one});");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {type} Two");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => new(({scalar})({typ.two}));");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {scalar} ScalarTwo");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => ({scalar})({typ.two});");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {scalar} ScalarZero");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine("        get => default;");
            sb.AppendLine("    }");
            sb.AppendLine();
            InheritDoc();
            sb.AppendLine($"    public static {scalar} ScalarOne");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {typ.one};");
            sb.AppendLine("    }");
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

        // the vector itself is the value of the only column of the matrix, and a mask reaches its true and its
        // false instead of the zero and the one of a number
        var one = bol ? "True" : "One";
        var two = bol ? "True" : "Two";

        sb.AppendLine();
        sb.AppendLine("    #region matrix");
        sb.AppendLine();
        Prop("public static int Columns", "1");
        Prop("public static int Rows", $"{size}");
        Prop($"public static {type} Identity", one);
        Prop($"public static {type} VectorZero", "default");
        Prop($"public static {type} VectorOne", one);
        Prop($"public static {type} VectorTwo", two);
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

        // a scalar that converts to the type of a component implicitly converts to a vector of the component as
        // well, see Typ.ScalarConverts
        if (Typ.ScalarConverts.TryGetValue(typ.compType, out var scalarConverts))
        {
            foreach (var source in scalarConverts)
            {
                if (source == scalar) continue;
                Doc($"Converts a {source} scalar to a broadcast vector");
                DocParam("value", "The value of every component");
                sb.AppendLine("    /// <returns>The broadcast vector</returns>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    public static implicit operator {type}({source} value) => new(value);");
                sb.AppendLine();
            }
        }

        if (storeVariant)
        {
            // the storage variant and the regular vector keep the same components in different storages, the
            // conversions go through the register of the 64 bit value and through the components otherwise
            var regular = VectorGenShared.VecName(typ, size, false);
            var fromRegular = v64 ? From128("value.vector") : $"new({Join(i => $"value.{comp[i]}")})";
            // the value of the storage variant is widened by the helper of the vector type, it zeroes the
            // padding lanes of the regular register
            var toRegular = v64
                ? $"new() {{ vector = {Load64("value.")} }}"
                : $"new({Join(i => $"value.{comp[i]}")})";
            Doc($"Creates the vector from the regular <see cref=\"{regular}\"/>");
            DocParam("value", "The vector to convert");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public {type}(in {regular} value) => this = {fromRegular};");
            sb.AppendLine();
            Doc($"Converts the regular <see cref=\"{regular}\"/> to the vector");
            DocParam("value", "The vector to convert");
            sb.AppendLine("    /// <returns>The vector</returns>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static implicit operator {type}(in {regular} value) => {fromRegular};");
            sb.AppendLine();
            Doc($"Converts the vector to the regular <see cref=\"{regular}\"/>");
            DocParam("value", "The vector to convert");
            sb.AppendLine("    /// <returns>The regular vector</returns>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static implicit operator {regular}(in {type} value) => {toRegular};");
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
        InheritDoc();
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

        // the components are reached through the static members of the interface of them, the fields and the
        // properties of the type stay beside them for the code that names the type of the vector. The members
        // replace a property of the interface, so they are implemented explicitly and they do not become a part
        // of the surface of the type itself.
        for (var i = 0; i < size; i++)
        {
            // the first two components are declared by the interface of 2 components and every longer one adds
            // its own component to it
            var components = $"IVector{(i < 2 ? 2 : i + 1)}Components<{type}, {scalar}>";
            // the algebra library declares the components of the size of the vector on the interface of the
            // size itself, so the same member is implemented explicitly for it as well
            var algebra = $"Algebras.IVector{size}<{type}, {scalar}>";
            foreach (var name in new[] { comp[i], Typ.rgba[i] })
            {
                InheritDoc();
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {scalar} {components}.get_{name}(in {type} self) => self.{name};");
                sb.AppendLine();
                InheritDoc();
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static void {components}.set_{name}(ref {type} self, {scalar} value) => self.{name} = value;");
                sb.AppendLine();
                // the algebra interface only declares the spelling of the position of a component
                if (name == comp[i])
                {
                    InheritDoc();
                    sb.AppendLine($"    {attr}");
                    sb.AppendLine($"    static {scalar} {algebra}.get_{name}(in {type} self) => self.{name};");
                    sb.AppendLine();
                    InheritDoc();
                    sb.AppendLine($"    {attr}");
                    sb.AppendLine($"    static void {algebra}.set_{name}(ref {type} self, {scalar} value) => self.{name} = value;");
                    sb.AppendLine();
                }
            }
        }

        // the index of a component replaced the indexer of the interface
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static {scalar} IVector<{type}, {scalar}>.get_at(in {type} self, int i) => self[i];");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static void IVector<{type}, {scalar}>.set_at(ref {type} self, int i, {scalar} value) => self[i] = value;");
        sb.AppendLine();
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

        // emits a comparison operator that returns the bool vector, it is not part of the vector interfaces
        void EmitMaskOp(string op, string vecOp, bool invert, string scalarOp, string doc)
        {
            string MaskExpr(string name, bool to64)
            {
                var args = to64 ? $"{Load64("left.")}, {Load64("right.")}" : "left.vector, right.vector";
                var expr = $"{name}.{vecOp}({args}).{maskAs}()";
                if (invert) expr = $"~{expr}";
                return expr;
            }

            Doc(doc);
            DocParam("left", "The left vector");
            DocParam("right", "The right vector");
            sb.AppendLine("    /// <returns>A mask with the result of <i>every</i> component</returns>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {boolType} operator {op}({type} left, {type} right)");
            sb.AppendLine("    {");
            // the padding lanes are zero on both sides, so a comparison over the whole vector of them is false,
            // the operators that keep false there write the field directly, the ones that turn it into true need
            // the mask because the mask of every component is compared against theirs by the bool vector checks
            var masked = op is "==" or "<=" or ">=";
            if (simd)
            {
                // the mask is a vector of the bool type of the vector and its register is 128 bits wide, so the
                // mask of the 64 bit register of the value is widened to it, the padding lanes the widening
                // creates are false and the ones the comparison creates are masked by the constructor
                var mask64 = MaskExpr(vecName, false);
                if (v64) mask64 = $"Vector128.Create({mask64})";
                sb.AppendLine($"        if ({vecName}.IsHardwareAccelerated)");
                sb.AppendLine($"            return {(masked ? $"new({mask64})" : $"new() {{ vector = {mask64} }}")};");
                if (v64)
                {
                    var mask128 = MaskExpr("Vector128", true);
                    sb.AppendLine("        if (Vector128.IsHardwareAccelerated)");
                    sb.AppendLine($"            return {(masked ? $"new({mask128})" : $"new() {{ vector = {mask128} }}")};");
                }
            }

            sb.AppendLine($"        return new({Join(i => $"left.{comp[i]} {scalarOp} right.{comp[i]}")});");
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
        if (!bol)
        {
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
        }

        EmitMaskOp("==", "Equals", false, "==",
            "Returns a mask that is true where the components of the two vectors are equal");
        EmitMaskOp("!=", "Equals", true, "!=",
            "Returns a mask that is true where the components of the two vectors are not equal");
        EmitMaskOp("<", "LessThan", false, "<",
            "Returns a mask that is true where a component of the left vector is less than the component of the right vector");
        EmitMaskOp(">", "GreaterThan", false, ">",
            "Returns a mask that is true where a component of the left vector is greater than the component of the right vector");
        EmitMaskOp("<=", "LessThanOrEqual", false, "<=",
            "Returns a mask that is true where a component of the left vector is less than or equal to the component of the right vector");
        EmitMaskOp(">=", "GreaterThanOrEqual", false, ">=",
            "Returns a mask that is true where a component of the left vector is greater than or equal to the component of the right vector");
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static bool IEqualityOperators<{type}, {type}, bool>.operator ==({type} left, {type} right) => left.Equals(right);");
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static bool IEqualityOperators<{type}, {type}, bool>.operator !=({type} left, {type} right) => !left.Equals(right);");
        sb.AppendLine();
        if (!bol)
        {
            EmitBoolOp("<", "LessThanAll", "<");
            EmitBoolOp(">", "GreaterThanAll", ">");
            EmitBoolOp("<=", "LessThanOrEqualAll", "<=");
            EmitBoolOp(">=", "GreaterThanOrEqualAll", ">=");
        }

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

        // a component that is an integer or a mask has the bitwise operators of its own, the bits of a floating
        // point one are reached through the bit conversions of the BCL
        var bitwise = !typ.i && !bol;

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
        if (!bol)
        {
            EmitBitOp("<<", "int", "b", "b",
                Join(i => BitShift("<<", $"a.{comp[i]}", "b")));
            EmitBitOp(">>", "int", "b", "b",
                Join(i => BitShift(">>", $"a.{comp[i]}", "b")));
            EmitBitOp(">>>", "int", "b", "b",
                Join(i => BitShift(">>>", $"a.{comp[i]}", "b")));
        }

        sb.AppendLine("    #endregion");

        #endregion

        #region str

        // the format of a vector is the list of its components between parentheses, the name of its type is not
        // a part of it, and every component is formatted with the format and the provider of the call
        void EmitFormatLiteral(string literal) =>
            sb.AppendLine($"        if (!FormatUtils.TryFormatPart(ref dst, ref n, {literal})) return false;");

        // a component of a bool vector is a mask, its text is the lower case name of the value it tests and the
        // format and the provider are not used, the other components are formatted with them
        void EmitFormatComponent(int i, bool utf8) =>
            sb.AppendLine(bol
                ? $"        if (!FormatUtils.TryFormatPart(ref dst, ref n, (bool){comp[i]} ? {Literal("true", utf8)} : {Literal("false", utf8)})) return false;"
                : $"        if (!FormatUtils.TryFormatPart(ref dst, ref n, {comp[i]}, format, provider)) return false;");

        string Literal(string text, bool utf8) => utf8 ? $"\"{text}\"u8" : $"\"{text}\"";

        // the body of a TryFormat, the literals of the utf8 one are the utf8 literals of the same text
        void EmitFormatBody(string open, string separator, string close, bool utf8)
        {
            sb.AppendLine("    {");
            sb.AppendLine("        nc = 0;");
            sb.AppendLine("        var n = 0;");
            EmitFormatLiteral(open);
            for (var i = 0; i < size; i++)
            {
                if (i != 0) EmitFormatLiteral(separator);
                EmitFormatComponent(i, utf8);
            }

            EmitFormatLiteral(close);
            sb.AppendLine("        nc = n;");
            sb.AppendLine("        return true;");
            sb.AppendLine("    }");
        }

        // the text of a component of the ToString members, a bool component ignores the format and the provider
        // and its conditional expression has to be parenthesized to be the whole of an interpolation
        string ComponentText(int i, bool formatted) => bol
            ? $"{{((bool){comp[i]} ? \"true\" : \"false\")}}"
            : formatted
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
        EmitFormatBody("\"(\"", "\", \"", "\")\"", false);
        sb.AppendLine();
        InheritDoc();
        sb.AppendLine("    public readonly bool TryFormat(Span<byte> dst, out int nc, ReadOnlySpan<char> format, IFormatProvider? provider)");
        EmitFormatBody("\"(\"u8", "\", \"u8", "\")\"u8", true);
        sb.AppendLine();
        sb.AppendLine("    #endregion");

        #endregion

        // the members that dispatch the value of the vector to a visitor, a mask has none of them and they are
        // a part of the file of the base members of the vector
        var dispatch = GenDispatch(typ, size, storeVariant);
        if (dispatch != null) sb.Append(dispatch);

        sb.AppendLine("}");

        return VectorDocs.Apply(sb.ToString());
    }
}
