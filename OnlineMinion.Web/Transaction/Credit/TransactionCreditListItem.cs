using OnlineMinion.Contracts.Transactions;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;

namespace OnlineMinion.Web.Transaction.Credit;

public sealed record TransactionCreditListItem(
    int                    Id,
    int                    PaymentInstrumentId,
    DateOnly               Date,
    decimal                Amount,
    string                 Subject,
    string                 Party,
    string?                Tags,
    TransactionPaymentData PaymentInstrument
) : BaseTransactionListItem(
    Id,
    PaymentInstrumentId,
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
            resp.PaymentInstrumentId,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            new(paymentInstrumentName)
        );

    public static TransactionCreditListItem FromUpdateRequest(
        UpdateTransactionCreditReq rq,
        string                     paymentInstrumentName
    ) =>
        new(
            rq.Id,
            rq.PaymentInstrumentId,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            new(paymentInstrumentName)
        );

    public static UpdateTransactionCreditReq ToUpdateRequest(TransactionCreditListItem vm) =>
        new(
            vm.Id,
            vm.Date,
            vm.Amount,
            vm.Subject,
            vm.Party,
            vm.PaymentInstrumentId,
            vm.Tags
        );
}
