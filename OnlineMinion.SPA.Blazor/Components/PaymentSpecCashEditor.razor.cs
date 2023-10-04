using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.SPA.Blazor.CurrencyInfo.ViewModels;
using OnlineMinion.SPA.Blazor.ViewModels.PaymentSpec;
using Radzen;

namespace OnlineMinion.SPA.Blazor.Components;

public partial class PaymentSpecCashEditor
{
    [Inject]
    private TooltipService TooltipService { get; set; } = default!;

    [Parameter]
    public UpsertEditorWrapper<BaseUpsertPaymentSpecReqData> WrapperRef { get; set; } = default!;

    // TODO: To Cascading Parameter
    [Parameter]
    [EditorRequired]
    public BaseUpsertPaymentSpecReqData Model { get; set; } = default!;

    [Parameter]
    public UpdatePaymentSpecEditorMetadata? Metadata { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    [EditorRequired]
    public IEnumerable<CurrencyInfoVm> CurrencyInfoVMs { get; set; } = Enumerable.Empty<CurrencyInfoVm>();

    private void ShowDisabledCurrencyCodeHelp(ElementReference elementRef)
    {
        var options = new TooltipOptions { Duration = null, };

        TooltipService.Open(elementRef, Str.CurrencyCodeCannotUpdate, options);
    }
}
