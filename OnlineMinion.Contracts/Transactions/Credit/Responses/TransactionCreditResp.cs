namespace OnlineMinion.Contracts.Transactions.Credit.Responses;

public sealed record TransactionCreditResp(
    int      Id,
    int      PaymentInstrumentId,
    DateOnly Date,
    decimal  Amount,
    string   Subject,
    string   Party,
    string?  Tags
) : BaseTransactionResp(Id, PaymentInstrumentId, Date, Amount, Subject, Party, Tags);
