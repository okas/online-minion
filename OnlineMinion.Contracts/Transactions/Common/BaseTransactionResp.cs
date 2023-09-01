namespace OnlineMinion.Contracts.Transactions.Common;

public abstract record BaseTransactionResp(
        int      Id,
        DateOnly Date,
        decimal  Amount,
        string   Subject,
        string   Party,
        int      PaymentInstrumentId,
        string?  Tags
    )
    : IHasIntId;
