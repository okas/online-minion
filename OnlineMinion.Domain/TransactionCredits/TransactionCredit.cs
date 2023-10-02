using OnlineMinion.Domain.TransactionShared;

namespace OnlineMinion.Domain.TransactionCredits;

public sealed class TransactionCredit : BaseTransactionData, IEntity<TransactionCreditId>
{
    public TransactionCreditId Id { get; } = new();
}
