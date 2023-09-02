using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Web.Pages.Base;
using OnlineMinion.Web.Transaction.Debit;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class TransactionDebitsPage
    : BaseCRUDPage<TransactionDebitListItem, TransactionDebitResp, BaseUpsertTransactionDebitReqData>
{
    protected override string ModelTypeName => "Debit Transaction";

    private List<PaymentSpecDescriptorResp> PaymentDescriptorViewModels { get; } = new();
    private List<AccountSpecDescriptorResp> AccountDescriptorViewModels { get; } = new();

    protected override Task LoadDependenciesAsync() => Task.WhenAll(
        LoadDependentVMsFromApiAsync(PaymentDescriptorViewModels),
        LoadDependentVMsFromApiAsync(AccountDescriptorViewModels)
    );

    protected override ICreateCommand CreateCommandFactory() => new CreateTransactionDebitReq();

    protected override IUpdateCommand UpdateCommandFactory(TransactionDebitListItem vm) =>
        TransactionDebitListItem.ToUpdateRequest(vm);

    protected override TransactionDebitListItem ConvertReqResponseToVM(TransactionDebitResp dto)
    {
        var (paymentSpec, accountSpec) = GetVMDependencies(dto.PaymentInstrumentId, dto.AccountSpecId);

        return TransactionDebitListItem.FromResponseDto(dto, paymentSpec, accountSpec);
    }

    protected override TransactionDebitListItem ConvertUpdateReqToVM(IUpdateCommand dto)
    {
        var rq = (UpdateTransactionDebitReq)dto;
        var (paymentSpec, accountSpec) = GetVMDependencies(rq.PaymentInstrumentId, rq.AccountSpecId);

        return TransactionDebitListItem.FromUpdateRequest(rq, paymentSpec, accountSpec);
    }

    private (PaymentSpecDescriptorResp paymentSpec, AccountSpecDescriptorResp accountSpec) GetVMDependencies(
        int paymentSpecId,
        int accountSpecId
    ) => (
        PaymentDescriptorViewModels.Single(vm => vm.Id == paymentSpecId),
        AccountDescriptorViewModels.Single(vm => vm.Id == accountSpecId)
    );

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionDebitPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(TransactionDebitListItem model) =>
        $"subject `{model.Subject}`, at {model.Date.ToString(CultureInfo.CurrentCulture)}";

    protected override IDeleteByIdCommand DeleteCommandFactory(TransactionDebitListItem vm) =>
        new DeleteTransactionDebitReq(vm.Id);
}
