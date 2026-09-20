using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace SourceGenerator.Templates;

#pragma warning disable BL0006

public abstract class RazorComponentTemplateBase : ComponentBase
{
    /// <summary>
    /// 触发渲染并提取生成的代码/文本
    /// </summary>
    public string Render()
    {
        var builder = new RenderTreeBuilder();

        BuildRenderTree(builder);

        var sb = new StringBuilder();
        var frames = builder.GetFrames();

        for (int i = 0; i < frames.Count; i++)
        {
            ref readonly var frame = ref frames.Array[i];
            if (frame.FrameType == RenderTreeFrameType.Text)
            {
                sb.Append(frame.TextContent);
            }
            else if (frame.FrameType == RenderTreeFrameType.Markup)
            {
                sb.Append(frame.MarkupContent);
            }
        }

        return sb.ToString();
    }
}
