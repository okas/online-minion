using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Web.Transaction.Debit.ViewModels;

namespace OnlineMinion.Web.Components;

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
