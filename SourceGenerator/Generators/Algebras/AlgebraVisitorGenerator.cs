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
/// members of a register. The number of the values that a member of a visitor takes decides the interface of it:
/// the name of it names the value that a member returns beside the value of every argument, so the interface of
/// one value is <c>INumberAlgebraVisitor_Self_Self&lt;V&gt;</c> and the one of three values has one more
/// <c>Self</c>. The interface of the dispatch of an algebra is written by hand, the interfaces of the visitors
/// are emitted into one file per count of the values.
/// </summary>
[Generator]
public class AlgebraVisitorGenerator : IIncrementalGenerator
{
    /// <summary>The namespace the interfaces of the visitors are emitted into</summary>
    public const string Namespace = "Coplt.Mathematics.Algebras.Generics";

    /// <summary>The greatest number of the values a visitor of an algebra takes</summary>
    public const int MaxCount = 3;

    /// <summary>
    /// The shapes of the arguments of the visitors: the value of the algebra itself first, then every argument
    /// that follows it, a <c>false</c> for one that is a value of the same kind and a <c>true</c> for one that
    /// is a single component of the value
    /// </summary>
    private static readonly bool[][] Shapes = BuildShapes();

    private static bool[][] BuildShapes()
    {
        var shapes = new List<bool[]>(MaxCount + 2);
        // the members of every count of the values, every one of them is a value of the same kind
        for (var count = 1; count <= MaxCount; count++) shapes.Add(new bool[count]);
        // the members that take the components of the value beside it
        shapes.Add(new[] { false, true });
        shapes.Add(new[] { false, true, true });
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
            foreach (var shape in Shapes)
            {
                var name = VisitorName(shape);
                ctx.AddSource(
                    $"{Namespace}.{name}.g.cs",
                    SourceText.From(Gen(shape, name), Encoding.UTF8));
            }
        });
    }

    /// <summary>
    /// Returns the name of the visitor interface of a shape: every argument of it names a <c>Self</c> or a
    /// <c>Scalar</c> and the value a member returns names the last <c>Self</c> of the name, so the interface of
    /// one value is <c>INumberAlgebraVisitor_Self_Self</c> and the one of a value with a component of it is
    /// <c>INumberAlgebraVisitor_Self_Scalar_Self</c>.
    /// </summary>
    /// <param name="shape">The shape of the arguments of the visitor</param>
    /// <returns>The name of the interface</returns>
    public static string VisitorName(bool[] shape)
    {
        var sb = new StringBuilder("INumberAlgebraVisitor");
        foreach (var component in shape) sb.Append(component ? "_Scalar" : "_Self");
        sb.Append("_Self");
        return sb.ToString();
    }

    private static string Gen(bool[] shape, string name)
    {
        var count = shape.Length;
        // the shape of an interface that takes a component of the value names the type of it, every member of
        // the interface names the same type, so the interface declares it and its members do not
        var component = false;
        for (var i = 0; i < count; i++) if (shape[i]) component = true;

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
        // the dispatch of a value that takes the type of a component names the type of it as well
        var dispatch = component ? "INumberAlgebraDispatch<TVector, TScalar>" : "INumberAlgebraDispatch<TVector>";
        var matrixDispatch = component ? "INumberAlgebraDispatch<TMatrix, TScalar>" : "INumberAlgebraDispatch<TMatrix>";

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
        const string scalarConstraint = "        where TScalar : unmanaged, IBinaryNumber<TScalar>";

        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine($"namespace {AlgebraVisitorGenerator.Namespace};");
        sb.AppendLine();
        if (component)
        {
            sb.AppendLine($"public interface {name}<V, TScalar>");
            sb.AppendLine($"    where V : {name}<V, TScalar>");
            sb.AppendLine("    where TScalar : unmanaged, IBinaryNumber<TScalar>");
        }
        else
        {
            sb.AppendLine($"public interface {name}<V> where V : {name}<V>");
        }

        sb.AppendLine("{");

        // the member of a scalar, every member of a shape reaches it for the component of a shape that is handed
        // over as a value
        sb.AppendLine(component
            ? $"    public static abstract TScalar AcceptScalar({scalarParameters});"
            : $"    public static abstract TScalar AcceptScalar<TScalar>({scalarParameters})");
        if (!component) sb.AppendLine(scalarConstraint + ";");
        sb.AppendLine();

        // the members of a register, the width of it is a part of the type of a vector
        foreach (var width in new[] { 64, 128, 256 })
        {
            sb.AppendLine($"    public static abstract TVector AcceptVector{vectorTypeParameter}({RegisterParameters(width)})");
            sb.AppendLine($"        where TVector : unmanaged, {dispatch}, INumberVector<TVector, TScalar>, IVector{width}Underlying<TVector>" + (component ? ";" : ""));
            if (!component) sb.AppendLine(scalarConstraint + ";");
            sb.AppendLine();
        }

        // the members of every count of a component, a value that is handed over as a vector reaches the member
        // of the scalar for every one of its components
        for (var size = 2; size <= 4; size++)
        {
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
            sb.AppendLine($"    public static virtual TMatrix AcceptMatrixMx{cols}{matrixTypeParameter}({matrixParameters})");
            sb.AppendLine(
                $"        where TMatrix : unmanaged, {matrixDispatch}, INumberMatrix<TMatrix, TScalar>, IMatrixMx{cols}Vector<TMatrix, TVector>, IMatrixScalar<TMatrix, TScalar>");
            sb.AppendLine($"        where TVector : unmanaged, {dispatch}, INumberVector<TVector, TScalar>");
            if (!component) sb.AppendLine(scalarConstraint);
            // the columns of a matrix of a visitor that takes a single value are short enough for a single line
            if (cols == 2 && count == 1)
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
                sb.AppendLine($"    public static virtual TMatrix AcceptMatrix{rows}x{cols}{matrixTypeParameter}({matrixParameters})");
                sb.AppendLine(
                    $"        where TMatrix : unmanaged, {matrixDispatch}, INumberMatrix<TMatrix, TScalar>, IMatrix{rows}x{cols}Vector<TMatrix, TVector>, IMatrix{rows}x{cols}Scalar<TMatrix, TScalar>");
                sb.AppendLine($"        where TVector : unmanaged, {dispatch}, INumberVector<TVector, TScalar>, IVector{rows}<TVector, TScalar>");
                if (!component) sb.AppendLine(scalarConstraint);
                sb.AppendLine($"        => V.AcceptMatrixMx{cols}<{matrixTypeArguments}>({vectorValues});");
                sb.AppendLine();
            }
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
