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
                        // a mask has no storage variant, every other component type has one for the shapes of
                        // the vectors that have one
                        var variants = !typ.bol && VectorGenShared.HasStorageVariant(typ, rows) ? 2 : 1;
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
    /// columns and the suffix of the storage variant. The name of a matrix of a mask differs by the letter of
    /// the kind of the value, a mask is a matrix of <c>m</c> and the vector of it is a vector of <c>v</c>.
    /// </summary>
    /// <param name="typ">The type of the component of the matrix</param>
    /// <param name="rows">The number of rows of the matrix</param>
    /// <param name="cols">The number of columns of the matrix</param>
    /// <param name="storeVariant">True for the storage variant of the matrix</param>
    /// <returns>The name of the matrix</returns>
    public static string Name(Typ typ, int rows, int cols, bool storeVariant) =>
        typ.bol
            ? $"{typ.name.Replace("v", "m")}{rows}x{cols}"
            : $"{typ.name}{rows}x{cols}{(storeVariant ? "s" : "")}";

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
        var bol = typ.bol;
        var zero = bol ? $"{col}.False" : $"{col}.Zero";
        var one = bol ? $"{col}.True" : $"{col}.One";
        var two = bol ? $"{col}.True" : $"{col}.Two";
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
        // a matrix of a number reaches the arithmetic of its kind, a matrix of a mask reaches its true and its
        // false instead, and every shape has the members of its own beside the ones every matrix has
        var ifaces = new List<string>
        {
            $"Algebras.IMatrix<{type}>",
            $"Algebras.IMatrixScalar<{type}, {scalar}>",
            $"Algebras.IMatrixVector<{type}, {col}>",
            $"Algebras.{(bol ? "IBoolMatrix" : typ.f ? typ.name == "half" ? "IFloatingPointMatrix" : "IFloatingPointIeee754Matrix" : typ.sig ? "ISignedNumberMatrix" : "INumberMatrix")}<{type}, {scalar}>",
            $"Algebras.IMatrix{shape}<{type}>",
            $"Algebras.IMatrix{shape}Vector<{type}, {col}>",
            $"Algebras.IMatrix{shape}Scalar<{type}, {scalar}>",
        };
        // a matrix of a number dispatches the value of it to a visitor and the vectors it is made of to the
        // visitors that reduce them to a vector, a matrix of a mask dispatches nothing
        if (!bol)
        {
            ifaces.Add($"Algebras.Generics.INumberAlgebraDispatch<{type}, {scalar}>");
            ifaces.AddRange(VectorGenShared.DispatchIfaces(typ)
                .Where(n => n != "INumberAlgebraDispatch")
                .Select(n => $"Algebras.Generics.{n}<{type}, {scalar}>"));
            ifaces.Add($"Algebras.Generics.INumberMatrixColumnDispatch<{type}, {col}>");
            ifaces.Add($"Algebras.Generics.INumberMatrixRowDispatch<{type}, {row}>");
        }
        VectorGenShared.FileHeader(sb, false);
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
        sb.AppendLine($"    public {type}({VectorGenShared.Join(cols, i => $"in {col} c{i}")})");
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
        sb.AppendLine($"    static {col} Algebras.IMatrixVector<{type}, {col}>.get_vector(in {type} self, int column) => " +
                      "self[column];");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static void Algebras.IMatrixVector<{type}, {col}>.set_vector(ref {type} self, int column, in {col} value) => " +
                      "self[column] = value;");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static {scalar} Algebras.IMatrixScalar<{type}, {scalar}>.get(in {type} self, int row, int column) => " +
                      "self[row, column];");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static void Algebras.IMatrixScalar<{type}, {scalar}>.set(ref {type} self, int row, int column, {scalar} value) => " +
                      "self[row, column] = value;");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static {scalar} Algebras.IAlgebra<{type}, {scalar}>.get(in {type} self, int index) => " +
                      $"self[index / {rows}, index % {rows}];");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static void Algebras.IAlgebra<{type}, {scalar}>.set(ref {type} self, int index, {scalar} value) => " +
                      $"self[index / {rows}, index % {rows}] = value;");
        sb.AppendLine();
        sb.AppendLine("    #endregion");
        sb.AppendLine();

        // a matrix is the value of its columns, so a scalar reaches the first component of the first column and
        // a vector fills every column with the value it holds
        sb.AppendLine("    #region view");
        sb.AppendLine();
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
        var zeroMatrix = $"new {type}({VectorGenShared.Join(cols, _ => zero)})";
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
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static {type} Broadcast(in {col} scalar) => " +
                      $"new({VectorGenShared.Join(cols, _ => "scalar")});");
        sb.AppendLine();
        Method($"public static {type} Vector(in {col} scalar)", $"var r = {zeroMatrix}; r[0] = scalar; return r;");
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
        // a mask reaches its true and its false instead of the zero and the one of a number
        if (bol)
        {
            Prop("A matrix whose columns are the vector of the true of its component type",
                "public static " + type + " True", $"new({VectorGenShared.Join(cols, _ => $"{col}.True")})");
            Prop("A matrix whose columns are the vector of the false of its component type",
                "public static " + type + " False", $"new({VectorGenShared.Join(cols, _ => $"{col}.False")})");
        }
        else
        {
            Prop("A matrix whose columns are the vector of the zero of its component type",
                "public static " + type + " Zero", $"new({VectorGenShared.Join(cols, _ => $"{col}.Zero")})");
            Prop("A matrix whose columns are the vector of the one of its component type",
                "public static " + type + " One", $"new({VectorGenShared.Join(cols, _ => $"{col}.One")})");
            Prop("A matrix whose columns are the vector of the two of its component type",
                "public static " + type + " Two", $"new({VectorGenShared.Join(cols, _ => $"{col}.Two")})");
            Prop("The zero of the component type of the matrix", $"public static {scalar} ScalarZero", "default");
            Prop("The one of the component type of the matrix", $"public static {scalar} ScalarOne", typ.one);
            Prop("The two of the component type of the matrix", $"public static {scalar} ScalarTwo",
                $"({scalar})({typ.two})");
        }

        // a matrix of a floating point number reaches the math constants of the algebra of its kind: every
        // component of it is the constant, which is the one of every column of it
        if (!bol && typ.f)
        {
            // the constants of the standard are the ones of a component type that names the standard itself
            var consts = typ.name == "half"
                ? VectorGenerator.FloatConsts.Select(c => c.Name)
                : VectorGenerator.FloatConsts.Select(c => c.Name).Concat(VectorGenerator.IeeeConsts);
            foreach (var name in consts)
            {
                Prop($"The {name} of the component type of the matrix", $"public static {scalar} Scalar{name}",
                    $"{col}.Scalar{name}");
                Prop($"A matrix whose every component is the {name} of the kind of it",
                    $"public static {type} {name}", $"new({VectorGenShared.Join(cols, _ => $"{col}.{name}")})");
            }
        }

        Prop("The zero of the vector of a column of the matrix", $"public static {col} VectorZero", zero);
        Prop("The one of the vector of a column of the matrix", $"public static {col} VectorOne", one);
        Prop("The two of the vector of a column of the matrix", $"public static {col} VectorTwo", two);
        sb.AppendLine("    /// <summary>The identity of the matrix, it is the value that keeps a matrix unchanged</summary>");
        sb.AppendLine($"    public static {type} Identity");
        sb.AppendLine("    {");
        sb.AppendLine($"        {attr}");
        sb.AppendLine("        get");
        sb.AppendLine("        {");
        sb.AppendLine($"            var r = {zeroMatrix};");
        for (var j = 0; j < rows && j < cols; j++)
        {
            // a mask reaches the true of a column instead of the one of a number
            sb.AppendLine($"            r[{j}, {j}] = {(bol ? $"{col}.True[{j}]" : "ScalarOne")};");
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
                      $"{VectorGenShared.Join(cols, i => $"in {col} c{i}")}) => " +
                      $"new({VectorGenShared.Join(cols, i => $"c{i}")});");
        sb.AppendLine();
        // the components of the matrix reach the constructor of it, which takes them in the order of a row
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    static {type} Algebras.IMatrix{shape}Scalar<{type}, {scalar}>.Create(" +
                      $"{string.Join(", ", scalarArgs)}) => new({string.Join(", ", scalarNames)});");
        sb.AppendLine();
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
                sb.AppendLine($"    static {col} Algebras.IMatrixMx{cols}Vector<{type}, {col}>.get_c{j}(in {type} self) => " +
                              $"self.c{j};");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static void Algebras.IMatrixMx{cols}Vector<{type}, {col}>.set_c{j}(ref {type} self, in {col} value) => " +
                              $"self.c{j} = value;");
                sb.AppendLine();
            }

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    sb.AppendLine("    /// <inheritdoc/>");
                    sb.AppendLine($"    {attr}");
                    sb.AppendLine($"    static {scalar} Algebras.IMatrix{shape}Scalar<{type}, {scalar}>.get_m{r}{c}(in {type} self) => " +
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
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static bool operator ==({type} left, {type} right) => left.Equals(right);");
        sb.AppendLine();
        sb.AppendLine("    /// <inheritdoc/>");
        sb.AppendLine($"    {attr}");
        sb.AppendLine($"    public static bool operator !=({type} left, {type} right) => !left.Equals(right);");
        sb.AppendLine();
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
        if (!bol)
        {
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
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    int IComparable.CompareTo(object? obj) => obj is {type} other");
            sb.AppendLine($"        ? CompareTo(other)");
            sb.AppendLine($"        : throw new ArgumentException(null, nameof(obj));");
            sb.AppendLine();
            foreach (var (name, op) in new[] { ("<", "<"), ("<=", "<="), (">", ">"), (">=", ">=") })
            {
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    public static bool operator {name}({type} left, {type} right) => " +
                              $"left.CompareTo(right) {op} 0;");
                sb.AppendLine();
            }

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
        // matches the shape, which the visitor reaches the columns of the matrix through. The members are
        // implemented explicitly, so a caller reaches them through the interface of the dispatch of the type
        if (!bol)
        {
            sb.AppendLine("    #region dispatch");
            sb.AppendLine();
            // the members of the dispatch of every kind of the value are the ones of the same shape: the
            // interface is the one of the kind, and a member that is implemented explicitly does not name the
            // constraints of the type of a component of it again, so the value of a member of every kind is
            // the same one
            foreach (var family in VectorGenShared.DispatchIfaces(typ))
            {
                // the interface of a value of the kind that takes the components of it
                var withScalar = $"Algebras.Generics.{family}<{type}, {scalar}>";
                // the one of the value of a number of the kind has a member that takes no component of it, the
                // one of a floating point number does not have it
                var self = family == "INumberAlgebraDispatch" ? $"Algebras.Generics.{family}<{type}>" : withScalar;

                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {type} {self}.Visit_Self<V>(in {type} self)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}, {scalar}>(self);");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {type} {self}.Visit_Self<V>(in {type} a, in {type} b)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}, {scalar}>(a, b);");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {type} {self}.Visit_Self<V>(in {type} a, in {type} b, in {type} c)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}, {scalar}>(a, b, c);");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {type} {withScalar}.Visit_Self<V>(in {type} a, {scalar} b)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}>(a, b);");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {type} {withScalar}.Visit_Self<V>(in {type} a, {scalar} b, {scalar} c)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}>(a, b, c);");
                sb.AppendLine();
                // a visitor that returns a single component reaches the columns of the matrix as well: the
                // reduction of the value of a matrix is the one of every column of it combined
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {scalar} {withScalar}.Visit_Scalar<V>(in {type} self)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}, {scalar}>(self);");
                sb.AppendLine();
                sb.AppendLine("    /// <inheritdoc/>");
                sb.AppendLine($"    {attr}");
                sb.AppendLine($"    static {scalar} {withScalar}.Visit_Scalar<V>(in {type} a, in {type} b)");
                sb.AppendLine($"        => V.AcceptMatrix{shape}<{type}, {col}, {scalar}>(a, b);");
                sb.AppendLine();
            }
            // the vectors a matrix is made of are the columns of it, so the member that reduces them to a
            // single vector is the one of the count of the columns of the matrix
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {col} Algebras.Generics.INumberMatrixColumnDispatch<{type}, {col}>.Visit_Vector<V>(in {type} self)");
            sb.AppendLine($"        => V.AcceptMatrixColumns{cols}<{col}, {scalar}>({VectorGenShared.Join(cols, j => $"self.c{j}")});");
            sb.AppendLine();
            // a row of the matrix has no value of its own, so the columns of it are handed over and every one
            // of them is reduced to a component by the visitor, which builds the vector of the reductions
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine($"    {attr}");
            sb.AppendLine($"    static {row} Algebras.Generics.INumberMatrixRowDispatch<{type}, {row}>.Visit_Vector<V>(in {type} self)");
            sb.AppendLine($"        => V.AcceptMatrixRow{cols}<{col}, {row}, {scalar}>({VectorGenShared.Join(cols, j => $"self.c{j}")});");
            sb.AppendLine();
            sb.AppendLine("    #endregion");
            sb.AppendLine();
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
}
