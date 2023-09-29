using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;

namespace OnlineMinion.SPA.Blazor.Components;

public partial class AccountSpecsEditor
{
    [Parameter]
    public UpsertEditorWrapper<BaseUpsertAccountSpecReqData> WrapperRef { get; set; } = default!;

    // TODO: To Cascading Parameter
    [Parameter]
    [EditorRequired]
    public BaseUpsertAccountSpecReqData Model { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }
}
