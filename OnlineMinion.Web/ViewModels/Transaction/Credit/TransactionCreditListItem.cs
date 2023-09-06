using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;

namespace OnlineMinion.Web.ViewModels.Transaction.Credit;

/// <inheritdoc />
public sealed record TransactionCreditListItem(
    int                       Id,
    DateOnly                  Date,
    decimal                   Amount,
    string                    Subject,
    string                    Party,
    string?                   Tags,
    PaymentSpecDescriptorResp PaymentInstrument
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
    public static TransactionCreditListItem FromResponseDto(
        TransactionCreditResp     resp,
        PaymentSpecDescriptorResp paymentDescriptor
    ) =>
        new(
            resp.Id,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            paymentDescriptor
        );

    public static TransactionCreditListItem FromUpdateRequest(
        UpdateTransactionCreditReq rq,
        PaymentSpecDescriptorResp  paymentDescriptor
    ) =>
        new(
            rq.Id,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            paymentDescriptor
        );

    public static UpdateTransactionCreditReq ToUpdateRequest(TransactionCreditListItem vm) =>
        new(
            vm.Id,
            vm.Date,
            vm.Amount,
            vm.Subject,
            vm.Party,
            vm.PaymentInstrument.Id,
            vm.Tags
        );
}
