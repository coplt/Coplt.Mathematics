using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace Tests.Arith;

/// <summary>
/// Asserts the member that every call of the overload checks is bound to. The member is not visible in the source
/// of a call, so the assembly that the tests run from is loaded and the body of the member that carries the calls
/// is read: every call of it is described by the member it reaches and compared with the member it is expected to
/// reach.
/// </summary>
public class TestOverloadResolutionCheck
{
    private const string Float3 = "Coplt.Mathematics.float3";
    private const string Math = "Coplt.Mathematics.math";
    private const string MathEx = "Coplt.Mathematics.math_ex";
    private const string MathExFloat = "Coplt.Mathematics.math_ex_float";
    private const string ExFloat = "Coplt.Mathematics.ex_float";
    private const string Single = "System.Single";

    /// <summary>
    /// The calls of <see cref="TestOverloadResolution.ScalarArguments"/> in the order of its body and the member
    /// that every one of them is bound to.
    /// </summary>
    private static readonly (string Call, string Member)[] Calls =
    {
        ("math.dot(v, v)", $"{ExFloat}::dot<{Float3}>({Float3}&, {Float3}&)"),
        ("math.clamp(v, 2, 5)", $"{Math}::clamp<{Float3}>({Float3}&, {Float3}&, {Float3}&)"),
        ("math.lerp(1, 2, v)", $"{ExFloat}::lerp<{Float3}>({Single}, {Single}, {Float3}&)"),
        ("math.unlerp(v, 2, 3)", $"{ExFloat}::unlerp<{Float3}>({Float3}&, {Single}, {Single})"),
        ("math.unlerp(v, 2, v)", $"{MathEx}::unlerp<{Float3}>({Float3}&, {Float3}&, {Float3}&)"),
        ("math.remap(v, 1, 2, 3, 4)", $"{ExFloat}::remap<{Float3}>({Float3}&, {Single}, {Single}, {Single}, {Single})"),
        ("math.remap(v, v, 2, 3, 4)", $"{MathEx}::remap<{Float3}>({Float3}&, {Float3}&, {Float3}&, {Float3}&, {Float3}&)"),
        ("math.length_sq(v)", $"{ExFloat}::length_sq<{Float3}>({Float3}&)"),
        ("math.distance_sq(v, v)", $"{ExFloat}::distance_sq<{Float3}>({Float3}&, {Float3}&)"),
        ("v.lerp(1, 2)", $"{MathExFloat}::lerp<{Float3}>({Float3}, {Single}, {Single})"),
        ("v.lerp(1, v)", $"{MathEx}::lerp<{Float3}>({Float3}, {Float3}&, {Float3}&)"),
        ("v.unlerp(1, 2)", $"{MathExFloat}::unlerp<{Float3}>({Float3}, {Single}, {Single})"),
        ("v.unlerp(1, v)", $"{MathEx}::unlerp<{Float3}>({Float3}, {Float3}&, {Float3}&)"),
        ("v.length_sq()", $"{MathExFloat}::length_sq<{Float3}>({Float3})"),
        ("v.distance_sq(v)", $"{MathExFloat}::distance_sq<{Float3}>({Float3}, {Float3}&)"),
    };

    /// <summary>
    /// The calls of <see cref="TestOverloadResolutionUsingStatic.ScalarArguments"/>, which are the ones of the
    /// member above without the ones that are called on a value.
    /// </summary>
    private static readonly (string Call, string Member)[] UsingStaticCalls = Calls[..9];

    [Test]
    public void ScalarArguments()
        => AssertCalls(typeof(TestOverloadResolution), nameof(TestOverloadResolution.ScalarArguments), Calls);

    [Test]
    public void ScalarArgumentsUsingStatic()
        => AssertCalls(typeof(TestOverloadResolutionUsingStatic), nameof(TestOverloadResolutionUsingStatic.ScalarArguments), UsingStaticCalls);

