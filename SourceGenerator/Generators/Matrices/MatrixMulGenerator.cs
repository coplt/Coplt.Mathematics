using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Coplt.Analyzers.Generators;

/// <summary>
/// Generates the members that multiply a matrix of <see cref="Typ"/> by another value, which are the product of
/// the matrix with a matrix, the product of the matrix with the vector of the columns of it and the product of
/// the vector of the rows of it with the matrix.
/// <para>The product of a matrix of <c>n</c> rows and <c>m</c> columns with a matrix of <c>m</c> rows and
/// <c>v</c> columns is the matrix of <c>n</c> rows and <c>v</c> columns whose column at the index of a column of
/// the second value is the sum of the columns of the first value scaled by the components of that column, so the
/// product of a matrix with a vector is the product of it with the vector as a matrix of a single column and the
/// product of a vector with a matrix is the product of the vector as a matrix of a single row with it.</para>
/// <para>A matrix is laid out by columns, so a column of it is a vector: the product of a matrix with a vector
/// scales every column of the matrix by the component of the vector at the same position, which is the product of
/// the matrix with the diagonal of the vector, and the product of a vector with a matrix is the vector of the dot
/// products of the value with the columns of the matrix. Both are the members of the operator of the value beside
/// the product of the value with a single component, which every matrix carries.</para>
/// <para>Every kind of a number that has arithmetic is reached. The unit of a sum of products is the fused
/// multiply and add of the kind of the value, which rounds the multiplication and the addition of it once. The
/// members are emitted into the type of the value and into the two classes of the members of the math class, the
/// one that names the type of the result where it is called and the one of the value itself, which every member
/// of the value of the library is emitted into.</para>
/// <para>The shape and the kind of the value name the file of the members: every member of a shape and a kind is
/// emitted into a file of its own, so a file holds the whole product of the value, whichever of the two values
/// of a product the value is. The value reaches the product of it with the matrix of the shapes the value
/// carries a column of alone, so the file of a shape holds the product of the value with every matrix whose rows
/// are the columns of the value and whose columns are from the count of the rows of the value to the count of
/// the columns of the value.</para>
/// </summary>
[Generator]
public class MatrixMulGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (var typ in Typ.Typs)
            {
                // only the kind of a number has a product
                if (!typ.arith) continue;

                for (var rows = 2; rows <= 4; rows++)
                {
                    for (var cols = 2; cols <= 4; cols++)
                    {
                        var matrix = MatrixGenerator.Name(typ, rows, cols, false);
                        // every member of the shape and the kind is emitted into the file of the shape, which
                        // the kind of a component of the value names as well
                        Add(ctx, matrix, Gen(typ, rows, cols));
                    }
                }
            }
        });
    }

    /// <summary>
    /// Adds the file of a shape and a kind, which the name of the matrix of the value names.
    /// </summary>
    /// <param name="ctx">The context of the generator</param>
    /// <param name="name">The name of the matrix of the shape and the kind</param>
    /// <param name="text">The file of the members</param>
    private static void Add(IncrementalGeneratorPostInitializationContext ctx, string name, string text) =>
        ctx.AddSource(
            $"{VectorGenerator.VecNamespace}.mul.{name}.g.cs",
            SourceText.From(text, Encoding.UTF8));

    /// <summary>
    /// Generates every member that multiplies the matrix of <paramref name="rows"/> rows and
    /// <paramref name="cols"/> columns of <paramref name="typ"/> by another value: the members of the operator of
    /// the value, the ones of the math class and the ones of the value itself.
    /// </summary>
    /// <param name="typ">The type of a component of the value</param>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <returns>The file of the members</returns>
    private static string Gen(Typ typ, int rows, int cols)
    {
        var matrix = MatrixGenerator.Name(typ, rows, cols, false);
        var vec = VectorGenShared.VecName(typ, cols, false);
        var res = VectorGenShared.VecName(typ, rows, false);
        // the second value of every product of the value with a matrix, which has the rows of the columns of the
        // value, the result of it, which has the rows of the value and the columns of the second value, and the
        // column of the result at the index of a column of the second value, which is the sum of the columns of
        // the value scaled by the components of that column
        var products = new List<(string Right, string Result, string[] Columns)>();
        for (var resCols = 2; resCols <= rows; resCols++)
        {
            var columns = new string[resCols];
            for (var j = 0; j < resCols; j++)
                columns[j] = Sum(cols, k => ($"a.c{k}", $"b.c{j}.{Repeated(k, rows)}"));
            products.Add((
                MatrixGenerator.Name(typ, cols, resCols, false),
                MatrixGenerator.Name(typ, rows, resCols, false),
                columns));
        }

        var sb = new StringBuilder();
        VectorGenShared.FileHeader(sb, simdHelpers: false);
        sb.AppendLine();

        sb.AppendLine($"public partial struct {matrix}");
        sb.AppendLine("{");
        sb.AppendLine(ColumnsOperator(rows, cols, matrix, vec));
        sb.AppendLine();
        sb.AppendLine(RowsOperator(rows, cols, matrix, res));
        sb.AppendLine("}");
        sb.AppendLine();

        sb.AppendLine("public static partial class math");
        sb.AppendLine("{");
        sb.AppendLine(ColumnsProduct(rows, cols, matrix, vec, res));
        sb.AppendLine();
        sb.AppendLine(RowsProduct(rows, cols, matrix, res, vec));
        foreach (var (right, result, columns) in products)
        {
            sb.AppendLine();
            sb.AppendLine(MatrixProduct(rows, cols, matrix, right, result, columns));
        }

        sb.AppendLine("}");
        sb.AppendLine();

        sb.AppendLine("public static partial class math_ex");
        sb.AppendLine("{");
        sb.AppendLine(Extension(matrix, vec, res));
        sb.AppendLine();
        sb.AppendLine(Extension(res, matrix, vec));
        foreach (var (right, result, _) in products)
        {
            sb.AppendLine();
            sb.AppendLine(Extension(matrix, right, result));
        }

        sb.AppendLine("}");
        return sb.ToString();
    }


    /// <summary>
    /// Returns the member of the operator of the value that scales the columns of the matrix of
    /// <paramref name="rows"/> rows and <paramref name="cols"/> columns by the components of the vector of the
    /// columns of it, which is the product of the value with the diagonal of the vector.
    /// </summary>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <param name="matrix">The name of the matrix</param>
    /// <param name="vec">The name of the vector of the columns of the matrix</param>
    /// <returns>The member</returns>
    private static string ColumnsOperator(int rows, int cols, string matrix, string vec)
    {
        // the column of the value at the index of a column of the vector scaled by the component of the vector
        // at the same index
        var scaled = VectorGenShared.Join(cols, j => $"a.c{j} * b.{Repeated(j, rows)}");
        var sb = new StringBuilder();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the value whose every column is the column of the value at the same position scaled by the");
        sb.AppendLine("    /// component of the vector at the same position");
        sb.AppendLine("    /// <para>The member scales the columns of a matrix by the components of a vector, which is the product");
        sb.AppendLine("    /// of the value with the diagonal of the vector.</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine($"    /// <param name=\"a\">The value, a matrix of {rows} rows and {cols} columns</param>");
        sb.AppendLine($"    /// <param name=\"b\">The vector that scales the columns of the value, a vector of {cols} components</param>");
        sb.AppendLine("    /// <returns>The matrix whose column at the index of a column of the value is that column scaled by the");
        sb.AppendLine("    /// component of the vector at the same index</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.Append($"    public static {matrix} operator *({matrix} a, {vec} b) => new({scaled});");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the member of the operator of the value that scales the rows of the matrix of
    /// <paramref name="rows"/> rows and <paramref name="cols"/> columns by the components of the vector of the
    /// rows of it.
    /// </summary>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <param name="matrix">The name of the matrix</param>
    /// <param name="vec">The name of the vector of the rows of the matrix</param>
    /// <returns>The member</returns>
    private static string RowsOperator(int rows, int cols, string matrix, string vec)
    {
        // every column of the value is scaled by the whole vector, so the component of a row of the value is
        // scaled by the component of the vector at the index of that row
        var scaled = VectorGenShared.Join(cols, j => $"b.c{j} * a");
        var sb = new StringBuilder();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the value whose every column is the column of the value at the same position scaled by the");
        sb.AppendLine("    /// component of the vector at the index of a row of that column");
        sb.AppendLine("    /// <para>The member scales the rows of a matrix by the components of a vector: every column of the value");
        sb.AppendLine("    /// is scaled by the whole vector, so the component of a row of the value is scaled by the component of");
        sb.AppendLine("    /// the vector at the index of that row.</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine($"    /// <param name=\"a\">The vector that scales the rows of the value, a vector of {rows} components</param>");
        sb.AppendLine($"    /// <param name=\"b\">The value, a matrix of {rows} rows and {cols} columns</param>");
        sb.AppendLine("    /// <returns>The matrix whose component at the index of a row of the value is that component of the value");
        sb.AppendLine("    /// scaled by the component of the vector at the same index</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.Append($"    public static {matrix} operator *({vec} a, {matrix} b) => new({scaled});");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the member of the math class that multiplies the matrix of <paramref name="rows"/> rows and
    /// <paramref name="cols"/> columns by the vector of the columns of it, which is the vector of the rows of the
    /// value.
    /// </summary>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <param name="matrix">The name of the matrix</param>
    /// <param name="vec">The name of the vector of the columns of the matrix</param>
    /// <param name="res">The name of the vector of the rows of the matrix</param>
    /// <returns>The member</returns>
    private static string ColumnsProduct(int rows, int cols, string matrix, string vec, string res)
    {
        var sum = Sum(cols, j => ($"a.c{j}", $"b.{Repeated(j, rows)}"));
        var sb = new StringBuilder();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the product of the matrix and the vector, which is the value of the vector whose component at");
        sb.AppendLine("    /// the index of a row of the matrix is the sum of the products of the components of that row with the");
        sb.AppendLine("    /// components of the vector");
        sb.AppendLine("    /// <para>The product is the product of the value with the vector as a matrix of a single column, so it is");
        sb.AppendLine("    /// the sum of the columns of the value scaled by the components of the vector.</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine($"    /// <param name=\"a\">The value, a matrix of {rows} rows and {cols} columns</param>");
        sb.AppendLine($"    /// <param name=\"b\">The vector, a vector of {cols} components</param>");
        sb.AppendLine($"    /// <returns>The product of the value, a vector of {rows} components</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {res} mul({matrix} a, {vec} b) =>");
        sb.Append($"        {sum};");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the member of the math class that multiplies the vector of the rows of the matrix of
    /// <paramref name="rows"/> rows and <paramref name="cols"/> columns by the matrix, which is the vector of the
    /// columns of the value.
    /// </summary>
    /// <param name="rows">The number of the rows of the matrix</param>
    /// <param name="cols">The number of the columns of the matrix</param>
    /// <param name="matrix">The name of the matrix</param>
    /// <param name="vec">The name of the vector of the rows of the matrix</param>
    /// <param name="res">The name of the vector of the columns of the matrix</param>
    /// <returns>The member</returns>
    private static string RowsProduct(int rows, int cols, string matrix, string vec, string res)
    {
        // the component of the result at the index of a column of the matrix is the dot product of the vector
        // with that column
        var products = VectorGenShared.Join(cols, j => $"math.dot(a, b.c{j})");
        var sb = new StringBuilder();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the product of the vector and the matrix, which is the value of the vector whose component at");
        sb.AppendLine("    /// the index of a column of the matrix is the dot product of the vector with that column");
        sb.AppendLine("    /// <para>The product is the product of the vector as a matrix of a single row with the value, so it is the");
        sb.AppendLine("    /// vector of the dot products of the value with the columns of the matrix.</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine($"    /// <param name=\"a\">The vector, a vector of {rows} components</param>");
        sb.AppendLine($"    /// <param name=\"b\">The value, a matrix of {rows} rows and {cols} columns</param>");
        sb.AppendLine($"    /// <returns>The product of the value, a vector of {cols} components</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.Append($"    public static {res} mul({vec} a, {matrix} b) => new({products});");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the member of the math class that multiplies the matrix of <paramref name="rows"/> rows and
    /// <paramref name="cols"/> columns by the matrix <paramref name="right"/>, which is the matrix
    /// <paramref name="res"/> whose column at the index of a column of the second value is the sum of the columns
    /// of the first value scaled by the components of that column.
    /// </summary>
    /// <param name="rows">The number of the rows of the first matrix</param>
    /// <param name="cols">The number of the columns of the first matrix</param>
    /// <param name="left">The name of the first matrix</param>
    /// <param name="right">The name of the second matrix</param>
    /// <param name="res">The name of the result of the product</param>
    /// <param name="columns">The column of the result at the index of a column of the second matrix</param>
    /// <returns>The member</returns>
    private static string MatrixProduct(
        int rows, int cols, string left, string right, string res, string[] columns)
    {
        var resCols = columns.Length;
        var sb = new StringBuilder();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Returns the product of the two matrices, which is the matrix whose column at the index of a column of");
        sb.AppendLine("    /// the second matrix is the sum of the columns of the first matrix scaled by the components of that");
        sb.AppendLine("    /// column");
        sb.AppendLine($"    /// <para>The product of a matrix of {rows} rows and {cols} columns with a matrix of {cols} rows and");
        sb.AppendLine($"    /// {resCols} columns is a matrix of {rows} rows and {resCols} columns.</para>");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine($"    /// <param name=\"a\">The first matrix, a matrix of {rows} rows and {cols} columns</param>");
        sb.AppendLine($"    /// <param name=\"b\">The second matrix, a matrix of {cols} rows and {resCols} columns</param>");
        sb.AppendLine($"    /// <returns>The product of the two matrices, a matrix of {rows} rows and {resCols} columns</returns>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.AppendLine($"    public static {res} mul({left} a, {right} b)");
        sb.AppendLine("    {");
        sb.AppendLine("        return new(");
        for (var j = 0; j < resCols; j++)
            sb.AppendLine($"            {columns[j]}{(j + 1 == resCols ? "" : ",")}");
        sb.AppendLine("        );");
        sb.Append("    }");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the member of the value itself that multiplies it by the second value of a product, which is the
    /// member of the math class that the value of the product is called on.
    /// </summary>
    /// <param name="first">The name of the value of the product</param>
    /// <param name="second">The name of the second value of the product</param>
    /// <param name="res">The name of the result of the product</param>
    /// <returns>The member</returns>
    private static string Extension(string first, string second, string res)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"    /// <inheritdoc cref=\"math.mul({first}, {second})\"/>");
        sb.AppendLine("    [MethodImpl(256)]");
        sb.Append($"    public static {res} mul(this {first} a, {second} b) => math.mul(a, b);");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the sum of the products of the terms, which the fused multiply and add of the kind of the value
    /// builds: the first term is the product of the two factors of it and every term that follows is the fused
    /// multiply and add of the two factors of it with the sum of the terms before it.
    /// </summary>
    /// <param name="count">The number of the terms</param>
    /// <param name="term">The two factors of the term at a position</param>
    /// <returns>The sum of the products of the terms</returns>
    private static string Sum(int count, Func<int, (string A, string B)> term)
    {
        var (a, b) = term(0);
        var sum = $"{a} * {b}";
        for (var i = 1; i < count; i++)
        {
            (a, b) = term(i);
            sum = $"math.fma({a}, {b}, {sum})";
        }

        return sum;
    }

    /// <summary>
    /// Returns the swizzle of a vector that names the component of it at a position <paramref name="count"/>
    /// times, which broadcasts a component of a column of a matrix to the whole column of a matrix of that many
    /// rows.
    /// </summary>
    /// <param name="index">The index of the component of the vector</param>
    /// <param name="count">The number of the components of the result</param>
    /// <returns>The name of the swizzle</returns>
    private static string Repeated(int index, int count) =>
        VectorGenShared.Join(count, _ => Typ.xyzw[index], "");
}
