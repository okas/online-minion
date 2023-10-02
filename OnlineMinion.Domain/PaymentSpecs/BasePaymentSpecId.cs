namespace OnlineMinion.Domain.PaymentSpecs;

public record BasePaymentSpecId(Guid Value) : IId
{
    public BasePaymentSpecId() : this(Guid.NewGuid()) { }
}
