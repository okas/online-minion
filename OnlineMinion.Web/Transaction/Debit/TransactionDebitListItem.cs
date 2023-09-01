using OnlineMinion.Contracts.Transactions;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;

namespace OnlineMinion.Web.Transaction.Debit;

public sealed record TransactionDebitListItem(
    int                    Id,
    int                    PaymentInstrumentId,
    int                    AccountSpecId,
    decimal                Fee,
    DateOnly               Date,
    decimal                Amount,
    string                 Subject,
    string                 Party,
    string?                Tags,
    TransactionPaymentData PaymentInstrument,
    TransactionAccountData AccountSpec
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
    public static TransactionDebitListItem FromResponseDto(
        TransactionDebitResp resp,
        string               paymentInstrumentName,
        string               accountSpecName
    ) =>
        new(
            resp.Id,
            resp.PaymentInstrumentId,
            resp.AccountSpecId,
            resp.Fee,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            new(paymentInstrumentName),
            new(accountSpecName)
        );

    public static TransactionDebitListItem FromUpdateRequest(
        UpdateTransactionDebitReq rq,
        string                    paymentInstrumentName,
        string                    accountSpecName
    ) =>
        new(
            rq.Id,
            rq.PaymentInstrumentId,
            rq.AccountSpecId,
            rq.Fee,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            new(paymentInstrumentName),
            new(accountSpecName)
        );

    public static UpdateTransactionDebitReq ToUpdateRequest(TransactionDebitListItem vm) =>
        new(
            vm.Id,
            vm.PaymentInstrumentId,
            vm.AccountSpecId,
            vm.Fee,
            vm.Date,
            vm.Amount,
            vm.Subject,
            vm.Party,
            vm.Tags
        );
}
