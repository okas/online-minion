namespace OnlineMinion.Contracts.Transactions.Debit.Responses;

public sealed record TransactionDebitResp(
    Guid     Id,
    Guid     PaymentInstrumentId,
    Guid     AccountSpecId,
    decimal  Fee,
    DateOnly Date,
    decimal  Amount,
    string   Subject,
    string   Party,
    string?  Tags
) : BaseTransactionResp(Id, PaymentInstrumentId, Date, Amount, Subject, Party, Tags);
