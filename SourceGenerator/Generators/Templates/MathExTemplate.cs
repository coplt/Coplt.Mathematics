using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Coplt.Analyzers.Generators.Templates;

public record struct Arg(string Name, string Type, RefKind Ref)
{
    public string TypeRef => Ref switch
    {
        RefKind.Ref => "ref ",
        RefKind.RefReadOnlyParameter => "ref readonly ",
        RefKind.Out => "out ",
        RefKind.In => "in ",
        _ => ""
    };
    public string ValueRef => Ref switch
    {
        RefKind.Ref => "ref ",
        RefKind.Out => "out ",
        RefKind.In or RefKind.RefReadOnlyParameter => "in ",
        _ => ""
    };
}

public record struct Fn(string Name, string ReturnType, ImmutableArray<Arg> Args, ImmutableArray<Arg> RawArgs);

public class MathExTemplate(
    GenBase GenBase,
    string Name,
    string SrcName,
    ImmutableArray<Fn> Fns
) : ATemplate(GenBase)
{
    protected override void DoGen()
    {
        sb.AppendLine(GenBase.Target.Code);
        sb.AppendLine("{");

        foreach (var fn in Fns)
        {
            sb.AppendLine();
            sb.Append($"    /// <inheritdoc cref=\"{SrcName}.{fn.Name}(");
            {
                var first = true;
                foreach (var arg in fn.RawArgs)
                {
                    if (first) first = false;
                    else sb.Append(", ");
                    sb.Append($"{arg.TypeRef}{arg.Type}");
                }
            }
            sb.AppendLine($")\"/>");
            sb.AppendLine($"    [MethodImpl(256 | 512)]");
            sb.Append($"    public static {fn.ReturnType} {fn.Name}(");
            {
                var first = true;
                foreach (var arg in fn.Args)
                {
                    if (first)
                    {
                        first = false;
                        sb.Append("this ");
                    }
                    else sb.Append(", ");
                    sb.Append($"{arg.TypeRef}{arg.Type} {arg.Name}");
                }
            }
            sb.Append($") => {SrcName}.{fn.Name}(");
            {
                var first = true;
                foreach (var arg in fn.RawArgs)
                {
                    if (first) first = false;
                    else sb.Append(", ");
                    sb.Append($"{arg.ValueRef}{arg.Name}");
                }
            }
            sb.AppendLine($");");
        }

        sb.AppendLine();
        sb.AppendLine("}");
    }
}
