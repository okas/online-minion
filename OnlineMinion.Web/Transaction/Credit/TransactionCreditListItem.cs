using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Transactions;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;

namespace OnlineMinion.Web.Transaction.Credit;

public record TransactionCreditListItem(
    int                    Id,
    DateOnly               Date,
    decimal                Amount,
    string                 Subject,
    string                 Party,
    int                    PaymentInstrumentId,
    TransactionPaymentData PaymentInstrument,
    string?                Tags
) : IHasIntId
{
    public static TransactionCreditListItem FromResponseDto(TransactionCreditResp resp, string paymentInstrumentName) =>
        new(
            resp.Id,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.PaymentInstrumentId,
            new(paymentInstrumentName),
            resp.Tags
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
            rq.PaymentInstrumentId,
            new(paymentInstrumentName),
            rq.Tags
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
