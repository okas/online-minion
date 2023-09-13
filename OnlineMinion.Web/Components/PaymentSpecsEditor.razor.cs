using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Web.CurrencyInfo.ViewModels;

namespace OnlineMinion.Web.Components;

public partial class PaymentSpecsEditor
{
    [Parameter]
    public UpsertEditorWrapper<BaseUpsertPaymentSpecReqData> WrapperRef { get; set; } = default!;

    // TODO: To Cascading Parameter
    [Parameter]
    [EditorRequired]
    public BaseUpsertPaymentSpecReqData Model { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    [EditorRequired]
    public IEnumerable<CurrencyInfoVm> CurrencyInfoVMs { get; set; } = Enumerable.Empty<CurrencyInfoVm>();
}