    /// <summary>
    /// Asserts that every call of the body of <paramref name="method"/> of <paramref name="type"/> is bound to
    /// the member of <paramref name="expected"/> that is at the same position.
    /// </summary>
    /// <param name="type">The type that declares the checked member</param>
    /// <param name="method">The name of the checked member</param>
    /// <param name="expected">The member every call of the checked member is expected to reach</param>
    private static void AssertCalls(Type type, string method, (string Call, string Member)[] expected)
    {
        var calls = BoundMembers(type, method);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(calls.Count, Is.EqualTo(expected.Length), "the number of the calls changed");
            for (var i = 0; i < expected.Length && i < calls.Count; i++)
            {
                Assert.That(calls[i], Is.EqualTo(expected[i].Member), $"the member of the call '{expected[i].Call}' changed");
            }
        }
    }

    /// <summary>
    /// Returns the member that every call in the body of <paramref name="method"/> of <paramref name="type"/>
    /// reaches, the assembly of the tests is the one that is being run and the type is looked up in it.
    /// </summary>
    /// <param name="type">The type that declares the checked member</param>
    /// <param name="method">The name of the checked member</param>
    /// <returns>The description of every call of the checked member</returns>
    private static List<string> BoundMembers(Type type, string method)
    {
        var assembly = AssemblyDefinition.FromFile(type.Assembly.Location);
        var declaration = assembly.ManifestModule!.GetAllTypes().FirstOrDefault(t => t.FullName == type.FullName)
                          ?? throw new InvalidOperationException($"the type {type.FullName} is not in {assembly.Name}");
        var definition = declaration.Methods.FirstOrDefault(m => m.Name == method && m.Parameters.Count == 0)
                         ?? throw new InvalidOperationException($"the member {type.FullName}.{method} is not in {assembly.Name}");
        var body = definition.CilMethodBody
                   ?? throw new InvalidOperationException($"the member {type.FullName}.{method} has no body");

        var calls = new List<string>();
        foreach (var instruction in body.Instructions)
        {
            if (instruction.OpCode.Code is not (CilCode.Call or CilCode.Callvirt)) continue;
            // the conversion of an argument, the construction of a value and the creation of a closure are done
            // by members of their own, they are not the member the call is made on
            var name = MemberName(instruction.Operand);
            if (name is "op_Implicit" or "op_Explicit" or ".ctor") continue;
            calls.Add(Describe(instruction.Operand));
        }

        return calls;
    }

    /// <summary>
    /// Returns the name of the member of a call.
    /// </summary>
    /// <param name="operand">The member of a call</param>
    /// <returns>The name of the member or an empty string</returns>
    private static string MemberName(object? operand) => operand switch
    {
        MethodSpecification specification => specification.Method.Name.ToString(),
        IMethodDefOrRef definition => definition.Name!.ToString(),
        _ => "",
    };

    /// <summary>
    /// Describes a call by the member it reaches: the name of the type that declares it, the name of the member,
    /// the type arguments of it and the types of the parameters of it.
    /// </summary>
    /// <param name="operand">The member of a call</param>
    /// <returns>The description of the call</returns>
    private static string Describe(object? operand)
    {
        var member = operand switch
        {
            MethodSpecification specification => specification.Method,
            IMethodDefOrRef definition => definition,
            _ => null,
        };
        if (member?.Signature is not { } signature) return operand?.ToString() ?? "null";
        // a generic member is called with the types it is instantiated with and the parameters of its definition
        // carry the type parameters of it, so every parameter that names one of them is written with the type
        var typeArguments = operand is MethodSpecification specification2 ? specification2.Signature!.TypeArguments : null;
        var parameters = signature.ParameterTypes.Select(p => p.FullName).ToArray();
        if (typeArguments is { Count: > 0 })
        {
            for (var i = 0; i < typeArguments.Count; i++)
            {
                for (var j = 0; j < parameters.Length; j++)
                {
                    parameters[j] = parameters[j].Replace($"!!{i}", typeArguments[i].FullName);
                }
            }
        }

        var typeArgumentsText = typeArguments is null or { Count: 0 }
            ? ""
            : $"<{string.Join(", ", typeArguments.Select(a => a.FullName))}>";
        return $"{member.DeclaringType?.FullName}::{member.Name}{typeArgumentsText}({string.Join(", ", parameters)})";
    }
}
