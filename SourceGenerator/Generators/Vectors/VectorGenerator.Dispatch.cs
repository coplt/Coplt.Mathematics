using System.Text;

namespace Coplt.Analyzers.Generators;

public partial class VectorGenerator
{
    /// <summary>
    /// Generates the members that dispatch the value of the vector to a visitor: the width of the register of a
    /// vector and the count of its components are a part of its type, so the type itself is the only one that
    /// knows them and a caller that does not know the type of the vector reaches them by calling the member of a
    /// visitor that matches them. The value of the vector is handed to the member: a vector that keeps its value
    /// in a register hands the register over and a vector without a register hands the vector itself over, which
    /// the member of the visitor reaches the components of through the count of them. The member of the visitor
    /// builds the result out of the type of the vector, which the constraints of the members of the visitor
    /// name, so the same visitor serves a vector and a matrix. The members are implemented explicitly and they
    /// are emitted into the file of the base members of the vector. Every vector of a number type implements the
    /// interface, it is the constraint of a member of a visitor that names the vector interface of a number.
    /// <para>The kind of the component decides the level of the member of the visitor that the value reaches:
    /// the members of the level of a floating point number fall back to the ones of the level of a number and
    /// those fall back to the ones that reach every value, so a visitor implements the members of the level of
    /// the kind it reaches alone. The members that take a single component of the value beside it and the ones
    /// that reduce the value to a single component of it are the members of the same visitor and they are named
    /// the same way, so every member of the interface of the dispatch of the value is implemented by the member
    /// of the visitor of the same name.</para>
    /// </summary>
    /// <param name="typ">The type of the component of the vector</param>
    /// <param name="size">The number of components of the vector</param>
    /// <param name="storeVariant">True for the storage variant of the vector</param>
    /// <returns>The members that dispatch the value of the vector, null when the vector implements no dispatch</returns>
    private static string? GenDispatch(Typ typ, int size, bool storeVariant)
    {
        // the storage variant of a vector holds the components of a value of the kind of it, it does not reach
        // the algebra of it and the members of it do not dispatch
        if (storeVariant) return null;

        var type = VectorGenShared.VecName(typ, size, storeVariant);
        var scalar = typ.compType;
        var reg = VectorGenShared.Register(typ, size, storeVariant);
        // the kind of the component decides the level of the members of the visitor that the value reaches: the
        // members of the level of a floating point number fall back to the ones of the level of a number and
        // those fall back to the ones that reach every value
        var level = VectorGenShared.DispatchLevel(typ);
        // the member of the visitor that matches the kind of the value: the width of the register of the vector
        // when it keeps its value in one, the count of its components when it has no register. The name of every
        // argument of a member is the one of the interface of the dispatch of it
        var one = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(self)"
            : $"V.Simd_{level}<{type}, {scalar}>(self.vector)";
        var two = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(a, b)"
            : $"V.Simd_{level}<{type}, {scalar}>(a.vector, b.vector)";
        var three = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(a, b, c)"
            : $"V.Simd_{level}<{type}, {scalar}>(a.vector, b.vector, c.vector)";
        // a member that takes a component of the value beside it names the type of the component of the value
        // through the type of the component itself, so it is not named with the type of the component again
        var oneComponent = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(a, b)"
            : $"V.Simd_{level}<{type}, {scalar}>(a.vector, b)";
        var twoComponents = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(a, b, c)"
            : $"V.Simd_{level}<{type}, {scalar}>(a.vector, b, c)";
        // a member that takes a value and returns a single component of it is the reduction of the value, which
        // the member of the visitor of the same name builds, so the value of the vector reaches the same member
        // of the visitor as the value of a matrix of the same kind does
        var reduceOne = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(a)"
            : $"V.Simd_{level}<{type}, {scalar}>(a.vector)";
        var reduceTwo = reg == 0
            ? $"V.Vector{size}_{level}<{type}, {scalar}>(a, b)"
            : $"V.Simd_{level}<{type}, {scalar}>(a.vector, b.vector)";
        // a member that takes a component of the value alone does not reach the value of the vector at all, so
        // the visitor reaches the component of every value of the kind of it the same way
        var scalarOne = $"V.Scalar_{level}(a)";
        var scalarTwo = $"V.Scalar_{level}(a, b)";
        var scalarThree = $"V.Scalar_{level}(a, b, c)";
        var combine = $"V.Combine_{level}(a, b)";

        // the members that take the value alone are the members of the interface that names the type of the
        // value, every other member takes a component of the value beside it or reaches the one it was given
        var self = VectorGenShared.DispatchIface(type);
        var withScalar = VectorGenShared.DispatchIfaceScalar(type, scalar);

        var sb = new StringBuilder();

        sb.AppendLine("    #region dispatch");
        sb.AppendLine();

        // the members of the dispatch of a value are implemented explicitly, so a member does not name the
        // constraints of the type of a component of it again and the value of a member of every kind is the
        // same one
        void Member(string ret, string iface, string member, string parameters, string body)
        {
            sb.AppendLine("    /// <inheritdoc/>");
            sb.AppendLine("    [MethodImpl(256)]");
            sb.AppendLine($"    static {ret} {iface}.{member}({parameters})");
            sb.AppendLine($"        => {body};");
            sb.AppendLine();
        }

        Member(type, self, "Self<V>", $"in {type} self", one);
        Member(type, self, "Self<V>", $"in {type} a, in {type} b", two);
        Member(type, self, "Self<V>", $"in {type} a, in {type} b, in {type} c", three);
        Member(type, withScalar, "Self<V>", $"in {type} a, {scalar} b", oneComponent);
        Member(type, withScalar, "Self<V>", $"in {type} a, {scalar} b, {scalar} c", twoComponents);
        Member(scalar, withScalar, "Scalar<V>", $"{scalar} a", scalarOne);
        Member(scalar, withScalar, "Scalar<V>", $"{scalar} a, {scalar} b", scalarTwo);
        Member(scalar, withScalar, "Scalar<V>", $"{scalar} a, {scalar} b, {scalar} c", scalarThree);
        // a member that reduces the value of the vector to a single component of it
        Member(scalar, withScalar, "Scalar<V>", $"in {type} a", reduceOne);
        Member(scalar, withScalar, "Scalar<V>", $"in {type} a, in {type} b", reduceTwo);
        Member(scalar, withScalar, "Combine<V>", $"{scalar} a, {scalar} b", combine);
        // the value of the vector hands itself over instead of a component of it, so the visitor decides the
        // type of a component of the value it reaches: the level of the kind of the component decides the member
        // of the visitor that reaches the value
        Member(type, self, "Map_Self<V>", $"in {type} self", $"V.Map_{level}<{type}, {scalar}>(self)");
        // a member that reaches the bool value of the vector hands the value over the same way every other member
        // that takes the value alone does: the register of the vector when it keeps its value in one and the
        // vector itself when it has no register
        Member("bool", self, "Bool<V>", $"in {type} self", one);
        // a member that takes a component of the vector alone does not reach the value at all, so the visitor
        // reaches the component of every value of the kind of it the same way
        Member("bool", withScalar, "Bool<V>", $"{scalar} a", scalarOne);

        sb.AppendLine("    #endregion");
        sb.AppendLine();
        return sb.ToString();
    }
}
