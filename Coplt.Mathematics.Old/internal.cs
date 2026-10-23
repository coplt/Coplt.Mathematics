namespace Coplt.Mathematics;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class ThisAttribute : Attribute;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class ArgAttribute(int Order) : Attribute
{
    private int Order { get; } = Order;
}

[AttributeUsage(AttributeTargets.Class)]
internal sealed class ExToAttribute(Type Type) : Attribute
{
    private Type Type { get; } = Type;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class ExAttribute : Attribute;
