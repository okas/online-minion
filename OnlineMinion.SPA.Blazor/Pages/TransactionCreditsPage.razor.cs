using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.SPA.Blazor.Components;
using OnlineMinion.SPA.Blazor.Pages.Base;
using OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Pages;

[UsedImplicitly]
public partial class TransactionCreditsPage
    : BaseCRUDPage<TransactionCreditListItem, TransactionCreditResp, BaseTransactionCreditUpsertVM>
{
    private readonly List<PaymentSpecDescriptorResp> _paymentDescriptorViewModels = new();

    private TransactionCreditsEditor? _editorRef;

    protected override string ModelTypeName => "Credit Transaction";

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        // In this page's case, the editor is separate component that encapsulates own content inside EditorWrapper.
        // Therefor, exposed parameter WrapperRef is only accessible if editor itself is rendered
        // and it can be pulled out now.
        if (_editorRef is not null)
        {
            EditorWrapperRef = _editorRef.WrapperRef;
        }
    }

    protected override async Task RunDependencyLoadingAsync() => await LoadDependencyFromApiAsync(
        new GetSomeModelDescriptorsReq<PaymentSpecDescriptorResp>(),
        resp => _paymentDescriptorViewModels.Add(resp)
    );

    protected override CreateTransactionCreditVM CreateVMFactory() => new();

    protected override UpdateTransactionCreditVM UpdateVMFactory(TransactionCreditListItem vm) => vm.ToUpdateVM();

    protected override TransactionCreditListItem ConvertReqResponseToVM(TransactionCreditResp dto)
    {
        var paymentSpec = GetVMDependencies(dto.PaymentInstrumentId);

        return TransactionCreditListItem.FromResponseDto(dto, paymentSpec);
    }

    protected override UpdateTransactionCreditReq ConvertUpdateVMToReq(IUpdateCommand reqOrVM) =>
        ((UpdateTransactionCreditVM)reqOrVM).ToCommand();

    protected override TransactionCreditListItem ConvertUpdateReqToVM(IUpdateCommand dto)
    {
        var rq = (UpdateTransactionCreditReq)dto;
        var paymentSpec = GetVMDependencies(rq.PaymentInstrumentId);

        return TransactionCreditListItem.FromUpdateRequest(rq, paymentSpec);
    }

    protected override CreateTransactionCreditReq ConvertCreateVMToReq(ICreateCommand reqOrVM) =>
        ((CreateTransactionCreditVM)reqOrVM).ToCommand();

    private PaymentSpecDescriptorResp GetVMDependencies(Guid id) =>
        _paymentDescriptorViewModels.Single(vm => vm.Id == id);

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionCreditPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(TransactionCreditListItem model) =>
        $"subject `{model.Subject}`, at {model.Date.ToString(CultureInfo.CurrentCulture)}";

    protected override DeleteTransactionCreditReq DeleteCommandFactory(TransactionCreditListItem vm) => new(vm.Id);
}
