using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Web.Pages.Base;
using OnlineMinion.Web.Transaction.Credit;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class TransactionCreditsPage
    : BaseCRUDPage<TransactionCreditListItem, TransactionCreditResp, BaseUpsertTransactionReqData>
{
    protected override string ModelTypeName => "Credit Transaction";

    private List<PaymentSpecDescriptorResp> PaymentDescriptorViewModels { get; } = new();

    protected override Task LoadDependenciesAsync() => LoadDependentVMsFromApiAsync(PaymentDescriptorViewModels);

    protected override ICreateCommand CreateCommandFactory() => new CreateTransactionCreditReq();

    protected override IUpdateCommand UpdateCommandFactory(TransactionCreditListItem vm) =>
        TransactionCreditListItem.ToUpdateRequest(vm);

    protected override TransactionCreditListItem ConvertReqResponseToVM(TransactionCreditResp dto)
    {
        var paymentSpec = GetVMDependencies(dto.PaymentInstrumentId);

        return TransactionCreditListItem.FromResponseDto(dto, paymentSpec);
    }

    protected override TransactionCreditListItem ConvertUpdateReqToVM(IUpdateCommand dto)
    {
        var rq = (UpdateTransactionCreditReq)dto;
        var paymentSpec = GetVMDependencies(rq.PaymentInstrumentId);

        return TransactionCreditListItem.FromUpdateRequest(rq, paymentSpec);
    }

    private PaymentSpecDescriptorResp GetVMDependencies(int id) =>
        PaymentDescriptorViewModels.Single(vm => vm.Id == id);

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionCreditPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(TransactionCreditListItem model) =>
        $"subject `{model.Subject}`, at {model.Date.ToString(CultureInfo.CurrentCulture)}";

    protected override IDeleteByIdCommand DeleteCommandFactory(TransactionCreditListItem vm) =>
        new DeleteTransactionCreditReq(vm.Id);
}
