using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace OnlineMinion.SPA.Blazor.Components;

public partial class RadzenDataGridWrapper<TItem> : IDisposable
{
    [Inject]
    public StateContainer SC { get; set; } = default!;

    [Parameter]
    public RadzenDataGrid<TItem> DataGridRef { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public int TotalItemsCount { get; set; }

    [Parameter]
    [EditorRequired]
    public IEnumerable<TItem> Data { get; set; } = Enumerable.Empty<TItem>();

    [Parameter]
    [EditorRequired]
    public IEnumerable<int> PageSizeOptions { get; set; } = Enumerable.Empty<int>();

    [Parameter]
    [EditorRequired]
    public EventCallback<LoadDataArgs> LoadData { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<PagerEventArgs> Page { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback AddClick { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<TItem> EditClick { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<TItem> DeleteClick { get; set; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment Columns { get; set; }

    void IDisposable.Dispose() => SC.OnChange -= StateHasChanged;
}
