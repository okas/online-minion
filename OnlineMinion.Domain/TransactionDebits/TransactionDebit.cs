using OnlineMinion.Domain.AccountSpecs;
using OnlineMinion.Domain.TransactionShared;

namespace OnlineMinion.Domain.TransactionDebits;

public sealed class TransactionDebit : BaseTransactionData, IEntity<TransactionDebitId>
{
    public required decimal Fee { get; set; }

    public required AccountSpecId AccountSpecId { get; set; }

    public AccountSpec? AccountSpec { get; set; }

    public TransactionDebitId Id { get; } = new();
}
