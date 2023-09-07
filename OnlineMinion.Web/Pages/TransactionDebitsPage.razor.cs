using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Web.Pages.Base;
using OnlineMinion.Web.ViewModels.Transaction.Debit;

namespace OnlineMinion.Web.Pages;

[UsedImplicitly]
public partial class TransactionDebitsPage
    : BaseCRUDPage<TransactionDebitListItem, TransactionDebitResp, BaseUpsertTransactionDebitReqData>
{
    private readonly List<AccountSpecDescriptorResp> _accountDescriptorViewModels = new();
    private readonly List<PaymentSpecDescriptorResp> _paymentDescriptorViewModels = new();

    protected override string ModelTypeName => "Debit Transaction";

    protected override async Task RunDependencyLoadingAsync() =>
        await Task.WhenAll(
            LoadDependencyFromApiAsync(
                new GetSomeModelDescriptorsReq<PaymentSpecDescriptorResp>(),
                resp => _paymentDescriptorViewModels.Add(resp)
            ),
            LoadDependencyFromApiAsync(
                new GetSomeModelDescriptorsReq<AccountSpecDescriptorResp>(),
                resp => _accountDescriptorViewModels.Add(resp)
            )
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
        _paymentDescriptorViewModels.Single(vm => vm.Id == paymentSpecId),
        _accountDescriptorViewModels.Single(vm => vm.Id == accountSpecId)
    );

    protected override IGetPagingInfoRequest PageCountRequestFactory(int pageSize) =>
        new GetTransactionDebitPagingMetaInfoReq(pageSize);

    protected override string GetDeleteMessageDescriptorData(TransactionDebitListItem model) =>
        $"subject `{model.Subject}`, at {model.Date.ToString(CultureInfo.CurrentCulture)}";

    protected override IDeleteByIdCommand DeleteCommandFactory(TransactionDebitListItem vm) =>
        new DeleteTransactionDebitReq(vm.Id);
}
