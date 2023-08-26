using OnlineMinion.Contracts.Transactions.Common;

namespace OnlineMinion.Contracts.Transactions.Responses;

public sealed record TransactionCreditResp(
    int      Id,
    DateOnly Date,
    decimal  Amount,
    string   Subject,
    string   Party,
    int      PaymentInstrumentId,
    string?  Tags,
    DateTime CreatedAt
) : BaseTransactionResp(Id, Date, Amount, Subject, Party, PaymentInstrumentId, Tags, CreatedAt);
