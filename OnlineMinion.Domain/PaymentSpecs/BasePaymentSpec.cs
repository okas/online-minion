namespace OnlineMinion.Domain.PaymentSpecs;

public abstract class BasePaymentSpec : IEntity<PaymentSpecId>
{
    public required string Name { get; set; }

    public required string CurrencyCode { get; set; }

    public string? Tags { get; set; }

    public PaymentSpecId Id { get; } = new();
}
