using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the interfaces of the visitors of the algebra dispatch. A visitor reaches the value of every shape:
/// the members of it are the member of a scalar, the members of a vector of every width of a register and every
/// count of a component and the members of a matrix of every shape. The member of a shape is virtual, every one
/// of them reaches the member of a scalar for the component of a shape that is handed over as a value and
/// reaches the member of the columns of a matrix, so a visitor only implements the member of a scalar and the
/// members of a register. A visitor that reduces the value of its arguments to a single component adds the member
/// that combines the results of two of them: the member of a vector that has no register of it is abstract,
/// because the way the reductions of the components of the vector combine is the one of the operation of the
/// visitor, and the member of a matrix is the one of every column of it combined. The number of the values that a
/// member of a visitor takes decides the interface of it: the name of it names the value that a member returns
/// beside the value of every argument, so the interface of one value is
/// <c>INumberAlgebraVisitor_Self_Self&lt;V&gt;</c>, the one of three values has one more <c>Self</c> and the one
/// of a visitor that reduces its single value to a single component is
/// <c>INumberAlgebraVisitor_Self_Scalar&lt;V&gt;</c>. The interface of the dispatch of an algebra is written by
/// hand, the interfaces of the visitors are emitted into one file per shape.
/// <para>The visitors of a kind of a floating point number reach the values of the kind alone: the members of a
/// visitor of the kind of a number reach the members of the kind of a floating point number of a component of a
/// value as well, so the shapes that take a component of a value, the ones that reduce a value to a component
/// of it and the ones that reduce the vectors a matrix is made of are the shapes of the kind of a number, and
/// every kind of a floating point number has the three shapes that take the values of the kind alone.</para>
/// <para>The result of a visitor is a value of the same kind as the one of its arguments, a single component of
/// a value or a vector of the algebra. Every visitor of a vector names the kind of it, so the visitors of a
/// vector are the one of the columns of a matrix, which is the vector a column of it is, and the one of the rows
/// of it, which is the vector a row of it is: a matrix is the only value that is made of vectors, so it is the
/// only one that hands its value over to them. The columns of a matrix are the vectors it is made of, so a
/// visitor of the columns combines them and a visitor of the rows reduces every one of them to a component and
/// builds the vector of the reductions.</para>
/// </summary>
[Generator]
public class AlgebraVisitorGenerator : IIncrementalGenerator
{
    /// <summary>The namespace the interfaces of the visitors are emitted into</summary>
    public const string Namespace = "Coplt.Mathematics.Algebras.Generics";

    /// <summary>The greatest number of the values a visitor of an algebra takes</summary>
    public const int MaxCount = 3;

    /// <summary>The value that the members of a visitor return</summary>
    public enum VisitorResult
    {
        /// <summary>A value of the same kind as the one of the arguments of the member, which is a <c>Self</c> of the name of the interface</summary>
        Self,

        /// <summary>A single component of a value, which is a <c>Scalar</c> of the name of the interface</summary>
        Scalar,

        /// <summary>A vector of the algebra that a column of a matrix is, which is a <c>ColumnVector</c> of the name of the interface</summary>
        ColumnVector,

        /// <summary>A vector of the algebra that a row of a matrix is, which is a <c>RowVector</c> of the name of the interface</summary>
        RowVector,
    }

    /// <summary>
    /// The kind of the algebra a visitor is emitted for: the number of the library, the floating point number of
    /// it and the one the ieee 754 standard names. The kind decides the name of the interface and the constraint
    /// of the type of a single component, which is the one of the kind of it, so the members of a visitor of a
    /// floating point number reach the members of the floating point kind of the scalar.
    /// </summary>
    public enum VisitorKind
    {
        /// <summary>The number of the algebra library</summary>
        Number,

        /// <summary>A floating point number</summary>
        FloatingPoint,

        /// <summary>The floating point number the ieee 754 standard names</summary>
        Ieee754,
    }

    /// <summary>The kinds of every visitor family, the one of a number is the first of them</summary>
    public static readonly VisitorKind[] Kinds =
    {
        VisitorKind.Number,
        VisitorKind.FloatingPoint,
        VisitorKind.Ieee754,
    };

    /// <summary>
    /// Returns the name of the family of the visitor interfaces of a kind, the name of the interface of a shape
    /// is the one of it with the name of the shape behind it.
    /// </summary>
    /// <param name="kind">The kind of the algebra</param>
    /// <returns>The name of the family</returns>
    public static string VisitorFamily(VisitorKind kind) => kind switch
    {
        VisitorKind.FloatingPoint => "IFloatingPointAlgebraVisitor",
        VisitorKind.Ieee754 => "IFloatingPointIeee754AlgebraVisitor",
        _ => "INumberAlgebraVisitor",
    };

