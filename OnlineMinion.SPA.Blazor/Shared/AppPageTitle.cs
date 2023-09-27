using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace OnlineMinion.SPA.Blazor.Shared;

public sealed class AppPageTitle : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public string Text { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<PageTitle>(0);
        builder.AddComponentParameter(1, nameof(PageTitle.ChildContent), (RenderFragment)BuildTitleRenderTree);
        builder.CloseComponent();
    }

    private void BuildTitleRenderTree(RenderTreeBuilder builder) =>
        builder.AddContent(0, $"{Text} | Minion");
}
