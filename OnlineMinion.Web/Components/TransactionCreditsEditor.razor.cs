using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Web.Transaction.Credit.ViewModels;

namespace OnlineMinion.Web.Components;

public partial class TransactionCreditsEditor
{
    private FieldIdentifier _dateFiledIdentifier;

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
