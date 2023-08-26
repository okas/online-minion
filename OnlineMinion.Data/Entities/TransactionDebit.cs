using OnlineMinion.Data.Entities.Shared;

namespace OnlineMinion.Data.Entities;

public class TransactionDebit : BaseTransaction
{
    public required decimal Fee { get; set; }

    public required int AccountSpecId { get; set; }

    public AccountSpec? AccountSpec { get; set; }
}
