using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;

namespace OnlineMinion.Web.ViewModels.Transaction.Debit;

/// <inheritdoc />
/// <param name="AccountSpec">NB! It is important too keep member name, so that API communication queries work.</param>
public sealed record TransactionDebitListItem(
    int                       Id,
    decimal                   Fee,
    DateOnly                  Date,
    decimal                   Amount,
    string                    Subject,
    string                    Party,
    string?                   Tags,
    PaymentSpecDescriptorResp PaymentInstrument,
    AccountSpecDescriptorResp AccountSpec
) : BaseTransactionListItem(
    Id,
    Date,
    Amount,
    Subject,
    Party,
    Tags,
    PaymentInstrument
)
{
    public static TransactionDebitListItem FromResponseDto(
        TransactionDebitResp      resp,
        PaymentSpecDescriptorResp paymentDescriptor,
        AccountSpecDescriptorResp accountDescriptor
    ) =>
        new(
            resp.Id,
            resp.Fee,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            paymentDescriptor,
            accountDescriptor
        );

    public static TransactionDebitListItem FromUpdateRequest(
        UpdateTransactionDebitReq rq,
        PaymentSpecDescriptorResp paymentDescriptor,
        AccountSpecDescriptorResp accountDescriptor
    ) =>
        new(
            rq.Id,
            rq.Fee,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            paymentDescriptor,
            accountDescriptor
        );

    public static UpdateTransactionDebitReq ToUpdateRequest(TransactionDebitListItem vm) =>
        new(
            vm.Id,
            vm.PaymentInstrument.Id,
            vm.AccountSpec.Id,
            vm.Fee,
            vm.Date,
            vm.Amount,
            vm.Subject,
            vm.Party,
            vm.Tags
        );
}
