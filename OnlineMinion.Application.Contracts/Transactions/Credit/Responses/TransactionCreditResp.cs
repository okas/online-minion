namespace OnlineMinion.Application.Contracts.Transactions.Credit.Responses;

public sealed record TransactionCreditResp(
    Guid     Id,
    Guid     PaymentInstrumentId,
    DateOnly Date,
    decimal  Amount,
    string   Subject,
    string   Party,
    string?  Tags
) : BaseTransactionResp(Id, PaymentInstrumentId, Date, Amount, Subject, Party, Tags);
