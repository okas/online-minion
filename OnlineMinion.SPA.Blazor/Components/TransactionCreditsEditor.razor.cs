using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Components;

public partial class TransactionCreditsEditor
{
    [Parameter]
    public UpsertEditorWrapper<BaseTransactionCreditUpsertVM> WrapperRef { get; set; } = default!;

    // TODO: To Cascading Parameter
    [Parameter]
    [EditorRequired]
    public BaseTransactionCreditUpsertVM Model { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    [EditorRequired]
    public IEnumerable<PaymentSpecDescriptorResp> PaymentSpecDescriptors { get; set; }
        = Enumerable.Empty<PaymentSpecDescriptorResp>();
}
