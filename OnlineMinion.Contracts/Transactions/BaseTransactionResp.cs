namespace OnlineMinion.Contracts.Transactions;

public abstract record BaseTransactionResp(
        int      Id,
        int      PaymentInstrumentId,
        DateOnly Date,
        decimal  Amount,
        string   Subject,
        string   Party,
        string?  Tags
    )
    : IHasId;
