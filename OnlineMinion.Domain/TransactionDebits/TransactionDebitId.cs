namespace OnlineMinion.Domain.TransactionDebits;

public record TransactionDebitId(Guid Value) : IId
{
    public TransactionDebitId() : this(Guid.NewGuid()) { }
}
