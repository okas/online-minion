using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Transactions;

namespace OnlineMinion.Web.Transaction;

public abstract record BaseTransactionListItem(
    int                    Id,
    int                    PaymentInstrumentId,
    DateOnly               Date,
    decimal                Amount,
    string                 Subject,
    string                 Party,
    string?                Tags,
    TransactionPaymentData PaymentInstrument
) : IHasIntId;
