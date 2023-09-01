namespace OnlineMinion.Contracts.Transactions.Common;

public abstract record BaseTransactionResp(
        int      Id,
        int      PaymentInstrumentId,
        DateOnly Date,
        decimal  Amount,
        string   Subject,
        string   Party,
        string?  Tags
    )
    : IHasIntId;
