using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;

namespace OnlineMinion.Web.Transaction.Credit;

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
        TransactionCreditResp resp,
        string                paymentInstrumentName
    ) =>
        new(
            resp.Id,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            new(resp.PaymentInstrumentId, paymentInstrumentName)
        );

    public static TransactionCreditListItem FromUpdateRequest(
        UpdateTransactionCreditReq rq,
        string                     paymentInstrumentName
    ) =>
        new(
            rq.Id,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            new(rq.PaymentInstrumentId, paymentInstrumentName)
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
