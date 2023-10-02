namespace OnlineMinion.Domain.TransactionCredits;

public record TransactionCreditId(Guid Value) : IId
{
    public TransactionCreditId() : this(Guid.NewGuid()) { }
}