    /// <summary>
    /// Returns the name of the interface of the dispatch of a kind, which the members of the visitors of it name
    /// as the constraint of the values they reach.
    /// </summary>
    /// <param name="kind">The kind of the algebra</param>
    /// <returns>The name of the interface of the dispatch</returns>
    public static string VisitorDispatch(VisitorKind kind) => kind switch
    {
        VisitorKind.FloatingPoint => "IFloatingPointAlgebraDispatch",
        VisitorKind.Ieee754 => "IFloatingPointIeee754AlgebraDispatch",
        _ => "INumberAlgebraDispatch",
    };

    /// <summary>
    /// Returns the constraints the type of a single component of the visitors of a kind carries beside the one of
    /// a binary number, which is empty for a number of the library and the one of the kind of it for a floating
    /// point number.
    /// </summary>
    /// <param name="kind">The kind of the algebra</param>
    /// <returns>The constraints behind the one of the binary number</returns>
    public static string ScalarConstraints(VisitorKind kind) => kind switch
    {
        VisitorKind.FloatingPoint => ", IFloatingPoint<TScalar>",
        VisitorKind.Ieee754 => ", IFloatingPointIeee754<TScalar>",
        _ => "",
    };

    /// <summary>
    /// The shapes of the visitors: the kind of every argument of one of them, a <c>false</c> for an argument
    /// that is a value of the same kind and a <c>true</c> for one that is a single component of the value, and
    /// the kind of the result of the members of it, which is a value of the same kind when the result is a
    /// <see cref="VisitorResult.Self"/>, a single component of it when it is a <see cref="VisitorResult.Scalar"/>,
    /// the vector a column of a matrix is when it is a <see cref="VisitorResult.ColumnVector"/> and the vector a
    /// row of it is when it is a <see cref="VisitorResult.RowVector"/>
    /// </summary>
    private static readonly (bool[] Args, VisitorResult Result)[] Shapes = BuildShapes();

    /// <summary>
    /// Tells whether the shape of a visitor takes the values of the kind of it alone: the shapes that take a
    /// component of a value beside it, the ones that reduce a value to a single component of it and the ones
    /// that reduce the vectors a matrix is made of are the shapes of the dispatch of the kind of a number, which
    /// reaches the members of the kind of a floating point number of a component of a value as well, so the
    /// dispatch of a floating point kind does not have them.
    /// </summary>
    /// <param name="shape">The shape of the visitor</param>
    /// <returns>True when the shape takes the values of the kind of it alone</returns>
    public static bool ReachesSelfOnly((bool[] Args, VisitorResult Result) shape)
    {
        if (shape.Result != VisitorResult.Self) return false;
        foreach (var component in shape.Args)
            if (component)
                return false;
        return true;
    }

    private static (bool[] Args, VisitorResult Result)[] BuildShapes()
    {
        var shapes = new List<(bool[], VisitorResult)>(MaxCount + 7);
        // the members of every count of the values, every one of them is a value of the same kind and the
        // result of them is a value of the same kind as well
        for (var count = 1; count <= MaxCount; count++) shapes.Add((new bool[count], VisitorResult.Self));
        // the members that take the components of the value beside it
        shapes.Add((new[] { false, true }, VisitorResult.Self));
        shapes.Add((new[] { false, true, true }, VisitorResult.Self));
        // the members that reduce the value and the value with another one of its kind to a single component
        shapes.Add((new[] { false }, VisitorResult.Scalar));
        shapes.Add((new[] { false, false }, VisitorResult.Scalar));
        // the members that reduce the vectors a matrix is made of to a single vector, which are the columns of
        // it for one of the axes and the rows of it for the other one
        shapes.Add((new[] { false }, VisitorResult.ColumnVector));
        shapes.Add((new[] { false }, VisitorResult.RowVector));
        return shapes.ToArray();
    }

    /// <summary>The letters that name the values of a visitor that takes more than one of them</summary>
    private static readonly string[] Letters = { "a", "b", "c" };

