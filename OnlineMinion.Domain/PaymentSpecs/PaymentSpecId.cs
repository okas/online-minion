namespace OnlineMinion.Domain.PaymentSpecs;

public record PaymentSpecId(Guid Value) : IId
{
    public PaymentSpecId() : this(Guid.NewGuid()) { }
}
