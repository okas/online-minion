namespace OnlineMinion.Application.Contracts.Transactions;

public abstract record BaseTransactionResp(
        Guid     Id,
        Guid     PaymentInstrumentId,
        DateOnly Date,
        decimal  Amount,
        string   Subject,
        string   Party,
        string?  Tags
    )
    : IHasId;
