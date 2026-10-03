using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the basic members of every matrix of <see cref="Typ"/>. A matrix is laid out by columns, so a
/// column is the primary value of it: the columns are the fields of the value, the index of a column reaches
/// one of them and the index of a component reaches a component of a column. The shape of a matrix is the
/// number of its rows and the number of its columns, a column of <c>r</c> rows is the vector of <c>r</c>
/// components of the same type, and the storage variant of a matrix is the one whose columns are the storage
/// variants of the vectors. The members are emitted into one file per type.
/// </summary>
[Generator]
public class MatrixGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (var typ in Typ.Typs)
            {
                for (var rows = 2; rows <= 4; rows++)
                {
                    for (var cols = 2; cols <= 4; cols++)
                    {
                        // a matrix of a number has a storage variant for the shapes of the vectors that have one
                        var variants = VectorGenShared.HasStorageVariant(typ, rows) ? 2 : 1;
                        for (var variant = 0; variant < variants; variant++)
                        {
                            var storeVariant = variant == 1;
                            var name = Name(typ, rows, cols, storeVariant);
                            ctx.AddSource(
                                $"{VectorGenerator.VecNamespace}.{name}.g.cs",
                                SourceText.From(Gen(typ, rows, cols, storeVariant), Encoding.UTF8));
                        }
                    }
                }
            }
        });
    }

    /// <summary>
    /// Returns the name of a matrix: the name of its component type, the number of its rows, the number of its
    /// columns and the suffix of the storage variant.
    /// </summary>
    /// <param name="typ">The type of the component of the matrix</param>
    /// <param name="rows">The number of rows of the matrix</param>
    /// <param name="cols">The number of columns of the matrix</param>
    /// <param name="storeVariant">True for the storage variant of the matrix</param>
    /// <returns>The name of the matrix</returns>
    public static string Name(Typ typ, int rows, int cols, bool storeVariant) =>
        $"{typ.name}{rows}x{cols}{(storeVariant ? "s" : "")}";

    private static string Gen(Typ typ, int rows, int cols, bool storeVariant)
    {
        var type = Name(typ, rows, cols, storeVariant);
        // a column of the matrix is the vector of the number of the rows, the storage variant of the matrix
        // keeps its columns in the storage variants of the vectors
        var col = VectorGenShared.VecName(typ, rows, storeVariant);
        // a row of the matrix is the vector of the number of the columns, it only keeps the storage variant of
        // the matrix when the vectors of that count have one of their own
        var row = VectorGenShared.VecName(typ, cols, storeVariant && VectorGenShared.HasStorageVariant(typ, cols));
        var scalar = typ.compType;
        var simd = VectorGenShared.Simd(typ, rows, storeVariant);
        var zero = $"{col}.Zero";
        var sizeByte = VectorGenShared.ByteSize(typ, rows, storeVariant) * cols;
        var attr = "[MethodImpl(256)]";

        var sb = new StringBuilder();

        // emits a property that returns a constant of the type
        void Prop(string doc, string decl, string expr)
        {
            sb.AppendLine($"    /// <summary>{doc}</summary>");
            sb.AppendLine($"    {decl}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {attr}");
            sb.AppendLine($"        get => {expr};");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        // emits a method that builds its result out of the value of the matrix
        void Method(string decl, string body)
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    {decl}");
            sb.AppendLine("    {");
            sb.AppendLine($"        {body}");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        var shape = $"{rows}x{cols}";
        // every shape has the members of its own beside the ones every matrix has
        var ifaces = new List<string>();
        // the comparison of the algebra of the kind of a matrix answers with the mask of the kind of the value,
        // which is the one the members of the value reach, so the bool result of the comparison of the framework
        // is the one the concrete type names on its own, see INumberAlgebra{TSelf}
        ifaces.Add($"IEqualityOperators<{type}, {type}, bool>");
        // the storage variant of a matrix holds the columns of a value of the kind of it, it reaches the interface
        // the json converter of it needs and nothing else: the algebra of the kind of the matrix, the members
        // that dispatch the value of it and the ones that reach a single component of it are the members of the
        // regular matrix of it alone
        if (storeVariant)
        {
            ifaces.Add($"Algebras.IMatrixMx{cols}Vector<{type}, {col}>");
        }
        else
        {
            ifaces.Add($"Algebras.IMatrix<{type}>");
            ifaces.Add($"Algebras.IMatrixScalar<{type}, {scalar}>");
            ifaces.Add($"Algebras.IMatrixVector<{type}, {col}>");
            ifaces.Add($"Algebras.{(typ.f ? "IFloatingPointMatrix" : typ.sig ? "ISignedNumberMatrix" : "INumberMatrix")}<{type}, {scalar}>");
            // the comparison of the algebra is the one of the mask of the kind of the value, the bool result of
            // the one of the framework is the one of the concrete type, which the storage variant of a matrix
            // does not reach
            ifaces.Add($"IComparisonOperators<{type}, {type}, bool>");
            ifaces.Add($"Algebras.IMatrix{shape}<{type}>");
            ifaces.Add($"Algebras.IMatrix{shape}Vector<{type}, {col}>");
            ifaces.Add($"Algebras.IMatrix{shape}Scalar<{type}, {scalar}>");
            // a matrix dispatches the value of it to a visitor and the vectors it is made of to the visitors
            // that reduce them to a vector
            ifaces.Add(VectorGenShared.DispatchIface(type));
            ifaces.Add(VectorGenShared.DispatchIfaceScalar(type, scalar));
            // the members that build two values out of the one they are handed are written for a floating point
            // kind alone, so a matrix of another kind does not reach the interface of the dispatch that reaches
            // them
            if (typ.f)
            {
                ifaces.Add(VectorGenShared.DispatchFloatIface(type));
                ifaces.Add(VectorGenShared.DispatchFloatIfaceScalar(type, scalar));
            }

            ifaces.Add($"Algebras.Generics.Dispatch.IMatrixColumnDispatch<{type}, {col}>");
            ifaces.Add($"Algebras.Generics.Dispatch.IMatrixRowDispatch<{type}, {row}>");

            // a signed matrix reaches the negative of every whole number of the vector a column of it is as well
            if (typ.sig) ifaces.Add($"Algebras.ISignedNumberMatrixVector<{type}, {col}>");
        }

        VectorGenShared.FileHeader(sb, false);
        // the converter of a matrix is shared by every matrix of the count of its columns: it names the type of
        // the matrix and the type of a column of it, and it reads and writes a column with the converter of the
        // vector itself, so the value is an array of the arrays of the components of its columns
        sb.AppendLine($"[JsonConverter(typeof(Json.MatrixMx{cols}JsonConverter<{type}, {col}>))]");
        sb.AppendLine($"public partial struct {type} :");
        sb.AppendLine("    " + string.Join(",\n    ", ifaces));
        sb.AppendLine("{");

        sb.AppendLine("    #region Meta");
        sb.AppendLine();
        Prop("True when every column of the matrix is backed by a hardware accelerated simd type",
            "public static bool IsSimdAccelerated", simd ? "true" : "false");
        Prop("The number of the columns of the matrix", "public static int Columns", $"{cols}");
        Prop("The number of the rows of the matrix", "public static int Rows", $"{rows}");
        Prop("The number of the components of the matrix", "public static int Length", $"{rows * cols}");
        Prop("The size of the value of the matrix in bytes", "public static int SizeByte", $"{sizeByte}");
        Prop("The size of the value of the matrix in bits", "public static int SizeBit", $"{sizeByte * 8}");
        sb.AppendLine("    #endregion");
        sb.AppendLine();

        sb.AppendLine("    #region fields");
        sb.AppendLine();
        for (var i = 0; i < cols; i++)
        {
            sb.AppendLine($"    /// <summary>The column of the matrix at the index <c>{i}</c></summary>");
            sb.AppendLine($"    public {col} c{i};");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // every component of the matrix is an argument of a constructor of it and of the create member of the
        // shape, the row of a component leads the name of the argument
        var scalarArgs = new List<string>();
        var scalarNames = new List<string>();
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                scalarArgs.Add($"{scalar} m{r}{c}");
                scalarNames.Add($"m{r}{c}");
            }
        }

        sb.AppendLine("    #region ctors");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Creates a matrix from its columns</summary>");
        sb.AppendLine($"    public {type}({VectorGenShared.Join(cols, i => $"{col} c{i}")})");
        sb.AppendLine("    {");
        for (var i = 0; i < cols; i++)
        {
            sb.AppendLine($"        this.c{i} = c{i};");
        }

        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Creates a matrix from its components, the row of a component leads its name</summary>");
        sb.AppendLine($"    public {type}({string.Join(", ", scalarArgs)})");
        sb.AppendLine("    {");
        for (var c = 0; c < cols; c++)
        {
            var comps = new List<string>();
            for (var r = 0; r < rows; r++) comps.Add($"m{r}{c}");
            sb.AppendLine($"        c{c} = new {col}({string.Join(", ", comps)});");
        }

        sb.AppendLine("    }");
        sb.AppendLine();
        // the storage variant of a matrix keeps its columns in the storage variants of the vectors, so the two of
        // them convert into each other through the columns of the value, the conversion between the two of them
        // is emitted beside the other conversions of the matrix, see GenConv
        if (VectorGenShared.HasStorageVariant(typ, rows))
        {
            var other = Name(typ, rows, cols, !storeVariant);
            var columns = VectorGenShared.Join(cols, i => $"value.c{i}");
            if (storeVariant)
            {
                sb.AppendLine($"    /// <summary>Creates the matrix from the regular <see cref=\"{other}\"/></summary>");
                sb.AppendLine($"    public {type}({other} value) => this = new({columns});");
                sb.AppendLine();
                sb.AppendLine($"    /// <summary>Converts the storage variant of the matrix to the regular <see cref=\"{other}\"/></summary>");
                sb.AppendLine($"    public readonly {other} to_compute");
                sb.AppendLine("    {");
                sb.AppendLine("        [MethodImpl(256)]");
                sb.AppendLine($"        get => ({other})this;");
                sb.AppendLine("    }");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine($"    /// <summary>Converts the matrix to its storage variant <see cref=\"{other}\"/></summary>");
                sb.AppendLine($"    public readonly {other} to_storage");
                sb.AppendLine("    {");
                sb.AppendLine("        [MethodImpl(256)]");
                sb.AppendLine($"        get => ({other})this;");
                sb.AppendLine("    }");
                sb.AppendLine();
            }
        }

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        sb.AppendLine("    #region index");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns the column of the matrix at <paramref name=\"column\"/></summary>");
        sb.AppendLine($"    public {col} this[int column]");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        readonly get => column switch");
        sb.AppendLine("        {");
        for (var i = 0; i < cols; i++)
        {
            sb.AppendLine($"            {i} => c{i},");
        }

        sb.AppendLine("            _ => throw new IndexOutOfRangeException(),");
        sb.AppendLine("        };");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        set");
        sb.AppendLine("        {");
        sb.AppendLine("            switch (column)");
        sb.AppendLine("            {");
        for (var i = 0; i < cols; i++)
        {
            sb.AppendLine($"                case {i}: c{i} = value; break;");
        }

        sb.AppendLine("                default: throw new IndexOutOfRangeException();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Returns the component of the matrix at <paramref name=\"row\"/> and " +
                      "<paramref name=\"column\"/></summary>");
        sb.AppendLine($"    public {scalar} this[int row, int column]");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        readonly get => this[column][row];");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        set");
        sb.AppendLine("        {");
        sb.AppendLine("            var c = this[column];");
        sb.AppendLine("            c[row] = value;");
        sb.AppendLine("            this[column] = c;");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine();
        // the component of the matrix at a row and a column is a member of its own, the row of it leads the
        // name so that the texts of two matrices of a different shape read the same
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                sb.AppendLine($"    /// <summary>The component of the matrix at the row <c>{r}</c> and the " +
                              $"column <c>{c}</c></summary>");
                sb.AppendLine($"    public {scalar} m{r}{c}");
                sb.AppendLine("    {");
                sb.AppendLine($"        {attr}");
                sb.AppendLine($"        readonly get => c{c}.{Typ.xyzw[r]};");
                sb.AppendLine($"        {attr}");
                sb.AppendLine($"        set => c{c}.{Typ.xyzw[r]} = value;");
                sb.AppendLine("    }");
                sb.AppendLine();
            }
        }

        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static {col} Algebras.IMatrixVector<{type}, {col}>.get_vector({type} self, int column) => " +
                      "self[column];");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static void Algebras.IMatrixVector<{type}, {col}>.set_vector(ref {type} self, int column, {col} value) => " +
                      "self[column] = value;");
        sb.AppendLine();
        // the storage variant of a matrix reaches a component of it through the members of the column of it
        if (!storeVariant)
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {scalar} Algebras.IMatrixScalar<{type}, {scalar}>.get({type} self, int row, int column) => " +
                          "self[row, column];");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static void Algebras.IMatrixScalar<{type}, {scalar}>.set(ref {type} self, int row, int column, {scalar} value) => " +
                          "self[row, column] = value;");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {scalar} Algebras.IAlgebra<{type}, {scalar}>.get({type} self, int index) => " +
                          $"self[index / {rows}, index % {rows}];");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static void Algebras.IAlgebra<{type}, {scalar}>.set(ref {type} self, int index, {scalar} value) => " +
                          $"self[index / {rows}, index % {rows}] = value;");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // a matrix is the value of its columns, so a scalar reaches the first component of the first column and
        // a vector fills every column with the value it holds
        sb.AppendLine("    #region view");
        sb.AppendLine();
        var zeroMatrix = $"new {type}({VectorGenShared.Join(cols, _ => zero)})";
        // the storage variant of a matrix holds the columns of a value of the kind of it and does not reach the
        // algebra of the kind of a single component, so it keeps the members that reach a column of it alone
        if (!storeVariant)
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} Broadcast({scalar} scalar) => " +
                          $"new({VectorGenShared.Join(cols, _ => $"{col}.Broadcast(scalar)")});");
            sb.AppendLine();
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} BroadcastUnsafe({scalar} scalar) => " +
                          $"new({VectorGenShared.Join(cols, _ => $"{col}.BroadcastUnsafe(scalar)")});");
            sb.AppendLine();
            Method($"public static {type} Scalar({scalar} scalar)",
                $"var r = {zeroMatrix}; r[0, 0] = scalar; return r;");
            Method($"public static {type} ScalarUnsafe({scalar} scalar)",
                $"return new({VectorGenShared.Join(cols, i => i == 0 ? $"{col}.ScalarUnsafe(scalar)" : zero)});");
            Method($"public static {type} Load(ReadOnlySpan<{scalar}> span)",
                $"var r = default({type}); var i = 0; " +
                $"for (var c = 0; c < {cols}; c++) for (var j = 0; j < {rows}; j++) r[j, c] = span[i++]; return r;");
            Method($"public static unsafe {type} Load({scalar}* ptr)",
                $"var r = default({type}); var i = 0; " +
                $"for (var c = 0; c < {cols}; c++) for (var j = 0; j < {rows}; j++) r[j, c] = ptr[i++]; return r;");
        }

        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} Broadcast({col} scalar) => " +
                      $"new({VectorGenShared.Join(cols, _ => "scalar")});");
        sb.AppendLine();
        Method($"public static {type} Vector({col} scalar)", $"var r = {zeroMatrix}; r[0] = scalar; return r;");
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} Load(ReadOnlySpan<{col}> span) => " +
                      $"new({VectorGenShared.Join(cols, i => $"span[{i}]")});");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static unsafe {type} Load({col}* ptr) => " +
                      $"new({VectorGenShared.Join(cols, i => $"ptr[{i}]")});");
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();

        sb.AppendLine("    #region Constants");
        sb.AppendLine();
        // every whole number the algebra reaches is a constant of the matrix, which is the value of every
        // column of it, beside the one of a single component of it
        for (var i = 0; i < VectorGenShared.NumberNames.Length; i++)
        {
            var name = VectorGenShared.NumberNames[i];
            Prop($"A matrix whose columns are the vector of the {name.ToLowerInvariant()} of its component type",
                "public static " + type + " " + name,
                $"new({VectorGenShared.Join(cols, _ => $"{col}.{name}")})");
            Prop($"The {name.ToLowerInvariant()} of the component type of the matrix",
                $"public static {scalar} Scalar{name}", VectorGenShared.NumberValue(scalar, i));
        }

        // a signed matrix reaches the negative of every whole number beside the zero as well, the storage
        // variant of a matrix holds the columns of a value and does not reach the algebra of its kind
        if (typ.sig && !storeVariant)
        {
            for (var i = 1; i < VectorGenShared.NumberNames.Length; i++)
            {
                var name = $"Negative{VectorGenShared.NumberNames[i]}";
                Prop($"A matrix whose columns are the vector of the {name.ToLowerInvariant()} of its component type",
                    "public static " + type + " " + name,
                    $"new({VectorGenShared.Join(cols, _ => $"{col}.{name}")})");
                Prop($"The {name.ToLowerInvariant()} of the component type of the matrix",
                    $"public static {scalar} Scalar{name}", VectorGenShared.NegativeValue(scalar, i));
            }
        }

        // a matrix of a floating point number reaches the math constants of the algebra of its kind: every
        // component of it is the constant, which is the one of every column of it. The storage variant of a
        // matrix reaches the columns of a value and not the algebra of its kind, so it holds no constant of it
        if (typ.f && !storeVariant)
        {
            // the constants of the kind of a floating point number, the constant of the denominator of a quotient
            // of the kind of it and the ones of the kind the ieee 754 standard names, which every floating point
            // type of the library names
            var consts = VectorGenerator.FloatConsts.Select(c => c.Name)
                .Append(VectorGenerator.DenomEpsilonName)
                .Append(VectorGenerator.SignMaskName)
                .Concat(VectorGenerator.IeeeConsts);
            foreach (var name in consts)
            {
                Prop($"The {name} of the component type of the matrix", $"public static {scalar} Scalar{name}",
                    $"{col}.Scalar{name}");
                Prop($"A matrix whose every component is the {name} of the kind of it",
                    $"public static {type} {name}", $"new({VectorGenShared.Join(cols, _ => $"{col}.{name}")})");
            }
        }

        // a matrix reaches the whole numbers of the vector a column of it is as well
        for (var i = 0; i < VectorGenShared.NumberNames.Length; i++)
        {
            var name = VectorGenShared.NumberNames[i];
            Prop($"The {name.ToLowerInvariant()} of the vector of a column of the matrix",
                $"public static {col} Vector{name}", $"{col}.{name}");
        }

        // a signed matrix reaches the negative of every one of them as well
        if (typ.sig && !storeVariant)
        {
            for (var i = 1; i < VectorGenShared.NumberNames.Length; i++)
            {
                var name = VectorGenShared.NumberNames[i];
                Prop($"The minus {name.ToLowerInvariant()} of the vector of a column of the matrix",
                    $"public static {col} VectorNegative{name}", $"{col}.Negative{name}");
            }
        }

        sb.AppendLine("    /// <summary>The identity of the matrix, it is the value that keeps a matrix unchanged</summary>");
        sb.AppendLine($"    public static {type} Identity");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        get");
        sb.AppendLine("        {");
        sb.AppendLine($"            var r = {zeroMatrix};");
        for (var j = 0; j < rows && j < cols; j++)
        {
            sb.AppendLine($"            r[{j}, {j}] = ScalarOne;");
        }

        sb.AppendLine("            return r;");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // the create members of the shape of the matrix, every component of it is an argument of one of them
        sb.AppendLine("    #region create");
        sb.AppendLine();
        // the members that reach the columns of the matrix are declared by the interface of the number of the
        // columns, the shape of the matrix implements them through it
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static {type} Algebras.IMatrixMx{cols}Vector<{type}, {col}>.Create(" +
                      $"{VectorGenShared.Join(cols, i => $"{col} c{i}")}) => " +
                      $"new({VectorGenShared.Join(cols, i => $"c{i}")});");
        sb.AppendLine();
        // the components of the matrix reach the constructor of it, which takes them in the order of a row. The
        // storage variant of a matrix holds the columns of a value of the kind of it alone, so it names no member
        // of the shape that reaches a single component
        if (!storeVariant)
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {type} Algebras.IMatrix{shape}Scalar<{type}, {scalar}>.Create(" +
                          $"{string.Join(", ", scalarArgs)}) => new({string.Join(", ", scalarNames)});");
            sb.AppendLine();
        }

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // the accessors of the shape, every column and every component of it has one
        if (rows >= 2 && cols >= 2)
        {
            sb.AppendLine("    #region index");
            sb.AppendLine();
            for (var j = 0; j < cols; j++)
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {col} Algebras.IMatrixMx{cols}Vector<{type}, {col}>.get_c{j}({type} self) => " +
                              $"self.c{j};");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static void Algebras.IMatrixMx{cols}Vector<{type}, {col}>.set_c{j}(ref {type} self, {col} value) => " +
                              $"self.c{j} = value;");
                sb.AppendLine();
            }

            // a component of the matrix reaches the column of it, the interface of the shape of the matrix names
            // the component and the storage variant of the matrix names the column of it alone
            if (!storeVariant)
                for (var r = 0; r < rows; r++)
                {
                    for (var c = 0; c < cols; c++)
                    {
                        sb.AppendLine("    /// <inheritdoc/>");
                        sb.AppendLine($"    {attr}");
                        sb.AppendLine($"    static {scalar} Algebras.IMatrix{shape}Scalar<{type}, {scalar}>.get_m{r}{c}({type} self) => " +
                                      $"self.c{c}.{Typ.xyzw[r]};");
                        sb.AppendLine();
                        sb.AppendLine("    /// <inheritdoc/>");
                        sb.AppendLine($"    {attr}");
                        sb.AppendLine($"    static void Algebras.IMatrix{shape}Scalar<{type}, {scalar}>.set_m{r}{c}(ref {type} self, {scalar} value) => " +
                                      $"self.c{c}.{Typ.xyzw[r]} = value;");
                        sb.AppendLine();
                    }
                }

            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        // the value of a matrix is the value of its columns, so every member of it is the member of a column
        sb.AppendLine("    #region ops");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly override int GetHashCode() => HashCode.Combine({VectorGenShared.Join(cols, i => $"c{i}")});");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly override bool Equals(object? obj) => obj is {type} other && Equals(other);");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly bool Equals({type} other) => " +
                      $"{VectorGenShared.Join(cols, i => $"c{i}.Equals(other.c{i})", " && ")};");
        sb.AppendLine();
        // a bool result of a comparison of two values of the library is a value of the kind of them itself: the
        // all bits set value of the kind says that the comparison holds and the zero of it says that it does not,
        // so the member of the interface that requires a bool result is the one that returns it
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator ==({type} left, {type} right) => " +
                      $"left.Equals(right) ? ~default({type}) : default;");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator !=({type} left, {type} right) => " +
                      $"left.Equals(right) ? default : ~default({type});");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static bool IEqualityOperators<{type}, {type}, bool>.operator ==({type} left, {type} right) => left.Equals(right);");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static bool IEqualityOperators<{type}, {type}, bool>.operator !=({type} left, {type} right) => !left.Equals(right);");
        sb.AppendLine();
        // a bitwise operator is a member of every value of the algebra library, so the storage variant of a matrix
        // reaches it as well
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator &({type} left, {type} right) => " +
                      $"new({VectorGenShared.Join(cols, i => $"left.c{i} & right.c{i}")});");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator |({type} left, {type} right) => " +
                      $"new({VectorGenShared.Join(cols, i => $"left.c{i} | right.c{i}")});");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator ^({type} left, {type} right) => " +
                      $"new({VectorGenShared.Join(cols, i => $"left.c{i} ^ right.c{i}")});");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} operator ~({type} value) => " +
                      $"new({VectorGenShared.Join(cols, i => $"~value.c{i}")});");
        sb.AppendLine();
        // a matrix of a number reaches the arithmetic of a column, the comparison of it is the one of its
        // columns in the order of them
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public readonly int CompareTo({type} other)");
        sb.AppendLine("    {");
        for (var i = 0; i < cols; i++)
        {
            sb.AppendLine($"        {(i == 0 ? "var" : "")} c = c{i}.CompareTo(other.c{i});");
            sb.AppendLine("        if (c != 0) return c;");
        }

        sb.AppendLine("        return 0;");
        sb.AppendLine("    }");
        sb.AppendLine();
        // the whole number of a matrix is a member of the algebra of the kind of it, the storage variant of a
        // matrix holds the columns of a value of the kind of it and does not reach it
        if (!storeVariant)
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    int IComparable.CompareTo(object? obj) => obj is {type} other");
            sb.AppendLine($"        ? CompareTo(other)");
            sb.AppendLine($"        : throw new ArgumentException(null, nameof(obj));");
            sb.AppendLine();
        }

        foreach (var (name, op) in new[] { ("<", "<"), ("<=", "<="), (">", ">"), (">=", ">=") })
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator {name}({type} left, {type} right) => " +
                          $"left.CompareTo(right) {op} 0 ? ~default({type}) : default;");
            sb.AppendLine();
        }

        // the comparison of the whole value is a member of the algebra of the kind of a number, which the
        // storage variant of a matrix does not reach, so it keeps the value of the comparison alone
        if (!storeVariant)
        {
            foreach (var (name, op) in new[] { ("<", "<"), ("<=", "<="), (">", ">"), (">=", ">=") })
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static bool IComparisonOperators<{type}, {type}, bool>.operator {name}({type} left, {type} right) => " +
                              $"left.CompareTo(right) {op} 0;");
                sb.AppendLine();
            }
        }

        // the arithmetic of a matrix is the arithmetic of its columns, the storage variant of a matrix holds
        // the columns of a value of the kind of it and does not reach it
        if (!storeVariant)
        {
            foreach (var (name, op) in new[]
                     {
                         ("+", "+"), ("-", "-"), ("*", "*"), ("/", "/"), ("%", "%")
                     })
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    public static {type} operator {name}({type} left, {type} right) => " +
                              $"new({VectorGenShared.Join(cols, i => $"left.c{i} {op} right.c{i}")});");
                sb.AppendLine();
            }

            // the algebra of a kind that names the type of a single component multiplies a value by the value of a
            // single component and the value of a single component by a value, so a matrix of a number reaches
            // both of them, every column of the matrix multiplies the value of the component
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator *({type} value, {scalar} scalar) => " +
                          $"new({VectorGenShared.Join(cols, i => $"value.c{i} * scalar")});");
            sb.AppendLine();

            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator *({scalar} scalar, {type} value) => " +
                          $"new({VectorGenShared.Join(cols, i => $"scalar * value.c{i}")});");
            sb.AppendLine();

            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public static {type} operator +({type} value) => " +
                          $"new({VectorGenShared.Join(cols, i => $"+value.c{i}")});");
            sb.AppendLine();
            if (typ.sig)
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    public static {type} operator -({type} value) => " +
                              $"new({VectorGenShared.Join(cols, i => $"-value.c{i}")});");
                sb.AppendLine();
            }

            foreach (var name in new[] { "<<", ">>", ">>>" })
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    public static {type} operator {name}({type} left, int right) => " +
                              $"new({VectorGenShared.Join(cols, i => $"left.c{i} {name} right")});");
                sb.AppendLine();
            }
        }

        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // the conversions of the kind of a component: a matrix of a number reaches the matrices of the kinds the
        // kind of a component of it reaches and every column of the value is converted by the conversion of the
        // vector that the column is
        var convMembers = GenConv(typ, rows, cols, storeVariant);
        if (convMembers != null)
        {
            sb.AppendLine("    #region conv");
            sb.AppendLine();
            sb.Append(convMembers.Trim('\r', '\n'));
            sb.AppendLine();
            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        sb.AppendLine("    #region str");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Formats the matrix as the columns of it</summary>");
        sb.AppendLine($"    {attr}");
        var text = VectorGenShared.Join(cols, i => "{c" + i + "}", ", ");
        sb.AppendLine("    public readonly override string ToString() => $\"(" + text + ")\";");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        var textFormat = VectorGenShared.Join(cols, i => "{c" + i + ".ToString(format, formatProvider)}", ", ");
        sb.AppendLine("    public readonly string ToString(string? format, IFormatProvider? formatProvider) => " +
                      "$\"(" + textFormat + ")\";");
        sb.AppendLine();

        // the text of a matrix is the text of its columns, every part of it is written into the destination
        // without a string in between, so a part that does not fit leaves the count at zero and fails
        List<string> Parts(string suffix)
        {
            var calls = new List<string>
            {
                $"FormatUtils.TryFormatPart(ref d, ref n, \"(\"{suffix})",
            };
            for (var i = 0; i < cols; i++)
            {
                if (i != 0) calls.Add($"FormatUtils.TryFormatPart(ref d, ref n, \", \"{suffix})");
                calls.Add($"FormatUtils.TryFormatPart(ref d, ref n, c{i}, format, provider)");
            }

            calls.Add($"FormatUtils.TryFormatPart(ref d, ref n, \")\"{suffix})");
            return calls;
        }

        void EmitTryFormat(string span, string suffix)
        {
            var calls = Parts(suffix);
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    public readonly bool TryFormat({span} destination, out int charsWritten, " +
                          "ReadOnlySpan<char> format, IFormatProvider? provider)");
            sb.AppendLine("    {");
            sb.AppendLine("        var n = 0;");
            sb.AppendLine("        var d = destination;");
            var cond = new StringBuilder();
            for (var i = 0; i < calls.Count; i++)
            {
                cond.Append(i == 0 ? $"if (!{calls[i]}" : $"\n            || !{calls[i]}");
            }

            cond.Append(")");
            sb.AppendLine($"        {cond}");
            sb.AppendLine("        {");
            sb.AppendLine("            charsWritten = 0;");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        charsWritten = n;");
            sb.AppendLine("        return true;");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        EmitTryFormat("Span<char>", "");
        EmitTryFormat("Span<byte>", "u8");
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // the shape of a matrix is a part of its type, so the type itself reaches the member of the visitor that
        // matches the shape, which the member reaches the columns of the matrix through. The members are
        // implemented explicitly, so a caller reaches them through the interface of the dispatch of the type. The
        // storage variant of a matrix holds the columns of a value of the kind of it, it does not dispatch
        if (!storeVariant)
        {
            sb.AppendLine("    #region dispatch");
            sb.AppendLine();

            // the kind of the component decides the level of the members of the visitor that the value reaches:
            // the members of the level of a floating point number fall back to the ones of the level of a number
            // and those fall back to the ones that reach every value
            var level = VectorGenShared.DispatchLevel(typ);
            // the member of the visitor that reaches the value of a matrix names the type of a column of it,
            // which the count of the columns of the matrix decides, so the shape of the matrix is a part of the
            // name of the member and the row of the matrix reaches the same member as the matrix of the
            // transposed shape
            var matrix = $"V.MatrixMx{cols}_{level}<{type}, {col}, {scalar}>";
            // the members that take the value alone are the members of the interface that names the type of the
            // value, every other member takes a component of the value beside it or reaches the one it was given
            var self = VectorGenShared.DispatchIface(type);
            var withScalar = VectorGenShared.DispatchIfaceScalar(type, scalar);

            // a member of the dispatch of a value is implemented explicitly, so a member does not name the
            // constraints of the type of a component of it again and the value of a member of every kind is the
            // same one
            void Member(string ret, string iface, string member, string parameters, string body)
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {ret} {iface}.{member}({parameters})");
                sb.AppendLine($"        => {body};");
                sb.AppendLine();
            }

            Member(type, self, "Self<V>", $"{type} self", $"{matrix}(self)");
            Member(type, self, "Self<V>", $"{type} a, {type} b", $"{matrix}(a, b)");
            Member(type, self, "Self<V>", $"{type} a, {type} b, {type} c", $"{matrix}(a, b, c)");
            Member(type, withScalar, "Self<V>", $"{type} a, {scalar} b", $"{matrix}(a, b)");
            Member(type, withScalar, "Self<V>", $"{type} a, {scalar} b, {scalar} c", $"{matrix}(a, b, c)");
            // a member that takes a component of the value alone does not reach the value of the matrix at all
            Member(scalar, withScalar, "Scalar<V>", $"{scalar} a", $"V.Scalar_{level}(a)");
            Member(scalar, withScalar, "Scalar<V>", $"{scalar} a, {scalar} b", $"V.Scalar_{level}(a, b)");
            Member(scalar, withScalar, "Scalar<V>", $"{scalar} a, {scalar} b, {scalar} c", $"V.Scalar_{level}(a, b, c)");
            // a member that reduces the value of the matrix to a single component of it
            Member(scalar, withScalar, "Scalar<V>", $"{type} a", $"{matrix}(a)");
            Member(scalar, withScalar, "Scalar<V>", $"{type} a, {type} b", $"{matrix}(a, b)");
            Member(scalar, withScalar, "Combine<V>", $"{scalar} a, {scalar} b", $"V.Combine_{level}(a, b)");
            // the value of the matrix hands itself over instead of a component of it, so the visitor decides the
            // type of a component of the value it reaches: a matrix is made of its columns, so every column of
            // it is mapped and the matrix is built out of the values the visitor returned
            Member(type, self, "Map_Self<V>", $"{type} self",
                $"new({VectorGenShared.Join(cols, j => $"V.Map_{level}<{col}, {scalar}>(self.c{j})")})");
            // a member that reaches the bool value of the matrix hands the value over the same way every other
            // member that takes the value alone does: the visitor reaches the shape of the matrix and builds the
            // bool value of every column of it out of the columns
            Member("bool", self, "Bool<V>", $"{type} self", $"{matrix}(self)");
            // a member that takes a component of the matrix alone does not reach the value at all, so the visitor
            // reaches the component of every value of the kind of it the same way
            Member("bool", withScalar, "Bool<V>", $"{scalar} a", $"V.Scalar_{level}(a)");

            // the members that build two values out of the one they are handed are written for a floating point
            // kind alone, so a matrix of another kind does not reach them: the value of the matrix is handed over
            // the way every other member that takes the value alone hands it over and the value of a component
            // of it reaches the member of the scalar of the visitor
            if (typ.f)
            {
                Member("void", VectorGenShared.DispatchFloatIface(type), "Self_out<V>",
                    $"{type} a, out {type} b, out {type} c", $"{matrix}(a, out b, out c)");
                Member("void", VectorGenShared.DispatchFloatIfaceScalar(type, scalar), "Scalar_out<V>",
                    $"{scalar} a, out {scalar} b, out {scalar} c", "V.Scalar_Float(a, out b, out c)");
            }

            // the vectors a matrix is made of are the columns of it, so the member that reduces them to a
            // single vector is the one of the count of the columns of the matrix
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {col} Algebras.Generics.Dispatch.IMatrixColumnDispatch<{type}, {col}>.Combine<V>({type} self)");
            sb.AppendLine($"        => V.Combine{cols}_{level}<{col}, {scalar}>({VectorGenShared.Join(cols, j => $"self.c{j}")});");
            sb.AppendLine();
            // a row of the matrix has no value of its own, so the columns of it are handed over and every one
            // of them is reduced to a component by the visitor, which builds the vector of the reductions
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {row} Algebras.Generics.Dispatch.IMatrixRowDispatch<{type}, {row}>.Reduce<V>({type} self)");
            sb.AppendLine($"        => V.Row{cols}_{level}<{col}, {row}, {scalar}>({VectorGenShared.Join(cols, j => $"self.c{j}")});");
            sb.AppendLine();
            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// Generates the conversions of the matrix described by <paramref name="typ"/> into the matrices that have the
    /// same shape and another component type, and the storage variant of the matrix itself: the target of every
    /// conversion of <see cref="Typ.ExplicitConverts"/>
    /// and <see cref="Typ.ImplicitConverts"/> of the kind of a component. Every column of the value is converted by
    /// the conversion of the vector that the column is, which the vector of the kind of the target names. The
    /// storage variant of a matrix only converts into the storage variants of the same shape, so a conversion that
    /// only exists beside one of them is kept on the regular matrix.
    /// </summary>
    /// <param name="typ">The type of the component of the matrix</param>
    /// <param name="rows">The number of rows of the matrix</param>
    /// <param name="cols">The number of columns of the matrix</param>
    /// <param name="storeVariant">True for the storage variant of the matrix</param>
    /// <returns>The conversions of the matrix or null when it has none</returns>
    private static string? GenConv(Typ typ, int rows, int cols, bool storeVariant)
    {
        var targets = VectorGenShared.ConvTargets(typ);
        targets.RemoveAll(a => storeVariant && !VectorGenShared.HasStorageVariant(a.Target, rows));

        var type = Name(typ, rows, cols, storeVariant);
        // the conversion between the matrix and its storage variant is emitted beside the numeric ones
        var store = !storeVariant && VectorGenShared.HasStorageVariant(typ, rows) ? Name(typ, rows, cols, true) : null;
        var regular = storeVariant ? Name(typ, rows, cols, false) : null;
        if (targets.Count == 0 && store == null && regular == null) return null;

        var sb = new StringBuilder();

        foreach (var (kind, target) in targets)
        {
            var targetType = Name(target, rows, cols, storeVariant);
            var targetCol = VectorGenShared.VecName(target, rows, storeVariant);
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static {kind} operator {targetType}({type} self) =>");
            sb.AppendLine($"        new({VectorGenShared.Join(cols, i => $"({targetCol})self.c{i}")});");
            sb.AppendLine();
        }

        // the conversion between the matrix and its storage variant is implicit and it goes through the columns
        // of the value, which convert themselves
        var columns = VectorGenShared.Join(cols, i => $"self.c{i}");

        if (store != null)
        {
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static implicit operator {store}({type} self) => new({columns});");
            sb.AppendLine();
        }

        if (regular != null)
        {
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    public static implicit operator {regular}({type} self) => new({columns});");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