    /// <summary>The names of the components of a value of the algebra</summary>
    private static readonly string[] Components = { "x", "y", "z", "w" };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (var kind in Kinds)
            {
                foreach (var shape in Shapes)
                {
                    // the dispatch of a floating point kind reaches the values of the kind of it alone, so the
                    // shapes of the visitors of it are the ones that take the values of the kind of it alone
                    if (kind != VisitorKind.Number && !ReachesSelfOnly(shape)) continue;
                    var name = VisitorName(shape.Args, shape.Result, kind);
                    ctx.AddSource(
                        $"{Namespace}.{name}.g.cs",
                        SourceText.From(Gen(kind, shape.Args, shape.Result, name), Encoding.UTF8));
                }
            }
        });
    }

    /// <summary>
    /// Returns the name of the visitor interface of a shape: every argument of it names a <c>Self</c> or a
    /// <c>Scalar</c>, the value a member returns names a <c>Self</c>, a <c>Scalar</c>, a <c>ColumnVector</c> or a
    /// <c>RowVector</c> behind the arguments, so the interface of one value is
    /// <c>INumberAlgebraVisitor_Self_Self</c>, the one of a value with a component of it is
    /// <c>INumberAlgebraVisitor_Self_Scalar_Self</c>, the one of a value that is reduced to a single component is
    /// <c>INumberAlgebraVisitor_Self_Scalar</c>, the one of a value that is reduced to the vector a column of it
    /// is is <c>INumberAlgebraVisitor_Self_ColumnVector</c> and the one of a value that is reduced to the vector
    /// a row of it is is <c>INumberAlgebraVisitor_Self_RowVector</c>.
    /// </summary>
    /// <param name="shape">The kind of every argument of the visitor</param>
    /// <param name="result">The kind of the value a member of the visitor returns</param>
    /// <param name="kind">The kind of the algebra the visitor is emitted for</param>
    /// <returns>The name of the interface</returns>
    public static string VisitorName(bool[] shape, VisitorResult result, VisitorKind kind = VisitorKind.Number)
    {
        var sb = new StringBuilder(VisitorFamily(kind));
        foreach (var component in shape) sb.Append(component ? "_Scalar" : "_Self");
        sb.Append(result switch
        {
            VisitorResult.Scalar => "_Scalar",
            VisitorResult.ColumnVector => "_ColumnVector",
            VisitorResult.RowVector => "_RowVector",
            _ => "_Self",
        });
        return sb.ToString();
    }

    private static string Gen(VisitorKind kind, bool[] shape, VisitorResult visitorResult, string name)
    {
        var reduce = visitorResult == VisitorResult.Scalar;
        var columnVector = visitorResult == VisitorResult.ColumnVector;
        var rowVector = visitorResult == VisitorResult.RowVector;
        var count = shape.Length;
        // the shape of an interface that takes a component of the value names the type of it, every member of
        // the interface names the same type, so the interface declares it and its members do not
        var component = false;
        for (var i = 0; i < count; i++)
            if (shape[i])
                component = true;

        // a visitor that takes a single value names it, one that takes more of them names them by the letters of
        // the alphabet
        var scalar = new string[count];
        var value = new string[count];
        for (var i = 0; i < count; i++)
        {
            scalar[i] = count == 1 ? "value" : Letters[i];
            value[i] = count == 1 ? "vector" : Letters[i];
        }

        string Join(Func<int, string> get)
        {
            var text = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                if (i != 0) text.Append(", ");
                text.Append(get(i));
            }

            return text.ToString();
        }

        // the count of the values of a join is not the one of the shape when it is the count of the columns of a
        // matrix, which is what the members of a visitor that returns a vector take
        string Columns(int columns, Func<int, string> get)
        {
            var text = new StringBuilder();
            for (var i = 0; i < columns; i++)
            {
                if (i != 0) text.Append(", ");
                text.Append(get(i));
            }

            return text.ToString();
        }

        // the components of the value itself that follow the one of the argument that names it
        var components = "";
        for (var i = 1; i < count; i++) components += $", {scalar[i]}";

        // the parameter of every argument of the shape: the one of a value is the value itself, the one of a
        // component is the type of it
        var scalarParameters = Join(i => $"TScalar {scalar[i]}");
        var vectorParameters = Join(i => shape[i] ? $"TScalar {scalar[i]}" : $"in TVector {value[i]}");
        var matrixParameters = Join(i => shape[i] ? $"TScalar {scalar[i]}" : $"in TMatrix {value[i]}");
        // the values of the shape, in the order of them
        var vectorValues = Join(i => shape[i] ? scalar[i] : value[i]);

        // the type parameters of a member: the member of a value that is not the one of the interface declares
        // the type of a component as well, the one of an interface that takes a component names the type the
        // interface declares
        var scalarTypeParameter = component ? "" : "<TScalar>";
        var vectorTypeParameter = component ? "<TVector>" : "<TVector, TScalar>";
        var matrixTypeParameter = component ? "<TMatrix, TVector>" : "<TMatrix, TVector, TScalar>";
        var matrixTypeArguments = component ? "TMatrix, TVector" : "TMatrix, TVector, TScalar";
        var visitorDispatch = VisitorDispatch(kind);
        // the dispatch of the kind of a number reaches the values of the kind of it and the ones that take a
        // component of the value beside it, so it names the type of a component as well, and the one of the kind
        // of a floating point number reaches the values of the kind of it alone, so the shape of a member that
        // names a component of the value is the shape of the kind of a number
        var namesScalar = component && kind == VisitorKind.Number;
        var dispatch = namesScalar ? $"{visitorDispatch}<TVector, TScalar>" : $"{visitorDispatch}<TVector>";
        var matrixDispatch = namesScalar ? $"{visitorDispatch}<TMatrix, TScalar>" : $"{visitorDispatch}<TMatrix>";
        // the members of a visitor that reduces the values of its arguments return a single component of the
        // value instead of a value of the same kind
        var result = reduce ? "TScalar" : "TVector";
        var matrixResult = reduce ? "TScalar" : "TMatrix";

        // the reduction of a column of a matrix, the value of it reaches the member of the vector of its own kind
        string ReducedColumn(string column) => component
            ? $"TVector.Visit_Scalar<V>(TMatrix.get_{column}({value[0]}){components})"
            : $"TVector.Visit_Scalar<V>({Join(i => $"TMatrix.get_{column}({value[i]})")})";

        // a reduction of a column of a matrix hands the register of the column over, so the vector of the
        // member names the type of a component of the value as well
        var matrixVectorDispatch = reduce ? $"{visitorDispatch}<TVector, TScalar>" : dispatch;

        // a visitor of the columns of a matrix and the one of the rows of it name the type of a component of the
        // value as well, which is the type of a component of every vector the matrix is made of
        var doubleDispatch = $"{visitorDispatch}<TVector, TScalar>";
        // a visitor of the rows takes the columns of the matrix, so it names the type of a column of it
        var columnDispatch = $"{visitorDispatch}<TColumn, TScalar>";

        string RegisterParameters(int width) =>
            Join(i => shape[i] ? $"TScalar {scalar[i]}" : $"in Vector{width}<TScalar> {value[i]}");

        // the component of every argument of the shape: the one of the value itself and the components that
        // follow it
        string ComponentValues(string c) => Join(i => shape[i] ? scalar[i] : $"TVector.get_{c}({value[i]})");

        // the columns of a matrix are reached through the dispatch of a value of the count of this visitor
        string ColumnValue(string column) => component
            ? $"TVector.Visit_Self<V>(TMatrix.get_{column}({value[0]}){components})"
            : $"TVector.Visit_Self<V>({Join(i => $"TMatrix.get_{column}({value[i]})")})";

        // the constraint of the member of a component: the interface of a shape that takes a component of the
        // value declares it, every member of an interface of values declares it itself
        var scalarConstraint = "        where TScalar : unmanaged, IBinaryNumber<TScalar>" + ScalarConstraints(kind);

        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine($"namespace {AlgebraVisitorGenerator.Namespace};");
        sb.AppendLine();
        // a visitor of the columns of a matrix and the one of the rows of it are the only ones whose result is
        // not a value of the same kind as the one of their arguments and not a component of it either
        if (columnVector)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// The visitor that reduces the columns of the value to a single vector");
            sb.AppendLine("/// <para>The columns of a matrix are handed over as the vectors they are, so the member of a");
            sb.AppendLine("/// count of the columns is the one that reaches them and the member that combines two of the");
            sb.AppendLine("/// vectors they are decides what the reduction of the columns is</para>");
            sb.AppendLine("/// </summary>");
        }
        else if (rowVector)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// The visitor that reduces the rows of the value to a single vector");
            sb.AppendLine("/// <para>The rows of a matrix have no value of their own, so the columns of it are handed over as");
            sb.AppendLine("/// the vectors they are and every one of them is reduced to a single component: the member of a");
            sb.AppendLine("/// count of the columns is the one that reaches them and the values of the reductions are the");
            sb.AppendLine("/// components of the vector it builds</para>");
            sb.AppendLine("/// </summary>");
        }

        if (component)
        {
            sb.AppendLine($"public interface {name}<V, TScalar>");
            sb.AppendLine($"    where V : {name}<V, TScalar>");
            sb.AppendLine("    where TScalar : unmanaged, IBinaryNumber<TScalar>" + ScalarConstraints(kind));
        }
        else
        {
            sb.AppendLine($"public interface {name}<V> where V : {name}<V>");
        }

        sb.AppendLine("{");

        // the columns of a matrix are handed over as the vectors they are, so a visitor of the columns is the
        // one that combines them and the member of the count of the columns holds the way two of them combine
        if (columnVector)
        {
            sb.AppendLine("    public static abstract TVector AcceptCombine<TVector, TScalar>(in TVector a, in TVector b)");
            sb.AppendLine($"        where TVector : unmanaged, {doubleDispatch}, INumberVector<TVector, TScalar>");
            sb.AppendLine(scalarConstraint + ";");
            sb.AppendLine();

            for (var columns = 2; columns <= 4; columns++)
            {
                sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
                sb.AppendLine($"    public static virtual TVector AcceptMatrixColumns{columns}<TVector, TScalar>({Columns(columns, i => $"in TVector c{i}")})");
                sb.AppendLine($"        where TVector : unmanaged, {doubleDispatch}, INumberVector<TVector, TScalar>");
                sb.AppendLine(scalarConstraint);
                if (columns == 2)
                {
                    sb.AppendLine("        => V.AcceptCombine<TVector, TScalar>(c0, c1);");
                }
                else
                {
                    sb.AppendLine("    {");
                    sb.AppendLine("        var r = V.AcceptCombine<TVector, TScalar>(c0, c1);");
                    for (var i = 2; i < columns; i++)
                    {
                        sb.AppendLine($"        r = V.AcceptCombine<TVector, TScalar>(r, c{i});");
                    }

                    sb.AppendLine("        return r;");
                    sb.AppendLine("    }");
                }

                sb.AppendLine();
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        // the rows of a matrix have no value of their own, so a visitor of the rows is the one that reduces
        // every column of the value to a component and builds the vector of the reductions
        if (rowVector)
        {
            for (var columns = 2; columns <= 4; columns++)
            {
                sb.AppendLine($"    public static abstract TRow AcceptMatrixRow{columns}<TColumn, TRow, TScalar>({Columns(columns, i => $"in TColumn c{i}")})");
                sb.AppendLine($"        where TColumn : unmanaged, {columnDispatch}, INumberVector<TColumn, TScalar>");
                sb.AppendLine($"        where TRow : unmanaged, INumberVector<TRow, TScalar>, IVector{columns}<TRow, TScalar>");
                sb.AppendLine(scalarConstraint + ";");
                sb.AppendLine();
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        // the member of a scalar, every member of a shape reaches it for the component of a shape that is handed
        // over as a value
        sb.AppendLine(component
            ? $"    public static abstract TScalar AcceptScalar({scalarParameters});"
            : $"    public static abstract TScalar AcceptScalar<TScalar>({scalarParameters})");
        if (!component) sb.AppendLine(scalarConstraint + ";");
        sb.AppendLine();

        // a visitor that reduces the value of its arguments to a single component combines the results of two of
        // them: the value of a matrix is the one of its columns, so the reduction of it is the one of every
        // column of it combined
        if (reduce)
        {
            sb.AppendLine(component
                ? "    public static abstract TScalar AcceptCombine(TScalar a, TScalar b);"
                : "    public static abstract TScalar AcceptCombine<TScalar>(TScalar a, TScalar b)");
            if (!component) sb.AppendLine(scalarConstraint + ";");
            sb.AppendLine();
        }

        // the members of a register, the width of it is a part of the type of a vector
        foreach (var width in new[] { 64, 128, 256 })
        {
            sb.AppendLine($"    public static abstract {result} AcceptVector{vectorTypeParameter}({RegisterParameters(width)})");
            sb.AppendLine($"        where TVector : unmanaged, {dispatch}, INumberVector<TVector, TScalar>, IVector{width}Underlying<TVector>" + (component ? ";" : ""));
            if (!component) sb.AppendLine(scalarConstraint + ";");
            sb.AppendLine();
        }

        // the members of every count of a component, a value that is handed over as a vector reaches the member
        // of the scalar for every one of its components
        for (var size = 2; size <= 4; size++)
        {
            if (reduce)
            {
                // the values of the components of a vector that has no register are reduced by the operation of
                // the visitor itself, the way the results of them combine is the one of it
                sb.AppendLine($"    public static abstract TScalar AcceptVector{size}{vectorTypeParameter}({vectorParameters})");
                sb.AppendLine($"        where TVector : unmanaged, {dispatch}, INumberVector<TVector, TScalar>, IVector{size}<TVector, TScalar>" + (component ? ";" : ""));
                if (!component) sb.AppendLine(scalarConstraint + ";");
                sb.AppendLine();
                continue;
            }

            sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
            sb.AppendLine($"    public static virtual TVector AcceptVector{size}{vectorTypeParameter}({vectorParameters})");
            sb.AppendLine($"        where TVector : unmanaged, {dispatch}, INumberVector<TVector, TScalar>, IVector{size}<TVector, TScalar>");
            if (count == 1)
            {
                sb.AppendLine(scalarConstraint);
                sb.AppendLine("    {");
                sb.AppendLine("        TVector r = default;");
                for (var c = 0; c < size; c++)
                {
                    sb.AppendLine($"        TVector.set_{Components[c]}(ref r, V.AcceptScalar(TVector.get_{Components[c]}({vectorValues})));");
                }

                sb.AppendLine("        return r;");
                sb.AppendLine("    }");
            }
            else
            {
                sb.AppendLine(component ? "        => TVector.Create(" : scalarConstraint + " => TVector.Create(");
                for (var c = 0; c < size; c++)
                {
                    sb.AppendLine($"        V.AcceptScalar({ComponentValues(Components[c])}){(c == size - 1 ? "" : ",")}");
                }

                sb.AppendLine("    );");
            }

            sb.AppendLine();
        }

        // the members of every count of the columns of a matrix, the number of the rows of it is not a part of
        // them: every column of the matrix reaches the member of the vector of its own kind
        for (var cols = 2; cols <= 4; cols++)
        {
            sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
            sb.AppendLine($"    public static virtual {matrixResult} AcceptMatrixMx{cols}{matrixTypeParameter}({matrixParameters})");
            sb.AppendLine(
                $"        where TMatrix : unmanaged, {matrixDispatch}, INumberMatrix<TMatrix, TScalar>, IMatrixMx{cols}Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>");
            sb.AppendLine($"        where TVector : unmanaged, {matrixVectorDispatch}, INumberVector<TVector, TScalar>");
            if (!component) sb.AppendLine(scalarConstraint);
            // the reduction of a matrix is the one of every column of it combined with the one of the next column
            if (reduce)
            {
                sb.AppendLine("    {");
                sb.AppendLine($"        var r = {ReducedColumn("c0")};");
                for (var c = 1; c < cols; c++)
                {
                    sb.AppendLine($"        r = V.AcceptCombine(r, {ReducedColumn($"c{c}")});");
                }

                sb.AppendLine("        return r;");
                sb.AppendLine("    }");
            }
            // the columns of a matrix of a visitor that takes a single value are short enough for a single line
            else if (cols == 2 && count == 1)
            {
                sb.AppendLine($"        => TMatrix.Create({ColumnValue("c0")}, {ColumnValue("c1")});");
            }
            else
            {
                sb.AppendLine("        => TMatrix.Create(");
                for (var c = 0; c < cols; c++)
                {
                    sb.AppendLine($"            {ColumnValue($"c{c}")}{(c == cols - 1 ? "" : ",")}");
                }

                sb.AppendLine("        );");
            }

            sb.AppendLine();
        }

        // the members of every shape, the number of the columns of the matrix decides the member of the count
        // that reaches the columns of it
        for (var rows = 2; rows <= 4; rows++)
        {
            for (var cols = 2; cols <= 4; cols++)
            {
                sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
                sb.AppendLine($"    public static virtual {matrixResult} AcceptMatrix{rows}x{cols}{matrixTypeParameter}({matrixParameters})");
                sb.AppendLine(
                    $"        where TMatrix : unmanaged, {matrixDispatch}, INumberMatrix<TMatrix, TScalar>, IMatrix{rows}x{cols}Vector<TMatrix, TVector>, IMatrix{rows}x{cols}Scalar<TMatrix, TScalar>");
                sb.AppendLine($"        where TVector : unmanaged, {matrixVectorDispatch}, INumberVector<TVector, TScalar>, IVector{rows}<TVector, TScalar>");
                if (!component) sb.AppendLine(scalarConstraint);
                sb.AppendLine($"        => V.AcceptMatrixMx{cols}<{matrixTypeArguments}>({vectorValues});");
                sb.AppendLine();
            }
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
