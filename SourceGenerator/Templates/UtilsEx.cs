using System.Collections.Generic;

namespace SourceGenerator.Templates;

public static class UtilsEx
{
    public static string Join(this IEnumerable<string> enumerable, string by) => string.Join(by, enumerable);
}
