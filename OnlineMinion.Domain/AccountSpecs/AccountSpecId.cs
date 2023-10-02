namespace OnlineMinion.Domain.AccountSpecs;

public record AccountSpecId(Guid Value) : IId
{
    public AccountSpecId() : this(Guid.NewGuid()) { }
}
