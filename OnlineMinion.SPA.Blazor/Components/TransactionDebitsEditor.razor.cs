using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.SPA.Blazor.Transaction.Debit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Components;

public partial class TransactionDebitsEditor
{
    [Parameter]
    public UpsertEditorWrapper<BaseTransactionDebitUpsertVM> WrapperRef { get; set; } = default!;

    // TODO: To Cascading Parameter
    [Parameter]
    [EditorRequired]
    public BaseTransactionDebitUpsertVM Model { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public EventCallback<EditContext> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    [EditorRequired]
    public IEnumerable<PaymentSpecDescriptorResp> PaymentSpecDescriptors { get; set; }
        = Enumerable.Empty<PaymentSpecDescriptorResp>();

    [Parameter]
    [EditorRequired]
    public IEnumerable<AccountSpecDescriptorResp> AccountSpecDescriptors { get; set; }
        = Enumerable.Empty<AccountSpecDescriptorResp>();
}
