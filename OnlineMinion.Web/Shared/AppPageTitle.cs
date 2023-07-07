using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace OnlineMinion.Web.Shared;

public sealed class AppPageTitle : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public string Text { get; set; } = null!;

    private string TemplatedTitle => $"{Text} | Minion";

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<PageTitle>(0);

        builder.AddComponentParameter(
            1,
            nameof(PageTitle.ChildContent),
            (RenderFragment)(builder2 => builder2.AddContent(0, TemplatedTitle))
        );

        builder.CloseComponent();
    }
}
